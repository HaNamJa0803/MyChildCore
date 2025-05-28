using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Characters;

namespace MyChildCore.Utilities
{
    public class ChildData
    {
        public string Name { get; set; }
        public long ParentID { get; set; }
        public int Gender { get; set; }
        public bool DarkSkinned { get; set; }
        public string CustomSpriteKey { get; set; }

        public ChildData(Child child)
        {
            Name = child.Name;
            ParentID = child.idOfParent.Value;
            Gender = (int)child.Gender;
            DarkSkinned = child.darkSkinned?.Value ?? false;
            CustomSpriteKey = ""; // 필요시 커스텀 스프라이트 키 관리
        }
    }
}