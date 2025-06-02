using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    public static class CacheManager
    {
        private static List<ChildData> _childCache;
        private static readonly string BackupPath = "MyChildData_Backup.json";

        public static List<ChildData> GetChildCache() => _childCache ?? new List<ChildData>();
        public static void SetChildCache(List<ChildData> list) => _childCache = list;
        public static void ClearChildCache() => _childCache = null;

        public static void BackupCache()
        {
            if (_childCache == null) return;
            File.WriteAllText(BackupPath, JsonConvert.SerializeObject(_childCache, Formatting.Indented));
        }

        public static void RestoreBackup()
        {
            if (!File.Exists(BackupPath)) return;
            _childCache = JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(BackupPath)) ?? new();
        }

        public static void SyncWithData(List<ChildData> external)
        {
            if (external == null) return;
            _childCache = external.ToList();
        }

        public static List<ChildData> GetBabies() => _childCache?.Where(c => c.Age == 0).ToList() ?? new();
        public static List<ChildData> GetToddlers() => _childCache?.Where(c => c.Age == 1).ToList() ?? new();
    }
}