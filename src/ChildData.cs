using MyChildCore;
using StardewValley.Characters;
using System;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 데이터 DTO (직렬화/저장/불러오기 전용) + 즉시감지(Observer) 지원
    /// </summary>
    public class ChildData
    {
        // === 즉시감지 이벤트 ===
        public event Action<string, object> OnFieldChanged;

        private void Notify(string field, object value)
        {
            try { OnFieldChanged?.Invoke(field, value); }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ChildData] 즉시감지 예외: {ex.Message}");
            }
        }

        // ==== 기본 정보 ====
        private string _name;
        public string Name { get => _name; set { if (_name != value) { _name = value; Notify(nameof(Name), value); } } }

        private long _parentID;
        public long ParentID { get => _parentID; set { if (_parentID != value) { _parentID = value; Notify(nameof(ParentID), value); } } }

        private int _gender;
        public int Gender { get => _gender; set { if (_gender != value) { _gender = value; Notify(nameof(Gender), value); } } }

        private int _age;
        public int Age { get => _age; set { if (_age != value) { _age = value; Notify(nameof(Age), value); } } }

        private bool _isMale;
        public bool IsMale { get => _isMale; set { if (_isMale != value) { _isMale = value; Notify(nameof(IsMale), value); } } }

        private string _spouseName;
        public string SpouseName { get => _spouseName; set { if (_spouseName != value) { _spouseName = value; Notify(nameof(SpouseName), value); } } }

        // ==== 파츠 필드 ====
        private string _topKeyMaleShort;
        public string TopKeyMaleShort { get => _topKeyMaleShort; set { if (_topKeyMaleShort != value) { _topKeyMaleShort = value; Notify(nameof(TopKeyMaleShort), value); } } }

        private string _topKeyMaleLong;
        public string TopKeyMaleLong { get => _topKeyMaleLong; set { if (_topKeyMaleLong != value) { _topKeyMaleLong = value; Notify(nameof(TopKeyMaleLong), value); } } }

        private string _topKeyFemaleShort;
        public string TopKeyFemaleShort { get => _topKeyFemaleShort; set { if (_topKeyFemaleShort != value) { _topKeyFemaleShort = value; Notify(nameof(TopKeyFemaleShort), value); } } }

        private string _topKeyFemaleLong;
        public string TopKeyFemaleLong { get => _topKeyFemaleLong; set { if (_topKeyFemaleLong != value) { _topKeyFemaleLong = value; Notify(nameof(TopKeyFemaleLong), value); } } }

        private string _pantsKeyMale;
        public string PantsKeyMale { get => _pantsKeyMale; set { if (_pantsKeyMale != value) { _pantsKeyMale = value; Notify(nameof(PantsKeyMale), value); } } }

        private string _skirtKeyFemale;
        public string SkirtKeyFemale { get => _skirtKeyFemale; set { if (_skirtKeyFemale != value) { _skirtKeyFemale = value; Notify(nameof(SkirtKeyFemale), value); } } }

        private string _shoesKey;
        public string ShoesKey { get => _shoesKey; set { if (_shoesKey != value) { _shoesKey = value; Notify(nameof(ShoesKey), value); } } }

        private string _neckKey;
        public string NeckKey { get => _neckKey; set { if (_neckKey != value) { _neckKey = value; Notify(nameof(NeckKey), value); } } }

        private string _hairKey;
        public string HairKey { get => _hairKey; set { if (_hairKey != value) { _hairKey = value; Notify(nameof(HairKey), value); } } }

        private string _skinKey;
        public string SkinKey { get => _skinKey; set { if (_skinKey != value) { _skinKey = value; Notify(nameof(SkinKey), value); } } }

        private string _eyeKey;
        public string EyeKey { get => _eyeKey; set { if (_eyeKey != value) { _eyeKey = value; Notify(nameof(EyeKey), value); } } }

        private string _pajamaKey;
        public string PajamaKey { get => _pajamaKey; set { if (_pajamaKey != value) { _pajamaKey = value; Notify(nameof(PajamaKey), value); } } }

        private string _pajamaStyle;
        public string PajamaStyle { get => _pajamaStyle; set { if (_pajamaStyle != value) { _pajamaStyle = value; Notify(nameof(PajamaStyle), value); } } }

        private int _pajamaColorIndex;
        public int PajamaColorIndex { get => _pajamaColorIndex; set { if (_pajamaColorIndex != value) { _pajamaColorIndex = value; Notify(nameof(PajamaColorIndex), value); } } }

        // ==== 계절/축제별 파츠 ====
        private string _springHatKeyMale;
        public string SpringHatKeyMale { get => _springHatKeyMale; set { if (_springHatKeyMale != value) { _springHatKeyMale = value; Notify(nameof(SpringHatKeyMale), value); } } }

        private string _springHatKeyFemale;
        public string SpringHatKeyFemale { get => _springHatKeyFemale; set { if (_springHatKeyFemale != value) { _springHatKeyFemale = value; Notify(nameof(SpringHatKeyFemale), value); } } }

        private string _summerHatKeyMale;
        public string SummerHatKeyMale { get => _summerHatKeyMale; set { if (_summerHatKeyMale != value) { _summerHatKeyMale = value; Notify(nameof(SummerHatKeyMale), value); } } }

        private string _summerHatKeyFemale;
        public string SummerHatKeyFemale { get => _summerHatKeyFemale; set { if (_summerHatKeyFemale != value) { _summerHatKeyFemale = value; Notify(nameof(SummerHatKeyFemale), value); } } }

        private string _summerTopKeyMale;
        public string SummerTopKeyMale { get => _summerTopKeyMale; set { if (_summerTopKeyMale != value) { _summerTopKeyMale = value; Notify(nameof(SummerTopKeyMale), value); } } }

        private string _summerTopKeyFemale;
        public string SummerTopKeyFemale { get => _summerTopKeyFemale; set { if (_summerTopKeyFemale != value) { _summerTopKeyFemale = value; Notify(nameof(SummerTopKeyFemale), value); } } }

        private string _fallTopKeyMale;
        public string FallTopKeyMale { get => _fallTopKeyMale; set { if (_fallTopKeyMale != value) { _fallTopKeyMale = value; Notify(nameof(FallTopKeyMale), value); } } }

        private string _fallTopKeyFemale;
        public string FallTopKeyFemale { get => _fallTopKeyFemale; set { if (_fallTopKeyFemale != value) { _fallTopKeyFemale = value; Notify(nameof(FallTopKeyFemale), value); } } }

        private string _winterHatKeyMale;
        public string WinterHatKeyMale { get => _winterHatKeyMale; set { if (_winterHatKeyMale != value) { _winterHatKeyMale = value; Notify(nameof(WinterHatKeyMale), value); } } }

        private string _winterHatKeyFemale;
        public string WinterHatKeyFemale { get => _winterHatKeyFemale; set { if (_winterHatKeyFemale != value) { _winterHatKeyFemale = value; Notify(nameof(WinterHatKeyFemale), value); } } }

        private string _winterTopKeyMale;
        public string WinterTopKeyMale { get => _winterTopKeyMale; set { if (_winterTopKeyMale != value) { _winterTopKeyMale = value; Notify(nameof(WinterTopKeyMale), value); } } }

        private string _winterTopKeyFemale;
        public string WinterTopKeyFemale { get => _winterTopKeyFemale; set { if (_winterTopKeyFemale != value) { _winterTopKeyFemale = value; Notify(nameof(WinterTopKeyFemale), value); } } }

        private string _winterNeckKey;
        public string WinterNeckKey { get => _winterNeckKey; set { if (_winterNeckKey != value) { _winterNeckKey = value; Notify(nameof(WinterNeckKey), value); } } }

        // ==== 아기 파츠 ====
        private string _babyHairKey;
        public string BabyHairKey { get => _babyHairKey; set { if (_babyHairKey != value) { _babyHairKey = value; Notify(nameof(BabyHairKey), value); } } }

        private string _babySkinKey;
        public string BabySkinKey { get => _babySkinKey; set { if (_babySkinKey != value) { _babySkinKey = value; Notify(nameof(BabySkinKey), value); } } }

        private string _babyEyeKey;
        public string BabyEyeKey { get => _babyEyeKey; set { if (_babyEyeKey != value) { _babyEyeKey = value; Notify(nameof(BabyEyeKey), value); } } }

        private string _babyBodyKey;
        public string BabyBodyKey { get => _babyBodyKey; set { if (_babyBodyKey != value) { _babyBodyKey = value; Notify(nameof(BabyBodyKey), value); } } }

        // === Child → ChildData 변환 (필수!) ===
        public static ChildData FromChild(Child child)
        {
            if (child == null)
                return null;

            ChildParts parts = (child.Age == 0)
                ? PartsManager.GetPartsForBaby(child, ModEntry.Config)
                : PartsManager.GetPartsForChild(child, ModEntry.Config);

            if (parts == null)
                parts = PartsManager.GetDefaultParts(child, child.Age == 0);

            return new ChildData
            {
                Name = child.Name,
                ParentID = child.idOfParent != null ? child.idOfParent.Value : -1,
                Gender = (int)child.Gender,
                Age = child.Age,
                IsMale = child.Gender == 0,
                SpouseName = AppearanceManager.GetRealSpouseName(child),

                TopKeyMaleShort = parts.BoyTopSpringOptions,
                TopKeyMaleLong = parts.BoyTopWinterOptions,
                TopKeyFemaleShort = parts.GirlTopSpringOptions,
                TopKeyFemaleLong = parts.GirlTopWinterOptions,
                PantsKeyMale = parts.PantsColorOptions,
                SkirtKeyFemale = parts.SkirtColorOptions,
                ShoesKey = parts.IsMale ? parts.BoyShoesColorOptions : parts.GirlShoesColorOptions,
                NeckKey = parts.IsMale ? parts.BoyNeckCollarColorOptions : parts.GirlNeckCollarColorOptions,
                HairKey = parts.IsMale ? parts.BoyHairStyles : parts.GirlHairStyles,
                SkinKey = parts.IsMale ? parts.BoySkins : parts.GirlSkins,
                EyeKey = parts.IsMale ? parts.BoyEyes : parts.GirlEyes,
                PajamaKey = parts.IsMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions,
                PajamaStyle = null,
                PajamaColorIndex = 0,

                SpringHatKeyMale = parts.FestivalSpringHat,
                SpringHatKeyFemale = parts.FestivalSpringHat,
                SummerHatKeyMale = parts.FestivalSummerHat,
                SummerHatKeyFemale = parts.FestivalSummerHat,
                SummerTopKeyMale = parts.BoyTopSummerOptions,
                SummerTopKeyFemale = parts.GirlTopSummerOptions,
                FallTopKeyMale = parts.BoyTopFallOptions,
                FallTopKeyFemale = parts.GirlTopFallOptions,
                WinterHatKeyMale = parts.FestivalWinterHat,
                WinterHatKeyFemale = parts.FestivalWinterHat,
                WinterTopKeyMale = parts.BoyTopWinterOptions,
                WinterTopKeyFemale = parts.GirlTopWinterOptions,
                WinterNeckKey = parts.FestivalWinterScarf,

                BabyHairKey = parts.BabyHairStyles,
                BabySkinKey = parts.BabySkins,
                BabyEyeKey = parts.BabyEyes,
                BabyBodyKey = parts.BabyBodies,
            };
        }

        // (Optional) ChildParts로 변환 (동기화/외형적용용)
        public ChildParts ToParts()
        {
            return new ChildParts
            {
                SpouseName = SpouseName,
                IsMale = IsMale,
                BoyTopSpringOptions = TopKeyMaleShort,
                BoyTopWinterOptions = TopKeyMaleLong,
                GirlTopSpringOptions = TopKeyFemaleShort,
                GirlTopWinterOptions = TopKeyFemaleLong,
                PantsColorOptions = PantsKeyMale,
                SkirtColorOptions = SkirtKeyFemale,
                BoyShoesColorOptions = IsMale ? ShoesKey : null,
                GirlShoesColorOptions = !IsMale ? ShoesKey : null,
                BoyNeckCollarColorOptions = IsMale ? NeckKey : null,
                GirlNeckCollarColorOptions = !IsMale ? NeckKey : null,
                BoyHairStyles = IsMale ? HairKey : null,
                GirlHairStyles = !IsMale ? HairKey : null,
                BoySkins = IsMale ? SkinKey : null,
                GirlSkins = !IsMale ? SkinKey : null,
                BoyEyes = IsMale ? EyeKey : null,
                GirlEyes = !IsMale ? EyeKey : null,
                BoyPajamaTypeOptions = IsMale ? PajamaKey : null,
                GirlPajamaTypeOptions = !IsMale ? PajamaKey : null,

                FestivalSpringHat = SpringHatKeyMale,
                FestivalSummerHat = SummerHatKeyMale,
                FestivalWinterHat = WinterHatKeyMale,
                FestivalWinterScarf = WinterNeckKey,

                BabyHairStyles = BabyHairKey,
                BabySkins = BabySkinKey,
                BabyEyes = BabyEyeKey,
                BabyBodies = BabyBodyKey
            };
        }
    }
}