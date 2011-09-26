#region Copyright
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Altaxo.Graph.Gdi
{
	[Serializable]
	public enum BrushType
	{
		SolidBrush,
		HatchBrush,
		TextureBrush,
		LinearGradientBrush,
		PathGradientBrush
	};

	[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase", "Altaxo.Graph.BrushType", 0)]
	[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(BrushType), 1)]
	public class BrushTypeXmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
	{
		public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
		{
			info.SetNodeContent(obj.ToString());
		}
		public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
		{

			string val = info.GetNodeContent();
			return System.Enum.Parse(typeof(BrushType), val, true);
		}
	}

	[Serializable]
	public enum LinearGradientShape
	{
		Linear,
		Triangular,
		SigmaBell
	}



	[Serializable]
	public enum LinearGradientModeEx
	{
		BackwardDiagonal,
		ForwardDiagonal,
		Horizontal,
		Vertical,
		RevBackwardDiagonal,
		RevForwardDiagonal,
		RevHorizontal,
		RevVertical
	}

	/// <summary>
	/// Holds all information neccessary to create a brush
	/// of any kind without allocating resources, so this class
	/// can be made serializable.
	/// </summary>
	[Serializable]
	public class BrushX : System.ICloneable, System.IDisposable, Main.IChangedEventSource
	{

		protected BrushType _brushType; // Type of the brush
		protected NamedColor _foreColor; // Color of the brush
		protected NamedColor _backColor; // Backcolor of brush, f.i.f. HatchStyle brushes
		protected HatchStyle _hatchStyle; // Attention: is not serializable!
		protected ImageProxy _textureImage; // f�r Texturebrush
		protected WrapMode _wrapMode; // f�r TextureBrush und LinearGradientBrush
		protected RectangleF _brushBoundingRectangle;
		protected float _focus;
		protected float _scale;
		protected bool _exchangeColors;
		protected LinearGradientMode _gradientMode;
		protected LinearGradientShape _gradientShape;

		[field: NonSerialized]
		public event System.EventHandler Changed;

		[NonSerialized]
		protected Brush _cachedBrush;      // this is the cached brush object

		/// <summary>Cached pixel size of the texture. Important for repeateable texture brushes only.</summary>
		[NonSerialized]
		protected int _cachedTexturePixelSize = 8;

		#region "Serialization"



		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase", "Altaxo.Graph.BrushHolder", 0)]
		class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				BrushX s = (BrushX)obj;
				info.AddValue("Type", s._brushType);
				switch (s._brushType)
				{
					case BrushType.SolidBrush:
						info.AddValue("ForeColor", s._foreColor);
						break;
					case BrushType.HatchBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddEnum("HatchStyle", s._hatchStyle);
						break;
				} // end of switch
			}
			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{

				BrushX s = null != o ? (BrushX)o : new BrushX(NamedColor.Black);

				s._brushType = (BrushType)info.GetValue("Type", s);
				switch (s._brushType)
				{
					case BrushType.SolidBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						break;
					case BrushType.HatchBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._hatchStyle = (HatchStyle)info.GetEnum("HatchStyle", typeof(HatchStyle));
						break;
				}
				return s;
			}
		}


		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase", "Altaxo.Graph.BrushHolder", 1)]
		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(BrushX), 2)]
		class XmlSerializationSurrogate1 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				BrushX s = (BrushX)obj;
				info.AddValue("Type", s._brushType);
				switch (s._brushType)
				{
					case BrushType.SolidBrush:
						info.AddValue("ForeColor", s._foreColor);
						break;
					case BrushType.HatchBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddEnum("HatchStyle", s._hatchStyle);
						break;
					case BrushType.LinearGradientBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddEnum("WrapMode", s._wrapMode);
						info.AddEnum("GradientMode", s._gradientMode);
						info.AddEnum("GradientShape", s._gradientShape);
						info.AddValue("Scale", s._scale);
						info.AddValue("Focus", s._focus);
						break;
					case BrushType.PathGradientBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddEnum("WrapMode", s._wrapMode);
						break;
					case BrushType.TextureBrush:
						info.AddValue("Texture", s._textureImage);
						info.AddEnum("WrapMode", s._wrapMode);
						info.AddValue("Scale", s._scale);
						break;
				} // end of switch
			}
			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{

				BrushX s = null != o ? (BrushX)o : new BrushX(NamedColor.Black);

				s._brushType = (BrushType)info.GetValue("Type", s);
				switch (s._brushType)
				{
					case BrushType.SolidBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						break;
					case BrushType.HatchBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._hatchStyle = (HatchStyle)info.GetEnum("HatchStyle", typeof(HatchStyle));
						break;
					case BrushType.LinearGradientBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._wrapMode = (WrapMode)info.GetEnum("WrapMode", typeof(WrapMode));
						LinearGradientModeEx gm = (LinearGradientModeEx)info.GetEnum("GradientMode", typeof(LinearGradientModeEx));
						string gmname = Enum.GetName(typeof(LinearGradientModeEx), gm);
						if (gmname.StartsWith("Rev"))
						{
							s._exchangeColors = true;
							s._gradientMode = (LinearGradientMode)Enum.Parse(typeof(LinearGradientMode), gmname.Substring(3));
						}
						else
						{
							s._gradientMode = (LinearGradientMode)Enum.Parse(typeof(LinearGradientMode), gmname);
						}

						s._gradientShape = (LinearGradientShape)info.GetEnum("GradientShape", typeof(LinearGradientShape));
						s._scale = info.GetSingle("Scale");
						s._focus = info.GetSingle("Focus");
						break;
					case BrushType.PathGradientBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._wrapMode = (WrapMode)info.GetEnum("WrapMode", typeof(WrapMode));
						break;
					case BrushType.TextureBrush:
						s.TextureImage = (ImageProxy)info.GetValue("Texture", s);
						s._wrapMode = (WrapMode)info.GetEnum("WrapMode", typeof(WrapMode));
						s._scale = info.GetSingle("Scale");
						break;
				}
				return s;
			}
		}

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(BrushX), 3)]
		class XmlSerializationSurrogate3 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				BrushX s = (BrushX)obj;
				info.AddValue("Type", s._brushType);
				switch (s._brushType)
				{
					case BrushType.SolidBrush:
						info.AddValue("ForeColor", s._foreColor);
						break;
					case BrushType.HatchBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddValue("ExchangeColors", s._exchangeColors);
						info.AddEnum("HatchStyle", s._hatchStyle);
						break;
					case BrushType.LinearGradientBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddValue("ExchangeColors", s._exchangeColors);
						info.AddEnum("WrapMode", s._wrapMode);
						info.AddEnum("GradientMode", s._gradientMode);
						info.AddEnum("GradientShape", s._gradientShape);
						info.AddValue("Scale", s._scale);
						info.AddValue("Focus", s._focus);
						break;
					case BrushType.PathGradientBrush:
						info.AddValue("ForeColor", s._foreColor);
						info.AddValue("BackColor", s._backColor);
						info.AddValue("ExchangeColors", s._exchangeColors);
						info.AddEnum("WrapMode", s._wrapMode);
						break;
					case BrushType.TextureBrush:
						info.AddValue("Texture", s._textureImage);
						info.AddEnum("WrapMode", s._wrapMode);
						info.AddValue("Scale", s._scale);
						break;
				} // end of switch
			}
			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{

				BrushX s = null != o ? (BrushX)o : new BrushX(NamedColor.Black);

				s._brushType = (BrushType)info.GetValue("Type", s);
				switch (s._brushType)
				{
					case BrushType.SolidBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						break;
					case BrushType.HatchBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._exchangeColors = info.GetBoolean("ExchangeColors");
						s._hatchStyle = (HatchStyle)info.GetEnum("HatchStyle", typeof(HatchStyle));
						break;
					case BrushType.LinearGradientBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._exchangeColors = info.GetBoolean("ExchangeColors");
						s._wrapMode = (WrapMode)info.GetEnum("WrapMode", typeof(WrapMode));
						s._gradientMode = (LinearGradientMode)info.GetEnum("GradientMode", typeof(LinearGradientMode));
						s._gradientShape = (LinearGradientShape)info.GetEnum("GradientShape", typeof(LinearGradientShape));
						s._scale = info.GetSingle("Scale");
						s._focus = info.GetSingle("Focus");
						break;
					case BrushType.PathGradientBrush:
						s._foreColor = (NamedColor)info.GetValue("ForeColor", s);
						s._backColor = (NamedColor)info.GetValue("BackColor", s);
						s._exchangeColors = info.GetBoolean("ExchangeColors");
						s._wrapMode = (WrapMode)info.GetEnum("WrapMode", typeof(WrapMode));
						break;
					case BrushType.TextureBrush:
						s.TextureImage = (ImageProxy)info.GetValue("Texture", s);
						s._wrapMode = (WrapMode)info.GetEnum("WrapMode", typeof(WrapMode));
						s._scale = info.GetSingle("Scale");
						break;
				}
				return s;
			}
		}



		#endregion

		public BrushX(BrushX from)
		{
			_brushType = from._brushType; // Type of the brush
			_cachedBrush = null;      // this is the cached brush object
			_foreColor = from._foreColor; // Color of the brush
			_backColor = from._backColor; // Backcolor of brush, f.i.f. HatchStyle brushes
			_hatchStyle = from._hatchStyle; // f�r HatchBrush
			_textureImage = null == from._textureImage ? null : (ImageProxy)from._textureImage.Clone(); // f�r Texturebrush
			_wrapMode = from._wrapMode; // f�r TextureBrush und LinearGradientBrush
			_brushBoundingRectangle = from._brushBoundingRectangle;
			_focus = from._focus;
			_exchangeColors = from._exchangeColors;
			this._gradientMode = from._gradientMode;
			this._gradientShape = from._gradientShape;
			this._scale = from._scale;
		}


		public BrushX(NamedColor c)
		{
			this._brushType = BrushType.SolidBrush;
			this._foreColor = c;
		}

		public static implicit operator System.Drawing.Brush(BrushX bh)
		{
			return bh == null ? null : bh.Brush;
		}


		public BrushType BrushType
		{
			get
			{
				return this._brushType;
			}
			set
			{
				BrushType oldValue = this._brushType;
				_brushType = value;
				if (_brushType != oldValue)
				{
					_SetBrushVariable(null);
					OnBrushTypeChanged(oldValue, value);
					OnChanged();
				}
			}
		}

		/// <summary>
		/// Intended to initialize some brush variables to default values if the brush type changed.
		/// </summary>
		/// <param name="oldValue">Old brush type.</param>
		/// <param name="newValue">New brush type.</param>
		protected virtual void OnBrushTypeChanged(BrushType oldValue, BrushType newValue)
		{
			switch (newValue)
			{
				case BrushType.LinearGradientBrush:
					_scale = 0;
					break;
				case BrushType.TextureBrush:
					_scale = 1;
					break;

			}
		}

		/// <summary>
		/// Returns true if the brush is visible, i.e. is not a transparent brush.
		/// </summary>
		public bool IsVisible
		{
			get
			{
				return !(_brushType == BrushType.SolidBrush && _foreColor.Color.A == 0);
			}
		}

		/// <summary>
		/// Returns true if the brush is invisible, i.e. is a solid and transparent brush.
		/// </summary>
		public bool IsInvisible
		{
			get
			{
				return _brushType == BrushType.SolidBrush && _foreColor.Color.A == 0;
			}
		}

		public NamedColor Color
		{
			get { return _foreColor; }
			set
			{
				bool bChanged = !_foreColor.Equals(value);
				_foreColor = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public NamedColor BackColor
		{
			get { return _backColor; }
			set
			{
				bool bChanged = !_backColor.Equals(value);
				_backColor = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public bool ExchangeColors
		{
			get
			{
				return _exchangeColors;
			}
			set
			{
				bool oldValue = _exchangeColors;
				_exchangeColors = value;
				if (value != oldValue)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public HatchStyle HatchStyle
		{
			get
			{
				return _hatchStyle;
			}
			set
			{
				bool bChanged = (_hatchStyle != value);
				_hatchStyle = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public WrapMode WrapMode
		{
			get
			{
				return _wrapMode;
			}
			set
			{
				bool bChanged = (_wrapMode != value);
				_wrapMode = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public LinearGradientMode GradientMode
		{
			get
			{
				return _gradientMode;
			}
			set
			{
				bool bChanged = (_gradientMode != value);
				_gradientMode = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}
		public static void ToLinearGradientMode(LinearGradientModeEx mode, out LinearGradientMode lgm, out bool reverse)
		{
			switch (mode)
			{
				case LinearGradientModeEx.BackwardDiagonal:
					lgm = LinearGradientMode.BackwardDiagonal;
					reverse = false;
					break;
				case LinearGradientModeEx.ForwardDiagonal:
					lgm = LinearGradientMode.ForwardDiagonal;
					reverse = false;
					break;
				default:
				case LinearGradientModeEx.Horizontal:
					lgm = LinearGradientMode.Horizontal;
					reverse = false;
					break;
				case LinearGradientModeEx.Vertical:
					lgm = LinearGradientMode.Vertical;
					reverse = false;
					break;

				case LinearGradientModeEx.RevBackwardDiagonal:
					lgm = LinearGradientMode.BackwardDiagonal;
					reverse = true;
					break;
				case LinearGradientModeEx.RevForwardDiagonal:
					lgm = LinearGradientMode.ForwardDiagonal;
					reverse = true;
					break;
				case LinearGradientModeEx.RevHorizontal:
					lgm = LinearGradientMode.Horizontal;
					reverse = true;
					break;
				case LinearGradientModeEx.RevVertical:
					lgm = LinearGradientMode.Vertical;
					reverse = true;
					break;
			}
		}

		public LinearGradientShape GradientShape
		{
			get
			{
				return _gradientShape;
			}
			set
			{
				bool bChanged = (_gradientShape != value);
				_gradientShape = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public float GradientFocus
		{
			get
			{
				return _focus;
			}
			set
			{
				bool bChanged = (_focus != value);
				_focus = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public float GradientScale
		{
			get
			{
				return _scale;
			}
			set
			{
				bool bChanged = (_scale != value);
				_scale = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public ImageProxy TextureImage
		{
			get
			{
				return _textureImage;
			}
			set
			{
				bool bChanged = _textureImage != value;
				_textureImage = value;
				if (bChanged)
				{
					_SetBrushVariable(null);
					OnChanged();
				}
			}
		}

		public float TextureScale
		{
			get
			{
				return _scale;
			}
			set
			{
				this.GradientScale = value;
			}
		}

		/// <summary>
		/// Sets the environment for the creation of native brush.
		/// </summary>
		/// <param name="boundingRectangle">Bounding rectangle used for gradient textures.</param>
		/// <param name="maxEffectiveResolution">Maximum effective resolution in Dpi. This information is neccessary for repeatable texture brushes. You can calculate this using <see cref="GetMaximumEffectiveResolution"/></param>
		/// <returns>True if changes to the brush were made. False otherwise.</returns>
		public bool SetEnvironment(RectangleF boundingRectangle, double maxEffectiveResolution)
		{
			bool changed = false;

			// fix: in order that a gradient is shown, the bounding rectangle's width and height must always be positive (this is not the case for instance for pens)
			if (boundingRectangle.Width < 0)
			{
				boundingRectangle.X += boundingRectangle.Width;
				boundingRectangle.Width = -boundingRectangle.Width;
			}
			else if (boundingRectangle.Width == 0)
			{
				boundingRectangle.Width = 1;
			}

			if (boundingRectangle.Height < 0)
			{
				boundingRectangle.Y += boundingRectangle.Height;
				boundingRectangle.Height = -boundingRectangle.Height;
			}
			else if (boundingRectangle.Height == 0)
			{
				boundingRectangle.Height = 1;
			}

			if (_brushType == BrushType.LinearGradientBrush || _brushType == BrushType.PathGradientBrush)
			{
				changed = (_brushBoundingRectangle != boundingRectangle);
				_brushBoundingRectangle = boundingRectangle;
			}
			else
			{
				_brushBoundingRectangle = boundingRectangle; // has no meaning for other brushes, so we set it but dont care
			}

			if (_brushType == BrushType.TextureBrush && (_textureImage is ISyntheticRepeatableTexture))
			{

				double s = maxEffectiveResolution * ((_textureImage as ISyntheticRepeatableTexture).Size) / 72.0;
				int w = Altaxo.Calc.Rounding.RoundUp((int)s, 8);

				// Make sure w is not too small nor too big
				if (!(w >= 8))
					w = 8;
				if (!(w <= 8192))
					w = 8192;


				if (w != _cachedTexturePixelSize)
				{
					_cachedTexturePixelSize = w;
					changed = true;
				}
			}

			if (changed)
			{
				_SetBrushVariable(null);
				OnChanged();
			}

			return changed;
		}

		public static double GetEffectiveMaximumResolution(Graphics g)
		{
			return GetEffectiveMaximumResolution(g, 1);
		}

		public static double GetEffectiveMaximumResolution(Graphics g, double objectScale)
		{
			double maxDpi = Math.Max(g.DpiX, g.DpiY);
			var e = g.Transform.Elements;
			var scaleX = e[0] * e[0] + e[1] * e[1];
			var scaleY = (e[0] * e[3] - e[1] * e[2]) / Math.Sqrt(scaleX);
			maxDpi *= Math.Max(scaleX, scaleY);
			maxDpi *= objectScale;
			return maxDpi;
		}


		private Bitmap GetDefaultTextureBitmap()
		{
			Bitmap result = new Bitmap(3, 3, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			result.SetPixel(1, 1, System.Drawing.Color.Black);
			return result;
		}

		private static System.Drawing.Color ToGdi(NamedColor color)
		{
			var c = color.Color;
			return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
		}

		public Brush Brush
		{
			get
			{
				if (_cachedBrush == null)
				{
					Brush br = null;
					switch (_brushType)
					{
						case BrushType.SolidBrush:
							br = new SolidBrush(ToGdi(_foreColor));
							break;
						case BrushType.HatchBrush:
							if (_exchangeColors)
								br = new HatchBrush(_hatchStyle, ToGdi(_backColor), ToGdi(_foreColor));
							else
								br = new HatchBrush(_hatchStyle, ToGdi(_foreColor), ToGdi(_backColor));
							break;
						case BrushType.LinearGradientBrush:
							if (_brushBoundingRectangle.IsEmpty)
								_brushBoundingRectangle = new RectangleF(0, 0, 1000, 1000);
							LinearGradientBrush lgb;
							br = lgb = new LinearGradientBrush(_brushBoundingRectangle, _exchangeColors ? ToGdi(_backColor) : ToGdi(_foreColor), _exchangeColors ? ToGdi(_foreColor) : ToGdi(_backColor), _gradientMode);
							if (_wrapMode != WrapMode.Clamp)
								lgb.WrapMode = _wrapMode;
							if (_gradientShape == LinearGradientShape.Triangular)
								lgb.SetBlendTriangularShape(_focus, _scale);
							else if (_gradientShape == LinearGradientShape.SigmaBell)
								lgb.SetSigmaBellShape(_focus, _scale);
							break;
						case BrushType.PathGradientBrush:
							GraphicsPath p = new GraphicsPath();
							if (_brushBoundingRectangle.IsEmpty)
								_brushBoundingRectangle = new RectangleF(0, 0, 1000, 1000);
							p.AddRectangle(_brushBoundingRectangle);
							PathGradientBrush pgb = new PathGradientBrush(p);
							if (_exchangeColors)
							{
								pgb.SurroundColors = new Color[] { ToGdi(_backColor) };
								pgb.CenterColor = ToGdi(_foreColor);
							}
							else
							{
								pgb.SurroundColors = new Color[] { ToGdi(_foreColor) };
								pgb.CenterColor = ToGdi(_backColor);
							}
							pgb.WrapMode = _wrapMode;
							br = pgb;
							break;
						case BrushType.TextureBrush:
							Image img = null;

							if (_textureImage is ISyntheticRepeatableTexture)
							{
								img = (_textureImage as ISyntheticRepeatableTexture).GetImage(_cachedTexturePixelSize);
							}
							else if (_textureImage != null)
							{
								img = _textureImage.GetImage();
							}

							if (img == null)
							{
								img = GetDefaultTextureBitmap();
							}
							TextureBrush tb = new TextureBrush(img);
							tb.WrapMode = this._wrapMode;
							double scale = _scale;
							if (_textureImage is ISyntheticRepeatableTexture)
							{
								scale *= (_textureImage as ISyntheticRepeatableTexture).Size / img.Width;
							}
							if (scale != 1)
								tb.ScaleTransform((float)scale, (float)scale);

							br = tb;
							break;
					} // end of switch
					this._SetBrushVariable(br);
				}
				return _cachedBrush;
			} // end of get
		} // end of prop. Brush

		public static bool AreEqual(BrushX b1, BrushX b2)
		{
			if (b1._brushType != b2._brushType)
				return false;

			// Brush types are equal - we have to go into details...
			switch (b1._brushType)
			{

				case BrushType.SolidBrush:
					if (b1._foreColor != b2._foreColor)
						return false;
					break;
				case BrushType.HatchBrush:
					if (b1._foreColor != b2._foreColor)
						return false;
					if (b1._backColor != b2._backColor)
						return false;
					if (b1._hatchStyle != b2._hatchStyle)
						return false;
					break;
				case BrushType.LinearGradientBrush:
					if (b1._foreColor != b2._foreColor)
						return false;
					if (b1._backColor != b2._backColor)
						return false;
					if (b1._gradientMode != b2._gradientMode)
						return false;
					if (b1._wrapMode != b2._wrapMode)
						return false;
					if (b1._gradientShape != b2._gradientShape)
						return false;
					break;
				case BrushType.PathGradientBrush:
					if (b1._foreColor != b2._foreColor)
						return false;
					if (b1._backColor != b2._backColor)
						return false;
					if (b1._wrapMode != b2._wrapMode)
						return false;
					break;
				case BrushType.TextureBrush:
					if (b1._wrapMode != b2._wrapMode)
						return false;
					if (b1._scale != b2._scale)
						return false;
					if (b1._textureImage.ToString() != b2._textureImage.ToString())
						return false;
					break;
			} // end of switch
			return true;
		}


		public void SetSolidBrush(NamedColor c)
		{
			_brushType = BrushType.SolidBrush;
			_foreColor = c;
			_SetBrushVariable(null);
			OnChanged();
		}

		public void SetHatchBrush(HatchStyle hs, NamedColor fc)
		{
			SetHatchBrush(hs, fc, NamedColor.Black);
		}

		public void SetHatchBrush(HatchStyle hs, NamedColor fc, NamedColor bc)
		{
			_brushType = BrushType.HatchBrush;
			_hatchStyle = hs;
			_foreColor = fc;
			_backColor = bc;

			_SetBrushVariable(null);
			OnChanged();
		}

		protected void _SetBrushVariable(Brush br)
		{
			if (null != _cachedBrush)
				_cachedBrush.Dispose();

			_cachedBrush = br;
		}

		object ICloneable.Clone()
		{
			return new BrushX(this);
		}

		public BrushX Clone()
		{
			return new BrushX(this);
		}

		public void Dispose()
		{
			if (null != _cachedBrush)
				_cachedBrush.Dispose();
			_cachedBrush = null;
		}
		#region IChangedEventSource Members



		protected virtual void OnChanged()
		{
			if (null != Changed)
				Changed(this, new EventArgs());
		}

		#endregion


		#region static members

		public static BrushX Empty
		{
			get
			{
				return new BrushX(NamedColor.Transparent);
			}
		}

		#endregion
	} // end of class BrushHolder
} // end of namespace
