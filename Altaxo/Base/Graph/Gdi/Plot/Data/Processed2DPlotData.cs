using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Altaxo.Data;
namespace Altaxo.Graph.Gdi.Plot.Data
{
  using Graph.Plot.Data;
  /// <summary>
  /// Allows access not only to the original physical plot data,
  /// but also to the plot ranges and to the plot points in absolute layer coordiates.
  /// </summary>
  public class Processed2DPlotData : I3DPhysicalVariantAccessor
  {
    public PlotRangeList RangeList;
    public PointF[] PlotPointsInAbsoluteLayerCoordinates;
    IndexedPhysicalValueAccessor _getXPhysical = new IndexedPhysicalValueAccessor(GetZeroValue);
    IndexedPhysicalValueAccessor _getYPhysical = new IndexedPhysicalValueAccessor(GetZeroValue);
    IndexedPhysicalValueAccessor _getZPhysical = new IndexedPhysicalValueAccessor(GetZeroValue);

    Processed2DPlotData _previousItemData;

    public AltaxoVariant GetXPhysical(int originalRowIndex)
    {
      return _getXPhysical(originalRowIndex);
    }
    public AltaxoVariant GetYPhysical(int originalRowIndex)
    {
      return  _getYPhysical(originalRowIndex) ;
    }
    public virtual AltaxoVariant GetZPhysical(int originalRowIndex)
    {
      return _getZPhysical(originalRowIndex);
    }


    public IndexedPhysicalValueAccessor XPhysicalAccessor
    {
      get
      {
        return _getXPhysical;
      }
      set
      {
        if (value != null)
          _getXPhysical = value;
        else
          _getXPhysical = new IndexedPhysicalValueAccessor(GetZeroValue);
      }
    }
    public IndexedPhysicalValueAccessor YPhysicalAccessor
    {
      get
      {
        return _getYPhysical;
      }
      set
      {
        if (value != null)
          _getYPhysical = value;
        else
          _getYPhysical = new IndexedPhysicalValueAccessor(GetZeroValue);
      }
    }
    public IndexedPhysicalValueAccessor ZPhysicalAccessor
    {
      get
      {
        return _getZPhysical;
      }
      set
      {
        if (value != null)
          _getZPhysical = value;
        else
          _getZPhysical = new IndexedPhysicalValueAccessor(GetZeroValue);
      }
    }

    public Processed2DPlotData PreviousItemData
    {
      get
      {
        return _previousItemData;
      }
      set
      {
        _previousItemData = value;
      }
    }


    /// <summary>
    /// Returns always a AltaxoVariant with the content of 0.0 (a double value). This function can 
    /// serve as an instance for the <see cref="IndexedPhysicalValueAccessor" /see> returning 0.
    /// </summary>
    /// <param name="i">Index. This parameter is ignored here.</param>
    /// <returns>Zero.</returns>
    public static AltaxoVariant GetZeroValue(int i)
    {
      return new AltaxoVariant(0.0);
    }

    /// <summary>
    /// Returns true if the z coordinate is used. Return false if the z coordinate is always 0 (zero), so we can
    /// </summary>
    public virtual bool IsZUsed { get { return false; } }
    /// <summary>
    /// Returns true if the z-value is constant. In this case some optimizations can be made.
    /// </summary>
    public virtual bool IsZConstant { get { return true; } }

  }
}
