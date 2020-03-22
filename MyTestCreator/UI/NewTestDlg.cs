using System;

namespace MyTestCreator
{
	public partial class NewTestDlg : Gtk.Dialog
	{
		public string TestName
		{
			get
			{
				return txtName.Text;
			}
		}

		public string TestAuthor
		{
			get
			{
				return txtAuthor.Text;
			}
		}

		public int TestTime
		{
			get
			{
				return (int)spinbtnTime.Value;
			}
		}

		public TestDescription.ETestMode TestMode
		{
			get
			{
				return chkModePunish.Active ? TestDescription.ETestMode.Punish : TestDescription.ETestMode.Loyal;
			}
		}

		public NewTestDlg ()
		{
			this.Build ();
		}

		protected void TextEntryChanged (object sender, EventArgs e)
		{
			bool isOk = true;
			// Check if everything is ok
			if (TestName.Length == 0)
			{
				isOk = false;
			}
			if (TestAuthor.Length == 0)
			{
				isOk = false;
			}
			buttonOk.Sensitive = isOk;
		}
	}
}

