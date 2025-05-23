using StardewValley.Characters;

namespace MyChildCore
{
    public static class PartApplier
    {
        public static void ApplyHair(Child child, string style, string color)
        {
            if (!string.IsNullOrEmpty(style) && !string.IsNullOrEmpty(color))
            {
                // 예: assets/parts/hair/{style}_{color}.png
                string path = $"assets/parts/hair/{style}_{color}.png";
                SpriteLoader.ApplyToChild(child, path);
            }
        }

        public static void ApplySleepwear(Child child, string style, string color)
        {
            if (!string.IsNullOrEmpty(style) && !string.IsNullOrEmpty(color))
            {
                // 예: assets/parts/sleepwear/{style}_{color}.png
                string path = $"assets/parts/sleepwear/{style}_{color}.png";
                SpriteLoader.ApplyToChild(child, path);
            }
        }

        public static void ApplySeasonalOutfit(Child child, string style, string color, string season, bool enableHat)
        {
            if (!string.IsNullOrEmpty(style) && !string.IsNullOrEmpty(color))
            {
                // 예: assets/parts/seasonal/{season}/{style}_{color}.png
                string outfitPath = $"assets/parts/seasonal/{season}/{style}_{color}.png";
                SpriteLoader.ApplyToChild(child, outfitPath);
            }

            if (enableHat)
            {
                string hatPath = $"assets/parts/seasonal/{season}/hat.png";
                SpriteLoader.ApplyToChild(child, hatPath);
            }

            if (season == "Winter")
            {
                string mufflerPath = $"assets/parts/seasonal/Winter/muffler.png";
                SpriteLoader.ApplyToChild(child, mufflerPath);
            }
        }
    }
}
