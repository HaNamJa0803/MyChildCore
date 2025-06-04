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

    // --- 아기(0세)
    public string BabyHairStyle { get; set; }
    public string BabyEye { get; set; }
    public string BabySkin { get; set; }
    public string BabyBody { get; set; }

    // --- 여자 자녀
    public string GirlHairStyle { get; set; }
    public string GirlEye { get; set; }
    public string GirlSkin { get; set; }
    public string GirlTopSpring { get; set; }
    public string GirlTopSummer { get; set; }
    public string GirlTopFall { get; set; }
    public string GirlTopWinter { get; set; }
    public string GirlSkirtColor { get; set; }
    public string GirlShoesColor { get; set; }
    public string GirlNeckCollarColor { get; set; }
    public string GirlPajamaType { get; set; }
    public string GirlPajamaColor { get; set; }
    public string GirlFestivalSummerSkirt { get; set; }
    public string GirlFestivalWinterSkirt { get; set; }
    public string GirlFestivalFallSkirt { get; set; }

    // --- 남자 자녀
    public string BoyHairStyle { get; set; }
    public string BoyEye { get; set; }
    public string BoySkin { get; set; }
    public string BoyTopSpring { get; set; }
    public string BoyTopSummer { get; set; }
    public string BoyTopFall { get; set; }
    public string BoyTopWinter { get; set; }
    public string BoyPantsColor { get; set; }
    public string BoyShoesColor { get; set; }
    public string BoyNeckCollarColor { get; set; }
    public string BoyPajamaType { get; set; }
    public string BoyPajamaColor { get; set; }
    public string BoyFestivalSummerPants { get; set; }
    public string BoyFestivalWinterPants { get; set; }
    public string BoyFestivalFallPants { get; set; }

    // --- 축제 (공용)
    public string FestivalSpringHat { get; set; }
    public string FestivalWinterHat { get; set; }
    public string FestivalWinterScarf { get; set; }
}