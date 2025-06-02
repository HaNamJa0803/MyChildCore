using System;
using System.IO;
using MyChildCore;

namespace MyChildCore.Utilities
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

        public static void Info(string msg)  => Write($"[INFO] {msg}");
        public static void Warn(string msg)  => Write($"[WARN] {msg}");
        public static void Error(string msg) => Write($"[ERROR] {msg}");

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
    }
}