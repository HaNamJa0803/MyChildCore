using System;
using System.Collections.Generic;
using StardewModdingAPI;

public static class DropdownConfig
{
    // === 글로벌 옵션 ===
    public static bool EnableMod { get; set; } = true;        // 전체 모드 온/오프
    public static bool EnablePajama { get; set; } = true;     // 잠옷 적용 온/오프
    public static bool EnableFestival { get; set; } = true;   // 축제복 적용 온/오프
     
    // ★★★ 아기 파츠(헤어/눈/스킨/바디) ★★★
    public static readonly string[] BabyHairStyles = { "Default" }; // 커스터마이징 범위 필요 시 추가
    public static readonly string[] BabyEyes = { "Default" };
    public static readonly string[] BabySkins = { "Default" };
    public static readonly string[] BabyBodies = { "Default" }; // ex) "Default", "BearSuit", 등등 확장 가능
    
    // 여자 유아
    public static readonly string[] GirlHairStyles = { "CherryTwin", "TwinTail", "PonyTail" };
    public static readonly string[] GirlEyes = { "Black", "Blue", "Brown", "Green", "Pink", "Violet" }; // 예시
    public static readonly string[] GirlSkins = { "Light", "Brown", "Dark" }; // 예시

    // 남자 유아
    public static readonly string[] BoyHairStyles = { "Short" };
    public static readonly string[] BoyEyes = { "Black", "Blue", "Brown", "Green", "Pink", "Violet" }; // 예시
    public static readonly string[] BoySkins = { "Light", "Brown", "Dark" }; // 예시

    // 치마/바지 색상
    public static readonly string[] SkirtColors = { "Black", "Blue", "Brown", "Emerald", "Green", "Pink", "SkyBlue", "Violet", "Yellow" }; // 10개
    public static readonly string[] PantsColors = { "Black", "Blue", "Brown", "Green", "Orange", "Pink", "SkyBlue", "Violet", "Yellow" }; // 10개

    // 신발 색상
    public static readonly string[] ShoesColors = { "Black", "Blue", "Red", "Socks" }; // 4개

    // 넥칼라 색상
    public static readonly string[] NeckCollarColors =
        { "Abigail", "Alex", "Black", "Blue", "Brown", "Elliott", "Emerald", "Emily", "Green", "Haley", "Harvey", "Leah", "Maru", "MossGreen", "Orange", "Penny", "Pink", "Purple", "Red", "Sam", "Sebastian", "Shane", "SkyBlue", "Violet", "Yellow", "YellowGreen" }; // 26개

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
    public static readonly string[] PajamaTypes = { "WelshCorgi", "Raccoon", "Sheep", "Shark", "Frog", "LesserPanda" };

    public static readonly Dictionary<string, string[]> PajamaColors = new()
    {
        { "Frog",     new[] { "Black", "Blue", "DarkGreen", "Green", "Pink", "Purple", "White", "Yellow" } },
        { "LesserPanda", new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
        { "Raccoon",  new[] { "BabyBlue", "BabyPink", "Brown", "Choco", "Gray", "Pink", "White", "Yellow" } },
        { "Sheep",    new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
        { "Shark",    new[] { "Black", "Blue", "Gray", "Pink", "Purple", "White", "Yellow" } },
        { "WelshCorgi",    new[] { "Blue", "Brown", "Choco", "Gray", "Orange", "Pink", "White", "Yellow" } },
    };

    // 축제 파츠(고정/공용)
    public static readonly string[] FestivalSpringHat = { "FestivalSpringHat" };
    public static readonly string[] FestivalSummerHat = { "FestivalSpringHat" };
    public static readonly string[] FestivalWinterHat = { "FestivalWinterHat" };
    public static readonly string[] FestivalWinterScarf = { "FestivalWinterScarf" };
    public static readonly string[] GirlFestivalSummerSkirtOptions = { "FestivalSummerSkirt1", "FestivalSummerSkirt2" };
    public static readonly string[] GirlFestivalFallSkirts = { "FestivalFallSkirt" };
    public static readonly string[] GirlFestivalWinterSkirtOptions = { "FestivalWinterSkirt1", "FestivalWinterSkirt2" };
    public static readonly string[] BoyFestivalSummerPantsOptions = { "FestivalSummerPants1", "FestivalSummerPants2" };
    public static readonly string[] BoyFestivalFallPants = { "FestivalFallPants" };
    public static readonly string[] BoyFestivalWinterPantsOptions = { "FestivalWinterPants1", "FestivalWinterPants2" };
}