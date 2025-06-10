using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 유니크 칠드런식 자녀 외부저장소/캐시/외형/리소스매니저까지 동기화 매니저
    /// </summary>
    public static class DataManager
    {
        private static string SaveDir =>
            Path.Combine("Saves", Game1.player?.Name ?? "Unknown");

        public static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        // === 외부 저장소 → 캐시 동기화 ===
        public static List<ChildData> LoadData()
        {
            try
            {
                if (!File.Exists(DataPath))
                {
                    CustomLogger.Warn("[DataManager] LoadData: 파일 없음, 빈 리스트 반환");
                    return new List<ChildData>();
                }
                string json = File.ReadAllText(DataPath);
                var list = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CustomLogger.Info($"[DataManager] LoadData: {list.Count}명");
                CacheManager.SyncWithData(list);
                return list;
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] LoadData 예외: {ex.Message}");
                return new List<ChildData>();
            }
        }

        // === 캐시 → 외부 저장소 저장 ===
        public static void SaveData(List<ChildData> data)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                File.WriteAllText(DataPath, JsonConvert.SerializeObject(data, Formatting.Indented));
                CustomLogger.Info($"[DataManager] SaveData: {data?.Count ?? 0}명 저장 완료");
                CacheManager.SetChildCache(data);
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] SaveData 예외: {ex.Message}");
            }
        }

        // === 외부저장소/캐시 동기화 + GMCM Key 체크 ===
        public static void AutoSyncCache(ModConfig config)
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);

            if (config != null)
            {
                foreach (var child in ChildManager.GetAllChildren())
                    GMCMKeyValidator.FindOrAddKey(child, config);
            }
        }

        // === 캐시 상태를 즉시 저장 ===
        public static void ManualSaveFromCache()
        {
            SaveData(CacheManager.GetChildCache());
        }

        // === 외부저장소 백업 ===
        public static void Backup()
        {
            try
            {
                var data = LoadData();
                string backupPath = DataPath + ".bak";
                File.WriteAllText(backupPath, JsonConvert.SerializeObject(data, Formatting.Indented));
                CustomLogger.Info("[DataManager] Backup: 최신 외부저장소 → 백업 저장 완료");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] Backup 예외: {ex.Message}");
            }
        }

        // === 외부저장소 강제 캐시 덮어쓰기 ===
        public static void SyncFromDisk()
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);
            CustomLogger.Info($"[DataManager] SyncFromDisk: {data.Count}명 동기화 완료");
        }

        // === 백업 복원(캐시 동시 동기화) ===
        public static void RestoreLatestBackup()
        {
            string backupPath = DataPath + ".bak";
            try
            {
                if (File.Exists(backupPath))
                {
                    var backupData = JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(backupPath));
                    if (backupData != null)
                    {
                        SaveData(backupData);
                        CacheManager.SyncWithData(backupData);
                        CustomLogger.Info("[DataManager] RestoreLatestBackup: 백업 복원 성공");
                    }
                    else
                        CustomLogger.Warn("[DataManager] RestoreLatestBackup: 백업 데이터 null");
                }
                else
                    CustomLogger.Warn("[DataManager] RestoreLatestBackup: 백업 파일 없음");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] RestoreLatestBackup 예외: {ex.Message}");
            }
        }

        // === 모든 자녀 외형 적용 (파츠/캐시/리소스 최신화) ===
        public static void ApplyAllAppearances(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child == null) continue;

                // [유니크 칠드런식] 파츠 동적 분기
                var parts = (child.Age >= 1)
                    ? PartsManager.GetPartsForChild(child, config)
                    : PartsManager.GetPartsForBaby(child, config);

                if (parts == null)
                {
                    CustomLogger.Warn($"[DataManager] {child.Name} 외형 파츠 누락 → Default 적용");
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                }

                // 외형 적용 (합성+적용)
                if (child.Age >= 1)
                    AppearanceManager.ApplyToddlerAppearance(child, parts);
                else
                    AppearanceManager.ApplyBabyAppearance(child, parts);

                // 최신화: ResourceManager로 캐시 무효화!
                ResourceManager.InvalidateChildSprite(child.Name);
            }
        }

        // === GMCMKey별 외형 적용(조건/캐싱/복구 포함) ===
        public static void ApplyAppearancesByGMCMKey(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child == null) continue;
                string gmcmKey = GMCMKeyUtil.GetChildKey(child);
                if (config.SpouseConfigs != null && config.SpouseConfigs.ContainsKey(gmcmKey))
                {
                    var parts = (child.Age >= 1)
                        ? PartsManager.GetPartsForChild(child, config)
                        : PartsManager.GetPartsForBaby(child, config);

                    if (parts == null)
                    {
                        CustomLogger.Warn($"[DataManager] {child.Name} 외형 파츠 누락 → Default 적용");
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                    }

                    if (child.Age >= 1)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                    else
                        AppearanceManager.ApplyBabyAppearance(child, parts);

                    ResourceManager.InvalidateChildSprite(child.Name);
                }
            }
        }
    }
}