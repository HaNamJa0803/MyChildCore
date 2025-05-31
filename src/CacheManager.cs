using System.Collections.Generic;
using System.Linq; // ★ 반드시 필요!

namespace MyChildCore.Utilities
{
    /// <summary>
    /// ChildData의 런타임 메모리 캐시 (세션 단위 캐싱, 영속 저장X)
    /// </summary>
    public static class CacheManager
    {
        // ────────────── 전체 자녀 캐시 (아기+유아 통합) ──────────────
        private static List<ChildData> _childCache;

        // ────────────── ChildData 관련 ──────────────

        /// <summary>
        /// ChildData 캐시 가져오기 (null 가능)
        /// </summary>
        public static List<ChildData> GetChildCache() => _childCache;

        /// <summary>
        /// ChildData 캐시 설정 (전체 교체)
        /// </summary>
        public static void SetChildCache(List<ChildData> list)
        {
            _childCache = list;
        }

        /// <summary>
        /// ChildData 캐시 초기화 (비우기)
        /// </summary>
        public static void ClearChildCache()
        {
            _childCache = null;
        }

        // ────────────── 아기/유아 명시적 분리 함수 ──────────────

        /// <summary>
        /// 전체 캐시에서 "아기(BABY)"만 가져오기 (Age == 0)
        /// </summary>
        public static List<ChildData> GetBabiesInCache()
            => _childCache?.Where(c => c.Age == 0).ToList() ?? new List<ChildData>();

        /// <summary>
        /// 전체 캐시에서 "유아(TODDLER)"만 가져오기 (Age == 1)
        /// </summary>
        public static List<ChildData> GetToddlersInCache()
            => _childCache?.Where(c => c.Age == 1).ToList() ?? new List<ChildData>();

        // ────────────── 전체 캐시 초기화 ──────────────

        /// <summary>
        /// 모든 캐시 초기화 (ChildData 전체)
        /// </summary>
        public static void ClearAllCache()
        {
            _childCache = null;
        }
    }
}