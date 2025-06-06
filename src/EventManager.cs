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
        // 항상 최신 config 인스턴스 사용 (싱글턴 패턴)
        private static ModConfig Config => ModEntry.Config;

        /// <summary>
        /// 이벤트 등록 (반드시 모드 Entry에서 호출)
        /// </summary>
        public static void RegisterEvents(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded   += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted   += OnDayStarted;
            helper.Events.Player.Warped         += OnWarped;
            helper.Events.Display.MenuChanged   += OnMenuChanged;
            helper.Events.GameLoop.Saved        += OnSaved;
        }

        // 각 이벤트에서 동기화 실행
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)   => SyncAllChildrenAppearance();
        public static void OnDayStarted(object sender, DayStartedEventArgs e)   => SyncAllChildrenAppearance();
        public static void OnWarped(object sender, WarpedEventArgs e)           => SyncAllChildrenAppearance();
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e) => SyncAllChildrenAppearance();
        public static void OnSaved(object sender, SavedEventArgs e)             => SyncAllChildrenAppearance();

        /// <summary>
        /// 모든 자녀의 외형을 최신 설정으로 동기화 (GMCMKey 대응, 아기+유아 모두 반영)
        /// </summary>
        public static void SyncAllChildrenAppearance()
        {
            if (Config == null || !Config.EnableMod || Config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                // GMCMKey 누락 시 자동 추가 (실시간 외형 적용 보장)
                GMCMKeyValidator.FindOrAddKey(child, Config);

                // GMCMKey 기준으로 parts 추출
                ChildParts parts;
                if (child.Age == 0)
                    parts = PartsManager.GetPartsForBaby(child, Config);
                else
                    parts = PartsManager.GetPartsForChild(child, Config);

                if (parts == null)
                {
                    CustomLogger.Warn($"[EventManager] ChildParts 생성 실패: {child?.Name ?? "null"}");
                    continue;
                }

                // 외형 적용
                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, parts);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, parts);
            }
        }

        /// <summary>
        /// GMCM 옵션 변경 후 해당 자녀만 즉시 외형 갱신 (OnGMCMChanged에서 호출)
        /// </summary>
        public static void SyncChildrenForSpouse(string spouse, bool isMale)
        {
            if (Config == null || !Config.EnableMod || Config.SpouseConfigs == null)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string spouseName = AppearanceManager.GetSpouseName(child);
                if (spouseName != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                // GMCMKey 누락 시 자동 추가
                GMCMKeyValidator.FindOrAddKey(child, Config);

                ChildParts parts;
                if (child.Age == 0)
                    parts = PartsManager.GetPartsForBaby(child, Config);
                else
                    parts = PartsManager.GetPartsForChild(child, Config);

                if (parts == null)
                    continue;

                if (child.Age == 0)
                    AppearanceManager.ApplyBabyAppearance(child, parts);
                else
                    AppearanceManager.ApplyToddlerAppearance(child, parts);
            }
        }
    }
}