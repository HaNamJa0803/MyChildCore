using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// SMAPI 4.x+ 전용 이벤트 핸들러 관리 유틸리티.
    /// Subscribe 반환 IDisposable 핸들러를 안전하게 일괄 관리/해제!
    /// </summary>
    public static class EventManager
    {
        private static readonly List<IDisposable> _handlers = new();

        /// <summary>
        /// 이벤트 핸들러(IDisposable) 등록 (helper.Events.XXX.Subscribe 등)
        /// </summary>
        public static void AddHandler(IDisposable handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        /// <summary>
        /// 모든 핸들러 Dispose 및 리스트 비움 (해제)
        /// </summary>
        public static void RemoveAll()
        {
            foreach (var handler in _handlers)
                handler?.Dispose();
            _handlers.Clear();
        }
    }
}