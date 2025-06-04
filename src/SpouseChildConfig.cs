using System.Collections.Generic;

namespace MyChildCore
{
    public class SpouseChildConfig
    {
        // 아기 (공용)
        public string BabyHairStyle { get; set; } = DropdownConfig.HairStyles[0];
        public string BabyEye { get; set; } = DropdownConfig.NeckCollarOptions[0];
        public string BabySkin { get; set; } = DropdownConfig.SkirtColors[0];
        public string BabyBody { get; set; } = DropdownConfig.PajamaTypes[0];

        // 여자 자녀
        public string GirlHairStyle { get; set; } = DropdownConfig.GirlHairStyles[0];
        public string GirlEye { get; set; } = DropdownConfig.NeckCollarOptions[0];
        public string GirlSkin { get; set; } = DropdownConfig.SkirtColors[0];
        public string GirlTopSpring { get; set; } = DropdownConfig.GirlTopSpringOptions[0];
        public string GirlTopSummer { get; set; } = DropdownConfig.GirlTopSummerOptions[0];
        public string GirlTopFall { get; set; } = DropdownConfig.GirlTopFallOptions[0];
        public string GirlTopWinter { get; set; } = DropdownConfig.GirlTopWinterOptions[0];
        public string GirlSkirtColor { get; set; } = DropdownConfig.SkirtColors[0];
        public string GirlShoesColor { get; set; } = DropdownConfig.ShoesOptions[0];
        public string GirlNeckCollarColor { get; set; } = DropdownConfig.NeckCollarOptions[0];
        public string GirlPajamaType { get; set; } = DropdownConfig.PajamaTypes[0];
        public string GirlPajamaColor { get; set; } = DropdownConfig.PajamaColors[DropdownConfig.PajamaTypes[0]][0];
        public string GirlFestivalSummerSkirt { get; set; } = DropdownConfig.GirlFestivalSummerSkirtOptions[0];
        public string GirlFestivalWinterSkirt { get; set; } = DropdownConfig.GirlFestivalWinterSkirtOptions[0];
        public string GirlFestivalFallSkirt { get; set; } = DropdownConfig.GirlFestivalFallSkirtOptions[0];

        // 남자 자녀
        public string BoyHairStyle { get; set; } = DropdownConfig.BoyHairStyles[0];
        public string BoyEye { get; set; } = DropdownConfig.NeckCollarOptions[0];
        public string BoySkin { get; set; } = DropdownConfig.PantsColors[0];
        public string BoyTopSpring { get; set; } = DropdownConfig.BoyTopSpringOptions[0];
        public string BoyTopSummer { get; set; } = DropdownConfig.BoyTopSummerOptions[0];
        public string BoyTopFall { get; set; } = DropdownConfig.BoyTopFallOptions[0];
        public string BoyTopWinter { get; set; } = DropdownConfig.BoyTopWinterOptions[0];
        public string BoyPantsColor { get; set; } = DropdownConfig.PantsColors[0];
        public string BoyShoesColor { get; set; } = DropdownConfig.ShoesOptions[0];
        public string BoyNeckCollarColor { get; set; } = DropdownConfig.NeckCollarOptions[0];
        public string BoyPajamaType { get; set; } = DropdownConfig.PajamaTypes[0];
        public string BoyPajamaColor { get; set; } = DropdownConfig.PajamaColors[DropdownConfig.PajamaTypes[0]][0];
        public string BoyFestivalSummerPants { get; set; } = DropdownConfig.BoyFestivalSummerPantsOptions[0];
        public string BoyFestivalWinterPants { get; set; } = DropdownConfig.BoyFestivalWinterPantsOptions[0];
        public string BoyFestivalFallPants { get; set; } = DropdownConfig.BoyFestivalFallPantsOptions[0];

        // 축제(공용, 선택 불가능/읽기 전용으로 모든 자녀한테 출력만)
        public string FestivalSpringHat { get; set; } = DropdownConfig.FestivalSpringHatOptions[0];
        public string FestivalWinterHat { get; set; } = DropdownConfig.FestivalWinterHatOptions[0];
        public string FestivalWinterScarf { get; set; } = DropdownConfig.FestivalWinterScarfOptions[0];
    }

    public class ModConfig
    {
        // 배우자별 섹션
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();
    }
}