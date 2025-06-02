using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }

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

        public string SpouseName { get; set; }
        public bool IsMale { get; set; }

        // 아기 파츠
        public string BabyHairKey { get; set; }
        public string BabySkinKey { get; set; }
        public string BabyEyeKey { get; set; }
        public string BabyBodyKey { get; set; }
    }
}