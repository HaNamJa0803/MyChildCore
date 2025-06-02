using System;
using System.Collections.Generic;
using StardewValley.Characters;
using MyChildCore;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// GMCM 고유키와 Child 객체의 매핑 유틸리티 (키 ↔ Child 역방향 전환)
    /// </summary>
    public static class GMCMKeyMapper
    {
        // 내부 캐시 (키→Child, Child→키)
        private static Dictionary<string, Child> keyToChildMap = new();
        private static Dictionary<Child, string> childToKeyMap = new();

        /// <summary>
        /// 현재 게임의 모든 자녀(Child) 인스턴스와 GMCMKey를 일괄 매핑 (초기화/리프레시)
        /// </summary>
        public static void RefreshMapping()
        {
            keyToChildMap.Clear();
            childToKeyMap.Clear();

            try
            {
                var children = ChildManager.GetAllChildren();
                foreach (var child in children)
                {
                    string key = GMCMKeyUtil.GetChildKey(child);
                    if (!string.IsNullOrEmpty(key))
                    {
                        keyToChildMap[key] = child;
                        childToKeyMap[child] = key;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[GMCMKeyMapper] 매핑 중 예외: {ex.GetType().Name} / {ex.Message}");
            }
        }

        /// <summary>
        /// GMCMKey로 Child 객체 반환 (없으면 null)
        /// </summary>
        public static Child GetChildByKey(string gmcmKey)
        {
            if (string.IsNullOrWhiteSpace(gmcmKey))
                return null;
            keyToChildMap.TryGetValue(gmcmKey, out var child);
            return child;
        }

        /// <summary>
        /// Child 객체로 GMCMKey 반환 (없으면 null)
        /// </summary>
        public static string GetKeyByChild(Child child)
        {
            if (child == null)
                return null;
            childToKeyMap.TryGetValue(child, out var key);
            return key;
        }

        /// <summary>
        /// 현재 매핑 최신화(자녀 변동/로드 후 반드시 호출)
        /// </summary>
        public static void EnsureMapping()
        {
            // 필요하다면 새로고침
            if (keyToChildMap.Count == 0 || childToKeyMap.Count == 0)
                RefreshMapping();
        }
    }
}