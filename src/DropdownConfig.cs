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

        // ===== 배우자 목록(섹션 구분용, 17명) =====
        public static readonly string[] SpouseNames = {
            "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
            "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene"
        };

        // ========== 아기 파츠(헤어/눈/스킨/바디) ==========
        public static readonly string[] BabyHairStyles = { "BabyHair" };
        public static readonly string[] BabyEyes = { "BabyEye" };
        public static readonly string[] BabySkins = { "BabySkin" };
        public static readonly string[] BabyBodies = { "BabyBody" };

        // ========== 여자 유아 ==========
        public static readonly string[] GirlHairStyles = { "CherryTwin", "TwinTail", "PonyTail" };
        public static readonly string[] GirlEyes = { "Eye" };
        public static readonly string[] GirlSkins = { "Skin" };

        // ========== 남자 유아 ==========
        public static readonly string[] BoyHairStyles = { "ShortCut" };
        public static readonly string[] BoyEyes = { "Eye" };
        public static readonly string[] BoySkins = { "Skin" };

        // ========== 치마/바지 색상 ==========
        public static readonly string[] SkirtColorOptions = {
            "Black", "Blue", "Brown", "Emerald", "Green", "MossGreen", "Pink",
            "Purple", "Red", "SkyBlue", "Violet", "Yellow", "YellowGreen"
        };
        public static readonly string[] PantsColorOptions = SkirtColorOptions; // 동일

        // ========== 신발 색상 ==========
        public static readonly string[] ShoesColorOptions = { "Black", "Blue", "Red", "Socks" };

        // ========== 넥칼라 색상 ==========
        public static readonly string[] NeckCollarColorOptions = {
            "Abigail", "Alex", "Black", "Blue", "Brown", "Elliott", "Emerald", "Emily", "Green", "Haley", "Harvey", "Leah", "Maru",
            "MossGreen", "Orange", "Penny", "Pink", "Purple", "Red", "Sam", "Sebastian", "Shane", "SkyBlue", "Violet", "Yellow", "YellowGreen"
        };

        // ========== 평상복 상의(계절별) ==========
        public static readonly string[] GirlTopSpringOptions = { "Short", "Half" };
        public static readonly string[] GirlTopSummerOptions = { "Short", "Half" };
        public static readonly string[] GirlTopFallOptions   = { "Long", "Half" };
        public static readonly string[] GirlTopWinterOptions = { "Long", "Half" };

        public static readonly string[] BoyTopSpringOptions = GirlTopSpringOptions;
        public static readonly string[] BoyTopSummerOptions = GirlTopSummerOptions;
        public static readonly string[] BoyTopFallOptions   = GirlTopFallOptions;
        public static readonly string[] BoyTopWinterOptions = GirlTopWinterOptions;

        // ========== 잠옷 종류와 색상 ==========
        public static readonly string[] PajamaTypeOptions = {
            "WelshCorgi", "Raccoon", "Sheep", "Shark", "Frog", "LesserPanda"
        };

        public static readonly Dictionary<string, string[]> PajamaColorOptions = new()
        {
            { "Frog",         new[] { "Black", "Blue", "DarkGreen", "Green", "Pink", "Purple", "White", "Yellow" } },
            { "LesserPanda",  new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
            { "Raccoon",      new[] { "BabyBlue", "BabyPink", "Brown", "Choco", "Gray", "Pink", "White", "Yellow" } },
            { "Sheep",        new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
            { "Shark",        new[] { "Black", "Blue", "Gray", "Pink", "Purple", "White", "Yellow" } },
            { "WelshCorgi",   new[] { "Blue", "Brown", "Choco", "Gray", "Orange", "Pink", "White", "Yellow" } },
        };

        // ========== 축제 파츠 ==========
        public static readonly string[] FestivalSpringHat = { "FestivalSpringHat" };
        public static readonly string[] FestivalSummerHat = { "FestivalSummerHat" };
        public static readonly string[] FestivalWinterHat = { "FestivalWinterHat" };
        public static readonly string[] FestivalWinterScarf = { "FestivalWinterScarf" };

        public static readonly string[] FestivalSummerSkirtOptions = { "FestivalSummerSkirt1", "FestivalSummerSkirt2" };
        public static readonly string[] FestivalFallSkirts = { "FestivalFallSkirt" };
        public static readonly string[] FestivalWinterSkirtOptions = { "FestivalWinterSkirt1", "FestivalWinterSkirt2" };

        public static readonly string[] FestivalSummerPantsOptions = { "FestivalSummerPants1", "FestivalSummerPants2" };
        public static readonly string[] FestivalFallPants = { "FestivalFallPants" };
        public static readonly string[] FestivalWinterPantsOptions = { "FestivalWinterPants1", "FestivalWinterPants2" };

        // ========== 실시간 옵션 이벤트 연결 예시 ==========
        public static void InitDropdownConfigSync()
        {
            OnConfigChanged += (prop) =>
            {
                // 전체 캐릭터 외형 실시간 동기화
                DataManager.ApplyAppearancesByGMCMKey(ModEntry.Config);

                // (옵션) 실시간 로그
                ModEntry.Log?.Log($"[DropdownConfig] {prop} 값이 변경되어 실시간 동기화 실행", LogLevel.Info);
            };
        }
    }
}