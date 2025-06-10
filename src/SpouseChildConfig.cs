using MyChildCore;
using System;

public class SpouseChildConfig
{
    // ===== 하드코딩 배우자 17명 =====
    public static readonly string[] SpouseNames = {
        "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
        "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene"
    };
    // === Observer: 파츠 값이 바뀔 때마다 알림 ===
    public event Action<string, object> OnPartChanged;

    // === 아기 (공용) ===
    private string _babyHairStyles = DropdownConfig.BabyHairStyles[0];
    public string BabyHairStyles
    {
        get => _babyHairStyles;
        set { if (_babyHairStyles != value) { _babyHairStyles = value; NotifyPartChanged(nameof(BabyHairStyles), value); } }
    }

    private string _babyEyes = DropdownConfig.BabyEyes[0];
    public string BabyEyes
    {
        get => _babyEyes;
        set { if (_babyEyes != value) { _babyEyes = value; NotifyPartChanged(nameof(BabyEyes), value); } }
    }

    private string _babySkins = DropdownConfig.BabySkins[0];
    public string BabySkins
    {
        get => _babySkins;
        set { if (_babySkins != value) { _babySkins = value; NotifyPartChanged(nameof(BabySkins), value); } }
    }

    private string _babyBodies = DropdownConfig.BabyBodies[0];
    public string BabyBodies
    {
        get => _babyBodies;
        set { if (_babyBodies != value) { _babyBodies = value; NotifyPartChanged(nameof(BabyBodies), value); } }
    }

    // === 여자 자녀 ===
    private string _girlHairStyles = DropdownConfig.GirlHairStyles[0];
    public string GirlHairStyles
    {
        get => _girlHairStyles;
        set { if (_girlHairStyles != value) { _girlHairStyles = value; NotifyPartChanged(nameof(GirlHairStyles), value); } }
    }

    private string _girlEyes = DropdownConfig.GirlEyes[0];
    public string GirlEyes
    {
        get => _girlEyes;
        set { if (_girlEyes != value) { _girlEyes = value; NotifyPartChanged(nameof(GirlEyes), value); } }
    }

    private string _girlSkins = DropdownConfig.GirlSkins[0];
    public string GirlSkins
    {
        get => _girlSkins;
        set { if (_girlSkins != value) { _girlSkins = value; NotifyPartChanged(nameof(GirlSkins), value); } }
    }

    private string _girlTopSpringOptions = DropdownConfig.GirlTopSpringOptions[0];
    public string GirlTopSpringOptions
    {
        get => _girlTopSpringOptions;
        set { if (_girlTopSpringOptions != value) { _girlTopSpringOptions = value; NotifyPartChanged(nameof(GirlTopSpringOptions), value); } }
    }

    private string _girlTopSummerOptions = DropdownConfig.GirlTopSummerOptions[0];
    public string GirlTopSummerOptions
    {
        get => _girlTopSummerOptions;
        set { if (_girlTopSummerOptions != value) { _girlTopSummerOptions = value; NotifyPartChanged(nameof(GirlTopSummerOptions), value); } }
    }

    private string _girlTopFallOptions = DropdownConfig.GirlTopFallOptions[0];
    public string GirlTopFallOptions
    {
        get => _girlTopFallOptions;
        set { if (_girlTopFallOptions != value) { _girlTopFallOptions = value; NotifyPartChanged(nameof(GirlTopFallOptions), value); } }
    }

    private string _girlTopWinterOptions = DropdownConfig.GirlTopWinterOptions[0];
    public string GirlTopWinterOptions
    {
        get => _girlTopWinterOptions;
        set { if (_girlTopWinterOptions != value) { _girlTopWinterOptions = value; NotifyPartChanged(nameof(GirlTopWinterOptions), value); } }
    }

    private string _skirtColorOptions = DropdownConfig.SkirtColorOptions[0];
    public string SkirtColorOptions
    {
        get => _skirtColorOptions;
        set { if (_skirtColorOptions != value) { _skirtColorOptions = value; NotifyPartChanged(nameof(SkirtColorOptions), value); } }
    }

    private string _girlShoesColorOptions = DropdownConfig.ShoesColorOptions[0];
    public string GirlShoesColorOptions
    {
        get => _girlShoesColorOptions;
        set { if (_girlShoesColorOptions != value) { _girlShoesColorOptions = value; NotifyPartChanged(nameof(GirlShoesColorOptions), value); } }
    }

    private string _girlNeckCollarColorOptions = DropdownConfig.NeckCollarColorOptions[0];
    public string GirlNeckCollarColorOptions
    {
        get => _girlNeckCollarColorOptions;
        set { if (_girlNeckCollarColorOptions != value) { _girlNeckCollarColorOptions = value; NotifyPartChanged(nameof(GirlNeckCollarColorOptions), value); } }
    }

    private string _girlPajamaTypeOptions = DropdownConfig.PajamaTypeOptions[0];
    public string GirlPajamaTypeOptions
    {
        get => _girlPajamaTypeOptions;
        set { if (_girlPajamaTypeOptions != value) { _girlPajamaTypeOptions = value; NotifyPartChanged(nameof(GirlPajamaTypeOptions), value); } }
    }

    private string _girlPajamaColorOptions = DropdownConfig.PajamaColorOptions[DropdownConfig.PajamaTypeOptions[0]][0];
    public string GirlPajamaColorOptions
    {
        get => _girlPajamaColorOptions;
        set { if (_girlPajamaColorOptions != value) { _girlPajamaColorOptions = value; NotifyPartChanged(nameof(GirlPajamaColorOptions), value); } }
    }

    private string _girlFestivalSummerSkirtOptions = DropdownConfig.FestivalSummerSkirtOptions[0];
    public string GirlFestivalSummerSkirtOptions
    {
        get => _girlFestivalSummerSkirtOptions;
        set { if (_girlFestivalSummerSkirtOptions != value) { _girlFestivalSummerSkirtOptions = value; NotifyPartChanged(nameof(GirlFestivalSummerSkirtOptions), value); } }
    }

    private string _girlFestivalWinterSkirtOptions = DropdownConfig.FestivalWinterSkirtOptions[0];
    public string GirlFestivalWinterSkirtOptions
    {
        get => _girlFestivalWinterSkirtOptions;
        set { if (_girlFestivalWinterSkirtOptions != value) { _girlFestivalWinterSkirtOptions = value; NotifyPartChanged(nameof(GirlFestivalWinterSkirtOptions), value); } }
    }

    private string _girlFestivalFallSkirts = DropdownConfig.FestivalFallSkirts[0];
    public string GirlFestivalFallSkirts
    {
        get => _girlFestivalFallSkirts;
        set { if (_girlFestivalFallSkirts != value) { _girlFestivalFallSkirts = value; NotifyPartChanged(nameof(GirlFestivalFallSkirts), value); } }
    }

    // === 남자 자녀 ===
    private string _boyHairStyles = DropdownConfig.BoyHairStyles[0];
    public string BoyHairStyles
    {
        get => _boyHairStyles;
        set { if (_boyHairStyles != value) { _boyHairStyles = value; NotifyPartChanged(nameof(BoyHairStyles), value); } }
    }

    private string _boyEyes = DropdownConfig.BoyEyes[0];
    public string BoyEyes
    {
        get => _boyEyes;
        set { if (_boyEyes != value) { _boyEyes = value; NotifyPartChanged(nameof(BoyEyes), value); } }
    }

    private string _boySkins = DropdownConfig.BoySkins[0];
    public string BoySkins
    {
        get => _boySkins;
        set { if (_boySkins != value) { _boySkins = value; NotifyPartChanged(nameof(BoySkins), value); } }
    }

    private string _boyTopSpringOptions = DropdownConfig.BoyTopSpringOptions[0];
    public string BoyTopSpringOptions
    {
        get => _boyTopSpringOptions;
        set { if (_boyTopSpringOptions != value) { _boyTopSpringOptions = value; NotifyPartChanged(nameof(BoyTopSpringOptions), value); } }
    }

    private string _boyTopSummerOptions = DropdownConfig.BoyTopSummerOptions[0];
    public string BoyTopSummerOptions
    {
        get => _boyTopSummerOptions;
        set { if (_boyTopSummerOptions != value) { _boyTopSummerOptions = value; NotifyPartChanged(nameof(BoyTopSummerOptions), value); } }
    }

    private string _boyTopFallOptions = DropdownConfig.BoyTopFallOptions[0];
    public string BoyTopFallOptions
    {
        get => _boyTopFallOptions;
        set { if (_boyTopFallOptions != value) { _boyTopFallOptions = value; NotifyPartChanged(nameof(BoyTopFallOptions), value); } }
    }

    private string _boyTopWinterOptions = DropdownConfig.BoyTopWinterOptions[0];
    public string BoyTopWinterOptions
    {
        get => _boyTopWinterOptions;
        set { if (_boyTopWinterOptions != value) { _boyTopWinterOptions = value; NotifyPartChanged(nameof(BoyTopWinterOptions), value); } }
    }

    private string _pantsColorOptions = DropdownConfig.PantsColorOptions[0];
    public string PantsColorOptions
    {
        get => _pantsColorOptions;
        set { if (_pantsColorOptions != value) { _pantsColorOptions = value; NotifyPartChanged(nameof(PantsColorOptions), value); } }
    }

    private string _boyShoesColorOptions = DropdownConfig.ShoesColorOptions[0];
    public string BoyShoesColorOptions
    {
        get => _boyShoesColorOptions;
        set { if (_boyShoesColorOptions != value) { _boyShoesColorOptions = value; NotifyPartChanged(nameof(BoyShoesColorOptions), value); } }
    }

    private string _boyNeckCollarColorOptions = DropdownConfig.NeckCollarColorOptions[0];
    public string BoyNeckCollarColorOptions
    {
        get => _boyNeckCollarColorOptions;
        set { if (_boyNeckCollarColorOptions != value) { _boyNeckCollarColorOptions = value; NotifyPartChanged(nameof(BoyNeckCollarColorOptions), value); } }
    }

    private string _boyPajamaTypeOptions = DropdownConfig.PajamaTypeOptions[0];
    public string BoyPajamaTypeOptions
    {
        get => _boyPajamaTypeOptions;
        set { if (_boyPajamaTypeOptions != value) { _boyPajamaTypeOptions = value; NotifyPartChanged(nameof(BoyPajamaTypeOptions), value); } }
    }

    private string _boyPajamaColorOptions = DropdownConfig.PajamaColorOptions[DropdownConfig.PajamaTypeOptions[0]][0];
    public string BoyPajamaColorOptions
    {
        get => _boyPajamaColorOptions;
        set { if (_boyPajamaColorOptions != value) { _boyPajamaColorOptions = value; NotifyPartChanged(nameof(BoyPajamaColorOptions), value); } }
    }

    private string _boyFestivalSummerPantsOptions = DropdownConfig.FestivalSummerPantsOptions[0];
    public string BoyFestivalSummerPantsOptions
    {
        get => _boyFestivalSummerPantsOptions;
        set { if (_boyFestivalSummerPantsOptions != value) { _boyFestivalSummerPantsOptions = value; NotifyPartChanged(nameof(BoyFestivalSummerPantsOptions), value); } }
    }

    private string _boyFestivalWinterPantsOptions = DropdownConfig.FestivalWinterPantsOptions[0];
    public string BoyFestivalWinterPantsOptions
    {
        get => _boyFestivalWinterPantsOptions;
        set { if (_boyFestivalWinterPantsOptions != value) { _boyFestivalWinterPantsOptions = value; NotifyPartChanged(nameof(BoyFestivalWinterPantsOptions), value); } }
    }

    private string _boyFestivalFallPants = DropdownConfig.FestivalFallPants[0];
    public string BoyFestivalFallPants
    {
        get => _boyFestivalFallPants;
        set { if (_boyFestivalFallPants != value) { _boyFestivalFallPants = value; NotifyPartChanged(nameof(BoyFestivalFallPants), value); } }
    }

    // === 축제(공용, 선택 불가능/읽기 전용) ===
    public string FestivalSpringHat { get; set; } = DropdownConfig.FestivalSpringHat[0];
    public string FestivalSummerHat { get; set; } = DropdownConfig.FestivalSummerHat[0];
    public string FestivalWinterHat { get; set; } = DropdownConfig.FestivalWinterHat[0];
    public string FestivalWinterScarf { get; set; } = DropdownConfig.FestivalWinterScarf[0];

    // === 추가된 파자마/축제복 활성화 여부 ===
    private bool _enablePajama = true;
    public bool EnablePajama
    {
        get => _enablePajama;
        set { if (_enablePajama != value) { _enablePajama = value; NotifyPartChanged(nameof(EnablePajama), value); } }
    }

    private bool _enableFestival = true;
    public bool EnableFestival
    {
        get => _enableFestival;
        set { if (_enableFestival != value) { _enableFestival = value; NotifyPartChanged(nameof(EnableFestival), value); } }
    }

    // === 이벤트 발생 ===
    private void NotifyPartChanged(string partName, object newValue)
    {
        try { OnPartChanged?.Invoke(partName, newValue); }
        catch (Exception ex)
        {
            // 실전: CustomLogger.Warn($"[SpouseChildConfig] {partName} 변경 이벤트 예외: {ex.Message}");
        }
    }
}