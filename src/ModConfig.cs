using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using GenericModConfigMenu;

namespace MyChildCore
{
    /// <summary>
    /// 모드 전체 설정: 글로벌 옵션 + 배우자별 자녀 설정(1:1 매핑)
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
        /// 배우자별 자녀 설정 (GMCM Key = 배우자이름/커스텀키)
        /// </summary>
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        /// <summary>
        /// 생성자: 필요 시 기본값 초기화
        /// </summary>
        public ModConfig()
        {
            // 여기서 대표 배우자 미리 추가 가능!
            // SpouseConfigs["Abigail"] = new SpouseChildConfig();
            // SpouseConfigs["Sebastian"] = new SpouseChildConfig();
        }
    }
}