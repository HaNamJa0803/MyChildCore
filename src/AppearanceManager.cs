using System.IO;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class AppearanceManager
    {
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;

            string topType = (Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower() is "spring" or "summer") ? "Short" : "Long";
            string topPath = parts.IsMale
                ? $"assets/Clothes/Top/{parts.TopKeyMaleShort}.png"
                : $"assets/Clothes/Top/{parts.TopKeyFemaleShort}.png";
            if (topType == "Long")
                topPath = parts.IsMale
                    ? $"assets/Clothes/Top/{parts.TopKeyMaleLong}.png"
                    : $"assets/Clothes/Top/{parts.TopKeyFemaleLong}.png";

            string bottomPath = parts.IsMale
                ? $"assets/Clothes/Bottom/Pants/{parts.PantsKeyMale}.png"
                : $"assets/Clothes/Bottom/Skirt/{parts.SkirtKeyFemale}.png";
            string shoesPath = $"assets/Clothes/Shoes/{parts.ShoesKey}.png";
            string neckPath  = $"assets/Clothes/Neck/{parts.NeckKey}.png";
            string hairPath  = $"assets/{parts.SpouseName}/Toddler/Hair/{parts.HairKey}.png";
            string eyePath   = $"assets/{parts.SpouseName}/Toddler/Eye/{parts.EyeKey}.png";
            string skinPath  = $"assets/{parts.SpouseName}/Toddler/Skin/{parts.SkinKey}.png";

            // 축제복 분기
            if (Game1.isFestival())
            {
                string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
                if (season == "spring")
                    ApplyIfExist(child, parts.IsMale ? parts.SpringHatKeyMale : parts.SpringHatKeyFemale, shoesPath);
                else if (season == "summer")
                    ApplyIfExist(child,
                        parts.IsMale ? parts.SummerTopKeyMale : parts.SummerTopKeyFemale,
                        parts.IsMale ? parts.SummerHatKeyMale : parts.SummerHatKeyFemale,
                        shoesPath);
                else if (season == "fall" || season == "autumn")
                    ApplyIfExist(child, parts.IsMale ? parts.FallTopKeyMale : parts.FallTopKeyFemale, shoesPath);
                else if (season == "winter")
                    ApplyIfExist(child,
                        parts.IsMale ? parts.WinterTopKeyMale : parts.WinterTopKeyFemale,
                        parts.IsMale ? parts.WinterHatKeyMale : parts.WinterHatKeyFemale,
                        parts.WinterNeckKey,
                        shoesPath);
                return;
            }

            // 잠옷 분기
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"assets/Clothes/Sleep/{parts.PajamaStyle}/{parts.PajamaKey}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 일반 외형
            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;
            ApplyIfExist(child,
                parts.BabyBodyKey,
                parts.BabyHairKey,
                parts.BabyEyeKey,
                parts.BabySkinKey);
        }

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

        public static string GetSpouseName(Child child)
        {
            if (child == null) return "Unknown";
            long parentId = child.idOfParent?.Value ?? -1;
            if (parentId > 0)
            {
                var parent = Game1.getAllFarmers().FirstOrDefault(f => f.UniqueMultiplayerID == parentId);
                if (parent != null && !string.IsNullOrEmpty(parent.Name))
                    return parent.Name;
            }
            return "Unknown";
        }
    }
}