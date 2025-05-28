using System.Collections.Generic;
using System.Linq;
using StardewValley.Characters;

namespace MyChildCore.Mod
{
    public static class CacheManager
    {
        private static Dictionary<string, ChildAppearanceData> _cache = new();

        public static void LoadAllFromJson()
        {
            var list = DataManager.LoadChildrenData();
            _cache = list.ToDictionary(d => d.UniqueKey, d => d);
        }

        public static ChildAppearanceData GetMyChildData(Child child)
        {
            if (child == null) return null;
            var key = $"{child.Name}_{child.idOfParent.Value}";
            _cache.TryGetValue(key, out var data);
            return data;
        }

        public static void UpdateChildCache(Child child, ChildAppearanceData data)
        {
            if (child == null || data == null) return;
            var key = $"{child.Name}_{child.idOfParent.Value}";
            _cache[key] = data;
            SaveAllToJson();
        }

        public static void SaveAllToJson()
        {
            DataManager.SaveChildrenData(_cache.Values.ToList());
        }

        public static void InvalidateCache()
        {
            _cache.Clear();
        }

        public static IEnumerable<ChildAppearanceData> GetAllCachedData() => _cache.Values;
    }
}