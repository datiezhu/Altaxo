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

#endregion Copyright

using System;
using System.Drawing;

namespace Altaxo.Graph.Plot.Data
{
	using Gdi.Plot.Data;

	#region XYFunctionPlotData

	/// <summary>
	/// Summary description for XYFunctionPlotData.
	/// </summary>
	[Serializable]
	public abstract class XYFunctionPlotDataBase
			:
			Main.SuspendableDocumentNodeWithSingleAccumulatedData<PlotItemDataChangedEventArgs>,
			IXYFunctionPlotData,
			Calc.IScalarFunctionDD
	{
		/// <summary>
		/// Only for derived classes and deserialization.
		/// </summary>
		protected XYFunctionPlotDataBase()
		{
		}

		public XYFunctionPlotDataBase(XYFunctionPlotDataBase from)
		{
			if (null == from)
				throw new ArgumentNullException(nameof(from));

			CopyFrom(from);
		}

		public virtual bool CopyFrom(object obj)
		{
			if (object.ReferenceEquals(this, obj))
				return true;

			if (obj is XYFunctionPlotDataBase from)
			{
				return true;
			}
			return false;
		}

		public abstract object Clone();

		#region IScalarFunctionDD Members

		public abstract double Evaluate(double x);

		#endregion IScalarFunctionDD Members

		#region Changed event handling

		protected override void AccumulateChangeData(object sender, EventArgs e)
		{
			_accumulatedEventData = PlotItemDataChangedEventArgs.Empty;
		}

		#endregion Changed event handling

		private class MyPlotData
		{
			public double[] _xPhysical;
			public double[] _yPhysical;

			public Altaxo.Data.AltaxoVariant GetXPhysical(int originalRowIndex)
			{
				return _xPhysical[originalRowIndex];
			}

			public Altaxo.Data.AltaxoVariant GetYPhysical(int originalRowIndex)
			{
				return _yPhysical[originalRowIndex];
			}
		}

		/// <summary>
		/// This will create a point list out of the data, which can be used to plot the data. In order to create this list,
		/// the function must have knowledge how to calculate the points out of the data. This will be done
		/// by a function provided by the calling function.
		/// </summary>
		/// <param name="layer">The plot layer.</param>
		/// <returns>An array of plot points in layer coordinates.</returns>
		public Processed2DPlotData GetRangesAndPoints(
				Gdi.IPlotArea layer)
		{
			const int functionPoints = 1000;
			const double MaxRelativeValue = 1E6;

			// allocate an array PointF to hold the line points
			PointF[] ptArray = new PointF[functionPoints];
			Processed2DPlotData result = new Processed2DPlotData();
			MyPlotData pdata = new MyPlotData();
			result.PlotPointsInAbsoluteLayerCoordinates = ptArray;
			double[] xPhysArray = new double[functionPoints];
			double[] yPhysArray = new double[functionPoints];
			pdata._xPhysical = xPhysArray;
			pdata._yPhysical = yPhysArray;
			result.XPhysicalAccessor = new IndexedPhysicalValueAccessor(pdata.GetXPhysical);
			result.YPhysicalAccessor = new IndexedPhysicalValueAccessor(pdata.GetYPhysical);

			// double xorg = layer.XAxis.Org;
			// double xend = layer.XAxis.End;
			// Fill the array with values
			// only the points where x and y are not NaNs are plotted!

			int i, j;

			bool bInPlotSpace = true;
			int rangeStart = 0;
			PlotRangeList rangeList = new PlotRangeList();
			result.RangeList = rangeList;
			Gdi.G2DCoordinateSystem coordsys = layer.CoordinateSystem;

			var xaxis = layer.XAxis;
			var yaxis = layer.YAxis;
			if (xaxis == null || yaxis == null)
				return null;

			for (i = 0, j = 0; i < functionPoints; i++)
			{
				double x_rel = ((double)i) / (functionPoints - 1);
				var x_variant = xaxis.NormalToPhysicalVariant(x_rel);
				double x = x_variant.ToDouble();
				double y = Evaluate(x);

				if (Double.IsNaN(x) || Double.IsNaN(y))
				{
					if (!bInPlotSpace)
					{
						bInPlotSpace = true;
						rangeList.Add(new PlotRange(rangeStart, j));
					}
					continue;
				}

				// double x_rel = layer.XAxis.PhysicalToNormal(x);
				double y_rel = yaxis.PhysicalVariantToNormal(y);

				// chop relative values to an range of about -+ 10^6
				if (y_rel > MaxRelativeValue)
					y_rel = MaxRelativeValue;
				if (y_rel < -MaxRelativeValue)
					y_rel = -MaxRelativeValue;

				// after the conversion to relative coordinates it is possible
				// that with the choosen axis the point is undefined
				// (for instance negative values on a logarithmic axis)
				// in this case the returned value is NaN
				double xcoord, ycoord;
				if (coordsys.LogicalToLayerCoordinates(new Logical3D(x_rel, y_rel), out xcoord, out ycoord))
				{
					if (bInPlotSpace)
					{
						bInPlotSpace = false;
						rangeStart = j;
					}
					xPhysArray[j] = x;
					yPhysArray[j] = y;
					ptArray[j].X = (float)xcoord;
					ptArray[j].Y = (float)ycoord;
					j++;
				}
				else
				{
					if (!bInPlotSpace)
					{
						bInPlotSpace = true;
						rangeList.Add(new PlotRange(rangeStart, j));
					}
				}
			} // end for
			if (!bInPlotSpace)
			{
				bInPlotSpace = true;
				rangeList.Add(new PlotRange(rangeStart, j)); // add the last range
			}
			return result;
		}
	}

	#endregion XYFunctionPlotData
}