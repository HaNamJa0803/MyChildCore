using MyChildCore;
using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// SMAPI 방식 순수 동적 경로 + 예외 안전 외형 매니저 (.NET 6.0, File IO 사용 안함, assets 구조만 맞추면 됨)
    /// </summary>
    public static class AppearanceManager
    {
        public static event Action<Child, ChildParts, Texture2D> AppearanceChanged;

        // SMAPI ModHelper 인젝션
        private static IModHelper _helper;
        public static void Init(IModHelper helper) => _helper = helper;

        /// <summary>
        /// 외형 전체 적용: SMAPI ModContent.Load만 사용, 경로 합성은 상대경로로만!
        /// </summary>
        public static void ApplyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;

            try
            {
                if (child.Age == 0)
                    ApplyBabyAppearance(child, parts);
                else
                    ApplyToddlerAppearance(child, parts);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyAppearance] 외형 적용 예외: {child?.Name}, {ex.Message}");
            }
        }

        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            try
            {
                var tex = GetBabyTexture(child, parts);
                if (tex != null)
                {
                    child.Sprite = new AnimatedSprite($"Characters/{child.Name}", 0, 22, 16);
                    child.Sprite.SpriteWidth = 22;
                    child.Sprite.SpriteHeight = 16;
                    child.Sprite.currentFrame = 0;
                    child.Sprite.ignoreSourceRectUpdates = false;
                    child.Sprite.UpdateSourceRect();

                    OnAppearanceChanged(child, parts, tex);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyBabyAppearance] 예외: {child?.Name}, {ex.Message}");
            }
        }

        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            try
            {
                var tex = GetToddlerTexture(child, parts);
                if (tex != null)
                {
                    child.Sprite = new AnimatedSprite($"Characters/{child.Name}", 0, 16, 32);
                    child.Sprite.SpriteWidth = 16;
                    child.Sprite.SpriteHeight = 32;
                    child.Sprite.currentFrame = 0;
                    child.Sprite.ignoreSourceRectUpdates = false;
                    child.Sprite.UpdateSourceRect();

                    OnAppearanceChanged(child, parts, tex);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyToddlerAppearance] 예외: {child?.Name}, {ex.Message}");
            }
        }

        /// <summary>
        /// SMAPI 경로만 사용해서 파츠 합성: File.Exists 없이 ModContent.Load만 시도, 없는 경우 예외만 잡고 스킵
        /// </summary>
        public static Texture2D GetBabyTexture(Child child, ChildParts parts)
        {
            var layers = GetLayerPaths(child, parts, isBaby: true);
            return CombinePartsToTexture(layers, fallbackAsset: "assets/CustomChild.png");
        }

        public static Texture2D GetToddlerTexture(Child child, ChildParts parts)
        {
            var layers = GetLayerPaths(child, parts, isBaby: false);
            return CombinePartsToTexture(layers, fallbackAsset: "assets/CustomChild.png");
        }

        /// <summary>
        /// 파츠별 상대경로만 조합 (assets 내부 구조만 맞추면 됨)
        /// </summary>
        public static List<string> GetLayerPaths(Child child, ChildParts parts, bool isBaby)
        {
            var layers = new List<string>();
            string spouse = parts.SpouseName ?? "Default";

            if (isBaby)
            {
                layers.Add($"assets/Clothes/BabyBody/{parts.BabyBodies}.png");
                layers.Add($"assets/{spouse}/Baby/Skin/{parts.BabySkins}.png");
                layers.Add($"assets/{spouse}/Baby/Eye/{parts.BabyEyes}.png");
                layers.Add($"assets/{spouse}/Baby/Hair/{parts.BabyHairStyles}.png");
            }
            else
            {
                // 필요에 따라 파츠 확장
                layers.Add($"assets/Clothes/Top/{(parts.IsMale ? "Boy" : "Girl")}/{(parts.IsMale ? parts.BoyTopSpringOptions : parts.GirlTopSpringOptions)}.png");
                layers.Add($"assets/Clothes/Bottom/{(parts.IsMale ? "Pants" : "Skirt")}/{(parts.IsMale ? parts.PantsColorOptions : parts.SkirtColorOptions)}.png");
                layers.Add($"assets/Clothes/Shoes/{(parts.IsMale ? parts.BoyShoesColorOptions : parts.GirlShoesColorOptions)}.png");
                layers.Add($"assets/{spouse}/Toddler/Hair/{(parts.IsMale ? "ShortCut" : parts.GirlHairStyles)}.png");
                layers.Add($"assets/{spouse}/Toddler/Eye/{(parts.IsMale ? parts.BoyEyes : parts.GirlEyes)}.png");
                layers.Add($"assets/{spouse}/Toddler/Skin/{(parts.IsMale ? parts.BoySkins : parts.GirlSkins)}.png");
            }
            return layers;
        }

        /// <summary>
        /// 레이어 이미지 합성: File.Exists 사용하지 않고, SMAPI ModContent.Load만
        /// </summary>
        public static Texture2D CombinePartsToTexture(List<string> layerPaths, string fallbackAsset = null)
        {
            Texture2D baseTex = null;
            var device = Game1.graphics.GraphicsDevice;

            // 가장 먼저 성공하는 레이어를 base로
            foreach (var path in layerPaths)
            {
                try
                {
                    baseTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                    if (baseTex != null)
                        break;
                }
                catch { }
            }
            if (baseTex == null)
            {
                // Fallback 처리
                if (!string.IsNullOrEmpty(fallbackAsset))
                {
                    try { baseTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(fallbackAsset); }
                    catch { }
                }
            }
            if (baseTex == null)
                return null;

            try
            {
                var result = new RenderTarget2D(device, baseTex.Width, baseTex.Height);
                var spriteBatch = new SpriteBatch(device);

                device.SetRenderTarget(result);
                device.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                foreach (var path in layerPaths)
                {
                    try
                    {
                        var tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                        if (tex != null)
                            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                    }
                    catch { }
                }

                spriteBatch.End();
                device.SetRenderTarget(null);
                spriteBatch.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                CustomLogger.Error("[CombinePartsToTexture] 예외 발생, 디폴트로 복구: " + ex.Message);
                return baseTex;
            }
        }

        /// <summary>
        /// 자녀의 진짜 배우자(부모) 이름 안전 반환 (Default/Unknown 보정)
        /// </summary>
        public static string GetRealSpouseName(Child child)
        {
            if (child == null) return "Unknown";
            try
            {
                long parentId = child.idOfParent != null ? child.idOfParent.Value : -1;
                if (parentId > 0)
                {
                    foreach (var farmer in Game1.getAllFarmers())
                    {
                        if (farmer.UniqueMultiplayerID == parentId && !string.IsNullOrEmpty(farmer.spouse))
                        {
                            if (DropdownConfig.SpouseNames != null &&
                                Array.Exists(DropdownConfig.SpouseNames, x => x == farmer.spouse))
                                return farmer.spouse;
                        }
                    }
                }
            }
            catch { }
            return "Default";
        }

        private static void OnAppearanceChanged(Child child, ChildParts parts, Texture2D tex)
        {
            try
            {
                AppearanceChanged?.Invoke(child, parts, tex);
                CustomLogger.Info($"[AppearanceManager] 실시간 동기화·연결 콜백 완료: {child?.Name}");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[OnAppearanceChanged] 예외: {child?.Name}, {ex.Message}");
            }
        }
    }
}