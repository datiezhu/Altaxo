using System;

using Altaxo.Main.GUI;

namespace Altaxo.Calc.Regression.Nonlinear
{
  public interface IParameterSetElementView
  {
    void Initialize(string name, string value, bool vary, string variance);
    IParameterSetElementViewEventSink Controller { get; set; }
  }

  public interface IParameterSetElementViewEventSink
  {
    void EhView_ParameterValidating(string value, System.ComponentModel.CancelEventArgs e);
    void EhView_VarianceValidating(string value, System.ComponentModel.CancelEventArgs e);
    void EhView_VarySelectionChanged(bool value);
  }

  public interface IParameterSetElementController : Altaxo.Main.GUI.IMVCAController, IParameterSetElementViewEventSink , Altaxo.Gui.IRefreshable
  {
  }

	/// <summary>
	/// Summary description for ParameterSetElementControl.
	/// </summary>
	[UserControllerForObject(typeof(ParameterSetElement),100)]
	public class ParameterSetElementController : IParameterSetElementController 
	{
    ParameterSetElement _doc;
    ParameterSetElement _tempdoc;
    IParameterSetElementView _view;


		public ParameterSetElementController(ParameterSetElement doc)
		{
      _doc = doc;
      _tempdoc = new ParameterSetElement(doc);
    }

    protected void Initialize()
    {
      if(_view!=null)
      {
        _view.Initialize(_tempdoc.Name,
                          Altaxo.Serialization.GUIConversion.ToString(_tempdoc.Parameter),
                          _tempdoc.Vary,
                          Altaxo.Serialization.GUIConversion.ToString(_tempdoc.Variance)
          );
      }
    }

    /// <summary>
    /// Called when the doc has changed outside the controller. All changes that have been
    /// made manually are discarded, and the values of the changed document are shown on the view.
    /// </summary>
    public void Refresh()
    {
      // the doc has 
      _tempdoc = new ParameterSetElement(_doc);
      Initialize();
    }

    public void EhView_ParameterValidating(string value, System.ComponentModel.CancelEventArgs e)
    {
      if(Altaxo.Serialization.GUIConversion.IsDouble(value))
      {
        double t;
        Altaxo.Serialization.GUIConversion.IsDouble(value,out t);
        _tempdoc.Parameter = t;
      }
      else
      {
        e.Cancel = true;
      }
    }

    public void EhView_VarianceValidating(string value, System.ComponentModel.CancelEventArgs e)
    {
      if(Altaxo.Serialization.GUIConversion.IsDouble(value))
      {
        double t;
        Altaxo.Serialization.GUIConversion.IsDouble(value,out t);
        _tempdoc.Variance = t;
      }
      else
      {
        e.Cancel = true;
      }
    }

    public void EhView_VarySelectionChanged(bool value)
    {
      _tempdoc.Vary = value;
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

        _view = value as IParameterSetElementView;
        
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
      _doc.CopyFrom(_tempdoc);
      return true;
    }

    #endregion
  }
}
