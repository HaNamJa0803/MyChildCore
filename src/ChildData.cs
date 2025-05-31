namespace MyChildCore.Utilities
{
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public string HairKey { get; set; }
        public string TopKey { get; set; }
        public string BottomKey { get; set; }
        public string ShoesKey { get; set; }
        public string NeckKey { get; set; }
        public string PajamaStyle { get; set; }
        public int PajamaColorIndex { get; set; }
        public string EyeKey { get; set; }
        public string SkinKey { get; set; }
        public string FestivalTopKey { get; set; }
        public string FestivalHatKey { get; set; }
        public string FestivalNeckKey { get; set; }

        public ChildData() { }
        public ChildData(
            string name, long parentID, int gender,
            string hairKey, string topKey, string bottomKey, string shoesKey, string neckKey,
            string pajamaStyle, int pajamaColorIndex,
            string eyeKey = null, string skinKey = null,
            string festivalTopKey = null, string festivalHatKey = null, string festivalNeckKey = null)
        {
            Name = name;
            ParentID = parentID;
            Gender = gender;
            HairKey = hairKey;
            TopKey = topKey;
            BottomKey = bottomKey;
            ShoesKey = shoesKey;
            NeckKey = neckKey;
            PajamaStyle = pajamaStyle;
            PajamaColorIndex = pajamaColorIndex;
            EyeKey = eyeKey;
            SkinKey = skinKey;
            FestivalTopKey = festivalTopKey;
            FestivalHatKey = festivalHatKey;
            FestivalNeckKey = festivalNeckKey;
        }
    }
}