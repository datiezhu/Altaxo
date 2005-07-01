using System;
using System.Text;
using System.Collections;

namespace Altaxo.Calc.Regression.Nonlinear
{
  public interface IFitFunctionSelectionView
  {
    IFitFunctionSelectionViewEventSink Controller { get; set; }
    void InitializeFitFunctionList(DictionaryEntry[] entries, System.Type currentSelection);
  }

  public interface IFitFunctionSelectionViewEventSink
  {
    void EhView_SelectionChanged(object selectedtag);
  }


  public class FitFunctionSelectionController : IFitFunctionSelectionViewEventSink, Main.GUI.IMVCAController
  {
    IFitFunction _doc;
    IFitFunction _tempdoc;
    IFitFunctionSelectionView _view;

    public FitFunctionSelectionController(IFitFunction doc)
    {
      _doc = doc;
      _tempdoc = doc;
      Initialize();
    }

    public void Initialize()
    {
      if(_view!=null)
      {
        DictionaryEntry[] entries = Altaxo.Main.Services.ReflectionService.GetAttributeInstancesAndClassTypes(typeof(FitFunctionAttribute));
        _view.InitializeFitFunctionList(entries,_tempdoc==null ? null : _tempdoc.GetType());
      }
    }

    public void EhView_SelectionChanged(object selectedtag)
    {
      if(selectedtag is System.Type)
      {
        _tempdoc = System.Activator.CreateInstance((System.Type)selectedtag) as IFitFunction;
      }

    }

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

        _view = value as IFitFunctionSelectionView;
        
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
      
      if(_tempdoc != null)
      {
        _doc = _tempdoc;
        return true;
      }

      return false;
    }

    #endregion
  }
}