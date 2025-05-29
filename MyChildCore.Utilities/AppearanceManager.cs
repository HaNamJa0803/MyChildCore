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

            string parentName = GetParentName(child);
            if (string.IsNullOrEmpty(parentName) || parentName == "Unknown")
            {
                CustomLogger.Warn($"[AppearanceManager] 부모명 누락: {child?.Name ?? "??"}");
                return;
            }

            string baseDir = $"assets/{parentName}/";
            string ageFolder = (child.Age == 0) ? "Baby" : "Toddler";
            string fullBase = $"{baseDir}{ageFolder}/";

            if (child.Age == 0)
            {
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_헤어.png");
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_눈.png");
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_스킨.png");
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_보디.png");
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

            ApplySpriteIfExists(child, $"{fullBase}Hair/{parentName}_{data.HairKey}.png");
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

        // 최신 API 반영: 부모명 가져오기
        public static string GetParentName(Child child)
        {
            try
            {
                string mother = child.GetMotherName();
                if (!string.IsNullOrEmpty(mother))
                    return mother;
                string father = child.GetFatherName();
                if (!string.IsNullOrEmpty(father))
                    return father;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[AppearanceManager] 부모명 추출 실패: {ex.Message}");
            }
            return "Unknown";
        }
    }
}