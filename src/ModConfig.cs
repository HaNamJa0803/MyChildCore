using MyChildCore;
using System;
using System.Collections.Generic;

namespace MyChildCore
{
    /// <summary>
    /// 모드 전체 설정: 글로벌 옵션 + 배우자별 자녀 설정
    /// + 실시간 GMCM 동기화 연동 (오직 상태/값 보관 책임만)
    /// </summary>
    public class ModConfig
    {
        // === [1] 글로벌 옵션 ===
        public bool EnableMod { get; set; } = true;
        public bool EnablePajama { get; set; } = true;
        public bool EnableFestival { get; set; } = true;

        // === [2] 배우자별 자녀 설정 ===
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        // === [3] 이벤트/옵저버 ===
        public event Action ConfigChanged;

        public ModConfig()
        {
            // GMCM 연동/실시간 동기화 시 ALWAYS!
            ConfigChanged += () =>
            {
                // GMCM에서 옵션 추가/수정 시점에 반드시 호출됨!
                InitSpouseKeys(DropdownConfig.SpouseNames);
            };
        }

        /// <summary>
        /// 누락 배우자만 조용히 추가 (절대 덮어쓰기 X)
        /// </summary>
        public void InitSpouseKeys(IEnumerable<string> spouseNames)
        {
            lock (SpouseConfigs)
            {
                foreach (var spouse in spouseNames)
                {
                    if (!SpouseConfigs.ContainsKey(spouse) || SpouseConfigs[spouse] == null)
                        SpouseConfigs[spouse] = new SpouseChildConfig();
                }
            }
        }

        /// <summary>
        /// 세이브 로드시 또는 외부 설정 반영 직후
        /// </summary>
        public void FixupSpouseKeys(IEnumerable<string> actualSpouseNames)
        {
            InitSpouseKeys(actualSpouseNames);
        }

        /// <summary>
        /// [실시간 보장] 배우자 키가 없으면 null만 (자동 추가 X)
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

        /// <summary>
        /// GMCM에서 옵션이 실제로 바뀌었을 때 호출: 
        /// 반드시 엔트리/이벤트/핫리로드에서 OnConfigChanged() 호출!
        /// </summary>
        public void OnConfigChanged()
        {
            ConfigChanged?.Invoke();
        }
    }
}