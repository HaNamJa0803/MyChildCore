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
        // ────────── 데이터 및 상태 ──────────
        private readonly string spouseName;
        private readonly Action<string, SpouseChildConfig, bool> onSave;
        private int selectedTab; // 0: 남자, 1: 여자

        // 스크롤
        private int scrollY = 0;
        private int maxScroll = 0;

        // 실제 옵션(파츠) 목록 (예시)
        private SpouseChildConfig config
            => MyChildCore.ModEntry.Config.SpouseConfigs.ContainsKey(spouseName)
            ? MyChildCore.ModEntry.Config.SpouseConfigs[spouseName]
            : new SpouseChildConfig();

        // ────────── 생성자 ──────────
        public ChildConfigMenu(string spouse, Action<string, SpouseChildConfig, bool> onSaveCallback)
            : base(Game1.viewport.Width / 2 - 500, Game1.viewport.Height / 2 - 400, 1000, 800, true)
        {
            spouseName = spouse;
            onSave = onSaveCallback;
            selectedTab = 0;
        }

        // ────────── draw ──────────
        public override void draw(SpriteBatch b)
        {
            // GMCM 스타일 배경
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);

            // 타이틀
            string title = spouseName + " 자녀 커스터마이즈";
            SpriteText.drawString(b, title, xPositionOnScreen + 60, yPositionOnScreen + 32);

            // 남/여 자녀 탭
            int tabX = xPositionOnScreen + 60;
            int tabY = yPositionOnScreen + 90;
            for (int i = 0; i < 2; i++)
            {
                bool selected = (i == selectedTab);
                IClickableMenu.drawTextureBox(
                    b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                    tabX + i * 180, tabY, 170, 40,
                    selected ? Color.White : Color.Gray, 1f);
                SpriteText.drawString(b, (i == 0 ? "👦 남자 자녀" : "👧 여자 자녀"), tabX + i * 180 + 18, tabY + 10);
            }

            // ────────── 자녀 옵션들(스크롤 영역) ──────────
            int optionsX = xPositionOnScreen + 60;
            int optionsY = yPositionOnScreen + 160 - scrollY;
            int optionHeight = 56;
            int optIndex = 0;

            // (예시) 주요 파츠들
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "헤어", config.GetHair(selectedTab == 0), (v) => { config.SetHair(selectedTab == 0, v); });
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "눈", config.GetEye(selectedTab == 0), (v) => { config.SetEye(selectedTab == 0, v); });
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "피부", config.GetSkin(selectedTab == 0), (v) => { config.SetSkin(selectedTab == 0, v); });
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "하의", config.GetBottom(selectedTab == 0), (v) => { config.SetBottom(selectedTab == 0, v); });
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "신발", config.GetShoes(selectedTab == 0), (v) => { config.SetShoes(selectedTab == 0, v); });
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "넥칼라", config.GetNeck(selectedTab == 0), (v) => { config.SetNeck(selectedTab == 0, v); });
            DrawArrowOption(b, optionsX, optionsY + optionHeight * optIndex++, "잠옷", config.GetPajama(selectedTab == 0), (v) => { config.SetPajama(selectedTab == 0, v); });

            // ────────── 스크롤바 (필요시) ──────────
            maxScroll = Math.Max(0, optionHeight * optIndex - (height - 220));
            if (maxScroll > 0)
                DrawScrollBar(b, optionsX + 750, yPositionOnScreen + 160, height - 220);

            // 저장/닫기 버튼
            DrawButton(b, xPositionOnScreen + 260, yPositionOnScreen + height - 90, 160, 54, "저장", true);
            DrawButton(b, xPositionOnScreen + 540, yPositionOnScreen + height - 90, 160, 54, "닫기", false);

            drawMouse(b);
        }

        // ────────── 화살표 옵션 (GMCM 느낌) ──────────
        private void DrawArrowOption(SpriteBatch b, int x, int y, string label, string value, Action<string> setValue)
        {
            // 좌/우 화살표, 옵션값
            SpriteText.drawString(b, label, x, y + 8);
            int arrowLeftX = x + 170;
            int valueX = x + 210;
            int arrowRightX = x + 440;

            // 좌 화살표
            b.Draw(Game1.mouseCursors, new Vector2(arrowLeftX, y), new Rectangle(226, 425, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            // 값(텍스트)
            b.DrawString(Game1.smallFont, value, new Vector2(valueX, y + 10), Color.Black);
            // 우 화살표
            b.Draw(Game1.mouseCursors, new Vector2(arrowRightX, y), new Rectangle(244, 425, 9, 9), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        }

        // ────────── 버튼 (저장/닫기) ──────────
        private void DrawButton(SpriteBatch b, int x, int y, int w, int h, string label, bool isSave)
        {
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, w, h, Color.White, 1f);
            SpriteText.drawString(b, label, x + 40, y + 18);
        }

        // ────────── 스크롤바(간단 구현) ──────────
        private void DrawScrollBar(SpriteBatch b, int x, int y, int h)
        {
            b.Draw(Game1.staminaRect, new Rectangle(x, y, 16, h), Color.Gray);
            int thumbH = Math.Max(48, h * (h / (maxScroll + h)));
            int thumbY = y + (scrollY * (h - thumbH) / (maxScroll == 0 ? 1 : maxScroll));
            b.Draw(Game1.staminaRect, new Rectangle(x, thumbY, 16, thumbH), Color.White);
        }

        // ────────── 클릭처리 ──────────
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            int tabX = xPositionOnScreen + 60;
            int tabY = yPositionOnScreen + 90;
            for (int i = 0; i < 2; i++)
            {
                var rect = new Rectangle(tabX + i * 180, tabY, 170, 40);
                if (rect.Contains(x, y))
                {
                    selectedTab = i;
                    Game1.playSound("smallSelect");
                    return;
                }
            }
            // 저장 버튼
            var saveRect = new Rectangle(xPositionOnScreen + 260, yPositionOnScreen + height - 90, 160, 54);
            if (saveRect.Contains(x, y))
            {
                onSave?.Invoke(spouseName, config, selectedTab == 0);
                Game1.playSound("smallSelect");
                exitThisMenu();
                return;
            }
            // 닫기 버튼
            var closeRect = new Rectangle(xPositionOnScreen + 540, yPositionOnScreen + height - 90, 160, 54);
            if (closeRect.Contains(x, y))
            {
                Game1.playSound("bigDeSelect");
                exitThisMenu();
                return;
            }
            // 스크롤바 클릭
            int scrollBarX = xPositionOnScreen + 810;
            int scrollBarY = yPositionOnScreen + 160;
            int scrollBarH = height - 220;
            if (new Rectangle(scrollBarX, scrollBarY, 16, scrollBarH).Contains(x, y) && maxScroll > 0)
            {
                float ratio = (float)(y - scrollBarY) / (float)scrollBarH;
                scrollY = (int)(maxScroll * ratio);
                scrollY = Math.Max(0, Math.Min(scrollY, maxScroll));
                Game1.playSound("tinyWhip");
            }
            // 좌/우 화살표(파츠변경) 등도 여기 추가
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (maxScroll > 0)
            {
                scrollY -= direction * 24;
                scrollY = Math.Max(0, Math.Min(scrollY, maxScroll));
            }
        }
    }
}