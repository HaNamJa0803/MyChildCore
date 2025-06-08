using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Characters;
using System.Linq;

namespace MyChildCore
{
    /// <summary>
    /// 자녀(Child) 리스트/외부저장소 동기화 & 예외복구/외형 일괄 적용 (유니크 칠드런식, 최신)
    /// </summary>
    public static class ChildManager
    {
        // === 1. 게임 내 모든 자녀 안전하게 반환 ===
        public static List<Child> GetAllChildren()
        {
            try
            {
                var result = new HashSet<Child>();

                foreach (var farmer in Game1.getAllFarmers())
                {
                    if (farmer == null || farmer.homeLocation == null) continue;

                    // ★ 타입 명시적으로!
                    var locations = new GameLocation[] { farmer.homeLocation, farmer.currentLocation }
                        .Where(l => l != null)
                        .Distinct();

                    foreach (var location in locations)
                        foreach (var npc in location.characters)
                            if (npc is Child child && child != null)
                                result.Add(child);
                }
                return result.ToList();
            }
            catch (Exception ex)
            {
                CustomLogger.Error("[ChildManager] GetAllChildren 예외: " + ex.Message);
                return new List<Child>();
            }
        }

        // === 2. 외부저장소(캐시) 모든 자녀 데이터 조회 ===
        public static List<ChildData> GetAllChildDataFromCache()
            => CacheManager.GetChildCache();

        // === 3. 캐시 → 게임 자녀 객체 외형 자동 동기화 ===
        public static void SyncGameChildrenWithCache()
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
                        var config = ModEntry.Config;
                        var parts = child.Age == 0
                            ? PartsManager.GetPartsForBaby(child, config)
                            : PartsManager.GetPartsForChild(child, config);

                        if (parts == null || !PartsManager.HasAllRequiredParts(parts))
                        {
                            CustomLogger.Warn("[ChildManager] 외형 파츠 누락: " + child.Name + " → Default로 복구!");
                            parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                        }

                        if (child.Age == 0)
                            AppearanceManager.ApplyBabyAppearance(child, parts);
                        else
                            AppearanceManager.ApplyToddlerAppearance(child, parts);

                        // 최신화: ResourceManager로 캐시 무효화!
                        ResourceManager.InvalidateChildSprite(child.Name);
                    }
                    catch (Exception ex)
                    {
                        CustomLogger.Error("[ChildManager] 외형 적용 예외: " + child.Name + ": " + ex.Message);
                    }
                }
            }
        }

        // === 4. 게임 내 자녀 → 외부저장소(캐시)로 덮어쓰기 ===
        public static void OverwriteCacheFromGame()
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

            CacheManager.SetChildCache(dataList);
            CustomLogger.Info("[ChildManager] 게임 내 자녀 " + dataList.Count + "명 → 외부저장소 덮어쓰기 완료");
        }

        // === 5. 자녀 개별 필터 (성별, 나이 등) ===
        public static List<ChildData> GetBabiesFromCache()      => CacheManager.GetBabies();
        public static List<ChildData> GetToddlersFromCache()    => CacheManager.GetToddlers();
        public static List<ChildData> GetByGenderFromCache(string gender) => CacheManager.GetByGender(gender);
        public static List<ChildData> GetBySpouseFromCache(string spouse) => CacheManager.GetBySpouse(spouse);

        // === 6. 자녀 데이터 일괄 외형 적용(복구까지, 유니크 칠드런식) ===
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
                    {
                        CustomLogger.Warn("[ChildManager] 외형 파츠 누락: " + child.Name + " → Default로 복구!");
                        parts = PartsManager.GetDefaultParts(child, child.Age == 0);
                    }

                    if (child.Age == 0)
                        AppearanceManager.ApplyBabyAppearance(child, parts);
                    else
                        AppearanceManager.ApplyToddlerAppearance(child, parts);

                    // 최신화: ResourceManager로 캐시 무효화!
                    ResourceManager.InvalidateChildSprite(child.Name);
                }
                catch (Exception ex)
                {
                    CustomLogger.Error("[ChildManager] ForceAllChildrenAppearance 예외: " + (child != null ? child.Name : "null") + ": " + ex.Message);
                }
            }
        }
    }
}