using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class DataManager
    {
        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        private static string BabyStoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyBabyData.json");

        // ------------------- ChildData 관련 기존 코드 (건드리지 않음) -------------------

        public static List<ChildData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath)) return new List<ChildData>();
            var json = File.ReadAllText(StoragePath);
            return JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
        }

        public static void SaveChildrenData(List<ChildData> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(StoragePath, json);
        }

        // ------------------- BabyData 저장/불러오기 추가 -------------------

        public static List<BabyData> LoadBabiesData()
        {
            if (!File.Exists(BabyStoragePath)) return new List<BabyData>();
            var json = File.ReadAllText(BabyStoragePath);
            return JsonConvert.DeserializeObject<List<BabyData>>(json) ?? new List<BabyData>();
        }

        public static void SaveBabiesData(List<BabyData> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(BabyStoragePath, json);
        }

        // ------------------- 외형 매니저 호출 추가 -------------------

        public static void ApplyAllAppearances(DropdownConfig config)
        {
            // Child 외형 적용
            foreach (var child in ChildManager.GetAllChildren())
            {
                var parts = PartsManager.GetPartsForChild(child, config);
                if (parts != null)
                    AppearanceManager.ApplyToddlerAppearance(child, parts);
            }

            // Baby 외형 적용
            foreach (var baby in ChildManager.GetAllBabies())
            {
                var spouseName = AppearanceManager.GetSpouseName(baby as Child);
                AppearanceManager.ApplyBabyAppearance(baby, spouseName);
            }
        }
    }
}