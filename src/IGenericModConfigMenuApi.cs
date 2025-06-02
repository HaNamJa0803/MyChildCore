using MyChildCore;
using System;
using System.Collections.Generic;

namespace MyChildCore
{
    public interface IGenericModConfigMenuApi
    {
        void Register(object mod, Action reset, Action save, bool titleScreenOnly = false);
        void AddSectionTitle(object mod, Func<string> text, Func<string> tooltip = null);
        void AddDropdown(
            object mod,
            Func<string> name,
            Func<string> tooltip,
            Func<string> getValue,
            Action<string> setValue,
            Func<IEnumerable<string>> allowedValues,
            Func<string, string> format = null
        );
        void AddBoolOption(
            object mod,
            Func<string> name,
            Func<string> tooltip,
            Func<bool> getValue,
            Action<bool> setValue
        );
    }
}