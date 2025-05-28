using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 모드 전용 통합 로거. (SDV 1.6.10+ 호환, 예외 처리/디버그까지)
    /// 반드시 Entry에서 Init 호출 필요!
    /// </summary>
    public static class CustomLogger
    {
        private static IMonitor _monitor;

        /// <summary>
        /// 반드시 Entry(최초 진입점)에서 1회 초기화 필요.
        /// </summary>
        public static void Init(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public static void Info(string message) => _monitor?.Log(message, LogLevel.Info);
        public static void Warn(string message) => _monitor?.Log(message, LogLevel.Warn);
        public static void Error(string message) => _monitor?.Log(message, LogLevel.Error);
        public static void Trace(string message) => _monitor?.Log(message, LogLevel.Trace);
        public static void Debug(string message) => _monitor?.Log(message, LogLevel.Debug);

        /// <summary>
        /// 예외(및 컨텍스트)까지 확실하게 로깅!
        /// </summary>
        public static void Exception(System.Exception ex, string context = "")
        {
            _monitor?.Log($"[EXCEPTION]{(string.IsNullOrWhiteSpace(context) ? "" : $" [{context}]")} {ex}", LogLevel.Error);
        }
    }
}