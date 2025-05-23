using Microsoft.Xna.Framework.Graphics; using System.IO;

namespace MyChildCore { public static class ShoesPartLoader { public static Texture2D LoadShoes(string season, bool isFestivalDay) { string path;

if (isFestivalDay)
        {
            // 봄 축제는 평상복 + 모자 구조이므로 기본 신발 사용
            if (season == "Spring")
                path = Path.Combine("assets", "parts", "Shoes", "Shoes.png");
            else
                path = Path.Combine("assets", "parts", "Shoes", $"{season}_Shoes.png");
        }
        else
        {
            path = Path.Combine("assets", "parts", "Shoes", "Shoes.png");
        }

        string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

        if (!File.Exists(fullPath))
        {
            ModEntry.Instance.Monitor.Log($"[MyChildCore] 신발 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
            return null;
        }

        return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
    }
}

}

