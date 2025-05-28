using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using System;

namespace MyChildCore.Utilities
{
    public static class EventManager
    {
        private static IMonitor Monitor;
        private static IModHelper Helper;
        private static Action OnChildrenChangedCallback; // 확장: GMCM 등 자동화 트리거

        /// <summary>
        /// 이벤트 등록 (초기화)
        /// </summary>
        public static void Initialize(IModHelper helper, IMonitor monitor, Action onChildrenChanged)
        {
            Helper = helper;
            Monitor = monitor;
            OnChildrenChangedCallback = onChildrenChanged;

            HookEvents();
        }

        /// <summary>
        /// 주요 게임 이벤트에 콜백 등록
        /// </summary>
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
            // GMCM 옵션 변경/자동화 트리거 등 추가 이벤트 확장 가능

            Monitor.Log("[EventManager] 게임 이벤트 훅 완료", LogLevel.Info);
        }

        /// <summary>
        /// 필요시 수동으로 콜백 실행(외부 호출)
        /// </summary>
        public static void TriggerChildrenChanged()
        {
            OnChildrenChangedCallback?.Invoke();
        }
    }
}