using MyChildCore;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 데이터 외부저장소/캐시/외형 동기화 매니저
    /// + 데이터 즉시 감지(Observer) 지원!
    /// </summary>
    public static class DataManager
    {
        private static string SaveDir =>
            Path.Combine("Saves", Game1.player?.Name ?? "Unknown");

        public static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        // === [옵저버] 데이터 변경 알림 ===
        public static event Action<List<ChildData>> OnDataChanged;

        // === 외부 저장소 → 캐시 동기화 ===
        public static List<ChildData> LoadData()
        {
            try
            {
                if (!File.Exists(DataPath))
                {
                    NotifyDataChanged(new List<ChildData>());
                    return new List<ChildData>();
                }
                string json = File.ReadAllText(DataPath);
                var list = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.SyncWithData(list);
                NotifyDataChanged(list);
                return list;
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] LoadData 예외: {ex.Message}");
                NotifyDataChanged(new List<ChildData>());
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
                CacheManager.SetChildCache(data);
                NotifyDataChanged(data); // 즉시 감지!
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] SaveData 예외: {ex.Message}");
            }
        }

        // === 외부저장소/캐시 동기화 ===
        public static void SyncFromDisk()
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);
            NotifyDataChanged(data);
        }

        // === 백업 ===
        public static void Backup()
        {
            try
            {
                var data = LoadData();
                string backupPath = DataPath + ".bak";
                File.WriteAllText(backupPath, JsonConvert.SerializeObject(data, Formatting.Indented));
                CustomLogger.Info("[DataManager] 백업 완료");
                NotifyDataChanged(data);
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] Backup 예외: {ex.Message}");
            }
        }

        // === 백업 복구 ===
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
                        NotifyDataChanged(backupData);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] RestoreLatestBackup 예외: {ex.Message}");
            }
        }

        // === 자녀 전체 외형 적용 (합성+실시간 반영, ResourceManager 통해 캐시까지 무효화) ===
        public static void ApplyAllAppearances(ModConfig config)
        {
            if (config == null || !config.EnableMod) return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child == null) continue;
                var parts = (child.Age >= 1)
                    ? PartsManager.GetPartsForChild(child, config)
                    : PartsManager.GetPartsForBaby(child, config);
                if (parts == null)
                    parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                try
                {
                    if (child.Age >= 1)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                    else
                        AppearanceManager.ApplyBabyAppearance(child, parts);

                    ResourceManager.InvalidateChildSprite(child.Name);
                }
                catch (Exception ex)
                {
                    CustomLogger.Warn($"[DataManager] {child.Name} 외형 적용 예외: {ex.Message}");
                }
            }
            NotifyDataChanged(CacheManager.GetChildCache());
        }

        // === GMCMKey별 외형 적용 ===
        public static void ApplyAppearancesByGMCMKey(ModConfig config)
        {
            if (config == null || !config.EnableMod) return;

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
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                    try
                    {
                        if (child.Age >= 1)
                            AppearanceManager.ApplyToddlerAppearance(child, parts);
                        else
                            AppearanceManager.ApplyBabyAppearance(child, parts);

                        ResourceManager.InvalidateChildSprite(child.Name);
                    }
                    catch (Exception ex)
                    {
                        CustomLogger.Warn($"[DataManager] {child.Name} 외형 적용 예외: {ex.Message}");
                    }
                }
            }
            NotifyDataChanged(CacheManager.GetChildCache());
        }

        // === [즉시 감지 알림 호출] ===
        private static void NotifyDataChanged(List<ChildData> data)
        {
            try
            {
                OnDataChanged?.Invoke(data);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[DataManager] OnDataChanged 알림 예외: {ex.Message}");
            }
        }
    }
}