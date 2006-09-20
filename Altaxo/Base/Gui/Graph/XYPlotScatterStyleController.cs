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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Altaxo.Collections;
using Altaxo.Main.GUI;
using Altaxo.Graph;

namespace Altaxo.Gui.Graph
{
  #region Interfaces
  /// <summary>
  /// This view interface is for showing the options of the XYXYPlotScatterStyle
  /// </summary>
  public interface IXYPlotScatterStyleView
  {
    // Get / sets the controller of this view
    IXYPlotScatterStyleViewEventSink Controller { get; set; }
  
    /// <summary>
    /// If activated, this causes the view to disable all gui elements if neither a line style nor a fill style is choosen.
    /// </summary>
    /// <param name="bActivate"></param>
    void SetEnableDisableMain(bool bActivate);    
    
    /// <summary>
    /// Initializes the plot style color combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections</param>
    /// <param name="sel">Current selection.</param>
    void InitializePlotStyleColor(Color sel);

    /// <summary>
    /// Initializes the symbol size combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections</param>
    /// <param name="sel">Current selection.</param>
    void InitializeSymbolSize(string[] arr , string sel);

    /// <summary>
    /// Initializes the independent symbol size check box.
    /// </summary>
    /// <param name="val">True when independent symbol size is choosen.</param>
    void InitializeIndependentSymbolSize(bool val);

    /// <summary>
    /// Initializes the symbol style combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections</param>
    /// <param name="sel">Current selection.</param>
    void InitializeSymbolStyle(string[] arr , string sel);

    /// <summary>
    /// Initializes the symbol shape combobox.
    /// </summary>
    /// <param name="arr">String array of possible selections</param>
    /// <param name="sel">Current selection.</param>
    void InitializeSymbolShape(string[] arr , string sel);


    /// <summary>
    /// Intitalizes the drop line checkboxes.
    /// </summary>
    /// <param name="names">List of names plus the information if they are selected or not.</param>
    void InitializeDropLineConditions(List<SelectableListNode> names);

    
    void InitializeIndependentColor(bool val);
   
    void InitializeSkipPoints(int val);
   

    #region Getter

    bool IndependentColor { get; }
    
    Color SymbolColor { get; }
    string SymbolShape {get; }
    bool   IndependentSymbolSize { get; }
    string SymbolStyle {get; }
    string SymbolSize  {get; }

    List<SelectableListNode> DropLines { get; }
    int SkipPoints { get; }


    #endregion // Getter
  }

  /// <summary>
  /// This is the controller interface of the XYPlotScatterStyleView
  /// </summary>
  public interface IXYPlotScatterStyleViewEventSink
  {
  }

  public interface IXYPlotScatterStyleController : Main.GUI.IMVCAController
  {
    /// <summary>
    /// If activated, this causes the view to disable all gui elements if neither a line style nor a fill style is choosen.
    /// </summary>
    /// <param name="bActivate"></param>
    void SetEnableDisableMain(bool bActivate);    
  }

  #endregion

  /// <summary>
  /// Summary description for XYPlotScatterStyleController.
  /// </summary>
  [UserControllerForObject(typeof(XYPlotScatterStyle))]
  public class XYPlotScatterStyleController : IXYPlotScatterStyleViewEventSink, IXYPlotScatterStyleController
  {
    XYPlotScatterStyle _doc;
    XYPlotScatterStyle _tempDoc;
    IXYPlotScatterStyleView _view;


    public XYPlotScatterStyleController(XYPlotScatterStyle doc)
    {

      _doc = doc;
      _tempDoc = (XYPlotScatterStyle)_doc.Clone();
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
        // now we have to set all dialog elements to the right values
        _view.InitializeIndependentColor(_tempDoc.IndependentColor);
        _view.InitializeIndependentSymbolSize(_tempDoc.IndependentSymbolSize);
       
        SetPlotStyleColor();
      


        // Scatter properties
        SetSymbolShape();
        SetSymbolStyle();
        SetSymbolSize();
        SetDropLineConditions();
        _view.SetEnableDisableMain(_ActivateEnableDisableMain);
        _view.InitializeSkipPoints(_tempDoc.SkipFrequency);
      }
    }


    public void SetSymbolSize()
    {
      string[] SymbolSizes = 
      { "0","1","3","5","8","12","15","18","24","30"};

      _view.InitializeSymbolSize(SymbolSizes, _tempDoc.SymbolSize.ToString());
    }

  
    public void SetSymbolStyle()
    {
      string [] names = System.Enum.GetNames(typeof(Altaxo.Graph.XYPlotScatterStyles.Style));
      _view.InitializeSymbolStyle(names,_tempDoc.Style.ToString());
    }

  
    public void SetSymbolShape()
    {
      string [] names = System.Enum.GetNames(typeof(Altaxo.Graph.XYPlotScatterStyles.Shape));

      Altaxo.Graph.XYPlotScatterStyles.Shape sh = Altaxo.Graph.XYPlotScatterStyles.Shape.NoSymbol;
      sh = _tempDoc.Shape;
      string name = sh.ToString();
      _view.InitializeSymbolShape(names,name);

    
    }
   

    


    public void SetDropLineConditions()
    {
      Altaxo.Graph.XYPlotLayer layer = Main.DocumentPath.GetRootNodeImplementing(_doc, typeof(Altaxo.Graph.XYPlotLayer)) as Altaxo.Graph.XYPlotLayer;

      List<SelectableListNode> names = new List<SelectableListNode>();

      foreach (A2DAxisStyleIdentifier id in layer.CoordinateSystem.GetJoinedAxisStyleIdentifier(layer.UsedAxisStyleIdentifier, _tempDoc.DropLine))
      {

        bool sel = _tempDoc.DropLine.Contains(id);
        A2DAxisStyleInformation info = layer.CoordinateSystem.GetAxisStyleInformation(id);
        names.Add(new SelectableListNode(info.NameOfAxisStyle, id, sel));
      }

      _view.InitializeDropLineConditions(names); 
    }




    public void SetPlotStyleColor()
    {
      _view.InitializePlotStyleColor(_tempDoc.Pen.Color);
    }


    #region IMVCController Members
    public object ViewObject
    {
      get { return _view; }
      set
      {
        if(_view!=null)
          _view.Controller = null;

        _view = value as IXYPlotScatterStyleView;
        
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
    #endregion
  
    #region IApplyController Members

    public bool Apply()
    {

      // don't trust user input, so all into a try statement
      try
      {
      

        // Symbol Color
        _doc.Color = _view.SymbolColor;

        _doc.IndependentColor = _view.IndependentColor;
      
        _doc.IndependentSymbolSize = _view.IndependentSymbolSize;

        // Symbol Shape
        string str = _view.SymbolShape;
        _doc.Shape = (Altaxo.Graph.XYPlotScatterStyles.Shape)Enum.Parse(typeof(Altaxo.Graph.XYPlotScatterStyles.Shape),str);

        // Symbol Style
        str = _view.SymbolStyle;
        _doc.Style = (Altaxo.Graph.XYPlotScatterStyles.Style)Enum.Parse(typeof(Altaxo.Graph.XYPlotScatterStyles.Style),str);

        // Symbol Size
        str = _view.SymbolSize;
        _doc.SymbolSize = System.Convert.ToSingle(str);

        // Drop line left
        _doc.DropLine.Clear();
        foreach (SelectableListNode node in _view.DropLines)
          _doc.DropLine.Add((A2DAxisStyleIdentifier)node.Item);

        // Skip points

        _doc.SkipFrequency = _view.SkipPoints;
       

      }
      catch(Exception ex)
      {
        Current.Gui.ErrorMessageBox("A problem occured: " + ex.Message);
        return false;
      }

      return true;
    }

    #endregion

   
  } // end of class XYPlotScatterStyleController
} // end of namespace
