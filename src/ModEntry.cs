using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewModdingAPI.Events;
using GenericModConfigMenu;

namespace MyChildCore
{
    public class ModEntry : Mod
    {
        public ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // GMCM API 동적 호출/직접 참조 모두 가능, 여기선 공식 예시(인터페이스)
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null)
            {
                Monitor.Log("GMCM 연동 실패! GMCM 설치 여부를 확인하세요.", LogLevel.Warn);
                return;
            }

            // 매니페스트(공식 방식)
            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "모드 활성화",
                tooltip: () => "모드를 켜거나 끕니다.",
                getValue: () => Config.EnableMod,
                setValue: value => Config.EnableMod = value
            );

            gmcm.AddDropdown(
                mod: ModManifest,
                name: () => "여아 헤어스타일",
                tooltip: () => "자녀(여)의 머리 스타일 선택",
                getValue: () => Config.GirlHairStyle,
                setValue: v => Config.GirlHairStyle = v,
                allowedValues: () => new string[] { "Basic", "Curly", "Ponytail" }
            );

            gmcm.AddDropdown(
                mod: ModManifest,
                name: () => "잠옷 종류",
                tooltip: () => "자녀 잠옷 스타일 선택",
                getValue: () => Config.PajamaType,
                setValue: v => Config.PajamaType = v,
                allowedValues: () => new string[] { "Pajama1", "Pajama2" }
            );
        }
    }

    // GMCM 인터페이스 최소 정의
    public interface IGenericModConfigMenuApi
    {
        void Register(object mod, Action reset, Action save, bool titleScreenOnly = false);
        void AddBoolOption(object mod, Func<string> name, Func<string> tooltip, Func<bool> getValue, Action<bool> setValue);
        void AddDropdown(object mod, Func<string> name, Func<string> tooltip, Func<string> getValue, Action<string> setValue, Func<IEnumerable<string>> allowedValues, Func<string, string> format = null);
    }
}