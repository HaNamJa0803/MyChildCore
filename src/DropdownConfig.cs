using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 모든 파츠/드롭다운/옵션 실시간 동기화 + 중앙관리 config
    /// 모든 옵션 png파일명, 남녀분리, 트리캐치(안전 Getter)
    /// </summary>
    public static class DropdownConfig
    {
        // ====== 글로벌 옵션(동기화) ======
        private static bool _enableMod = true;
        public static bool EnableMod
        {
            get => _enableMod;
            set { if (_enableMod != value) { _enableMod = value; OnConfigChanged?.Invoke("EnableMod"); } }
        }
        private static bool _enablePajama = true;
        public static bool EnablePajama
        {
            get => _enablePajama;
            set { if (_enablePajama != value) { _enablePajama = value; OnConfigChanged?.Invoke("EnablePajama"); } }
        }
        private static bool _enableFestival = true;
        public static bool EnableFestival
        {
            get => _enableFestival;
            set { if (_enableFestival != value) { _enableFestival = value; OnConfigChanged?.Invoke("EnableFestival"); } }
        }
        public static event Action<string> OnConfigChanged;

        // ===== 배우자 목록 =====
        public static readonly string[] SpouseNames = {
            "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
            "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene"
        };

        // 아기 헤어
        public static List<string> BabyHairTypeOptions { get; set; } = new() { "BabyHair" };
        public static string BabyHairType { get; set; } = "BabyHair";

        public static Dictionary<string, List<string>> BabyHairColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabyHair.png" } }
        };
        public static string BabyHairColor { get; set; } = "BabyHair.png";
        
        // 눈
        public static List<string> BabyEyeTypeOptions { get; set; } = new() { "BabyEye" };
        public static string BabyEyeType { get; set; } = "BabyEye";

        public static Dictionary<string, List<string>> BabyEyeColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabyEye.png" } }
        };
        public static string BabyEyeColor { get; set; } = "BabyEye.png";
        
        // 피부
        public static List<string> BabySkinTypeOptions { get; set; } = new() { "BabySkin" };
        public static string BabySkinType { get; set; } = "BabySkin";

        public static Dictionary<string, List<string>> BabySkinColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabySkin.png" } }
        };
        public static string BabySkinColor { get; set; } = "BabySkin.png";
        
        // 의상
        public static List<string> BabyBodyTypeOptions { get; set; } = new() { "BabyBody" };
        public static string BabyBodyType { get; set; } = "BabyBody";

        public static Dictionary<string, List<string>> BabyBodyColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabyBody.png" } }
        };
        public static string BabyBodyColor { get; set; } = "BabyBody.png";

        // 여아 헤어
        public static List<string> GirlHairTypeOptions { get; set; } = new() { "GirlHair" };
        public static string GirlHairType { get; set; } = "GirlHair";

        public static Dictionary<string, List<string>> GirlHairColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "CherryTwin.png", "TwinTail.png", "PonyTail.png" } }
        };
        public static string GirlHairColor { get; set; } = "CherryTwin.png";
        
        // 눈
        public static List<string> GirlEyeTypeOptions { get; set; } = new() { "GirlEye" };
        public static string GirlEyeType { get; set; } = "GirlEye";

        public static Dictionary<string, List<string>> GirlEyeColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Eye.png" } }
        };
        public static string GirlEyeColor { get; set; } = "Eye.png";
        
        // 피부
        public static List<string> GirlSkinTypeOptions { get; set; } = new() { "GirlSkin" };
        public static string GirlSkinType { get; set; } = "GirlSkin";

        public static Dictionary<string, List<string>> GirlSkinColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skin.png" } }
        };
        public static string GirlSkinColor { get; set; } = "Skin.png";
        
        // 평상복
        public static List<string> GirlTopSpringTypeOptions { get; set; } = new() { "GirlSpringTop" };
        public static string GirlTopSpringType { get; set; } = "GirlSpringTop";

        public static Dictionary<string, List<string>> GirlTopSpringColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string GirlTopSpringColor { get; set; } = "Top_Short.png";
        
        public static List<string> GirlTopSummerTypeOptions { get; set; } = new() { "GirlSummerTop" };
        public static string GirlTopSummerType { get; set; } = "GirlSummerTop";

        public static Dictionary<string, List<string>> GirlTopSummerColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string GirlTopSummerColor { get; set; } = "Top_Short.png";
        
        public static List<string> GirlTopFallTypeOptions { get; set; } = new() { "GirlFallTop" };
        public static string GirlTopFallType { get; set; } = "GirlFallTop";

        public static Dictionary<string, List<string>> GirlTopFallColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string GirlTopFallColor { get; set; } = "Top_Short.png";
        
        public static List<string> GirlTopWinterTypeOptions { get; set; } = new() { "GirlWinterTop" };
        public static string GirlTopWinterType { get; set; } = "GirlWinterTop";

        public static Dictionary<string, List<string>> GirlTopWinterColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string GirlTopWinterColor { get; set; } = "Top_Short.png";
        
        // 치마
        public static List<string> SkirtTypeOptions { get; set; } = new() { "Skirt" };
        public static string SkirtType { get; set; } = "Skirt";

        public static Dictionary<string, List<string>> SkirtColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Brown.png", "Emerald.png", "Green.png", "MossGreen.png", "Orange.png", "Pink.png", "Purple.png", "Red.png", "SkyBlue.png", "Violet.png", "Yellow.png", "YellowGreen.png" } }
        };

        public static string SkirtColor { get; set; } = "Red.png";
        
        // 신발
        public static List<string> GirlShoesTypeOptions { get; set; } = new() { "Shoes" };
        public static string GirlShoesType { get; set; } = "Shoes";
        
        public static Dictionary<string, List<string>> GirlShoesColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Red.png", "Socks.png" } }
        };
        public static string GirlShoesColor { get; set; } = "Red.png";
        
        // 넥칼라
        public static List<string> GirlNeckCollarTypeOptions { get; set; } = new() { "NeckCollar" };
        public static string GirlNeckCollarType { get; set; } = "NeckCollar";
        
        public static Dictionary<string, List<string>> GirlNeckCollarColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Abigail.png","Alex.png","Black.png","Blue.png","Brown.png","Elliott.png","Emerald.png","Emily.png",
            "Green.png","Haley.png","Harvey.png","Leah.png","Maru.png","MossGreen.png","Orange.png","Penny.png",
            "Pink.png","Purple.png","Red.png","Sam.png","Sebastian.png","Shane.png","SkyBlue.png","Violet.png",
            "Yellow.png","YellowGreen.png" } }
        };
        
        public static string GirlNeckCollarColor { get; set; } = "Red.png";
        
        // 잠옷
        public static List<string> GirlPajamaTypeOptions { get; set; } = new()
        {
            "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi"
        };
        public static string GirlPajamaType { get; set; } = "Frog";

        public static Dictionary<string, List<string>> GirlPajamaColorOptions { get; set; } = new()
        {
            { "Frog.png",        new List<string> { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "LesserPanda.png", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Raccoon.png",     new List<string> { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Sheep.png",       new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Shark.png",       new List<string> { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "WelshCorgi.png",  new List<string> { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" } }
        };
        public static string GirlPajamaColor { get; set; } = "Pink.png";
        
        // 봄 모자
        public static List<string> GirlFestivalSpringHatTypeOptions { get; set; } = new() { "FestivalSpringHat" };
        public static string GirlFestivalSpringHatType { get; set; } = "FestivalSpringHat";
        
        public static Dictionary<string, List<string>> GirlFestivalSpringHatOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public static string GirlFestivalSpringHat { get; set; } = "Hat.png";

        // 여름 모자
        public static List<string> GirlFestivalSummerHatTypeOptions { get; set; } = new() { "FestivalSummerHat" };
        public static string GirlFestivalSummerHatType { get; set; } = "FestivalSummerHat";
        
        public static Dictionary<string, List<string>> GirlFestivalSummerHatOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public static string GirlFestivalSummerHat { get; set; } = "Hat.png";

        // 여름 의상
        public static List<string> GirlFestivalSummerSkirtTypeOptions { get; set; } = new() { "FestivalSummerSkirt" };
        public static string GirlFestivalSummerSkirtType { get; set; } = "FestivalSummerSkirt";
        
        public static Dictionary<string, List<string>> GirlFestivalSummerSkirtOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skirt1.png" } }
        };
        public static string GirlFestivalSummerSkirt { get; set; } = "Skirt1.png";

        // 가을 의상
        public static List<string> GirlFestivalFallSkirtTypeOptions { get; set; } = new() { "FestivalFallSkirt" };
        public static string GirlFestivalFallSkirtType { get; set; } = "FestivalFallSkirt";

        public static Dictionary<string, List<string>> GirlFestivalFallSkirtOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skirt.png" } }
        };
        public static string GirlFestivalFallSkirt { get; set; } = "Skirt.png";

        // 겨울 모자
        public static List<string> GirlFestivalWinterHatTypeOptions { get; set; } = new() { "FestivalWinterHat" };
        public static string GirlFestivalWinterHatType { get; set; } = "FestivalWinterHat";

        public static Dictionary<string, List<string>> GirlFestivalWinterHatOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public static string GirlFestivalWinterHat { get; set; } = "Hat.png";

        // 겨울 의상
        public static List<string> GirlFestivalWinterSkirtTypeOptions { get; set; } = new() { "FestivalWinterSkirt" };
        public static string GirlFestivalWinterSkirtType { get; set; } = "FestivalWinterSkirt";

        public static Dictionary<string, List<string>> GirlFestivalWinterSkirtOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skirt1.png", "Skirt2.png" } }
        };
        public static string GirlFestivalWinterSkirt { get; set; } = "Skirt1.png";

        // 겨울 목도리
        public static List<string> GirlFestivalWinterSkarfTypeOptions { get; set; } = new() { "FestivalWinterSkarf" };
        public static string GirlFestivalWinterSkarfType { get; set; } = "FestivalWinterSkarf";

        public static Dictionary<string, List<string>> GirlFestivalWinterSkarfOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skarf.png" } }
        };
        public static string GirlFestivalWinterSkarf { get; set; } = "Skarf.png";
        
        // 남아 헤어
        public static List<string> BoyHairTypeOptions { get; set; } = new() { "BoyHair" };
        public static string BoyHairType { get; set; } = "BoyHair";

        public static Dictionary<string, List<string>> BoyHairColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "CherryTwin.png", "TwinTail.png", "PonyTail.png" } }
        };
        public static string BoyHairColor { get; set; } = "CherryTwin.png";
        
        // 눈
        public static List<string> BoyEyeTypeOptions { get; set; } = new() { "BoyEye" };
        public static string BoyEyeType { get; set; } = "BoyEye";

        public static Dictionary<string, List<string>> BoyEyeColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Eye.png" } }
        };
        public static string BoyEyeColor { get; set; } = "Eye.png";
        
        // 피부
        public static List<string> BoySkinTypeOptions { get; set; } = new() { "BoySkin" };
        public static string BoySkinType { get; set; } = "BoySkin";

        public static Dictionary<string, List<string>> BoySkinColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skin.png" } }
        };
        public static string BoySkinColor { get; set; } = "Skin.png";
        
        // 평상복
        public static List<string> BoyTopSpringTypeOptions { get; set; } = new() { "BoySpringTop" };
        public static string BoyTopSpringType { get; set; } = "BoySpringTop";

        public static Dictionary<string, List<string>> BoyTopSpringColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string BoyTopSpringColor { get; set; } = "Top_Short.png";
        
        public static List<string> BoyTopSummerTypeOptions { get; set; } = new() { "BoySummerTop" };
        public static string BoyTopSummerType { get; set; } = "BoySummerTop";

        public static Dictionary<string, List<string>> BoyTopSummerColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string BoyTopSummerColor { get; set; } = "Top_Short.png";
        
        public static List<string> BoyTopFallTypeOptions { get; set; } = new() { "BoyFallTop" };
        public static string BoyTopFallType { get; set; } = "BoyFallTop";

        public static Dictionary<string, List<string>> BoyTopFallColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string BoyTopFallColor { get; set; } = "Top_Short.png";
        
        public static List<string> BoyTopWinterTypeOptions { get; set; } = new() { "BoyWinterTop" };
        public static string BoyTopWinterType { get; set; } = "BoyWinterTop";

        public static Dictionary<string, List<string>> BoyTopWinterColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public static string BoyTopWinterColor { get; set; } = "Top_Short.png";
        
        // 바지
        public static List<string> PantsTypeOptions { get; set; } = new() { "Pants" };
        public static string PantsType { get; set; } = "Pants";

        public static Dictionary<string, List<string>> PantsColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Brown.png", "Emerald.png", "Green.png", "MossGreen.png", "Orange.png", "Pink.png", "Purple.png", "Red.png", "SkyBlue.png", "Violet.png", "Yellow.png", "YellowGreen.png"} }
        };

        public static string PantsColor { get; set; } = "Blue.png";
        
        // 신발
        public static List<string> BoyShoesTypeOptions { get; set; } = new() { "Shoes" };
        public static string BoyShoesType { get; set; } = "Shoes";

        public static Dictionary<string, List<string>> BoyShoesColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Red.png", "Socks.png" } }
        };
        
        public static string BoyShoesColor { get; set; } = "Blue.png";
        
        // 넥칼라
        public static List<string> BoyNeckCollarTypeOptions { get; set; } = new() { "NeckCollar" };
        public static string BoyNeckCollarType { get; set; } = "NeckCollar";
        
        public static Dictionary<string, List<string>> BoyNeckCollarColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Abigail.png","Alex.png","Black.png","Blue.png","Brown.png","Elliott.png","Emerald.png","Emily.png",
            "Green.png","Haley.png","Harvey.png","Leah.png","Maru.png","MossGreen.png","Orange.png","Penny.png",
            "Pink.png","Purple.png","Red.png","Sam.png","Sebastian.png","Shane.png","SkyBlue.png","Violet.png",
            "Yellow.png","YellowGreen.png" } }
        };
        
        public static string BoyNeckCollarColor { get; set; } = "Blue.png";
        
        // 잠옷
        public static List<string> BoyPajamaTypeOptions { get; set; } = new() { "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi" };
        public static string BoyPajamaType { get; set; } = "Frog";

        public static Dictionary<string, List<string>> BoyPajamaColorOptions { get; set; } = new()
        {
            { "Frog", new List<string> { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "LesserPanda", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Raccoon", new List<string> { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Sheep", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Shark", new List<string> { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "WelshCorgi", new List<string> { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" } }
        };
        public static string BoyPajamaColor { get; set; } = "Blue.png";

        // 봄 모자
        public static List<string> BoyFestivalSpringHatTypeOptions { get; set; } = new() { "FestivalSpringHat" };
        public static string BoyFestivalSpringHatType { get; set; } = "FestivalSpringHat";
        
        public static Dictionary<string, List<string>> BoyFestivalSpringHatOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public static string BoyFestivalSpringHat { get; set; } = "Hat.png";

        // 여름 모자
        public static List<string> BoyFestivalSummerHatTypeOptions { get; set; } = new() { "FestivalSummerHat" };
        public static string BoyFestivalSummerHatType { get; set; } = "FestivalSummerHat";
        
        public static Dictionary<string, List<string>> BoyFestivalSummerHatOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public static string BoyFestivalSummerHat { get; set; } = "Hat.png";

        // 여름 의상
        public static List<string> BoyFestivalSummerPantsTypeOptions { get; set; } = new() { "FestivalSummerPants" };
        public static string BoyFestivalSummerPantsType { get; set; } = "FestivalSummerPants";
        
        public static Dictionary<string, List<string>> BoyFestivalSummerPantsOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Pants1.png" } }
        };
        public static string BoyFestivalSummerPants { get; set; } = "Pants1.png";

        // 가을 의상
        public static List<string> BoyFestivalFallPantsTypeOptions { get; set; } = new() { "FestivalFallPants" };
        public static string BoyFestivalFallPantsType { get; set; } = "FestivalFallPants";

        public static Dictionary<string, List<string>> BoyFestivalFallPantsOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Pants.png" } }
        };
        public static string BoyFestivalFallPants { get; set; } = "Pants.png";

        // 겨울 모자
        public static List<string> BoyFestivalWinterHatTypeOptions { get; set; } = new() { "FestivalWinterHat" };
        public static string BoyFestivalWinterHatType { get; set; } = "FestivalWinterHat";

        public static Dictionary<string, List<string>> BoyFestivalWinterHatOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public static string BoyFestivalWinterHat { get; set; } = "Hat.png";

        // 겨울 의상
        public static List<string> BoyFestivalWinterPantsTypeOptions { get; set; } = new() { "FestivalWinterPants" };
        public static string BoyFestivalWinterPantsType { get; set; } = "FestivalWinterPants";

        public static Dictionary<string, List<string>> BoyFestivalWinterPantsOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Pants1.png" } }
        };
        public static string BoyFestivalWinterPants { get; set; } = "Pants1.png";

        // 겨울 목도리
        public static List<string> BoyFestivalWinterSkarfTypeOptions { get; set; } = new() { "FestivalWinterSkarf" };
        public static string BoyFestivalWinterSkarfType { get; set; } = "FestivalWinterSkarf";

        public static Dictionary<string, List<string>> BoyFestivalWinterSkarfOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skarf.png" } }
        };
        public static string BoyFestivalWinterSkarf { get; set; } = "Skarf.png";

        // ========== 트리캐치/안전 Getter ==========
        public static string SafeGet(string[] arr, int idx = 0)
        {
            try
            {
                if (arr == null || arr.Length <= idx) return "";
                return arr[idx] ?? "";
            }
            catch
            {
                return "";
            }
        }

        public static string SafeGetList(Dictionary<string, string[]> dict, string key, int idx = 0)
        {
            try
            {
                if (dict == null || !dict.ContainsKey(key)) return "";
                var arr = dict[key];
                if (arr == null || arr.Length <= idx) return "";
                return arr[idx] ?? "";
            }
            catch
            {
                return "";
            }
        }

        // ========== 실시간 옵션 이벤트 연결 예시 ==========
        public static void InitDropdownConfigSync()
        {
            OnConfigChanged += (prop) =>
            {
                DataManager.ApplyAppearancesByGMCMKey(ModEntry.Config);
                ModEntry.Log?.Log($"[DropdownConfig] {prop} 값이 변경되어 실시간 동기화 실행", LogLevel.Info);
            };
        }
    }
}