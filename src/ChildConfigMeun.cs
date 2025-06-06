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
        private string selectedSpouse;
        private int selectedTab = 0; // 0: 남자, 1: 여자

        // 옵션 상태(실제 컨피그 연동)
        private bool enableMod, enablePajama, enableFestival;

        // 스크롤 상태
        private int scrollOffset = 0;
        private const int scrollStep = 40;
        private int maxScroll = 0;

        public ChildConfigMenu(List<string> spouseList, Action onClose, string initialSpouse = null)
            : base(Game1.viewport.Width / 2 - 480, Game1.viewport.Height / 2 - 540, 960, 1080, true)
        {
            this.spouses = spouseList;
            this.onClose = onClose;
            this.selectedSpouse = initialSpouse ?? spouseList[0];
            enableMod = MyChildCore.ModEntry.Config.EnableMod;
            enablePajama = MyChildCore.ModEntry.Config.EnablePajama;
            enableFestival = MyChildCore.ModEntry.Config.EnableFestival;
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);
            b.DrawString(Game1.dialogueFont, "자녀 외형 커스터마이즈", new Vector2(xPositionOnScreen + 64, yPositionOnScreen + 32), Color.Black);

            int yBase = yPositionOnScreen + 110;
            int y = yBase - scrollOffset;

            // 글로벌 옵션 (GMCM 체크박스)
            DrawCheckbox(b, xPositionOnScreen + 72, y, "모드 활성화", enableMod, 0);
            DrawCheckbox(b, xPositionOnScreen + 320, y, "잠옷 활성화", enablePajama, 1);
            DrawCheckbox(b, xPositionOnScreen + 600, y, "축제복 활성화", enableFestival, 2);
            y += 70;

            // 배우자 목록 (GMCM 섹션 스타일)
            b.DrawString(Game1.smallFont, "배우자 선택", new Vector2(xPositionOnScreen + 70, y), Color.Black);
            y += 54;

            int spouseListStartY = y;
            for (int i = 0; i < spouses.Count; i++)
            {
                bool selected = spouses[i] == selectedSpouse;
                var boxColor = selected ? Color.White : new Color(240, 240, 240);
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                    xPositionOnScreen + 70, y, 260, 40, boxColor, 1f);
                b.DrawString(Game1.smallFont, spouses[i], new Vector2(xPositionOnScreen + 95, y + 8), Color.Black);
                y += 50;
            }
            int spouseListEndY = y;

            // 자녀 설정 탭 (GMCM 옵션 스타일)
            y += 26;
            string[] tabs = { "👦 남자 자녀", "👧 여자 자녀" };
            for (int i = 0; i < 2; i++)
            {
                var color = (i == selectedTab) ? Color.White : new Color(240,240,240);
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                    xPositionOnScreen + 70 + i * 170, y, 150, 40, color, 1f);
                b.DrawString(Game1.smallFont, tabs[i], new Vector2(xPositionOnScreen + 90 + i * 170, y + 10), Color.Black);
            }

            // 실제 옵션들(예시: 헤어/눈/피부/넥칼라/하의/신발 등)
            int optY = y + 70;
            DrawArrowOption(b, xPositionOnScreen + 70, optY, "헤어", 0);
            DrawArrowOption(b, xPositionOnScreen + 70, optY + 55, "눈", 1);
            DrawArrowOption(b, xPositionOnScreen + 70, optY + 110, "피부", 2);
            DrawArrowOption(b, xPositionOnScreen + 70, optY + 165, "넥칼라", 3);

            // 스크롤바 그리기 (우측)
            DrawScrollbar(b, spouseListStartY, spouseListEndY);

            // 하단 버튼 (GMCM 스타일)
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                xPositionOnScreen + width / 2 - 220, yPositionOnScreen + height - 110, 120, 48, Color.White, 1f);
            b.DrawString(Game1.smallFont, "저장", new Vector2(xPositionOnScreen + width / 2 - 195, yPositionOnScreen + height - 98), Color.Black);

            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                xPositionOnScreen + width / 2 + 100, yPositionOnScreen + height - 110, 120, 48, Color.White, 1f);
            b.DrawString(Game1.smallFont, "닫기", new Vector2(xPositionOnScreen + width / 2 + 125, yPositionOnScreen + height - 98), Color.Black);

            drawMouse(b);
        }

        private void DrawCheckbox(SpriteBatch b, int x, int y, string label, bool value, int optionIdx)
        {
            b.Draw(Game1.mouseCursors, new Vector2(x, y), new Rectangle(227, 425, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            if (value)
                b.Draw(Game1.mouseCursors, new Vector2(x, y), new Rectangle(236, 425, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.DrawString(Game1.smallFont, label, new Vector2(x + 44, y + 8), Color.Black);
        }

        private void DrawArrowOption(SpriteBatch b, int x, int y, string label, int optionIndex)
        {
            b.DrawString(Game1.smallFont, label, new Vector2(x, y + 8), Color.Black);
            b.Draw(Game1.mouseCursors, new Vector2(x + 180, y), new Rectangle(226, 425, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.DrawString(Game1.smallFont, "Default", new Vector2(x + 215, y + 8), Color.Black);
            b.Draw(Game1.mouseCursors, new Vector2(x + 355, y), new Rectangle(244, 425, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        }

        private void DrawScrollbar(SpriteBatch b, int contentStartY, int contentEndY)
        {
            int barHeight = height - 200;
            int scrollBarHeight = Math.Max(32, barHeight * barHeight / (barHeight + maxScroll + 1));
            int scrollBarY = yPositionOnScreen + 90 + (scrollOffset * (barHeight - scrollBarHeight) / Math.Max(1, maxScroll));
            b.Draw(Game1.menuTexture, new Rectangle(xPositionOnScreen + width - 40, yPositionOnScreen + 90, 24, barHeight), new Rectangle(403, 383, 6, 6), Color.White);
            b.Draw(Game1.menuTexture, new Rectangle(xPositionOnScreen + width - 40, scrollBarY, 24, scrollBarHeight), new Rectangle(435, 400, 6, 6), Color.Gray);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            int yStart = yPositionOnScreen + 180 - scrollOffset;
            int spouseY = yStart;
            for (int i = 0; i < spouses.Count; i++)
            {
                var rect = new Rectangle(xPositionOnScreen + 70, spouseY, 260, 40);
                if (rect.Contains(x, y))
                {
                    selectedSpouse = spouses[i];
                    Game1.playSound("smallSelect");
                    return;
                }
                spouseY += 50;
            }
            // 남/여 탭 클릭
            int tabY = yPositionOnScreen + 370 - scrollOffset;
            for (int i = 0; i < 2; i++)
            {
                var rect = new Rectangle(xPositionOnScreen + 70 + i * 170, tabY, 150, 40);
                if (rect.Contains(x, y))
                {
                    selectedTab = i;
                    Game1.playSound("smallSelect");
                    return;
                }
            }

            // 저장 버튼
            var saveRect = new Rectangle(xPositionOnScreen + width / 2 - 220, yPositionOnScreen + height - 110, 120, 48);
            if (saveRect.Contains(x, y))
            {
                Game1.playSound("smallSelect");
                onClose?.Invoke();
                exitThisMenu();
                return;
            }
            // 닫기 버튼
            var closeRect = new Rectangle(xPositionOnScreen + width / 2 + 100, yPositionOnScreen + height - 110, 120, 48);
            if (closeRect.Contains(x, y))
            {
                Game1.playSound("bigDeSelect");
                onClose?.Invoke();
                exitThisMenu();
                return;
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            int totalContentHeight = 350 + (spouses.Count * 50) + 340; // 대략적 컨텐츠 길이
            maxScroll = Math.Max(0, totalContentHeight - (height - 200));
            scrollOffset -= direction / 120 * scrollStep;
            if (scrollOffset < 0) scrollOffset = 0;
            if (scrollOffset > maxScroll) scrollOffset = maxScroll;
        }
    }
}