using System;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public static class AppearanceManager
    {
        public static void ApplyAppearance(Child child, string spritePath)
        {
            if (child == null || string.IsNullOrEmpty(spritePath)) return;
            child.Sprite = new StardewValley.AnimatedSprite(spritePath, 0, 16, 32);
        }
    }
}