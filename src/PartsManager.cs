using MyChildCore;
using System;
using StardewValley;
using StardewValley.Characters;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 파츠 조합 추출 매니저 (딕셔너리, 네이밍 통일)
    /// </summary>
    public static class PartsManager
    {
        public static event Action<Child, ChildParts> OnPartsChanged;
        public static IModHelper Helper { get; set; }

        // === 아기(0세) 파츠 조합 ===
        public static ChildParts GetPartsForBaby(Child child, ModConfig config)
        {
            try
            {
                if (child == null || config == null || child.Age != 0)
                    return GetDefaultParts(child, true);

                string spouse = AppearanceManager.GetRealSpouseName(child);
                if (string.IsNullOrEmpty(spouse)
                    || !config.SpouseConfigs.TryGetValue(spouse, out var spouseConfig)
                    || spouseConfig == null)
                    return GetDefaultParts(child, true);

                var parts = new ChildParts
                {
                    SpouseName = spouse,
                    BabyHairTypeOptions = spouseConfig.BabyHairTypeOptions,
                    BabyHairType = spouseConfig.BabyHairType,
                    BabyHairColorOptions = spouseConfig.BabyHairColorOptions,
                    BabyHairColor = spouseConfig.BabyHairColor,
                    BabyEyeTypeOptions = spouseConfig.BabyEyeTypeOptions,
                    BabyEyeType = spouseConfig.BabyEyeType,
                    BabyEyeColorOptions = spouseConfig.BabyEyeColorOptions,
                    BabyEyeColor = spouseConfig.BabyEyeColor,
                    BabySkinTypeOptions = spouseConfig.BabySkinTypeOptions,
                    BabySkinType = spouseConfig.BabySkinType,
                    BabySkinColorOptions = spouseConfig.BabySkinColorOptions,
                    BabySkinColor = spouseConfig.BabySkinColor,
                    BabyBodyTypeOptions = spouseConfig.BabyBodyTypeOptions,
                    BabyBodyType = spouseConfig.BabyBodyType,
                    BabyBodyColorOptions = spouseConfig.BabyBodyColorOptions,
                    BabyBodyColor = spouseConfig.BabyBodyColor
                };
                OnPartsChanged?.Invoke(child, parts);
                return parts;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Baby] 파츠 추출 예외: {ex.Message}");
                var def = GetDefaultParts(child, true);
                OnPartsChanged?.Invoke(child, def);
                return def;
            }
        }

        // === 남아(1세) 파츠 조합 ===
        public static ChildParts GetPartsForBoy(Child child, ModConfig config)
        {
            try
            {
                if (child == null || config == null || child.Age != 1 || !child.Gender)
                    return GetDefaultParts(child, false, true);

                string spouse = AppearanceManager.GetRealSpouseName(child);
                if (string.IsNullOrEmpty(spouse)
                    || !config.SpouseConfigs.TryGetValue(spouse, out var spouseConfig)
                    || spouseConfig == null)
                    return GetDefaultParts(child, false, true);

                var parts = new ChildParts
                {
                    SpouseName = spouse,
                    BoyHairTypeOptions = spouseConfig.BoyHairTypeOptions,
                    BoyHairType = spouseConfig.BoyHairType,
                    BoyHairColorOptions = spouseConfig.BoyHairColorOptions,
                    BoyHairColor = spouseConfig.BoyHairColor,
                    BoyEyeTypeOptions = spouseConfig.BoyEyeTypeOptions,
                    BoyEyeType = spouseConfig.BoyEyeType,
                    BoyEyeColorOptions = spouseConfig.BoyEyeColorOptions,
                    BoyEyeColor = spouseConfig.BoyEyeColor,
                    BoySkinTypeOptions = spouseConfig.BoySkinTypeOptions,
                    BoySkinType = spouseConfig.BoySkinType,
                    BoySkinColorOptions = spouseConfig.BoySkinColorOptions,
                    BoySkinColor = spouseConfig.BoySkinColor,
                    BoyTopSpringTypeOptions = spouseConfig.BoyTopSpringTypeOptions,
                    BoyTopSpringType = spouseConfig.BoyTopSpringType,
                    BoyTopSpringColorOptions = spouseConfig.BoyTopSpringColorOptions,
                    BoyTopSpringColor = spouseConfig.BoyTopSpringColor,
                    BoyTopSummerTypeOptions = spouseConfig.BoyTopSummerTypeOptions,
                    BoyTopSummerType = spouseConfig.BoyTopSummerType,
                    BoyTopSummerColorOptions = spouseConfig.BoyTopSummerColorOptions,
                    BoyTopSummerColor = spouseConfig.BoyTopSummerColor,
                    BoyTopFallTypeOptions = spouseConfig.BoyTopFallTypeOptions,
                    BoyTopFallType = spouseConfig.BoyTopFallType,
                    BoyTopFallColorOptions = spouseConfig.BoyTopFallColorOptions,
                    BoyTopFallColor = spouseConfig.BoyTopFallColor,
                    BoyTopWinterTypeOptions = spouseConfig.BoyTopWinterTypeOptions,
                    BoyTopWinterType = spouseConfig.BoyTopWinterType,
                    BoyTopWinterColorOptions = spouseConfig.BoyTopWinterColorOptions,
                    BoyTopWinterColor = spouseConfig.BoyTopWinterColor,
                    PantsTypeOptions = spouseConfig.PantsTypeOptions,
                    PantsType = spouseConfig.PantsType,
                    PantsColorOptions = spouseConfig.PantsColorOptions,
                    PantsColor = spouseConfig.PantsColor,
                    BoyShoesTypeOptions = spouseConfig.BoyShoesTypeOptions,
                    BoyShoesType = spouseConfig.BoyShoesType,
                    BoyShoesColorOptions = spouseConfig.BoyShoesColorOptions,
                    BoyShoesColor = spouseConfig.BoyShoesColor,
                    BoyNeckCollarTypeOptions = spouseConfig.BoyNeckCollarTypeOptions,
                    BoyNeckCollarType = spouseConfig.BoyNeckCollarType,
                    BoyNeckCollarColorOptions = spouseConfig.BoyNeckCollarColorOptions,
                    BoyNeckCollarColor = spouseConfig.BoyNeckCollarColor,
                    BoyPajamaTypeOptions = spouseConfig.BoyPajamaTypeOptions,
                    BoyPajamaType = spouseConfig.BoyPajamaType,
                    BoyPajamaColorOptions = spouseConfig.BoyPajamaColorOptions,
                    BoyPajamaColor = spouseConfig.BoyPajamaColor,
                    BoyFestivalSpringHatTypeOptions = spouseConfig.BoyFestivalSpringHatTypeOptions,
                    BoyFestivalSpringHatType = spouseConfig.BoyFestivalSpringHatType,
                    BoyFestivalSpringHatColorOptions = spouseConfig.BoyFestivalSpringHatColorOptions,
                    BoyFestivalSpringHatColor = spouseConfig.BoyFestivalSpringHatColor,
                    BoyFestivalSummerHatTypeOptions = spouseConfig.BoyFestivalSummerHatTypeOptions,
                    BoyFestivalSummerHatType = spouseConfig.BoyFestivalSummerHatType,
                    BoyFestivalSummerHatColorOptions = spouseConfig.BoyFestivalSummerHatColorOptions,
                    BoyFestivalSummerHatColor = spouseConfig.BoyFestivalSummerHatColor,
                    BoyFestivalSummerPantsTypeOptions = spouseConfig.BoyFestivalSummerPantsTypeOptions,
                    BoyFestivalSummerPantsType = spouseConfig.BoyFestivalSummerPantsType,
                    BoyFestivalSummerPantsColorOptions = spouseConfig.BoyFestivalSummerPantsColorOptions,
                    BoyFestivalSummerPantsColor = spouseConfig.BoyFestivalSummerPantsColor,
                    BoyFestivalFallPantsTypeOptions = spouseConfig.BoyFestivalFallPantsTypeOptions,
                    BoyFestivalFallPantsType = spouseConfig.BoyFestivalFallPantsType,
                    BoyFestivalFallPantsColorOptions = spouseConfig.BoyFestivalFallPantsColorOptions,
                    BoyFestivalFallPantsColor = spouseConfig.BoyFestivalFallPantsColor,
                    BoyFestivalWinterHatTypeOptions = spouseConfig.BoyFestivalWinterHatTypeOptions,
                    BoyFestivalWinterHatType = spouseConfig.BoyFestivalWinterHatType,
                    BoyFestivalWinterHatColorOptions = spouseConfig.BoyFestivalWinterHatColorOptions,
                    BoyFestivalWinterHatColor = spouseConfig.BoyFestivalWinterHatColor,
                    BoyFestivalWinterPantsTypeOptions = spouseConfig.BoyFestivalWinterPantsTypeOptions,
                    BoyFestivalWinterPantsType = spouseConfig.BoyFestivalWinterPantsType,
                    BoyFestivalWinterPantsColorOptions = spouseConfig.BoyFestivalWinterPantsColorOptions,
                    BoyFestivalWinterPantsColor = spouseConfig.BoyFestivalWinterPantsColor,
                    BoyFestivalWinterSkarfTypeOptions = spouseConfig.BoyFestivalWinterSkarfTypeOptions,
                    BoyFestivalWinterSkarfType = spouseConfig.BoyFestivalWinterSkarfType,
                    BoyFestivalWinterSkarfColorOptions = spouseConfig.BoyFestivalWinterSkarfColorOptions,
                    BoyFestivalWinterSkarfColor = spouseConfig.BoyFestivalWinterSkarfColor
                };
                OnPartsChanged?.Invoke(child, parts);
                return parts;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Toddler:Boy] 파츠 추출 예외: {ex.Message}");
                var def = GetDefaultParts(child, false, true);
                OnPartsChanged?.Invoke(child, def);
                return def;
            }
        }

        // === 여아(1세) 파츠 조합 ===
        public static ChildParts GetPartsForGirl(Child child, ModConfig config)
        {
            try
            {
                if (child == null || config == null || child.Age != 1 || child.Gender)
                    return GetDefaultParts(child, false, false);

                string spouse = AppearanceManager.GetRealSpouseName(child);
                if (string.IsNullOrEmpty(spouse)
                    || !config.SpouseConfigs.TryGetValue(spouse, out var spouseConfig)
                    || spouseConfig == null)
                    return GetDefaultParts(child, false, false);

                var parts = new ChildParts
                {
                    SpouseName = spouse,
                    GirlHairTypeOptions = spouseConfig.GirlHairTypeOptions,
                    GirlHairType = spouseConfig.GirlHairType,
                    GirlHairColorOptions = spouseConfig.GirlHairColorOptions,
                    GirlHairColor = spouseConfig.GirlHairColor,
                    GirlEyeTypeOptions = spouseConfig.GirlEyeTypeOptions,
                    GirlEyeType = spouseConfig.GirlEyeType,
                    GirlEyeColorOptions = spouseConfig.GirlEyeColorOptions,
                    GirlEyeColor = spouseConfig.GirlEyeColor,
                    GirlSkinTypeOptions = spouseConfig.GirlSkinTypeOptions,
                    GirlSkinType = spouseConfig.GirlSkinType,
                    GirlSkinColorOptions = spouseConfig.GirlSkinColorOptions,
                    GirlSkinColor = spouseConfig.GirlSkinColor,
                    GirlTopSpringTypeOptions = spouseConfig.GirlTopSpringTypeOptions,
                    GirlTopSpringType = spouseConfig.GirlTopSpringType,
                    GirlTopSpringColorOptions = spouseConfig.GirlTopSpringColorOptions,
                    GirlTopSpringColor = spouseConfig.GirlTopSpringColor,
                    GirlTopSummerTypeOptions = spouseConfig.GirlTopSummerTypeOptions,
                    GirlTopSummerType = spouseConfig.GirlTopSummerType,
                    GirlTopSummerColorOptions = spouseConfig.GirlTopSummerColorOptions,
                    GirlTopSummerColor = spouseConfig.GirlTopSummerColor,
                    GirlTopFallTypeOptions = spouseConfig.GirlTopFallTypeOptions,
                    GirlTopFallType = spouseConfig.GirlTopFallType,
                    GirlTopFallColorOptions = spouseConfig.GirlTopFallColorOptions,
                    GirlTopFallColor = spouseConfig.GirlTopFallColor,
                    GirlTopWinterTypeOptions = spouseConfig.GirlTopWinterTypeOptions,
                    GirlTopWinterType = spouseConfig.GirlTopWinterType,
                    GirlTopWinterColorOptions = spouseConfig.GirlTopWinterColorOptions,
                    GirlTopWinterColor = spouseConfig.GirlTopWinterColor,
                    SkirtTypeOptions = spouseConfig.SkirtTypeOptions,
                    SkirtType = spouseConfig.SkirtType,
                    SkirtColorOptions = spouseConfig.SkirtColorOptions,
                    SkirtColor = spouseConfig.SkirtColor,
                    GirlShoesTypeOptions = spouseConfig.GirlShoesTypeOptions,
                    GirlShoesType = spouseConfig.GirlShoesType,
                    GirlShoesColorOptions = spouseConfig.GirlShoesColorOptions,
                    GirlShoesColor = spouseConfig.GirlShoesColor,
                    GirlNeckCollarTypeOptions = spouseConfig.GirlNeckCollarTypeOptions,
                    GirlNeckCollarType = spouseConfig.GirlNeckCollarType,
                    GirlNeckCollarColorOptions = spouseConfig.GirlNeckCollarColorOptions,
                    GirlNeckCollarColor = spouseConfig.GirlNeckCollarColor,
                    GirlPajamaTypeOptions = spouseConfig.GirlPajamaTypeOptions,
                    GirlPajamaType = spouseConfig.GirlPajamaType,
                    GirlPajamaColorOptions = spouseConfig.GirlPajamaColorOptions,
                    GirlPajamaColor = spouseConfig.GirlPajamaColor,
                    GirlFestivalSpringHatTypeOptions = spouseConfig.GirlFestivalSpringHatTypeOptions,
                    GirlFestivalSpringHatType = spouseConfig.GirlFestivalSpringHatType,
                    GirlFestivalSpringHatColorOptions = spouseConfig.GirlFestivalSpringHatColorOptions,
                    GirlFestivalSpringHatColor = spouseConfig.GirlFestivalSpringHatColor,
                    GirlFestivalSummerHatTypeOptions = spouseConfig.GirlFestivalSummerHatTypeOptions,
                    GirlFestivalSummerHatType = spouseConfig.GirlFestivalSummerHatType,
                    GirlFestivalSummerHatColorOptions = spouseConfig.GirlFestivalSummerHatColorOptions,
                    GirlFestivalSummerHatColor = spouseConfig.GirlFestivalSummerHatColor,
                    GirlFestivalSummerSkirtTypeOptions = spouseConfig.GirlFestivalSummerSkirtTypeOptions,
                    GirlFestivalSummerSkirtType = spouseConfig.GirlFestivalSummerSkirtType,
                    GirlFestivalSummerSkirtColorOptions = spouseConfig.GirlFestivalSummerSkirtColorOptions,
                    GirlFestivalSummerSkirtColor = spouseConfig.GirlFestivalSummerSkirtColor,
                    GirlFestivalFallSkirtTypeOptions = spouseConfig.GirlFestivalFallSkirtTypeOptions,
                    GirlFestivalFallSkirtType = spouseConfig.GirlFestivalFallSkirtType,
                    GirlFestivalFallSkirtColorOptions = spouseConfig.GirlFestivalFallSkirtColorOptions,
                    GirlFestivalFallSkirtColor = spouseConfig.GirlFestivalFallSkirtColor,
                    GirlFestivalWinterHatTypeOptions = spouseConfig.GirlFestivalWinterHatTypeOptions,
                    GirlFestivalWinterHatType = spouseConfig.GirlFestivalWinterHatType,
                    GirlFestivalWinterHatColorOptions = spouseConfig.GirlFestivalWinterHatColorOptions,
                    GirlFestivalWinterHatColor = spouseConfig.GirlFestivalWinterHatColor,
                    GirlFestivalWinterSkirtTypeOptions = spouseConfig.GirlFestivalWinterSkirtTypeOptions,
                    GirlFestivalWinterSkirtType = spouseConfig.GirlFestivalWinterSkirtType,
                    GirlFestivalWinterSkirtColorOptions = spouseConfig.GirlFestivalWinterSkirtColorOptions,
                    GirlFestivalWinterSkirtColor = spouseConfig.GirlFestivalWinterSkirtColor,
                    GirlFestivalWinterSkarfTypeOptions = spouseConfig.GirlFestivalWinterSkarfTypeOptions,
                    GirlFestivalWinterSkarfType = spouseConfig.GirlFestivalWinterSkarfType,
                    GirlFestivalWinterSkarfColorOptions = spouseConfig.GirlFestivalWinterSkarfColorOptions,
                    GirlFestivalWinterSkarfColor = spouseConfig.GirlFestivalWinterSkarfColor
                };
                OnPartsChanged?.Invoke(child, parts);
                return parts;
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[PartsManager][Toddler:Girl] 파츠 추출 예외: {ex.Message}");
                var def = GetDefaultParts(child, false, false);
                OnPartsChanged?.Invoke(child, def);
                return def;
            }
        }

        // === Default 파츠 (누락/에러시 자동 복구) ===
        public static ChildParts GetDefaultParts(Child child, bool isBaby = false, bool? isBoy = null)
        {
            // 실제 옵션 기본값은 사장님 맞춤!
            var def = new ChildParts
            {
                SpouseName = "Default"
            };

                if (isBaby)
                {
                    // 아기(0세)
                    def.BabyHairTypeOptions = new() { "BabyHair" };
                    def.BabyHairType = "BabyHair";
                    def.BabyHairColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "BabyHair.png" } } };
                    def.BabyHairColor = "BabyHair.png";

                    def.BabyEyeTypeOptions = new() { "BabyEye" };
                    def.BabyEyeType = "BabyEye";
                    def.BabyEyeColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "BabyEye.png" } } };
                    def.BabyEyeColor = "BabyEye.png";

                    def.BabySkinTypeOptions = new() { "BabySkin" };
                    def.BabySkinType = "BabySkin";
                    def.BabySkinColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "BabySkin.png" } } };
                    def.BabySkinColor = "BabySkin.png";

                    def.BabyBodyTypeOptions = new() { "BabyBody" };
                    def.BabyBodyType = "BabyBody";
                    def.BabyBodyColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "BabyBody.png" } } };
                    def.BabyBodyColor = "BabyBody.png";
                }
                else if (isBoy == true)
                {
                    // 여아(1세)
                    def.GirlHairTypeOptions = new() { "GirlHair" };
                    def.GirlHairType = "GirlHair";
                    def.GirlHairColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "CherryTwin.png", "TwinTail.png", "PonyTail.png" } } };
                    def.GirlHairColor = "CherryTwin.png";

                    def.GirlEyeTypeOptions = new() { "GirlEye" };
                    def.GirlEyeType = "GirlEye";
                    def.GirlEyeColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Eye.png" } } };
                    def.GirlEyeColor = "Eye.png";

                    def.GirlSkinTypeOptions = new() { "GirlSkin" };
                    def.GirlSkinType = "GirlSkin";
                    def.GirlSkinColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skin.png" } } };
                    def.GirlSkinColor = "Skin.png";

                    def.GirlTopSpringTypeOptions = new() { "GirlSpringTop" };
                    def.GirlTopSpringType = "GirlSpringTop";
                    def.GirlTopSpringColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.GirlTopSpringColor = "Top_Short.png";

                    def.GirlTopSummerTypeOptions = new() { "GirlSummerTop" };
                    def.GirlTopSummerType = "GirlSummerTop";
                    def.GirlTopSummerColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.GirlTopSummerColor = "Top_Short.png";

                    def.GirlTopFallTypeOptions = new() { "GirlFallTop" };
                    def.GirlTopFallType = "GirlFallTop";
                    def.GirlTopFallColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.GirlTopFallColor = "Top_Short.png";

                    def.GirlTopWinterTypeOptions = new() { "GirlWinterTop" };
                    def.GirlTopWinterType = "GirlWinterTop";
                    def.GirlTopWinterColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.GirlTopWinterColor = "Top_Short.png";

                    // 치마
                    def.SkirtTypeOptions = new() { "Skirt" };
                    def.SkirtType = "Skirt";
                    def.SkirtColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Black.png", "Blue.png", "Brown.png", "Emerald.png", "Green.png", "MossGreen.png", "Orange.png", "Pink.png", "Purple.png", "Red.png", "SkyBlue.png", "Violet.png", "Yellow.png", "YellowGreen.png" } } };
                    def.SkirtColor = "Red.png";

                    // 신발
                    def.GirlShoesTypeOptions = new() { "Shoes" };
                    def.GirlShoesType = "Shoes";
                    def.GirlShoesColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Black.png", "Blue.png", "Red.png", "Socks.png" } } };
                    def.GirlShoesColor = "Red.png";

                    // 넥칼라
                    def.GirlNeckCollarTypeOptions = new() { "NeckCollar" };
                    def.GirlNeckCollarType = "NeckCollar";
                    def.GirlNeckCollarColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Abigail.png","Alex.png","Black.png","Blue.png","Brown.png","Elliott.png","Emerald.png","Emily.png","Green.png","Haley.png","Harvey.png","Leah.png","Maru.png","MossGreen.png","Orange.png","Penny.png","Pink.png","Purple.png","Red.png","Sam.png","Sebastian.png","Shane.png","SkyBlue.png","Violet.png","Yellow.png","YellowGreen.png" } } };
                    def.GirlNeckCollarColor = "Red.png";

                    // 잠옷
                    def.GirlPajamaTypeOptions = new() { "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi" };
                    def.GirlPajamaType = "Frog";

                    def.GirlPajamaColorOptions = new Dictionary<string, List<string>>()
                    {
                        { "Frog",        new List<string> { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
                        { "LesserPanda", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
                        { "Raccoon",     new List<string> { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" } },
                        { "Sheep",       new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
                        { "Shark",       new List<string> { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
                        { "WelshCorgi",  new List<string> { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" } }
                    };
                    def.GirlPajamaColor = "Pink.png";

                    // 축제복(모자/의상/목도리) - 계절별 대표값
                    def.GirlFestivalSpringHatTypeOptions = new() { "FestivalSpringHat" };
                    def.GirlFestivalSpringHatType = "FestivalSpringHat";
                    def.GirlFestivalSpringHatColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Hat.png" } } };
                    def.GirlFestivalSpringHatColor = "Hat.png";

                    def.GirlFestivalSummerHatTypeOptions = new() { "FestivalSummerHat" };
                    def.GirlFestivalSummerHatType = "FestivalSummerHat";
                    def.GirlFestivalSummerHatColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Hat.png" } } };
                    def.GirlFestivalSummerHatColor = "Hat.png";

                    def.GirlFestivalSummerSkirtTypeOptions = new() { "FestivalSummerSkirt" };
                    def.GirlFestivalSummerSkirtType = "FestivalSummerSkirt";
                    def.GirlFestivalSummerSkirtColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skirt1.png", "Skirt2.png" } } };
                    def.GirlFestivalSummerSkirtColor = "Skirt1.png";

                    def.GirlFestivalFallSkirtTypeOptions = new() { "FestivalFallSkirt" };
                    def.GirlFestivalFallSkirtType = "FestivalFallSkirt";
                    def.GirlFestivalFallSkirtColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skirt.png" } } };
                    def.GirlFestivalFallSkirtColor = "Skirt.png";

                    def.GirlFestivalWinterHatTypeOptions = new() { "FestivalWinterHat" };
                    def.GirlFestivalWinterHatType = "FestivalWinterHat";
                    def.GirlFestivalWinterHatColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Hat.png" } } };
                    def.GirlFestivalWinterHatColor = "Hat.png";

                    def.GirlFestivalWinterSkirtTypeOptions = new() { "FestivalWinterSkirt" };
                    def.GirlFestivalWinterSkirtType = "FestivalWinterSkirt";
                    def.GirlFestivalWinterSkirtColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skirt1.png", "Skirt2.png" } } };
                    def.GirlFestivalWinterSkirtColor = "Skirt1.png";

                    def.GirlFestivalWinterSkarfTypeOptions = new() { "FestivalWinterSkarf" };
                    def.GirlFestivalWinterSkarfType = "FestivalWinterSkarf";
                    def.GirlFestivalWinterSkarfColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skarf.png" } } };
                    def.GirlFestivalWinterSkarfColor = "Skarf.png";
                }
                else
                {
                    // 남아
                    def.BoyHairTypeOptions = new() { "BoyHair" };
                    def.BoyHairType = "BoyHair";
                    def.BoyHairColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "ShortCut.png" } } };
                    def.BoyHairColor = "ShortCut.png";

                    def.BoyEyeTypeOptions = new() { "BoyEye" };
                    def.BoyEyeType = "BoyEye";
                    def.BoyEyeColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Eye.png" } } };
                    def.BoyEyeColor = "Eye.png";

                    def.BoySkinTypeOptions = new() { "BoySkin" };
                    def.BoySkinType = "BoySkin";
                    def.BoySkinColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skin.png" } } };
                    def.BoySkinColor = "Skin.png";

                    // 상의
                    def.BoyTopSpringTypeOptions = new() { "BoySpringTop" };
                    def.BoyTopSpringType = "BoySpringTop";
                    def.BoyTopSpringColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.BoyTopSpringColor = "Top_Short.png";

                    def.BoyTopSummerTypeOptions = new() { "BoySummerTop" };
                    def.BoyTopSummerType = "BoySummerTop";
                    def.BoyTopSummerColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.BoyTopSummerColor = "Top_Short.png";

                    def.BoyTopFallTypeOptions = new() { "BoyFallTop" };
                    def.BoyTopFallType = "BoyFallTop";
                    def.BoyTopFallColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.BoyTopFallColor = "Top_Short.png";

                    def.BoyTopWinterTypeOptions = new() { "BoyWinterTop" };
                    def.BoyTopWinterType = "BoyWinterTop";
                    def.BoyTopWinterColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } } };
                    def.BoyTopWinterColor = "Top_Short.png";

                    // 바지
                    def.PantsTypeOptions = new() { "Pants" };
                    def.PantsType = "Pants";
                    def.PantsColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Black.png", "Blue.png", "Brown.png", "Emerald.png", "Green.png", "MossGreen.png", "Orange.png", "Pink.png", "Purple.png", "Red.png", "SkyBlue.png", "Violet.png", "Yellow.png", "YellowGreen.png" } } };
                    def.PantsColor = "Blue.png";

                    // 신발
                    def.BoyShoesTypeOptions = new() { "Shoes" };
                    def.BoyShoesType = "Shoes";
                    def.BoyShoesColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Black.png", "Blue.png", "Red.png", "Socks.png" } } };
                    def.BoyShoesColor = "Blue.png";

                    // 넥칼라
                    def.BoyNeckCollarTypeOptions = new() { "NeckCollar" };
                    def.BoyNeckCollarType = "NeckCollar";
                    def.BoyNeckCollarColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Abigail.png","Alex.png","Black.png","Blue.png","Brown.png","Elliott.png","Emerald.png","Emily.png","Green.png","Haley.png","Harvey.png","Leah.png","Maru.png","MossGreen.png","Orange.png","Penny.png","Pink.png","Purple.png","Red.png","Sam.png","Sebastian.png","Shane.png","SkyBlue.png","Violet.png","Yellow.png","YellowGreen.png" } } };
                    def.BoyNeckCollarColor = "Blue.png";

                    // 잠옷
                    def.GirlPajamaColorOptions = new Dictionary<string, List<string>>()
                    {
                        { "Frog",        new List<string> { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
                        { "LesserPanda", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
                        { "Raccoon",     new List<string> { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" } },
                        { "Sheep",       new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
                        { "Shark",       new List<string> { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
                        { "WelshCorgi",  new List<string> { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" } }
                    };
                    def.GirlPajamaColor = "Blue.png";

                    // 축제복
                    def.BoyFestivalSpringHatTypeOptions = new() { "FestivalSpringHat" };
                    def.BoyFestivalSpringHatType = "FestivalSpringHat";
                    def.BoyFestivalSpringHatColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Hat.png" } } };
                    def.BoyFestivalSpringHatColor = "Hat.png";

                    def.BoyFestivalSummerHatTypeOptions = new() { "FestivalSummerHat" };
                    def.BoyFestivalSummerHatType = "FestivalSummerHat";
                    def.BoyFestivalSummerHatColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Hat.png" } } };
                    def.BoyFestivalSummerHatColor = "Hat.png";

                    def.BoyFestivalSummerPantsTypeOptions = new() { "FestivalSummerPants" };
                    def.BoyFestivalSummerPantsType = "FestivalSummerPants";
                    def.BoyFestivalSummerPantsColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Pants1.png", "Pants2.png" } } };
                    def.BoyFestivalSummerPantsColor = "Pants1.png";

                    def.BoyFestivalFallPantsTypeOptions = new() { "FestivalFallPants" };
                    def.BoyFestivalFallPantsType = "FestivalFallPants";
                    def.BoyFestivalFallPantsColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Pants.png" } } };
                    def.BoyFestivalFallPantsColor = "Pants.png";

                    def.BoyFestivalWinterHatTypeOptions = new() { "FestivalWinterHat" };
                    def.BoyFestivalWinterHatType = "FestivalWinterHat";
                    def.BoyFestivalWinterHatColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Hat.png" } } };
                    def.BoyFestivalWinterHatColor = "Hat.png";

                    def.BoyFestivalWinterPantsTypeOptions = new() { "FestivalWinterPants" };
                    def.BoyFestivalWinterPantsType = "FestivalWinterPants";
                    def.BoyFestivalWinterPantsColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Pants1.png", "Pants2.png" } } };
                    def.BoyFestivalWinterPantsColor = "Pants1.png";

                    def.BoyFestivalWinterSkarfTypeOptions = new() { "FestivalWinterSkarf" };
                    def.BoyFestivalWinterSkarfType = "FestivalWinterSkarf";
                    def.BoyFestivalWinterSkarfColorOptions = new Dictionary<string, List<string>>() { { "Default", new List<string> { "Skarf.png" } } };
                    def.BoyFestivalWinterSkarfColor = "Skarf.png";
                }
                return def;
            }

        // (SMAPI Helper로 동적 로딩)
        public static Microsoft.Xna.Framework.Graphics.Texture2D TryLoadTexture(string assetPath)
        {
            try
            {
                if (Helper == null || string.IsNullOrWhiteSpace(assetPath))
                    return null;
                return Helper.ModContent.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(assetPath);
            }
            catch
            {
                return null;
            }
        }
    }
}