using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 모든 게임 이벤트(저장, 날짜변경, 워프 등)에서 자녀 외형을 자동 동기화하는 매니저
    /// (GMCM Key 기반, 폴리아모리·다자녀 완전 대응)
    /// </summary>
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

        // 주요 게임 이벤트마다 동기화 실행
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)   => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnDayStarted(object sender, DayStartedEventArgs e)   => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnWarped(object sender, WarpedEventArgs e)           => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnSaved(object sender, SavedEventArgs e)             => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);

        /// <summary>
        /// 모든 자녀의 외형을 최신 설정으로 동기화 (GMCMKey 대응, 아기+유아 모두 반영)
        /// </summary>
        public static void SyncAllChildrenAppearance(DropdownConfig config)
        {
            if (config == null || config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string gmcmKey = GMCMKeyUtil.GetChildKey(child); // 반드시 고유 식별자 사용
                if (!config.SpouseConfigs.TryGetValue(gmcmKey, out var spouseConfig))
                    continue;

                if (child.Age >= 1) // 유아/아이
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts == null)
                    {
                        CustomLogger.Warn($"[EventManager] ChildParts 생성 실패: {child?.Name ?? "null"}");
                        continue;
                    }
                    AppearanceManager.ApplyToddlerAppearance(child, parts);
                }
                else // 아기(0세)
                {
                    var babyParts = PartsManager.GetPartsForBaby(child, config);
                    if (babyParts == null)
                    {
                        CustomLogger.Warn($"[EventManager] BabyParts 생성 실패: {child?.Name ?? "null"}");
                        continue;
                    }
                    AppearanceManager.ApplyBabyAppearance(child, babyParts);
                }
            }
        }
    }
}