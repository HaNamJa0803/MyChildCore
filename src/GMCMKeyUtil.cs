namespace MyChildCore.Utilities
{
    public static class GMCMKeyUtil
    {
        // 부모 이름 + 성별로 고유 키 생성 (중복 예외 방지)
        public static string GetChildKey(Child child)
        {
            string parent = ChildManager.GetSpouseName(child);
            string gender = ((int)child.Gender == 0) ? "male" : "female";
            return $"{parent}-{gender}";
        }
    }
}