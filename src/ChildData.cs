using System;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 및 식별 정보(캐싱/저장용 DTO)
    /// </summary>
    public class ChildData
    {
        // ────────── 기본 정보 ──────────
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }    // 0=남, 1=여

        // ────────── 파츠 속성 ──────────

        // 헤어(여아는 선택, 남아는 "Short" 고정)
        public string HairStyle { get; set; }

        // 하의(번호, 1~10) - 남:Pants, 여:Skirt
        public int BottomColorIndex { get; set; }

        // 신발(번호, 1~4)
        public int ShoesColorIndex { get; set; }

        // 넥칼라(번호, 1~26)
        public int NeckColorIndex { get; set; }

        // 잠옷
        public string PajamaStyle { get; set; }      // "Frog" 등
        public int PajamaColorIndex { get; set; }

        // ────────── 생성자 ──────────
        public ChildData() { } // 역직렬화용

        public ChildData(
            string name,
            long parentId,
            int gender,
            string hairStyle,
            int bottomColor,
            int shoesColor,
            int neckColor,
            string pajamaStyle,
            int pajamaColor
        )
        {
            Name = name;
            ParentID = parentId;
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