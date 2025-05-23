using Microsoft.Xna.Framework.Graphics; using System.IO;

namespace MyChildCore { public static class AccessoryPartLoader { public static Texture2D LoadAccessory(string season, bool isFestivalDay) { if (!isFestivalDay || season != "Winter") return null; // 겨울 축제에만 액세서리 적용

string path = Path.Combine("assets", "parts", "accessories", "Winter_Muffler.png");
        string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

        if (!File.Exists(fullPath))
        {
            ModEntry.Instance.Monitor.Log($"[MyChildCore] 겨울 액세서리 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
            return null;
        }

        return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
    }
}

}

