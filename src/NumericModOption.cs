using System;
using SpaceShared;

namespace GenericModConfigMenu.Framework.ModOption;

internal class NumericModOption<T> : SimpleModOption<T> where T : struct
{
	private readonly Func<T, string> FormatValueImpl;

	public T? Minimum { get; }

	public T? Maximum { get; }

	public T? Interval { get; }

	public override T Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			if (Minimum.HasValue || Maximum.HasValue)
			{
				T t = value;
				value = Util.Clamp(Minimum.GetValueOrDefault(value), t, Maximum.GetValueOrDefault(value));
			}
			if (Interval.HasValue)
			{
				value = Util.Adjust(value, Interval.Value);
			}
			base.Value = value;
		}
	}

	public NumericModOption(string fieldId, Func<string> name, Func<string> tooltip, ModConfig mod, Func<T> getValue, Action<T> setValue, T? min, T? max, T? interval, Func<T, string> formatValue)
		: base(fieldId, name, tooltip, mod, getValue, setValue)
	{
		Minimum = min;
		Maximum = max;
		Interval = interval;
		FormatValueImpl = formatValue;
	}

	public override string FormatValue()
	{
		return FormatValueImpl?.Invoke(Value) ?? Value.ToString();
	}
}
