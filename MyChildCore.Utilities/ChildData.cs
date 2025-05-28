using System;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// Child(자녀) 정보/외형/연동 DTO (1.6.10+ 기준)
    /// </summary>
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; } // 0=남, 1=여
        public bool DarkSkinned { get; set; }
        public string CustomSpriteKey { get; set; } = "";

        public ChildData(Child child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            Name = child.Name;
            ParentID = child.idOfParent?.Value ?? 0;
            Gender = (int)child.Gender; // 0=Male, 1=Female (SDV Child.Gender enum)
            DarkSkinned = child.darkSkinned?.Value ?? false;
            // 필요시 child의 외형 Key 또는 커스텀 속성도 자유롭게 확장!
        }
    }
}