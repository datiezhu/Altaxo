#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2004 Dr. Dirk Lellinger
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
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Altaxo.Data;
using Altaxo.Graph;
using Altaxo.Serialization;

namespace Altaxo.Graph.GUI.GraphControllerMouseHandlers
{
  /// <summary>
  /// Handles the mouse events when the <see cref="GraphTools.ObjectPointer"/> tools is selected.
  /// </summary>
  public class ReadXYCoordinatesMouseHandler : MouseStateHandler
  {
    /// <summary>
    /// Number of the current layer.
    /// </summary>
    protected int _LayerNumber;

    /// <summary>
    /// Coordinates of the red data reader cross (in printable coordinates)
    /// </summary>
    protected PointF m_Cross;

    /// <summary>
    /// The parent graph controller.
    /// </summary>
    protected GraphController _grac;

    protected float _MovementIncrement=4;

    public ReadXYCoordinatesMouseHandler(GraphController grac)
    {
      _grac = grac;
    }

    /// <summary>
    /// Handles the mouse move event.
    /// </summary>
    /// <param name="grac">The GraphController that sends this event.</param>
    /// <param name="e">MouseEventArgs as provided by the view.</param>
    /// <returns>The next mouse state handler that should handle mouse events.</returns>
    public override MouseStateHandler OnMouseMove(GraphController grac, System.Windows.Forms.MouseEventArgs e)
    {
      base.OnMouseMove(grac,e);
        
      return this;
    }

    /// <summary>
    /// Handles the MouseDown event when the plot point tool is selected
    /// </summary>
    /// <param name="grac">The sender of the event.</param>
    /// <param name="e">The mouse event args</param>
     
    public override MouseStateHandler OnMouseDown(GraphController grac, System.Windows.Forms.MouseEventArgs e)
    {
      base.OnMouseDown(grac, e);

      PointF mouseXY = new PointF(e.X,e.Y);
      m_Cross = grac.PixelToPrintableAreaCoordinates(mouseXY);

      DisplayCrossCoordinates();
      
      _grac.RepaintGraphArea(); // no refresh necessary, only invalidate to show the cross
         
      return this;
    } // end of function


    void DisplayCrossCoordinates()
    {
     

      XYPlotLayer layer = _grac.ActiveLayer;
      if(layer==null)
        return;

      PointF layerXY = layer.GraphToLayerCoordinates(m_Cross);

      double xr = layerXY.X/layer.Size.Width;
      double yr = layerXY.Y/layer.Size.Height;

      double xphys, yphys;

      layer.AreaToLogicalConversion.Convert(xr,yr,out xphys, out yphys);

      this.DisplayData(_grac.CurrentLayerNumber,xphys,yphys);
    }


    void DisplayData(int layerNumber, double x, double y)
    {
      Current.DataDisplay.WriteOneLine(string.Format(
        "Layer({0}) X={1}, Y={2}",
        layerNumber,
        x,
        y));
    }


    /// <summary>
    /// Moves the cross along the plot.
    /// </summary>
    /// <param name="increment"></param>
    void MoveLeftRight(float increment)
    {
      
       m_Cross.X += increment;

       DisplayCrossCoordinates();

       _grac.RepaintGraphArea(); // no refresh necessary, only invalidate to show the cross
    }

    /// <summary>
    /// Moves the cross to the next plot item. If no plot item is found in this layer, it moves the cross to the next layer.
    /// </summary>
    /// <param name="increment"></param>
    void MoveUpDown(float increment)
    {
      
      m_Cross.X += increment;

      DisplayCrossCoordinates();

       _grac.RepaintGraphArea(); // no refresh necessary, only invalidate to show the cross

      
    }


    /// <summary>
    /// Handles the mouse up event.
    /// </summary>
    /// <param name="grac">The GraphController that sends this event.</param>
    /// <param name="e">MouseEventArgs as provided by the view.</param>
    /// <returns>The next mouse state handler that should handle mouse events.</returns>
    public override MouseStateHandler OnMouseUp(GraphController grac, System.Windows.Forms.MouseEventArgs e)
    {
      base.OnMouseUp(grac,e);

      return this;
    }

    /// <summary>
    /// Handles the mouse doubleclick event.
    /// </summary>
    /// <param name="grac">The GraphController that sends this event.</param>
    /// <param name="e">EventArgs as provided by the view.</param>
    /// <returns>The next mouse state handler that should handle mouse events.</returns>
    public override MouseStateHandler OnDoubleClick(GraphController grac, System.EventArgs e)
    {
      base.OnDoubleClick(grac,e);

      
      return this;
    }


    /// <summary>
    /// Handles the mouse click event.
    /// </summary>
    /// <param name="grac">The GraphController that sends this event.</param>
    /// <param name="e">EventArgs as provided by the view.</param>
    /// <returns>The next mouse state handler that should handle mouse events.</returns>
    public override MouseStateHandler OnClick(GraphController grac, System.EventArgs e)
    {
      base.OnClick(grac,e);

      

      return this;
    }


    public override void AfterPaint(GraphController grac, Graphics g)
    {
      base.AfterPaint (grac, g);
      g.TranslateTransform(_grac.Doc.PrintableBounds.X,_grac.Doc.PrintableBounds.Y);

      // draw a red cross onto the selected data point

      g.DrawLine(System.Drawing.Pens.Red,m_Cross.X+1,m_Cross.Y,m_Cross.X+10,m_Cross.Y);
      g.DrawLine(System.Drawing.Pens.Red,m_Cross.X-1,m_Cross.Y,m_Cross.X-10,m_Cross.Y);
      g.DrawLine(System.Drawing.Pens.Red,m_Cross.X,m_Cross.Y+1,m_Cross.X,m_Cross.Y+10);
      g.DrawLine(System.Drawing.Pens.Red,m_Cross.X,m_Cross.Y-1,m_Cross.X,m_Cross.Y-10);
    }


    /// <summary>
    /// This function is called if a key is pressed.
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="keyData"></param>
    /// <returns></returns>
    public override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if(keyData == Keys.Left)
      {
        System.Diagnostics.Trace.WriteLine("Read tool key handler, left key pressed!");
        MoveLeftRight(-_MovementIncrement);
        return true;
      }
      else if(keyData == Keys.Right)
      {
        System.Diagnostics.Trace.WriteLine("Read tool key handler, right key pressed!");
        MoveLeftRight(_MovementIncrement);
        return true;
      }
      else if(keyData == Keys.Up)
      {
        MoveUpDown(_MovementIncrement);
        return true;
      }
      else if(keyData == Keys.Down)
      {
        MoveUpDown(-_MovementIncrement);
        return true;
      }


      return false; // per default the key is not processed
    }


    

  } // end of class

 }
