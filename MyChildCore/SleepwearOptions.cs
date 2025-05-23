using System.Collections.Generic;

namespace MyChildCore
{
    public static class SleepwearOptions
    {
        public static string[] Styles => new[] { "Frog", "Racoon", "LesserPanda", "Sheep", "Shark", "WelshCorgi" };

        public static string FormatStyle(string value) => value switch
        {
            "Frog" => "개구리", "Racoon" => "너구리", "LesserPanda" => "레서판다",
            "Sheep" => "양", "Shark" => "상어", "WelshCorgi" => "웰시코기",
            _ => value
        };

        public static string[] AllColors => new[]
        {
            "BabyBlue", "BabyPink", "Black", "Blue", "Brown", "Choco",
            "Cream", "DarkGreen", "Gray", "Green", "Orange", "Pink",
            "Purple", "White", "Yellow"
        };

        public static string FormatColor(string value) => value switch
        {
            "BabyBlue" => "연하늘색", "BabyPink" => "연분홍색", "Black" => "검정색",
            "Blue" => "파란색", "Brown" => "갈색", "Choco" => "초콜릿색",
            "Cream" => "크림색", "DarkGreen" => "진초록", "Gray" => "회색",
            "Green" => "초록색", "Orange" => "주황색", "Pink" => "분홍색",
            "Purple" => "보라색", "White" => "하얀색", "Yellow" => "노란색",
            _ => value
        };

        private static readonly Dictionary<string, string[]> ColorMap = new()
        {
            { "Frog", new[] { "Black", "Blue", "DarkGreen", "Green", "Pink", "Purple", "White", "Yellow" } },
            { "LesserPanda", new[] { "Black", "Blue", "Brown", "Choco", "Pink", "White", "Yellow" } },
            { "Racoon", new[] { "BabyBlue", "BabyPink", "Brown", "Choco", "Gray", "Pink", "White", "Yellow" } },
            { "Shark", new[] { "Black", "Blue", "Gray", "Pink", "Purple", "White", "Yellow" } },
            { "Sheep", new[] { "Black", "Blue", "Brown", "Cream", "Pink", "White", "Yellow" } },
            { "WelshCorgi", new[] { "Blue", "Brown", "Choco", "Gray", "Orange", "Pink", "White", "Yellow" } },
        };

        public static string[] GetFilteredColors(string style)
            => ColorMap.TryGetValue(style, out var colors) ? colors : AllColors;
    }
}