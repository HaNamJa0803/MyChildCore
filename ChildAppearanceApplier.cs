using StardewValley;
using System.IO;

namespace MyChildCore
{
    public static class ChildAppearanceApplier
    {
        public static void ApplyAppearance(Child child, string imagePath)
        {
            if (!File.Exists(imagePath))
                return;

            try
            {
                var texture = Game1.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(imagePath);
                child.Sprite.Texture = texture;
            }
            catch
            {
                ModEntry.Instance.Monitor.Log($"Failed to load texture at path: {imagePath}", StardewModdingAPI.LogLevel.Error);
            }
        }
    }
}
