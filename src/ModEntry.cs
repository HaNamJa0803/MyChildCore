using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewModdingAPI.Events;
using GenericModConfigMenu;

namespace MyChildCore
{
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
                return;

            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "모드 활성화",
                tooltip: () => "자녀 외형 커스터마이징 기능을 활성화합니다.",
                getValue: () => Config.EnableMod,
                setValue: value => Config.EnableMod = value
            );

            gmcm.AddTextOption(
                mod: ModManifest,
                name: () => "아기 헤어스타일",
                tooltip: () => "아기일 때의 헤어스타일을 설정합니다.",
                getValue: () => Config.BabyHairStyle,
                setValue: value => Config.BabyHairStyle = value,
                allowedValues: new[] { "기본", "곱슬", "포니테일" }
            );

            gmcm.AddTextOption(
                mod: ModManifest,
                name: () => "유아 잠옷",
                tooltip: () => "유아일 때의 잠옷 스타일을 설정합니다.",
                getValue: () => Config.ToddlerSleepwear,
                setValue: value => Config.ToddlerSleepwear = value,
                allowedValues: new[] { "기본", "잠옷1", "잠옷2" }
            );
        }
    }
}
