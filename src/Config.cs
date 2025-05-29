using System;
using System.Collections.Generic;

namespace MyChildCore.Config
{
    public class ChildAppearanceConfig
    {
        public string HairStyle { get; set; } = "체리트윈";
        public int BottomIndex { get; set; } = 1; // 1~10
        public int ShoesIndex { get; set; } = 1;  // 1~4
        public int NeckIndex { get; set; } = 1;   // 1~26
        public string PajamaStyle { get; set; } = "기본";
        public string PajamaColor { get; set; } = "기본";
    }

    public class ModConfig
    {
        public Dictionary<string, ChildAppearanceConfig> ChildrenConfigs { get; set; } = new();

        public ModConfig()
        {
            string[] spouses = new string[]
            {
                "Abigail", "Alissa", "Blair", "Corine", "Daia", "Emily", "Faye", "Flor",
                "Haley", "Irene", "Kiarra", "Leah", "Maddie", "Maru", "Paula", "Penny", "Ysabelle"
            };

            foreach (var spouse in spouses)
            {
                ChildrenConfigs[$"{spouse}_Son"] = new ChildAppearanceConfig
                {
                    HairStyle = "숏"
                };

                ChildrenConfigs[$"{spouse}_Daughter"] = new ChildAppearanceConfig
                {
                    HairStyle = "체리트윈"
                };
            }
        }
    }
}