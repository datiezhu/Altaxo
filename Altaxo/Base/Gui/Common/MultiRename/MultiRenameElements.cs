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

namespace Altaxo.Gui.Common.MultiRename
{
	public interface IMultiRenameElement
	{
		/// <summary>Retrieves the content for this element by providing the original object index of the object and a sort order.</summary>
		/// <param name="originalListIndex">Index in the original object list of <see cref="MultiRenameData"/>.</param>
		/// <param name="currentSortIndex">Index of the current sort order (for instance when sorted in a view).</param>
		/// <returns>The string content of this element.</returns>
		string GetContent(int originalListIndex, int currentSortIndex);
	}

	public abstract class MultiRenameBaseElement : IMultiRenameElement
	{
		protected MultiRenameData _renameData;
		protected string _shortCut;

		public MultiRenameBaseElement(MultiRenameData multiRenameData, string shortCut)
		{
			_renameData = multiRenameData;
			_shortCut = shortCut;
		}



		/// <summary>Retrieves the content for this element by providing the original object index of the object and a sort order.</summary>
		/// <param name="originalListIndex">Index in the original object list of <see cref="MultiRenameData"/>.</param>
		/// <param name="currentSortIndex">Index of the current sort order (for instance when sorted in a view).</param>
		/// <returns>The string content of this element.</returns>
		public abstract string GetContent(int originalListIndex, int currentSortIndex);
	}

	public class MultiRenameLiteralElement : IMultiRenameElement
	{
		string _literal;

		public MultiRenameLiteralElement(string literal)
		{
			_literal = literal;
		}


		public string GetContent(int originalListIndex, int currentSortIndex)
		{
			return _literal;
		}
	}

	public class MultiRenameElementCollection : IMultiRenameElement
	{
		List<IMultiRenameElement> _elements = new List<IMultiRenameElement>();

		public MultiRenameElementCollection()
		{

		}

		public List<IMultiRenameElement> Elements { get { return _elements; } }

		public string GetContent(int originalListIndex, int currentSortIndex)
		{
			StringBuilder stb = new StringBuilder();
			foreach (var ele in _elements)
				stb.Append(ele.GetContent(originalListIndex, currentSortIndex));

			return stb.ToString();
		}
	}

	public class MultiRenameStringElement : MultiRenameBaseElement
	{
		int _start, _last;

		public MultiRenameStringElement(MultiRenameData multiRenameData, string shortCut, int start, int last)
			: base(multiRenameData, shortCut)
		{
			_start = start;
			_last = last;
		}

		public override string GetContent(int originalListIndex, int currentSortIndex)
		{
			string stringValue = _renameData.GetStringValueOfShortcut(_shortCut, originalListIndex, currentSortIndex);
			if (string.IsNullOrEmpty(stringValue))
				return string.Empty;

			if (_start < 0)
				_start += stringValue.Length;
			if (_last < 0)
				_last += stringValue.Length;

			_start = Math.Max(_start, 0);
			_last = Math.Max(_last, 0);
			_start = Math.Min(_start, stringValue.Length - 1);
			_last = Math.Min(_last, stringValue.Length - 1);

			if (_start <= _last)
				return stringValue.Substring(_start, _last - _start + 1);
			else
				return string.Empty;
		}
	}

	public class MultiRenameIntegerElement : MultiRenameBaseElement
	{
		int _numberOfDigits;
		int _offset;
		int _step;
		string _formatString;

		public MultiRenameIntegerElement(MultiRenameData multiRenameData, string shortCut, int numberOfDigits, int offset, int step)
			: base(multiRenameData, shortCut)
		{
			_numberOfDigits = numberOfDigits;
			_offset = offset;
			_step = step;
			_formatString = "D" + numberOfDigits.ToString();
		}

		public override string GetContent(int originalListIndex, int currentSortIndex)
		{
			int val = _renameData.GetIntegerValueOfShortcut(_shortCut, originalListIndex, currentSortIndex);

			int result = val * _step + _offset;

			if (_numberOfDigits <= 0)
			{
				return result.ToString();
			}
			else
			{
				return result.ToString(_formatString);
			}
		}
	}

	public class MultiRenameDateTimeElement : MultiRenameBaseElement
	{
		const string DefaultDateTimeFormat = "yyyy-dd-MM HH:mm:ss";
		string _dateTimeFormat;
		bool _useUtcTime;
		public MultiRenameDateTimeElement(MultiRenameData multiRenameData, string shortCut, string dateTimeFormat, bool useUtcTime)
			: base(multiRenameData, shortCut)
		{
			_dateTimeFormat = string.IsNullOrEmpty(dateTimeFormat) ? DefaultDateTimeFormat : dateTimeFormat;

			_useUtcTime = useUtcTime;

		}

		public override string GetContent(int originalListIndex, int currentSortIndex)
		{
			var val = _renameData.GetDateTimeValueOfShortcut(_shortCut, originalListIndex, currentSortIndex);
			if (_useUtcTime)
				val = val.ToUniversalTime();
			else
				val = val.ToLocalTime();

			try
			{
				return val.ToString(_dateTimeFormat);
			}
			catch (Exception)
			{
			}

			return val.ToString();
		}
	}

	public class MultiRenameArrayElement : MultiRenameBaseElement
	{
		int _start, _last;
		string _separator;

		public MultiRenameArrayElement(MultiRenameData multiRenameData, string shortCut, int start, int last, string separator)
			: base(multiRenameData, shortCut)
		{
			_start = start;
			_last = last;
			_separator = separator;
		}

		public override string GetContent(int originalListIndex, int currentSortIndex)
		{
			string[] arr = _renameData.GetStringArrayValueOfShortcut(_shortCut, originalListIndex, currentSortIndex);

			if (_start < 0)
				_start += arr.Length;
			if (_last < 0)
				_last += arr.Length;

			_start = Math.Max(_start, 0);
			_last = Math.Max(_last, 0);
			_start = Math.Min(_start, arr.Length - 1);
			_last = Math.Min(_last, arr.Length - 1);

			if (_start <= _last)
				return string.Join(_separator, arr, _start, _last - _start + 1);
			else
				return string.Empty;
		}
	}


}
