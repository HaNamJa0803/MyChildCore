using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MyChildCore.UI
{
    public class CustomConfigPage : IClickableMenu
    {
        private readonly List<string> spouses;
        private int selectedSpouse = 0;
        private int selectedTab = 0; // 0: 남자, 1: 여자

        private readonly Action onClose;

        public CustomConfigPage(List<string> spouseList, Action onClose)
            : base(Game1.viewport.Width / 2 - 320, Game1.viewport.Height / 2 - 360, 640, 720, true)
        {
            this.spouses = spouseList;
            this.onClose = onClose;
        }

        public override void draw(SpriteBatch b)
        {
            // 배경, 타이틀
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);
            SpriteText.drawString(b, "자녀 외형 커스터마이즈", xPositionOnScreen + 60, yPositionOnScreen + 32);

            // 배우자 탭
            for (int i = 0; i < spouses.Count; i++)
            {
                var tabX = xPositionOnScreen + 32;
                var tabY = yPositionOnScreen + 80 + i * 36;
                var color = (i == selectedSpouse) ? Color.White : Color.Gray;
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), tabX, tabY, 180, 36, color, 1f);
                SpriteText.drawString(b, spouses[i], tabX + 12, tabY + 8, 999, -1, 999, 1f, 0.88f);
            }

            // 남/여 탭
            string[] tabs = { "👦 남자 자녀", "👧 여자 자녀" };
            for (int i = 0; i < 2; i++)
            {
                var tabX = xPositionOnScreen + 240 + i * 140;
                var tabY = yPositionOnScreen + 80;
                var color = (i == selectedTab) ? Color.White : Color.Gray;
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), tabX, tabY, 130, 36, color, 1f);
                SpriteText.drawString(b, tabs[i], tabX + 12, tabY + 8);
            }

            // 자녀 설정 항목
            int y0 = yPositionOnScreen + 140;
            var spouseName = spouses[selectedSpouse];
            var cfg = ModEntry.Config.SpouseConfigs[spouseName];
            DrawChildOptions(b, cfg, selectedTab == 0, xPositionOnScreen + 220, y0);

            // 저장/닫기 버튼
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                xPositionOnScreen + 120, yPositionOnScreen + height - 80, 120, 48, Color.White, 1f);
            SpriteText.drawString(b, "저장", xPositionOnScreen + 145, yPositionOnScreen + height - 70);

            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                xPositionOnScreen + 300, yPositionOnScreen + height - 80, 120, 48, Color.White, 1f);
            SpriteText.drawString(b, "닫기", xPositionOnScreen + 330, yPositionOnScreen + height - 70);

            drawMouse(b);
        }

        private void DrawChildOptions(SpriteBatch b, SpouseChildConfig cfg, bool isBoy, int x, int y)
        {
            // 예시: 주요 항목 5개 (실제 옵션 더 추가해도 됨)
            int dy = 48;
            int line = 0;

            // 헤어
            SpriteText.drawString(b, "헤어스타일", x, y + dy * line);
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                x + 120, y + dy * line, 120, 36, Color.White, 1f);
            SpriteText.drawString(b, isBoy ? cfg.BoyHairStyles : cfg.GirlHairStyles, x + 135, y + dy * line + 5);
            // 좌/우 화살표 생략(아래 Click에서 구현)

            // 눈동자, 피부, 상의, 하의(치마/바지) 등도 동일하게 추가!
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            // 배우자 탭 클릭
            for (int i = 0; i < spouses.Count; i++)
            {
                var rect = new Rectangle(xPositionOnScreen + 32, yPositionOnScreen + 80 + i * 36, 180, 36);
                if (rect.Contains(x, y))
                {
                    selectedSpouse = i;
                    Game1.playSound("smallSelect");
                    return;
                }
            }
            // 남/여 탭 클릭
            for (int i = 0; i < 2; i++)
            {
                var rect = new Rectangle(xPositionOnScreen + 240 + i * 140, yPositionOnScreen + 80, 130, 36);
                if (rect.Contains(x, y))
                {
                    selectedTab = i;
                    Game1.playSound("smallSelect");
                    return;
                }
            }
            // 저장 버튼
            var saveRect = new Rectangle(xPositionOnScreen + 120, yPositionOnScreen + height - 80, 120, 48);
            if (saveRect.Contains(x, y))
            {
                ModEntry.SaveConfig();
                Game1.playSound("smallSelect");
                return;
            }
            // 닫기 버튼
            var closeRect = new Rectangle(xPositionOnScreen + 300, yPositionOnScreen + height - 80, 120, 48);
            if (closeRect.Contains(x, y))
            {
                onClose?.Invoke();
                exitThisMenu();
                return;
            }
        }
    }
}