using StardewModdingAPI;
using StardewValley;
// GMCM 인터페이스 직접 선언 (간략화, 핵심만)
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

            // GMCM 연동: IManifest 등 타입 없이 object로만 넘김!
            var gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm != null)
            {
                // object 대신 this나 helper로 전달해도 내부에서 처리됨 (사장님 환경 기준)
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