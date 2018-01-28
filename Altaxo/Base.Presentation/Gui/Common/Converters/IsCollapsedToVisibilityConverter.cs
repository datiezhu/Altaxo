﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Altaxo.Gui.Common.Converters
{
	/// <summary>
	/// Converter that converts a boolean 'IsCollapsed=true' to Visiblity.Collapsed, and 'IsCollapsed=false' to Visibility.Visible.
	/// </summary>
	/// <seealso cref="System.Windows.Data.IValueConverter" />
	public class IsCollapsedToVisibilityConverter : IValueConverter
	{
		public static IsCollapsedToVisibilityConverter Instance { get; private set; } = new IsCollapsedToVisibilityConverter();

		/// <inheritdoc/>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Boolean isCollapsed)
			{
				return isCollapsed ? Visibility.Collapsed : Visibility.Visible;
			}
			return Binding.DoNothing;
		}

		/// <inheritdoc/>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibility)
			{
				return visibility == Visibility.Collapsed ? true : false;
			}
			return Binding.DoNothing;
		}
	}
}