using MyChildCore;
using System;
using StardewModdingAPI;
using GenericModConfigMenu;

namespace MyChildCore
{
    public static class GMCMManager
    {
        public static event Action OnConfigChanged;

        private static ModConfig Config => ModEntry.Config;
        private static IModHelper Helper => ModEntry.ModHelper;

        public static void RegisterGMCM(IModHelper helper, Mod mod)
        {
            Config.InitSpouseKeys(DropdownConfig.SpouseNames);

            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null)
            {
                CustomLogger.Warn("[GMCMManager] GMCM API를 찾을 수 없습니다!");
                return;
            }

            // ==== 1. 전역 옵션 ====
            gmcm.Register(
                mod: mod.ModManifest,
                reset: ResetConfig,
                save: SaveConfig
            );

            gmcm.AddBoolOption(
                mod: mod.ModManifest,
                name: () => "모드 활성화",
                tooltip: () => "이 모드를 켜거나 끕니다.",
                getValue: () => Config.EnableMod,
                setValue: v => { Config.EnableMod = v; NotifyConfigChanged(); }
            );
            gmcm.AddBoolOption(
                mod: mod.ModManifest,
                name: () => "잠옷 전체 활성화",
                tooltip: () => "자녀의 잠옷을 활성화/비활성화합니다.",
                getValue: () => Config.EnablePajama,
                setValue: v => { Config.EnablePajama = v; NotifyConfigChanged(); }
            );
            gmcm.AddBoolOption(
                mod: mod.ModManifest,
                name: () => "축제복 전체 활성화",
                tooltip: () => "자녀의 축제복 시스템을 활성화/비활성화합니다.",
                getValue: () => Config.EnableFestival,
                setValue: v => { Config.EnableFestival = v; NotifyConfigChanged(); }
            );

            // ==== 2. 배우자 페이지 링크 ====
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                string pageId = $"spouse_{spouse}";
                gmcm.AddPageLink(
                    mod: mod.ModManifest,
                    pageId: pageId,
                    text: () => Helper.Translation.Get(spouse) ?? spouse,
                    tooltip: () => $"{Helper.Translation.Get(spouse) ?? spouse} 자녀 설정 페이지로 이동"
                );
            }

            // ==== 3. 배우자별 파츠/옵션 ====
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                string pageId = $"spouse_{spouse}";
                gmcm.AddPage(
                    mod: mod.ModManifest,
                    pageId: pageId,
                    pageTitle: () => $"{Helper.Translation.Get(spouse) ?? spouse} 자녀 설정"
                );

                // ======= 초기화, 잠옷/축제복 활성화 =======
                gmcm.AddBoolOption(
                    mod: mod.ModManifest,
                    name: () => "[기본값으로 초기화]",
                    tooltip: () => "체크하면 이 배우자의 자녀 설정이 모두 기본값으로 초기화됩니다.",
                    getValue: () => false,
                    setValue: v =>
                    {
                        if (v)
                        {
                            Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                            SaveConfig();
                        }
                    }
                );
                
                // 아기
                gmcm.AddSectionTitle(
                    mod: mod.ModManifest,
                    text: () => "아기",
                    tooltip: null
                );

                // 1. 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.hair.type") ?? "헤어 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.hair.type") ?? "남자 유아의 헤어 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boyHairType,
                    setValue: v => Config.SpouseConfigs[spouse].boyHairType = v,
                    allowedValues: new[] { "짧은머리", "곱슬머리", "바가지머리" }, // 여기에 실제 헤어 타입들 넣어주세요
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "헤어 색상",
                    tooltip: () => "헤어의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boyHairColor,
                    setValue: v => Config.SpouseConfigs[spouse].boyHairColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" }, // 타입1의 실제 색상들
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 눈
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.eye.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.eye.type") ?? "남자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinType,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinType = v,
                    allowedValues: DropdownConfig.boySkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.babyy.eye.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.eye.color") ?? "남자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.boySkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].boySkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 피부
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.skin.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.skin.type") ?? "남자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinType,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinType = v,
                    allowedValues: DropdownConfig.boySkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.skin.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.skin.color") ?? "남자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.boySkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].boySkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 여자 유아
                gmcm.AddSectionTitle(
                    mod: mod.ModManifest,
                    text: () => "여자 유아",
                    tooltip: null
                );

                // 1. 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.hair.type") ?? "헤어 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.hair.type") ?? "여자 유아의 헤어 타입",
                    getValue: () => Config.SpouseConfigs[spouse].girlHairType,
                    setValue: v => Config.SpouseConfigs[spouse].girlHairType = v,
                    allowedValues: new[] { "짧은머리", "곱슬머리", "바가지머리" }, // 여기에 실제 헤어 타입들 넣어주세요
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "헤어 색상",
                    tooltip: () => "헤어의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].girlHairColor,
                    setValue: v => Config.SpouseConfigs[spouse].girlHairColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값", "기본값", "기본값" }, // 타입1의 실제 색상들
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 눈
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.eye.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.eye.type") ?? "여자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].girlSkinType,
                    setValue: v => Config.SpouseConfigs[spouse].girlSkinType = v,
                    allowedValues: DropdownConfig.girlSkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.eye.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.eye.color") ?? "여자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].girlSkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].girlSkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.girlSkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].girlSkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 피부
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.skin.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.skin.type") ?? "남자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].girlSkinType,
                    setValue: v => Config.SpouseConfigs[spouse].girlSkinType = v,
                    allowedValues: DropdownConfig.girlSkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.skin.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.skin.color") ?? "남자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].girlSkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].girlSkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.girlSkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].girlSkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                                // 1. 봄 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.spring.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.spring.type") ?? "여자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopSpringType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopSpringType = v,
                    allowedValues: new[] { "반팔셔츠", "긴팔셔츠", "민소매셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 셔츠 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "반팔셔츠 색상",
                    tooltip: () => "봄 반팔셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopSpringColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopSpringColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" }, // 봄 색상들
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 1. 여름 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.summer.type") ?? "여름 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.summer.type") ?? "여자 유아 여름 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].girlTopSummerType,
                    setValue: v => Config.SpouseConfigs[spouse].girlTopSummerType = v,
                    allowedValues: new[] { "민소매셔츠", "반팔셔츠", "긴팔셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 셔츠 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "민소매셔츠 색상",
                    tooltip: () => "여름 셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].girlTopSummerColor,
                    setValue: v => Config.SpouseConfigs[spouse].girlTopSummerColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 1. 가을 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.fall.type") ?? "가을 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.fall.type") ?? "여자 유아 가을 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].girlTopFallType,
                    setValue: v => Config.SpouseConfigs[spouse].girlTopFallType = v,
                    allowedValues: new[] { "반팔셔츠", "긴팔셔츠", "민소매셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 가을 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "반팔셔츠 색상",
                    tooltip: () => "가을 셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].girlTopFallColor,
                    setValue: v => Config.SpouseConfigs[spouse].girlTopFallColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 1. 겨울 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.winter.type") ?? "겨울 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.winter.type") ?? "여자 유아 겨울 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].girlTopWinterType,
                    setValue: v => Config.SpouseConfigs[spouse].girlTopWinterType = v,
                    allowedValues: new[] { "긴팔셔츠", "반팔셔츠", "민소매셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 겨울 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "긴팔셔츠 색상",
                    tooltip: () => "겨울 셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].girlTopWinterColor,
                    setValue: v => Config.SpouseConfigs[spouse].girlTopWinterColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 1. 치마
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.spring.type") ?? "치마 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.spring.type") ?? "여자 유아 치마 타입",
                    getValue: () => Config.SpouseConfigs[spouse].SkirtTypeType,
                    setValue: v => Config.SpouseConfigs[spouse].SkirtTypeType = v,
                    allowedValues: new[] { "멜빵치마" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 치마 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "치마 색상",
                    tooltip: () => "치마의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].SkirtColor,
                    setValue: v => Config.SpouseConfigs[spouse].SkirtColor = v,
                    allowedValues: new[] { "블루", "레드", "옐로", "그린", "화이트", "블랙" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 1. 신발
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.spring.type") ?? "신발 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.spring.type") ?? "여자 유아 신발 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlShoesType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlShoesType = v,
                    allowedValues: new[] { "로퍼" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 신발 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "로퍼 색상",
                    tooltip: () => "로퍼의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlShoesColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlShoesColor = v,
                    allowedValues: new[] { "블랙", "레드", "옐로", "그린", "화이트", "블루" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 넥칼라
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.neckcollar.type") ?? "넥칼라 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.neckcollar.type") ?? "여자 유아의 넥칼라 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlNeckCollarType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlNeckCollarType = v,
                    allowedValues: DropdownConfig.GirlNeckCollarTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.neckcollar.color") ?? "넥칼라 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.neckcollar.color") ?? "여자 유아의 넥칼라 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlNeckCollarColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlNeckCollarColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlNeckCollarColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlNeckCollarType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 1. 잠옷 타입
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.pajama.type") ?? "잠옷 스타일",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.pajama.type") ?? "여자 유아의 잠옷 스타일",
                    getValue: () => Config.SpouseConfigs[spouse].GirlPajamaType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlPajamaType = v,
                    allowedValues: new[] { "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 잠옷 색상 (타입별)
                var pajamaColorDict = new Dictionary<string, string[]> {
                    ["Frog"] = new[] { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" },
                    ["LesserPanda"] = new[] { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" },
                    ["Raccoon"] = new[] { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" },
                    ["Sheep"] = new[] { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" },
                    ["Shark"] = new[] { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" },
                    ["WelshCorgi"] = new[] { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" },
                };

                foreach (var type in pajamaColorDict.Keys)
                {
                    gmcm.AddTextOption(
                        mod: mod.ModManifest,
                        name: () => $"{Helper.Translation.Get("option.girl.pajama.color")} ({type})",
                        tooltip: () => $"{type} 잠옷의 색상을 선택하세요.",
                        getValue: () => Config.SpouseConfigs[spouse].GirlPajamaType == type ? Config.SpouseConfigs[spouse].GirlPajamaColor : "",
                        setValue: v => { if (Config.SpouseConfigs[spouse].GirlPajamaType == type) Config.SpouseConfigs[spouse].GirlPajamaColor = v; },
                        allowedValues: pajamaColorDict[type],
                        formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                    );
                }

                // 여름 모자
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 모자 타입",
                    tooltip: () => "여자 유아의 여름 축제복 모자 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSummerHatType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSummerHatType = v,
                    allowedValues: DropdownConfig.GirlFestivalSummerHatTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 모자 색상",
                    tooltip: () => "여자 유아의 여름 축제복 모자 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSummerHatColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSummerHatColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalSummerHatColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalSummerHatType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 여름 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 바지 타입",
                    tooltip: () => "여자 유아의 여름 축제복 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSummerPantsType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSummerPantsType = v,
                    allowedValues: DropdownConfig.GirlFestivalSummerPantsTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 바지 색상",
                    tooltip: () => "여자 유아의 여름 축제복 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSummerPantsColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSummerPantsColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalSummerPantsColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalSummerPantsType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 가을 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "가을 축제복 바지 타입",
                    tooltip: () => "여자 유아의 가을 축제복 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalFallPantsType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalFallPantsType = v,
                    allowedValues: DropdownConfig.GirlFestivalFallPantsTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "가을 축제복 바지 색상",
                    tooltip: () => "여자 유아의 가을 축제복 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalFallPantsColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalFallPantsColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalFallPantsColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalFallPantsType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 겨울 모자
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 모자 타입",
                    tooltip: () => "여자 유아의 겨울 축제복 모자 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterHatType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterHatType = v,
                    allowedValues: DropdownConfig.GirlFestivalWinterHatTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 모자 색상",
                    tooltip: () => "여자 유아의 겨울 축제복 모자 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterHatColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterHatColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalWinterHatColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalWinterHatType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 겨울 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 바지 타입",
                    tooltip: () => "여자 유아의 겨울 축제복 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterPantsType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterPantsType = v,
                    allowedValues: DropdownConfig.GirlFestivalWinterPantsTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 바지 색상",
                    tooltip: () => "여자 유아의 겨울 축제복 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterPantsColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterPantsColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalWinterPantsColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalWinterPantsType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 겨울 스카프
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 스카프 타입",
                    tooltip: () => "여자 유아의 겨울 축제복 스카프 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterSkarfType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterSkarfType = v,
                    allowedValues: DropdownConfig.GirlFestivalWinterSkarfTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 스카프 색상",
                    tooltip: () => "여자 유아의 겨울 축제복 스카프 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterSkarfColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterSkarfColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalWinterSkarfColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalWinterSkarfType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 남자 유아
                gmcm.AddSectionTitle(
                    mod: mod.ModManifest,
                    text: () => "남자 유아",
                    tooltip: null
                );

                // 1. 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.hair.type") ?? "헤어 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.hair.type") ?? "남자 유아의 헤어 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boyHairType,
                    setValue: v => Config.SpouseConfigs[spouse].boyHairType = v,
                    allowedValues: new[] { "짧은머리", "곱슬머리", "바가지머리" }, // 여기에 실제 헤어 타입들 넣어주세요
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "헤어 색상",
                    tooltip: () => "헤어의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boyHairColor,
                    setValue: v => Config.SpouseConfigs[spouse].boyHairColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" }, // 타입1의 실제 색상들
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 눈
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.eye.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.eye.type") ?? "남자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinType,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinType = v,
                    allowedValues: DropdownConfig.boySkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.eye.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.eye.color") ?? "남자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.boySkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].boySkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 피부
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.skin.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.skin.type") ?? "남자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinType,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinType = v,
                    allowedValues: DropdownConfig.boySkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.skin.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.skin.color") ?? "남자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boySkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].boySkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.boySkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].boySkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                                // 1. 봄 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.spring.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.spring.type") ?? "남자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopSpringType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopSpringType = v,
                    allowedValues: new[] { "반팔셔츠", "긴팔셔츠", "민소매셔츠"},
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 셔츠 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "반팔셔츠 색상",
                    tooltip: () => "봄 반팔셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopSpringColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopSpringColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" }, // 봄 색상들
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 1. 여름 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.summer.type") ?? "여름 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.summer.type") ?? "남자 유아 여름 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boyTopSummerType,
                    setValue: v => Config.SpouseConfigs[spouse].boyTopSummerType = v,
                    allowedValues: new[] { "민소매셔츠", "반팔셔츠", "긴팔셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 셔츠 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "민소매셔츠 색상",
                    tooltip: () => "여름 셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boyTopSummerColor,
                    setValue: v => Config.SpouseConfigs[spouse].boyTopSummerColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 1. 가을 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.fall.type") ?? "가을 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.fall.type") ?? "남자 유아 가을 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boyTopFallType,
                    setValue: v => Config.SpouseConfigs[spouse].boyTopFallType = v,
                    allowedValues: new[] { "반팔셔츠", "긴팔셔츠", "민소매셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 가을 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "반팔셔츠 색상",
                    tooltip: () => "가을 셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boyTopFallColor,
                    setValue: v => Config.SpouseConfigs[spouse].boyTopFallColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 1. 겨울 상의
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.winter.type") ?? "겨울 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.winter.type") ?? "남자 유아 겨울 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].boyTopWinterType,
                    setValue: v => Config.SpouseConfigs[spouse].boyTopWinterType = v,
                    allowedValues: new[] { "긴팔셔츠", "반팔셔츠", "민소매셔츠" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 겨울 색상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "긴팔셔츠 색상",
                    tooltip: () => "겨울 셔츠의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].boyTopWinterColor,
                    setValue: v => Config.SpouseConfigs[spouse].boyTopWinterColor = v,
                    allowedValues: new[] { "기본값", "기본값", "기본값" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.skirt.type") ?? "바지 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.skirt.type") ?? "남자 유아의 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].SkirtType,
                    setValue: v => Config.SpouseConfigs[spouse].SkirtType = v,
                    allowedValues: DropdownConfig.SkirtTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.skirt.color") ?? "바지 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.skirt.color") ?? "남자 유아의 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].SkirtColor,
                    setValue: v => Config.SpouseConfigs[spouse].SkirtColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.SkirtColorOptions;
                        var type = Config.SpouseConfigs[spouse].SkirtType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 신발
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.shoes.type") ?? "신발 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.shoes.type") ?? "남자 유아의 신발 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyShoesType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyShoesType = v,
                    allowedValues: DropdownConfig.BoyShoesTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.shoes.color") ?? "신발 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.shoes.color") ?? "남자 유아의 신발 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyShoesColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyShoesColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyShoesColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyShoesType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 넥칼라
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.neckcollar.type") ?? "넥칼라 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.neckcollar.type") ?? "남자 유아의 넥칼라 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyNeckCollarType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyNeckCollarType = v,
                    allowedValues: DropdownConfig.BoyNeckCollarTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.neckcollar.color") ?? "넥칼라 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.neckcollar.color") ?? "남자 유아의 넥칼라 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyNeckCollarColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyNeckCollarColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyNeckCollarColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyNeckCollarType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 1. 잠옷 타입
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.pajama.type") ?? "잠옷 스타일",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.pajama.type") ?? "여자 유아의 잠옷 스타일",
                    getValue: () => Config.SpouseConfigs[spouse].BoyPajamaType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyPajamaType = v,
                    allowedValues: new[] { "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi" },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 2. 잠옷 색상 (타입별)
                var pajamaColorDict = new Dictionary<string, string[]> {
                    ["Frog"] = new[] { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" },
                    ["LesserPanda"] = new[] { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" },
                    ["Raccoon"] = new[] { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" },
                    ["Sheep"] = new[] { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" },
                    ["Shark"] = new[] { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" },
                    ["WelshCorgi"] = new[] { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" },
                };

                foreach (var type in pajamaColorDict.Keys)
                {
                    gmcm.AddTextOption(
                        mod: mod.ModManifest,
                        name: () => $"{Helper.Translation.Get("option.boy.pajama.color")} ({type})",
                        tooltip: () => $"{type} 잠옷의 색상을 선택하세요.",
                        getValue: () => Config.SpouseConfigs[spouse].BoyPajamaType == type ? Config.SpouseConfigs[spouse].BoyPajamaColor : "",
                        setValue: v => { if (Config.SpouseConfigs[spouse].BoyPajamaType == type) Config.SpouseConfigs[spouse].BoyPajamaColor = v; },
                        allowedValues: pajamaColorDict[type],
                        formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                    );
                }

                // 봄 모자
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "봄 축제복 모자 타입",
                    tooltip: () => "남자 유아의 봄 축제복 모자 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSpringHatType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSpringHatType = v,
                    allowedValues: DropdownConfig.BoyFestivalSpringHatTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "봄 축제복 모자 색상",
                    tooltip: () => "남자 유아의 봄 축제복 모자 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSpringHatColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSpringHatColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalSpringHatColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalSpringHatType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 여름 모자
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 모자 타입",
                    tooltip: () => "남자 유아의 여름 축제복 모자 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSummerHatType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSummerHatType = v,
                    allowedValues: DropdownConfig.BoyFestivalSummerHatTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 모자 색상",
                    tooltip: () => "남자 유아의 여름 축제복 모자 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSummerHatColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSummerHatColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalSummerHatColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalSummerHatType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 여름 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 바지 타입",
                    tooltip: () => "남자 유아의 여름 축제복 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSummerPantsType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSummerPantsType = v,
                    allowedValues: DropdownConfig.BoyFestivalSummerPantsTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "여름 축제복 바지 색상",
                    tooltip: () => "남자 유아의 여름 축제복 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSummerPantsColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSummerPantsColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalSummerPantsColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalSummerPantsType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 가을 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "가을 축제복 바지 타입",
                    tooltip: () => "남자 유아의 가을 축제복 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalFallPantsType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalFallPantsType = v,
                    allowedValues: DropdownConfig.BoyFestivalFallPantsTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "가을 축제복 바지 색상",
                    tooltip: () => "남자 유아의 가을 축제복 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalFallPantsColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalFallPantsColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalFallPantsColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalFallPantsType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 겨울 모자
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 모자 타입",
                    tooltip: () => "남자 유아의 겨울 축제복 모자 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterHatType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterHatType = v,
                    allowedValues: DropdownConfig.BoyFestivalWinterHatTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 모자 색상",
                    tooltip: () => "남자 유아의 겨울 축제복 모자 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterHatColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterHatColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalWinterHatColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalWinterHatType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 겨울 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 바지 타입",
                    tooltip: () => "남자 유아의 겨울 축제복 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterPantsType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterPantsType = v,
                    allowedValues: DropdownConfig.BoyFestivalWinterPantsTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 바지 색상",
                    tooltip: () => "남자 유아의 겨울 축제복 바지 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterPantsColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterPantsColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalWinterPantsColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalWinterPantsType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 겨울 스카프
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 스카프 타입",
                    tooltip: () => "남자 유아의 겨울 축제복 스카프 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterSkarfType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterSkarfType = v,
                    allowedValues: DropdownConfig.BoyFestivalWinterSkarfTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "겨울 축제복 스카프 색상",
                    tooltip: () => "남자 유아의 겨울 축제복 스카프 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterSkarfColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterSkarfColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyFestivalWinterSkarfColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyFestivalWinterSkarfType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
            }
        }

        private static void NotifyConfigChanged()
        {
            try { OnConfigChanged?.Invoke(); }
            catch (Exception ex) { CustomLogger.Warn("[GMCMManager] OnConfigChanged 예외: " + ex.Message); }
        }
        public static void ResetConfig()
        {
            ModEntry.Config = new ModConfig();
            ModEntry.SaveConfig();
            NotifyConfigChanged();
        }
        public static void SaveConfig()
        {
            ModEntry.SaveConfig();
            NotifyConfigChanged();
        }
    }
}