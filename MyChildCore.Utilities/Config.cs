using System;
using System.Collections.Generic;

namespace MyChildCore.Config
{
    /// <summary>
    /// 자녀 외형 선택 데이터 (GMCM 연동용)
    /// - 각 배우자 자녀(남/여) 별로 개별 저장
    /// - 하드코딩: GMCM에서 선택할 수 있는 항목만 제한 제공 (스타듀 엔진의 강제 외형 변경 대응)
    /// </summary>
    public class ChildAppearanceConfig
    {
        /// <summary>
        /// 헤어 선택 (여자만 선택 가능, 남자는 항상 "숏" 고정)
        /// - 여자: 체리트윈, 트윈테일, 포니테일 중 선택
        /// - 남자: "숏" 고정
        /// </summary>
        public string HairStyle { get; set; } = "체리트윈";

        /// <summary>하의 선택 (1~10)</summary>
        public int BottomIndex { get; set; } = 1;

        /// <summary>신발 선택 (1~4)</summary>
        public int ShoesIndex { get; set; } = 1;

        /// <summary>넥칼라(액세서리) 선택 (1~26)</summary>
        public int NeckIndex { get; set; } = 1;

        /// <summary>잠옷 스타일 (CSV 기반, 스타일_번호)</summary>
        public string PajamaStyle { get; set; } = "기본";

        /// <summary>잠옷 색상 (CSV 기반)</summary>
        public string PajamaColor { get; set; } = "기본";
    }

    /// <summary>
    /// 전체 Config 클래스 (GMCM 저장 대상)
    /// - 배우자 17명 * 자녀 성별 2 (남/여) = 34개 항목
    /// - 하드코딩: Key = "배우자_Son", "배우자_Daughter"
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// 배우자별 자녀 외형 선택 데이터 (Key = "배우자_Son" / "배우자_Daughter")
        /// </summary>
        public Dictionary<string, ChildAppearanceConfig> ChildrenConfigs { get; set; } = new();

        /// <summary>
        /// 생성자: 배우자 목록 기반으로 기본값 초기화 (남자 = 숏 고정, 여자 = 체리트윈)
        /// </summary>
        public ModConfig()
        {
            string[] spouses = new string[]
            {
                "Abigail", "Alissa", "Blair", "Corine", "Daia", "Emily", "Faye", "Flor",
                "Haley", "Irene", "Kiarra", "Leah", "Maddie", "Maru", "Paula", "Penny", "Ysabelle"
            };

            foreach (var spouse in spouses)
            {
                // 남자 자녀: 헤어는 항상 "숏" 고정 (선택 불가)
                ChildrenConfigs[$"{spouse}_Son"] = new ChildAppearanceConfig
                {
                    HairStyle = "숏"
                };

                // 여자 자녀: 기본 헤어 "체리트윈" (GMCM에서 선택 가능)
                ChildrenConfigs[$"{spouse}_Daughter"] = new ChildAppearanceConfig
                {
                    HairStyle = "체리트윈"
                };
            }
        }
    }
}