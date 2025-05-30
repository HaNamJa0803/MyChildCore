using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        // 모든 Child 리스트 반환
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        // 현재 플레이어(호스트)의 모든 자녀 반환
        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        // 내 첫번째 자녀 (없으면 null)
        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        // 성별 필터 (true=남, false=여)
        public static List<Child> FilterByGender(bool isMale)
            => GetAllChildren().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();
    }
}