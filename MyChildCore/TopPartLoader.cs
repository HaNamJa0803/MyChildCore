using Microsoft.Xna.Framework.Graphics; using StardewValley.Characters; using System.IO;

namespace MyChildCore { public static class TopPartLoader { public static Texture2D LoadTop(Child child, string season, bool isFestivalDay) { // 상의는 평상시 또는 봄 축제일에만 적용 if (isFestivalDay && season != "Spring") return null;

string topFile = child.Gender == 0 ? "Pants.png" : "Skirt.png";
        string path = Path.Combine("assets", "parts", "festival", season, topFile);
        string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

        if (!File.Exists(fullPath))
        {
            ModEntry.Instance.Monitor.Log($"[MyChildCore] 상의 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
            return null;
        }

        return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
    }
}

}

