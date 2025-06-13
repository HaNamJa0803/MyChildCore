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
        public bool IsGirl { get; set; }
        public bool IsBoy { get; set; }
        public Func<string, string> GetValue { get; set; }
        public Action<string, string> SetValue { get; set; }
        public Func<string, string[]> AllowedValues { get; set; }
    }

    public static class GMCMOptionsMenus
    {
        public static readonly List<GMCMOption> GMCMOptions = new()
        {
            // 헤어
            new GMCMOption {
                NameKey = "option.baby.hair.type",
                TooltipKey = "tooltip.baby.hair.type",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyHairType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyHairType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BabyHairTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.baby.hair.color",
                TooltipKey = "tooltip.baby.hair.color",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyHairColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyHairColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BabyHairColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BabyHairType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 눈
            new GMCMOption {
                NameKey = "option.baby.eye.type",
                TooltipKey = "tooltip.baby.eye.type",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyEyeType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyEyeType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BabyEyeTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.baby.eye.color",
                TooltipKey = "tooltip.baby.eye.color",
                IsBoy = false,
                IsGirl = false, 
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyEyeColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyEyeColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BabyEyeColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BabyEyeType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 피부
            new GMCMOption {
                NameKey = "option.baby.skin.type",
                TooltipKey = "tooltip.baby.skin.type",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabySkinType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabySkinType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BabySkinTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.baby.skin.color",
                TooltipKey = "tooltip.baby.skin.color",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabySkinColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabySkinColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BabySkinColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BabySkinType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 의상
            new GMCMOption {
                NameKey = "option.baby.body.type",
                TooltipKey = "tooltip.baby.body.type",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyBodyType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyBodyType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BabyBodyTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.baby.body.color",
                TooltipKey = "tooltip.baby.body.color",
                IsBoy = false,
                IsGirl = false,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BabyBodyColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BabyBodyColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BabyBodyColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BabyBodyType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 헤어
            new GMCMOption {
                NameKey = "option.girl.hair.type",
                TooltipKey = "tooltip.girl.hair.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlHairType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlHairType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlHairTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.hair.color",
                TooltipKey = "tooltip.girl.hair.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlHairColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlHairColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlHairColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlHairType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 눈
            new GMCMOption {
                NameKey = "option.girl.eye.type",
                TooltipKey = "tooltip.girl.eye.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlEyeType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlEyeType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlEyeTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.eye.color",
                TooltipKey = "tooltip.girl.eye.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlEyeColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlEyeColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlEyeColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlEyeType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 피부
            new GMCMOption {
                NameKey = "option.girl.skin.type",
                TooltipKey = "tooltip.girl.skin.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlSkinType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlSkinType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlSkinTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.skin.color",
                TooltipKey = "tooltip.girl.skin.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlSkinColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlSkinColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlSkinColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlSkinType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 봄 평상복
            new GMCMOption {
                NameKey = "option.girl.top.spring.type",
                TooltipKey = "tooltip.girl.top.spring.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSpringType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopSpringType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlTopSpringTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.top.spring.color",
                TooltipKey = "tooltip.girl.top.spring.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSpringColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopSpringColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlTopSpringColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlTopSpringType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 여름 평상복
            new GMCMOption {
                NameKey = "option.girl.top.summer.type",
                TooltipKey = "tooltip.girl.top.summer.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSummerType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopSummerType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlTopSummerTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.top.summer.color",
                TooltipKey = "tooltip.girl.top.summer.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopSummerColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopSummerColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlTopSummerColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlTopSummerType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 가을 평상복
            new GMCMOption {
                NameKey = "option.girl.top.fall.type",
                TooltipKey = "tooltip.girl.top.fall.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopFallType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopFallType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlTopFallTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.top.fall.color",
                TooltipKey = "tooltip.girl.top.fall.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopFallColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopFallColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlTopFallColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlTopFallType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 겨울 평상복
            new GMCMOption {
                NameKey = "option.girl.top.winter.type",
                TooltipKey = "tooltip.girl.top.winter.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopWinterType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopWinterType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlTopWinterTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.top.winter.color",
                TooltipKey = "tooltip.girl.top.winter.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlTopWinterColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlTopWinterColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlTopWinterColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlTopWinterType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 치마
            new GMCMOption {
                NameKey = "option.girl.pants.type",
                TooltipKey = "tooltip.girl.pants.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].SkirtType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].SkirtType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].SkirtTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.pants.color",
                TooltipKey = "tooltip.girl.pants.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].SkirtColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].SkirtColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].SkirtColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].SkirtType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 신발
            new GMCMOption {
                NameKey = "option.girl.shoes.type",
                TooltipKey = "tooltip.girl.shoes.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlShoesType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlShoesType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlShoesTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.shoes.color",
                TooltipKey = "tooltip.girl.shoes.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlShoesColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlShoesColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlShoesColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlShoesType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 넥칼라
            new GMCMOption {
                NameKey = "option.girl.neckcollar.type",
                TooltipKey = "tooltip.girl.neckcollar.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlNeckCollarType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.neckcollar.color",
                TooltipKey = "tooltip.girl.neckcollar.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlNeckCollarColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlNeckCollarType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 잠옷
            new GMCMOption {
                NameKey = "option.girl.pajama.type",
                TooltipKey = "tooltip.girl.pajama.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlPajamaType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.pajama.color",
                TooltipKey = "tooltip.girl.pajama.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlPajamaColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlPajamaColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlPajamaColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlPajamaType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() : Array.Empty<string>();
                }
            },

            // 봄 모자
            new GMCMOption {
                NameKey = "option.girl.festival.spring.hat.type",
                TooltipKey = "tooltip.girl.festival.spring.hat.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.spring.hat.color",
                TooltipKey = "tooltip.girl.festival.spring.hat.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalSpringHatType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 여름 모자
            new GMCMOption {
                NameKey = "option.girl.festival.summer.hat.type",
                TooltipKey = "tooltip.girl.festival.summer.hat.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.summer.hat.color",
                TooltipKey = "tooltip.girl.festival.summer.hat.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerHatType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 여름 치마
            new GMCMOption {
                NameKey = "option.girl.festival.summer.skirt.type",
                TooltipKey = "tooltip.girl.festival.summer.skirt.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.summer.skirt.color",
                TooltipKey = "tooltip.girl.festival.summer.skirt.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalSummerSkirtType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 가을 치마
            new GMCMOption {
                NameKey = "option.girl.festival.fall.skirt.type",
                TooltipKey = "tooltip.girl.festival.fall.skirt.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.fall.skirt.color",
                TooltipKey = "tooltip.girl.festival.fall.skirt.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalFallSkirtType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 겨울 모자
            new GMCMOption {
                NameKey = "option.girl.festival.winter.hat.type",
                TooltipKey = "tooltip.girl.festival.winter.hat.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.winter.hat.color",
                TooltipKey = "tooltip.girl.festival.winter.hat.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterHatType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 겨울 치마
            new GMCMOption {
                NameKey = "option.girl.festival.winter.skirt.type",
                TooltipKey = "tooltip.girl.festival.winter.skirt.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.winter.skirt.color",
                TooltipKey = "tooltip.girl.festival.winter.skirt.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkirtType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 겨울 스카프
            new GMCMOption {
                NameKey = "option.girl.festival.winter.skarf.type",
                TooltipKey = "tooltip.girl.festival.winter.skarf.type",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.girl.festival.winter.skarf.color",
                TooltipKey = "tooltip.girl.festival.winter.skarf.color",
                IsGirl = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].GirlFestivalWinterSkarfType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
        
            // 헤어
            new GMCMOption {
                NameKey = "option.boy.hair.type",
                TooltipKey = "tooltip.boy.hair.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyHairType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyHairType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyHairTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.hair.color",
                TooltipKey = "tooltip.boy.hair.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyHairColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyHairColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyHairColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyHairType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 눈
            new GMCMOption {
                NameKey = "option.boy.eye.type",
                TooltipKey = "tooltip.boy.eye.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyEyeType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyEyeType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyEyeTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.eye.color",
                TooltipKey = "tooltip.boy.eye.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyEyeColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyEyeColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyEyeColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyEyeType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 피부
            new GMCMOption {
                NameKey = "option.boy.skin.type",
                TooltipKey = "tooltip.boy.skin.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoySkinType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoySkinType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoySkinTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.skin.color",
                TooltipKey = "tooltip.boy.skin.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoySkinColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoySkinColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoySkinColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoySkinType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 봄 평상복
            new GMCMOption {
                NameKey = "option.boy.top.spring.type",
                TooltipKey = "tooltip.boy.top.spring.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSpringType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopSpringType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyTopSpringTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.top.spring.color",
                TooltipKey = "tooltip.boy.top.spring.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSpringColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopSpringColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyTopSpringColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyTopSpringType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 여름 평상복
            new GMCMOption {
                NameKey = "option.boy.top.summer.type",
                TooltipKey = "tooltip.boy.top.summer.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSummerType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopSummerType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyTopSummerTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.top.summer.color",
                TooltipKey = "tooltip.boy.top.summer.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopSummerColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopSummerColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyTopSummerColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyTopSummerType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 가을 평상복
            new GMCMOption {
                NameKey = "option.boy.top.fall.type",
                TooltipKey = "tooltip.boy.top.fall.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopFallType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopFallType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyTopFallTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.top.fall.color",
                TooltipKey = "tooltip.boy.top.fall.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopFallColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopFallColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyTopFallColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyTopFallType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // 겨울 평상복
            new GMCMOption {
                NameKey = "option.boy.top.winter.type",
                TooltipKey = "tooltip.boy.top.winter.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopWinterType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopWinterType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyTopWinterTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.top.winter.color",
                TooltipKey = "tooltip.boy.top.winter.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyTopWinterColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyTopWinterColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyTopWinterColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyTopWinterType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() :
                        (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 바지
            new GMCMOption {
                NameKey = "option.boy.pants.type",
                TooltipKey = "tooltip.boy.pants.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].SkirtType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].SkirtType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].SkirtTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.pants.color",
                TooltipKey = "tooltip.boy.pants.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].SkirtColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].SkirtColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].SkirtColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].SkirtType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 신발
            new GMCMOption {
                NameKey = "option.boy.shoes.type",
                TooltipKey = "tooltip.boy.shoes.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyShoesType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyShoesType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyShoesTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.shoes.color",
                TooltipKey = "tooltip.boy.shoes.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyShoesColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyShoesColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyShoesColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyShoesType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 넥칼라
            new GMCMOption {
                NameKey = "option.boy.neckcollar.type",
                TooltipKey = "tooltip.boy.neckcollar.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyNeckCollarType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.neckcollar.color",
                TooltipKey = "tooltip.boy.neckcollar.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyNeckCollarColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyNeckCollarType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 잠옷
            new GMCMOption {
                NameKey = "option.boy.pajama.type",
                TooltipKey = "tooltip.boy.pajama.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyPajamaType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.pajama.color",
                TooltipKey = "tooltip.boy.pajama.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyPajamaColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyPajamaColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyPajamaColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyPajamaType;
                    return dict.ContainsKey(type) ? dict[type].ToArray() : Array.Empty<string>();
                }
            },
            
            // 봄 모자
            new GMCMOption {
                NameKey = "option.boy.festival.spring.hat.type",
                TooltipKey = "tooltip.boy.festival.spring.hat.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.spring.hat.color",
                TooltipKey = "tooltip.boy.festival.spring.hat.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalSpringHatType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 여름 모자
            new GMCMOption {
                NameKey = "option.boy.festival.summer.hat.type",
                TooltipKey = "tooltip.boy.festival.summer.hat.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.summer.hat.color",
                TooltipKey = "tooltip.boy.festival.summer.hat.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerHatType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 여름 바지
            new GMCMOption {
                NameKey = "option.boy.festival.summer.pants.type",
                TooltipKey = "tooltip.boy.festival.summer.pants.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.summer.pants.color",
                TooltipKey = "tooltip.boy.festival.summer.pants.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalSummerPantsType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 가을 바지
            new GMCMOption {
                NameKey = "option.boy.festival.fall.pants.type",
                TooltipKey = "tooltip.boy.festival.fall.pants.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.fall.pants.color",
                TooltipKey = "tooltip.boy.festival.fall.pants.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalFallPantsType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 겨울 모자
            new GMCMOption {
                NameKey = "option.boy.festival.winter.hat.type",
                TooltipKey = "tooltip.boy.festival.winter.hat.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.winter.hat.color",
                TooltipKey = "tooltip.boy.festival.winter.hat.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterHatType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 겨울 바지
            new GMCMOption {
                NameKey = "option.boy.festival.winter.pants.type",
                TooltipKey = "tooltip.boy.festival.winter.pants.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.winter.pants.color",
                TooltipKey = "tooltip.boy.festival.winter.pants.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterPantsType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },

            // 겨울 스카프
            new GMCMOption {
                NameKey = "option.boy.festival.winter.skarf.type",
                TooltipKey = "tooltip.boy.festival.winter.skarf.type",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfType,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfType = v; },
                AllowedValues = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfTypeOptions.ToArray()
            },
            new GMCMOption {
                NameKey = "option.boy.festival.winter.skarf.color",
                TooltipKey = "tooltip.boy.festival.winter.skarf.color",
                IsBoy = true,
                GetValue = s => ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfColor,
                SetValue = (s, v) => { ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfColor = v; },
                AllowedValues = s => {
                    var dict = ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfColorOptions;
                    var type = ModEntry.Config.SpouseConfigs[s].BoyFestivalWinterSkarfType;
                    return dict.ContainsKey(type) ? dict[type].ToArray()
                        : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                }
            },
            
            // === 공용 활성화 스위치(최하단) ===
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
        };

        // Observer 실시간 연동
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