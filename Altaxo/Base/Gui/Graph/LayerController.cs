#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2005 Dr. Dirk Lellinger
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
#endregion

using System;
using System.Collections.Generic;

using Altaxo.Graph;
using Altaxo.Graph.Gdi;
using Altaxo.Graph.Gdi.Axis;
using Altaxo.Graph.Gdi.Plot;
using Altaxo.Graph.Gdi.Plot.Data;
using Altaxo.Graph.Gdi.Plot.Styles;
using Altaxo.Gui;
using Altaxo.Gui.Common;

namespace Altaxo.Gui.Graph
{
  #region Interfaces
  public interface ILayerController: IApplyController
  {
    void EhView_PageChanged(string firstChoice);
    void EhView_SecondChoiceChanged(int index, string item);
    void EhView_PageEnabledChanged( bool pageEnabled);

  }

  public interface ILayerView
  {

    ILayerController Controller { get; set; }

    System.Windows.Forms.Form Form  { get; }

    void AddTab(string name, string text);

    object CurrentContent { get; set; }
    void SetCurrentContentWithEnable(object guielement, bool enable, string title);
    bool IsPageEnabled { get; set; }

    void SelectTab(string name);
    void InitializeSecondaryChoice(string[] names, string name);

  }
  #endregion

  /// <summary>
  /// Summary description for LayerController.
  /// </summary>
  public class LayerController : ILayerController
  {
    protected ILayerView _view;

    protected XYPlotLayer _layer;

    private string   m_CurrentPage;

    enum TabType { Unique, Scales, Styles, Planes };
    TabType _primaryChoice; // which tab type is currently choosen
    private int _currentScale; // which scale is choosen 0==X-AxisScale, 1==Y-AxisScale

    private CSLineID _currentAxisID; // which style is currently choosen
    private CSPlaneID _currentPlaneID; // which plane is currently chosen for the grid
  

    IMVCController m_CurrentController;


    protected Altaxo.Gui.Graph.CoordinateSystemController _coordinateController;
    protected ILayerPositionController m_LayerPositionController;
    protected ILineScatterLayerContentsController m_LayerContentsController;
    protected IAxisScaleController[] m_AxisScaleController;
    protected ITitleFormatLayerController[] m_TitleFormatLayerController;
    protected Altaxo.Gui.Graph.IXYAxisLabelStyleController[] m_LabelStyleController;
    protected Altaxo.Gui.Graph.IXYAxisLabelStyleController[] m_MinorLabelStyleController;
    //protected IMVCAController[] _GridStyleController;

    Dictionary<CSLineID, A2DAxisStyleInformation> _axisStyleIds;
    List<A2DAxisStyleInformation> _axisStyleInfoSortedByName;
    List<CSPlaneID> _planeIdentifier;
    
    Dictionary<CSLineID, ITitleFormatLayerController> _TitleFormatController;
    Dictionary<CSLineID, Altaxo.Gui.Graph.IXYAxisLabelStyleController> _MajorLabelController;
    Dictionary<CSLineID, Altaxo.Gui.Graph.IXYAxisLabelStyleController> _MinorLabelController;

    Dictionary<CSPlaneID, IMVCAController> _GridStyleController;

    Dictionary<CSLineID, bool> _enableMajorLabels;
    Dictionary<CSLineID, bool> _enableMinorLabels;
  
  
    public LayerController(XYPlotLayer layer)
      : this(layer,"Scale",1,null)
    {
    }
    public LayerController(XYPlotLayer layer, string currentPage, CSLineID id)
      : this(layer,currentPage,id.ParallelAxisNumber,id)
    {
    }


    LayerController(XYPlotLayer layer, string currentPage, int axisScaleIdx, CSLineID id)
    {
      _layer = layer;

      // collect the AxisStyleIdentifier from the actual layer and also all possible AxisStyleIdentifier
      _axisStyleIds = new Dictionary<CSLineID, A2DAxisStyleInformation>();
      _axisStyleInfoSortedByName = new List<A2DAxisStyleInformation>();
      foreach (CSLineID ids in _layer.CoordinateSystem.GetJoinedAxisStyleIdentifier(_layer.AxisStyles.AxisStyleIDs, new CSLineID[] { id }))
      {
        A2DAxisStyleInformation info = _layer.CoordinateSystem.GetAxisStyleInformation(ids);
        _axisStyleIds.Add(info.Identifier, info);
        _axisStyleInfoSortedByName.Add(info);
      }

      _planeIdentifier = new List<CSPlaneID>();
      _planeIdentifier.Add(CSPlaneID.Front);


      _currentScale = axisScaleIdx;
      _currentAxisID = id;
      _currentPlaneID = CSPlaneID.Front;


      m_AxisScaleController = new AxisScaleController[2];
      _GridStyleController = new Dictionary<CSPlaneID, IMVCAController>();
      _TitleFormatController = new Dictionary<CSLineID, ITitleFormatLayerController>();
      _MajorLabelController = new Dictionary<CSLineID, Altaxo.Gui.Graph.IXYAxisLabelStyleController>();
      _MinorLabelController = new Dictionary<CSLineID, Altaxo.Gui.Graph.IXYAxisLabelStyleController>();

      _enableMajorLabels = new Dictionary<CSLineID, bool>();
      _enableMinorLabels = new Dictionary<CSLineID, bool>();
      foreach(CSLineID ident in _axisStyleIds.Keys)
      {
        AxisStyle prop = layer.AxisStyles.AxisStyle(ident);
        if(prop==null)
        {
          _enableMajorLabels.Add(ident, false);
          _enableMinorLabels.Add(ident, false);
        }
        else
        {
          _enableMajorLabels.Add(ident, prop.ShowMajorLabels);
          _enableMinorLabels.Add(ident, prop.ShowMinorLabels);
        }
      }



      m_CurrentPage = currentPage;

      if(null!=View)
        SetViewElements();
    }

    public static void RegisterEditHandlers()
    {
      // register here editor methods

      XYPlotLayer.AxisScaleEditorMethod = new DoubleClickHandler(EhAxisScaleEdit);
      XYPlotLayer.AxisStyleEditorMethod = new DoubleClickHandler(EhAxisStyleEdit);
      XYPlotLayer.AxisLabelStyleEditorMethod = new DoubleClickHandler(EhAxisLabelStyleEdit);
      XYPlotLayer.LayerPositionEditorMethod = new DoubleClickHandler(EhLayerPositionEdit);

    }

    public static bool EhLayerPositionEdit(IHitTestObject hit)
    {
      XYPlotLayer layer = hit.HittedObject as XYPlotLayer;
      if(layer==null)
        return false;

      ShowDialog(Current.MainWindow, layer, "Position");

      return false;
    }

    public static bool EhAxisScaleEdit(IHitTestObject hit)
    {
      AxisLineStyle style = hit.HittedObject as AxisLineStyle;
      if(style==null || hit.ParentLayer==null)
        return false;
   

      ShowDialog(Current.MainWindow, hit.ParentLayer, "Scale", style.AxisStyleID);

      return false;
    }

    public static bool EhAxisStyleEdit(IHitTestObject hit)
    {
      AxisLineStyle style = hit.HittedObject as AxisLineStyle;
      if(style==null || hit.ParentLayer==null)
        return false;

      ShowDialog(Current.MainWindow, hit.ParentLayer, "TitleAndFormat",style.AxisStyleID);

      return false;
    }

    public static bool EhAxisLabelStyleEdit(IHitTestObject hit)
    {
      AxisLabelStyle style = hit.HittedObject as AxisLabelStyle;
      if(style==null || hit.ParentLayer==null)
        return false;

      ShowDialog(Current.MainWindow, hit.ParentLayer, "MajorLabels",style.AxisStyleID);

      return false;
    }

    public ILayerView View
    {
      get { return _view; }
      set 
      {
        if(null!=_view)
        {
          _view.Controller = null;
        }

        _view = value;
        
        if(null!=_view)
        {
          _view.Controller = this;
          SetViewElements();
        }
      }
    }

    void SetViewElements()
    {
      if(null==View)
        return;

      // add all necessary Tabs
      View.AddTab("Scale","Scale");
      View.AddTab("CS", "Coord.System");
      View.AddTab("TitleAndFormat","Title&&Format");
      View.AddTab("Contents","Contents");
      View.AddTab("Position","Position");
      View.AddTab("MajorLabels","Major labels");
      View.AddTab("MinorLabels","Minor labels");
      View.AddTab("GridStyle","Grid style");

      // Set the controller of the current visible Tab
      SetCurrentTabController(true);
    }



    void SetCurrentTabController(bool pageChanged)
    {
      switch(m_CurrentPage)
      {
        case "Contents":
          if (pageChanged)
          {
            View.SelectTab(m_CurrentPage);
            SetLayerSecondaryChoice();
          }
          if (null == m_LayerContentsController)
          {
            m_LayerContentsController = new LineScatterLayerContentsController(_layer);
            m_LayerContentsController.View = new LineScatterLayerContentsControl();
          }
          m_CurrentController = m_LayerContentsController;
          View.CurrentContent = m_CurrentController.ViewObject;
          break;
        case "Position":
          if (pageChanged)
          {
            View.SelectTab(m_CurrentPage);
            SetLayerSecondaryChoice();
          }
          if (null == m_LayerPositionController)
          {
            m_LayerPositionController = new LayerPositionController(_layer);
            m_LayerPositionController.View = new LayerPositionControl();
          }
          m_CurrentController = m_LayerPositionController;
          View.CurrentContent = m_LayerPositionController.ViewObject;
          break;


        case "Scale":
          if(pageChanged)
          {
            View.SelectTab(m_CurrentPage);
            SetHorzVertSecondaryChoice();
          }
          if (m_AxisScaleController[_currentScale] == null)
          {
            m_AxisScaleController[_currentScale] = new AxisScaleController(_layer, _currentScale);
            m_AxisScaleController[_currentScale].ViewObject = new AxisScaleControl();
          }
          m_CurrentController = m_AxisScaleController[_currentScale];
          View.CurrentContent = m_CurrentController.ViewObject;
          break;

        case "CS":
          if (pageChanged)
          {
            View.SelectTab(m_CurrentPage);
            SetLayerSecondaryChoice();
          }
          if (null == this._coordinateController)
          {
            this._coordinateController = new Altaxo.Gui.Graph.CoordinateSystemController(_layer.CoordinateSystem);
            Current.Gui.FindAndAttachControlTo(this._coordinateController);
          }
          m_CurrentController = this._coordinateController;
          View.CurrentContent = this._coordinateController.ViewObject;
          break;

        case "GridStyle":
          if (pageChanged)
          {
            View.SelectTab(m_CurrentPage);
            SetPlaneSecondaryChoice();
          }

          if (!_GridStyleController.ContainsKey(_currentPlaneID))
          {
            GridPlane p = _layer.GridPlanes.Contains(_currentPlaneID) ? _layer.GridPlanes[_currentPlaneID] : new GridPlane(_currentPlaneID);
            GridPlaneController ctrl = new GridPlaneController(p);
            Current.Gui.FindAndAttachControlTo(ctrl);
            _GridStyleController.Add(_currentPlaneID, ctrl);
          }
          m_CurrentController = _GridStyleController[_currentPlaneID];
          View.CurrentContent = this.m_CurrentController.ViewObject;
          
          
          break;

        case "TitleAndFormat":
        case "MajorLabels":
        case "MinorLabels":
          if(pageChanged)
          {
            View.SelectTab(m_CurrentPage);
            SetEdgeSecondaryChoice();
          }

          if (!_TitleFormatController.ContainsKey(_currentAxisID))
          {
            AxisStyle ast = _layer.AxisStyles.Contains(_currentAxisID) ? _layer.AxisStyles.AxisStyle(_currentAxisID) : new AxisStyle(_currentAxisID);
            _TitleFormatController.Add(_currentAxisID, new TitleFormatLayerController(ast));
            if (ast.MajorLabelStyle == null)
              ast.ShowMajorLabels = true;
            _MajorLabelController.Add(_currentAxisID, new XYAxisLabelStyleController((AxisLabelStyle)ast.MajorLabelStyle));

            if (ast.MinorLabelStyle == null)
              ast.ShowMinorLabels = true;
            _MinorLabelController.Add(_currentAxisID, new XYAxisLabelStyleController((AxisLabelStyle)ast.MinorLabelStyle));
          }

          if (m_CurrentPage == "TitleAndFormat")
          {
            m_CurrentController = _TitleFormatController[_currentAxisID];
            if (m_CurrentController.ViewObject == null)
              m_CurrentController.ViewObject = new TitleFormatLayerControl();
          }
          else if (m_CurrentPage == "MajorLabels")
            m_CurrentController = _MajorLabelController[_currentAxisID];
          else
            m_CurrentController = _MinorLabelController[_currentAxisID];

          if (m_CurrentController.ViewObject == null)
            Current.Gui.FindAndAttachControlTo(m_CurrentController);

          if (m_CurrentPage == "TitleAndFormat")
            View.CurrentContent = this.m_CurrentController.ViewObject;
          else if (m_CurrentPage == "MajorLabels")
            View.SetCurrentContentWithEnable(this.m_CurrentController.ViewObject, _enableMajorLabels[_currentAxisID], "Enable major labels");
          else 
            View.SetCurrentContentWithEnable(this.m_CurrentController.ViewObject, _enableMinorLabels[_currentAxisID], "Enable minor labels");

          break;
      }
    }


    void SetLayerSecondaryChoice()
    {
      string[] names = new string[1]{"Common"};
      string name = names[0];
      this._primaryChoice = TabType.Unique;
      View.InitializeSecondaryChoice(names,name);
    }

    void SetHorzVertSecondaryChoice()
    {
      string[] names = new string[2]{"Y-Scale","X-Scale"};
      string name = names[_currentScale];
      this._primaryChoice = TabType.Scales;
      View.InitializeSecondaryChoice(names, name);
    }

    void SetEdgeSecondaryChoice()
    {
      string[] names = new string[_axisStyleInfoSortedByName.Count];
      string name = string.Empty;
      for (int i = 0; i < names.Length; i++)
      {
        names[i] = _axisStyleInfoSortedByName[i].NameOfAxisStyle;
        if (_axisStyleInfoSortedByName[i].Identifier == _currentAxisID)
          name = _axisStyleInfoSortedByName[i].NameOfAxisStyle;
      }
      this._primaryChoice = TabType.Styles;
      View.InitializeSecondaryChoice(names,name);
    }

    void SetPlaneSecondaryChoice()
    {
      /*
      string[] names = new string[_axisStyleInfoSortedByName.Count];
      string name = string.Empty;
      for (int i = 0; i < names.Length; i++)
      {
        names[i] = _axisStyleInfoSortedByName[i].NameOfAxisStyle;
        if (_axisStyleInfoSortedByName[i].Identifier == _currentAxisID)
          name = _axisStyleInfoSortedByName[i].NameOfAxisStyle;
      }
      this._primaryChoice = TabType.Styles;
      */
      this._primaryChoice = TabType.Planes;
      string[] names = new string[] { "Front" };
      string name = "Front";
      View.InitializeSecondaryChoice(names, name);
    }
  
    public void EhView_PageChanged(string firstChoice)
    {
      m_CurrentPage = firstChoice;
      SetCurrentTabController(true);
    }

    public void EhView_PageEnabledChanged( bool pageEnabled)
    {
      if(m_CurrentPage=="MajorLabels")
        this._enableMajorLabels[_currentAxisID] = pageEnabled;
      if(m_CurrentPage=="MinorLabels")
        this._enableMinorLabels[_currentAxisID] = pageEnabled;
    }

    public void EhView_SecondChoiceChanged(int index, string item)
    {
      if (_primaryChoice == TabType.Scales)
      {
        _currentScale = index;
      }
      else if (_primaryChoice == TabType.Styles)
      {
        _currentAxisID = _axisStyleInfoSortedByName[index].Identifier;
      }
      else if (_primaryChoice == TabType.Planes)
      {
        _currentPlaneID = _planeIdentifier[index];
      }

      SetCurrentTabController(false);
    }


    public static bool ShowDialog(System.Windows.Forms.Form parentWindow, XYPlotLayer layer)
    {
      return ShowDialog(parentWindow,layer,"Scale", new CSLineID(0,0) );
    }
    public static bool ShowDialog(System.Windows.Forms.Form parentWindow, XYPlotLayer layer, string currentPage)
    {
      return ShowDialog(parentWindow, layer, currentPage, new CSLineID(0,0));
    }

    public static bool ShowDialog(System.Windows.Forms.Form parentWindow, XYPlotLayer layer, string currentPage, CSLineID currentEdge)
    {
     
      LayerController ctrl = new LayerController(layer,currentPage,currentEdge);
      LayerControl view = new LayerControl();
      ctrl.View = view;

      DialogShellController dsc = new DialogShellController(
        new DialogShellView(view), ctrl);

      return dsc.ShowDialog(parentWindow);
    }


    #region IApplyController Members

    public bool Apply()
    {
      int i;

      if (null != this._coordinateController)
      {
        if (this._coordinateController.Apply())
          _layer.CoordinateSystem = (G2DCoordinateSystem)_coordinateController.ModelObject;
        else
          return false;
      }

      if(null!=this.m_LayerContentsController && !this.m_LayerContentsController.Apply())
        return false;

      if(null!=m_LayerPositionController && !this.m_LayerPositionController.Apply())
        return false;

      // do the apply for all controllers that are allocated so far
      for(i=0;i<2;i++)
      {
        if(null != m_AxisScaleController[i] && !m_AxisScaleController[i].Apply())
        {
          return false;
        }
      }


      foreach (CSLineID id in _TitleFormatController.Keys)
      {
        if(!_TitleFormatController[id].Apply())
        {
          return false;
        }
      }

      foreach (CSLineID id in _axisStyleIds.Keys)
      {
        if (this._enableMajorLabels[id])
        {
          if (_MajorLabelController.ContainsKey(id) && !_MajorLabelController[id].Apply())
          {
            return false;
          }
        }
        else
        {
          if(_layer.AxisStyles.Contains(id))
            _layer.AxisStyles.AxisStyle(id).ShowMajorLabels = false;
        }
      }

      foreach (CSLineID id in _axisStyleIds.Keys)
      {
        if (this._enableMinorLabels[id])
        {
          if (_MinorLabelController.ContainsKey(id)  && !_MinorLabelController[id].Apply())
          {
            return false;
          }
        }
        else
        {
          if (_layer.AxisStyles.Contains(id))
            this._layer.AxisStyles.AxisStyle(id).ShowMinorLabels = false;
        }
      }

      foreach(KeyValuePair<CSPlaneID,IMVCAController> gpair in _GridStyleController)
      {
        if (gpair.Value != null)
        {
          if (gpair.Value.Apply())
          {
            GridPlane gp = (GridPlane)gpair.Value.ModelObject;
            this._layer.GridPlanes[gpair.Key] = gp.IsUsed ? gp : null;
          }
          else
            return false;
        }
      }

      return true;
    }

    #endregion
  }
}