using System;
using StardewModdingAPI;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// SMAPI IMonitor 기반 커스텀 Logger 유틸리티 (1.6.10+ 실전 대응)
    /// - 단일 진입점: _monitor 하나만 관리 (다른 로그 시스템 사용 X)
    /// - 반드시 모드 Entry에서 Init() 호출 필수 (명시적 초기화)
    /// - 로그 레벨별 출력 메서드 제공 (Info, Warn, Error, Trace, Debug)
    /// </summary>
    public static class CustomLogger
    {
        /// <summary>
        /// 내부 IMonitor 참조 (단일 인스턴스)
        /// </summary>
        private static IMonitor? _monitor;

        /// <summary>
        /// SMAPI IMonitor 지정 (반드시 1회 호출, 보통 Entry.cs에서)
        /// </summary>
        /// <param name="monitor">SMAPI IMonitor 인스턴스</param>
        public static void Init(IMonitor monitor)
        {
            _monitor = monitor ?? throw new ArgumentNullException(nameof(monitor), "CustomLogger Init 실패: monitor는 null일 수 없음");
        }

        /// <summary>
        /// 일반 정보 로그 (LogLevel.Info)
        /// </summary>
        public static void Info(string message)  => _monitor?.Log(message, LogLevel.Info);

        /// <summary>
        /// 경고 로그 (LogLevel.Warn)
        /// </summary>
        public static void Warn(string message)  => _monitor?.Log(message, LogLevel.Warn);

        /// <summary>
        /// 오류 로그 (LogLevel.Error)
        /// </summary>
        public static void Error(string message) => _monitor?.Log(message, LogLevel.Error);

        /// <summary>
        /// 상세 추적 로그 (LogLevel.Trace)
        /// </summary>
        public static void Trace(string message) => _monitor?.Log(message, LogLevel.Trace);

        /// <summary>
        /// 디버그 로그 (LogLevel.Debug)
        /// </summary>
        public static void Debug(string message) => _monitor?.Log(message, LogLevel.Debug);

        /// <summary>
        /// Init이 호출되지 않았을 경우 예외 발생 (의도적으로 NullReference 방지)
        /// - 팀원에게 Init 필수 호출을 명확히 알리기 위한 안전장치
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_monitor == null)
                throw new InvalidOperationException("CustomLogger.Init(monitor)를 먼저 호출해야 합니다!");
        }
    }
}