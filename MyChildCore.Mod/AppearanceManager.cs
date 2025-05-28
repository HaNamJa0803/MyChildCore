using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyChildCore.Utilities
{
    public static class AppearanceManager
    {
        /// <summary>
        /// 단일 커스텀 스프라이트 적용 (테스트/실전 공용)
        /// </summary>
        public static void ApplyAppearance(Child child, Texture2D sprite, int frameX = 0, int frameY = 0)
        {
            if (child == null || sprite == null)
                return;

            child.Sprite.Texture = sprite;
            child.Sprite.SourceRect = new Rectangle(frameX, frameY, 16, 32);
        }
    }
}