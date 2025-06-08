using MyChildCore;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewValley.Characters;

namespace MyChildCore
{
    public static class EventManager
    {
        // 항상 최신 config 인스턴스 사용
        private static ModConfig Config => ModEntry.Config;

        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
        }

        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
            => SyncAllChildrenAppearance(Config);

        public static void OnDayStarted(object sender, DayStartedEventArgs e)
            => SyncAllChildrenAppearance(Config);

        public static void OnWarped(object sender, WarpedEventArgs e)
            => SyncAllChildrenAppearance(Config);

        public static void OnMenuChanged(object sender, MenuChangedEventArgs e)
            => SyncAllChildrenAppearance(Config);

        public static void OnSaved(object sender, SavedEventArgs e)
            => SyncAllChildrenAppearance(Config);

        public static void SyncAllChildrenAppearance(ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                // ⭐⭐⭐ 배우자(스파우스) 키 자동 생성!
                string spouseName = AppearanceManager.GetSpouseName(child);
                if (!config.SpouseConfigs.ContainsKey(spouseName))
                {
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();
                    CustomLogger.Warn($"[GMCMKeyValidator] 누락된 배우자 키 자동 추가: {spouseName}");
                }

                // GMCMKeyValidator.FindOrAddKey(child, config); // 중복 방지용, 필요하면 유지

                ChildParts parts = child.Age == 0
                    ? PartsManager.GetPartsForBaby(child, config)
                    : PartsManager.GetPartsForChild(child, config);

                if (parts == null)
                {
                    CustomLogger.Warn($"[EventManager] ChildParts 생성 실패: {child?.Name ?? "null"} (spouse: {spouseName})");
                    continue;
                }

                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, parts);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, parts);
            }
        }

        public static void SyncChildrenForSpouse(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetSpouseName(child);

                // ⭐⭐⭐ 배우자(스파우스) 키 자동 생성!
                if (!config.SpouseConfigs.ContainsKey(spouseName))
                {
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();
                    CustomLogger.Warn($"[GMCMKeyValidator] 누락된 배우자 키 자동 추가: {spouseName}");
                }

                if (spouseName != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                // GMCMKeyValidator.FindOrAddKey(child, config); // 필요시 유지

                ChildParts parts = child.Age == 0
                    ? PartsManager.GetPartsForBaby(child, config)
                    : PartsManager.GetPartsForChild(child, config);

                if (parts == null)
                {
                    CustomLogger.Warn($"[EventManager] ChildParts 생성 실패: {child?.Name ?? "null"} (spouse: {spouseName})");
                    continue;
                }

                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, parts);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, parts);    
            }
        }
    }
}