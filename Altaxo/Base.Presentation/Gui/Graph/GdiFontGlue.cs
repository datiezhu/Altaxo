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
using System.ComponentModel;
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


using Altaxo.Gui.Common.Drawing;
using sd = System.Drawing;

namespace Altaxo.Gui.Graph
{
	public class GdiFontGlue
	{
		double _fontSize = 12;
		sd.FontFamily _fontFamily = sd.FontFamily.GenericSansSerif;
		sd.FontStyle _fontStyle = sd.FontStyle.Regular;

		public double FontSize
		{
			get { return _fontSize; }
			set
			{
				var oldValue = _fontSize;
				_fontSize = value;
				if (_guiFontSize != null && oldValue != value)
					_guiFontSize.SelectedQuantityInPoints = value;
			}
		}


		public sd.FontFamily FontFamily
		{
			get { return _fontFamily; }
			set
			{
				var oldValue = _fontFamily;
				_fontFamily = value;
				if (null != _guiFontFamily && oldValue != value)
					_guiFontFamily.SelectedGdiFontFamily = value;
			}
		}


		public sd.FontStyle FontStyle
		{
			get { return _fontStyle; }
			set
			{
				var oldValue = _fontStyle;
				_fontStyle = value;
				if (null != _guiFontStyle && oldValue != value)
					_guiFontStyle.SelectedFontStyle = value;
			}
		}


		public event EventHandler SelectedFontChanged;

		public sd.Font SelectedFont
		{
			get
			{
				return new sd.Font(_fontFamily, (float)_fontSize, _fontStyle);
			}
			set
			{
				FontSize = value.Size;
				FontStyle = value.Style;
				FontFamily = value.FontFamily;
			}
		}


		FontSizeComboBox _guiFontSize;
		public FontSizeComboBox GuiFontSize
		{
			get { return _guiFontSize; }
			set
			{
				if (null != _guiFontSize)
					_guiFontSize.SelectedQuantityChanged -= _guiFontStyle_SelectedFontSizeChanged;

				_guiFontSize = value;
				_guiFontSize.SelectedQuantityInPoints = _fontSize;

				if (null != _guiFontSize)
					_guiFontSize.SelectedQuantityChanged += _guiFontStyle_SelectedFontSizeChanged;
			}
		}


		FontFamilyComboBox _guiFontFamily;
		public FontFamilyComboBox GuiFontFamily
		{
			get { return _guiFontFamily; }
			set
			{
				if (null != _guiFontFamily)
					_guiFontFamily.SelectedFontFamilyChanged -= _guiFontStyle_SelectedFontFamilyChanged;

				_guiFontFamily = value;
				_guiFontFamily.SelectedGdiFontFamily = _fontFamily;


				if (null != _guiFontFamily)
					_guiFontFamily.SelectedFontFamilyChanged += _guiFontStyle_SelectedFontFamilyChanged;
			}
		}


		FontStyleComboBox _guiFontStyle;
		public FontStyleComboBox GuiFontStyle
		{
			get { return _guiFontStyle; }
			set
			{
				if (null != _guiFontStyle)
					_guiFontStyle.SelectedFontStyleChanged -= _guiFontStyle_SelectedFontStyleChanged;

				_guiFontStyle = value;
				_guiFontStyle.SelectedFontStyle = _fontStyle;

				if (null != _guiFontStyle)
					_guiFontStyle.SelectedFontStyleChanged += _guiFontStyle_SelectedFontStyleChanged;
			}
		}

		void _guiFontStyle_SelectedFontSizeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var oldFontSize = _fontSize;
			_fontSize = _guiFontSize.SelectedQuantityInPoints;

			if (oldFontSize != _fontSize)
				OnFontChanged();
		}

		void _guiFontStyle_SelectedFontStyleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var oldFontStyle = _fontStyle;
			_fontStyle = _guiFontStyle.SelectedFontStyle;
			if (oldFontStyle != _fontStyle)
				OnFontChanged();
		}

		void _guiFontStyle_SelectedFontFamilyChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var oldFontFamily = _fontFamily;
			_fontFamily = _guiFontFamily.SelectedGdiFontFamily;
			if (oldFontFamily != _fontFamily)
				OnFontChanged();
		}

		protected virtual void OnFontChanged()
		{
			if (null != SelectedFontChanged)
				SelectedFontChanged(this, EventArgs.Empty);
		}

	}
}