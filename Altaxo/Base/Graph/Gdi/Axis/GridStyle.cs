#region Copyright

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
using Altaxo.Geometry;
using Altaxo.Graph.Scales;
using Altaxo.Graph.Scales.Ticks;

namespace Altaxo.Graph.Gdi.Axis
{
  [Serializable]
  public class GridStyle
    :
    Main.SuspendableDocumentNodeWithSetOfEventArgs,
    IRoutedPropertyReceiver,
    ICloneable
  {
    private PenX _minorPen;
    private PenX _majorPen;
    private bool _showGrid;

    private bool _showMinor;
    private bool _showZeroOnly;

    #region Serialization

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase", "Altaxo.Graph.GridStyle", 0)]
    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(GridStyle), 1)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (GridStyle)obj;

        info.AddValue("Visible", s._showGrid);
        if (s._showGrid)
        {
          info.AddValue("ZeroOnly", s._showZeroOnly);
          info.AddValue("MajorPen", s._majorPen);
          info.AddValue("ShowMinor", s._showMinor);
          if (s._showMinor)
            info.AddValue("MinorPen", s._minorPen);
        }
      }

      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        GridStyle s = SDeserialize(o, info, parent);
        return s;
      }

      protected virtual GridStyle SDeserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        GridStyle s = null != o ? (GridStyle)o : new GridStyle();

        s._showGrid = info.GetBoolean("Visible");
        if (s._showGrid)
        {
          s._showZeroOnly = info.GetBoolean("ZeroOnly");
          s._majorPen = (PenX)info.GetValue("MajorPen", s);
          s._majorPen.ParentObject = s;

          s._showMinor = info.GetBoolean("ShowMinor");
          if (s._showMinor)
          {
            s._minorPen = (PenX)info.GetValue("MinorPen", s);
            s._minorPen.ParentObject = s;
          }
        }

        return s;
      }
    }

    #endregion Serialization

    public GridStyle()
    {
      _showGrid = true;
    }

    public GridStyle(GridStyle from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(GridStyle from)
    {
      if (object.ReferenceEquals(this, from))
        return;

      MajorPen = from._majorPen == null ? null : from._majorPen.Clone();
      MinorPen = from._minorPen == null ? null : from._minorPen.Clone();
      _showGrid = from._showGrid;
      _showMinor = from._showMinor;
      _showZeroOnly = from._showZeroOnly;
    }

    protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
    {
      if (null != _majorPen)
        yield return new Main.DocumentNodeAndName(_majorPen, "MajorPen");
      if (null != _minorPen)
        yield return new Main.DocumentNodeAndName(_minorPen, "MinorPen");
    }

    public PenX MajorPen
    {
      get
      {
        if (null == _majorPen)
          MajorPen = new PenX(NamedColors.Blue);
        return _majorPen;
      }
      set
      {
        if (ChildSetMember(ref _majorPen, value ?? new PenX(NamedColors.Blue)))
        {
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public PenX MinorPen
    {
      get
      {
        if (null == _minorPen)
          MinorPen = new PenX(NamedColors.LightBlue);

        return _minorPen;
      }
      set
      {
        if (ChildSetMember(ref _minorPen, value ?? new PenX(NamedColors.LightBlue)))
        {
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public bool ShowGrid
    {
      get { return _showGrid; }
      set
      {
        if (value != _showGrid)
        {
          _showGrid = value;
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public bool ShowMinor
    {
      get { return _showMinor; }
      set
      {
        if (value != _showMinor)
        {
          _showMinor = value;
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public bool ShowZeroOnly
    {
      get { return _showZeroOnly; }
      set
      {
        if (value != _showZeroOnly)
        {
          _showZeroOnly = value;
          EhSelfChanged(EventArgs.Empty);
        }
      }
    }

    public void Paint(Graphics g, IPlotArea layer, int axisnumber)
    {
      if (!_showGrid)
        return;

      Scale axis = layer.Scales[axisnumber];
      TickSpacing ticking = layer.Scales[axisnumber].TickSpacing;

      var layerRect = new RectangleD2D(PointD2D.Empty, layer.Size);

      if (_showZeroOnly)
      {
        var var = new Altaxo.Data.AltaxoVariant(0.0);
        double rel = axis.PhysicalVariantToNormal(var);
        _majorPen.SetEnvironment(layerRect, BrushX.GetEffectiveMaximumResolution(g, 1));
        if (rel >= 0 && rel <= 1)
        {
          if (axisnumber == 0)
            layer.CoordinateSystem.DrawIsoline(g, MajorPen, new Logical3D(rel, 0), new Logical3D(rel, 1));
          else
            layer.CoordinateSystem.DrawIsoline(g, MajorPen, new Logical3D(0, rel), new Logical3D(1, rel));

          //layer.DrawIsoLine(g, MajorPen, axisnumber, rel, 0, 1);
        }
      }
      else
      {
        double[] ticks;

        if (_showMinor)
        {
          _minorPen.SetEnvironment(layerRect, BrushX.GetEffectiveMaximumResolution(g, 1));
          ticks = ticking.GetMinorTicksNormal(axis);
          for (int i = 0; i < ticks.Length; ++i)
          {
            if (axisnumber == 0)
              layer.CoordinateSystem.DrawIsoline(g, MinorPen, new Logical3D(ticks[i], 0), new Logical3D(ticks[i], 1));
            else
              layer.CoordinateSystem.DrawIsoline(g, MinorPen, new Logical3D(0, ticks[i]), new Logical3D(1, ticks[i]));

            //layer.DrawIsoLine(g, MinorPen, axisnumber, ticks[i], 0, 1);
          }
        }

        MajorPen.SetEnvironment(layerRect, BrushX.GetEffectiveMaximumResolution(g, 1));
        ticks = ticking.GetMajorTicksNormal(axis);
        for (int i = 0; i < ticks.Length; ++i)
        {
          if (axisnumber == 0)
            layer.CoordinateSystem.DrawIsoline(g, MajorPen, new Logical3D(ticks[i], 0), new Logical3D(ticks[i], 1));
          else
            layer.CoordinateSystem.DrawIsoline(g, MajorPen, new Logical3D(0, ticks[i]), new Logical3D(1, ticks[i]));

          //layer.DrawIsoLine(g, MajorPen, axisnumber, ticks[i], 0, 1);
        }
      }
    }

    #region ICloneable Members

    public object Clone()
    {
      return new GridStyle(this);
    }

    #endregion ICloneable Members

    #region IRoutedPropertyReceiver Members

    public IEnumerable<(string PropertyName, object PropertyValue, Action<object> PropertySetter)> GetRoutedProperties(string propertyName)
    {
      switch (propertyName)
      {
        case "StrokeWidth":
          if (null != _majorPen)
            yield return (propertyName, _majorPen.Width, (value) => _majorPen.Width = (double)value);
          if (null != _minorPen)
            yield return (propertyName, _minorPen.Width, (value) => _minorPen.Width = (double)value);

          break;
      }

      yield break;
    }

    #endregion IRoutedPropertyReceiver Members
  }
}
