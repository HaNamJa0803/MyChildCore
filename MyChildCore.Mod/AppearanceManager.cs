using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형/스프라이트 적용 매니저 (SDV 1.6.10+ 확장 호환)
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 단일 커스텀 스프라이트 즉시 적용
        /// </summary>
        public static void ApplyAppearance(Child child, Texture2D sprite, int frameX = 0, int frameY = 0)
        {
            if (child == null || sprite == null)
                return;

            child.Sprite.Texture = sprite;
            child.Sprite.SourceRect = new Rectangle(frameX, frameY, 16, 32);
        }

        /// <summary>
        /// (확장) appearanceKey 기반 자동화 적용 자리 (GMCM, 계절, 성별 등)
        /// </summary>
        public static void ApplyAppearanceAuto(Child child, string appearanceKey)
        {
            // TODO: appearanceKey 기반 외형(스프라이트시트, 스타일, GMCM, 자동화 등) 분기
        }

        /// <summary>
        /// (확장) 외형 미지정 시 기본 스프라이트 적용
        /// </summary>
        public static void ApplyDefaultAppearance(Child child)
        {
            // TODO: 기본 리소스 지정 예시
            // child.Sprite.Texture = 기본 Texture2D;
            // child.Sprite.SourceRect = new Rectangle(0, 0, 16, 32);
        }
    }
}