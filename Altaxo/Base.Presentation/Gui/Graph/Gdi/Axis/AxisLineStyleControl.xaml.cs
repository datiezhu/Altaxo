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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Altaxo.Gui.Graph.Gdi.Axis
{
  using Altaxo.Drawing;
  using Altaxo.Gui.Common.Drawing;

  /// <summary>
  /// Interaction logic for AxisLineStyleControl.xaml
  /// </summary>
  public partial class AxisLineStyleControl : UserControl, IAxisLineStyleView
  {
    private PenControlsGlue _linePenGlue;
    private PenControlsGlue _majorPenGlue;
    private PenControlsGlue _minorPenGlue;

    public AxisLineStyleControl()
    {
      InitializeComponent();

      _linePenGlue = new PenControlsGlue(false)
      {
        CbBrush = _lineBrushColor,
        CbLineThickness = _lineLineThickness
      };

      _majorPenGlue = new PenControlsGlue(false)
      {
        CbBrush = _majorLineColor,
        CbLineThickness = _lineMajorThickness
      };

      _minorPenGlue = new PenControlsGlue(false)
      {
        CbBrush = _minorLineColor,
        CbLineThickness = _lineMinorThickness
      };

      _linePenGlue.PenChanged += new EventHandler(EhLinePen_Changed);
    }

    private void EhLinePen_Changed(object sender, EventArgs e)
    {
      if (false == _chkCustomMajorColor.IsChecked)
      {
        if (_majorPenGlue.Pen != null)
          _majorPenGlue.Pen = _majorPenGlue.Pen.WithBrush(_linePenGlue.Pen.Brush);
      }
      if (false == _chkCustomMinorColor.IsChecked)
      {
        if (_minorPenGlue.Pen != null)
          _minorPenGlue.Pen = _minorPenGlue.Pen.WithBrush(_linePenGlue.Pen.Brush);
      }

      if (false == _chkCustomMajorThickness.IsChecked)
        _lineMajorThickness.SelectedQuantity = _lineLineThickness.SelectedQuantity;
      if (false == _chkCustomMinorThickness.IsChecked)
        _lineMinorThickness.SelectedQuantity = _lineLineThickness.SelectedQuantity;
    }

    private void EhIndividualMajorColor_CheckChanged(object sender, RoutedEventArgs e)
    {
      if (false == _chkCustomMajorColor.IsChecked)
        _majorLineColor.SelectedBrush = _lineBrushColor.SelectedBrush;
      _majorLineColor.IsEnabled = true == _chkCustomMajorColor.IsChecked;
    }

    private void EhIndividualMajorThickness_CheckChanged(object sender, RoutedEventArgs e)
    {
      if (false == _chkCustomMajorThickness.IsChecked)
        _lineMajorThickness.SelectedQuantity = _lineLineThickness.SelectedQuantity;
      _lineMajorThickness.IsEnabled = true == _chkCustomMajorThickness.IsChecked;
    }

    private void EhIndividualMinorColor_CheckChanged(object sender, RoutedEventArgs e)
    {
      if (false == _chkCustomMinorColor.IsChecked)
        _minorLineColor.SelectedBrush = _lineBrushColor.SelectedBrush;
      _minorLineColor.IsEnabled = true == _chkCustomMinorColor.IsChecked;
    }

    private void EhIndividualMinorThickness_CheckChanged(object sender, RoutedEventArgs e)
    {
      if (false == _chkCustomMinorThickness.IsChecked)
        _lineMinorThickness.SelectedQuantity = _lineLineThickness.SelectedQuantity;
      _lineMinorThickness.IsEnabled = true == _chkCustomMinorThickness.IsChecked;
    }

    #region Helper

    private bool CustomMajorThickness
    {
      set
      {
        _chkCustomMajorThickness.IsChecked = value;
        _lineMajorThickness.IsEnabled = value;
      }
    }

    private bool CustomMinorThickness
    {
      set
      {
        _chkCustomMinorThickness.IsChecked = value;
        _lineMinorThickness.IsEnabled = value;
      }
    }

    private bool CustomMajorColor
    {
      set
      {
        _chkCustomMajorColor.IsChecked = value;
        _majorLineColor.IsEnabled = value;
      }
    }

    private bool CustomMinorColor
    {
      set
      {
        _chkCustomMinorColor.IsChecked = value;
        _minorLineColor.IsEnabled = value;
      }
    }

    #endregion Helper

    #region IAxisLineStyleView

    public bool ShowLine
    {
      get
      {
        return true == _chkEnableLine.IsChecked;
      }
      set
      {
        _chkEnableLine.IsChecked = value;
      }
    }

    public PenX LinePen
    {
      get
      {
        return _linePenGlue.Pen;
      }
      set
      {
        _linePenGlue.Pen = value;
      }
    }

    public PenX MajorPen
    {
      get
      {
        return _majorPenGlue.Pen;
      }
      set
      {
        _majorPenGlue.Pen = value;
        if (value != null)
        {
          CustomMajorColor = !PenX.AreEqualUnlessWidth(value, _linePenGlue.Pen);
          CustomMajorThickness = (value.Width != _linePenGlue.Pen.Width);
        }
      }
    }

    public PenX MinorPen
    {
      get
      {
        return _minorPenGlue.Pen;
      }
      set
      {
        _minorPenGlue.Pen = value;
        if (value != null)
        {
          CustomMinorColor = !PenX.AreEqualUnlessWidth(value, _linePenGlue.Pen);
          CustomMinorThickness = (value.Width != _linePenGlue.Pen.Width);
        }
      }
    }

    public double MajorTickLength
    {
      get
      {
        return _lineMajorLength.SelectedQuantityAsValueInPoints;
      }
      set
      {
        _lineMajorLength.SelectedQuantityAsValueInPoints = value;
      }
    }

    public double MinorTickLength
    {
      get
      {
        return _lineMinorLength.SelectedQuantityAsValueInPoints;
      }
      set
      {
        _lineMinorLength.SelectedQuantityAsValueInPoints = value;
      }
    }

    public Collections.SelectableListNodeList MajorPenTicks
    {
      get
      {
        return (Collections.SelectableListNodeList)_majorWhichTicksLayout.ItemsSource;
      }
      set
      {
        _majorWhichTicksLayout.ItemsSource = value;
      }
    }

    public Collections.SelectableListNodeList MinorPenTicks
    {
      get
      {
        return (Collections.SelectableListNodeList)_minorWhichTicksLayout.ItemsSource;
      }
      set
      {
        _minorWhichTicksLayout.ItemsSource = value;
      }
    }

    #endregion IAxisLineStyleView
  }
}
