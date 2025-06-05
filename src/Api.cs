using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GenericModConfigMenu.Framework.ModOption;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShared;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace GenericModConfigMenu.Framework;

public class Api : IGenericModConfigMenuApi, IGenericModConfigMenuApiWithObsoleteMethods
{
	private readonly ModConfigManager ConfigManager;

	private readonly Action<IManifest> OpenModMenuImpl;

	private readonly Action<IManifest> OpenModMenuImplChild;

	private readonly IManifest mod;

	private readonly Action<string> DeprecationWarner;

	internal Api(IManifest mod, ModConfigManager configManager, Action<IManifest> openModMenu, Action<IManifest> openModMenuChild, Action<string> DeprecationWarner)
	{
		this.mod = mod;
		ConfigManager = configManager;
		OpenModMenuImpl = openModMenu;
		OpenModMenuImplChild = openModMenuChild;
		this.DeprecationWarner = DeprecationWarner;
	}

	public void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = true)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(reset, "reset");
		AssertNotNull(save, "save");
		if (ConfigManager.Get(mod, assert: false) != null)
		{
			throw new InvalidOperationException("The '" + mod.Name + "' mod has already registered a config menu, so it can't do it again.");
		}
		if (this.mod.UniqueID != mod.UniqueID)
		{
			Log.Trace(this.mod.UniqueID + " is registering on behalf of " + mod.UniqueID);
		}
		ConfigManager.Set(mod, new ModConfig(mod, reset, save, titleScreenOnly));
	}

	public void AddSectionTitle(IManifest mod, Func<string> text, Func<string> tooltip = null)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(text, "text");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.AddOption(new SectionTitleModOption(text, tooltip, modConfig));
	}

	public void AddParagraph(IManifest mod, Func<string> text)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(text, "text");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.AddOption(new ParagraphModOption(text, modConfig));
	}

	public void AddImage(IManifest mod, Func<Texture2D> texture, Rectangle? texturePixelArea = null, int scale = 4)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(texture, "texture");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.AddOption(new ImageModOption(texture, texturePixelArea, scale, modConfig));
	}

	public void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null)
	{
		AddSimpleOption(mod, name, tooltip, getValue, setValue, fieldId);
	}

	public void AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name = null, Func<string> tooltip = null, int? min = null, int? max = null, int? interval = null, Func<int, string> formatValue = null, string fieldId = null)
	{
		AddNumericOption(mod, name, tooltip, getValue, setValue, min, max, interval, formatValue, fieldId);
	}

	public void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name = null, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, Func<float, string> formatValue = null, string fieldId = null)
	{
		AddNumericOption(mod, name, tooltip, getValue, setValue, min, max, interval, formatValue, fieldId);
	}

	public void AddTextOption(IManifest mod, Func<string> getValue, Action<string> setValue, Func<string> name = null, Func<string> tooltip = null, string[] allowedValues = null, Func<string, string> formatAllowedValue = null, string fieldId = null)
	{
		if (allowedValues != null && allowedValues.Any())
		{
			AddChoiceOption(mod, name, tooltip, getValue, setValue, allowedValues, formatAllowedValue, fieldId);
		}
		else
		{
			AddSimpleOption(mod, name, tooltip, getValue, setValue, fieldId);
		}
	}

	public void AddKeybind(IManifest mod, Func<SButton> getValue, Action<SButton> setValue, Func<string> name = null, Func<string> tooltip = null, string fieldId = null)
	{
		AddSimpleOption(mod, name, tooltip, getValue, setValue, fieldId);
	}

	public void AddKeybindList(IManifest mod, Func<KeybindList> getValue, Action<KeybindList> setValue, Func<string> name = null, Func<string> tooltip = null, string fieldId = null)
	{
		AddSimpleOption(mod, name, tooltip, getValue, setValue, fieldId);
	}

	public void AddPage(IManifest mod, string pageId, Func<string> pageTitle = null)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(pageId, "pageId");
		ConfigManager.Get(mod, assert: true).SetActiveRegisteringPage(pageId, pageTitle);
	}

	public void AddPageLink(IManifest mod, string pageId, Func<string> text, Func<string> tooltip = null)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(pageId, "pageId");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.AddOption(new PageLinkModOption(pageId, text, tooltip, modConfig));
	}

	public void AddComplexOption(IManifest mod, Func<string> name, Action<SpriteBatch, Vector2> draw, Func<string> tooltip = null, Action beforeMenuOpened = null, Action beforeSave = null, Action afterSave = null, Action beforeReset = null, Action afterReset = null, Action beforeMenuClosed = null, Func<int> height = null, string fieldId = null)
	{
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.AddOption(new ComplexModOption(fieldId, name, tooltip, modConfig, height, draw, beforeMenuOpened, beforeSave, afterSave, beforeReset, afterReset, beforeMenuClosed));
	}

	public void SetTitleScreenOnlyForNextOptions(IManifest mod, bool titleScreenOnly)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.DefaultTitleScreenOnly = titleScreenOnly;
	}

	public void OnFieldChanged(IManifest mod, Action<string, object> onChange)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(onChange, "onChange");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.ChangeHandlers.Add(onChange);
	}

	public void OpenModMenu(IManifest mod)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		OpenModMenuImpl(mod);
	}

	public void OpenModMenuAsChildMenu(IManifest mod)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		OpenModMenuImplChild(mod);
	}

	public void Unregister(IManifest mod)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		ConfigManager.Remove(mod);
	}

	public bool TryGetCurrentMenu(out IManifest mod, out string page)
	{
		SpecificModConfigMenu specificModConfigMenu = Mod.ActiveConfigMenu as SpecificModConfigMenu;
		if (specificModConfigMenu == null)
		{
			specificModConfigMenu = null;
		}
		mod = specificModConfigMenu?.Manifest;
		page = specificModConfigMenu?.CurrPage;
		return specificModConfigMenu != null;
	}

	[Obsolete]
	public void AddComplexOption(IManifest mod, Func<string> name, Action<SpriteBatch, Vector2> draw, Func<string> tooltip = null, Action beforeSave = null, Action afterSave = null, Action beforeReset = null, Action afterReset = null, Func<int> height = null, string fieldId = null)
	{
		LogDeprecation(mod, "AddComplexOption(IManifest mod, Func<string> name, Action<SpriteBatch, Vector2> draw, Func<string> tooltip = null, Action beforeSave = null, Action afterSave = null, Action beforeReset = null, Action afterReset = null, Func<int> height = null, string fieldId = null)");
		AddComplexOption(mod, name, draw, tooltip, null, beforeSave, afterSave, beforeReset, afterReset, null, height, fieldId);
	}

	[Obsolete]
	public void AddComplexOption(IManifest mod, Func<string> name, Func<string> tooltip, Action<SpriteBatch, Vector2> draw, Action saveChanges, Func<int> height = null, string fieldId = null)
	{
		LogDeprecation(mod, "AddChoiceOption(IManifest mod, Func<string> name, Func<string> tooltip, Action<SpriteBatch, Vector2> draw, Action saveChanges, Func<int> height = null, string fieldId = null)");
		AddComplexOption(mod, name, draw, tooltip, null, saveChanges, null, null, null, null, height, fieldId);
	}

	[Obsolete]
	public void AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name = null, Func<string> tooltip = null, int? min = null, int? max = null, int? interval = null, string fieldId = null)
	{
		LogDeprecation(mod, "AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name = null, Func<string> tooltip = null, int? min = null, int? max = null, int? interval = null, string fieldId = null)");
		AddNumericOption(mod, name, tooltip, getValue, setValue, min, max, interval, null, fieldId);
	}

	[Obsolete]
	public void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name = null, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, string fieldId = null)
	{
		LogDeprecation(mod, "AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name = null, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, string fieldId = null)");
		AddNumericOption(mod, name, tooltip, getValue, setValue, min, max, interval, null, fieldId);
	}

	[Obsolete]
	public void AddTextOption(IManifest mod, Func<string> getValue, Action<string> setValue, Func<string> name = null, Func<string> tooltip = null, string[] allowedValues = null, string fieldId = null)
	{
		LogDeprecation(mod, "AddTextOption(IManifest mod, Func<string> getValue, Action<string> setValue, Func<string> name = null, Func<string> tooltip = null, string[] allowedValues = null, string fieldId = null)");
		AddTextOption(mod, getValue, setValue, name, tooltip, allowedValues, null, fieldId);
	}

	[Obsolete]
	public void RegisterModConfig(IManifest mod, Action revertToDefault, Action saveToFile)
	{
		LogDeprecation(mod, "RegisterModConfig");
		Register(mod, revertToDefault, saveToFile);
	}

	[Obsolete]
	public void UnregisterModConfig(IManifest mod)
	{
		LogDeprecation(mod, "UnregisterModConfig");
		Unregister(mod);
	}

	[Obsolete]
	public void SetDefaultIngameOptinValue(IManifest mod, bool optedIn)
	{
		LogDeprecation(mod, "SetDefaultIngameOptinValue");
		SetTitleScreenOnlyForNextOptions(mod, !optedIn);
	}

	[Obsolete]
	public void StartNewPage(IManifest mod, string pageName)
	{
		LogDeprecation(mod, "StartNewPage");
		AddPage(mod, pageName, () => pageName);
	}

	[Obsolete]
	public void OverridePageDisplayName(IManifest mod, string pageName, string displayName)
	{
		LogDeprecation(mod, "OverridePageDisplayName");
		if (mod == null)
		{
			mod = this.mod;
		}
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		ModConfigPage modConfigPage = modConfig.Pages.GetOrDefault(pageName) ?? throw new ArgumentException("Page not registered");
		modConfigPage.SetPageTitle(() => displayName);
	}

	[Obsolete]
	public void RegisterLabel(IManifest mod, string labelName, string labelDesc)
	{
		LogDeprecation(mod, "RegisterLabel");
		AddSectionTitle(mod, () => labelName, () => labelDesc);
	}

	[Obsolete]
	public void RegisterPageLabel(IManifest mod, string labelName, string labelDesc, string newPage)
	{
		LogDeprecation(mod, "RegisterPageLabel");
		AddPageLink(mod, newPage, () => labelName, () => labelDesc);
	}

	[Obsolete]
	public void RegisterParagraph(IManifest mod, string paragraph)
	{
		LogDeprecation(mod, "RegisterParagraph");
		AddParagraph(mod, () => paragraph);
	}

	[Obsolete]
	public void RegisterImage(IManifest mod, string texPath, Rectangle? texRect = null, int scale = 4)
	{
		LogDeprecation(mod, "RegisterImage");
		AddImage(mod, () => Game1.content.Load<Texture2D>(texPath), texRect, scale);
	}

	[Obsolete]
	public void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<bool> optionGet, Action<bool> optionSet)
	{
		LogDeprecation(mod, "RegisterSimpleOption");
		AddBoolOption(mod, fieldId: optionName, getValue: optionGet, setValue: optionSet, name: () => optionName, tooltip: () => optionDesc);
	}

	[Obsolete]
	public void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet)
	{
		LogDeprecation(mod, "RegisterSimpleOption");
		Func<string> name = () => optionName;
		Func<string> tooltip = () => optionDesc;
		string fieldId = optionName;
		AddNumericOption(mod, name, tooltip, optionGet, optionSet, null, null, null, null, fieldId);
	}

	[Obsolete]
	public void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet)
	{
		LogDeprecation(mod, "RegisterSimpleOption");
		Func<string> name = () => optionName;
		Func<string> tooltip = () => optionDesc;
		string fieldId = optionName;
		AddNumericOption(mod, name, tooltip, optionGet, optionSet, null, null, null, null, fieldId);
	}

	[Obsolete]
	public void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<string> optionGet, Action<string> optionSet)
	{
		LogDeprecation(mod, "RegisterSimpleOption");
		AddTextOption(mod, fieldId: optionName, getValue: optionGet, setValue: optionSet, name: () => optionName, tooltip: () => optionDesc);
	}

	[Obsolete]
	public void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<SButton> optionGet, Action<SButton> optionSet)
	{
		LogDeprecation(mod, "RegisterSimpleOption");
		AddKeybind(mod, fieldId: optionName, getValue: optionGet, setValue: optionSet, name: () => optionName, tooltip: () => optionDesc);
	}

	[Obsolete]
	public void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<KeybindList> optionGet, Action<KeybindList> optionSet)
	{
		LogDeprecation(mod, "RegisterSimpleOption");
		AddKeybindList(mod, fieldId: optionName, getValue: optionGet, setValue: optionSet, name: () => optionName, tooltip: () => optionDesc);
	}

	[Obsolete]
	public void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet, int min, int max)
	{
		LogDeprecation(mod, "RegisterClampedOption");
		AddNumericOption<int>(mod, () => optionName, () => optionDesc, fieldId: optionName, getValue: optionGet, setValue: optionSet, min: min, max: max, interval: null, formatValue: null);
	}

	[Obsolete]
	public void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet, float min, float max)
	{
		LogDeprecation(mod, "RegisterClampedOption");
		AddNumericOption<float>(mod, () => optionName, () => optionDesc, fieldId: optionName, getValue: optionGet, setValue: optionSet, min: min, max: max, interval: null, formatValue: null);
	}

	[Obsolete]
	public void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet, int min, int max, int interval)
	{
		LogDeprecation(mod, "RegisterClampedOption");
		AddNumericOption<int>(mod, () => optionName, () => optionDesc, fieldId: optionName, getValue: optionGet, setValue: optionSet, min: min, max: max, interval: interval, formatValue: null);
	}

	[Obsolete]
	public void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet, float min, float max, float interval)
	{
		LogDeprecation(mod, "RegisterClampedOption");
		AddNumericOption<float>(mod, () => optionName, () => optionDesc, fieldId: optionName, getValue: optionGet, setValue: optionSet, min: min, max: max, interval: interval, formatValue: null);
	}

	[Obsolete]
	public void RegisterChoiceOption(IManifest mod, string optionName, string optionDesc, Func<string> optionGet, Action<string> optionSet, string[] choices)
	{
		LogDeprecation(mod, "RegisterChoiceOption");
		AddTextOption(mod, fieldId: optionName, getValue: optionGet, setValue: optionSet, name: () => optionName, tooltip: () => optionDesc, allowedValues: choices);
	}

	[Obsolete]
	public void RegisterComplexOption(IManifest mod, string optionName, string optionDesc, Func<Vector2, object, object> widgetUpdate, Func<SpriteBatch, Vector2, object, object> widgetDraw, Action<object> onSave)
	{
		LogDeprecation(mod, "RegisterComplexOption");
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(widgetUpdate, "widgetUpdate");
		AssertNotNull(widgetDraw, "widgetDraw");
		AssertNotNull(onSave, "onSave");
		object state = null;
		AddComplexOption(mod, () => optionName, () => optionDesc, fieldId: optionName, draw: Draw, saveChanges: Save);
		void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			state = widgetUpdate(position, state);
			state = widgetDraw(spriteBatch, position, state);
		}
		void Save()
		{
			onSave(state);
		}
	}

	[Obsolete]
	public void SubscribeToChange(IManifest mod, Action<string, bool> changeHandler)
	{
		LogDeprecation(mod, "SubscribeToChange(IManifest mod, Action<string, bool> changeHandler)");
		SubscribeToChange<bool>(mod, changeHandler);
	}

	[Obsolete]
	public void SubscribeToChange(IManifest mod, Action<string, int> changeHandler)
	{
		LogDeprecation(mod, "SubscribeToChange(IManifest mod, Action<string, int> changeHandler)");
		SubscribeToChange<int>(mod, changeHandler);
	}

	[Obsolete]
	public void SubscribeToChange(IManifest mod, Action<string, float> changeHandler)
	{
		LogDeprecation(mod, "SubscribeToChange(IManifest mod, Action<string, float> changeHandler)");
		SubscribeToChange<float>(mod, changeHandler);
	}

	[Obsolete]
	public void SubscribeToChange(IManifest mod, Action<string, string> changeHandler)
	{
		LogDeprecation(mod, "SubscribeToChange(IManifest mod, Action<string, string> changeHandler)");
		SubscribeToChange<string>(mod, changeHandler);
	}

	private void AddSimpleOption<T>(IManifest mod, Func<string> name, Func<string> tooltip, Func<T> getValue, Action<T> setValue, string fieldId)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(name, "name");
		AssertNotNull(getValue, "getValue");
		AssertNotNull(setValue, "setValue");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		Type[] source = new Type[6]
		{
			typeof(bool),
			typeof(int),
			typeof(float),
			typeof(string),
			typeof(SButton),
			typeof(KeybindList)
		};
		if (!source.Contains(typeof(T)))
		{
			throw new ArgumentException("Invalid config option type.");
		}
		modConfig.AddOption(new SimpleModOption<T>(fieldId, name, tooltip, modConfig, getValue, setValue));
	}

	private void AddNumericOption<T>(IManifest mod, Func<string> name, Func<string> tooltip, Func<T> getValue, Action<T> setValue, T? min, T? max, T? interval, Func<T, string> formatValue, string fieldId) where T : struct
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(name, "name");
		AssertNotNull(getValue, "getValue");
		AssertNotNull(setValue, "setValue");
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		Type[] source = new Type[2]
		{
			typeof(int),
			typeof(float)
		};
		if (!source.Contains(typeof(T)))
		{
			throw new ArgumentException("Invalid config option type.");
		}
		modConfig.AddOption(new NumericModOption<T>(fieldId, name, tooltip, modConfig, getValue, setValue, min, max, interval, formatValue));
	}

	private void AddChoiceOption(IManifest mod, Func<string> name, Func<string> tooltip, Func<string> getValue, Action<string> setValue, string[] allowedValues, Func<string, string> formatAllowedValues, string fieldId)
	{
		if (mod == null)
		{
			mod = this.mod;
		}
		AssertNotNull(name, "name");
		AssertNotNull(getValue, "getValue");
		AssertNotNull(setValue, "setValue");
		if (name == null)
		{
			name = () => fieldId;
		}
		ModConfig modConfig = ConfigManager.Get(mod, assert: true);
		modConfig.AddOption(new ChoiceModOption<string>(fieldId, name, tooltip, modConfig, getValue, setValue, allowedValues, formatAllowedValues));
	}

	[Obsolete("This only exists to support obsolete methods.")]
	private void SubscribeToChange<TValue>(IManifest mod, Action<string, TValue> changeHandler)
	{
		AssertNotNull(changeHandler, "changeHandler");
		OnFieldChanged(mod, delegate(string fieldId, object rawValue)
		{
			if (rawValue is TValue arg)
			{
				changeHandler(fieldId, arg);
			}
		});
	}

	private void AssertNotNull(object value, [CallerArgumentExpression("value")] string paramName = "")
	{
		if (value == null)
		{
			throw new ArgumentNullException(paramName);
		}
	}

	private void AssertNotNullOrWhitespace(string value, [CallerArgumentExpression("value")] string paramName = "")
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentNullException(paramName);
		}
	}

	private void LogDeprecation(IManifest client, string method)
	{
		DeprecationWarner($"{mod.UniqueID} (registering for {client.UniqueID}) is using deprecated code ({method}) that will break in a future version of GMCM.");
	}
}
