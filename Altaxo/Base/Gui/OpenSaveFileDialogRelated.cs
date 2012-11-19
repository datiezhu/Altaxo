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
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Gui
{
  /// <summary>
  /// Data to control the open file dialog and to get the results.
  /// </summary>
	public class OpenFileOptions
	{
		List<KeyValuePair<string, string>> _filterList = new List<KeyValuePair<string, string>>();

    /// <summary>
    /// Adds a file filter string to show in the open file dialog.
    /// </summary>
    /// <param name="filter">The file filters, separated by semicolon. See example below.</param>
    /// <param name="description">The description of the filter. See example below..</param>
    /// <example>
    /// <code>
    ///	options.AddFilter("*.csv;*.dat;*.txt", "Text files (*.csv;*.dat;*.txt)");
		///	options.AddFilter("*.*", "All files (*.*)");
    /// </code>
    /// </example>
		public void AddFilter(string filter, string description)
		{
  		_filterList.Add(new KeyValuePair<string, string>(filter, description));
		}

    /// <summary>
    /// Provides read-only to the filter list.
    /// </summary>
    /// <value>
    /// The filter list.
    /// </value>
		public IList<KeyValuePair<string,string>> FilterList
		{
			get
			{
				return _filterList.AsReadOnly();
			}
		}
    
    /// <summary>
    /// Gets or sets the title of the open file dialog.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
		public string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the index of the filter that is shown by default.
    /// </summary>
    /// <value>
    /// The index of the default file filter.
    /// </value>
		public int FilterIndex { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether multiple files can be selected.
    /// </summary>
    /// <value>
    ///   <c>true</c> if  multiple files can be selected; otherwise, <c>false</c>.
    /// </value>
		public bool Multiselect { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to restore the directory.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [restore directory]; otherwise, <c>false</c>.
    /// </value>
		public bool RestoreDirectory { get; set; }

    /// <summary>
    /// Gets or sets the initial directory.
    /// </summary>
    /// <value>
    /// The initial directory.
    /// </value>
		public string InitialDirectory { get; set; }

    /// <summary>
    /// Gets or sets the file names (only if multiselect is on).
    /// </summary>
    /// <value>
    /// The file names.
    /// </value>
		public string[] FileNames { get; set; }

    /// <summary>
    /// Gets or sets the name of the file the user has chosen.
    /// </summary>
    /// <value>
    /// The name of the file.
    /// </value>
		public string FileName { get; set; }
	}


  /// <summary>
  /// Data to control the save file dialog and to get the results.
  /// </summary>
	public class SaveFileOptions : OpenFileOptions
	{
	}
}
