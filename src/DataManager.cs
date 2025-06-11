using MyChildCore;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI; // ★ Constants.SaveFolderName 사용

namespace MyChildCore
{
    /// <summary>
    /// 자녀 데이터 외부저장소/캐시/외형 동기화 매니저 (게임 데이터 연동 보완)
    /// </summary>
    public static class DataManager
    {
        // === 세이브폴더 자동 감지 (SMAPI의 Constants 활용) ===
        private static string SaveDir
        {
            get
            {
                // null-safe
                var folder = Constants.SaveFolderName;
                if (string.IsNullOrEmpty(folder))
                    folder = "Unknown";
                return Path.Combine("Saves", folder);
            }
        }

        public static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        public static event Action<List<ChildData>> OnDataChanged;

        /// <summary>
        /// [최우선] 실제 게임 내 자녀 객체 → 데이터 반환 (없으면 외부저장소)
        /// </summary>
        public static List<ChildData> LoadData()
        {
            // 1. 실제 게임 내 자녀 객체 우선
            var children = ChildManager.GetAllChildren();
            if (children.Count > 0)
            {
                var list = new List<ChildData>();
                foreach (var child in children)
                {
                    list.Add(ChildData.FromChild(child));
                }
                // 동기화 및 알림
                CacheManager.SyncWithData(list);
                NotifyDataChanged(list);
                return list;
            }

            // 2. 외부저장소(보험) 읽기
            if (File.Exists(DataPath))
            {
                var json = File.ReadAllText(DataPath);
                var list = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.SyncWithData(list);
                NotifyDataChanged(list);
                return list;
            }

            // 3. 아무것도 없으면 빈 리스트
            var empty = new List<ChildData>();
            CacheManager.SyncWithData(empty);
            NotifyDataChanged(empty);
            return empty;
        }

        /// <summary>
        /// [최우선] 캐시 → 외부저장소(보험) 저장 (실제 게임 내 자녀는 SMAPI가 저장)
        /// </summary>
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

        /// <summary>
        /// 외부저장소 → 캐시 동기화 (수동/긴급 복구)
        /// </summary>
        public static void SyncFromDisk()
        {
            if (File.Exists(DataPath))
            {
                var json = File.ReadAllText(DataPath);
                var data = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.SyncWithData(data);
                NotifyDataChanged(data);
            }
            else
            {
                var empty = new List<ChildData>();
                CacheManager.SyncWithData(empty);
                NotifyDataChanged(empty);
            }
        }

        // === 백업/복구 ===
        public static void Backup()
        {
            try
            {
                var data = LoadData();
                string backupPath = DataPath + ".bak";
                File.WriteAllText(backupPath, JsonConvert.SerializeObject(data, Formatting.Indented));
                CustomLogger.Info("[DataManager] 백업 완료");
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] Backup 예외: {ex.Message}");
            }
        }

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

        // === 자녀 전체 외형 적용 (합성+실시간 반영, 캐시까지 무효화) ===
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

        // === GMCMKey별 외형 적용 (생략 가능) ===
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