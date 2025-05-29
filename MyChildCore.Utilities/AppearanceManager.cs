using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// Child 외형 관리 헬퍼 (1.6.10+ 완전 대응, 하드코딩 방식)
    /// - 게임 엔진의 강제 외형 변경 문제를 해결하기 위해, 조건마다 무조건 강제 적용
    /// - 성능보단 안정성과 정확성 최우선
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// ChildData 기반 외형 강제 적용
        /// - 시간대/축제 상황에 따라 자녀 외형을 무조건 덮어씀
        /// </summary>
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

            // 기본 경로
            string baseDir = $"assets/{spouseName}/";
            string ageFolder = (child.Age == 0) ? "Baby" : "Toddler";
            string fullBase = $"{baseDir}{ageFolder}/";

            // 아기(Baby) 처리: 모든 파츠 고정 (배우자 전용)
            if (child.Age == 0)
            {
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_헤어.png");
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_눈.png");
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_스킨.png");
                ApplySpriteIfExists(child, $"{fullBase}{spouseName}_보디.png");
                return;
            }

            // 밤 6시~6시: 잠옷 적용 (스타일/색상 번호 기반)
            if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
            {
                string pajamaPath = $"{fullBase}Clothes/Sleep/{data.PajamaStyle}_{data.PajamaColor}.png";
                ApplySpriteIfExists(child, pajamaPath);
                return;
            }

            // 축제일: 시즌별 하드코딩 외형 적용
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

            // 평상시: 선택 파츠 강제 적용 (헤어/하의/신발/넥칼라)
            ApplySpriteIfExists(child, $"{fullBase}Hair/{spouseName}_{data.HairKey}.png");
            ApplySpriteIfExists(child, $"{fullBase}Clothes/Bottom/하의_{data.BottomKey}.png");
            ApplySpriteIfExists(child, $"{fullBase}Clothes/Shoes/신발_{data.ShoesKey}.png");
            ApplySpriteIfExists(child, $"{fullBase}Clothes/NeckCollar/넥칼라_{data.NeckCollarKey}.png");
        }

        /// <summary>
        /// 경로 존재 확인 후 스프라이트 강제 적용
        /// </summary>
        private static void ApplySpriteIfExists(Child child, string path)
        {
            if (File.Exists(path))
            {
                child.Sprite = new AnimatedSprite(path, 0, 16, 32);
                CustomLogger.Debug($"[AppearanceManager] 적용 완료: {child.Name} ({path})");
            }
        }

        /// <summary>
        /// 배우자 이름 추출 (spouse → motherName → fatherName)
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
            catch (Exception ex)
            {
                CustomLogger.Warn($"[AppearanceManager] 배우자명 추출 실패: {ex.Message}");
            }
            return "Unknown";
        }
    }
}