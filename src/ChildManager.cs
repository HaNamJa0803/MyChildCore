using MyChildCore;
using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 유니크 칠드런식 자녀(Child) 매니저:
    /// - 배우자(플레이어/결혼 가능 NPC) 기준 1:N 구조
    /// - 세이브/외부데이터/실시간 완전 동기화 지원
    /// - Observer, GMCM, 캐시, 외형 일괄 적용 지원
    /// </summary>
    public static class ChildManager
    {
        // === Observer 패턴 (외부에서 구독) ===
        public static event Action<List<Child>> OnChildrenSynced;
        public static event Action<Child, ChildParts> OnChildAppearanceUpdated;

        /// <summary>
        /// 모든 결혼 가능한 NPC(플레이어 포함) 리스트
        /// </summary>
        public static List<NPC> GetAllSpouses()
        {
            var all = new List<NPC>();
            foreach (var npc in Utility.getAllCharacters())
            {
                if (npc is NPC candidate && (candidate.isVillager() && (candidate.datable.Value || candidate.isMarried())))
                    all.Add(candidate);
            }
            // 플레이어도 spouse 목록에 포함
            all.Add(Game1.player);
            return all;
        }

        /// <summary>
        /// 모든 자녀 객체 리스트 (모든 지역)
        /// </summary>
        public static List<Child> GetAllChildren()
        {
            var result = new List<Child>();
            foreach (var loc in Game1.locations.ToList())
            {
                foreach (var npc in loc.characters.ToList())
                {
                    if (npc is Child child)
                        result.Add(child);
                }
            }
            return result;
        }

        /// <summary>
        /// 각 배우자별 자녀 리스트 (1:N 구조, key=배우자명)
        /// </summary>
        public static Dictionary<string, List<Child>> GetChildrenBySpouse()
        {
            var dict = new Dictionary<string, List<Child>>();
            var children = GetAllChildren();

            foreach (var child in children)
            {
                var spouseName = AppearanceManager.GetRealSpouseName(child);
                if (string.IsNullOrEmpty(spouseName)) spouseName = "Default";
                if (!dict.ContainsKey(spouseName))
                    dict[spouseName] = new List<Child>();
                dict[spouseName].Add(child);
            }
            return dict;
        }

        /// <summary>
        /// 게임 내 자녀 → 캐시/외부저장소 동기화 (Observer 호출)
        /// </summary>
        public static void OverwriteCacheFromGame(IModHelper helper)
        {
            var children = GetAllChildren();
            var dataList = new List<ChildData>();
            foreach (var child in children)
            {
                dataList.Add(ChildData.FromChild(child));
            }
            CacheManager.SetChildCache(helper, dataList);
            NotifyChildrenSynced(children);
        }

        /// <summary>
        /// 캐시/외부저장소 → 게임 내 자녀 반영 (외형 동기화)
        /// </summary>
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
                    NotifyChildAppearanceUpdated(child, parts);
                }
            }
            NotifyChildrenSynced(children);
        }

        /// <summary>
        /// [확장] 모든 배우자 기준 자녀 동기화 (GMCM/옵션/UI 등)
        /// </summary>
        public static void SyncAllBySpouse(ModConfig config)
        {
            var dict = GetChildrenBySpouse();
            foreach (var pair in dict)
            {
                var spouseName = pair.Key;
                var children = pair.Value;
                foreach (var child in children)
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
                    NotifyChildAppearanceUpdated(child, parts);
                }
            }
            NotifyChildrenSynced(GetAllChildren());
        }

        /// <summary>
        /// 외형 일괄 적용(캐시/외부 데이터 무시, 무조건 config 기준 재적용)
        /// </summary>
        public static void ForceAllChildrenAppearance(ModConfig config)
        {
            var children = GetAllChildren();
            foreach (var child in children)
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
                NotifyChildAppearanceUpdated(child, parts);
            }
            NotifyChildrenSynced(children);
        }

        // ==== 캐시 필터링 ====
        public static List<ChildData> GetBabiesFromCache()      => CacheManager.GetBabies();
        public static List<ChildData> GetToddlersFromCache()    => CacheManager.GetToddlers();
        public static List<ChildData> GetByGenderFromCache(string gender) => CacheManager.GetByGender(gender);
        public static List<ChildData> GetBySpouseFromCache(string spouse) => CacheManager.GetBySpouse(spouse);

        /// <summary>
        /// 실제 존재하는 모든 배우자명(중복 제거)
        /// </summary>
        public static IEnumerable<string> GetAllSpouseNames()
        {
            return GetAllSpouses().Select(s => s.Name).Distinct().ToList();
        }

        // ==== Observer 알림 ====
        private static void NotifyChildrenSynced(List<Child> children)
        {
            OnChildrenSynced?.Invoke(children);
        }

        private static void NotifyChildAppearanceUpdated(Child child, ChildParts parts)
        {
            OnChildAppearanceUpdated?.Invoke(child, parts);
        }
    }
}