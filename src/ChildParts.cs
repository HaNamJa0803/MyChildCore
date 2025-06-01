namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀/아기 모든 외형 파츠 DTO (성별/계절/잠옷/축제/기본 분리)
    /// </summary>
    public class ChildParts
    {
        // 유아/아이 상의
        public string TopKeyMaleShort { get; set; }
        public string TopKeyMaleLong { get; set; }
        public string TopKeyFemaleShort { get; set; }
        public string TopKeyFemaleLong { get; set; }

        // 하의, 신발, 넥칼라
        public string PantsKeyMale { get; set; }     // 남아 바지
        public string SkirtKeyFemale { get; set; }   // 여아 치마
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }

        // 얼굴/헤어
        public string HairKey { get; set; }
        public string SkinKey { get; set; }
        public string EyeKey { get; set; }

        // 잠옷
        public string PajamaKey { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }

        // 축제복 (시즌/성별 분기)
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

        // 공통
        public string SpouseName { get; set; }
        public bool IsMale { get; set; }

        // 아기(0세)용 파츠
        public string BabyHairKey { get; set; }
        public string BabySkinKey { get; set; }
        public string BabyEyeKey { get; set; }
        public string BabyBodyKey { get; set; }
    }
}