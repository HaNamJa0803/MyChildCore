using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MyChildCore.UI
{
    public class ChildConfigMenu : IClickableMenu
    {
        private readonly List<string> spouses;
        private readonly Action onClose;
        private int selectedSpouse = 0;
        private int selectedGender = 0; // 0: 남, 1: 여

        public ChildConfigMenu(List<string> spouseList, Action onClose, string focusSpouse = null)
            : base(Game1.viewport.Width / 2 - 320, Game1.viewport.Height / 2 - 360, 640, 720, true)
        {
            this.spouses = spouseList;
            this.onClose = onClose;
            if (focusSpouse != null)
                selectedSpouse = spouses.IndexOf(focusSpouse);
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);
            b.DrawString(Game1.dialogueFont, "자녀 외형 커스터마이즈", new Vector2(xPositionOnScreen + 60, yPositionOnScreen + 32), Color.Black);

            // 배우자 탭
            int spouseY = yPositionOnScreen + 80;
            for (int i = 0; i < spouses.Count; i++)
            {
                var tabRect = new Rectangle(xPositionOnScreen + 40, spouseY + i * 36, 180, 32);
                var color = (i == selectedSpouse) ? Color.White : Color.Gray;
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                    tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height, color, 1f);
                b.DrawString(Game1.smallFont, spouses[i], new Vector2(tabRect.X + 8, tabRect.Y + 6), Color.Black);
            }

            // 성별 탭
            int tabX = xPositionOnScreen + 260;
            int tabY = yPositionOnScreen + 80;
            for (int g = 0; g < 2; g++)
            {
                var rect = new Rectangle(tabX + g * 140, tabY, 130, 36);
                var color = (g == selectedGender) ? Color.White : Color.Gray;
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                    rect.X, rect.Y, rect.Width, rect.Height, color, 1f);
                b.DrawString(Game1.smallFont, (g == 0 ? "남자 자녀" : "여자 자녀"), new Vector2(rect.X + 8, rect.Y + 8), Color.Black);
            }

            // === 파츠 옵션 ===
            int optY = yPositionOnScreen + 140;
            string spouseName = spouses[selectedSpouse];
            // 실제로는 아래 반복문 등으로 드롭다운/슬라이더 등 옵션 뿌리기
            DrawOption(b, "헤어", optY, 0, spouseName);
            DrawOption(b, "눈", optY + 40, 1, spouseName);
            DrawOption(b, "피부", optY + 80, 2, spouseName);
            DrawOption(b, "넥칼라", optY + 120, 3, spouseName);
            // ...실제 파츠/색상/스타일 반복문으로 자동화!...

            // 저장/닫기 버튼
            DrawButton(b, xPositionOnScreen + 120, yPositionOnScreen + height - 80, 120, 48, "저장");
            DrawButton(b, xPositionOnScreen + 300, yPositionOnScreen + height - 80, 120, 48, "닫기");
            drawMouse(b);
        }

        private void DrawOption(SpriteBatch b, string label, int y, int idx, string spouseName)
        {
            b.DrawString(Game1.smallFont, label, new Vector2(xPositionOnScreen + 260, y), Color.Black);
            // 실제 구현에서는 Config/DropdownConfig에서 값을 읽어서 좌/우 화살표, 값 표시 등 처리
        }

        private void DrawButton(SpriteBatch b, int x, int y, int w, int h, string label)
        {
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, w, h, Color.White, 1f);
            b.DrawString(Game1.smallFont, label, new Vector2(x + 20, y + 14), Color.Black);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            // 배우자 탭
            int spouseY = yPositionOnScreen + 80;
            for (int i = 0; i < spouses.Count; i++)
            {
                if (new Rectangle(xPositionOnScreen + 40, spouseY + i * 36, 180, 32).Contains(x, y))
                {
                    selectedSpouse = i;
                    Game1.playSound("smallSelect");
                    return;
                }
            }
            // 성별 탭
            int tabX = xPositionOnScreen + 260;
            int tabY = yPositionOnScreen + 80;
            for (int g = 0; g < 2; g++)
            {
                if (new Rectangle(tabX + g * 140, tabY, 130, 36).Contains(x, y))
                {
                    selectedGender = g;
                    Game1.playSound("smallSelect");
                    return;
                }
            }
            // 저장/닫기 버튼
            if (new Rectangle(xPositionOnScreen + 120, yPositionOnScreen + height - 80, 120, 48).Contains(x, y) ||
                new Rectangle(xPositionOnScreen + 300, yPositionOnScreen + height - 80, 120, 48).Contains(x, y))
            {
                Game1.playSound("smallSelect");
                onClose?.Invoke();
                exitThisMenu();
                return;
            }
            // 옵션 화살표 등 클릭 로직도 실제 구현에선 추가
        }
    }
}