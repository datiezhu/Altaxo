// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using ICSharpCode.Core.AddIns;
using ICSharpCode.Core.Services;
using ICSharpCode.SharpDevelop.Services;
using WeifenLuo.WinFormsUI;


namespace ICSharpCode.SharpDevelop.Gui
{
	public class SdiWorkspaceWindow : DockContent, IWorkbenchWindow, IOwnerState
	{
		readonly static string contextMenuPath = "/SharpDevelop/Workbench/OpenFileTab/ContextMenu";
		
		#region IOwnerState
		[Flags]
		public enum OpenFileTabState {
			Nothing             = 0,
			FileDirty           = 1,
			ClickedWindowIsForm = 2,
			FileUntitled        = 4
		}
		
		OpenFileTabState internalState = OpenFileTabState.Nothing;

		public System.Enum InternalState {
			get {
				return internalState;
			}
		}
		#endregion

		TabControl   viewTabControl = null;
		IViewContent content;
		ArrayList    subViewContents = null;
		
		string myUntitledTitle     = null;
		static StringParserService stringParserService = (StringParserService)ServiceManager.Services.GetService(typeof(StringParserService));
		
		public string Title {
			get {
				return Text;
			}
			set {
				Text = value;
				OnTitleChanged(null);
			}
		}
		
		public ArrayList SubViewContents {
			get {
				return subViewContents;
			}
		}
		
		public IBaseViewContent ActiveViewContent {
			get {
				if (viewTabControl != null && viewTabControl.SelectedIndex > 0) {
					return (IBaseViewContent)subViewContents[viewTabControl.SelectedIndex];
				}
				return content;
			}
		}
		
		public void SwitchView(int viewNumber)
		{
			if (viewTabControl != null) {
				this.viewTabControl.SelectedIndex = viewNumber;
			}
		}
		
		public void SelectWindow()	
		{
			Show();
		}
		
		public SdiWorkspaceWindow(IViewContent content)
		{
			this.content = content;
			
			content.WorkbenchWindow = this;
			content.TitleNameChanged += new EventHandler(SetTitleEvent);
			content.DirtyChanged    += new EventHandler(SetTitleEvent);
			content.BeforeSave      += new EventHandler(BeforeSave);
			SetTitleEvent(null, null);
			
			this.DockableAreas = WeifenLuo.WinFormsUI.DockAreas.Document;
			
			this.DockPadding.All = 2;
			content.Control.Dock = DockStyle.Fill;
			Controls.Add(content.Control);
			SetTitleEvent(this, EventArgs.Empty);
			MenuService menuService = (MenuService)ICSharpCode.Core.Services.ServiceManager.Services.GetService(typeof(MenuService));
			this.TabPageContextMenu  = menuService.CreateContextMenu(this, contextMenuPath);
		}
		
		void BeforeSave(object sender, EventArgs e)
		{
			ISecondaryViewContent secondaryViewContent = ActiveViewContent as ISecondaryViewContent;
			Console.WriteLine("Before SAVE  -- " + secondaryViewContent);
			if (secondaryViewContent != null) {
				secondaryViewContent.NotifyBeforeSave();
			}
		}
		
		void LeaveTabPage(object sender, EventArgs e)
		{
			OnWindowDeselected(EventArgs.Empty);
		}
		
		public IViewContent ViewContent {
			get {
				return content;
			}
			set {
				content = value;
			}
		}
		
		public void SetTitleEvent(object sender, EventArgs e)
		{
			internalState = OpenFileTabState.Nothing;
			
			if (content == null) {
				return;
			}
			
			string newTitle = "";
			if (content.TitleName == null) {
				myUntitledTitle = Path.GetFileNameWithoutExtension(content.UntitledName);
//				if (myUntitledTitle == null) {
//					string baseName  
//					int    number    = 1;
//					bool   found     = true;
//					while (found) {
//						found = false;
//						foreach (IViewContent windowContent in WorkbenchSingleton.Workbench.ViewContentCollection) {
//							string title = windowContent.WorkbenchWindow.Title;
//							if (title.EndsWith("*") || title.EndsWith("+")) {
//								title = title.Substring(0, title.Length - 1);
//							}
//							if (title == baseName + number) {
//								found = true;
//								++number;
//								break;
//							}
//						}
//					}
//					myUntitledTitle = baseName + number;
//				}
				newTitle = myUntitledTitle;
				internalState |= OpenFileTabState.FileUntitled;
			} else {
				newTitle = content.TitleName;
			}
			
			if (content.IsDirty) {
				internalState |= OpenFileTabState.FileDirty;
				newTitle += "*";
			} else if (content.IsReadOnly) {
				newTitle += "+";
			}
			
			if (newTitle != Title) {
				Text = newTitle;
			}
		}
		
		public void DetachContent()
		{
			content.TitleNameChanged -= new EventHandler(SetTitleEvent);
			content.DirtyChanged     -= new EventHandler(SetTitleEvent);
			content.BeforeSave       -= new EventHandler(BeforeSave);
		}
		
		public bool CloseWindow(bool force)
		{
			if (!force && ViewContent != null && ViewContent.IsDirty) {
				ResourceService resourceService = (ResourceService)ServiceManager.Services.GetService(typeof(IResourceService));
				DialogResult dr = MessageBox.Show(
				    resourceService.GetString("MainWindow.SaveChangesMessage"),
					resourceService.GetString("MainWindow.SaveChangesMessageHeader") + " " + Title + " ?",
					MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				switch (dr) {
					case DialogResult.Yes:
						if (content.FileName == null) {
							while (true) {
								new ICSharpCode.SharpDevelop.Commands.SaveFileAs().Run();
								if (ViewContent.IsDirty) {
									IMessageService messageService =(IMessageService)ServiceManager.Services.GetService(typeof(IMessageService));
									if (messageService.AskQuestion("Do you really want to discard your changes ?")) {
										break;
									}
								} else {
									break;
								}
							}
							
						} else {
							FileUtilityService fileUtilityService = (FileUtilityService)ServiceManager.Services.GetService(typeof(FileUtilityService));
							fileUtilityService.ObservedSave(new FileOperationDelegate(ViewContent.Save), ViewContent.FileName , FileErrorPolicy.ProvideAlternative);
						}
						break;
					case DialogResult.No:
						break;
					case DialogResult.Cancel:
						return false;
				}
			}
			OnCloseEvent(null);
			Dispose();
			return true;
		}
		
		public void AttachSecondaryViewContent(ISecondaryViewContent subViewContent)
		{
			TabPage newPage;
			
			if (subViewContents == null) {
				subViewContents = new ArrayList();
				subViewContents.Add(content);
				viewTabControl      = new TabControl();
				viewTabControl.Alignment = TabAlignment.Bottom;
				viewTabControl.Dock = DockStyle.Fill;
				viewTabControl.SelectedIndexChanged += new EventHandler(viewTabControlIndexChanged);
				
				Controls.Clear();
				Controls.Add(viewTabControl);
				
				newPage = new TabPage(stringParserService.Parse(content.TabPageText));
				newPage.Tag = content;
				content.Control.Dock = DockStyle.Fill;
				newPage.Controls.Add(content.Control);
				viewTabControl.TabPages.Add(newPage);
			}
			subViewContent.WorkbenchWindow = this;
			subViewContents.Add(subViewContent);
			
			newPage = new TabPage(stringParserService.Parse(subViewContent.TabPageText));
			newPage.Tag = subViewContent;
			try {
				subViewContent.Control.Dock = DockStyle.Fill;
			} catch (Exception) {}
			newPage.Controls.Add(subViewContent.Control);
			viewTabControl.TabPages.Add(newPage);
		}
		
		int oldIndex = -1;
		void viewTabControlIndexChanged(object sender, EventArgs e)
		{
			if (oldIndex >= 0) {
				IBaseViewContent secondaryViewContent = subViewContents[oldIndex] as IBaseViewContent;
				if (secondaryViewContent != null) {
					secondaryViewContent.Deselected();
				}
			}
			
			if (viewTabControl.SelectedIndex >= 0) {
				IBaseViewContent secondaryViewContent = subViewContents[viewTabControl.SelectedIndex] as IBaseViewContent;
				if (secondaryViewContent != null) {
					secondaryViewContent.Selected();
				}
			}
			oldIndex = viewTabControl.SelectedIndex;
			WorkbenchSingleton.Workbench.WorkbenchLayout.OnActiveWorkbenchWindowChanged(EventArgs.Empty);
		}
		
		public virtual void RedrawContent()
		{
			if (viewTabControl != null) {
				for (int i = 0; i < viewTabControl.TabPages.Count; ++i) {
					TabPage tabPage = viewTabControl.TabPages[i];
					if (i == 0) {
						tabPage.Text = stringParserService.Parse(content.TabPageText);
					} else {
						tabPage.Text = stringParserService.Parse(((IBaseViewContent)subViewContents[i]).TabPageText);
					}
				}
			}
		}
		
		protected virtual void OnTitleChanged(EventArgs e)
		{
			if (TitleChanged != null) {
				TitleChanged(this, e);
			}
			WorkbenchSingleton.Workbench.WorkbenchLayout.OnActiveWorkbenchWindowChanged(EventArgs.Empty);
		}
		
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = !CloseWindow(false);
		}
		
		protected virtual void OnCloseEvent(EventArgs e)
		{
			OnWindowDeselected(e);
			if (CloseEvent != null) {
				CloseEvent(this, e);
			}
		}
		
		public virtual void OnWindowSelected(EventArgs e)
		{
			if (WindowSelected != null) {
				WindowSelected(this, e);
			}
		}
		public virtual void OnWindowDeselected(EventArgs e)
		{
			if (WindowDeselected != null) {
				WindowDeselected(this, e);
			}
		}
		
		public event EventHandler WindowSelected;
		public event EventHandler WindowDeselected;
				
		public event EventHandler TitleChanged;
		public event EventHandler CloseEvent;
	}
}
