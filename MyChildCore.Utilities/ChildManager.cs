using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 조회/필터/검색 등 관리 유틸리티 (1.6.10+ 실전 대응)
    /// </summary>
    public static class ChildManager
    {
        /// <summary>
        /// 게임 내 모든 Child 객체 리스트 (유저, NPC 등 모두 포함)
        /// </summary>
        public static List<Child> GetAllChildren()
        {
            // 1.6.10+에서 여전히 지원. (혹시 구조 바뀌면 유틸리티/리플렉션 응용)
            return Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();
        }

        /// <summary>
        /// 현재 플레이어의 모든 자녀만 반환
        /// </summary>
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        /// <summary>
        /// 내 첫 번째 자녀 (없으면 null)
        /// </summary>
        public static Child GetMyFirstChild()
        {
            return GetChildrenByPlayer().FirstOrDefault();
        }
    }
}