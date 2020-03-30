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
using System.Drawing.Drawing2D;
using System.Text;

namespace Altaxo.Graph.Gdi.LineCaps
{
  /// <summary>
  /// Draws a cap that is a line perpendicular to the end of the line, and on the right side of the line.
  /// </summary>
  public class RightBarLineCap : GdiLineCapBase
  {
    public override Type ExtendsType => typeof(Altaxo.Drawing.LineCaps.RightBarLineCap);

    protected override CustomLineCap GetCustomLineCap(Pen pen, float size, bool startCap)
    {
      float endPoint;

      endPoint = pen.Width == 0 ? 1 : size / pen.Width;

      if (startCap)
        endPoint = -endPoint;

      var hPath = new GraphicsPath();
      // Create the outline for our custom end cap.
      hPath.AddLine(new PointF(endPoint < 0 ? 0.5f : -0.5f, 0), new PointF(endPoint / 2, 0));
      var clone = new CustomLineCap(null, hPath); // we set the stroke path only
      clone.SetStrokeCaps(LineCap.Flat, LineCap.Flat);
      return clone;
    }


  }
}
