using MyChildCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace MyChildCore
{
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

            // 실제 배우자명 기준으로만 Fixup(누락 추가, 경고 없음)
            helper.Events.GameLoop.SaveLoaded += (s, e) =>
            {
                var actualSpouseNames = ChildManager.GetAllChildren()
                    .Select(child => AppearanceManager.GetRealSpouseName(child))
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct()
                    .ToList();
                Config.FixupSpouseKeys(actualSpouseNames);
            };

            // Config 변경 감지 - 실시간 반영
            Config.ConfigChanged += OnConfigChanged;

            // 이벤트 매니저 연결
            EventManager.RegisterEvents(helper);

            // GMCM 연동
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // 외형 리소스 패치 (PatchImage, 동적 루프)
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // 핫리로드/동기화 이벤트 등(선택적)
            HotReloadWatcher.StartWatching(DataManager.DataPath, helper, Config);

            // 콘솔 명령어 등록
            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);

            Monitor.Log("MyChildCore 모드(유니크 칠드런 오리지널식) 로드 완료!", LogLevel.Info);
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            GMCMManager.RegisterGMCM(ModHelper, this);
        }

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
                    {
                        e.Edit(asset => asset.AsImage().PatchImage(finalSprite));
                    }
                    else
                    {
                        var fallback = ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                        e.Edit(asset => asset.AsImage().PatchImage(fallback));
                    }
                }
            }
        }

        private void OnConfigChanged()
        {
            DataManager.SaveData(CacheManager.GetChildCache());
            DataManager.ApplyAllAppearances(Config);
        }

        private static void ApplyAllChildren(string cmd, string[] args)
        {
            DataManager.ApplyAllAppearances(Config);
        }

        private static void BackupChildren(string cmd, string[] args)
        {
            DataManager.Backup();
        }

        private static void RestoreChildren(string cmd, string[] args)
        {
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
        }

        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            ModHelper.WriteConfig(Config);
            DataManager.SaveData(CacheManager.GetChildCache());
            DataManager.ApplyAllAppearances(Config);
        }

        public static void SaveConfig()
        {
            ModHelper.WriteConfig(Config);
            DataManager.SaveData(CacheManager.GetChildCache());
        }
    }
}