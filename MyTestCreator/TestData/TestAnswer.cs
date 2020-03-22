using System;

namespace MyTestCreator
{
	public class TestAnswer
	{
		public string Text;
		public bool Correct;
		
		public TestAnswer (string text, bool correct)
		{
			Text = text;
			Correct = correct;
		}

		public static bool operator==(TestAnswer a, TestAnswer b)
		{
			return (a.Text == b.Text) && (a.Correct == b.Correct);
		}

		public static bool operator!=(TestAnswer a, TestAnswer b)
		{
			return !(a == b);
		}

		public override bool Equals (object obj)
		{
			return base.Equals (obj);
		}

		public override string ToString ()
		{
			return string.Format ("Answer{1} = {0}", Text, Correct ? "+" : "");
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}

