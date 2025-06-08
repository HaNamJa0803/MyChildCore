using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley.Characters;
using MyChildCore;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 모든 외형 파츠 DTO (성별/계절/잠옷/축제/기본 분리)
    /// </summary>
    public class ChildParts
    {
        // 공통 메타
        public string SpouseName { get; set; }
        public bool IsMale { get; set; }
        public bool EnablePajama { get; set; }
        public bool EnableFestival { get; set; }

        // --- 아기(0세)
        public string BabyHairStyles { get; set; }
        public string BabyEyes { get; set; }
        public string BabySkins { get; set; }
        public string BabyBodies { get; set; }

        // --- 여자 자녀
        public string GirlHairStyles { get; set; }
        public string GirlEyes { get; set; }
        public string GirlSkins { get; set; }
        public string GirlTopSpringOptions { get; set; }
        public string GirlTopSummerOptions { get; set; }
        public string GirlTopFallOptions { get; set; }
        public string GirlTopWinterOptions { get; set; }
        public string SkirtColorOptions { get; set; }
        public string GirlShoesColorOptions { get; set; }
        public string GirlNeckCollarColorOptions { get; set; }
        public string GirlPajamaTypeOptions { get; set; }
        public string GirlPajamaColorOptions { get; set; }
        public string GirlFestivalSummerSkirtOptions { get; set; }
        public string GirlFestivalWinterSkirtOptions { get; set; }
        public string GirlFestivalFallSkirts { get; set; }

        // --- 남자 자녀
        public string BoyHairStyles { get; set; }
        public string BoyEyes { get; set; }
        public string BoySkins { get; set; }
        public string BoyTopSpringOptions { get; set; }
        public string BoyTopSummerOptions { get; set; }
        public string BoyTopFallOptions { get; set; }
        public string BoyTopWinterOptions { get; set; }
        public string PantsColorOptions { get; set; }
        public string BoyShoesColorOptions { get; set; }
        public string BoyNeckCollarColorOptions { get; set; }
        public string BoyPajamaTypeOptions { get; set; }
        public string BoyPajamaColorOptions { get; set; }
        public string BoyFestivalSummerPantsOptions { get; set; }
        public string BoyFestivalWinterPantsOptions { get; set; }
        public string BoyFestivalFallPants { get; set; }

        // --- 축제 (공용)
        public string FestivalSpringHat { get; set; }
        public string FestivalSummerHat { get; set; }
        public string FestivalWinterHat { get; set; }
        public string FestivalWinterScarf { get; set; }
    }
}