using StardewModdingAPI;

namespace MyChildCore
{
    // 최소 설정 클래스
    public class ModConfig
    {
        public bool UseSleepwear { get; set; } = true;
    }

    // GMCM 최소 인터페이스 (object 파라미터만!)
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

    public class ModEntry : Mod
    {
        public static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();

            // GMCM 연동 - IManifest/ModManifest/IModManifest 전혀 사용하지 않음
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
                    name: () => "잠옷 사용 (테스트)",
                    tooltip: () => "자녀 잠옷 기능을 켜고 끄는 테스트 옵션입니다."
                );
            }

            Monitor.Log("MyChildCore (NET 4.8) loaded!", LogLevel.Info);
        }
    }
}