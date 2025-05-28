using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace MyChildCore.Utilities
{
    public static class EventManager
    {
        private static IMonitor Monitor;
        private static IModHelper Helper;
        private static Action OnChildrenChangedCallback;

        public static void Initialize(IModHelper helper, IMonitor monitor, Action onChildrenChanged)
        {
            Helper = helper;
            Monitor = monitor;
            OnChildrenChangedCallback = onChildrenChanged;

            HookEvents();
        }

        private static void HookEvents()
        {
            if (Helper == null || Monitor == null || OnChildrenChangedCallback == null)
            {
                Monitor?.Log("[EventManager] 초기화 실패(Helper/Monitor/Callback 미지정)", LogLevel.Error);
                return;
            }

            Helper.Events.GameLoop.SaveLoaded += (s, e) => OnChildrenChangedCallback();
            Helper.Events.GameLoop.DayStarted += (s, e) => OnChildrenChangedCallback();
            Helper.Events.Player.Warped += (s, e) => OnChildrenChangedCallback();
            Helper.Events.World.NpcListChanged += (s, e) => OnChildrenChangedCallback();

            Monitor.Log("[EventManager] 게임 이벤트 훅 완료", LogLevel.Info);
        }

        public static void TriggerChildrenChanged()
        {
            OnChildrenChangedCallback?.Invoke();
        }
    }
}