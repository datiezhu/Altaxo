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

namespace Altaxo.Drawing.LineCaps
{
  /// <summary>
  /// Draws a cap that is a line perpendicular to the end of the line, and on the right side of the line.
  /// </summary>
  public class RightBarLineCap : LineCapBase
  {
    public RightBarLineCap()
    {
    }

    public RightBarLineCap(double minimumAbsoluteSizePt, double minimumRelativeSize)
      : base(minimumAbsoluteSizePt, minimumRelativeSize)
    {
    }

    public override string Name { get { return "BarRight"; } }

    public override double DefaultMinimumAbsoluteSizePt { get { return 8; } }

    public override double DefaultMinimumRelativeSize { get { return 4; } }
  }
}
