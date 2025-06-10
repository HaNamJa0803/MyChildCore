using MyChildCore;
using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (유니크 칠드런식, GMCM/조건분기/자동복구/실시간동기화)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 파츠 변경(추출, 복구 등)시 알림을 주는 Observer 이벤트 (child, parts)
        /// </summary>
        public static event Action<Child, ChildParts> OnPartsChanged;

        /// <summary>
        /// 아기(0세) 파츠 조합 추출 (조건분기/자동복구)
        /// </summary>
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            ChildParts result;
            if (child == null || config == null || child.Age != 0)
            {
                result = GetDefaultParts(child, isBaby: true);
                NotifyPartsChanged(child, result);
                return result;
            }

            string spouseName = AppearanceManager.GetRealSpouseName(child);
            if (string.IsNullOrEmpty(spouseName)
                || !config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig)
                || spouseConfig == null)
            {
                result = GetDefaultParts(child, isBaby: true);
                NotifyPartsChanged(child, result);
                return result;
            }

            result = new ChildParts
            {
                SpouseName    = spouseName,
                IsMale        = child.Gender == 0,
                BabyHairStyles= spouseConfig.BabyHairStyles,
                BabyEyes      = spouseConfig.BabyEyes,
                BabySkins     = spouseConfig.BabySkins,
                BabyBodies    = spouseConfig.BabyBodies
            };

            CustomLogger.Info($"[PartsManager][Baby] {spouseName} Hair={result.BabyHairStyles}, Eyes={result.BabyEyes}, Skin={result.BabySkins}, Body={result.BabyBodies}");
            NotifyPartsChanged(child, result);
            return result;
        }

        /// <summary>
        /// 유아(1세) 파츠 조합 추출 (조건분기/자동복구)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            ChildParts result;
            if (child == null || config == null || child.Age != 1)
            {
                result = GetDefaultParts(child, isBaby: false);
                NotifyPartsChanged(child, result);
                return result;
            }

            string spouseName = AppearanceManager.GetRealSpouseName(child);
            if (string.IsNullOrEmpty(spouseName)
                || !config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig)
                || spouseConfig == null)
            {
                result = GetDefaultParts(child, isBaby: false);
                NotifyPartsChanged(child, result);
                return result;
            }

            bool isMale = child.Gender == 0;
            result = new ChildParts
            {
                SpouseName    = spouseName,
                IsMale        = isMale,

                // 여자 자녀
                GirlHairStyles                = spouseConfig.GirlHairStyles,
                GirlEyes                      = spouseConfig.GirlEyes,
                GirlSkins                     = spouseConfig.GirlSkins,
                GirlTopSpringOptions          = spouseConfig.GirlTopSpringOptions,
                GirlTopSummerOptions          = spouseConfig.GirlTopSummerOptions,
                GirlTopFallOptions            = spouseConfig.GirlTopFallOptions,
                GirlTopWinterOptions          = spouseConfig.GirlTopWinterOptions,
                SkirtColorOptions             = spouseConfig.SkirtColorOptions,
                GirlShoesColorOptions         = spouseConfig.GirlShoesColorOptions,
                GirlNeckCollarColorOptions    = spouseConfig.GirlNeckCollarColorOptions,
                GirlPajamaTypeOptions         = spouseConfig.GirlPajamaTypeOptions,
                GirlPajamaColorOptions        = spouseConfig.GirlPajamaColorOptions,
                GirlFestivalSummerSkirtOptions= spouseConfig.GirlFestivalSummerSkirtOptions,
                GirlFestivalWinterSkirtOptions= spouseConfig.GirlFestivalWinterSkirtOptions,
                GirlFestivalFallSkirts        = spouseConfig.GirlFestivalFallSkirts,

                // 남자 자녀
                BoyHairStyles                 = spouseConfig.BoyHairStyles,
                BoyEyes                      = spouseConfig.BoyEyes,
                BoySkins                     = spouseConfig.BoySkins,
                BoyTopSpringOptions          = spouseConfig.BoyTopSpringOptions,
                BoyTopSummerOptions          = spouseConfig.BoyTopSummerOptions,
                BoyTopFallOptions            = spouseConfig.BoyTopFallOptions,
                BoyTopWinterOptions          = spouseConfig.BoyTopWinterOptions,
                PantsColorOptions            = spouseConfig.PantsColorOptions,
                BoyShoesColorOptions         = spouseConfig.BoyShoesColorOptions,
                BoyNeckCollarColorOptions    = spouseConfig.BoyNeckCollarColorOptions,
                BoyPajamaTypeOptions         = spouseConfig.BoyPajamaTypeOptions,
                BoyPajamaColorOptions        = spouseConfig.BoyPajamaColorOptions,
                BoyFestivalSummerPantsOptions= spouseConfig.BoyFestivalSummerPantsOptions,
                BoyFestivalWinterPantsOptions= spouseConfig.BoyFestivalWinterPantsOptions,
                BoyFestivalFallPants         = spouseConfig.BoyFestivalFallPants,

                // 축제(공용)
                FestivalSpringHat            = spouseConfig.FestivalSpringHat,
                FestivalSummerHat            = spouseConfig.FestivalSummerHat,
                FestivalWinterHat            = spouseConfig.FestivalWinterHat,
                FestivalWinterScarf          = spouseConfig.FestivalWinterScarf
            };

            CustomLogger.Info(
                $"[PartsManager][Toddler] {spouseName} Male={isMale} " +
                $"Hair={result.BoyHairStyles}/{result.GirlHairStyles}, " +
                $"Top(Spring)={result.BoyTopSpringOptions}/{result.GirlTopSpringOptions}, " +
                $"Bottom(Pants/Skirt)={result.PantsColorOptions}/{result.SkirtColorOptions}, " +
                $"Shoes={result.BoyShoesColorOptions}/{result.GirlShoesColorOptions}, " +
                $"Festival={result.FestivalSpringHat},{result.FestivalSummerHat},{result.FestivalWinterHat},{result.FestivalWinterScarf}"
            );
            NotifyPartsChanged(child, result);
            return result;
        }

        /// <summary>
        /// Default 파츠(누락/에러시 안전하게 자동 복구, 파츠명 고정)
        /// </summary>
        public static ChildParts GetDefaultParts(Child child, bool isBaby = false)
        {
            var def = new ChildParts
            {
                SpouseName = "Default",
                IsMale = child?.Gender == 0
            };

            if (isBaby)
            {
                def.BabyHairStyles = "DefaultHair";
                def.BabyEyes = "DefaultEyes";
                def.BabySkins = "DefaultSkin";
                def.BabyBodies = "DefaultBody";
            }
            else
            {
                def.GirlHairStyles = "DefaultGirlHair";
                def.GirlEyes = "DefaultGirlEyes";
                def.GirlSkins = "DefaultGirlSkin";
                def.GirlTopSpringOptions = "DefaultGirlSpringTop";
                def.GirlTopSummerOptions = "DefaultGirlSummerTop";
                def.GirlTopFallOptions = "DefaultGirlFallTop";
                def.GirlTopWinterOptions = "DefaultGirlWinterTop";
                def.SkirtColorOptions = "DefaultSkirt";
                def.GirlShoesColorOptions = "DefaultGirlShoes";
                def.GirlNeckCollarColorOptions = "DefaultGirlNeck";
                def.GirlPajamaTypeOptions = "DefaultGirlPajamaType";
                def.GirlPajamaColorOptions = "DefaultGirlPajamaColor";
                def.GirlFestivalSummerSkirtOptions = "DefaultGirlFestivalSummerSkirt";
                def.GirlFestivalWinterSkirtOptions = "DefaultGirlFestivalWinterSkirt";
                def.GirlFestivalFallSkirts = "DefaultGirlFestivalFallSkirt";

                def.BoyHairStyles = "DefaultBoyHair";
                def.BoyEyes = "DefaultBoyEyes";
                def.BoySkins = "DefaultBoySkin";
                def.BoyTopSpringOptions = "DefaultBoySpringTop";
                def.BoyTopSummerOptions = "DefaultBoySummerTop";
                def.BoyTopFallOptions = "DefaultBoyFallTop";
                def.BoyTopWinterOptions = "DefaultBoyWinterTop";
                def.PantsColorOptions = "DefaultPants";
                def.BoyShoesColorOptions = "DefaultBoyShoes";
                def.BoyNeckCollarColorOptions = "DefaultBoyNeck";
                def.BoyPajamaTypeOptions = "DefaultBoyPajamaType";
                def.BoyPajamaColorOptions = "DefaultBoyPajamaColor";
                def.BoyFestivalSummerPantsOptions = "DefaultBoyFestivalSummerPants";
                def.BoyFestivalWinterPantsOptions = "DefaultBoyFestivalWinterPants";
                def.BoyFestivalFallPants = "DefaultBoyFestivalFallPants";

                def.FestivalSpringHat = "DefaultSpringHat";
                def.FestivalSummerHat = "DefaultSummerHat";
                def.FestivalWinterHat = "DefaultWinterHat";
                def.FestivalWinterScarf = "DefaultWinterScarf";
            }

            CustomLogger.Warn("[PartsManager] DefaultParts 반환 (누락/에러시 복구)");
            return def;
        }

        /// <summary>
        /// 필수 파츠 모두 유효성 체크 - true면 정상
        /// </summary>
        public static bool HasAllRequiredParts(ChildParts parts)
        {
            if (parts == null) return false;
            if (string.IsNullOrEmpty(parts.SpouseName)) return false;
            // 실제 적용에 필수인 파츠명을 추가 체크할 수 있음
            return true;
        }

        /// <summary>
        /// 파츠 변경 이벤트를 호출
        /// </summary>
        private static void NotifyPartsChanged(Child child, ChildParts parts)
        {
            try
            {
                OnPartsChanged?.Invoke(child, parts);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager] OnPartsChanged 알림 예외: {ex.Message}");
            }
        }
    }
}