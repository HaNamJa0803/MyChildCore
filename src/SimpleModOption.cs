using System;
using SpaceShared;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace GenericModConfigMenu.Framework.ModOption;

internal class SimpleModOption<T> : BaseModOption
{
	private T CachedValue;

	protected readonly Func<T> GetValue;

	protected readonly Action<T> SetValue;

	public Type Type => typeof(T);

	public virtual T Value
	{
		get
		{
			return CachedValue;
		}
		set
		{
			if (!CachedValue.Equals(value))
			{
				base.Owner.ChangeHandlers.ForEach(delegate(Action<string, object> handler)
				{
					handler(base.FieldId, value);
				});
			}
			CachedValue = value;
		}
	}

	public SimpleModOption(string fieldId, Func<string> name, Func<string> tooltip, ModConfig mod, Func<T> getValue, Action<T> setValue)
		: base(fieldId, name, tooltip, mod)
	{
		GetValue = getValue;
		SetValue = setValue;
		CachedValue = GetValue();
	}

	public override void BeforeReset()
	{
		GetLatest();
	}

	public override void AfterReset()
	{
		GetLatest();
	}

	public override void BeforeSave()
	{
		Log.Trace("saving " + base.Name() + " " + base.Tooltip());
		SetValue(CachedValue);
	}

	public override void AfterSave()
	{
	}

	public override void BeforeMenuOpened()
	{
		GetLatest();
	}

	public override void BeforeMenuClosed()
	{
	}

	public virtual string FormatValue()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		T value = Value;
		if (!(value is SButton val))
		{
			object obj = value;
			KeybindList val2 = (KeybindList)((obj is KeybindList) ? obj : null);
			if (val2 != null && !val2.IsBound)
			{
				goto IL_0045;
			}
		}
		else if ((int)val == 0)
		{
			goto IL_0045;
		}
		T value2 = Value;
		if (value2 == null)
		{
			return null;
		}
		return value2.ToString();
		IL_0045:
		return I18n.Config_RebindKey_NoKey();
	}

	private void GetLatest()
	{
		CachedValue = GetValue();
	}
}
