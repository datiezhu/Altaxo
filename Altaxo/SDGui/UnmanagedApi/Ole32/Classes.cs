﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2014 Dr. Dirk Lellinger
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

using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Altaxo.UnmanagedApi.Ole32
{
	[StructLayout(LayoutKind.Sequential)]
	public class COMRECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;

		public COMRECT()
		{
		}

		public COMRECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public static COMRECT FromXYWH(int x, int y, int width, int height)
		{
			return new COMRECT(x, y, x + width, y + height);
		}

		public override string ToString()
		{
			return string.Concat(new object[] { "Left = ", this.left, " Top ", this.top, " Right = ", this.right, " Bottom = ", this.bottom });
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public sealed class tagOLEVERB
	{
		public int lVerb;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszVerbName;

		[MarshalAs(UnmanagedType.U4)]
		public int fuFlags;

		[MarshalAs(UnmanagedType.U4)]
		public int grfAttribs;

		public tagOLEVERB()
		{
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public sealed class tagLOGPALETTE
	{
		[MarshalAs(UnmanagedType.U2)]
		public short palVersion;

		[MarshalAs(UnmanagedType.U2)]
		public short palNumEntries;

		public tagLOGPALETTE()
		{
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public sealed class tagSIZEL
	{
		public int cx;
		public int cy;

		public tagSIZEL()
		{
		}

		public tagSIZEL(int cx, int cy)
		{
			this.cx = cx; this.cy = cy;
		}

		public tagSIZEL(tagSIZEL o)
		{
			this.cx = o.cx; this.cy = o.cy;
		}
	}
}