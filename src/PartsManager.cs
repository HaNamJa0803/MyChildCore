using System;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class PartsManager
    {
        // 아기(0세) 파츠 조합
        public static ChildParts GetPartsForBaby(object childObj, ModConfig config)
        {
            if (childObj == null || config == null) return null;
            dynamic child = childObj;
            if (child.Age != 0) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            var parts = new ChildParts
            {
                SpouseName = spouseName,
                IsMale = child.Gender == 0, // 0: Boy, 1: Girl (Child.cs 참고!)

                BabyHairStyles = spouseConfig.BabyHairStyles,
                BabyEyes = spouseConfig.BabyEyes,
                BabySkins = spouseConfig.BabySkins,
                BabyBodies = spouseConfig.BabyBodies
            };

            return parts;
        }

        // 유아/아이(토들러) 파츠 조합
        public static ChildParts GetPartsForChild(object childObj, ModConfig config)
        {
            if (childObj == null || config == null) return null;
            dynamic child = childObj;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            bool isMale = child.Gender == 0; // 0: Boy, 1: Girl

            var parts = new ChildParts
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

            return parts;
        }
    }
}