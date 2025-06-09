using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework.Graphics;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/파츠 리소스(스프라이트/이미지) 캐시 및 할당 매니저 (유니크 칠드런식 + PatchImage)
    /// </summary>
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
            }
            catch
            {
                CustomLogger.Warn($"[ResourceManager] {childName} 캐시 무효화/직접 할당 실패!");
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
            }
            catch
            {
                CustomLogger.Warn("[ResourceManager] 전체 캐시 무효화 실패!");
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
            catch
            {
                CustomLogger.Warn($"[ResourceManager] 캐시 무효화 실패: {childName}");
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
            catch
            {
                CustomLogger.Warn($"[ResourceManager] 커스텀 캐시 무효화 실패: {assetPath}");
            }
        }
    }

    /// <summary>
    /// PatchImage용: 캐릭터 스프라이트 동적 교체 패처 (직접 대입)
    /// </summary>
    public class ChildSpritePatcher : IAssetEditor
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