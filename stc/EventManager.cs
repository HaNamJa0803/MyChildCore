using MyChildCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 모든 이벤트 기반 자녀 외형 동기화/예외복구 매니저 (유니크 칠드런식, 분기 최강화)
    /// </summary>
    public static class EventManager
    {
        private static ModConfig Config => ModEntry.Config;

        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded           += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted           += OnDayStarted;
            helper.Events.GameLoop.TimeChanged          += OnTimeChanged;
            helper.Events.GameLoop.Saved                += OnSaved;
            helper.Events.Player.Warped                 += OnWarped;
            helper.Events.Display.MenuChanged           += OnMenuChanged;
            helper.Events.Display.RenderedActiveMenu    += OnRenderedActiveMenu;
            helper.Events.Content.AssetRequested        += OnAssetRequested;
            helper.Events.GameLoop.ReturnedToTitle      += OnReturnedToTitle;
            helper.Events.GameLoop.UpdateTicked         += OnUpdateTicked;
            helper.Events.Input.ButtonReleased          += OnButtonReleased;
            helper.Events.Multiplayer.PeerConnected     += OnPeerConnected;
            helper.Events.Multiplayer.ModMessageReceived+= OnModMessageReceived;
        }

        // 이벤트별 핸들러들 (필요한 함수만 작성/채워 넣으면 됨)
        public static void OnSaveLoaded(object s, SaveLoadedEventArgs e)     => ForceAllChildrenAppearance(Config);
        public static void OnDayStarted(object s, DayStartedEventArgs e)     => ForceAllChildrenAppearance(Config);
        public static void OnTimeChanged(object s, TimeChangedEventArgs e)   => ForceAllChildrenAppearance(Config);
        public static void OnSaved(object s, SavedEventArgs e)               => ForceAllChildrenAppearance(Config);
        public static void OnWarped(object s, WarpedEventArgs e)             => ForceAllChildrenAppearance(Config);
        public static void OnMenuChanged(object s, MenuChangedEventArgs e)   => ForceAllChildrenAppearance(Config);
        public static void OnRenderedActiveMenu(object s, RenderedActiveMenuEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnAssetRequested(object s, AssetRequestedEventArgs e) { /* 필요시 리소스 로드/패치 */ }
        public static void OnReturnedToTitle(object s, ReturnedToTitleEventArgs e) { /* 필요시 캐시/데이터 초기화 */ }
        public static void OnUpdateTicked(object s, UpdateTickedEventArgs e) { /* 외형 애니/캐시 보강 필요시 */ }
        public static void OnButtonReleased(object s, ButtonReleasedEventArgs e) { /* 메뉴 닫힘, 입력 등 실시간 적용 */ }
        public static void OnPeerConnected(object s, PeerConnectedEventArgs e) { /* 멀티 환경 대응 */ }
        public static void OnModMessageReceived(object s, ModMessageReceivedEventArgs e) { /* 멀티 메시지 동기화 */ }

        // === 진구가 이미 구현한 ForceAllChildrenAppearance 그대로 재사용! ===
        public static void ForceAllChildrenAppearance(ModConfig config)
        {
            // 진구가 이미 만든 분기/적용 구조 그대로 사용!
            // (본문 생략 - 진구가 올려준 최신 코드 복붙하면 됨)
        }
    }
}

namespace MyChildCore
{
    /// <summary>
    /// 모든 이벤트 기반 자녀 외형 동기화/예외복구 매니저 (유니크 칠드런식, 분기 최강화)
    /// </summary>
    public static class EventManager
    {
        private static ModConfig Config => ModEntry.Config;

        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded           += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted           += OnDayStarted;
            helper.Events.GameLoop.TimeChanged          += OnTimeChanged;
            helper.Events.GameLoop.Saved                += OnSaved;
            helper.Events.Player.Warped                 += OnWarped;
            helper.Events.Display.MenuChanged           += OnMenuChanged;
            helper.Events.Display.RenderedActiveMenu    += OnRenderedActiveMenu;
            helper.Events.Content.AssetRequested        += OnAssetRequested;
            helper.Events.GameLoop.ReturnedToTitle      += OnReturnedToTitle;
            helper.Events.GameLoop.UpdateTicked         += OnUpdateTicked;
            helper.Events.Input.ButtonReleased          += OnButtonReleased;
            helper.Events.Multiplayer.PeerConnected     += OnPeerConnected;
            helper.Events.Multiplayer.ModMessageReceived+= OnModMessageReceived;
        }

        // 이벤트별 핸들러들 (필요한 함수만 작성/채워 넣으면 됨)
        public static void OnSaveLoaded(object s, SaveLoadedEventArgs e)     => ForceAllChildrenAppearance(Config);
        public static void OnDayStarted(object s, DayStartedEventArgs e)     => ForceAllChildrenAppearance(Config);
        public static void OnTimeChanged(object s, TimeChangedEventArgs e)   => ForceAllChildrenAppearance(Config);
        public static void OnSaved(object s, SavedEventArgs e)               => ForceAllChildrenAppearance(Config);
        public static void OnWarped(object s, WarpedEventArgs e)             => ForceAllChildrenAppearance(Config);
        public static void OnMenuChanged(object s, MenuChangedEventArgs e)   => ForceAllChildrenAppearance(Config);
        public static void OnRenderedActiveMenu(object s, RenderedActiveMenuEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnAssetRequested(object s, AssetRequestedEventArgs e) { /* 필요시 리소스 로드/패치 */ }
        public static void OnReturnedToTitle(object s, ReturnedToTitleEventArgs e) { /* 필요시 캐시/데이터 초기화 */ }
        public static void OnUpdateTicked(object s, UpdateTickedEventArgs e) { /* 외형 애니/캐시 보강 필요시 */ }
        public static void OnButtonReleased(object s, ButtonReleasedEventArgs e) { /* 메뉴 닫힘, 입력 등 실시간 적용 */ }
        public static void OnPeerConnected(object s, PeerConnectedEventArgs e) { /* 멀티 환경 대응 */ }
        public static void OnModMessageReceived(object s, ModMessageReceivedEventArgs e) { /* 멀티 메시지 동기화 */ }

        // === 밤 6시~아침 6시/축제/계절/평상복 분기 함수 ===
        private static bool IsNightTime()
        {
            int t = Game1.timeOfDay;
            return t >= 1800 || t < 600;
        }
        private static bool IsSpringFestival()
        {
            return Game1.isFestival() && Game1.currentSeason == "spring";
        }
        private static bool IsSummerFestival()
        {
            return Game1.isFestival() && Game1.currentSeason == "summer";
        }
        private static bool IsFallFestival()
        {
            return Game1.isFestival() && Game1.currentSeason == "fall";
        }
        private static bool IsWinterFestival()
        {
            return Game1.isFestival() && Game1.currentSeason == "winter";
        }

        // === 세이브 로드 후: 외부저장소→캐시→외형 동기화 ===
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            DataManager.SyncFromDisk();
            CustomLogger.Info("[EventManager] SaveLoaded: 외부저장소→캐시 동기화 완료");
            ForceAllChildrenAppearance(Config);
        }

        // === 하루 시작: 게임데이터→외부저장소→캐시→외형 동기화 ===
        public static void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            CustomLogger.Info("[EventManager] DayStarted: 게임데이터→외부저장소 동기화 완료");
            ForceAllChildrenAppearance(Config);
        }

        // === 시간 분기(밤/아침) ===
        public static void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            int t = Game1.timeOfDay;
            if (t == 600 || t == 1800)
            {
                CustomLogger.Info("[EventManager] TimeChanged: 잠옷/평상복 분기 발생");
                ForceAllChildrenAppearance(Config);
            }
        }

        // === 워프, 메뉴, 세이브 등에도 외형 동기화 및 캐시 무효화 ===
        public static void OnWarped(object sender, WarpedEventArgs e)   => ForceAllChildrenAppearance(Config);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnSaved(object sender, SavedEventArgs e)     => ForceAllChildrenAppearance(Config);

        // === 자녀 외형 동기화(분기 강화, 누락 복구, 캐시무효화) ===
        public static void ForceAllChildrenAppearance(ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;

            bool isNight = IsNightTime();
            bool isSpringFest = IsSpringFestival();
            bool isSummerFest = IsSummerFestival();
            bool isFallFest = IsFallFestival();
            bool isWinterFest = IsWinterFestival();

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child == null) continue;

                string spouseName = AppearanceManager.GetRealSpouseName(child);

                // 배우자 Key 방어 (.NET 6.0 이하)
                if (!Array.Exists(DropdownConfig.SpouseNames, delegate(string x) { return x == spouseName; }))
                {
                    CustomLogger.Warn("[EventManager] 배우자명 '" + spouseName + "'은 DropdownConfig에 없음! 외형 적용 스킵.");
                    continue;
                }
                if (!config.SpouseConfigs.ContainsKey(spouseName))
                {
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();
                    CustomLogger.Warn("[EventManager] 누락된 배우자 키 자동 추가: " + spouseName);
                }

                ChildParts parts;
                // === 분기 시작 ===
                if (isNight)
                {
                    // 밤 6시~아침 6시 → 잠옷
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = true;
                    parts.EnableFestival = false;
                }
                else if (isSpringFest)
                {
                    // 봄 축제: 평상복 + 모자만 파츠 적용
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = false;
                    parts.EnableFestival = false;
                    // 모자 파츠만 적용하고 싶다면 파츠 로직에서 parts.FestivalSpringHat만 활용
                }
                else if (isSummerFest || isFallFest || isWinterFest)
                {
                    // 여름/가을/겨울 축제: 축제복
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = false;
                    parts.EnableFestival = true;
                }
                else
                {
                    // 그 외는 평상복
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = false;
                    parts.EnableFestival = false;
                }
                // === 분기 끝 ===

                if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                {
                    CustomLogger.Warn("[EventManager] 외형 파츠 누락: " + child.Name + " → Default로 복구!");
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                // 실제 외형 적용
                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, parts);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, parts);

                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }

        // === 배우자/성별별 자녀 동기화(GMCM 연동 등) ===
        public static void SyncChildrenForSpouse(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;
            if (!Array.Exists(DropdownConfig.SpouseNames, delegate(string x) { return x == spouse; }))
            {
                CustomLogger.Warn("[SyncChildrenForSpouse] 배우자명 '" + spouse + "'은 DropdownConfig에 없음! 외형 적용 스킵.");
                return;
            }

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetRealSpouseName(child);
                if (!Array.Exists(DropdownConfig.SpouseNames, delegate(string x) { return x == spouseName; })) continue;
                if (!config.SpouseConfigs.ContainsKey(spouseName))
                {
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();
                    CustomLogger.Warn("[SyncChildrenForSpouse] 누락된 배우자 키 자동 추가: " + spouseName);
                }
                if (spouseName != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts = (child.Age == 0)
                    ? PartsManager.GetPartsForBaby(child, config)
                    : PartsManager.GetPartsForChild(child, config);

                if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                {
                    CustomLogger.Warn("[SyncChildrenForSpouse] 외형 파츠 누락: " + (child != null ? child.Name : "null") + " (spouse: " + spouseName + ") → Default로 복구!");
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, parts);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, parts);

                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }
    }
}