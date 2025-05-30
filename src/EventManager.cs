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
    /// <summary>
    /// GMCM 드롭다운 전용 Config(배우자별+자녀 성별별)
    /// </summary>
    public class DropdownConfig
    {
        public static readonly string[] SpouseNames =
        {
            "Abigail", "Emily", "Haley", "Maru", "Leah", "Penny",
            "Alisa", "Faye", "Medi", "Paula", "Dia", "Ysabelle", "Corinne", "Blair", "Flo", "Irene", "Kiara"
        };

        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        public DropdownConfig()
        {
            foreach (var spouse in SpouseNames)
            {
                if (!SpouseConfigs.ContainsKey(spouse))
                    SpouseConfigs[spouse] = new SpouseChildConfig();
            }
        }

        public static readonly string[] GirlHairStyles = { "CherryTwin", "TwinTail", "PonyTail" };
        public static readonly string[] SkirtOptions = {
            "Skirt_01","Skirt_02","Skirt_03","Skirt_04","Skirt_05",
            "Skirt_06","Skirt_07","Skirt_08","Skirt_09","Skirt_10"
        };
        public static readonly string[] PantsOptions = {
            "Pants_01","Pants_02","Pants_03","Pants_04","Pants_05",
            "Pants_06","Pants_07","Pants_08","Pants_09","Pants_10"
        };
        public static readonly string[] ShoesOptions = { "Shoes_01", "Shoes_02", "Shoes_03", "Shoes_04" };
        public static readonly string[] NeckCollarOptions = {
            "NeckCollar_01","NeckCollar_02","NeckCollar_03","NeckCollar_04","NeckCollar_05",
            "NeckCollar_06","NeckCollar_07","NeckCollar_08","NeckCollar_09","NeckCollar_10",
            "NeckCollar_11","NeckCollar_12","NeckCollar_13","NeckCollar_14","NeckCollar_15",
            "NeckCollar_16","NeckCollar_17","NeckCollar_18","NeckCollar_19","NeckCollar_20",
            "NeckCollar_21","NeckCollar_22","NeckCollar_23","NeckCollar_24","NeckCollar_25",
            "NeckCollar_26"
        };
        public static readonly string[] PajamaStyles = { "Frog", "WelshCorgi", "Sheep", "LesserPanda", "Racoon", "Shark" };
        public static readonly Dictionary<string, int> PajamaColorMax = new()
        {
            { "Frog", 8 }, { "WelshCorgi", 8 }, { "Sheep", 7 }, { "LesserPanda", 7 }, { "Racoon", 8 }, { "Shark", 7 }
        };
    }

    /// <summary>
    /// 배우자별 자녀 성별별 개별 옵션
    /// </summary>
    public class SpouseChildConfig
    {
        // 여아(딸)
        public string GirlHairStyle { get; set; } = "CherryTwin";
        public string GirlSkirt { get; set; } = "Skirt_01";
        public string GirlShoes { get; set; } = "Shoes_01";
        public string GirlNeckCollar { get; set; } = "NeckCollar_01";
        public string GirlPajamaStyle { get; set; } = "Frog";
        public int GirlPajamaColorIndex { get; set; } = 1;
        // 남아(아들)
        public string BoyPants { get; set; } = "Pants_01";
        public string BoyShoes { get; set; } = "Shoes_01";
        public string BoyNeckCollar { get; set; } = "NeckCollar_01";
        public string BoyPajamaStyle { get; set; } = "Frog";
        public int BoyPajamaColorIndex { get; set; } = 1;
    }

    public static class MyChildCoreUtilities
    {
        // ================= 이벤트 콜백 등록 ===================
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

        // ================= 외형 적용 메인 로직 ==================
        public static void ApplyToddlerParts(
            Child child, bool isMale, string selectedHair, int bottomIndex, int shoesIndex, int neckIndex, string pajamaStyle, int pajamaColorIndex)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);
            string clothesBase = "Clothes/";

            // 축제 분기 (남아/여아 상의 분리)
            if (Game1.isFestivalDay || Game1.isFestival)
            {
                string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
                string genderKey = isMale ? "Male" : "Female";
                if (season == "spring")
                    ApplyIfExist(child, $"{clothesBase}Festival/Spring/FestivalTop_{genderKey}_Spring.png");
                else if (season == "summer")
                    ApplyIfExist(child, $"{clothesBase}Festival/Summer/FestivalTop_{genderKey}_Summer.png", $"{clothesBase}Festival/Summer/FestivalHat_{genderKey}_Summer.png");
                else if (season == "fall" || season == "autumn")
                    ApplyIfExist(child, $"{clothesBase}Festival/Fall/FestivalTop_{genderKey}_Fall.png");
                else if (season == "winter")
                    ApplyIfExist(child,
                        $"{clothesBase}Festival/Winter/FestivalTop_{genderKey}_Winter.png",
                        $"{clothesBase}Festival/Winter/FestivalHat_{genderKey}_Winter.png",
                        $"{clothesBase}Festival/Winter/FestivalNeck_{genderKey}_Winter.png");
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