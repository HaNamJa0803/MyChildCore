using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    public static class AppearanceManager
    {
        // 1. Baby(아기) 외형 적용 (헤어, 눈, 스킨: 배우자별 고정 1장 / 몸통: 공용 1장)
        public static void ApplyBabyAppearance(Child child)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);
            string basePath = $"assets/{spouseName}/Baby/";

            // 배우자별 파츠(헤어, 눈, 스킨) / 몸통(의상)은 Clothes/Baby/Body.png 공용
            string hairPath = $"{basePath}Hair/{spouseName}_Baby_Hair.png";
            string eyePath  = $"{basePath}Eye/{spouseName}_Baby_Eye.png";
            string skinPath = $"{basePath}Skin/{spouseName}_Baby_Skin.png";
            string bodyPath = $"assets/Clothes/Baby/Body.png";

            // 실제 적용 (순서대로 먼저 존재하는 파일만 입힘)
            ApplyIfExist(child, skinPath, bodyPath, hairPath, eyePath);
        }

        // 2. Toddler(유아) 외형 적용 (축제/잠옷/평상복, Clothes 폴더 파츠 사용)
        public static void ApplyToddlerAppearance(
            Child child, bool isMale, string selectedHairStyle,
            int bottomIndex, int shoesIndex, int neckIndex,
            string pajamaStyle, int pajamaColorIndex)
        {
            if (child == null) return;
            string spouseName = GetSpouseName(child);
            string basePath = $"assets/{spouseName}/Toddler/";
            string clothesRoot = "assets/Clothes/";

            // 헤어 (남아: Short, 여아: 선택 3종)
            string hairKey = isMale ? "Short" : selectedHairStyle;
            string hairPath = $"{basePath}Hair/{spouseName}_{hairKey}.png";
            // 눈, 피부 (배우자 고정 1장)
            string eyePath  = $"{basePath}Eye/{spouseName}_Toddler_Eye.png";
            string skinPath = $"{basePath}Skin/{spouseName}_Toddler_Skin.png";

            // 1. 축제복 (계절별로 공용 의상)
            if (Game1.isFestivalDay || Game1.isFestival)
            {
                string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
                string festivalPath = null;
                switch (season)
                {
                    case "spring":
                        festivalPath = $"{clothesRoot}Festival/Spring/FestivalHat_Spring.png"; break;
                    case "summer":
                        festivalPath = $"{clothesRoot}Festival/Summer/FestivalTop_Summer.png"; break;
                    case "fall":
                    case "autumn":
                        festivalPath = $"{clothesRoot}Festival/Fall/FestivalTop_Fall.png"; break;
                    case "winter":
                        festivalPath = $"{clothesRoot}Festival/Winter/FestivalTop_Winter.png"; break;
                }
                if (!string.IsNullOrEmpty(festivalPath))
                    ApplyIfExist(child, festivalPath);
                else
                    CustomLogger.Warn($"[AppearanceManager] 축제복 파츠 없음: {season}");
                return;
            }

            // 2. 잠옷 (오후 6시~오전 6시)
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"{clothesRoot}Sleep/{pajamaStyle}/{pajamaStyle}_{pajamaColorIndex:D2}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 3. 평상복(번호=색상/디자인)
            string topPath = isMale
                ? $"{clothesRoot}Top/Male/Top_Male_Short.png"
                : $"{clothesRoot}Top/Female/Top_Female_Short.png";
            string bottomPath = isMale
                ? $"{clothesRoot}Bottom/Pants/Pants_{bottomIndex:D2}.png"
                : $"{clothesRoot}Bottom/Skirt/Skirt_{bottomIndex:D2}.png";
            string shoesPath = $"{clothesRoot}Shoes/Shoes_{shoesIndex:D2}.png";
            string neckPath  = $"{clothesRoot}NeckCollar/NeckCollar_{neckIndex:D2}.png";

            // 컬러는 파일 인덱스(번호)로만 구분 - 별도 색상 파라미터 X
            ApplyIfExist(child, skinPath, topPath, bottomPath, shoesPath, neckPath, hairPath, eyePath);
        }

        // 파츠 존재하는 파일만 차례로 적용 (여러 장 레이어X, 가장 먼저 찾은 것만)
        private static void ApplyIfExist(Child child, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (File.Exists(path))
                {
                    child.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    CustomLogger.Debug($"[AppearanceManager] 스프라이트 적용: {child.Name} ({path})");
                    break;
                }
            }
        }

        // 배우자명 추출 (오류=Unknown)
        public static string GetSpouseName(Child child)
        {
            try
            {
                if (child?.spouse != null && !string.IsNullOrEmpty(child.spouse.Name))
                    return child.spouse.Name;
                if (!string.IsNullOrEmpty(child.motherName.Value))
                    return child.motherName.Value;
                if (!string.IsNullOrEmpty(child.fatherName.Value))
                    return child.fatherName.Value;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[AppearanceManager] 배우자명 추출 실패: {ex.Message}");
            }
            return "Unknown";
        }
    }
}