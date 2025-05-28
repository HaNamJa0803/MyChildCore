using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class CacheManager
    {
        private static Dictionary<string, DataManager.ChildAppearanceData> _cache = new();

        public static void LoadAllFromJson()
        {
            var list = DataManager.LoadChildrenData();
            _cache = list.ToDictionary(d => d.UniqueKey, d => d);
            CustomLogger.Info($"[캐시] 전체 자녀 데이터 {list.Count}명 캐싱 완료");
        }

        public static DataManager.ChildAppearanceData GetMyChildData(Child child)
        {
            if (child == null) return null;
            var key = DataManager.GetUniqueKey(child);
            _cache.TryGetValue(key, out var data);
            return data;
        }

        public static void UpdateChildCache(Child child, DataManager.ChildAppearanceData data)
        {
            if (child == null || data == null) return;
            var key = DataManager.GetUniqueKey(child);
            _cache[key] = data;
            SaveAllToJson();
            CustomLogger.Info($"[캐시] 자녀({key}) 데이터 갱신 및 저장");
        }

        public static void SaveAllToJson()
        {
            DataManager.SaveChildrenData(_cache.Values.ToList());
            CustomLogger.Info("[캐시] 전체 캐시를 외부 JSON에 저장 완료");
        }

        public static void InvalidateCache()
        {
            _cache.Clear();
            CustomLogger.Info("[캐시] 전체 캐시 초기화");
        }

        public static IEnumerable<DataManager.ChildAppearanceData> GetAllCachedData() => _cache.Values;
    }
}