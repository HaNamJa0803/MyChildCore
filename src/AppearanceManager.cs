using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 외형 유지/적용 하드코딩 분기 모듈 (보조 DLL)
    /// SDV 1.6.10+ 완전 대응, 계절/축제/잠옷/GMCM/상의 긴팔/하드코딩 분기 모두 포함
    /// </summary>
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

            // 1. Baby(아기): 배우자 고정 파츠 100% 적용
            if (child.Age == 0)
            {
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_헤어.png");
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_눈.png");
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_스킨.png");
                ApplySpriteIfExists(child, $"{fullBase}{parentName}_보디.png");
                return;
            }

            // 2. 잠옷 시간 (18:00~06:00): 잠옷 스타일/색상
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"{fullBase}Clothes/Sleep/{data.PajamaStyle}_{data.PajamaColor}.png";
                ApplySpriteIfExists(child, pajamaPath);
                return;
            }

            // 3. 축제: 시즌별 하드코딩 파츠
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
                    ApplySpriteIfExists(child, $"{festDir}상의_긴팔.png");
                }
                else if (season == "winter")
                {
                    ApplySpriteIfExists(child, $"{festDir}모자.png");
                    ApplySpriteIfExists(child, $"{festDir}넥칼라_{data.NeckCollarKey}.png");
                    ApplySpriteIfExists(child, $"{festDir}상의_긴팔.png");
                }
                return;
            }

            // 4. 평상시: GMCM 파츠 + 시즌별 상의(가을/겨울=긴팔, 그 외=반팔)
            ApplySpriteIfExists(child, $"{fullBase}Hair/{parentName}_{data.HairKey}.png");

            string topFile =
                (Game1.currentSeason == "fall" || Game1.currentSeason == "winter")
                ? $"{fullBase}Clothes/Top/{parentName}_상의_긴팔.png"
                : $"{fullBase}Clothes/Top/{parentName}_상의.png";
            ApplySpriteIfExists(child, topFile);

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

        public static string GetParentName(Child child)
        {
            try
            {
                if (child != null)
                {
                    var parent = Game1.GetPlayer(child.idOfParent.Value);
                    if (parent != null)
                        return parent.Name;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[AppearanceManager] 부모명 추출 실패: {ex.Message}");
            }
            return "Unknown";
        }
    }
}