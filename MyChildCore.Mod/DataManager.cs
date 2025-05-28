using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    public static class DataManager
    {
        private static string StoragePath => Path.Combine(StardewModdingAPI.Constants.CurrentSavePath ?? "", "MyChildData.json");

        public static void SaveChildrenData(List<ChildAppearanceData> childrenData)
        {
            if (childrenData == null) return;
            try
            {
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
            }
            catch { }
        }

        public static List<ChildAppearanceData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath))
                return new List<ChildAppearanceData>();
            try
            {
                string json = File.ReadAllText(StoragePath);
                return JsonConvert.DeserializeObject<List<ChildAppearanceData>>(json) ?? new List<ChildAppearanceData>();
            }
            catch
            {
                return new List<ChildAppearanceData>();
            }
        }

        public static string GetUniqueKey(ChildAppearanceData data)
            => ChildAppearanceData.GetUniqueKey(data);
    }
}