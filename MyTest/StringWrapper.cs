using System;

namespace StringMultilineWrapper
{
	public static class StringWrapper
	{
		public static string MakeMultiline(string str, int maxLength)
		{
			var retStr = str;
			if (retStr.Length >= maxLength)
			{
				var nums = Math.Round((double)(retStr.Length / maxLength));
				for (int i = 1; i <= nums; ++i)
				{
					retStr = retStr.Insert (maxLength * i, "\n");
				}
			}
			return retStr;
		}

		public static string WordWrap(string str, int maxLength)
		{
			string retStr = "";
			var strings = str.Split(' ');
			int line = 1;
			foreach (var s in strings)
			{
				if ((retStr.Length + s.Length) / line <= maxLength) {
					retStr += $"{s} ";
				}
				else
				{
					retStr += "\n";
					line++;
					retStr += $"{s} ";
				}
			}
			return retStr;
		}
	}
}

