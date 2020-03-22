
// This file has been generated by the GUI designer. Do not modify.
namespace MyTestCreator
{
	public partial class SelectTestToEncryptDlg
	{
		private global::Gtk.FileChooserWidget TestFileChooser;
		
		private global::Gtk.Button btnQuit;
		
		private global::Gtk.Button btnRestore;
		
		private global::Gtk.Button btnCompile;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget MyTestCreator.SelectTestToEncryptDlg
			this.Name = "MyTestCreator.SelectTestToEncryptDlg";
			this.Title = global::Mono.Unix.Catalog.GetString ("MyTest Composer");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child MyTestCreator.SelectTestToEncryptDlg.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.TestFileChooser = new global::Gtk.FileChooserWidget (((global::Gtk.FileChooserAction)(0)));
			this.TestFileChooser.Name = "TestFileChooser";
			w1.Add (this.TestFileChooser);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(w1 [this.TestFileChooser]));
			w2.Position = 0;
			// Internal child MyTestCreator.SelectTestToEncryptDlg.ActionArea
			global::Gtk.HButtonBox w3 = this.ActionArea;
			w3.Name = "dialog1_ActionArea";
			w3.Spacing = 10;
			w3.BorderWidth = ((uint)(5));
			w3.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.btnQuit = new global::Gtk.Button ();
			this.btnQuit.CanDefault = true;
			this.btnQuit.CanFocus = true;
			this.btnQuit.Name = "btnQuit";
			this.btnQuit.UseStock = true;
			this.btnQuit.UseUnderline = true;
			this.btnQuit.Label = "gtk-quit";
			this.AddActionWidget (this.btnQuit, 0);
			global::Gtk.ButtonBox.ButtonBoxChild w4 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w3 [this.btnQuit]));
			w4.Expand = false;
			w4.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.btnRestore = new global::Gtk.Button ();
			this.btnRestore.CanFocus = true;
			this.btnRestore.Name = "btnRestore";
			this.btnRestore.UseUnderline = true;
			this.btnRestore.Label = global::Mono.Unix.Catalog.GetString ("Исходный текст");
			global::Gtk.Image w5 = new global::Gtk.Image ();
			w5.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-undo", global::Gtk.IconSize.Menu);
			this.btnRestore.Image = w5;
			this.AddActionWidget (this.btnRestore, 0);
			global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w3 [this.btnRestore]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.btnCompile = new global::Gtk.Button ();
			this.btnCompile.CanDefault = true;
			this.btnCompile.CanFocus = true;
			this.btnCompile.Name = "btnCompile";
			this.btnCompile.UseUnderline = true;
			this.btnCompile.Label = global::Mono.Unix.Catalog.GetString ("Скомпоновать");
			global::Gtk.Image w7 = new global::Gtk.Image ();
			w7.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-apply", global::Gtk.IconSize.Menu);
			this.btnCompile.Image = w7;
			this.AddActionWidget (this.btnCompile, -10);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w3 [this.btnCompile]));
			w8.Position = 2;
			w8.Expand = false;
			w8.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 769;
			this.DefaultHeight = 565;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDelete);
			this.btnQuit.Clicked += new global::System.EventHandler (this.BtnQuit);
			this.btnRestore.Clicked += new global::System.EventHandler (this.BtnDecomposeClick);
			this.btnCompile.Clicked += new global::System.EventHandler (this.BtnComposeClick);
		}
	}
}