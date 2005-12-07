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
using System.Drawing;
namespace Altaxo.Graph.Procedures
{

  /// <summary>
  /// Document of the <see cref="Altaxo.Gui.Graph.ArrangeLayersController" /> controller.
  /// </summary>
  public class ArrangeLayersDocument
  {
    public int NumberOfRows=2;
    public int NumberOfColumns=1;
    public double HorizontalSpacing=5;
    public double VerticalSpacing=5;
    public double LeftMargin=15;
    public double TopMargin=10;
    public double RightMargin=10;
    public double BottomMargin=15;

  
    public void CopyFrom(ArrangeLayersDocument from)
    {
      this.NumberOfColumns = from.NumberOfColumns;
      this.NumberOfRows = from.NumberOfRows;
      this.HorizontalSpacing = from.HorizontalSpacing;
      this.VerticalSpacing = from.VerticalSpacing;
      this.LeftMargin = from.LeftMargin;
      this.TopMargin = from.TopMargin;
      this.RightMargin = from.RightMargin;
      this.BottomMargin = from.BottomMargin;
    }

  
    public static void ArrangeLayers(GraphDocument graph)
    {
      ArrangeLayersDocument doc = new ArrangeLayersDocument();
      object doco = doc;
      if(Current.Gui.ShowDialog(ref doco,"Arrange layers"))
      {
        doc = (ArrangeLayersDocument)doco;
        try 
        {
          ArrangeLayers(doc,graph);
        }
        catch(Exception ex)
        {
          Current.Gui.ErrorMessageBox(ex.Message);
        }
      }
    }

    public static void ArrangeLayers(ArrangeLayersDocument arrangement, GraphDocument graph)
    {
      int numPresentLayers = graph.Layers.Count;
      int numDestLayers = arrangement.NumberOfColumns*arrangement.NumberOfRows;

      int additionalLayers = Math.Max(0,numDestLayers-numPresentLayers);


      // calculate the size of each layer
      double relHorzSize = 100-(arrangement.LeftMargin + arrangement.RightMargin + (arrangement.NumberOfColumns-1)*arrangement.VerticalSpacing);
      double relVertSize = 100-(arrangement.TopMargin + arrangement.BottomMargin + (arrangement.NumberOfRows-1)*arrangement.HorizontalSpacing);
      relHorzSize /= arrangement.NumberOfColumns;
      relVertSize /= arrangement.NumberOfRows;


      if(relHorzSize<=0)
        throw new ArgumentException("The calculated horizontal size of the resulting layers would be negative");
      if(relVertSize<=0)
        throw new ArgumentException("The calculated vertical size of the resulting layers would be negative");

      SizeF layerSize = new SizeF((float)(relHorzSize*graph.PrintableSize.Width),(float)(relVertSize*graph.PrintableSize.Height));

      int nLayer=-1;
      for(int i=0;i<arrangement.NumberOfRows;++i)
      {
        double relVertPos = arrangement.TopMargin + i*(arrangement.HorizontalSpacing+relVertSize);

        for(int j=0;j<arrangement.NumberOfColumns;++j)
        {
          nLayer++;

          double relHorzPos = arrangement.LeftMargin + j*(arrangement.VerticalSpacing+relHorzSize);

          // calculate position
          PointF layerPosition = new PointF((float)(relHorzPos*graph.PrintableSize.Width),(float)(relVertPos*graph.PrintableSize.Height));

          if(nLayer>=numPresentLayers)
          {
            graph.Layers.Add(new XYPlotLayer(layerPosition,layerSize));
            graph.Layers[nLayer].TopAxisEnabled = false;
            graph.Layers[nLayer].RightAxisEnabled = false;
          }
          SizeF oldSize = graph.Layers[nLayer].Size;
          graph.Layers[nLayer].SetSize(relHorzSize/100,XYPlotLayerSizeType.RelativeToGraphDocument,relVertSize/100,XYPlotLayerSizeType.RelativeToGraphDocument);
          SizeF newSize = graph.Layers[nLayer].Size;

          if(oldSize!=newSize)
            graph.Layers[nLayer].RescaleInnerItemPositions(newSize.Width/oldSize.Width,newSize.Height/oldSize.Height);
          graph.Layers[nLayer].SetPosition(relHorzPos/100,XYPlotLayerPositionType.RelativeToGraphDocument,relVertPos/100,XYPlotLayerPositionType.RelativeToGraphDocument);

        }

      }
     
    }
  }
}