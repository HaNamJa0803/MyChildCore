using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShared.UI;

internal abstract class Container : Element
{
	private readonly IList<Element> ChildrenImpl = new List<Element>();

	private Element renderLast;

	protected bool UpdateChildren { get; set; } = true;


	public Element RenderLast
	{
		get
		{
			return renderLast;
		}
		set
		{
			renderLast = value;
			if (base.Parent == null)
			{
				return;
			}
			if (value == null)
			{
				if (base.Parent.RenderLast == this)
				{
					base.Parent.RenderLast = null;
				}
			}
			else
			{
				base.Parent.RenderLast = this;
			}
		}
	}

	public Element[] Children => ChildrenImpl.ToArray();

	public void AddChild(Element element)
	{
		element.Parent?.RemoveChild(element);
		ChildrenImpl.Add(element);
		element.Parent = this;
		OnChildrenChanged();
	}

	public void RemoveChild(Element element)
	{
		if (element.Parent != this)
		{
			throw new ArgumentException("Element must be a child of this container.");
		}
		ChildrenImpl.Remove(element);
		element.Parent = null;
		OnChildrenChanged();
	}

	public virtual void OnChildrenChanged()
	{
	}

	public override void Update(bool isOffScreen = false)
	{
		base.Update(isOffScreen);
		if (!UpdateChildren)
		{
			return;
		}
		foreach (Element item in ChildrenImpl)
		{
			item.Update(isOffScreen);
		}
	}

	public override void Draw(SpriteBatch b)
	{
		if (IsHidden())
		{
			return;
		}
		foreach (Element item in ChildrenImpl)
		{
			if (item != RenderLast)
			{
				item.Draw(b);
			}
		}
		RenderLast?.Draw(b);
	}
}
