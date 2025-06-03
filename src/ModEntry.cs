using System;
using System.Collections.Generic;
using StardewModdingAPI.Utilities;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;
using StardewValley;

namespace MyChildCore
{
    public class ModConfig
    {
        public Dictionary<string, SpouseSetting> SpouseConfigs { get; set; } = new();

        public ModConfig()
        {
            // 예시 초기값
            foreach (var spouse in new[] { "Penny", "Leah", "Abigail" })
                SpouseConfigs[spouse] = new SpouseSetting();
        }
    }

    public class SpouseSetting
    {
        public string GirlHair { get; set; } = "Default";
        public string GirlPajama { get; set; } = "Basic";
        public string BoyPajama { get; set; } = "Basic";
        // ... 원하는 만큼 필드 추가
    }

    public class ModEntry : Mod
    {
        public static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null)
            {
                Monitor.Log("GMCM 연동 실패! GMCM 설치 여부를 확인하세요.", LogLevel.Warn);
                return;
            }

            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            // 반복문으로 배우자별/성별별 옵션 드롭다운 자동 생성
            foreach (var spouse in Config.SpouseConfigs.Keys)
            {
                var s = spouse; // 클로저 문제 방지

                gmcm.AddSectionTitle(ModManifest, () => $"{s} 자녀 설정");

                // 여아 헤어 스타일
                gmcm.AddDropdown(
                    mod: ModManifest,
                    name: () => $"{s} 여아 헤어",
                    tooltip: () => $"{s}의 딸 헤어",
                    getValue: () => Config.SpouseConfigs[s].GirlHair,
                    setValue: v => Config.SpouseConfigs[s].GirlHair = v,
                    allowedValues: () => new[] { "Default", "Curly", "Ponytail" }
                );

                // 여아 잠옷
                gmcm.AddDropdown(
                    mod: ModManifest,
                    name: () => $"{s} 여아 잠옷",
                    tooltip: () => $"{s}의 딸 잠옷 스타일",
                    getValue: () => Config.SpouseConfigs[s].GirlPajama,
                    setValue: v => Config.SpouseConfigs[s].GirlPajama = v,
                    allowedValues: () => new[] { "Basic1", "Basic2" }
                );

                // 남아 잠옷
                gmcm.AddDropdown(
                    mod: ModManifest,
                    name: () => $"{s} 남아 잠옷",
                    tooltip: () => $"{s}의 아들 잠옷 스타일",
                    getValue: () => Config.SpouseConfigs[s].BoyPajama,
                    setValue: v => Config.SpouseConfigs[s].BoyPajama = v,
                    allowedValues: () => new[] { "Basic1", "Basic2" }
                );
                // ... 필요한 만큼 반복 자동 생성 가능
            }
        }
    }
}
