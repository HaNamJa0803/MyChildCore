using MyChildCore;
using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 모든 파츠/드롭다운/옵션 실시간 동기화 + 중앙관리 config
    /// 값 변경시 전체 시스템 자동 동기화까지 지원
    /// </summary>
    public static class DropdownConfig
    {
        // ====== 글로벌 옵션(동기화) ======
        private static bool _enableMod = true;
        public static bool EnableMod
        {
            get => _enableMod;
            set
            {
                if (_enableMod != value)
                {
                    _enableMod = value;
                    OnConfigChanged?.Invoke("EnableMod");
                }
            }
        }

        private static bool _enablePajama = true;
        public static bool EnablePajama
        {
            get => _enablePajama;
            set
            {
                if (_enablePajama != value)
                {
                    _enablePajama = value;
                    OnConfigChanged?.Invoke("EnablePajama");
                }
            }
        }

        private static bool _enableFestival = true;
        public static bool EnableFestival
        {
            get => _enableFestival;
            set
            {
                if (_enableFestival != value)
                {
                    _enableFestival = value;
                    OnConfigChanged?.Invoke("EnableFestival");
                }
            }
        }

        /// <summary>
        /// 값 변경시 호출될 이벤트. 파라미터=프로퍼티명(옵션)
        /// </summary>
        public static event Action<string> OnConfigChanged;

        // ===== 배우자 목록(섹션 구분용, 34명) =====
        public static readonly string[] SpouseNames = {
            // 여자 17명
            "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
            "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene",
            // (남자 등 확장 가능)
        };

        // ★★★ 아기 파츠(헤어/눈/스킨/바디) ★★★
        public static readonly string[] BabyHairStyles = { "Default" };
        public static readonly string[] BabyEyes = { "Default" };
        public static readonly string[] BabySkins = { "Default" };
        public static readonly string[] BabyBodies = { "Default" };

        // 여자 유아
        public static readonly string[] GirlHairStyles = { "CherryTwin", "TwinTail", "PonyTail" };
        public static readonly string[] GirlEyes = { "Default" };
        public static readonly string[] GirlSkins = { "Default" };

        // 남자 유아
        public static readonly string[] BoyHairStyles = { "ShortCut" };
        public static readonly string[] BoyEyes = { "Default" };
        public static readonly string[] BoySkins = { "Default" };

        // 치마/바지 색상
        public static readonly string[] SkirtColorOptions = { "Black", "Blue", "Brown", "Emerald", "Green", "MossGreen", "Pink", "Purple", "Red", "SkyBlue", "Violet", "Yellow", "YellowGreen" };
        public static readonly string[] PantsColorOptions = { "Black", "Blue", "Brown", "Emerald", "Green", "MossGreen", "Pink", "Purple", "Red", "SkyBlue", "Violet", "Yellow", "YellowGreen" };

        public static readonly string[] GirlSkirtColorOptions = SkirtColorOptions;
        public static readonly string[] BoyPantsColorOptions = PantsColorOptions;

        // 신발 색상
        public static readonly string[] ShoesColorOptions = { "Black", "Blue", "Red", "Socks" };
        public static readonly string[] GirlShoesColorOptions = ShoesColorOptions;
        public static readonly string[] BoyShoesColorOptions = ShoesColorOptions;

        // 넥칼라 색상
        public static readonly string[] NeckCollarColorOptions = {
            "Abigail", "Alex", "Black", "Blue", "Brown", "Elliott", "Emerald", "Emily", "Green", "Haley", "Harvey", "Leah", "Maru",
            "MossGreen", "Orange", "Penny", "Pink", "Purple", "Red", "Sam", "Sebastian", "Shane", "SkyBlue", "Violet", "Yellow", "YellowGreen"
        };
        public static readonly string[] GirlNeckCollarColorOptions = NeckCollarColorOptions;
        public static readonly string[] BoyNeckCollarColorOptions = NeckCollarColorOptions;

        // 평상복 상의
        public static readonly string[] GirlTopSpringOptions = { "Short", "Half" };
        public static readonly string[] GirlTopSummerOptions = { "Short", "Half" };
        public static readonly string[] GirlTopFallOptions = { "Long", "Half" };
        public static readonly string[] GirlTopWinterOptions = { "Long", "Half" };

        public static readonly string[] BoyTopSpringOptions = { "Short", "Half" };
        public static readonly string[] BoyTopSummerOptions = { "Short", "Half" };
        public static readonly string[] BoyTopFallOptions = { "Long", "Half" };
        public static readonly string[] BoyTopWinterOptions = { "Long", "Half" };

        // 잠옷 종류와 색상
        public static readonly string[] PajamaTypeOptions = { "WelshCorgi", "Raccoon", "Sheep", "Shark", "Frog", "LesserPanda" };
        public static readonly string[] GirlPajamaTypeOptions = PajamaTypeOptions;
        public static readonly string[] BoyPajamaTypeOptions = PajamaTypeOptions;

        public static readonly Dictionary<string, string[]> PajamaColorOptions = new()
        {
            { "Frog",         new[] { "Black", "Blue", "DarkGreen", "Green", "Pink", "Purple", "White", "Yellow" } },
            { "LesserPanda",  new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
            { "Raccoon",      new[] { "BabyBlue", "BabyPink", "Brown", "Choco", "Gray", "Pink", "White", "Yellow" } },
            { "Sheep",        new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
            { "Shark",        new[] { "Black", "Blue", "Gray", "Pink", "Purple", "White", "Yellow" } },
            { "WelshCorgi",   new[] { "Blue", "Brown", "Choco", "Gray", "Orange", "Pink", "White", "Yellow" } },
        };
        public static readonly Dictionary<string, string[]> GirlPajamaColorOptions = PajamaColorOptions;
        public static readonly Dictionary<string, string[]> BoyPajamaColorOptions = PajamaColorOptions;

        // 축제 파츠(고정/공용)
        public static readonly string[] FestivalSpringHat = { "FestivalSpringHat" };
        public static readonly string[] FestivalSummerHat = { "FestivalSpringHat" };
        public static readonly string[] FestivalWinterHat = { "FestivalWinterHat" };
        public static readonly string[] FestivalWinterScarf = { "FestivalWinterScarf" };
        public static readonly string[] FestivalSummerSkirtOptions = { "FestivalSummerSkirt1", "FestivalSummerSkirt2" };
        public static readonly string[] GirlFestivalSummerSkirtOptions = FestivalSummerSkirtOptions;
        public static readonly string[] FestivalFallSkirts = { "FestivalFallSkirt" };
        public static readonly string[] GirlFestivalFallSkirts = FestivalFallSkirts;
        public static readonly string[] FestivalWinterSkirtOptions = { "FestivalWinterSkirt1", "FestivalWinterSkirt2" };
        public static readonly string[] GirlFestivalWinterSkirtOptions = FestivalWinterSkirtOptions;
        public static readonly string[] FestivalSummerPantsOptions = { "FestivalSummerPants1", "FestivalSummerPants2" };
        public static readonly string[] BoyFestivalSummerPantsOptions = FestivalSummerPantsOptions;
        public static readonly string[] FestivalFallPants = { "FestivalFallPants" };
        public static readonly string[] BoyFestivalFallPants = FestivalFallPants;
        public static readonly string[] FestivalWinterPantsOptions = { "FestivalWinterPants1", "FestivalWinterPants2" };
        public static readonly string[] BoyFestivalWinterPantsOptions = FestivalWinterPantsOptions;

        // ========== 이벤트 연결 예시 ==========
        /// <summary>
        /// 반드시 GameLaunched나 모드 엔트리에서 구독해야 함
        /// </summary>
        public static void InitDropdownConfigSync()
        {
            OnConfigChanged += (prop) =>
            {
                // 동기화가 필요한 모든 매니저에 알림!
                // 1. 전체 캐릭터 외형 실시간 동기화
                DataManager.ApplyAppearancesByGMCMKey(ModEntry.Config);

                // 2. 외형 옵션 동기화
                // 필요시 각 매니저/리소스매니저/GMCM에 콜백 등록

                // 3. (옵션) 실시간 로그
                ModEntry.Log?.Log($"[DropdownConfig] {prop} 값이 변경되어 실시간 동기화 실행", LogLevel.Info);
            };
        }
    }
}