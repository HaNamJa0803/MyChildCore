using ModConfigs; // 또는 실제 네임스페이스
using System.Runtime.CompilerServices;

namespace MyChildCore;

public class SpouseSetting
{
	[field: CompilerGenerated]
	public bool UseMaleOutfit
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;


	[field: CompilerGenerated]
	public bool UseFemaleOutfit
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = true;


	[field: CompilerGenerated]
	public string MaleOutfit
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = "";


	[field: CompilerGenerated]
	public string FemaleOutfit
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = "";


	[field: CompilerGenerated]
	public ToddlerGenderSetting ToddlerGenderSetting
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	} = new ToddlerGenderSetting();

}
