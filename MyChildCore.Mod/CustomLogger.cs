using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    public static class CustomLogger
    {
        private static IMonitor _monitor;
        public static void Init(IMonitor monitor) => _monitor = monitor;
        public static void Info(string message) => _monitor?.Log(message, LogLevel.Info);
        public static void Warn(string message) => _monitor?.Log(message, LogLevel.Warn);
        public static void Error(string message) => _monitor?.Log(message, LogLevel.Error);
    }
}