using MyChildCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyChildCore
{
    /// <summary>
    /// 모든 이벤트 + 실시간 데이터 변경 기반 자녀 외형 동기화/즉시 string 경로로 Sprite 교체 매니저 (.NET 6.0 호환)
    /// </summary>
    public static class EventManager
    {
        private static ModConfig Config => ModEntry.Config;

        public static void RegisterEvents(IModHelper helper)
        {
            // SMAPI 표준 이벤트 등록
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

            // 핫리로드 감시 등록
            helper.Events.GameLoop.SaveLoaded += (s, e) =>
            {
                HotReloadWatcher.StartWatching(DataManager.DataPath, helper, ModEntry.Config);
            };
            helper.Events.GameLoop.ReturnedToTitle += (s, e) =>
            {
                HotReloadWatcher.StopWatching();
            };

            // 실시간 데이터 변경(캐시 등) 알림 구독
            CacheManager.OnChildCacheChanged += (childList) =>
            {
                ForceAllChildrenAppearance(Config);
            };

            // === (1) 외형 합성/교체 발생시 연결 ===
            AppearanceManager.AppearanceChanged += (child, parts, tex) =>
            {
                // 외형 변경시, 즉시 리소스 무효화 및 캐시 갱신
                ResourceManager.InvalidateChildSprite(child?.Name);
                // 필요시 DataManager 등 추가 동기화
                // DataManager.SaveData(CacheManager.GetChildCache());
                CustomLogger.Info($"[EventManager] 외형 합성 이벤트 연동 완료: {child?.Name}");
            };

            // === (2) 파츠 조합 변경시 연결 ===
            PartsManager.OnPartsChanged += (child, parts) =>
            {
                // 파츠 변경시, 해당 자녀 외형 강제 적용
                if (child != null && parts != null)
                {
                    if (child.Age == 0)
                        AppearanceManager.ApplyBabyAppearance(child, parts);
                    else
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                }
            };

            // === (3) 리소스(스프라이트) 변경시 연결 ===
            ResourceManager.OnSpriteChanged += (childName) =>
            {
                // 스프라이트 캐시 변경 시 전체 외형 동기화 (혹은 부분 UI 갱신 등)
                // ForceAllChildrenAppearance(Config); // 전체 적용 필요시 활성화
                CustomLogger.Info($"[EventManager] 리소스(스프라이트) 변경 연동 완료: {childName}");
            };

            ResourceManager.OnAllSpritesInvalidated += () =>
            {
                // 전체 스프라이트 무효화시 전체 외형 동기화
                ForceAllChildrenAppearance(Config);
                CustomLogger.Info("[EventManager] 모든 스프라이트 무효화 이벤트 연동 완료!");
            };
        }

        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            DataManager.SyncFromDisk();
            ForceAllChildrenAppearance(Config);
        }
        public static void OnDayStarted(object sender, DayStartedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            int t = Game1.timeOfDay;
            if (t == 600 || t == 1800)
                ForceAllChildrenAppearance(Config);
        }
        public static void OnSaved(object sender, SavedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnWarped(object sender, WarpedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnRenderedActiveMenu(object s, RenderedActiveMenuEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnAssetRequested(object s, AssetRequestedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnReturnedToTitle(object s, ReturnedToTitleEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnUpdateTicked(object s, UpdateTickedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnButtonReleased(object s, ButtonReleasedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnPeerConnected(object s, PeerConnectedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnModMessageReceived(object s, ModMessageReceivedEventArgs e) => ForceAllChildrenAppearance(Config);

        // 경로 조합 함수 (.NET 6.0)
        public static string BuildSpritePath(Child child, ChildParts parts)
        {
            string spouse = parts?.SpouseName ?? "Default";
            string gender = (child.Gender == 0) ? "Boy" : "Girl";
            string type = (child.Age == 0) ? "Baby" : "Toddler";

            string skin = "body";
            if (child.Age == 0)
                skin = parts?.BabySkins ?? "body";
            else
                skin = (child.Gender == 0) ? (parts?.BoySkins ?? "body") : (parts?.GirlSkins ?? "body");

            string suffix = "";
            if (skin.ToLower().Contains("dark"))
                suffix = "_dark";

            string path = $"assets/{spouse}/{type}/{gender}/{skin}{suffix}.png";
            return path;
        }

        // string 경로로 Sprite 교체
        public static void ApplyChildAppearanceByPath(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;
            string spritePath = BuildSpritePath(child, parts);

            try
            {
                Texture2D spriteTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(spritePath);
                if (spriteTex != null)
                {
                    child.Sprite = new AnimatedSprite(spritePath, 0, 16, 32);
                }
                else
                {
                    CustomLogger.Warn($"[EventManager] 경로 없음: {spritePath}, Default Sprite로 복구");
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[EventManager] Sprite 경로 교체 실패: {spritePath}, {ex.Message}");
            }
        }

        // 메인 진입점
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
                if (string.IsNullOrEmpty(spouseName))
                    spouseName = "Default";

                if (!config.SpouseConfigs.ContainsKey(spouseName))
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();

                if (!Array.Exists(DropdownConfig.SpouseNames, x => x == spouseName))
                {
                    CustomLogger.Warn($"[EventManager] 배우자명 '{spouseName}'은 DropdownConfig에 없음! 외형 적용 스킵.");
                    continue;
                }

                ChildParts parts;
                if (isNight)
                {
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = true;
                    parts.EnableFestival = false;
                }
                else if (isSpringFest)
                {
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = false;
                    parts.EnableFestival = false;
                }
                else if (isSummerFest || isFallFest || isWinterFest)
                {
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = false;
                    parts.EnableFestival = true;
                }
                else
                {
                    parts = (child.Age == 0)
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);
                    parts.EnablePajama = false;
                    parts.EnableFestival = false;
                }

                if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                {
                    CustomLogger.Warn($"[EventManager] 외형 파츠 누락: {child.Name} → Default로 복구!");
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                // === string 경로로 Sprite 교체 ===
                ApplyChildAppearanceByPath(child, parts);

                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }

        // GMCM 옵션에 따라 동기화 (string 경로 기반)
        public static void SyncChildrenForSpouse(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;
            if (!Array.Exists(DropdownConfig.SpouseNames, x => x == spouse))
            {
                CustomLogger.Warn($"[SyncChildrenForSpouse] 배우자명 '{spouse}'은 DropdownConfig에 없음! 외형 적용 스킵.");
                return;
            }

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetRealSpouseName(child);
                if (string.IsNullOrEmpty(spouseName))
                    spouseName = "Default";

                if (!config.SpouseConfigs.ContainsKey(spouseName))
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();

                if (!Array.Exists(DropdownConfig.SpouseNames, x => x == spouseName)) continue;
                if (spouseName != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts = (child.Age == 0)
                    ? PartsManager.GetPartsForBaby(child, config)
                    : PartsManager.GetPartsForChild(child, config);

                if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                {
                    CustomLogger.Warn($"[SyncChildrenForSpouse] 외형 파츠 누락: {(child != null ? child.Name : "null")} (spouse: {spouseName}) → Default로 복구!");
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                ApplyChildAppearanceByPath(child, parts);

                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }

        // 시즌, 밤/낮, 축제 분기
        private static bool IsNightTime() => Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
        private static bool IsSpringFestival()   => Game1.isFestival() && Game1.currentSeason == "spring";
        private static bool IsSummerFestival()   => Game1.isFestival() && Game1.currentSeason == "summer";
        private static bool IsFallFestival()     => Game1.isFestival() && Game1.currentSeason == "fall";
        private static bool IsWinterFestival()   => Game1.isFestival() && Game1.currentSeason == "winter";
    }
}