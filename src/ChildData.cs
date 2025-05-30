using System;
using System.Linq;
using MyChildCore.Utilities;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 파츠/연동 DTO (Stardew Valley 1.6.10+ 완전 대응)
    /// </summary>
    public class ChildData
    {
        // ────────── 기본 식별 정보 ──────────
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }    // 0=남, 1=여

        // ────────── 유아 파츠 시스템 ──────────

        // 헤어(여아만 선택, 남아 고정)
        public string HairStyle { get; set; }        // "Short"(남), "CherryTwin"/"TwinTail"/"PonyTail"(여)

        // 하의(번호만)
        public int BottomColorIndex { get; set; }    // 1~10 (남: Pants, 여: Skirt)

        // 신발(번호만)
        public int ShoesColorIndex { get; set; }     // 1~4

        // 넥칼라(번호만)
        public int NeckColorIndex { get; set; }      // 1~26

        // 잠옷
        public string PajamaStyle { get; set; }      // "Frog", "WelshCorgi", "Sheep", "LesserPanda", "Racoon", "Shark"
        public int PajamaColorIndex { get; set; }    // 1~N (스타일별 색상수)

        // 기타(확장) - 필요시만
        // public string EyeColor { get; set; }
        // public string SkinColor { get; set; }
        // public string Etc { get; set; }

        // 역직렬화용 기본 생성자
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