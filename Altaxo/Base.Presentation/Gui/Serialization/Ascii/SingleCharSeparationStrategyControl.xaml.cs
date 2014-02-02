﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2014 Dr. Dirk Lellinger
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

namespace Altaxo.Gui.Serialization.Ascii
{
	/// <summary>
	/// Interaction logic for SingleCharSeparationStrategyControl.xaml
	/// </summary>
	public partial class SingleCharSeparationStrategyControl : UserControl, ISingleCharSeparationStrategyView
	{
		public SingleCharSeparationStrategyControl()
		{
			InitializeComponent();
		}

		public char SeparatorChar
		{
			get
			{
				var txt = _guiSeparationChar.Text;
				if (txt.Length == 0)
					return ' ';
				else if (txt.Length == 1)
					return txt[0];
				else
				{
					var txtTrim = txt.Trim();
					if (txtTrim.Length == 1)
						return txtTrim[0];
					else
						return txt[0];
				}
			}
			set
			{
				_guiSeparationChar.Text = string.Empty + value;
			}
		}
	}
}