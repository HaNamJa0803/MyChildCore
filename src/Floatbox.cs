namespace SpaceShared.UI;

internal class Floatbox : Textbox
{
	public float Value
	{
		get
		{
			if (!float.TryParse(String, out var result))
			{
				return 0f;
			}
			return result;
		}
		set
		{
			String = value.ToString();
		}
	}

	public bool IsValid
	{
		get
		{
			float result;
			return float.TryParse(String, out result);
		}
	}

	protected override void ReceiveInput(string str)
	{
		bool flag = String.Contains('.');
		bool flag2 = true;
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if (!char.IsDigit(c) && (c != '.' || flag) && (c != '-' || !(String == "") || i != 0))
			{
				flag2 = false;
				break;
			}
			if (c == '.')
			{
				flag = true;
			}
		}
		if (flag2)
		{
			String += str;
			base.Callback?.Invoke(this);
		}
	}
}
