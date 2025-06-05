using System;
using GenericModConfigMenu;
using StardewModdingAPI;

namespace MyChildCore
{
    /// <summary>
    /// GMCM 연동 전용 API - Entry 전용(헬퍼/매니페스트를 Entry에서 직접 사용)
    /// </summary>
    public static class ChildMenuApi
    {
        public static IGenericModConfigMenuApi Gmcm { get; private set; }

        /// <summary>
        /// GMCM API 인스턴스 미리 받아오기 (Entry에서 1회 호출)
        /// </summary>
        public static void Init(IModHelper helper)
        {
            Gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        public static bool IsLoaded => Gmcm != null;

        public static void Register(IManifest modManifest, Action reset, Action save)
        {
            Gmcm?.Register(
                mod: modManifest,
                reset: reset,
                save: save
            );
        }

        public static void AddSectionTitle(IManifest modManifest, Func<string> name, Func<string> tooltip = null)
        {
            Gmcm?.AddSectionTitle(
                mod: modManifest,
                name: name,
                tooltip: tooltip
            );
        }

        public static void AddDropdown(
            IManifest modManifest,
            Func<string> name, Func<string> tooltip,
            Func<string> getValue, Action<string> setValue,
            Func<string[]> allowedValues, Func<string, string> format = null)
        {
            Gmcm?.AddDropdown(
                mod: modManifest,
                name: name,
                tooltip: tooltip,
                getValue: getValue,
                setValue: setValue,
                allowedValues: allowedValues,
                format: format
            );
        }

        public static void AddBoolOption(
            IManifest modManifest,
            Func<string> name, Func<string> tooltip,
            Func<bool> getValue, Action<bool> setValue)
        {
            Gmcm?.AddBoolOption(
                mod: modManifest,
                name: name,
                tooltip: tooltip,
                getValue: getValue,
                setValue: setValue
            );
        }
    }
}