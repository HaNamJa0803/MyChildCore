using System;
using System.Globalization;
using Netcode;
using StardewValley;
using StardewValley.Mods;
using StardewValley.Network;

namespace SpaceShared;

internal static class ModDataHelper
{
	public static bool GetBool(this ModDataDictionary data, string key, bool @default = false)
	{
		string value = default(string);
		if (!((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).TryGetValue(key, ref value) || !bool.TryParse(value, out var result))
		{
			return @default;
		}
		return result;
	}

	public static void SetBool(this ModDataDictionary data, string key, bool value, bool @default = false)
	{
		if (value == @default)
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).Remove(key);
		}
		else
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data)[key] = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	public static float GetFloat(this ModDataDictionary data, string key, float @default = 0f, float? min = null)
	{
		string s = default(string);
		if (!((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).TryGetValue(key, ref s) || !float.TryParse(s, out var result) || !(result >= min))
		{
			return @default;
		}
		return result;
	}

	public static void SetFloat(this ModDataDictionary data, string key, float value, float @default = 0f, float? min = null, float? max = null)
	{
		if (value < min)
		{
			value = min.Value;
		}
		if (value > max)
		{
			value = max.Value;
		}
		if (value == @default)
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).Remove(key);
		}
		else
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data)[key] = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	public static int GetInt(this ModDataDictionary data, string key, int @default = 0, int? min = null)
	{
		string s = default(string);
		if (!((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).TryGetValue(key, ref s) || !int.TryParse(s, out var result) || !(result >= min))
		{
			return @default;
		}
		return result;
	}

	public static void SetInt(this ModDataDictionary data, string key, int value, int @default = 0, int? min = null)
	{
		if (value == @default || value <= min)
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).Remove(key);
		}
		else
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data)[key] = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	public static T GetCustom<T>(this ModDataDictionary data, string key, Func<string, T> parse, T @default = default(T), bool suppressError = true)
	{
		string arg = default(string);
		if (!((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).TryGetValue(key, ref arg))
		{
			return @default;
		}
		try
		{
			return parse(arg);
		}
		catch when (suppressError)
		{
			return @default;
		}
	}

	public static void SetCustom<T>(this ModDataDictionary data, string key, T value, Func<T, string> serialize = null)
	{
		string text = ((serialize != null) ? serialize(value) : value?.ToString());
		if (string.IsNullOrWhiteSpace(text))
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data).Remove(key);
		}
		else
		{
			((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>)(object)data)[key] = text;
		}
	}
}
