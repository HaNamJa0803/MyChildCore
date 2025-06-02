using StardewModdingAPI;
using StardewValley;

// GMCM 인터페이스 (필요한 부분만)
public interface IGenericModConfigMenuApi
{
    void Register(object mod, System.Action reset, System.Action save, bool titleScreenOnly = false);
    void AddBoolOption(
        object mod,
        System.Func<bool> getValue,
        System.Action<bool> setValue,
        System.Func<string> name,
        System.Func<string> tooltip = null,
        string fieldId = null
    );
}

namespace MyChildCore
{
    public class ModConfig
    {
        public bool UseSleepwear { get; set; } = true;
    }

    public class ModEntry : Mod
    {
        public static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();

            // GMCM 연동
            var gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm != null)
            {
                gmcm.Register(this,
                    reset: () => Config = new ModConfig(),
                    save: () => helper.WriteConfig(Config)
                );

                gmcm.AddBoolOption(
                    this,
                    getValue: () => Config.UseSleepwear,
                    setValue: v => { Config.UseSleepwear = v; helper.WriteConfig(Config); },
                    name: () => "잠옷 사용 (테스트용)",
                    tooltip: () => "자녀 잠옷 기능 ON/OFF"
                );
            }
        }
    }
}