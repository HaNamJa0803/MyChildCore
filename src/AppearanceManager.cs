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
    /// 자녀/아기 외형 합성 및 직접 Sprite 교체 매니저 (실시간 동기화·연결)
    /// </summary>
    public static class AppearanceManager
    {
        // 모든 외형 적용 변경이 발생하면 이 이벤트로 "연결"할 수 있음
        public static event Action<Child, ChildParts, Texture2D> AppearanceChanged;

        // 아기(Baby) 외형 합성 → Texture2D 반환
        public static Texture2D GetBabyTexture(Child child, ChildParts parts)
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

        // 유아(토들러) 외형 합성 → Texture2D 반환
        public static Texture2D GetToddlerTexture(Child child, ChildParts parts)
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

        // === "직접 Sprite 교체" (동기화·연결) ===
        public static void ApplyBabyAppearance(Child child, ChildParts parts)
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
                CustomLogger.Info("[AppearanceManager] ApplyBabyAppearance 직접 교체: " + child.Name);
                OnAppearanceChanged(child, parts, tex);
            }
        }

        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
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
                CustomLogger.Info("[AppearanceManager] ApplyToddlerAppearance 직접 교체: " + child.Name);
                OnAppearanceChanged(child, parts, tex);
            }
        }

        // === 파츠 레이어 경로 생성 ===
        public static List<string> GetLayerPaths(Child child, ChildParts parts)
        {
            var layerPaths = new List<string>();
            string spouse = parts.SpouseName;
            bool isMale = parts.IsMale;
            string season = Utility.getSeasonNameFromNumber(Game1.seasonIndex).ToLower();
            bool isNight = Game1.timeOfDay >= 1800 || Game1.timeOfDay < 600;

            if (child.Age == 0)
            {
                layerPaths.Add("assets/Clothes/BabyBody/" + parts.BabyBodies + ".png");
                layerPaths.Add("assets/" + spouse + "/Baby/Skin/" + parts.BabySkins + ".png");
                layerPaths.Add("assets/" + spouse + "/Baby/Eye/" + parts.BabyEyes + ".png");
                layerPaths.Add("assets/" + spouse + "/Baby/Hair/" + parts.BabyHairStyles + ".png");
            }
            else
            {
                if (parts.EnablePajama && isNight)
                {
                    layerPaths.Add("assets/Clothes/Sleep/" + (isMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions) + "/" + (isMale ? parts.BoyPajamaColorOptions : parts.GirlPajamaColorOptions) + ".png");
                }
                else if (parts.EnableFestival && Game1.isFestival())
                {
                    if (season == "spring")
                        layerPaths.Add("assets/Clothes/Festival/Spring/" + parts.FestivalSpringHat + ".png");
                    else if (season == "summer")
                    {
                        layerPaths.Add("assets/Clothes/Festival/Summer/" + (isMale ? "BoyHat" : "GirlHat") + ".png");
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Festival/Summer/" + parts.BoyFestivalSummerPantsOptions + ".png"
                            : "assets/Clothes/Festival/Summer/" + parts.GirlFestivalSummerSkirtOptions + ".png");
                    }
                    else if (season == "fall")
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Festival/Fall/" + parts.BoyFestivalFallPants + ".png"
                            : "assets/Clothes/Festival/Fall/" + parts.GirlFestivalFallSkirts + ".png");
                    else if (season == "winter")
                    {
                        layerPaths.Add("assets/Clothes/Festival/Winter/" + parts.FestivalWinterHat + ".png");
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Festival/Winter/" + parts.BoyFestivalWinterPantsOptions + ".png"
                            : "assets/Clothes/Festival/Winter/" + parts.GirlFestivalWinterSkirtOptions + ".png");
                        layerPaths.Add("assets/Clothes/Festival/Winter/" + parts.FestivalWinterScarf + ".png");
                    }
                }
                else
                {
                    if (season == "spring")
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Top/Boy/" + parts.BoyTopSpringOptions + ".png"
                            : "assets/Clothes/Top/Girl/" + parts.GirlTopSpringOptions + ".png");
                    else if (season == "summer")
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Top/Boy/" + parts.BoyTopSummerOptions + ".png"
                            : "assets/Clothes/Top/Girl/" + parts.GirlTopSummerOptions + ".png");
                    else if (season == "fall")
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Top/Boy/" + parts.BoyTopFallOptions + ".png"
                            : "assets/Clothes/Top/Girl/" + parts.GirlTopFallOptions + ".png");
                    else if (season == "winter")
                        layerPaths.Add(isMale
                            ? "assets/Clothes/Top/Boy/" + parts.BoyTopWinterOptions + ".png"
                            : "assets/Clothes/Top/Girl/" + parts.GirlTopWinterOptions + ".png");

                    layerPaths.Add(isMale
                        ? "assets/Clothes/Bottom/Pants/" + parts.PantsColorOptions + ".png"
                        : "assets/Clothes/Bottom/Skirt/" + parts.SkirtColorOptions + ".png");
                    layerPaths.Add("assets/Clothes/Shoes/" + (isMale ? parts.BoyShoesColorOptions : parts.GirlShoesColorOptions) + ".png");
                    layerPaths.Add("assets/Clothes/NeckCollar/" + (isMale ? parts.BoyNeckCollarColorOptions : parts.GirlNeckCollarColorOptions) + ".png");
                    layerPaths.Add(isMale
                        ? "assets/" + spouse + "/Toddler/Hair/ShortCut.png"
                        : "assets/" + spouse + "/Toddler/Hair/" + parts.GirlHairStyles + ".png");
                    layerPaths.Add("assets/" + spouse + "/Toddler/Eye/" + (isMale ? parts.BoyEyes : parts.GirlEyes) + ".png");
                    layerPaths.Add("assets/" + spouse + "/Toddler/Skin/" + (isMale ? parts.BoySkins : parts.GirlSkins) + ".png");
                }
            }
            return layerPaths;
        }

        // === 레이어 이미지 합성: 입력 경로 순서대로 Overlay ===
        public static Texture2D CombinePartsToTexture(List<string> layerPaths, Child child = null, bool isBaby = false)
        {
            if (layerPaths == null || layerPaths.Count == 0)
            {
                CustomLogger.Warn("[CombinePartsToTexture] layerPaths가 없음, 디폴트로 복구");
                return PartsManager.GetDefaultTexture(child, isBaby);
            }
            var device = Game1.graphics.GraphicsDevice;

            // 첫 레이어(기본 크기) 기준
            Texture2D baseTex = null;
            foreach (var path in layerPaths)
            {
                if (File.Exists(path))
                {
                    baseTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                    break;
                }
            }
            if (baseTex == null)
            {
                CustomLogger.Warn("[CombinePartsToTexture] baseTex가 없음, 디폴트로 복구");
                return PartsManager.GetDefaultTexture(child, isBaby);
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
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        Texture2D tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                        spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                        anyDrawn = true;
                    }
                }

                spriteBatch.End();
                device.SetRenderTarget(null);
                spriteBatch.Dispose();

                if (!anyDrawn)
                {
                    CustomLogger.Warn("[CombinePartsToTexture] 모든 레이어 누락, 디폴트로 복구");
                    return PartsManager.GetDefaultTexture(child, isBaby);
                }

                return result;
            }
            catch (Exception ex)
            {
                CustomLogger.Error("[CombinePartsToTexture] 예외 발생, 디폴트로 복구: " + ex.Message);
                return PartsManager.GetDefaultTexture(child, isBaby);
            }
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
            // 1. 리소스/캐시/외부데이터 즉시 최신화 (예: ResourceManager, DataManager)
            ResourceManager.InvalidateChildSprite(child?.Name);

            // 2. 캐시 동기화(옵션): 필요시 CacheManager 등에서 별도 관리 가능
            // CacheManager.InvalidateChildSpriteCache(child?.Name);

            // 3. 동기화 이벤트: 외부에서 구독하여 실시간 반응 가능
            AppearanceChanged?.Invoke(child, parts, tex);

            // 4. 추가로 핫리로드/GMCM/저장 등 필요시 여기에 연결
            // ex) DataManager.SaveData(CacheManager.GetChildCache());
            // ex) DataManager.ApplyAllAppearances(ModEntry.Config);

            CustomLogger.Info($"[AppearanceManager] 실시간 동기화·연결 콜백 완료: {child?.Name}");
        }
    }
}