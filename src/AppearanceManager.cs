using MyChildCore;
using System;
using System.Collections.Generic;
using System.IO;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 합성 및 직접 Sprite 교체 매니저 (실시간 동기화·연결, 이중적용, 직렬화, try-catch)
    /// </summary>
    public static class AppearanceManager
    {
        // 실시간 외형 변경 이벤트 (외부 시스템 연동용)
        public static event Action<Child, ChildParts, Texture2D> AppearanceChanged;

        /// <summary>
        /// (실시간 연동 구독) 외부에서 한 번만 호출해도 됨
        /// </summary>
        public static void Subscribe()
        {
            // 이미 구독된 경우 중복 방지
            AppearanceChanged -= InternalSyncHook;
            AppearanceChanged += InternalSyncHook;
        }

        private static void InternalSyncHook(Child child, ChildParts parts, Texture2D tex)
        {
            try
            {
                // 1. 리소스 최신화
                ResourceManager.InvalidateChildSprite(child?.Name);

                // 2. 캐시 최신화
                CacheManager.InvalidateChildSpriteCache(child?.Name);

                // 3. 데이터 저장
                DataManager.SaveData(CacheManager.GetChildCache());

                // 4. 핫리로드 Watcher 알림(선택)
                HotReloadWatcher.NotifyChanged(child?.Name);

                // 5. GMCM 실시간 리프레시(선택)
                // GMCMManager.ReloadMenu();

                CustomLogger.Info($"[연동] 외형 실시간 연동 완료: {child?.Name}");
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[연동] 외형 연동 예외: {ex.Message}");
            }
        }

        /// <summary>
        /// 외형 일괄 적용 (이중 적용)
        /// </summary>
        public static void ApplyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
                return;

            try
            {
                if (child.Age == 0)
                {
                    ApplyBabyAppearance(child, parts);
                    ApplyBabyAppearance(child, parts); // 이중 적용!
                }
                else
                {
                    ApplyToddlerAppearance(child, parts);
                    ApplyToddlerAppearance(child, parts); // 이중 적용!
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyAppearance] 외형 적용 예외: {child?.Name}, {ex.Message}");
            }
        }

        /// <summary>
        /// 아기 외형 적용 (Sprite 교체 + 합성 + 실시간 연동)
        /// </summary>
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

                    // 실시간 연동 이벤트 호출
                    OnAppearanceChanged(child, parts, tex);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyBabyAppearance] 예외: {child?.Name}, {ex.Message}");
            }
        }

        /// <summary>
        /// 유아 외형 적용 (Sprite 교체 + 합성 + 실시간 연동)
        /// </summary>
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

                    // 실시간 연동 이벤트 호출
                    OnAppearanceChanged(child, parts, tex);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.Warn($"[ApplyToddlerAppearance] 예외: {child?.Name}, {ex.Message}");
            }
        }

        /// <summary>
        /// 아기(Baby) 외형 합성 → Texture2D 반환
        /// </summary>
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

        /// <summary>
        /// 유아(토들러) 외형 합성 → Texture2D 반환
        /// </summary>
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

        /// <summary>
        /// 파츠 레이어 경로 생성 (Default 경로까지 포함)
        /// </summary>
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

        /// <summary>
        /// 레이어 이미지 합성: 입력 경로 순서대로 Overlay (파일명 상관없이 png면 적용)
        /// </summary>
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

        /// <summary>
        /// layerPaths 중 실제로 존재하는 png 파일을 아무거나 반환 (파일명 무관)
        /// </summary>
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
            // 아예 없으면 null
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

        /// <summary>
        /// 외형 변경/합성 시 호출 → 전 시스템 동기화/연결을 위해 이벤트/콜백 전파
        /// </summary>
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