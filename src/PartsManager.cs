using StardewValley.Characters;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (GMCM 옵션 완전 반영)
    /// </summary>
    public static class PartsManager
    {
        /// <summary>
        /// 아기(0세) 파츠 조합
        /// </summary>
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            if (child == null || config == null || child.Age != 0) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            var parts = new ChildParts
            {
                SpouseName = spouseName,
                IsMale = child.Gender == StardewValley.Child.Gender_Boy,

                BabyHairStyle = spouseConfig.BabyHairStyle,
                BabyEye = spouseConfig.BabyEye,
                BabySkin = spouseConfig.BabySkin,
                BabyBody = spouseConfig.BabyBody,

                // 축제(공용)
                FestivalSpringHat = spouseConfig.FestivalSpringHat,
                FestivalWinterHat = spouseConfig.FestivalWinterHat,
                FestivalWinterScarf = spouseConfig.FestivalWinterScarf
            };

            return parts;
        }

        /// <summary>
        /// 유아/아이(토들러) 파츠 조합 (배우자별/성별별, 옵션 기반)
        /// </summary>
        public static ChildParts GetPartsForChild(Child child, ModConfig config)
        {
            if (child == null || config == null) return null;

            string spouseName = AppearanceManager.GetSpouseName(child);
            if (!config.SpouseConfigs.TryGetValue(spouseName, out var spouseConfig))
                return null;

            bool isMale = child.Gender == StardewValley.Child.Gender_Boy;

            var parts = new ChildParts
            {
                SpouseName = spouseName,
                IsMale = isMale,

                // 여자 자녀
                GirlHairStyle = spouseConfig.GirlHairStyle,
                GirlEye = spouseConfig.GirlEye,
                GirlSkin = spouseConfig.GirlSkin,
                GirlTopSpring = spouseConfig.GirlTopSpring,
                GirlTopSummer = spouseConfig.GirlTopSummer,
                GirlTopFall = spouseConfig.GirlTopFall,
                GirlTopWinter = spouseConfig.GirlTopWinter,
                GirlSkirtColor = spouseConfig.GirlSkirtColor,
                GirlShoesColor = spouseConfig.GirlShoesColor,
                GirlNeckCollarColor = spouseConfig.GirlNeckCollarColor,
                GirlPajamaType = spouseConfig.GirlPajamaType,
                GirlPajamaColor = spouseConfig.GirlPajamaColor,
                GirlFestivalSummerSkirt = spouseConfig.GirlFestivalSummerSkirt,
                GirlFestivalWinterSkirt = spouseConfig.GirlFestivalWinterSkirt,
                GirlFestivalFallSkirt = spouseConfig.GirlFestivalFallSkirt,

                // 남자 자녀
                BoyHairStyle = spouseConfig.BoyHairStyle,
                BoyEye = spouseConfig.BoyEye,
                BoySkin = spouseConfig.BoySkin,
                BoyTopSpring = spouseConfig.BoyTopSpring,
                BoyTopSummer = spouseConfig.BoyTopSummer,
                BoyTopFall = spouseConfig.BoyTopFall,
                BoyTopWinter = spouseConfig.BoyTopWinter,
                BoyPantsColor = spouseConfig.BoyPantsColor,
                BoyShoesColor = spouseConfig.BoyShoesColor,
                BoyNeckCollarColor = spouseConfig.BoyNeckCollarColor,
                BoyPajamaType = spouseConfig.BoyPajamaType,
                BoyPajamaColor = spouseConfig.BoyPajamaColor,
                BoyFestivalSummerPants = spouseConfig.BoyFestivalSummerPants,
                BoyFestivalWinterPants = spouseConfig.BoyFestivalWinterPants,
                BoyFestivalFallPants = spouseConfig.BoyFestivalFallPants,

                // 축제(공용)
                FestivalSpringHat = spouseConfig.FestivalSpringHat,
                FestivalWinterHat = spouseConfig.FestivalWinterHat,
                FestivalWinterScarf = spouseConfig.FestivalWinterScarf
            };

            return parts;
        }
    }
}