using System;
using GenericModConfigMenu;
using StardewModdingAPI;

namespace MyChildCore
{
    public static class ChildMenuApi
    {
        private static IGenericModConfigMenuApi _gmcm;
        private static IModHelper _helper;

        // Entry에서 한번만 호출
        public static void Init(IModHelper helper)
        {
            _helper = helper;
            _gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        public static bool IsLoaded => _gmcm != null;

        // 내부에서만 _helper.ModManifest 사용 (IManifest 타입 노출 없음)
        private static object ModManifest => _helper?.ModManifest;

        public static void Register(Action reset, Action save)
        {
            _gmcm?.Register(_helper.ModManifest, reset, save);
        }

        public static void AddSectionTitle(Func<string> name, Func<string> tooltip = null)
        {
            _gmcm?.AddSectionTitle(_helper.ModManifest, name, tooltip);
        }

        public static void AddDropdown(
            Func<string> name, Func<string> tooltip,
            Func<string> getValue, Action<string> setValue,
            Func<string[]> allowedValues, Func<string, string> format = null)
        {
            _gmcm?.AddDropdown(_helper.ModManifest, name, tooltip, getValue, setValue, allowedValues, format);
        }

        public static void AddBoolOption(
            Func<string> name, Func<string> tooltip,
            Func<bool> getValue, Action<bool> setValue)
        {
            _gmcm?.AddBoolOption(_helper.ModManifest, name, tooltip, getValue, setValue);
        }
    }
}