using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;

namespace GenericModConfigMenu.Framework.Overlays;

internal interface IKeybindOverlay
{
	bool IsFinished { get; }

	void OnButtonsChanged(ButtonsChangedEventArgs e);

	void OnWindowResized();

	void OnLeftClick(int x, int y);

	void Draw(SpriteBatch spriteBatch);
}
