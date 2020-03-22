using System;
using System.Collections.Generic;

namespace MyTestCreator
{
	public class TestQuestion
	{
		public string Text;
		public float Value;
		public IList<TestAnswer> Answers = new List<TestAnswer>();

		public TestQuestion (string text, float value)
		{
			Text = text;
			Value = value;
		}

		public override string ToString ()
		{
			return string.Format ("Question = {0}\nValue = {1}", Text, Value);
		}
	}
}

