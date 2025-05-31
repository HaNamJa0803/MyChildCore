namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀의 모든 파츠 정보를 담은 DTO (축제복 포함)
    /// </summary>
    public class ChildParts
    {
        public string HairKey { get; set; }
        public string TopKey { get; set; }
        public string BottomKey { get; set; }
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }
        public string EyeKey { get; set; }
        public string SkinKey { get; set; }
        public string SpouseName { get; set; }
        public bool IsMale { get; set; }

        // ============ 축제복 파츠 ============
        public string FestivalTopKey { get; set; }
        public string FestivalHatKey { get; set; }
        public string FestivalNeckKey { get; set; }
        
        // ============ 아기 전용 파츠 ============
       public string BabyBodyKey { get; set; }
       public string BabyHairKey { get; set; }
       public string BabyEyeKey { get; set; }
       public string BabySkinKey { get; set; }
    }
}