using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형(스프라이트) 변경 헬퍼 (절대 Texture2D 직접 할당하지 마세요!)
    /// 1.6.10+ 및 SMAPI 4.x 권장 방식!
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 자녀에게 새 스프라이트를 적용합니다.
        /// </summary>
        /// <param name="child">타겟 자녀 인스턴스</param>
        /// <param name="spritePath">스프라이트(스프라이트시트) 파일 경로 (assets/....png)</param>
        public static void ApplyAppearance(Child child, string spritePath)
        {
            if (child == null || string.IsNullOrEmpty(spritePath))
                return;

            // AnimatedSprite 경로로만 교체 (Texture2D 직접 할당 불가, SDV 1.6.10+ 기준!)
            // SDV는 내부적으로 Content.Load<Texture2D>(textureName) 호출하므로, 외부 경로만 올바르면 됨.
            child.Sprite = new AnimatedSprite(spritePath, 0, 16, 32);
        }
    }
}