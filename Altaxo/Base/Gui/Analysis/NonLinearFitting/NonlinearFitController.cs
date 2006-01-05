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
using Altaxo.Main.GUI;
using Altaxo.Graph;
using Altaxo.Scripting;
using Altaxo.Gui.Scripting;
using Altaxo.Calc.Regression.Nonlinear;


namespace Altaxo.Gui.Analysis.NonLinearFitting
{
  #region interfaces
  public interface INonlinearFitView
  {
    INonlinearFitViewEventSink Controller { get; set; }
    void SetParameterControl(object control);
    void SetSelectFunctionControl(object control);
    void SetFitEnsembleControl(object control);
    void SetChiSquare(double chiSquare);
  }

  public interface INonlinearFitViewEventSink
  {
    void EhView_DoFit();
    void EhView_DoSimplex();
    void EhView_EvaluateChiSqr();
    void EhView_SelectFitFunction();
    void EhView_NewFitFunction();
  }

  #endregion
  /// <summary>
  /// Summary description for NonlinearFitController.
  /// </summary>
  [UserControllerForObject(typeof(NonlinearFitDocument))]
  public class NonlinearFitController : INonlinearFitViewEventSink, Main.GUI.IMVCAController
  {
    NonlinearFitDocument _doc;
    INonlinearFitView _view;

    Main.GUI.IMVCAController _parameterController;
    FitFunctionSelectionController _funcselController;
    IFitEnsembleController _fitEnsembleController;

    double _chiSquare;

    public NonlinearFitController(NonlinearFitDocument doc)
    {
      _doc = doc;
      _parameterController = (Main.GUI.IMVCAController)Current.Gui.GetControllerAndControl(new object[]{_doc.CurrentParameters},typeof(Main.GUI.IMVCAController));
      _fitEnsembleController = (IFitEnsembleController)Current.Gui.GetControllerAndControl(new object[]{_doc.FitEnsemble},typeof(IFitEnsembleController));

      _funcselController = new FitFunctionSelectionController(_doc.FitEnsemble.Count==0 ? null : _doc.FitEnsemble[0].FitFunction);
      Current.Gui.GetControl(_funcselController);
    
      _doc.FitEnsemble.Changed += new EventHandler(EhFitEnsemble_Changed);
    }

    public void Initialize()
    {
      if(_view!=null)
      {
        _view.SetParameterControl(_parameterController.ViewObject);
        _view.SetSelectFunctionControl(_funcselController.ViewObject);
        _view.SetFitEnsembleControl(_fitEnsembleController.ViewObject);
      }
    }

    private void EhFitEnsemble_Changed(object sender, EventArgs e)
    {
    }

    #region  INonlinearFitViewEventSink

    public void EhView_DoSimplex()
    {
      if(true==this._parameterController.Apply())
      {
        //        _doc.FitEnsemble.InitializeParametersFromParameterSet(_doc.CurrentParameters);

        LevMarAdapter fitAdapter = new LevMarAdapter(_doc.FitEnsemble,_doc.CurrentParameters);

        fitAdapter.DoSimplexMinimization();

        this._chiSquare = fitAdapter.ResultingChiSquare;

        fitAdapter.CopyParametersBackTo(_doc.CurrentParameters);

        //_doc.FitEnsemble.InitializeParametersFromParameterSet(_doc.CurrentParameters);
        //_doc.FitEnsemble.DistributeParameters();
        
        OnAfterFittingStep();
      }
      else
      {
        Current.Gui.ErrorMessageBox("Some of your parameter input is not valid!");
      }
    }


    public void EhView_DoFit()
    {
      if(true==this._parameterController.Apply())
      {
        //        _doc.FitEnsemble.InitializeParametersFromParameterSet(_doc.CurrentParameters);

        LevMarAdapter fitAdapter = new LevMarAdapter(_doc.FitEnsemble,_doc.CurrentParameters);
  
        fitAdapter.Fit();

        this._chiSquare = fitAdapter.ResultingChiSquare;

        fitAdapter.CopyParametersBackTo(_doc.CurrentParameters);

        //_doc.FitEnsemble.InitializeParametersFromParameterSet(_doc.CurrentParameters);
        //_doc.FitEnsemble.DistributeParameters();
        
        OnAfterFittingStep();
      }
      else
      {
        Current.Gui.ErrorMessageBox("Some of your parameter input is not valid!");
      }
    }

    public     void EhView_EvaluateChiSqr()
    {
      if(true==this._parameterController.Apply())
      {
        LevMarAdapter fitAdapter = new LevMarAdapter(_doc.FitEnsemble,_doc.CurrentParameters);
        this._chiSquare = fitAdapter.EvaluateChiSquare();
        //_doc.FitEnsemble.InitializeParametersFromParameterSet(_doc.CurrentParameters);
        //_doc.FitEnsemble.DistributeParameters();
        OnAfterFittingStep();
      }
      else
      {
        Current.Gui.ErrorMessageBox("Some of your parameter input is not valid!");
      }
    }


    enum SelectionChoice { SelectAsOnly, SelectAsAdditional};
    SelectionChoice _lastSelectionChoice=SelectionChoice.SelectAsOnly;
    void Select(IFitFunction func)
    {
      bool changed = false;
      if(_doc.FitEnsemble.Count==0)
      {
        FitElement newele = new FitElement();
        newele.FitFunction = func;
        _doc.FitEnsemble.Add(newele);
        changed=true;
      }
      else if(_doc.FitEnsemble.Count>0 && _doc.FitEnsemble[_doc.FitEnsemble.Count-1].FitFunction==null)
      {
        _doc.FitEnsemble[_doc.FitEnsemble.Count-1].FitFunction = func;
        changed = true;
      }
      else // Count>0, and there is already a fit function, we
      { // have to ask the user whether he wants to discard the old functions or keep them

        System.Enum selchoice = _lastSelectionChoice;
        if(Current.Gui.ShowDialog(ref selchoice,"As only or as additional?"))
        {
          _lastSelectionChoice = (SelectionChoice)selchoice;
          if(_lastSelectionChoice==SelectionChoice.SelectAsAdditional)
          {
            FitElement newele = new FitElement();
            newele.FitFunction = func;
            _doc.FitEnsemble.Add(newele);
            changed=true;
          }
          else // select as only
          {
            _doc.FitEnsemble[0].FitFunction = func;
            for(int i= _doc.FitEnsemble.Count-1;i>=1;--i)
              _doc.FitEnsemble.RemoveAt(i);

            changed = true;
          }
        }
      }

      if(changed)
      {
        // _doc.FitEnsemble.InitializeParameterSetFromEnsembleParameters(_doc.CurrentParameters);
        
        this._fitEnsembleController.Refresh();

      }
    }


    public void EhView_SelectFitFunction()
    {
      

      if(_funcselController.Apply())
      {
        Select((IFitFunction)_funcselController.ModelObject);       
      }

    }

    public void EhView_NewFitFunction()
    {
      FitFunctionScript script = new FitFunctionScript();

      object scriptAsObject = script;
      if(Current.Gui.ShowDialog(ref scriptAsObject,"Create fit function"))
      {
        script = (FitFunctionScript)scriptAsObject;

        Current.Gui.ShowDialog(new FitFunctionNameAndCategoryController(script), "Name your script");

        Current.Project.FitFunctionScripts.Add(script);

        Select(script);

        _funcselController.Refresh();
        
      }
    }

    System.Collections.ArrayList _functionPlotItems = new System.Collections.ArrayList();
    public void OnAfterFittingStep()
    {
      
     
      if(_view!=null)
        _view.SetChiSquare(this._chiSquare);

 

      if(_doc.FitContext is Altaxo.Graph.GUI.GraphController)
      {
        // for every dependent variable in the FitEnsemble, create a function graph
        Altaxo.Graph.GUI.GraphController graph = _doc.FitContext as Altaxo.Graph.GUI.GraphController;

        int funcNumber=0;
        for(int i=0;i<_doc.FitEnsemble.Count;i++)
        {
          FitElement fitEle = _doc.FitEnsemble[i];

          for(int k=0;k<fitEle.NumberOfDependentVariables;k++, funcNumber++)
          {
            if(funcNumber<_functionPlotItems.Count && _functionPlotItems[funcNumber]!=null)
            {
              Altaxo.Graph.XYFunctionPlotItem plotItem = (Altaxo.Graph.XYFunctionPlotItem)_functionPlotItems[funcNumber];
              FitFunctionToScalarFunctionDDWrapper wrapper = (FitFunctionToScalarFunctionDDWrapper)plotItem.Data.Function;
              wrapper.Initialize(fitEle.FitFunction,k,0,_doc.GetParametersForFitElement(i));
            }
            else
            {
              FitFunctionToScalarFunctionDDWrapper wrapper = new FitFunctionToScalarFunctionDDWrapper(fitEle.FitFunction,k, _doc.GetParametersForFitElement(i));
              Altaxo.Graph.XYFunctionPlotData plotdata = new Altaxo.Graph.XYFunctionPlotData(wrapper);
              Altaxo.Graph.XYFunctionPlotItem plotItem = new Altaxo.Graph.XYFunctionPlotItem(plotdata,new Altaxo.Graph.XYPlotStyleCollection(LineScatterPlotStyleKind.Line));
              graph.ActiveLayer.PlotItems.Add(plotItem);
              _functionPlotItems.Add(plotItem);
            }
          }
        }

        // if there are more elements in _functionPlotItems, remove them from the graph
        for(int i=_functionPlotItems.Count-1;i>=funcNumber;--i)
        {
          if(_functionPlotItems[i]!=null)
          {
            graph.ActiveLayer.PlotItems.Remove((Altaxo.Graph.PlotItem)_functionPlotItems[i]);
            _functionPlotItems.RemoveAt(i);

          }
        }
        graph.RefreshGraph();
      }
    }
    #endregion

    #region IMVCController Members

    public object ViewObject
    {
      get
      {
        
        return _view;
      }
      set
      {
        if(_view!=null)
          _view.Controller = null;

        _view = value as INonlinearFitView;
        
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
      return true;
    }

    #endregion

   
  }
}
