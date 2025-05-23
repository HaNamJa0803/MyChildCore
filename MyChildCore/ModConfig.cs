using System.Collections.Generic;

namespace MyChildCore { public class ModConfig { public Dictionary<string, string> Settings { get; set; } = new();

public bool EnableSeasonalClothing { get; set; } = true;
    public bool EnableHat { get; set; } = true;

    public ModConfig()
    {
        string[] spouses = SpouseNames.SpouseList;
        string[] genders = { "Boy", "Girl" };
        string[] props = { "Hair", "HairColor", "BottomsColor", "SleepStyle", "SleepColor" };
        string[] seasons = { "Spring", "Summer", "Fall", "Winter" };

        foreach (var spouse in spouses)
        {
            foreach (var gender in genders)
            {
                foreach (var prop in props)
                    Settings[$"{spouse}_{gender}_{prop}"] = "";

                foreach (var season in seasons)
                {
                    Settings[$"{spouse}_{gender}_{season}Style"] = "";
                    Settings[$"{spouse}_{gender}_{season}Color"] = "";
                }
            }
        }
    }

    public string Get(string spouse, string gender, string prop)
    {
        string key = $"{spouse}_{gender}_{prop}";
        return Settings.TryGetValue(key, out var value) ? value : null;
    }

    public void Set(string spouse, string gender, string prop, string value)
    {
        string key = $"{spouse}_{gender}_{prop}";
        Settings[key] = value;
    }
}

}

