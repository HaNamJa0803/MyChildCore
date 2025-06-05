using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SpaceShared.UI;

internal class Table : Container
{
	private readonly List<Element[]> Rows = new List<Element[]>();

	private Vector2 SizeImpl;

	private const int RowPadding = 16;

	private int RowHeightImpl;

	private bool FixedRowHeight;

	private int ContentHeight;

	public Vector2 Size
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return SizeImpl;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			SizeImpl = new Vector2(value.X, (float)((int)value.Y / RowHeight * RowHeight));
			UpdateScrollbar();
		}
	}

	public int RowHeight
	{
		get
		{
			return RowHeightImpl;
		}
		set
		{
			RowHeightImpl = value + 16;
			UpdateScrollbar();
		}
	}

	public int RowCount => Rows.Count;

	public Scrollbar Scrollbar { get; }

	public override int Width => (int)Size.X;

	public override int Height => (int)Size.Y;

	public Table(bool fixedRowHeight = true)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		FixedRowHeight = fixedRowHeight;
		base.UpdateChildren = false;
		Scrollbar = new Scrollbar
		{
			LocalPosition = new Vector2(0f, 0f)
		};
		AddChild(Scrollbar);
	}

	public void AddRow(Element[] elements)
	{
		Rows.Add(elements);
		int num = 0;
		foreach (Element element in elements)
		{
			AddChild(element);
			num = Math.Max(num, element.Height);
		}
		ContentHeight += (FixedRowHeight ? RowHeight : (num + 16));
		UpdateScrollbar();
	}

	public override void Update(bool isOffScreen = false)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		base.Update(isOffScreen);
		if (IsHidden(isOffScreen))
		{
			return;
		}
		int num = 0;
		foreach (Element[] row in Rows)
		{
			int num2 = 0;
			Element[] array = row;
			foreach (Element element in array)
			{
				element.LocalPosition = new Vector2(element.LocalPosition.X, (float)(num - Scrollbar.TopRow * RowHeight));
				bool flag = isOffScreen || IsElementOffScreen(element);
				if (!flag || element is Label)
				{
					element.Update(flag);
				}
				num2 = Math.Max(num2, element.Height);
			}
			num += (FixedRowHeight ? RowHeight : (num2 + 16));
		}
		if (num != ContentHeight)
		{
			ContentHeight = num;
			Scrollbar.Rows = PxToRow(ContentHeight);
		}
		Scrollbar.Update();
	}

	public void ForceUpdateEvenHidden(bool isOffScreen = false)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Element[] row in Rows)
		{
			int num2 = 0;
			Element[] array = row;
			foreach (Element element in array)
			{
				element.LocalPosition = new Vector2(element.LocalPosition.X, (float)num - Scrollbar.ScrollPercent * (float)Rows.Count * (float)RowHeight);
				bool isOffScreen2 = isOffScreen || IsElementOffScreen(element);
				element.Update(isOffScreen2);
				num2 = Math.Max(num2, element.Height);
			}
			num += (FixedRowHeight ? RowHeight : (num2 + 16));
		}
		ContentHeight = num;
		Scrollbar.Update(isOffScreen);
	}

	public override void Draw(SpriteBatch b)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		if (IsHidden())
		{
			return;
		}
		Rectangle val = default(Rectangle);
		((Rectangle)(ref val))._002Ector((int)base.Position.X - 32, (int)base.Position.Y - 32, (int)Size.X + 64, (int)Size.Y + 64);
		int num = 12;
		Rectangle val2 = default(Rectangle);
		((Rectangle)(ref val2))._002Ector(val.X + num, val.Y + num, val.Width - num * 2, val.Height - num * 2);
		IClickableMenu.drawTextureBox(b, val.X, val.Y, val.Width, val.Height, Color.White);
		b.Draw(Game1.menuTexture, val2, (Rectangle?)new Rectangle(64, 128, 64, 64), Color.White);
		Element renderLast = null;
		InScissorRectangle(b, val2, delegate(SpriteBatch contentBatch)
		{
			foreach (Element[] row in Rows)
			{
				Element[] array = row;
				foreach (Element element in array)
				{
					if (!IsElementOffScreen(element))
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
			return element.Position.Y > base.Position.Y + Size.Y;
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
		Scrollbar.LocalPosition = new Vector2(Size.X + 48f, Scrollbar.LocalPosition.Y);
		Scrollbar.RequestHeight = (int)Size.Y;
		Scrollbar.Rows = PxToRow(ContentHeight);
		Scrollbar.FrameSize = (int)(Size.Y / (float)RowHeight);
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
		return (px + RowHeight - 1) / RowHeight;
	}
}
