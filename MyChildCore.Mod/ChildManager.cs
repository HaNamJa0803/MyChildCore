using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀(Child) 통합 관리 매니저 (1.6.10+ 실전, 캐싱/필터/확장성 완비)
    /// </summary>
    public static class ChildManager
    {
        // 일별 자녀 캐싱 (필요시 InvalidateCache로 재생성)
        private static List<Child> _cachedChildren = null;
        private static int _lastDayCached = -1;

        /// <summary>
        /// 세이브 전체 자녀 리스트 (캐시)
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
        /// 현재 플레이어 자녀 리스트 (유니크 칠드런 기준)
        /// </summary>
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            return GetAllChildren().Where(c => c.idOfParent.Value == Game1.player.UniqueMultiplayerID).ToList();
        }

        /// <summary>
        /// 현재 플레이어의 첫 번째 자녀 반환 (없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
        {
            return GetChildrenByPlayer().FirstOrDefault();
        }

        /// <summary>
        /// 자녀 성별 필터 (확장성)
        /// </summary>
        public static List<Child> FilterByGender(List<Child> children, bool isMale)
        {
            return children.Where(c => c.Gender == (isMale ? Child.Gender.Male : Child.Gender.Female)).ToList();
        }

        /// <summary>
        /// 복합 필터: 성별/나이/부모ID/이름 등
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

        /// <summary>
        /// 캐시 무효화 (SaveLoaded/DayStarted 등에서 사용)
        /// </summary>
        public static void InvalidateCache()
        {
            _cachedChildren = null;
            _lastDayCached = -1;
        }

        // 추가 기능(자녀 추가/삭제, GMCM 연동 등) 필요시 계속 확장
    }
}