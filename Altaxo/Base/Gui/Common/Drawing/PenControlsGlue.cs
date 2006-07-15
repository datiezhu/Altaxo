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
using System.Text;
using System.ComponentModel;

using Altaxo.Graph;

namespace Altaxo.Gui.Common.Drawing
{
  public class PenControlsGlue : Component
  {
    private bool _userChangedStartCapSize;
    private bool _userChangedEndCapSize;

    public PenControlsGlue()
    {
      this.BrushGlue = new BrushControlsGlue();
    }

    #region Pen

    PenHolder _pen;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public PenHolder Pen
    {
      get
      {
        //  if (_pen != null)
        //    GetGuiElementValues(_pen);
        return _pen;
      }
      set
      {
        _pen = value;

        BrushGlue = _brushGlue;
        CbLineThickness = _cbThickness;
        CbDashStyle = _cbDashStyle;
        CbDashCap = _cbDashCap;
        CbStartCap = _cbStartCap;
        CbStartCapSize = _cbStartCapSize;
        CbEndCap = _cbEndCap;
        CbEndCapSize = _cbEndCapSize;
        CbLineJoin = _cbLineJoin;
        CbMiterLimit = _cbMiterLimit;
      }
    }

    public event EventHandler PenChanged;
    protected virtual void OnPenChanged()
    {
      if (PenChanged != null)
        PenChanged(this, EventArgs.Empty);
    }

    #endregion

    #region BrushGlue

    System.Windows.Forms.ToolStripItem _customPenDialogItem;
    BrushControlsGlue _brushGlue;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public BrushControlsGlue BrushGlue
    {
      get { return _brushGlue; }
      private set
      {
        if (_brushGlue != null)
        {
          _brushGlue.BrushChanged -= EhBrushChanged;

          _brushGlue.RemoveContextMenuItem(_customPenDialogItem);
        }

        _brushGlue = value;
        if (_pen != null && _brushGlue != null)
          _brushGlue.Brush = _pen.BrushHolder;


        if (_brushGlue != null)
        {
          _brushGlue.BrushChanged += EhBrushChanged;

          if (_customPenDialogItem == null)
          {
            _customPenDialogItem = new System.Windows.Forms.ToolStripMenuItem();
            _customPenDialogItem.Text = "Custom Pen ...";
            _customPenDialogItem.Click += EhShowCustomPenDialog;
          }
          _brushGlue.InsertContextMenuItem(_customPenDialogItem);
        }

      }
    }

    void EhBrushChanged(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.BrushHolder = _brushGlue.Brush;
        OnPenChanged();
      }
    }

    #endregion

    #region Dash

    DashStyleComboBox _cbDashStyle;
    public DashStyleComboBox CbDashStyle
    {
      get { return _cbDashStyle; }
      set
      {
        if (_cbDashStyle != null)
          _cbDashStyle.SelectionChangeCommitted -= EhDashStyle_SelectionChangeCommitted;

        _cbDashStyle = value;
        if (_pen != null && _cbDashStyle != null)
          _cbDashStyle.DashStyleEx = _pen.DashStyleEx;

        if (_cbDashStyle != null)
          _cbDashStyle.SelectionChangeCommitted += EhDashStyle_SelectionChangeCommitted;
      }
    }

    void EhDashStyle_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.DashStyleEx = _cbDashStyle.DashStyleEx;
        OnPenChanged();
      }
    }

    DashCapComboBox _cbDashCap;
    public DashCapComboBox CbDashCap
    {
      get { return _cbDashCap; }
      set
      {
        if (_cbDashCap != null)
          _cbDashCap.SelectionChangeCommitted -= EhDashCap_SelectionChangeCommitted;

        _cbDashCap = value;
        if (_pen != null && _cbDashCap != null)
          _cbDashCap.DashCap = _pen.DashCap;

        if (_cbDashCap != null)
          _cbDashCap.SelectionChangeCommitted += EhDashCap_SelectionChangeCommitted;
      }
    }

    void EhDashCap_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.DashCap = _cbDashCap.DashCap;
        OnPenChanged();
      }
    }

    #endregion

    #region Width

    LineThicknessComboBox _cbThickness;
    public LineThicknessComboBox CbLineThickness
    {
      get { return _cbThickness; }
      set
      {
        if (_cbThickness != null)
        {
          _cbThickness.SelectionChangeCommitted -= EhThickness_SelectionChangeCommitted;
          _cbThickness.TextUpdate -= EhThickness_TextChanged;
        }

        _cbThickness = value;
        if (_pen != null && _cbThickness != null)
          _cbThickness.Thickness = _pen.Width;

        if (_cbThickness != null)
        {
          _cbThickness.SelectionChangeCommitted += EhThickness_SelectionChangeCommitted;
          _cbThickness.TextUpdate += EhThickness_TextChanged;
        }
      }
    }

    void EhThickness_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.Width = _cbThickness.Thickness;
        OnPenChanged();
      }
    }
    void EhThickness_TextChanged(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.Width = _cbThickness.Thickness;
        OnPenChanged();
      }
    }

    #endregion

    #region StartCap

    LineCapComboBox _cbStartCap;

    public LineCapComboBox CbStartCap
    {
      get { return _cbStartCap; }
      set
      {
        if (_cbStartCap != null)
          _cbStartCap.SelectionChangeCommitted -= EhStartCap_SelectionChangeCommitted;

        _cbStartCap = value;
        if (_pen != null && _cbStartCap != null)
          _cbStartCap.LineCapEx = _pen.StartCap;

        if (_cbStartCap != null)
          _cbStartCap.SelectionChangeCommitted += EhStartCap_SelectionChangeCommitted;
      }
    }

    void EhStartCap_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        LineCapEx cap = _cbStartCap.LineCapEx;
        if (_userChangedStartCapSize && _cbStartCapSize != null)
          cap.Size = _cbStartCapSize.Thickness;

        _pen.StartCap = cap;

        if (_cbStartCapSize != null)
          _cbStartCapSize.Thickness = cap.Size;

        OnPenChanged();
      }

    }

    LineCapSizeComboBox _cbStartCapSize;

    public LineCapSizeComboBox CbStartCapSize
    {
      get { return _cbStartCapSize; }
      set
      {
        if (_cbStartCapSize != null)
        {

          _cbStartCapSize.SelectionChangeCommitted -= EhStartCapSize_SelectionChangeCommitted;
          _cbStartCapSize.TextUpdate -= EhStartCapSize_TextChanged;
        }
        _cbStartCapSize = value;
        if (_pen != null && _cbStartCapSize != null)
          _cbStartCapSize.Thickness = _pen.StartCap.Size;

        if (_cbStartCapSize != null)
        {
          _cbStartCapSize.SelectionChangeCommitted += EhStartCapSize_SelectionChangeCommitted;
          _cbStartCapSize.TextUpdate += EhStartCapSize_TextChanged;
        }
      }
    }

    void EhStartCapSize_SelectionChangeCommitted(object sender, EventArgs e)
    {
      _userChangedStartCapSize = true;

      if (_pen != null)
      {
        LineCapEx cap = _pen.StartCap;
        cap.Size = _cbStartCapSize.Thickness;
        _pen.StartCap = cap;

        OnPenChanged();
      }

    }
    void EhStartCapSize_TextChanged(object sender, EventArgs e)
    {
      EhStartCapSize_SelectionChangeCommitted(sender, e);
    }

    #endregion

    #region EndCap

    LineCapComboBox _cbEndCap;

    public LineCapComboBox CbEndCap
    {
      get { return _cbEndCap; }
      set
      {
        if (_cbEndCap != null)
          _cbEndCap.SelectionChangeCommitted -= EhEndCap_SelectionChangeCommitted;

        _cbEndCap = value;
        if (_pen != null && _cbEndCap != null)
          _cbEndCap.LineCapEx = _pen.EndCap;

        if (_cbEndCap != null)
          _cbEndCap.SelectionChangeCommitted += EhEndCap_SelectionChangeCommitted;
      }
    }



    void EhEndCap_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        LineCapEx cap = _cbEndCap.LineCapEx;
        if (_userChangedEndCapSize && _cbEndCapSize != null)
          cap.Size = _cbEndCapSize.Thickness;

        _pen.EndCap = cap;

        if (_cbEndCapSize != null)
          _cbEndCapSize.Thickness = cap.Size;

        OnPenChanged();
      }


    }

    LineCapSizeComboBox _cbEndCapSize;

    public LineCapSizeComboBox CbEndCapSize
    {
      get { return _cbEndCapSize; }
      set
      {
        if (_cbEndCapSize != null)
        {
          _cbEndCapSize.SelectionChangeCommitted -= EhEndCapSize_SelectionChangeCommitted;
          _cbEndCapSize.TextUpdate -= EhEndCapSize_TextChanged;
        }

        _cbEndCapSize = value;
        if (_pen != null && _cbEndCapSize != null)
          _cbEndCapSize.Thickness = _pen.EndCap.Size;

        if (_cbEndCapSize != null)
        {
          _cbEndCapSize.SelectionChangeCommitted += EhEndCapSize_SelectionChangeCommitted;
          _cbEndCapSize.TextUpdate += EhEndCapSize_TextChanged;
        }
      }
    }

    void EhEndCapSize_SelectionChangeCommitted(object sender, EventArgs e)
    {
      _userChangedEndCapSize = true;

      if (_pen != null)
      {
        LineCapEx cap = _pen.EndCap;
        cap.Size = _cbEndCapSize.Thickness;
        _pen.EndCap = cap;

        OnPenChanged();
      }

    }
    void EhEndCapSize_TextChanged(object sender, EventArgs e)
    {
      EhEndCapSize_SelectionChangeCommitted(sender, e);
    }


    #endregion

    #region LineJoin

    LineJoinComboBox _cbLineJoin;

    public LineJoinComboBox CbLineJoin
    {
      get { return _cbLineJoin; }
      set
      {
        if (_cbLineJoin != null)
          _cbLineJoin.SelectionChangeCommitted -= EhLineJoin_SelectionChangeCommitted;


        _cbLineJoin = value;
        if (_pen != null && _cbLineJoin != null)
          _cbLineJoin.LineJoin = _pen.LineJoin;

        if (_cbLineJoin != null)
          _cbLineJoin.SelectionChangeCommitted += EhLineJoin_SelectionChangeCommitted;

      }
    }

    void EhLineJoin_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.LineJoin = _cbLineJoin.LineJoin;
        OnPenChanged();
      }
    }

    #endregion

    #region Miter
    MiterLimitComboBox _cbMiterLimit;

    public MiterLimitComboBox CbMiterLimit
    {
      get { return _cbMiterLimit; }
      set
      {
        if (_cbMiterLimit != null)
        {
          _cbMiterLimit.SelectionChangeCommitted -= EhMiterLimit_SelectionChangeCommitted;
          _cbMiterLimit.TextUpdate -= EhMiterLimit_TextChanged;
        }

        _cbMiterLimit = value;
        if (_pen != null && _cbMiterLimit != null)
          _cbMiterLimit.MiterValue = _pen.MiterLimit;

        if (_cbLineJoin != null)
        {
          _cbMiterLimit.SelectionChangeCommitted += EhMiterLimit_SelectionChangeCommitted;
          _cbMiterLimit.TextUpdate += EhMiterLimit_TextChanged;
        }
      }
    }

    void EhMiterLimit_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.MiterLimit = _cbMiterLimit.MiterValue;
        OnPenChanged();
      }
    }
    void EhMiterLimit_TextChanged(object sender, EventArgs e)
    {
      if (_pen != null)
      {
        _pen.MiterLimit = _cbMiterLimit.MiterValue;
        OnPenChanged();
      }
    }


    #endregion

    #region Dialog

    void EhShowCustomPenDialog(object sender, EventArgs e)
    {
      PenAllPropertiesControl ctrl = new PenAllPropertiesControl();
      ctrl.Pen = this.Pen;
      if (Current.Gui.ShowDialog(ctrl, "Pen properties"))
      {
        this.Pen = ctrl.Pen;
      }
    }

    #endregion

    #region Mirrored Brush Properties

    public ColorType ColorType
    {
      get
      {
        return _brushGlue.ColorType;
      }
      set
      {
        _brushGlue.ColorType = value;
      }
    }

    public BrushTypeComboBox CbBrushType
    {
      get
      {
        return _brushGlue.CbBrushType;
      }
      set
      {
        _brushGlue.CbBrushType = value;
      }
    }

    public HatchStyleComboBox CbBrushHatchStyle
    {
      get
      {
        return _brushGlue.CbHatchStyle;
      }
      set
      {
        _brushGlue.CbHatchStyle = value;
      }
    }

    public ColorComboBox CbBrushColor
    {
      get
      {
        return _brushGlue.CbColor1;
      }
      set
      {
        _brushGlue.CbColor1 = value;
      }
    }
    public ColorComboBox CbBrushColor2
    {
      get
      {
        return _brushGlue.CbColor2;
      }
      set
      {
        _brushGlue.CbColor2 = value;
      }
    }

    #endregion
  }
}