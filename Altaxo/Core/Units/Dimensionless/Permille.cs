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
using System.Linq;
using System.Text;

#nullable enable

namespace Altaxo.Units.Dimensionless
{
  [UnitDescription("Relation", 0, 0, 0, 0, 0, 0, 0)]
  public class Permille : UnitBase, IUnit
  {
    private static readonly Permille _instance = new Permille();

    public static Permille Instance { get { return _instance; } }

    protected Permille()
    {
    }

    public string Name
    {
      get { return "Per mill"; }
    }

    public string ShortCut
    {
      get { return "‰"; }
    }

    public double ToSIUnit(double x)
    {
      return x / 1000;
    }

    public double FromSIUnit(double x)
    {
      return x * 1000;
    }

    public ISIPrefixList Prefixes
    {
      get { return SIPrefix.ListWithNonePrefixOnly; }
    }

    public SIUnit SIUnit
    {
      get { return Unity.Instance; }
    }
  }
}
