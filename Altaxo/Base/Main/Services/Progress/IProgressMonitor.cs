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
using System.Threading;

namespace Altaxo.Main.Services
{
  /// <summary>
  /// Interface for the other site of a <see cref="IProgressReporter"/>, i.e. the site that reads the progress and bring it to display.
  /// </summary>
  public interface IProgressMonitor
  {
    /// <summary>
    /// Indicates that new report text has arrived that was not displayed yet.
    /// </summary>
    bool HasReportText { get; }

    /// <summary>
    /// Gets the report text. When called, the function has to reset the <see cref="HasReportText"/> flag.
    /// </summary>
    string GetReportText();

    /// <summary>Gets the progress as fraction. If you are not able to calculate the progress, this function should return <see cref="double.NaN"/>.</summary>
    /// <returns>The progress as fraction value [0..1], or <see cref="double.NaN"/>.</returns>
    double GetProgressFraction();
  }
}
