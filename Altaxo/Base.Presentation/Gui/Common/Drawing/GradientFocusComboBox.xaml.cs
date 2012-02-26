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
using System.Reflection;
using System.Diagnostics;

namespace Altaxo.Gui.Common.Drawing
{
	public partial class GradientFocusComboBox : DimensionfulQuantityImageComboBox
	{
		static Dictionary<double, ImageSource> _cachedImages = new Dictionary<double, ImageSource>();

		static readonly double[] _initialValues = new double[] { 0.0, 0.25, 0.5, 0.75, 1.0 };

		public GradientFocusComboBox()
		{
			UnitEnvironment = RelationEnvironment.Instance;
			_converter.ValidationAfterSuccessfulConversion = EhValidateQuantity;

			InitializeComponent();


			foreach (var e in _initialValues)
				Items.Add(new ImageComboBoxItem(this, new Units.DimensionfulQuantity(e, Units.Dimensionless.Unity.Instance).AsQuantityIn(UnitEnvironment.DefaultUnit)));

			_img.Source = GetImage(SelectedQuantityInSIUnits);
		}

		protected override void ImplantImage(double width, double height)
		{
			base.ImplantImage(width, height);
			var h = _img.Height;
			const double hMargin = 6;
			_img.Margin = new Thickness(_img.Margin.Left, _img.Margin.Top + hMargin, _img.Margin.Right, _img.Margin.Bottom + hMargin);
			_img.Height = h - 2 * hMargin;
		}


		private static ValidationResult EhValidateQuantity(Units.DimensionfulQuantity quantity)
		{
			string error = null;
			double val = quantity.AsValueInSIUnits;
			if (double.IsInfinity(val))
				error = "Value must not be infinity";
			else if (double.IsNaN(val))
				error = "Value must be a valid number";
			else if (val < 0)
				error = "Value must be a non-negative number";
			else if (val > 1)
				error = "Value must be less or equal than 1";

			return error == null ? ValidationResult.ValidResult : new ValidationResult(false, error);
		}

		protected override void OnSelectedQuantityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			base.OnSelectedQuantityChanged(obj, args);

			if (null != _img)
			{
				var val = SelectedQuantityInSIUnits;
				_img.Source = GetImage(val);
			}
		}


		public override ImageSource GetItemImage(object item)
		{
			double val = ((Units.DimensionfulQuantity)item).AsValueInSIUnits;
			ImageSource result;
			if (!_cachedImages.TryGetValue(val, out result))
				_cachedImages.Add(val, result = GetImage(val));
			return result;
		}


		public override string GetItemText(object item)
		{
			return (string)_converter.Convert(item, typeof(string), null, System.Globalization.CultureInfo.CurrentUICulture);
		}

		public static ImageSource GetImage(double val)
		{
			const double height = 1;
			const double width = 2;
			const double lineWidth = 0;
			const double lwHalf = lineWidth / 2;

			// draws a transparent outline to fix the borders
			var geometryDrawing = new GeometryDrawing();
			geometryDrawing.Geometry = new RectangleGeometry(new Rect(-lineWidth, -lineWidth, width + lineWidth, height + lineWidth));
			geometryDrawing.Pen = new Pen(Brushes.Transparent, 0);

			var gradStops = new GradientStopCollection();
			gradStops.Add(new GradientStop(Colors.Black, 0));
			gradStops.Add(new GradientStop(Colors.White, val));
			gradStops.Add(new GradientStop(Colors.Black, 1));

			geometryDrawing.Brush = new LinearGradientBrush(gradStops, 0);
			var geometryImage = new DrawingImage(geometryDrawing);

			// Freeze the DrawingImage for performance benefits.
			geometryImage.Freeze();
			return geometryImage;
		}
	}
}
