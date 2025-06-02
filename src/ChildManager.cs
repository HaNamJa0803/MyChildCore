using System.Linq;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Characters;
using MyChildCore;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        // 폴리아모리 API(있을 때 우선) + 기본 SDV 자녀 반환
        public static List<Child> GetAllChildren()
        {
            // Polygamy Suite API 인스턴스 가져오기 (예시)
            var polygamyApi = GetPolygamyApi();
            if (polygamyApi != null)
            {
                try
                {
                    var polyChildren = polygamyApi.GetAllChildren(); // 실제 API 메서드명 확인 필요
                    if (polyChildren != null && polyChildren.Count > 0)
                        return polyChildren;
                }
                catch { /* 예외 무시, fallback 사용 */ }
            }

            // 기본 SDV 방식 fallback
            var list = new List<Child>();
            var npcs = StardewValley.Utility.getAllCharacters();
            if (npcs != null)
            {
                foreach (var npc in npcs)
                {
                    if (npc is Child child)
                        list.Add(child);
                }
            }
            return list;
        }

        // 폴리아모리 API 획득 함수 (예시)
        private static dynamic GetPolygamyApi()
        {
            // 실제 모드 환경에서는 ModRegistry에서 API를 획득해야 함
            // 예시: Helper.ModRegistry.GetApi<dynamic>("stokastic.Polygamy")
            return null;
        }

        // 부모+성별로 특정 자녀 탐색
        public static Child FindChild(string parentName, bool isMale)
        {
            var all = GetAllChildren();
            return all.Find(child =>
                GetSpouseName(child) == parentName &&
                ((int)child.Gender == (isMale ? 0 : 1)));
        }

        // 부모 이름 추출 (SDV 1.6.15 대응)
        public static string GetSpouseName(Child child)
        {
            try
            {
                if (child == null) return "Unknown";
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    var parent = Game1.getAllFarmers().FirstOrDefault(f => f.UniqueMultiplayerID == parentId);
                    if (parent != null && !string.IsNullOrEmpty(parent.Name))
                        return parent.Name;
                }
            }
            catch { }
            return "Unknown";
        }
    }
}