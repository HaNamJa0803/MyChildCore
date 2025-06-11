using MyChildCore;
using System;
using System.Collections.Generic;

namespace MyChildCore
{
    // Observer 콜백
    public static class GMCMObserver
    {
        public static event Action<string, string, string> OnOptionChanged;

        public static void Notify(string spouse, string optionName, string newValue)
        {
            try { OnOptionChanged?.Invoke(spouse, optionName, newValue); }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[GMCMObserver] 알림 이벤트 예외: {ex.Message}");
            }
        }
    }

    public class GMCMOption
    {
        public string NameKey { get; set; }
        public string TooltipKey { get; set; }
        public bool IsBoy { get; set; }
        public Func<string, string> GetValue { get; set; }
        public Action<string, string> SetValue { get; set; }
        public Func<string, string[]> AllowedValues { get; set; }
    }

    public static class GMCMOptionsMenus
    {
        public static readonly List<GMCMOption> GMCMOptions = new()
        {
            // === 글로벌(공통) 활성화 스위치 ===
            new GMCMOption {
                NameKey = "option.enable_mod",
                TooltipKey = "tooltip.enable_mod",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnableMod.ToString(),
                SetValue = (s, v) => { DropdownConfig.EnableMod = v == "True"; GMCMObserver.Notify(s, "EnableMod", v); },
                AllowedValues = s => new[] { "True", "False" }
            },
            new GMCMOption {
                NameKey = "option.enable_pajama",
                TooltipKey = "tooltip.enable_pajama",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnablePajama.ToString(),
                SetValue = (s, v) => { DropdownConfig.EnablePajama = v == "True"; GMCMObserver.Notify(s, "EnablePajama", v); },
                AllowedValues = s => new[] { "True", "False" }
            },
            new GMCMOption {
                NameKey = "option.enable_festival",
                TooltipKey = "tooltip.enable_festival",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnableFestival.ToString(),
                SetValue = (s, v) => { DropdownConfig.EnableFestival = v == "True"; GMCMObserver.Notify(s, "EnableFestival", v); },
                AllowedValues = s => new[] { "True", "False" }
            },

            // ====== 아기(공용) ======
            new GMCMOption {
                NameKey = "option.baby.hair",
                TooltipKey = "tooltip.baby.hair",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyHairStyles,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyHairStyles = v; },
                AllowedValues = s => DropdownConfig.BabyHairStyles
            },
            new GMCMOption {
                NameKey = "option.baby.eye",
                TooltipKey = "tooltip.baby.eye",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyEyes,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyEyes = v; },
                AllowedValues = s => DropdownConfig.BabyEyes
            },
            new GMCMOption {
                NameKey = "option.baby.skin",
                TooltipKey = "tooltip.baby.skin",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabySkins,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabySkins = v; },
                AllowedValues = s => DropdownConfig.BabySkins
            },
            new GMCMOption {
                NameKey = "option.baby.body",
                TooltipKey = "tooltip.baby.body",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyBodies,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyBodies = v; },
                AllowedValues = s => DropdownConfig.BabyBodies
            },

            // ===== 유아 (남/여) =====
            // --- 남아
            new GMCMOption {
                NameKey = "option.boy.hair",
                TooltipKey = "tooltip.boy.hair",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyHairStyles,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyHairStyles = v; },
                AllowedValues = s => DropdownConfig.BoyHairStyles
            },
            new GMCMOption {
                NameKey = "option.boy.eye",
                TooltipKey = "tooltip.boy.eye",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyEyes,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyEyes = v; },
                AllowedValues = s => DropdownConfig.BoyEyes
            },
            new GMCMOption {
                NameKey = "option.boy.skin",
                TooltipKey = "tooltip.boy.skin",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoySkins,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoySkins = v; },
                AllowedValues = s => DropdownConfig.BoySkins
            },

            // --- 여아
            new GMCMOption {
                NameKey = "option.girl.hair",
                TooltipKey = "tooltip.girl.hair",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlHairStyles,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlHairStyles = v; },
                AllowedValues = s => DropdownConfig.GirlHairStyles
            },
            new GMCMOption {
                NameKey = "option.girl.eye",
                TooltipKey = "tooltip.girl.eye",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlEyes,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlEyes = v; },
                AllowedValues = s => DropdownConfig.GirlEyes
            },
            new GMCMOption {
                NameKey = "option.girl.skin",
                TooltipKey = "tooltip.girl.skin",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlSkins,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlSkins = v; },
                AllowedValues = s => DropdownConfig.GirlSkins
            },

            // ===== 의상/신발/넥칼라/잠옷/축제(공통) =====
            new GMCMOption {
                NameKey = "option.bottom.skirt",
                TooltipKey = "tooltip.bottom.skirt",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].SkirtColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].SkirtColorOptions = v; },
                AllowedValues = s => DropdownConfig.SkirtColorOptions
            },
            new GMCMOption {
                NameKey = "option.bottom.pants",
                TooltipKey = "tooltip.bottom.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].PantsColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].PantsColorOptions = v; },
                AllowedValues = s => DropdownConfig.PantsColorOptions
            },
            new GMCMOption {
                NameKey = "option.shoes",
                TooltipKey = "tooltip.shoes",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlShoesColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlShoesColorOptions = v; },
                AllowedValues = s => DropdownConfig.GirlShoesColorOptions
            },
            new GMCMOption {
                NameKey = "option.shoes.boy",
                TooltipKey = "tooltip.shoes.boy",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyShoesColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyShoesColorOptions = v; },
                AllowedValues = s => DropdownConfig.BoyShoesColorOptions
            },
            new GMCMOption {
                NameKey = "option.neckcollar",
                TooltipKey = "tooltip.neckcollar",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColorOptions = v; },
                AllowedValues = s => DropdownConfig.GirlNeckCollarColorOptions
            },
            new GMCMOption {
                NameKey = "option.neckcollar.boy",
                TooltipKey = "tooltip.neckcollar.boy",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColorOptions = v; },
                AllowedValues = s => DropdownConfig.BoyNeckCollarColorOptions
            },
            // ===== 잠옷(타입/색상, 남여 구분) =====
            new GMCMOption {
                NameKey = "option.pajama.type.girl",
                TooltipKey = "tooltip.pajama.type.girl",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions = v; },
                AllowedValues = s => DropdownConfig.GirlPajamaTypeOptions
            },
            new GMCMOption {
                NameKey = "option.pajama.color.girl",
                TooltipKey = "tooltip.pajama.color.girl",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlPajamaColorOptions = v; },
                AllowedValues = s => {
                    var type = ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions ?? DropdownConfig.GirlPajamaTypeOptions[0];
                    return DropdownConfig.GirlPajamaColorOptions[type];
                }
            },
            new GMCMOption {
                NameKey = "option.pajama.type.boy",
                TooltipKey = "tooltip.pajama.type.boy",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions = v; },
                AllowedValues = s => DropdownConfig.BoyPajamaTypeOptions
            },
            new GMCMOption {
                NameKey = "option.pajama.color.boy",
                TooltipKey = "tooltip.pajama.color.boy",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaColorOptions,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyPajamaColorOptions = v; },
                AllowedValues = s => {
                    var type = ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions ?? DropdownConfig.BoyPajamaTypeOptions[0];
                    return DropdownConfig.BoyPajamaColorOptions[type];
                }
            },

            // ===== 축제 =====
            new GMCMOption {
                NameKey = "option.festival.spring.hat",
                TooltipKey = "tooltip.festival.spring.hat",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalSpringHat,
                SetValue = (s, v) => { },
                AllowedValues = s => DropdownConfig.FestivalSpringHat
            },
            new GMCMOption {
                NameKey = "option.festival.summer.hat",
                TooltipKey = "tooltip.festival.summer.hat",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalSummerHat,
                SetValue = (s, v) => { },
                AllowedValues = s => DropdownConfig.FestivalSummerHat
            },
            new GMCMOption {
                NameKey = "option.festival.winter.hat",
                TooltipKey = "tooltip.festival.winter.hat",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalWinterHat,
                SetValue = (s, v) => { },
                AllowedValues = s => DropdownConfig.FestivalWinterHat
            },
            new GMCMOption {
                NameKey = "option.festival.winter.scarf",
                TooltipKey = "tooltip.festival.winter.scarf",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalWinterScarf,
                SetValue = (s, v) => { },
                AllowedValues = s => DropdownConfig.FestivalWinterScarf
            }
        };

        // Observer 실시간 연동 (예시)
        static GMCMOptionsMenus()
        {
            GMCMObserver.OnOptionChanged += (spouse, option, value) =>
            {
                var config = ModEntry.Config;
                if (config.SpouseConfigs.ContainsKey(spouse))
                {
                    ChildManager.ForceAllChildrenAppearance(config);
                    DataManager.SaveData(CacheManager.GetChildCache());
                    ResourceManager.InvalidateAllChildrenSprites();
                }
            };
        }
    }
}