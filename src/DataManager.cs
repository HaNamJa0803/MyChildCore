using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 데이터 저장/로드 전담 매니저 (캐싱은 CacheManager에서 분리)
    /// </summary>
    public static class DataManager
    {
        private static string StoragePath => Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 저장: 자녀 데이터 전체를 JSON으로 기록
        /// </summary>
        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;
            File.WriteAllText(StoragePath, JsonConvert.SerializeObject(childrenData, Formatting.Indented));
        }

        /// <summary>
        /// 불러오기: JSON 파일에서 자녀 데이터 전체를 역직렬화
        /// </summary>
        public static List<ChildData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath)) return new List<ChildData>();
            return JsonConvert.DeserializeObject<List<ChildData>>(File.ReadAllText(StoragePath));
        }
    }
}