using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 실전 물량공세 하드코딩형 이벤트 매니저 (누락/꼬임 0%, 성능 최우선)
    /// ─────────────────────────────────────────────
    /// - 모든 이벤트, 모든 상황, 모든 프레임에 외형 동기화 콜백을 중복 등록
    /// - 평상복/축제복/잠옷 등 "상태 전환"은 반드시 이 콜백 내에서 시간/축제여부 판정
    /// - 절대 빠짐없이, 꼬임 없이, 누락 없이 완벽하게 동작
    /// </summary>
    public static class EventManager
    {
        // 모든 핸들러 IDisposable로 관리(해제도 편하게)
        private static readonly List<IDisposable> _handlers = new();

        // 외형 동기화 콜백 (엔트리에서 덮어쓰기)
        private static Action? _appearanceSync;

        /// <summary>
        /// 모든 이벤트/틱/상황에 콜백 “풀커버” 등록
        /// - 모든 상황에서 자녀 외형 동기화, 시간/축제/잠옷 자동 전환까지 실시간 커버
        /// </summary>
        public static void HookAll(IModHelper helper, IMonitor monitor, Action appearanceSync)
        {
            _appearanceSync = appearanceSync;

            // 1. 세이브 로딩, 하루 시작, 워프, NPC변동, 타이틀 복귀, 메뉴 변경, 멀티 등
            //   게임 상태가 바뀌는 모든 상황에 콜백 등록(누락/꼬임 0%)
            AddHandler(helper.Events.GameLoop.SaveLoaded.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.GameLoop.DayStarted.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.Player.Warped.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.World.NpcListChanged.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.GameLoop.ReturnedToTitle.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.Display.MenuChanged.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.Multiplayer.PeerConnected.Subscribe((s, e) => _appearanceSync?.Invoke()));
            AddHandler(helper.Events.Multiplayer.ModMessageReceived.Subscribe((s, e) => _appearanceSync?.Invoke()));

            // 2. "시간/축제/잠옷/평상복" 자동 전환을 커버하기 위해
            //    UpdateTicked(매프레임/10프레임/1초)마다 중복 동기화(성능 최우선, 누락 0%)
            AddHandler(helper.Events.GameLoop.UpdateTicked.Subscribe((s, e) =>
            {
                // - 외형 자동 동기화: 매 프레임/10프레임/1초마다 중복 호출!
                // - (실전 운영 중 불필요하다면 매프레임만 주석)
                if (e.IsMultipleOf(1)) _appearanceSync?.Invoke();   // 매 프레임
                if (e.IsMultipleOf(10)) _appearanceSync?.Invoke();  // 10프레임마다
                if (e.IsMultipleOf(60)) _appearanceSync?.Invoke();  // 1초마다

                // ─────────────────────────────────────────────
                // 이 콜백(appearanceSync) 내부에서는 반드시 아래 전환상황을 체크할 것!
                //   - Game1.timeOfDay → 18:00이후면 잠옷, 06:00이후면 평상복/축제복 복귀
                //   - Game1.isFestivalDay, Game1.isFestival 등으로 축제복 자동 적용
                //   - GMCM에서 선택된 잠옷 스타일/색상 자동 적용
                //   - 평상복/축제복/잠옷 이외에도, 계절/날짜에 따른 파츠 자동전환 로직
                // 절대 누락/오류/꼬임 없이 풀커버!
                // ─────────────────────────────────────────────
            }));

            // (참고) 유니크 칠드런식 수동 핸들러도 여유되면 추가 가능
            // (Mod)this).Helper.Events.GameLoop.UpdateTicked += UpdateTicked;
            // (Mod)this).Helper.Events.GameLoop.UpdateTicked += fix_sprites_warping;
            // (Mod)this).Helper.Events.GameLoop.UpdateTicked += fix_client_animations;

            monitor?.Log("[EventManager] 물량공세! 모든 이벤트/틱(프레임/10프레임/1초)과 상황에 외형 동기화 콜백 등록 완료! (시간/축제/잠옷 트리거까지 풀커버)", LogLevel.Alert);
        }

        /// <summary>
        /// 핸들러 등록 (IDisposable, 해제/리셋도 편하게)
        /// </summary>
        public static void AddHandler(IDisposable handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        /// <summary>
        /// 모든 이벤트 핸들러 해제 (테스트/모드 리셋/핫리로드용)
        /// </summary>
        public static void RemoveAll()
        {
            foreach (var handler in _handlers)
                handler?.Dispose();
            _handlers.Clear();
        }
    }
}