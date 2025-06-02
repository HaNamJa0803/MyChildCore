using System.Collections.Generic;
using MyChildCore;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// GMCM 드롭다운 설정 객체 (배우자별 + 자녀 성별별 + 커스텀 옵션 + 색상)
    /// </summary>
    public class DropdownConfig
    {
        // 배우자 목록(정확한 스펠링)
        public static readonly string[] SpouseNames =
        {
            "Abigail", "Emily", "Haley", "Maru", "Leah", "Penny",
            "Alissa", "Faye", "Maddie", "Paula", "Daia", "Ysabelle", "Corine", "Blair", "Flor", "Irene", "Kiarra"
        };

        // 배우자별 자녀 설정(딕셔너리: 배우자명 → 옵션)
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        // 드롭다운 옵션값(하드코딩)
        public static readonly string[] GirlHairStyles = { "CherryTwin", "TwinTail", "PonyTail" };
        public static readonly string[] SkirtOptions = {
            "Skirt_01","Skirt_02","Skirt_03","Skirt_04","Skirt_05",
            "Skirt_06","Skirt_07","Skirt_08","Skirt_09","Skirt_10"
        };
        public static readonly string[] PantsOptions = {
            "Pants_01","Pants_02","Pants_03","Pants_04","Pants_05",
            "Pants_06","Pants_07","Pants_08","Pants_09","Pants_10"
        };
        public static readonly string[] ShoesOptions = { "Shoes_01", "Shoes_02", "Shoes_03", "Shoes_04" };
        public static readonly string[] NeckCollarOptions = {
            "NeckCollar_01","NeckCollar_02","NeckCollar_03","NeckCollar_04","NeckCollar_05",
            "NeckCollar_06","NeckCollar_07","NeckCollar_08","NeckCollar_09","NeckCollar_10",
            "NeckCollar_11","NeckCollar_12","NeckCollar_13","NeckCollar_14","NeckCollar_15",
            "NeckCollar_16","NeckCollar_17","NeckCollar_18","NeckCollar_19","NeckCollar_20",
            "NeckCollar_21","NeckCollar_22","NeckCollar_23","NeckCollar_24","NeckCollar_25",
            "NeckCollar_26"
        };
        public static readonly string[] PajamaStyles = { "Frog", "WelshCorgi", "Sheep", "LesserPanda", "Racoon", "Shark" };
        public static readonly Dictionary<string, int> PajamaColorMax = new()
        {
            { "Frog", 8 }, { "WelshCorgi", 8 }, { "Sheep", 7 }, { "LesserPanda", 7 }, { "Racoon", 8 }, { "Shark", 7 }
        };

        // 파츠별 색상 최대치 (슬라이더/드롭다운 바인딩용 상수)
        public const int SkirtColorCount = 10;        // 치마 색상 (0~9)
        public const int PantsColorCount = 10;        // 바지 색상 (0~9)
        public const int ShoesColorCount = 4;         // 신발 색상 (0~3)
        public const int NeckCollarColorCount = 26;   // 넥칼라 색상 (0~25)

        // ----------------- 커스텀 활성화 옵션(체크박스용) -----------------
        /// <summary>
        /// 잠옷 커스텀 활성화 여부 (체크박스)
        /// </summary>
        public bool EnablePajama { get; set; } = true;

        /// <summary>
        /// 축제복 커스텀 활성화 여부 (체크박스)
        /// </summary>
        public bool EnableFestival { get; set; } = true;

        // ----------------- 생성자에서 배우자별 옵션 자동 추가 -----------------
        public DropdownConfig()
        {
            foreach (var spouse in SpouseNames)
                if (!SpouseConfigs.ContainsKey(spouse))
                    SpouseConfigs[spouse] = new SpouseChildConfig();
        }
    }

    /// <summary>
    /// 배우자별 자녀 성별별 개별 옵션 (파츠, 색상 등)
    /// </summary>
    public class SpouseChildConfig
    {
        // 여아(딸)
        public string GirlHairStyle { get; set; } = "CherryTwin";
        public string GirlSkirt { get; set; } = "Skirt_01";
        public int GirlSkirtColor { get; set; } = 0;              // 치마 색상 (option.girl.skirtcolor)
        public string GirlShoes { get; set; } = "Shoes_01";
        public int GirlShoesColor { get; set; } = 0;              // 신발 색상 (option.girl.shoescolor)
        public string GirlNeckCollar { get; set; } = "NeckCollar_01";
        public int GirlNeckCollarColor { get; set; } = 0;         // 넥칼라 색상 (option.girl.neckcollarcolor)
        public string GirlPajamaStyle { get; set; } = "Frog";
        public int GirlPajamaColorIndex { get; set; } = 1;

        // 남아(아들)
        public string BoyPants { get; set; } = "Pants_01";
        public int BoyPantsColor { get; set; } = 0;               // 바지 색상 (option.boy.pantscolor)
        public string BoyShoes { get; set; } = "Shoes_01";
        public int BoyShoesColor { get; set; } = 0;               // 신발 색상 (option.boy.shoescolor)
        public string BoyNeckCollar { get; set; } = "NeckCollar_01";
        public int BoyNeckCollarColor { get; set; } = 0;          // 넥칼라 색상 (option.boy.neckcollarcolor)
        public string BoyPajamaStyle { get; set; } = "Frog";
        public int BoyPajamaColorIndex { get; set; } = 1;
    }
}