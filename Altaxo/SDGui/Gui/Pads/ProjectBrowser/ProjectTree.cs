#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2005 Dr. Dirk Lellinger
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
#endregion

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
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;

using Altaxo.Main;
namespace Altaxo.Gui.Pads
{
  public class ProjectTree : MWControlSuite.MWTreeView
  {
    protected TreeNode tablesNode;
    protected TreeNode graphsNode;


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

      graphsNode = rootNode.Nodes.Add("Graphs");
      graphsNode.ImageIndex = 8;
      graphsNode.SelectedImageIndex = 8;
      graphsNode.Tag = "Graphs";


      foreach (Altaxo.Data.DataTable table in Current.Project.DataTableCollection)
      {
        TreeNode node = tablesNode.Nodes.Add(table.Name);
        node.Tag = table.Name;
      }

      foreach (Altaxo.Graph.GraphDocument graph in Current.Project.GraphDocumentCollection)
      {
        TreeNode node = tablesNode.Nodes.Add(graph.Name);
        node.Tag = graph.Name;
      }

      foreach (ICSharpCode.SharpDevelop.Gui.IViewContent content in Current.Workbench.ViewContentCollection)
      {
        TreeNode node = viewNodes.Nodes.Add(content.TitleName);
        node.Tag = content.TitleName;
      }

      rootNode.Expand();
      viewNodes.Expand();

      Current.ProjectService.ProjectOpened += new ProjectEventHandler(this.EhProjectOpened);
      Current.ProjectService.ProjectClosed += new ProjectEventHandler(this.EhProjectClosed);

      Current.Project.DataTableCollection.CollectionChanged += new EventHandler(DataTableCollection_Changed);
      Current.Project.GraphDocumentCollection.CollectionChanged += new EventHandler(GraphDocumentCollection_Changed);

      InitializeComponent();
    }

    int getNodeLevel(TreeNode node)
    {
      TreeNode parent = node;
      int depth = 0;

      while (true)
      {
        parent = parent.Parent;
        if (parent == null)
        {
          return depth;
        }
        depth++;
      }
    }

    void InitializeComponent()
    {
      BeforeSelect += new TreeViewCancelEventHandler(SetClosedIcon);
      AfterSelect += new TreeViewEventHandler(SetOpenedIcon);
    }

    void SetClosedIcon(object sender, TreeViewCancelEventArgs e) // Set icon as closed
    {
      if (SelectedNode != null)
      {
        if (getNodeLevel(SelectedNode) > 2)
        {
          SelectedNode.ImageIndex = SelectedNode.SelectedImageIndex = 0;
        }
      }
    }

    void SetOpenedIcon(object sender, TreeViewEventArgs e) // Set icon as opened
    {
      if (getNodeLevel(e.Node) > 2)
      {
        if (e.Node.Parent != null && e.Node.Parent.Parent != null)
        {
          e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
        }
      }
    }

    void PopulateShellTree(string path)
    {
      string[] pathlist = path.Split(new char[] { Path.DirectorySeparatorChar });
      TreeNodeCollection curnode = Nodes;

      foreach (string dir in pathlist)
      {

        foreach (TreeNode childnode in curnode)
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
          directories = Directory.GetDirectories(curNode.Tag.ToString() + Path.DirectorySeparatorChar);
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
              TreeNode node = curNode.Nodes.Add(dir);
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
        if (e.Node.Parent != null && e.Node.Parent.Parent != null)
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
        MessageService.ShowError(excpt, "Device error");
        e.Cancel = true;
      }

      Cursor.Current = Cursors.Default;
    }

    private void DataTableCollection_Changed(object sender, EventArgs e)
    {
      this.tablesNode.Nodes.Clear();
      foreach (Altaxo.Data.DataTable table in Current.Project.DataTableCollection)
      {
        TreeNode node = tablesNode.Nodes.Add(table.Name);
        node.Tag = table.Name;
      }

    }

    private void GraphDocumentCollection_Changed(object sender, EventArgs e)
    {
      this.graphsNode.Nodes.Clear();
      foreach (Altaxo.Graph.GraphDocument item in Current.Project.GraphDocumentCollection)
      {
        TreeNode node = graphsNode.Nodes.Add(item.Name);
        node.Tag = item.Name;
      }

    }

    private void EhProjectOpened(object sender, ProjectEventArgs e)
    {
      e.Project.DataTableCollection.CollectionChanged += new EventHandler(DataTableCollection_Changed);
      e.Project.GraphDocumentCollection.CollectionChanged += new EventHandler(GraphDocumentCollection_Changed);

      DataTableCollection_Changed(sender, e);
      GraphDocumentCollection_Changed(sender, e);
    }

    private void EhProjectClosed(object sender, ProjectEventArgs e)
    {
      e.Project.DataTableCollection.CollectionChanged -= new EventHandler(DataTableCollection_Changed);
      e.Project.GraphDocumentCollection.CollectionChanged -= new EventHandler(GraphDocumentCollection_Changed);
    }

    
    protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
    {
     
      base.OnNodeMouseDoubleClick(e);
      string tag = (string)e.Node.Tag;
      // table nodes
      if (e.Node.Parent != null
        && ((string)e.Node.Parent.Tag) == "Tables"
        && Current.Project.DataTableCollection.ContainsTable(tag))
      {
        // tag is the name of the table clicked, so look for a view that has the table or
        // create a new one
        Current.ProjectService.OpenOrCreateWorksheetForTable(Current.Project.DataTableCollection[tag]);
      }

      // graph nodes
      if (e.Node.Parent != null
        && ((string)e.Node.Parent.Tag) == "Graphs"
        && Current.Project.GraphDocumentCollection.Contains(tag))
      {
        // tag is the name of the table clicked, so look for a view that has the table or
        // create a new one
        Current.ProjectService.OpenOrCreateGraphForGraphDocument(Current.Project.GraphDocumentCollection[tag]);
      }
    }

    protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
    {
      // the context menu for the items should be set here
      //e.Node.ContextMenuStrip = 
      base.OnBeforeSelect(e);
    }
  }
}