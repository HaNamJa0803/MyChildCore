using System;
using System.Collections.Generic;
using StardewModdingAPI;

public static class DropdownConfig
{
    // === 글로벌 옵션 ===
    public static bool EnableMod { get; set; } = true;
    public static bool EnablePajama { get; set; } = true;
    public static bool EnableFestival { get; set; } = true;

    // ===== 배우자 목록(섹션 구분용, 34명) =====
    public static readonly string[] SpouseNames = {
        // 여자 17명
        "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
        "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene",
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
    public static readonly string[] BoyHairStyles = { "Short" };
    public static readonly string[] BoyEyes = { "Default" };
    public static readonly string[] BoySkins = { "Default" };

    // 치마/바지 색상
    public static readonly string[] SkirtColorOptions = { "Black", "Blue", "Brown", "Emerald", "Green", "MossGreen", "Pink", "Purple", "Red", "SkyBlue", "Violet", "Yellow", "YellowGreen"};
    public static readonly string[] PantsColorOptions = { "Black", "Blue", "Brown", "Emerald", "Green", "MossGreen", "Pink", "Purple", "Red", "SkyBlue", "Violet", "Yellow", "YellowGreen" };

    // 남녀 각각의 이름으로 매칭
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
        { "Frog",     new[] { "Black", "Blue", "DarkGreen", "Green", "Pink", "Purple", "White", "Yellow" } },
        { "LesserPanda", new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
        { "Raccoon",  new[] { "BabyBlue", "BabyPink", "Brown", "Choco", "Gray", "Pink", "White", "Yellow" } },
        { "Sheep",    new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
        { "Shark",    new[] { "Black", "Blue", "Gray", "Pink", "Purple", "White", "Yellow" } },
        { "WelshCorgi", new[] { "Blue", "Brown", "Choco", "Gray", "Orange", "Pink", "White", "Yellow" } },
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
}