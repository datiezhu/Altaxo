﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


using Altaxo.Gui;
using Altaxo.Gui.Scripting;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.AddIn;

namespace Altaxo.Gui.Scripting
{
	/// <summary>
	/// Interaction logic for SDPureScriptControlWpf.xaml
	/// </summary>
	[UserControlForController(typeof(IPureScriptViewEventSink), 120)]
	public partial class SDPureScriptControlWpf : UserControl, IPureScriptView
	{
		AvalonEditViewContent _editViewContent;
		ICSharpCode.AvalonEdit.AddIn.CodeEditor _codeView;

		public SDPureScriptControlWpf()
		{
			InitializeComponent();
		}

		void InitializeEditor(string initialText, string scriptName)
		{
			// The trick is here to create an untitled file, so that the binary content is used, 
			// but at the same time to give the file an unique name in order to get processed by the parser
			var openFile = FileService.CreateUntitledOpenedFile(scriptName, StringToByte(initialText));

			_editViewContent = new AvalonEditViewContent(openFile);
			this._codeView = _editViewContent.CodeEditor;

			this._codeView.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(edFormula_IsVisibleChanged);
			this._codeView.Name = "edFormula";
			this.Content = _codeView;
		}



		bool _registered;
		void Register()
		{
			if (!_registered)
			{
				_registered = true;
				ParserService.RegisterModalContent(_editViewContent);
			}

		}
		void Unregister()
		{

			ParserService.UnregisterModalContent();
			_registered = false;

		}


		#region IPureScriptView Members

		IPureScriptViewEventSink _controller;
		public IPureScriptViewEventSink Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}

		public string ScriptText
		{
			get
			{
				return this._codeView.Document.Text;
			}
			set
			{
				if (this._codeView == null)
				{
					string scriptName = System.Guid.NewGuid().ToString() + ".cs";
					InitializeEditor(value, scriptName);
				}
				else if (this._codeView.Document.Text != value)
				{
					this._codeView.Document.Text = value;
				}
			}
		}

		public int ScriptCursorLocation
		{
			set
			{
				var location = _codeView.Document.GetLocation(value);
				_codeView.PrimaryTextEditor.TextArea.Caret.Location = location;

			}

		}

		public int InitialScriptCursorLocation
		{
			set
			{
				// do nothing here, because folding is active
			}

		}

		/// <summary>
		/// Sets the cursor location inside the script and focuses on the text. Line and column are starting with 1.
		/// </summary>
		/// <param name="line">Script line (1-based).</param>
		/// <param name="column">Script column (1-based).</param>
		public void SetScriptCursorLocation(int line, int column)
		{
			_codeView.PrimaryTextEditor.TextArea.Caret.Location = new ICSharpCode.AvalonEdit.Document.TextLocation(line, column);
			_codeView.PrimaryTextEditor.TextArea.Focus();
		}


		public void MarkText(int pos1, int pos2)
		{

		}




		#endregion

		Window _parentForm;
		void edFormula_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (_codeView.IsVisible)
			{
				if (null == _parentForm)
				{
					_parentForm = Window.GetWindow(this);
					_parentForm.Closing +=_parentForm_Closing;

					Register();
				}
			}
		}

		private void _parentForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_parentForm.Closing -= _parentForm_Closing;
			Unregister();
		}


		public static byte[] StringToByte(string fileContent)
		{
			MemoryStream memoryStream = new MemoryStream();
			TextWriter tw = new StreamWriter(memoryStream);
			tw.Write(fileContent);
			tw.Flush();
			return memoryStream.ToArray();
		}

	}
}
