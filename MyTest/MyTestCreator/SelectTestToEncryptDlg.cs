using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MyTestCreator
{
	public partial class SelectTestToEncryptDlg : Gtk.Dialog
	{
		const string MsgOk = "Операция успешно завершена!";
		const string MsgErr = "Не удалось сформировать файл!";
		const string MsgErrCompile = "Файл уже и так скомпонован!";
		const string MsgErrDecompile = "Файл уже и так восстановлен!";

		public SelectTestToEncryptDlg ()
		{
			this.Build ();
			TestFileChooser.AddFilter (TestFileEncDec.MyTestRawFileFilter);
			TestFileChooser.AddFilter (TestFileEncDec.MyTestCompiledFileFilter);
			TestFileChooser.SetCurrentFolder (System.IO.Directory.GetCurrentDirectory ());
		}

		protected void OnDelete (object o, Gtk.DeleteEventArgs args)
		{
			Gtk.Application.Quit ();
			args.RetVal = true;
		}

		protected void BtnQuit (object sender, EventArgs e)
		{
			Gtk.Application.Quit ();
		}

		protected void BtnComposeClick (object sender, EventArgs e)
		{
			// Don't compile already compiled files
			if (TestFileChooser.Filter.Name == TestFileEncDec.MyTestCompiledFileFilter.Name)
			{
				MessageBox(MsgErrCompile, false);
				return;
			}
			try
			{
				var testFileEnc = new TestFileEncDec(TestFileChooser.Filename, TestFileEncDec.ETestFileMode.Encode);
				var data = testFileEnc.Data;
				var outfile = TestFileChooser.Filename.Replace(TestFileEncDec.MyTestRaw, TestFileEncDec.MyTestCompiled);
				var testFileCompiled = File.Create(outfile);
				testFileCompiled.Write(data, 0, data.Length);
				testFileCompiled.Close();
				MessageBox (MsgOk);
			}
			catch(Exception err)
			{
				MessageBox (MsgErr, false);
				return;
			}
		}

		protected void BtnDecomposeClick (object sender, EventArgs e)
		{
			// Don't compile already compiled files
			if (TestFileChooser.Filter.Name == TestFileEncDec.MyTestRawFileFilter.Name)
			{
				MessageBox(MsgErrDecompile, false);
				return;
			}
			try
			{
				var testFileDec = new TestFileEncDec(TestFileChooser.Filename, TestFileEncDec.ETestFileMode.Decode);
				var data = testFileDec.Data;
				var outfile = TestFileChooser.Filename.Replace(TestFileEncDec.MyTestCompiled, TestFileEncDec.MyTestRaw);
				var testFileRaw = File.Create(outfile);
				testFileRaw.Write(data, 0, data.Length);
				testFileRaw.Close();
				MessageBox(MsgOk);
			}
			catch(Exception err)
			{
				MessageBox(MsgErr, false);
				return;
			}
		}

		// Message box
		void MessageBox(string msg, bool info = true)
		{
			var mb = new Gtk.MessageDialog (this, Gtk.DialogFlags.Modal, 
											info ? Gtk.MessageType.Info : Gtk.MessageType.Error,
											info ? Gtk.ButtonsType.Ok : Gtk.ButtonsType.Close, 
											msg);
			mb.Run ();
			mb.Destroy ();
		}
	}
}

