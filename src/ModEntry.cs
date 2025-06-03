using StardewModdingAPI;

namespace MyChildCore;

public class ModEntry : Mod
{
	public override void Entry(IModHelper helper)
	{
		((Mod)this).Monitor.Log("MyChildCore (NET 8.0) loaded!", (LogLevel)2);
	}
}
