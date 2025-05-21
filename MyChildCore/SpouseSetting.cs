namespace MyChildCore
{
    public class SpouseSetting
    {
        public bool UseMaleOutfit { get; set; } = true;
        public bool UseFemaleOutfit { get; set; } = true;
        public string MaleOutfit { get; set; } = "";
        public string FemaleOutfit { get; set; } = "";
        public ToddlerGenderSetting MaleToddler { get; set; } = new();
        public ToddlerGenderSetting FemaleToddler { get; set; } = new();
    }

    public class ToddlerGenderSetting
    {
        public OutfitSetting Sleep { get; set; } = new();
        public OutfitSetting Festival { get; set; } = new();
        public OutfitSetting Seasonal { get; set; } = new();
    }

    public class OutfitSetting
    {
        public string Style { get; set; } = "1";
        public string Color { get; set; } = "1";
    }
}
