using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (GMCM 옵션 완전 반영)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 아기(0세) 파츠 조합
        /// </summary>
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            if (child == null || config == null || child.Age != 0) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            var parts = new ChildParts
            {
                SpouseName = spouseName,
                IsMale = child.Gender == StardewValley.Child.Gender_Boy,

                BabyHairStyles = spouseConfig.BabyHairStyles,
                BabyEyes = spouseConfig.BabyEyes,
                BabySkins = spouseConfig.BabySkins,
                BabyBodies = spouseConfig.BabyBodies,

            };

            return parts;
        }

        /// <summary>
        /// 유아/아이(토들러) 파츠 조합 (배우자별/성별별, 옵션 기반)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            if (child == null || config == null) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            bool isMale = child.Gender == StardewValley.Child.Gender_Boy;

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
                GirlSkirtColorOptions = spouseConfig.SkirtColorOptions,
                GirlShoesColorOptions = spouseConfig.ShoesColorOptions,
                GirlNeckCollarColorOptions = spouseConfig.NeckCollarColorOptions,
                GirlPajamaTypeOptions = spouseConfig.PajamaTypeOptions,
                GirlPajamaColorOptions = spouseConfig.PajamaColorOptions,
                GirlFestivalSummerSkirtOptions = spouseConfig.FestivalSummerSkirtOptions,
                GirlFestivalWinterSkirtOptions = spouseConfig.FestivalWinterSkirtOptions,
                GirlFestivalFallSkirts = spouseConfig.FestivalFallSkirts,

                // 남자 자녀
                BoyHairStyles = spouseConfig.BoyHairStyles,
                BoyEyes = spouseConfig.BoyEyes,
                BoySkins = spouseConfig.BoySkins,
                BoyTopSpringOptions = spouseConfig.BoyTopSpringOptions,
                BoyTopSummerOptions = spouseConfig.BoyTopSummerOptions,
                BoyTopFallOptions = spouseConfig.BoyTopFallOptions,
                BoyTopWinterOptions = spouseConfig.BoyTopWinterOptions,
                BoyPantsColorOptions = spouseConfig.PantsColorOptions,
                BoyShoesColorOptions = spouseConfig.ShoesColorOptions,
                BoyNeckCollarColorOptions = spouseConfig.NeckCollarColorOptions,
                BoyPajamaTypeOptions = spouseConfig.PajamaTypeOptions,
                BoyPajamaColorOptions = spouseConfig.PajamaColorOptions,
                BoyFestivalSummerPantsOptions = spouseConfig.FestivalSummerPantsOptions,
                BoyFestivalWinterPantsOptions = spouseConfig.FestivalWinterPantsOptions,
                BoyFestivalFallPants = spouseConfig.FestivalFallPants,

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