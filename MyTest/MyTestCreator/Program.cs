using System;
using Gtk;

namespace MyTestCreator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			SelectTestToEncryptDlg testSelector = new SelectTestToEncryptDlg ();
			testSelector.Show ();
			Application.Run ();
		}
	}
}
