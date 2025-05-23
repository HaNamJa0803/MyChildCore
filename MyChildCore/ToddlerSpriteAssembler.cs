using Microsoft.Xna.Framework.Graphics; using StardewValley.Characters; using Microsoft.Xna.Framework;

namespace MyChildCore { public static class ToddlerSpriteAssembler { public static Texture2D Assemble(Child child, string spouse, string hairStyle, string hairColor, string bottomsColor, string season, bool isFestivalDay, bool enableHat) { string genderFolder = child.Gender == 0 ? "Boy" : "Girl"; string bodyPath = $"assets/{spouse}/toddler/{genderFolder}/body.png"; Texture2D body = ModEntry.Instance.Helper.ModContent.Load<Texture2D>(bodyPath);

// 파츠 로드
        Texture2D top = TopPartLoader.LoadTop(child, season, isFestivalDay);
        Texture2D bottoms = BottomsPartLoader.LoadBottom(child, bottomsColor, season, isFestivalDay);
        Texture2D shoes = ShoesPartLoader.LoadShoes(season, isFestivalDay);
        Texture2D hair = HairPartLoader.LoadHair(child, hairStyle, hairColor);
        Texture2D hat = enableHat ? HatPartLoader.LoadHat(season, isFestivalDay) : null;
        Texture2D accessory = AccessoryPartLoader.LoadAccessory(season, isFestivalDay);

        RenderTarget2D composite = new RenderTarget2D(Game1.graphics.GraphicsDevice, body.Width, body.Height);
        Game1.graphics.GraphicsDevice.SetRenderTarget(composite);
        Game1.graphics.GraphicsDevice.Clear(Color.Transparent);

        SpriteBatch b = Game1.spriteBatch;
        b.Begin();
        b.Draw(body, Vector2.Zero, Color.White);
        if (bottoms != null) b.Draw(bottoms, Vector2.Zero, Color.White); // 상하의 일체형일 수도 있으므로 우선 적용
        if (top != null) b.Draw(top, Vector2.Zero, Color.White); // 분리형 상의는 그 위에 적용
        if (shoes != null) b.Draw(shoes, Vector2.Zero, Color.White);
        if (hair != null) b.Draw(hair, Vector2.Zero, Color.White);
        if (hat != null) b.Draw(hat, Vector2.Zero, Color.White);
        if (accessory != null) b.Draw(accessory, Vector2.Zero, Color.White);
        b.End();

        Game1.graphics.GraphicsDevice.SetRenderTarget(null);
        return composite;
    }
}

}

