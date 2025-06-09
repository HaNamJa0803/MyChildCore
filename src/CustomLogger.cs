using System;
using System.IO;

namespace MyChildCore
{
    /// <summary>
    /// 통합 커스텀 로그 (파일+콘솔+게임 로그+예외처리, 유니크 칠드런식)
    /// </summary>
    public static class CustomLogger
    {
        private static string LogDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyChildCoreLogs");
        private static string LogPath =>
            Path.Combine(LogDir, $"Log_{DateTime.Now:yyyyMMdd}.txt");
        private static bool _enabled = true; // 필요시 On/Off

        /// <summary>로그 활성화/비활성화</summary>
        public static void SetEnabled(bool enable) => _enabled = enable;
        public static bool IsEnabled() => _enabled;

        // === 통합 로그 출력 ===
        public static void Debug(string msg) => Write($"[DEBUG] {msg}", StardewModdingAPI.LogLevel.Trace);
        public static void Info(string msg)  => Write($"[INFO] {msg}", StardewModdingAPI.LogLevel.Info);
        public static void Warn(string msg)  => Write($"[WARN] {msg}", StardewModdingAPI.LogLevel.Warn);
        public static void Error(string msg) => Write($"[ERROR] {msg}", StardewModdingAPI.LogLevel.Error);
        public static void Fatal(string msg) => Write($"[FATAL] {msg}", StardewModdingAPI.LogLevel.Alert);

        private static void Write(string msg, StardewModdingAPI.LogLevel level = StardewModdingAPI.LogLevel.Info)
        {
            if (!_enabled) return;
            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            try
            {
                Directory.CreateDirectory(LogDir);
                File.AppendAllText(LogPath, line + Environment.NewLine);
            }
            catch { /* 파일 시스템 오류 무시 */ }
            // 게임 내 SMAPI 로그도 같이 남기기 (엔트리에서 Log 인스턴스 전달 필요)
            try
            {
                if (MyChildCore.ModEntry.Log != null)
                    MyChildCore.ModEntry.Log.Log(line, level);
            }
            catch { /* 게임 로그 실패도 무시 */ }
#if DEBUG
            Console.WriteLine(line);
#endif
        }

        /// <summary>
        /// 예외 전체 출력용 (트레이스까지 기록, SMAPI까지)
        /// </summary>
        public static void Exception(Exception ex, string prefix = "[EXCEPTION]")
        {
            try
            {
                Write($"{prefix} {ex.Message}", StardewModdingAPI.LogLevel.Error);
                Write($"{prefix} {ex.StackTrace}", StardewModdingAPI.LogLevel.Error);
                if (ex.InnerException != null)
                {
                    Write($"{prefix} Inner: {ex.InnerException.Message}", StardewModdingAPI.LogLevel.Error);
                    Write($"{prefix} InnerStack: {ex.InnerException.StackTrace}", StardewModdingAPI.LogLevel.Error);
                }
            }
            catch { /* 무시 */ }
        }
    }
}