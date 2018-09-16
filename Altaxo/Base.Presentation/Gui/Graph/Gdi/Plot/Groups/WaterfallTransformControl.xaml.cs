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
using System.Windows.Controls;
using Altaxo.Units;

namespace Altaxo.Gui.Graph.Gdi.Plot.Groups
{
  /// <summary>
  /// Interaction logic for WaterfallTransformControl.xaml
  /// </summary>
  public partial class WaterfallTransformControl : UserControl, IWaterfallTransformView
  {
    public WaterfallTransformControl()
    {
      InitializeComponent();
    }

    #region IWaterfallTransformView Members

    public void SetXScaleUnitEnvironment(QuantityWithUnitGuiEnvironment environment)
    {
      _edXScale.UnitEnvironment = environment;
    }

    public DimensionfulQuantity XScale
    {
      get
      {
        return _edXScale.SelectedQuantity;
      }
      set
      {
        _edXScale.SelectedQuantity = value;
      }
    }

    public void SetYScaleUnitEnvironment(QuantityWithUnitGuiEnvironment environment)
    {
      _edYScale.UnitEnvironment = environment;
    }

    public DimensionfulQuantity YScale
    {
      get
      {
        return _edYScale.SelectedQuantity;
      }
      set
      {
        _edYScale.SelectedQuantity = value;
      }
    }

    public bool UseClipping
    {
      get
      {
        return true == _chkClipValues.IsChecked;
      }
      set
      {
        _chkClipValues.IsChecked = value;
      }
    }

    #endregion IWaterfallTransformView Members
  }
}
