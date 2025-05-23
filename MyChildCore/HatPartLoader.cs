using Microsoft.Xna.Framework.Graphics; using System.IO;

namespace MyChildCore { public static class HatPartLoader { public static Texture2D LoadHat(string season, bool isFestivalDay) { if (!isFestivalDay) return null;

string path = Path.Combine("assets", "parts", "hat", $"{season}_Hat.png");
        string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

        if (!File.Exists(fullPath))
        {
            ModEntry.Instance.Monitor.Log($"[MyChildCore] 모자 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
            return null;
        }

        return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
    }
}

}

