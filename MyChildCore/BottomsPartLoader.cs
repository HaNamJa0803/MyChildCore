using Microsoft.Xna.Framework.Graphics; using StardewValley.Characters; using System.IO;

namespace MyChildCore { public static class BottomsPartLoader { public static Texture2D LoadBottom(Child child, string color, string season, bool isFestivalDay) { if (isFestivalDay && season != "Spring") { // 여름~겨울 축제는 상하의 일체형 복장 적용 string file = child.Gender == 0 ? $"{season}_Pants.png" : $"{season}_Skirt.png"; string path = Path.Combine("assets", "parts", "festival", season, file); string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

if (!File.Exists(fullPath))
            {
                ModEntry.Instance.Monitor.Log($"[MyChildCore] 축제용 상의 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
                return null;
            }

            return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
        }
        else
        {
            // 평상시 또는 봄 축제 → 바지/치마 분리형 적용
            string type = child.Gender == 0 ? "pants" : "skirt";
            string file = $"{color}.png";
            string path = Path.Combine("assets", "parts", "bottoms", type, file);
            string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

            if (!File.Exists(fullPath))
            {
                ModEntry.Instance.Monitor.Log($"[MyChildCore] 평상복 하의 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
                return null;
            }

            return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
        }
    }
}

}

