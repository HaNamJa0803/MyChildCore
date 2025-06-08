using System;
using StardewModdingAPI;
uaing GenericModConfigMenu;

namespace MyChildCore
{
    /// <summary>
    /// GMCM 연동/등록/옵션 관리 전담 매니저 (유니크 칠드런식, 반복코드 구간 주석 표시)
    /// </summary>
    public static class GMCMManager
    {
        // ModEntry에서 static으로 관리하는 리소스/설정 가져오기
        private static IManifest ModManifest => ModEntry.ModManifest;
        private static ModConfig Config => ModEntry.Config;
        private static IModHelper Helper => ModEntry.ModHelper;
        
        /// <summary>
        /// GMCM 설정 메뉴 등록 (반복 코드 구간 주석으로 안내)
        /// </summary>
        public static void RegisterGMCM(IModHelper helper, Mod mod)
        {
            // GMCM API 불러오기
            var gmcmApi = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcmApi == null)
            {
                CustomLogger.Warn("[GMCMManager] GMCM API를 찾을 수 없습니다!");
                return;
            }

            // 기본 옵션 등록(리셋/저장)
            gmcm.Register(
                mod: mod.ModManifest,
                reset: () => {
                    ModEntry.Config = new ModConfig();
                    ModEntry.SaveConfig();
                },
                save: () => ModEntry.SaveConfig()
            );

            // ────────────── 여기에 [반복 코드] 붙여넣기 ──────────────
            // ────────── 배우자 탭 (AddPageLink) ──────────
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                string pageId = $"spouse_{spouse}";
                gmcm.AddPageLink(
                    mod: ModEntry.Instance.ModManifest,
                    pageId: pageId,
                    text: () => Helper.Translation.Get(spouse) ?? spouse,
                    tooltip: () => $"{Helper.Translation.Get(spouse) ?? spouse} 자녀 설정 페이지로 이동"
                );
            }

            // ────────── 배우자별 세부 페이지 (AddPage) ──────────
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                string pageId = $"spouse_{spouse}";
                gmcm.AddPage(
                    mod: ModManifest,
                    pageId: pageId,
                    pageTitle: () => $"{Helper.Translation.Get(spouse) ?? spouse} 자녀 설정"
                    );
                        
                        gmcm.AddSectionTitle(mod: ModManifest, text: () => "아기", tooltip: null);
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "헤어스타일",
                            tooltip: () => "아기의 헤어스타일",
                            getValue: () => Config.SpouseConfigs[spouse].BabyHairStyles,
                            setValue: v => Config.SpouseConfigs[spouse].BabyHairStyles = v,
                            allowedValues: DropdownConfig.BabyHairStyles,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "눈동자 색상",
                            tooltip: () => "아기의 눈동자 색상",
                            getValue: () => Config.SpouseConfigs[spouse].BabyEyes,
                            setValue: v => Config.SpouseConfigs[spouse].BabyEyes = v,
                            allowedValues: DropdownConfig.BabyEyes,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "피부색",
                            tooltip: () => "아기의 피부색",
                            getValue: () => Config.SpouseConfigs[spouse].BabySkins,
                            setValue: v => Config.SpouseConfigs[spouse].BabySkins = v,
                            allowedValues: DropdownConfig.BabySkins,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "의상",
                            tooltip: () => "아기의 옷",
                            getValue: () => Config.SpouseConfigs[spouse].BabyBodies,
                            setValue: v => Config.SpouseConfigs[spouse].BabyBodies = v,
                            allowedValues: DropdownConfig.BabyBodies,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddSectionTitle(mod: ModManifest, text: () => "남자 유아", tooltip: null);
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "헤어스타일",
                            tooltip: () => "남자 유아의 헤어스타일",
                            getValue: () => Config.SpouseConfigs[spouse].BoyHairStyles,
                            setValue: v => Config.SpouseConfigs[spouse].BoyHairStyles = v,
                            allowedValues: DropdownConfig.BoyHairStyles,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "눈동자 색상",
                            tooltip: () => "남자 유아의 눈동자 색상",
                            getValue: () => Config.SpouseConfigs[spouse].BoyEyes,
                            setValue: v => Config.SpouseConfigs[spouse].BoyEyes = v,
                            allowedValues: DropdownConfig.BoyEyes,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "피부색",
                            tooltip: () => "남자 유아의 피부색",
                            getValue: () => Config.SpouseConfigs[spouse].BoySkins,
                            setValue: v => Config.SpouseConfigs[spouse].BoySkins = v,
                            allowedValues: DropdownConfig.BoySkins,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "봄 상의",
                            tooltip: () => "남자 유아의 봄 상의",
                            getValue: () => Config.SpouseConfigs[spouse].BoyTopSpringOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyTopSpringOptions = v,
                            allowedValues: DropdownConfig.BoyTopSpringOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "여름 상의",
                            tooltip: () => "남자 유아의 여름 상의",
                            getValue: () => Config.SpouseConfigs[spouse].BoyTopSummerOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyTopSummerOptions = v,
                            allowedValues: DropdownConfig.BoyTopSummerOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "가을 상의",
                            tooltip: () => "남자 유아의 가을 상의",
                            getValue: () => Config.SpouseConfigs[spouse].BoyTopFallOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyTopFallOptions = v,
                            allowedValues: DropdownConfig.BoyTopFallOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 상의",
                            tooltip: () => "남자 유아의 겨울 상의",
                            getValue: () => Config.SpouseConfigs[spouse].BoyTopWinterOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyTopWinterOptions = v,
                            allowedValues: DropdownConfig.BoyTopWinterOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "바지 색상",
                            tooltip: () => "남자 유아의 바지 색상",
                            getValue: () => Config.SpouseConfigs[spouse].PantsColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].PantsColorOptions = v,
                            allowedValues: DropdownConfig.PantsColorOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "신발 색상",
                            tooltip: () => "남자 유아의 신발 색상",
                            getValue: () => Config.SpouseConfigs[spouse].BoyShoesColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyShoesColorOptions = v,
                            allowedValues: DropdownConfig.BoyShoesColorOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "넥칼라 색상",
                            tooltip: () => "남자 유아의 넥칼라 색상",
                            getValue: () => Config.SpouseConfigs[spouse].BoyNeckCollarColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyNeckCollarColorOptions = v,
                            allowedValues: DropdownConfig.BoyNeckCollarColorOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "잠옷 스타일",
                            tooltip: () => "남자 유아의 잠옷 스타일",
                            getValue: () => Config.SpouseConfigs[spouse].BoyPajamaTypeOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyPajamaTypeOptions = v,
                            allowedValues: DropdownConfig.BoyPajamaTypeOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "잠옷 색상",
                            tooltip: () => "남자 유아의 잠옷 색상",
                            getValue: () => Config.SpouseConfigs[spouse].BoyPajamaColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyPajamaColorOptions = v,
                            allowedValues: DropdownConfig.BoyPajamaColorOptions[Config.SpouseConfigs[spouse].BoyPajamaTypeOptions],
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value 
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "봄 축제복 모자",
                            tooltip: () => "남자 유아의 봄 축제복 모자",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalSpringHat,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalSpringHat = v,
                            allowedValues: DropdownConfig.FestivalSpringHat,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "여름 축제복 모자",
                            tooltip: () => "남자 유아의 여름 축제복 모자",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalSummerHat,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalSummerHat = v,
                            allowedValues: DropdownConfig.FestivalSummerHat,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "여름 축제복",
                            tooltip: () => "남자 유아의 여름 축제복",
                            getValue: () => Config.SpouseConfigs[spouse].BoyFestivalSummerPantsOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyFestivalSummerPantsOptions = v,
                            allowedValues: DropdownConfig.FestivalSummerPantsOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "가을 축제복",
                            tooltip: () => "남자 유아의 가을 축제복",
                            getValue: () => Config.SpouseConfigs[spouse].BoyFestivalFallPants,
                            setValue: v => Config.SpouseConfigs[spouse].BoyFestivalFallPants = v,
                            allowedValues: DropdownConfig.FestivalFallPants,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 축제복 모자",
                            tooltip: () => "남자 유아의 겨울 축제복 모자",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalWinterHat,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalWinterHat = v,
                            allowedValues: DropdownConfig.FestivalWinterHat,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 축제복",
                            tooltip: () => "남자 유아의 겨울 축제복",
                            getValue: () => Config.SpouseConfigs[spouse].BoyFestivalWinterPantsOptions,
                            setValue: v => Config.SpouseConfigs[spouse].BoyFestivalWinterPantsOptions = v,
                            allowedValues: DropdownConfig.FestivalWinterPantsOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 목도리",
                            tooltip: () => "남자 유아의 겨울 축제복 목도리",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalWinterScarf,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalWinterScarf = v,
                            allowedValues: DropdownConfig.FestivalWinterScarf,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddSectionTitle(mod: ModManifest, text: () => "여자 유아", tooltip: null);

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "헤어스타일",
                            tooltip: () => "여자 유아의 헤어스타일",
                            getValue: () => Config.SpouseConfigs[spouse].GirlHairStyles,
                            setValue: v => Config.SpouseConfigs[spouse].GirlHairStyles = v,
                            allowedValues: DropdownConfig.GirlHairStyles,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "눈동자 색상",
                            tooltip: () => "여자 유아의 눈동자 색상",
                            getValue: () => Config.SpouseConfigs[spouse].GirlEyes,
                            setValue: v => Config.SpouseConfigs[spouse].GirlEyes = v,
                            allowedValues: DropdownConfig.GirlEyes,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "피부색",
                            tooltip: () => "여자 유아의 피부색",
                            getValue: () => Config.SpouseConfigs[spouse].GirlSkins,
                            setValue: v => Config.SpouseConfigs[spouse].GirlSkins = v,
                            allowedValues: DropdownConfig.GirlSkins,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "봄 상의",
                            tooltip: () => "여자 유아의 봄 상의",
                            getValue: () => Config.SpouseConfigs[spouse].GirlTopSpringOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlTopSpringOptions = v,
                            allowedValues: DropdownConfig.GirlTopSpringOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "여름 상의",
                            tooltip: () => "여자 유아의 여름 상의",
                            getValue: () => Config.SpouseConfigs[spouse].GirlTopSummerOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlTopSummerOptions = v,
                            allowedValues: DropdownConfig.GirlTopSummerOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "가을 상의",
                            tooltip: () => "여자 유아의 가을 상의",
                            getValue: () => Config.SpouseConfigs[spouse].GirlTopFallOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlTopFallOptions = v,
                            allowedValues: DropdownConfig.GirlTopFallOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 상의",
                            tooltip: () => "여자 유아의 겨울 상의",
                            getValue: () => Config.SpouseConfigs[spouse].GirlTopWinterOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlTopWinterOptions = v,
                            allowedValues: DropdownConfig.GirlTopWinterOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "치마 색상",
                            tooltip: () => "여자 유아의 치마 색상",
                            getValue: () => Config.SpouseConfigs[spouse].SkirtColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].SkirtColorOptions = v,
                            allowedValues: DropdownConfig.SkirtColorOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "신발 색상",
                            tooltip: () => "여자 유아의 신발 색상",
                            getValue: () => Config.SpouseConfigs[spouse].GirlShoesColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlShoesColorOptions = v,
                            allowedValues: DropdownConfig.GirlShoesColorOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "넥칼라 색상",
                            tooltip: () => "여자 유아의 넥칼라 색상",
                            getValue: () => Config.SpouseConfigs[spouse].GirlNeckCollarColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlNeckCollarColorOptions = v,
                            allowedValues: DropdownConfig.GirlNeckCollarColorOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "잠옷 스타일",
                            tooltip: () => "여자 유아의 잠옷 스타일",
                            getValue: () => Config.SpouseConfigs[spouse].GirlPajamaTypeOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlPajamaTypeOptions = v,
                            allowedValues: DropdownConfig.GirlPajamaTypeOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );

                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "잠옷 색상",
                            tooltip: () => "여자 유아의 잠옷 색상",
                            getValue: () => Config.SpouseConfigs[spouse].GirlPajamaColorOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlPajamaColorOptions = v,
                            allowedValues: DropdownConfig.GirlPajamaColorOptions[Config.SpouseConfigs[spouse].GirlPajamaTypeOptions],
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "봄 축제복 모자",
                            tooltip: () => "여자 유아의 봄 축제복 모자",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalSpringHat,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalSpringHat = v,
                            allowedValues: DropdownConfig.FestivalSpringHat,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "여름 축제복 모자",
                            tooltip: () => "여자 유아의 여름 축제복 모자",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalSummerHat,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalSummerHat = v,
                            allowedValues: DropdownConfig.FestivalSummerHat,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "여름 축제복",
                            tooltip: () => "여자 유아의 여름 축제복",
                            getValue: () => Config.SpouseConfigs[spouse].GirlFestivalSummerSkirtOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlFestivalSummerSkirtOptions = v,
                            allowedValues: DropdownConfig.GirlFestivalSummerSkirtOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "가을 축제복",
                            tooltip: () => "여자 유아의 가을 축제복",
                            getValue: () => Config.SpouseConfigs[spouse].GirlFestivalFallSkirts,
                            setValue: v => Config.SpouseConfigs[spouse].GirlFestivalFallSkirts = v,
                            allowedValues: DropdownConfig.FestivalFallSkirts,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 축제복 모자",
                            tooltip: () => "여자 유아의 겨울 축제복 모자",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalWinterHat,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalWinterHat = v,
                            allowedValues: DropdownConfig.FestivalWinterHat,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 축제복",
                            tooltip: () => "여자 유아의 겨울 축제복",
                            getValue: () => Config.SpouseConfigs[spouse].GirlFestivalWinterSkirtOptions,
                            setValue: v => Config.SpouseConfigs[spouse].GirlFestivalWinterSkirtOptions = v,
                            allowedValues: DropdownConfig.GirlFestivalWinterSkirtOptions,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
                        
                        gmcm.AddTextOption(
                            mod: ModManifest,
                            name: () => "겨울 목도리",
                            tooltip: () => "여자 유아의 겨울 축제복 목도리",
                            getValue: () => Config.SpouseConfigs[spouse].FestivalWinterScarf,
                            setValue: v => Config.SpouseConfigs[spouse].FestivalWinterScarf = v,
                            allowedValues: DropdownConfig.FestivalWinterScarf,
                            formatAllowedValue: value => Helper.Translation.Get(value) ?? value
                        );
              // ──────────────────────────────────────────────────────

            // 고정(전역) 옵션 등록 예시
            gmcm.AddBoolOption(
                mod: mod.ModManifest,
                name: () => "모드 전체 활성화",
                tooltip: () => "이 모드를 켜거나 끕니다.",
                getValue: () => ModEntry.Config.EnableMod,
                setValue: v => ModEntry.Config.EnableMod = v
            );
            gmcm.AddBoolOption(
                mod: mod.ModManifest,
                name: () => "잠옷 시스템 활성화",
                tooltip: () => "자녀의 잠옷을 활성화/비활성화합니다.",
                getValue: () => ModEntry.Config.EnablePajama,
                setValue: v => ModEntry.Config.EnablePajama = v
            );
            gmcm.AddBoolOption(
                mod: mod.ModManifest,
                name: () => "축제복 시스템 활성화",
                tooltip: () => "자녀의 축제복 시스템을 활성화/비활성화합니다.",
                getValue: () => ModEntry.Config.EnableFestival,
                setValue: v => ModEntry.Config.EnableFestival = v
            );
            // (전역 옵션 더 추가 가능)
        }
    }

        // 필요시 GMCM 갱신/리셋 등 유틸 함수 추가
        public static void ResetConfig()
        {
            ModEntry.Config = new ModConfig();
            ModEntry.SaveConfig();
        }

        public static void SaveConfig()
        {
            ModEntry.SaveConfig();
        }
    }
}