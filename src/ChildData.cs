using StardewValley.Characters;
using System;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 데이터 DTO (딱 “파일명 기반” 파츠 1:1 명칭! 직렬화/저장/불러오기 전용)
    /// </summary>
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; } // 0:Boy, 1:Girl
        public int Age { get; set; }
        public string SpouseName { get; set; }

        // 아기 파츠 (Baby)
        public string BabyHair { get; set; }
        public string BabyEye { get; set; }
        public string BabySkin { get; set; }
        public string BabyBody { get; set; }

        // 유아 파츠 (Toddler, 남/여 통합)
        public string ToddlerHair { get; set; }
        public string ToddlerEye { get; set; }
        public string ToddlerSkin { get; set; }

        // 의상/액세서리 (유아)
        public string Top { get; set; }
        public string Bottom { get; set; }
        public string Shoes { get; set; }
        public string NeckCollar { get; set; }

        // 잠옷/축제
        public string PajamaType { get; set; }
        public string PajamaColor { get; set; }
        public string FestivalHat { get; set; }
        public string FestivalScarf { get; set; }

        // === Child → ChildData 변환 (필수!) ===
        public static ChildData FromChild(Child child)
        {
            if (child == null)
                return null;

            var parts = (child.Age == 0)
                ? PartsManager.GetPartsForBaby(child, ModEntry.Config)
                : PartsManager.GetPartsForChild(child, ModEntry.Config);

            if (parts == null)
                parts = PartsManager.GetDefaultParts(child, child.Age == 0);

            return new ChildData
            {
                Name = child.Name,
                ParentID = child.idOfParent?.Value ?? -1,
                Gender = (int)child.Gender,
                Age = child.Age,
                SpouseName = AppearanceManager.GetRealSpouseName(child),

                BabyHair = parts.BabyHair,
                BabyEye = parts.BabyEye,
                BabySkin = parts.BabySkin,
                BabyBody = parts.BabyBody,

                ToddlerHair = parts.ToddlerHair,
                ToddlerEye = parts.ToddlerEye,
                ToddlerSkin = parts.ToddlerSkin,

                Top = parts.Top,
                Bottom = parts.Bottom,
                Shoes = parts.Shoes,
                NeckCollar = parts.NeckCollar,

                PajamaType = parts.PajamaType,
                PajamaColor = parts.PajamaColor,
                FestivalHat = parts.FestivalHat,
                FestivalScarf = parts.FestivalScarf
            };
        }

        // === ChildParts로 변환 (동기화/외형적용) ===
        public ChildParts ToParts()
        {
            return new ChildParts
            {
                SpouseName = SpouseName,

                BabyHair = BabyHair,
                BabyEye = BabyEye,
                BabySkin = BabySkin,
                BabyBody = BabyBody,

                ToddlerHair = ToddlerHair,
                ToddlerEye = ToddlerEye,
                ToddlerSkin = ToddlerSkin,

                Top = Top,
                Bottom = Bottom,
                Shoes = Shoes,
                NeckCollar = NeckCollar,

                PajamaType = PajamaType,
                PajamaColor = PajamaColor,
                FestivalHat = FestivalHat,
                FestivalScarf = FestivalScarf
            };
        }
    }
}