using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley.Characters;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using MyChildCore;

namespace MyChildCore
{
    public static class DataManager
    {
        private static string SaveDir
        {
            get
            {
                string playerName = (Game1.player != null && !string.IsNullOrEmpty(Game1.player.Name)) ? Game1.player.Name : "Unknown";
                return Path.Combine("Saves", playerName);
            }
        }
        private static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        // 외부 저장소 → 캐시 동기화
        public static List<ChildData> LoadData()
        {
            try
            {
                if (!File.Exists(DataPath))
                    return new List<ChildData>();
                string json = File.ReadAllText(DataPath);
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChildData>>(json);
                return list ?? new List<ChildData>();
            }
            catch { return new List<ChildData>(); }
        }

        public static void SaveData(List<ChildData> data)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                File.WriteAllText(DataPath, Newtonsoft.Json.JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch { }
        }

        // 외부 저장소 동기화 + GMCMKey 체크까지!
        public static void AutoSyncCache(ModConfig config)
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);

            if (config != null)
            {
                List<Child> children = ChildManager.GetAllChildren();
                for (int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
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
                File.WriteAllText(DataPath + ".bak", Newtonsoft.Json.JsonConvert.SerializeObject(data, Formatting.Indented));
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
                    var backupData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(backupPath));
                    if (backupData != null)
                    {
                        SaveData(backupData);
                        CacheManager.SyncWithData(backupData);
                    }
                }
            }
            catch { }
        }

        // === 모든 자녀 외형 적용(ON/OFF까지) ===
        public static void ApplyAllAppearances(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            List<Child> children = ChildManager.GetAllChildren();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child == null) continue;

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
                        AppearanceManager.ApplyBabyAppearance(child, parts);
                }
            }
        }

        // === GMCMKey별 외형 적용 ===
        public static void ApplyAppearancesByGMCMKey(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            List<Child> children = ChildManager.GetAllChildren();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child == null) continue;
                string gmcmKey = GMCMKeyUtil.GetChildKey(child);
                if (config.SpouseConfigs != null && config.SpouseConfigs.ContainsKey(gmcmKey))
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
                            AppearanceManager.ApplyBabyAppearance(child, parts);
                    }
                }
            }
        }
    }
}