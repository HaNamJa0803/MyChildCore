using System;
using System.Collections.Generic;
using StardewModdingAPI;
using System.IO;
using Newtonsoft.Json;
using System.Linq; // 추가 필요
using MyChildCore;

namespace MyChildCore
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
            try
            {
                File.WriteAllText(BackupPath, JsonConvert.SerializeObject(_childCache, Formatting.Indented));
            }
            catch { /* 필요시 로깅 */ }
        }

        public static void RestoreBackup()
        {
            try
            {
                if (!File.Exists(BackupPath)) return;
                _childCache = JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(BackupPath)) ?? new();
            }
            catch { _childCache = new List<ChildData>(); }
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