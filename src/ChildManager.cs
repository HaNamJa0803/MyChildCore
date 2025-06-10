using MyChildCore;
using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace MyChildCore
{
    /// <summary>
    /// 유니크 칠드런식 자녀(Child) 매니저: 세이브/외부 데이터 ↔ 게임 내 자녀 완전 동기화 + 실시간 연결 지원
    /// </summary>
    public static class ChildManager
    {
        // Observer 패턴: 자녀 데이터/외형 동기화 알림 (예: 캐시, 외형, 리소스 등에서 구독)
        public static event Action<List<Child>> OnChildrenSynced;
        public static event Action<Child, ChildParts> OnChildAppearanceUpdated;

        // 1. 모든 게임 내 자녀 객체 반환 (모든 플레이어, 모든 지역)
        public static List<Child> GetAllChildren()
        {
            var result = new List<Child>();
            try
            {
                foreach (var farmer in Game1.getAllFarmers())
                {
                    foreach (var loc in Game1.locations)
                    {
                        foreach (var npc in loc.characters)
                        {
                            if (npc is Child child && child != null)
                                result.Add(child);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Error("[ChildManager] GetAllChildren 예외: " + ex.Message);
            }
            return result;
        }

        // 2. 게임 내 자녀 → 캐시/외부저장소 동기화 (즉시 알림)
        public static void OverwriteCacheFromGame(IModHelper helper)
        {
            var children = GetAllChildren();
            var dataList = new List<ChildData>();
            foreach (var child in children)
            {
                try
                {
                    dataList.Add(ChildData.FromChild(child));
                }
                catch (Exception ex)
                {
                    CustomLogger.Error("[ChildManager] FromChild 변환 예외: " + (child != null ? child.Name : "null") + ": " + ex.Message);
                }
            }
            CacheManager.SetChildCache(helper, dataList);
            CustomLogger.Info("[ChildManager] 게임 내 자녀 " + dataList.Count + "명 → 캐시/외부저장소 덮어쓰기 완료");
            NotifyChildrenSynced(children);
        }

        // 3. 캐시/외부저장소 → 게임 내 자녀 반영 (외형 동기화, Observer 알림)
        public static void SyncGameChildrenWithCache(ModConfig config)
        {
            var cacheList = CacheManager.GetChildCache();
            var children = GetAllChildren();
            foreach (var cached in cacheList)
            {
                var child = children.FirstOrDefault(
                    c => c.Name == cached.Name && (int)c.Gender == cached.Gender && c.Age == cached.Age
                );
                if (child != null)
                {
                    try
                    {
                        var parts = child.Age == 0
                            ? PartsManager.GetPartsForBaby(child, config)
                            : PartsManager.GetPartsForChild(child, config);

                        if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                            parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                        if (child.Age == 0)
                            AppearanceManager.ApplyBabyAppearance(child, parts);
                        else
                            AppearanceManager.ApplyToddlerAppearance(child, parts);

                        ResourceManager.InvalidateChildSprite(child.Name);

                        // 알림: 자녀 외형 업데이트
                        NotifyChildAppearanceUpdated(child, parts);
                    }
                    catch (Exception ex)
                    {
                        CustomLogger.Error("[ChildManager] 외형 적용 예외: " + child.Name + ": " + ex.Message);
                    }
                }
            }
            NotifyChildrenSynced(children);
        }

        // 4. 외형 일괄 적용(캐시/외부 데이터 무시, 무조건 config 기준 재적용, Observer 알림)
        public static void ForceAllChildrenAppearance(ModConfig config)
        {
            var children = GetAllChildren();
            foreach (var child in children)
            {
                try
                {
                    var parts = child.Age == 0
                        ? PartsManager.GetPartsForBaby(child, config)
                        : PartsManager.GetPartsForChild(child, config);

                    if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);

                    if (child.Age == 0)
                        AppearanceManager.ApplyBabyAppearance(child, parts);
                    else
                        AppearanceManager.ApplyToddlerAppearance(child, parts);

                    ResourceManager.InvalidateChildSprite(child.Name);

                    // 알림: 자녀 외형 업데이트
                    NotifyChildAppearanceUpdated(child, parts);
                }
                catch (Exception ex)
                {
                    CustomLogger.Error("[ChildManager] ForceAllChildrenAppearance 예외: " + (child != null ? child.Name : "null") + ": " + ex.Message);
                }
            }
            NotifyChildrenSynced(children);
        }

        // 5. 캐시 필터링 메서드 (기존 그대로)
        public static List<ChildData> GetBabiesFromCache()      => CacheManager.GetBabies();
        public static List<ChildData> GetToddlersFromCache()    => CacheManager.GetToddlers();
        public static List<ChildData> GetByGenderFromCache(string gender) => CacheManager.GetByGender(gender);
        public static List<ChildData> GetBySpouseFromCache(string spouse) => CacheManager.GetBySpouse(spouse);

        // Observer 알림 호출: 전체 자녀 동기화 완료 시
        private static void NotifyChildrenSynced(List<Child> children)
        {
            try
            {
                OnChildrenSynced?.Invoke(children);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn("[ChildManager] OnChildrenSynced 알림 예외: " + ex.Message);
            }
        }

        // Observer 알림 호출: 자녀 외형 업데이트 시
        private static void NotifyChildAppearanceUpdated(Child child, ChildParts parts)
        {
            try
            {
                OnChildAppearanceUpdated?.Invoke(child, parts);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn("[ChildManager] OnChildAppearanceUpdated 알림 예외: " + ex.Message);
            }
        }
    }
}