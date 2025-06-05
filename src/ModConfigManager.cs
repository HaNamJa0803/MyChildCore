using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace GenericModConfigMenu.Framework;

internal class ModConfigManager
{
	private readonly Dictionary<string, ModConfig> Configs = new Dictionary<string, ModConfig>(StringComparer.OrdinalIgnoreCase);

	public ModConfig Get(IManifest manifest, bool assert)
	{
		AssertManifest(manifest);
		lock (Configs)
		{
			if (Configs.TryGetValue(manifest.UniqueID, out var value))
			{
				return value;
			}
			if (!assert)
			{
				return null;
			}
			throw new KeyNotFoundException("The '" + manifest.Name + "' mod hasn't registered a config menu.");
		}
	}

	public IEnumerable<ModConfig> GetAll()
	{
		lock (Configs)
		{
			return Configs.Values;
		}
	}

	public void Set(IManifest manifest, ModConfig config)
	{
		lock (Configs)
		{
			AssertManifest(manifest);
			Configs[manifest.UniqueID] = config ?? throw new ArgumentNullException("config");
		}
	}

	public void Remove(IManifest manifest)
	{
		lock (Configs)
		{
			AssertManifest(manifest);
			if (Configs.ContainsKey(manifest.UniqueID))
			{
				Configs.Remove(manifest.UniqueID);
			}
		}
	}

	private void AssertManifest(IManifest manifest)
	{
		if (manifest == null)
		{
			throw new ArgumentNullException("manifest");
		}
		if (string.IsNullOrWhiteSpace(manifest.UniqueID))
		{
			throw new ArgumentException("The '" + manifest.Name + "' mod manifest doesn't have a unique ID value.", "manifest");
		}
		if (string.IsNullOrWhiteSpace(manifest.Name))
		{
			throw new ArgumentException("The '" + manifest.UniqueID + "' mod manifest doesn't have a name value.", "manifest");
		}
	}
}
