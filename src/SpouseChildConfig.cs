using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore
{
    public class SpouseChildConfig
    {
        // 아기 (공용)
        public string BabyHairStyles { get; set; } = DropdownConfig.BabyHairStyles[0];
        public string BabyEyes { get; set; } = DropdownConfig.BabyEyes[0];
        public string BabySkins { get; set; } = DropdownConfig.BabySkins[0];
        public string BabyBodies { get; set; } = DropdownConfig.BabyBodies[0];

        // 여자 자녀
        public string GirlHairStyles { get; set; } = DropdownConfig.GirlHairStyles[0];
        public string GirlEyes { get; set; } = DropdownConfig.GirlEyes[0];
        public string GirlSkins { get; set; } = DropdownConfig.GirlSkins[0];
        public string GirlTopSpring { get; set; } = DropdownConfig.GirlTopSpringOptions[0];
        public string GirlTopSummer { get; set; } = DropdownConfig.GirlTopSummerOptions[0];
        public string GirlTopFall { get; set; } = DropdownConfig.GirlTopFallOptions[0];
        public string GirlTopWinter { get; set; } = DropdownConfig.GirlTopWinterOptions[0];
        public string GirlSkirtColor { get; set; } = DropdownConfig.SkirtColors[0];
        public string GirlShoesColor { get; set; } = DropdownConfig.ShoesColors[0];
        public string GirlNeckCollarColor { get; set; } = DropdownConfig.NeckCollarColors[0];
        public string GirlPajamaType { get; set; } = DropdownConfig.PajamaTypeOption[0];
        public string GirlPajamaColor { get; set; } = DropdownConfig.PajamaColorOptions[DropdownConfig.PajamaTypeOptions[0]][0];
        public string GirlFestivalSummerSkirt { get; set; } = DropdownConfig.GirlFestivalSummerSkirtOptions[0];
        public string GirlFestivalWinterSkirt { get; set; } = DropdownConfig.GirlFestivalWinterSkirtOptions[0];
        public string GirlFestivalFallSkirt { get; set; } = DropdownConfig.GirlFestivalFallSkirts[0];

        // 남자 자녀
        public string BoyHairStyle { get; set; } = DropdownConfig.BoyHairStyles[0];
        public string BoyEye { get; set; } = DropdownConfig.BoyEyes[0];
        public string BoySkin { get; set; } = DropdownConfig.BoySkins[0];
        public string BoyTopSpring { get; set; } = DropdownConfig.BoyTopSpringOptions[0];
        public string BoyTopSummer { get; set; } = DropdownConfig.BoyTopSummerOptions[0];
        public string BoyTopFall { get; set; } = DropdownConfig.BoyTopFallOptions[0];
        public string BoyTopWinter { get; set; } = DropdownConfig.BoyTopWinterOptions[0];
        public string BoyPantsColor { get; set; } = DropdownConfig.PantsColors[0];
        public string BoyShoesColor { get; set; } = DropdownConfig.ShoesColors[0];
        public string BoyNeckCollarColor { get; set; } = DropdownConfig.NeckCollarColors[0];
        public string BoyPajamaType { get; set; } = DropdownConfig.PajamaTypeOptions[0];
        public string BoyPajamaColor { get; set; } = DropdownConfig.PajamaColorOptions[DropdownConfig.PajamaTypeOptions[0]][0];
        public string BoyFestivalSummerPants { get; set; } = DropdownConfig.BoyFestivalSummerPantsOptions[0];
        public string BoyFestivalWinterPants { get; set; } = DropdownConfig.BoyFestivalWinterPantsOptions[0];
        public string BoyFestivalFallPants { get; set; } = DropdownConfig.BoyFestivalFallPants[0];

        // 축제(공용, 선택 불가능/읽기 전용으로 모든 자녀한테 출력만)
        public string FestivalSpringHat { get; set; } = DropdownConfig.FestivalSpringHat[0];
        public string FestivalWinterHat { get; set; } = DropdownConfig.FestivalWinterHat[0];
        public string FestivalWinterScarf { get; set; } = DropdownConfig.FestivalWinterScarf[0];
    }
}