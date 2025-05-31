namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형 파츠(경로/키) DTO
    /// </summary>
    public class ChildParts
    {
        // 상의(남자/여자, 숏/롱)
        public string TopKeyMaleShort { get; set; }
        public string TopKeyMaleLong { get; set; }
        public string TopKeyFemaleShort { get; set; }
        public string TopKeyFemaleLong { get; set; }
        
        // 하의(남/여 분리), 신발, 넥칼라, 기타
        public string PantsKey { get; set; }
        public string SkirtKey { get; set; }
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }
        
        // 헤어, 스킨, 눈
        public string HairKey { get; set; }
        public string SkinKey { get; set; }
        public string EyeKey { get; set; }
        
        // 잠옷
        public string PajamaKey { get; set; }
        public int PajamaColorIndex { get; set;}     
        
        // 축제복 상의(남/여), 모자, 넥칼라
        public string SpringHatKeyMale { get; set; }
        public string SummerHatKey { get; set; }
        public string SummerTopKeyMale { get; set; }
        public string SummerTopKeyFemale { get; set; }
        public string FallTopKeyMale { get; set; }
        public string FallTopKeyFemale { get; set; }
        public string WinterHatKey { get; set; }
        public string WinterTopKeyMale { get; set; }
        public string WinterTopKeyFemale { get; set; }
        public string WinterNeckKey { get; set; }
        
        public string SpouseName { get; set; }
        public bool IsMale { get; set; }

        // 아기(0세)용 파츠
        public string BabyHairKey { get; set; }
        public string BabySkinKey { get; set; }
        public string BabyEyeKey { get; set; }
        public string BabyBodyKey { get; set; }
    }
}