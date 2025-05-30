using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// ChildData의 영구 저장소 연동(읽기/쓰기), 캐시와 독립적으로 동작
    /// </summary>
    public static class DataManager
    {
        // 파일 경로는 save 폴더 등, 필요에 따라 조정
        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 외부 저장소에서 자녀 데이터 읽어오기(없으면 빈 리스트)
        /// </summary>
        public static List<ChildData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath)) return new List<ChildData>();
            var json = File.ReadAllText(StoragePath);
            return JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
        }

        /// <summary>
        /// 자녀 데이터 저장(전체 덮어쓰기, 리스트 단위)
        /// </summary>
        public static void SaveChildrenData(List<ChildData> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(StoragePath, json);
        }
    }
}