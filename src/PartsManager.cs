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

            // 배우자 이름 및 성별
            string spouseName = AppearanceManager.GetSpouseName(child);
            bool isMale = ((int)child.Gender == 0);

            // 설정 가져오기 (누락 시 기본값)
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                spouseConfig = new SpouseChildConfig();

            string hairKey    = isMale ? "Short" : (spouseConfig.GirlHairStyle ?? "CherryTwin");
            string topKey     = isMale ? "Top_Male" : "Top_Female";
            string bottomKey  = isMale ? spouseConfig.BoyPants ?? "Pants_01" : spouseConfig.GirlSkirt ?? "Skirt_01";
            string shoesKey   = isMale ? spouseConfig.BoyShoes ?? "Shoes_01" : spouseConfig.GirlShoes ?? "Shoes_01";
            string neckKey    = isMale ? spouseConfig.BoyNeckCollar ?? "NeckCollar_01" : spouseConfig.GirlNeckCollar ?? "NeckCollar_01";
            string pajamaStyle = isMale ? spouseConfig.BoyPajamaStyle ?? "Frog" : spouseConfig.GirlPajamaStyle ?? "Frog";
            int pajamaColor   = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
            if (pajamaColor < 1) pajamaColor = 1;

            // 스킨/눈(고정값)
            string skinKey = $"assets/{spouseName}/Toddler/{spouseName}_Toddler_Skin.png";
            string eyeKey  = $"assets/{spouseName}/Toddler/{spouseName}_Toddler_Eye.png";

            // 축제복(분기, 존재하지 않으면 null)
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            string genderKey = isMale ? "Male" : "Female";
            string festivalTopKey = null, festivalHatKey = null, festivalNeckKey = null;

            if (Game1.isFestival)
            {
                if (season == "spring")
                {
                    festivalTopKey = $"Clothes/Festival/Spring/FestivalTop_{genderKey}_Spring.png";
                }
                else if (season == "summer")
                {
                    festivalTopKey = $"Clothes/Festival/Summer/FestivalTop_{genderKey}_Summer.png";
                    festivalHatKey = $"Clothes/Festival/Summer/FestivalHat_{genderKey}_Summer.png";
                }
                else if (season == "fall" || season == "autumn")
                {
                    festivalTopKey = $"Clothes/Festival/Fall/FestivalTop_{genderKey}_Fall.png";
                }
                else if (season == "winter")
                {
                    festivalTopKey = $"Clothes/Festival/Winter/FestivalTop_{genderKey}_Winter.png";
                    festivalHatKey = $"Clothes/Festival/Winter/FestivalHat_{genderKey}_Winter.png";
                    festivalNeckKey = $"Clothes/Festival/Winter/FestivalNeck_{genderKey}_Winter.png";
                }
            }

            return new ChildParts
            {
                HairKey = hairKey,
                TopKey = topKey,
                BottomKey = bottomKey,
                ShoesKey = shoesKey,
                NeckKey = neckKey,
                PajamaStyle = pajamaStyle,
                PajamaColorIndex = pajamaColor,
                SpouseName = spouseName,
                IsMale = isMale,
                SkinKey = skinKey,
                EyeKey = eyeKey,
                FestivalTopKey = festivalTopKey,
                FestivalHatKey = festivalHatKey,
                FestivalNeckKey = festivalNeckKey
            };
        }

        /// <summary>
        /// 아기 전용 파츠 조합 추출 (하드코딩 & 정확성 우선)
        /// </summary>
        public static ChildParts GetPartsForBaby(Child baby, DropdownConfig config)
        {
            if (baby == null || baby.Age != 0) return null;

            string spouseName = AppearanceManager.GetSpouseName(baby);
            bool isMale = ((int)baby.Gender == 0);

            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig) || spouseConfig == null)
                spouseConfig = new SpouseChildConfig();

            // 아기용 파츠 세팅 (공용 바디, 배우자별 헤어/눈/스킨)
            string babyBodyKey = "Baby_Body"; // 공용 경로, 필요 시 조정 가능
            string babyHairKey = $"assets/{spouseName}/Baby/hair/{spouseName}_Baby_hair.png";
            string babyEyeKey  = $"assets/{spouseName}/Baby/eye/{spouseName}_Baby_eye.png";
            string babySkinKey = $"assets/{spouseName}/Baby/skin/{spouseName}_Baby_skin.png";

            // 잠옷 스타일/색상 (유아용과 동일 로직)
            string pajamaStyle = isMale ? spouseConfig.BoyPajamaStyle ?? "Frog" : spouseConfig.GirlPajamaStyle ?? "Frog";
            int pajamaColor = isMale ? spouseConfig.BoyPajamaColorIndex : spouseConfig.GirlPajamaColorIndex;
            if (pajamaColor < 1) pajamaColor = 1;

            return new ChildParts
            {
                BabyBodyKey = babyBodyKey,
                BabyHairKey = babyHairKey,
                BabyEyeKey = babyEyeKey,
                BabySkinKey = babySkinKey,
                PajamaStyle = pajamaStyle,
                PajamaColorIndex = pajamaColor,
                SpouseName = spouseName,
                IsMale = isMale
            };
        }
    }
}