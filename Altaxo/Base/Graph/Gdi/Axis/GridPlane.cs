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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Altaxo.Drawing;

namespace Altaxo.Graph.Gdi.Axis
{
  [Serializable]
  public class GridPlane :
    Main.SuspendableDocumentNodeWithSetOfEventArgs,
    ICloneable
  {
    /// <summary>
    /// Identifies the plane by the axis that is perpendicular to the plane.
    /// </summary>
    private CSPlaneID _planeID;

    /// <summary>
    /// Gridstyle of the smaller of the two axis numbers.
    /// </summary>
    private GridStyle _grid1;

    /// <summary>
    /// Gridstyle of the greater axis number.
    /// </summary>
    private GridStyle _grid2;

    /// <summary>
    /// Background of the grid plane.
    /// </summary>
    private BrushX _background;

    [NonSerialized]
    private GridIndexer _cachedIndexer;

    private void CopyFrom(GridPlane from)
    {
      if (object.ReferenceEquals(this, from))
        return;

      _planeID = from._planeID;
      GridStyleFirst = from._grid1 == null ? null : (GridStyle)from._grid1.Clone();
      GridStyleSecond = from._grid2 == null ? null : (GridStyle)from._grid2.Clone();
      Background = from._background;
    }

    #region Serialization

    #region Version 0

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(GridPlane), 0)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (GridPlane)obj;

        info.AddValue("ID", s._planeID);
        info.AddValue("Grid1", s._grid1);
        info.AddValue("Grid2", s._grid2);
        info.AddValue("Background", s._background);
      }

      protected virtual GridPlane SDeserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var id = (CSPlaneID)info.GetValue("ID", null);
        GridPlane s = (o == null ? new GridPlane(id) : (GridPlane)o);
        s.GridStyleFirst = (GridStyle)info.GetValue("Grid1", s);
        s.GridStyleSecond = (GridStyle)info.GetValue("Grid2", s);
        s.Background = (BrushX)info.GetValue("Background", s);

        return s;
      }

      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        GridPlane s = SDeserialize(o, info, parent);
        return s;
      }
    }

    #endregion Version 0

    #endregion Serialization

    public GridPlane(CSPlaneID id)
    {
      _cachedIndexer = new GridIndexer(this);
      _planeID = id;
    }

    public GridPlane(GridPlane from)
    {
      _cachedIndexer = new GridIndexer(this);
      CopyFrom(from);
    }

    protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
    {
      if (null != _grid1)
        yield return new Main.DocumentNodeAndName(_grid1, "Grid1");
      if (null != _grid2)
        yield return new Main.DocumentNodeAndName(_grid2, "Grid2");
    }

    public GridPlane Clone()
    {
      return new GridPlane(this);
    }

    object ICloneable.Clone()
    {
      return new GridPlane(this);
    }

    public CSPlaneID PlaneID
    {
      get
      {
        return _planeID;
      }
    }

    public GridStyle GridStyleFirst
    {
      get { return _grid1; }
      set
      {
        if (ChildSetMember(ref _grid1, value))
        {
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public GridStyle GridStyleSecond
    {
      get { return _grid2; }
      set
      {
        if (ChildSetMember(ref _grid2, value))
        {
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public Altaxo.Collections.IArray<GridStyle> GridStyle
    {
      get { return _cachedIndexer; }
    }

    public BrushX Background
    {
      get { return _background; }
      set
      {
        if (!(_background == value))
        {
          _background = value;
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Indicates if this grid is used, i.e. hase some visible elements. Returns false
    /// if Grid1 and Grid2 and Background are null.
    /// </summary>
    public bool IsUsed
    {
      get
      {
        return _grid1 != null || _grid2 != null || _background != null;
      }
    }

    public void PaintBackground(Graphics g, IPlotArea layer)
    {
      Region region = layer.CoordinateSystem.GetRegion();
      if (_background != null)
      {
        RectangleF innerArea = region.GetBounds(g);
        using (var gdiBackgroundBrush = BrushCacheGdi.Instance.BorrowBrush(_background, innerArea, g, 1))
        {
          g.FillRegion(gdiBackgroundBrush, region);
        }
      }
    }

    public void PaintGrid(Graphics g, IPlotArea layer)
    {
      Region region = layer.CoordinateSystem.GetRegion();
      Region oldClipRegion = g.Clip;
      g.Clip = region;
      if (null != _grid1)
        _grid1.Paint(g, layer, _planeID.InPlaneAxisNumber1);
      if (null != _grid2)
        _grid2.Paint(g, layer, _planeID.InPlaneAxisNumber2);
      g.Clip = oldClipRegion;
    }

    public void Paint(Graphics g, IPlotArea layer)
    {
      PaintBackground(g, layer);
      PaintGrid(g, layer);
    }

    #region Inner class GridIndexer

    private class GridIndexer : Altaxo.Collections.IArray<GridStyle>
    {
      private GridPlane _parent;

      public GridIndexer(GridPlane parent)
      {
        _parent = parent;
      }

      #region IArray<GridStyle> Members

      public GridStyle this[int i]
      {
        get
        {
          return 0 == i ? _parent._grid1 : _parent._grid2;
        }
        set
        {
          if (0 == i)
            _parent.GridStyleFirst = value;
          else
            _parent.GridStyleSecond = value;
        }
      }

      public int Count
      {
        get { return 2; }
      }

      #endregion IArray<GridStyle> Members
    }

    #endregion Inner class GridIndexer
  }
}
