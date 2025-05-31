using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// ChildData, BabyData의 런타임 메모리 캐시 (세션 단위 캐싱, 영속 저장X)
    /// </summary>
    public static class CacheManager
    {
        // 유아 (Child) 캐시
        private static List<ChildData> _childCache;

        // 아기 (Baby) 캐시
        private static List<BabyData> _babyCache;

        // ========= ChildData 관련 =========

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

        // ========= BabyData 관련 =========

        /// <summary>
        /// BabyData 캐시 가져오기 (null 가능)
        /// </summary>
        public static List<BabyData> GetBabyCache() => _babyCache;

        /// <summary>
        /// BabyData 캐시 설정 (전체 교체)
        /// </summary>
        public static void SetBabyCache(List<BabyData> list)
        {
            _babyCache = list;
        }

        /// <summary>
        /// BabyData 캐시 초기화 (비우기)
        /// </summary>
        public static void ClearBabyCache()
        {
            _babyCache = null;
        }

        // ========= 전체 캐시 초기화 =========

        /// <summary>
        /// 모든 캐시 초기화 (Child + Baby)
        /// </summary>
        public static void ClearAllCache()
        {
            _childCache = null;
            _babyCache = null;
        }
    }
}