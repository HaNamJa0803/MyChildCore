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
    /// </summary>
    public static class DataManager
    {
        // ───────────── 연동되는 유틸리티 및 데이터 ─────────────
        // ChildData: 파츠 DTO/데이터 구조
        // ChildManager: 게임 내 자녀 리스트/필터 등 실시간 연산
        // CacheManager: 데이터 백업/복구/임시 캐싱
        // CustomLogger: 실전 상태/오류/트러블 로그 기록
        // EventManager: 외형동기화, 자동화, 이벤트 트리거 전담

        private static string StoragePath =>
            Path.Combine(Constants.CurrentSavePath ?? "", "MyChildData.json");

        /// <summary>
        /// 자녀 데이터 외부 저장 + 캐시 동기화 + 로그 기록
        /// </summary>
        public static void SaveChildrenData(List<ChildData> childrenData)
        {
            if (childrenData == null) return;
            try
            {
                // 1. 파일 저장
                string json = JsonConvert.SerializeObject(childrenData, Formatting.Indented);
                File.WriteAllText(StoragePath, json);

                // 2. 캐시에도 동기화
                CacheManager.Set("MyChildData", childrenData);

                // 3. 캐시 파일 백업
                CacheManager.SaveCacheToFile(StoragePath + ".cache");

                // 4. 로그
                CustomLogger.Info($"[DataManager] 자녀 데이터 저장: {StoragePath} ({childrenData.Count}명)");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[DataManager] Save 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 외부 저장소/캐시에서 자녀 데이터 불러오기 + 캐시 동기화 + 로그
        /// </summary>
        public static List<ChildData> LoadChildrenData()
        {
            // 캐시 우선 사용
            var cached = CacheManager.Get<List<ChildData>>("MyChildData");
            if (cached != null)
                return cached;

            if (!File.Exists(StoragePath))
                return new List<ChildData>();

            try
            {
                string json = File.ReadAllText(StoragePath);
                var data = JsonConvert.DeserializeObject<List<ChildData>>(json) ?? new List<ChildData>();
                CacheManager.Set("MyChildData", data); // 캐시에도 저장
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
        /// 캐시 파일에서 데이터 복구 (비상 복구/핫리로드)
        /// </summary>
        public static void RestoreChildrenDataFromCache()
        {
            CacheManager.RestoreFromLastSave();
            CustomLogger.Info("[DataManager] 캐시 백업 파일에서 자녀 데이터 복구 시도");
        }

        /// <summary>
        /// 모든 자녀 데이터 “분기+물량공세”로 게임 내 오브젝트에 동기화
        /// </summary>
        public static void ApplyAllChildDataFromStorage()
        {
            var loaded = LoadChildrenData();
            if (loaded.Count == 0) return;

            // ChildManager를 통한 전체 Child 순회(완전 연동)
            foreach (var child in ChildManager.GetAllChildren())
            {
                var data = loaded.Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);
                if (data == null) continue;

                // ────────── 분기/상태 자동화 ──────────
                // 축제/잠옷/평상복 등 외형 적용 분기
                if (Game1.isFestivalDay || Game1.isFestival)
                {
                    AppearanceManager.ApplyParts(
                        child,
                        data.HatKey, data.HairKey, data.EyeKey, data.SkinKey,
                        data.TopKey, data.BottomKey, data.NeckCollarKey, data.ShoesKey,
                        data.ClothesKey, null, null // 축제: 통합복/특정 파츠만 적용
                    );
                    continue;
                }
                if (Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600)
                {
                    AppearanceManager.ApplyParts(
                        child,
                        data.HatKey, data.HairKey, data.EyeKey, data.SkinKey,
                        null, null, null, null, null, data.PajamaStyle, data.PajamaColor // 잠옷 전환
                    );
                    continue;
                }
                // 평상복(모든 파츠 하드코딩 적용)
                AppearanceManager.ApplyParts(
                    child,
                    data.HatKey, data.HairKey, data.EyeKey, data.SkinKey,
                    data.TopKey, data.BottomKey, data.NeckCollarKey, data.ShoesKey,
                    data.ClothesKey, null, null
                );
            }
            CustomLogger.Info("[DataManager] 자녀 외형(파츠) 분기+물량공세 동기화 완료!");
        }

        /// <summary>
        /// (EventManager 연동) 외형 자동 동기화 이벤트 트리거용
        /// </summary>
        public static void RegisterToEventManager(IModHelper helper, IMonitor monitor)
        {
            EventManager.HookAll(helper, monitor, ApplyAllChildDataFromStorage);
            CustomLogger.Info("[DataManager] EventManager에 외형 동기화 콜백 등록 완료!");
        }
    }
}