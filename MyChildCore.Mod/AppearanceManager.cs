using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 외형(스프라이트) 변경 헬퍼 (절대 Texture2D 직접 할당하지 마세요!)
    /// </summary>
    public static class AppearanceManager
    {
        /// <summary>
        /// 자녀에게 새 스프라이트를 적용합니다.
        /// </summary>
        /// <param name="child">타겟 자녀 인스턴스</param>
        /// <param name="spritePath">스프라이트(스프라이트시트) 파일 경로</param>
        public static void ApplyAppearance(Child child, string spritePath)
        {
            if (child == null || string.IsNullOrEmpty(spritePath))
                return;
            // SDV: AnimatedSprite(string textureName, int currentFrame, int frameWidth, int frameHeight)
            // - textureName은 SDV 내부에서 content.Load<Texture2D>(textureName) 형태로 자동 처리
            // - Texture2D 객체 할당은 불가, 경로만 세팅!
            child.Sprite = new AnimatedSprite(spritePath, 0, 16, 32);
        }
    }
}