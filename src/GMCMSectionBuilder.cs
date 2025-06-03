using System;
using System.Collections.Generic;
using StardewModdingAPI;
using MyChildCore;

namespace MyChildCore
{
    /// <summary>
    /// GMCM 설정 탭(섹션) 자동 빌더 - 배우자별/자녀 성별별, 옵션별로 그룹화 지원 (i18n 연동, 동적 호출)
    /// </summary>
    public static class GMCMSectionBuilder
    {
        public static void BuildSections_Dynamic(
            object gmcm,
            object mod,
            ITranslationHelper i18n,
            DropdownConfig config,
            Action<string, bool> onChanged // (spouse, isMale) 콜백
        )
        {
            var gmcmType = gmcm.GetType();

            var addSectionTitle = gmcmType.GetMethod("AddSectionTitle");
            var addDropdown = gmcmType.GetMethod("AddDropdown");
            var addBoolOption = gmcmType.GetMethod("AddBoolOption");

            foreach (var spouse in DropdownConfig.SpouseNames)
            {
                // 섹션 타이틀 (배우자별)
                addSectionTitle?.Invoke(gmcm, new object[] {
                    mod,
                    (Func<string>)(() => i18n.Get($"section.spouse.{spouse}")),
                    (Func<string>)null
                });

                if (!config.SpouseConfigs.ContainsKey(spouse))
                    config.SpouseConfigs[spouse] = new SpouseChildConfig();

                var spouseConfig = config.SpouseConfigs[spouse];

                // === 여아(딸) 옵션 ===
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.hairstyle")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.hairstyle")),
                    (Func<string>)(() => spouseConfig.GirlHairStyle),
                    (Action<string>)(v => { spouseConfig.GirlHairStyle = v; onChanged(spouse, false); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.GirlHairStyles),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.skirt")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.skirt")),
                    (Func<string>)(() => spouseConfig.GirlSkirt),
                    (Action<string>)(v => { spouseConfig.GirlSkirt = v; onChanged(spouse, false); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.SkirtOptions),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.skirtcolor")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.skirtcolor")),
                    (Func<string>)(() => spouseConfig.GirlSkirtColor.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlSkirtColor = idx; onChanged(spouse, false); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.SkirtColorCount; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.shoes")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.shoes")),
                    (Func<string>)(() => spouseConfig.GirlShoes),
                    (Action<string>)(v => { spouseConfig.GirlShoes = v; onChanged(spouse, false); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.ShoesOptions),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.shoescolor")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.shoescolor")),
                    (Func<string>)(() => spouseConfig.GirlShoesColor.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlShoesColor = idx; onChanged(spouse, false); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.ShoesColorCount; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.neckcollar")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.neckcollar")),
                    (Func<string>)(() => spouseConfig.GirlNeckCollar),
                    (Action<string>)(v => { spouseConfig.GirlNeckCollar = v; onChanged(spouse, false); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.NeckCollarOptions),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.neckcollarcolor")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.neckcollarcolor")),
                    (Func<string>)(() => spouseConfig.GirlNeckCollarColor.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlNeckCollarColor = idx; onChanged(spouse, false); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.NeckCollarColorCount; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.pajama")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.pajama")),
                    (Func<string>)(() => spouseConfig.GirlPajamaStyle),
                    (Action<string>)(v => { spouseConfig.GirlPajamaStyle = v; onChanged(spouse, false); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.PajamaStyles),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.girl.pajamacolor")),
                    (Func<string>)(() => i18n.Get("tooltip.girl.pajamacolor")),
                    (Func<string>)(() => spouseConfig.GirlPajamaColorIndex.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.GirlPajamaColorIndex = idx; onChanged(spouse, false); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var style = spouseConfig.GirlPajamaStyle;
                        int max = DropdownConfig.PajamaColorMax.TryGetValue(style, out int c) ? c : 8;
                        var list = new List<string>();
                        for (int i = 1; i <= max; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });

                // === 남아(아들) 옵션 ===
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.pants")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.pants")),
                    (Func<string>)(() => spouseConfig.BoyPants),
                    (Action<string>)(v => { spouseConfig.BoyPants = v; onChanged(spouse, true); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.PantsOptions),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.pantscolor")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.pantscolor")),
                    (Func<string>)(() => spouseConfig.BoyPantsColor.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyPantsColor = idx; onChanged(spouse, true); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.PantsColorCount; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.shoes")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.shoes")),
                    (Func<string>)(() => spouseConfig.BoyShoes),
                    (Action<string>)(v => { spouseConfig.BoyShoes = v; onChanged(spouse, true); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.ShoesOptions),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.shoescolor")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.shoescolor")),
                    (Func<string>)(() => spouseConfig.BoyShoesColor.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyShoesColor = idx; onChanged(spouse, true); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.ShoesColorCount; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.neckcollar")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.neckcollar")),
                    (Func<string>)(() => spouseConfig.BoyNeckCollar),
                    (Action<string>)(v => { spouseConfig.BoyNeckCollar = v; onChanged(spouse, true); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.NeckCollarOptions),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.neckcollarcolor")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.neckcollarcolor")),
                    (Func<string>)(() => spouseConfig.BoyNeckCollarColor.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyNeckCollarColor = idx; onChanged(spouse, true); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var list = new List<string>();
                        for (int i = 1; i <= DropdownConfig.NeckCollarColorCount; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.pajama")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.pajama")),
                    (Func<string>)(() => spouseConfig.BoyPajamaStyle),
                    (Action<string>)(v => { spouseConfig.BoyPajamaStyle = v; onChanged(spouse, true); }),
                    (Func<IEnumerable<string>>)(() => DropdownConfig.PajamaStyles),
                    null
                });
                addDropdown?.Invoke(gmcm, new object[]
                {
                    mod,
                    (Func<string>)(() => i18n.Get("option.boy.pajamacolor")),
                    (Func<string>)(() => i18n.Get("tooltip.boy.pajamacolor")),
                    (Func<string>)(() => spouseConfig.BoyPajamaColorIndex.ToString()),
                    (Action<string>)(v => { if (int.TryParse(v, out int idx)) { spouseConfig.BoyPajamaColorIndex = idx; onChanged(spouse, true); } }),
                    (Func<IEnumerable<string>>)(() =>
                    {
                        var style = spouseConfig.BoyPajamaStyle;
                        int max = DropdownConfig.PajamaColorMax.TryGetValue(style, out int c) ? c : 8;
                        var list = new List<string>();
                        for (int i = 1; i <= max; i++) list.Add(i.ToString());
                        return list;
                    }),
                    null
                });
            }

            // === 전역 기능 옵션 ===
            addBoolOption?.Invoke(gmcm, new object[]
            {
                mod,
                (Func<string>)(() => i18n.Get("option.enable_pajama")),
                (Func<string>)(() => i18n.Get("tooltip.enable_pajama")),
                (Func<bool>)(() => config.EnablePajama),
                (Action<bool>)(v => config.EnablePajama = v)
            });
            addBoolOption?.Invoke(gmcm, new object[]
            {
                mod,
                (Func<string>)(() => i18n.Get("option.enable_festival")),
                (Func<string>)(() => i18n.Get("tooltip.enable_festival")),
                (Func<bool>)(() => config.EnableFestival),
                (Action<bool>)(v => config.EnableFestival = v)
            });
        }
    }
}