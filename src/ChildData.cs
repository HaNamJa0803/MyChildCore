using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley.Characters; // Child 타입 위해 필요
using MyChildCore;

namespace MyChildCore
{
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }

        // 외형 파츠 (ChildParts와 1:1 동기화)
        public string TopKeyMaleShort { get; set; }
        public string TopKeyMaleLong { get; set; }
        public string TopKeyFemaleShort { get; set; }
        public string TopKeyFemaleLong { get; set; }
        public string PantsKeyMale { get; set; }
        public string SkirtKeyFemale { get; set; }
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }
        public string HairKey { get; set; }
        public string SkinKey { get; set; }
        public string EyeKey { get; set; }
        public string PajamaKey { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }

        // 계절/축제별 파츠
        public string SpringHatKeyMale { get; set; }
        public string SpringHatKeyFemale { get; set; }
        public string SummerHatKeyMale { get; set; }
        public string SummerHatKeyFemale { get; set; }
        public string SummerTopKeyMale { get; set; }
        public string SummerTopKeyFemale { get; set; }
        public string FallTopKeyMale { get; set; }
        public string FallTopKeyFemale { get; set; }
        public string WinterHatKeyMale { get; set; }
        public string WinterHatKeyFemale { get; set; }
        public string WinterTopKeyMale { get; set; }
        public string WinterTopKeyFemale { get; set; }
        public string WinterNeckKey { get; set; }

        // 아기 파츠
        public string BabyHairKey { get; set; }
        public string BabySkinKey { get; set; }
        public string BabyEyeKey { get; set; }
        public string BabyBodyKey { get; set; }

        // === Child → ChildData 변환 (필수!) ===
        public static ChildData FromChild(Child child)
        {
            if (child == null)
                return null;

            // 나이별 파츠 분기
            ChildParts parts = (child.Age == 0)
                ? PartsManager.GetPartsForBaby(child, ModEntry.Config)
                : PartsManager.GetPartsForChild(child, ModEntry.Config);

            if (parts == null)
            {
                parts = PartsManager.GetDefaultParts(child, child.Age == 0);
            }

            // 아래는 예시! 필요에 따라 파츠 값 추출 방법 맞춰 넣어야 함
            return new ChildData
            {
                Name = child.Name,   // 만약 Name 프로퍼티 추가하면 포함
                ParentID = child.idOfParent != null ? child.idOfParent.Value : -1,
                Gender = (int)child.Gender,
                Age = child.Age,
                IsMale = child.Gender == 0,
                SpouseName = AppearanceManager.GetRealSpouseName(child),

                // 파츠 값 1:1 복사 (예시)
                TopKeyMaleShort = parts.BoyTopSpringOptions,
                TopKeyMaleLong = parts.BoyTopWinterOptions,
                TopKeyFemaleShort = parts.GirlTopSpringOptions,
                TopKeyFemaleLong = parts.GirlTopWinterOptions,
                PantsKeyMale = parts.PantsColorOptions,
                SkirtKeyFemale = parts.SkirtColorOptions,
                ShoesKey = parts.IsMale ? parts.BoyShoesColorOptions : parts.GirlShoesColorOptions,
                NeckKey = parts.IsMale ? parts.BoyNeckCollarColorOptions : parts.GirlNeckCollarColorOptions,
                HairKey = parts.IsMale ? parts.BoyHairStyles : parts.GirlHairStyles,
                SkinKey = parts.IsMale ? parts.BoySkins : parts.GirlSkins,
                EyeKey = parts.IsMale ? parts.BoyEyes : parts.GirlEyes,
                PajamaKey = parts.IsMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions,
                PajamaStyle = null, // 필요시 parts에서 읽어서 대입
                PajamaColorIndex = 0, // 색상 값이 enum/string이면 변환해서 대입

                // 계절/축제별 파츠
                SpringHatKeyMale = parts.FestivalSpringHat,
                SpringHatKeyFemale = parts.FestivalSpringHat,
                SummerHatKeyMale = parts.FestivalSummerHat,
                SummerHatKeyFemale = parts.FestivalSummerHat,
                SummerTopKeyMale = parts.BoyTopSummerOptions,
                SummerTopKeyFemale = parts.GirlTopSummerOptions,
                FallTopKeyMale = parts.BoyTopFallOptions,
                FallTopKeyFemale = parts.GirlTopFallOptions,
                WinterHatKeyMale = parts.FestivalWinterHat,
                WinterHatKeyFemale = parts.FestivalWinterHat,
                WinterTopKeyMale = parts.BoyTopWinterOptions,
                WinterTopKeyFemale = parts.GirlTopWinterOptions,
                WinterNeckKey = parts.FestivalWinterScarf,

                // 아기 파츠
                BabyHairKey = parts.BabyHairStyles,
                BabySkinKey = parts.BabySkins,
                BabyEyeKey = parts.BabyEyes,
                BabyBodyKey = parts.BabyBodies,
            };
        }
    } // ← ChildData 클래스 닫는 중괄호

} // ← 네임스페이스 닫는 중괄호