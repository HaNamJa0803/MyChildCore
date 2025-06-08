using System;
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
            {
                CustomLogger.Warn("[ApplyBabyAppearance] child or parts null");
                return;
            }

            string spouse = GetRealSpouseName(child);

            // 배우자 키 자동 생성(방어)
            if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
            {
                ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                CustomLogger.Warn($"[AppearanceManager] 누락된 배우자 키 자동 추가: {spouse}");
            }

            string hairPath = $"assets/{spouse}/Baby/Hair/{parts.BabyHairStyles}.png";
            string eyePath = $"assets/{spouse}/Baby/Eye/{parts.BabyEyes}.png";
            string skinPath = $"assets/{spouse}/Baby/Skin/{parts.BabySkins}.png";
            string bodyPath = $"assets/Clothes/BabyBody/{parts.BabyBodies}.png";

            CustomLogger.Info($"[ApplyBabyAppearance] {child.Name} 적용시도: {bodyPath}, {hairPath}, {eyePath}, {skinPath}");

            ApplyIfExist(child, bodyPath, hairPath, eyePath, skinPath);
        }

        // 유아(토들러) 외형 적용
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
            {
                CustomLogger.Warn("[ApplyToddlerAppearance] child or parts null");
                return;
            }

            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
            bool usePajama = parts.EnablePajama;
            bool useFestival = parts.EnableFestival;
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            bool isMale = parts.IsMale;
            string spouse = GetRealSpouseName(child);

            // 배우자 키 자동 생성(방어)
            if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
            {
                ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                CustomLogger.Warn($"[AppearanceManager] 누락된 배우자 키 자동 추가: {spouse}");
            }

            // 1. 잠옷
            if (usePajama && isNight)
            {
                string pajamaPath = $"assets/Clothes/Sleep/{(isMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions)}/{(isMale ? parts.BoyPajamaColorOptions : parts.GirlPajamaColorOptions)}.png";
                CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 잠옷 적용: {pajamaPath}");
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
                    CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 봄 축제 모자 적용: {festivalSpringHatPath}");
                    ApplyIfExist(child, festivalSpringHatPath);
                    return;
                }
                else if (season == "summer")
                {
                    string summerHatPath = $"assets/Clothes/Festival/Summer/{(isMale ? "BoyHat" : "GirlHat")}.png";
                    string summerBottomPath = isMale
                        ? $"assets/Clothes/Festival/Summer/{parts.BoyFestivalSummerPantsOptions}.png"
                        : $"assets/Clothes/Festival/Summer/{parts.GirlFestivalSummerSkirtOptions}.png";
                    CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 여름 축제 적용: {summerHatPath}, {summerBottomPath}");
                    ApplyIfExist(child, summerHatPath, summerBottomPath);
                    return;
                }
                else if (season == "fall")
                {
                    string fallBottomPath = isMale
                        ? $"assets/Clothes/Festival/Fall/{parts.BoyFestivalFallPants}.png"
                        : $"assets/Clothes/Festival/Fall/{parts.GirlFestivalFallSkirts}.png";
                    CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 가을 축제 적용: {fallBottomPath}");
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
                    CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 겨울 축제 적용: {winterHatPath}, {winterBottomPath}, {winterScarfPath}");
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

            string hairPath = isMale
                ? $"assets/{spouse}/Toddler/Hair/ShortCut.png"
                : $"assets/{spouse}/Toddler/Hair/{parts.GirlHairStyles}.png";
            string eyePath = $"assets/{spouse}/Toddler/Eye/{(isMale ? parts.BoyEyes : parts.GirlEyes)}.png";
            string skinPath = $"assets/{spouse}/Toddler/Skin/{(isMale ? parts.BoySkins : parts.GirlSkins)}.png";

            CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 평상복 적용: {hairPath}, {topPath}, {bottomPath}, {eyePath}, {skinPath}, {shoesPath}, {neckPath}");

            ApplyIfExist(child, hairPath, topPath, bottomPath, eyePath, skinPath, shoesPath, neckPath);
        }

        // 파일 존재 시 스프라이트 적용 (존재/미존재 모두 로그)
        private static void ApplyIfExist(Character character, params string[] spritePaths)
        {
            foreach (var path in spritePaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    CustomLogger.Info($"[ApplyIfExist] 적용 성공: {path}");
                    character.Sprite = new AnimatedSprite(path, 0, 16, 32);
                    break;
                }
                else
                {
                    CustomLogger.Warn($"[ApplyIfExist] 파일 없음: {path}");
                }
            }
        }

        // 진짜 배우자 이름만 반환!
        public static string GetRealSpouseName(Child child)
        {
            if (child == null) return "Unknown";
            try
            {
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    foreach (var farmer in Game1.getAllFarmers())
                    {
                        // 플레이어 자신의 spouse 필드를 무조건 사용!
                        if (farmer.UniqueMultiplayerID == parentId && !string.IsNullOrEmpty(farmer.spouse))
                        {
                            // 배우자명이 드롭다운Config에 포함되는지 반드시 체크!
                            if (DropdownConfig.SpouseNames.Contains(farmer.spouse))
                                return farmer.spouse;
                        }
                    }
                }
            }
            catch { }
            return "Default";
        }

        // GMCM 옵션 변경 시 해당 배우자/성별 자녀만 즉시 적용
        public static void ApplyForGMCMChange(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (GetRealSpouseName(child) != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts;
                if (child.Age >= 1)
                    parts = PartsManager.GetPartsForChild(child);
                else
                    parts = PartsManager.GetPartsForBaby(child);

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