using MyChildCore;
using System;
using StardewModdingAPI;
using Microsoft.Xna.Framework.Graphics;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 리소스(스프라이트 등) 실시간 캐시 무효화 및 직접 교체 매니저 (SMAPI 6.0 기준)
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// 자녀 한 명의 캐릭터 스프라이트 캐시 무효화 + 즉시 이미지 할당 (PatchImage 즉시)
        /// </summary>
        public static void ApplyChildSprite(string childName, Texture2D sprite)
        {
            if (string.IsNullOrEmpty(childName) || sprite == null)
                return;

            try
            {
                // 1. 기존 캐시 무효화
                ModEntry.ModHelper.GameContent.InvalidateCache($"Characters/{childName}");
                CustomLogger.Info($"[ResourceManager] 캐릭터 스프라이트 캐시 무효화: {childName}");

                // 2. PatchImage Editor 등록 (실시간 패치)
                ModEntry.ModHelper.GameContent.Edit(
                    $"Characters/{childName}",
                    asset =>
                    {
                        if (asset != null && asset.AsImage() != null)
                        {
                            asset.AsImage().PatchImage(sprite);
                            CustomLogger.Info($"[ResourceManager] {childName}에 커스텀 합성 이미지 직접 할당 완료!");
                        }
                    }
                );

                // 3. 실시간 동기화 알림
                OnSpriteChanged?.Invoke(childName);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] {childName} 캐시 무효화/직접 할당 실패! Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 모든 자녀 스프라이트/파츠 캐시 무효화
        /// </summary>
        public static void InvalidateAllChildrenSprites()
        {
            try
            {
                foreach (var child in ChildManager.GetAllChildren())
                    InvalidateChildSprite(child.Name);

                CustomLogger.Info("[ResourceManager] 모든 자녀 캐시 무효화 완료!");

                OnAllSpritesInvalidated?.Invoke();
            }
            catch (Exception ex)
            {
                CustomLogger.Warn("[ResourceManager] 전체 캐시 무효화 실패! Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 자녀 한 명의 캐릭터 스프라이트 캐시 무효화
        /// </summary>
        public static void InvalidateChildSprite(string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return;

            try
            {
                ModEntry.ModHelper.GameContent.InvalidateCache($"Characters/{childName}");
                CustomLogger.Info($"[ResourceManager] 캐릭터 스프라이트 캐시 무효화: {childName}");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] 캐시 무효화 실패: {childName} Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 특정 리소스 경로 캐시 무효화 (확장성)
        /// </summary>
        public static void InvalidateResource(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return;

            try
            {
                ModEntry.ModHelper.GameContent.InvalidateCache(assetPath);
                CustomLogger.Info($"[ResourceManager] 커스텀 리소스 캐시 무효화: {assetPath}");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] 커스텀 캐시 무효화 실패: {assetPath} Error: {ex.Message}");
            }
        }

        // --- 실시간 동기화 이벤트: 외부 시스템 연동 포인트 ---
        public static event Action<string> OnSpriteChanged;
        public static event Action OnAllSpritesInvalidated;
    }
}