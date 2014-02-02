﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2011 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////

#endregion Copyright

using Altaxo.Gui;
using Altaxo.Gui.Common;
using Altaxo.Main.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace Altaxo.Gui
{
	public class GuiFactoryServiceWpfWin : Altaxo.Main.Services.GUIFactoryService
	{
		#region Still dependent on Windows Forms

		public override IntPtr MainWindowHandle
		{
			get { return ((System.Windows.Forms.IWin32Window)Current.Workbench.ViewObject).Handle; }
		}

		public override Altaxo.Graph.RectangleD GetScreenInformation(double virtual_x, double virtual_y)
		{
			var wa = System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point((int)virtual_x, (int)virtual_y));

			return new Altaxo.Graph.RectangleD(wa.X, wa.Y, wa.Width, wa.Height);
		}

		#endregion Still dependent on Windows Forms

		public System.Windows.Window MainWindowWpf
		{
			get
			{
				return (System.Windows.Window)Current.Workbench.ViewObject;
			}
		}

		private Altaxo.Graph.PointD2D _screenResolution;

		/// <summary>Gets the screen resolution that is set in windows in dots per inch.</summary>
		public override Altaxo.Graph.PointD2D ScreenResolutionDpi
		{
			get
			{
				if (_screenResolution.IsEmpty)
				{
					if (null == Current.Workbench.ViewObject)
						return new Altaxo.Graph.PointD2D(96, 96); // until we have a workbench, we assume 96 dpi
					var MainWindowPresentationSource = System.Windows.PresentationSource.FromVisual((System.Windows.Window)Current.Workbench.ViewObject);
					if (null == MainWindowPresentationSource)
						return new Altaxo.Graph.PointD2D(96, 96); // until we have a valid presentation source, we assume 96 dpi
					var m = MainWindowPresentationSource.CompositionTarget.TransformToDevice;
					_screenResolution = new Altaxo.Graph.PointD2D(96 * m.M11, 96 * m.M22);
				}
				return _screenResolution;
			}
		}

		public override bool InvokeRequired()
		{
			return Current.Workbench.SynchronizingObject.InvokeRequired;
		}

		/// <summary>
		/// Consider using rather either one of the methods Execute or Evaluate instead of this. This is only a basic function for invoking a method synchronously with the Gui.
		/// </summary>
		/// <param name="act">Method to invoke.</param>
		/// <param name="args">Method parameter.</param>
		/// <returns>The return value of the method.</returns>
		public override object Invoke(Delegate act, object[] args)
		{
			return Current.Workbench.SynchronizingObject.Invoke(act, args);
		}

		public override IAsyncResult BeginInvoke(Delegate act, object[] args)
		{
			return Current.Workbench.SynchronizingObject.BeginInvoke(act, args);
		}

		/// <summary>
		/// Shows a configuration dialog for an object.
		/// </summary>
		/// <param name="controller">The controller to show in the dialog</param>
		/// <param name="title">The title of the dialog to show.</param>
		/// <param name="showApplyButton">If true, the "Apply" button is visible on the dialog.</param>
		/// <returns>True if the object was successfully configured, false otherwise.</returns>
		public override bool ShowDialog(IMVCAController controller, string title, bool showApplyButton)
		{
			return Evaluate(InternalShowDialog, controller, title, showApplyButton);
		}

		/// <summary>
		/// Shows a configuration dialog for an object.
		/// </summary>
		/// <param name="controller">The controller to show in the dialog</param>
		/// <param name="title">The title of the dialog to show.</param>
		/// <param name="showApplyButton">If true, the "Apply" button is visible on the dialog.</param>
		/// <returns>True if the object was successfully configured, false otherwise.</returns>
		private bool InternalShowDialog(IMVCAController controller, string title, bool showApplyButton)
		{
			if (controller.ViewObject == null)
			{
				FindAndAttachControlTo(controller);
			}

			if (controller.ViewObject == null)
				throw new ArgumentException("Can't find a view object for controller of type " + controller.GetType());

			if (controller is Altaxo.Gui.Scripting.IScriptController)
			{
				var dlgctrl = new Altaxo.Gui.Scripting.ScriptExecutionDialog((Altaxo.Gui.Scripting.IScriptController)controller);
				dlgctrl.Owner = MainWindowWpf;
				dlgctrl.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
				var pos = System.Windows.Input.Mouse.GetPosition(MainWindowWpf);
				dlgctrl.Top = pos.Y;
				dlgctrl.Left = pos.X;
				return (true == dlgctrl.ShowDialog());
			}
			else if (controller.ViewObject is System.Windows.UIElement)
			{
				var dlgview = new DialogShellViewWpf((System.Windows.UIElement)controller.ViewObject);
				dlgview.Owner = MainWindowWpf;
				dlgview.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
				var pos = System.Windows.Input.Mouse.GetPosition(MainWindowWpf);
				dlgview.Top = pos.Y;
				dlgview.Left = pos.X;
				var dlgctrl = new DialogShellController(dlgview, controller, title, showApplyButton);
				return true == dlgview.ShowDialog();
			}
			else
			{
				throw new NotSupportedException("This type of UIElement is not supported: " + controller.ViewObject.GetType().ToString());
				/*
				DialogShellView dlgview = new DialogShellView((System.Windows.Forms.UserControl)controller.ViewObject);
				DialogShellController dlgctrl = new DialogShellController(dlgview, controller, title, showApplyButton);
				return System.Windows.Forms.DialogResult.OK == dlgview.ShowDialog(MainWindow);
				*/
			}
		}

		/// <summary>
		/// Shows a message box with the error text.
		/// </summary>
		/// <param name="errortxt">The error text.</param>
		/// <param name="title">The titel (header) of the message box.</param>
		public override void ErrorMessageBox(string errortxt, string title)
		{
			Evaluate(System.Windows.MessageBox.Show, MainWindowWpf, errortxt, title ?? "Error(s)!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
		}

		public override void InfoMessageBox(string infotxt, string title)
		{
			Evaluate(System.Windows.MessageBox.Show, MainWindowWpf, infotxt, title ?? "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
		}

		/// <summary>
		/// Shows a message box with a question to be answered either yes or no.
		/// </summary>
		/// <param name="txt">The question text.</param>
		/// <param name="caption">The caption of the dialog box.</param>
		/// <param name="defaultanswer">If true, the default answer is "yes", otherwise "no".</param>
		/// <returns>True if the user answered with Yes, otherwise false.</returns>
		public override bool YesNoMessageBox(string txt, string caption, bool defaultanswer)
		{
			if (null != Current.Workbench)
				return System.Windows.MessageBoxResult.Yes == Evaluate(System.Windows.MessageBox.Show, MainWindowWpf, txt, caption, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, defaultanswer ? System.Windows.MessageBoxResult.OK : System.Windows.MessageBoxResult.No);
			else
				return System.Windows.MessageBoxResult.Yes == System.Windows.MessageBox.Show(txt, caption, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, defaultanswer ? System.Windows.MessageBoxResult.OK : System.Windows.MessageBoxResult.No);
		}

		/// <summary>
		/// Shows a message box with a questtion to be answered either by YES, NO, or CANCEL.
		/// </summary>
		/// <param name="text">The question text.</param>
		/// <param name="caption">The caption of the dialog box.</param>
		/// <param name="defaultAnswer">If true, the default answer is "yes", if false the default answer is "no", if null the default answer is "Cancel".</param>
		/// <returns>True if the user answered with Yes, false if the user answered No, null if the user pressed Cancel.</returns>
		public override bool? YesNoCancelMessageBox(string text, string caption, bool? defaultAnswer)
		{
			var defaultButton = System.Windows.MessageBoxResult.Cancel;
			if (defaultAnswer != null)
				defaultButton = ((bool)defaultAnswer) ? System.Windows.MessageBoxResult.Yes : System.Windows.MessageBoxResult.No;

			var result = Evaluate(System.Windows.MessageBox.Show, MainWindowWpf, text, caption, System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question, defaultButton);

			if (result == System.Windows.MessageBoxResult.Yes)
				return true;
			else if (result == System.Windows.MessageBoxResult.No)
				return false;
			else
				return null;
		}

		public override bool ShowBackgroundCancelDialog(int millisecondsDelay, IExternalDrivenBackgroundMonitor monitor, System.Threading.Thread thread)
		{
			if (InvokeRequired())
				throw new ApplicationException("Trying to show a BackgroundCancelDialog initiated by a background thread. This nesting is not supported");

			for (int i = 0; i < millisecondsDelay && thread.IsAlive; i += 10)
				System.Threading.Thread.Sleep(10);

			if (thread.IsAlive)
			{
				var dlg = new BackgroundCancelDialogWpf(thread, monitor);
				if (thread.IsAlive)
				{
					dlg.Owner = MainWindowWpf;
					return true == dlg.ShowDialog();
				}
			}
			return false;
		}

		private string GetFilterString(OpenFileOptions options)
		{
			StringBuilder stb = new StringBuilder();
			foreach (var entry in options.FilterList)
			{
				stb.Append(entry.Value);
				stb.Append('|');
				stb.Append(entry.Key);
				stb.Append('|');
			}
			if (stb.Length > 0)
				stb.Length -= 1; // account for the trailing | char

			return stb.ToString();
		}

		public override bool ShowOpenFileDialog(OpenFileOptions options)
		{
			if (InvokeRequired())
			{
				return (bool)MainWindowWpf.Dispatcher.Invoke((Func<OpenFileOptions, bool>)InternalShowOpenFileDialog, new object[] { options });
			}
			else
			{
				return InternalShowOpenFileDialog(options);
			}
		}

		private bool InternalShowOpenFileDialog(OpenFileOptions options)
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.Filter = GetFilterString(options);
			dlg.FilterIndex = options.FilterIndex;
			dlg.Multiselect = options.Multiselect;
			if (options.Title != null)
				dlg.Title = options.Title;
			if (options.InitialDirectory != null)
				dlg.InitialDirectory = options.InitialDirectory;
			dlg.RestoreDirectory = options.RestoreDirectory;

			if (true == dlg.ShowDialog(MainWindowWpf))
			{
				options.FileName = dlg.FileName;
				options.FileNames = dlg.FileNames;
				return true;
			}
			else
				return false;
		}

		public override bool ShowSaveFileDialog(SaveFileOptions options)
		{
			if (InvokeRequired())
				return (bool)MainWindowWpf.Dispatcher.Invoke((Func<SaveFileOptions, bool>)InternalShowSaveFileDialog, new object[] { options });
			else
				return InternalShowSaveFileDialog(options);
		}

		private bool InternalShowSaveFileDialog(SaveFileOptions options)
		{
			var dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.Filter = GetFilterString(options);
			dlg.FilterIndex = options.FilterIndex;
			//dlg.Multiselect = options.Multiselect;
			if (options.Title != null)
				dlg.Title = options.Title;
			if (options.InitialDirectory != null)
				dlg.InitialDirectory = options.InitialDirectory;
			dlg.RestoreDirectory = options.RestoreDirectory;
			dlg.OverwritePrompt = options.OverwritePrompt;
			dlg.AddExtension = options.AddExtension;

			if (true == dlg.ShowDialog(MainWindowWpf))
			{
				options.FileName = dlg.FileName;
				options.FileNames = dlg.FileNames;
				return true;
			}
			else
			{
				options.FileName = null;
				options.FileNames = null;
				return false;
			}
		}

		#region Clipboard

		/* old WinForm Clipboard wrappers

		private class ClipDataWrapper : System.Windows.Forms.DataObject, IClipboardSetDataObject
		{
			public void SetCommaSeparatedValues(string text) { this.SetData(System.Windows.Forms.DataFormats.CommaSeparatedValue, text); }
		}

		private class ClipGetDataWrapper : IClipboardGetDataObject
		{
			System.Windows.Forms.DataObject _dao;

			public ClipGetDataWrapper(System.Windows.Forms.DataObject value)
			{
				_dao = value;
			}

			public string[] GetFormats() { return _dao.GetFormats(); }
			public bool GetDataPresent(string format) { return _dao.GetDataPresent(format); }
			public bool GetDataPresent(System.Type type) { return _dao.GetDataPresent(type); }
			public object GetData(string format) { return _dao.GetData(format); }
			public object GetData(System.Type type) { return _dao.GetData(type); }
			public bool ContainsFileDropList() { return _dao.ContainsFileDropList(); }
			public System.Collections.Specialized.StringCollection GetFileDropList() { return _dao.GetFileDropList(); }
			public bool ContainsImage() { return _dao.ContainsImage(); }
			public System.Drawing.Image GetImage() { return _dao.GetImage(); }
		}
		*/

		private class WpfClipSetDataWrapper : IClipboardSetDataObject
		{
			private System.Windows.DataObject _dao = new System.Windows.DataObject();

			public System.Windows.IDataObject DataObject { get { return _dao; } }

			public void SetImage(System.Drawing.Image image)
			{
				_dao.SetData(image);
			}

			public void SetFileDropList(System.Collections.Specialized.StringCollection filePaths)
			{
				_dao.SetFileDropList(filePaths);
			}

			public void SetData(string format, object data)
			{
				_dao.SetData(format, data);
			}

			public void SetData(Type format, object data)
			{
				_dao.SetData(format, data);
			}

			public void SetCommaSeparatedValues(string text)
			{
				_dao.SetData("Csv", text);
			}
		}

		private class WpfClipGetDataWrapper : IClipboardGetDataObject
		{
			private System.Windows.DataObject _dao;

			public WpfClipGetDataWrapper(System.Windows.DataObject value)
			{
				_dao = value;
			}

			public string[] GetFormats()
			{
				return _dao.GetFormats();
			}

			public bool GetDataPresent(string format)
			{
				return _dao.GetDataPresent(format);
			}

			public bool GetDataPresent(System.Type type)
			{
				return _dao.GetDataPresent(type);
			}

			public object GetData(string format)
			{
				return _dao.GetData(format);
			}

			public object GetData(System.Type type)
			{
				return _dao.GetData(type);
			}

			public bool ContainsFileDropList()
			{
				return _dao.ContainsFileDropList();
			}

			public System.Collections.Specialized.StringCollection GetFileDropList()
			{
				return _dao.GetFileDropList();
			}

			public bool ContainsImage()
			{
				return _dao.ContainsImage();
			}

			public System.Drawing.Image GetImage()
			{
				try
				{
					if (_dao.GetDataPresent("EnhancedMetafile"))
						return (System.Drawing.Imaging.Metafile)_dao.GetData("EnhancedMetafile");
					else if (_dao.GetDataPresent("System.Drawing.Imaging.Metafile"))
						return (System.Drawing.Imaging.Metafile)_dao.GetData("System.Drawing.Imaging.Metafile");
					else if (_dao.GetDataPresent("System.Drawing.Bitmap"))
						return (System.Drawing.Bitmap)_dao.GetData("System.Drawing.Bitmap");
				}
				catch (Exception)
				{
				}

				return null;
			}
		}

		public override IClipboardSetDataObject GetNewClipboardDataObject()
		{
			return new WpfClipSetDataWrapper();
		}

		public override IClipboardGetDataObject OpenClipboardDataObject()
		{
			//var dao = System.Windows.Forms.Clipboard.GetDataObject() as System.Windows.Forms.DataObject;
			//return new ClipGetDataWrapper(dao);

			var dao = System.Windows.Clipboard.GetDataObject() as System.Windows.DataObject;
			return new WpfClipGetDataWrapper(dao);
		}

		public override void SetClipboardDataObject(IClipboardSetDataObject dataObject, bool copy)
		{
			//System.Windows.Forms.Clipboard.SetDataObject(dataObject, copy);
			System.Windows.Clipboard.SetDataObject(((WpfClipSetDataWrapper)dataObject).DataObject, copy);
		}

		#endregion Clipboard

		#region Context menu

		/// <summary>
		/// Creates and shows a context menu.
		/// </summary>
		/// <param name="parent">Parent class of this context menu. This determines the Gui technology to be used.</param>
		/// <param name="owner">The object that will be owner of this context menu.</param>
		/// <param name="addInTreePath">Add in tree path used to build the context menu.</param>
		/// <param name="x">The x coordinate of the location where to show the context menu.</param>
		/// <param name="y">The y coordinate of the location where to show the context menu.</param>
		/// <returns>The context menu. Returns Null if there is no registered context menu provider</returns>
		public override void ShowContextMenu(object parent, object owner, string addInTreePath, double x, double y)
		{
			foreach (var entry in RegistedContextMenuProviders)
			{
				if (ReflectionService.IsSubClassOfOrImplements(parent.GetType(), entry.Key))
				{
					entry.Value(parent, owner, addInTreePath, x, y);
					return;
				}
			}
		}

		#endregion Context menu
	}
}