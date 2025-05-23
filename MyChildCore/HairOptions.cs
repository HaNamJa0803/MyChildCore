namespace MyChildCore { public static class HairOptions { public static string[] Styles => new[] { "Short", "Ponytail", "TwinTail", "CherryTwin" };

public static string FormatStyle(string value) => value switch
    {
        "Short" => "단발",
        "Ponytail" => "포니테일",
        "TwinTail" => "양갈래",
        "CherryTwin" => "체리 양갈래",
        _ => value
    };

    public static string[] Colors => new[]
    {
        "Abigail", "Alex", "BabyBlue", "BabyPink", "Beige", "Black",
        "Elliott", "Emily", "Gray", "Green", "Haley", "Harvey",
        "Leah", "Maru", "Obsidian", "Orange", "Penny", "Platinum",
        "Red", "Sam", "Sebastian", "Shane", "White"
    };

    public static string FormatColor(string color) => color switch
    {
        "Abigail" => "아비게일", "Alex" => "알렉스", "BabyBlue" => "연하늘색",
        "BabyPink" => "연분홍색", "Beige" => "베이지", "Black" => "검정색",
        "Elliott" => "엘리엇", "Emily" => "에밀리", "Gray" => "회색",
        "Green" => "초록색", "Haley" => "헤일리", "Harvey" => "하비",
        "Leah" => "리아", "Maru" => "마루", "Obsidian" => "흑요석",
        "Orange" => "주황색", "Penny" => "페니", "Platinum" => "플래티넘",
        "Red" => "빨간색", "Sam" => "샘", "Sebastian" => "세바스찬",
        "Shane" => "셰인", "White" => "하얀색",
        _ => color
    };
}

}

