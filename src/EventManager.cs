using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using Newtonsoft.Json;

namespace MyChildCore.Utilities
{
    public static class MyChildCoreUtilities
    {
        // ------------- [이벤트 콜백 등록부] -------------
        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
        }

        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e) => SyncAllChildrenAppearance(GlobalDropdownConfig.Instance);
        public static void OnDayStarted(object sender, DayStartedEventArgs e) => SyncAllChildrenAppearance(GlobalDropdownConfig.Instance);
        public static void OnWarped(object sender, WarpedEventArgs e) => SyncAllChildrenAppearance(GlobalDropdownConfig.Instance);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => SyncAllChildrenAppearance(GlobalDropdownConfig.Instance);
        public static void OnSaved(object sender, SavedEventArgs e) => SyncAllChildrenAppearance(GlobalDropdownConfig.Instance);

        // ------------- [외형 적용 메인 로직] -------------
        public static void ApplyToddlerParts(
            Child child, bool isMale, string selectedHair, int bottomIndex, int shoesIndex, int neckIndex, string pajamaStyle, int pajamaColorIndex)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);
            string clothesBase = "Clothes/";

            // 축제 분기
            if (Game1.isFestivalDay || Game1.isFestival)
            {
                string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
                if (season == "spring")
                    ApplyIfExist(child, $"{clothesBase}Festival/Spring/FestivalHat_Spring.png");
                else if (season == "summer")
                    ApplyIfExist(child, $"{clothesBase}Festival/Summer/FestivalTop_Summer.png", $"{clothesBase}Festival/Summer/FestivalHat_Summer.png");
                else if (season == "fall" || season == "autumn")
                    ApplyIfExist(child, $"{clothesBase}Festival/Fall/FestivalTop_Fall.png");
                else if (season == "winter")
                    ApplyIfExist(child, $"{clothesBase}Festival/Winter/FestivalTop_Winter.png", $"{clothesBase}Festival/Winter/FestivalHat_Winter.png", $"{clothesBase}Festival/Winter/FestivalNeck_Winter.png");
                return;
            }

            // 잠옷
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaFile = $"{pajamaStyle}_{pajamaColorIndex:D2}.png";
                string pajamaPath = $"{clothesBase}Sleep/{pajamaStyle}/{pajamaFile}";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 평상복
            string topPath = isMale
                ? $"{clothesBase}Top/Male/Top_Male_{(IsShortTop() ? "Short" : "Long")}.png"
                : $"{clothesBase}Top/Female/Top_Female_{(IsShortTop() ? "Short" : "Long")}.png";
            string bottomPath = isMale
                ? $"{clothesBase}Bottom/Pants/Pants_{bottomIndex:D2}.png"
                : $"{clothesBase}Bottom/Skirt/Skirt_{bottomIndex:D2}.png";
            string shoesPath = $"{clothesBase}Shoes/Shoes_{shoesIndex:D2}.png";
            string neckPath = $"{clothesBase}NeckCollar/NeckCollar_{neckIndex:D2}.png";

            string basePath = $"assets/{spouseName}/Toddler/";
            string hairKey = isMale ? "Short" : selectedHair;
            string hairPath = $"{basePath}Hair/{spouseName}_Toddler_{hairKey}.png";
            string eyePath = $"{basePath}Eye/{spouseName}_Toddler_Eye.png";
            string skinPath = $"{basePath}Skin/{spouseName}_Toddler_Skin.png";

            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        private static bool IsShortTop()
        {
            var season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            return season == "spring" || season == "summer";
        }

        private static void ApplyIfExist(Child child, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (File.Exists(path))
                {
                    child.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
            }
        }

        public static string GetSpouseName(Child child)
        {
            try
            {
                if (child?.spouse != null && !string.IsNullOrEmpty(child.spouse.Name)) return child.spouse.Name;
                if (!string.IsNullOrEmpty(child.motherName.Value)) return child.motherName.Value;
                if (!string.IsNullOrEmpty(child.fatherName.Value)) return child.fatherName.Value;
            }
            catch { }
            return "Unknown";
        }

        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        // 동기화 진입점 (GMCM에서 DropdownConfig 넘길 때)
        public static void SyncAllChildrenAppearance(DropdownConfig config)
        {
            foreach (var child in GetAllChildren())
            {
                string spouseName = GetSpouseName(child);
                bool isMale = ((int)child.Gender == 0);

                if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                {
                    // (Optional) 로그
                    // Console.WriteLine($"[DropdownConfig] '{spouseName}'에 대한 자녀 설정 누락, 기본값 적용");
                    spouseConfig = new SpouseChildConfig();
                }

                string hairStyle = isMale ? "Short" : (spouseConfig.GirlHairStyle ?? "CherryTwin");
                string skirt     = spouseConfig.GirlSkirt ?? "Skirt_01";
                string pants     = spouseConfig.BoyPants ?? "Pants_01";
                string shoes     = isMale ? (spouseConfig.BoyShoes ?? "Shoes_01") : (spouseConfig.GirlShoes ?? "Shoes_01");
                string neck      = isMale ? (spouseConfig.BoyNeckCollar ?? "NeckCollar_01") : (spouseConfig.GirlNeckCollar ?? "NeckCollar_01");
                string pajama    = isMale ? (spouseConfig.BoyPajamaStyle ?? "Frog") : (spouseConfig.GirlPajamaStyle ?? "Frog");
                int pajamaColor  = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
                if (pajamaColor < 1) pajamaColor = 1;

                ApplyToddlerParts(
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

        private static int ToIndex(string partName)
        {
            if (string.IsNullOrEmpty(partName)) return 1;
            var num = new string(partName.Where(char.IsDigit).ToArray());
            if (int.TryParse(num, out int idx)) return idx;
            return 1;
        }
    }
}