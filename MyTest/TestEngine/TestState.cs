using System;
using System.Collections.Generic;
using System.IO;
/*
 * Project: MyTest App
 * Module: TestState
 * Author: Andrey N. Glushenkov
 * Date: 03.10.2017
 * Description:
 * TestState module loads chosen test. It acts as a Question container and counter of correct answers.
 * After the test has ended it shows the resulting statistics.
 */

namespace MyTest
{
	using static MyTest.TestFileParser;
	
	public class TestState
	{
		// Test modes
		public enum ETestMode
		{
			Loyal,	// Default value
			Punish
		};

		// Mode strings
		static string ModeLoyal = "loyal";
		static string ModePunish = "punish";
		
		// Test name
		public string Name
		{
			get
			{
				return name;
			}
		}

		// Test author
		public string Author
		{
			get
			{
				return author;
			}
		}

		// Date on last modification of the test
		public DateTime LastModified
		{
			get
			{
				return lastModified;
			}
		}

		// Test mode
		public ETestMode Mode
		{
			get
			{
				return mode;
			}
		}

		// Test time
		public int Time
		{
			get
			{
				return time;
			}
		}

		// Questions
		public IList<TestQuestion> Questions
		{
			get
			{
				foreach (var question in questions)
				{
					question.Shuffle (rng);
				}
				return questions;
			}
		}

		// Maximum value
		public float Value
		{
			get
			{
				float value = 0;
				foreach (var question in questions)
				{
					value += question.Value;
				}
				return value;
			}
		}

		// Test results
		public TestResults Results = null;

		public TestState()
		{
			Results = new TestResults(this);
		}

		// Load test
		public void LoadTest(string testFilePath)
		{
			StringReader testFile = null;
			var testDecryptor = new MyTestCreator.TestFileEncDec (testFilePath, MyTestCreator.TestFileEncDec.ETestFileMode.Decode);
			{
				var data = testDecryptor.Data;
				testFile = new StringReader (System.Text.Encoding.UTF8.GetString (data));
			}
			// Read whole text into memory
			//var file = File.OpenText (testFilePath);
			//var testFile = new StringReader (file.ReadToEnd ());
			//file.Close ();
			// Current question being parsed
			TestQuestion question = null;
			string line = null;//testFile.ReadLine();
			do
			{
				line = testFile.ReadLine ();
				// Skip commentary lines
				if (line == null || line.Length == 0 || line [0] == '#')
				{
					continue;
				}
				// Parse the rest of the file
				var keyValue = GetKeyValueFromLine (line);
				switch (keyValue.Key)
				{
					case ETestTags.Test:
						{
							name = keyValue.Value;
						}
						break;
					case ETestTags.Author:
						{
							author = keyValue.Value;
						}
						break;
					case ETestTags.Date:
						{
							var dateStr = keyValue.Value.Split ('.');
							lastModified = new DateTime (Convert.ToInt32 (dateStr [2]), Convert.ToInt32 (dateStr [1]), Convert.ToInt32 (dateStr [0]));
						}
						break;
					case ETestTags.Time:
						{
							// If time is infinite, then time = 0
							if (keyValue.Value != "inf")
							{
								time = Convert.ToInt32 (keyValue.Value);
							}
						}
						break;
					case ETestTags.Mode:
						{
							var modeString = keyValue.Value.ToLower ();
							if (modeString == ModePunish)
							{
								mode = ETestMode.Punish;
							}
							else
							{
								if (modeString == ModeLoyal)
								{
									mode = ETestMode.Loyal;
								}
							}
						}
						break;
					case ETestTags.Question:
						{
							// Add previous question to the list
							if (question != null)
							{
								questions.Add (question);
							}
							question = new TestQuestion (keyValue.Value);
						}
						break;
					case ETestTags.Value:
						{
							// Ignore this keyword if no Question keyword encountered
							if (question != null)
							{
								question.Value = Convert.ToSingle (keyValue.Value);
							}
						}
						break;
					case ETestTags.Answer:
						{
							// Ignore this keyword if no Question keyword encountered
							if (question != null)
							{
								var answer = new TestAnswer (keyValue.Value, false);
								question.Answers.Add (answer);
							}
						}
						break;
					case ETestTags.AnswerTrue:
						{
							// Ignore this keyword if no Question keyword encountered
							if (question != null)
							{
								var answer = new TestAnswer (keyValue.Value, true);
								question.Answers.Add (answer);
							}
						}
						break;
					default:
						continue;
				}
			}
			while(line != null && line.Length != 0);
			// Add last question we've parsed
			if (question != null)
			{
				if (!questions.Contains (question))
				{
					questions.Add (question);
				}
			}
			testFile.Close ();
		}

		// Shuffle questions so they don't repeat their order each time
		public void Shuffle()
		{
			var questionsOld = questions;
			questions = new List<TestQuestion> ();
			var count = questionsOld.Count;
			while (questions.Count < count)
			{
				var i = rng.Next (0, questionsOld.Count);
				questions.Add (questionsOld[i]);
				questionsOld.RemoveAt (i);
			}
			questionsOld.Clear ();
			// Shuffle answers in questions
			foreach (var question in questions)
			{
				question.Shuffle (rng);
			}
		}

		// Infinite test time definition
		public static int TimeInfinite = 0;

		string name;
		string author;
		DateTime lastModified;
		ETestMode mode = ETestMode.Loyal;
		int time = TimeInfinite; // Infinite time by default
		List<TestQuestion> questions = new List<TestQuestion>();
		// Shuffle randomizer
		Random rng = new Random();
	}
}

