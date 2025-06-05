using System;
using System.Collections.Generic;
using StardewModdingAPI;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using MyChildCore;

namespace MyChildCore
{
    public static class DataManager
    {
        private static string SaveDir => Path.Combine("Saves", Game1.player?.Name ?? "Unknown");
        private static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        public static List<ChildData> LoadData()
        {
            try
            {
                if (!File.Exists(DataPath)) return new();
                return JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(DataPath)) ?? new();
            }
            catch { return new List<ChildData>(); }
        }

        public static void SaveData(List<ChildData> data)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                File.WriteAllText(DataPath, JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch { }
        }

        public static void AutoSyncCache(ModConfig config)
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);

            if (config != null)
            {
                foreach (var child in ChildManager.GetAllChildren())
                {
                    GMCMKeyValidator.FindOrAddKey(child, config);
                }
            }
        }

        public static void ManualSaveFromCache()
        {
            SaveData(CacheManager.GetChildCache());
        }

        public static void Backup()
        {
            try
            {
                var data = LoadData();
                File.WriteAllText(DataPath + ".bak", JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch { }
        }

        public static void SyncFromDisk()
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);
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
                    }
                }
            }
            catch { }
        }

        // === 핵심: 모드 전체 ON/OFF까지 반영 ===
        public static void ApplyAllAppearances(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (child.Age >= 1)
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts != null)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                }
                else
                {
                    var babyParts = PartsManager.GetPartsForBaby(child, config);
                    if (babyParts != null)
                        AppearanceManager.ApplyBabyAppearance(child, babyParts);
                }
            }
        }

        public static void ApplyAppearancesByGMCMKey(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                string gmcmKey = GMCMKeyUtil.GetChildKey(child);
                if (config.SpouseConfigs.TryGetValue(gmcmKey, out var spouseConfig))
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts != null)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                }
            }
        }
    }
}