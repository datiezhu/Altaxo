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

using Altaxo.Graph;
using Altaxo.Graph.Gdi;
using Altaxo.Graph.Gdi.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Gui.Graph.Shapes
{
	public interface IOpenPathShapeView
	{
		PenX DocPen { get; set; }

		object LocationView { set; }
	}

	[UserControllerForObject(typeof(OpenPathShapeBase), 101)]
	[ExpectedTypeOfView(typeof(IOpenPathShapeView))]
	public class OpenPathShapeController : MVCANControllerBase<OpenPathShapeBase, IOpenPathShapeView>
	{
		private IMVCANController _locationController;

		#region IMVCController Members

		protected override void Initialize(bool initData)
		{
			if (initData)
			{
				_locationController = (IMVCANController)Current.Gui.GetController(new object[] { _doc.Location }, typeof(IMVCANController), UseDocument.Directly);
				Current.Gui.FindAndAttachControlTo(_locationController);
			}
			if (_view != null)
			{
				_view.DocPen = _doc.Pen;
				_view.LocationView = _locationController.ViewObject;
			}
		}

		#endregion IMVCController Members

		#region IApplyController Members

		public override bool Apply()
		{
			try
			{
				if (!_locationController.Apply())
					return false;

				_doc.Location.CopyFrom((ItemLocationDirect)_locationController.ModelObject);

				_doc.Pen = _view.DocPen;
			}
			catch (Exception ex)
			{
				Current.Gui.ErrorMessageBox(string.Format("An exception has occured during applying of your settings. The message is: {0}", ex.Message));
				return false;
			}

			if (!object.ReferenceEquals(_doc, _originalDoc))
				_originalDoc.CopyFrom(_doc);

			return true;
		}

		#endregion IApplyController Members
	}
}