using System;
using System.Collections.Generic;
using System.IO;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 데이터 캐시/세이브 데이터 연동 + 외부 백업 매니저 (유니크 칠드런 확장형)
    /// </summary>
    public static class CacheManager
    {
        private static List<ChildData> _childCache = new List<ChildData>();

        // SMAPI 세이브 데이터 Key (고유하게!)
        private const string SaveDataKey = "MyChildCore.ChildData";
        // 외부 JSON 백업 경로
        private static readonly string BackupPath = Path.Combine("data", "MyChildData_Backup.json");

        // === [1] 세이브 데이터 → 메모리 캐시 복원 ===
        public static void LoadCacheFromSaveData(IModHelper helper)
        {
            try
            {
                var saved = helper.Data.ReadSaveData<List<ChildData>>(SaveDataKey);
                _childCache = saved ?? new List<ChildData>();
                CustomLogger.Info($"[CacheManager] 세이브 데이터 → 캐시 복원: {_childCache.Count}명");
            }
            catch (Exception ex)
            {
                _childCache = new List<ChildData>();
                CustomLogger.Error($"[CacheManager] LoadCacheFromSaveData 예외: {ex.Message}");
            }
        }

        // === [2] 캐시 → 세이브 데이터 저장 ===
        public static void SaveCacheToSaveData(IModHelper helper)
        {
            try
            {
                helper.Data.WriteSaveData(SaveDataKey, _childCache);
                CustomLogger.Info($"[CacheManager] 캐시 → 세이브 데이터 저장: {_childCache.Count}명");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[CacheManager] SaveCacheToSaveData 예외: {ex.Message}");
            }
        }

        // === [3] 보조: 외부 JSON 즉시 백업 ===
        public static void BackupCacheToExternal()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(BackupPath) ?? "");
                System.IO.File.WriteAllText(BackupPath, Newtonsoft.Json.JsonConvert.SerializeObject(_childCache, Newtonsoft.Json.Formatting.Indented));
                CustomLogger.Info($"[CacheManager] 캐시 → 외부저장소 즉시 백업: {_childCache.Count}명");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[CacheManager] BackupCacheToExternal 예외: {ex.Message}");
            }
        }

        // === [4] 보조: 외부 JSON → 메모리 캐시 복원 (수동/긴급 복구용) ===
        public static void RestoreBackupFromExternal()
        {
            try
            {
                if (!System.IO.File.Exists(BackupPath))
                {
                    CustomLogger.Warn("[CacheManager] 외부저장소 파일 없음");
                    _childCache = new List<ChildData>();
                    return;
                }
                var data = System.IO.File.ReadAllText(BackupPath);
                _childCache = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChildData>>(data) ?? new List<ChildData>();
                CustomLogger.Info($"[CacheManager] 외부저장소 → 캐시 복원: {_childCache.Count}명");
            }
            catch (Exception ex)
            {
                _childCache = new List<ChildData>();
                CustomLogger.Error($"[CacheManager] RestoreBackupFromExternal 예외: {ex.Message}");
            }
        }

        // === [5] 캐시 세팅(저장/동기화) ===
        public static void SetChildCache(IModHelper helper, List<ChildData> list)
        {
            _childCache = list ?? new List<ChildData>();
            SaveCacheToSaveData(helper);
            ResourceManager.InvalidateAllChildrenSprites();
        }

        // === [6] 캐시 전체 초기화 ===
        public static void FullClear(IModHelper helper)
        {
            _childCache = new List<ChildData>();
            SaveCacheToSaveData(helper);
            BackupCacheToExternal(); // 필요하면 백업도 동시 초기화
            ResourceManager.InvalidateAllChildrenSprites();
            CustomLogger.Warn("[CacheManager] 캐시/세이브 데이터/외부 백업 초기화 완료");
        }

        // === [7] 캐시 조회 ===
        public static List<ChildData> GetChildCache()
        {
            if (_childCache == null) _childCache = new List<ChildData>();
            return _childCache;
        }

        // === [8] 자식별 필터/컨텍스트 ===
        public static List<ChildData> GetBabies()   => _childCache?.FindAll(c => c.Age == 0) ?? new List<ChildData>();
        public static List<ChildData> GetToddlers() => _childCache?.FindAll(c => c.Age == 1) ?? new List<ChildData>();
        public static List<ChildData> GetByGender(string gender)
        {
            int genderInt;
            if (int.TryParse(gender, out genderInt))
                return _childCache?.FindAll(c => c.Gender == genderInt) ?? new List<ChildData>();
            return new List<ChildData>();
        }
        public static List<ChildData> GetBySpouse(string spouse)
            => _childCache?.FindAll(c => c.SpouseName == spouse) ?? new List<ChildData>();

        // === [9] 리소스 캐시 무효화 ===
        public static void InvalidateChildSpriteCache(string childName = null)
        {
            if (string.IsNullOrEmpty(childName))
                ResourceManager.InvalidateAllChildrenSprites();
            else
                ResourceManager.InvalidateChildSprite(childName);
        }
    }
}