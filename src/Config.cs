using System.Collections.Generic;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 전체 모드 설정 연결부(GMCM, 매니저, 저장 등 통합)
    /// </summary>
    public class DropdownConfig
    {
        // ===== 배우자 목록(정확한 스펠링 적용) =====
        public static readonly string[] SpouseNames =
        {
            "Abigail", "Emily", "Haley", "Maru", "Leah", "Penny",
            "Alissa", "Faye", "Maddie", "Paula", "Daia", "Ysabelle", "Corine", "Blair", "Flor", "Irene", "Kiarra"
        };

        // ===== 배우자별 자녀 설정 (연결부) =====
        public Dictionary<string, SpouseChildConfig> SpouseConfigs { get; set; } = new();

        // ===== 외형 선택 옵션(드롭다운/연동 기준) =====
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

        public DropdownConfig()
        {
            // 배우자별 개별 설정 인스턴스 자동 생성(초기 연결)
            foreach (var spouse in SpouseNames)
                if (!SpouseConfigs.ContainsKey(spouse))
                    SpouseConfigs[spouse] = new SpouseChildConfig();
        }
    }

    /// <summary>
    /// 배우자별 자녀 성별별(남/여) 옵션 연결 DTO
    /// </summary>
    public class SpouseChildConfig
    {
        // Female(여아)
        public string GirlHairStyle { get; set; } = "CherryTwin";
        public string GirlSkirt { get; set; } = "Skirt_01";
        public string GirlShoes { get; set; } = "Shoes_01";
        public string GirlNeckCollar { get; set; } = "NeckCollar_01";
        public string GirlPajamaStyle { get; set; } = "Frog";
        public int GirlPajamaColorIndex { get; set; } = 1;

        // Male(남아)
        public string BoyPants { get; set; } = "Pants_01";
        public string BoyShoes { get; set; } = "Shoes_01";
        public string BoyNeckCollar { get; set; } = "NeckCollar_01";
        public string BoyPajamaStyle { get; set; } = "Frog";
        public int BoyPajamaColorIndex { get; set; } = 1;
    }
}