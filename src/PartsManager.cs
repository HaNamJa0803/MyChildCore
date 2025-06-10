using MyChildCore;
using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (즉시감지/생성+실시간 알림)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 파츠 변경(추출, 복구 등)시 알림을 주는 Observer 이벤트 (child, parts)
        /// </summary>
        public static event Action<Child, ChildParts> OnPartsChanged;

        // === 아기(0세) 파츠 조합 추출 (조건분기/자동복구) ===
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            ChildParts result = null;
            try
            {
                if (child == null || config == null || child.Age != 0)
                {
                    result = GetDefaultParts(child, isBaby: true);
                }
                else
                {
                    string spouseName = AppearanceManager.GetRealSpouseName(child);
                    if (string.IsNullOrEmpty(spouseName)
                        || !config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig)
                        || spouseConfig == null)
                    {
                        result = GetDefaultParts(child, isBaby: true);
                    }
                    else
                    {
                        result = new ChildParts
                        {
                            SpouseName     = spouseName,
                            IsMale         = child.Gender == 0,
                            BabyHairStyles = spouseConfig.BabyHairStyles,
                            BabyEyes       = spouseConfig.BabyEyes,
                            BabySkins      = spouseConfig.BabySkins,
                            BabyBodies     = spouseConfig.BabyBodies
                        };
                        CustomLogger.Info($"[PartsManager][Baby] {spouseName} 파츠 생성 완료");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Baby] 파츠 추출 예외: {ex.Message}");
                result = GetDefaultParts(child, isBaby: true);
            }

            try { OnPartsChanged?.Invoke(child, result); } catch (Exception e) { CustomLogger.Warn($"[PartsManager] OnPartsChanged 예외: {e.Message}"); }
            return result;
        }

        // === 유아(1세) 파츠 조합 추출 (조건분기/자동복구) ===
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            ChildParts result = null;
            try
            {
                if (child == null || config == null || child.Age != 1)
                {
                    result = GetDefaultParts(child, isBaby: false);
                }
                else
                {
                    string spouseName = AppearanceManager.GetRealSpouseName(child);
                    if (string.IsNullOrEmpty(spouseName)
                        || !config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig)
                        || spouseConfig == null)
                    {
                        result = GetDefaultParts(child, isBaby: false);
                    }
                    else
                    {
                        bool isMale = child.Gender == 0;
                        result = new ChildParts
                        {
                            SpouseName     = spouseName,
                            IsMale         = isMale,
                            GirlHairStyles = spouseConfig.GirlHairStyles,
                            GirlEyes       = spouseConfig.GirlEyes,
                            GirlSkins      = spouseConfig.GirlSkins,
                            GirlTopSpringOptions = spouseConfig.GirlTopSpringOptions,
                            GirlTopSummerOptions = spouseConfig.GirlTopSummerOptions,
                            GirlTopFallOptions   = spouseConfig.GirlTopFallOptions,
                            GirlTopWinterOptions = spouseConfig.GirlTopWinterOptions,
                            SkirtColorOptions    = spouseConfig.SkirtColorOptions,
                            GirlShoesColorOptions = spouseConfig.GirlShoesColorOptions,
                            GirlNeckCollarColorOptions = spouseConfig.GirlNeckCollarColorOptions,
                            GirlPajamaTypeOptions = spouseConfig.GirlPajamaTypeOptions,
                            GirlPajamaColorOptions = spouseConfig.GirlPajamaColorOptions,
                            GirlFestivalSummerSkirtOptions = spouseConfig.GirlFestivalSummerSkirtOptions,
                            GirlFestivalWinterSkirtOptions = spouseConfig.GirlFestivalWinterSkirtOptions,
                            GirlFestivalFallSkirts = spouseConfig.GirlFestivalFallSkirts,
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
                            FestivalSpringHat = spouseConfig.FestivalSpringHat,
                            FestivalSummerHat = spouseConfig.FestivalSummerHat,
                            FestivalWinterHat = spouseConfig.FestivalWinterHat,
                            FestivalWinterScarf = spouseConfig.FestivalWinterScarf
                        };
                        CustomLogger.Info($"[PartsManager][Toddler] {spouseName} 파츠 생성 완료");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Toddler] 파츠 추출 예외: {ex.Message}");
                result = GetDefaultParts(child, isBaby: false);
            }

            try { OnPartsChanged?.Invoke(child, result); } catch (Exception e) { CustomLogger.Warn($"[PartsManager] OnPartsChanged 예외: {e.Message}"); }
            return result;
        }

        // === Default 파츠(누락/에러시 자동 복구) ===
        public static ChildParts GetDefaultParts(Child child, bool isBaby = false)
        {
            try
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
            catch (Exception ex)
            {
                CustomLogger.Error($"[PartsManager] GetDefaultParts 예외: {ex.Message}");
                return null;
            }
        }

        // === 필수 파츠 모두 유효성 체크 ===
        public static bool HasAllRequiredParts(ChildParts parts)
        {
            try
            {
                if (parts == null) return false;
                if (string.IsNullOrEmpty(parts.SpouseName)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}