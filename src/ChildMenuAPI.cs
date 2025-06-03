using System;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using GenericModConfigMenu;

namespace MyChildCore
{
    // GMCM 연동 전용 API - 동적 호출 방식
    public class ChildMenuApi
    {
        private readonly object gmcm;
        private readonly object modManifest;

        public ChildMenuApi(IModHelper helper, ModManifest manifest)
        {
            gmcm = helper.ModRegistry.GetApi<object>("spacechase0.GenericModConfigMenu");
            modManifest = manifest;
        }

        public bool IsLoaded => gmcm != null;

        public void Register(Action reset, Action save)
        {
            gmcm?.GetType().GetMethod("Register")?.Invoke(gmcm, new object[] { modManifest, reset, save, false });
        }

        public void AddSectionTitle(Func<string> name, Func<string> tooltip = null)
        {
            gmcm?.GetType().GetMethod("AddSectionTitle")?.Invoke(gmcm, new object[] { modManifest, name, tooltip });
        }

        public void AddDropdown(
            Func<string> name, Func<string> tooltip,
            Func<string> getValue, Action<string> setValue,
            Func<string[]> allowedValues, Func<string, string> format = null)
        {
            gmcm?.GetType().GetMethod("AddDropdown")?.Invoke(gmcm, new object[]
            {
                modManifest, name, tooltip, getValue, setValue, allowedValues, format
            });
        }

        public void AddBoolOption(
            Func<string> name, Func<string> tooltip,
            Func<bool> getValue, Action<bool> setValue)
        {
            gmcm?.GetType().GetMethod("AddBoolOption")?.Invoke(gmcm, new object[]
            {
                modManifest, name, tooltip, getValue, setValue
            });
        }
    }
}