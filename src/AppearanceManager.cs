using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형(스프라이트) 적용 전담 매니저
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 유아(Child) 외형 적용
        /// </summary>
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;

            // 시즌별 숏/롱 상의 경로 결정
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            string topType = "Short";
            switch (season)
            {
                case "spring":
                case "summer":
                    topType = "Short";
                    break;
                case "fall":
                case "autumn":
                case "winter":
                    topType = "Long";
                    break;
            }
            string topPath = parts.IsMale
                ? $"assets/clothes/top/Top_Male_{topType}.png"
                : $"assets/clothes/top/Top_Female_{topType}.png";

            // 하의, 신발, 넥칼라 경로
            string bottomPath = parts.IsMale
                ? $"assets/clothes/bottom/Pants_{parts.BottomKey}.png"
                : $"assets/clothes/bottom/Skirt_{parts.BottomKey}.png";
            string shoesPath = $"assets/clothes/shoes/Shoes_{parts.ShoesKey}.png";
            string neckPath = $"assets/clothes/neck/Neck_{parts.NeckKey}.png";

            // 헤어, 눈, 피부 경로
            string spouseName = parts.SpouseName;
            string hairPath = $"assets/{spouseName}/toddler/hair/{spouseName}_toddler_{parts.HairKey}.png";
            string eyePath = $"assets/{spouseName}/toddler/eye/{spouseName}_toddler_eye.png";
            string skinPath = $"assets/{spouseName}/toddler/skin/{spouseName}_toddler_skin.png";

            // 축제복 경로 (남/여 공용 상의, 모자, 넥칼라까지 계절별 적용)
            string festivalKey = parts.IsMale ? "Male" : "Female";
            string festivalTop = "", festivalHat = "", festivalNeck = "";

            if (Game1.isFestivalDay || Game1.isFestival)
            {
                // 봄: 모자만
                if (season == "spring")
                {
                    festivalHat = $"assets/clothes/festival/spring/FestivalHat_{festivalKey}_Spring.png";
                    ApplyIfExist(child, festivalHat, shoesPath);
                    return;
                }
                // 여름: 상의 + 모자
                else if (season == "summer")
                {
                    festivalTop = $"assets/clothes/festival/summer/FestivalTop_{festivalKey}_Summer.png";
                    festivalHat = $"assets/clothes/festival/summer/FestivalHat_{festivalKey}_Summer.png";
                    ApplyIfExist(child, festivalTop, festivalHat, shoesPath);
                    return;
                }
                // 가을: 상의만
                else if (season == "fall" || season == "autumn")
                {
                    festivalTop = $"assets/clothes/festival/fall/FestivalTop_{festivalKey}_Fall.png";
                    ApplyIfExist(child, festivalTop, shoesPath);
                    return;
                }
                // 겨울: 상의 + 모자 + 넥칼라
                else if (season == "winter")
                {
                    festivalTop = $"assets/clothes/festival/winter/FestivalTop_{festivalKey}_Winter.png";
                    festivalHat = $"assets/clothes/festival/winter/FestivalHat_{festivalKey}_Winter.png";
                    festivalNeck = $"assets/clothes/festival/winter/FestivalNeck_{festivalKey}_Winter.png";
                    ApplyIfExist(child, festivalTop, festivalHat, festivalNeck, shoesPath);
                    return;
                }
            }

            // 잠옷 경로
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"assets/clothes/sleep/{parts.PajamaStyle}/{parts.PajamaStyle}_{parts.PajamaColorIndex:D2}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 일반 외형 적용 (순서: 헤어 → 상의 → 하의 → 눈 → 피부 → 신발 → 넥칼라)
            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        /// <summary>
        /// 아기(Child) 외형 적용 (모든 아기 공용 Body, 눈/스킨/헤어는 배우자 폴더)
        /// </summary>
        public static void ApplyChildAppearance(Child Child, string spouseName)
        {
            if (Child == null || string.IsNullOrEmpty(spouseName)) return;

            // 공용 바디(의상) 경로
            string bodyPath = "assets/clothes/Child/Child_Body.png";
            // 배우자별 헤어, 눈, 스킨 경로
            string hairPath = $"assets/{spouseName}/Child/hair/{spouseName}_Child_hair.png";
            string eyePath = $"assets/{spouseName}/Child/eye/{spouseName}_Child_eye.png";
            string skinPath = $"assets/{spouseName}/Child/skin/{spouseName}_Child_skin.png";

            // 적용 (순서: 바디 → 헤어 → 눈 → 스킨)
            ApplyIfExist(Child, bodyPath, hairPath, eyePath, skinPath);
        }

        // 경로 중 존재하는 파일만 적용
        private static void ApplyIfExist(Character character, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (string.IsNullOrEmpty(path)) continue;
                if (File.Exists(path))
                {
                    character.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
            }
        }

        /// <summary>
        /// 자녀 인스턴스에서 배우자 이름 추출 (null, 값 없는 경우 "Unknown" 반환)
        /// </summary>
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
            catch { }
            return "Unknown";
        }
    }
}