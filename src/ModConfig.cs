using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;

public class ModConfig
{
    public string PajamaStyle { get; set; } = "Frog";
    public bool EnablePajama { get; set; } = true;
}

public class ModEntry : Mod
{
    private ModConfig Config;

    public override void Entry(IModHelper helper)
    {
        Config = helper.ReadConfig<ModConfig>();
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }

    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (gmcm == null)
        {
            Monitor.Log("GMCM not found!", LogLevel.Warn);
            return;
        }

        gmcm.Register(
            this,
            reset: () => { Config = new ModConfig(); Helper.WriteConfig(Config); },
            save: () => { Helper.WriteConfig(Config); }
        );

        gmcm.AddDropdown(
            this,
            name: () => "잠옷 스타일",
            tooltip: () => "자녀 잠옷 스타일",
            getValue: () => Config.PajamaStyle,
            setValue: v => { Config.PajamaStyle = v; Helper.WriteConfig(Config); },
            allowedValues: () => new string[] { "Frog", "Shark", "Sheep" }
        );

        gmcm.AddBoolOption(
            this,
            name: () => "잠옷 커스텀",
            tooltip: () => "자녀 잠옷 커스텀 활성화",
            getValue: () => Config.EnablePajama,
            setValue: v => { Config.EnablePajama = v; Helper.WriteConfig(Config); }
        );
    }
}