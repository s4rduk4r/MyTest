using System;
using System.Collections.Generic;
using Gtk;

/*
 * Project: MyTest App
 * Module: ResultsWindow
 * Author: Andrey N. Glushenkov
 * Date: 04.10.2017
 * Description:
 * ResultsWindow is a representation of the user test score. It has 3 buttons: New, Retry, Quit.
 * 	New -> Takes user to the start of the program where it has to select new test file
 * 	Retry -> Shuffles this test, nullyfies current score and shrinks test time by 1/3.
 *  No more than 3 tries allowed. The ending result is the worst of all tries.
 * 	Quit -> Terminates the program
 */
namespace MyTest
{
	public partial class ResultsWindow : Gtk.Window
	{
		public ResultsWindow (TestResults testResults) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.lblTestName.LabelProp = testResults.TestState.Name;
			resultsView.AppendColumn ("No.", new Gtk.CellRendererText (), "text", 0);
			resultsView.AppendColumn ("Вопрос (Question)", new Gtk.CellRendererText(), "text", 1);
			resultsView.AppendColumn ("Баллы (Score)", new Gtk.CellRendererText(), "text", 2);
			resultsView.AppendColumn ("Точность, % (Accuracy, %)", new Gtk.CellRendererText (), "text", 3);
			Gtk.ListStore questionsList = new Gtk.ListStore (typeof(string), typeof(string), typeof(string), typeof(string));
			resultsView.Model = questionsList;
			// Take max symbols to fit data on screen
			var display = this.Display.GetScreen (0);
			int fntSize = (int)(this.PangoContext.FontDescription.Size / Pango.Scale.PangoScale);
			int maxSymbols = display.Width / fntSize;
			// Fill in the view with results
			var questions = testResults.TestState.Questions;
			foreach (var question in questions)
			{
				var num = (questions.IndexOf (question) + 1).ToString ();
				var text = StringMultilineWrapper.StringWrapper.WordWrap(question.Text, maxSymbols);
				var score = string.Format("{0:F2}", testResults.QuestionScore (question));
				var acc = testResults.QuestionScore (question) / question.Value;
				acc = acc > 0 ? acc : 0;
				var accuracy = string.Format ("{0:P2}", acc);
				questionsList.AppendValues (num, text, score, accuracy);
			}
			var percent = testResults.Score / testResults.TestState.Value;
			var mark = 5 * percent;
			questionsList.AppendValues ("", "------------------------------", "------");
			questionsList.AppendValues ("", "Баллы (Score):", string.Format("{0:F2}", testResults.Score));
			questionsList.AppendValues ("", "Точных ответов (Accurate answers): ", string.Format("{0:P2}", percent > 0 ? percent : 0));
			questionsList.AppendValues ("", "Оценка (Mark): ", GetMark(mark));
			resultsView.ShowAll ();
		}

		string GetMark(double mark)
		{
			if (mark == 5)
			{
				return "Идеально: 5+ ( A )";
			}
			var value = (int)Math.Round (mark + 0.001);
			switch (value)
			{
			case 5:
				return "Отлично: 5 ( B )";
			case 4:
				return "Хорошо: 4 ( C )";
			case 3:
				return "Удовлетворительно: 3 ( D )";
			case 2:
				return "Неудовлетворительно: 2 ( E )";
			}
			if (value > 0 && value < 2)
			{
				return "Плохо: 1 ( F ) ";
			}
			return "Ужасно: 0 ( FX )";
		}

		protected void BtnQuitClick (object sender, EventArgs e)
		{
			Gtk.Application.Quit ();
		}

		protected void NewTest (object sender, EventArgs e)
		{
			this.Destroy ();
			if (!Program.StartTest ())
			{
				Application.Quit ();
			}
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected void MakeFullscreen (object o, SizeAllocatedArgs args)
		{
			this.Fullscreen ();
		}
	}
}

