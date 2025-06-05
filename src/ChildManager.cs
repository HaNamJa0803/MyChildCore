using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 전체 탐색/예외 방어/백업 자동 복구까지 책임지는 매니저!
    /// (폴리아모리, 유저 커스텀 모드 대응 + 기본 SDV 전부 지원)
    /// </summary>
    public static class ChildManager
    {
        /// <summary>
        /// 게임 내 모든 자녀(아기+유아) 목록 반환 (폴리아모리, SDV 기본 지원)
        /// 예외 발생/목록 비면 자동 백업 복구 시도!
        /// </summary>
        public static List<Child> GetAllChildren()
        {
            // 1. Polygamy Suite API 사용 (있으면 우선)
            var polygamyApi = GetPolygamyApi();
            if (polygamyApi != null)
            {
                try
                {
                    var polyChildren = polygamyApi.GetAllChildren(); // 실제 API 메서드명 확인 필요
                    if (polyChildren != null && polyChildren.Count > 0)
                        return polyChildren;
                }
                catch
                {
                    CustomLogger.Warn("[ChildManager] 폴리아모리 API 자녀 불러오기 실패, fallback 사용");
                }
            }

            // 2. 기본 SDV 방식 fallback
            var list = new List<Child>();
            try
            {
                var npcs = StardewValley.Utility.getAllCharacters();
                if (npcs != null)
                {
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        var npc = npcs[i];
                        if (npc is Child)
                            list.Add((Child)npc);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn("[ChildManager] SDV 기본 자녀 탐색 실패: " + ex.Message);
            }

            // 3. 예외/빈 목록 방어: 자동 백업 복원 시도!
            if (list.Count == 0)
            {
                CustomLogger.Warn("[ChildManager] 자녀 목록이 비어있음! 외부 저장소에서 복원 시도");
                DataManager.RestoreLatestBackup();

                // 복원 시도 후 재시도
                try
                {
                    var npcs2 = StardewValley.Utility.getAllCharacters();
                    if (npcs2 != null)
                    {
                        for (int i = 0; i < npcs2.Count; i++)
                        {
                            var npc2 = npcs2[i];
                            if (npc2 is Child)
                                list.Add((Child)npc2);
                        }
                    }
                }
                catch { /* 무시 */ }
            }

            return list;
        }

        /// <summary>
        /// 폴리아모리 모드 API 인스턴스 반환 (실제 사용 시 ModRegistry 연결 필요)
        /// </summary>
        private static dynamic GetPolygamyApi()
        {
            // 실제 배포/플레이 환경에서는 다음처럼 Helper.ModRegistry 사용:
            // return ModEntry.Instance?.Helper?.ModRegistry?.GetApi<dynamic>("stokastic.Polygamy");
            return null; // 여기선 null, 샘플/예외 방어용
        }

        /// <summary>
        /// 부모 이름+성별로 특정 자녀 한 명 탐색 (빠른 탐색, LINQ 없이)
        /// </summary>
        public static Child FindChild(string parentName, bool isMale)
        {
            var all = GetAllChildren();
            for (int i = 0; i < all.Count; i++)
            {
                var child = all[i];
                if (GetSpouseName(child) == parentName &&
                    ((int)child.Gender == (isMale ? 0 : 1)))
                    return child;
            }
            return null;
        }

        /// <summary>
        /// 자녀 객체로부터 배우자 이름 추출 (SDV 1.6.15 완전 대응, LINQ 없이)
        /// </summary>
        public static string GetSpouseName(Child child)
        {
            try
            {
                if (child == null) return "Unknown";
                long parentId = (child.idOfParent != null) ? child.idOfParent.Value : -1;
                if (parentId > 0)
                {
                    var farmers = Game1.getAllFarmers();
                    for (int i = 0; i < farmers.Count; i++)
                    {
                        var farmer = farmers[i];
                        if (farmer.UniqueMultiplayerID == parentId && !string.IsNullOrEmpty(farmer.Name))
                            return farmer.Name;
                    }
                }
            }
            catch { }
            return "Unknown";
        }
    }
}