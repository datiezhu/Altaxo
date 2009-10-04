#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2007 Dr. Dirk Lellinger
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
using Altaxo.Serialization;
using Altaxo.Graph.Scales;
using Altaxo.Graph.Gdi;
using Altaxo.Gui;

namespace Altaxo.Gui.Graph
{
  #region Interfaces
  public interface IAxisLinkController : IMVCAController
  {
    /// <summary>
    /// Get/sets the view this controller controls.
    /// </summary>
    IAxisLinkView View { get; set; }

    /// <summary>
    /// Called if the type of the link is changed.
    /// </summary>
    /// <param name="linktype">The linktype. Valid arguments are "None", "Straight" and "Custom".</param>
    void EhView_LinkTypeChanged(bool isStraight);

    /// <summary>
    /// Called when the contents of OrgA is changed.
    /// </summary>
    /// <param name="orgA">Contents of OrgA.</param>
    /// <param name="bCancel">Normally false, this can be set to true if OrgA is not a valid entry.</param>
    void EhView_OrgAValidating(string orgA, ref bool bCancel);
    /// <summary>
    /// Called when the contents of OrgB is changed.
    /// </summary>
    /// <param name="orgB">Contents of OrgB.</param>
    /// <param name="bCancel">Normally false, this can be set to true if OrgB is not a valid entry.</param>
    void EhView_OrgBValidating(string orgB, ref bool bCancel);
    /// <summary>
    /// Called when the contents of EndA is changed.
    /// </summary>
    /// <param name="endA">Contents of EndA.</param>
    /// <param name="bCancel">Normally false, this can be set to true if EndA is not a valid entry.</param>
    void EhView_EndAValidating(string endA, ref bool bCancel);
    /// <summary>
    /// Called when the contents of EndB is changed.
    /// </summary>
    /// <param name="endB">Contents of EndB.</param>
    /// <param name="bCancel">Normally false, this can be set to true if EndB is not a valid entry.</param>
    void EhView_EndBValidating(string endB, ref bool bCancel);


  }

  public interface IAxisLinkView : IMVCView
  {

    /// <summary>
    /// Get/sets the controller of this view.
    /// </summary>
    IAxisLinkController Controller { get; set; }

    /// <summary>
    /// Initializes the type of the link.
    /// </summary>
    /// <param name="linktype"></param>
    void LinkType_Initialize(bool isStraight);

    /// <summary>
    /// Initializes the content of the OrgA edit box.
    /// </summary>
    void OrgA_Initialize(string text);

    /// <summary>
    /// Initializes the content of the OrgB edit box.
    /// </summary>
    void OrgB_Initialize(string text);

    /// <summary>
    /// Initializes the content of the EndA edit box.
    /// </summary>
    void EndA_Initialize(string text);

    /// <summary>
    /// Initializes the content of the EndB edit box.
    /// </summary>
    void EndB_Initialize(string text);


    /// <summary>
    /// Enables / Disables the edit boxes for the org and end values
    /// </summary>
    /// <param name="bEnable">True if the boxes are enabled for editing.</param>
    void Enable_OrgAndEnd_Boxes(bool bEnable);
  
  }
  #endregion

  /// <summary>
  /// Summary description for LinkAxisController.
  /// </summary>
	[ExpectedTypeOfView(typeof(IAxisLinkView))]
	[UserControllerForObject(typeof(LinkedScaleParameters))]
  public class AxisLinkController : IAxisLinkController
  {
    IAxisLinkView m_View;
    XYPlotLayer m_Layer;
    int _scaleIdx;

    bool m_LinkType;

		LinkedScaleParameters _doc;
		LinkedScaleParameters _tempDoc;

    double m_OrgA;
    double m_OrgB;
    double m_EndA;
    double m_EndB;


    public AxisLinkController(LinkedScaleParameters doc)
    {
			_doc = doc;
			_tempDoc = (LinkedScaleParameters)_doc;
			m_LinkType = _tempDoc.IsStraightLink;
      SetElements(true);
    }


    void SetElements(bool bInit)
    {
      if(null!=View)
      {
        View.LinkType_Initialize(m_LinkType);
        View.OrgA_Initialize(Serialization.GUIConversion.ToString(_tempDoc.OrgA));
				View.OrgB_Initialize(Serialization.GUIConversion.ToString(_tempDoc.OrgB));
				View.EndA_Initialize(Serialization.GUIConversion.ToString(_tempDoc.EndA));
				View.EndB_Initialize(Serialization.GUIConversion.ToString(_tempDoc.EndB));
      }
    }
    #region ILinkAxisController Members

    public IAxisLinkView View
    {
      get
      {
        return m_View;
      }
      set
      {
        if(null!=m_View)
          m_View.Controller = null;
        
        m_View = value;

        if(null!=m_View)
        {
          m_View.Controller = this;
          SetElements(false); // set only the view elements, dont't initialize the variables
        }
      }
    }

    public void EhView_LinkTypeChanged(bool isStraightLink)
    {
			m_LinkType = isStraightLink;

      if(null!=View)
        View.Enable_OrgAndEnd_Boxes(!isStraightLink);
    }

    public void EhView_OrgAValidating(string orgA, ref bool bCancel)
    {
			double val;
			if (NumberConversion.IsDouble(orgA, out val))
				_tempDoc.OrgA = val;
			else
				bCancel = true;
    }

    public void EhView_OrgBValidating(string orgB, ref bool bCancel)
    {
			double val;
			if (NumberConversion.IsDouble(orgB, out val))
				_tempDoc.OrgB = val;
			else
				bCancel = true;
		}

    public void EhView_EndAValidating(string endA, ref bool bCancel)
    {
			double val;
			if (NumberConversion.IsDouble(endA, out val))
				_tempDoc.EndA = val;
			else
				bCancel = true;
		}

    public void EhView_EndBValidating(string endB, ref bool bCancel)
    {
			double val;
			if (NumberConversion.IsDouble(endB, out val))
				_tempDoc.EndB = val;
			else
				bCancel = true;
		}

    #endregion

    #region IApplyController Members

    public bool Apply()
    {
			if (m_LinkType)
				_doc.SetToStraightLink();
			else
				_doc.CopyFrom(_tempDoc);
		
      return true;
    }

    #endregion

    #region IMVCController Members

    public object ViewObject
    {
      get
      {
        return View;
      }
      set
      {
        View = value as IAxisLinkView;
      }
    }

    public object ModelObject
    {
      get { return this.m_Layer; }
    }

    #endregion
  }
}
