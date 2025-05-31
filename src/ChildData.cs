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
        public string BabyEyeKey { get; set; } // 아기 전용 눈 파츠

        // 기타 공통 파츠
        public string TopKeyMaleShort { get; set; }
        public string TopKeyMaleLong { get; set; }
        public string TopKeyFemaleShort { get; set; }
        public string TopKeyFemaleLong { get; set; }
        public string PantsKey { get; set; }
        public string SkirtKey { get; set; }
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }
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
        public string BodyKey { get; set; } // 아기 바디

        public ChildData() { }

        public ChildData(
            string name, long parentID, int gender, int age,
            string hairKey, string skinKey, string eyeKey,
            string babyHairKey, string babySkinKey, string babyEyeKey,
            string topKeyMaleShort, string topKeyMaleLong,
            string topKeyFemaleShort, string topKeyFemaleLong,
            string pantsKey, string skirtKey, string shoesKey, string neckKey,
            string pajamaStyle, int pajamaColorIndex,
            string springHatKeyMale, string summerHatKey,
            string summerTopKeyMale, string summerTopKeyFemale,
            string fallTopKeyMale, string fallTopKeyFemale,
            string winterHatKey, string winterTopKeyMale, string winterTopKeyFemale, string winterNeckKey,
            string bodyKey
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
            TopKeyMaleShort = topKeyMaleShort;
            TopKeyMaleLong = topKeyMaleLong;
            TopKeyFemaleShort = topKeyFemaleShort;
            TopKeyFemaleLong = topKeyFemaleLong;
            PantsKey = pantsKey;
            SkirtKey = skirtKey;
            ShoesKey = shoesKey;
            NeckKey = neckKey;
            PajamaStyle = pajamaStyle;
            PajamaColorIndex = pajamaColorIndex;
            SpringHatKeyMale = springHatKeyMale;
            SummerHatKey = summerHatKey;
            SummerTopKeyMale = summerTopKeyMale;
            SummerTopKeyFemale = summerTopKeyFemale;
            FallTopKeyMale = fallTopKeyMale;
            FallTopKeyFemale = fallTopKeyFemale;
            WinterHatKey = winterHatKey;
            WinterTopKeyMale = winterTopKeyMale;
            WinterTopKeyFemale = winterTopKeyFemale;
            WinterNeckKey = winterNeckKey;
            BodyKey = bodyKey;
        }
    }
}