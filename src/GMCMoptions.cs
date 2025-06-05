using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore
{
    public class GMCMOption
    {
        public string NameKey { get; set; }
        public string TooltipKey { get; set; }
        public bool IsBoy { get; set; }
        public Func<string, string> GetValue { get; set; }
        public Action<string, string> SetValue { get; set; }
        public Func<string[]> AllowedValues { get; set; }
    }

    public static class GMCMOptionsMenus // 이름은 자유
    {
        public static readonly List<GMCMOption> GMCMOptions = new()
        {
            // ======= 글로벌(공통) 활성화 스위치 =======
            new GMCMOption
            {
                NameKey = "option.enable_mod",
                TooltipKey = "tooltip.enable_mod",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnableMod.ToString(),
                SetValue = (s, v) => DropdownConfig.EnableMod = v == "True",
                AllowedValues = () => new[] { "True", "False" }
            },
            new GMCMOption
            {
                NameKey = "option.enable_pajama",
                TooltipKey = "tooltip.enable_pajama",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnablePajama.ToString(),
                SetValue = (s, v) => DropdownConfig.EnablePajama = v == "True",
                AllowedValues = () => new[] { "True", "False" }
            },
            new GMCMOption
            {
                NameKey = "option.enable_festival",
                TooltipKey = "tooltip.enable_festival",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnableFestival.ToString(),
                SetValue = (s, v) => DropdownConfig.EnableFestival = v == "True",
                AllowedValues = () => new[] { "True", "False" }
            },
            // ====== 아기(공용) ======
            new GMCMOption
            {
                NameKey = "option.baby.hair.style",
                TooltipKey = "tooltip.baby.hair.style",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyHairStyle,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabyHairStyle = v,
                AllowedValues = () => DropdownConfig.BabyHairStyles
            },
            new GMCMOption
            {
                NameKey = "option.baby.eye",
                TooltipKey = "tooltip.baby.eye",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyEye,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabyEye = v,
                AllowedValues = () => DropdownConfig.BabyEyes
            },
            new GMCMOption
            {
                NameKey = "option.baby.skin",
                TooltipKey = "tooltip.baby.skin",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabySkin,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabySkin = v,
                AllowedValues = () => DropdownConfig.BabySkins
            },
            new GMCMOption
            {
                NameKey = "option.baby.body",
                TooltipKey = "tooltip.baby.body",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyBody,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabyBody = v,
                AllowedValues = () => DropdownConfig.BabyBodies
            },
            // === 여자 자녀 (Girl) ===
            new GMCMOption
            {
                NameKey = "option.girl.hair.style",
                TooltipKey = "tooltip.girl.hair.style",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlHairStyle,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlHairStyle = v,
                AllowedValues = () => DropdownConfig.GirlHairStyles
            },
            new GMCMOption
            {
                NameKey = "option.girl.eye",
                TooltipKey = "tooltip.girl.eye",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlEye,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlEye = v,
                AllowedValues = () => DropdownConfig.GirlEyes
            },
            new GMCMOption
            {
                NameKey = "option.girl.skin",
                TooltipKey = "tooltip.girl.skin",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlSkin,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlSkin = v,
                AllowedValues = () => DropdownConfig.GirlSkins
            },
            // 평상복 상의(계절별)
            new GMCMOption
            {
                NameKey = "option.girl.top.spring",
                TooltipKey = "tooltip.girl.top.spring",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSpring,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopSpring = v,
                AllowedValues = () => DropdownConfig.GirlTopSpringOptions
            },
            new GMCMOption
            {
                 NameKey = "option.girl.top.summer",
                 TooltipKey = "tooltip.girl.top.summer",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSummer,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopSummer = v,
                 AllowedValues = () => DropdownConfig.GirlTopSummerOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.top.fall",
                 TooltipKey = "tooltip.girl.top.fall",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopFall,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopFall = v,
                 AllowedValues = () => DropdownConfig.GirlTopFallOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.top.winter",
                 TooltipKey = "tooltip.girl.top.winter",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopWinter,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopWinter = v,
                 AllowedValues = () => DropdownConfig.GirlTopWinterOptions
             },
             // 하의, 신발, 넥칼라
             new GMCMOption
             {
                 NameKey = "option.girl.skirt",
                 TooltipKey = "tooltip.girl.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlSkirtColor,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlSkirtColor = v,
                 AllowedValues = () => DropdownConfig.SkirtColors
             },
             new GMCMOption
             {
                 NameKey = "option.girl.shoes",
                 TooltipKey = "tooltip.girl.shoes",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlShoesColor,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlShoesColor = v,
                 AllowedValues = () => DropdownConfig.ShoesColors
             },
             new GMCMOption
             {
                 NameKey = "option.girl.neckcollar",
                 TooltipKey = "tooltip.girl.neckcollar",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColor,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColor = v,
                 AllowedValues = () => DropdownConfig.NeckCollarColors
             },
             // 잠옷(타입/색상)
             new GMCMOption
             {
                 NameKey = "option.girl.pajama.type",
                 TooltipKey = "tooltip.girl.pajama.type",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaType,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlPajamaType = v,
                 AllowedValues = () => DropdownConfig.PajamaTypeOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.pajama.color",
                 TooltipKey = "tooltip.girl.pajama.color",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaColor,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlPajamaColor = v,
                 AllowedValues = () =>
             {
                 var type = ModEntry.Config.SpouseConfigs[s].GirlPajamaType ?? DropdownConfig.PajamaTypeOptions[0];
                 return DropdownConfig.PajamaColors[type];
             }
             },
             // 축제복
             new GMCMOption
             {
                 NameKey = "option.girl.festival.summer.skirt",
                 TooltipKey = "tooltip.girl.festival.summer.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirt,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirt = v,
                 AllowedValues = () => DropdownConfig.GirlFestivalSummerSkirtOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.festival.fall.skirt",
                 TooltipKey = "tooltip.girl.festival.fall.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirt,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirt = v,
                 AllowedValues = () => DropdownConfig.GirlFestivalFallSkirts
             },
             new GMCMOption
             {
                 NameKey = "option.girl.festival.winter.skirt",
                 TooltipKey = "tooltip.girl.festival.winter.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirt,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirt = v,
                 AllowedValues = () => DropdownConfig.GirlFestivalWinterSkirtOptions
            },
            // ====== 남자 자녀 (Boy) ======
            new GMCMOption
            {
                NameKey = "option.boy.hair.style",
                TooltipKey = "tooltip.boy.hair.style",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyHairStyle,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyHairStyle = v,
                AllowedValues = () => DropdownConfig.BoyHairStyles
            },
            new GMCMOption
            {
                NameKey = "option.boy.eye",
                TooltipKey = "tooltip.boy.eye",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyEye,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyEye = v,
                AllowedValues = () => DropdownConfig.BoyEyes
            },
            new GMCMOption
            {   
                NameKey = "option.boy.skin",
                TooltipKey = "tooltip.boy.skin",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoySkin,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoySkin = v,
                AllowedValues = () => DropdownConfig.BoySkins
            },
            // 평상복 상의(계절별)
            new GMCMOption {
                NameKey = "option.boy.top.spring",
                TooltipKey = "tooltip.boy.top.spring",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSpring,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopSpring = v,
                AllowedValues = () => DropdownConfig.BoyTopSpringOptions
            },
            new GMCMOption {
                NameKey = "option.boy.top.summer",
                TooltipKey = "tooltip.boy.top.summer",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSummer,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopSummer = v,
                AllowedValues = () => DropdownConfig.BoyTopSummerOptions
            },
            new GMCMOption {
                NameKey = "option.boy.top.fall",
                TooltipKey = "tooltip.boy.top.fall",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopFall,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopFall = v,
                AllowedValues = () => DropdownConfig.BoyTopFallOptions
            },
            new GMCMOption {
                NameKey = "option.boy.top.winter",
                TooltipKey = "tooltip.boy.top.winter",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopWinter,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopWinter = v,
                AllowedValues = () => DropdownConfig.BoyTopWinterOptions
            },
            // 하의, 신발, 넥칼라
            new GMCMOption {
                NameKey = "option.boy.pants",
                TooltipKey = "tooltip.boy.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPantsColor,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyPantsColor = v,
                AllowedValues = () => DropdownConfig.PantsColors
            },
            new GMCMOption {
                NameKey = "option.boy.shoes",
                TooltipKey = "tooltip.boy.shoes",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyShoesColor,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyShoesColor = v,
                AllowedValues = () => DropdownConfig.ShoesColors
            },
            new GMCMOption {
                NameKey = "option.boy.neckcollar",
                TooltipKey = "tooltip.boy.neckcollar",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColor,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColor = v,
                AllowedValues = () => DropdownConfig.NeckCollarColors
            },
            // 잠옷(타입/색상)
            new GMCMOption {
                NameKey = "option.boy.pajama.type",
                TooltipKey = "tooltip.boy.pajama.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaType,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyPajamaType = v,
                AllowedValues = () => DropdownConfig.PajamaTypeOptions
            },
            new GMCMOption {
                NameKey = "option.boy.pajama.color",
                TooltipKey = "tooltip.boy.pajama.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaColor,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyPajamaColor = v,
                AllowedValues = () =>
            {
                var type = ModEntry.Config.SpouseConfigs[s].BoyPajamaType ?? DropdownConfig.PajamaTypeOptions[0];
                return DropdownConfig.PajamaColorOptions[type];
            }
            },
            // 축제 바지
            new GMCMOption {
            NameKey = "option.boy.festival.summer.pants",
            TooltipKey = "tooltip.boy.festival.summer.pants",
            IsBoy = true,
            GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPants,
            SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPants = v,
            AllowedValues = () => DropdownConfig.BoyFestivalSummerPantsOptions
            },
            new GMCMOption {
                NameKey = "option.boy.festival.fall.pants",
                TooltipKey = "tooltip.boy.festival.fall.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPants,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPants = v,
                AllowedValues = () => DropdownConfig.BoyFestivalFallPants
            },
            new GMCMOption {
                NameKey = "option.boy.festival.winter.pants",
                TooltipKey = "tooltip.boy.festival.winter.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPants,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPants = v,
                AllowedValues = () => DropdownConfig.BoyFestivalWinterPantsOptions
            },
            // ====== 축제 공용(읽기 전용, 모든 자녀) ======
            new GMCMOption {
                NameKey = "option.festival.spring.hat",
                TooltipKey = "tooltip.festival.spring.hat",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalSpringHat,
                SetValue = (s, v) => { },
                AllowedValues = () => DropdownConfig.FestivalSpringHat
            },
            new GMCMOption {
                NameKey = "option.festival.summer.hat",
                TooltipKey = "tooltip.festival.summer.hat",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalSummerHat,
                SetValue = (s, v) => { },
                AllowedValues = () => DropdownConfig.FestivalSummerHat
            },
            new GMCMOption {
                NameKey = "option.festival.winter.hat",
                TooltipKey = "tooltip.festival.winter.hat",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalWinterHat,
                SetValue = (s, v) => { },
                AllowedValues = () => DropdownConfig.FestivalWinterHat
            },
            new GMCMOption {
                NameKey = "option.festival.winter.scarf",
                TooltipKey = "tooltip.festival.winter.scarf",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].FestivalWinterScarf,
                SetValue = (s, v) => { },
                AllowedValues = () => DropdownConfig.FestivalWinterScarf
            }
                    };
                }
            }