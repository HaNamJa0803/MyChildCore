using StardewValley;
using StardewValley.Characters;
using MyChildCore;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (GMCM 옵션 완전 반영)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 유아/아이(토들러) 파츠 조합 (배우자별/성별별, 옵션 기반)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, DropdownConfig config)
        {
            if (child == null || config == null) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            bool isMale = ((int)child.Gender == 0);

            // GMCM 커스텀키 추출
            string gmcmKey = GMCMKeyUtil.GetChildKey(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                spouseConfig = new SpouseChildConfig(); // fallback

            // 하의/신발/넥칼라
            string pantsKey      = spouseConfig?.BoyPants ?? "Pants_01";
            string skirtKey      = spouseConfig?.GirlSkirt ?? "Skirt_01";
            string shoesKey      = isMale ? (spouseConfig?.BoyShoes ?? "Shoes_01") : (spouseConfig?.GirlShoes ?? "Shoes_01");
            string neckKey       = isMale ? (spouseConfig?.BoyNeckCollar ?? "NeckCollar_01") : (spouseConfig?.GirlNeckCollar ?? "NeckCollar_01");

            // 헤어/잠옷
            string hairKey       = isMale ? "Short" : (spouseConfig?.GirlHairStyle ?? "CherryTwin");
            string pajamaStyle   = isMale ? (spouseConfig?.BoyPajamaStyle ?? "Frog") : (spouseConfig?.GirlPajamaStyle ?? "Frog");
            int pajamaColorIndex = isMale ? (spouseConfig?.BoyPajamaColorIndex ?? 1) : (spouseConfig?.GirlPajamaColorIndex ?? 1);

            // 축제복(하드코딩, 활성화만 옵션으로 분기)
            string springHatKeyMale   = "FestivalHat_Male_Spring";
            string springHatKeyFemale = "FestivalHat_Female_Spring";
            string summerHatKeyMale   = "FestivalHat_Male_Summer";
            string summerHatKeyFemale = "FestivalHat_Female_Summer";
            string summerTopKeyMale   = "FestivalTop_Male_Summer";
            string summerTopKeyFemale = "FestivalTop_Female_Summer";
            string fallTopKeyMale     = "FestivalTop_Male_Fall";
            string fallTopKeyFemale   = "FestivalTop_Female_Fall";
            string winterHatKeyMale   = "FestivalHat_Male_Winter";
            string winterHatKeyFemale = "FestivalHat_Female_Winter";
            string winterTopKeyMale   = "FestivalTop_Male_Winter";
            string winterTopKeyFemale = "FestivalTop_Female_Winter";
            string winterNeckKey      = "FestivalNeck_Winter";

            return new ChildParts
            {
                // 상의(계절)
                TopKeyMaleShort   = "Top_Male_Short",
                TopKeyMaleLong    = "Top_Male_Long",
                TopKeyFemaleShort = "Top_Female_Short",
                TopKeyFemaleLong  = "Top_Female_Long",

                // 하의/신발/악세
                PantsKeyMale      = pantsKey,
                SkirtKeyFemale    = skirtKey,
                ShoesKey          = shoesKey,
                NeckKey           = neckKey,
                HairKey           = hairKey,
                PajamaKey         = $"{pajamaStyle}_{pajamaColorIndex}",
                PajamaStyle       = pajamaStyle,
                PajamaColorIndex  = pajamaColorIndex,

                // 축제복
                SpringHatKeyMale   = springHatKeyMale,
                SpringHatKeyFemale = springHatKeyFemale,
                SummerHatKeyMale   = summerHatKeyMale,
                SummerHatKeyFemale = summerHatKeyFemale,
                SummerTopKeyMale   = summerTopKeyMale,
                SummerTopKeyFemale = summerTopKeyFemale,
                FallTopKeyMale     = fallTopKeyMale,
                FallTopKeyFemale   = fallTopKeyFemale,
                WinterHatKeyMale   = winterHatKeyMale,
                WinterHatKeyFemale = winterHatKeyFemale,
                WinterTopKeyMale   = winterTopKeyMale,
                WinterTopKeyFemale = winterTopKeyFemale,
                WinterNeckKey      = winterNeckKey,

                // 기타
                SpouseName         = spouseName,
                IsMale             = isMale
            };
        }

        /// <summary>
        /// 아기(0세) 파츠 조합 (경로/키 하드코딩)
        /// </summary>
        public static ChildParts GetPartsForBaby(Child child, DropdownConfig config)
        {
            if (child == null || child.Age != 0) return null;
            string spouseName = AppearanceManager.GetSpouseName(child);

            return new ChildParts
            {
                BabyHairKey = $"assets/{spouseName}/Baby/Hair/Baby_Hair.png",
                BabySkinKey = $"assets/{spouseName}/Baby/Skin/Baby_Skin.png",
                BabyEyeKey  = $"assets/{spouseName}/Baby/Eye/Baby_Eye.png",
                BabyBodyKey = "assets/Clothes/Baby/Baby_Body.png",
                SpouseName  = spouseName
            };
        }
    }
}