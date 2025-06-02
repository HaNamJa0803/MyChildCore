using System;
using System.Collections.Generic;
using StardewModdingAPI;
using MyChildCore.Utilities;

namespace MyChildCore.GMCM
{
    /// <summary>
    /// GMCM 설정 탭(섹션) 자동 빌더 - 배우자별/자녀 성별별, 옵션별로 그룹화 지원 (i18n 연동)
    /// </summary>
    public static class GMCMSectionBuilder
    {
        public static void BuildSections(
            IGenericModConfigMenuApi gmcm,
            object mod,
            ITranslationHelper i18n,
            DropdownConfig config,
            Action<string, bool> onChanged // (spouse, isMale) 콜백
        )
        {
            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                // 섹션 타이틀 (배우자별)
                gmcm.AddSectionTitle(
                    mod,
                    () => i18n.Get($"section.spouse.{spouse}"),
                    () => null
                );

                if (!config.SpouseConfigs.ContainsKey(spouse))
                    config.SpouseConfigs[spouse] = new SpouseChildConfig();

                var spouseConfig = config.SpouseConfigs[spouse];

                // === 여아(딸) 옵션 ===
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.hairstyle"),
                    tooltip: () => i18n.Get("tooltip.girl.hairstyle"),
                    getValue: () => spouseConfig.GirlHairStyle,
                    setValue: v => { spouseConfig.GirlHairStyle = v; onChanged(spouse, false); },
                    allowedValues: () => DropdownConfig.GirlHairStyles
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.skirt"),
                    tooltip: () => i18n.Get("tooltip.girl.skirt"),
                    getValue: () => spouseConfig.GirlSkirt,
                    setValue: v => { spouseConfig.GirlSkirt = v; onChanged(spouse, false); },
                    allowedValues: () => DropdownConfig.SkirtOptions
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.skirtcolor"),
                    tooltip: () => i18n.Get("tooltip.girl.skirtcolor"),
                    getValue: () => spouseConfig.GirlSkirtColor.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlSkirtColor = idx; onChanged(spouse, false); } },
                    allowedValues: () =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.SkirtColorCount; i++) list.Add(i.ToString());
                        return list;
                    }
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.shoes"),
                    tooltip: () => i18n.Get("tooltip.girl.shoes"),
                    getValue: () => spouseConfig.GirlShoes,
                    setValue: v => { spouseConfig.GirlShoes = v; onChanged(spouse, false); },
                    allowedValues: () => DropdownConfig.ShoesOptions
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.shoescolor"),
                    tooltip: () => i18n.Get("tooltip.girl.shoescolor"),
                    getValue: () => spouseConfig.GirlShoesColor.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlShoesColor = idx; onChanged(spouse, false); } },
                    allowedValues: () =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.ShoesColorCount; i++) list.Add(i.ToString());
                        return list;
                    }
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.neckcollar"),
                    tooltip: () => i18n.Get("tooltip.girl.neckcollar"),
                    getValue: () => spouseConfig.GirlNeckCollar,
                    setValue: v => { spouseConfig.GirlNeckCollar = v; onChanged(spouse, false); },
                    allowedValues: () => DropdownConfig.NeckCollarOptions
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.neckcollarcolor"),
                    tooltip: () => i18n.Get("tooltip.girl.neckcollarcolor"),
                    getValue: () => spouseConfig.GirlNeckCollarColor.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlNeckCollarColor = idx; onChanged(spouse, false); } },
                    allowedValues: () =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.NeckCollarColorCount; i++) list.Add(i.ToString());
                        return list;
                    }
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.pajama"),
                    tooltip: () => i18n.Get("tooltip.girl.pajama"),
                    getValue: () => spouseConfig.GirlPajamaStyle,
                    setValue: v => { spouseConfig.GirlPajamaStyle = v; onChanged(spouse, false); },
                    allowedValues: () => DropdownConfig.PajamaStyles
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.girl.pajamacolor"),
                    tooltip: () => i18n.Get("tooltip.girl.pajamacolor"),
                    getValue: () => spouseConfig.GirlPajamaColorIndex.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlPajamaColorIndex = idx; onChanged(spouse, false); } },
                    allowedValues: () =>
                    {
                        var style = spouseConfig.GirlPajamaStyle;
                        int max = DropdownConfig.PajamaColorMax.TryGetValue(style, out int c) ? c : 8;
                        var list = new List<string>();
                        for (int i = 1; i <= max; i++) list.Add(i.ToString());
                        return list;
                    }
                );

                // === 남아(아들) 옵션 ===
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.pants"),
                    tooltip: () => i18n.Get("tooltip.boy.pants"),
                    getValue: () => spouseConfig.BoyPants,
                    setValue: v => { spouseConfig.BoyPants = v; onChanged(spouse, true); },
                    allowedValues: () => DropdownConfig.PantsOptions
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.pantscolor"),
                    tooltip: () => i18n.Get("tooltip.boy.pantscolor"),
                    getValue: () => spouseConfig.BoyPantsColor.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyPantsColor = idx; onChanged(spouse, true); } },
                    allowedValues: () =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.PantsColorCount; i++) list.Add(i.ToString());
                        return list;
                    }
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.shoes"),
                    tooltip: () => i18n.Get("tooltip.boy.shoes"),
                    getValue: () => spouseConfig.BoyShoes,
                    setValue: v => { spouseConfig.BoyShoes = v; onChanged(spouse, true); },
                    allowedValues: () => DropdownConfig.ShoesOptions
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.shoescolor"),
                    tooltip: () => i18n.Get("tooltip.boy.shoescolor"),
                    getValue: () => spouseConfig.BoyShoesColor.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyShoesColor = idx; onChanged(spouse, true); } },
                    allowedValues: () =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.ShoesColorCount; i++) list.Add(i.ToString());
                        return list;
                    }
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.neckcollar"),
                    tooltip: () => i18n.Get("tooltip.boy.neckcollar"),
                    getValue: () => spouseConfig.BoyNeckCollar,
                    setValue: v => { spouseConfig.BoyNeckCollar = v; onChanged(spouse, true); },
                    allowedValues: () => DropdownConfig.NeckCollarOptions
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.neckcollarcolor"),
                    tooltip: () => i18n.Get("tooltip.boy.neckcollarcolor"),
                    getValue: () => spouseConfig.BoyNeckCollarColor.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyNeckCollarColor = idx; onChanged(spouse, true); } },
                    allowedValues: () =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.NeckCollarColorCount; i++) list.Add(i.ToString());
                        return list;
                    }
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.pajama"),
                    tooltip: () => i18n.Get("tooltip.boy.pajama"),
                    getValue: () => spouseConfig.BoyPajamaStyle,
                    setValue: v => { spouseConfig.BoyPajamaStyle = v; onChanged(spouse, true); },
                    allowedValues: () => DropdownConfig.PajamaStyles
                );
                gmcm.AddDropdown(
                    mod,
                    name: () => i18n.Get("option.boy.pajamacolor"),
                    tooltip: () => i18n.Get("tooltip.boy.pajamacolor"),
                    getValue: () => spouseConfig.BoyPajamaColorIndex.ToString(),
                    setValue: v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyPajamaColorIndex = idx; onChanged(spouse, true); } },
                    allowedValues: () =>
                    {
                        var style = spouseConfig.BoyPajamaStyle;
                        int max = DropdownConfig.PajamaColorMax.TryGetValue(style, out int c) ? c : 8;
                        var list = new List<string>();
                        for (int i = 1; i <= max; i++) list.Add(i.ToString());
                        return list;
                    }
                );
            }

            // === 전역 기능 옵션 ===
            gmcm.AddBoolOption(
                mod,
                name: () => i18n.Get("option.enable_pajama"),
                tooltip: () => i18n.Get("tooltip.enable_pajama"),
                getValue: () => config.EnablePajama,
                setValue: v => config.EnablePajama = v
            );
            gmcm.AddBoolOption(
                mod,
                name: () => i18n.Get("option.enable_festival"),
                tooltip: () => i18n.Get("tooltip.enable_festival"),
                getValue: () => config.EnableFestival,
                setValue: v => config.EnableFestival = v
            );
        }
    }
}