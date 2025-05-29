using System;
using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// SMAPI IMonitor 기반 커스텀 Logger 유틸리티 (1.6.10+ 실전 대응)
    /// </summary>
    public static class CustomLogger
    {
        private static IMonitor? _monitor;

        /// <summary>
        /// SMAPI IMonitor 지정 (반드시 모드 Entry 등에서 1회 호출)
        /// </summary>
        public static void Init(IMonitor monitor)
        {
            _monitor = monitor;
        }

        /// <summary>
        /// 일반 정보 로그 (Info)
        /// </summary>
        public static void Info(string message)  => _monitor?.Log(message, LogLevel.Info);

        /// <summary>
        /// 경고 로그 (Warn)
        /// </summary>
        public static void Warn(string message)  => _monitor?.Log(message, LogLevel.Warn);

        /// <summary>
        /// 오류 로그 (Error)
        /// </summary>
        public static void Error(string message) => _monitor?.Log(message, LogLevel.Error);

        /// <summary>
        /// 상세 추적 로그 (Trace)
        /// </summary>
        public static void Trace(string message) => _monitor?.Log(message, LogLevel.Trace);

        /// <summary>
        /// 디버그 로그 (Debug)
        /// </summary>
        public static void Debug(string message) => _monitor?.Log(message, LogLevel.Debug);

        /// <summary>
        /// 로그 출력 없이 Init이 누락된 경우 예외를 throw할 수도 있음 (옵션)
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_monitor == null)
                throw new InvalidOperationException("CustomLogger.Init(monitor)를 먼저 호출해야 합니다!");
        }
    }
}