using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace SpaceShared.UI;

internal class Scrollbar : Element
{
	private bool DragScroll;

	public int RequestHeight { get; set; }

	public int Rows { get; set; }

	public int FrameSize { get; set; }

	public int TopRow { get; private set; }

	public int MaxTopRow => Math.Max(0, Rows - FrameSize);

	public float ScrollPercent
	{
		get
		{
			if (MaxTopRow <= 0)
			{
				return 0f;
			}
			return (float)TopRow / (float)MaxTopRow;
		}
	}

	public override int Width => 24;

	public override int Height => RequestHeight;

	public void ScrollBy(int amount)
	{
		int num = Util.Clamp(0, TopRow + amount, MaxTopRow);
		if (num != TopRow)
		{
			Game1.playSound("shwip", (int?)null);
			TopRow = num;
		}
	}

	public void ScrollTo(int row)
	{
		if (TopRow != row)
		{
			Game1.playSound("shiny4", (int?)null);
			TopRow = Util.Clamp(0, row, MaxTopRow);
		}
	}

	public override void Update(bool isOffScreen = false)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		base.Update(isOffScreen);
		if (base.Clicked)
		{
			DragScroll = true;
		}
		MouseState val;
		if ((int)Constants.TargetPlatform != 0)
		{
			if (DragScroll)
			{
				val = Mouse.GetState();
				if ((int)((MouseState)(ref val)).LeftButton == 0)
				{
					DragScroll = false;
				}
			}
		}
		else if (DragScroll)
		{
			val = Game1.input.GetMouseState();
			if ((int)((MouseState)(ref val)).LeftButton == 0)
			{
				DragScroll = false;
			}
		}
		if (DragScroll)
		{
			int mouseY = Game1.getMouseY();
			int num = (int)((float)mouseY - base.Position.Y - 20f);
			ScrollTo((int)Math.Round((float)num / (float)(Height - 40) * (float)MaxTopRow));
		}
	}

	public override void Draw(SpriteBatch b)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		if (!IsHidden() && MaxTopRow != 0)
		{
			Rectangle val = default(Rectangle);
			((Rectangle)(ref val))._002Ector((int)base.Position.X, (int)base.Position.Y, Width, Height);
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector((float)val.X, (float)val.Y + (float)(Height - 40) * ScrollPercent);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), val.X, val.Y, val.Width, val.Height, Color.White, 4f, false, -1f);
			b.Draw(Game1.mouseCursors, val2, (Rectangle?)new Rectangle(435, 463, 6, 12), Color.White, 0f, default(Vector2), 4f, (SpriteEffects)0, 0.77f);
		}
	}
}
