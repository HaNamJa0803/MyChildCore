using System;
using System.IO;
using MyChildCore;

namespace MyChildCore
{
    /// <summary>
    /// 통합 커스텀 로그 (파일+콘솔+구분)
    /// </summary>
    public static class CustomLogger
    {
        private static string LogDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyChildCoreLogs");
        private static string LogPath =>
            Path.Combine(LogDir, $"Log_{DateTime.Now:yyyyMMdd}.txt");
        private static bool _enabled = true; // 필요시 On/Off

        /// <summary>로그 활성화/비활성화 (true: 로그 출력, false: 기록X)</summary>
        public static void SetEnabled(bool enable) => _enabled = enable;
        public static bool IsEnabled() => _enabled;

        public static void Debug(string msg) => Write($"[DEBUG] {msg}");
        public static void Info(string msg)  => Write($"[INFO] {msg}");
        public static void Warn(string msg)  => Write($"[WARN] {msg}");
        public static void Error(string msg) => Write($"[ERROR] {msg}");
        public static void Fatal(string msg) => Write($"[FATAL] {msg}");

        private static void Write(string msg)
        {
            if (!_enabled) return;
            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            try
            {
                Directory.CreateDirectory(LogDir);
                File.AppendAllText(LogPath, line + Environment.NewLine);
            }
            catch { /* 파일 시스템 오류 무시 */ }
#if DEBUG
            Console.WriteLine(line);
#endif
        }

        /// <summary>
        /// 예외 전체 출력용 (트레이스까지 기록)
        /// </summary>
        public static void Exception(Exception ex, string prefix = "[EXCEPTION]")
        {
            try
            {
                Write($"{prefix} {ex.Message}");
                Write($"{prefix} {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Write($"{prefix} Inner: {ex.InnerException.Message}");
                    Write($"{prefix} InnerStack: {ex.InnerException.StackTrace}");
                }
            }
            catch { /* 무시 */ }
        }
    }
}