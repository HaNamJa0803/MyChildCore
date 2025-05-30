using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class EventManager
    {
        // (이벤트 등록)
        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
        }

        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)    => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnDayStarted(object sender, DayStartedEventArgs e)    => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnWarped(object sender, WarpedEventArgs e)            => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e)  => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);
        public static void OnSaved(object sender, SavedEventArgs e)              => SyncAllChildrenAppearance(DropdownConfigGlobal.Instance);

        /// <summary>
        /// 자녀 전체 외형 동기화 (GMCM/외부저장소 기반)
        /// </summary>
        public static void SyncAllChildrenAppearance(DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetSpouseName(child);
                bool isMale = ((int)child.Gender == 0);

                // 안전 분기 (누락 시 기본값)
                if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                    spouseConfig = new SpouseChildConfig();

                string hairStyle = isMale ? "Short" : (spouseConfig.GirlHairStyle ?? "CherryTwin");
                string skirt     = spouseConfig.GirlSkirt ?? "Skirt_01";
                string pants     = spouseConfig.BoyPants ?? "Pants_01";
                string shoes     = isMale ? (spouseConfig.BoyShoes ?? "Shoes_01") : (spouseConfig.GirlShoes ?? "Shoes_01");
                string neck      = isMale ? (spouseConfig.BoyNeckCollar ?? "NeckCollar_01") : (spouseConfig.GirlNeckCollar ?? "NeckCollar_01");
                string pajama    = isMale ? (spouseConfig.BoyPajamaStyle ?? "Frog") : (spouseConfig.GirlPajamaStyle ?? "Frog");
                int pajamaColor  = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
                if (pajamaColor < 1) pajamaColor = 1;

                AppearanceManager.ApplyToddlerParts(
                    child,
                    isMale,
                    hairStyle,
                    isMale ? ToIndex(pants) : ToIndex(skirt),
                    ToIndex(shoes),
                    ToIndex(neck),
                    pajama,
                    pajamaColor
                );
            }
        }

        // "Pants_02" -> 2, null/실패=1
        private static int ToIndex(string partName)
        {
            if (string.IsNullOrEmpty(partName)) return 1;
            var num = new string(partName.Where(char.IsDigit).ToArray());
            if (int.TryParse(num, out int idx)) return idx;
            return 1;
        }
    }
}