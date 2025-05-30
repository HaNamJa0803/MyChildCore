using System;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 베이비(아기) 외형 적용 전용 매니저 (보디 공용)
    /// </summary>
    public static class BabyAppearanceManager
    {
        /// <summary>
        /// 아기 파츠 적용 (눈/헤어/스킨은 배우자별, 바디는 공용)
        /// </summary>
        public static void ApplyBabyParts(Child child)
        {
            if (child == null)
                return;

            string spouseName = GetSpouseName(child);
            if (string.IsNullOrEmpty(spouseName) || spouseName == "Unknown")
            {
                CustomLogger.Warn($"[BabyAppearanceManager] 배우자명이 누락된 자녀: {child?.Name ?? "??"}");
                return;
            }

            // 폴더 구조에 맞춘 파츠 경로 (100% 일치)
            string basePath = $"assets/{spouseName}/Baby/";
            string eyePath  = $"{basePath}Eye/{spouseName}_Baby_Eye.png";
            string hairPath = $"{basePath}Hair/{spouseName}_Baby_Hair.png";
            string skinPath = $"{basePath}Skin/{spouseName}_Baby_Skin.png";
            // 바디(공용)
            string bodyPath = $"Clothes/Baby/Body.png";

            // 우선순위: 눈 → 헤어 → 스킨 → 바디
            if (File.Exists(eyePath))
                ApplyAppearance(child, eyePath);
            else if (File.Exists(hairPath))
                ApplyAppearance(child, hairPath);
            else if (File.Exists(skinPath))
                ApplyAppearance(child, skinPath);
            else if (File.Exists(bodyPath))
                ApplyAppearance(child, bodyPath);
            else
                CustomLogger.Warn($"[BabyAppearanceManager] 아기 파츠 파일 누락: {child.Name} ({spouseName})");
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
                CustomLogger.Warn($"[BabyAppearanceManager] 배우자명 추출 실패: {ex.Message}");
            }
            return "Unknown";
        }

        public static void ApplyAppearance(Child child, string spritePath)
        {
            if (child == null || string.IsNullOrEmpty(spritePath))
                return;
            child.Sprite = new AnimatedSprite(spritePath, 0, 16, 32);
            CustomLogger.Debug($"[BabyAppearanceManager] 스프라이트 적용: {child.Name} ({spritePath})");
        }
    }
}