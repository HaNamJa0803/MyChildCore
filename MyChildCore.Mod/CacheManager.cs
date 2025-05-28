using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class CacheManager
    {
        // uniqueKey(이름+부모ID) → 외형 데이터 캐시
        private static Dictionary<string, ChildAppearanceData> _cache = new();

        /// <summary>
        /// 전체 데이터(외부 저장소) → 캐시로 한 번에 로딩 (성능 극대화)
        /// </summary>
        public static void LoadAllFromJson()
        {
            var list = DataManager.LoadChildrenData(); // List<ChildAppearanceData>
            _cache = list.ToDictionary(d => d.UniqueKey, d => d);
            CustomLogger.Info($"[캐시] 전체 자녀 데이터 {list.Count}명 캐싱 완료");
        }

        /// <summary>
        /// 내 자녀 한 명(유니크 방식) 캐시에서 바로 조회
        /// </summary>
        public static ChildAppearanceData GetMyChildData(Child child)
        {
            if (child == null) return null;
            var key = DataManager.GetUniqueKey(child);
            _cache.TryGetValue(key, out var data);
            return data;
        }

        /// <summary>
        /// 캐시 갱신(추가/수정) 및 파일 저장까지 동기화
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
        /// 전체 캐시를 외부 JSON 저장소로 동기화
        /// </summary>
        public static void SaveAllToJson()
        {
            DataManager.SaveChildrenData(_cache.Values.ToList());
            CustomLogger.Info("[캐시] 전체 캐시를 외부 JSON에 저장 완료");
        }

        /// <summary>
        /// 캐시 비우기(세이브 리로드 등)
        /// </summary>
        public static void InvalidateCache()
        {
            _cache.Clear();
            CustomLogger.Info("[캐시] 전체 캐시 초기화");
        }

        /// <summary>
        /// 전체 캐싱 데이터 반환 (확장, 디버깅 등)
        /// </summary>
        public static IEnumerable<ChildAppearanceData> GetAllCachedData() => _cache.Values;
    }
}