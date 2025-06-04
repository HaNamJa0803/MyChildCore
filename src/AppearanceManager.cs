using System;
using System.Collections.Generic;
using StardewModdingAPI;
using System.IO;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// Child appearance manager (hardcoded, safe & explicit for each season/gender/event/pajama)
    /// - 아기 → 유아(토들러) 순서로 분기/주석/구현
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 아기(Baby) 외형 적용 (이벤트/잠옷 분기 없음)
        /// </summary>
        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null || !DropdownConfig.EnableMod)
                return;

            string spouse = parts.SpouseName ?? "Unknown";
            string hairPath = $"assets/{spouse}/Baby/Hair/{parts.BabyHairStyle}.png";
            string eyePath = $"assets/{spouse}/Baby/Eye/{parts.BabyEye}.png";
            string skinPath = $"assets/{spouse}/Baby/Skin/{parts.BabySkin}.png";
            string bodyPath = $"assets/Clothes/BabyBody/{parts.BabyBody}.png";
            ApplyIfExist(child, bodyPath, hairPath, eyePath, skinPath);
        }

        /// <summary>
        /// 유아(토들러) 외형 적용 (계절/성별/축제/잠옷 완전 하드코딩 분기)
        /// </summary>
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null || !DropdownConfig.EnableMod)
                return;

            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
            bool usePajama = DropdownConfig.EnablePajama;
            bool useFestival = DropdownConfig.EnableFestival;
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            bool isMale = parts.IsMale;

            // 1. 잠옷
            if (usePajama && isNight)
            {
                string pajamaPath = $"assets/Clothes/Sleep/{(isMale ? parts.BoyPajamaType : parts.GirlPajamaType)}/{(isMale ? parts.BoyPajamaColor : parts.GirlPajamaColor)}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 2. 축제복
            if (useFestival && Game1.isFestival())
            {
                string festivalSpringHatPath = $"assets/Clothes/Festival/Spring/{parts.FestivalSpringHat}.png";
                string festivalWinterHatPath = $"assets/Clothes/Festival/Winter/{parts.FestivalWinterHat}.png";
                string festivalWinterScarfPath = $"assets/Clothes/Festival/Winter/{parts.FestivalWinterScarf}.png";
                if (season == "spring")
                {
                    ApplyIfExist(child, festivalSpringHatPath);
                    return;
                }
                else if (season == "summer")
                {
                    string summerHatPath = $"assets/Clothes/Festival/Summer/{(isMale ? "BoyHat" : "GirlHat")}.png";
                    string summerBottomPath = isMale
                        ? $"assets/Clothes/Festival/Summer/{parts.BoyFestivalSummerPants}.png"
                        : $"assets/Clothes/Festival/Summer/{parts.GirlFestivalSummerSkirt}.png";
                    ApplyIfExist(child, summerHatPath, summerBottomPath);
                    return;
                }
                else if (season == "fall")
                {
                    string fallBottomPath = isMale
                        ? $"assets/Clothes/Festival/Fall/{parts.BoyFestivalFallPants}.png"
                        : $"assets/Clothes/Festival/Fall/{parts.GirlFestivalFallSkirt}.png";
                    ApplyIfExist(child, fallBottomPath);
                    return;
                }
                else if (season == "winter")
                {
                    string winterHatPath = festivalWinterHatPath;
                    string winterScarfPath = festivalWinterScarfPath;
                    string winterBottomPath = isMale
                        ? $"assets/Clothes/Festival/Winter/{parts.BoyFestivalWinterPants}.png"
                        : $"assets/Clothes/Festival/Winter/{parts.GirlFestivalWinterSkirt}.png";
                    ApplyIfExist(child, winterHatPath, winterBottomPath, winterScarfPath);
                    return;
                }
            }

            // 3. 평상복(계절/성별)
            string topPath = null;
            if (season == "spring")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopSpring}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopSpring}.png";
            }
            else if (season == "summer")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopSummer}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopSummer}.png";
            }
            else if (season == "fall")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopFall}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopFall}.png";
            }
            else if (season == "winter")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopWinter}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopWinter}.png";
            }

            string bottomPath = isMale
                ? $"assets/Clothes/Bottom/Pants/{parts.BoyPantsColor}.png"
                : $"assets/Clothes/Bottom/Skirt/{parts.GirlSkirtColor}.png";
            string shoesPath = $"assets/Clothes/Shoes/{(isMale ? parts.BoyShoesColor : parts.GirlShoesColor)}.png";
            string neckPath = $"assets/Clothes/NeckCollar/{(isMale ? parts.BoyNeckCollarColor : parts.GirlNeckCollarColor)}.png";

            string spouse = parts.SpouseName ?? "Unknown";
            string hairPath = isMale
                ? $"assets/{spouse}/Toddler/Hair/Short.png"
                : $"assets/{spouse}/Toddler/Hair/{parts.GirlHairStyle}.png";
            string eyePath = $"assets/{spouse}/Toddler/Eye/{(isMale ? parts.BoyEye : parts.GirlEye)}.png";
            string skinPath = $"assets/{spouse}/Toddler/Skin/{(isMale ? parts.BoySkin : parts.GirlSkin)}.png";

            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        /// <summary>
        /// 파일 존재 시 스프라이트 적용
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
        /// 배우자 이름 추출 (예외 방어)
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
        public static void ApplyForGMCMChange(string spouse, bool isMale)
        {
            if (!DropdownConfig.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (GetSpouseName(child) != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts = PartsManager.GetPartsForChild(child);
                if (parts == null) continue;

                if (child.Age >= 1)
                    ApplyToddlerAppearance(child, parts);
                else
                    ApplyBabyAppearance(child, parts);
            }
        }
    }
}