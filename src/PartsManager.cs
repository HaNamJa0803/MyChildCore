using System.Linq;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 파츠 조합 추출 매니저 (스킨, 눈, 축제복 포함)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 자녀 및 설정 기반 파츠 조합 추출 (하드코딩 & 정확성 우선)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, DropdownConfig config)
        {
            if (child == null) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            bool isMale = ((int)child.Gender == 0);

            // 설정 가져오기 (누락 시 기본값)
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                spouseConfig = new SpouseChildConfig();

            // 상의(시즌/성별)
            string TopKeyMaleShort   = "Top_Male_Short";
            string TopKeyMaleLong    = "Top_Male_Long";
            string TopKeyFemaleShort = "Top_Female_Short";
            string TopKeyFemaleLong  = "Top_Female_Long";

            // 하의(남/여 분리)
            string PantsKey = spouseConfig.BoyPants ?? "Pants_01";
            string SkirtKey = spouseConfig.GirlSkirt ?? "Skirt_01";

            // 신발, 넥칼라
            string ShoesKey = isMale ? spouseConfig.BoyShoes ?? "Shoes_01" : spouseConfig.GirlShoes ?? "Shoes_01";
            string NeckKey = isMale ? spouseConfig.BoyNeckCollar ?? "NeckCollar_01" : spouseConfig.GirlNeckCollar ?? "NeckCollar_01";

            // 헤어, 스킨, 눈 (리소스 대소문자 일치)
            string HairKey = isMale ? "Short" : (spouseConfig.GirlHairStyle ?? "CherryTwin");
            string SkinKey = $"assets/{spouseName}/Toddler/{spouseName}_Toddler_Skin.png";
            string EyeKey  = $"assets/{spouseName}/Toddler/{spouseName}_Toddler_Eye.png";

            // 잠옷 (예: Frog_01, Sheep_04, ...)
            string PajamaStyle = isMale ? spouseConfig.BoyPajamaStyle ?? "Frog" : spouseConfig.GirlPajamaStyle ?? "Frog";
            int PajamaColorIndex = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
            if (PajamaColorIndex < 1) PajamaColorIndex = 1;
            string PajamaKey = $"{PajamaStyle}_{PajamaColorIndex:D2}"; // → 실제 파일: assets/clothes/sleep/Frog/Frog_01.png

            // 축제복 (시즌/성별 분리, 실제 파일명은 반드시 일치)
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            string SpringHatKeyMale   = null;
            string SummerHatKey       = null;
            string SummerTopKeyMale   = null;
            string SummerTopKeyFemale = null;
            string FallTopKeyMale     = null;
            string FallTopKeyFemale   = null;
            string WinterHatKey       = null;
            string WinterTopKeyMale   = null;
            string WinterTopKeyFemale = null;
            string WinterNeckKey      = null;

            if (Game1.isFestival())
            {
                if (season == "spring")
                {
                    SpringHatKeyMale = "FestivalHat_Male_Spring";
                }
                else if (season == "summer")
                {
                    SummerHatKey       = isMale ? "FestivalHat_Male_Summer" : "FestivalHat_Female_Summer";
                    SummerTopKeyMale   = "FestivalTop_Male_Summer";
                    SummerTopKeyFemale = "FestivalTop_Female_Summer";
                }
                else if (season == "fall" || season == "autumn")
                {
                    FallTopKeyMale   = "FestivalTop_Male_Fall";
                    FallTopKeyFemale = "FestivalTop_Female_Fall";
                }
                else if (season == "winter")
                {
                    WinterHatKey       = isMale ? "FestivalHat_Male_Winter" : "FestivalHat_Female_Winter";
                    WinterTopKeyMale   = "FestivalTop_Male_Winter";
                    WinterTopKeyFemale = "FestivalTop_Female_Winter";
                    WinterNeckKey      = "FestivalNeck_Winter";
                }
            }

            return new ChildParts
            {
                TopKeyMaleShort   = TopKeyMaleShort,
                TopKeyMaleLong    = TopKeyMaleLong,
                TopKeyFemaleShort = TopKeyFemaleShort,
                TopKeyFemaleLong  = TopKeyFemaleLong,

                PantsKey = PantsKey,
                SkirtKey = SkirtKey,
                ShoesKey = ShoesKey,
                NeckKey  = NeckKey,

                HairKey  = HairKey,
                SkinKey  = SkinKey,
                EyeKey   = EyeKey,

                PajamaKey = PajamaKey,
                PajamaColorIndex = PajamaColorIndex,

                // 축제복
                SpringHatKeyMale   = SpringHatKeyMale,
                SummerHatKey       = SummerHatKey,
                SummerTopKeyMale   = SummerTopKeyMale,
                SummerTopKeyFemale = SummerTopKeyFemale,
                FallTopKeyMale     = FallTopKeyMale,
                FallTopKeyFemale   = FallTopKeyFemale,
                WinterHatKey       = WinterHatKey,
                WinterTopKeyMale   = WinterTopKeyMale,
                WinterTopKeyFemale = WinterTopKeyFemale,
                WinterNeckKey      = WinterNeckKey,

                SpouseName = spouseName,
                IsMale     = isMale
            };
        }

        /// <summary>
        /// 아기 전용 파츠 조합 추출 (하드코딩 & 정확성 우선)
        /// </summary>
        public static ChildParts GetPartsForBaby(Child Baby, DropdownConfig config)
        {
            if (Baby == null || Baby.Age != 0) return null;

            string spouseName = AppearanceManager.GetSpouseName(Baby);
            bool isMale = ((int)Baby.Gender == 0);

            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                spouseConfig = new SpouseChildConfig();

            // 아기용 파츠 (실제 경로는 대소문자 주의)
            string BabyBodyKey = "Baby_Body";
            string BabyHairKey = $"assets/{spouseName}/Baby/Hair/{spouseName}_Baby_Hair.png";
            string BabyEyeKey  = $"assets/{spouseName}/Baby/Eye/{spouseName}_Baby_Eye.png";
            string BabySkinKey = $"assets/{spouseName}/Baby/Skin/{spouseName}_Baby_Skin.png";

            // 잠옷
            string PajamaStyle = isMale ? spouseConfig.BoyPajamaStyle ?? "Frog" : spouseConfig.GirlPajamaStyle ?? "Frog";
            int PajamaColorIndex = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
            if (PajamaColorIndex < 1) PajamaColorIndex = 1;
            string PajamaKey = $"{PajamaStyle}_{PajamaColorIndex:D2}";

            return new ChildParts
            {
                BabyBodyKey = BabyBodyKey,
                BabyHairKey = BabyHairKey,
                BabyEyeKey = BabyEyeKey,
                BabySkinKey = BabySkinKey,
                PajamaKey = PajamaKey,
                PajamaColorIndex = PajamaColorIndex,
                SpouseName = spouseName,
                IsMale = isMale
            };
        }
    }
}