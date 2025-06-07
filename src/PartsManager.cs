using System;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (GMCM 옵션 완전 반영)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 아기(0세) 파츠 조합 추출
        /// </summary>
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            if (child == null || config == null || child.Age != 0)
                return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (string.IsNullOrEmpty(spouseName) || !config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            return new ChildParts
            {
                SpouseName = spouseName,
                IsMale = child.Gender == 0,
                BabyHairStyles = spouseConfig.BabyHairStyles,
                BabyEyes = spouseConfig.BabyEyes,
                BabySkins = spouseConfig.BabySkins,
                BabyBodies = spouseConfig.BabyBodies
            };
        }

        /// <summary>
        /// 유아(1세 이상) 파츠 조합 추출 (배우자/성별/옵션 기반)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            if (child == null || config == null)
                return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (string.IsNullOrEmpty(spouseName) || !config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            bool isMale = child.Gender == 0;

            return new ChildParts
            {
                SpouseName = spouseName,
                IsMale = isMale,

                // 여자 자녀
                GirlHairStyles = spouseConfig.GirlHairStyles,
                GirlEyes = spouseConfig.GirlEyes,
                GirlSkins = spouseConfig.GirlSkins,
                GirlTopSpringOptions = spouseConfig.GirlTopSpringOptions,
                GirlTopSummerOptions = spouseConfig.GirlTopSummerOptions,
                GirlTopFallOptions = spouseConfig.GirlTopFallOptions,
                GirlTopWinterOptions = spouseConfig.GirlTopWinterOptions,
                SkirtColorOptions = spouseConfig.SkirtColorOptions,
                GirlShoesColorOptions = spouseConfig.GirlShoesColorOptions,
                GirlNeckCollarColorOptions = spouseConfig.GirlNeckCollarColorOptions,
                GirlPajamaTypeOptions = spouseConfig.GirlPajamaTypeOptions,
                GirlPajamaColorOptions = spouseConfig.GirlPajamaColorOptions,
                GirlFestivalSummerSkirtOptions = spouseConfig.GirlFestivalSummerSkirtOptions,
                GirlFestivalWinterSkirtOptions = spouseConfig.GirlFestivalWinterSkirtOptions,
                GirlFestivalFallSkirts = spouseConfig.GirlFestivalFallSkirts,

                // 남자 자녀
                BoyHairStyles = spouseConfig.BoyHairStyles,
                BoyEyes = spouseConfig.BoyEyes,
                BoySkins = spouseConfig.BoySkins,
                BoyTopSpringOptions = spouseConfig.BoyTopSpringOptions,
                BoyTopSummerOptions = spouseConfig.BoyTopSummerOptions,
                BoyTopFallOptions = spouseConfig.BoyTopFallOptions,
                BoyTopWinterOptions = spouseConfig.BoyTopWinterOptions,
                PantsColorOptions = spouseConfig.PantsColorOptions,
                BoyShoesColorOptions = spouseConfig.BoyShoesColorOptions,
                BoyNeckCollarColorOptions = spouseConfig.BoyNeckCollarColorOptions,
                BoyPajamaTypeOptions = spouseConfig.BoyPajamaTypeOptions,
                BoyPajamaColorOptions = spouseConfig.BoyPajamaColorOptions,
                BoyFestivalSummerPantsOptions = spouseConfig.BoyFestivalSummerPantsOptions,
                BoyFestivalWinterPantsOptions = spouseConfig.BoyFestivalWinterPantsOptions,
                BoyFestivalFallPants = spouseConfig.BoyFestivalFallPants,

                // 축제(공용)
                FestivalSpringHat = spouseConfig.FestivalSpringHat,
                FestivalSummerHat = spouseConfig.FestivalSummerHat,
                FestivalWinterHat = spouseConfig.FestivalWinterHat,
                FestivalWinterScarf = spouseConfig.FestivalWinterScarf
            };
        }
    }
}