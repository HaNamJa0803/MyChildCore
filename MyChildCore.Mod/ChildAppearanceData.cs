namespace MyChildCore.Utilities
{
    public class ChildAppearanceData
    {
        public string UniqueKey { get; set; } // ex) 이름_부모ID
        public string Name { get; set; }
        public long ParentID { get; set; }
        public bool IsMale { get; set; }
        public string SkinTone { get; set; }
        public string AppearanceKey { get; set; }
        // 필요시 추가 필드 확장
    }
}