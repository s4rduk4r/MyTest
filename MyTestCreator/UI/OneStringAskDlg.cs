using System;

namespace MyTestCreator
{
	public partial class OneStringAskDlg : Gtk.Dialog
	{
		public string Value
		{
			get
			{
				return txtValue.Text;
			}
			set
			{
				txtValue.Text = value;
			}
		}

		public bool IsChecked
		{
			get
			{
				return chkbtnCorrectAnswer.Active;
			}
			set
			{
				chkbtnCorrectAnswer.Active = value;
			}
		}
		
		public OneStringAskDlg (string labelText, bool checkBox = false, string checkboxString = "")
		{
			this.Build ();
			label1.LabelProp = labelText;
			chkbtnCorrectAnswer.Visible = checkBox;
			chkbtnCorrectAnswer.Label = checkboxString;
		}
	}
}

