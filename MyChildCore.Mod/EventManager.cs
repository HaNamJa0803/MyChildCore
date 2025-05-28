using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀/월드/GMCM/게임 상태 변화 등 일괄 이벤트 후킹 유틸.
    /// 반드시 Entry에서 Initialize 호출!
    /// </summary>
    public static class EventManager
    {
        private static IMonitor Monitor;
        private static IModHelper Helper;
        private static Action OnChildrenChangedCallback;

        /// <summary>
        /// (Entry 진입점에서 1회) 이벤트 등록/콜백 지정
        /// </summary>
        public static void Initialize(IModHelper helper, IMonitor monitor, Action onChildrenChanged)
        {
            Helper = helper;
            Monitor = monitor;
            OnChildrenChangedCallback = onChildrenChanged;
            HookEvents();
        }

        /// <summary>
        /// 주요 게임 이벤트에 콜백 연결
        /// </summary>
        private static void HookEvents()
        {
            if (Helper == null || Monitor == null || OnChildrenChangedCallback == null)
            {
                Monitor?.Log("[EventManager] 초기화 실패 (Helper/Monitor/Callback 필요)", LogLevel.Error);
                return;
            }

            Helper.Events.GameLoop.SaveLoaded += (_, _) => OnChildrenChangedCallback();
            Helper.Events.GameLoop.DayStarted += (_, _) => OnChildrenChangedCallback();
            Helper.Events.Player.Warped += (_, _) => OnChildrenChangedCallback();
            Helper.Events.World.NpcListChanged += (_, _) => OnChildrenChangedCallback();
            // GMCM OptionChanged, 커스텀 이벤트 등 자유 확장

            Monitor.Log("[EventManager] 게임 이벤트 후킹 완료!", LogLevel.Info);
        }

        /// <summary>
        /// 외부에서 임의로 자녀 변화 트리거 가능
        /// </summary>
        public static void TriggerChildrenChanged()
        {
            OnChildrenChangedCallback?.Invoke();
        }
    }
}