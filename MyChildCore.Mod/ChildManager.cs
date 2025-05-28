using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        // 전체 자녀 리스트 캐싱(옵션)
        private static List<Child> _cachedChildren = null;
        private static int _lastDayCached = -1;

        /// <summary>
        /// 모든 자녀(Child) 리스트 (캐싱)
        /// </summary>
        public static List<Child> GetAllChildren()
        {
            if (Game1.dayOfMonth != _lastDayCached || _cachedChildren == null)
            {
                _cachedChildren = Utility.getAllCharacters().OfType<Child>().ToList();
                _lastDayCached = Game1.dayOfMonth;
            }
            return _cachedChildren ?? new List<Child>();
        }

        /// <summary>
        /// 현재 플레이어의 자녀 전체 (유니크 칠드런 방식)
        /// </summary>
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            return GetAllChildren().Where(c => c.idOfParent.Value == Game1.player.UniqueMultiplayerID).ToList();
        }

        /// <summary>
        /// 현재 플레이어의 첫 번째 자녀 (없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
        {
            var list = GetChildrenByPlayer();
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 성별별로 자녀 필터 (확장성)
        /// </summary>
        public static List<Child> FilterByGender(List<Child> children, bool isMale)
        {
            return children.Where(c => c.Gender == (isMale ? Child.Gender.Male : Child.Gender.Female)).ToList();
        }

        /// <summary>
        /// 나이, 부모, 이름 등 복합 조건 필터 (확장 구조)
        /// </summary>
        public static List<Child> FilterChildren(
            bool? isMale = null, int? age = null, long? parentId = null, string name = null)
        {
            IEnumerable<Child> children = GetAllChildren();
            if (isMale.HasValue)
                children = children.Where(c => c.Gender == (isMale.Value ? Child.Gender.Male : Child.Gender.Female));
            if (age.HasValue)
                children = children.Where(c => c.Age == age.Value);
            if (parentId.HasValue)
                children = children.Where(c => c.idOfParent.Value == parentId.Value);
            if (!string.IsNullOrEmpty(name))
                children = children.Where(c => c.Name == name);
            return children.ToList();
        }

        // (추가 확장) 자녀 삭제, 정보 갱신 등 기능도 여기서 구현 가능
    }
}