using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    public static class EventManager
    {
        private static readonly List<IDisposable> _handlers = new();
        private static readonly List<Action> _manualHandlers = new();  // 수동 핸들러
        private static Action? _appearanceSync;

        public static void HookAll(IModHelper helper, IMonitor monitor, Action appearanceSync)
        {
            _appearanceSync = appearanceSync;

            AddHandler(helper.Events.GameLoop.SaveLoaded.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.GameLoop.DayStarted.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Player.Warped.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.World.NpcListChanged.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.GameLoop.ReturnedToTitle.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Display.MenuChanged.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Multiplayer.PeerConnected.Subscribe((s, e) => FullSync()));
            AddHandler(helper.Events.Multiplayer.ModMessageReceived.Subscribe((s, e) => FullSync()));

            AddHandler(helper.Events.GameLoop.UpdateTicked.Subscribe((s, e) =>
            {
                if (e.IsMultipleOf(1)) FullSync();   // 매프레임
                if (e.IsMultipleOf(10)) FullSync();  // 10프레임마다
                if (e.IsMultipleOf(60)) FullSync();  // 1초마다
            }));

            monitor?.Log("[EventManager] 물량공세! 모든 이벤트/프레임/상황에 외형 동기화 콜백 등록 완료! (시간/축제/잠옷 트리거까지 풀커버, 수동 핸들러 포함)", LogLevel.Alert);
        }

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

        public static void AddHandler(IDisposable handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        public static void AddManualHandler(Action handler)
        {
            if (handler != null)
                _manualHandlers.Add(handler);
        }

        public static void RemoveAll()
        {
            foreach (var handler in _handlers)
                handler?.Dispose();
            _handlers.Clear();
            _manualHandlers.Clear();
        }
    }
}