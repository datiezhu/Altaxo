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

using Altaxo.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Altaxo.Gui.Common.Drawing
{
	/// <summary>
	/// Interaction logic for BrushSimpleConditionalControl.xaml
	/// </summary>
	public partial class BrushSimpleConditionalControl : UserControl
	{
		public BrushSimpleConditionalControl()
		{
			InitializeComponent();
		}

		public Altaxo.Graph.Gdi.BrushX SelectedBrush
		{
			get
			{
				if (_chkEnableBrush.IsChecked == true)
				{
					return _cbBrush.SelectedBrush;
				}
				else
				{
					var brush = _cbBrush.SelectedBrush;
					brush.Color = NamedColors.Transparent;
					return brush;
				}
			}
			set
			{
				_cbBrush.SelectedBrush = value;
				_chkEnableBrush.IsChecked = value.IsVisible;
			}
		}

		private void EhEnableFill_Checked(object sender, RoutedEventArgs e)
		{
			var brush = _cbBrush.SelectedBrush.Clone();
			if (!brush.IsVisible)
			{
				brush.Color = NamedColors.AliceBlue;
				_cbBrush.SelectedBrush = brush;
			}
		}
	}
}