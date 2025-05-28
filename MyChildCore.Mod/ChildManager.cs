using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class ChildManager
    {
        private static List<Child> _cachedChildren = null;
        private static int _lastDayCached = -1;

        public static List<Child> GetAllChildren()
        {
            if (Game1.dayOfMonth != _lastDayCached || _cachedChildren == null)
            {
                _cachedChildren = Utility.getAllCharacters().OfType<Child>().ToList();
                _lastDayCached = Game1.dayOfMonth;
            }
            return _cachedChildren ?? new List<Child>();
        }

        public static List<Child> GetChildrenByPlayer()
        {
            if (Game1.player == null)
                return new List<Child>();
            return GetAllChildren().Where(c => c.idOfParent.Value == Game1.player.UniqueMultiplayerID).ToList();
        }

        public static Child GetMyFirstChild()
        {
            var list = GetChildrenByPlayer();
            return list.FirstOrDefault();
        }

        public static List<Child> FilterByGender(List<Child> children, bool isMale)
        {
            return children.Where(c => c.Gender == (isMale ? Child.Gender.Male : Child.Gender.Female)).ToList();
        }

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
    }
}