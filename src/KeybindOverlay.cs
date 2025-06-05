using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GenericModConfigMenu.Framework.ModOption;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShared.UI;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;

namespace GenericModConfigMenu.Framework.Overlays;

internal class KeybindOverlay<TKeybind> : IKeybindOverlay
{
	private readonly SimpleModOption<TKeybind> Option;

	private readonly Label Label;

	private Rectangle Bounds;

	private ClickableTextureComponent OkButton;

	private ClickableTextureComponent ClearButton;

	private bool ShouldResetLayout;

	private bool HasPressedButton;

	private List<SButton> PressedButtons;

	public bool IsFinished { get; private set; }

	public KeybindOverlay(SimpleModOption<TKeybind> option, Label label)
	{
		Option = option;
		Label = label;
		ShouldResetLayout = true;
		HasPressedButton = false;
		PressedButtons = new List<SButton>();
	}

	public void OnButtonsChanged(ButtonsChangedEventArgs e)
	{
		if (e.Pressed.Any())
		{
			HasPressedButton = true;
			PressedButtons.AddRange(e.Pressed.Where((SButton b) => IsValidKey(b) && !PressedButtons.Contains(b)));
		}
		if (HasPressedButton)
		{
			SButton[] source = e.Released.Where(IsValidKey).ToArray();
			if (source.Any())
			{
				HandleButtons(PressedButtons.ToArray());
				IsFinished = true;
			}
		}
	}

	public void OnWindowResized()
	{
		ShouldResetLayout = true;
	}

	public void OnLeftClick(int x, int y)
	{
		if (!ShouldResetLayout)
		{
			if (((ClickableComponent)ClearButton).containsPoint(x, y))
			{
				Game1.playSound("coin", (int?)null);
				SetValue(Array.Empty<SButton>());
				IsFinished = true;
			}
			else if (((ClickableComponent)OkButton).containsPoint(x, y) || !((Rectangle)(ref Bounds)).Contains(x, y))
			{
				Game1.playSound("bigDeSelect", (int?)null);
				IsFinished = true;
			}
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		if (ShouldResetLayout)
		{
			ResetLayout();
			ShouldResetLayout = false;
		}
		spriteBatch.Draw(Game1.staminaRect, new Rectangle(0, 0, ((Rectangle)(ref Game1.uiViewport)).Width, ((Rectangle)(ref Game1.uiViewport)).Height), new Color(0, 0, 0, 192));
		IClickableMenu.drawTextureBox(spriteBatch, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, Color.White);
		string text = I18n.Config_RebindKey_Title(Option.Name());
		int num = (int)Game1.dialogueFont.MeasureString(text).X;
		spriteBatch.DrawString(Game1.dialogueFont, text, new Vector2((float)(((Rectangle)(ref Bounds)).Center.X - num / 2), (float)(Bounds.Y + 20)), Game1.textColor);
		string text2;
		if (!PressedButtons.Any())
		{
			text2 = ((typeof(TKeybind) == typeof(KeybindList)) ? I18n.Config_RebindKey_ComboInstructions() : I18n.Config_RebindKey_SimpleInstructions());
		}
		else
		{
			object obj;
			if (!(typeof(TKeybind) == typeof(KeybindList)))
			{
				SButton val = PressedButtons.Last();
				obj = ((object)(SButton)(ref val)).ToString() ?? "";
			}
			else
			{
				obj = string.Join(" + ", PressedButtons);
			}
			text2 = (string)obj;
		}
		int num2 = (int)Game1.dialogueFont.MeasureString(text2).X;
		spriteBatch.DrawString(Game1.dialogueFont, text2, new Vector2((float)(((Rectangle)(ref Bounds)).Center.X - num2 / 2), (float)(Bounds.Y + 100)), Game1.textColor);
		OkButton.draw(spriteBatch);
		ClearButton.draw(spriteBatch);
	}

	private void ResetLayout()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		Vector2 topLeftPositionForCenteringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(650, 200, 0, 0);
		int num = (int)topLeftPositionForCenteringOnScreen.X;
		int num2 = (int)topLeftPositionForCenteringOnScreen.Y;
		int num3 = 650;
		int num4 = 200;
		Bounds = new Rectangle(num, num2, num3, num4);
		OkButton = new ClickableTextureComponent("OK", new Rectangle(num + num3 - 128, num2 + num4, 64, 64), (string)null, (string)null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
		ClearButton = new ClickableTextureComponent("Cancel", new Rectangle(num + num3 - 64, num2 + num4, 64, 64), (string)null, (string)null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
	}

	private bool IsValidKey(SButton button)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		SButton[] array = new SButton[9];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		SButton[] source = (SButton[])(object)array;
		return !source.Contains(button);
	}

	private void HandleButtons(SButton[] buttons)
	{
		if (buttons.Any((SButton p) => (int)p == 27))
		{
			Game1.playSound("bigDeSelect", (int?)null);
			return;
		}
		Game1.playSound("coin", (int?)null);
		SetValue(buttons);
	}

	private void SetValue(SButton[] buttons)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		SimpleModOption<TKeybind> option = Option;
		if (!(option is SimpleModOption<SButton> simpleModOption))
		{
			if (!(option is SimpleModOption<KeybindList> simpleModOption2))
			{
				throw new InvalidOperationException("Unsupported keybind type " + typeof(TKeybind).FullName + ".");
			}
			simpleModOption2.Value = new KeybindList((Keybind[])(object)new Keybind[1]
			{
				new Keybind(buttons.ToArray())
			});
			Label.String = simpleModOption2.FormatValue();
		}
		else
		{
			simpleModOption.Value = (SButton)(buttons.Any() ? ((int)buttons.Last()) : 0);
			Label.String = simpleModOption.FormatValue();
		}
	}
}
