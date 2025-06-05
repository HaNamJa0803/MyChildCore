namespace SpaceShared.UI;

internal class Intbox : Textbox
{
	public int Value
	{
		get
		{
			if (!int.TryParse(String, out var result))
			{
				return 0;
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
			int result;
			return int.TryParse(String, out result);
		}
	}

	protected override void ReceiveInput(string str)
	{
		bool flag = true;
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if (!char.IsDigit(c) && (c != '-' || !(String == "") || i != 0))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			String += str;
			base.Callback?.Invoke(this);
		}
	}
}
