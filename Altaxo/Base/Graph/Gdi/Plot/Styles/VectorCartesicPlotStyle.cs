#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2016 Dr. Dirk Lellinger
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

using Altaxo.Data;
using Altaxo.Graph.Plot.Data;
using Altaxo.Graph.Plot.Groups;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Graph.Gdi.Plot.Styles
{
	using Altaxo.Graph;
	using Altaxo.Main;
	using Data;
	using Drawing;
	using Geometry;
	using Groups;
	using System.Drawing;
	using System.Drawing.Drawing2D;

	public class VectorCartesicPlotStyle
		:
		Main.SuspendableDocumentNodeWithEventArgs, IG2DPlotStyle
	{
		/// <summary>
		/// Designates how to interpret the values of the error columns.
		/// </summary>
		public enum ValueInterpretation
		{
			/// <summary>The columns are absolute differences, i.e. absolute deviations from the nominal value. The target value is the nominal value plus this difference.</summary>
			AbsoluteDifference = 0,

			/// <summary>The columns are interpretet as values that designate the target location of the vector.</summary>
			AbsoluteValue = 1
		}

		private INumericColumnProxy _columnX;
		private INumericColumnProxy _columnY;

		private ValueInterpretation _meaningOfValues;

		/// <summary>If true, the vector length is set manually, and the three columns are used only to determine the vector direction.</summary>
		private bool _useManualVectorLength;

		/// <summary>Constant value of the vector length. Used only if <see cref="_useManualVectorLength"/> is set to true.</summary>
		private double _vectorLengthOffset = 2;

		/// <summary>Factor that is multiplied with the cached symbol size to determine the vector length. Used only if <see cref="_useManualVectorLength"/> is set to true.</summary>
		private double _vectorLengthFactor;

		/// <summary>
		/// True if the color of the label is not dependent on the color of the parent plot style.
		/// </summary>
		protected bool _independentColor;

		/// <summary>Pen used to draw the vector.</summary>
		private PenX _strokePen;

		/// <summary>
		/// True if the symbol size is independent, i.e. is not published nor updated by a group style.
		/// </summary>
		private bool _independentSymbolSize;

		/// <summary>Controls the length of the end bar.</summary>
		private double _symbolSize;

		/// <summary>
		/// True when the vector line is not drawn in the circel of diameter SymbolSize around the symbol center.
		/// </summary>
		private bool _useSymbolGap;

		/// <summary>
		/// Offset used to calculate the real gap between symbol center and beginning of the bar, according to the formula:
		/// realGap = _symbolGap * _symbolGapFactor + _symbolGapOffset;
		/// </summary>
		private double _symbolGapOffset;

		/// <summary>
		/// Factor used to calculate the real gap between symbol center and beginning of the bar, according to the formula:
		/// realGap = _symbolGap * _symbolGapFactor + _symbolGapOffset;
		/// </summary>
		private double _symbolGapFactor = 1.25;

		private double _endCapSizeFactor = 1;

		private double _endCapSizeOffset;

		private double _lineWidth1Offset;
		private double _lineWidth1Factor;

		/// <summary>If true, group styles that shift the logical position of the items (for instance <see cref="BarSizePosition3DGroupStyle"/>) are not applied. I.e. when true, the position of the item remains unperturbed.</summary>
		private bool _independentOnShiftingGroupStyles = true; // default is true, because we don't want irritated users

		/// <summary>
		/// Skip frequency.
		/// </summary>
		protected int _skipFrequency;

		protected bool _independentSkipFrequency;

		/// <summary>Logical x shift between the location of the real data point and the point where the item is finally drawn.</summary>
		private double _cachedLogicalShiftX;

		/// <summary>Logical y shift between the location of the real data point and the point where the item is finally drawn.</summary>
		private double _cachedLogicalShiftY;

		/// <summary>If this function is set, then _symbolSize is ignored and the symbol size is evaluated by this function.</summary>
		[field: NonSerialized]
		protected Func<int, double> _cachedSymbolSizeForIndexFunction;

		/// <summary>If this function is set, the symbol color is determined by calling this function on the index into the data.</summary>
		[field: NonSerialized]
		protected Func<int, System.Drawing.Color> _cachedColorForIndexFunction;

		#region Serialization

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(VectorCartesicPlotStyle), 0)]
		private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				VectorCartesicPlotStyle s = (VectorCartesicPlotStyle)obj;

				info.AddEnum("MeaningOfValues", s._meaningOfValues);
				info.AddValue("ColumnX", s._columnX);
				info.AddValue("ColumnY", s._columnY);
				info.AddValue("IndependentSkipFreq", s._independentSkipFrequency);
				info.AddValue("SkipFreq", s._skipFrequency);
				info.AddValue("UseManualVectorLength", s._useManualVectorLength);
				info.AddValue("VectorLengthOffset", s._vectorLengthOffset);
				info.AddValue("VectorLengthFactor", s._vectorLengthFactor);

				info.AddValue("IndependentSymbolSize", s._independentSymbolSize);
				info.AddValue("SymbolSize", s._symbolSize);

				info.AddValue("Pen", s._strokePen);
				info.AddValue("IndependentColor", s._independentColor);

				info.AddValue("LineWidth1Offset", s._lineWidth1Offset);
				info.AddValue("LineWidth1Factor", s._lineWidth1Factor);

				info.AddValue("EndCapSizeOffset", s._endCapSizeOffset);
				info.AddValue("EndCapSizeFactor", s._endCapSizeFactor);

				info.AddValue("UseSymbolGap", s._useSymbolGap);
				info.AddValue("SymbolGapOffset", s._symbolGapOffset);
				info.AddValue("SymbolGapFactor", s._symbolGapFactor);

				info.AddValue("IndependentOnShiftingGroupStyles", s._independentOnShiftingGroupStyles);
			}

			protected virtual VectorCartesicPlotStyle SDeserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				VectorCartesicPlotStyle s = null != o ? (VectorCartesicPlotStyle)o : new VectorCartesicPlotStyle(info);

				s._meaningOfValues = (ValueInterpretation)info.GetEnum("MeaningOfValues", typeof(ValueInterpretation));

				s._columnX = (Altaxo.Data.INumericColumnProxy)info.GetValue("ColumnX", s);
				if (null != s._columnX) s._columnX.ParentObject = s;

				s._columnY = (Altaxo.Data.INumericColumnProxy)info.GetValue("ColumnY", s);
				if (null != s._columnY) s._columnY.ParentObject = s;

				s._independentSkipFrequency = info.GetBoolean("IndependentSkipFreq");
				s._skipFrequency = info.GetInt32("SkipFreq");

				s._useManualVectorLength = info.GetBoolean("UseManualVectorLength");
				s._vectorLengthOffset = info.GetDouble("VectorLengthOffset");
				s._vectorLengthFactor = info.GetDouble("VectorLengthFactor");

				s._independentSymbolSize = info.GetBoolean("IndependentSymbolSize");
				s._symbolSize = info.GetDouble("SymbolSize");

				s.Pen = (PenX)info.GetValue("Pen", s);
				s._independentColor = info.GetBoolean("IndependentColor");

				s._lineWidth1Offset = info.GetDouble("LineWidth1Offset");
				s._lineWidth1Factor = info.GetDouble("LineWidth1Factor");

				s._endCapSizeOffset = info.GetDouble("EndCapSizeOffset");
				s._endCapSizeFactor = info.GetDouble("EndCapSizeFactor");

				s._useSymbolGap = info.GetBoolean("UseSymbolGap");
				s._symbolGapOffset = info.GetDouble("SymbolGapOffset");
				s._symbolGapFactor = info.GetDouble("SymbolGapFactor");

				s._independentOnShiftingGroupStyles = info.GetBoolean("IndependentOnShiftingGroupStyles");

				return s;
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				VectorCartesicPlotStyle s = SDeserialize(o, info, parent);

				return s;
			}
		}

		#endregion Serialization

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info">The information.</param>
		protected VectorCartesicPlotStyle(Altaxo.Serialization.Xml.IXmlDeserializationInfo info)
		{
		}

		public VectorCartesicPlotStyle(Altaxo.Main.Properties.IReadOnlyPropertyBag context)
		{
			var penWidth = GraphDocument.GetDefaultPenWidth(context);
			var color = GraphDocument.GetDefaultPlotColor(context);

			_lineWidth1Offset = penWidth;
			_lineWidth1Factor = 0;

			ChildSetMember(ref _strokePen, new PenX(color, penWidth) { EndCap = new LineCaps.ArrowF10LineCap() });
		}

		public VectorCartesicPlotStyle(VectorCartesicPlotStyle from, bool copyWithDataReferences)
		{
			CopyFrom(from, copyWithDataReferences);
		}

		public bool CopyFrom(object obj, bool copyWithDataReferences)
		{
			if (object.ReferenceEquals(this, obj))
				return true;
			var from = obj as VectorCartesicPlotStyle;
			if (null != from)
			{
				this._meaningOfValues = from._meaningOfValues;
				this._independentSkipFrequency = from._independentSkipFrequency;
				this._skipFrequency = from._skipFrequency;
				this._useManualVectorLength = from._useManualVectorLength;
				this._vectorLengthOffset = from._vectorLengthOffset;
				this._vectorLengthFactor = from._vectorLengthFactor;

				this._independentSymbolSize = from._independentSymbolSize;
				this._symbolSize = from._symbolSize;

				this._strokePen = from._strokePen;
				this._independentColor = from._independentColor;

				_lineWidth1Offset = from._lineWidth1Offset;
				_lineWidth1Factor = from._lineWidth1Factor;

				this._endCapSizeFactor = from._endCapSizeFactor;
				this._endCapSizeOffset = from._endCapSizeOffset;

				this._useSymbolGap = from._useSymbolGap;
				this._symbolGapFactor = from._symbolGapFactor;
				this._symbolGapOffset = from._symbolGapOffset;

				this._independentSkipFrequency = from._independentSkipFrequency;
				this._skipFrequency = from._skipFrequency;
				this._independentOnShiftingGroupStyles = from._independentOnShiftingGroupStyles;

				this._cachedLogicalShiftX = from._cachedLogicalShiftX;
				this._cachedLogicalShiftY = from._cachedLogicalShiftY;

				if (copyWithDataReferences)
				{
					ChildCloneToMember(ref _columnX, from._columnX);
					ChildCloneToMember(ref _columnY, from._columnY);
				}

				EhSelfChanged();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Copies the member variables from another instance.
		/// </summary>
		/// <param name="obj">Another instance to copy the data from.</param>
		/// <returns>True if data was copied, otherwise false.</returns>
		public bool CopyFrom(object obj)
		{
			return CopyFrom(obj, true);
		}

		/// <inheritdoc/>
		public object Clone(bool copyWithDataReferences)
		{
			return new VectorCartesicPlotStyle(this, copyWithDataReferences);
		}

		/// <inheritdoc/>
		public object Clone()
		{
			return new VectorCartesicPlotStyle(this, true);
		}

		protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
		{
			if (null != _columnX)
				yield return new Main.DocumentNodeAndName(_columnX, nameof(ColumnX));

			if (null != _columnY)
				yield return new Main.DocumentNodeAndName(_columnY, nameof(ColumnY));
		}

		#region Properties

		public ValueInterpretation MeaningOfValues
		{
			get
			{
				return _meaningOfValues;
			}
			set
			{
				if (!(_meaningOfValues == value))
				{
					_meaningOfValues = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>
		/// Data that define the error in the positive direction.
		/// </summary>
		public INumericColumn ColumnX
		{
			get
			{
				return _columnX?.Document;
			}
			set
			{
				var oldValue = _columnX?.Document;
				if (!object.ReferenceEquals(value, oldValue))
				{
					ChildSetMember(ref _columnX, null == value ? null : NumericColumnProxyBase.FromColumn(value));
					EhSelfChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets the name of the common error column, if it is a data column. Otherwise, null is returned.
		/// </summary>
		/// <value>
		/// The name of the common error column if it is a data column. Otherwise, null.
		/// </value>
		public string ColumnXDataColumnName
		{
			get
			{
				return _columnX?.DocumentPath?.LastPartOrDefault;
			}
		}

		/// <summary>
		/// Data that define the error in the positive direction.
		/// </summary>
		public INumericColumn ColumnY
		{
			get
			{
				return _columnY?.Document;
			}
			set
			{
				var oldValue = _columnY?.Document;
				if (!object.ReferenceEquals(value, oldValue))
				{
					ChildSetMember(ref _columnY, null == value ? null : NumericColumnProxyBase.FromColumn(value));
					EhSelfChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets the name of the positive error column, if it is a data column. Otherwise, null is returned.
		/// </summary>
		/// <value>
		/// The name of the positive error column if it is a data column. Otherwise, null.
		/// </value>
		public string ColumnYDataColumnName
		{
			get
			{
				return _columnY?.DocumentPath?.LastPartOrDefault;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the skip frequency is independent on other sub group styles using <see cref="SkipFrequency"/>.
		/// </summary>
		/// <value>
		/// <c>true</c> if the skip frequency is independent on other sub group styles using <see cref="SkipFrequency"/>; otherwise, <c>false</c>.
		/// </value>
		public bool IndependentSkipFrequency
		{
			get { return _independentSkipFrequency; }
			set
			{
				if (!(_independentSkipFrequency == value))
				{
					_independentSkipFrequency = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>Controls how many items are plotted. A value of 1 means every item, a value of 2 every other item, and so on.</summary>
		public int SkipFrequency
		{
			get { return _skipFrequency; }
			set
			{
				if (!(_skipFrequency == value))
				{
					_skipFrequency = Math.Max(1, value);
					EhSelfChanged();
				}
			}
		}

		/// <summary>If true, the vector length is set manually, and the three columns are used only to determine the vector direction.</summary>
		public bool UseManualVectorLength
		{
			get { return _useManualVectorLength; }
			set
			{
				if (!(_useManualVectorLength == value))
				{
					_useManualVectorLength = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>Constant value of the vector length. Used only if <see cref="_useManualVectorLength"/> is set to true.</summary>
		public double VectorLengthOffset
		{
			get
			{
				return _vectorLengthOffset;
			}
			set
			{
				if (!(_vectorLengthOffset == value))
				{
					_vectorLengthOffset = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>Factor that is multiplied with the cached symbol size to determine the vector length. Used only if <see cref="_useManualVectorLength"/> is set to true.</summary>
		public double VectorLengthFactor
		{
			get
			{
				return _vectorLengthFactor;
			}
			set
			{
				if (!(_vectorLengthFactor == value))
				{
					_vectorLengthFactor = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>
		/// true if the symbol size is independent, i.e. is not published nor updated by a group style.
		/// </summary>
		public bool IndependentSymbolSize
		{
			get { return _independentSymbolSize; }
			set
			{
				var oldValue = _independentSymbolSize;
				_independentSymbolSize = value;
				if (oldValue != value)
					EhSelfChanged(EventArgs.Empty);
			}
		}

		/// <summary>Controls the length of the end bar.</summary>
		public double SymbolSize
		{
			get { return _symbolSize; }
			set
			{
				var oldValue = _symbolSize;
				_symbolSize = value;
				if (oldValue != value)
					EhSelfChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// True when the line is not drawn in the circel of diameter SymbolSize around the symbol center.
		/// </summary>
		public bool UseSymbolGap
		{
			get { return _useSymbolGap; }
			set
			{
				var oldValue = _useSymbolGap;
				_useSymbolGap = value;
				if (oldValue != value)
					EhSelfChanged(EventArgs.Empty);
			}
		}

		public double SymbolGapOffset
		{
			get
			{
				return _symbolGapOffset;
			}
			set
			{
				if (!(_symbolGapOffset == value))
				{
					_symbolGapOffset = value;
					EhSelfChanged();
				}
			}
		}

		public double SymbolGapFactor
		{
			get
			{
				return _symbolGapFactor;
			}
			set
			{
				if (!(_symbolGapFactor == value))
				{
					_symbolGapFactor = value;
					EhSelfChanged();
				}
			}
		}

		public double LineWidth1Offset
		{
			get
			{
				return _lineWidth1Offset;
			}
			set
			{
				if (!(_lineWidth1Offset == value))
				{
					_lineWidth1Offset = value;
					EhSelfChanged();
				}
			}
		}

		public double LineWidth1Factor
		{
			get
			{
				return _lineWidth1Factor;
			}
			set
			{
				if (!(_lineWidth1Factor == value))
				{
					_lineWidth1Factor = value;
					EhSelfChanged();
				}
			}
		}

		public double EndCapSizeOffset
		{
			get
			{
				return _endCapSizeOffset;
			}
			set
			{
				if (!(_endCapSizeOffset == value))
				{
					_endCapSizeOffset = value;
					EhSelfChanged();
				}
			}
		}

		public double EndCapSizeFactor
		{
			get
			{
				return _endCapSizeFactor;
			}
			set
			{
				if (!(_endCapSizeFactor == value))
				{
					_endCapSizeFactor = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>
		/// True if the color of the label is not dependent on the color of the parent plot style.
		/// </summary>
		public bool IndependentColor
		{
			get { return _independentColor; }
			set
			{
				var oldValue = _independentColor;
				_independentColor = value;
				if (oldValue != value)
					EhSelfChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// True when we don't want to shift the position of the items, for instance due to the bar graph plot group.
		/// </summary>
		public bool IndependentOnShiftingGroupStyles
		{
			get
			{
				return _independentOnShiftingGroupStyles;
			}
			set
			{
				if (!(_independentOnShiftingGroupStyles == value))
				{
					_independentOnShiftingGroupStyles = value;
					EhSelfChanged();
				}
			}
		}

		/// <summary>Pen used to draw the error bar.</summary>
		public PenX Pen
		{
			get { return _strokePen; }
			set
			{
				if (null == value)
					throw new ArgumentNullException(nameof(value));

				if (!_strokePen.Equals(value))
				{
					ChildCloneToMember(ref _strokePen, value);
					EhSelfChanged(EventArgs.Empty);
				}
			}
		}

		#endregion Properties

		#region IG3DPlotStyle Members

		public void CollectExternalGroupStyles(PlotGroupStyleCollection externalGroups)
		{
			if (!_independentColor)
				Graph.Plot.Groups.ColorGroupStyle.AddExternalGroupStyle(externalGroups);
		}

		public void CollectLocalGroupStyles(PlotGroupStyleCollection externalGroups, PlotGroupStyleCollection localGroups)
		{
			if (!_independentColor)
				Graph.Plot.Groups.ColorGroupStyle.AddLocalGroupStyle(externalGroups, localGroups);

			if (!_independentSkipFrequency)
				SkipFrequencyGroupStyle.AddLocalGroupStyle(externalGroups, localGroups); // (local group only)
		}

		public void PrepareGroupStyles(PlotGroupStyleCollection externalGroups, PlotGroupStyleCollection localGroups, IPlotArea layer, Processed2DPlotData pdata)
		{
			if (!_independentColor)
				Graph.Plot.Groups.ColorGroupStyle.PrepareStyle(externalGroups, localGroups, delegate () { return this._strokePen.Color; });

			if (!_independentSkipFrequency)
				SkipFrequencyGroupStyle.PrepareStyle(externalGroups, localGroups, delegate () { return SkipFrequency; });

			// note: symbol size and barposition are only applied, but not prepared
			// this item can not be used as provider of a symbol size
		}

		public void ApplyGroupStyles(PlotGroupStyleCollection externalGroups, PlotGroupStyleCollection localGroups)
		{
			_cachedColorForIndexFunction = null;
			_cachedSymbolSizeForIndexFunction = null;
			// color
			if (!_independentColor)
			{
				ColorGroupStyle.ApplyStyle(externalGroups, localGroups, delegate (NamedColor c) { _strokePen.Color = c; });

				// but if there is a color evaluation function, then use that function with higher priority
				VariableColorGroupStyle.ApplyStyle(externalGroups, localGroups, delegate (Func<int, System.Drawing.Color> evalFunc) { _cachedColorForIndexFunction = evalFunc; });
			}

			if (!_independentSkipFrequency)
				SkipFrequencyGroupStyle.ApplyStyle(externalGroups, localGroups, delegate (int c) { this.SkipFrequency = c; });

			// symbol size
			if (!_independentSymbolSize)
			{
				this._symbolSize = 0;
				SymbolSizeGroupStyle.ApplyStyle(externalGroups, localGroups, delegate (double size) { this._symbolSize = size; });

				// but if there is an symbol size evaluation function, then use this with higher priority.
				_cachedSymbolSizeForIndexFunction = null;
				VariableSymbolSizeGroupStyle.ApplyStyle(externalGroups, localGroups, delegate (Func<int, double> evalFunc) { _cachedSymbolSizeForIndexFunction = evalFunc; });
			}
			else
			{
				_cachedSymbolSizeForIndexFunction = null;
			}

			// Shift the items ?
			_cachedLogicalShiftX = 0;
			_cachedLogicalShiftY = 0;
			if (!_independentOnShiftingGroupStyles)
			{
				var shiftStyle = PlotGroupStyle.GetFirstStyleToApplyImplementingInterface<IShiftLogicalXYGroupStyle>(externalGroups, localGroups);
				if (null != shiftStyle)
				{
					shiftStyle.Apply(out _cachedLogicalShiftX, out _cachedLogicalShiftY);
				}
			}
		}

		public void Paint(Graphics g, IPlotArea layer, Processed2DPlotData pdata, Processed2DPlotData prevItemData, Processed2DPlotData nextItemData)
		{
			PaintErrorBars(g, layer, pdata);
		}

		protected void PaintErrorBars(Graphics g, IPlotArea layer, Processed2DPlotData pdata)
		{
			const double logicalClampMinimum = -10;
			const double logicalClampMaximum = 11;

			// Plot error bars for the dependent variable (y)
			PlotRangeList rangeList = pdata.RangeList;
			var ptArray = pdata.PlotPointsInAbsoluteLayerCoordinates;
			INumericColumn columnX = ColumnX;
			INumericColumn columnY = ColumnY;

			if (columnX == null || columnY == null)
				return; // nothing to do if both error columns are null

			var strokePen = _strokePen.Clone();

			using (GraphicsPath isoLine = new GraphicsPath())
			{
				foreach (PlotRange r in rangeList)
				{
					int lower = r.LowerBound;
					int upper = r.UpperBound;
					int offset = r.OffsetToOriginal;

					for (int j = lower; j < upper; j += _skipFrequency)
					{
						int originalRow = j + offset;
						double symbolSize = null == _cachedSymbolSizeForIndexFunction ? _symbolSize : _cachedSymbolSizeForIndexFunction(originalRow);

						strokePen.Width = (_lineWidth1Offset + _lineWidth1Factor * symbolSize);

						if (null != _cachedColorForIndexFunction)
							strokePen.Color = GdiColorHelper.ToNamedColor(_cachedColorForIndexFunction(originalRow), "VariableColor");

						if (!(strokePen.EndCap is LineCaps.FlatCap))
							strokePen.EndCap = strokePen.EndCap.WithMinimumAbsoluteAndRelativeSize(symbolSize * _endCapSizeFactor + _endCapSizeOffset, 1 + 1E-6);

						// Calculate target
						AltaxoVariant targetX, targetY;
						switch (_meaningOfValues)
						{
							case ValueInterpretation.AbsoluteDifference:
								{
									targetX = pdata.GetXPhysical(originalRow) + columnX[originalRow];
									targetY = pdata.GetYPhysical(originalRow) + columnY[originalRow];
								}
								break;

							case ValueInterpretation.AbsoluteValue:
								{
									targetX = columnX[originalRow];
									targetY = columnY[originalRow];
								}
								break;

							default:
								throw new NotImplementedException(nameof(_meaningOfValues));
						}

						var logicalTarget = layer.GetLogical3D(targetX, targetY);
						var logicalOrigin = layer.GetLogical3D(pdata, originalRow);

						if (!_independentOnShiftingGroupStyles && (0 != _cachedLogicalShiftX || 0 != _cachedLogicalShiftY))
						{
							logicalOrigin.RX += _cachedLogicalShiftX;
							logicalOrigin.RY += _cachedLogicalShiftY;
							logicalTarget.RX += _cachedLogicalShiftX;
							logicalTarget.RY += _cachedLogicalShiftY;
						}

						if (!Calc.RMath.IsInIntervalCC(logicalOrigin.RX, logicalClampMinimum, logicalClampMaximum))
							continue;
						if (!Calc.RMath.IsInIntervalCC(logicalOrigin.RY, logicalClampMinimum, logicalClampMaximum))
							continue;
						if (!Calc.RMath.IsInIntervalCC(logicalOrigin.RZ, logicalClampMinimum, logicalClampMaximum))
							continue;

						if (!Calc.RMath.IsInIntervalCC(logicalTarget.RX, logicalClampMinimum, logicalClampMaximum))
							continue;
						if (!Calc.RMath.IsInIntervalCC(logicalTarget.RY, logicalClampMinimum, logicalClampMaximum))
							continue;
						if (!Calc.RMath.IsInIntervalCC(logicalTarget.RZ, logicalClampMinimum, logicalClampMaximum))
							continue;

						isoLine.Reset();

						layer.CoordinateSystem.GetIsoline(isoLine, logicalOrigin, logicalTarget);
						if (null == isoLine)
							continue;

						PointF[] isoLinePathPoints = null;

						if (_useManualVectorLength)
						{
							isoLine.Flatten();
							isoLinePathPoints = isoLine.PathPoints;

							double length = _vectorLengthOffset + _vectorLengthFactor * symbolSize;
							double isoLineLength = isoLinePathPoints.TotalLineLength();
							isoLinePathPoints = isoLinePathPoints.ShortenedBy(RADouble.NewAbs(0), RADouble.NewAbs(isoLineLength - length));
							if (null == isoLine)
								continue;
						}

						if (_useSymbolGap)
						{
							if (null == isoLinePathPoints)
							{
								isoLine.Flatten();
								isoLinePathPoints = isoLine.PathPoints;
							}

							double gap = _symbolGapOffset + _symbolGapFactor * symbolSize;
							if (gap != 0)
							{
								isoLinePathPoints = isoLinePathPoints.ShortenedBy(RADouble.NewAbs(gap / 2), RADouble.NewAbs(0));
								if (null == isoLine)
									continue;
							}
						}

						if (null != isoLinePathPoints)
							g.DrawLines(_strokePen, isoLinePathPoints);
						else
							g.DrawPath(strokePen, isoLine);
					}
				}
			}
		}

		/// <summary>
		/// Paints a appropriate symbol in the given rectangle. The width of the rectangle is mandatory, but if the heigth is too small,
		/// you should extend the bounding rectangle and set it as return value of this function.
		/// </summary>
		/// <param name="g">The graphics context.</param>
		/// <param name="bounds">The bounds, in which the symbol should be painted.</param>
		/// <returns>If the height of the bounding rectangle is sufficient for painting, returns the original bounding rectangle. Otherwise, it returns a rectangle that is
		/// inflated in y-Direction. Do not inflate the rectangle in x-direction!</returns>
		public RectangleF PaintSymbol(Graphics g, RectangleF bounds)
		{
			return RectangleF.Empty;
		}

		/// <summary>
		/// Prepares the scale of this plot style. Since this style does not utilize a scale, this function does nothing.
		/// </summary>
		/// <param name="layer">The parent layer.</param>
		public void PrepareScales(IPlotArea layer)
		{
		}

		#endregion IG3DPlotStyle Members

		#region IDocumentNode Members

		/// <summary>
		/// Replaces path of items (intended for data items like tables and columns) by other paths. Thus it is possible
		/// to change a plot so that the plot items refer to another table.
		/// </summary>
		/// <param name="Report">Function that reports the found <see cref="DocNodeProxy"/> instances to the visitor.</param>
		public void VisitDocumentReferences(DocNodeProxyReporter Report)
		{
			if (null != _columnX)
				Report(_columnX, this, nameof(ColumnX));
			if (null != _columnY)
				Report(_columnY, this, nameof(ColumnY));
		}

		/// <summary>
		/// Gets the columns used additionally by this style, e.g. the label column for a label plot style, or the error columns for an error bar plot style.
		/// </summary>
		/// <returns>An enumeration of tuples. Each tuple consist of the column name, as it should be used to identify the column in the data dialog. The second item of this
		/// tuple is a function that returns the column proxy for this column, in order to get the underlying column or to set the underlying column.</returns>
		public IEnumerable<Tuple<
			string, // Column label
			IReadableColumn, // the column as it was at the time of this call
			string, // the name of the column (last part of the column proxies document path)
			Action<IReadableColumn> // action to set the column during Apply of the controller
			>> GetAdditionallyUsedColumns()
		{
			yield return new Tuple<string, IReadableColumn, string, Action<IReadableColumn>>(nameof(ColumnX), ColumnX, _columnX?.DocumentPath?.LastPartOrDefault, (col) => ColumnX = col as INumericColumn);

			yield return new Tuple<string, IReadableColumn, string, Action<IReadableColumn>>(nameof(ColumnY), ColumnY, _columnY?.DocumentPath?.LastPartOrDefault, (col) => ColumnY = col as INumericColumn);
		}

		#endregion IDocumentNode Members
	}
}