using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
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
                CustomLogger.Info($"[데이터] 전체 자녀 외형 데이터 저장: {StoragePath}");
            }
            catch (System.Exception ex)
            {
                CustomLogger.Error($"[데이터] 저장 실패: {ex}");
            }
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
            catch (System.Exception ex)
            {
                CustomLogger.Error($"[데이터] 불러오기 실패: {ex}");
                return new List<ChildAppearanceData>();
            }
        }

        public static ChildAppearanceData LoadMyChildData(Child myChild)
        {
            if (myChild == null) return null;
            var list = LoadChildrenData();
            string uniqueKey = GetUniqueKey(myChild);
            return list.FirstOrDefault(d => d.UniqueKey == uniqueKey);
        }

        public static void SaveMyChildData(Child myChild, ChildAppearanceData appearanceData)
        {
            var list = LoadChildrenData();
            string uniqueKey = GetUniqueKey(myChild);
            var idx = list.FindIndex(d => d.UniqueKey == uniqueKey);
            if (idx >= 0) list[idx] = appearanceData;
            else list.Add(appearanceData);
            SaveChildrenData(list);
        }

        public static string GetUniqueKey(Child child)
            => child == null ? "" : $"{child.Name}_{child.idOfParent.Value}";
    }

    public class ChildAppearanceData
    {
        public string UniqueKey { get; set; }
        public string Name { get; set; }
        public long ParentID { get; set; }
        public bool IsMale { get; set; }
        public string SkinTone { get; set; }
        public string AppearanceKey { get; set; }
        // 필요시 확장 필드 추가
    }
}