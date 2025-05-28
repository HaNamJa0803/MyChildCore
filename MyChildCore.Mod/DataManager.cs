using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형/설정 등 커스텀 데이터의 JSON 저장/불러오기 및 유니크 관리
    /// (세이브별 분리, 확장성/백업/복구 대응)
    /// </summary>
    public static class DataManager
    {
        // 세이브별 외부 스토리지 경로
        private static string StoragePath => Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 전체 자녀 외형/설정 데이터 저장(JSON)
        /// </summary>
        public static void SaveChildrenData(List<ChildAppearanceData> childrenData)
        {
            if (childrenData == null) return;
            try
            {
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
                CustomLogger.Info($"[DataManager] 저장 성공: {StoragePath}");
            }
            catch (System.Exception ex)
            {
                CustomLogger.Error($"[DataManager] 저장 실패: {ex}");
            }
        }

        /// <summary>
        /// 전체 자녀 외형/설정 데이터 불러오기(JSON)
        /// </summary>
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
                CustomLogger.Error($"[DataManager] 불러오기 실패: {ex}");
                return new List<ChildAppearanceData>();
            }
        }

        /// <summary>
        /// 내 자녀 한 명의 외형 데이터만 가져오기(유니크 키 기반)
        /// </summary>
        public static ChildAppearanceData LoadMyChildData(Child myChild)
        {
            if (myChild == null) return null;
            var list = LoadChildrenData();
            string uniqueKey = GetUniqueKey(myChild);
            return list.FirstOrDefault(d => d.UniqueKey == uniqueKey);
        }

        /// <summary>
        /// 내 자녀 한 명의 데이터만 갱신/저장
        /// </summary>
        public static void SaveMyChildData(Child myChild, ChildAppearanceData appearanceData)
        {
            var list = LoadChildrenData();
            string uniqueKey = GetUniqueKey(myChild);
            var idx = list.FindIndex(d => d.UniqueKey == uniqueKey);
            if (idx >= 0) list[idx] = appearanceData;
            else list.Add(appearanceData);
            SaveChildrenData(list);
        }

        /// <summary>
        /// 자녀의 유니크 키(이름+부모ID)
        /// </summary>
        public static string GetUniqueKey(Child child)
            => child == null ? "" : $"{child.Name}_{child.idOfParent.Value}";
    }

    /// <summary>
    /// 자녀 외형/설정 데이터(확장성/호환성 최적화)
    /// </summary>
    public class ChildAppearanceData
    {
        public string UniqueKey { get; set; }         // ex) 이름_부모ID
        public string Name { get; set; }
        public long ParentID { get; set; }
        public bool IsMale { get; set; }
        public string SkinTone { get; set; }
        public string AppearanceKey { get; set; }
        // 자유 확장: public string HatKey, PajamaStyle, CustomPartsJson ...
        // public int FrameX { get; set; }
        // public int FrameY { get; set; }
    }
}