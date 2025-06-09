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
            Config = helper.ReadConfig<ModConfig>();

            // 1. 이벤트 매니저 연결
            EventManager.RegisterEvents(helper);

            // 2. GMCM 연동
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // 3. 외형 리소스 패치 (PatchImage, 중복 루프)
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // 4. 콘솔 명령어 등록
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

        // 외형 리소스 패치 (PatchImage 중복 루프!)
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            // 캐릭터 자녀 스프라이트는 반드시 "중복 루프"로 각각 PatchImage!
            if (e.NameWithoutLocale.IsEquivalentTo("Characters/Child"))
            {
                var allChildren = ChildManager.GetAllChildren();

                if (allChildren.Count == 0)
                {
                    var defaultTex = ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                    e.Edit(asset => asset.AsImage().PatchImage(defaultTex));
                    CustomLogger.Info("[AssetRequested] Characters/Child → Default 커스텀 이미지 패치");
                }
                else
                {
                    // === 유니크 칠드런식 중복 루프! ===
                    foreach (var child in allChildren)
                    {
                        ChildParts parts = (child.Age >= 1)
                            ? PartsManager.GetPartsForChild(child, Config)
                            : PartsManager.GetPartsForBaby(child, Config);

                        if (parts == null)
                            parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                        var layerPaths = AppearanceManager.GetLayerPaths(child, parts);
                        Texture2D finalSprite = AppearanceManager.CombinePartsToTexture(layerPaths);

                        if (finalSprite != null)
                        {
                            e.Edit(asset => asset.AsImage().PatchImage(finalSprite));
                            CustomLogger.Info("[AssetRequested] " + child.Name + " 스프라이트 합성 이미지로 패치!");
                        }
                        else
                        {
                            var fallbackTex = ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                            e.Edit(asset => asset.AsImage().PatchImage(fallbackTex));
                            CustomLogger.Warn("[AssetRequested] " + child.Name + " 합성 실패, Default 이미지로 대체");
                        }
                    }
                }
            }
        }

        // 콘솔 명령어: 외형 즉시 반영 (직접 AnimatedSprite 대입)
        private static void ApplyAllChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] applychild 실행");
            DataManager.ApplyAllAppearances(Config); // 자녀 전체 직접 교체 루프
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
            DataManager.ApplyAllAppearances(Config); // 복구 후 직접 교체 루프
        }

        // GMCM 옵션 콜백 (변경 시 즉시 저장 + 직접 교체 루프)
        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            CustomLogger.Info("[GMCM] 옵션 변경: " + spouse + " / " + (isBoy ? "Boy" : "Girl"));
            DataManager.SaveData(CacheManager.GetChildCache());
            DataManager.ApplyAllAppearances(Config); // 변경 즉시 직접 교체 루프
        }

        // GMCM에서 반드시 호출!
        public static void SaveConfig()
        {
            ModHelper.WriteConfig(Config);
        }
    }
}