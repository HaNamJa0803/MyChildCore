using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShared;

public static class WeightedExtensions
{
	public static T Choose<T>(this Weighted<T>[] choices, Random r = null)
	{
		if (choices.Length == 0)
		{
			return default(T);
		}
		if (choices.Length == 1)
		{
			return choices[0].Value;
		}
		if (r == null)
		{
			r = new Random();
		}
		double num = choices.Sum((Weighted<T> choice) => choice.Weight);
		double num2 = r.NextDouble() * num;
		foreach (Weighted<T> weighted in choices)
		{
			if (num2 < weighted.Weight)
			{
				return weighted.Value;
			}
			num2 -= weighted.Weight;
		}
		throw new Exception("This should never happen");
	}

	public static T Choose<T>(this List<Weighted<T>> choices, Random r = null)
	{
		return choices.ToArray().Choose(r);
	}
}
