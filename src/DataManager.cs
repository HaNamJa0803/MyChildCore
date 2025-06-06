using System;
using System.Collections.Generic;
using StardewValley.Characters;
using StardewModdingAPI;
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

        public static List<ChildData> LoadData()
        {
            try
            {
                if (!File.Exists(DataPath)) return new List<ChildData>();
                string json = File.ReadAllText(DataPath);
                var list = JsonConvert.DeserializeObject<List<ChildData>>(json);
                return list ?? new List<ChildData>();
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
                List<Child> children = ChildManager.GetAllChildren();
                for (int i = 0; i < children.Count; i++)
                {
                    GMCMKeyValidator.FindOrAddKey(Child, Config);
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

            List<Child> children = ChildManager.GetAllChildren();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.Age >= 1)
                {
                    AppearanceManager.ApplyToddlerAppearance(child, config);
                }
                else
                {
                    AppearanceManager.ApplyBabyAppearance(child, config);
                }
            }
        }

        public static void ApplyAppearancesByGMCMKey(ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            List<Child> children = ChildManager.GetAllChildren();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                string gmcmKey = GMCMKeyUtil.GetChildKey(Child);
                if (config.SpouseConfigs != null && config.SpouseConfigs.ContainsKey(gmcmKey))
                {
                    if (child.Age >= 1)
                        AppearanceManager.ApplyToddlerAppearance(Child, Config);
                    else
                        AppearanceManager.ApplyBabyAppearance(Child, Config);
                }
            }
        }
    }
}