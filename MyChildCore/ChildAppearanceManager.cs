using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI;
using System;

namespace MyChildCore
{
    public static class ChildAppearanceManager
    {
        public static void Apply(Child child, ModConfig config)
        {
            try
            {
                if (child == null || child.spouse == null)
                    return;

                string spouse = child.spouse;
                string gender = child.Gender == 0 ? "Boy" : "Girl";

                // 헤어
                string hairStyle = config.Get(spouse, gender, "Hair");
                string hairColor = config.Get(spouse, gender, "HairColor");

                // 잠옷
                string sleepStyle = config.Get(spouse, gender, "SleepStyle");
                string sleepColor = config.Get(spouse, gender, "SleepColor");

                // 계절
                string season = config.EnableSeasonalClothing ? Game1.currentSeason : "Spring";
                string seasonalStyle = config.Get(spouse, gender, $"{season}Style");
                string seasonalColor = config.Get(spouse, gender, $"{season}Color");

                // 파츠 적용 (각 함수는 내부에서 sprite 변경 처리)
                PartApplier.ApplyHair(child, hairStyle, hairColor);
                PartApplier.ApplySleepwear(child, sleepStyle, sleepColor);
                PartApplier.ApplySeasonalOutfit(child, seasonalStyle, seasonalColor, season, config.EnableHat);

                // 외형 적용 완료 기록
                child.modData["mychild.applied"] = "true";
            }
            catch (Exception ex)
            {
                ModEntry.Instance.Monitor.Log($"[MyChildCore] 자녀 외형 적용 중 오류: {ex}", LogLevel.Error);
            }
        }
    }
}