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
//    along with ctrl program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

using Altaxo.Data;

namespace Altaxo.Worksheet.Commands
{
  /// <summary>
  /// PlotCommands contains methods for creating different plot types.
  /// </summary>
  public class PlotCommands
  {
    /// <summary>
    /// Plots the currently selected data columns of a worksheet.
    /// </summary>
    /// <param name="dg">The worksheet controller where the columns are selected in.</param>
    /// <param name="bLine">If true, a line is plotted.</param>
    /// <param name="bScatter">If true, scatter symbols are plotted.</param>
    public static void PlotLine(GUI.WorksheetController dg, bool bLine, bool bScatter)
    {
      PlotLine(dg.DataTable,dg.SelectedDataColumns,bLine,bScatter);
    }

    /// <summary>
    /// Plots selected data columns of a table.
    /// </summary>
    /// <param name="table">The source table.</param>
    /// <param name="selectedColumns">The data columns of the table that should be plotted.</param>
    /// <param name="bLine">If true, the line style is activated (the points are connected by lines).</param>
    /// <param name="bScatter">If true, the scatter style is activated (the points are plotted as symbols).</param>
    public static void PlotLine(DataTable table, Altaxo.Collections.IAscendingIntegerCollection selectedColumns, bool bLine, bool bScatter)
    {
      Altaxo.Graph.XYPlotStyleCollection templatePlotStyle;
      if(bLine && bScatter)
        templatePlotStyle  = new Altaxo.Graph.XYPlotStyleCollection(Graph.LineScatterPlotStyleKind.LineAndScatter);
      else if (bLine)
        templatePlotStyle = new Altaxo.Graph.XYPlotStyleCollection(Graph.LineScatterPlotStyleKind.Line);
      else
        templatePlotStyle = new Altaxo.Graph.XYPlotStyleCollection(Graph.LineScatterPlotStyleKind.Scatter);

      // first, create a plot association for every selected column in
      // the data grid

      int len = selectedColumns.Count;

      Graph.XYColumnPlotData[] pa = new Graph.XYColumnPlotData[len];
      Graph.XYPlotStyleCollection[] ps = new Altaxo.Graph.XYPlotStyleCollection[len];
      for(int i=0;i<len;++i)
        ps[i] = (Altaxo.Graph.XYPlotStyleCollection)templatePlotStyle.Clone();


      int nNumberOfPlotData=0;
      for(int i=0;i<len;i++)
      {
        Altaxo.Data.DataColumn ycol = table[selectedColumns[i]];

        Altaxo.Data.DataColumn xcol = table.DataColumns.FindXColumnOf(ycol);
      
        if(null!=xcol)
          pa[i] = new Graph.XYColumnPlotData(xcol,ycol);
        else
          pa[i] = new Graph.XYColumnPlotData( new Altaxo.Data.IndexerColumn(), ycol);

        nNumberOfPlotData++;

        // if the next column is a label column, add it also
        if((i+1)<len && ColumnKind.Label==table.DataColumns.GetColumnKind(selectedColumns[i+1]))
        {
          Altaxo.Graph.XYPlotLabelStyle labelStyle = new Altaxo.Graph.XYPlotLabelStyle(table.DataColumns[i]);
          ps[i].Add(labelStyle);
          i++;
        }


      }
      
      // now create a new Graph with this plot associations

      Altaxo.Graph.GUI.IGraphController gc = Current.ProjectService.CreateNewGraph();

      Altaxo.Graph.PlotItemCollection newPlotGroup = new Altaxo.Graph.PlotItemCollection(gc.Doc.Layers[0].PlotItems);

      for(int i=0;i<nNumberOfPlotData;i++)
      {
        Altaxo.Graph.IGPlotItem pi = new Altaxo.Graph.XYColumnPlotItem(pa[i],ps[i]);
        newPlotGroup.Add(pi);
      }

      newPlotGroup.CollectStyles(newPlotGroup.GroupStyles);

      gc.Doc.Layers[0].PlotItems.Add(newPlotGroup);
    }





    /// <summary>
    /// Plots a density image of the selected columns.
    /// </summary>
    /// <param name="dg"></param>
    /// <param name="bLine"></param>
    /// <param name="bScatter"></param>
    public static void PlotDensityImage(GUI.WorksheetController dg, bool bLine, bool bScatter)
    {
      Altaxo.Graph.DensityImagePlotStyle plotStyle = new Altaxo.Graph.DensityImagePlotStyle();

      // if nothing is selected, assume that the whole table should be plotted
      int len = dg.SelectedDataColumns.Count;

      Graph.XYZEquidistantMeshColumnPlotData assoc = new Graph.XYZEquidistantMeshColumnPlotData(dg.Doc.DataColumns,len==0 ? null : dg.SelectedDataColumns);

      
      // now create a new Graph with this plot associations

      Altaxo.Graph.GUI.IGraphController gc = Current.ProjectService.CreateNewGraph();

      Altaxo.Graph.IGPlotItem pi = new Altaxo.Graph.DensityImagePlotItem(assoc,plotStyle);
      gc.Doc.Layers[0].PlotItems.Add(pi);

    }




  }
}
