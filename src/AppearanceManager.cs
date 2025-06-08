using System;
using System.Collections.Generic;
using StardewModdingAPI;
using System.IO;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// Child appearance manager (hardcoded, safe & explicit for each season/gender/event/pajama)
    /// </summary>
    public static class AppearanceManager
    {
        // 아기(Baby) 외형 적용
        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
                return;

            string spouse = parts.SpouseName ?? "Unknown";
            string hairPath = $"assets/{spouse}/Baby/Hair/{parts.BabyHairStyles}.png";
            string eyePath = $"assets/{spouse}/Baby/Eye/{parts.BabyEyes}.png";
            string skinPath = $"assets/{spouse}/Baby/Skin/{parts.BabySkins}.png";
            string bodyPath = $"assets/Clothes/BabyBody/{parts.BabyBodies}.png";
            ApplyIfExist(child, bodyPath, hairPath, eyePath, skinPath);
        }

        // 유아(토들러) 외형 적용
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
                return;

            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
            // parts에서 파생 정보만 사용! config 직접 접근 X
            bool usePajama = parts.EnablePajama;
            bool useFestival = parts.EnableFestival;
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            bool isMale = parts.IsMale;

            // 1. 잠옷
            if (usePajama && isNight)
            {
                string pajamaPath = $"assets/Clothes/Sleep/{(isMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions)}/{(isMale ? parts.BoyPajamaColorOptions : parts.GirlPajamaColorOptions)}.png";
                ApplyIfExist(child, pajamaPath);
                return;
            }

            // 2. 축제복
            if (useFestival && Game1.isFestival())
            {
                string festivalSpringHatPath = $"assets/Clothes/Festival/Spring/{parts.FestivalSpringHat}.png";
                string festivalSummerHatPath = $"assets/Clothes/Festival/Summer/{parts.FestivalSummerHat}.png";
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
                        ? $"assets/Clothes/Festival/Summer/{parts.BoyFestivalSummerPantsOptions}.png"
                        : $"assets/Clothes/Festival/Summer/{parts.GirlFestivalSummerSkirtOptions}.png";
                    ApplyIfExist(child, summerHatPath, summerBottomPath);
                    return;
                }
                else if (season == "fall")
                {
                    string fallBottomPath = isMale
                        ? $"assets/Clothes/Festival/Fall/{parts.BoyFestivalFallPants}.png"
                        : $"assets/Clothes/Festival/Fall/{parts.GirlFestivalFallSkirts}.png";
                    ApplyIfExist(child, fallBottomPath);
                    return;
                }
                else if (season == "winter")
                {
                    string winterHatPath = festivalWinterHatPath;
                    string winterScarfPath = festivalWinterScarfPath;
                    string winterBottomPath = isMale
                        ? $"assets/Clothes/Festival/Winter/{parts.BoyFestivalWinterPantsOptions}.png"
                        : $"assets/Clothes/Festival/Winter/{parts.GirlFestivalWinterSkirtOptions}.png";
                    ApplyIfExist(child, winterHatPath, winterBottomPath, winterScarfPath);
                    return;
                }
            }

            // 3. 평상복(계절/성별)
            string topPath = null;
            if (season == "spring")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopSpringOptions}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopSpringOptions}.png";
            }
            else if (season == "summer")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopSummerOptions}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopSummerOptions}.png";
            }
            else if (season == "fall")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopFallOptions}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopFallOptions}.png";
            }
            else if (season == "winter")
            {
                topPath = isMale
                    ? $"assets/Clothes/Top/Boy/{parts.BoyTopWinterOptions}.png"
                    : $"assets/Clothes/Top/Girl/{parts.GirlTopWinterOptions}.png";
            }

            string bottomPath = isMale
                ? $"assets/Clothes/Bottom/Pants/{parts.PantsColorOptions}.png"
                : $"assets/Clothes/Bottom/Skirt/{parts.SkirtColorOptions}.png";
            string shoesPath = $"assets/Clothes/Shoes/{(isMale ? parts.BoyShoesColorOptions : parts.GirlShoesColorOptions)}.png";
            string neckPath = $"assets/Clothes/NeckCollar/{(isMale ? parts.BoyNeckCollarColorOptions : parts.GirlNeckCollarColorOptions)}.png";

            string spouse = parts.SpouseName ?? "Unknown";
            string hairPath = isMale
                ? $"assets/{spouse}/Toddler/Hair/Short.png"
                : $"assets/{spouse}/Toddler/Hair/{parts.GirlHairStyles}.png";
            string eyePath = $"assets/{spouse}/Toddler/Eye/{(isMale ? parts.BoyEyes : parts.GirlEyes)}.png";
            string skinPath = $"assets/{spouse}/Toddler/Skin/{(isMale ? parts.BoySkins : parts.GirlSkins)}.png";

            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        // 파일 존재 시 스프라이트 적용
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

        // 배우자 이름 추출
        public static string GetSpouseName(Child child)
        {
            if (child == null) return "Unknown";
            try
            {
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    foreach (var farmer in Game1.getAllFarmers())
                    {
                        if (farmer.UniqueMultiplayerID == parentId && !string.IsNullOrEmpty(farmer.Name))
                            return farmer.Name;
                    }
                }
            }
            catch { }
            return "Unknown";
        }

        // GMCM 옵션 변경 시 해당 배우자/성별 자녀만 즉시 적용
        public static void ApplyForGMCMChange(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (GetSpouseName(child) != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts;
                if (child.Age >= 1)
                    parts = PartsManager.GetPartsForChild(child, config);
                else
                    parts = PartsManager.GetPartsForBaby(child, config);

                if (parts == null)
                    continue;

                if (child.Age >= 1)
                    ApplyToddlerAppearance(child, parts);
                else
                    ApplyBabyAppearance(child, parts);
            }
        }
    }
}