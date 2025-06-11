using MyChildCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MyChildCore
{
    public class ModEntry : Mod
    {
        public static ModConfig Config;
        public static IModHelper ModHelper;
        public static IMonitor Log;

        public override void Entry(IModHelper helper)
        {
            // ★ 반드시 SMAPI 경로 시스템으로 DataManager 초기화
            DataManager.Init(helper);

            ModHelper = helper;
            Log = Monitor;
            Config = helper.ReadConfig<ModConfig>();

            // [1] 실제 배우자명 기준 Fixup (SaveLoaded/DayStarted)
            helper.Events.GameLoop.SaveLoaded += OnAfterSaveLoaded;   // 핫리로드 감시는 세이브 로딩 후 시작
            helper.Events.GameLoop.DayStarted += OnFixupSpouseKeys;  // 매일 아침에도 키 보정

            // [2] Config 변경 감지 - 실시간 반영
            Config.ConfigChanged += OnConfigChanged;

            // [3] 이벤트 매니저 연결
            EventManager.RegisterEvents(helper);

            // [4] GMCM 연동
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // [5] 외형 리소스 패치
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // [6] 콘솔 명령어 등록
            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);

            Monitor.Log("MyChildCore 모드(유니크 칠드런 오리지널식) 로드 완료!", LogLevel.Info);
        }

        // [핫리로드/키 Fixup] SaveLoaded에서만 감시 시작!
        private void OnAfterSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            // 세이브 BIN/무확장 등 실제 세이브파일 감시 (경로 자동판별)
            HotReloadWatcher.StartWatching(ModHelper, Config);

            // Fixup도 같이!
            OnFixupSpouseKeys(sender, e);
        }

        // 실제 배우자명 기준으로만 Fixup(누락 추가, 경고 없음)
        private void OnFixupSpouseKeys(object sender, EventArgs e)
        {
            var actualSpouseNames = ChildManager.GetAllChildren()
                .Select(child => AppearanceManager.GetRealSpouseName(child))
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .ToList();

            Config.FixupSpouseKeys(actualSpouseNames);

            // 데이터, 캐시도 동기화
            DataManager.SyncFromDisk();
            DataManager.ApplyAllAppearances(Config);
        }

        // GMCM 연동 (런칭 시)
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            GMCMManager.RegisterGMCM(ModHelper, this);
        }

        // 자녀별 PatchImage (실시간 파츠 기반)
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                string assetName = $"Characters/{child.Name}";
                if (e.NameWithoutLocale.IsEquivalentTo(assetName))
                {
                    ChildParts parts = (child.Age >= 1)
                        ? PartsManager.GetPartsForChild(child, Config)
                        : PartsManager.GetPartsForBaby(child, Config);

                    if (parts == null)
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                    var layerPaths = AppearanceManager.GetLayerPaths(child, parts);
                    Texture2D finalSprite = AppearanceManager.CombinePartsToTexture(layerPaths);

                    if (finalSprite != null)
                        e.Edit(asset => asset.AsImage().PatchImage(finalSprite));
                    else
                    {
                        var fallback = ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                        e.Edit(asset => asset.AsImage().PatchImage(fallback));
                    }
                }
            }
        }

        // 모든 설정 변경시 데이터/외형 동기화
        private void OnConfigChanged()
        {
            DataManager.SaveData(CacheManager.GetChildCache());
            DataManager.ApplyAllAppearances(Config);
        }

        // 콘솔 명령어: 전체 자녀 외형 즉시 반영
        private static void ApplyAllChildren(string cmd, string[] args)
        {
            DataManager.ApplyAllAppearances(Config);
        }

        // 콘솔 명령어: 자녀 데이터 백업
        private static void BackupChildren(string cmd, string[] args)
        {
            DataManager.Backup();
        }

        // 콘솔 명령어: 백업 복구 및 외형 재적용
        private static void RestoreChildren(string cmd, string[] args)
        {
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
        }

        // GMCM 옵션 콜백: 부분 반영 지원 (spouse, isBoy만)
        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            ModHelper.WriteConfig(Config);
            DataManager.SaveData(CacheManager.GetChildCache());

            foreach (var child in ChildManager.GetAllChildren())
            {
                string realSpouse = AppearanceManager.GetRealSpouseName(child);
                if (realSpouse == spouse && ((int)child.Gender == (isBoy ? 0 : 1)))
                {
                    var parts = (child.Age >= 1)
                        ? PartsManager.GetPartsForChild(child, Config)
                        : PartsManager.GetPartsForBaby(child, Config);

                    if (parts == null)
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                    if (child.Age >= 1)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                    else
                        AppearanceManager.ApplyBabyAppearance(child, parts);

                    ResourceManager.InvalidateChildSprite(child.Name);
                }
            }
            // 전체 동기화로 마무리
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