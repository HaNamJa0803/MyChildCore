using System;
using System.IO;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore;

public class ChildAppearanceApplier
{
	private readonly IMonitor Monitor;

	public ChildAppearanceApplier(IMonitor monitor)
	{
		Monitor = monitor;
	}

	public void ApplyCustomSprite(Child child, string path)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			string text = Path.Combine(path, ((Character)child).Name + ".png");
			if (File.Exists(text))
			{
				((Character)child).Sprite = new AnimatedSprite(text, 0, 32, 32);
				Monitor.Log("Loaded custom sprite for " + ((Character)child).Name + " from " + text, (LogLevel)2);
			}
			else
			{
				Monitor.Log("Custom sprite for " + ((Character)child).Name + " not found at " + text, (LogLevel)3);
			}
		}
		catch (global::System.Exception ex)
		{
			Monitor.Log("Failed to load custom sprite: " + ex.Message, (LogLevel)4);
		}
	}
}
