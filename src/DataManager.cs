using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class DataManager
    {
        private static string SaveDir => Path.Combine("Saves", Game1.player?.Name ?? "Unknown");
        private static string DataPath => Path.Combine(SaveDir, "MyChildData.json");

        public static List<ChildData> LoadData()
        {
            if (!File.Exists(DataPath)) return new();
            return JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(DataPath)) ?? new();
        }

        public static void SaveData(List<ChildData> data)
        {
            Directory.CreateDirectory(SaveDir);
            File.WriteAllText(DataPath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void AutoSyncCache()
        {
            var data = LoadData();
            CacheManager.SyncWithData(data);
        }

        public static void ManualSaveFromCache()
        {
            SaveData(CacheManager.GetChildCache());
        }

        public static void Backup()
        {
            var data = LoadData();
            File.WriteAllText(DataPath + ".bak", JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        // 실시간 적용 보조 (함수로 정의)
        public static void ApplyAllAppearances(DropdownConfig config)
        {
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
    }
}