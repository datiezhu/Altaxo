﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2015 Dr. Dirk Lellinger
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

using Altaxo.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altaxo.Graph.Graph3D.GraphicsContext.D3D
{
	public class PositionUVIndexedTriangleBuffer : IndexedTriangleBuffer, IPositionUVIndexedTriangleBuffer
	{
		public PositionUVIndexedTriangleBuffer(ITransformationContext parent)
			: base(parent)
		{
		}

		protected override int BytesPerVertex { get { return 6 * 4; } }

		public void AddTriangleVertex(double x, double y, double z, double u, double v)
		{
			var pt = _parent.Transformation.TransformPoint(new PointD3D(x, y, z));

			int offs = _numberOfVertices * 6;

			if (offs + 6 >= _vertexStream.Length)
				Array.Resize(ref _vertexStream, _vertexStream.Length * 2);

			_vertexStream[offs + 0] = (float)pt.X;
			_vertexStream[offs + 1] = (float)pt.Y;
			_vertexStream[offs + 2] = (float)pt.Z;
			_vertexStream[offs + 3] = 1;

			_vertexStream[offs + 4] = (float)u;
			_vertexStream[offs + 5] = (float)v;
			++_numberOfVertices;
		}
	}
}