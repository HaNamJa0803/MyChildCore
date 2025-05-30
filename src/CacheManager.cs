using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// ChildData의 런타임 메모리 캐시 (한 세션 한정, 영속 저장X)
    /// </summary>
    public static class CacheManager
    {
        private static List<ChildData> _cache;

        /// <summary>
        /// 캐시에서 불러오기(없으면 null)
        /// </summary>
        public static List<ChildData> GetCache() => _cache;

        /// <summary>
        /// 캐시에 저장(전체 교체)
        /// </summary>
        public static void SetCache(List<ChildData> list)
        {
            _cache = list;
        }

        /// <summary>
        /// 캐시 지우기
        /// </summary>
        public static void ClearCache()
        {
            _cache = null;
        }
    }
}