using System;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (GMCM 옵션 완전 반영 + 커스텀 로깅)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 아기(0세) 파츠 조합 추출
        /// </summary>
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            if (child == null)
            {
                CustomLogger.Warn("[PartsManager][Baby] child is null!");
                return null;
            }
            if (config == null)
            {
                CustomLogger.Warn("[PartsManager][Baby] config is null!");
                return null;
            }
            if (child.Age != 0)
            {
                CustomLogger.Warn($"[PartsManager][Baby] Not a baby (Age={child.Age})");
                return null;
            }

            // *** 반드시 '진짜 배우자명'으로 ***
            string spouseName = AppearanceManager.GetRealSpouseName(child);
            if (string.IsNullOrEmpty(spouseName))
            {
                CustomLogger.Warn("[PartsManager][Baby] spouseName is null or empty!");
                return null;
            }
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
            {
                CustomLogger.Warn($"[PartsManager][Baby] SpouseConfig for '{spouseName}' not found!");
                return null;
            }

            var result = new ChildParts
            {
                SpouseName = spouseName,
                IsMale = child.Gender == 0,
                BabyHairStyles = spouseConfig.BabyHairStyles,
                BabyEyes = spouseConfig.BabyEyes,
                BabySkins = spouseConfig.BabySkins,
                BabyBodies = spouseConfig.BabyBodies
            };

            CustomLogger.Info($"[PartsManager][Baby] {spouseName}: Hair={result.BabyHairStyles}, Eyes={result.BabyEyes}, Skin={result.BabySkins}, Body={result.BabyBodies}");
            return result;
        }

        /// <summary>
        /// 유아(1세 이상) 파츠 조합 추출 (배우자/성별/옵션 기반)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            if (child == null)
            {
                CustomLogger.Warn("[PartsManager][Toddler] child is null!");
                return null;
            }
            if (config == null)
            {
                CustomLogger.Warn("[PartsManager][Toddler] config is null!");
                return null;
            }

            // *** 반드시 '진짜 배우자명'으로 ***
            string spouseName = AppearanceManager.GetRealSpouseName(child);
            if (string.IsNullOrEmpty(spouseName))
            {
                CustomLogger.Warn("[PartsManager][Toddler] spouseName is null or empty!");
                return null;
            }
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
            {
                CustomLogger.Warn($"[PartsManager][Toddler] SpouseConfig for '{spouseName}' not found!");
                return null;
            }

            bool isMale = child.Gender == 0;

            var result = new ChildParts
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

            CustomLogger.Info(
                $"[PartsManager][Toddler] {spouseName}: " +
                $"Male={isMale} " +
                $"Hair={result.BoyHairStyles}/{result.GirlHairStyles}, " +
                $"Top(Spring)={result.BoyTopSpringOptions}/{result.GirlTopSpringOptions}, " +
                $"Bottom(Pants/Skirt)={result.PantsColorOptions}/{result.SkirtColorOptions}, " +
                $"Shoes={result.BoyShoesColorOptions}/{result.GirlShoesColorOptions}, " +
                $"Festival={result.FestivalSpringHat},{result.FestivalSummerHat},{result.FestivalWinterHat},{result.FestivalWinterScarf}"
            );

            return result;
        }
    }
}