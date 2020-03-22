using System;
using System.Collections.Generic;
using MyTest;

namespace MyTestCreator
{
	public class TestDescription
	{
		// Test modes
		public enum ETestMode
		{
			Loyal,	// Default value
			Punish
		};
		
		// Test name
		public string Name;
		// Test author
		public string Author;
		// Test time, seconds
		public int Time = 0;
		// Test mode
		public ETestMode Mode = ETestMode.Loyal;
		// Last modification date
		public DateTime Date = DateTime.Now;
		// Test questions
		public IList<TestQuestion> Questions = new List<TestQuestion>();

		public TestDescription ()
		{
		}

		public TestDescription(string name, string author, int time, ETestMode mode, DateTime date)
		{
			Name = name;
			Author = author;
			Time = time;
			Mode = mode;
			Date = date;
		}

		public override string ToString ()
		{
			var date = string.Format ("{0}.{1}.{2}", Date.Day, Date.Month, Date.Year);
			var time = (Time > 0) ? Time.ToString () : "inf";
			var mode = (Mode == ETestMode.Punish) ? "Punish": "Loyal";
			var str = string.Format ("# MyTestCreator v0.1\nTest = {0}\nAuthor = {1}\nDate = {2}\nTime = {3}\nMode = {4}\n", Name, Author, date, time, mode);
			foreach (var question in Questions)
			{
				str += question.ToString() + "\n";
				foreach (var answer in question.Answers)
				{
					str += answer.ToString () + "\n";
				}
			}
			return str;
		}
	}
}

