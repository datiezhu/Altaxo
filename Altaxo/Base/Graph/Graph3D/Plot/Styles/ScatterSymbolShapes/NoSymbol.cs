﻿#region Copyright

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

using Altaxo.Drawing.D3D;
using Altaxo.Geometry;
using Altaxo.Graph.Graph3D.GraphicsContext;
using Altaxo.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Altaxo.Graph.Graph3D.Plot.Styles.ScatterSymbolShapes
{
	/// <summary>
	/// Represents the null symbol in a scatter plot, i.e. this symbol is not visible.
	/// </summary>
	/// <seealso cref="Altaxo.Graph.Graph3D.Plot.Styles.IScatterSymbolShape" />
	public sealed class NoSymbol : IScatterSymbolShape
	{
		public static NoSymbol Instance { get; private set; } = new NoSymbol();

		private NoSymbol()
		{
		}

		public void Paint(IGraphicsContext3D g, IMaterial material, PointD3D centerLocation, double symbolSize)
		{
		}
	}
}