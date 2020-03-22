using System;
using System.IO;

namespace MyTest
{
	public partial class SelectTestFileDlg : Gtk.Dialog
	{
		static string TestsDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Tests");

		public string TestFile
		{
			get
			{
				return testFileChooserDlg.Filename;
			}
		}

		public SelectTestFileDlg ()
		{
			this.Build ();
			// Set directory to "Tests"
			if (!Directory.Exists (TestsDirectory))
			{
				Directory.CreateDirectory (TestsDirectory);
			}
			testFileChooserDlg.SetCurrentFolder (TestsDirectory);
			testFileChooserDlg.AddFilter (MyTestCreator.TestFileEncDec.MyTestCompiledFileFilter);
		}

		protected void ConfineToTestDirectory (object sender, EventArgs e)
		{
			var folder = testFileChooserDlg.CurrentFolder;
			if (!folder.Contains(TestsDirectory))
			{
				testFileChooserDlg.SetCurrentFolder (TestsDirectory);
			}
		}
	}
}

