using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MyChildCore;

public class ModConfig
{
	[field: CompilerGenerated]
	public bool UseSleepwear
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;


	[field: CompilerGenerated]
	public bool UseFestivalOutfits
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;


	[field: CompilerGenerated]
	public bool UseSeasonalOutfits
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;


	[field: CompilerGenerated]
	public bool AppendSpouseNameToChild
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;


	[field: CompilerGenerated]
	public Dictionary<string, SpouseSetting> SpouseSettings
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = new Dictionary<string, SpouseSetting>();

}
