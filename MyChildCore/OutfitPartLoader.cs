using StardewValley.Characters; using Microsoft.Xna.Framework.Graphics; using System.IO;

namespace MyChildCore { public static class OutfitPartLoader { public static Texture2D LoadOutfit(Child child, string season) { string partName = child.Gender == 0 ? "pants.png" : "skirt.png"; string path = Path.Combine("assets", "parts", "seasonal", season, partName); string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

if (!File.Exists(fullPath))
        {
            ModEntry.Instance.Monitor.Log($"[MyChildCore] 의상 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
            return null;
        }

        return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
    }
}

}

