using System;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 파츠/연동 DTO (Stardew Valley 1.6.15 완전 대응)
    /// </summary>
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public string HairStyle { get; set; }
        public int BottomColorIndex { get; set; }
        public int ShoesColorIndex { get; set; }
        public int NeckColorIndex { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }

        public ChildData() { }
        public ChildData(string name, long parentID, int gender,
            string hairStyle, int bottomColor, int shoesColor, int neckColor,
            string pajamaStyle, int pajamaColor)
        {
            Name = name;
            ParentID = parentID;
            Gender = gender;
            HairStyle = hairStyle;
            BottomColorIndex = bottomColor;
            ShoesColorIndex = shoesColor;
            NeckColorIndex = neckColor;
            PajamaStyle = pajamaStyle;
            PajamaColorIndex = pajamaColor;
        }
    }
}