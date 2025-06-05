using System;
using GenericModConfigMenu;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class ChildMenuApi
    {
        private static IGenericModConfigMenuApi _gmcm;
        private static IModHelper _helper;

        /// <summary>
        /// Entry에서 한 번만 초기화
        /// </summary>
        public static void Init(IModHelper helper)
        {
            _helper = helper;
            _gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        public static bool IsLoaded => _gmcm != null;
        private static IManifest ModManifest => _helper?.ModRegistry.ModMetadata.Manifest;

        public static void Register(Action reset, Action save)
        {
            _gmcm?.Register(ModManifest, reset, save);
        }
        public static void AddSectionTitle(Func<string> name, Func<string> tooltip = null)
        {
            _gmcm?.AddSectionTitle(ModManifest, name, tooltip);
        }
        public static void AddDropdown(
            Func<string> name, Func<string> tooltip,
            Func<string> getValue, Action<string> setValue,
            Func<string[]> allowedValues, Func<string, string> format = null)
        {
            _gmcm?.AddDropdown(ModManifest, name, tooltip, getValue, setValue, allowedValues, format);
        }
        public static void AddBoolOption(
            Func<string> name, Func<string> tooltip,
            Func<bool> getValue, Action<bool> setValue)
        {
            _gmcm?.AddBoolOption(ModManifest, name, tooltip, getValue, setValue);
        }
    }
}