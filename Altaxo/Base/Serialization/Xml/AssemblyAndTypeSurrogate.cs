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

namespace Altaxo.Serialization.Xml
{
  /// <summary>
  /// Summary description for AssemblyAndTypeSurrogate.
  /// </summary>
  ///
  public class AssemblyAndTypeSurrogate
  {
    private string _assemblyName;
    private string _typeName;

    #region Serialization

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(AssemblyAndTypeSurrogate), 0)]
    public class XmlSerializationSurrogate : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (AssemblyAndTypeSurrogate)obj;

        info.AddValue("AssemblyName", s._assemblyName);
        info.AddValue("TypeName", s._typeName);
      }

      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        AssemblyAndTypeSurrogate s = o == null ? new AssemblyAndTypeSurrogate() : (AssemblyAndTypeSurrogate)o;

        s._assemblyName = info.GetString("AssemblyName");
        s._typeName = info.GetString("TypeName");

        return s;
      }
    }

    #endregion Serialization

    protected AssemblyAndTypeSurrogate()
    {
    }

    public AssemblyAndTypeSurrogate(object o)
    {
      if (o == null)
        throw new ArgumentNullException("To determine the type, the argument must not be null");
      _assemblyName = o.GetType().Assembly.FullName;
      _typeName = o.GetType().FullName;
    }

    public object CreateInstance()
    {
      try
      {
#if NETFRAMEWORK
        System.Runtime.Remoting.ObjectHandle oh = System.Activator.CreateInstance(_assemblyName, _typeName);
        return oh.Unwrap();
#else
        throw new NotImplementedException("Need to find Core function for line below");
#endif
      }
      catch (Exception)
      {
      }
      return null;
    }
  }
}
