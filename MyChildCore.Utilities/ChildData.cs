using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// Child(자녀) 파츠/외형/연동 DTO (1.6.10+ 기준, 최신 파츠 시스템 완전 대응)
    /// - 모든 파츠/속성 한 번에 관리! (성능 위주, 누락 ZERO)
    /// </summary>
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        /// <summary>성별 (0=남, 1=여) - StardewValley.Gender enum (0: Male, 1: Female)</summary>
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