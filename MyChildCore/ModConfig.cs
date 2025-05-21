using System.Collections.Generic;

namespace MyChildCore
{
    public class ModConfig
    {
        public bool UseSleepwear { get; set; } = true;
        public bool UseFestivalOutfits { get; set; } = true;
        public bool UseSeasonalOutfits { get; set; } = true;
        public bool AppendSpouseNameToChild { get; set; } = true;
        public Dictionary<string, SpouseSetting> SpouseSettings { get; set; } = new();
    }
}
