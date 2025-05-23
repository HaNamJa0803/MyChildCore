namespace MyChildCore { public static class BottomsOptions { public static string[] Colors => new[] { "Black", "Blue", "Brown", "Emerald", "Green", "Pink", "Red", "Skyblue", "Violet", "Yellow" };

public static string FormatColor(string color) => color switch
    {
        "Black" => "검정색", "Blue" => "파란색", "Brown" => "갈색",
        "Emerald" => "에메랄드", "Green" => "초록색", "Pink" => "분홍색",
        "Red" => "빨간색", "Skyblue" => "하늘색", "Violet" => "보라색",
        "Yellow" => "노란색",
        _ => color
    };
}

}

