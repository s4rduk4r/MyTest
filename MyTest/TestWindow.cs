using System;
using Gtk;
using System.Collections.Generic;
using System.Timers;
using MyTest;
using StringMultilineWrapper;

public partial class TestWindow: Gtk.Window
{
	// Answers for a question
	CheckButton[] answers = null;
	// Test
	IList<TestQuestion> questions = null;
	IEnumerator<TestQuestion> question = null;
	TestResults results = null;
	TestState testState = null;
	Timer testTimer = null;
	double timeTick = 500; // ms
	double time = 0;

	public TestWindow (IList<TestQuestion> questions, TestResults results) : base (Gtk.WindowType.Toplevel)
	{
		this.Fullscreen ();
		Build ();
		// Set fonts and text adjustment to the center of the widget
		txtQuestion.PixelsAboveLines = (int)(txtQuestion.Allocation.Height * 0.9);
		txtQuestion.ModifyFont (Pango.FontDescription.FromString("sans,serif,monospace bold 14"));
		// Prepare everything for collecting data
		this.results = results;
		this.testState = results.TestState;
		this.questions = questions;
		this.Title = testState.Name;
		question = questions.GetEnumerator ();
		question.MoveNext ();
		// Show first question
		ShowQuestion (question.Current);
		// Start timer
		if (testState.Time != TestState.TimeInfinite)
		{
			pbTestTimer.Text = "";
			pbTestTimer.Fraction = 0.0;
			testTimer = new Timer (timeTick);
			testTimer.Elapsed += TimerTick;
			testTimer.Start ();
		}
		else
		{
			pbTestTimer.Destroy ();
		}
	}

	void ShowTestTitle()
	{
		var iQuestionNumber = testState.Questions.IndexOf (question.Current) + 1;
		var iQuestionsCount = testState.Questions.Count;
		lblTestName.Justify = Justification.Center;
		lblTestName.LabelProp = string.Format ("{0}\nВопрос: {1} из {2}", testState.Name, iQuestionNumber, iQuestionsCount);
	}

	// Stop timer
	void StopTimer()
	{
		if (testTimer != null)
		{
			testTimer.Stop ();
			testTimer.Close ();
		}
	}

	// Timer event
	void TimerTick(object sender, ElapsedEventArgs args)
	{
		time += 0.001 * timeTick;
		Gtk.Application.Invoke (
			delegate
			{
				pbTestTimer.Fraction = time / testState.Time;
			}
		);
		if (time >= testState.Time)
		{
			Application.Invoke (NextQuestion);
			Application.Invoke (HaltTest);
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
		
	public void ShowQuestion(TestQuestion question)
	{
		ShowTestTitle ();
		txtQuestion.Buffer.Text = question.Text;
		// Clean previous answers
		foreach (var child in AnswersHolder.Children)
		{
			child.Destroy ();
		}
		// Create new answer items
		answers = new CheckButton[question.Answers.Count];
		// Get user selected items
		var answered = results.Answered(question);
		for (int i = 0; i < question.Answers.Count; ++i)
		{
			answers [i] = new CheckButton ();
			var display = this.Display.GetScreen (0);
			int fntSize = (int)(answers [i].PangoContext.FontDescription.Size / Pango.Scale.PangoScale);
			int maxSymbols = display.Width / fntSize; 
			answers [i].Label = StringWrapper.WordWrap(question.Answers[i].Text, maxSymbols);
			AnswersHolder.Add (answers[i]);
			// Show answers already checked by user
			foreach (var answer in answered)
			{
				if (answer == question.Answers [i])
				{
					answers [i].Active = true;
				}
			}
		}
		AnswersHolder.ShowAll ();
		// Fix focus
		FocusGrabbedByTextQuestion (null, null);
	}

	void ShowResults()
	{
		StopTimer ();
		ResultsWindow resWin = new ResultsWindow(results);
		resWin.Show ();
		this.Destroy ();
	}

	protected void NextQuestion (object sender, EventArgs e)
	{
		// Get user selected answers
		var userAnswers = new List<TestAnswer>();
		for (int i = 0; i < answers.Length; ++i)
		{
			if (answers [i].Active)
			{
				userAnswers.Add (question.Current.Answers [i]);
			}
		}
		results.ForgetAnswer (question.Current);
		results.Answer (question.Current, userAnswers);

		// Show next question
		if (question.MoveNext ())
		{
			ShowQuestion (question.Current);
		}
		// If all questions has been answered, then show test results
		else
		{
			ShowResults ();
		}
	}

	protected void PreviousQuestion (object sender, EventArgs e)
	{
		int index = question.Current != null ? questions.IndexOf (question.Current) - 1 : questions.Count - 2;
		if (index <= 0)
		{
			index = 0;
		}
		// Set enumerator on the previous question
		question.Reset ();
		for (int i = 0; i <= index; ++i)
		{
			question.MoveNext ();
		}
		ShowQuestion (question.Current);
	}

	protected void HaltTest (object sender, EventArgs e)
	{
		ShowResults ();
	}

	protected void MakeFullscreen (object o, SizeAllocatedArgs args)
	{
		this.Fullscreen ();
	}

	protected void HotKeyPressed (object o, KeyPressEventArgs args)
	{
		// Help screen
		if (args.Event.Key == Gdk.Key.F1)
		{
			var help = "F1 - Окно помощи (это окно)";
			help += "\n----------------------------";
			help += "\nCtrl+R - обновить содержимое окна";
			help += "\n----------------------------";
			help += "\nPageDown - следующий вопрос";
			help += "\nPageUp - предыдущий вопрос";
			help += "\n1,2,3,4,5,6,7,8,9,0 - отметить/снять ответ";
			var msgHelp = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, help);
			msgHelp.Run ();
			msgHelp.Destroy ();
			return;
		}
		// Hopefully manual fix for the screen problem
		if ((args.Event.State & Gdk.ModifierType.ControlMask) == Gdk.ModifierType.ControlMask)
		{
			// Refresh the window position
			if ((args.Event.Key == Gdk.Key.r))
			{
				this.Fullscreen ();
				return;
			}
		}
		// Anti-cheat Alt+Tab blocker
		if ((args.Event.State & Gdk.ModifierType.Mod1Mask) == Gdk.ModifierType.Mod1Mask)
		{
			if (args.Event.Key == Gdk.Key.Tab)
			{
				var msg = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Не подглядывай!\nDon't try to cheat!\n:P");
				msg.Run ();
				msg.Destroy ();
				return;
			}
		}
		// Next question
		if (args.Event.Key == Gdk.Key.Page_Down)
		{
			NextQuestion (null, null);
			return;
		}
		// Previous question
		if (args.Event.Key == Gdk.Key.Page_Up)
		{
			PreviousQuestion (null, null);
			return;
		}
		// Halt test
		if (
			args.Event.Key == Gdk.Key.Escape || 
			(((args.Event.State & Gdk.ModifierType.Mod1Mask) == Gdk.ModifierType.Mod1Mask) && args.Event.Key == Gdk.Key.F4)
		   )
		{
			var question = "Вы уверены, что хотите завершить тест досрочно?\nAre you sure you wish to halt the test?";
			var msg = new MessageDialog (this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, question);
			int result = msg.Run ();
			msg.Destroy ();
			if (result == (int)ResponseType.Yes)
			{
				HaltTest (null, null);
			}
			return;
		}
		// Select/Deselect answer from 1 to 10
		switch (args.Event.Key)
		{
		case Gdk.Key.Key_1:
			{
				if (answers.Length >= 1)
				{
					answers [0].Active = !answers [0].Active;
				}
			}
			break;
		case Gdk.Key.Key_2:
			{
				if (answers.Length >= 2)
				{
					answers [1].Active = !answers [1].Active;
				}
			}
			break;
		case Gdk.Key.Key_3:
			{
				if (answers.Length >= 3)
				{
					answers [2].Active = !answers [2].Active;
				}
			}
			break;
		case Gdk.Key.Key_4:
			{
				if (answers.Length >= 4)
				{
					answers [3].Active = !answers [3].Active;
				}
			}
			break;
		case Gdk.Key.Key_5:
			{
				if (answers.Length >= 5)
				{
					answers [4].Active = !answers [4].Active;
				}
			}
			break;
		case Gdk.Key.Key_6:
			{
				if (answers.Length >= 6)
				{
					answers [5].Active = !answers [5].Active;
				}
			}
			break;
		case Gdk.Key.Key_7:
			{
				if (answers.Length >= 7)
				{
					answers [6].Active = !answers [6].Active;
				}
			}
			break;
		case Gdk.Key.Key_8:
			{
				if (answers.Length >= 8)
				{
					answers [7].Active = !answers [7].Active;
				}
			}
			break;
		case Gdk.Key.Key_9:
			{
				if (answers.Length >= 9)
				{
					answers [8].Active = !answers [8].Active;
				}
			}
			break;
		case Gdk.Key.Key_0:
			{
				if (answers.Length >= 10)
				{
					answers [9].Active = !answers [9].Active;
				}
			}
			break;
		default:
			break;
		}
	}

	protected void FocusGrabbedByTextQuestion (object sender, EventArgs e)
	{
		// Set focus outside of the Question text area
		if (answers.Length > 0)
		{
			answers [0].GrabFocus ();
		}
		else
		{
			this.btnNext.GrabFocus ();
		}
	}
}
