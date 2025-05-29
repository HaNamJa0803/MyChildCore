using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 실전 물량공세 하드코딩형 이벤트 매니저 (누락/꼬임 0%, 성능 < 안정성 최우선)
    /// ─────────────────────────────────────────────
    /// - 모든 이벤트, 모든 상황, 모든 프레임에 외형 동기화 콜백을 중복 등록
    /// - 평상복/축제복/잠옷 등 상태 전환은 반드시 이 콜백 내에서 시간/축제여부 판정
    /// - 수동 핸들러도 팀원 코드에서 추가 가능 (예: Custom 이벤트)
    /// </summary>
    public static class EventManager
    {
        private static readonly List<IDisposable> _handlers = new();
        private static readonly List<Action> _manualHandlers = new();  // 🆕 수동 핸들러 (팀원 추가용)

        private static Action? _appearanceSync;

        /// <summary>
        /// 모든 이벤트/프레임/상황에 콜백 풀커버 등록 (한번에 모든 상황 커버)
        /// </summary>
        public static void HookAll(IModHelper helper, IMonitor monitor, Action appearanceSync)
        {
            _appearanceSync = appearanceSync;

            // 1️⃣ 자동 이벤트 (모든 상황 풀커버)
            AddHandler(helper.Events.GameLoop.SaveLoaded.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.GameLoop.DayStarted.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Player.Warped.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.World.NpcListChanged.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.GameLoop.ReturnedToTitle.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Display.MenuChanged.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Multiplayer.PeerConnected.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Multiplayer.ModMessageReceived.Subscribe((s, e) => FullSync()));

            // 2️⃣ 매프레임/10프레임/1초 중복 적용
            AddHandler(helper.Events.GameLoop.UpdateTicked.Subscribe((s, e) =>
            {
                if (e.IsMultipleOf(1)) FullSync();   // 매프레임
                if (e.IsMultipleOf(10)) FullSync();  // 10프레임마다
                if (e.IsMultipleOf(60)) FullSync();  // 1초마다
            }));

            monitor?.Log("[EventManager] 물량공세! 모든 이벤트/프레임/상황에 외형 동기화 콜백 등록 완료! (시간/축제/잠옷 트리거까지 풀커버, 수동 핸들러 포함)", LogLevel.Alert);
        }

        /// <summary>
        /// 외형 동기화 + 수동 핸들러 동시 실행
        /// </summary>
        private static void FullSync()
        {
            _appearanceSync?.Invoke();

            foreach (var manual in _manualHandlers)
            {
                try { manual?.Invoke(); }
                catch (Exception ex)
                {
                    CustomLogger.Warn($"[EventManager] 수동 핸들러 실행 실패: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 핸들러 등록 (자동 이벤트용)
        /// </summary>
        public static void AddHandler(IDisposable handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        /// <summary>
        /// 수동 핸들러 추가 (팀원 코드에서 직접)
        /// - 예: (Mod)this).Helper.Events.GameLoop.UpdateTicked += MyCustomHandler;
        /// </summary>
        public static void AddManualHandler(Action handler)
        {
            if (handler != null)
                _manualHandlers.Add(handler);
        }

        /// <summary>
        /// 모든 이벤트 핸들러 해제 (테스트/리셋/핫리로드용)
        /// </summary>
        public static void RemoveAll()
        {
            foreach (var handler in _handlers)
                handler?.Dispose();
            _handlers.Clear();
            _manualHandlers.Clear();
        }
    }
}