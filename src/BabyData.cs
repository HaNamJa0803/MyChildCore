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
        public string BodyKey { get; set; }   // 아기 공용 바디(의상) 경로
        public string SkinKey { get; set; }   // 배우자 기반 스킨 경로
        public string EyeKey { get; set; }    // 배우자 기반 눈 경로
        public string HairKey { get; set; }   // 배우자 기반 헤어 경로

        // ────────── 확장(필요시) ──────────
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }

        // ────────── 생성자 ──────────
        public BabyData() { }

        public BabyData(
            string name,
            long parentId,
            int gender,
            string bodyKey,
            string skinKey,
            string eyeKey,
            string hairKey,
            string pajamaStyle = null,
            int pajamaColor = 1
        )
        {
            Name = name;
            ParentID = parentId;
            Gender = gender;
            BodyKey = bodyKey;
            SkinKey = skinKey;
            EyeKey = eyeKey;
            HairKey = hairKey;
            PajamaStyle = pajamaStyle;
            PajamaColorIndex = pajamaColor;
        }
    }
}