using System;
using System.IO;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyChildCore
{
    /// <summary>
    /// 자녀/아기 외형 합성 매니저 (SMAPI PatchImage 적용만, Sprite 직접 할당 금지!)
    /// </summary>
    public static class AppearanceManager
    {
        // 아기(Baby) 외형 합성 → Texture2D 반환
        public static Texture2D GetBabyTexture(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
            {
                CustomLogger.Warn("[GetBabyTexture] child or parts null (Default 복구)");
                parts = PartsManager.GetDefaultParts(child, isBaby: true);
            }

            string spouse = GetRealSpouseName(child);

            if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
            {
                ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                CustomLogger.Warn("[AppearanceManager] 누락된 배우자 키 자동 추가: " + spouse);
            }

            List<string> layerPaths = GetLayerPaths(child, parts);
            CustomLogger.Info("[GetBabyTexture] " + child.Name + " 레이어 합성: " + string.Join(", ", layerPaths));
            return CombinePartsToTexture(layerPaths);
        }

        // 유아(토들러) 외형 합성 → Texture2D 반환
        public static Texture2D GetToddlerTexture(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
            {
                CustomLogger.Warn("[GetToddlerTexture] child or parts null (Default 복구)");
                parts = PartsManager.GetDefaultParts(child, isBaby: false);
            }

            string spouse = GetRealSpouseName(child);

            if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
            {
                ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                CustomLogger.Warn("[AppearanceManager] 누락된 배우자 키 자동 추가: " + spouse);
            }

            List<string> layerPaths = GetLayerPaths(child, parts);
            CustomLogger.Info("[GetToddlerTexture] " + child.Name + " 레이어 합성: " + string.Join(", ", layerPaths));
            return CombinePartsToTexture(layerPaths);
        }

        // ★★★★★ ChildManager에서 반드시 필요! 외형 적용 트리거용 ★★★★★
        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            var tex = GetBabyTexture(child, parts);
            // PatchImage 등 SMAPI 동적 패치 시스템 사용 권장!
            // (직접 child.Sprite 할당 금지)
            // 리소스 매니저 등에서 실질적 Sprite 패치 연동
            CustomLogger.Info("[AppearanceManager] ApplyBabyAppearance 호출: " + child.Name);
        }

        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            var tex = GetToddlerTexture(child, parts);
            // PatchImage 등 SMAPI 동적 패치 시스템 사용 권장!
            // (직접 child.Sprite 할당 금지)
            // 리소스 매니저 등에서 실질적 Sprite 패치 연동
            CustomLogger.Info("[AppearanceManager] ApplyToddlerAppearance 호출: " + child.Name);
        }

        // 파츠 레이어 경로 동적 생성 (상황별 1:1 매핑)
        public static List<string> GetLayerPaths(Child child, ChildParts parts)
        {
            var layerPaths = new List<string>();
            string spouse = GetRealSpouseName(child);
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

        // 레이어 이미지 합성: 입력 경로 순서대로 Overlay
        public static Texture2D CombinePartsToTexture(List<string> layerPaths)
        {
            if (layerPaths == null || layerPaths.Count == 0) return null;
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
            if (baseTex == null) return null;

            RenderTarget2D result = new(device, baseTex.Width, baseTex.Height);
            var spriteBatch = new SpriteBatch(device);

            device.SetRenderTarget(result);
            device.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var path in layerPaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    Texture2D tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                    spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                }
            }

            spriteBatch.End();
            device.SetRenderTarget(null);
            spriteBatch.Dispose();

            return result;
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
                            // 배열 Contains는 Array.Exists로!
                            if (DropdownConfig.SpouseNames != null &&
                                Array.Exists(DropdownConfig.SpouseNames, delegate(string x) { return x == farmer.spouse; }))
                                return farmer.spouse;
                        }
                    }
                }
            }
            catch { }
            return "Default";
        }
    }
}