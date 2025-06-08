using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MyChildCore
{
    /// <summary>
    /// 엔트리포인트: 모든 매니저 연결 및 핵심 이벤트·GMCM 연동 (유니크 칠드런 최신화)
    /// </summary>
    public class ModEntry : Mod
    {
        public static ModConfig Config;
        public static IModHelper ModHelper;
        public static IMonitor Log;
        private static int lastSyncedDay = -1;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            Log = Monitor;
            Config = helper.ReadConfig<ModConfig>();

            // === [1] 이벤트 매니저 연결 ===
            EventManager.RegisterEvents(helper);

            // === [2] GMCM 연동 ===
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // === [3] SMAPI Content AssetRequested 연동(외형 리소스 패치) ===
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // === [4] 콘솔 명령어 등록 ===
            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);

            Log.Info("MyChildCore 모드(유니크 칠드런 최신) 로드 완료!");
        }

        // === [5] GMCM 연동 ===
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            GMCMManager.RegisterGMCM(ModHelper, this);
        }

        // === [6] 외형 리소스 패치: 자녀별 파츠 합성 이미지로 강제 덮어쓰기 ===
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
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
                    foreach (var child in allChildren)
                    {
                        ChildParts parts = (child.Age >= 1)
                            ? PartsManager.GetPartsForChild(child, Config)
                            : PartsManager.GetPartsForBaby(child, Config);
                        if (parts == null)
                        {
                            parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                        }

                        var layerPaths = AppearanceManager.GetLayerPaths(child, parts);
                        Texture2D finalSprite = AppearanceManager.CombinePartsToTexture(layerPaths);
                        if (finalSprite != null)
                        {
                            e.Edit(asset => asset.AsImage().PatchImage(finalSprite));
                            CustomLogger.Info($"[AssetRequested] {child.Name} 스프라이트 합성 이미지로 강제 패치!");
                        }
                        else
                        {
                            CustomLogger.Warn($"[AssetRequested] {child.Name} 합성 실패, Default 이미지로 대체");
                            var fallbackTex = ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                            e.Edit(asset => asset.AsImage().PatchImage(fallbackTex));
                        }
                    }
                }
            }
        }

        // === [7] 콘솔 명령 ===
        private static void ApplyAllChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] applychild 실행");
            DataManager.ApplyAllAppearances(Config);
        }
        private static void BackupChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] backupchild 실행");
            DataManager.Backup();
        }
        private static void RestoreChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] restorechild 실행");
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
        }

        // === [8] GMCM 옵션 콜백: 변경시 외형/데이터 동기화 ===
        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            CustomLogger.Info($"[GMCM] 옵션 변경: {spouse} / {(isBoy ? "Boy" : "Girl")}");
            AppearanceManager.ApplyForGMCMChange(spouse, isBoy, Config);
            DataManager.SaveData(CacheManager.GetChildCache());
            foreach (var child in ChildManager.GetAllChildren())
                ResourceManager.InvalidateChildSprite(child.Name);
        }
    }
}