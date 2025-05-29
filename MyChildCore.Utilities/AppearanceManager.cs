using System;
using StardewValley;
using StardewValley.Characters;
using System.IO;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형(스프라이트) 변경 헬퍼 (Baby/Toddler 연령별, 배우자별 파츠 완전 대응)
    /// - 폴더 구조: assets/배우자명/Baby/, assets/배우자명/Toddler/
    /// - 파일명 규칙: {배우자명}_헤어, {배우자명}_스킨 등, 하의/신발/넥칼라는 번호
    /// - 성능/안정성/누락0%/오류 즉시 로그/확장성100%
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 자녀에게 새 스프라이트를 적용합니다. (절대 Texture2D 직접 할당 금지, 경로만 전달!)
        /// </summary>
        public static void ApplyAppearance(Child child, string spritePath)
        {
            if (child == null || string.IsNullOrEmpty(spritePath))
                return;
            child.Sprite = new AnimatedSprite(spritePath, 0, 16, 32);
            CustomLogger.Debug($"[AppearanceManager] 스프라이트 적용: {child.Name} ({spritePath})");
        }

        /// <summary>
        /// 자녀의 배우자명(부모명) 자동 추출 (누락/예외시 "Unknown" 반환)
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

        /// <summary>
        /// 연령(Baby/Toddler) 및 파츠 세트에 따라 자동 경로 생성 및 적용
        /// </summary>
        public static void ApplyParts(
            Child child,
            bool isMale,              // 성별 (true=남, false=여)
            string hairType,          // Baby: ""(무시), Toddler: "숏"/"체리트윈"/"트윈테일"/"포니테일"
            int bottomIndex,          // Toddler: 하의(1~10), Baby: 0
            int shoesIndex,           // Toddler: 신발(1~4), Baby: 0
            int neckIndex             // Toddler: 넥칼라(1~26), Baby: 0
        )
        {
            if (child == null)
                return;

            string spouseName = GetSpouseName(child);
            if (string.IsNullOrEmpty(spouseName) || spouseName == "Unknown")
            {
                CustomLogger.Warn($"[AppearanceManager] 배우자명이 누락된 자녀: {child?.Name ?? "??"}");
                return;
            }

            string baseDir = $"assets/{spouseName}/";
            string ageFolder = (child.Age == 0) ? "Baby" : "Toddler";
            string fullBase = $"{baseDir}{ageFolder}/";

            if (child.Age == 0) // Baby(아기)
            {
                string hairPath = $"{fullBase}{spouseName}_헤어.png";
                string eyePath  = $"{fullBase}{spouseName}_눈.png";
                string skinPath = $"{fullBase}{spouseName}_스킨.png";
                string bodyPath = $"{fullBase}{spouseName}_보디.png";
                if (File.Exists(skinPath))         ApplyAppearance(child, skinPath);
                else if (File.Exists(hairPath))    ApplyAppearance(child, hairPath);
                else if (File.Exists(eyePath))     ApplyAppearance(child, eyePath);
                else if (File.Exists(bodyPath))    ApplyAppearance(child, bodyPath);
                else CustomLogger.Warn($"[AppearanceManager] 아기 파츠 파일 누락: {child.Name} ({spouseName})");
            }
            else // Toddler(유아)
            {
                string hairKey = isMale ? "숏" : hairType;
                string hairPath = $"{fullBase}{spouseName}_{hairKey}.png";
                string eyePath    = $"{fullBase}{spouseName}_눈.png";
                string skinPath   = $"{fullBase}{spouseName}_스킨.png";
                string topPath    = $"{fullBase}{spouseName}_상의.png";
                string bottomPath = $"{fullBase}하의_{bottomIndex:D2}.png";
                string shoesPath  = $"{fullBase}신발_{shoesIndex:D2}.png";
                string neckPath   = $"{fullBase}넥칼라_{neckIndex:D2}.png";

                if (File.Exists(topPath))          ApplyAppearance(child, topPath);
                else if (File.Exists(hairPath))    ApplyAppearance(child, hairPath);
                else if (File.Exists(skinPath))    ApplyAppearance(child, skinPath);
                else if (File.Exists(eyePath))     ApplyAppearance(child, eyePath);
                else if (File.Exists(bottomPath))  ApplyAppearance(child, bottomPath);
                else if (File.Exists(shoesPath))   ApplyAppearance(child, shoesPath);
                else if (File.Exists(neckPath))    ApplyAppearance(child, neckPath);
                else CustomLogger.Warn($"[AppearanceManager] 유아 파츠 파일 누락: {child.Name} ({spouseName})");
            }
        }
    }
}