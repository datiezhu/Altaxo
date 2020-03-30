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
using System.Text;
using Altaxo.Drawing;

namespace Altaxo.Gui.Common.Drawing
{
  public interface IBrushViewSimple
  {
    BrushX Brush { get; set; }
  }

  [UserControllerForObject(typeof(BrushX))]
  [ExpectedTypeOfView(typeof(IBrushViewSimple))]
  public class BrushControllerSimple : MVCANControllerEditImmutableDocBase<BrushX, IBrushViewSimple>
  {
    public override IEnumerable<ControllerAndSetNullMethod> GetSubControllers()
    {
      yield break;
    }

    protected override void Initialize(bool initData)
    {
      base.Initialize(initData);

      if (_view != null)
      {
        _view.Brush = _doc;
      }
    }

    public override bool Apply(bool disposeController)
    {
      if (_doc != null || _view.Brush.IsVisible)
        _doc = _view.Brush;

      return ApplyEnd(true, disposeController);
    }
  }
}
