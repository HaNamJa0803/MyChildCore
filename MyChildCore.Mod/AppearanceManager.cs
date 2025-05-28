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

            // 프레임 기준: 16x32 (확장시 파라미터 자동화)
            child.Sprite.Texture = sprite;
            child.Sprite.SourceRect = new Rectangle(frameX, frameY, 16, 32);
        }

        // --- [확장] GMCM/자동화 스타일 분기 구조 ---
        // 추후, GMCM 옵션/자동화 조건/외형키 기반 자동 적용 구조 자리만 미리 확장
        public static void ApplyAppearanceAuto(Child child, string appearanceKey)
        {
            // TODO: appearanceKey 기반 외형 리소스 로딩 및 적용
            // 예: GMCM에서 설정한 옵션(스타일/계절/성별 등)에 따라 분기 적용
        }

        // (확장) 기본값 적용, 다양한 외형 오버로드 등도 필요시 추가
        public static void ApplyDefaultAppearance(Child child)
        {
            // 예시: 외형 미지정 시 기본값 적용 (직접 Sprite 리소스 지정)
            // 추후 확장
        }
    }
}