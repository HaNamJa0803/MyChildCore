using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// SMAPI 이벤트 일괄 등록/해제 및 후킹 유틸리티.
    /// 다양한 콜백을 리스트업해서 관리/실행 가능!
    /// </summary>
    public static class EventManager
    {
        private static readonly List<IDisposable> _handlers = new List<IDisposable>();

        /// <summary>
        /// 이벤트 등록(콜백 핸들러 등록) 및 내부 리스트에 저장.
        /// </summary>
        public static void AddHandler(IDisposable handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        /// <summary>
        /// (예시) 등록된 모든 이벤트 핸들러 일괄 해제.
        /// </summary>
        public static void RemoveAll()
        {
            foreach (var handler in _handlers)
                handler?.Dispose();
            _handlers.Clear();
        }
    }
}