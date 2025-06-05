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
        public Func<string, string[]> AllowedValues { get; set; } // ← 여기!
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
                AllowedValues = s => new[] { "True", "False" }
            },
            new GMCMOption
            {
                NameKey = "option.enable_pajama",
                TooltipKey = "tooltip.enable_pajama",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnablePajama.ToString(),
                SetValue = (s, v) => DropdownConfig.EnablePajama = v == "True",
                AllowedValues = s => new[] { "True", "False" }
            },
            new GMCMOption
            {
                NameKey = "option.enable_festival",
                TooltipKey = "tooltip.enable_festival",
                IsBoy = false,
                GetValue = s => DropdownConfig.EnableFestival.ToString(),
                SetValue = (s, v) => DropdownConfig.EnableFestival = v == "True",
                AllowedValues = s => new[] { "True", "False" }
            },
            // ====== 아기(공용) ======
            new GMCMOption
            {
                NameKey = "option.baby.hair.style",
                TooltipKey = "tooltip.baby.hair.style",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyHairStyles,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabyHairStyles = v,
                AllowedValues = s => DropdownConfig.BabyHairStyles
            },
            new GMCMOption
            {
                NameKey = "option.baby.eye",
                TooltipKey = "tooltip.baby.eye",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyEyes,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabyEyes = v,
                AllowedValues = s => DropdownConfig.BabyEyes
            },
            new GMCMOption
            {
                NameKey = "option.baby.skin",
                TooltipKey = "tooltip.baby.skin",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabySkins,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabySkins = v,
                AllowedValues = s => DropdownConfig.BabySkins
            },
            new GMCMOption
            {
                NameKey = "option.baby.body",
                TooltipKey = "tooltip.baby.body",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyBodies,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BabyBodies = v,
                AllowedValues = s => DropdownConfig.BabyBodies
            },
            // === 여자 자녀 (Girl) ===
            new GMCMOption
            {
                NameKey = "option.girl.hair.style",
                TooltipKey = "tooltip.girl.hair.style",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlHairStyles,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlHairStyles = v,
                AllowedValues = s => DropdownConfig.GirlHairStyles
            },
            new GMCMOption
            {
                NameKey = "option.girl.eye",
                TooltipKey = "tooltip.girl.eye",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlEyes,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlEyes = v,
                AllowedValues = s => DropdownConfig.GirlEyes
            },
            new GMCMOption
            {
                NameKey = "option.girl.skin",
                TooltipKey = "tooltip.girl.skin",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlSkins,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlSkins = v,
                AllowedValues = s => DropdownConfig.GirlSkins
            },
            // 평상복 상의(계절별)
            new GMCMOption
            {
                NameKey = "option.girl.top.spring",
                TooltipKey = "tooltip.girl.top.spring",
                IsBoy = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSpringOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopSpringOptions = v,
                AllowedValues = s => DropdownConfig.GirlTopSpringOptions
            },
            new GMCMOption
            {
                 NameKey = "option.girl.top.summer",
                 TooltipKey = "tooltip.girl.top.summer",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSummerOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopSummerOptions = v,
                 AllowedValues = s => DropdownConfig.GirlTopSummerOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.top.fall",
                 TooltipKey = "tooltip.girl.top.fall",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopFallOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopFallOptions = v,
                 AllowedValues = s => DropdownConfig.GirlTopFallOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.top.winter",
                 TooltipKey = "tooltip.girl.top.winter",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopWinterOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlTopWinterOptions = v,
                 AllowedValues = s => DropdownConfig.GirlTopWinterOptions
             },
             // 하의, 신발, 넥칼라
             new GMCMOption
             {
                 NameKey = "option.girl.skirt",
                 TooltipKey = "tooltip.girl.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].SkirtColorOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].SkirtColorOptions = v,
                 AllowedValues = s => DropdownConfig.SkirtColorOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.shoes",
                 TooltipKey = "tooltip.girl.shoes",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlShoesColorOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlShoesColorOptions = v,
                 AllowedValues = s => DropdownConfig.GirlShoesColorOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.neckcollar",
                 TooltipKey = "tooltip.girl.neckcollar",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColorOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColorOptions = v,
                 AllowedValues = s => DropdownConfig.GirlNeckCollarColorOptions
             },
             // 잠옷(타입/색상)
             new GMCMOption
             {
                 NameKey = "option.girl.pajama.type",
                 TooltipKey = "tooltip.girl.pajama.type",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions = v,
                 AllowedValues = s => DropdownConfig.GirlPajamaTypeOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.pajama.color",
                 TooltipKey = "tooltip.girl.pajama.color",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaColorOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlPajamaColorOptions = v,
                 AllowedValues = s =>
             {
                 var type = ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions ?? DropdownConfig.GirlPajamaTypeOptions[0];
                 return DropdownConfig.GirlPajamaColorOptions[type];
             }
             },
             // 축제복
             new GMCMOption
             {
                 NameKey = "option.girl.festival.summer.skirt",
                 TooltipKey = "tooltip.girl.festival.summer.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtOptions = v,
                 AllowedValues = s => DropdownConfig.GirlFestivalSummerSkirtOptions
             },
             new GMCMOption
             {
                 NameKey = "option.girl.festival.fall.skirt",
                 TooltipKey = "tooltip.girl.festival.fall.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirts,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirts = v,
                 AllowedValues = s => DropdownConfig.GirlFestivalFallSkirts
             },
             new GMCMOption
             {
                 NameKey = "option.girl.festival.winter.skirt",
                 TooltipKey = "tooltip.girl.festival.winter.skirt",
                 IsBoy = false,
                 GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtOptions,
                 SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtOptions = v,
                 AllowedValues = s => DropdownConfig.GirlFestivalWinterSkirtOptions
            },
            // ====== 남자 자녀 (Boy) ======
            new GMCMOption
            {
                NameKey = "option.boy.hair.style",
                TooltipKey = "tooltip.boy.hair.style",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyHairStyles,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyHairStyles = v,
                AllowedValues = s => DropdownConfig.BoyHairStyles
            },
            new GMCMOption
            {
                NameKey = "option.boy.eye",
                TooltipKey = "tooltip.boy.eye",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyEyes,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyEyes = v,
                AllowedValues = s => DropdownConfig.BoyEyes
            },
            new GMCMOption
            {   
                NameKey = "option.boy.skin",
                TooltipKey = "tooltip.boy.skin",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoySkins,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoySkins = v,
                AllowedValues = s => DropdownConfig.BoySkins
            },
            // 평상복 상의(계절별)
            new GMCMOption {
                NameKey = "option.boy.top.spring",
                TooltipKey = "tooltip.boy.top.spring",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSpringOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopSpringOptions = v,
                AllowedValues = s => DropdownConfig.BoyTopSpringOptions
            },
            new GMCMOption {
                NameKey = "option.boy.top.summer",
                TooltipKey = "tooltip.boy.top.summer",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSummerOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopSummerOptions = v,
                AllowedValues = s => DropdownConfig.BoyTopSummerOptions
            },
            new GMCMOption {
                NameKey = "option.boy.top.fall",
                TooltipKey = "tooltip.boy.top.fall",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopFallOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopFallOptions = v,
                AllowedValues = s => DropdownConfig.BoyTopFallOptions
            },
            new GMCMOption {
                NameKey = "option.boy.top.winter",
                TooltipKey = "tooltip.boy.top.winter",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopWinterOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyTopWinterOptions = v,
                AllowedValues = s => DropdownConfig.BoyTopWinterOptions
            },
            // 하의, 신발, 넥칼라
            new GMCMOption {
                NameKey = "option.boy.pants",
                TooltipKey = "tooltip.boy.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].PantsColorOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].PantsColorOptions = v,
                AllowedValues = s => DropdownConfig.PantsColorOptions
            },
            new GMCMOption {
                NameKey = "option.boy.shoes",
                TooltipKey = "tooltip.boy.shoes",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyShoesColorOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyShoesColorOptions = v,
                AllowedValues = s => DropdownConfig.BoyShoesColorOptions
            },
            new GMCMOption {
                NameKey = "option.boy.neckcollar",
                TooltipKey = "tooltip.boy.neckcollar",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColorOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColorOptions = v,
                AllowedValues = s => DropdownConfig.BoyNeckCollarColorOptions
            },
            // 잠옷(타입/색상)
            new GMCMOption {
                NameKey = "option.boy.pajama.type",
                TooltipKey = "tooltip.boy.pajama.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions = v,
                AllowedValues = s => DropdownConfig.BoyPajamaTypeOptions
            },
            new GMCMOption {
                NameKey = "option.boy.pajama.color",
                TooltipKey = "tooltip.boy.pajama.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaColorOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyPajamaColorOptions = v,
                AllowedValues = s =>
            {
                var type = ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions ?? DropdownConfig.BoyPajamaTypeOptions[0];
                return DropdownConfig.BoyPajamaColorOptions[type];
            }
            },
            // 축제 바지
            new GMCMOption {
            NameKey = "option.boy.festival.summer.pants",
            TooltipKey = "tooltip.boy.festival.summer.pants",
            IsBoy = true,
            GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsOptions,
            SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsOptions = v,
            AllowedValues = s => DropdownConfig.BoyFestivalSummerPantsOptions
            },
            new GMCMOption {
                NameKey = "option.boy.festival.fall.pants",
                TooltipKey = "tooltip.boy.festival.fall.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPants,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPants = v,
                AllowedValues = s => DropdownConfig.BoyFestivalFallPants
            },
            new GMCMOption {
                NameKey = "option.boy.festival.winter.pants",
                TooltipKey = "tooltip.boy.festival.winter.pants",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsOptions,
                SetValue = (s, v) => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsOptions = v,
                AllowedValues = s => DropdownConfig.BoyFestivalWinterPantsOptions
            },
            // ====== 축제 공용(읽기 전용, 모든 자녀) ======
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
                }
            }