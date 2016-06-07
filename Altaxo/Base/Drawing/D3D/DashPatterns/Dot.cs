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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Drawing.D3D.DashPatterns
{
	public class Dot : DashPatternBase
	{
		#region Serialization

		/// <summary>
		/// 2016-04-22 initial version.
		/// </summary>
		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(Dot), 0)]
		private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				return new Dot();
			}
		}

		#endregion Serialization

		public override double this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return 1;

					case 1:
						return 1;

					default:
						throw new IndexOutOfRangeException(nameof(index));
				}
			}
			set
			{
				throw new InvalidOperationException("Sorry, this class is read-only");
			}
		}

		public override int Count
		{
			get
			{
				return 2;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is Dot;
		}

		public override int GetHashCode()
		{
			return 0xEDD4F40;
		}
	}
}