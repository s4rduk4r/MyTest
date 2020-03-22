using System;
using Gtk;

using MyTest;
using System.Collections;
using MyTestCreator;

public partial class MainWindow: Gtk.Window
{
	// Test
	TestDescription test = new TestDescription("Новый тест", "Безымянный", 0, TestDescription.ETestMode.Punish, DateTime.Now);
	// Test file to save
	string testFilename = null;

	// Answers to store
	ListStore testAnswers = null;
	ListStore testQuestions = null;
	class AnswerTreeViewColumns
	{
		public TreeViewColumn Number;
		public TreeViewColumn Text;
		public TreeViewColumn Type;

		public AnswerTreeViewColumns(TreeViewColumn numberColumn, TreeViewColumn answerTestColumn, TreeViewColumn answerTypeColumn)
		{
			Number = numberColumn;
			Text = answerTestColumn;
			Type = answerTypeColumn;
		}
	};

	class QuestionTreeViewColumns
	{
		public TreeViewColumn Number;
		public TreeViewColumn Text;
		public TreeViewColumn Value;

		public QuestionTreeViewColumns(TreeViewColumn number, TreeViewColumn text, TreeViewColumn value)
		{
			Number = number;
			Text = text;
			Value = value;
		}
	};

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		testAnswers = CreateAnswersTree ();
		testQuestions = CreateQuestionsTree ();
		UpdateInfoWidgets ();
	}

	// Updates the state of the info widgets
	private void UpdateInfoWidgets()
	{
		var name = (test.Name != null && test.Name.Length > 0) ? test.Name : "Новый тест" ;
		var author = (test.Author != null && test.Author.Length > 0) ? test.Author : "Безымянный автор";
		var date = string.Format ("{0}.{1}.{2}", test.Date.Day, test.Date.Month, test.Date.Year);
		this.Title = string.Format("{0} - {1}. Последняя модификация: {2}", name, author, date);
		setTestMode.Active = test.Mode == TestDescription.ETestMode.Punish;
		setTestMode.Label = setTestMode.Active ? "Режим: Строгий" : "Режим: Лояльный";
		setTestMode.ShortLabel = setTestMode.Label;
		TestModeAction.Active = test.Mode == TestDescription.ETestMode.Punish;
		lblTestMode.LabelProp = test.Mode == TestDescription.ETestMode.Punish ? "Режим: Строгий" : "Режим: Лояльный";
		lblTestTime.LabelProp = string.Format ("Время теста: {0}", test.Time > 0 ? test.Time.ToString() + " сек" : "inf");
		setTestTime.Label = test.Time > 0 ? test.Time.ToString () + " сек" : "inf";
		setTestTime.ShortLabel = test.Time > 0 ? test.Time.ToString () + " сек" : "inf";
	}

	private ListStore CreateAnswersTree()
	{
		// Create and attach model
		var testAnswers = new ListStore(typeof(TestAnswer));
		AnswersEditArea.Model = testAnswers;
		var testAnswerColumns = new AnswerTreeViewColumns (
			new TreeViewColumn ("No.", new CellRendererText (), "text", 0),
			new TreeViewColumn ("Формулировка ответа", new CellRendererText (), "text", 1),
			new TreeViewColumn ("Верный ответ", new CellRendererToggle (), "active", 2)
		);
		AnswersEditArea.AppendColumn (testAnswerColumns.Number);
		AnswersEditArea.AppendColumn (testAnswerColumns.Text);
		AnswersEditArea.AppendColumn (testAnswerColumns.Type);
		var answerNumCol = testAnswerColumns.Number;
		var render = answerNumCol.CellRenderers [0] as CellRendererText;
		answerNumCol.SetCellDataFunc (render, new TreeCellDataFunc (AnswerNumberColRender));

		var answerTextCol = testAnswerColumns.Text;
		answerTextCol.Resizable = true;
		var renderAnswerText = answerTextCol.CellRenderers [0] as CellRendererText;
		renderAnswerText.Editable = true;
		renderAnswerText.Edited += AnswerTextEdited;
		answerTextCol.SetCellDataFunc (renderAnswerText, new TreeCellDataFunc (AnswerTextColRender));
		var answerTypeCol = testAnswerColumns.Type;
		var renderAnswerType = answerTypeCol.CellRenderers [0] as CellRendererToggle;
		renderAnswerType.Activatable = true;
		renderAnswerType.Toggled += AnswerTypeToggled;
		answerTypeCol.SetCellDataFunc (renderAnswerType, new TreeCellDataFunc (AnswerTypeColRender));

		return testAnswers;
	}

	private ListStore CreateQuestionsTree()
	{
		// Create and attach model
		var testQuestions = new ListStore(typeof(TestQuestion));
		QuestionEditArea.Model = testQuestions;
		var testQuestionCols = new QuestionTreeViewColumns (
			new TreeViewColumn ("No.", new CellRendererText (), "text", 0),
			new TreeViewColumn ("Формулировка вопроса", new CellRendererText (), "text", 1),
			new TreeViewColumn ("Стоимость в баллах", new CellRendererSpin (), "value", 2)
		);
		QuestionEditArea.AppendColumn(testQuestionCols.Number);
		QuestionEditArea.AppendColumn (testQuestionCols.Text);
		QuestionEditArea.AppendColumn (testQuestionCols.Value);
		testQuestionCols.Text.Resizable = true;
		var render = testQuestionCols.Number.CellRenderers [0] as CellRendererText;
		testQuestionCols.Number.SetCellDataFunc (render, new TreeCellDataFunc (QuestionNumberColRender));
		render = testQuestionCols.Text.CellRenderers [0] as CellRendererText;
		testQuestionCols.Text.SetCellDataFunc (render, new TreeCellDataFunc (QuestionTextColRender));
		render.Editable = true;
		render.Edited += QuestionTextEdited;
		var renderValue = testQuestionCols.Value.CellRenderers [0] as CellRendererSpin;
		testQuestionCols.Value.SetCellDataFunc (renderValue, new TreeCellDataFunc (QuestionValueColRender));
		renderValue.ClimbRate = 1;
		renderValue.Adjustment = new Adjustment (0, 0, 1000, 1, 10, 1);
		renderValue.Digits = 0;
		renderValue.Editable = true;
		renderValue.Edited += QuestionValueEdited;

		return testQuestions;
	}

	// ----EDIT EVENTS---
	void AnswerTextEdited(object o, EditedArgs args)
	{
		TreeIter iter;
		testAnswers.GetIter(out iter, new Gtk.TreePath(args.Path));
		var answer = (testAnswers.GetValue (iter, 0) as TestAnswer);
		answer.Text = args.NewText;
	}

	void AnswerTypeToggled(object o, ToggledArgs args)
	{
		TreeIter iter;
		testAnswers.GetIter (out iter, new Gtk.TreePath (args.Path));
		var answer = testAnswers.GetValue (iter, 0) as TestAnswer;
		answer.Correct = !answer.Correct;
	}

	void QuestionTextEdited(object o, EditedArgs args)
	{
		TreeIter iter;
		testQuestions.GetIter(out iter, new Gtk.TreePath(args.Path));
		var question = (testQuestions.GetValue (iter, 0) as TestQuestion);
		question.Text = args.NewText;
	}

	void QuestionValueEdited(object renderer, EditedArgs args)
	{
		TreeIter iter;
		testQuestions.GetIter(out iter, new Gtk.TreePath(args.Path));
		var question = (testQuestions.GetValue (iter, 0) as TestQuestion);
		try
		{
			(renderer as CellRendererSpin).Adjustment.Value = Convert.ToSingle(args.NewText);
			question.Value = (float)(renderer as CellRendererSpin).Adjustment.Value;
		}
		catch(Exception exc)
		{
			var msgErr = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, exc.Message);
			msgErr.Run ();
			msgErr.Destroy ();
		}
	}

	// ----CELL RENDERERS----
	// Answers
	void AnswerNumberColRender(TreeViewColumn col, CellRenderer render, TreeModel treemdl, TreeIter iter)
	{
		var path = treemdl.GetPath (iter);
		(render as CellRendererText).Text = (path.Indices [0] + 1).ToString();
	}

	void AnswerTextColRender(TreeViewColumn col, CellRenderer render, TreeModel treemdl, TreeIter iter)
	{
		var answer = (treemdl.GetValue (iter, 0) as TestAnswer);
		(render as CellRendererText).Text = answer.Text;
	}

	void AnswerTypeColRender(TreeViewColumn col, CellRenderer render, TreeModel treemdl, TreeIter iter)
	{
		var answer = treemdl.GetValue (iter, 0) as TestAnswer;
		(render as CellRendererToggle).Active = answer.Correct;
	}

	// Questions
	void QuestionNumberColRender(TreeViewColumn col, CellRenderer render, TreeModel treemdl, TreeIter iter)
	{
		var path = treemdl.GetPath(iter);
		(render as CellRendererText).Text = (path.Indices [0] + 1).ToString ();
	}

	void QuestionTextColRender(TreeViewColumn col, CellRenderer render, TreeModel treemdl, TreeIter iter)
	{
		var question = treemdl.GetValue(iter, 0) as TestQuestion;
		(render as CellRendererText).Text = question.Text;
	}

	void QuestionValueColRender(TreeViewColumn col, CellRenderer render, TreeModel treemdl, TreeIter iter)
	{
		var question = treemdl.GetValue (iter, 0) as TestQuestion;
		(render as CellRendererSpin).Text = question.Value.ToString();
	}

	protected void QuestionSelected (object sender, EventArgs e)
	{
		var questionsTree = sender as TreeView;
		TreeIter iter;
		questionsTree.Selection.GetSelected (out iter);
		var question = testQuestions.GetValue (iter, 0) as TestQuestion;
		testAnswers.Clear ();
		foreach (var answer in question.Answers)
		{
			testAnswers.AppendValues (answer);
		}
	}

	// ----TOOLBAR----
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void QuitAction (object sender, EventArgs e)
	{
		Application.Quit ();
	}

	protected void NewTestAction(object sender, EventArgs e)
	{
		var newTestDlg = new NewTestDlg ();
		if (newTestDlg.Run () == (int)ResponseType.Ok)
		{
			if (test != null)
			{
				test.Questions.Clear ();
			}
			test = new TestDescription ();
			test.Name = newTestDlg.TestName;
			test.Author = newTestDlg.TestAuthor;
			test.Mode = newTestDlg.TestMode;
			test.Time = newTestDlg.TestTime;
			testQuestions.Clear ();
			testAnswers.Clear ();
			setTestMode.Active = test.Mode == TestDescription.ETestMode.Punish;
			TestModeAction.Active = test.Mode == TestDescription.ETestMode.Punish;
			UpdateInfoWidgets ();
		}
		newTestDlg.Destroy ();
	}

	protected void OpenTestAction (object sender, EventArgs e)
	{
		var openFileDlg = new FileChooserDialog ("Открыть файл теста...", this, FileChooserAction.Open, "_Отмена", (int)ResponseType.Cancel, "_Oткрыть", (int)ResponseType.Ok);
		openFileDlg.AddFilter (MyTestCreator.TestFileEncDec.MyTestRawFileFilter);
		openFileDlg.SetCurrentFolder (System.IO.Directory.GetCurrentDirectory());
		openFileDlg.LocalOnly = true;
		if (openFileDlg.Run () == (int)ResponseType.Ok)
		{
			try
			{
				var ifile = System.IO.File.OpenText (openFileDlg.Filename);
				var testFile = new System.IO.StringReader(ifile.ReadToEnd());
				ifile.Close();
				// Parse strings
				string ModeLoyal = "loyal";
				string ModePunish = "punish";
				string TimeInfinite = "inf";
				// Create new test
				test = new TestDescription();
				testFilename = openFileDlg.Filename;
				// Parse test file
				TestQuestion question = null;
				string line = null;
				do
				{
					line = testFile.ReadLine();
					// Skip commentary lines
					if (line == null || line.Length == 0 || line [0] == '#')
					{
						continue;
					}
					// Parse the rest of the file
					var parsed = TestFileParser.GetKeyValueFromLine(line);
					switch (parsed.Key)
					{
						case TestFileParser.ETestTags.Test:
							{
								test.Name = parsed.Value;
							}
							break;
						case TestFileParser.ETestTags.Author:
							{
								test.Author = parsed.Value;
							}
							break;
						case TestFileParser.ETestTags.Date:
							{
								var dateStr = parsed.Value.Split ('.');
								test.Date = new DateTime (Convert.ToInt32 (dateStr [2]), Convert.ToInt32 (dateStr [1]), Convert.ToInt32 (dateStr [0]));
							}
							break;
						case TestFileParser.ETestTags.Time:
							{
								// If time is infinite, then time = 0
								if (parsed.Value != TimeInfinite)
								{
									test.Time = Convert.ToInt32 (parsed.Value);
								}
							}
							break;
						case TestFileParser.ETestTags.Mode:
							{
								var modeString = parsed.Value.ToLower();
								if (modeString == ModePunish)
								{
									test.Mode = TestDescription.ETestMode.Punish;
								}
								else
								{
									if (modeString == ModeLoyal)
									{
										test.Mode = TestDescription.ETestMode.Loyal;
									}
								}
							}
							break;
						case TestFileParser.ETestTags.Question:
							{
								// Add previous question to the list
								if (question != null)
								{
									test.Questions.Add (question);
								}
								question = new TestQuestion (parsed.Value, 0);
							}
							break;
						case TestFileParser.ETestTags.Value:
							{
								// Ignore this keyword if no Question keyword encountered
								if (question != null)
								{
									question.Value = Convert.ToSingle (parsed.Value);
								}
							}
							break;
						case TestFileParser.ETestTags.Answer:
							{
								// Ignore this keyword if no Question keyword encountered
								if (question != null)
								{
									var answer = new TestAnswer (parsed.Value, false);
									question.Answers.Add (answer);
								}
							}
							break;
						case TestFileParser.ETestTags.AnswerTrue:
							{
								// Ignore this keyword if no Question keyword encountered
								if (question != null)
								{
									var answer = new TestAnswer (parsed.Value, true);
									question.Answers.Add (answer);
								}
							}
							break;
						default:
							continue;
					}
				}
				while(line != null && line.Length != 0);
				if (question != null)
				{
					if (!test.Questions.Contains (question))
					{
						test.Questions.Add (question);
					}
				}
				testFile.Close ();
				// Update the view
				testAnswers.Clear();
				testQuestions.Clear();
				foreach(var qstn in test.Questions)
				{
					testQuestions.AppendValues(qstn);
				}
				UpdateInfoWidgets();
			}
			catch(Exception err)
			{
				var msgErr = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Ошибка открытия файла!");
				msgErr.Run();
				msgErr.Destroy();
			}
		}
		openFileDlg.Destroy ();
	}

	protected void SaveTestAction (object sender, EventArgs e)
	{
		if (testFilename == null || (sender as Gtk.Action) == SaveAs)
		{
			var dlgSave = new FileChooserDialog ("Сохранить тест как...", this, FileChooserAction.Save, "Отмена", (int)ResponseType.Cancel, "Сохранить", (int)ResponseType.Ok);
			dlgSave.AddFilter (MyTestCreator.TestFileEncDec.MyTestRawFileFilter);
			dlgSave.LocalOnly = true;
			dlgSave.SetCurrentFolder (testFilename != null ? System.IO.Path.GetDirectoryName (testFilename) : System.IO.Directory.GetCurrentDirectory ());
			if (dlgSave.Run () == (int)ResponseType.Ok)
			{
				testFilename = System.IO.Path.ChangeExtension (dlgSave.Filename, MyTestCreator.TestFileEncDec.MyTestRaw);
			}
			dlgSave.Destroy ();
		}
		try
		{
			var data = new System.IO.MemoryStream (System.Text.Encoding.UTF8.GetBytes (test.ToString ()));
			var ofile = System.IO.File.Create (testFilename);
			data.CopyTo (ofile);
			ofile.Close();
			data.Close();
		}
		catch(Exception err)
		{
			var msgErr = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Ошибка записи файла!");
		}
	}

	//-----MENU-----
	protected void TestNameEnter (object sender, EventArgs e)
	{
		var dlgGetTestName = new OneStringAskDlg ("Введите название теста");
		dlgGetTestName.Modal = true;
		dlgGetTestName.Value = test.Name;
		if (dlgGetTestName.Run () == (int)ResponseType.Ok)
		{
			var testName = dlgGetTestName.Value;
			if (testName.Length > 0)
			{
				test.Name = dlgGetTestName.Value;
			}
		}
		dlgGetTestName.Destroy ();
		UpdateInfoWidgets ();
	}

	protected void AuthorNameEnter (object sender, EventArgs e)
	{
		var dlgGetAuthorName = new OneStringAskDlg ("Введите имя автора теста");
		dlgGetAuthorName.Modal = true;
		dlgGetAuthorName.Value = test.Author;
		if (dlgGetAuthorName.Run () == (int)ResponseType.Ok)
		{
			var author = dlgGetAuthorName.Value;
			if (author.Length > 0)
			{
				test.Author = author;
			}
		}
		dlgGetAuthorName.Destroy ();
		UpdateInfoWidgets ();
	}

	protected void AddQuestionClick (object sender, EventArgs e)
	{
		var dlgQuestionAdd = new OneStringValueAskDlg ("Новый вопрос", "Текст вопроса", "Стоимость в баллах (>0)");
		dlgQuestionAdd.Modal = true;
		if (dlgQuestionAdd.Run () == (int)ResponseType.Ok)
		{
			var text = dlgQuestionAdd.Text;
			if (text.Length > 0)
			{
				var value = dlgQuestionAdd.Value;
				var question = new TestQuestion (dlgQuestionAdd.Text, dlgQuestionAdd.Value);
				test.Questions.Add (question);
				testQuestions.AppendValues (question);
			}
		}
		dlgQuestionAdd.Destroy ();
	}

	protected void SetValueClick (object sender, EventArgs e)
	{
		TreeIter iter;
		QuestionEditArea.Selection.GetSelected (out iter);
		var question = testQuestions.GetValue (iter, 0) as TestQuestion;
		// If no selection - do nothing
		if (question == null)
		{
			return;
		}
		var dlgSetValue = new OneStringValueAskDlg ("Укажите новую стоимость вопроса", null, "Стоимость в баллах (>0)");
		dlgSetValue.Modal = true;
		dlgSetValue.Value = question.Value;
		if (dlgSetValue.Run () == (int)ResponseType.Ok)
		{
			var value = dlgSetValue.Value;
			if (value != question.Value)
			{
				question.Value = value;
			}
		}
		dlgSetValue.Destroy ();
	}

	protected void RemoveQuestionClick (object sender, EventArgs e)
	{
		TreeIter iter;
		QuestionEditArea.Selection.GetSelected (out iter);
		var question = testQuestions.GetValue (iter, 0);
		if (question == null)
		{
			return;
		}
		test.Questions.Remove (question as TestQuestion);
		testQuestions.Remove (ref iter);
		testAnswers.Clear ();
	}

	protected void AddAnswerClick (object sender, EventArgs e)
	{
		TreeIter iter;
		QuestionEditArea.Selection.GetSelected (out iter);
		var question = testQuestions.GetValue (iter, 0) as TestQuestion;
		if (question == null)
		{
			return;
		}
		var dlgAddAnswer = new OneStringAskDlg ("Новый ответ", true, "Верный ответ");
		dlgAddAnswer.Modal = true;
		if (dlgAddAnswer.Run () == (int)ResponseType.Ok)
		{
			var newAnswer = new TestAnswer (dlgAddAnswer.Value, dlgAddAnswer.IsChecked);
			question.Answers.Add (newAnswer);
			testAnswers.AppendValues (newAnswer);
		}
		dlgAddAnswer.Destroy ();
	}

	protected void RemoveAnswerClick (object sender, EventArgs e)
	{
		TreeIter iter;
		AnswersEditArea.Selection.GetSelected (out iter);
		var answer = testAnswers.GetValue (iter, 0);
		if (answer == null)
		{
			return;
		}
		foreach (var question in test.Questions)
		{
			foreach (var ans in question.Answers)
			{
				if (ans == answer as TestAnswer)
				{
					question.Answers.Remove (ans);
					testAnswers.Remove (ref iter);
					return;
				}
			}
		}
	}

	protected void SetTestModeClick (object sender, EventArgs e)
	{
		var obj = (sender as ToggleAction);
		if (obj == setTestMode)
		{
			TestModeAction.Active = obj.Active;
			if (obj.Active)
			{
				obj.StockId = "gtk-no";
				obj.Tooltip = "Строгий режим";
				obj.Label = "Режим: Строгий";
				obj.ShortLabel = obj.Label;
			}
			else
			{
				obj.StockId = "gtk-yes";
				obj.Tooltip = "Лояльный режим";
				obj.Label = "Режим: Лояльный";
				obj.ShortLabel = obj.Label;
			}
		}
		else
		{
			if (obj == TestModeAction)
			{
				setTestMode.Active = obj.Active;
			}
			else
			{
				return;
			}
		}
		if ((sender as ToggleAction).Active)
		{
			test.Mode = TestDescription.ETestMode.Punish;
			lblTestMode.LabelProp = "Режим: Строгий";
		}
		else
		{
			test.Mode = TestDescription.ETestMode.Loyal;
			lblTestMode.LabelProp = "Режим: Лояльный";
		}
	}

	protected void SetTestTimeClick (object sender, EventArgs e)
	{
		var dlgSetTime = new OneStringValueAskDlg ("Время прохождения теста", null, "Время, сек (0 -> inf)", 9999);
		dlgSetTime.Value = test.Time;
		if (dlgSetTime.Run () == (int)ResponseType.Ok)
		{
			test.Time = (int)dlgSetTime.Value;
			lblTestTime.LabelProp = string.Format ("Время теста: {0}", test.Time > 0 ? test.Time.ToString () + " сек" : "inf");
			setTestTime.Label = test.Time > 0 ? test.Time.ToString () + " сек" : "inf";
			setTestTime.ShortLabel = test.Time > 0 ? test.Time.ToString () + " сек" : "inf";
		}
		dlgSetTime.Destroy ();
	}

	protected void DecompileTestClick (object sender, EventArgs e)
	{
		var testDecompileDlg = new FileChooserDialog ("Укажите тест для декомпиляции", this, FileChooserAction.Open, 
													  "Отмена", (int)ResponseType.Cancel, "Декомпиляция", (int)ResponseType.Ok);
		testDecompileDlg.AddFilter (TestFileEncDec.MyTestCompiledFileFilter);
		testDecompileDlg.LocalOnly = true;
		testDecompileDlg.SetCurrentFolder (System.IO.Directory.GetCurrentDirectory ());
		if (testDecompileDlg.Run () == (int)ResponseType.Ok)
		{
			try
			{
				var testToDecompile = testDecompileDlg.Filename;
				var testDecompiled = System.IO.Path.ChangeExtension(testToDecompile, TestFileEncDec.MyTestRaw);
				var data = new System.IO.MemoryStream(new TestFileEncDec(testToDecompile, TestFileEncDec.ETestFileMode.Decode).Data);
				var testDecompiledFile = System.IO.File.Create(testDecompiled, (int)data.Length);
				data.CopyTo(testDecompiledFile);
				testDecompiledFile.Close();
				data.Close();
				var msgOk = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Операция успешно завершена!");
				msgOk.Run ();
				msgOk.Destroy ();
			}
			catch(Exception err)
			{
				var msgErr = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Не удалось декомпилировать!");
				msgErr.Run ();
				msgErr.Destroy ();
			}
		}
		testDecompileDlg.Destroy ();
	}

	protected void CompileTestClick (object sender, EventArgs e)
	{
		var msgAsk = new MessageDialog (this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Компоновать открытый тест?");
		if (msgAsk.Run () == (int)ResponseType.Yes)
		{
			try
			{
				if (testFilename == null)
				{
					SaveTestAction (sender, e);
				}
				var testCompiledFilename = System.IO.Path.ChangeExtension (testFilename, TestFileEncDec.MyTestCompiled);
				var data = new System.IO.MemoryStream (new TestFileEncDec (testFilename, TestFileEncDec.ETestFileMode.Encode).Data);
				var testCompiledFile = System.IO.File.Create (testCompiledFilename, (int)data.Length);
				data.CopyTo (testCompiledFile);
				data.Close ();
				testCompiledFile.Close ();
				var msgOk = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Операция успешно завершена!");
				msgOk.Run ();
				msgOk.Destroy ();
			}
			catch (Exception err)
			{
				var msgErr = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Не удалось скомпоновать!");
				msgErr.Run ();
				msgErr.Destroy ();
			}
		}
		else
		{
			msgAsk.Destroy ();
			var testOpener = new FileChooserDialog ("Укажите тест для компоновки", this, FileChooserAction.Open, 
													"Отмена", (int)ResponseType.Cancel, "Компоновать", (int)ResponseType.Ok);
			testOpener.AddFilter (TestFileEncDec.MyTestRawFileFilter);
			testOpener.LocalOnly = true;
			testOpener.SetCurrentFolder (System.IO.Directory.GetCurrentDirectory ());
			if (testOpener.Run () == (int)ResponseType.Ok)
			{
				try
				{
					var testName = testOpener.Filename;
					var testCompiledFilename = System.IO.Path.ChangeExtension (testName, TestFileEncDec.MyTestCompiled);
					var data = new System.IO.MemoryStream (new TestFileEncDec (testName, TestFileEncDec.ETestFileMode.Encode).Data);
					var testCompiledFile = System.IO.File.Create (testCompiledFilename, (int)data.Length);
					data.CopyTo (testCompiledFile);
					data.Close ();
					testCompiledFile.Close ();
					var msgOk = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, "Операция успешно завершена!");
					msgOk.Run ();
					msgOk.Destroy ();
				}
				catch(Exception err)
				{
					var msgErr = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Не удалось скомпоновать!");
					msgErr.Run ();
					msgErr.Destroy ();
				}
			}
			testOpener.Destroy ();
		}
		msgAsk.Destroy ();
	}

	protected void HelpActionClicked (object sender, EventArgs e)
	{
		var helpDir = System.IO.Path.Combine (System.IO.Directory.GetCurrentDirectory (), "Help");
		var helpFile = System.IO.Path.Combine (helpDir, "help.pdf");
		if (!System.IO.Directory.Exists (helpDir))
		{
			System.IO.Directory.CreateDirectory (helpDir);
		}
		if (System.IO.File.Exists (helpFile))
		{
			System.Diagnostics.Process.Start (helpFile);
		}
		else
		{
			var msgErr = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "Не найден файл справки!");
			msgErr.Run ();
			msgErr.Destroy ();
		}
	}

	protected void AboutClick (object sender, EventArgs e)
	{
		var aboutDlg = new AboutDialog ();
		aboutDlg.ProgramName = "MyTest Creator";
		aboutDlg.Title = "About MyTest Creator";
		aboutDlg.Version = "v1.0";
		aboutDlg.Comments = "Программа создания тестов для оболочки MyTest";
		aboutDlg.Authors = new string[]{ "Глушенков А.Н., 2017" };
		aboutDlg.Documenters = new string[] { "Глушенков А.Н., 2017" };
		aboutDlg.Copyright = "Крымский федеральный университет им. В.И. Вернадского";
		aboutDlg.Copyright += "\nФизико-технический институт";
		aboutDlg.Copyright += "\nКафедра медицинской физики и информатики";
		aboutDlg.Run ();
		aboutDlg.Destroy ();
	}

}
