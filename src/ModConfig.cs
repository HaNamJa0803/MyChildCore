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
        /// (값이 이미 있으면 절대 덮어쓰지 않음!)
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
        /// (값이 이미 있으면 절대 덮어쓰지 않음!)
        /// </summary>
        public void InitSpouseKeys(IEnumerable<string> spouseNames)
        {
            foreach (var spouse in spouseNames)
            {
                // 값이 없을 때만 새로 추가
                if (!SpouseConfigs.ContainsKey(spouse) || SpouseConfigs[spouse] == null)
                {
                    SpouseConfigs[spouse] = new SpouseChildConfig();
                    CustomLogger.Warn($"[ModConfig] 누락된 배우자 키 자동 추가: {spouse}");
                }
            }
        }

        /// <summary>
        /// 생성자: 절대 여기서는 InitSpouseKeys를 호출하지 않는다!
        /// (외부저장소/파일에서 읽어오고 난 뒤에만 InitSpouseKeys 사용)
        /// </summary>
        public ModConfig()
        {
            // *** 주의: 생성자에서는 SpouseConfigs 초기화 금지!
            // 외부저장소 불러오기 전에 여기서 채우면 커스텀 값이 다 덮어씀
        }

        /// <summary>
        /// 외부저장소에서 읽은 후, 누락된 배우자 키만 안전하게 추가 (값 있는 경우 절대 덮어쓰지 않음)
        /// </summary>
        public void FixupSpouseKeys()
        {
            InitSpouseKeys(DropdownConfig.SpouseNames);
        }
    }
}