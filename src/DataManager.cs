using MyChildCore;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class DataManager
    {
        private static IModHelper Helper;
        public static void Init(IModHelper helper) => Helper = helper;

        private static string SaveDir
        {
            get
            {
                var folder = Constants.SaveFolderName;
                if (string.IsNullOrEmpty(folder)) folder = "Unknown";
                // Helper.Data.GetSaveFolder() = platform-safe per-save data folder
                return System.IO.Path.Combine(Helper.Data.GetSaveFolder(), folder);
            }
        }
        public static string DataPath => System.IO.Path.Combine(SaveDir, "MyChildData.json");

        public static event Action<List<ChildData>> OnDataChanged;

        public static List<ChildData> LoadData()
        {
            var children = ChildManager.GetAllChildren();
            if (children.Count > 0)
            {
                var list = new List<ChildData>();
                foreach (var child in children)
                    list.Add(ChildData.FromChild(child));
                CacheManager.SyncWithData(list);
                NotifyDataChanged(list);
                return list;
            }

            // 플랫폼-safe 백업 읽기
            if (Helper.Data.ReadJsonFile<List<ChildData>>(DataPath) is List<ChildData> backup && backup.Count > 0)
            {
                CacheManager.SyncWithData(backup);
                NotifyDataChanged(backup);
                return backup;
            }

            var empty = new List<ChildData>();
            CacheManager.SyncWithData(empty);
            NotifyDataChanged(empty);
            return empty;
        }

        public static void SaveData(List<ChildData> data)
        {
            try
            {
                // Directory.CreateDirectory(SaveDir); // 필요없음! Helper.Data.WriteJsonFile이 알아서 함
                Helper.Data.WriteJsonFile(DataPath, data);
                CacheManager.SetChildCache(data);
                NotifyDataChanged(data);
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] SaveData 예외: {ex.Message}");
            }
        }

        public static void SyncFromDisk()
        {
            if (Helper.Data.ReadJsonFile<List<ChildData>>(DataPath) is List<ChildData> backup && backup.Count > 0)
            {
                CacheManager.SyncWithData(backup);
                NotifyDataChanged(backup);
            }
            else
            {
                var empty = new List<ChildData>();
                CacheManager.SyncWithData(empty);
                NotifyDataChanged(empty);
            }
        }

        public static void Backup()
        {
            try
            {
                var data = LoadData();
                string backupPath = DataPath + ".bak";
                Helper.Data.WriteJsonFile(backupPath, data);
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
                if (Helper.Data.ReadJsonFile<List<ChildData>>(backupPath) is List<ChildData> backupData && backupData != null)
                {
                    SaveData(backupData);
                    CacheManager.SyncWithData(backupData);
                    NotifyDataChanged(backupData);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Error($"[DataManager] RestoreLatestBackup 예외: {ex.Message}");
            }
        }

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

        private static void NotifyDataChanged(List<ChildData> data)
        {
            try { OnDataChanged?.Invoke(data); }
            catch (Exception ex) { CustomLogger.Warn($"[DataManager] OnDataChanged 알림 예외: {ex.Message}"); }
        }
    }
}