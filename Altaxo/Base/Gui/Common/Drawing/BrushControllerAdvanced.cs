﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2011 Dr. Dirk Lellinger
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

#endregion Copyright

using System;
using System.Collections.Generic;
using System.Text;
using Altaxo.Drawing;
using Altaxo.Geometry;

namespace Altaxo.Gui.Common.Drawing
{
  public interface IBrushViewAdvanced
  {
    BrushType BrushType { get; set; }

    event Action BrushTypeChanged;

    NamedColor ForeColor { get; set; }

    event Action ForeColorChanged;

    void ForeColorEnable(bool enable);

    bool RestrictBrushColorToPlotColorsOnly { set; }

    NamedColor BackColor { get; set; }

    event Action BackColorChanged;

    void BackColorEnable(bool enable);

    bool ExchangeColors { get; set; }

    event Action ExchangeColorsChanged;

    void ExchangeColorsEnable(bool enable);

    System.Drawing.Drawing2D.WrapMode WrapMode { get; set; }

    event Action WrapModeChanged;

    void WrapModeEnable(bool enable);

    double GradientFocus { get; set; }

    event Action GradientFocusChanged;

    void GradientFocusEnable(bool enable);

    double GradientColorScale { get; set; }

    event Action GradientColorScaleChanged;

    void GradientColorScaleEnable(bool enable);

    double GradientAngle { get; set; }

    event Action GradientAngleChanged;

    void GradientAngleEnable(bool enable);

    double TextureOffsetX { get; set; }

    event Action TextureOffsetXChanged;

    void TextureOffsetXEnable(bool enable);

    double TextureOffsetY { get; set; }

    event Action TextureOffsetYChanged;

    void TextureOffsetYEnable(bool enable);

    void InitTextureImage(ImageProxy proxy, BrushType imageType);

    ImageProxy TextureImage { get; }

    event Action TextureImageChanged;

    void TextureImageEnable(bool enable);

    Main.IInstancePropertyView AdditionalPropertiesView { get; }

    ITextureScalingView TextureScalingView { get; }

    void TextureScalingViewEnable(bool enable);

    void UpdatePreview(BrushX brush);

    event Action PreviewPanelSizeChanged;
  }

  [UserControllerForObject(typeof(BrushX))]
  [ExpectedTypeOfView(typeof(IBrushViewAdvanced))]
  public class BrushControllerAdvanced : MVCANDControllerEditImmutableDocBase<BrushX, IBrushViewAdvanced>
  {
    private Main.InstancePropertyController _imageProxyController;

    private TextureScalingController _textureScalingController;
    private bool _restrictBrushColorToPlotColorsOnly;

    public override IEnumerable<ControllerAndSetNullMethod> GetSubControllers()
    {
      yield return new ControllerAndSetNullMethod(_imageProxyController, () => _imageProxyController = null);
      yield return new ControllerAndSetNullMethod(_textureScalingController, () => _textureScalingController = null);
    }

    protected override void Initialize(bool initData)
    {
      base.Initialize(initData);

      if (initData)
      {
        _imageProxyController = new Main.InstancePropertyController() { UseDocumentCopy = UseDocument.Directly };
        _imageProxyController.MadeDirty += EhAdditionalPropertiesChanged;
        _imageProxyController.InitializeDocument(_doc.TextureImage);

        _textureScalingController = new TextureScalingController() { UseDocumentCopy = UseDocument.Directly };
        _textureScalingController.MadeDirty += EhTextureScalingChanged;
        _textureScalingController.InitializeDocument(_doc.TextureScale);
        if (null != _doc.TextureImage)
          _textureScalingController.SourceTextureSize = GetSizeOfImageProxy(_doc.TextureImage);
      }

      if (null != _view)
      {
        _view.RestrictBrushColorToPlotColorsOnly = _restrictBrushColorToPlotColorsOnly;

        _view.BrushType = _doc.BrushType;
        InitializeViewElementsWhenBrushTypeChanged();

        _imageProxyController.ViewObject = _view.AdditionalPropertiesView;
        _textureScalingController.ViewObject = _view.TextureScalingView;

        EnableElementsInDependenceOnBrushType();
        _view.UpdatePreview(_doc);
      }
    }

    public override bool Apply(bool disposeController)
    {
      return ApplyEnd(true, disposeController);
    }

    protected override void AttachView()
    {
      _view.BrushTypeChanged += EhBrushTypeChanged;
      _view.ForeColorChanged += EhForeColorChanged;
      _view.BackColorChanged += EhBackColorChanged;
      _view.ExchangeColorsChanged += EhExchangeColorsChanged;
      _view.WrapModeChanged += EhWrapModeChanged;
      _view.GradientFocusChanged += EhGradientFocusChanged;
      _view.GradientColorScaleChanged += EhGradientScaleChanged;
      _view.GradientAngleChanged += EhGradientAngleChanged;
      _view.TextureOffsetXChanged += EhTextureOffsetXChanged;
      _view.TextureOffsetYChanged += EhTextureOffsetYChanged;
      _view.TextureImageChanged += EhTextureImageChanged;
      _view.PreviewPanelSizeChanged += EhPreviewPanelSizeChanged;

      base.AttachView();
    }

    protected override void DetachView()
    {
      _view.BrushTypeChanged -= EhBrushTypeChanged;
      _view.ForeColorChanged -= EhForeColorChanged;
      _view.BackColorChanged -= EhBackColorChanged;
      _view.ExchangeColorsChanged -= EhExchangeColorsChanged;
      _view.WrapModeChanged -= EhWrapModeChanged;
      _view.GradientFocusChanged -= EhGradientFocusChanged;
      _view.GradientColorScaleChanged -= EhGradientScaleChanged;
      _view.GradientAngleChanged -= EhGradientAngleChanged;
      _view.TextureOffsetXChanged -= EhTextureOffsetXChanged;
      _view.TextureOffsetYChanged -= EhTextureOffsetYChanged;
      _view.TextureImageChanged -= EhTextureImageChanged;
      _view.PreviewPanelSizeChanged -= EhPreviewPanelSizeChanged;

      base.DetachView();
    }

    public bool RestrictBrushColorToPlotColorsOnly
    {
      get
      {
        return _restrictBrushColorToPlotColorsOnly;
      }
      set
      {
        var oldValue = _restrictBrushColorToPlotColorsOnly;
        _restrictBrushColorToPlotColorsOnly = value;
        if (value != oldValue && null != _view)
        {
          _view.RestrictBrushColorToPlotColorsOnly = _restrictBrushColorToPlotColorsOnly;
        }
      }
    }

    private void InitializeViewElementsWhenBrushTypeChanged()
    {
      using (var suppressor = _suppressDirtyEvent.SuspendGetToken())
      {
        _view.ForeColor = _doc.Color;
        _view.BackColor = _doc.BackColor;
        _view.ExchangeColors = _doc.ExchangeColors;
        _view.WrapMode = _doc.WrapMode;
        _view.GradientFocus = _doc.GradientFocus;
        _view.GradientColorScale = _doc.GradientColorScale;
        _view.GradientAngle = _doc.GradientAngle;
        _view.TextureOffsetX = _doc.TextureOffsetX;
        _view.TextureOffsetY = _doc.TextureOffsetY;
        _view.InitTextureImage(_doc.TextureImage, _doc.BrushType);
      }
    }

    protected override void OnMadeDirty()
    {
      base.OnMadeDirty();

      if (!_suppressDirtyEvent.IsSuspended && null != _view)
        _view.UpdatePreview(_doc);
    }

    #region Other helper functions

    private void EnableElementsInDependenceOnBrushType()
    {
      bool foreColor = false, backColor = false, exchangeColor = false, wrapMode = false,
        gradientFocus = false, gradientColorScale = false, gradientAngle = false,
        textureScale = false, textureImage = false, textureOffsetX = false, textureOffsetY = false;

      switch (_doc.BrushType)
      {
        case BrushType.SolidBrush:
          foreColor = true;
          break;

        case BrushType.LinearGradientBrush:
        case BrushType.SigmaBellShapeLinearGradientBrush:
        case BrushType.TriangularShapeLinearGradientBrush:
          foreColor = true;
          backColor = true;
          exchangeColor = true;
          wrapMode = true;
          gradientAngle = true;
          if (_doc.BrushType != BrushType.LinearGradientBrush)
          {
            gradientFocus = true;
            gradientColorScale = true;
          }
          break;

        case BrushType.PathGradientBrush:
        case BrushType.SigmaBellShapePathGradientBrush:
        case BrushType.TriangularShapePathGradientBrush:
          foreColor = true;
          backColor = true;
          exchangeColor = true;
          wrapMode = true;
          textureOffsetX = true;
          textureOffsetY = true;
          if (_doc.BrushType != BrushType.PathGradientBrush)
          {
            gradientColorScale = true;
          }
          break;

        case BrushType.HatchBrush:
        case BrushType.SyntheticTextureBrush:
          foreColor = true;
          backColor = true;
          exchangeColor = true;
          wrapMode = true;
          gradientAngle = true;
          textureScale = true;
          textureImage = true;
          textureOffsetX = true;
          textureOffsetY = true;
          break;

        case BrushType.TextureBrush:
          wrapMode = true;
          gradientAngle = true;
          textureScale = true;
          textureImage = true;
          textureOffsetX = true;
          textureOffsetY = true;
          break;
      }
      _view.ForeColorEnable(foreColor);
      _view.BackColorEnable(backColor);
      _view.ExchangeColorsEnable(exchangeColor);
      _view.WrapModeEnable(wrapMode);
      _view.GradientFocusEnable(gradientFocus);
      _view.GradientColorScaleEnable(gradientColorScale);
      _view.GradientAngleEnable(gradientAngle);
      _view.TextureScalingViewEnable(textureScale);
      _view.TextureOffsetXEnable(textureOffsetX);
      _view.TextureOffsetYEnable(textureOffsetY);
      _view.TextureImageEnable(textureImage);
      //_view.AdditionalPropertiesView
    }

    private VectorD2D GetSizeOfImageProxy(ImageProxy proxy)
    {
      return proxy.Size;
    }

    #endregion Other helper functions

    #region Event handlers

    private void EhBrushTypeChanged()
    {
      _doc = _doc.WithBrushType(_view.BrushType);
      InitializeViewElementsWhenBrushTypeChanged();
      EnableElementsInDependenceOnBrushType();
      OnMadeDirty();
    }

    private void EhForeColorChanged()
    {
      _doc = _doc.WithColor(_view.ForeColor);
      OnMadeDirty();
    }

    private void EhBackColorChanged()
    {
      _doc = _doc.WithBackColor(_view.BackColor);
      OnMadeDirty();
    }

    private void EhExchangeColorsChanged()
    {
      _doc = _doc.WithExchangedColors(_view.ExchangeColors);
      _view.RestrictBrushColorToPlotColorsOnly = _restrictBrushColorToPlotColorsOnly;
      OnMadeDirty();
    }

    private void EhWrapModeChanged()
    {
      _doc = _doc.WithWrapMode(_view.WrapMode);
      OnMadeDirty();
    }

    private void EhGradientFocusChanged()
    {
      _doc = _doc.WithGradientFocus(_view.GradientFocus);
      OnMadeDirty();
    }

    private void EhGradientScaleChanged()
    {
      _doc = _doc.WithGradientColorScale(_view.GradientColorScale);
      OnMadeDirty();
    }

    private void EhGradientAngleChanged()
    {
      _doc = _doc.WithGradientAngle(_view.GradientAngle);
      OnMadeDirty();
    }

    private void EhTextureOffsetXChanged()
    {
      _doc = _doc.WithTextureOffsetX(_view.TextureOffsetX);
      OnMadeDirty();
    }

    private void EhTextureOffsetYChanged()
    {
      _doc = _doc.WithTextureOffsetY(_view.TextureOffsetY);
      OnMadeDirty();
    }

    private void EhTextureScaleChanged()
    {
      //_doc.TextureScale = (float)_view.TextureScale;
      OnMadeDirty();
    }

    private void EhTextureImageChanged()
    {
      var oldTexture = _doc.TextureImage;
      var newTexture = _view.TextureImage;
      if (newTexture is Altaxo.Main.ICopyFrom)
        ((Altaxo.Main.ICopyFrom)newTexture).CopyFrom(oldTexture); // Try to keep the settings from the old texture

      _doc = _doc.WithTextureImage(newTexture);
      _imageProxyController.InitializeDocument(_doc.TextureImage);
      if (null != _doc.TextureImage)
        _textureScalingController.SourceTextureSize = GetSizeOfImageProxy(_doc.TextureImage);
      OnMadeDirty();
    }

    private void EhAdditionalPropertiesChanged(IMVCANController ctrl)
    {
      _doc = _doc.WithTextureImage((ImageProxy)_imageProxyController.ProvisionalModelObject);
      OnMadeDirty();
    }

    private void EhTextureScalingChanged(IMVCANController ctrl)
    {
      _doc = _doc.WithTextureScale((TextureScaling)_textureScalingController.ProvisionalModelObject);
      OnMadeDirty();
    }

    private void EhPreviewPanelSizeChanged()
    {
      _view.UpdatePreview(_doc);
    }

    #endregion Event handlers
  }
}
