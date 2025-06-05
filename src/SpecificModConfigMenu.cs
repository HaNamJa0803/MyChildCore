using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericModConfigMenu.Framework.ModOption;
using GenericModConfigMenu.Framework.Overlays;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShared.UI;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;

namespace GenericModConfigMenu.Framework;

internal class SpecificModConfigMenu : IClickableMenu
{
	private const int MinimumButtonGap = 32;

	private readonly Action<string> OpenPage;

	private readonly Action ReturnToList;

	private readonly ModConfig ModConfig;

	private readonly int ScrollSpeed;

	private RootElement Ui = new RootElement();

	private readonly Table Table;

	private readonly List<Label> OptHovers = new List<Label>();

	private bool ExitOnNextUpdate;

	private IKeybindOverlay ActiveKeybindOverlay;

	private int TitleLabelWidth;

	private ModConfigManager ConfigsForKeybinds;

	private List<Label> keybindOpts = new List<Label>();

	public readonly string CurrPage;

	private int scrollCounter;

	private bool IsSubPage => !string.IsNullOrEmpty(CurrPage);

	private bool InGame => Context.IsWorldReady;

	private bool IsBindingKey => ActiveKeybindOverlay != null;

	public IManifest Manifest => ModConfig?.ModManifest;

	public bool IsKeybindingsPage => Manifest == null;

	public SpecificModConfigMenu(ModConfigManager mods, int scrollSpeed, Action returnToList)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		ConfigsForKeybinds = mods;
		ScrollSpeed = scrollSpeed;
		ReturnToList = returnToList;
		Table = new Table(fixedRowHeight: false)
		{
			RowHeight = 50,
			Size = new Vector2((float)Math.Min(1200, ((Rectangle)(ref Game1.uiViewport)).Width - 200), (float)(((Rectangle)(ref Game1.uiViewport)).Height - 128 - 116))
		};
		Table.LocalPosition = new Vector2(((float)((Rectangle)(ref Game1.uiViewport)).Width - Table.Size.X) / 2f, ((float)((Rectangle)(ref Game1.uiViewport)).Height - Table.Size.Y) / 2f);
		foreach (ModConfig item in mods.GetAll())
		{
			List<Element[]> list = new List<Element[]>();
			foreach (BaseModOption allOption in item.GetAllOptions())
			{
				if (!(allOption is SimpleModOption<SButton>) && !(allOption is SimpleModOption<KeybindList>))
				{
					continue;
				}
				string @string = allOption.Name();
				string text = allOption.Tooltip();
				if (InGame && allOption.IsTitleScreenOnly)
				{
					continue;
				}
				allOption.BeforeMenuOpened();
				Label label = new Label
				{
					String = @string,
					UserData = text
				};
				if (!string.IsNullOrEmpty(text))
				{
					OptHovers.Add(label);
				}
				Element element = new Label
				{
					String = "TODO",
					LocalPosition = new Vector2(500f, 0f)
				};
				Label label2 = null;
				SimpleModOption<SButton> simpleModOption = allOption as SimpleModOption<SButton>;
				if (simpleModOption == null)
				{
					SimpleModOption<KeybindList> simpleModOption2 = allOption as SimpleModOption<KeybindList>;
					if (simpleModOption2 != null)
					{
						if ((int)Constants.TargetPlatform == 0)
						{
							continue;
						}
						element = new Label
						{
							String = simpleModOption2.FormatValue(),
							LocalPosition = new Vector2(Table.Size.X / 5f * 4f, 0f),
							Callback = delegate(Element e)
							{
								ShowKeybindOverlay(simpleModOption2, e as Label);
							},
							UserData = allOption
						};
					}
				}
				else
				{
					if ((int)Constants.TargetPlatform == 0)
					{
						continue;
					}
					element = new Label
					{
						String = simpleModOption.FormatValue(),
						LocalPosition = new Vector2(Table.Size.X / 5f * 4f, 0f),
						Callback = delegate(Element e)
						{
							ShowKeybindOverlay(simpleModOption, e as Label);
						},
						UserData = allOption
					};
				}
				keybindOpts.Add(element as Label);
				list.Add(new Element[3] { label, element, label2 }.Where((Element p) => p != null).ToArray());
			}
			if (list.Count <= 0)
			{
				continue;
			}
			Label label3 = new Label
			{
				String = item.ModName,
				UserData = item.ModManifest.Description,
				Bold = true
			};
			if (!string.IsNullOrEmpty(item.ModManifest.Description))
			{
				OptHovers.Add(label3);
			}
			Table.AddRow(new Element[1] { label3 });
			foreach (Element[] item2 in list)
			{
				Table.AddRow(item2);
			}
			Table.AddRow(Array.Empty<Element>());
		}
		Ui.AddChild(Table);
		AddDefaultLabels(null);
		Table.ForceUpdateEvenHidden();
		RefreshKeybindingColor();
	}

	public SpecificModConfigMenu(ModConfig config, int scrollSpeed, string page, Action<string> openPage, Action returnToList)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		ModConfig = config;
		ScrollSpeed = scrollSpeed;
		OpenPage = openPage;
		ReturnToList = returnToList;
		CurrPage = page ?? "";
		ModConfig.ActiveDisplayPage = ModConfig.Pages[CurrPage];
		Table = new Table(fixedRowHeight: false)
		{
			RowHeight = 50,
			Size = new Vector2((float)Math.Min(1200, ((Rectangle)(ref Game1.uiViewport)).Width - 200), (float)(((Rectangle)(ref Game1.uiViewport)).Height - 128 - 116))
		};
		Table.LocalPosition = new Vector2(((float)((Rectangle)(ref Game1.uiViewport)).Width - Table.Size.X) / 2f, ((float)((Rectangle)(ref Game1.uiViewport)).Height - Table.Size.Y) / 2f);
		Vector2 val2 = default(Vector2);
		Vector2 localPosition = default(Vector2);
		foreach (BaseModOption option in ModConfig.Pages[CurrPage].Options)
		{
			string text = option.Name();
			string text2 = option.Tooltip();
			if (InGame && option.IsTitleScreenOnly)
			{
				continue;
			}
			option.BeforeMenuOpened();
			Label label = new Label
			{
				String = text,
				UserData = text2
			};
			if (!string.IsNullOrEmpty(text2))
			{
				OptHovers.Add(label);
			}
			Element element = new Label
			{
				String = "TODO",
				LocalPosition = new Vector2(500f, 0f)
			};
			Label rightLabel = null;
			BaseModOption baseModOption = option;
			SimpleModOption<int> simpleModOption4;
			SimpleModOption<float> simpleModOption5;
			if (!(baseModOption is ComplexModOption modOption))
			{
				SimpleModOption<bool> simpleModOption = baseModOption as SimpleModOption<bool>;
				if (simpleModOption == null)
				{
					SimpleModOption<SButton> simpleModOption2 = baseModOption as SimpleModOption<SButton>;
					if (simpleModOption2 == null)
					{
						SimpleModOption<KeybindList> simpleModOption3 = baseModOption as SimpleModOption<KeybindList>;
						if (simpleModOption3 == null)
						{
							NumericModOption<int> numericModOption = baseModOption as NumericModOption<int>;
							if (numericModOption == null)
							{
								NumericModOption<float> numericModOption2 = baseModOption as NumericModOption<float>;
								if (numericModOption2 == null)
								{
									ChoiceModOption<string> choiceModOption = baseModOption as ChoiceModOption<string>;
									if (choiceModOption == null)
									{
										simpleModOption4 = baseModOption as SimpleModOption<int>;
										if (simpleModOption4 != null)
										{
											goto IL_07af;
										}
										simpleModOption5 = baseModOption as SimpleModOption<float>;
										if (simpleModOption5 != null)
										{
											goto IL_0814;
										}
										SimpleModOption<string> simpleModOption6 = baseModOption as SimpleModOption<string>;
										if (simpleModOption6 == null)
										{
											if (!(baseModOption is SectionTitleModOption))
											{
												PageLinkModOption pageLinkModOption = baseModOption as PageLinkModOption;
												if (pageLinkModOption == null)
												{
													if (!(baseModOption is ParagraphModOption))
													{
														if (baseModOption is ImageModOption imageModOption)
														{
															Texture2D val = imageModOption.Texture();
															((Vector2)(ref val2))._002Ector((float)val.Width, (float)val.Height);
															if (imageModOption.TexturePixelArea.HasValue)
															{
																((Vector2)(ref val2))._002Ector((float)imageModOption.TexturePixelArea.Value.Width, (float)imageModOption.TexturePixelArea.Value.Height);
															}
															val2 *= (float)imageModOption.Scale;
															((Vector2)(ref localPosition))._002Ector(Table.Size.X / 2f - val2.X / 2f, 0f);
															element = new Image
															{
																Texture = val,
																TexturePixelArea = (Rectangle)(((_003F?)imageModOption.TexturePixelArea) ?? new Rectangle(0, 0, (int)val2.X, (int)val2.Y)),
																Scale = imageModOption.Scale,
																LocalPosition = localPosition
															};
														}
													}
													else
													{
														label = null;
														element = null;
														StringBuilder stringBuilder = new StringBuilder(text.Length + 50);
														string text3 = "";
														string[] array = text.Split(' ');
														foreach (string text4 in array)
														{
															if (text4 == "\n")
															{
																stringBuilder.AppendLine(text3);
																text3 = "";
																continue;
															}
															if (text3 == "")
															{
																text3 = text4;
																continue;
															}
															string text5 = (text3 + " " + text4).Trim();
															if (Label.MeasureString(text5, bold: false, 1f, Game1.smallFont).X <= Table.Size.X)
															{
																text3 = text5;
																continue;
															}
															stringBuilder.AppendLine(text3);
															text3 = text4;
														}
														if (text3 != "")
														{
															stringBuilder.AppendLine(text3);
														}
														label = null;
														element = new Label
														{
															UserData = text2,
															NonBoldScale = 1f,
															NonBoldShadow = false,
															Font = Game1.smallFont,
															String = stringBuilder.ToString()
														};
													}
												}
												else
												{
													label.Bold = true;
													label.Callback = delegate
													{
														OpenPage(pageLinkModOption.PageId);
													};
													element = null;
												}
											}
											else
											{
												label.LocalPosition = new Vector2(-8f, 0f);
												label.Bold = true;
												if (text == "")
												{
													label = null;
												}
												element = null;
											}
										}
										else
										{
											if ((int)Constants.TargetPlatform == 0)
											{
												continue;
											}
											element = new Textbox
											{
												LocalPosition = new Vector2(Table.Size.X / 2f - 8f, 0f),
												String = simpleModOption6.Value,
												Callback = delegate(Element e)
												{
													simpleModOption6.Value = (e as Textbox).String;
												}
											};
										}
									}
									else
									{
										element = new Dropdown
										{
											Choices = choiceModOption.Choices,
											Labels = choiceModOption.Choices.Select((string value) => choiceModOption.FormatChoice?.Invoke(value) ?? value).ToArray(),
											LocalPosition = new Vector2(Table.Size.X / 2f, 0f),
											RequestWidth = (int)Table.Size.X / 2,
											Value = choiceModOption.Value,
											MaxValuesAtOnce = Math.Min(choiceModOption.Choices.Length, 5),
											Callback = delegate(Element e)
											{
												choiceModOption.Value = (e as Dropdown).Value;
											}
										};
									}
								}
								else
								{
									if (!numericModOption2.Minimum.HasValue || !numericModOption2.Maximum.HasValue)
									{
										simpleModOption5 = (SimpleModOption<float>)baseModOption;
										goto IL_0814;
									}
									rightLabel = new Label
									{
										String = numericModOption2.FormatValue()
									};
									element = new Slider<float>
									{
										LocalPosition = new Vector2(Table.Size.X / 2f, 0f),
										RequestWidth = (int)Table.Size.X / 3,
										Value = numericModOption2.Value,
										Minimum = numericModOption2.Minimum.Value,
										Maximum = numericModOption2.Maximum.Value,
										Interval = numericModOption2.Interval.GetValueOrDefault(0.01f),
										Callback = delegate(Element e)
										{
											numericModOption2.Value = (e as Slider<float>).Value;
											rightLabel.String = numericModOption2.FormatValue();
										}
									};
									rightLabel.LocalPosition = element.LocalPosition + new Vector2((float)(element.Width + 15), 0f);
								}
							}
							else
							{
								if (!numericModOption.Minimum.HasValue || !numericModOption.Maximum.HasValue)
								{
									simpleModOption4 = (SimpleModOption<int>)baseModOption;
									goto IL_07af;
								}
								rightLabel = new Label
								{
									String = numericModOption.FormatValue()
								};
								element = new Slider<int>
								{
									LocalPosition = new Vector2(Table.Size.X / 2f, 0f),
									RequestWidth = (int)Table.Size.X / 3,
									Value = numericModOption.Value,
									Minimum = numericModOption.Minimum.Value,
									Maximum = numericModOption.Maximum.Value,
									Interval = numericModOption.Interval.GetValueOrDefault(1),
									Callback = delegate(Element e)
									{
										numericModOption.Value = (e as Slider<int>).Value;
										rightLabel.String = numericModOption.FormatValue();
									}
								};
								rightLabel.LocalPosition = element.LocalPosition + new Vector2((float)(element.Width + 15), 0f);
							}
						}
						else
						{
							if ((int)Constants.TargetPlatform == 0)
							{
								continue;
							}
							element = new Label
							{
								String = simpleModOption3.FormatValue(),
								LocalPosition = new Vector2(Table.Size.X / 2f, 0f),
								Callback = delegate(Element e)
								{
									ShowKeybindOverlay(simpleModOption3, e as Label);
								}
							};
						}
					}
					else
					{
						if ((int)Constants.TargetPlatform == 0)
						{
							continue;
						}
						element = new Label
						{
							String = simpleModOption2.FormatValue(),
							LocalPosition = new Vector2(Table.Size.X / 2f, 0f),
							Callback = delegate(Element e)
							{
								ShowKeybindOverlay(simpleModOption2, e as Label);
							}
						};
					}
				}
				else
				{
					element = new Checkbox
					{
						LocalPosition = new Vector2(Table.Size.X / 2f, 0f),
						Checked = simpleModOption.Value,
						Callback = delegate(Element e)
						{
							simpleModOption.Value = (e as Checkbox).Checked;
						}
					};
				}
			}
			else
			{
				element = new ComplexModOptionWidget(modOption)
				{
					LocalPosition = new Vector2(Table.Size.X / 2f, 0f)
				};
			}
			goto IL_0b6b;
			IL_07af:
			if ((int)Constants.TargetPlatform == 0)
			{
				continue;
			}
			element = new Intbox
			{
				LocalPosition = new Vector2(Table.Size.X / 2f - 8f, 0f),
				Value = simpleModOption4.Value,
				Callback = delegate(Element e)
				{
					simpleModOption4.Value = (e as Intbox).Value;
				}
			};
			goto IL_0b6b;
			IL_0b6b:
			Table.AddRow(new Element[3] { label, element, rightLabel }.Where((Element p) => p != null).ToArray());
			continue;
			IL_0814:
			if ((int)Constants.TargetPlatform == 0)
			{
				continue;
			}
			element = new Floatbox
			{
				LocalPosition = new Vector2(Table.Size.X / 2f - 8f, 0f),
				Value = simpleModOption5.Value,
				Callback = delegate(Element e)
				{
					simpleModOption5.Value = (e as Floatbox).Value;
				}
			};
			goto IL_0b6b;
		}
		Ui.AddChild(Table);
		AddDefaultLabels(Manifest);
		Table.ForceUpdateEvenHidden();
	}

	public override void receiveLeftClick(int x, int y, bool playSound = true)
	{
		if (IsBindingKey)
		{
			ActiveKeybindOverlay.OnLeftClick(x, y);
			if (ActiveKeybindOverlay.IsFinished)
			{
				CloseKeybindOverlay();
			}
		}
	}

	public override void receiveKeyPress(Keys key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		if ((int)key == 27 && !IsBindingKey)
		{
			ExitOnNextUpdate = true;
		}
	}

	public override void receiveScrollWheelAction(int direction)
	{
		if (Dropdown.ActiveDropdown == null)
		{
			Table.Scrollbar.ScrollBy(direction / -ScrollSpeed);
		}
	}

	public override bool readyToClose()
	{
		return false;
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
		if (ExitOnNextUpdate)
		{
			Cancel();
		}
	}

	public override void draw(SpriteBatch b)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		((IClickableMenu)this).draw(b);
		b.Draw(Game1.staminaRect, new Rectangle(0, 0, ((Rectangle)(ref Game1.uiViewport)).Width, ((Rectangle)(ref Game1.uiViewport)).Height), new Color(0, 0, 0, 192));
		int num = Math.Clamp(TitleLabelWidth + 64, 864, ((Rectangle)(ref Game1.uiViewport)).Width);
		IClickableMenu.drawTextureBox(b, (((Rectangle)(ref Game1.uiViewport)).Width - num) / 2, 32, num, 70, Color.White);
		IClickableMenu.drawTextureBox(b, (((Rectangle)(ref Game1.uiViewport)).Width - 800) / 2 - 32 - 64, ((Rectangle)(ref Game1.uiViewport)).Height - 50 - 20 - 32, 992, 70, Color.White);
		Ui.Draw(b);
		ActiveKeybindOverlay?.Draw(b);
		((IClickableMenu)this).drawMouse(b, false, -1);
		if ((int)Constants.TargetPlatform == 0 || ((IClickableMenu)this).GetChildMenu() != null)
		{
			return;
		}
		foreach (Label optHover in OptHovers)
		{
			if (optHover.Hover)
			{
				string text = (string)optHover.UserData;
				if (text != null && !text.Contains("\n"))
				{
					text = Game1.parseText(text, Game1.smallFont, 800);
				}
				string text2 = optHover.String;
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
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		Ui = new RootElement();
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector((float)Math.Min(1200, ((Rectangle)(ref Game1.uiViewport)).Width - 200), (float)(((Rectangle)(ref Game1.uiViewport)).Height - 128 - 116));
		Element[] children = Table.Children;
		foreach (Element element in children)
		{
			element.LocalPosition = new Vector2(val.X / (Table.Size.X / element.LocalPosition.X), element.LocalPosition.Y);
			if (element is Slider slider)
			{
				slider.RequestWidth = (int)(val.X / (Table.Size.X / (float)slider.Width));
			}
		}
		Table.Size = val;
		Table.LocalPosition = new Vector2(((float)((Rectangle)(ref Game1.uiViewport)).Width - Table.Size.X) / 2f, ((float)((Rectangle)(ref Game1.uiViewport)).Height - Table.Size.Y) / 2f);
		Table.Scrollbar.Update();
		Ui.AddChild(Table);
		AddDefaultLabels(Manifest);
		ActiveKeybindOverlay?.OnWindowResized();
	}

	public override bool overrideSnappyMenuCursorMovementBan()
	{
		return true;
	}

	public void OnButtonsChanged(ButtonsChangedEventArgs e)
	{
		if (IsBindingKey)
		{
			ActiveKeybindOverlay.OnButtonsChanged(e);
			if (ActiveKeybindOverlay.IsFinished)
			{
				CloseKeybindOverlay();
			}
		}
	}

	private void AddDefaultLabels(IManifest modManifest)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		string text = ((modManifest == null) ? "" : ModConfig.Pages[CurrPage].PageTitle());
		Label label = new Label
		{
			String = ((modManifest == null) ? I18n.List_Keybindings() : (modManifest.Name + ((text == "") ? "" : (" > " + text)))),
			Bold = true
		};
		label.LocalPosition = new Vector2(((float)((Rectangle)(ref Game1.uiViewport)).Width - label.Measure().X) / 2f, 44f);
		label.HoverTextColor = label.IdleTextColor;
		Ui.AddChild(label);
		TitleLabelWidth = (int)label.Measure().X;
		Vector2 localPosition = default(Vector2);
		((Vector2)(ref localPosition))._002Ector((float)(((Rectangle)(ref Game1.uiViewport)).Width / 2 - 450), (float)(((Rectangle)(ref Game1.uiViewport)).Height - 50 - 36));
		Label label2 = new Label
		{
			String = I18n.Config_Buttons_Cancel(),
			Bold = true,
			LocalPosition = localPosition,
			Callback = delegate
			{
				Cancel();
			}
		};
		Label label3 = new Label
		{
			String = I18n.Config_Buttons_ResetToDefault(),
			Bold = true,
			LocalPosition = localPosition,
			Callback = delegate
			{
				ResetConfig();
			},
			ForceHide = () => IsSubPage || modManifest == null
		};
		Label label4 = new Label
		{
			String = I18n.Config_Buttons_Save(),
			Bold = true,
			LocalPosition = localPosition,
			Callback = delegate
			{
				SaveConfig();
			}
		};
		Label label5 = new Label
		{
			String = I18n.Config_Buttons_SaveAndClose(),
			Bold = true,
			LocalPosition = localPosition,
			Callback = delegate
			{
				SaveConfig();
				Close();
			}
		};
		Label[] array = new Label[4] { label2, label3, label4, label5 };
		int[] source = array.Select((Label p) => p.Width).ToArray();
		int num = source.Sum();
		int num2 = 0;
		int num3 = (914 - num) / (array.Length - 1);
		if (num3 < 32)
		{
			num2 = -((32 - num3) / 2) * (array.Length - 1);
			num3 = 32;
		}
		for (int i = 0; i < array.Length; i++)
		{
			Label obj = array[i];
			obj.LocalPosition += new Vector2((float)(num2 + source.Take(i).Sum() + num3 * i), 0f);
		}
		Label[] array2 = array;
		foreach (Label element in array2)
		{
			Ui.AddChild(element);
		}
	}

	private void ResetConfig()
	{
		Game1.playSound("backpackIN", (int?)null);
		foreach (BaseModOption allOption in ModConfig.GetAllOptions())
		{
			allOption.BeforeReset();
		}
		ModConfig.Reset();
		foreach (BaseModOption allOption2 in ModConfig.GetAllOptions())
		{
			allOption2.AfterReset();
		}
		SaveConfig(playSound: false);
		OpenPage(CurrPage);
	}

	private void SaveConfig(bool playSound = true)
	{
		if (playSound)
		{
			Game1.playSound("money", (int?)null);
		}
		if (ModConfig != null)
		{
			foreach (BaseModOption allOption in ModConfig.GetAllOptions())
			{
				allOption.BeforeSave();
			}
			ModConfig.Save();
			{
				foreach (BaseModOption allOption2 in ModConfig.GetAllOptions())
				{
					allOption2.AfterSave();
				}
				return;
			}
		}
		ModConfig[] array = ConfigsForKeybinds.GetAll().ToArray();
		foreach (ModConfig modConfig in array)
		{
			bool flag = false;
			foreach (BaseModOption allOption3 in modConfig.GetAllOptions())
			{
				if (allOption3 is SimpleModOption<SButton> || allOption3 is SimpleModOption<KeybindList>)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				continue;
			}
			foreach (BaseModOption allOption4 in modConfig.GetAllOptions())
			{
				allOption4.BeforeSave();
			}
			modConfig.Save();
			foreach (BaseModOption allOption5 in modConfig.GetAllOptions())
			{
				allOption5.AfterSave();
			}
		}
	}

	private void Close()
	{
		if (ModConfig != null)
		{
			foreach (BaseModOption option in ModConfig.ActiveDisplayPage.Options)
			{
				option.BeforeMenuClosed();
			}
		}
		else
		{
			foreach (ModConfig item in ConfigsForKeybinds.GetAll())
			{
				foreach (BaseModOption allOption in item.GetAllOptions())
				{
					if (allOption is SimpleModOption<SButton> || allOption is SimpleModOption<KeybindList>)
					{
						allOption.BeforeMenuClosed();
					}
				}
			}
		}
		if (IsSubPage)
		{
			OpenPage(null);
		}
		else
		{
			ReturnToList();
		}
	}

	private void Cancel()
	{
		Game1.playSound("bigDeSelect", (int?)null);
		Close();
	}

	private void ShowKeybindOverlay<TKeybind>(SimpleModOption<TKeybind> option, Label label)
	{
		Game1.playSound("breathin", (int?)null);
		ActiveKeybindOverlay = new KeybindOverlay<TKeybind>(option, label);
		Ui.Obscured = true;
	}

	private void CloseKeybindOverlay()
	{
		ActiveKeybindOverlay = null;
		Ui.Obscured = false;
		RefreshKeybindingColor();
	}

	private void RefreshKeybindingColor()
	{
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		if (!IsKeybindingsPage)
		{
			return;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (Label keybindOpt in keybindOpts)
		{
			if (keybindOpt.UserData is SimpleModOption<Keybind> simpleModOption)
			{
				string key = simpleModOption.FormatValue();
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, 0);
				}
				dictionary[key]++;
			}
			else if (keybindOpt.UserData is SimpleModOption<KeybindList> simpleModOption2)
			{
				string key2 = simpleModOption2.FormatValue();
				if (!dictionary.ContainsKey(key2))
				{
					dictionary.Add(key2, 0);
				}
				dictionary[key2]++;
			}
		}
		foreach (Label keybindOpt2 in keybindOpts)
		{
			string text = "";
			if (keybindOpt2.UserData is SimpleModOption<Keybind> simpleModOption3)
			{
				text = simpleModOption3.FormatValue();
			}
			else if (keybindOpt2.UserData is SimpleModOption<KeybindList> simpleModOption4)
			{
				text = simpleModOption4.FormatValue();
			}
			if (dictionary.ContainsKey(text))
			{
				if (dictionary[text] > 1 && text != "(None)")
				{
					keybindOpt2.IdleTextColor = Color.Red;
					keybindOpt2.HoverTextColor = Color.PaleVioletRed;
				}
				else
				{
					keybindOpt2.IdleTextColor = Game1.textColor;
					keybindOpt2.HoverTextColor = Game1.unselectedOptionColor;
				}
			}
		}
	}
}
