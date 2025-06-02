using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 및 아기 파츠 조합 추출 매니저 (GMCM 연동, 모든 분기 포함)
    /// </summary>
    public static class PartsManager
    {
        // 유아/아이 파츠 조합 (성별/계절/스타일별)
        public static ChildParts GetPartsForChild(Child child, DropdownConfig config)
        {
            if (child == null) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            bool isMale = ((int)child.Gender == 0);
            config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig);

            // 성별/계절별 기본값 및 GMCM 옵션
            string pantsKey      = spouseConfig?.BoyPants ?? "Pants_01";
            string skirtKey      = spouseConfig?.GirlSkirt ?? "Skirt_01";
            string shoesKey      = isMale ? (spouseConfig?.BoyShoes ?? "Shoes_01") : (spouseConfig?.GirlShoes ?? "Shoes_01");
            string neckKey       = isMale ? (spouseConfig?.BoyNeckCollar ?? "NeckCollar_01") : (spouseConfig?.GirlNeckCollar ?? "NeckCollar_01");
            string hairKey       = isMale ? "Short" : (spouseConfig?.GirlHairStyle ?? "CherryTwin");
            string pajamaStyle   = isMale ? (spouseConfig?.BoyPajamaStyle ?? "Frog") : (spouseConfig?.GirlPajamaStyle ?? "Frog");
            int pajamaColorIndex = isMale ? (spouseConfig?.BoyPajamaColorIndex ?? 1) : (spouseConfig?.GirlPajamaColorIndex ?? 1);

            // 축제복, 경로 하드코딩
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
                TopKeyMaleShort   = "Top_Male_Short",
                TopKeyMaleLong    = "Top_Male_Long",
                TopKeyFemaleShort = "Top_Female_Short",
                TopKeyFemaleLong  = "Top_Female_Long",

                PantsKeyMale      = pantsKey,
                SkirtKeyFemale    = skirtKey,
                ShoesKey          = shoesKey,
                NeckKey           = neckKey,
                HairKey           = hairKey,
                PajamaStyle       = pajamaStyle,
                PajamaColorIndex  = pajamaColorIndex,

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

                SpouseName         = spouseName,
                IsMale             = isMale
            };
        }

        // 아기 파츠 조합 (경로/파일명 대소문자, 폴더/이름 완전 분리)
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