using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SpaceShared.UI;

internal class ScrollContainer : StaticContainer
{
	private int ContentHeight;

	public int lastScroll;

	public Scrollbar Scrollbar { get; }

	public override int Width => (int)base.Size.X;

	public override int Height => (int)base.Size.Y;

	public ScrollContainer()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		base.UpdateChildren = false;
		Scrollbar = new Scrollbar
		{
			LocalPosition = new Vector2(0f, 0f)
		};
		AddChild(Scrollbar);
	}

	public override void OnChildrenChanged()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Element[] children = base.Children;
		foreach (Element element in children)
		{
			if (element != Scrollbar)
			{
				num = Math.Max(num, element.Bounds.Y + element.Bounds.Height);
			}
		}
		if (num != ContentHeight)
		{
			ContentHeight = num;
			Scrollbar.Rows = PxToRow(ContentHeight);
		}
		UpdateScrollbar();
	}

	public override void Update(bool isOffScreen = false)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		base.Update(isOffScreen);
		if (IsHidden(isOffScreen))
		{
			return;
		}
		if (lastScroll != Scrollbar.TopRow)
		{
			float num = lastScroll * 50 - Scrollbar.TopRow * 50;
			lastScroll = Scrollbar.TopRow;
			Element[] children = base.Children;
			foreach (Element element in children)
			{
				if (element != Scrollbar)
				{
					element.LocalPosition = new Vector2(element.LocalPosition.X, element.LocalPosition.Y + num);
				}
			}
		}
		Element[] children2 = base.Children;
		foreach (Element element2 in children2)
		{
			if (element2 != Scrollbar)
			{
				bool flag = isOffScreen || IsElementOffScreen(element2);
				if (!flag || element2 is Label)
				{
					element2.Update(flag);
				}
			}
		}
		Scrollbar.Update();
	}

	public void ForceUpdateEvenHidden(bool isOffScreen = false)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (lastScroll != Scrollbar.TopRow)
		{
			float num = lastScroll * 50 - Scrollbar.TopRow * 50;
			lastScroll = Scrollbar.TopRow;
			Element[] children = base.Children;
			foreach (Element element in children)
			{
				if (element != Scrollbar)
				{
					element.LocalPosition = new Vector2(element.LocalPosition.X, element.LocalPosition.Y + num);
				}
			}
		}
		Element[] children2 = base.Children;
		foreach (Element element2 in children2)
		{
			if (element2 != Scrollbar)
			{
				bool flag = isOffScreen || IsElementOffScreen(element2);
				if (!flag || element2 is Label)
				{
					element2.Update(flag);
				}
			}
		}
		Scrollbar.Update(isOffScreen);
	}

	public override void Draw(SpriteBatch b)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (IsHidden())
		{
			return;
		}
		if (base.OutlineColor.HasValue)
		{
			IClickableMenu.drawTextureBox(b, (int)base.Position.X - 12, (int)base.Position.Y - 12, Width + 24, Height + 24, base.OutlineColor.Value);
		}
		Rectangle val = default(Rectangle);
		((Rectangle)(ref val))._002Ector((int)base.Position.X - 32, (int)base.Position.Y - 32, (int)base.Size.X + 64, (int)base.Size.Y + 64);
		int num = 12;
		Rectangle area = default(Rectangle);
		((Rectangle)(ref area))._002Ector(val.X + 20 + num, val.Y + 20 + num, val.Width - 40 - num * 2, val.Height - 40 - num * 2);
		Element renderLast = null;
		InScissorRectangle(b, area, delegate(SpriteBatch contentBatch)
		{
			Element[] children = base.Children;
			foreach (Element element in children)
			{
				if (element != Scrollbar && !IsElementOffScreen(element))
				{
					if (element == base.RenderLast)
					{
						renderLast = element;
					}
					else
					{
						element.Draw(contentBatch);
					}
				}
			}
		});
		renderLast?.Draw(b);
		Scrollbar.Draw(b);
	}

	private bool IsElementOffScreen(Element element)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (!(element.Position.Y + (float)element.Height < base.Position.Y))
		{
			return element.Position.Y > base.Position.Y + base.Size.Y;
		}
		return true;
	}

	private void UpdateScrollbar()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Scrollbar.LocalPosition = new Vector2(base.Size.X + 48f, Scrollbar.LocalPosition.Y);
		Scrollbar.RequestHeight = (int)base.Size.Y;
		Scrollbar.Rows = PxToRow(ContentHeight);
		Scrollbar.FrameSize = (int)(base.Size.Y / 50f);
	}

	private void InScissorRectangle(SpriteBatch spriteBatch, Rectangle area, Action<SpriteBatch> draw)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.End();
		SpriteBatch val = new SpriteBatch(Game1.graphics.GraphicsDevice);
		try
		{
			GraphicsDevice graphicsDevice = Game1.graphics.GraphicsDevice;
			Rectangle scissorRectangle = graphicsDevice.ScissorRectangle;
			try
			{
				graphicsDevice.ScissorRectangle = area;
				val.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, Utility.ScissorEnabled, (Effect)null, (Matrix?)null);
				draw(val);
				val.End();
			}
			finally
			{
				graphicsDevice.ScissorRectangle = scissorRectangle;
			}
			spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private int PxToRow(int px)
	{
		return (px + 50 - 1) / 50;
	}
}
