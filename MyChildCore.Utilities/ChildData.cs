using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public string HatKey { get; set; }
        public string HairKey { get; set; }
        public string EyeKey { get; set; }
        public string SkinKey { get; set; }
        public string TopKey { get; set; }
        public string BottomKey { get; set; }
        public string NeckCollarKey { get; set; }
        public string ShoesKey { get; set; }
        public string ClothesKey { get; set; }
        public string PajamaStyle { get; set; }
        public string PajamaColor { get; set; }
        public string FaceSkinKey { get; set; }
        public string BodyKey { get; set; }

        public ChildData(Child child,
            string hatKey = "", string hairKey = "", string eyeKey = "", string skinKey = "",
            string topKey = "", string bottomKey = "", string neckCollarKey = "",
            string shoesKey = "", string clothesKey = "",
            string pajamaStyle = "", string pajamaColor = "",
            string faceSkinKey = "", string bodyKey = "")
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            Name = child.Name;
            ParentID = child.idOfParent?.Value ?? 0;
            Gender = (int)child.Gender;

            HatKey = hatKey;
            HairKey = hairKey;
            EyeKey = eyeKey;
            SkinKey = skinKey;
            TopKey = topKey;
            BottomKey = bottomKey;
            NeckCollarKey = neckCollarKey;
            ShoesKey = shoesKey;
            ClothesKey = clothesKey;
            PajamaStyle = pajamaStyle;
            PajamaColor = pajamaColor;
            FaceSkinKey = faceSkinKey;
            BodyKey = bodyKey;
        }

        public ChildData() { }

        public override string ToString()
        {
            return $"ChildData(Name: {Name}, ParentID: {ParentID}, Gender: {(Gender == 0 ? "Male" : "Female")}, " +
                   $"Hat: {HatKey}, Hair: {HairKey}, Bottom: {BottomKey}, Shoes: {ShoesKey}, Pajama: {PajamaStyle}/{PajamaColor})";
        }
    }
}