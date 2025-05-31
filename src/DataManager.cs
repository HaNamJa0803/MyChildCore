using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 데이터(아기+유아) 외부 저장/불러오기 및 외형 적용 전담 매니저
    /// </summary>
    public static class DataManager
    {
        // 저장 파일 경로 (아기/유아 통합)
        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 외부 저장소에서 자녀 데이터(아기+유아) 읽어오기 (없으면 빈 리스트)
        /// </summary>
        public static List<ChildData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath)) 
                return new List<ChildData>();

            var json = File.ReadAllText(StoragePath);
            return JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
        }

        /// <summary>
        /// 자녀 데이터(아기+유아) 저장 (전체 덮어쓰기, 리스트 단위)
        /// </summary>
        public static void SaveChildrenData(List<ChildData> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(StoragePath, json);
        }

        /// <summary>
        /// 저장된 설정(config) 기반으로 모든 자녀(아기+유아) 외형을 일괄 적용
        /// </summary>
        public static void ApplyAllAppearances(DropdownConfig config)
        {
            foreach (var child in ChildManager.GetAllChildren())
            {
                // 유아/아이
                if (child.Age >= 1)
                {
                    var parts = PartsManager.GetPartsForChild(child, config);
                    if (parts != null)
                        AppearanceManager.ApplyToddlerAppearance(child, parts);
                }
                // 아기 (Age == 0)
                else
                {
                    // 필요 시, 아기 전용 외형 로직을 여기에 추가
                    var spouseName = AppearanceManager.GetSpouseName(child);
                    AppearanceManager.ApplyBabyAppearance(child, spouseName);
                }
            }
        }
    }
}