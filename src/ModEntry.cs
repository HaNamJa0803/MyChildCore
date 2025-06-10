using MyChildCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyChildCore
{
    /// <summary>
    /// MyChildCore 엔트리 (유니크 칠드런 오리지널식: 직접 AnimatedSprite 교체 + PatchImage 중복 루프)
    /// </summary>
    public class ModEntry : Mod
    {
        public static ModConfig Config;
        public static IModHelper ModHelper;
        public static IMonitor Log;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            Log = Monitor;

            // 1. 설정 파일 로딩(외부 저장소에서 누락 복구까지 포함)
            Config = helper.ReadConfig<ModConfig>();
            Config.FixupSpouseKeys();

            // 2. Config 변경 감지 - 실시간 반영
            Config.ConfigChanged += OnConfigChanged;

            // 3. 이벤트 매니저 연결
            EventManager.RegisterEvents(helper);

            // 4. GMCM 연동 (런칭 시)
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // 5. 외형 리소스 패치 (PatchImage, 동적 루프)
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // 6. 핫리로드/동기화 이벤트 등(선택적)
            HotReloadWatcher.StartWatching(DataManager.DataPath, helper, Config);

            // 7. 콘솔 명령어 등록
            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);

            Monitor.Log("MyChildCore 모드(유니크 칠드런 오리지널식) 로드 완료!", LogLevel.Info);
        }

        // GMCM 연동 (게임 런칭 시)
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            GMCMManager.RegisterGMCM(ModHelper, this);
        }

        // === [자녀별 PatchImage: Characters/{child.Name}를 다 순회해서 패치!] ===
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            // 모든 자녀의 이름에 해당하는 스프라이트만 PatchImage
            foreach (var child in ChildManager.GetAllChildren())
            {
                string assetName = $"Characters/{child.Name}";
                if (e.NameWithoutLocale.IsEquivalentTo(assetName))
                {
                    // 최신 파츠 즉시 추출
                    ChildParts parts = (child.Age >= 1)
                        ? PartsManager.GetPartsForChild(child, Config)
                        : PartsManager.GetPartsForBaby(child, Config);

                    if (parts == null)
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                    // 레이어 합성은 항상 최신화!
                    var layerPaths = AppearanceManager.GetLayerPaths(child, parts);
                    Texture2D finalSprite = AppearanceManager.CombinePartsToTexture(layerPaths);

                    if (finalSprite != null)
                    {
                        e.Edit(asset => asset.AsImage().PatchImage(finalSprite));
                        CustomLogger.Info($"[AssetRequested] {child.Name} → 합성 이미지 패치!");
                    }
                    else
                    {
                        // Fallback: 디폴트 이미지
                        var fallback = ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                        e.Edit(asset => asset.AsImage().PatchImage(fallback));
                        CustomLogger.Warn($"[AssetRequested] {child.Name} 합성 실패 → Default 이미지로 대체");
                    }
                }
            }
        }

        // === Config/GMCM 등 변경 → 모든 자녀 실시간 재적용/동기화 ===
        private void OnConfigChanged()
        {
            CustomLogger.Info("[ConfigChanged] 설정 변경 감지됨 → 즉시 동기화 및 외형 전체 적용");
            DataManager.SaveData(CacheManager.GetChildCache());    // 캐시 즉시 저장
            DataManager.ApplyAllAppearances(Config);               // 전체 자녀 즉시 교체
        }

        // 콘솔 명령어: 외형 즉시 반영 (직접 AnimatedSprite 대입)
        private static void ApplyAllChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] applychild 실행");
            DataManager.ApplyAllAppearances(Config);
        }

        // 콘솔 명령어: 자녀 데이터 백업
        private static void BackupChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] backupchild 실행");
            DataManager.Backup();
        }

        // 콘솔 명령어: 백업 복구 및 외형 재적용
        private static void RestoreChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] restorechild 실행");
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
        }

        // GMCM 옵션 콜백 (변경 시 즉시 저장 + 직접 교체 루프)
        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            CustomLogger.Info("[GMCM] 옵션 변경: " + spouse + " / " + (isBoy ? "Boy" : "Girl"));
            ModHelper.WriteConfig(Config);
            DataManager.SaveData(CacheManager.GetChildCache());
            DataManager.ApplyAllAppearances(Config);
        }

        // GMCM에서 반드시 호출!
        public static void SaveConfig()
        {
            ModHelper.WriteConfig(Config);
            DataManager.SaveData(CacheManager.GetChildCache());
        }
    }
}