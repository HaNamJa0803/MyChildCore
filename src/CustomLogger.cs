using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 모드 전용 커스텀 로그 매니저 (SMAPI 로그에만 기록)
    /// </summary>
    public static class CustomLogger
    {
        private static IMonitor _monitor;

        /// <summary>
        /// SMAPI 모니터 인스턴스 세팅 (최초 1회)
        /// </summary>
        public static void Init(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public static void Info(string message)
        {
            _monitor?.Log(message, LogLevel.Info);
        }

        public static void Warn(string message)
        {
            _monitor?.Log(message, LogLevel.Warn);
        }

        public static void Error(string message)
        {
            _monitor?.Log(message, LogLevel.Error);
        }

        public static void Debug(string message)
        {
            _monitor?.Log(message, LogLevel.Debug);
        }
    }
}