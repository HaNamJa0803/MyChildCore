using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형적용/파츠 경로 생성/스포우스 추출 전용 매니저
    /// (캐시/저장/로드/GMCM 연동 등은 일절 없음)
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 자녀(Child) 객체의 배우자명(스포우스 키) 추출
        /// (최우선 spouse 필드, 없으면 mother/fatherName)
        /// </summary>
        public static string GetSpouseName(Child child)
        {
            try
            {
                if (child?.spouse != null && !string.IsNullOrEmpty(child.spouse.Name))
                    return child.spouse.Name;
                if (!string.IsNullOrEmpty(child.motherName?.Value))
                    return child.motherName.Value;
                if (!string.IsNullOrEmpty(child.fatherName?.Value))
                    return child.fatherName.Value;
            }
            catch { }
            return "Unknown";
        }

        /// <summary>
        /// 외형 파츠 적용(축제/잠옷/평상복 포함) - 하드코딩 경로
        /// </summary>
        public static void ApplyToddlerParts(
            Child child, bool isMale, string selectedHair, int bottomIndex, int shoesIndex, int neckIndex, string pajamaStyle, int pajamaColorIndex)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);
            string clothesBase = "Clothes/";

            // 축제 분기 (남/여 상의 분리)
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

            // 평상복(계절별 상의)
            string topPath = isMale
                ? $"{clothesBase}Top/Male/Top_Male_{(IsShortTop() ? "Short" : "Long")}.png"
                : $"{clothesBase}Top/Female/Top_Female_{(IsShortTop() ? "Short" : "Long")}.png";
            string bottomPath = isMale
                ? $"{clothesBase}Bottom/Pants/Pants_{bottomIndex:D2}.png"
                : $"{clothesBase}Bottom/Skirt/Skirt_{bottomIndex:D2}.png";
            string shoesPath = $"{clothesBase}Shoes/Shoes_{shoesIndex:D2}.png";
            string neckPath = $"{clothesBase}NeckCollar/NeckCollar_{neckIndex:D2}.png";

            // 배우자 전용 파츠 (헤어/눈/피부)
            string basePath = $"assets/{spouseName}/Toddler/";
            string hairKey = isMale ? "Short" : selectedHair;
            string hairPath = $"{basePath}Hair/{spouseName}_Toddler_{hairKey}.png";
            string eyePath = $"{basePath}Eye/{spouseName}_Toddler_Eye.png";
            string skinPath = $"{basePath}Skin/{spouseName}_Toddler_Skin.png";

            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        /// <summary>
        /// 계절별로 숏/롱 상의 자동 분기
        /// </summary>
        private static bool IsShortTop()
        {
            var season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            return season == "spring" || season == "summer";
        }

        /// <summary>
        /// 지정 경로 파일이 있으면 적용(하나만 적용)
        /// </summary>
        private static void ApplyIfExist(Child child, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    child.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
            }
        }
    }
}