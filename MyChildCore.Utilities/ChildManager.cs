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
            => GetChildrenByPlayer().Where(c => (int)c.Gender == (isMale ? 0 : 1)).ToList();

        public static List<Child> FilterByParts(
            string? hatKey = null,
            string? hairKey = null,
            string? eyeKey = null,
            string? skinKey = null,
            string? topKey = null,
            string? bottomKey = null,
            string? neckCollarKey = null,
            string? shoesKey = null,
            string? clothesKey = null,
            string? pajamaStyle = null,
            string? pajamaColor = null)
        {
            return GetChildrenByPlayer().Where(child =>
            {
                var data = DataManager.LoadChildrenData()
                    .Find(d => d.Name == child.Name && d.ParentID == child.idOfParent.Value);

                if (data == null) return false;
                bool cond = true;
                if (!string.IsNullOrEmpty(hatKey)) cond &= (data.HatKey == hatKey);
                if (!string.IsNullOrEmpty(hairKey)) cond &= (data.HairKey == hairKey);
                if (!string.IsNullOrEmpty(eyeKey)) cond &= (data.EyeKey == eyeKey);
                if (!string.IsNullOrEmpty(skinKey)) cond &= (data.SkinKey == skinKey);
                if (!string.IsNullOrEmpty(topKey)) cond &= (data.TopKey == topKey);
                if (!string.IsNullOrEmpty(bottomKey)) cond &= (data.BottomKey == bottomKey);
                if (!string.IsNullOrEmpty(neckCollarKey)) cond &= (data.NeckCollarKey == neckCollarKey);
                if (!string.IsNullOrEmpty(shoesKey)) cond &= (data.ShoesKey == shoesKey);
                if (!string.IsNullOrEmpty(clothesKey)) cond &= (data.ClothesKey == clothesKey);
                if (!string.IsNullOrEmpty(pajamaStyle)) cond &= (data.PajamaStyle == pajamaStyle);
                if (!string.IsNullOrEmpty(pajamaColor)) cond &= (data.PajamaColor == pajamaColor);
                return cond;
            }).ToList();
        }

        public static string ChildToString(Child child)
        {
            if (child == null)
                return "(null)";

            return $"Child(Name: {child.Name}, ParentID: {child.idOfParent?.Value}, " +
                   $"Gender: {(child.Gender == 0 ? "Male" : "Female")}, " +
                   $"Age: {child.Age})";
        }

        public static void LogAllChildren()
        {
            var all = GetAllChildren();
            CustomLogger.Debug($"[ChildManager] 현재 존재하는 자녀 {all.Count}명:");
            foreach (var c in all)
            {
                CustomLogger.Debug($"  - {ChildToString(c)}");
            }
        }
    }
}