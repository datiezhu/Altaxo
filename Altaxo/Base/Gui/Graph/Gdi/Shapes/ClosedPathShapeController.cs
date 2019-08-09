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
using Altaxo.Graph;
using Altaxo.Graph.Gdi;
using Altaxo.Graph.Gdi.Shapes;

namespace Altaxo.Gui.Graph.Gdi.Shapes
{
  public interface IClosedPathShapeView
  {
    PenX DocPen { get; set; }

    BrushX DocBrush { get; set; }

    object LocationView { set; }
  }

  [UserControllerForObject(typeof(ClosedPathShapeBase))]
  [ExpectedTypeOfView(typeof(IClosedPathShapeView))]
  public class ClosedPathShapeController : MVCANControllerEditOriginalDocBase<ClosedPathShapeBase, IClosedPathShapeView>
  {
    private IMVCANController _locationController;

    public override IEnumerable<ControllerAndSetNullMethod> GetSubControllers()
    {
      yield return new ControllerAndSetNullMethod(_locationController, () => _locationController = null);
    }

    protected override void Initialize(bool initData)
    {
      base.Initialize(initData);

      if (initData)
      {
        _locationController = (IMVCANController)Current.Gui.GetController(new object[] { _doc.Location }, typeof(IMVCANController), UseDocument.Directly);
        Current.Gui.FindAndAttachControlTo(_locationController);
      }
      if (_view != null)
      {
        _view.DocPen = _doc.Pen;
        _view.DocBrush = _doc.Brush;
        _view.LocationView = _locationController.ViewObject;
      }
    }

    public override bool Apply(bool disposeController)
    {
      if (!_locationController.Apply(disposeController))
        return false;

      _doc.Pen = _view.DocPen;
      _doc.Brush = _view.DocBrush;

      if (!object.ReferenceEquals(_doc.Location, _locationController.ModelObject))
        _doc.Location.CopyFrom((ItemLocationDirect)_locationController.ModelObject);

      return ApplyEnd(true, disposeController);
    }
  }
}
