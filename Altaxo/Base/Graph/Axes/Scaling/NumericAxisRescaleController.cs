using System;
using Altaxo.Graph.GUI;
using Altaxo.Serialization;
using Altaxo.Main.GUI;

namespace Altaxo.Graph.Axes.Scaling
{
	/// <summary>
	/// Summary description for NumericAxisRescaleController.
	/// </summary>
	[UserControllerForObject(typeof(NumericAxisRescaleConditions))]
	public class NumericAxisRescaleController 
    :
    IOrgEndSpanViewEventReceiver,
    IMVCAController
	{
    protected IOrgEndSpanView _view;
    protected NumericAxisRescaleConditions _doc;
    protected NumericalAxis _axis;
    
    protected double _org;
    protected double _end;
    protected double _span;

    protected BoundaryRescaling _orgRescaling;
    protected BoundaryRescaling _endRescaling;
    protected BoundaryRescaling _spanRescaling;

    protected bool _orgChanged;
    protected bool _endChanged;
    protected bool _spanChanged;

  


    public NumericAxisRescaleController(NumericAxisRescaleConditions doc, NumericalAxis ax)
    {
      _doc = doc;
      _axis = ax;
      
      SetElements(true);
        
      
    }

    protected virtual void SetElements(bool bInit)
    {
      if(bInit)
      {
        _orgRescaling =  _doc.OrgRescaling;
        _endRescaling =  _doc.EndRescaling;
        _spanRescaling = _doc.SpanRescaling;

        _org =  _doc.Org;
        _end =  _doc.End;
        _span = _doc.Span;

        if(_axis!=null)
        {
          if(_orgRescaling==BoundaryRescaling.Auto)
            _org = _axis.Org;
          if(_endRescaling==BoundaryRescaling.Auto)
            _end = _axis.End;
          if(_spanRescaling==BoundaryRescaling.Auto)
            _span = _axis.End - _axis.Org;
        }
      }

      if(null!=_view)
        InitView();
    }

    /// <summary>
    /// Has to match the indices of BoundaryRescaling
    /// </summary>
    static readonly string[] _choices = { "Auto", "Fixed", "<=", ">=" };

    protected virtual void InitView()
    {
      _view.SetLabel1("Org:");
      _view.SetLabel2("End:");
      _view.SetLabel3("Span:");

      _view.SetChoice1(_choices, (int)_orgRescaling);
      _view.SetChoice2(_choices, (int)_endRescaling);
      _view.SetChoice3(_choices, (int)_spanRescaling);


      _view.SetValue1(NumberConversion.ToString(_org));
      _view.SetValue2(NumberConversion.ToString(_end));
      _view.SetValue3(NumberConversion.ToString(_span));

      SetEnableState();
    }

    protected virtual void SetEnableState()
    {
      bool enableSpan = _spanRescaling!=BoundaryRescaling.Auto;
      _view.EnableChoice1(!enableSpan);
      _view.EnableChoice2(!enableSpan);
      _view.EnableChoice3(true);

      _view.EnableValue1(!enableSpan);
      _view.EnableValue2(!enableSpan);
      _view.EnableValue3(enableSpan);
    }

    #region IOrgEndSpanControlEventReceiver Members

    public void EhChoice1Changed(string txt, int selected)
    {
      _orgRescaling = (BoundaryRescaling)selected;
    }

    public void EhChoice2Changed(string txt, int selected)
    {
      _endRescaling = (BoundaryRescaling)selected;
    }

    public void EhChoice3Changed(string txt, int selected)
    {
      _spanRescaling = (BoundaryRescaling)selected;
      SetEnableState();
    }

    public virtual bool EhValue1Changed(string txt)
    {
      if(!NumberConversion.IsNumeric(txt))
        return true;

      NumberConversion.IsDouble(txt,out _org);
      _orgChanged = true;
      return false;
    }

    public virtual bool EhValue2Changed(string txt)
    {
      if(!NumberConversion.IsNumeric(txt))
        return true;

      NumberConversion.IsDouble(txt,out _end);
      _endChanged = true;
      return false;   
    }

    public virtual bool EhValue3Changed(string txt)
    {
      if(!NumberConversion.IsNumeric(txt))
        return true;

      NumberConversion.IsDouble(txt,out _span);
      return false;  
    }

    #endregion

    #region IMVCController Members

    public object ViewObject
    {
      get { return _view; }
      set
      {
        if(null!=_view && _view.Controller==this)
          _view.Controller=null;

        _view = (IOrgEndSpanView)value;

        if(_view!=null)
        {
          InitView();
          _view.Controller=this;
        }
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
      _doc.SetOrgEndSpan(_orgRescaling,_org,_endRescaling,_end,_spanRescaling,_span);

      if(null!=_axis)
      {
        // if the user changed org or end, he maybe want to set the scale temporarily to the chosen values
        if(_orgRescaling==BoundaryRescaling.Auto && _endRescaling==BoundaryRescaling.Auto && (_orgChanged || _endChanged))
          _axis.ProcessDataBounds(_org,true,_end,true);
        else
          _axis.ProcessDataBounds();
      }

      _orgChanged = _endChanged = false;

      SetElements(true);

      return true;
    }

    #endregion
  }
}
