using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MyChildCore
{
    public static class HairPartLoader
    {
        public static Texture2D LoadHair(Child child, string style, string color)
        {
            // 남자 아이는 스타일 고정 "Short" + 색상 선택 허용
            if (child.Gender == 0)
                style = "Short";

            string path = Path.Combine("assets", "parts", "hair", style, $"{color}.png");
            string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, path);

            if (!File.Exists(fullPath))
            {
                ModEntry.Instance.Monitor.Log($"[MyChildCore] 헤어 파츠 파일을 찾을 수 없습니다: {path}", StardewModdingAPI.LogLevel.Warn);
                return null;
            }

            return ModEntry.Instance.Helper.ModContent.Load<Texture2D>(path);
        }
    }
}