using MyChildCore;
using System;
using System.Collections.Generic;

namespace MyChildCore
{
    /// <summary>
    /// 모드 전체 설정: 글로벌 옵션 + 배우자별 자녀 설정 (유니크 칠드런식 1:1 매핑 + 동적 동기화)
    /// </summary>
    public class ModConfig
    {
        public bool EnableMod { get; set; } = true;
        public bool EnablePajama { get; set; } = true;
        public bool EnableFestival { get; set; } = true;

        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        public ModConfig() { }

        /// <summary>
        /// SaveLoaded 등에서 "실제 배우자명" 목록을 전달받아 동기화(누락만 동적 추가)
        /// </summary>
        public void FixupSpouseKeys(IEnumerable<string> actualSpouseNames)
        {
            lock (SpouseConfigs)
            {
                foreach (var spouse in actualSpouseNames)
                {
                    if (!SpouseConfigs.ContainsKey(spouse) || SpouseConfigs[spouse] == null)
                    {
                        SpouseConfigs[spouse] = new SpouseChildConfig();
                        // 경고 로그도 제거!
                    }
                }
            }
        }

        public SpouseChildConfig GetOrAddSpouseConfig(string spouseName)
        {
            if (string.IsNullOrEmpty(spouseName))
                spouseName = "Default";
            lock (SpouseConfigs)
            {
                if (!SpouseConfigs.TryGetValue(spouseName, out var config) || config == null)
                {
                    SpouseConfigs[spouseName] = new SpouseChildConfig();
                }
                return SpouseConfigs[spouseName];
            }
        }
    }
}