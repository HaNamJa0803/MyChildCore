using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// [허브] 자녀 데이터/파츠/캐시/로그/이벤트 완전 연동 핵심 DataManager
    /// - 모든 파츠, 데이터 흐름, 로그, 캐시, 자녀관리, 이벤트까지 이곳에서 통제
    /// - 성능보다 안정성/강제성/정확성 우선 (스타듀 엔진의 외형 강제 변경 방지)
    /// </summary>
    public static class DataManager
    {
        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 자녀 데이터 저장 (JSON + 캐시 + 로그)
        /// </summary>
        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;

            try
            {
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
                CacheManager.Set("MyChildData", childrenData);
                CacheManager.SaveCacheToFile(StoragePath + ".cache");
                CustomLogger.Info($"[DataManager] 자녀 데이터 저장: {StoragePath} ({childrenData.Count}명)");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[DataManager] Save 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 자녀 데이터 로드 (캐시 우선, 없으면 파일)
        /// </summary>
        public static List<ChildData> LoadChildrenData()
        {
            var cached = CacheManager.Get<List<ChildData>>("MyChildData");
            if (cached != null) return cached;

            if (!File.Exists(StoragePath))
                return new List<ChildData>();

            try
            {
                string json = File.ReadAllText(StoragePath);
                var data = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.Set("MyChildData", data);
                CustomLogger.Debug($"[DataManager] 파일에서 자녀 데이터 로드 및 캐시 동기화");
                return data;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[DataManager] Load 실패: {ex.Message}");
                return new List<ChildData>();
            }
        }

        /// <summary>
        /// 캐시 백업 파일에서 데이터 복구 (비상용)
        /// </summary>
        public static void RestoreChildrenDataFromCache()
        {
            CacheManager.RestoreFromLastSave();
            CustomLogger.Info("[DataManager] 캐시 백업 파일에서 자녀 데이터 복구 시도");
        }

        /// <summary>
        /// 자녀 외형 데이터 강제 적용 (시간/축제/기본 분기, 하드코딩 방식)
        /// - 게임 엔진의 강제 초기화 방지
        /// - 매번 전체 순회, 매번 강제 적용 (성능 < 정확성)
        /// </summary>
        public static void ApplyAllChildDataFromStorage()
        {
            var loaded = LoadChildrenData();
            if (loaded.Count == 0) return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                var data = loaded.Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);
                if (data == null) continue;

                // 1️⃣ 아기(Baby) → 고정 파츠만 (AppearanceManager 내부 처리)
                if (child.Age == 0)
                {
                    AppearanceManager.ApplyParts(child, data);
                    continue;
                }

                // 2️⃣ 잠옷 시간대 (밤 6시~6시) → 잠옷 강제 적용
                if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
                {
                    AppearanceManager.ApplyParts(child, data);
                    continue;
                }

                // 3️⃣ 축제일 → 시즌별 하드코딩 파츠 강제 적용
                if (Game1.CurrentEvent?.isFestival == true)
                {
                    AppearanceManager.ApplyParts(child, data);
                    continue;
                }

                // 4️⃣ 기본 상태 → 선택 파츠 강제 적용
                AppearanceManager.ApplyParts(child, data);
            }

            CustomLogger.Info("[DataManager] 자녀 외형(파츠) 강제 동기화 완료!");
        }

        /// <summary>
        /// EventManager에 외형 동기화 콜백 등록 (하드코딩 구조 유지)
        /// </summary>
        public static void RegisterToEventManager(IModHelper helper, IMonitor monitor)
        {
            EventManager.HookAll(helper, monitor, ApplyAllChildDataFromStorage);
            CustomLogger.Info("[DataManager] EventManager에 외형 동기화 콜백 등록 완료!");
        }
    }
}