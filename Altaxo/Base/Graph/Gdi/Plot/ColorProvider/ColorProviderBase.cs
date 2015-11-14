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
using System.Drawing;
using System.Linq;
using System.Text;

namespace Altaxo.Graph.Gdi.Plot.ColorProvider
{
	using Altaxo.Graph;
	using Drawing;

	/// <summary>
	/// Abstract class to calculate a color out of a relative value that is normally
	/// between 0 and 1. Special colors are used here for values between 0, above 1, and for NaN.
	/// </summary>
	public abstract class ColorProviderBase
		:
		Main.SuspendableDocumentLeafNodeWithEventArgs,
		IColorProvider
	{
		/// <summary>The color used if the values are below the lower bound.</summary>
		protected NamedColor _colorBelow;

		/// <summary>The color used if the values are above the upper bound.</summary>
		protected NamedColor _colorAbove;

		/// <summary>The color used for invalid values (missing values).</summary>
		protected NamedColor _colorInvalid;

		/// <summary>Alpha channel for the generated colors. Range from 0 to 255.</summary>
		protected int _alphaChannel;

		/// <summary>
		/// Number of colors if colors should be stepwise shown. If zero, the color is shown continuously.
		/// </summary>
		protected int _colorSteps;

		/// <summary>Cached Gdi color for <see cref="_colorBelow"/>.</summary>
		protected Color _cachedGdiColorBelow;

		/// <summary>Cached Gdi color for <see cref="_colorAbove"/>.</summary>
		protected Color _cachedGdiColorAbove;

		/// <summary>Cached Gdi color for <see cref="_colorInvalid"/>.</summary>
		protected Color _cachedGdiColorInvalid;

		public ColorProviderBase()
		{
			this.ColorBelow = NamedColors.Black;
			this.ColorAbove = NamedColors.Snow;
			this.ColorInvalid = NamedColors.Transparent;
			_alphaChannel = 255;
			_colorSteps = 0;
		}

		public ColorProviderBase(ColorProviderBase from)
		{
			CopyFrom(from);
		}

		public virtual bool CopyFrom(object obj)
		{
			if (object.ReferenceEquals(this, obj))
				return true;

			bool result = false;
			var from = obj as ColorProviderBase;
			if (null != from)
			{
				this._colorBelow = from._colorBelow;
				this._cachedGdiColorBelow = from._cachedGdiColorBelow;

				this._colorAbove = from._colorAbove;
				this._cachedGdiColorAbove = from._cachedGdiColorAbove;

				this._colorInvalid = from._colorInvalid;
				this._cachedGdiColorInvalid = from._cachedGdiColorInvalid;

				this._alphaChannel = from._alphaChannel;
				this._colorSteps = from._colorSteps;
				result = true;
			}
			return result;
		}

		#region Serialization

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(ColorProviderBase), 0)]
		private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				ColorProviderBase s = (ColorProviderBase)obj;

				info.AddValue("ColorBelow", s._colorBelow);
				info.AddValue("ColorAbove", s._colorAbove);
				info.AddValue("ColorInvalid", s._colorInvalid);
				info.AddValue("Transparency", s.Transparency);
				info.AddValue("ColorSteps", s.ColorSteps);
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				ColorProviderBase s = (ColorProviderBase)o;

				s._colorBelow = (NamedColor)info.GetValue("ColorBelow", s);
				s._cachedGdiColorBelow = GdiColorHelper.ToGdi(s._colorBelow);

				s._colorAbove = (NamedColor)info.GetValue("ColorAbove", s);
				s._cachedGdiColorAbove = GdiColorHelper.ToGdi(s._colorAbove);

				s._colorInvalid = (NamedColor)info.GetValue("ColorInvalid", s);
				s._cachedGdiColorInvalid = GdiColorHelper.ToGdi(s._colorInvalid);

				s.Transparency = info.GetDouble("Transparency");
				s.ColorSteps = info.GetInt32("ColorSteps");

				return s;
			}
		}

		#endregion Serialization

		public abstract object Clone();

		/// <summary>
		/// Gets/sets the color used when the relative value is smaller than 0.
		/// </summary>
		public NamedColor ColorBelow
		{
			get { return _colorBelow; }
			set
			{
				var oldValue = _colorBelow;
				_colorBelow = value;
				_cachedGdiColorBelow = GdiColorHelper.ToGdi(_colorBelow);

				if (_colorBelow != oldValue)
				{
					EhSelfChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Get/sets the color used when the relative value is greater than 1.
		/// </summary>
		public NamedColor ColorAbove
		{
			get { return _colorAbove; }
			set
			{
				var oldValue = _colorAbove;

				_colorAbove = value;
				_cachedGdiColorAbove = GdiColorHelper.ToGdi(_colorAbove);

				if (_colorAbove != oldValue)
				{
					EhSelfChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets/sets the color when the relative value is NaN.
		/// </summary>
		public NamedColor ColorInvalid
		{
			get { return _colorInvalid; }
			set
			{
				var oldValue = _colorInvalid;
				_colorInvalid = value;
				_cachedGdiColorInvalid = GdiColorHelper.ToGdi(_colorInvalid);

				if (_colorInvalid != oldValue)
				{
					EhSelfChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Get/sets the transparency, which is a value between 0 (full opaque) and 1 (full transparent).
		/// </summary>
		public double Transparency
		{
			get { return 1 - _alphaChannel / 255.0; }
			set
			{
				var oldValue = _alphaChannel;

				if (0 <= value && value <= 1)
					_alphaChannel = (int)(255 * (1 - value));
				else
					_alphaChannel = 255;

				if (_alphaChannel != oldValue)
					EhSelfChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Number of discrete colors to be shown in a stepwise manner. If the value is zero, the colors are shown continuously.
		/// </summary>
		public int ColorSteps
		{
			get
			{
				return _colorSteps;
			}
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("ColorSteps is negative");

				var oldValue = _colorSteps;
				_colorSteps = value;

				if (value != oldValue)
					EhSelfChanged(EventArgs.Empty);
			}
		}

		public virtual Color GetOutOfBoundsColor(double relVal)
		{
			if (relVal < 0)
				return _cachedGdiColorBelow;
			else if (relVal > 1)
				return _cachedGdiColorAbove;
			else
				return _cachedGdiColorInvalid;
		}

		/// <summary>
		/// Calculates a color from the provided relative value.
		/// </summary>
		/// <param name="relVal">Value used for color calculation. Normally between 0 and 1.</param>
		/// <returns>A color associated with the relative value.</returns>
		public Color GetColor(double relVal)
		{
			if (relVal >= 0 && relVal <= 1)
			{
				if (_colorSteps > 0)
					relVal = relVal < 1 ? (Math.Floor(relVal * _colorSteps) + 0.5) / _colorSteps : (_colorSteps - 0.5) / _colorSteps;

				return GetColorFrom0To1Continuously(relVal);
			}
			else
			{
				return GetOutOfBoundsColor(relVal);
			}
		}

		/// <summary>
		/// Calculates a color from the provided relative value, that is guaranteed to be between 0 and 1
		/// </summary>
		/// <param name="relVal">Value used for color calculation. Guaranteed to be between 0 and 1.</param>
		/// <returns>A color associated with the relative value.</returns>
		protected abstract Color GetColorFrom0To1Continuously(double relVal);
	}
}