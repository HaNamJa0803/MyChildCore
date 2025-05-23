using StardewModdingAPI; using StardewModdingAPI.Utilities; using StardewValley; using GenericModConfigMenu;

namespace MyChildCore { public class ModEntry : Mod { public static ModEntry Instance { get; private set; } public ModConfig Config { get; private set; }

public override void Entry(IModHelper helper)
    {
        Instance = this;
        Config = helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
    }

    private void OnGameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
    {
        var api = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (api == null)
            return;

        var manifest = ModManifest;
        var config = Config;

        api.Register(manifest, () => Config = new ModConfig(), () => Helper.WriteConfig(Config));

        api.AddBoolOption(
            mod: manifest,
            name: () => Helper.Translation.Get("enableHat"),
            tooltip: () => Helper.Translation.Get("enableHat.tooltip"),
            getValue: () => config.EnableHat,
            setValue: value => config.EnableHat = value
        );

        api.AddBoolOption(
            mod: manifest,
            name: () => Helper.Translation.Get("enableSeasonalClothing"),
            tooltip: () => Helper.Translation.Get("enableSeasonalClothing.tooltip"),
            getValue: () => config.EnableSeasonalClothing,
            setValue: value => config.EnableSeasonalClothing = value
        );

        foreach (var spouse in SpouseNames.SpouseList)
        {
            foreach (var gender in new[] { "Boy", "Girl" })
            {
                string key(string prop) => $"{spouse}_{gender}_{prop}";

                api.AddTextOption(
                    mod: manifest,
                    name: () => "하의 색상",
                    getValue: () => config.Get(spouse, gender, "BottomsColor"),
                    setValue: value => config.Set(spouse, gender, "BottomsColor", value),
                    allowedValues: BottomsOptions.Colors,
                    formatAllowedValue: BottomsOptions.FormatColor
                );

                if (gender == "Girl")
                {
                    api.AddTextOption(
                        mod: manifest,
                        name: () => "헤어 스타일",
                        getValue: () => config.Get(spouse, gender, "Hair"),
                        setValue: value => config.Set(spouse, gender, "Hair", value),
                        allowedValues: new[] { "Ponytail", "TwinTail", "CherryTwin" },
                        formatAllowedValue: HairOptions.FormatStyle
                    );
                }

                api.AddTextOption(
                    mod: manifest,
                    name: () => "헤어 색상",
                    getValue: () => config.Get(spouse, gender, "HairColor"),
                    setValue: value => config.Set(spouse, gender, "HairColor", value),
                    allowedValues: HairOptions.Colors,
                    formatAllowedValue: HairOptions.FormatColor
                );
            }
        }
    }

    private void OnSaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
    {
        // 외형 적용 로직은 별도로 존재
    }
}

}

