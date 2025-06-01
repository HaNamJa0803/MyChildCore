using System;
using StardewModdingAPI;

namespace MyTestGmcmMod
{
    // GMCM 인터페이스 사본
    public interface IGenericModConfigMenuApi
    {
        void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);
        void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
    }

    // 설정 클래스
    public class ModConfig
    {
        public bool TestOption { get; set; } = false;
    }

    public class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            // 설정 파일 로드
            this.Config = helper.ReadConfig<ModConfig>();

            // GMCM API 런타임 동적 로드
            var gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm != null)
            {
                gmcm.Register(this.ModManifest, this.ResetConfig, this.SaveConfig);
                gmcm.AddBoolOption(
                    this.ModManifest,
                    () => this.Config.TestOption,
                    value => this.Config.TestOption = value,
                    () => "테스트 옵션",
                    () => "이 옵션은 테스트용입니다."
                );
            }
        }

        private void ResetConfig()
        {
            this.Config = new ModConfig();
            this.Helper.WriteConfig(this.Config);
        }

        private void SaveConfig()
        {
            this.Helper.WriteConfig(this.Config);
        }
    }
}