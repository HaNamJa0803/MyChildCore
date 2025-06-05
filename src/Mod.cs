using System;
using System.Collections.Generic;
using GenericModConfigMenu.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShared;
using SpaceShared.UI;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Menus;
using StardewValley.Triggers;
using xTile.Dimensions;

namespace GenericModConfigMenu;

internal class Mod : Mod
{
	public static Mod instance;

	private OwnModConfig Config;

	private RootElement? Ui;

	private Button ConfigButton;

	private int countdown = 5;

	internal readonly ModConfigManager ConfigManager = new ModConfigManager();

	private static HashSet<string> DidDeprecationWarningsFor = new HashSet<string>();

	private bool wasConfigMenu;

	public static IClickableMenu ActiveConfigMenu
	{
		get
		{
			if (Game1.activeClickableMenu is TitleMenu)
			{
				return TitleMenu.subMenu;
			}
			IClickableMenu val = Game1.activeClickableMenu;
			if (val == null)
			{
				return null;
			}
			while (val.GetChildMenu() != null)
			{
				val = val.GetChildMenu();
			}
			if ((!(val is ModConfigMenu) && !(val is SpecificModConfigMenu)) || 1 == 0)
			{
				return null;
			}
			return val;
		}
		set
		{
			if (Game1.activeClickableMenu is TitleMenu)
			{
				TitleMenu.subMenu = value;
			}
			else if (Game1.activeClickableMenu != null)
			{
				IClickableMenu val = Game1.activeClickableMenu;
				while (val.GetChildMenu() != null)
				{
					val = val.GetChildMenu();
				}
				if (value == null)
				{
					if (val.GetParentMenu() != null)
					{
						val.GetParentMenu().SetChildMenu((IClickableMenu)null);
					}
					else
					{
						Game1.activeClickableMenu = null;
					}
				}
				else
				{
					val.SetChildMenu(value);
				}
			}
			else
			{
				Game1.activeClickableMenu = value;
			}
		}
	}

	public override void Entry(IModHelper helper)
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		instance = this;
		I18n.Init(helper.Translation);
		Log.Monitor = ((Mod)this).Monitor;
		Config = helper.ReadConfig<OwnModConfig>();
		helper.Events.GameLoop.GameLaunched += OnGameLaunched;
		helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
		helper.Events.Display.WindowResized += OnWindowResized;
		helper.Events.Display.Rendered += OnRendered;
		helper.Events.Display.MenuChanged += OnMenuChanged;
		helper.Events.Input.MouseWheelScrolled += OnMouseWheelScrolled;
		helper.Events.Input.ButtonPressed += OnButtonPressed;
		helper.Events.Input.ButtonsChanged += OnButtonChanged;
		helper.Events.Content.AssetRequested += delegate(object? _, AssetRequestedEventArgs e)
		{
			AssetManager.Apply(e);
		};
		TriggerActionManager.RegisterAction("spacechase0.GenericModConfigMenu_OpenModConfig", (TriggerActionDelegate)delegate(string[] args, TriggerActionContext ctx, out string error)
		{
			if (args.Length < 2)
			{
				error = "Not enough arguments";
				return false;
			}
			if (!((Mod)this).Helper.ModRegistry.IsLoaded(args[1]))
			{
				error = "Mod " + args[1] + " not loaded.";
				return false;
			}
			IManifest manifest = ((Mod)this).Helper.ModRegistry.Get(args[1]).Manifest;
			if (ConfigManager.Get(manifest, assert: false) == null)
			{
				error = "Mod " + args[1] + " not registered with GMCM.";
				return false;
			}
			OpenModMenuNew(manifest, null, null);
			error = null;
			return true;
		});
	}

	public override object GetApi(IModInfo mod)
	{
		return new Api(mod.Manifest, ConfigManager, delegate(IManifest mod)
		{
			OpenModMenu(mod, null, null);
		}, delegate(IManifest mod)
		{
			OpenModMenuNew(mod, null, null);
		}, delegate(string s)
		{
			LogDeprecated(mod.Manifest.UniqueID, s);
		});
	}

	private void LogDeprecated(string modid, string str)
	{
		if (!DidDeprecationWarningsFor.Contains(modid))
		{
			DidDeprecationWarningsFor.Add(modid);
			Log.Info(str);
		}
	}

	private void OpenListMenuNew(int? scrollRow = null)
	{
		ActiveConfigMenu = (IClickableMenu)(object)new ModConfigMenu(Config.ScrollSpeed, delegate(IManifest mod, int curScrollRow)
		{
			OpenModMenuNew(mod, null, curScrollRow);
		}, delegate(int currScrollRow)
		{
			OpenKeybindingsMenuNew(currScrollRow);
		}, ConfigManager, ((Mod)this).Helper.GameContent.Load<Texture2D>(AssetManager.KeyboardButton), scrollRow);
	}

	private void OpenListMenu(int? scrollRow = null)
	{
		ModConfigMenu modConfigMenu = new ModConfigMenu(Config.ScrollSpeed, delegate(IManifest mod, int curScrollRow)
		{
			OpenModMenu(mod, null, curScrollRow);
		}, delegate(int currScrollRow)
		{
			OpenKeybindingsMenu(currScrollRow);
		}, ConfigManager, ((Mod)this).Helper.GameContent.Load<Texture2D>(AssetManager.KeyboardButton), scrollRow);
		if (Game1.activeClickableMenu is TitleMenu)
		{
			TitleMenu.subMenu = (IClickableMenu)(object)modConfigMenu;
		}
		else
		{
			Game1.activeClickableMenu = (IClickableMenu)(object)modConfigMenu;
		}
	}

	private void OpenKeybindingsMenuNew(int listScrollRow)
	{
		ActiveConfigMenu = (IClickableMenu)(object)new SpecificModConfigMenu(ConfigManager, Config.ScrollSpeed, delegate
		{
			if (Game1.activeClickableMenu is TitleMenu)
			{
				OpenListMenuNew(listScrollRow);
			}
			else
			{
				ActiveConfigMenu = null;
			}
		});
	}

	private void OpenKeybindingsMenu(int listScrollRow)
	{
		SpecificModConfigMenu specificModConfigMenu = new SpecificModConfigMenu(ConfigManager, Config.ScrollSpeed, delegate
		{
			OpenListMenu(listScrollRow);
		});
		if (Game1.activeClickableMenu is TitleMenu)
		{
			TitleMenu.subMenu = (IClickableMenu)(object)specificModConfigMenu;
		}
		else
		{
			Game1.activeClickableMenu = (IClickableMenu)(object)specificModConfigMenu;
		}
	}

	private void OpenModMenuNew(IManifest mod, string page, int? listScrollRow)
	{
		ModConfig config = ConfigManager.Get(mod, assert: true);
		ActiveConfigMenu = (IClickableMenu)(object)new SpecificModConfigMenu(config, Config.ScrollSpeed, page, delegate(string newPage)
		{
			if (!(Game1.activeClickableMenu is TitleMenu))
			{
				ActiveConfigMenu = null;
			}
			OpenModMenuNew(mod, newPage, listScrollRow);
		}, delegate
		{
			if (Game1.activeClickableMenu is TitleMenu)
			{
				OpenListMenuNew(listScrollRow);
			}
			else
			{
				ActiveConfigMenu = null;
			}
		});
	}

	private void OpenModMenu(IManifest mod, string page, int? listScrollRow)
	{
		ModConfig config = ConfigManager.Get(mod, assert: true);
		SpecificModConfigMenu specificModConfigMenu = new SpecificModConfigMenu(config, Config.ScrollSpeed, page, delegate(string newPage)
		{
			OpenModMenu(mod, newPage, listScrollRow);
		}, delegate
		{
			OpenListMenu(listScrollRow);
		});
		if (Game1.activeClickableMenu is TitleMenu)
		{
			TitleMenu.subMenu = (IClickableMenu)(object)specificModConfigMenu;
		}
		else
		{
			Game1.activeClickableMenu = (IClickableMenu)(object)specificModConfigMenu;
		}
	}

	private void SetupTitleMenuButton()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		if (Ui == null)
		{
			Ui = new RootElement();
			Texture2D tex = ((Mod)this).Helper.GameContent.Load<Texture2D>(AssetManager.ConfigButton);
			ConfigButton = new Button(tex)
			{
				LocalPosition = new Vector2(36f, (float)(((Rectangle)(ref Game1.viewport)).Height - 100)),
				Callback = delegate
				{
					Game1.playSound("newArtifact", (int?)null);
					OpenListMenuNew();
				}
			};
			Ui.AddChild(ConfigButton);
		}
		IClickableMenu activeClickableMenu = Game1.activeClickableMenu;
		TitleMenu val = (TitleMenu)(object)((activeClickableMenu is TitleMenu) ? activeClickableMenu : null);
		if (val != null && ((IClickableMenu)val).allClickableComponents?.Find((ClickableComponent cc) => cc != null && cc.myID == 509800) == null)
		{
			Texture2D val2 = ((Mod)this).Helper.GameContent.Load<Texture2D>(AssetManager.ConfigButton);
			ClickableComponent item = new ClickableComponent(new Rectangle(0, ((Rectangle)(ref Game1.viewport)).Height - 100, val2.Width / 2, val2.Height / 2), "GMCM")
			{
				myID = 509800,
				rightNeighborID = ((ClickableComponent)val.buttons[0]).myID
			};
			((IClickableMenu)val).allClickableComponents?.Add(item);
			((ClickableComponent)val.buttons[0]).leftNeighborID = 509800;
		}
	}

	private bool IsTitleMenuInteractable()
	{
		IClickableMenu activeClickableMenu = Game1.activeClickableMenu;
		TitleMenu val = (TitleMenu)(object)((activeClickableMenu is TitleMenu) ? activeClickableMenu : null);
		if (val == null || TitleMenu.subMenu != null)
		{
			return false;
		}
		IReflectedMethod method = ((Mod)this).Helper.Reflection.GetMethod((object)val, "ShouldAllowInteraction", false);
		if (method != null)
		{
			return method.Invoke<bool>(Array.Empty<object>());
		}
		return ((Mod)this).Helper.Reflection.GetField<bool>((object)val, "titleInPosition", true).GetValue();
	}

	private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
	{
		((Mod)this).Helper.Events.GameLoop.UpdateTicking += FiveTicksAfterGameLaunched;
		Api api = new Api(((Mod)this).ModManifest, ConfigManager, delegate(IManifest mod)
		{
			OpenModMenu(mod, null, null);
		}, delegate(IManifest mod)
		{
			OpenModMenuNew(mod, null, null);
		}, delegate(string s)
		{
			LogDeprecated(((Mod)this).ModManifest.UniqueID, s);
		});
		api.Register(((Mod)this).ModManifest, delegate
		{
			Config = new OwnModConfig();
		}, delegate
		{
			((Mod)this).Helper.WriteConfig<OwnModConfig>(Config);
		}, titleScreenOnly: false);
		api.AddNumberOption(((Mod)this).ModManifest, () => Config.ScrollSpeed, delegate(int value)
		{
			Config.ScrollSpeed = value;
		}, I18n.Options_ScrollSpeed_Name, I18n.Options_ScrollSpeed_Desc, 1, 500);
		api.AddKeybindList(((Mod)this).ModManifest, () => Config.OpenMenuKey, delegate(KeybindList value)
		{
			Config.OpenMenuKey = value;
		}, I18n.Options_OpenMenuKey_Name, I18n.Options_OpenMenuKey_Desc);
	}

	private void FiveTicksAfterGameLaunched(object sender, UpdateTickingEventArgs e)
	{
		if (countdown-- < 0)
		{
			SetupTitleMenuButton();
			((Mod)this).Helper.Events.GameLoop.UpdateTicking -= FiveTicksAfterGameLaunched;
		}
	}

	private void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
	{
		if (IsTitleMenuInteractable())
		{
			SetupTitleMenuButton();
			Ui?.Update();
		}
		if (wasConfigMenu && TitleMenu.subMenu == null)
		{
			IReflectedField<bool> field = ((Mod)this).Helper.Reflection.GetField<bool>((object)Game1.activeClickableMenu, "titleInPosition", true);
			if (!field.GetValue())
			{
				field.SetValue(true);
			}
		}
		wasConfigMenu = TitleMenu.subMenu is ModConfigMenu || TitleMenu.subMenu is SpecificModConfigMenu;
	}

	private void OnWindowResized(object sender, WindowResizedEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (ConfigButton != null)
		{
			ConfigButton.LocalPosition = new Vector2(ConfigButton.Position.X, (float)(((Rectangle)(ref Game1.viewport)).Height - 100));
		}
	}

	private void OnRendered(object sender, RenderedEventArgs e)
	{
		if (IsTitleMenuInteractable())
		{
			Ui?.Draw(e.SpriteBatch);
		}
	}

	private void OnMenuChanged(object sender, MenuChangedEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		IClickableMenu newMenu = e.NewMenu;
		GameMenu val = (GameMenu)(object)((newMenu is GameMenu) ? newMenu : null);
		if (val != null)
		{
			OptionsPage val2 = (OptionsPage)val.pages[GameMenu.optionsTab];
			val2.options.Add((OptionsElement)new OptionsButton(I18n.Button_ModOptions(), (Action)delegate
			{
				OpenListMenuNew();
			}));
		}
	}

	private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Keys val = default(Keys);
		if (Context.IsPlayerFree && Config.OpenMenuKey.JustPressed())
		{
			OpenListMenuNew();
		}
		else if (ActiveConfigMenu is SpecificModConfigMenu specificModConfigMenu && SButtonExtensions.TryGetKeyboard(e.Button, ref val))
		{
			((IClickableMenu)specificModConfigMenu).receiveKeyPress(val);
		}
	}

	private void OnButtonChanged(object sender, ButtonsChangedEventArgs e)
	{
		if (ActiveConfigMenu is SpecificModConfigMenu specificModConfigMenu)
		{
			specificModConfigMenu.OnButtonsChanged(e);
		}
	}

	private void OnMouseWheelScrolled(object sender, MouseWheelScrolledEventArgs e)
	{
		Dropdown.ActiveDropdown?.ReceiveScrollWheelAction(e.Delta);
	}
}
