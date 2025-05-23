using StardewModdingAPI; using StardewValley.Characters; using Microsoft.Xna.Framework.Graphics; using System;

namespace MyChildCore { public static class SpriteLoader { public static void ApplyToChild(Child child, string relativePath) { try { // 절대 경로로 변환 string fullPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, relativePath);

if (!File.Exists(fullPath))
            {
                ModEntry.Instance.Monitor.Log($"[MyChildCore] 스프라이트 파일을 찾을 수 없습니다: {relativePath}", LogLevel.Warn);
                return;
            }

            // 텍스처 로드
            Texture2D texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>(relativePath);

            // 자녀 스프라이트에 적용
            child.Sprite.SpriteTexture = texture;
        }
        catch (Exception ex)
        {
            ModEntry.Instance.Monitor.Log($"[MyChildCore] 스프라이트 적용 실패: {relativePath} - {ex.Message}", LogLevel.Error);
        }
    }
}

}

