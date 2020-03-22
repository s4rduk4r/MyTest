using System;

namespace MyTestCreator
{
	public partial class OneStringValueAskDlg : Gtk.Dialog
	{
		public string Text
		{
			set
			{
				txtText.Text = value;
			}
			get
			{
				return txtText.Text;
			}
		}

		public float Value
		{
			set
			{
				spinbtnValue.Value = value;
			}
			get
			{
				return (float)spinbtnValue.Value;
			}
		}

		public OneStringValueAskDlg (string askText, string upperText, string lowerText, int maxValue = 1000)
		{
			this.Build ();
			// Ask text show/hide
			if (askText == null)
			{
				lblAskText.Visible = false;
			}
			else
			{
				lblAskText.LabelProp = askText;
			}
			// Upper label show/hide
			if (upperText == null)
			{
				lblUpperText.Visible = false;
				txtText.Visible = false;
			}
			else
			{
				lblUpperText.LabelProp = upperText;
			}
			// Lower label show/hide
			if (lowerText == null)
			{
				lblLowerText.Visible = false;
			}
			else
			{
				lblLowerText.LabelProp = lowerText;
			}
			// Max value set
			spinbtnValue.Adjustment.Upper = maxValue;
		}

	}
}

