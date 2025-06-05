using System;
using System.Collections.Generic;
using GenericModConfigMenu.Framework.ModOption;
using StardewModdingAPI;

namespace GenericModConfigMenu.Framework;

internal class ModConfig
{
	private ModConfigPage ActiveRegisteringPage;

	public string ModName => ModManifest.Name;

	public IManifest ModManifest { get; }

	public Action Reset { get; }

	public Action Save { get; }

	public bool DefaultTitleScreenOnly { get; set; }

	public bool AnyEditableInGame { get; set; }

	public Dictionary<string, ModConfigPage> Pages { get; } = new Dictionary<string, ModConfigPage>();


	public ModConfigPage ActiveDisplayPage { get; set; }

	public List<Action<string, object>> ChangeHandlers { get; } = new List<Action<string, object>>();


	public ModConfig(IManifest manifest, Action reset, Action save, bool defaultTitleScreenOnly)
	{
		ModManifest = manifest;
		Reset = reset;
		Save = save;
		DefaultTitleScreenOnly = defaultTitleScreenOnly;
		SetActiveRegisteringPage("", null);
	}

	public void SetActiveRegisteringPage(string pageId, Func<string> pageTitle)
	{
		if (Pages.TryGetValue(pageId, out var value))
		{
			ActiveRegisteringPage = value;
		}
		else
		{
			Pages[pageId] = (ActiveRegisteringPage = new ModConfigPage(pageId, pageTitle));
		}
	}

	public void AddOption(BaseModOption option)
	{
		ActiveRegisteringPage.Options.Add(option);
		if (!DefaultTitleScreenOnly)
		{
			AnyEditableInGame = true;
		}
	}

	public IEnumerable<BaseModOption> GetAllOptions()
	{
		foreach (ModConfigPage value in Pages.Values)
		{
			foreach (BaseModOption option in value.Options)
			{
				yield return option;
			}
		}
	}
}
