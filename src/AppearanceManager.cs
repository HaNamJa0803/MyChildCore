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
    /// 자녀/아기 외형 적용 매니저 (유니크 칠드런식, 레이어 합성/오탈자/캐시복구)
    /// </summary>
    public static class AppearanceManager
    {
        // 아기(Baby) 외형 적용 (레이어 합성)
        public static void ApplyBabyAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
            {
                CustomLogger.Warn("[ApplyBabyAppearance] child or parts null (Default 복구)");
                parts = PartsManager.GetDefaultParts(child, isBaby: true);
            }

            string spouse = GetRealSpouseName(child);

            // 방어 코드
            if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
            {
                ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                CustomLogger.Warn($"[AppearanceManager] 누락된 배우자 키 자동 추가: {spouse}");
            }

            // 1:1 매핑된 파츠 경로 (순서 중요!)
            List<string> layerPaths = GetLayerPaths(child, parts);

            CustomLogger.Info($"[ApplyBabyAppearance] {child.Name} 레이어 합성: {string.Join(", ", layerPaths)}");

            // 파츠 레이어 합성
            Texture2D combined = CombinePartsToTexture(layerPaths);

            if (combined != null)
            {
                child.Sprite = new AnimatedSprite(combined, 0, 16, 32);
                CustomLogger.Info($"[ApplyBabyAppearance] {child.Name} 합성/적용 성공");
            }
            else
            {
                CustomLogger.Warn($"[ApplyBabyAppearance] {child.Name} 합성 실패(Default 복구)");
            }

            ResourceManager.InvalidateChildSprite(child.Name);
        }

        // 유아(토들러) 외형 적용 (레이어 합성)
        public static void ApplyToddlerAppearance(Child child, ChildParts parts)
        {
            if (child == null || parts == null)
            {
                CustomLogger.Warn("[ApplyToddlerAppearance] child or parts null (Default 복구)");
                parts = PartsManager.GetDefaultParts(child, isBaby: false);
            }

            string spouse = GetRealSpouseName(child);

            if (!ModEntry.Config.SpouseConfigs.ContainsKey(spouse))
            {
                ModEntry.Config.SpouseConfigs[spouse] = new SpouseChildConfig();
                CustomLogger.Warn($"[AppearanceManager] 누락된 배우자 키 자동 추가: {spouse}");
            }

            // 상황별 파츠 조합 (잠옷/축제/계절 등)
            List<string> layerPaths = GetLayerPaths(child, parts);

            CustomLogger.Info($"[ApplyToddlerAppearance] {child.Name} 레이어 합성: {string.Join(", ", layerPaths)}");

            ApplyParts(child, layerPaths);
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
                // 아기용 파츠(순서: 바디 → 스킨 → 눈 → 헤어)
                layerPaths.Add($"assets/Clothes/BabyBody/{parts.BabyBodies}.png");
                layerPaths.Add($"assets/{spouse}/Baby/Skin/{parts.BabySkins}.png");
                layerPaths.Add($"assets/{spouse}/Baby/Eye/{parts.BabyEyes}.png");
                layerPaths.Add($"assets/{spouse}/Baby/Hair/{parts.BabyHairStyles}.png");
            }
            else
            {
                // 토들러: 잠옷/축제/평상복 자동 분기
                if (parts.EnablePajama && isNight)
                {
                    layerPaths.Add($"assets/Clothes/Sleep/{(isMale ? parts.BoyPajamaTypeOptions : parts.GirlPajamaTypeOptions)}/{(isMale ? parts.BoyPajamaColorOptions : parts.GirlPajamaColorOptions)}.png");
                }
                else if (parts.EnableFestival && Game1.isFestival())
                {
                    if (season == "spring")
                    {
                        layerPaths.Add($"assets/Clothes/Festival/Spring/{parts.FestivalSpringHat}.png");
                    }
                    else if (season == "summer")
                    {
                        layerPaths.Add($"assets/Clothes/Festival/Summer/{(isMale ? "BoyHat" : "GirlHat")}.png");
                        layerPaths.Add(isMale
                            ? $"assets/Clothes/Festival/Summer/{parts.BoyFestivalSummerPantsOptions}.png"
                            : $"assets/Clothes/Festival/Summer/{parts.GirlFestivalSummerSkirtOptions}.png");
                    }
                    else if (season == "fall")
                    {
                        layerPaths.Add(isMale
                            ? $"assets/Clothes/Festival/Fall/{parts.BoyFestivalFallPants}.png"
                            : $"assets/Clothes/Festival/Fall/{parts.GirlFestivalFallSkirts}.png");
                    }
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
                    // 평상복(계절/성별별)
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

            return layerPaths;
        }

        // 파츠 레이어 이미지 합성 및 적용
        private static void ApplyParts(Character character, List<string> layerPaths)
        {
            Texture2D combined = CombinePartsToTexture(layerPaths);

            if (combined != null)
            {
                character.Sprite = new AnimatedSprite(combined, 0, 16, 32);
                CustomLogger.Info("[ApplyParts] 최종 합성/적용 성공");
            }
            else
            {
                CustomLogger.Warn("[ApplyParts] 파츠 합성 실패! (Default 복구?)");
            }

            if (character is Child childObj)
                ResourceManager.InvalidateChildSprite(childObj.Name);
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

        // 배우자 이름 반환
        public static string GetRealSpouseName(Child child)
        {
            if (child == null) return "Unknown";
            try
            {
                long parentId = child.idOfParent?.Value ?? -1;
                if (parentId > 0)
                {
                    foreach (var farmer in Game1.getAllFarmers())
                    {
                        if (farmer.UniqueMultiplayerID == parentId && !string.IsNullOrEmpty(farmer.spouse))
                        {
                            if (DropdownConfig.SpouseNames.Contains(farmer.spouse))
                                return farmer.spouse;
                        }
                    }
                }
            }
            catch { }
            return "Default";
        }

        // GMCM 옵션 변경시 즉시 반영
        public static void ApplyForGMCMChange(string spouse, bool isMale, ModConfig config)
        {
            if (config == null || !config.EnableMod)
                return;

            foreach (var child in ChildManager.GetAllChildren())
            {
                if (GetRealSpouseName(child) != spouse) continue;
                if (((int)child.Gender != (isMale ? 0 : 1))) continue;

                ChildParts parts = (child.Age >= 1)
                    ? PartsManager.GetPartsForChild(child, config)
                    : PartsManager.GetPartsForBaby(child, config);

                if (parts == null)
                {
                    parts = PartsManager.GetDefaultParts(child, isBaby: child.Age == 0);
                }

                if (child.Age >= 1)
                    ApplyToddlerAppearance(child, parts);
                else
                    ApplyBabyAppearance(child, parts);
            }
        }
    }
}