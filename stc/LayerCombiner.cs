using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MyChildCore
{
    public static class LayerCombiner
    {
        /// <summary>
        /// 파츠 이미지 경로 배열을 받아서, 모두 레이어로 합성해 최종 Texture2D 반환
        /// </summary>
        public static Texture2D CombineLayers(GraphicsDevice device, List<string> layerPaths)
        {
            if (layerPaths == null || layerPaths.Count == 0) return null;

            // 첫 레이어(기본 크기) 기준
            Texture2D baseTex = ModEntry.ModHelper.ModContent.Load<Texture2D>(layerPaths[0]);
            RenderTarget2D result = new(device, baseTex.Width, baseTex.Height);
            var spriteBatch = new SpriteBatch(device);

            device.SetRenderTarget(result);
            device.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var path in layerPaths)
            {
                if (string.IsNullOrEmpty(path)) continue;
                Texture2D tex = ModEntry.ModHelper.ModContent.Load<Texture2D>(path);
                spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            }

            spriteBatch.End();
            device.SetRenderTarget(null);

            spriteBatch.Dispose();
            return result;
        }
    }
}