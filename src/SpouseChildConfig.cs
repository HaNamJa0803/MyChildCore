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
        public string GirlTopSpringOptions { get; set; } = DropdownConfig.GirlTopSpringOptions[0];
        public string GirlTopSummerOptions { get; set; } = DropdownConfig.GirlTopSummerOptions[0];
        public string GirlTopFallOptions { get; set; } = DropdownConfig.GirlTopFallOptions[0];
        public string GirlTopWinterOptions { get; set; } = DropdownConfig.GirlTopWinterOptions[0];
        public string GirlSkirtColorOptions { get; set; } = DropdownConfig.GirlSkirtColorOptions[0];
        public string GirlShoesColorOptions { get; set; } = DropdownConfig.GirlShoesColorOptions[0];
        public string GirlNeckCollarColorOptions { get; set; } = DropdownConfig.GirlNeckCollarColorOptions[0];
        public string GirlPajamaTypeOptions { get; set; } = DropdownConfig.GirlPajamaTypeOptions[0];
        public string GirlPajamaColorOptions { get; set; } = DropdownConfig.GirlPajamaColorOptions[DropdownConfig.PajamaTypeOptions[0]][0];
        public string GirlFestivalSummerSkirtOptions { get; set; } = DropdownConfig.GirlFestivalSummerSkirtOptions[0];
        public string GirlFestivalWinterSkirtOptions { get; set; } = DropdownConfig.GirlFestivalWinterSkirtOptions[0];
        public string GirlFestivalFallSkirts { get; set; } = DropdownConfig.GirlFestivalFallSkirts[0];

        // 남자 자녀
        public string BoyHairStyles { get; set; } = DropdownConfig.BoyHairStyles[0];
        public string BoyEyes { get; set; } = DropdownConfig.BoyEyes[0];
        public string BoySkins { get; set; } = DropdownConfig.BoySkins[0];
        public string BoyTopSpringOptions { get; set; } = DropdownConfig.BoyTopSpringOptions[0];
        public string BoyTopSummerOptions { get; set; } = DropdownConfig.BoyTopSummerOptions[0];
        public string BoyTopFallOptions { get; set; } = DropdownConfig.BoyTopFallOptions[0];
        public string BoyTopWinterOptions { get; set; } = DropdownConfig.BoyTopWinterOptions[0];
        public string BoyPantsColorOptions { get; set; } = DropdownConfig.BoyPantsColorOptions[0];
        public string BoyShoesColorOptions { get; set; } = DropdownConfig.BoyShoesColorOptions[0];
        public string BoyNeckCollarColorOptions { get; set; } = DropdownConfig.BoyNeckCollarColorOptions[0];
        public string BoyPajamaTypeOptions { get; set; } = DropdownConfig.BoyPajamaTypeOptions[0];
        public string BoyPajamaColorOptions { get; set; } = DropdownConfig.BoyPajamaColorOptions[DropdownConfig.PajamaTypeOptions[0]][0];
        public string BoyFestivalSummerPantsOptions { get; set; } = DropdownConfig.BoyFestivalSummerPantsOptions[0];
        public string BoyFestivalWinterPantsOptions { get; set; } = DropdownConfig.BoyFestivalWinterPantsOptions[0];
        public string BoyFestivalFallPants { get; set; } = DropdownConfig.BoyFestivalFallPants[0];

        // 축제(공용, 선택 불가능/읽기 전용으로 모든 자녀한테 출력만)
        public string FestivalSpringHat { get; set; } = DropdownConfig.FestivalSpringHat[0];
        public string FestivalSummerHat { get; set; } = DropdownConfig.FestivalSummerHat[0];
        public string FestivalWinterHat { get; set; } = DropdownConfig.FestivalWinterHat[0];
        public string FestivalWinterScarf { get; set; } = DropdownConfig.FestivalWinterScarf[0];
    }
}