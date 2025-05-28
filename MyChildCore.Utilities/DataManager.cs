using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 데이터 (JSON) 세이브/로드 전용 헬퍼 (SDV 1.6.10+, SMAPI 4.x+)
    /// </summary>
    public static class DataManager
    {
        // SMAPI 4.x+ 환경에서 세이브별 저장 경로 안전하게 반환!
        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 자녀 데이터 JSON으로 저장
        /// </summary>
        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;
            try
            {
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
            }
            catch (Exception ex)
            {
                // 실전에서는 반드시 로그 남기세요!
                // ex.Message로 모드 충돌/경로 권한 등 디버깅 가능
                // 예: MyLogger.Warn($"[DataManager] Save 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 자녀 데이터 JSON에서 읽기
        /// </summary>
        public static List<ChildData> LoadChildrenData()
        {
            if (!File.Exists(StoragePath))
                return new List<ChildData>();
            try
            {
                string json = File.ReadAllText(StoragePath);
                return JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
            }
            catch (Exception ex)
            {
                // 실전 로그: MyLogger.Warn($"[DataManager] Load 실패: {ex.Message}");
                return new List<ChildData>();
            }
        }
    }
}