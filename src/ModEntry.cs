using MyChildCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;
using StardewValley;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MyChildCore
{
    public class ModEntry : Mod
    {
        public static ModConfig Config;
        private static int lastSyncedDay = -1;

        public override void Entry(IModHelper helper)
        {
            Config = new ModConfig();
            Config = helper.ReadConfig<ModConfig>();

            // 이벤트 등록 (항상 내 외형으로 덮어쓰기!)
            helper.Events.Content.AssetRequested += OnAssetRequested;
            EventManager.RegisterEvents(helper);

            Monitor.Log("MyChildCore 모드가 성공적으로 로드되었습니다!", LogLevel.Info);

            // GMCM 연동
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // 게임 이벤트 등록
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;

            // 콘솔 명령 등록
            helper.ConsoleCommands.Add("applychild", "모든 자녀 외형 일괄 적용", ApplyAllChildren);
            helper.ConsoleCommands.Add("backupchild", "자녀 데이터 백업", BackupChildren);
            helper.ConsoleCommands.Add("restorechild", "자녀 백업 복구", RestoreChildren);
        }

        // 1. AssetRequested(항상 내 외형으로 덮어쓰기, 로그도 출력)
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            // 예시: 자녀 외형을 무조건 내 파츠로 교체!
            if (e.NameWithoutLocale.IsEquivalentTo("Characters/Child"))
            {
                e.Edit(asset =>
                {
                    var myTexture = Helper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                    asset.AsImage().PatchImage(myTexture);
                });
                CustomLogger.Info($"[AssetRequested] Characters/Child → 내 이미지로 패치!");
            }
            // 필요하면 다른 경로도 if문으로 추가!
        }

        // 2. GMCM 옵션 등 (생략/요약, 기존 코드 활용)
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null)
            {
                Monitor.Log("GMCM 연동 실패! GMCM 설치 여부 확인", LogLevel.Warn);
                return;
            }
            
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                if (!Config.SpouseConfigs.ContainsKey(spouse))
                {
                    Config.SpouseConfigs[spouse] = new SpouseChildConfig()
                    {
                        BabyHairStyles = "Default",
                        BabyEyes = "Default",
                        BabySkins = "Default",
                        BabyBodies = "Default",
                        BoyHairStyles = "ShortCut",
                        BoyEyes = "Default",
                        BoySkins = "Default",
                        GirlEyes = "Default",
                        GirlSkins = "Default"
                        // 추가로 필요한 파츠도 여기 Default 값으로 세팅!
                    };
                    CustomLogger.Info($"[ConfigInit] 배우자 키 자동 생성: {spouse}");
                }
            }

            gmcm.Register(
                mod: ModManifest,
                reset: delegate { Config = new ModConfig(); },
                save: delegate { }
            );

            // ────────── 글로벌 옵션 3개 ──────────
            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "모드 활성화",
                tooltip: () => "모드를 켭니다.",
                getValue: () => Config.EnableMod,
                setValue: v => Config.EnableMod = v
            );
            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "잠옷 활성화",
                tooltip: () => "잠옷 기능을 켭니다.",
                getValue: () => Config.EnablePajama,
                setValue: v => Config.EnablePajama = v
            );
            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "축제복 활성화",
                tooltip: () => "축제복 기능을 켭니다.",
                getValue: () => Config.EnableFestival,
                setValue: v => Config.EnableFestival = v
            );

            // ────────── 배우자 탭 (AddPageLink) ──────────
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                string pageId = $"spouse_{spouse}";
                gmcm.AddPageLink(
                    mod: ModManifest,
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
                     }
                  }

        // 3. 이벤트별 외형 일괄 적용(필요시 로그)
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            CustomLogger.Info("SaveLoaded: 자녀 외형 일괄 적용 시작");
            DataManager.ApplyAllAppearances(Config);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            CustomLogger.Info("DayStarted: 자녀 외형 일괄 적용 시작");
            DataManager.ApplyAllAppearances(Config);
            lastSyncedDay = Game1.Date.TotalDays;
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (Context.IsMainPlayer && e.IsLocalPlayer)
            {
                CustomLogger.Info("Warped: 자녀 외형 일괄 적용");
                DataManager.ApplyAllAppearances(Config);
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (Context.IsWorldReady)
            {
                CustomLogger.Info("MenuChanged: 자녀 외형 일괄 적용");
                DataManager.ApplyAllAppearances(Config);
            }
        }

        private void OnSaved(object sender, SavedEventArgs e)
        {
            CustomLogger.Info("Saved: 자녀 외형 일괄 적용");
            DataManager.ApplyAllAppearances(Config);
        }

        private void OnOneSecondUpdateTicked(object sender, EventArgs e)
        {
            if (Game1.Date.TotalDays != lastSyncedDay)
            {
                DataManager.SyncFromDisk();
                lastSyncedDay = Game1.Date.TotalDays;
                CustomLogger.Info("외부 저장소와 동기화 완료");
            }
        }

        // 콘솔 명령
        private static void ApplyAllChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] applychild 실행");
            DataManager.ApplyAllAppearances(Config);
        }

        private static void BackupChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] backupchild 실행");
            DataManager.Backup();
        }

        private static void RestoreChildren(string cmd, string[] args)
        {
            CustomLogger.Info("[콘솔] restorechild 실행");
            DataManager.RestoreLatestBackup();
            DataManager.ApplyAllAppearances(Config);
        }

        // GMCM 옵션 변경 콜백
        public static void OnGMCMChanged(string spouse, bool isBoy)
        {
            CustomLogger.Info($"[GMCM] 옵션 변경: {spouse} / {(isBoy ? "Boy" : "Girl")}");
            AppearanceManager.ApplyForGMCMChange(spouse, isBoy, Config);
            DataManager.SaveData(CacheManager.GetChildCache());
        }
    }
}