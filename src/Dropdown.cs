using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;

namespace SpaceShared.UI;

internal class Dropdown : Element, ISingleTexture
{
	public bool Dropped;

	public Action<Element> Callback;

	public static Dropdown ActiveDropdown;

	public static int SinceDropdownWasActive;

	public int RequestWidth { get; set; }

	public int MaxValuesAtOnce { get; set; }

	public Texture2D Texture { get; set; } = Game1.mouseCursors;


	public Rectangle BackgroundTextureRect { get; set; } = OptionsDropDown.dropDownBGSource;


	public Rectangle ButtonTextureRect { get; set; } = OptionsDropDown.dropDownButtonSource;


	public string Value
	{
		get
		{
			return Choices[ActiveChoice];
		}
		set
		{
			if (Choices.Contains(value))
			{
				ActiveChoice = Array.IndexOf(Choices, value);
			}
		}
	}

	public string Label => Labels[ActiveChoice];

	public int ActiveChoice { get; set; }

	public int ActivePosition { get; set; }

	public string[] Choices { get; set; } = new string[1] { "null" };


	public string[] Labels { get; set; } = new string[1] { "null" };


	public override int Width => Math.Max(300, Math.Min(500, RequestWidth));

	public override int Height => 44;

	public override string ClickedSound => "shwip";

	public override void Update(bool isOffScreen = false)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Invalid comparison between Unknown and I4
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		base.Update(isOffScreen);
		bool flag = false;
		if (base.Clicked && ActiveDropdown == null)
		{
			flag = true;
			Dropped = true;
			base.Parent.RenderLast = this;
		}
		if (Dropped)
		{
			MouseState val;
			GamePadState gamePadState;
			GamePadButtons buttons;
			if ((int)Constants.TargetPlatform != 0)
			{
				val = Mouse.GetState();
				if ((int)((MouseState)(ref val)).LeftButton == 1 && (int)((MouseState)(ref Game1.oldMouseState)).LeftButton == 0)
				{
					goto IL_0099;
				}
				gamePadState = Game1.input.GetGamePadState();
				buttons = ((GamePadState)(ref gamePadState)).Buttons;
				if ((int)((GamePadButtons)(ref buttons)).A == 1)
				{
					buttons = ((GamePadState)(ref Game1.oldPadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons)).A == 0)
					{
						goto IL_0099;
					}
				}
			}
			else
			{
				val = Game1.input.GetMouseState();
				if ((int)((MouseState)(ref val)).LeftButton == 1 && (int)((MouseState)(ref Game1.oldMouseState)).LeftButton == 0)
				{
					goto IL_0133;
				}
				gamePadState = Game1.input.GetGamePadState();
				buttons = ((GamePadState)(ref gamePadState)).Buttons;
				if ((int)((GamePadButtons)(ref buttons)).A == 1)
				{
					buttons = ((GamePadState)(ref Game1.oldPadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons)).A == 0)
					{
						goto IL_0133;
					}
				}
			}
			goto IL_016c;
		}
		goto IL_0217;
		IL_0217:
		if (Dropped)
		{
			ActiveDropdown = this;
			SinceDropdownWasActive = 3;
			return;
		}
		if (ActiveDropdown == this)
		{
			ActiveDropdown = null;
		}
		ActivePosition = Math.Min(ActiveChoice, Choices.Length - MaxValuesAtOnce);
		return;
		IL_0099:
		if (!flag)
		{
			Game1.playSound("drumkit6", (int?)null);
			Dropped = false;
			if (base.Parent.RenderLast == this)
			{
				base.Parent.RenderLast = null;
			}
		}
		goto IL_016c;
		IL_016c:
		int num = Math.Min(MaxValuesAtOnce, Choices.Length - ActivePosition) * Height;
		int num2 = Math.Min((int)base.Position.Y, ((Rectangle)(ref Game1.uiViewport)).Height - num);
		Rectangle val2 = default(Rectangle);
		((Rectangle)(ref val2))._002Ector((int)base.Position.X, num2, Width, Height * MaxValuesAtOnce);
		if (((Rectangle)(ref val2)).Contains(Game1.getOldMouseX(), Game1.getOldMouseY()))
		{
			int num3 = (Game1.getOldMouseY() - num2) / Height;
			ActiveChoice = num3 + ActivePosition;
			Callback?.Invoke(this);
		}
		goto IL_0217;
		IL_0133:
		if (!flag)
		{
			Game1.playSound("drumkit6", (int?)null);
			Dropped = false;
			if (base.Parent.RenderLast == this)
			{
				base.Parent.RenderLast = null;
			}
		}
		goto IL_016c;
	}

	public void ReceiveScrollWheelAction(int direction)
	{
		if (Dropped)
		{
			ActivePosition = Math.Min(Math.Max(ActivePosition - direction / 120, 0), Choices.Length - MaxValuesAtOnce);
		}
		else
		{
			ActiveDropdown = null;
		}
	}

	public void DrawOld(SpriteBatch b)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		IClickableMenu.drawTextureBox(b, Texture, BackgroundTextureRect, (int)base.Position.X, (int)base.Position.Y, Width - 48, Height, Color.White, 4f, false, -1f);
		b.DrawString(Game1.smallFont, Value, new Vector2(base.Position.X + 4f, base.Position.Y + 8f), Game1.textColor);
		b.Draw(Texture, new Vector2(base.Position.X + (float)Width - 48f, base.Position.Y), (Rectangle?)ButtonTextureRect, Color.White, 0f, Vector2.Zero, 4f, (SpriteEffects)0, 0f);
		if (!Dropped)
		{
			return;
		}
		int num = Choices.Length * Height;
		IClickableMenu.drawTextureBox(b, Texture, BackgroundTextureRect, (int)base.Position.X, (int)base.Position.Y, Width - 48, num, Color.White, 4f, false, -1f);
		for (int i = 0; i < Choices.Length; i++)
		{
			if (i == ActiveChoice)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)base.Position.X + 4, (int)base.Position.Y + i * Height, Width - 48 - 8, Height), (Rectangle?)null, Color.Wheat, 0f, Vector2.Zero, (SpriteEffects)0, 0.98f);
			}
			b.DrawString(Game1.smallFont, Choices[i], new Vector2(base.Position.X + 4f, base.Position.Y + (float)(i * Height) + 8f), Game1.textColor, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 1f);
		}
	}

	public override void Draw(SpriteBatch b)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		if (IsHidden())
		{
			return;
		}
		IClickableMenu.drawTextureBox(b, Texture, BackgroundTextureRect, (int)base.Position.X, (int)base.Position.Y, Width - 48, Height, Color.White, 4f, false, -1f);
		b.DrawString(Game1.smallFont, Label, new Vector2(base.Position.X + 4f, base.Position.Y + 8f), Game1.textColor);
		b.Draw(Texture, new Vector2(base.Position.X + (float)Width - 48f, base.Position.Y), (Rectangle?)ButtonTextureRect, Color.White, 0f, Vector2.Zero, 4f, (SpriteEffects)0, 0f);
		if (!Dropped)
		{
			return;
		}
		int maxValuesAtOnce = MaxValuesAtOnce;
		int activePosition = ActivePosition;
		int num = Math.Min(Choices.Length, activePosition + maxValuesAtOnce);
		int num2 = Math.Min(maxValuesAtOnce, Choices.Length - ActivePosition) * Height;
		int num3 = Math.Min((int)base.Position.Y, ((Rectangle)(ref Game1.uiViewport)).Height - num2);
		IClickableMenu.drawTextureBox(b, Texture, BackgroundTextureRect, (int)base.Position.X, num3, Width - 48, num2, Color.White, 4f, false, -1f);
		for (int i = activePosition; i < num; i++)
		{
			if (i == ActiveChoice)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)base.Position.X + 4, num3 + (i - ActivePosition) * Height, Width - 48 - 8, Height), (Rectangle?)null, Color.Wheat, 0f, Vector2.Zero, (SpriteEffects)0, 0.98f);
			}
			b.DrawString(Game1.smallFont, Labels[i], new Vector2(base.Position.X + 4f, (float)(num3 + (i - ActivePosition) * Height + 8)), Game1.textColor, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 1f);
		}
	}
}
