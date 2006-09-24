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
using System.Windows.Forms;
using ICSharpCode.Core;
using Altaxo;
using Altaxo.Main;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpDevelop.Gui;

namespace Altaxo.Main.Commands
{
  public class CreateNewWorksheet : AbstractMenuCommand
  {
    public override void Run()
    {
      Current.ProjectService.CreateNewWorksheet();
    }
  }

  public class CreateNewStandardWorksheet : AbstractMenuCommand
  {
    public override void Run()
    {
      Altaxo.Worksheet.GUI.IWorksheetController controller = Current.ProjectService.CreateNewWorksheet();
      controller.Doc.DataColumns.Add(new Altaxo.Data.DoubleColumn(),"A",Altaxo.Data.ColumnKind.X);
      controller.Doc.DataColumns.Add(new Altaxo.Data.DoubleColumn(),"B",Altaxo.Data.ColumnKind.V);
    }
  }
  
  public class CreateNewGraph : AbstractMenuCommand
  {
    public override void Run()
    {
      Current.ProjectService.CreateNewGraph();
    }
  }

  public class CreateNewWorksheetOrGraphFromFile : AbstractMenuCommand
  {
    public override void Run()
    {
      System.IO.Stream myStream ;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
 
      openFileDialog1.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*"  ;
      openFileDialog1.FilterIndex = 1 ;
      openFileDialog1.RestoreDirectory = true ;
 
      if(openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        if((myStream = openFileDialog1.OpenFile()) != null)
        {
          Altaxo.Serialization.Xml.XmlStreamDeserializationInfo info = new Altaxo.Serialization.Xml.XmlStreamDeserializationInfo();
          info.BeginReading(myStream);
          object deserObject = info.GetValue("Table",null);
          info.EndReading();
          myStream.Close();

          // if it is a table, add it to the DataTableCollection
          if(deserObject is Altaxo.Data.DataTable)
          {
            Altaxo.Data.DataTable table = deserObject as Altaxo.Data.DataTable;
            if(table.Name==null || table.Name==string.Empty)
              table.Name = Current.Project.DataTableCollection.FindNewTableName();
            else if( Current.Project.DataTableCollection.ContainsTable(table.Name))
              table.Name = Current.Project.DataTableCollection.FindNewTableName(table.Name);

            Current.Project.DataTableCollection.Add(table);
            info.AnnounceDeserializationEnd(Current.Project); // fire the event to resolve path references
            
            Current.ProjectService.CreateNewWorksheet(table);
          }
            // if it is a table, add it to the DataTableCollection
          else if(deserObject is Altaxo.Worksheet.TablePlusLayout)
          {
            Altaxo.Worksheet.TablePlusLayout tableAndLayout = deserObject as Altaxo.Worksheet.TablePlusLayout;
            Altaxo.Data.DataTable table = tableAndLayout.Table;
            if(table.Name==null || table.Name==string.Empty)
              table.Name = Current.Project.DataTableCollection.FindNewTableName();
            else if( Current.Project.DataTableCollection.ContainsTable(table.Name))
              table.Name = Current.Project.DataTableCollection.FindNewTableName(table.Name);
            Current.Project.DataTableCollection.Add(table);

            if(tableAndLayout.Layout!=null)
              Current.Project.TableLayouts.Add(tableAndLayout.Layout);

            info.AnnounceDeserializationEnd(Current.Project); // fire the event to resolve path references

            tableAndLayout.Layout.DataTable = table; // this is the table for the layout now
            
            Current.ProjectService.CreateNewWorksheet(table,tableAndLayout.Layout);
          }
          else if (deserObject is Altaxo.Graph.Gdi.GraphDocument)
          {
            Altaxo.Graph.Gdi.GraphDocument graph = deserObject as Altaxo.Graph.Gdi.GraphDocument;
            if(graph.Name==null || graph.Name==string.Empty)
              graph.Name = Current.Project.GraphDocumentCollection.FindNewName();
            else if( Current.Project.GraphDocumentCollection.Contains(graph.Name))
              graph.Name = Current.Project.GraphDocumentCollection.FindNewName(graph.Name);

            Current.Project.GraphDocumentCollection.Add(graph);
            info.AnnounceDeserializationEnd(Current.Project); // fire the event to resolve path references in the graph

            Current.ProjectService.CreateNewGraph(graph);
          }
        }
      }
    }
  }


  public class FileOpen : AbstractMenuCommand
  {
    public override void Run()
    {
      

      if(Current.Project.IsDirty)
      {
        System.ComponentModel.CancelEventArgs cancelargs = new System.ComponentModel.CancelEventArgs();
        Current.ProjectService.AskForSavingOfProject(cancelargs);
        if(cancelargs.Cancel)
          return;
      }

      bool saveDirtyState = Current.Project.IsDirty; // save the dirty state of the project in case the user cancels the open file dialog
      Current.Project.IsDirty = false; // set document to non-dirty


      OpenFileDialog openFileDialog1 = new OpenFileDialog();

      openFileDialog1.Filter = "Altaxo project files (*.axoprj)|*.axoprj|All files (*.*)|*.*" ;
      openFileDialog1.FilterIndex = 1 ;
      openFileDialog1.RestoreDirectory = true ;

      

      if(openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        Current.ProjectService.OpenProject(openFileDialog1.FileName);
      }
      else // in case the user cancels the open file dialog
      {
        Current.Project.IsDirty = saveDirtyState; // restore the dirty state of the current project
      }
    }
  }


  public class FileSaveAs : AbstractMenuCommand
  {
    public override void Run()
    {
      Current.ProjectService.SaveProjectAs();
    }

    public  void RunOld1()
    {
      SaveFileDialog dlg = this.GetSaveAsDialog();
      if(dlg.ShowDialog(Current.MainWindow) == DialogResult.OK)
      {
        System.IO.Stream myStream;
        if((myStream = dlg.OpenFile()) != null)
        {
          try
          {
            Altaxo.Serialization.Xml.XmlStreamSerializationInfo info = new Altaxo.Serialization.Xml.XmlStreamSerializationInfo();
            ZipOutputStream zippedStream = new ZipOutputStream(myStream);
            Current.Project.SaveToZippedFile(new ZipOutputStreamWrapper(zippedStream), info);
            Current.ProjectService.SaveWindowStateToZippedFile(new ZipOutputStreamWrapper(zippedStream), info);
            zippedStream.Close();
            Current.Project.IsDirty=false;

          }
          catch(Exception exc)
          {
            System.Windows.Forms.MessageBox.Show(Current.MainWindow,"An error occured saving the document, details see below:\n" + exc.ToString(),"Error",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
          }
          finally
          {
            myStream.Close();
          }
        }
      }
    }

    protected SaveFileDialog GetSaveAsDialog()
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
 
      saveFileDialog1.Filter = "Altaxo project files (*.axoprj)|*.axoprj|All files (*.*)|*.*"  ;
      saveFileDialog1.FilterIndex = 1 ;
      saveFileDialog1.RestoreDirectory = true ;
  
      return saveFileDialog1;
    }
  }

  public class FileSave : AbstractMenuCommand
  {
    public override void Run()
    {
      if(Current.ProjectService.CurrentProjectFileName != null)
        Current.ProjectService.SaveProject();
      else
        Current.ProjectService.SaveProjectAs();
    }
  }


  public class CloseProject : AbstractMenuCommand
  {
    public override void Run()
    {
      Current.ProjectService.CloseProject();
    }
  }

  /// <summary>
  /// Taken from Commands.MenuItemBuilders. See last line for change.
  /// </summary>
  public class RecentProjectsMenuBuilder : ISubmenuBuilder
  {
    public ToolStripItem[] BuildSubmenu(Codon codon, object owner)
    {
      RecentOpen recentOpen = FileService.RecentOpen;

      if (recentOpen.RecentProject.Count > 0)
      {
        MenuCommand[] items = new MenuCommand[recentOpen.RecentProject.Count];
        for (int i = 0; i < recentOpen.RecentProject.Count; ++i)
        {
          string accelaratorKeyPrefix = i < 10 ? "&" + ((i + 1) % 10) + " " : "";
          items[i] = new MenuCommand(accelaratorKeyPrefix + recentOpen.RecentProject[i], new EventHandler(LoadRecentProject));
          items[i].Tag = recentOpen.RecentProject[i].ToString();
          items[i].Description = StringParser.Parse(ResourceService.GetString("Dialog.Componnents.RichMenuItem.LoadProjectDescription"),
                                                    new string[,] { { "PROJECT", recentOpen.RecentProject[i].ToString() } });
        }
        return items;
      }

      MenuCommand defaultMenu = new MenuCommand("${res:Dialog.Componnents.RichMenuItem.NoRecentProjectsString}");
      defaultMenu.Enabled = false;

      return new MenuCommand[] { defaultMenu };
    }


    void LoadRecentProject(object sender, EventArgs e)
    {
      MenuCommand item = (MenuCommand)sender;

      string fileName = item.Tag.ToString();

      // The following line was changed to load an altaxo solution.
      FileUtility.ObservedLoad(new NamedFileOperationDelegate(Current.ProjectService.OpenProject), fileName);
    }
  }

  public class FileExit : AbstractMenuCommand
  {
    public override void Run()
    {
      ((Form)WorkbenchSingleton.Workbench).Close(); 
    }
  }

  public class Duplicate : AbstractMenuCommand
  {
    public override void Run()
    {
      if(Current.Workbench.ActiveViewContent is Altaxo.Worksheet.GUI.IWorksheetController)
      {
        new Altaxo.Worksheet.Commands.WorksheetDuplicate().Run();
      }
      else if (Current.Workbench.ActiveViewContent is Altaxo.Graph.GUI.IGraphController)
      {
        new Altaxo.Graph.Commands.DuplicateGraph().Run();
      }
    }
  }

  public class HelpAboutAltaxo : AbstractMenuCommand
  {
    public override void Run()
    {
      Altaxo.Main.AboutDialog dlg = new Altaxo.Main.AboutDialog();
      dlg.ShowDialog(((Form)WorkbenchSingleton.Workbench));
    }
  }

  public class FileImportOriginOpj : AbstractMenuCommand
  {
    public override void Run()
    {
      using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
      {
        openFileDialog1.Filter = "OPJ files (*.opj)|*.opj|All files (*.*)|*.*";
        openFileDialog1.FilterIndex = 1;
        openFileDialog1.RestoreDirectory = true;
        openFileDialog1.Multiselect = false;

        if (openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog1.FileName.Length > 0)
        {
          string result = Altaxo.Serialization.Origin.Importer.Import(openFileDialog1.FileName);
          if (result != null)
            Current.Gui.ErrorMessageBox(result);
        }
      }
    }
  }
}
