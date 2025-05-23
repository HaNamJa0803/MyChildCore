using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using System.IO;

namespace MyChildCore
{
    public static class BabySpriteLoader
    {
        public static void ApplyBabySprite(Child child, string spouse)
        {
            string path = Path.Combine("assets", spouse, "baby", "body.png");
            string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

            if (!File.Exists(fullPath))
            {
                ModEntry.Instance.Monitor.Log($"[MyChildCore] 아기 외형 파일이 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
                return;
            }

            Texture2D texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
            child.Sprite.SpriteTexture = texture;
        }
    }
}
