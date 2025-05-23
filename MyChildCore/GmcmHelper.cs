using GenericModConfigMenu;

namespace MyChildCore { public static class GmcmHelper { public static void RegisterAllOptions(IGenericModConfigMenuApi api, IManifest manifest, ModConfig config) { api.Register(manifest, () => new ModConfig(), () => ModEntry.Instance.Helper.WriteConfig(config));

api.AddBoolOption(
            mod: manifest,
            name: () => ModEntry.Instance.Helper.Translation.Get("enableHat"),
            tooltip: () => ModEntry.Instance.Helper.Translation.Get("enableHat.tooltip"),
            getValue: () => config.EnableHat,
            setValue: value => config.EnableHat = value
        );

        api.AddBoolOption(
            mod: manifest,
            name: () => ModEntry.Instance.Helper.Translation.Get("enableSeasonalClothing"),
            tooltip: () => ModEntry.Instance.Helper.Translation.Get("enableSeasonalClothing.tooltip"),
            getValue: () => config.EnableSeasonalClothing,
            setValue: value => config.EnableSeasonalClothing = value
        );

        void RegisterSpouseGroup(string title, string[] spouses)
        {
            api.StartPage(mod: manifest, pageId: title, pageTitle: () => title);

            foreach (var spouse in spouses)
            {
                foreach (var gender in new[] { "Boy", "Girl" })
                {
                    api.AddTextOption(
                        mod: manifest,
                        name: () => $"[{spouse}] {gender} 헤어 색상",
                        getValue: () => config.Get(spouse, gender, "HairColor"),
                        setValue: v => config.Set(spouse, gender, "HairColor", v),
                        allowedValues: HairOptions.Colors,
                        formatAllowedValue: HairOptions.FormatColor,
                        fieldId: $"{spouse}_{gender}_HairColor",
                        pageId: title
                    );

                    if (gender == "Girl")
                    {
                        api.AddTextOption(
                            mod: manifest,
                            name: () => $"[{spouse}] {gender} 헤어 스타일",
                            getValue: () => config.Get(spouse, gender, "Hair"),
                            setValue: v => config.Set(spouse, gender, "Hair", v),
                            allowedValues: new[] { "Ponytail", "TwinTail", "CherryTwin" },
                            formatAllowedValue: HairOptions.FormatStyle,
                            fieldId: $"{spouse}_{gender}_Hair",
                            pageId: title
                        );
                    }

                    api.AddTextOption(
                        mod: manifest,
                        name: () => $"[{spouse}] {gender} 하의 색상",
                        getValue: () => config.Get(spouse, gender, "BottomsColor"),
                        setValue: v => config.Set(spouse, gender, "BottomsColor", v),
                        allowedValues: BottomsOptions.Colors,
                        formatAllowedValue: BottomsOptions.FormatColor,
                        fieldId: $"{spouse}_{gender}_BottomsColor",
                        pageId: title
                    );
                }
            }
        }

        RegisterSpouseGroup("남자 배우자 설정", SpouseNames.Male);
        RegisterSpouseGroup("여자 배우자 설정", SpouseNames.Female);
    }
}

}

