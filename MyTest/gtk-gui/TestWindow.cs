
// This file has been generated by the GUI designer. Do not modify.

public partial class TestWindow
{
	private global::Gtk.VBox VertLayout;
	
	private global::Gtk.Label lblTestName;
	
	private global::Gtk.ScrolledWindow TextHolder;
	
	private global::Gtk.TextView txtQuestion;
	
	private global::Gtk.VBox AnswersHolder;
	
	private global::Gtk.HButtonBox BtnHorizHolder;
	
	private global::Gtk.Button btnQuit;
	
	private global::Gtk.Button btnPrev;
	
	private global::Gtk.Button btnNext;
	
	private global::Gtk.ProgressBar pbTestTimer;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget TestWindow
		this.Name = "TestWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MyTest App Window");
		this.Icon = global::Stetic.IconLoader.LoadIcon (this, "stock_dialog-info", global::Gtk.IconSize.Menu);
		this.WindowPosition = ((global::Gtk.WindowPosition)(1));
		// Container child TestWindow.Gtk.Container+ContainerChild
		this.VertLayout = new global::Gtk.VBox ();
		this.VertLayout.Name = "VertLayout";
		this.VertLayout.Spacing = 6;
		// Container child VertLayout.Gtk.Box+BoxChild
		this.lblTestName = new global::Gtk.Label ();
		this.lblTestName.Name = "lblTestName";
		this.lblTestName.LabelProp = global::Mono.Unix.Catalog.GetString ("Test Name");
		this.VertLayout.Add (this.lblTestName);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.VertLayout [this.lblTestName]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child VertLayout.Gtk.Box+BoxChild
		this.TextHolder = new global::Gtk.ScrolledWindow ();
		this.TextHolder.Name = "TextHolder";
		this.TextHolder.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child TextHolder.Gtk.Container+ContainerChild
		this.txtQuestion = new global::Gtk.TextView ();
		this.txtQuestion.Buffer.Text = "Question text.\n\nLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
		this.txtQuestion.CanFocus = true;
		this.txtQuestion.Name = "txtQuestion";
		this.txtQuestion.Editable = false;
		this.txtQuestion.CursorVisible = false;
		this.txtQuestion.AcceptsTab = false;
		this.txtQuestion.Justification = ((global::Gtk.Justification)(2));
		this.txtQuestion.WrapMode = ((global::Gtk.WrapMode)(2));
		this.txtQuestion.PixelsAboveLines = 20;
		this.TextHolder.Add (this.txtQuestion);
		this.VertLayout.Add (this.TextHolder);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.VertLayout [this.TextHolder]));
		w3.Position = 1;
		// Container child VertLayout.Gtk.Box+BoxChild
		this.AnswersHolder = new global::Gtk.VBox ();
		this.AnswersHolder.Name = "AnswersHolder";
		this.AnswersHolder.Spacing = 6;
		this.VertLayout.Add (this.AnswersHolder);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.VertLayout [this.AnswersHolder]));
		w4.Position = 2;
		// Container child VertLayout.Gtk.Box+BoxChild
		this.BtnHorizHolder = new global::Gtk.HButtonBox ();
		this.BtnHorizHolder.Name = "BtnHorizHolder";
		// Container child BtnHorizHolder.Gtk.ButtonBox+ButtonBoxChild
		this.btnQuit = new global::Gtk.Button ();
		this.btnQuit.CanFocus = true;
		this.btnQuit.Name = "btnQuit";
		this.btnQuit.UseUnderline = true;
		this.btnQuit.Label = global::Mono.Unix.Catalog.GetString ("Остановить тест (Halt test)");
		global::Gtk.Image w5 = new global::Gtk.Image ();
		w5.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-delete", global::Gtk.IconSize.Menu);
		this.btnQuit.Image = w5;
		this.BtnHorizHolder.Add (this.btnQuit);
		global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.BtnHorizHolder [this.btnQuit]));
		w6.Expand = false;
		w6.Fill = false;
		// Container child BtnHorizHolder.Gtk.ButtonBox+ButtonBoxChild
		this.btnPrev = new global::Gtk.Button ();
		this.btnPrev.CanFocus = true;
		this.btnPrev.Name = "btnPrev";
		this.btnPrev.UseUnderline = true;
		this.btnPrev.Label = global::Mono.Unix.Catalog.GetString (" Назад (Back)");
		global::Gtk.Image w7 = new global::Gtk.Image ();
		w7.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-goto-first", global::Gtk.IconSize.Menu);
		this.btnPrev.Image = w7;
		this.BtnHorizHolder.Add (this.btnPrev);
		global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.BtnHorizHolder [this.btnPrev]));
		w8.Position = 1;
		w8.Expand = false;
		w8.Fill = false;
		// Container child BtnHorizHolder.Gtk.ButtonBox+ButtonBoxChild
		this.btnNext = new global::Gtk.Button ();
		this.btnNext.WidthRequest = 300;
		this.btnNext.CanFocus = true;
		this.btnNext.Name = "btnNext";
		this.btnNext.UseUnderline = true;
		this.btnNext.Label = global::Mono.Unix.Catalog.GetString (" Далее (Next)");
		global::Gtk.Image w9 = new global::Gtk.Image ();
		w9.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-goto-last", global::Gtk.IconSize.Menu);
		this.btnNext.Image = w9;
		this.BtnHorizHolder.Add (this.btnNext);
		global::Gtk.ButtonBox.ButtonBoxChild w10 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.BtnHorizHolder [this.btnNext]));
		w10.Position = 2;
		w10.Expand = false;
		w10.Fill = false;
		this.VertLayout.Add (this.BtnHorizHolder);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.VertLayout [this.BtnHorizHolder]));
		w11.Position = 3;
		w11.Expand = false;
		// Container child VertLayout.Gtk.Box+BoxChild
		this.pbTestTimer = new global::Gtk.ProgressBar ();
		this.pbTestTimer.HeightRequest = 10;
		this.pbTestTimer.Name = "pbTestTimer";
		this.pbTestTimer.Text = global::Mono.Unix.Catalog.GetString ("Время, с (Time, s): {0}");
		this.pbTestTimer.Fraction = 0.399999999999999;
		this.pbTestTimer.PulseStep = 0.01;
		this.VertLayout.Add (this.pbTestTimer);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.VertLayout [this.pbTestTimer]));
		w12.Position = 4;
		w12.Expand = false;
		w12.Fill = false;
		this.Add (this.VertLayout);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 924;
		this.DefaultHeight = 478;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.KeyPressEvent += new global::Gtk.KeyPressEventHandler (this.HotKeyPressed);
		this.txtQuestion.FocusGrabbed += new global::System.EventHandler (this.FocusGrabbedByTextQuestion);
		this.btnQuit.Clicked += new global::System.EventHandler (this.HaltTest);
		this.btnPrev.Clicked += new global::System.EventHandler (this.PreviousQuestion);
		this.btnNext.Clicked += new global::System.EventHandler (this.NextQuestion);
	}
}
