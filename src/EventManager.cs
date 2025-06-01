using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class EventManager
    {
        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
        }

        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        }
        public static void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        }
        public static void OnWarped(object sender, WarpedEventArgs e)
        {
            SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        }
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        }
        public static void OnSaved(object sender, SavedEventArgs e)
        {
            SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        }

        /// <summary>
        /// 모든 자녀의 외형을 최신 설정으로 동기화 (누락/오류/기본값 방어)
        /// </summary>
        public static void SyncAllChildrenAppearance(DropdownConfig config)
        {
            if (config == null || config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetSpouseName(child);
                bool isMale = ((int)child.Gender == 0);

                var parts = PartsManager.GetPartsForChild(child, config);
                if (parts == null)
                {
                    CustomLogger.Warn($"[EventManager] ChildParts 생성 실패: {child?.Name ?? "null"}");
                    continue;
                }

                AppearanceManager.ApplyToddlerAppearance(child, parts);
            }
        }
    }
}