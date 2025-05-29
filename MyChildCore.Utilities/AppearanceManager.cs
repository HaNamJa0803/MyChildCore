using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class AppearanceManager
    {
        public static void ApplyParts(Child child, ChildData data)
        {
            if (child == null || data == null)
                return;

            string spouseName = GetSpouseName(child);
            if (string.IsNullOrEmpty(spouseName) || spouseName == "Unknown")
            {
                CustomLogger.Warn($"[AppearanceManager] 배우자명 누락: {child?.Name ?? "??"}");
                return;
            }

            string baseDir = $"assets/{spouseName}/";
            string ageFolder = (child.Age == 0) ? "Baby" : "Toddler";
            string fullBase = $"{baseDir}{ageFolder}/";

            if (child.Age == 0)
            {
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_헤어.png");
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_눈.png");
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_스킨.png");
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_보디.png");
                return;
            }

            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"{fullBase}Clothes/Sleep/{data.PajamaStyle}_{data.PajamaColor}.png";
                ApplySpriteIfExists(child, pajamaPath);
                return;
            }

            if (Game1.CurrentEvent?.isFestival == true)
            {
                string season = Game1.currentSeason;
                string festDir = $"{fullBase}Clothes/Festival/{season}/";

                if (season == "spring")
                {
                    ApplySpriteIfExists(child, $"{festDir}모자.png");
                }
                else if (season == "summer")
                {
                    ApplySpriteIfExists(child, $"{festDir}모자.png");
                    ApplySpriteIfExists(child, $"{festDir}상의.png");
                }
                else if (season == "fall")
                {
                    ApplySpriteIfExists(child, $"{festDir}상의.png");
                }
                else if (season == "winter")
                {
                    ApplySpriteIfExists(child, $"{festDir}모자.png");
                    ApplySpriteIfExists(child, $"{festDir}넥칼라_{data.NeckCollarKey}.png");
                    ApplySpriteIfExists(child, $"{festDir}상의.png");
                }
                return;
            }

            ApplySpriteIfExists(child, $"{fullBase}Hair/{spouseName}_{data.HairKey}.png");
            ApplySpriteIfExists(child, $"{fullBase}Clothes/Bottom/하의_{data.BottomKey}.png");
            ApplySpriteIfExists(child, $"{fullBase}Clothes/Shoes/신발_{data.ShoesKey}.png");
            ApplySpriteIfExists(child, $"{fullBase}Clothes/NeckCollar/넥칼라_{data.NeckCollarKey}.png");
        }

        private static void ApplySpriteIfExists(Child child, string path)
        {
            if (File.Exists(path))
            {
                child.Sprite = new AnimatedSprite(path, 0, 16, 32);
                CustomLogger.Debug($"[AppearanceManager] 적용 완료: {child.Name} ({path})");
            }
        }

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