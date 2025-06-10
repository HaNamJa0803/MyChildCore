using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace MyChildCore
{
    /// <summary>
    /// 파츠 레이어 이미지 합성 핵심부 (유니크 칠드런식, 안전성/동기화 강화)
    /// </summary>
    public static class LayerCombiner
    {
        /// <summary>
        /// 파츠 이미지 경로 배열을 받아서, 모두 레이어로 합성해 최종 Texture2D 반환
        /// </summary>
        public static Texture2D CombineLayers(GraphicsDevice device, List<string> layerPaths)
        {
            if (layerPaths == null || layerPaths.Count == 0)
            {
                CustomLogger.Warn("[LayerCombiner] 레이어 경로가 없습니다.");
                return null;
            }

            // baseTex: 유효한 첫 이미지 찾기
            Texture2D baseTex = null;
            string basePath = null;
            foreach (var path in layerPaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    try
                    {
                        baseTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                        basePath = path;
                        break;
                    }
                    catch { }
                }
            }

            if (baseTex == null)
            {
                CustomLogger.Error("[LayerCombiner] 모든 레이어 파일을 찾을 수 없습니다: " + string.Join(", ", layerPaths));
                return null;
            }

            RenderTarget2D result = null;
            SpriteBatch spriteBatch = null;

            try
            {
                result = new RenderTarget2D(device, baseTex.Width, baseTex.Height);
                spriteBatch = new SpriteBatch(device);

                device.SetRenderTarget(result);
                device.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                foreach (var path in layerPaths)
                {
                    if (string.IsNullOrEmpty(path)) continue;

                    Texture2D tex = null;
                    try
                    {
                        if (File.Exists(path))
                        {
                            tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                        }
                        else
                        {
                            CustomLogger.Warn($"[LayerCombiner] 레이어 파일 없음: {path}");
                            continue;
                        }
                    }
                    catch
                    {
                        CustomLogger.Warn($"[LayerCombiner] 텍스처 로드 실패: {path}");
                        continue;
                    }
                    if (tex != null)
                    {
                        spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                    }
                }

                spriteBatch.End();
                device.SetRenderTarget(null);

                CustomLogger.Info($"[LayerCombiner] 레이어 합성 완료: {string.Join(", ", layerPaths)} (기준: {basePath})");
                return result;
            }
            catch (System.Exception ex)
            {
                CustomLogger.Error($"[LayerCombiner] 합성 예외: {ex.Message}");
                return null;
            }
            finally
            {
                spriteBatch?.Dispose();
            }
        }
    }
}