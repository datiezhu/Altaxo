using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Xml;
using ICSharpCode.Core.Properties;
using ICSharpCode.Core.Services;

using ICSharpCode.SharpDevelop.Services;

using ICSharpCode.SharpDevelop.Gui.Pads;

namespace Altaxo.Main.GUI
{

	public class ProjectScout : UserControl, ICSharpCode.SharpDevelop.Gui.IPadContent
	{
		ResourceService resourceService = (ResourceService)ServiceManager.Services.GetService(typeof(ResourceService));
		public Control Control 
		{
			get 
			{
				return this;
			}
		}
		
		public string Title 
		{
			get 
			{
				return resourceService.GetString("MainWindow.Windows.ProjectScoutLabel");
			}
		}
		
		public string Icon 
		{
			get 
			{
				return "Icons.16x16.OpenFolderBitmap";
			}
		}
		
		public void RedrawContent()
		{
			OnTitleChanged(null);
			OnIconChanged(null);
		}
		
		Splitter      splitter1     = new Splitter();
		
		FileList   filelister = new FileList();
		ProjectTree  filetree   = new ProjectTree();
		
		public ProjectScout()
		{
			Dock      = DockStyle.Fill;
			
			filetree.Dock = DockStyle.Top;
			filetree.BorderStyle = BorderStyle.Fixed3D;
			filetree.Location = new System.Drawing.Point(0, 22);
			filetree.Size = new System.Drawing.Size(184, 157);
			filetree.TabIndex = 1;
			filetree.AfterSelect += new TreeViewEventHandler(DirectorySelected);
			ImageList imglist = new ImageList();
			imglist.ColorDepth = ColorDepth.Depth32Bit;
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.ClosedFolderBitmap"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.OpenFolderBitmap"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.FLOPPY"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.DRIVE"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.CDROM"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.NETWORK"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.Desktop"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.PersonalFiles"));
			imglist.Images.Add(resourceService.GetBitmap("Icons.16x16.MyComputer"));
			
			filetree.ImageList = imglist;
			
			filelister.Dock = DockStyle.Fill;
			filelister.BorderStyle = BorderStyle.Fixed3D;
			filelister.Location = new System.Drawing.Point(0, 184);
			
			filelister.Sorting = SortOrder.Ascending;
			filelister.Size = new System.Drawing.Size(184, 450);
			filelister.TabIndex = 3;
			filelister.ItemActivate += new EventHandler(FileSelected);
			
			splitter1.Dock = DockStyle.Top;
			splitter1.Location = new System.Drawing.Point(0, 179);
			splitter1.Size = new System.Drawing.Size(184, 5);
			splitter1.TabIndex = 2;
			splitter1.TabStop = false;
			splitter1.MinSize = 50;
			splitter1.MinExtra = 50;
			
			this.Controls.Add(filelister);
			this.Controls.Add(splitter1);
			this.Controls.Add(filetree);
		}
		
		void DirectorySelected(object sender, TreeViewEventArgs e)
		{
			filelister.ShowFilesInPath(filetree.NodePath + Path.DirectorySeparatorChar);
		}
		
		void FileSelected(object sender, EventArgs e)
		{
			IProjectService projectService = (IProjectService)ICSharpCode.Core.Services.ServiceManager.Services.GetService(typeof(IProjectService));
			IFileService    fileService    = (IFileService)ICSharpCode.Core.Services.ServiceManager.Services.GetService(typeof(IFileService));
			FileUtilityService fileUtilityService = (FileUtilityService)ServiceManager.Services.GetService(typeof(FileUtilityService));
			
			foreach (FileList.FileListItem item in filelister.SelectedItems) 
			{
				
				switch (Path.GetExtension(item.FullName)) 
				{
					case ".cmbx":
					case ".prjx":
						projectService.OpenCombine(item.FullName);
						break;
					default:
						fileService.OpenFile(item.FullName);
						break;
				}
			}
		}
		protected virtual void OnTitleChanged(EventArgs e)
		{
			if (TitleChanged != null) 
			{
				TitleChanged(this, e);
			}
		}
		protected virtual void OnIconChanged(EventArgs e)
		{
			if (IconChanged != null) 
			{
				IconChanged(this, e);
			}
		}
		public event EventHandler TitleChanged;
		public event EventHandler IconChanged;
	}
	



	public class ProjectTree : TreeView
	{
		protected TreeNode tablesNode;

		public string NodePath 
		{
			get 
			{
				return (string)SelectedNode.Tag;
			}
			set 
			{
				PopulateShellTree(value);
			}
		}
		
		public ProjectTree()
		{
			Sorted = true;
			TreeNode rootNode = Nodes.Add("Project");
			rootNode.ImageIndex = 6;
			rootNode.SelectedImageIndex = 6;
			rootNode.Tag = "Project";

			TreeNode viewNodes = Nodes.Add("Views");
			viewNodes.ImageIndex = 7;
			viewNodes.SelectedImageIndex = 7;
			viewNodes.Tag = "Views";


			tablesNode = rootNode.Nodes.Add("Tables");
			tablesNode.ImageIndex = 7;
			tablesNode.SelectedImageIndex = 7;
			tablesNode.Tag = "Tables";
			
			TreeNode graphsNode = rootNode.Nodes.Add("Graphs");
			graphsNode.ImageIndex = 8;
			graphsNode.SelectedImageIndex = 8;
			graphsNode.Tag = "Graphs";
			

			foreach(Altaxo.Data.DataTable table in Current.Project.DataTableCollection)
			{
				TreeNode node = tablesNode.Nodes.Add(table.Name);
				node.Tag = table.Name;
			}

			foreach(Altaxo.Graph.GraphDocument graph in Current.Project.GraphDocumentCollection)
			{
				TreeNode node = tablesNode.Nodes.Add(graph.Name);
				node.Tag = graph.Name;
			}
			
			foreach(ICSharpCode.SharpDevelop.Gui.IViewContent content in Current.Workbench.ViewContentCollection)
			{
				TreeNode node = viewNodes.Nodes.Add(content.ContentName);
				node.Tag = content.ContentName;
			}

			rootNode.Expand();
			viewNodes.Expand();

			Current.Project.DataTableCollection.CollectionChanged += new EventHandler(DataTableCollection_Changed);
			
			InitializeComponent();
		}
		
		int getNodeLevel(TreeNode node)
		{
			TreeNode parent = node;
			int depth = 0;
			
			while(true)
			{
				parent = parent.Parent;
				if(parent == null) 
				{
					return depth;
				}
				depth++;
			}
		}
		
		void InitializeComponent ()
		{
			BeforeSelect   += new TreeViewCancelEventHandler(SetClosedIcon);
			AfterSelect    += new TreeViewEventHandler(SetOpenedIcon);
		}
		
		void SetClosedIcon(object sender, TreeViewCancelEventArgs e) // Set icon as closed
		{
			if (SelectedNode != null) 
			{
				if(getNodeLevel(SelectedNode) > 2) 
				{
					SelectedNode.ImageIndex = SelectedNode.SelectedImageIndex = 0;
				}
			}
		}
		
		void SetOpenedIcon(object sender, TreeViewEventArgs e) // Set icon as opened
		{
			if(getNodeLevel(e.Node) > 2) 
			{
				if (e.Node.Parent != null && e.Node.Parent.Parent != null) 
				{
					e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
				}
			}
		}
		
		void PopulateShellTree(string path)
		{
			string[]  pathlist = path.Split(new char[] { Path.DirectorySeparatorChar });
			TreeNodeCollection  curnode = Nodes;
			
			foreach(string dir in pathlist) 
			{
				
				foreach(TreeNode childnode in curnode) 
				{
					if (((string)childnode.Tag).ToUpper().Equals(dir.ToUpper())) 
					{
						SelectedNode = childnode;
						
						PopulateSubDirectory(childnode, 2);
						childnode.Expand();
						
						curnode = childnode.Nodes;
						break;
					}
				}
			}
		}
		
		void PopulateSubDirectory(TreeNode curNode, int depth)
		{
			if (--depth < 0) 
			{
				return;
			}
			
			if (curNode.Nodes.Count == 1 && curNode.Nodes[0].Text.Equals("")) 
			{
				
				string[] directories = null;
				try 
				{
					directories  = Directory.GetDirectories(curNode.Tag.ToString() + Path.DirectorySeparatorChar);
				} 
				catch (Exception) 
				{
					return;
				}
				
				curNode.Nodes.Clear();
				
				foreach (string fulldir in directories) 
				{
					try 
					{
						string dir = System.IO.Path.GetFileName(fulldir);
						
						FileAttributes attr = File.GetAttributes(fulldir);
						if ((attr & FileAttributes.Hidden) == 0) 
						{
							TreeNode node   = curNode.Nodes.Add(dir);
							node.Tag = curNode.Tag.ToString() + Path.DirectorySeparatorChar + dir;
							node.ImageIndex = node.SelectedImageIndex = 0;
							
							node.Nodes.Add(""); // Add dummy child node to make node expandable
							
							PopulateSubDirectory(node, depth);
						}
					} 
					catch (Exception) 
					{
					}
				}
			} 
			else 
			{
				foreach (TreeNode node in curNode.Nodes) 
				{
					PopulateSubDirectory(node, depth); // Populate sub directory
				}
			}
		}
		


		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			
			try 
			{
				// do not populate if the "My Cpmputer" node is expaned
				if(e.Node.Parent != null && e.Node.Parent.Parent != null) 
				{
					PopulateSubDirectory(e.Node, 2);
					Cursor.Current = Cursors.Default;
				} 
				else 
				{
					PopulateSubDirectory(e.Node, 1);
					Cursor.Current = Cursors.Default;
				}
			} 
			catch (Exception excpt) 
			{
				IMessageService messageService =(IMessageService)ServiceManager.Services.GetService(typeof(IMessageService));
				messageService.ShowError(excpt, "Device error");
				e.Cancel = true;
			}
			
			Cursor.Current = Cursors.Default;
		}

		private void DataTableCollection_Changed(object sender, EventArgs e)
		{
			this.tablesNode.Nodes.Clear();
			foreach(Altaxo.Data.DataTable table in Current.Project.DataTableCollection)
			{
				TreeNode node = tablesNode.Nodes.Add(table.Name);
				node.Tag = table.Name;
			}

		}

		TreeNode nodeClickedNow;
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			if(null!=this.SelectedNode && this.SelectedNode.Bounds.Contains(e.X,e.Y))
			{
				nodeClickedNow = this.SelectedNode;
			}
			else
			{
				nodeClickedNow = null;
			}
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);

			if(nodeClickedNow != null)
			{
				string tag = (string)nodeClickedNow.Tag;
				if(	nodeClickedNow.Parent!=null
						&& ((string)nodeClickedNow.Parent.Tag)=="Tables"
					&& Current.Project.DataTableCollection.ContainsTable(tag))
				{
					
					Current.ProjectService.CreateNewWorksheet(Current.Project.DataTableCollection[tag]);
				}
			}
		}


	}

}
