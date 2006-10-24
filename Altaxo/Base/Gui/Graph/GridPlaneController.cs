using System;
using System.Collections.Generic;
using System.Text;

using Altaxo.Graph.Gdi;
using Altaxo.Graph.Gdi.Background;
using Altaxo.Graph.Gdi.Axis;
using Altaxo.Gui.Common;
using Altaxo.Gui.Common.Drawing;

namespace Altaxo.Gui.Graph
{

  [UserControllerForObject(typeof(GridPlane))]
  public class GridPlaneController : MultiChildController, IMVCANController
  {
    GridPlane _doc;

    IMVCAController _grid1;
    IMVCAController _grid2;
    IMVCAController _background;

    public GridPlaneController()
    {
    }

    public GridPlaneController(GridPlane doc)
    {
      InitializeDocument(doc);
    }

    UseDocument _useDocument;
    public UseDocument UseDocumentCopy { set { _useDocument = value; } }

    public bool InitializeDocument(params object[] args)
    {
      if (args.Length == 0 || !(args[0] is GridPlane))
        return false;

      bool isVirgin = null == _doc;

      _doc = (GridPlane)args[0];

      Initialize(true);
      return true;
    }


    void Initialize(bool bInit)
    {
      if(bInit)
      {
      _grid1 = new XYGridStyleController(_doc.GridStyleFirst);
      Current.Gui.FindAndAttachControlTo(_grid1);
      ControlViewElement c1 = new ControlViewElement("Grid1", _grid1, _grid1.ViewObject);

      _grid2 = new XYGridStyleController(_doc.GridStyleSecond);
      Current.Gui.FindAndAttachControlTo(_grid2);
      ControlViewElement c2 = new ControlViewElement("Grid2", _grid2, _grid2.ViewObject);

      _background = new BrushControllerSimple(_doc.Background);
      Current.Gui.FindAndAttachControlTo(_background);
      ControlViewElement c3 = new ControlViewElement("Background", _background, _background.ViewObject);
      
        base.Initialize(new ControlViewElement[] { c1, c2, c3 }, false);
      }
    }

   

    public override object ModelObject
    {
      get
      {
        return _doc;
      }
    }

    public override bool Apply()
    {
      if (false == base.Apply())
        return false;

      _doc.GridStyleFirst = (GridStyle)_grid1.ModelObject;
      _doc.GridStyleSecond = (GridStyle)_grid2.ModelObject;
      _doc.Background = (BrushX)_background.ModelObject;

      return true;
    }
  }
}
