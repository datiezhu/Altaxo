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
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Altaxo.Graph;
namespace Altaxo.Gui.Graph.Shapes
{
	/// <summary>
	/// Interaction logic for TextGraphicControl.xaml
	/// </summary>
	public partial class TextGraphicControl : UserControl, ITextGraphicView
	{
		BackgroundControlsGlue _backgroundGlue;
		GdiToWpfBitmap _previewBitmap;

		public TextGraphicControl()
		{
			InitializeComponent();

			_positionSizeGlue.EdPositionX = m_edPosX;
			_positionSizeGlue.EdPositionY = m_edPosY;
			_positionSizeGlue.GuiShear = _edShear;
			_positionSizeGlue.CbRotation = m_cbRotation;
			_positionSizeGlue.GuiScaleX = _edScaleX;
			_positionSizeGlue.GuiScaleY = _edScaleY;



			_backgroundGlue = new BackgroundControlsGlue();
			_backgroundGlue.CbStyle = _cbBackgroundStyle;
			_backgroundGlue.CbBrush = _cbBackgroundBrush;
			_backgroundGlue.BackgroundStyleChanged += new EventHandler(EhBackgroundStyleChanged);

			_previewBitmap = new GdiToWpfBitmap(16, 16);
			m_pnPreview.Source = _previewBitmap.WpfBitmap;

		}

		void EhBackgroundStyleChanged(object sender, EventArgs e)
		{
			if (null != _controller)
				_controller.EhView_BackgroundStyleChanged();
		}

		private void EhLineSpacingChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if(null != _controller)
				_controller.EhView_LineSpacingChanged();
		}

		private void EhFontFamilyChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_FontFamilyChanged();
		}

		private void EhFontSize_Changed(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_FontSizeChanged();
		}

		private void EhTextBrush_SelectionChangeCommitted(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_TextFillBrushChanged();
		}

		private void EhNormal_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_NormalClick();
		}

		private void EhBold_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_BoldClick();
		}

		private void EhItalic_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_ItalicClick();
		}

		private void EhUnderline_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_UnderlineClick();
		}

		private void EhStrikeout_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_StrikeoutClick();
		}

		private void EhSupIndex_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_SupIndexClick();
		}

		private void EhSubIndex_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_SubIndexClick();
		}

		private void EhGreek_Click(object sender, RoutedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_GreekClick();
		}

		private void EhEditText_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (_controller != null)
				_controller.EhView_EditTextChanged();
		}

		private void EhPreview_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			int w = (int)e.NewSize.Width;
			int h = (int)e.NewSize.Height;

			if (w > 0 && h > 0 && (w != _previewBitmap.GdiBitmap.Width || h != _previewBitmap.GdiBitmap.Height))
			{
				_previewBitmap.Resize(w, h);
				m_pnPreview.Source = _previewBitmap.WpfBitmap;
				InvalidatePreviewPanel();
			}
		}

		#region  ITextGraphicView

		public void BeginUpdate()
		{

		}

		public void EndUpdate()
		{

		}

		ITextGraphicViewEventSink _controller;
		public ITextGraphicViewEventSink Controller { set { _controller = value; } }


		public Altaxo.Graph.Gdi.Background.IBackgroundStyle SelectedBackground
		{
			get
			{
				return _backgroundGlue.BackgroundStyle;
			}
			set
			{
				_backgroundGlue.BackgroundStyle = value;
			}
		}

		public double SelectedLineSpacing
		{
			get { return _guiLineSpacing.SelectedQuantityAsValueInSIUnits; }
			set { _guiLineSpacing.SelectedQuantityAsValueInSIUnits = value; }
		}

		public string EditText
		{
			get
			{
				return m_edText.Text;
			}
			set
			{
				m_edText.Text = value;
			}
		}

		public Altaxo.Graph.PointD2D SelectedPosition
		{
			get
			{
				return _positionSizeGlue.Position;
			}
			set
			{
				_positionSizeGlue.Position = value;
			}
		}

		public double SelectedRotation
		{
			get
			{
				return _positionSizeGlue.Rotation;
			}
			set
			{
				_positionSizeGlue.Rotation = value;
			}
		}

		public double SelectedShear
		{
			get
			{
				return _positionSizeGlue.Shear;
			}
			set
			{
				_positionSizeGlue.Shear = value;
			}
		}

		public double SelectedScaleX
		{
			get
			{
				return _positionSizeGlue.ScaleX;
			}
			set
			{
				_positionSizeGlue.ScaleX = value;
			}
		}

		public double SelectedScaleY
		{
			get
			{
				return _positionSizeGlue.ScaleY;
			}
			set
			{
				_positionSizeGlue.ScaleY = value;
			}
		}

		public System.Drawing.FontFamily SelectedFontFamily
		{
			get
			{
				return m_cbFonts.SelectedGdiFontFamily;
			}
			set
			{
				m_cbFonts.SelectedGdiFontFamily = value;
			}
		}

		public double SelectedFontSize
		{
			get
			{
				return m_cbFontSize.SelectedQuantityAsValueInPoints;
			}
			set
			{
				m_cbFontSize.SelectedQuantityAsValueInPoints = value;
			}
		}

		public Altaxo.Graph.Gdi.BrushX SelectedFontBrush
		{
			get
			{
				return m_cbFontColor.SelectedBrush;
			}
			set
			{
				m_cbFontColor.SelectedBrush = value;
			}
		}

		public XAnchorPositionType XAnchor
		{
			get
			{
				return _guiAnchoring.SelectedXAnchor;
			}
			set
			{
				_guiAnchoring.SelectedXAnchor = value;
			}
		}

		public YAnchorPositionType YAnchor
		{
			get
			{
				return _guiAnchoring.SelectedYAnchor;
			}
			set
			{
				_guiAnchoring.SelectedYAnchor = value;
			}
		}

		public void InsertBeforeAndAfterSelectedText(string insbefore, string insafter)
		{
			if (0 != this.m_edText.SelectionLength)
			{
				// insert \b( at beginning of selection and ) at the end of the selection
				int len = m_edText.Text.Length;
				int start = m_edText.SelectionStart;
				int end = m_edText.SelectionStart + m_edText.SelectionLength;
				m_edText.Text = m_edText.Text.Substring(0, start) + insbefore + m_edText.Text.Substring(start, end - start) + insafter + m_edText.Text.Substring(end, len - end);

				// now select the text plus the text before and after
				m_edText.Focus(); // necassary to show the selected area
				m_edText.Select(start, end - start + insbefore.Length + insafter.Length);
			}
		}

		public void RevertToNormal()
		{
			// remove a backslash x ( at the beginning and the closing brace at the end of the selection
			if (this.m_edText.SelectionLength >= 4)
			{
				int len = m_edText.Text.Length;
				int start = m_edText.SelectionStart;
				int end = m_edText.SelectionStart + m_edText.SelectionLength;

				if (m_edText.Text[start] == '\\' && m_edText.Text[start + 2] == '(' && m_edText.Text[end - 1] == ')')
				{
					m_edText.Text = m_edText.Text.Substring(0, start)
						+ m_edText.Text.Substring(start + 3, end - start - 4)
						+ m_edText.Text.Substring(end, len - end);

					// now select again the rest of the text
					m_edText.Focus(); // neccessary to show the selected area
					m_edText.Select(start, end - start - 4);
				}
			}
		}

		public void InvalidatePreviewPanel()
		{
			if (_controller != null)
			{
				_controller.EhView_PreviewPanelPaint(_previewBitmap.GdiGraphics);
				_previewBitmap.WpfBitmap.Invalidate();
			}
		}

		#endregion

		private void EhLoaded(object sender, RoutedEventArgs e)
		{
			this.m_edText.Focus();
		}

		private void EhMoreModifiersClicked(object sender, RoutedEventArgs e)
		{
			var menu = sender as MenuItem;
			if (null != menu && menu.Tag is string)
			{
				m_edText.AppendText((string)menu.Tag);
			}
		}

	




	}
}
