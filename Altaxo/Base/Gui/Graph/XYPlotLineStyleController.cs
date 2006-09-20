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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Altaxo.Collections;
using Altaxo.Graph;
using Altaxo.Main.GUI;
using Altaxo.Gui.Common.Drawing;

namespace Altaxo.Gui.Graph
{
  #region Interfaces
  /// <summary>
  /// This view interface is for showing the options of the XYXYPlotLineStyle
  /// </summary>
  public interface IXYPlotLineStyleView
  {
    // Get / sets the controller of this view
    IXYPlotLineStyleViewEventSink Controller { get; set; }
    
    
    /// <summary>
    /// If activated, this causes the view to disable all gui elements if neither a line style nor a fill style is choosen.
    /// </summary>
    /// <param name="bActivate"></param>
    void SetEnableDisableMain(bool bActivate);    


    void InitializeIndependentColor(bool val);
 
    void InitializePen(IColorTypeThicknessPenController controller);

    /// <summary>
    /// Initializes the LineSymbolGap check box.
    /// </summary>
    /// <param name="bGap">True if a gap between symbols and line should be shown.</param>
    void InitializeLineSymbolGapCondition(bool bGap);


    /// <summary>
    /// Initializes the Line connection combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections.</param>
    /// <param name="sel">Current selection.</param>
    void InitializeLineConnect(string[] arr , string sel);

   

    /// <summary>
    /// Initializes the fill check box.
    /// </summary>
    /// <param name="bFill">True if the plot should be filled.</param>
    void InitializeFillCondition(bool bFill);

    /// <summary>
    /// Initializes the fill direction combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections</param>
    /// <param name="sel">Current selection.</param>
    void InitializeFillDirection(List<ListNode> list, int sel);

    /// <summary>
    /// Initializes the fill color combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections</param>
    /// <param name="sel">Current selection.</param>
    void InitializeFillColor(BrushHolder sel);
  

    #region Getter

    bool LineSymbolGap { get; }
    bool IndependentColor { get; }
   
 
    string LineConnect { get; }
  
    bool   LineFillArea { get; }
    ListNode LineFillDirection { get; }
    BrushHolder LineFillColor {get; }

    

    #endregion // Getter
  }

  /// <summary>
  /// This is the controller interface of the XYPlotLineStyleView
  /// </summary>
  public interface IXYPlotLineStyleViewEventSink
  {
    

  }

  public interface IXYPlotLineStyleController : Main.GUI.IMVCAController
  {
    /// <summary>
    /// If activated, this causes the view to disable all gui elements if neither a line style nor a fill style is choosen.
    /// </summary>
    /// <param name="bActivate"></param>
    void SetEnableDisableMain(bool bActivate);    
  }


  #endregion

  /// <summary>
  /// Summary description for XYPlotLineStyleController.
  /// </summary>
  [UserControllerForObject(typeof(XYPlotLineStyle))]
  public class XYPlotLineStyleController : IXYPlotLineStyleViewEventSink, IXYPlotLineStyleController
  {
    IXYPlotLineStyleView _view;
    XYPlotLineStyle _doc;
    XYPlotLineStyle _tempDoc;
    IColorTypeThicknessPenController _penController;

    public XYPlotLineStyleController(XYPlotLineStyle doc)
    {
      _doc = doc;
      _tempDoc = (XYPlotLineStyle)_doc.Clone();
      _penController = new ColorTypeThicknessPenController(_tempDoc.PenHolder);
    }


   
    public object ViewObject
    {
      get { return _view; }
      set
      {
        if(_view!=null)
          _view.Controller = null;

        _view = value as IXYPlotLineStyleView;
        
        Initialize();

        if(_view!=null)
          _view.Controller = this;
      }
    }
    public object ModelObject
    {
      get
      {
        return _doc;
      }
    }

    public static string [] GetPlotColorNames()
    {
      string[] arr = new string[1+PlotColors.Colors.Count];

      arr[0] = "Custom";

      int i=1;
      foreach(PlotColor c in PlotColors.Colors)
      {
        arr[i++] = c.Name;
      }

      return arr;
    }


    bool _ActivateEnableDisableMain = false;
    /// <summary>
    /// If activated, this causes the view to disable all gui elements if neither a line style nor a fill style is choosen.
    /// </summary>
    /// <param name="bActivate"></param>
    public void SetEnableDisableMain(bool bActivate)
    {
      _ActivateEnableDisableMain = bActivate;
      if(null!=_view)
        _view.SetEnableDisableMain(bActivate);
    }

    void Initialize()
    {
      if(_view!=null)
      {
        _view.InitializeIndependentColor(_tempDoc.IndependentColor);
       

        // now we have to set all dialog elements to the right values
        _view.InitializePen(_penController);
        SetLineSymbolGapCondition();

        // Line properties
        SetLineConnect();
        SetFillCondition();
        SetFillDirection();
        SetFillColor();
        _view.SetEnableDisableMain(_ActivateEnableDisableMain);
      }
    }


    public void SetLineSymbolGapCondition()
    {
      _view.InitializeLineSymbolGapCondition( _tempDoc.LineSymbolGap );
    }


    public void SetLineConnect()
    {

      string [] names = System.Enum.GetNames(typeof(Altaxo.Graph.XYPlotLineStyles.ConnectionStyle));
    
      _view.InitializeLineConnect(names,_tempDoc.Connection.ToString());
    }

   


  
    

    public void SetFillCondition()
    {
      _view.InitializeFillCondition( _tempDoc.FillArea );
    }

    public void SetFillDirection()
    {
      Altaxo.Graph.IPlotArea layer = Main.DocumentPath.GetRootNodeImplementing(_doc, typeof(Altaxo.Graph.IPlotArea)) as Altaxo.Graph.IPlotArea;

      List<ListNode> names = new List<ListNode>();

      int idx = -1;
      if (layer != null)
      {
        foreach (A2DAxisStyleInformation info in layer.CoordinateSystem.AxisStyles)
          names.Add(new ListNode(info.NameOfAxisStyle, info));

        idx = layer.CoordinateSystem.IndexOfAxisStyle(_tempDoc.FillDirection);


        if (idx < 0 && _tempDoc.FillDirection != null)
        {
          A2DAxisStyleInformation info = layer.CoordinateSystem.GetAxisStyleInformation(_tempDoc.FillDirection);
          names.Add(new ListNode(info.NameOfAxisStyle, info));
          idx = names.Count - 1;
        }

      }
      _view.InitializeFillDirection(names,Math.Max(idx,0)); // _tempDoc.FillDirection.ToString());
    }

    public void SetFillColor()
    {
      _view.InitializeFillColor(_tempDoc.FillBrush);
    }


 


    #region IApplyController Members

    public bool Apply()
    {

      // don't trust user input, so all into a try statement
      try
      {

        // Symbol Gap
        _doc.LineSymbolGap = _view.LineSymbolGap;

        // Pen
        _penController.Apply();
        _doc.PenHolder.CopyFrom( _tempDoc.PenHolder );

       
        // Line Connect
        _doc.Connection = (Altaxo.Graph.XYPlotLineStyles.ConnectionStyle)Enum.Parse(typeof(Altaxo.Graph.XYPlotLineStyles.ConnectionStyle),_view.LineConnect);


        // Fill Area
        _doc.FillArea = _view.LineFillArea;
        // Line fill direction
        A2DAxisStyleIdentifier id = null;
        if(_doc.FillArea && null!=_view.LineFillDirection)
          id = ((A2DAxisStyleInformation)_view.LineFillDirection.Item).Identifier;

        _doc.FillDirection = id;
        // Line fill color
        _doc.FillBrush = _view.LineFillColor;

      }
      catch(Exception ex)
      {
        Current.Gui.ErrorMessageBox("A problem occured. " + ex.Message);
        return false;
      }

      return true;
    }

    #endregion

 
  } // end of class XYPlotLineStyleController
} // end of namespace
