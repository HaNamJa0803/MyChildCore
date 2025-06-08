using StardewModdingAPI;
using StardewValley;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MyChildCore
{
    public static class AppearanceManager
    {
        // 자녀 외형 적용 - 각 자녀마다 다른 이미지를 보이게 함
        public static void ApplyChildAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
            {
                CustomLogger.Warn("[ApplyChildAppearance] child or parts null (Default 복구)");
                parts = PartsManager.GetDefaultParts(child);
            }

            string spouse = GetRealSpouseName(child);
            List<string> layerPaths = GetLayerPaths(child, parts);

            CustomLogger.Info($"[ApplyChildAppearance] {child.Name} 레이어 합성: {string.Join(", ", layerPaths)}");

            // 파츠 합성
            Texture2D combinedTexture = CombinePartsToTexture(layerPaths);
            if (combinedTexture != null)
            {
                // 1. Sprite가 없으면 임시로 기본값 생성
                if (child.Sprite == null)
                {
                    child.Sprite = new AnimatedSprite("Characters/Default/Child", 0, 16, 32);
                }
                // 2. Texture2D를 직접 Sprite에 대입 (유니크 칠드런 트릭)
                child.Sprite.Texture = combinedTexture;
                CustomLogger.Info($"[ApplyChildAppearance] {child.Name} 합성/적용 성공");
            }
            else
            {
                CustomLogger.Warn($"[ApplyChildAppearance] {child.Name} 합성 실패, Default 이미지로 대체");
                var fallbackTex = ModEntry.ModHelper.ModContent.Load<Texture2D>("assets/CustomChild.png");
                if (child.Sprite == null)
                    child.Sprite = new AnimatedSprite("Characters/Default/Child", 0, 16, 32);
                child.Sprite.Texture = fallbackTex; // Default 이미지 적용
            }

            ResourceManager.InvalidateChildSprite(child.Name);
        }

        // 파츠 레이어 경로 생성 (자녀의 나이, 성별, 시즌 등을 고려)
        public static List<string> GetLayerPaths(Child child, ChildParts parts)
        {
            var layerPaths = new List<string>();
            string spouse = GetRealSpouseName(child);
            bool isMale = parts.IsMale;
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;

            // 자녀의 나이에 맞춰 파츠 경로 생성
            if (child.Age == 0) // 아기
            {
                layerPaths.Add($"assets/{spouse}/BabyBody/{parts.BabyBodies}.png");
                layerPaths.Add($"assets/{spouse}/BabySkin/{parts.BabySkins}.png");
                layerPaths.Add($"assets/{spouse}/BabyEyes/{parts.BabyEyes}.png");
                layerPaths.Add($"assets/{spouse}/BabyHair/{parts.BabyHairStyles}.png");
            }
            else // 유아(1세 이상)
            {
                if (parts.EnablePajama && isNight)
                {
                    layerPaths.Add($"assets/Clothes/Sleep/{(isMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions)}.png");
                }
                else if (Game1.isFestival())
                {
                    if (season == "spring") layerPaths.Add($"assets/Clothes/Festival/Spring/{parts.FestivalSpringHat}.png");
                    // 여름, 가을, 겨울 추가 방식
                }
                else
                {
                    // 평상복 계절별
                    if (season == "spring")
                        layerPaths.Add(isMale ? $"assets/Clothes/Top/Boy/{parts.BoyTopSpringOptions}.png" : $"assets/Clothes/Top/Girl/{parts.GirlTopSpringOptions}.png");
                    // 여름, 가을, 겨울 추가
                }
            }

            return layerPaths;
        }

        // 파츠 레이어 이미지 합성
        public static Texture2D CombinePartsToTexture(List<string> layerPaths)
        {
            if (layerPaths == null || layerPaths.Count == 0) return null;
            var device = Game1.graphics.GraphicsDevice;

            Texture2D baseTex = null;
            foreach (var path in layerPaths)
            {
                try
                {
                    baseTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                    break;
                }
                catch { baseTex = null; }
            }
            if (baseTex == null) return null;

            RenderTarget2D result = new(device, baseTex.Width, baseTex.Height);
            var spriteBatch = new SpriteBatch(device);

            device.SetRenderTarget(result);
            device.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var path in layerPaths)
            {
                try
                {
                    Texture2D tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                    spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                }
                catch
                {
                    CustomLogger.Warn($"[CombinePartsToTexture] {path} 로드 실패");
                }
            }

            spriteBatch.End();
            device.SetRenderTarget(null);
            spriteBatch.Dispose();

            return result;
        }

        // 배우자 이름 반환
        public static string GetRealSpouseName(Child child)
        {
            // 로직에 따라 배우자 이름 반환
            return "Default"; // 예시, 실제 구현은 필요에 맞게 추가
        }
    }
}