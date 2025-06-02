using System.IO;
using System.Linq;
using StardewValley;
using StardewValley.Characters;
using MyChildCore;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 실시간 적용/즉시반영 매니저 (GMCM 구조와 동기화)
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 유아(토들러) 외형 적용
        /// </summary>
        public static void ApplyToddlerAppearance(Child child, ChildParts parts, DropdownConfig config)
        {
            if (child == null || parts == null || config == null) return;

            // 잠옷(비활성화시 기본복장)
            bool usePajama = config.EnablePajama;
            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
            if (usePajama && isNight)
            {
                string pajamaPath = $"assets/Clothes/Sleep/{parts.PajamaStyle}/{parts.PajamaKey}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 축제복(비활성화시 무시)
            bool useFestival = config.EnableFestival;
            if (useFestival && Game1.isFestival())
            {
                string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
                if (season == "spring")
                {
                    ApplyIfExist(child, parts.IsMale ? parts.SpringHatKeyMale : parts.SpringHatKeyFemale, parts.ShoesKey);
                }
                else if (season == "summer")
                {
                    ApplyIfExist(child,
                        parts.IsMale ? parts.SummerTopKeyMale : parts.SummerTopKeyFemale,
                        parts.IsMale ? parts.SummerHatKeyMale : parts.SummerHatKeyFemale,
                        parts.ShoesKey);
                }
                else if (season == "fall" || season == "autumn")
                {
                    ApplyIfExist(child, parts.IsMale ? parts.FallTopKeyMale : parts.FallTopKeyFemale, parts.ShoesKey);
                }
                else if (season == "winter")
                {
                    ApplyIfExist(child,
                        parts.IsMale ? parts.WinterTopKeyMale : parts.WinterTopKeyFemale,
                        parts.IsMale ? parts.WinterHatKeyMale : parts.WinterHatKeyFemale,
                        parts.WinterNeckKey,
                        parts.ShoesKey);
                }
                return;
            }

            // 일반복(계절에 따라 상의 롱/숏 자동)
            string topType = (Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower() is "spring" or "summer") ? "Short" : "Long";
            string topPath = parts.IsMale
                ? $"assets/Clothes/Top/{(topType == "Short" ? parts.TopKeyMaleShort : parts.TopKeyMaleLong)}.png"
                : $"assets/Clothes/Top/{(topType == "Short" ? parts.TopKeyFemaleShort : parts.TopKeyFemaleLong)}.png";

            string bottomPath = parts.IsMale
                ? $"assets/Clothes/Bottom/Pants/{parts.PantsKeyMale}.png"
                : $"assets/Clothes/Bottom/Skirt/{parts.SkirtKeyFemale}.png";
            string shoesPath = $"assets/Clothes/Shoes/{parts.ShoesKey}.png";
            string neckPath = $"assets/Clothes/Neck/{parts.NeckKey}.png";
            string hairPath = $"assets/{parts.SpouseName}/Toddler/Hair/{parts.HairKey}.png";
            string eyePath = $"assets/{parts.SpouseName}/Toddler/Eye/{parts.EyeKey}.png";
            string skinPath = $"assets/{parts.SpouseName}/Toddler/Skin/{parts.SkinKey}.png";

            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        /// <summary>
        /// 아기 외형 적용 (축제/잠옷 옵션 없음)
        /// </summary>
        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;
            ApplyIfExist(child,
                parts.BabyBodyKey,
                parts.BabyHairKey,
                parts.BabyEyeKey,
                parts.BabySkinKey);
        }

        /// <summary>
        /// 스프라이트 파일 존재하면 적용 (첫 번째 유효 경로 적용)
        /// </summary>
        private static void ApplyIfExist(Character character, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    character.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
            }
        }

        /// <summary>
        /// 자녀의 배우자(부모) 이름 추출 (예외 방어)
        /// </summary>
        public static string GetSpouseName(Child child)
        {
            if (child == null) return "Unknown";
            try
            {
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    var parent = Game1.getAllFarmers().FirstOrDefault(f => f.UniqueMultiplayerID == parentId);
                    if (parent != null && !string.IsNullOrEmpty(parent.Name))
                        return parent.Name;
                }
            }
            catch { }
            return "Unknown";
        }

        /// <summary>
        /// GMCM 옵션 변경 시 해당 배우자/성별 자녀만 즉시 적용
        /// </summary>
        public static void ApplyForGMCMChange(string spouse, bool isMale, DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                if (GetSpouseName(child) != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts = PartsManager.GetPartsForChild(child, config);
                if (parts == null) continue;

                if (child.Age >= 1)
                    ApplyToddlerAppearance(child, parts, config);
                else
                    ApplyBabyAppearance(child, parts);
            }
        }
    }
}