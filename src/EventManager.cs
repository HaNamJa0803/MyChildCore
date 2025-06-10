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
    /// 모든 이벤트 기반 자녀 외형 동기화/즉시합성+즉시 Sprite 교체 (유니크 칠드런식)
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

        // === 모든 이벤트에서 즉시 할당 ===
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            DataManager.SyncFromDisk();
            ForceAllChildrenAppearance(Config);
        }
        public static void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            ForceAllChildrenAppearance(Config);
        }
        public static void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            int t = Game1.timeOfDay;
            if (t == 600 || t == 1800)
                ForceAllChildrenAppearance(Config);
        }
        public static void OnSaved(object sender, SavedEventArgs e)     => ForceAllChildrenAppearance(Config);
        public static void OnWarped(object sender, WarpedEventArgs e)   => ForceAllChildrenAppearance(Config);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnRenderedActiveMenu(object s, RenderedActiveMenuEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnAssetRequested(object s, AssetRequestedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnReturnedToTitle(object s, ReturnedToTitleEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnUpdateTicked(object s, UpdateTickedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnButtonReleased(object s, ButtonReleasedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnPeerConnected(object s, PeerConnectedEventArgs e) => ForceAllChildrenAppearance(Config);
        public static void OnModMessageReceived(object s, ModMessageReceivedEventArgs e) => ForceAllChildrenAppearance(Config);

        // === 시간 분기 ===
        private static bool IsNightTime() => Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
        private static bool IsSpringFestival()   => Game1.isFestival() && Game1.currentSeason == "spring";
        private static bool IsSummerFestival()   => Game1.isFestival() && Game1.currentSeason == "summer";
        private static bool IsFallFestival()     => Game1.isFestival() && Game1.currentSeason == "fall";
        private static bool IsWinterFestival()   => Game1.isFestival() && Game1.currentSeason == "winter";

        // === 핵심: 자녀 외형 동기화(즉시합성+Sprite 직접 교체) ===
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

                // KEY 보장
                if (!config.SpouseConfigs.ContainsKey(spouseName))
                {
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();
                }

                // 배열에 없는 spouseName은 스킵
                if (!Array.Exists(DropdownConfig.SpouseNames, x => x == spouseName))
                    continue;

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
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                // === Texture2D 직접 합성 & AnimatedSprite 교체 ===
                Texture2D sprite = (child.Age == 0)
                    ? AppearanceManager.GetBabyTexture(child, parts)
                    : AppearanceManager.GetToddlerTexture(child, parts);

                if (sprite != null)
                {
                    // 아기/유아별 Sprite 크기 다름 주의 (필요하면 분기)
                    int width = (child.Age == 0) ? 22 : 16;
                    int height = (child.Age == 0) ? 16 : 32;
                    child.Sprite = new AnimatedSprite(sprite, 0, width, height);
                }

                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }

        // === GMCM에서 호출하는 배우자/성별별 동기화 ===
        public static void SyncChildrenForSpouse(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;
            if (!Array.Exists(DropdownConfig.SpouseNames, x => x == spouse))
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetRealSpouseName(child);
                if (string.IsNullOrEmpty(spouseName))
                    spouseName = "Default";

                if (!config.SpouseConfigs.ContainsKey(spouseName))
                {
                    config.SpouseConfigs[spouseName] = new SpouseChildConfig();
                }
                if (!Array.Exists(DropdownConfig.SpouseNames, x => x == spouseName)) continue;
                if (spouseName != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts = (child.Age == 0)
                    ? PartsManager.GetPartsForBaby(child, config)
                    : PartsManager.GetPartsForChild(child, config);

                if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                {
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                Texture2D sprite = (child.Age == 0)
                    ? AppearanceManager.GetBabyTexture(child, parts)
                    : AppearanceManager.GetToddlerTexture(child, parts);

                if (sprite != null)
                {
                    int width = (child.Age == 0) ? 22 : 16;
                    int height = (child.Age == 0) ? 16 : 32;
                    child.Sprite = new AnimatedSprite(sprite, 0, width, height);
                }

                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }
    }
}