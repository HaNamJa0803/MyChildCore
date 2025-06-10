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

        public ModConfig()
        {
            ConfigChanged += () =>
            {
                // 추가만 하고, 경고/로그 없음. 이미 있으면 무시!
                InitSpouseKeys(DropdownConfig.SpouseNames);
            };
        }

        public event Action ConfigChanged;
        private void OnConfigChanged()
        {
            ConfigChanged?.Invoke();
        }

        /// <summary>
        /// 없는 배우자명은 무시, 이미 있으면 절대 덮어쓰지 않음!
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
                        // 누락키 경고/로그 없이 조용히 추가만!
                    }
                }
            }
        }

        /// <summary>
        /// [외부 설정 로드 후, SaveLoaded 이벤트에서 호출!]  
        /// 실제 자녀/배우자 정보가 준비된 후에만 반드시 호출!
        /// </summary>
        public void FixupSpouseKeys(IEnumerable<string> actualSpouseNames)
        {
            InitSpouseKeys(actualSpouseNames);
        }

        /// <summary>
        /// [실시간 보장] 배우자 키가 없으면 null만 리턴 (자동 추가 안함)
        /// </summary>
        public SpouseChildConfig GetSpouseConfig(string spouseName)
        {
            if (string.IsNullOrEmpty(spouseName))
                spouseName = "Default";

            lock (SpouseConfigs)
            {
                SpouseConfigs.TryGetValue(spouseName, out var config);
                return config;
            }
        }
    }
}