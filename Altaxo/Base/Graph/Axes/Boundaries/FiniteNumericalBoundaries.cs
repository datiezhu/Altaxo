#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2005 Dr. Dirk Lellinger
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
using Altaxo.Serialization;

namespace Altaxo.Graph.Axes.Boundaries
{
  /// <summary>
  /// FinitePhysicalBoundaries is intended to use for LinearAxis
  /// it keeps track of the most negative and most positive value
  /// </summary>
  [SerializationSurrogate(0,typeof(FiniteNumericalBoundaries.SerializationSurrogate0))]
  [SerializationVersion(0)]
  public class FiniteNumericalBoundaries : NumericalBoundaries
  {
    #region Serialization
    /// <summary>Used to serialize the FinitePhysicalBoundaries Version 0.</summary>
    public new class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
    {
      /// <summary>
      /// Serializes FinitePhysicalBoundaries Version 0.
      /// </summary>
      /// <param name="obj">The FinitePhysicalBoundaries to serialize.</param>
      /// <param name="info">The serialization info.</param>
      /// <param name="context">The streaming context.</param>
      public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context  )
      {
        FiniteNumericalBoundaries s = (FiniteNumericalBoundaries)obj;
        // get the surrogate selector of the base class
        System.Runtime.Serialization.ISurrogateSelector ss = AltaxoStreamingContext.GetSurrogateSelector(context);
        if(null!=ss)
        {
          System.Runtime.Serialization.ISerializationSurrogate surr =
            ss.GetSurrogate(obj.GetType().BaseType,context, out ss);

          // serialize the base class
          surr.GetObjectData(obj,info,context); // stream the data of the base object
        }
        else 
        {
          throw new NotImplementedException(string.Format("Serializing a {0} without surrogate not implemented yet!",obj.GetType()));
        }
      }
      /// <summary>
      /// Deserializes the FinitePhysicalBoundaries Version 0.
      /// </summary>
      /// <param name="obj">The empty FinitePhysicalBoundaries object to deserialize into.</param>
      /// <param name="info">The serialization info.</param>
      /// <param name="context">The streaming context.</param>
      /// <param name="selector">The deserialization surrogate selector.</param>
      /// <returns>The deserialized FinitePhysicalBoundaries.</returns>
      public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
      {
        FiniteNumericalBoundaries s = (FiniteNumericalBoundaries)obj;
        // get the surrogate selector of the base class
        System.Runtime.Serialization.ISurrogateSelector ss = AltaxoStreamingContext.GetSurrogateSelector(context);
        if(null!=ss)
        {
          System.Runtime.Serialization.ISerializationSurrogate surr =
            ss.GetSurrogate(obj.GetType().BaseType,context, out ss);
          // deserialize the base class
          surr.SetObjectData(obj,info,context,selector);
        }
        else 
        {
          throw new NotImplementedException(string.Format("Serializing a {0} without surrogate not implemented yet!",obj.GetType()));
        }

        return s;
      }
    }

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase","Altaxo.Graph.FinitePhysicalBoundaries",0)]
      [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(FiniteNumericalBoundaries),1)]
      public new class XmlSerializationSurrogate1 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        FiniteNumericalBoundaries s = (FiniteNumericalBoundaries)obj;
        info.AddBaseValueEmbedded(s,typeof(FiniteNumericalBoundaries).BaseType);
      }
      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        
        FiniteNumericalBoundaries s = null!=o ? (FiniteNumericalBoundaries)o : new FiniteNumericalBoundaries();
        info.GetBaseValueEmbedded(s,typeof(FiniteNumericalBoundaries).BaseType,parent);
        return s;
      }
    }

    /// <summary>
    /// Finale measures after deserialization.
    /// </summary>
    /// <param name="obj">Not used.</param>
    public override void OnDeserialization(object obj)
    {
    }
    #endregion

    public FiniteNumericalBoundaries()
      : base()
    {
    }

    public FiniteNumericalBoundaries(FiniteNumericalBoundaries c)
      : base(c)
    {
    }

    public override object Clone()
    {
      return new FiniteNumericalBoundaries(this);
    }


    public override bool Add(Altaxo.Data.IReadableColumn col, int idx)
    {
      // if column is not numeric, use the index instead
      double d = (col is Altaxo.Data.INumericColumn) ? ((Altaxo.Data.INumericColumn)col)[idx] : idx;
  
      if(EventsEnabled)
      {
        if(!double.IsInfinity(d))
        {
          bool bLower=false, bUpper=false;
          if(d<minValue) { minValue = d; bLower=true; }
          if(d>maxValue) { maxValue = d; bUpper=true; }
          numberOfItems++;
  
          OnNumberOfItemsChanged();

          if(bLower || bUpper) 
            OnBoundaryChanged(bLower,bUpper);
  
          return true;
        }
      }
      else // Events not enabled
      {
        if(!double.IsInfinity(d))
        {
          if(d<minValue) minValue = d;
          if(d>maxValue) maxValue = d;
          numberOfItems++;
          return true;
        }
      }
    
  
      return false;
    }
  }

}
