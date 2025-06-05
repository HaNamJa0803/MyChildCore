using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 모든 게임 이벤트(저장, 날짜변경, 워프 등)에서 자녀 외형을 자동 동기화하는 매니저
    /// (GMCMKey 기반, 폴리아모리·다자녀 완전 대응)
    /// </summary>
    public static class EventManager
    {
        // 더 이상 정적 Config 참조하지 않음!

        /// <summary>
        /// 이벤트 등록 (반드시 모드 Entry에서 호출)
        /// </summary>
        public static void RegisterEvents(IModHelper helper, ModConfig config)
        {
            // 람다로 config 인자 유지
            helper.Events.GameLoop.SaveLoaded   += (s, e) => OnSaveLoaded(s, e, config);
            helper.Events.GameLoop.DayStarted   += (s, e) => OnDayStarted(s, e, config);
            helper.Events.Player.Warped         += (s, e) => OnWarped(s, e, config);
            helper.Events.Display.MenuChanged   += (s, e) => OnMenuChanged(s, e, config);
            helper.Events.GameLoop.Saved        += (s, e) => OnSaved(s, e, config);
        }

        // 각 이벤트마다 config 인자 전달
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e, ModConfig config)   => SyncAllChildrenAppearance(config);
        public static void OnDayStarted(object sender, DayStartedEventArgs e, ModConfig config)   => SyncAllChildrenAppearance(config);
        public static void OnWarped(object sender, WarpedEventArgs e, ModConfig config)           => SyncAllChildrenAppearance(config);
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e, ModConfig config) => SyncAllChildrenAppearance(config);
        public static void OnSaved(object sender, SavedEventArgs e, ModConfig config)             => SyncAllChildrenAppearance(config);

        /// <summary>
        /// 모든 자녀의 외형을 최신 설정으로 동기화 (GMCMKey 대응, 아기+유아 모두 반영)
        /// </summary>
        public static void SyncAllChildrenAppearance(ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                // GMCMKey 누락 시 자동 추가 (실시간 외형 적용 보장)
                GMCMKeyValidator.FindOrAddKey(child, config);

                // GMCMKey 기준으로 parts 추출
                ChildParts parts;
                if (child.Age == 0)
                    parts = PartsManager.GetPartsForBaby(child, config);
                else
                    parts = PartsManager.GetPartsForChild(child, config);

                if (parts == null)
                {
                    CustomLogger.Warn($"[EventManager] ChildParts 생성 실패: {child?.Name ?? "null"}");
                    continue;
                }

                // 외형 적용 - 항상 config 인자와 함께 전달!
                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, config);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, config);
            }
        }

        /// <summary>
        /// GMCM 옵션 변경 후 해당 자녀만 즉시 외형 갱신 (OnGMCMChanged에서 호출)
        /// </summary>
        public static void SyncChildrenForSpouse(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod || config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetSpouseName(child);
                if (spouseName != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                // GMCMKey 누락 시 자동 추가
                GMCMKeyValidator.FindOrAddKey(child, config);

                ChildParts parts;
                if (child.Age == 0)
                    parts = PartsManager.GetPartsForBaby(child, config);
                else
                    parts = PartsManager.GetPartsForChild(child, config);

                if (parts == null)
                    continue;

                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, config);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, config);
            }
        }
    }
}