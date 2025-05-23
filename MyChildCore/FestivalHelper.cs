using StardewValley;

namespace MyChildCore { public static class FestivalHelper { public static bool IsFestivalDay() { return !string.IsNullOrEmpty(Game1.currentLocation?.festivalName); }

public static bool IsFestivalDay(out string season)
    {
        season = Game1.currentSeason;
        return IsFestivalDay();
    }

    public static string GetCurrentSeason()
    {
        return Game1.currentSeason;
    }
}

}

