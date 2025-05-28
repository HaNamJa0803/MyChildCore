using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class DataManager
    {
        private static string StoragePath => Path.Combine(StardewModdingAPI.Constants.CurrentSavePath ?? "", "MyChildData.json");

        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;
            try
            {
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
            }
            catch { }
        }

        public static List<ChildData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath))
                return new List<ChildData>();
            try
            {
                string json = File.ReadAllText(StoragePath);
                return JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
            }
            catch
            {
                return new List<ChildData>();
            }
        }
    }
}