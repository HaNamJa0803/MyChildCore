namespace MyChildCore.Utilities
{
    /// <summary>
    /// 아기/유아 겸용 외형 및 식별 데이터 DTO (Age로 구분)
    /// </summary>
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; } // 0: 아기, 1이상: 유아/아이

        // 유아/아이 단계(1 이상)용
        public string HairKey { get; set; }
        public string SkinKey { get; set; }
        public string EyeKey { get; set; } // 유아/아이용 눈

        // 아기 단계(0)용
        public string BabyHairKey { get; set; }
        public string BabySkinKey { get; set; }
        public string BabyEyeKey { get; set; } // ★ 아기 전용 눈 파츠

        // 기타 공통 파츠
        public string TopKeyMaleShort { get; set; }
        public string TopKeyMaleLong { get; set; }
        public string TopKeyFemaleShort { get; set; }
        public string TopKeyFemaleLong { get; set; }
        public string BottomKey { get; set; }
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }
        public string FestivalTopKey { get; set; }
        public string FestivalHatKey { get; set; }
        public string FestivalNeckKey { get; set; }
        public string BodyKey { get; set; } // 아기 바디

        public ChildData() { }

        public ChildData(
            string name, long parentID, int gender, int age,
            string hairKey, string skinKey, string eyeKey,
            string babyHairKey, string babySkinKey, string babyEyeKey, // 아기용 파츠
            string topKey, string bottomKey, string shoesKey, string neckKey,
            string pajamaStyle, int pajamaColorIndex,
            string festivalTopKey = null, string festivalHatKey = null, string festivalNeckKey = null,
            string bodyKey = null
        )
        {
            Name = name;
            ParentID = parentID;
            Gender = gender;
            Age = age;
            HairKey = hairKey;
            SkinKey = skinKey;
            EyeKey = eyeKey;
            BabyHairKey = babyHairKey;
            BabySkinKey = babySkinKey;
            BabyEyeKey = babyEyeKey;
            TopKey = topKey;
            BottomKey = bottomKey;
            ShoesKey = shoesKey;
            NeckKey = neckKey;
            PajamaStyle = pajamaStyle;
            PajamaColorIndex = pajamaColorIndex;
            FestivalTopKey = festivalTopKey;
            FestivalHatKey = festivalHatKey;
            FestivalNeckKey = festivalNeckKey;
            BodyKey = bodyKey;
        }
    }
}