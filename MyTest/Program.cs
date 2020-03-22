using System;
using Gtk;
using System.IO;

namespace MyTest
{
	static class Program
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			if (!Program.StartTest ())
			{
				return;
			}
			Application.Run ();
		}

		public static bool StartTest()
		{
			// Select test or quit
			string testFile = null;
			// Keep selecting until we either quit or hit a file
			while (true)
			{
				try
				{
					testFile = SelectTestFile();
					if (testFile == null)
					{
						return false;
					}
					var file = File.OpenRead (testFile);
					file.Close();
					break;
				}
				catch(Exception e)
				{
					continue;
				}
			}
			// Load test
			var testState = new TestState();
			testState.LoadTest (testFile);
			// Prepare for the test process
			testState.Shuffle ();
			// Commence test
			TestWindow win = new TestWindow (testState.Questions, testState.Results);
			win.Show ();
			return true;
		}

		static string SelectTestFile()
		{
			var stfd = new SelectTestFileDlg();
			ResponseType response = (ResponseType)stfd.Run ();
			var testFile = stfd.TestFile;
			stfd.Destroy ();
			if (response != ResponseType.Accept)
			{
				return null;
			}
			return testFile;
		}
	}
}
