using System.Collections.Generic;
using System.Linq;

namespace MyChildCore
{
    public static class SleepwearTranslations
    {
        public static readonly Dictionary<string, string> StyleKorean = new()
        {
            { "1", "강아지" },
            { "2", "양" },
            { "3", "상어" },
            { "4", "레서판다" },
            { "5", "개구리" },
            { "6", "라쿤" }
        };

        public static readonly Dictionary<string, string> ColorKorean = new()
        {
            { "1", "검정" }, { "2", "갈색" }, { "3", "분홍" }, { "4", "하늘" },
            { "5", "초록" }, { "6", "노랑" }, { "7", "보라" },
            { "8", "흰색" }, { "9", "오렌지" }, { "10", "남색" },
            { "11", "연두" }, { "12", "회색" }, { "13", "빨강" },
            { "14", "청록" }, { "15", "금색" }
        };

        public static string GetStyleDisplay(string key) =>
            StyleKorean.TryGetValue(key, out var value) ? value : key;

        public static string GetColorDisplay(string key) =>
            ColorKorean.TryGetValue(key, out var value) ? value : key;

        public static string GetStyleKey(string display) =>
            StyleKorean.FirstOrDefault(pair => pair.Value == display).Key ?? display;

        public static string GetColorKey(string display) =>
            ColorKorean.FirstOrDefault(pair => pair.Value == display).Key ?? display;
    }
}
