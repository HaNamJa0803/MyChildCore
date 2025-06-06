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
        private readonly string spouseName;
        private readonly Action<string, SpouseChildConfig, bool> onSave;
        private int selectedTab; // 0: 남자, 1: 여자

        private SpouseChildConfig config
            => MyChildCore.ModEntry.Config.SpouseConfigs.ContainsKey(spouseName)
            ? MyChildCore.ModEntry.Config.SpouseConfigs[spouseName]
            : new SpouseChildConfig();

        // 각 파츠 드롭다운 옵션
        private readonly string[] boyHairStyles = DropdownConfig.BoyHairStyles;
        private readonly string[] girlHairStyles = DropdownConfig.GirlHairStyles;
        private readonly string[] eyes = DropdownConfig.BoyEyes; // 예시
        private readonly string[] skins = DropdownConfig.BoySkins; // 예시

        public ChildConfigMenu(string spouse, Action<string, SpouseChildConfig, bool> onSaveCallback)
            : base(Game1.viewport.Width / 2 - 500, Game1.viewport.Height / 2 - 400, 1000, 800, true)
        {
            spouseName = spouse;
            onSave = onSaveCallback;
            selectedTab = 0;
        }

        public override void draw(SpriteBatch b)
        {
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);
            b.DrawString(Game1.smallFont, spouseName + " 자녀 외형 설정", new Vector2(xPositionOnScreen + 40, yPositionOnScreen + 30), Color.Black);

            // 탭
            string[] tabs = { "남자 자녀", "여자 자녀" };
            for (int i = 0; i < 2; i++)
            {
                var rect = new Rectangle(xPositionOnScreen + 40 + i * 150, yPositionOnScreen + 80, 120, 40);
                b.Draw(Game1.staminaRect, rect, (selectedTab == i) ? Color.White : Color.LightGray);
                b.DrawString(Game1.smallFont, tabs[i], new Vector2(rect.X + 16, rect.Y + 10), Color.Black);
            }

            int optY = yPositionOnScreen + 140;

            // --- 파츠 옵션 (필드 직접 접근) ---
            if (selectedTab == 0)
            {
                // 남자
                DrawArrowOption(b, xPositionOnScreen + 60, optY, "헤어", config.BoyHairStyle, boyHairStyles, (v) => config.BoyHairStyle = v);
                DrawArrowOption(b, xPositionOnScreen + 60, optY + 56, "눈", config.BoyEye, eyes, (v) => config.BoyEye = v);
                DrawArrowOption(b, xPositionOnScreen + 60, optY + 112, "피부", config.BoySkin, skins, (v) => config.BoySkin = v);
            }
            else
            {
                // 여자
                DrawArrowOption(b, xPositionOnScreen + 60, optY, "헤어", config.GirlHairStyle, girlHairStyles, (v) => config.GirlHairStyle = v);
                DrawArrowOption(b, xPositionOnScreen + 60, optY + 56, "눈", config.GirlEye, eyes, (v) => config.GirlEye = v);
                DrawArrowOption(b, xPositionOnScreen + 60, optY + 112, "피부", config.GirlSkin, skins, (v) => config.GirlSkin = v);
            }

            // 저장/닫기
            DrawButton(b, xPositionOnScreen + 340, yPositionOnScreen + height - 90, 120, 54, "저장", true);
            DrawButton(b, xPositionOnScreen + 540, yPositionOnScreen + height - 90, 120, 54, "닫기", false);

            base.draw(b);
            drawMouse(b);
        }

        private void DrawArrowOption(SpriteBatch b, int x, int y, string label, string value, string[] values, Action<string> setValue)
        {
            int idx = Array.IndexOf(values, value);
            if (idx < 0) idx = 0;
            // 레이블
            b.DrawString(Game1.smallFont, label, new Vector2(x, y), Color.Black);
            // < 왼쪽
            b.Draw(Game1.mouseCursors, new Vector2(x + 110, y), new Rectangle(226, 425, 9, 9), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            // 값
            b.DrawString(Game1.smallFont, values[idx], new Vector2(x + 150, y), Color.Black);
            // > 오른쪽
            b.Draw(Game1.mouseCursors, new Vector2(x + 340, y), new Rectangle(244, 425, 9, 9), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        }

        private void DrawButton(SpriteBatch b, int x, int y, int w, int h, string label, bool isSave)
        {
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, w, h, Color.White, 1f);
            b.DrawString(Game1.smallFont, label, new Vector2(x + 26, y + 14), Color.Black);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            // 탭 클릭
            for (int i = 0; i < 2; i++)
            {
                var rect = new Rectangle(xPositionOnScreen + 40 + i * 150, yPositionOnScreen + 80, 120, 40);
                if (rect.Contains(x, y))
                {
                    selectedTab = i;
                    Game1.playSound("smallSelect");
                    return;
                }
            }

            // --- 화살표 옵션 변경 ---
            int optY = yPositionOnScreen + 140;
            if (selectedTab == 0)
            {
                HandleArrowClick(x, y, xPositionOnScreen + 60, optY, DropdownConfig.BoyHairStyles, ref config.BoyHairStyle);
                HandleArrowClick(x, y, xPositionOnScreen + 60, optY + 56, DropdownConfig.BoyEyes, ref config.BoyEye);
                HandleArrowClick(x, y, xPositionOnScreen + 60, optY + 112, DropdownConfig.BoySkins, ref config.BoySkin);
            }
            else
            {
                HandleArrowClick(x, y, xPositionOnScreen + 60, optY, DropdownConfig.GirlHairStyles, ref config.GirlHairStyle);
                HandleArrowClick(x, y, xPositionOnScreen + 60, optY + 56, DropdownConfig.GirlEyes, ref config.GirlEye);
                HandleArrowClick(x, y, xPositionOnScreen + 60, optY + 112, DropdownConfig.GirlSkins, ref config.GirlSkin);
            }

            // 저장 버튼
            var saveRect = new Rectangle(xPositionOnScreen + 340, yPositionOnScreen + height - 90, 120, 54);
            if (saveRect.Contains(x, y))
            {
                onSave?.Invoke(spouseName, config, selectedTab == 0);
                Game1.playSound("smallSelect");
                exitThisMenu();
                return;
            }
            // 닫기 버튼
            var closeRect = new Rectangle(xPositionOnScreen + 540, yPositionOnScreen + height - 90, 120, 54);
            if (closeRect.Contains(x, y))
            {
                Game1.playSound("bigDeSelect");
                exitThisMenu();
                return;
            }
        }

        private void HandleArrowClick(int mouseX, int mouseY, int baseX, int baseY, string[] arr, ref string value)
        {
            int idx = Array.IndexOf(arr, value);
            if (idx < 0) idx = 0;
            // < 왼쪽
            var leftRect = new Rectangle(baseX + 110, baseY, 27, 36);
            // > 오른쪽
            var rightRect = new Rectangle(baseX + 340, baseY, 27, 36);
            if (leftRect.Contains(mouseX, mouseY))
            {
                idx = (idx - 1 + arr.Length) % arr.Length;
                value = arr[idx];
                Game1.playSound("drumkit6");
            }
            else if (rightRect.Contains(mouseX, mouseY))
            {
                idx = (idx + 1) % arr.Length;
                value = arr[idx];
                Game1.playSound("drumkit6");
            }
        }
    }
}