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
        // ───────── 기본 식별 정보 ─────────
        /// <summary>자녀 이름 (동명이인, 쌍둥이 구분 가능)</summary>
        public string Name { get; set; }
        /// <summary>부모 고유ID (멀티/입양/쌍둥이까지 구분)</summary>
        public long ParentID { get; set; }
        /// <summary>성별 (0=남, 1=여)</summary>
        public int Gender { get; set; }

        // ───────── 파츠 시스템(누락 0%) ─────────
        /// <summary>모자 파츠키</summary>
        public string HatKey { get; set; }
        /// <summary>헤어 파츠키</summary>
        public string HairKey { get; set; }
        /// <summary>눈 파츠키</summary>
        public string EyeKey { get; set; }
        /// <summary>피부/스킨 파츠키 (얼굴 등)</summary>
        public string SkinKey { get; set; }
        /// <summary>상의 파츠키</summary>
        public string TopKey { get; set; }
        /// <summary>하의 파츠키</summary>
        public string BottomKey { get; set; }
        /// <summary>넥칼라/목도리 파츠키</summary>
        public string NeckCollarKey { get; set; }
        /// <summary>신발 파츠키</summary>
        public string ShoesKey { get; set; }
        /// <summary>의상(통합/축제복 등) 파츠키</summary>
        public string ClothesKey { get; set; }
        /// <summary>잠옷 스타일</summary>
        public string PajamaStyle { get; set; }
        /// <summary>잠옷 색상</summary>
        public string PajamaColor { get; set; }

        // ───────── 구버전 호환 (필요시) ─────────
        /// <summary>구버전 호환: 얼굴 스킨 파츠키</summary>
        public string FaceSkinKey { get; set; }
        /// <summary>구버전 호환: 바디(공용) 파츠키</summary>
        public string BodyKey { get; set; }

        // ───────── 생성자 및 변환 ─────────
        /// <summary>
        /// 게임 내 Child 인스턴스 → ChildData로 변환 (파츠 자동 매핑)
        /// </summary>
        public ChildData(
            Child child,
            string hatKey = "", string hairKey = "", string eyeKey = "", string skinKey = "",
            string topKey = "", string bottomKey = "", string neckCollarKey = "",
            string shoesKey = "", string clothesKey = "",
            string pajamaStyle = "", string pajamaColor = "",
            string faceSkinKey = "", string bodyKey = ""
        )
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

            // 구버전 호환 필드(필요시)
            FaceSkinKey = faceSkinKey;
            BodyKey = bodyKey;
        }

        /// <summary>
        /// 역직렬화/직접 생성용 파라미터 없는 생성자
        /// </summary>
        public ChildData() { }
    }
}