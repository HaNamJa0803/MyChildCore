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

                // 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.hair.type") ?? "헤어 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.hair.type") ?? "아기 헤어 타입 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabyHairType,
                    setValue: v => Config.SpouseConfigs[spouse].BabyHairType = v,
                    allowedValues: DropdownConfig.BabyHairTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.hair.color") ?? "헤어 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.hair.color") ?? "아기 헤어 색상 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabyHairColor,
                    setValue: v => Config.SpouseConfigs[spouse].BabyHairColor = v,
                    allowedValues: () => DropdownConfig.BabyHairColorOptions.TryGetValue(Config.SpouseConfigs[spouse].BabyHairType, out var list) ? list.ToArray() : (DropdownConfig.BabyHairColorOptions.TryGetValue("Default", out var def) ? def.ToArray() : Array.Empty<string>())
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 눈
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.eye.type") ?? "눈동자 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.eye.type") ?? "아기 눈동자 타입 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabyEyeType,
                    setValue: v => Config.SpouseConfigs[spouse].BabyEyeType = v,
                    allowedValues: DropdownConfig.BabyEyeTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.eye.color") ?? "눈동자 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.eye.color") ?? "아기 눈동자 색상 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabyEyeColor,
                    setValue: v => Config.SpouseConfigs[spouse].BabyEyeColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BabyEyeColorOptions;
                        var type = Config.SpouseConfigs[spouse].BabyEyeType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 피부
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.skin.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.skin.type") ?? "아기 피부 타입 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabySkinType,
                    setValue: v => Config.SpouseConfigs[spouse].BabySkinType = v,
                    allowedValues: DropdownConfig.BabySkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.skin.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.skin.color") ?? "아기 피부 색상 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabySkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].BabySkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BabySkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].BabySkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 의상
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.body.type") ?? "의상 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.body.type") ?? "아기 의상 타입 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabyBodyType,
                    setValue: v => Config.SpouseConfigs[spouse].BabyBodyType = v,
                    allowedValues: DropdownConfig.BabyBodyTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.baby.body.color") ?? "의상 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.baby.body.color") ?? "아기 의상 색상 선택",
                    getValue: () => Config.SpouseConfigs[spouse].BabyBodyColor,
                    setValue: v => Config.SpouseConfigs[spouse].BabyBodyColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BabyBodyColorOptions;
                        var type = Config.SpouseConfigs[spouse].BabyBodyType;
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

                // 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.hair.type") ?? "헤어 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.hair.type") ?? "여자 유아의 헤어 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlHairType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlHairType = v,
                    allowedValues: DropdownConfig.GirlHairTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.hair.color") ?? "헤어 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.hair.color") ?? "여자 유아의 헤어 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlHairColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlHairColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlHairColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlHairType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 눈
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.eye.type") ?? "눈 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.eye.type") ?? "여자 유아의 눈 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlEyeType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlEyeType = v,
                    allowedValues: DropdownConfig.GirlEyeTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.eye.color") ?? "눈 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.eye.color") ?? "여자 유아의 눈 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlEyeColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlEyeColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlEyeColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlEyeType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 피부
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.skin.type") ?? "피부 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.skin.type") ?? "여자 유아의 피부 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlSkinType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlSkinType = v,
                    allowedValues: DropdownConfig.GirlSkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.skin.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.skin.color") ?? "여자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlSkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlSkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlSkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlSkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 봄 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.spring.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.spring.type") ?? "여자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopSpringType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopSpringType = v,
                    allowedValues: DropdownConfig.GirlTopSpringTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.spring.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.spring.color") ?? "여자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopSpringColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopSpringColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlTopSpringColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlTopSpringType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 여름 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.summer.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.summer.type") ?? "여자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopSummerType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopSummerType = v,
                    allowedValues: DropdownConfig.GirlTopSummerTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.summer.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.summer.color") ?? "여자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopSummerColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopSummerColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlTopSummerColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlTopSummerType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 가을 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.fall.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.fall.type") ?? "여자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopFallType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopFallType = v,
                    allowedValues: DropdownConfig.GirlTopFallTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.fall.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.fall.color") ?? "여자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopFallColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopFallColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlTopFallColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlTopFallType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 겨울 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.winter.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.winter.type") ?? "여자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopWinterType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopWinterType = v,
                    allowedValues: DropdownConfig.GirlTopWinterTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.top.winter.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.top.winter.color") ?? "여자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlTopWinterColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlTopWinterColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlTopWinterColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlTopWinterType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 바지
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.skirt.type") ?? "바지 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.skirt.type") ?? "여자 유아의 바지 타입",
                    getValue: () => Config.SpouseConfigs[spouse].SkirtType,
                    setValue: v => Config.SpouseConfigs[spouse].SkirtType = v,
                    allowedValues: DropdownConfig.SkirtTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.skirt.color") ?? "바지 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.skirt.color") ?? "여자 유아의 바지 색상",
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
                    name: () => Helper.Translation.Get("option.girl.shoes.type") ?? "신발 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.shoes.type") ?? "여자 유아의 신발 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlShoesType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlShoesType = v,
                    allowedValues: DropdownConfig.GirlShoesTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.shoes.color") ?? "신발 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.shoes.color") ?? "여자 유아의 신발 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlShoesColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlShoesColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlShoesColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlShoesType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
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

                // 잠옷
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.pajama.type") ?? "잠옷 스타일",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.pajama.type") ?? "여자 유아의 잠옷 스타일",
                    getValue: () => Config.SpouseConfigs[spouse].GirlPajamaType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlPajamaType = v,
                    allowedValues: DropdownConfig.GirlPajamaTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.girl.pajama.color") ?? "잠옷 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.girl.pajama.color") ?? "여자 유아의 잠옷 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlPajamaColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlPajamaColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlPajamaColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlPajamaType;
                        return dict.ContainsKey(type) ? dict[type].ToArray() : Array.Empty<string>();
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 봄 모자
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "봄 축제복 모자 타입",
                    tooltip: () => "여자 유아의 봄 축제복 모자 타입",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSpringHatType,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSpringHatType = v,
                    allowedValues: DropdownConfig.GirlFestivalSpringHatTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => "봄 축제복 모자 색상",
                    tooltip: () => "여자 유아의 봄 축제복 모자 색상",
                    getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSpringHatColor,
                    setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSpringHatColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.GirlFestivalSpringHatColorOptions;
                        var type = Config.SpouseConfigs[spouse].GirlFestivalSpringHatType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

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

                // 헤어
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.hair.type") ?? "헤어 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.hair.type") ?? "남자 유아의 헤어 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyHairType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyHairType = v,
                    allowedValues: DropdownConfig.BoyHairTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.hair.color") ?? "헤어 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.hair.color") ?? "남자 유아의 헤어 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyHairColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyHairColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyHairColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyHairType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 눈
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.eye.type") ?? "눈 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.eye.type") ?? "남자 유아의 눈 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyEyeType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyEyeType = v,
                    allowedValues: DropdownConfig.BoyEyeTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.eye.color") ?? "눈 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.eye.color") ?? "남자 유아의 눈 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyEyeColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyEyeColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyEyeColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyEyeType;
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
                    getValue: () => Config.SpouseConfigs[spouse].BoySkinType,
                    setValue: v => Config.SpouseConfigs[spouse].BoySkinType = v,
                    allowedValues: DropdownConfig.BoySkinTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.skin.color") ?? "피부 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.skin.color") ?? "남자 유아의 피부 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoySkinColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoySkinColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoySkinColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoySkinType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

                // 봄 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.spring.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.spring.type") ?? "남자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopSpringType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopSpringType = v,
                    allowedValues: DropdownConfig.BoyTopSpringTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.spring.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.spring.color") ?? "남자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopSpringColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopSpringColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyTopSpringColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyTopSpringType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 여름 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.summer.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.summer.type") ?? "남자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopSummerType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopSummerType = v,
                    allowedValues: DropdownConfig.BoyTopSummerTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.summer.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.summer.color") ?? "남자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopSummerColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopSummerColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyTopSummerColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyTopSummerType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 가을 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.fall.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.fall.type") ?? "남자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopFallType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopFallType = v,
                    allowedValues: DropdownConfig.BoyTopFallTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.fall.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.fall.color") ?? "남자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopFallColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopFallColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyTopFallColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyTopFallType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                
                // 겨울 평상복
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.winter.type") ?? "봄 상의 타입",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.winter.type") ?? "남자 유아 봄 상의 타입",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopWinterType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopWinterType = v,
                    allowedValues: DropdownConfig.BoyTopWinterTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.top.winter.color") ?? "봄 상의 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.top.winter.color") ?? "남자 유아 봄 상의 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyTopWinterColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyTopWinterColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyTopWinterColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyTopWinterType;
                        return dict.ContainsKey(type) ? dict[type].ToArray()
                            : (dict.ContainsKey("Default") ? dict["Default"].ToArray() : Array.Empty<string>());
                    },
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

                // 잠옷
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.pajama.type") ?? "잠옷 스타일",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.pajama.type") ?? "남자 유아의 잠옷 스타일",
                    getValue: () => Config.SpouseConfigs[spouse].BoyPajamaType,
                    setValue: v => Config.SpouseConfigs[spouse].BoyPajamaType = v,
                    allowedValues: DropdownConfig.BoyPajamaTypeOptions.ToArray(),
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );
                gmcm.AddTextOption(
                    mod: mod.ModManifest,
                    name: () => Helper.Translation.Get("option.boy.pajama.color") ?? "잠옷 색상",
                    tooltip: () => Helper.Translation.Get("tooltip.boy.pajama.color") ?? "남자 유아의 잠옷 색상",
                    getValue: () => Config.SpouseConfigs[spouse].BoyPajamaColor,
                    setValue: v => Config.SpouseConfigs[spouse].BoyPajamaColor = v,
                    allowedValues: () => {
                        var dict = DropdownConfig.BoyPajamaColorOptions;
                        var type = Config.SpouseConfigs[spouse].BoyPajamaType;
                        return dict.ContainsKey(type) ? dict[type].ToArray() : Array.Empty<string>();
                    },
                    formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                );

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