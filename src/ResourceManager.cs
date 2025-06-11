using MyChildCore;
using StardewModdingAPI;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyChildCore
{
    /// <summary>
    /// 자녀 리소스(스프라이트 등) 실시간 캐시 무효화 및 Observer 알림만 담당 (SMAPI 6.0 기준)
    /// 실제 PatchImage는 AssetRequested에서 처리!
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// 캐릭터 스프라이트 캐시 무효화만! (이미지 교체는 AssetRequested에서)
        /// </summary>
        public static void ApplyChildSprite(string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return;

            try
            {
                // 캐시 무효화만 실행
                ModEntry.ModHelper.GameContent.InvalidateCache($"Characters/{childName}");
                CustomLogger.Info($"[ResourceManager] 캐릭터 스프라이트 캐시 무효화: {childName}");

                NotifySpriteChanged(childName);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] {childName} 캐시 무효화 실패! Error: {ex.Message}");
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
                NotifyAllSpritesInvalidated();
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
                NotifySpriteChanged(childName);
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
                NotifySpriteChanged(assetPath);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] 커스텀 캐시 무효화 실패: {assetPath} Error: {ex.Message}");
            }
        }

        // === Observer/즉시감지: 외부 시스템에서 실시간 구독! ===
        public static event Action<string> OnSpriteChanged;
        public static event Action OnAllSpritesInvalidated;

        private static void NotifySpriteChanged(string childName)
        {
            try { OnSpriteChanged?.Invoke(childName); }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] OnSpriteChanged 이벤트 예외: {ex.Message}");
            }
        }
        private static void NotifyAllSpritesInvalidated()
        {
            try { OnAllSpritesInvalidated?.Invoke(); }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ResourceManager] OnAllSpritesInvalidated 이벤트 예외: {ex.Message}");
            }
        }
    }
}