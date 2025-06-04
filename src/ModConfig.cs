using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 모드 전체 설정: 글로벌 옵션 + 배우자별 자녀 설정(1:1 매핑)
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// 모드 전체 ON/OFF (필수 아님, 사용 시 GMCM 옵션 추가)
        /// </summary>
        public bool EnableMod { get; set; } = true;

        /// <summary>
        /// 잠옷 시스템 ON/OFF (GMCM 옵션에서 연결)
        /// </summary>
        public bool EnablePajama { get; set; } = true;

        /// <summary>
        /// 축제복 시스템 ON/OFF (GMCM 옵션에서 연결)
        /// </summary>
        public bool EnableFestival { get; set; } = true;

        /// <summary>
        /// 배우자별 자녀 설정 (GMCM Key = 배우자이름/커스텀키)
        /// </summary>
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        /// <summary>
        /// 생성자: 기본값 자동 할당 (선택)
        /// </summary>
        public ModConfig()
        {
            // (옵션) 기본적으로 대표 배우자 키 추가 - 필요 시 초기화 로직 작성
            // ex)
            // SpouseConfigs["Abigail"] = new SpouseChildConfig();
        }
    }
}