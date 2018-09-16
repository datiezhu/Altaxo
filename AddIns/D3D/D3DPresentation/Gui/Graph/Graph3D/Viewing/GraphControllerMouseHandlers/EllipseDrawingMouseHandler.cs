#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2016 Dr. Dirk Lellinger
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

using System;
using System.Windows.Input;
using Altaxo.Geometry;
using Altaxo.Graph.Graph3D;
using Altaxo.Graph.Graph3D.Shapes;

namespace Altaxo.Gui.Graph.Graph3D.Viewing.GraphControllerMouseHandlers
{
  /// <summary>
  /// This class handles the mouse events in case the text tool is selected.
  /// </summary>
  public class EllipseDrawingMouseHandler : MouseStateHandler
  {
    /// <summary>The graph controller this mouse handler belongs to.</summary>
    private Graph3DController _grac;

    public EllipseDrawingMouseHandler(Graph3DController grac)
    {
      _grac = grac;
      _grac?.View?.SetPanelCursor(Cursors.IBeam);
    }

    public override GraphToolType GraphToolType
    {
      get { return GraphToolType.EllipseDrawing; }
    }

    /// <summary>
    /// Handles the click event by opening the text tool dialog.
    /// </summary>
    /// <param name="e">EventArgs.</param>
    /// <param name="position">Mouse position.</param>
    /// <returns>The mouse state handler for handling the next mouse events.</returns>
    public override void OnClick(PointD3D position, MouseButtonEventArgs e)
    {
      base.OnClick(position, e);

      _cachedActiveLayer = _grac.ActiveLayer;
      _cachedActiveLayerTransformation = _cachedActiveLayer.TransformationFromRootToHere();
      GetHitPointOnActiveLayerPlaneFacingTheCamera(_grac.Doc, _grac.ActiveLayer, position, out var hitPointOnLayerPlaneInLayerCoordinates, out var rotationsRadian);

      double sphereDiameter = _cachedActiveLayer.Size.Length / 10;
      var tgo = new Ellipsoid();
      tgo.SetParentSize(_cachedActiveLayer.Size, false);
      tgo.Size = new VectorD3D(sphereDiameter, sphereDiameter, sphereDiameter);
      tgo.Position = hitPointOnLayerPlaneInLayerCoordinates;
      tgo.ParentObject = _grac.ActiveLayer;

      // deselect the text tool
      _grac.CurrentGraphTool = GraphToolType.ObjectPointer;

      _grac.ActiveLayer.GraphObjects.Add(tgo);
    }
  }
}
