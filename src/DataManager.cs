using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    public static class DataManager
    {
        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;
            try
            {
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
                CacheManager.Set("MyChildData", childrenData);
                CacheManager.SaveCacheToFile(StoragePath + ".cache");
                CustomLogger.Info($"[DataManager] 자녀 데이터 저장: {StoragePath} ({childrenData.Count}명)");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[DataManager] Save 실패: {ex.Message}");
            }
        }

        public static List<ChildData> LoadChildrenData()
        {
            var cached = CacheManager.Get<List<ChildData>>("MyChildData");
            if (cached != null) return cached;

            if (!File.Exists(StoragePath))
                return new List<ChildData>();

            try
            {
                string json = File.ReadAllText(StoragePath);
                var data = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.Set("MyChildData", data);
                CustomLogger.Debug($"[DataManager] 파일에서 자녀 데이터 로드 및 캐시 동기화");
                return data;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[DataManager] Load 실패: {ex.Message}");
                return new List<ChildData>();
            }
        }

        public static void RestoreChildrenDataFromCache()
        {
            CacheManager.RestoreFromLastSave();
            CustomLogger.Info("[DataManager] 캐시 백업 파일에서 자녀 데이터 복구 시도");
        }

        public static void ApplyAllChildDataFromStorage()
        {
            var loaded = LoadChildrenData();
            if (loaded.Count == 0) return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                var data = loaded.Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);
                if (data == null) continue;

                AppearanceManager.ApplyParts(child, data);
            }

            CustomLogger.Info("[DataManager] 자녀 외형(파츠) 하드코딩 동기화 완료!");
        }

        public static void RegisterToEventManager(IModHelper helper, IMonitor monitor)
        {
            EventManager.HookAll(helper, monitor, ApplyAllChildDataFromStorage);
            CustomLogger.Info("[DataManager] EventManager에 외형 동기화 콜백 등록 완료!");
        }
    }
}