using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 데이터 캐시 매니저 (최적화, 외부 저장 연동, 유니크/성능 극대화)
    /// </summary>
    public static class CacheManager
    {
        // uniqueKey(이름+부모ID) → 외형 데이터 캐시
        private static Dictionary<string, ChildAppearanceData> _cache = new();

        /// <summary>
        /// 외부 JSON 데이터 → 캐시(딕셔너리)로 전체 로딩 (최초 1회)
        /// </summary>
        public static void LoadAllFromJson()
        {
            var list = DataManager.LoadChildrenData();
            _cache = list.ToDictionary(d => d.UniqueKey, d => d);
            CustomLogger.Info($"[캐시] 전체 자녀 {list.Count}명 캐싱 완료");
        }

        /// <summary>
        /// 자녀 1명(유니크) 캐시에서 바로 조회 (메모리 O(1) 조회)
        /// </summary>
        public static ChildAppearanceData GetMyChildData(Child child)
        {
            if (child == null) return null;
            var key = DataManager.GetUniqueKey(child);
            _cache.TryGetValue(key, out var data);
            return data;
        }

        /// <summary>
        /// 캐시 갱신(1명 추가/수정) + JSON 저장까지 즉시 반영
        /// </summary>
        public static void UpdateChildCache(Child child, ChildAppearanceData data)
        {
            if (child == null || data == null) return;
            var key = DataManager.GetUniqueKey(child);
            _cache[key] = data;
            SaveAllToJson();
            CustomLogger.Info($"[캐시] 자녀({key}) 데이터 갱신 및 저장");
        }

        /// <summary>
        /// 전체 캐시 데이터 → 외부 저장소 동기화
        /// </summary>
        public static void SaveAllToJson()
        {
            DataManager.SaveChildrenData(_cache.Values.ToList());
            CustomLogger.Info("[캐시] 전체 캐시를 외부 JSON에 저장 완료");
        }

        /// <summary>
        /// 캐시 비우기 (세이브 리로드, 리셋 등)
        /// </summary>
        public static void InvalidateCache()
        {
            _cache.Clear();
            CustomLogger.Info("[캐시] 전체 캐시 초기화");
        }

        /// <summary>
        /// 전체 캐싱된 데이터 반환 (디버그/관리용)
        /// </summary>
        public static IEnumerable<ChildAppearanceData> GetAllCachedData() => _cache.Values;
    }
}