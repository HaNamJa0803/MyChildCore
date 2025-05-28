using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Graphics;

namespace MyChildCore.Utilities
{
    public class ChildData
    {
        private readonly Child innerChild;

        public ChildData(Child child)
        {
            innerChild = child ?? throw new System.ArgumentNullException(nameof(child));
        }

        public string Name
        {
            get => innerChild.Name;
            set => innerChild.Name = value;
        }

        public long ParentID => innerChild.idOfParent.Value;
        public int Age => innerChild.Age;

        // Gender: 0 = 남, 1 = 여
        public int Gender => innerChild.Gender;
        public bool IsMale => innerChild.Gender == 0;
        public bool IsFemale => innerChild.Gender == 1;

        public string SkinTone { get; set; } = "Default";
        public string AppearanceKey { get; set; } = "Default";

        public AnimatedSprite Sprite
        {
            get => innerChild.Sprite;
            set => innerChild.Sprite = value;
        }

        public Child InnerChild => innerChild;
    }
}