using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace SpaceShared;

internal static class Util
{
	public static bool UsingMono => Type.GetType("Mono.Runtime") != null;

	public static Texture2D FetchTexture(IModRegistry modRegistry, string modIdAndPath)
	{
		if (modIdAndPath == null || modIdAndPath.IndexOf('/') == -1)
		{
			return Game1.staminaRect;
		}
		string text = modIdAndPath.Substring(0, modIdAndPath.IndexOf('/'));
		string text2 = modIdAndPath.Substring(modIdAndPath.IndexOf('/') + 1);
		IModInfo val = modRegistry.Get(text);
		if (val == null)
		{
			return Game1.staminaRect;
		}
		object? obj = ((object)val).GetType().GetProperty("Mod")?.GetValue(val);
		IMod val2 = (IMod)((obj is IMod) ? obj : null);
		if (val2 != null)
		{
			return val2.Helper.ModContent.Load<Texture2D>(text2);
		}
		object? obj2 = ((object)val).GetType().GetProperty("ContentPack")?.GetValue(val);
		IContentPack val3 = (IContentPack)((obj2 is IContentPack) ? obj2 : null);
		if (val3 != null)
		{
			return val3.ModContent.Load<Texture2D>(text2);
		}
		return Game1.staminaRect;
	}

	public static IAssetName? FetchTextureLocation(IModRegistry modRegistry, string modIdAndPath)
	{
		if (modIdAndPath == null || modIdAndPath.IndexOf('/') == -1)
		{
			return null;
		}
		string text = modIdAndPath.Substring(0, modIdAndPath.IndexOf('/'));
		string text2 = modIdAndPath.Substring(modIdAndPath.IndexOf('/') + 1);
		IModInfo val = modRegistry.Get(text);
		if (val == null)
		{
			return null;
		}
		object? obj = ((object)val).GetType().GetProperty("Mod")?.GetValue(val);
		IMod val2 = (IMod)((obj is IMod) ? obj : null);
		if (val2 != null)
		{
			return val2.Helper.ModContent.GetInternalAssetName(text2);
		}
		object? obj2 = ((object)val).GetType().GetProperty("ContentPack")?.GetValue(val);
		IContentPack val3 = (IContentPack)((obj2 is IContentPack) ? obj2 : null);
		if (val3 != null)
		{
			return val3.ModContent.GetInternalAssetName(text2);
		}
		return null;
	}

	public static string? FetchTexturePath(IModRegistry modRegistry, string modIdAndPath)
	{
		IAssetName? obj = FetchTextureLocation(modRegistry, modIdAndPath);
		if (obj == null)
		{
			return null;
		}
		return obj.BaseName;
	}

	public static string FetchFullPath(IModRegistry modRegistry, string modIdAndPath)
	{
		if (modIdAndPath == null || modIdAndPath.IndexOf('/') == -1)
		{
			return null;
		}
		string text = modIdAndPath.Substring(0, modIdAndPath.IndexOf('/'));
		string path = modIdAndPath.Substring(modIdAndPath.IndexOf('/') + 1);
		IModInfo val = modRegistry.Get(text);
		if (val == null)
		{
			return null;
		}
		object? obj = ((object)val).GetType().GetProperty("Mod")?.GetValue(val);
		IMod val2 = (IMod)((obj is IMod) ? obj : null);
		if (val2 != null)
		{
			return Path.Combine(val2.Helper.DirectoryPath, path);
		}
		object? obj2 = ((object)val).GetType().GetProperty("ContentPack")?.GetValue(val);
		IContentPack val3 = (IContentPack)((obj2 is IContentPack) ? obj2 : null);
		if (val3 != null)
		{
			return Path.Combine(val3.DirectoryPath, path);
		}
		return null;
	}

	public static Texture2D DoPaletteSwap(Texture2D baseTex, Texture2D from, Texture2D to)
	{
		Color[] array = (Color[])(object)new Color[from.Height];
		Color[] array2 = (Color[])(object)new Color[to.Height];
		from.GetData<Color>(array);
		to.GetData<Color>(array2);
		return DoPaletteSwap(baseTex, array, array2);
	}

	public static Texture2D DoPaletteSwap(Texture2D baseTex, Color[] fromCols, Color[] toCols)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<Color, Color> dictionary = new Dictionary<Color, Color>();
		for (int i = 0; i < fromCols.Length; i++)
		{
			dictionary.Add(fromCols[i], toCols[i]);
		}
		Color[] array = (Color[])(object)new Color[baseTex.Width * baseTex.Height];
		baseTex.GetData<Color>(array);
		for (int j = 0; j < array.Length; j++)
		{
			if (dictionary.TryGetValue(array[j], out var value))
			{
				array[j] = value;
			}
		}
		Texture2D val = new Texture2D(Game1.graphics.GraphicsDevice, baseTex.Width, baseTex.Height);
		val.SetData<Color>(array);
		return val;
	}

	public static T Clamp<T>(T min, T t, T max)
	{
		if (Comparer<T>.Default.Compare(min, t) > 0)
		{
			return min;
		}
		if (Comparer<T>.Default.Compare(max, t) < 0)
		{
			return max;
		}
		return t;
	}

	public static T Adjust<T>(T value, T interval)
	{
		if (value is float num && interval is float num2)
		{
			value = (T)(object)(float)((decimal)num - (decimal)num % (decimal)num2);
		}
		if (value is int num3 && interval is int num4)
		{
			value = (T)(object)(num3 - num3 % num4);
		}
		return value;
	}

	public static void Swap<T>(ref T lhs, ref T rhs)
	{
		T val = lhs;
		lhs = rhs;
		rhs = val;
	}

	public static Color ColorFromHsv(double hue, double saturation, double value)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		int num = Convert.ToInt32(Math.Floor(hue / 60.0)) % 6;
		double num2 = hue / 60.0 - Math.Floor(hue / 60.0);
		value *= 255.0;
		int num3 = Convert.ToInt32(value);
		int num4 = Convert.ToInt32(value * (1.0 - saturation));
		int num5 = Convert.ToInt32(value * (1.0 - num2 * saturation));
		int num6 = Convert.ToInt32(value * (1.0 - (1.0 - num2) * saturation));
		return (Color)(num switch
		{
			0 => new Color(num3, num6, num4), 
			1 => new Color(num5, num3, num4), 
			2 => new Color(num4, num3, num6), 
			3 => new Color(num4, num5, num3), 
			4 => new Color(num6, num4, num3), 
			_ => new Color(num3, num4, num5), 
		});
	}

	public static IEnumerable<Color> GetColorGradient(Color from, Color to, int totalNumberOfColors)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (totalNumberOfColors < 2)
		{
			throw new ArgumentException("Gradient cannot have less than two colors.", "totalNumberOfColors");
		}
		double num = ((Color)(ref to)).A - ((Color)(ref from)).A;
		double num2 = ((Color)(ref to)).R - ((Color)(ref from)).R;
		double num3 = ((Color)(ref to)).G - ((Color)(ref from)).G;
		double num4 = ((Color)(ref to)).B - ((Color)(ref from)).B;
		int steps = totalNumberOfColors - 1;
		double stepA = num / (double)steps;
		double stepR = num2 / (double)steps;
		double stepG = num3 / (double)steps;
		double stepB = num4 / (double)steps;
		yield return from;
		int i = 1;
		while (i < steps)
		{
			yield return new Color(c(((Color)(ref from)).R, stepR), c(((Color)(ref from)).G, stepG), c(((Color)(ref from)).B, stepB), c(((Color)(ref from)).A, stepA));
			int num5 = i + 1;
			i = num5;
		}
		yield return to;
		int c(int fromC, double stepC)
		{
			return (int)Math.Round((double)fromC + stepC * (double)i);
		}
	}

	public static void InvokeEvent(string name, IEnumerable<Delegate> handlers, object sender)
	{
		EventArgs e = new EventArgs();
		foreach (EventHandler item in handlers.Cast<EventHandler>())
		{
			try
			{
				item(sender, e);
			}
			catch (Exception value)
			{
				Log.Error($"Exception while handling event {name}:\n{value}");
			}
		}
	}

	public static void InvokeEvent<T>(string name, IEnumerable<Delegate> handlers, object sender, T args)
	{
		foreach (EventHandler<T> item in handlers.Cast<EventHandler<T>>())
		{
			try
			{
				item(sender, args);
			}
			catch (Exception value)
			{
				Log.Error($"Exception while handling event {name}:\n{value}");
			}
		}
	}

	public static bool InvokeEventCancelable<T>(string name, IEnumerable<Delegate> handlers, object sender, T args) where T : CancelableEventArgs
	{
		foreach (EventHandler<T> item in handlers.Cast<EventHandler<T>>())
		{
			try
			{
				item(sender, args);
			}
			catch (Exception value)
			{
				Log.Error($"Exception while handling event {name}:\n{value}");
			}
		}
		return args.Cancel;
	}
}
