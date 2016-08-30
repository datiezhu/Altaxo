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

using Altaxo.Collections;
using Altaxo.Data;
using Altaxo.Drawing.D3D;
using Altaxo.Graph.Graph3D.Plot.Styles;
using Altaxo.Gui.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Gui.Graph3D.Plot.Styles
{
	#region Interfaces

	public interface IErrorBarPlotStyleView
	{
		bool IndependentColor { get; set; }

		bool ShowPlotColorsOnly { set; }

		PenX3D Pen { get; set; }

		bool IndependentSymbolSize { get; set; }

		double SymbolSize { get; set; }

		double LineWidth1Offset { get; set; }
		double LineWidth1Factor { get; set; }
		double LineWidth2Offset { get; set; }
		double LineWidth2Factor { get; set; }

		double EndCapSizeOffset { get; set; }
		double EndCapSizeFactor { get; set; }

		bool UseSymbolGap { get; set; }

		double SymbolGapOffset { get; set; }

		double SymbolGapFactor { get; set; }

		int SkipFrequency { get; set; }

		bool IndependentSkipFrequency { get; set; }

		bool DoNotShiftIndependentVariable { get; set; }

		bool UseCommonErrorColumn { get; set; }

		void Initialize_MeaningOfValues(SelectableListNodeList list);

		/// <summary>
		/// Initializes the common error column.
		/// </summary>
		/// <param name="columnAsText">Column's name.</param>
		void Initialize_CommonErrorColumn(string columnAsText, string toolTip, int status);

		void Initialize_CommonErrorColumnTransformation(string transformationTextToShow, string transformationToolTip);

		/// <summary>
		/// Initializes the positive error column.
		/// </summary>
		/// <param name="positiveErrorColumnAsText">Column's name.</param>
		void Initialize_PositiveErrorColumn(string positiveErrorColumnAsText, string positiveErrorColumnToolTip, int positiveErrorColumnStatus);

		void Initialize_PositiveErrorColumnTransformation(string transformationTextToShow, string transformationToolTip);

		/// <summary>
		/// Initializes the positive error column.
		/// </summary>
		/// <param name="negativeErrorColumnAsText">Column's name.</param>
		void Initialize_NegativeErrorColumn(string negativeErrorColumnAsText, string negativeErrorColumnToolTip, int negativeErrorColumnStatus);

		void Initialize_NegativeErrorColumnTransformation(string transformationTextToShow, string transformationToolTip);

		event Action<bool> UseCommonErrorColumnChanged;

		/// <summary>
		/// Occurs when the user choice for IndependentColor of the fill brush has changed.
		/// </summary>
		event Action IndependentColorChanged;
	}

	#endregion Interfaces

	[UserControllerForObject(typeof(ErrorBarPlotStyle))]
	[ExpectedTypeOfView(typeof(IErrorBarPlotStyleView))]
	public class ErrorBarPlotStyleController : MVCANControllerEditOriginalDocBase<ErrorBarPlotStyle, IErrorBarPlotStyleView>
	{
		/// <summary>Tracks the presence of a color group style in the parent collection.</summary>
		private ColorGroupStylePresenceTracker _colorGroupStyleTracker;

		private SelectableListNodeList _meaningOfValues;

		/// <summary>
		/// The data table that the column of the style should belong to.
		/// </summary>
		private DataTable _supposedParentDataTable;

		public override bool InitializeDocument(params object[] args)
		{
			if (args.Length >= 2 && (args[1] is DataTable))
				_supposedParentDataTable = (DataTable)args[1];

			return base.InitializeDocument(args);
		}

		public override IEnumerable<ControllerAndSetNullMethod> GetSubControllers()
		{
			yield break;
		}

		public override void Dispose(bool isDisposing)
		{
			_colorGroupStyleTracker = null;

			base.Dispose(isDisposing);
		}

		protected override void Initialize(bool initData)
		{
			base.Initialize(initData);

			if (initData)
			{
				_colorGroupStyleTracker = new ColorGroupStylePresenceTracker(_doc, EhIndependentColorChanged);

				_meaningOfValues = new SelectableListNodeList(_doc.MeaningOfValues);
			}
			if (_view != null)
			{
				_view.IndependentColor = _doc.IndependentColor;
				_view.ShowPlotColorsOnly = _colorGroupStyleTracker.MustUsePlotColorsOnly(_doc.IndependentColor);
				_view.Pen = _doc.Pen;

				_view.IndependentSymbolSize = _doc.IndependentSymbolSize;
				_view.SymbolSize = _doc.SymbolSize;

				_view.LineWidth1Offset = _doc.LineWidth1Offset;
				_view.LineWidth1Factor = _doc.LineWidth1Factor;

				_view.LineWidth2Offset = _doc.LineWidth2Offset;
				_view.LineWidth2Factor = _doc.LineWidth2Factor;

				_view.EndCapSizeOffset = _doc.EndCapSizeOffset;
				_view.EndCapSizeFactor = _doc.EndCapSizeFactor;

				_view.UseSymbolGap = _doc.UseSymbolGap;
				_view.SymbolGapOffset = _doc.SymbolGapOffset;
				_view.SymbolGapFactor = _doc.SymbolGapFactor;

				_view.SkipFrequency = _doc.SkipFrequency;
				_view.IndependentSkipFrequency = _doc.IndependentSkipFrequency;

				_view.DoNotShiftIndependentVariable = _doc.DoNotShiftIndependentVariable;

				_view.Initialize_MeaningOfValues(_meaningOfValues);

				// Errors

				_view.UseCommonErrorColumn = _doc.UseCommonErrorColumn;

				if (_doc.UseCommonErrorColumn)
				{
					this.InitializeCommonErrorColumnText();
				}
				else
				{
					this.InitializePositiveErrorColumnText();
					this.InitializeNegativeErrorColumnText();
				}
			}
		}

		public override bool Apply(bool disposeController)
		{
			_doc.IndependentColor = _view.IndependentColor;
			_doc.Pen = _view.Pen;
			_doc.IndependentSymbolSize = _view.IndependentSymbolSize;
			_doc.SymbolSize = _view.SymbolSize;

			_doc.LineWidth1Offset = _view.LineWidth1Offset;
			_doc.LineWidth1Factor = _view.LineWidth1Factor;

			_doc.LineWidth2Offset = _view.LineWidth2Offset;
			_doc.LineWidth2Factor = _view.LineWidth2Factor;

			_doc.EndCapSizeOffset = _view.EndCapSizeOffset;
			_doc.EndCapSizeFactor = _view.EndCapSizeFactor;

			_doc.UseSymbolGap = _view.UseSymbolGap;
			_doc.SymbolGapOffset = _view.SymbolGapOffset;
			_doc.SymbolGapFactor = _view.SymbolGapFactor;

			_doc.IndependentSkipFrequency = _view.IndependentSkipFrequency;
			_doc.SkipFrequency = _view.SkipFrequency;

			_doc.DoNotShiftIndependentVariable = _view.DoNotShiftIndependentVariable;

			_doc.UseCommonErrorColumn = _view.UseCommonErrorColumn;

			_doc.MeaningOfValues = (ErrorBarPlotStyle.ValueInterpretation)_meaningOfValues.FirstSelectedNode.Tag;

			return ApplyEnd(true, disposeController);
		}

		protected override void AttachView()
		{
			base.AttachView();

			_view.IndependentColorChanged += EhIndependentColorChanged;
			_view.UseCommonErrorColumnChanged += EhUseCommonErrorColumnChanged;
		}

		protected override void DetachView()
		{
			_view.IndependentColorChanged -= EhIndependentColorChanged;
			_view.UseCommonErrorColumnChanged -= EhUseCommonErrorColumnChanged;

			base.DetachView();
		}

		private void InitializeCommonErrorColumnText()
		{
			var info = new Data.PlotColumnInformation(_doc.ErrorColumn, _doc.ErrorColumnDataColumnName);
			info.Update(_supposedParentDataTable);

			_view?.Initialize_CommonErrorColumn(info.PlotColumnBoxText, info.PlotColumnToolTip, (int)info.PlotColumnBoxState);
			_view?.Initialize_CommonErrorColumnTransformation(info.TransformationTextToShow, info.TransformationToolTip);
		}

		private void InitializePositiveErrorColumnText()
		{
			var info = new Data.PlotColumnInformation(_doc.PositiveErrorColumn, _doc.PositiveErrorColumnDataColumnName);
			info.Update(_supposedParentDataTable);

			_view?.Initialize_PositiveErrorColumn(info.PlotColumnBoxText, info.PlotColumnToolTip, (int)info.PlotColumnBoxState);
			_view?.Initialize_PositiveErrorColumnTransformation(info.TransformationTextToShow, info.TransformationToolTip);
		}

		private void InitializeNegativeErrorColumnText()
		{
			var info = new Data.PlotColumnInformation(_doc.NegativeErrorColumn, _doc.NegativeErrorColumnDataColumnName);
			info.Update(_supposedParentDataTable);

			_view?.Initialize_NegativeErrorColumn(info.PlotColumnBoxText, info.PlotColumnToolTip, (int)info.PlotColumnBoxState);
			_view?.Initialize_NegativeErrorColumnTransformation(info.TransformationTextToShow, info.TransformationToolTip);
		}

		private void EhUseCommonErrorColumnChanged(bool useCommonErrorColumn)
		{
			_doc.UseCommonErrorColumn = useCommonErrorColumn;

			_view.UseCommonErrorColumn = _doc.UseCommonErrorColumn;

			if (_doc.UseCommonErrorColumn)
			{
				this.InitializeCommonErrorColumnText();
			}
			else
			{
				this.InitializePositiveErrorColumnText();
				this.InitializeNegativeErrorColumnText();
			}
		}

		private void EhIndependentColorChanged()
		{
			if (null != _view)
			{
				_doc.IndependentColor = _view.IndependentColor;
				_view.ShowPlotColorsOnly = _colorGroupStyleTracker.MustUsePlotColorsOnly(_doc.IndependentColor);
			}
		}
	}
}