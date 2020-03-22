using System;

namespace MyTest
{
	public partial class SelectTestDlg : Gtk.Window
	{
		public SelectTestDlg () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

