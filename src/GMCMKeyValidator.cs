using StardewValley.Characters;
using System.Collections.Generic;
using MyChildCore;

namespace MyChildCore
{
    /// <summary>
    /// GMCMKey 유효성 검사 및 자동 생성/복구 유틸리티
    /// </summary>
    public static class GMCMKeyValidator
    {
        /// <summary>
        /// 해당 자녀 키가 설정에 존재하는지 확인 (존재하면 true)
        /// </summary>
        public static bool IsValidKey(string gmcmKey, DropdownConfig config)
        {
            if (string.IsNullOrEmpty(gmcmKey) || config == null || config.SpouseConfigs == null)
                return false;
            return config.SpouseConfigs.ContainsKey(gmcmKey);
        }

        /// <summary>
        /// 해당 자녀의 키가 없으면 추가(최초 자동복구, 기본값 적용)
        /// </summary>
        public static void FindOrAddKey(Child child, DropdownConfig config)
        {
            if (child == null || config == null || config.SpouseConfigs == null)
                return;

            string gmcmKey = GMCMKeyUtil.GetChildKey(child);

            if (!config.SpouseConfigs.ContainsKey(gmcmKey))
            {
                // 기본값으로 추가 (오류 방지)
                config.SpouseConfigs[gmcmKey] = new SpouseChildConfig();
                CustomLogger.Warn($"[GMCMKeyValidator] 누락된 자녀 키 자동 추가: {gmcmKey}");
            }
        }

        /// <summary>
        /// 누락된(등록되지 않은) 자녀 키 전체 반환
        /// </summary>
        public static List<string> GetMissingKeys(IEnumerable<Child> children, DropdownConfig config)
        {
            var missing = new List<string>();
            if (children == null || config == null || config.SpouseConfigs == null)
                return missing;

            foreach (var child in children)
            {
                string gmcmKey = GMCMKeyUtil.GetChildKey(child);
                if (!config.SpouseConfigs.ContainsKey(gmcmKey))
                    missing.Add(gmcmKey);
            }
            return missing;
        }
    }
}