using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;

namespace MyChildCore.Utilities
{
    /// <summary>
    /// 자녀 정보 래퍼(외형 커스텀/세이브 확장 대비)
    /// </summary>
    public class ChildData
    {
        private readonly Child _child;

        public ChildData(Child child)
        {
            _child = child ?? throw new System.ArgumentNullException(nameof(child));
            SkinTone = "Default";
            AppearanceKey = "Default";
        }

        public string Name
        {
            get => _child.Name;
            set => _child.Name = value;
        }

        public long ParentID => _child.idOfParent.Value;
        public int Age => _child.Age;
        public int Gender => _child.Gender; // 0=남 1=여
        public bool IsMale => _child.Gender == 0;
        public bool IsFemale => _child.Gender == 1;

        // --- 외형 커스텀/저장용 확장 ---
        public string SkinTone { get; set; }
        public string AppearanceKey { get; set; }

        // SDV 애니메이션 스프라이트 직접 접근/변경
        public AnimatedSprite Sprite
        {
            get => _child.Sprite;
            set => _child.Sprite = value;
        }

        // 내부 Child 인스턴스 반환 (확장성, 직접참조 필요시)
        public Child InnerChild => _child;
    }
}