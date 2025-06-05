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
        private static IGenericModConfigMenuApi _gmcm;
        private static IManifest _modManifest;

        /// <summary>
        /// GMCM API 인스턴스와 매니페스트 저장 (Entry에서 1회만 호출)
        /// </summary>
        public static void Init(IModHelper helper, IManifest modManifest)
        {
            _gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            _modManifest = modManifest;
        }

        public static bool IsLoaded => _gmcm != null;

        public static void Register(Action reset, Action save)
        {
            _gmcm?.Register(
                mod: _modManifest,
                reset: reset,
                save: save
            );
        }

        public static void AddSectionTitle(Func<string> name, Func<string> tooltip = null)
        {
            _gmcm?.AddSectionTitle(
                mod: _modManifest,
                name: name,
                tooltip: tooltip
            );
        }

        public static void AddDropdown(
            Func<string> name, Func<string> tooltip,
            Func<string> getValue, Action<string> setValue,
            Func<string[]> allowedValues, Func<string, string> format = null)
        {
            _gmcm?.AddDropdown(
                mod: _modManifest,
                name: name,
                tooltip: tooltip,
                getValue: getValue,
                setValue: setValue,
                allowedValues: allowedValues,
                format: format
            );
        }

        public static void AddBoolOption(
            Func<string> name, Func<string> tooltip,
            Func<bool> getValue, Action<bool> setValue)
        {
            _gmcm?.AddBoolOption(
                mod: _modManifest,
                name: name,
                tooltip: tooltip,
                getValue: getValue,
                setValue: setValue
            );
        }
    }
}