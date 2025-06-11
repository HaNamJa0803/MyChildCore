using MyChildCore;
using System;

namespace MyChildCore
{
    /// <summary>
    /// 파츠 DTO – 파일명·경로 표준화 버전 (.NET 6.0, DTO/Config 직렬화/SMAPI)
    /// </summary>
    public class ChildParts
    {
        // === 즉시감지 이벤트 (옵저버) ===
        public event Action<string, object> OnFieldChanged;
        private void Notify(string prop, object value)
        {
            try { OnFieldChanged?.Invoke(prop, value); }
            catch (Exception ex) { CustomLogger.Warn($"[ChildParts] 즉시감지 예외: {ex.Message}"); }
        }

        // === 공통 메타 ===
        public string SpouseName { get => _spouseName; set { if (_spouseName != value) { _spouseName = value; Notify(nameof(SpouseName), value); } } }
        private string _spouseName;

        public bool IsMale { get => _isMale; set { if (_isMale != value) { _isMale = value; Notify(nameof(IsMale), value); } } }
        private bool _isMale;

        public bool EnablePajama { get => _enablePajama; set { if (_enablePajama != value) { _enablePajama = value; Notify(nameof(EnablePajama), value); } } }
        private bool _enablePajama;

        public bool EnableFestival { get => _enableFestival; set { if (_enableFestival != value) { _enableFestival = value; Notify(nameof(EnableFestival), value); } } }
        private bool _enableFestival;

        // === 아기(0세) ===
        public string BabyHairStyles { get => _babyHairStyles; set { if (_babyHairStyles != value) { _babyHairStyles = value; Notify(nameof(BabyHairStyles), value); } } }
        private string _babyHairStyles = "BabyHair";

        public string BabyEyes { get => _babyEyes; set { if (_babyEyes != value) { _babyEyes = value; Notify(nameof(BabyEyes), value); } } }
        private string _babyEyes = "BabyEye";

        public string BabySkins { get => _babySkins; set { if (_babySkins != value) { _babySkins = value; Notify(nameof(BabySkins), value); } } }
        private string _babySkins = "BabySkin";

        public string BabyBodies { get => _babyBodies; set { if (_babyBodies != value) { _babyBodies = value; Notify(nameof(BabyBodies), value); } } }
        private string _babyBodies = "BabyBody";

        // === 여자 유아(1세) ===
        public string GirlHairStyles { get => _girlHairStyles; set { if (_girlHairStyles != value) { _girlHairStyles = value; Notify(nameof(GirlHairStyles), value); } } }
        private string _girlHairStyles = "CherryTwin"; // 예: "CherryTwin", "TwinTail", "PonyTail"

        public string GirlEyes { get => _girlEyes; set { if (_girlEyes != value) { _girlEyes = value; Notify(nameof(GirlEyes), value); } } }
        private string _girlEyes = "Eye";

        public string GirlSkins { get => _girlSkins; set { if (_girlSkins != value) { _girlSkins = value; Notify(nameof(GirlSkins), value); } } }
        private string _girlSkins = "Skin";

        // === 남자 유아(1세) ===
        public string BoyHairStyles { get => _boyHairStyles; set { if (_boyHairStyles != value) { _boyHairStyles = value; Notify(nameof(BoyHairStyles), value); } } }
        private string _boyHairStyles = "ShortCut";

        public string BoyEyes { get => _boyEyes; set { if (_boyEyes != value) { _boyEyes = value; Notify(nameof(BoyEyes), value); } } }
        private string _boyEyes = "Eye";

        public string BoySkins { get => _boySkins; set { if (_boySkins != value) { _boySkins = value; Notify(nameof(BoySkins), value); } } }
        private string _boySkins = "Skin";

        // === 의상(공용 파츠 – 코드 상/하의/신발 등) ===
        public string GirlTopSpringOptions { get => _girlTopSpringOptions; set { if (_girlTopSpringOptions != value) { _girlTopSpringOptions = value; Notify(nameof(GirlTopSpringOptions), value); } } }
        private string _girlTopSpringOptions = "Top_Short"; // "Top_Short", "Top_Harf", "Top_Long"

        public string GirlTopSummerOptions { get => _girlTopSummerOptions; set { if (_girlTopSummerOptions != value) { _girlTopSummerOptions = value; Notify(nameof(GirlTopSummerOptions), value); } } }
        private string _girlTopSummerOptions = "Top_Short";

        public string GirlTopFallOptions { get => _girlTopFallOptions; set { if (_girlTopFallOptions != value) { _girlTopFallOptions = value; Notify(nameof(GirlTopFallOptions), value); } } }
        private string _girlTopFallOptions = "Top_Long";

        public string GirlTopWinterOptions { get => _girlTopWinterOptions; set { if (_girlTopWinterOptions != value) { _girlTopWinterOptions = value; Notify(nameof(GirlTopWinterOptions), value); } } }
        private string _girlTopWinterOptions = "Top_Long";

        public string BoyTopSpringOptions { get => _boyTopSpringOptions; set { if (_boyTopSpringOptions != value) { _boyTopSpringOptions = value; Notify(nameof(BoyTopSpringOptions), value); } } }
        private string _boyTopSpringOptions = "Top_Short";

        public string BoyTopSummerOptions { get => _boyTopSummerOptions; set { if (_boyTopSummerOptions != value) { _boyTopSummerOptions = value; Notify(nameof(BoyTopSummerOptions), value); } } }
        private string _boyTopSummerOptions = "Top_Short";

        public string BoyTopFallOptions { get => _boyTopFallOptions; set { if (_boyTopFallOptions != value) { _boyTopFallOptions = value; Notify(nameof(BoyTopFallOptions), value); } } }
        private string _boyTopFallOptions = "Top_Long";

        public string BoyTopWinterOptions { get => _boyTopWinterOptions; set { if (_boyTopWinterOptions != value) { _boyTopWinterOptions = value; Notify(nameof(BoyTopWinterOptions), value); } } }
        private string _boyTopWinterOptions = "Top_Long";

        // === 하의, 신발, 넥칼라 ===
        public string SkirtColorOptions { get => _skirtColorOptions; set { if (_skirtColorOptions != value) { _skirtColorOptions = value; Notify(nameof(SkirtColorOptions), value); } } }
        private string _skirtColorOptions = "Black";

        public string PantsColorOptions { get => _pantsColorOptions; set { if (_pantsColorOptions != value) { _pantsColorOptions = value; Notify(nameof(PantsColorOptions), value); } } }
        private string _pantsColorOptions = "Black";

        public string GirlShoesColorOptions { get => _girlShoesColorOptions; set { if (_girlShoesColorOptions != value) { _girlShoesColorOptions = value; Notify(nameof(GirlShoesColorOptions), value); } } }
        private string _girlShoesColorOptions = "Black";

        public string BoyShoesColorOptions { get => _boyShoesColorOptions; set { if (_boyShoesColorOptions != value) { _boyShoesColorOptions = value; Notify(nameof(BoyShoesColorOptions), value); } } }
        private string _boyShoesColorOptions = "Black";

        public string GirlNeckCollarColorOptions { get => _girlNeckCollarColorOptions; set { if (_girlNeckCollarColorOptions != value) { _girlNeckCollarColorOptions = value; Notify(nameof(GirlNeckCollarColorOptions), value); } } }
        private string _girlNeckCollarColorOptions = "Abigail";

        public string BoyNeckCollarColorOptions { get => _boyNeckCollarColorOptions; set { if (_boyNeckCollarColorOptions != value) { _boyNeckCollarColorOptions = value; Notify(nameof(BoyNeckCollarColorOptions), value); } } }
        private string _boyNeckCollarColorOptions = "Abigail";

        // === 잠옷/축제 등은 필요한 만큼 추가
        public string GirlPajamaTypeOptions { get => _girlPajamaTypeOptions; set { if (_girlPajamaTypeOptions != value) { _girlPajamaTypeOptions = value; Notify(nameof(GirlPajamaTypeOptions), value); } } }
        private string _girlPajamaTypeOptions = "Frog"; // "Frog", "LesserPanda", ...

        public string GirlPajamaColorOptions { get => _girlPajamaColorOptions; set { if (_girlPajamaColorOptions != value) { _girlPajamaColorOptions = value; Notify(nameof(GirlPajamaColorOptions), value); } } }
        private string _girlPajamaColorOptions = "Green";

        public string BoyPajamaTypeOptions { get => _boyPajamaTypeOptions; set { if (_boyPajamaTypeOptions != value) { _boyPajamaTypeOptions = value; Notify(nameof(BoyPajamaTypeOptions), value); } } }
        private string _boyPajamaTypeOptions = "Frog";

        public string BoyPajamaColorOptions { get => _boyPajamaColorOptions; set { if (_boyPajamaColorOptions != value) { _boyPajamaColorOptions = value; Notify(nameof(BoyPajamaColorOptions), value); } } }
        private string _boyPajamaColorOptions = "Green";

        // === 축제 ===
        public string FestivalSpringHat { get => _festivalSpringHat; set { if (_festivalSpringHat != value) { _festivalSpringHat = value; Notify(nameof(FestivalSpringHat), value); } } }
        private string _festivalSpringHat = "Hat";

        public string FestivalSummerHat { get => _festivalSummerHat; set { if (_festivalSummerHat != value) { _festivalSummerHat = value; Notify(nameof(FestivalSummerHat), value); } } }
        private string _festivalSummerHat = "Hat";

        public string FestivalWinterHat { get => _festivalWinterHat; set { if (_festivalWinterHat != value) { _festivalWinterHat = value; Notify(nameof(FestivalWinterHat), value); } } }
        private string _festivalWinterHat = "Hat";

        public string FestivalWinterScarf { get => _festivalWinterScarf; set { if (_festivalWinterScarf != value) { _festivalWinterScarf = value; Notify(nameof(FestivalWinterScarf), value); } } }
        private string _festivalWinterScarf = "Skarf";
    }
}