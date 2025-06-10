using MyChildCore;
using System;
using System.Collections.Generic;
using System.IO;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 합성 및 직접 Sprite 교체 매니저 (딜레이 이중적용, SMAPI 6.0, 직렬화, 예외처리, 최적화)
    /// </summary>
    public static class AppearanceManager
    {
        public static event Action<Child, ChildParts, Texture2D> AppearanceChanged;

        // SMAPI ModHelper 인젝션(딜레이 이중적용용)
        private static IModHelper _helper;
        public static void Init(IModHelper helper) => _helper = helper;

        /// <summary>
        /// 실시간 연동 구독 (내부용)
        /// </summary>
        public static void Subscribe()
        {
            AppearanceChanged -= InternalSyncHook;
            AppearanceChanged += InternalSyncHook;
        }

        // 내부 연동 훅 (불필요한 알림 제거)
        private static void InternalSyncHook(Child child, ChildParts parts, Texture2D tex)
        {
            try
            {
                ResourceManager.InvalidateChildSprite(child.Name);
                CacheManager.InvalidateChildSpriteCache(child?.Name);
                DataManager.SaveData(CacheManager.GetChildCache());
                CustomLogger.Info($"[연동] 외형 실시간 연동 완료: {child?.Name}");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[연동] 외형 연동 예외: {ex.Message}");
            }
        }

        /// <summary>
        /// 외형 일괄 적용 (즉시+1프레임 딜레이 이중적용)
        /// </summary>
        public static void ApplyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null) return;

            try
            {
                // 1. 즉시 1회
                if (child.Age == 0)
                    ApplyBabyAppearance(child, parts);
                else
                    ApplyToddlerAppearance(child, parts);

                // 2. 한 프레임(혹은 1초) 딜레이 후 1회 더
                if (_helper != null)
                {
                    int tickCount = 0;
                    void Handler(object s, UpdateTickedEventArgs e)
                    {
                        tickCount++;
                        if (tickCount >= 60) // 60틱 = 1초
                        {
                            if (child.Age == 0)
                                ApplyBabyAppearance(child, parts);
                            else
                                ApplyToddlerAppearance(child, parts);
                            _helper.Events.GameLoop.UpdateTicked -= Handler;
                        }
                    }
                    _helper.Events.GameLoop.UpdateTicked += Handler;
                }
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
                    child.Sprite = new AnimatedSprite(tex, 0, 22, 16);
                    child.Sprite.SpriteWidth = 22;
                    child.Sprite.SpriteHeight = 16;
                    child.Sprite.currentFrame = 0;
                    child.Sprite.ignoreSourceRectUpdates = false;
                    child.Sprite.UpdateSourceRect();
                    CustomLogger.Info("[AppearanceManager] ApplyBabyAppearance: " + child.Name);

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
                    child.Sprite = new AnimatedSprite(tex, 0, 16, 32);
                    child.Sprite.SpriteWidth = 16;
                    child.Sprite.SpriteHeight = 32;
                    child.Sprite.currentFrame = 0;
                    child.Sprite.ignoreSourceRectUpdates = false;
                    child.Sprite.UpdateSourceRect();
                    CustomLogger.Info("[AppearanceManager] ApplyToddlerAppearance: " + child.Name);

                    OnAppearanceChanged(child, parts, tex);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyToddlerAppearance] 예외: {child?.Name}, {ex.Message}");
            }
        }

        public static Texture2D GetBabyTexture(Child child, ChildParts parts)
        {
            try
            {
                if (child == null || parts == null)
                {
                    CustomLogger.Warn("[GetBabyTexture] child or parts null (Default 복구)");
                    parts = PartsManager.GetDefaultParts(child, isBaby: true);
                }
                string spouse = parts.SpouseName;
                if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
                {
                    ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                    CustomLogger.Warn("[AppearanceManager] 누락된 배우자 키 자동 추가: " + spouse);
                }
                List<string> layerPaths = GetLayerPaths(child, parts);
                CustomLogger.Info("[GetBabyTexture] " + child.Name + " 레이어 합성: " + string.Join(", ", layerPaths));
                return CombinePartsToTexture(layerPaths, child, true);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[GetBabyTexture] 예외: {child?.Name}, {ex.Message}");
                return null;
            }
        }

        public static Texture2D GetToddlerTexture(Child child, ChildParts parts)
        {
            try
            {
                if (child == null || parts == null)
                {
                    CustomLogger.Warn("[GetToddlerTexture] child or parts null (Default 복구)");
                    parts = PartsManager.GetDefaultParts(child, isBaby: false);
                }
                string spouse = parts.SpouseName;
                if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
                {
                    ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                    CustomLogger.Warn("[AppearanceManager] 누락된 배우자 키 자동 추가: " + spouse);
                }
                List<string> layerPaths = GetLayerPaths(child, parts);
                CustomLogger.Info("[GetToddlerTexture] " + child.Name + " 레이어 합성: " + string.Join(", ", layerPaths));
                return CombinePartsToTexture(layerPaths, child, false);
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[GetToddlerTexture] 예외: {child?.Name}, {ex.Message}");
                return null;
            }
        }

        public static List<string> GetLayerPaths(Child child, ChildParts parts)
        {
            var layerPaths = new List<string>();
            string spouse = parts.SpouseName;
            bool isMale = parts.IsMale;
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;
            try
            {
                if (child.Age == 0)
                {
                    layerPaths.Add($"assets/Clothes/BabyBody/{parts.BabyBodies}.png");
                    layerPaths.Add($"assets/{spouse}/Baby/Skin/{parts.BabySkins}.png");
                    layerPaths.Add($"assets/{spouse}/Baby/Eye/{parts.BabyEyes}.png");
                    layerPaths.Add($"assets/{spouse}/Baby/Hair/{parts.BabyHairStyles}.png");
                }
                else
                {
                    if (parts.EnablePajama && isNight)
                    {
                        layerPaths.Add($"assets/Clothes/Sleep/{(isMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions)}/" +
                                       $"{(isMale ? parts.BoyPajamaColorOptions : parts.GirlPajamaColorOptions)}.png");
                    }
                    else if (parts.EnableFestival && Game1.isFestival())
                    {
                        if (season == "spring")
                            layerPaths.Add($"assets/Clothes/Festival/Spring/{parts.FestivalSpringHat}.png");
                        else if (season == "summer")
                        {
                            layerPaths.Add($"assets/Clothes/Festival/Summer/{(isMale ? "BoyHat" : "GirlHat")}.png");
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Festival/Summer/{parts.BoyFestivalSummerPantsOptions}.png"
                                : $"assets/Clothes/Festival/Summer/{parts.GirlFestivalSummerSkirtOptions}.png");
                        }
                        else if (season == "fall")
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Festival/Fall/{parts.BoyFestivalFallPants}.png"
                                : $"assets/Clothes/Festival/Fall/{parts.GirlFestivalFallSkirts}.png");
                        else if (season == "winter")
                        {
                            layerPaths.Add($"assets/Clothes/Festival/Winter/{parts.FestivalWinterHat}.png");
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Festival/Winter/{parts.BoyFestivalWinterPantsOptions}.png"
                                : $"assets/Clothes/Festival/Winter/{parts.GirlFestivalWinterSkirtOptions}.png");
                            layerPaths.Add($"assets/Clothes/Festival/Winter/{parts.FestivalWinterScarf}.png");
                        }
                    }
                    else
                    {
                        if (season == "spring")
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Top/Boy/{parts.BoyTopSpringOptions}.png"
                                : $"assets/Clothes/Top/Girl/{parts.GirlTopSpringOptions}.png");
                        else if (season == "summer")
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Top/Boy/{parts.BoyTopSummerOptions}.png"
                                : $"assets/Clothes/Top/Girl/{parts.GirlTopSummerOptions}.png");
                        else if (season == "fall")
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Top/Boy/{parts.BoyTopFallOptions}.png"
                                : $"assets/Clothes/Top/Girl/{parts.GirlTopFallOptions}.png");
                        else if (season == "winter")
                            layerPaths.Add(isMale
                                ? $"assets/Clothes/Top/Boy/{parts.BoyTopWinterOptions}.png"
                                : $"assets/Clothes/Top/Girl/{parts.GirlTopWinterOptions}.png");

                        layerPaths.Add(isMale
                            ? $"assets/Clothes/Bottom/Pants/{parts.PantsColorOptions}.png"
                            : $"assets/Clothes/Bottom/Skirt/{parts.SkirtColorOptions}.png");
                        layerPaths.Add($"assets/Clothes/Shoes/{(isMale ? parts.BoyShoesColorOptions : parts.GirlShoesColorOptions)}.png");
                        layerPaths.Add($"assets/Clothes/NeckCollar/{(isMale ? parts.BoyNeckCollarColorOptions : parts.GirlNeckCollarColorOptions)}.png");
                        layerPaths.Add(isMale
                            ? $"assets/{spouse}/Toddler/Hair/ShortCut.png"
                            : $"assets/{spouse}/Toddler/Hair/{parts.GirlHairStyles}.png");
                        layerPaths.Add($"assets/{spouse}/Toddler/Eye/{(isMale ? parts.BoyEyes : parts.GirlEyes)}.png");
                        layerPaths.Add($"assets/{spouse}/Toddler/Skin/{(isMale ? parts.BoySkins : parts.GirlSkins)}.png");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[GetLayerPaths] 예외: {child?.Name}, {ex.Message}");
            }
            return layerPaths;
        }

        public static Texture2D CombinePartsToTexture(List<string> layerPaths, Child child = null, bool isBaby = false)
        {
            if (layerPaths == null || layerPaths.Count == 0)
            {
                CustomLogger.Warn("[CombinePartsToTexture] layerPaths가 없음, 디폴트로 복구");
                return TryGetAnyPngTexture(layerPaths) ?? null;
            }
            var device = Game1.graphics.GraphicsDevice;
            Texture2D baseTex = TryGetAnyPngTexture(layerPaths);
            if (baseTex == null)
            {
                CustomLogger.Warn("[CombinePartsToTexture] baseTex가 없음, 디폴트로 복구");
                return null;
            }

            try
            {
                RenderTarget2D result = new(device, baseTex.Width, baseTex.Height);
                var spriteBatch = new SpriteBatch(device);

                device.SetRenderTarget(result);
                device.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                bool anyDrawn = false;
                foreach (var path in layerPaths)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(path) && File.Exists(path))
                        {
                            Texture2D tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                            anyDrawn = true;
                        }
                    }
                    catch { }
                }

                spriteBatch.End();
                device.SetRenderTarget(null);
                spriteBatch.Dispose();

                if (!anyDrawn)
                {
                    CustomLogger.Warn("[CombinePartsToTexture] 모든 레이어 누락, 디폴트로 복구");
                    return baseTex;
                }

                return result;
            }
            catch (Exception ex)
            {
                CustomLogger.Error("[CombinePartsToTexture] 예외 발생, 디폴트로 복구: " + ex.Message);
                return baseTex;
            }
        }

        private static Texture2D TryGetAnyPngTexture(List<string> layerPaths)
        {
            foreach (var path in layerPaths)
            {
                try
                {
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        return ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                    }
                }
                catch { }
            }
            return null;
        }

        // 배우자 이름 반환 (.NET 6.0 이하 안전하게!)
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