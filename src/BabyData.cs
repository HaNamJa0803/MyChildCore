using System;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 영아(아기)용 외형/식별 정보 DTO (Child와 분리, Baby 단계 전용)
    /// </summary>
    public class BabyData
    {
        // ────────── 식별 정보 ──────────
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }    // 0=남, 1=여

        // ────────── 외형 파츠 속성 ──────────
        // ※ 아기는 일반적으로 복잡한 파츠 없음(필요하면 추가)
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }

        // 확장 필드(예: 담요, 피부톤 등 필요 시 확장)
        // public string BlanketColor { get; set; }
        // public string SkinColor { get; set; }

        // ────────── 생성자 ──────────
        public BabyData() { }

        public BabyData(
            string name,
            long parentId,
            int gender,
            string pajamaStyle,
            int pajamaColor
        )
        {
            Name = name;
            ParentID = parentId;
            Gender = gender;
            PajamaStyle = pajamaStyle;
            PajamaColorIndex = pajamaColor;
        }
    }
}