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
using System.Text;

namespace Altaxo.Graph.Scales.Boundaries
{
  public class DummyNumericalBoundaries : NumericalBoundaries
  {
    #region Serialization

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(DummyNumericalBoundaries), 0)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
      }

      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var s = (DummyNumericalBoundaries)o ?? new DummyNumericalBoundaries();
        return s;
      }
    }

    #endregion Serialization

    public DummyNumericalBoundaries()
    {
    }

    public DummyNumericalBoundaries(DummyNumericalBoundaries from)
      : base(from)
    {
    }

    public override bool Add(Altaxo.Data.IReadableColumn col, int idx)
    {
      return true;
    }

    public override bool Add(Altaxo.Data.AltaxoVariant item)
    {
      return true;
    }

    public override object Clone()
    {
      return new DummyNumericalBoundaries(this);
    }

    protected override void AccumulateChangeData(object sender, EventArgs e)
    {
    }
  }
}
