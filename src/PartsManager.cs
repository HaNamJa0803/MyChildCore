using MyChildCore;
using System;
using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (SMAPI Helper 기반, 1:1 파츠명 기반)
    /// </summary>
    public static class PartsManager
    {
        public static event Action<Child, ChildParts> OnPartsChanged;

        // Helper 인젝션 (초기화)
        public static IModHelper Helper { get; set; }

        // === 아기(0세) 파츠 조합 ===
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            ChildParts result = null;
            try
            {
                if (child == null || config == null || child.Age != 0)
                {
                    result = GetDefaultParts(child, true);
                }
                else
                {
                    string spouse = AppearanceManager.GetRealSpouseName(child);
                    if (string.IsNullOrEmpty(spouse)
                        || !config.SpouseConfigs.TryGetValue(spouse, out var spouseConfig)
                        || spouseConfig == null)
                    {
                        result = GetDefaultParts(child, true);
                    }
                    else
                    {
                        result = new ChildParts
                        {
                            SpouseName = spouse,
                            BabyHair = spouseConfig.BabyHair,
                            BabyEye = spouseConfig.BabyEye,
                            BabySkin = spouseConfig.BabySkin,
                            BabyBody = spouseConfig.BabyBody
                        };
                        CustomLogger.Info($"[PartsManager][Baby] {spouse} 파츠 생성 완료");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Baby] 파츠 추출 예외: {ex.Message}");
                result = GetDefaultParts(child, true);
            }

            try { OnPartsChanged?.Invoke(child, result); } catch { }
            return result;
        }

        // === 유아(1세) 파츠 조합 ===
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            ChildParts result = null;
            try
            {
                if (child == null || config == null || child.Age != 1)
                {
                    result = GetDefaultParts(child, false);
                }
                else
                {
                    string spouse = AppearanceManager.GetRealSpouseName(child);
                    if (string.IsNullOrEmpty(spouse)
                        || !config.SpouseConfigs.TryGetValue(spouse, out var spouseConfig)
                        || spouseConfig == null)
                    {
                        result = GetDefaultParts(child, false);
                    }
                    else
                    {
                        result = new ChildParts
                        {
                            SpouseName = spouse,
                            ToddlerHair = spouseConfig.ToddlerHair,
                            ToddlerEye = spouseConfig.ToddlerEye,
                            ToddlerSkin = spouseConfig.ToddlerSkin,
                            Top = spouseConfig.Top,
                            Bottom = spouseConfig.Bottom,
                            Shoes = spouseConfig.Shoes,
                            NeckCollar = spouseConfig.NeckCollar,
                            PajamaType = spouseConfig.PajamaType,
                            PajamaColor = spouseConfig.PajamaColor,
                            FestivalHat = spouseConfig.FestivalHat,
                            FestivalScarf = spouseConfig.FestivalScarf
                        };
                        CustomLogger.Info($"[PartsManager][Toddler] {spouse} 파츠 생성 완료");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Toddler] 파츠 추출 예외: {ex.Message}");
                result = GetDefaultParts(child, false);
            }

            try { OnPartsChanged?.Invoke(child, result); } catch { }
            return result;
        }

        // === Default 파츠 (누락/에러시 자동 복구) ===
        public static ChildParts GetDefaultParts(Child child, bool isBaby = false)
        {
            var def = new ChildParts
            {
                SpouseName = "Default"
            };

            if (isBaby)
            {
                def.BabyHair = "BabyHair";
                def.BabyEye = "BabyEye";
                def.BabySkin = "BabySkin";
                def.BabyBody = "BabyBody";
            }
            else
            {
                def.ToddlerHair = "ShortCut";
                def.ToddlerEye = "Eye";
                def.ToddlerSkin = "Skin";
                def.Top = "Top_Short";
                def.Bottom = "Black";
                def.Shoes = "Black";
                def.NeckCollar = "Black";
                def.PajamaType = "Frog";
                def.PajamaColor = "Blue";
                def.FestivalHat = "Hat";
                def.FestivalScarf = "Scarf";
            }
            return def;
        }

        // (SMAPI Helper로 동적 로딩)
        public static Microsoft.Xna.Framework.Graphics.Texture2D TryLoadTexture(string assetPath)
        {
            try
            {
                if (Helper == null || string.IsNullOrWhiteSpace(assetPath))
                    return null;
                return Helper.ModContent.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(assetPath);
            }
            catch
            {
                return null;
            }
        }
    }
}