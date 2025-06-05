using System;

namespace GenericModConfigMenu.Framework.ModOption;

internal abstract class BaseModOption
{
	public string FieldId { get; }

	public Func<string> Name { get; }

	public Func<string> Tooltip { get; }

	public bool IsTitleScreenOnly { get; }

	public ModConfig Owner { get; }

	public abstract void BeforeReset();

	public abstract void AfterReset();

	public abstract void BeforeSave();

	public abstract void AfterSave();

	public abstract void BeforeMenuOpened();

	public abstract void BeforeMenuClosed();

	protected BaseModOption(string fieldId, Func<string> name, Func<string> tooltip, ModConfig mod)
	{
		if (fieldId == null)
		{
			fieldId = Guid.NewGuid().ToString("N");
		}
		if (tooltip == null)
		{
			tooltip = () => (string)null;
		}
		Name = name;
		Tooltip = tooltip;
		FieldId = fieldId;
		IsTitleScreenOnly = mod.DefaultTitleScreenOnly;
		Owner = mod;
	}
}
