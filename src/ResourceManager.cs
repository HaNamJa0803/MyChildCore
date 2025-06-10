using System;
using System.IO;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class ResourceManager
    {
        /// <summary>
        /// 자녀 한 명의 캐릭터 스프라이트 캐시 무효화 + 즉시 이미지 할당 (직접교체, PatchImage)
        /// </summary>
        public static void ApplyChildSprite(string childName, Texture2D sprite)
        {
            if (string.IsNullOrEmpty(childName) || sprite == null)
                return;

            try
            {
                // 1. 기존 캐시 무효화 (정석)
                ModEntry.ModHelper.GameContent.InvalidateCache($"Characters/{childName}");
                CustomLogger.Info($"[ResourceManager] 캐릭터 스프라이트 캐시 무효화: {childName}");

                // 2. PatchImage 에디터 직접 추가 - 진짜 즉시 반영
                ModEntry.ModHelper.GameContent.AssetEditors.Add(new ChildSpritePatcher(childName, sprite));
                CustomLogger.Info($"[ResourceManager] {childName}에 커스텀 합성 이미지 직접 할당 완료!");

                // **실시간 동기화 보완** - 외형 적용 후 다른 매니저에게 알림
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

                // **실시간 동기화 보완** - 모든 캐시가 무효화된 후 알림
                OnAllSpritesInvalidated?.Invoke();
            }
            catch (Exception ex)
            {
                CustomLogger.Warn("[ResourceManager] 전체 캐시 무효화 실패! Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 자녀 한 명의 캐릭터 스프라이트 캐시 무효화 (간접방식)
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

        // **실시간 동기화 이벤트 추가** - 외형 적용 후 알림
        public static event Action<string> OnSpriteChanged;

        // **실시간 동기화 이벤트 추가** - 모든 스프라이트 무효화 후 알림
        public static event Action OnAllSpritesInvalidated;
    }

    /// <summary>
    /// PatchImage용: 캐릭터 스프라이트 동적 교체 패처 (직접 대입)
    /// (IAssetEditor 상속만 제거, 나머지는 그대로!)
    /// </summary>
    public class ChildSpritePatcher
    {
        private readonly string _childName;
        private readonly Texture2D _sprite;

        public ChildSpritePatcher(string childName, Texture2D sprite)
        {
            _childName = childName;
            _sprite = sprite;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            // 해당 캐릭터 스프라이트만 편집 (완전 일치)
            return asset.AssetNameEquals($"Characters/{_childName}");
        }

        public void Edit<T>(IAssetData asset)
        {
            // Texture2D만 합성(덮어쓰기)
            if (typeof(T) == typeof(Texture2D) && _sprite != null)
            {
                asset.AsImage().PatchImage(_sprite);
            }
        }
    }
}