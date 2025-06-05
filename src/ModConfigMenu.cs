using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShared;
using SpaceShared.UI;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;

namespace GenericModConfigMenu.Framework;

internal class ModConfigMenu : IClickableMenu
{
	private RootElement Ui;

	private readonly Table Table;

	private readonly int ScrollSpeed;

	private readonly Action<IManifest, int> OpenModMenu;

	private List<Label> LabelsWithTooltips = new List<Label>();

	private int scrollCounter;

	private bool InGame => Context.IsWorldReady;

	public int ScrollRow
	{
		get
		{
			return Table.Scrollbar.TopRow;
		}
		set
		{
			Table.Scrollbar.ScrollTo(value);
		}
	}

	public ModConfigMenu(int scrollSpeed, Action<IManifest, int> openModMenu, Action<int> openKeybindingsMenu, ModConfigManager configs, Texture2D keybindingsTex, int? scrollTo = null)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		ModConfigMenu modConfigMenu = this;
		ScrollSpeed = scrollSpeed;
		OpenModMenu = openModMenu;
		Ui = new RootElement();
		Table = new Table
		{
			RowHeight = 50,
			LocalPosition = new Vector2((float)((((Rectangle)(ref Game1.uiViewport)).Width - 800) / 2), 64f),
			Size = new Vector2(800f, (float)(((Rectangle)(ref Game1.uiViewport)).Height - 128))
		};
		Label label = new Label
		{
			String = I18n.List_EditableHeading(),
			Bold = true
		};
		label.LocalPosition = new Vector2((800f - label.Measure().X) / 2f, label.LocalPosition.Y);
		Table.AddRow(new Element[1] { label });
		ModConfig[] array = (from entry in configs.GetAll()
			where entry.AnyEditableInGame || !modConfigMenu.InGame
			orderby entry.ModName
			select entry).ToArray();
		ModConfig[] array2 = array;
		foreach (ModConfig entry2 in array2)
		{
			Label label2 = new Label
			{
				String = entry2.ModName,
				UserData = entry2.ModManifest.Description,
				Callback = delegate
				{
					modConfigMenu.ChangeToModPage(entry2.ModManifest);
				}
			};
			Table.AddRow(new Element[1] { label2 });
			LabelsWithTooltips.Add(label2);
		}
		ModConfig[] array3 = (from entry in configs.GetAll()
			where !entry.AnyEditableInGame && modConfigMenu.InGame
			orderby entry.ModName
			select entry).ToArray();
		if (array3.Any())
		{
			Label label3 = new Label
			{
				String = I18n.List_NotEditableHeading(),
				Bold = true
			};
			Table.AddRow(Array.Empty<Element>());
			Table.AddRow(new Element[1] { label3 });
			ModConfig[] array4 = array3;
			foreach (ModConfig modConfig in array4)
			{
				Label label4 = new Label
				{
					String = modConfig.ModName,
					UserData = modConfig.ModManifest.Description,
					IdleTextColor = Color.Black * 0.4f,
					HoverTextColor = Color.Black * 0.4f
				};
				Table.AddRow(new Element[1] { label4 });
				LabelsWithTooltips.Add(label4);
			}
		}
		Ui.AddChild(Table);
		Button element = new Button(keybindingsTex)
		{
			LocalPosition = Table.LocalPosition - new Vector2((float)(keybindingsTex.Width / 2 + 32), 0f),
			Callback = delegate
			{
				openKeybindingsMenu(modConfigMenu.ScrollRow);
			}
		};
		Ui.AddChild(element);
		if ((int)Constants.TargetPlatform == 0)
		{
			((IClickableMenu)this).initializeUpperRightCloseButton();
		}
		else
		{
			base.upperRightCloseButton = null;
		}
		if (scrollTo.HasValue)
		{
			ScrollRow = scrollTo.Value;
		}
		if (!InGame)
		{
			((Mod)Mod.instance).Helper.Reflection.GetField<bool>((object)Game1.activeClickableMenu, "titleInPosition", true).SetValue(false);
		}
	}

	public override void receiveLeftClick(int x, int y, bool playSound = true)
	{
		ClickableTextureComponent upperRightCloseButton = base.upperRightCloseButton;
		if (upperRightCloseButton != null && ((ClickableComponent)upperRightCloseButton).containsPoint(x, y) && ((IClickableMenu)this).readyToClose())
		{
			if (playSound)
			{
				Game1.playSound("bigDeSelect", (int?)null);
			}
			Mod.ActiveConfigMenu = null;
		}
	}

	public override void receiveScrollWheelAction(int direction)
	{
		Table.Scrollbar.ScrollBy(direction / -ScrollSpeed);
	}

	public override void update(GameTime time)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		((IClickableMenu)this).update(time);
		Ui.Update();
		GamePadState gamePadState = Game1.input.GetGamePadState();
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref gamePadState)).ThumbSticks;
		if (((GamePadThumbSticks)(ref thumbSticks)).Right.Y != 0f)
		{
			if (++scrollCounter == 5)
			{
				scrollCounter = 0;
				Scrollbar scrollbar = Table.Scrollbar;
				gamePadState = Game1.input.GetGamePadState();
				thumbSticks = ((GamePadState)(ref gamePadState)).ThumbSticks;
				scrollbar.ScrollBy(Math.Sign(((GamePadThumbSticks)(ref thumbSticks)).Right.Y) * 120 / -ScrollSpeed);
			}
		}
		else
		{
			scrollCounter = 0;
		}
	}

	public override void draw(SpriteBatch b)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		((IClickableMenu)this).draw(b);
		b.Draw(Game1.staminaRect, new Rectangle(0, 0, ((Rectangle)(ref Game1.uiViewport)).Width, ((Rectangle)(ref Game1.uiViewport)).Height), new Color(0, 0, 0, 192));
		Ui.Draw(b);
		ClickableTextureComponent upperRightCloseButton = base.upperRightCloseButton;
		if (upperRightCloseButton != null)
		{
			upperRightCloseButton.draw(b);
		}
		if (InGame)
		{
			((IClickableMenu)this).drawMouse(b, false, -1);
		}
		if ((int)Constants.TargetPlatform == 0 || ((IClickableMenu)this).GetChildMenu() != null)
		{
			return;
		}
		foreach (Label labelsWithTooltip in LabelsWithTooltips)
		{
			if (labelsWithTooltip.Hover && labelsWithTooltip.UserData != null)
			{
				string text = (string)labelsWithTooltip.UserData;
				if (text != null && !text.Contains("\n"))
				{
					text = Game1.parseText(text, Game1.smallFont, 800);
				}
				string text2 = labelsWithTooltip.String;
				if (text2 != null && !text2.Contains("\n"))
				{
					text2 = Game1.parseText(text2, Game1.dialogueFont, 800);
				}
				IClickableMenu.drawToolTip(b, text, text2, (Item)null, false, -1, 0, (string)null, -1, (CraftingRecipe)null, -1, (IList<Item>)null);
			}
		}
	}

	public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		RootElement ui = Ui;
		Ui = new RootElement();
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(800f, (float)(((Rectangle)(ref Game1.uiViewport)).Height - 128));
		Table.LocalPosition = new Vector2((float)((((Rectangle)(ref Game1.uiViewport)).Width - 800) / 2), 64f);
		Element[] children = Table.Children;
		foreach (Element element in children)
		{
			element.LocalPosition = new Vector2(val.X / (Table.Size.X / element.LocalPosition.X), element.LocalPosition.Y);
		}
		Table.Size = val;
		Table.Scrollbar.Update();
		Ui.AddChild(Table);
		Element element2 = ui.Children.First((Element e) => e is Button);
		ui.RemoveChild(element2);
		Ui.AddChild(element2);
	}

	public override bool overrideSnappyMenuCursorMovementBan()
	{
		return true;
	}

	private void ChangeToModPage(IManifest modManifest)
	{
		Log.Trace("Changing to mod config page for mod " + modManifest.UniqueID);
		Game1.playSound("bigSelect", (int?)null);
		OpenModMenu(modManifest, ScrollRow);
	}
}
