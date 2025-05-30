using System;
using System.Linq;
using StardewModdingAPI;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 일관된 로깅 및 빠른 디버깅용 커스텀 로거 (SMAPI Log 연동, 실전 하드코딩)
    /// </summary>
    public static class CustomLogger
    {
        public static IMonitor Monitor;

        /// <summary>
        /// SMAPI에서 모니터 인스턴스를 등록(Entry에서 반드시 할당!)
        /// </summary>
        public static void Init(IMonitor monitor)
        {
            Monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
        }

        public static void Info(string msg)
        {
            Monitor?.Log(msg, LogLevel.Info);
        }

        public static void Warn(string msg)
        {
            Monitor?.Log(msg, LogLevel.Warn);
        }

        public static void Error(string msg)
        {
            Monitor?.Log(msg, LogLevel.Error);
        }

        public static void Debug(string msg)
        {
#if DEBUG
            Monitor?.Log("[DEBUG] " + msg, LogLevel.Debug);
#endif
        }
    }
}