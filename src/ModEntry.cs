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
    // 컨피그 클래스: 반드시 한 번만!
    public class ModConfig
    {
        public bool UseSleepwear { get; set; } = true;
    }

    // 모드 엔트리: 반드시 한 번만!
    public class ModEntry : Mod
    {
        public static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            // GMCM 연동은 반드시 GameLaunched 이벤트에서!
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        // 모든 모드 로딩 이후에만 GMCM 연동
        private void OnGameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm != null)
            {
                gmcm.Register(this,
                    reset: () => Config = new ModConfig(),
                    save: () => Helper.WriteConfig(Config)
                );

                gmcm.AddBoolOption(
                    this,
                    getValue: () => Config.UseSleepwear,
                    setValue: v => { Config.UseSleepwear = v; Helper.WriteConfig(Config); },
                    name: () => "잠옷 사용 (테스트용)",
                    tooltip: () => "자녀 잠옷 기능 ON/OFF"
                );
            }
        }
    }
}