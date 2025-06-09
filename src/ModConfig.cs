using System;
using System.Collections.Generic;

namespace MyChildCore
{
    /// <summary>
    /// 모드 전체 설정: 글로벌 옵션 + 배우자별 자녀 설정 (유니크 칠드런식 1:1 매핑 + 자동 방어)
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// 모드 전체 ON/OFF (GMCM 옵션)
        /// </summary>
        public bool EnableMod { get; set; } = true;

        /// <summary>
        /// 잠옷 시스템 ON/OFF (GMCM 옵션)
        /// </summary>
        public bool EnablePajama { get; set; } = true;

        /// <summary>
        /// 축제복 시스템 ON/OFF (GMCM 옵션)
        /// </summary>
        public bool EnableFestival { get; set; } = true;

        /// <summary>
        /// 배우자별 자녀 설정 (GMCM Key = 배우자 이름/커스텀키)
        /// </summary>
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        /// <summary>
        /// 안전 방어용: 배우자 키가 없으면 자동 추가해서 리턴
        /// </summary>
        public SpouseChildConfig GetOrAddSpouseConfig(string spouseName)
        {
            if (string.IsNullOrEmpty(spouseName))
                spouseName = "Default";
            if (!SpouseConfigs.TryGetValue(spouseName, out var config) || config == null)
            {
                SpouseConfigs[spouseName] = new SpouseChildConfig();
                CustomLogger.Warn($"[ModConfig] 누락된 배우자 키 자동 추가: {spouseName}");
            }
            return SpouseConfigs[spouseName];
        }

        /// <summary>
        /// 안전 방어용: 대표 배우자/테스트용 모두 1:1 초기화 (선택)
        /// </summary>
        public void InitSpouseKeys(IEnumerable<string> spouseNames)
        {
            foreach (var spouse in spouseNames)
                GetOrAddSpouseConfig(spouse);
        }

        /// <summary>
        /// 생성자: 대표 배우자 키 자동 초기화!
        /// </summary>
        public ModConfig()
        {
            // ★ config가 생성될 때 배우자 키를 무조건 1:1로 미리 다 채워준다!
            InitSpouseKeys(DropdownConfig.SpouseNames);
        }

        // ★★ config.json 등에서 읽은 후에도 꼭 호출해주면 더 안전!
        public void FixupSpouseKeys()
        {
            InitSpouseKeys(DropdownConfig.SpouseNames);
        }
    }
}