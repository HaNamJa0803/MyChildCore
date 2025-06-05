using IManifest = StardewModdingAPI.IManifest;
using System;
using System.Collections.Generic;
using StardewModdingAPI;
using GenericModConfigMenu;

namespace MyChildCore
{
    /// <summary>
    /// GMCM 연동 전용 API - 정적/타입 안전 방식
    /// </summary>
    public class ChildMenuApi
    {
        private readonly IGenericModConfigMenuApi gmcm;
        private readonly IManifest modManifest;

        public ChildMenuApi(IModHelper helper, IManifest manifest)
        {
            gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            modManifest = manifest;
        }

        public bool IsLoaded => gmcm != null;

        public void Register(Action reset, Action save)
        {
            gmcm?.Register(
                mod: modManifest,
                reset: reset,
                save: save
            );
        }

        public void AddSectionTitle(Func<string> name, Func<string> tooltip = null)
        {
            gmcm?.AddSectionTitle(
                mod: modManifest,
                name: name,
                tooltip: tooltip
            );
        }

        public void AddDropdown(
            Func<string> name, Func<string> tooltip,
            Func<string> getValue, Action<string> setValue,
            Func<string[]> allowedValues, Func<string, string> format = null)
        {
            gmcm?.AddDropdown(
                mod: modManifest,
                name: name,
                tooltip: tooltip,
                getValue: getValue,
                setValue: setValue,
                allowedValues: allowedValues,
                format: format
            );
        }

        public void AddBoolOption(
            Func<string> name, Func<string> tooltip,
            Func<bool> getValue, Action<bool> setValue)
        {
            gmcm?.AddBoolOption(
                mod: modManifest,
                name: name,
                tooltip: tooltip,
                getValue: getValue,
                setValue: setValue
            );
        }
    }
}