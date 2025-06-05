using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SpaceShared.UI;

internal class Button : Element, ISingleTexture
{
	private float Scale = 1f;

	public Texture2D Texture { get; set; }

	public Rectangle IdleTextureRect { get; set; }

	public Rectangle HoverTextureRect { get; set; }

	public Action<Element> Callback { get; set; }

	public override int Width => IdleTextureRect.Width;

	public override int Height => IdleTextureRect.Height;

	public override string HoveredSound => "Cowboy_Footstep";

	public Button()
	{
	}

	public Button(Texture2D tex)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Texture = tex;
		IdleTextureRect = new Rectangle(0, 0, tex.Width / 2, tex.Height);
		HoverTextureRect = new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height);
	}

	public override void Update(bool isOffScreen = false)
	{
		base.Update(isOffScreen);
		Scale = (base.Hover ? Math.Min(Scale + 0.013f, 1.083f) : Math.Max(Scale - 0.013f, 1f));
		if (base.Clicked)
		{
			Callback?.Invoke(this);
		}
	}

	public override void Draw(SpriteBatch b)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (!IsHidden())
		{
			Rectangle val = (base.Hover ? HoverTextureRect : IdleTextureRect);
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector((float)val.Width / 2f, (float)val.Height / 2f);
			b.Draw(Texture, base.Position + val2, (Rectangle?)val, Color.White, 0f, val2, Scale, (SpriteEffects)0, 0f);
			IClickableMenu activeClickableMenu = Game1.activeClickableMenu;
			if (activeClickableMenu != null)
			{
				activeClickableMenu.drawMouse(b, false, -1);
			}
		}
	}
}
