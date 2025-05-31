using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        public static List<Child> GetAllChildren()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllChildren().Where(c => c.idOfParent?.Value == playerId).ToList();
        }

        public static Child GetMyFirstChild()
            => GetChildrenByPlayer().FirstOrDefault();

        public static List<Child> FilterByGender(bool isMale)
            => GetAllChildren().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();

        // ========== Child 호출 메서드 추가 ==========
        public static List<Child> GetAllBabies()
            => Utility.getAllCharacters()?.OfType<Child>()?.ToList() ?? new List<Child>();

        public static List<Child> GetBabiesByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            var playerId = Game1.player.UniqueMultiplayerID;
            return GetAllBabies().Where(b => b.idOfParent?.Value == playerId).ToList();
        }

        public static Child GetMyFirstChild()
            => GetBabiesByPlayer().FirstOrDefault();
    }
}