using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MyChildCore
{
    /// <summary>
    /// 통합 커스텀 로그 (파일+콘솔+게임 로그+예외처리, 유니크 칠드런식)
    /// + 실시간 동기화/설정변경 이벤트도 자동 기록
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
        public static void Debug(string msg,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
            => Write($"[DEBUG] {msg} ({member}:{Path.GetFileName(file)}:{line})", StardewModdingAPI.LogLevel.Trace);

        public static void Info(string msg,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
            => Write($"[INFO] {msg} ({member}:{Path.GetFileName(file)}:{line})", StardewModdingAPI.LogLevel.Info);

        public static void Warn(string msg,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
            => Write($"[WARN] {msg} ({member}:{Path.GetFileName(file)}:{line})", StardewModdingAPI.LogLevel.Warn);

        public static void Error(string msg,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
            => Write($"[ERROR] {msg} ({member}:{Path.GetFileName(file)}:{line})", StardewModdingAPI.LogLevel.Error);

        public static void Fatal(string msg,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
            => Write($"[FATAL] {msg} ({member}:{Path.GetFileName(file)}:{line})", StardewModdingAPI.LogLevel.Alert);

        private static void Write(string msg, StardewModdingAPI.LogLevel level = StardewModdingAPI.LogLevel.Info)
        {
            if (!_enabled) return;
            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            // 1. 파일
            try
            {
                Directory.CreateDirectory(LogDir);
                File.AppendAllText(LogPath, line + Environment.NewLine);
            }
            catch { /* 파일 시스템 오류 무시 */ }
            // 2. 게임 내 SMAPI 로그
            try
            {
                MyChildCore.ModEntry.Log?.Log(line, level);
            }
            catch { /* SMAPI 로그 실패도 무시 */ }
            // 3. 콘솔(Debug)
#if DEBUG
            try { Console.WriteLine(line); } catch { }
#endif
        }

        /// <summary>
        /// 예외 전체 출력용 (트레이스까지 기록, SMAPI까지)
        /// </summary>
        public static void Exception(Exception ex, string prefix = "[EXCEPTION]",
            [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            try
            {
                Write($"{prefix} {ex.Message} ({member}:{Path.GetFileName(file)}:{line})", StardewModdingAPI.LogLevel.Error);
                Write($"{prefix} {ex.StackTrace}", StardewModdingAPI.LogLevel.Error);
                if (ex.InnerException != null)
                {
                    Write($"{prefix} Inner: {ex.InnerException.Message}", StardewModdingAPI.LogLevel.Error);
                    Write($"{prefix} InnerStack: {ex.InnerException.StackTrace}", StardewModdingAPI.LogLevel.Error);
                }
            }
            catch { /* 무시 */ }
        }

        // =========== 실시간 동기화/옵션 변경시 자동 로그 Hook ===========
        /// <summary>
        /// DataManager/DropdownConfig/핫리로드 등 시스템 변경 이벤트와 연결 (엔트리서 호출)
        /// </summary>
        public static void InitRealtimeHooks()
        {
            // 1. DropdownConfig 등 옵션 값 변경시
            DropdownConfig.OnConfigChanged += (prop) =>
            {
                Info($"[SYNC] DropdownConfig 변경: {prop} (동기화 수행됨)");
            };
            // 2. 데이터/외형/핫리로드 등 동기화 콜백들에도 연결 예시 (선택)
            // DataManager.OnSync += () => Info("[SYNC] DataManager 데이터 동기화 수행됨");
            // 핫리로드/리소스매니저 등에도 추가 가능
        }

        // =========== 로그 파일 자동정리 ===========
        /// <summary>
        /// 지정 일수 이상된 로그 자동 삭제(유지관리)
        /// </summary>
        public static void CleanupOldLogs(int keepDays = 14)
        {
            try
            {
                if (!Directory.Exists(LogDir)) return;
                foreach (var file in Directory.GetFiles(LogDir, "Log_*.txt"))
                {
                    var fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddDays(-keepDays))
                        fi.Delete();
                }
            }
            catch { /* 무시 */ }
        }
    }
}