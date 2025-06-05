using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;

namespace SpaceShared.UI;

internal abstract class Element
{
	public Func<bool> ForceHide;

	public object UserData { get; set; }

	public Container Parent { get; internal set; }

	public Vector2 LocalPosition { get; set; }

	public Vector2 Position
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Parent != null)
			{
				return Parent.Position + LocalPosition;
			}
			return LocalPosition;
		}
	}

	public abstract int Width { get; }

	public abstract int Height { get; }

	public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

	public bool Hover { get; private set; }

	public virtual string HoveredSound => null;

	public bool ClickGestured { get; private set; }

	public bool Clicked
	{
		get
		{
			if (Hover)
			{
				return ClickGestured;
			}
			return false;
		}
	}

	public virtual string ClickedSound => null;

	public virtual void Update(bool isOffScreen = false)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		bool flag = IsHidden(isOffScreen);
		if (flag)
		{
			Hover = false;
			ClickGestured = false;
			return;
		}
		int num;
		int num2;
		if ((int)Constants.TargetPlatform == 0)
		{
			num = Game1.getMouseX();
			num2 = Game1.getMouseY();
		}
		else
		{
			num = Game1.getOldMouseX();
			num2 = Game1.getOldMouseY();
		}
		int num3;
		if (!flag && !GetRoot().Obscured)
		{
			Rectangle bounds = Bounds;
			num3 = (((Rectangle)(ref bounds)).Contains(num, num2) ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		bool flag2 = (byte)num3 != 0;
		if (flag2 && !Hover && HoveredSound != null)
		{
			Game1.playSound(HoveredSound, (int?)null);
		}
		Hover = flag2;
		MouseState mouseState = Game1.input.GetMouseState();
		ClickGestured = (int)((MouseState)(ref mouseState)).LeftButton == 1 && (int)((MouseState)(ref Game1.oldMouseState)).LeftButton == 0;
		int clickGestured;
		if (!ClickGestured)
		{
			if (Game1.options.gamepadControls)
			{
				GamePadState gamePadState = Game1.input.GetGamePadState();
				clickGestured = ((((GamePadState)(ref gamePadState)).IsButtonDown((Buttons)4096) && !((GamePadState)(ref Game1.oldPadState)).IsButtonDown((Buttons)4096)) ? 1 : 0);
			}
			else
			{
				clickGestured = 0;
			}
		}
		else
		{
			clickGestured = 1;
		}
		ClickGestured = (byte)clickGestured != 0;
		if (ClickGestured && (Dropdown.SinceDropdownWasActive > 0 || Dropdown.ActiveDropdown != null))
		{
			ClickGestured = false;
		}
		if (Clicked && ClickedSound != null)
		{
			Game1.playSound(ClickedSound, (int?)null);
		}
	}

	public abstract void Draw(SpriteBatch b);

	public RootElement GetRoot()
	{
		return GetRootImpl();
	}

	internal virtual RootElement GetRootImpl()
	{
		if (Parent == null)
		{
			throw new Exception("Element must have a parent.");
		}
		return Parent.GetRoot();
	}

	public bool IsHidden(bool isOffScreen = false)
	{
		if (!isOffScreen)
		{
			return ForceHide?.Invoke() ?? false;
		}
		return true;
	}
}
