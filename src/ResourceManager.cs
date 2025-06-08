using StardewModdingAPI;
using StardewValley;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/파츠 리소스(스프라이트/이미지) 캐시 관리 매니저 (유니크 칠드런식)
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// 자녀 한 명의 캐릭터 스프라이트 캐시 무효화
        /// </summary>
        public static void InvalidateChildSprite(string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return;

            try
            {
                // 기본 캐릭터 스프라이트
                ModEntry.ModHelper.GameContent.InvalidateCache($"Characters/{childName}");
                CustomLogger.Info($"[ResourceManager] 캐릭터 스프라이트 캐시 무효화: {childName}");

                // 필요시 추가 파츠별 캐시 무효화도 여기서!
                // 예: 눈/헤어/의상/액세서리 등
                // ModEntry.ModHelper.GameContent.InvalidateCache($"assets/{childName}/SomePart.png");
            }
            catch
            {
                CustomLogger.Warn($"[ResourceManager] 캐시 무효화 실패: {childName}");
            }
        }

        /// <summary>
        /// 모든 자녀 스프라이트/파츠 캐시 무효화 (동적 반복, 오탈자X)
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
        /// 특정 리소스 경로 캐시 무효화 (확장성/기타 커스텀 파츠)
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

        // === 향후 확장: 커스텀 파츠 동적 로드/저장/삭제 등도 여기에 추가 가능! ===
    }
}