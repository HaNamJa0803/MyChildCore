using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        public static List<Child> GetAllChildren()
        {
            return Utility.getAllCharacters().OfType<Child>().ToList();
        }

        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            return GetAllChildren().Where(c => c.idOfParent.Value == Game1.player.UniqueMultiplayerID).ToList();
        }

        public static Child GetMyFirstChild()
        {
            return GetChildrenByPlayer().FirstOrDefault();
        }
    }
}