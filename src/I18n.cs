using System;
using System.CodeDom.Compiler;
using StardewModdingAPI;

namespace GenericModConfigMenu;

[GeneratedCode("TextTemplatingFileGenerator", "1.0.0")]
internal static class I18n
{
	private static ITranslationHelper? Translations;

	public static void Init(ITranslationHelper translations)
	{
		Translations = translations;
	}

	public static string Button_ModOptions()
	{
		return Translation.op_Implicit(GetByKey("button.mod-options"));
	}

	public static string List_EditableHeading()
	{
		return Translation.op_Implicit(GetByKey("list.editable-heading"));
	}

	public static string List_NotEditableHeading()
	{
		return Translation.op_Implicit(GetByKey("list.not-editable-heading"));
	}

	public static string List_Keybindings()
	{
		return Translation.op_Implicit(GetByKey("list.keybindings"));
	}

	public static string Config_Buttons_Cancel()
	{
		return Translation.op_Implicit(GetByKey("config.buttons.cancel"));
	}

	public static string Config_Buttons_ResetToDefault()
	{
		return Translation.op_Implicit(GetByKey("config.buttons.reset-to-default"));
	}

	public static string Config_Buttons_Save()
	{
		return Translation.op_Implicit(GetByKey("config.buttons.save"));
	}

	public static string Config_Buttons_SaveAndClose()
	{
		return Translation.op_Implicit(GetByKey("config.buttons.save-and-close"));
	}

	public static string Config_RebindKey_Title(object optionName)
	{
		return Translation.op_Implicit(GetByKey("config.rebind-key.title", new { optionName }));
	}

	public static string Config_RebindKey_SimpleInstructions()
	{
		return Translation.op_Implicit(GetByKey("config.rebind-key.simple-instructions"));
	}

	public static string Config_RebindKey_ComboInstructions()
	{
		return Translation.op_Implicit(GetByKey("config.rebind-key.combo-instructions"));
	}

	public static string Config_RebindKey_NoKey()
	{
		return Translation.op_Implicit(GetByKey("config.rebind-key.no-key"));
	}

	public static string Options_OpenMenuKey_Name()
	{
		return Translation.op_Implicit(GetByKey("options.open-menu-key.name"));
	}

	public static string Options_OpenMenuKey_Desc()
	{
		return Translation.op_Implicit(GetByKey("options.open-menu-key.desc"));
	}

	public static string Options_ScrollSpeed_Name()
	{
		return Translation.op_Implicit(GetByKey("options.scroll-speed.name"));
	}

	public static string Options_ScrollSpeed_Desc()
	{
		return Translation.op_Implicit(GetByKey("options.scroll-speed.desc"));
	}

	private static Translation GetByKey(string key, object? tokens = null)
	{
		if (Translations == null)
		{
			throw new InvalidOperationException("You must call I18n.Init from the mod's entry method before reading translations.");
		}
		return Translations.Get(key, tokens);
	}
}
