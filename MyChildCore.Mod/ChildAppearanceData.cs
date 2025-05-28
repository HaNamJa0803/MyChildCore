using StardewValley.Characters;

namespace MyChildCore.Mod
{
    public class ChildAppearanceData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public bool IsMale { get; set; }
        public string AppearanceKey { get; set; }

        public string UniqueKey => $"{Name}_{ParentID}";

        public static string GetUniqueKey(ChildAppearanceData data)
            => data == null ? "" : $"{data.Name}_{data.ParentID}";

        // 생성자: Child → 데이터 객체
        public ChildAppearanceData(Child child)
        {
            Name = child?.Name ?? "";
            ParentID = child?.idOfParent?.Value ?? 0;
            IsMale = child?.Gender == 0;
            AppearanceKey = "";
        }
        public ChildAppearanceData() { } // 빈 생성자 (역직렬화용)
    }
}