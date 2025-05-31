using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 게임 내 모든 자녀(아기+유아) 관리를 위한 매니저
    /// </summary>
    public static class ChildManager
    {
        // ────────────── 아기(BABY) 관련 ──────────────

        /// <summary>
        /// 현재 게임에 존재하는 모든 "아기(BABY)" 상태의 Child 리스트 반환
        /// Child.Age == 0 (아기)
        /// </summary>
        public static List<Child> GetAllBabies()
            => GetAllChildren().Where(c => c.Age == 0).ToList();

        /// <summary>
        /// 현재 플레이어(본인)가 부모인 "아기(BABY)" 상태의 Child 리스트 반환
        /// </summary>
        public static List<Child> GetBabiesByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllBabies().Where(b => b.idOfParent?.Value == playerId).ToList();
        }

        // ────────────── 유아(TODDLER) 관련 ──────────────

        /// <summary>
        /// 현재 게임에 존재하는 모든 "유아(TODDLER)" 상태의 Child 리스트 반환
        /// Child.Age == 1 (유아)
        /// </summary>
        public static List<Child> GetAllToddlers()
            => GetAllChildren().Where(c => c.Age == 1).ToList();

        /// <summary>
        /// 현재 플레이어(본인)가 부모인 "유아(TODDLER)" 상태의 Child 리스트 반환
        /// </summary>
        public static List<Child> GetToddlersByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllToddlers().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        // ────────────── 공통(ALL CHILDREN) 관련 ──────────────

        /// <summary>
        /// 현재 게임에 존재하는 모든 자녀(Child) 리스트 반환 (아기+유아 포함)
        /// </summary>
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        /// <summary>
        /// 현재 플레이어(본인)가 부모인 자녀(Child) 리스트 반환 (아기+유아 포함)
        /// </summary>
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        /// <summary>
        /// 현재 플레이어(본인)가 부모인 첫 번째 자녀 반환 (없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        /// <summary>
        /// 성별 기준으로 자녀 필터링 (isMale: true=남아, false=여아)
        /// </summary>
        public static List<Child> FilterByGender(bool isMale)
            => GetAllChildren().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();
    }
}