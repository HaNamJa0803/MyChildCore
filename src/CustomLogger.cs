using System;
using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    public static class CustomLogger
    {
        private static IMonitor? _monitor;

        public static void Init(IMonitor monitor)
        {
            _monitor = monitor ?? throw new ArgumentNullException(nameof(monitor), "CustomLogger Init 실패: monitor는 null일 수 없음");
        }

        public static void Info(string message)  => _monitor?.Log(message, LogLevel.Info);
        public static void Warn(string message)  => _monitor?.Log(message, LogLevel.Warn);
        public static void Error(string message) => _monitor?.Log(message, LogLevel.Error);
        public static void Trace(string message) => _monitor?.Log(message, LogLevel.Trace);
        public static void Debug(string message) => _monitor?.Log(message, LogLevel.Debug);

        public static void EnsureInitialized()
        {
            if (_monitor == null)
                throw new InvalidOperationException("CustomLogger.Init(monitor)를 먼저 호출해야 합니다!");
        }
    }
}