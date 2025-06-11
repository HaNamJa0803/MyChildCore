using System;

namespace MyChildCore
{
    /// <summary>
    /// 배우자별 자녀/아기 파츠 1:1 설정 DTO (옵저버 알림)
    /// </summary>
    public class SpouseChildConfig
    {
        // ===== 배우자명(예시 17명) =====
        public static readonly string[] SpouseNames = {
            "Abigail", "Alissa", "Blair", "Emily", "Haley", "Kiarra", "Penny", "Leah", "Maru",
            "Ysabelle", "Corine", "Faye", "Maddie", "Daia", "Paula", "Flor", "Irene"
        };

        // ==== 옵저버 알림 ====
        public event Action<string, object> OnPartChanged;
        private void NotifyPartChanged(string prop, object value)
        {
            try { OnPartChanged?.Invoke(prop, value); }
            catch (Exception) { }
        }

        // ====== 아기 ======
        private string _babyHair = "BabyHair";
        public string BabyHair { get => _babyHair; set { if (_babyHair != value) { _babyHair = value; NotifyPartChanged(nameof(BabyHair), value); } } }

        private string _babyEye = "BabyEye";
        public string BabyEye { get => _babyEye; set { if (_babyEye != value) { _babyEye = value; NotifyPartChanged(nameof(BabyEye), value); } } }

        private string _babySkin = "BabySkin";
        public string BabySkin { get => _babySkin; set { if (_babySkin != value) { _babySkin = value; NotifyPartChanged(nameof(BabySkin), value); } } }

        private string _babyBody = "BabyBody";
        public string BabyBody { get => _babyBody; set { if (_babyBody != value) { _babyBody = value; NotifyPartChanged(nameof(BabyBody), value); } } }

        // ====== 유아(공통/성별 구분) ======
        private string _toddlerHair = "ShortCut"; // Boy: ShortCut, Girl: CherryTwin 등
        public string ToddlerHair { get => _toddlerHair; set { if (_toddlerHair != value) { _toddlerHair = value; NotifyPartChanged(nameof(ToddlerHair), value); } } }

        private string _toddlerEye = "Eye";
        public string ToddlerEye { get => _toddlerEye; set { if (_toddlerEye != value) { _toddlerEye = value; NotifyPartChanged(nameof(ToddlerEye), value); } } }

        private string _toddlerSkin = "Skin";
        public string ToddlerSkin { get => _toddlerSkin; set { if (_toddlerSkin != value) { _toddlerSkin = value; NotifyPartChanged(nameof(ToddlerSkin), value); } } }

        // ====== 의상/악세 ======
        private string _top = "Top_Short";
        public string Top { get => _top; set { if (_top != value) { _top = value; NotifyPartChanged(nameof(Top), value); } } }

        private string _bottom = "Black";
        public string Bottom { get => _bottom; set { if (_bottom != value) { _bottom = value; NotifyPartChanged(nameof(Bottom), value); } } }

        private string _shoes = "Black";
        public string Shoes { get => _shoes; set { if (_shoes != value) { _shoes = value; NotifyPartChanged(nameof(Shoes), value); } } }

        private string _neckCollar = "Black";
        public string NeckCollar { get => _neckCollar; set { if (_neckCollar != value) { _neckCollar = value; NotifyPartChanged(nameof(NeckCollar), value); } } }

        // ====== 잠옷 ======
        private string _pajamaType = "Frog";
        public string PajamaType { get => _pajamaType; set { if (_pajamaType != value) { _pajamaType = value; NotifyPartChanged(nameof(PajamaType), value); } } }

        private string _pajamaColor = "Blue";
        public string PajamaColor { get => _pajamaColor; set { if (_pajamaColor != value) { _pajamaColor = value; NotifyPartChanged(nameof(PajamaColor), value); } } }

        // ====== 축제 ======
        private string _festivalHat = "Hat";
        public string FestivalHat { get => _festivalHat; set { if (_festivalHat != value) { _festivalHat = value; NotifyPartChanged(nameof(FestivalHat), value); } } }

        private string _festivalScarf = "Scarf";
        public string FestivalScarf { get => _festivalScarf; set { if (_festivalScarf != value) { _festivalScarf = value; NotifyPartChanged(nameof(FestivalScarf), value); } } }

        // ===== 추가: 옵션 활성화 =====
        private bool _enablePajama = true;
        public bool EnablePajama { get => _enablePajama; set { if (_enablePajama != value) { _enablePajama = value; NotifyPartChanged(nameof(EnablePajama), value); } } }

        private bool _enableFestival = true;
        public bool EnableFestival { get => _enableFestival; set { if (_enableFestival != value) { _enableFestival = value; NotifyPartChanged(nameof(EnableFestival), value); } } }
    }
}