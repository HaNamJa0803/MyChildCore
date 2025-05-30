using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class AppearanceManager
    {
        /// <summary>
        /// 실제 외형(스프라이트) 파츠 적용. GMCM/동기화 호출 시 사용
        /// </summary>
        public static void ApplyToddlerParts(
            Child child, bool isMale, string selectedHair, int bottomIndex, int shoesIndex, int neckIndex, string pajamaStyle, int pajamaColorIndex)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);
            string clothesBase = "Clothes/";

            // 축제복 (성별 분기)
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

        // 계절별 상의 분기 (봄/여름은 숏)
        private static bool IsShortTop()
        {
            var season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            return season == "spring" || season == "summer";
        }

        // 실제 스프라이트 적용
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

        /// <summary>
        /// 배우자 이름 안전 추출 (Fallback "Unknown")
        /// </summary>
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
    }
}