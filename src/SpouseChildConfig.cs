using System;
using System.Collections.Generic;

namespace MyChildCore
{
    /// <summary>
    /// 배우자별 자녀 파츠/의상 설정 DTO – 1:1 파츠명, 모든 png 옵션 리스트, 트리캐치(안전 Getter)
    /// </summary>
    public class SpouseChildConfig
    {
        // 배우자 이름(키) 목록
        public static readonly string[] SpouseKeys = {
            "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
            "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene"
        };
        // 아기 헤어
        public List<string> BabyHairTypeOptions { get; set; } = new() { "BabyHair" };
        public string BabyHairType { get; set; } = "BabyHair";

        public Dictionary<string, List<string>> BabyHairColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabyHair.png" } }
        };
        public string BabyHairColor { get; set; } = "BabyHair.png";
        
        // 눈
        public List<string> BabyEyeTypeOptions { get; set; } = new() { "BabyEye" };
        public string BabyEyeType { get; set; } = "BabyEye";

        public Dictionary<string, List<string>> BabyEyeColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabyEye.png" } }
        };
        public string BabyEyeColor { get; set; } = "BabyEye.png";
        
        // 피부
        public List<string> BabySkinTypeOptions { get; set; } = new() { "BabySkin" };
        public string BabySkinType { get; set; } = "BabySkin";

        public Dictionary<string, List<string>> BabySkinColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabySkin.png" } }
        };
        public string BabySkinColor { get; set; } = "BabySkin.png";
        
        // 의상
        public List<string> BabyBodyTypeOptions { get; set; } = new() { "BabyBody" };
        public string BabyBodyType { get; set; } = "BabyBody";

        public Dictionary<string, List<string>> BabyBodyColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "BabyBody.png" } }
        };
        public string BabyBodyColor { get; set; } = "BabyBody.png";

        // 여아 헤어
        public List<string> GirlHairTypeOptions { get; set; } = new() { "GirlHair" };
        public string GirlHairType { get; set; } = "GirlHair";

        public Dictionary<string, List<string>> GirlHairColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "CherryTwin.png", "TwinTail.png", "PonyTail.png" } }
        };
        public string GirlHairColor { get; set; } = "CherryTwin.png";
        
        // 눈
        public List<string> GirlEyeTypeOptions { get; set; } = new() { "GirlEye" };
        public string GirlEyeType { get; set; } = "GirlEye";

        public Dictionary<string, List<string>> GirlEyeColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Eye.png" } }
        };
        public string GirlEyeColor { get; set; } = "Eye.png";
        
        // 피부
        public List<string> GirlSkinTypeOptions { get; set; } = new() { "GirlSkin" };
        public string GirlSkinType { get; set; } = "GirlSkin";

        public Dictionary<string, List<string>> GirlSkinColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skin.png" } }
        };
        public string GirlSkinColor { get; set; } = "Skin.png";
        
        // 평상복
        public List<string> GirlTopSpringTypeOptions { get; set; } = new() { "GirlSpringTop" };
        public string GirlTopSpringType { get; set; } = "GirlSpringTop";

        public Dictionary<string, List<string>> GirlTopSpringColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string GirlTopSpringColor { get; set; } = "Top_Short.png";
        
        public List<string> GirlTopSummerTypeOptions { get; set; } = new() { "GirlSummerTop" };
        public string GirlTopSummerType { get; set; } = "GirlSummerTop";

        public Dictionary<string, List<string>> GirlTopSummerColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string GirlTopSummerColor { get; set; } = "Top_Short.png";
        
        public List<string> GirlTopFallTypeOptions { get; set; } = new() { "GirlFallTop" };
        public string GirlTopFallType { get; set; } = "GirlFallTop";

        public Dictionary<string, List<string>> GirlTopFallColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string GirlTopFallColor { get; set; } = "Top_Short.png";
        
        public List<string> GirlTopWinterTypeOptions { get; set; } = new() { "GirlWinterTop" };
        public string GirlTopWinterType { get; set; } = "GirlWinterTop";

        public Dictionary<string, List<string>> GirlTopWinterColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string GirlTopWinterColor { get; set; } = "Top_Short.png";
        
        // 치마
        public List<string> SkirtTypeOptions { get; set; } = new() { "Skirt" };
        public string SkirtType { get; set; } = "Skirt";

        public Dictionary<string, List<string>> SkirtColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Brown.png", "Emerald.png", "Green.png", "MossGreen.png", "Orange.png", "Pink.png", "Purple.png", "Red.png", "SkyBlue.png", "Violet.png", "Yellow.png", "YellowGreen.png" } }
        };

        public string SkirtColor { get; set; } = "Red.png";
        
        // 신발
        public List<string> GirlShoesTypeOptions { get; set; } = new() { "Shoes" };
        public string GirlShoesType { get; set; } = "Shoes";
        
        public Dictionary<string, List<string>> GirlShoesColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Red.png", "Socks.png" } }
        };
        public string GirlShoesColor { get; set; } = "Red.png";
        
        // 넥칼라
        public List<string> GirlNeckCollarTypeOptions { get; set; } = new() { "NeckCollar" };
        public string GirlNeckCollarType { get; set; } = "NeckCollar";
        
        public Dictionary<string, List<string>> GirlNeckCollarColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Abigail.png","Alex.png","Black.png","Blue.png","Brown.png","Elliott.png","Emerald.png","Emily.png",
            "Green.png","Haley.png","Harvey.png","Leah.png","Maru.png","MossGreen.png","Orange.png","Penny.png",
            "Pink.png","Purple.png","Red.png","Sam.png","Sebastian.png","Shane.png","SkyBlue.png","Violet.png",
            "Yellow.png","YellowGreen.png" } }
        };
        
        public string GirlNeckCollarColor { get; set; } = "Red.png";
        
        // 잠옷
        public List<string> GirlPajamaTypeOptions { get; set; } = new()
        {
            "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi"
        };
        public string GirlPajamaType { get; set; } = "Frog";

        public Dictionary<string, List<string>> GirlPajamaColorOptions { get; set; } = new()
        {
            { "Frog.png",        new List<string> { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "LesserPanda.png", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Raccoon.png",     new List<string> { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Sheep.png",       new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Shark.png",       new List<string> { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "WelshCorgi.png",  new List<string> { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" } }
        };
        public string GirlPajamaColor { get; set; } = "Pink.png";
        
        // 봄 모자
        public List<string> GirlFestivalSpringHatTypeOptions { get; set; } = new() { "FestivalSpringHat" };
        public string GirlFestivalSpringHatType { get; set; } = "FestivalSpringHat";
        
        public Dictionary<string, List<string>> GirlFestivalSpringHatColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public string GirlFestivalSpringHatColor { get; set; } = "Hat.png";

        // 여름 모자
        public List<string> GirlFestivalSummerHatTypeOptions { get; set; } = new() { "FestivalSummerHat" };
        public string GirlFestivalSummerHatType { get; set; } = "FestivalSummerHat";
        
        public Dictionary<string, List<string>> GirlFestivalSummerHatColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public string GirlFestivalSummerHatColor { get; set; } = "Hat.png";

        // 여름 의상
        public List<string> GirlFestivalSummerSkirtTypeOptions { get; set; } = new() { "FestivalSummerSkirt" };
        public string GirlFestivalSummerSkirtType { get; set; } = "FestivalSummerSkirt";
        
        public Dictionary<string, List<string>> GirlFestivalSummerSkirtColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skirt1.png", "Skirt2.png" } }
        };
        public string GirlFestivalSummerSkirtColor { get; set; } = "Skirt1.png";

        // 가을 의상
        public List<string> GirlFestivalFallSkirtTypeOptions { get; set; } = new() { "FestivalFallSkirt" };
        public string GirlFestivalFallSkirtType { get; set; } = "FestivalFallSkirt";

        public Dictionary<string, List<string>> GirlFestivalFallSkirtColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skirt.png" } }
        };
        public string GirlFestivalFallSkirtColor { get; set; } = "Skirt.png";

        // 겨울 모자
        public List<string> GirlFestivalWinterHatTypeOptions { get; set; } = new() { "FestivalWinterHat" };
        public string GirlFestivalWinterHatType { get; set; } = "FestivalWinterHat";

        public Dictionary<string, List<string>> GirlFestivalWinterHatColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public string GirlFestivalWinterHatColor { get; set; } = "Hat.png";

        // 겨울 의상
        public List<string> GirlFestivalWinterSkirtTypeOptions { get; set; } = new() { "FestivalWinterSkirt" };
        public string GirlFestivalWinterSkirtType { get; set; } = "FestivalWinterSkirt";

        public Dictionary<string, List<string>> GirlFestivalWinterSkirtColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skirt1.png", "Skirt2.png" } }
        };
        public string GirlFestivalWinterSkirtColor { get; set; } = "Skirt1.png";

        // 겨울 목도리
        public List<string> GirlFestivalWinterSkarfTypeOptions { get; set; } = new() { "FestivalWinterSkarf" };
        public string GirlFestivalWinterSkarfType { get; set; } = "FestivalWinterSkarf";

        public Dictionary<string, List<string>> GirlFestivalWinterSkarfColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skarf.png" } }
        };
        public string GirlFestivalWinterSkarfColor { get; set; } = "Skarf.png";

        // 남아 헤어
        public List<string> BoyHairTypeOptions { get; set; } = new() { "BoyHair" };
        public string BoyHairType { get; set; } = "BoyHair";

        public Dictionary<string, List<string>> BoyHairColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "CherryTwin.png", "TwinTail.png", "PonyTail.png" } }
        };
        public string BoyHairColor { get; set; } = "CherryTwin.png";
        
        // 눈
        public List<string> BoyEyeTypeOptions { get; set; } = new() { "BoyEye" };
        public string BoyEyeType { get; set; } = "BoyEye";

        public Dictionary<string, List<string>> BoyEyeColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Eye.png" } }
        };
        public string BoyEyeColor { get; set; } = "Eye.png";
        
        // 피부
        public List<string> BoySkinTypeOptions { get; set; } = new() { "BoySkin" };
        public string BoySkinType { get; set; } = "BoySkin";

        public Dictionary<string, List<string>> BoySkinColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skin.png" } }
        };
        public string BoySkinColor { get; set; } = "Skin.png";
        
        // 평상복
        public List<string> BoyTopSpringTypeOptions { get; set; } = new() { "BoySpringTop" };
        public string BoyTopSpringType { get; set; } = "BoySpringTop";

        public Dictionary<string, List<string>> BoyTopSpringColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string BoyTopSpringColor { get; set; } = "Top_Short.png";
        
        public List<string> BoyTopSummerTypeOptions { get; set; } = new() { "BoySummerTop" };
        public string BoyTopSummerType { get; set; } = "BoySummerTop";

        public Dictionary<string, List<string>> BoyTopSummerColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string BoyTopSummerColor { get; set; } = "Top_Short.png";
        
        public List<string> BoyTopFallTypeOptions { get; set; } = new() { "BoyFallTop" };
        public string BoyTopFallType { get; set; } = "BoyFallTop";

        public Dictionary<string, List<string>> BoyTopFallColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string BoyTopFallColor { get; set; } = "Top_Short.png";
        
        public List<string> BoyTopWinterTypeOptions { get; set; } = new() { "BoyWinterTop" };
        public string BoyTopWinterType { get; set; } = "BoyWinterTop";

        public Dictionary<string, List<string>> BoyTopWinterColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Top_Short.png", "Top_Harf.png", "Top_Long.png" } }
        };

        public string BoyTopWinterColor { get; set; } = "Top_Short.png";
        
        // 바지
        public List<string> PantsTypeOptions { get; set; } = new() { "Pants" };
        public string PantsType { get; set; } = "Pants";

        public Dictionary<string, List<string>> PantsColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Brown.png", "Emerald.png", "Green.png", "MossGreen.png", "Orange.png", "Pink.png", "Purple.png", "Red.png", "SkyBlue.png", "Violet.png", "Yellow.png", "YellowGreen.png"} }
        };

        public string PantsColor { get; set; } = "Blue.png";
        
        // 신발
        public List<string> BoyShoesTypeOptions { get; set; } = new() { "Shoes" };
        public string BoyShoesType { get; set; } = "Shoes";

        public Dictionary<string, List<string>> BoyShoesColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Black.png", "Blue.png", "Red.png", "Socks.png" } }
        };
        
        public string BoyShoesColor { get; set; } = "Blue.png";
        
        // 넥칼라
        public List<string> BoyNeckCollarTypeOptions { get; set; } = new() { "NeckCollar" };
        public string BoyNeckCollarType { get; set; } = "NeckCollar";
        
        public Dictionary<string, List<string>> BoyNeckCollarColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Abigail.png","Alex.png","Black.png","Blue.png","Brown.png","Elliott.png","Emerald.png","Emily.png",
            "Green.png","Haley.png","Harvey.png","Leah.png","Maru.png","MossGreen.png","Orange.png","Penny.png",
            "Pink.png","Purple.png","Red.png","Sam.png","Sebastian.png","Shane.png","SkyBlue.png","Violet.png",
            "Yellow.png","YellowGreen.png" } }
        };
        
        public string BoyNeckCollarColor { get; set; } = "Blue.png";
        
        // 잠옷
        public List<string> BoyPajamaTypeOptions { get; set; } = new() { "Frog", "LesserPanda", "Raccoon", "Sheep", "Shark", "WelshCorgi" };
        public string BoyPajamaType { get; set; } = "Frog";

        public Dictionary<string, List<string>> BoyPajamaColorOptions { get; set; } = new()
        {
            { "Frog", new List<string> { "Black.png", "Blue.png", "DarkGreen.png", "Green.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "LesserPanda", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Raccoon", new List<string> { "BabyBlue.png", "BabyPink.png", "Brown.png", "Choco.png", "Gray.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Sheep", new List<string> { "Black.png", "Blue.png", "Brown.png", "Choco.png", "Pink.png", "White.png", "Yellow.png" } },
            { "Shark", new List<string> { "Black.png", "Blue.png", "Gray.png", "Pink.png", "Purple.png", "White.png", "Yellow.png" } },
            { "WelshCorgi", new List<string> { "Blue.png", "Brown.png", "Choco.png", "Gray.png", "Orange.png", "Pink.png", "White.png", "Yellow.png" } }
        };
        public string BoyPajamaColor { get; set; } = "Blue.png";

        // 봄 모자
        public List<string> BoyFestivalSpringHatTypeOptions { get; set; } = new() { "FestivalSpringHat" };
        public string BoyFestivalSpringHatType { get; set; } = "FestivalSpringHat";
        
        public Dictionary<string, List<string>> BoyFestivalSpringHatColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public string BoyFestivalSpringHatColor { get; set; } = "Hat.png";

        // 여름 모자
        public List<string> BoyFestivalSummerHatTypeOptions { get; set; } = new() { "FestivalSummerHat" };
        public string BoyFestivalSummerHatType { get; set; } = "FestivalSummerHat";
        
        public Dictionary<string, List<string>> BoyFestivalSummerHatColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public string BoyFestivalSummerHatColor { get; set; } = "Hat.png";

        // 여름 의상
        public List<string> BoyFestivalSummerPantsTypeOptions { get; set; } = new() { "FestivalSummerPants" };
        public string BoyFestivalSummerPantsType { get; set; } = "FestivalSummerPants";
        
        public Dictionary<string, List<string>> BoyFestivalSummerPantsColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Pants1.png", "Pants2.png" } }
        };
        public string BoyFestivalSummerPantsColor { get; set; } = "Pants1.png";

        // 가을 의상
        public List<string> BoyFestivalFallPantsTypeOptions { get; set; } = new() { "FestivalFallPants" };
        public string BoyFestivalFallPantsType { get; set; } = "FestivalFallPants";

        public Dictionary<string, List<string>> BoyFestivalFallPantsColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Pants.png" } }
        };
        public string BoyFestivalFallPantsColor { get; set; } = "Pants.png";

        // 겨울 모자
        public List<string> BoyFestivalWinterHatTypeOptions { get; set; } = new() { "FestivalWinterHat" };
        public string BoyFestivalWinterHatType { get; set; } = "FestivalWinterHat";

        public Dictionary<string, List<string>> BoyFestivalWinterHatColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Hat.png" } }
        };
        public string BoyFestivalWinterHatColor { get; set; } = "Hat.png";

        // 겨울 의상
        public List<string> BoyFestivalWinterPantsTypeOptions { get; set; } = new() { "FestivalWinterPants" };
        public string BoyFestivalWinterPantsType { get; set; } = "FestivalWinterPants";

        public Dictionary<string, List<string>> BoyFestivalWinterPantsColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Pants1.png", "Pants2.png" } }
        };
        public string BoyFestivalWinterPantsColor { get; set; } = "Pants1.png";

        // 겨울 목도리
        public List<string> BoyFestivalWinterSkarfTypeOptions { get; set; } = new() { "FestivalWinterSkarf" };
        public string BoyFestivalWinterSkarfType { get; set; } = "FestivalWinterSkarf";

        public Dictionary<string, List<string>> BoyFestivalWinterSkarfColorOptions { get; set; } = new()
        {
            { "Default", new List<string> { "Skarf.png" } }
        };
        public string BoyFestivalWinterSkarfColor { get; set; } = "Skarf.png";

        // === 활성화 플래그 ===
        public bool EnablePajama { get; set; } = true;
        public bool EnableFestival { get; set; } = true;

        // === 트리캐치/안전 Getter ===
        public string SafeGet(string prop)
        {
            try
            {
                var value = GetType().GetProperty(prop)?.GetValue(this);
                if (value is string str) return string.IsNullOrWhiteSpace(str) ? "" : str;
                if (value is IList<string> list && list.Count > 0) return list[0];
                return "";
            }
            catch
            {
                return "";
            }
        }

        public IList<string> SafeGetList(string prop)
        {
            try
            {
                var value = GetType().GetProperty(prop)?.GetValue(this) as IList<string>;
                return value ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}