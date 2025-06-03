using System;
using System.Collections.Generic;
using System.Linq;

namespace MyChildCore;

public static class SleepwearTranslations
{
	public static readonly Dictionary<string, string> StyleKorean;

	public static readonly Dictionary<string, string> ColorKorean;

	public static string GetStyleDisplay(string key)
	{
		string result = default(string);
		if (!StyleKorean.TryGetValue(key, out result))
		{
			return key;
		}
		return result;
	}

	public static string GetColorDisplay(string key)
	{
		string result = default(string);
		if (!ColorKorean.TryGetValue(key, out result))
		{
			return key;
		}
		return result;
	}

	public static string GetStyleKey(string display)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return Enumerable.FirstOrDefault<KeyValuePair<string, string>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, string>>)StyleKorean, (Func<KeyValuePair<string, string>, bool>)((KeyValuePair<string, string> pair) => pair.Value == display)).Key ?? display;
	}

	public static string GetColorKey(string display)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return Enumerable.FirstOrDefault<KeyValuePair<string, string>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, string>>)ColorKorean, (Func<KeyValuePair<string, string>, bool>)((KeyValuePair<string, string> pair) => pair.Value == display)).Key ?? display;
	}

	static SleepwearTranslations()
	{
		Dictionary<string, string> obj = new Dictionary<string, string>();
		obj.Add("1", "강아지");
		obj.Add("2", "양");
		obj.Add("3", "상어");
		obj.Add("4", "레서판다");
		obj.Add("5", "개구리");
		obj.Add("6", "라쿤");
		StyleKorean = obj;
		Dictionary<string, string> obj2 = new Dictionary<string, string>();
		obj2.Add("1", "검정");
		obj2.Add("2", "갈색");
		obj2.Add("3", "분홍");
		obj2.Add("4", "하늘");
		obj2.Add("5", "초록");
		obj2.Add("6", "노랑");
		obj2.Add("7", "보라");
		obj2.Add("8", "흰색");
		obj2.Add("9", "오렌지");
		obj2.Add("10", "남색");
		obj2.Add("11", "연두");
		obj2.Add("12", "회색");
		obj2.Add("13", "빨강");
		obj2.Add("14", "청록");
		obj2.Add("15", "금색");
		ColorKorean = obj2;
	}
}
