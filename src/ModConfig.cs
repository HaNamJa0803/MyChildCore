using MyChildCore;
using System;
using System.Collections.Generic;

namespace MyChildCore
{
    /// <summary>
    /// 모드 전체 설정: 글로벌 옵션 + 배우자별 자녀 설정 (유니크 칠드런식 1:1 매핑 + 자동 방어 + 실시간 동기화)
    /// </summary>
    public class ModConfig
    {
        // === [1] 글로벌 옵션 ===
        public bool EnableMod { get; set; } = true;
        public bool EnablePajama { get; set; } = true;
        public bool EnableFestival { get; set; } = true;

        // === [2] 배우자별 자녀 설정 ===
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        /// <summary>
        /// [실시간 보장] 배우자 키가 없으면 자동 생성 후 리턴 (설정 손실 방지)
        /// </summary>
        public SpouseChildConfig GetOrAddSpouseConfig(string spouseName)
        {
            if (string.IsNullOrEmpty(spouseName))
                spouseName = "Default";

            lock (SpouseConfigs)
            {
                if (!SpouseConfigs.TryGetValue(spouseName, out var config) || config == null)
                {
                    SpouseConfigs[spouseName] = new SpouseChildConfig();
                    CustomLogger.Warn($"[ModConfig] 누락된 배우자 키 자동 추가: {spouseName}");
                    OnConfigChanged();
                }
                return SpouseConfigs[spouseName];
            }
        }

        /// <summary>
        /// [실시간 동기화] 모든 배우자 키(리스트 기반) 누락 자동 복구
        /// (이미 값 있으면 절대 덮어쓰지 않음)
        /// </summary>
        public void InitSpouseKeys(IEnumerable<string> spouseNames)
        {
            lock (SpouseConfigs)
            {
                foreach (var spouse in spouseNames)
                {
                    if (!SpouseConfigs.ContainsKey(spouse) || SpouseConfigs[spouse] == null)
                    {
                        SpouseConfigs[spouse] = new SpouseChildConfig();
                        CustomLogger.Warn($"[ModConfig] 누락된 배우자 키 자동 추가: {spouse}");
                        OnConfigChanged();
                    }
                }
            }
        }

        /// <summary>
        /// [외부 설정 로드 후 호출] : 누락된 배우자 키 보완 (절대 덮어쓰지 않음)
        /// </summary>
        public void FixupSpouseKeys()
        {
            InitSpouseKeys(DropdownConfig.SpouseNames);
        }

        /// <summary>
        /// [실시간] 모든 설정 변경/저장 직후 즉시 반영(동기화) 이벤트 트리거
        /// (외부에서 구독 가능)
        /// </summary>
        public event Action ConfigChanged;

        private void OnConfigChanged()
        {
            ConfigChanged?.Invoke();
        }

        /// <summary>
        /// [생성자] - 여기선 SpouseConfigs에 초기값 넣지 않음
        /// </summary>
        public ModConfig() { }
    }
}