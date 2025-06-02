using StardewModdingAPI;

namespace MyChildCore;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        Monitor.Log("MyChildCore (NET 4.8) loaded!", LogLevel.Info);
    }
}