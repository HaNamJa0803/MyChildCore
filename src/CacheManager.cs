using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 데이터 캐시/외부저장소 동기화 매니저 (유니크 칠드런식)
    /// - 외형 리소스 변경시 캐시 무효화/즉시 동기화
    /// - 자식별/컨텍스트별 캐시 일관성 보장
    /// </summary>
    public static class CacheManager
    {
        private static List<ChildData> _childCache = new List<ChildData>();
        private static readonly string BackupPath = Path.Combine("data", "MyChildData_Backup.json");

        // === [1] 캐시/백업 완전 초기화 + 리소스 캐시 무효화 ===
        public static void FullClear()
        {
            _childCache = new List<ChildData>();
            try { if (File.Exists(BackupPath)) File.Delete(BackupPath); } catch { }
            ResourceManager.InvalidateAllChildrenSprites();
            CustomLogger.Warn("[CacheManager] 캐시/백업/리소스 전부 초기화 완료");
        }

        // === [2] 메모리 캐시 즉시 조회 ===
        public static List<ChildData> GetChildCache()
        {
            if (_childCache == null) _childCache = new List<ChildData>();
            return _childCache;
        }

        // === [3] 메모리 캐시 강제 세팅(즉시 외부저장소 백업+리소스 캐시 무효화) ===
        public static void SetChildCache(List<ChildData> list)
        {
            _childCache = list ?? new List<ChildData>();
            CustomLogger.Info("[CacheManager] SetChildCache: " + _childCache.Count + "명");
            BackupCache();
            ResourceManager.InvalidateAllChildrenSprites();
        }

        // === [4] 메모리 → 외부저장소 즉시 백업 ===
        public static void BackupCache()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(BackupPath) ?? "");
                File.WriteAllText(BackupPath, JsonConvert.SerializeObject(_childCache, Formatting.Indented));
                CustomLogger.Info("[CacheManager] 캐시 → 외부저장소 즉시 백업: " + _childCache.Count + "명");
            }
            catch (Exception ex)
            {
                CustomLogger.Error("[CacheManager] BackupCache 예외: " + ex.Message);
            }
        }

        // === [5] 외부저장소 → 메모리 복원(강제 덮어쓰기+리소스 무효화) ===
        public static void RestoreBackup()
        {
            try
            {
                if (!File.Exists(BackupPath))
                {
                    CustomLogger.Warn("[CacheManager] 외부저장소 파일 없음");
                    _childCache = new List<ChildData>();
                    return;
                }
                var data = File.ReadAllText(BackupPath);
                _childCache = JsonConvert.DeserializeObject<List<ChildData>>(data) ?? new List<ChildData>();
                CustomLogger.Info("[CacheManager] 외부저장소 → 캐시 복원: " + _childCache.Count + "명");
                ResourceManager.InvalidateAllChildrenSprites();
            }
            catch (Exception ex)
            {
                _childCache = new List<ChildData>();
                CustomLogger.Error("[CacheManager] RestoreBackup 예외: " + ex.Message);
            }
        }

        // === [6] 외부 데이터 리스트 무조건 메모리로 받고, 즉시 백업+리소스 무효화 ===
        public static void SyncWithData(List<ChildData> external)
        {
            if (external == null)
            {
                CustomLogger.Warn("[CacheManager] SyncWithData: null 무시");
                return;
            }
            _childCache = new List<ChildData>(external);
            BackupCache();
            ResourceManager.InvalidateAllChildrenSprites();
            CustomLogger.Info("[CacheManager] 외부데이터 Sync&Backup 완료: " + external.Count + "명");
        }

        // === [7] 강제 메모리 덮기(외부저장소/컨피그 등) ===
        public static void OverwriteFrom(List<ChildData> anySource)
        {
            _childCache = new List<ChildData>(anySource ?? new List<ChildData>());
            BackupCache();
            ResourceManager.InvalidateAllChildrenSprites();
            CustomLogger.Info("[CacheManager] OverwriteFrom: 새로 " + _childCache.Count + "명 적용/백업");
        }

        // === [8] 하드 리로드(메모리 무효화+파일 복원+리소스 무효화) ===
        public static void Reload()
        {
            _childCache = null;
            RestoreBackup();
        }

        // === [9] 자식별 필터/컨텍스트 (유니크 칠드런식 확장성) ===
        public static List<ChildData> GetBabies()
        {
            return _childCache != null ? _childCache.FindAll(c => c.Age == 0) : new List<ChildData>();
        }
        public static List<ChildData> GetToddlers()
        {
            return _childCache != null ? _childCache.FindAll(c => c.Age == 1) : new List<ChildData>();
        }
        public static List<ChildData> GetByGender(string gender)
        {
            int genderInt;
            if (int.TryParse(gender, out genderInt))
                return _childCache != null ? _childCache.FindAll(c => c.Gender == genderInt) : new List<ChildData>();
            return new List<ChildData>();
        }
        public static List<ChildData> GetBySpouse(string spouse)
        {
            return _childCache != null ? _childCache.FindAll(c => c.SpouseName == spouse) : new List<ChildData>();
        }

        // === [10] 캐시 강제 무효화 (자식별/전체 리소스) ===
        public static void InvalidateChildSpriteCache(string childName = null)
        {
            if (string.IsNullOrEmpty(childName))
                ResourceManager.InvalidateAllChildrenSprites();
            else
                ResourceManager.InvalidateChildSprite(childName);
        }
    }
}