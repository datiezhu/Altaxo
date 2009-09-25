#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2007 Dr. Dirk Lellinger
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
using Altaxo;

namespace Altaxo.Data
{
  /// <summary>
  /// Summary description for Altaxo.Data.DoubleColumn.
  /// </summary>
  [SerializationSurrogate(0,typeof(Altaxo.Data.DoubleColumn.SerializationSurrogate0))]
  [SerializationVersion(0)]
  [Serializable()]
  public class DoubleColumn 
    :
    Altaxo.Data.DataColumn,
    System.Runtime.Serialization.ISerializable,
    System.Runtime.Serialization.IDeserializationCallback,
    INumericColumn
  {
    private double[] _data;
    private int      _capacity; // shortcut to m_Array.Length;
    private int        _count;
    public static readonly double NullValue = Double.NaN;

    #region Overridden functions

    public override object Clone()
    {
      return new DoubleColumn(this);
    }

    public override int Count
    {
      get
      {
        return _count;
      }
    }

    // indexers
    public override void SetValueAt(int i, AltaxoVariant val)
    {
      try
      {
        this[i] = val.ToDouble();
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format("Error: Try to set {0}[{1}] with the string {2}, exception: {3}", this.TypeAndName, i, val.ToString(), ex.Message));
      }
    }

    public override AltaxoVariant GetVariantAt(int i)
    {
      return new AltaxoVariant(this[i]);
    }



    public override bool IsElementEmpty(int i)
    {
      return i < _count ? Double.IsNaN(_data[i]) : true;
    }


    public override void SetElementEmpty(int i)
    {
      if (i < _count)
        this[i] = NullValue;
    }

    public override void RemoveRows(int nDelFirstRow, int nDelCount)
    {
      if (nDelFirstRow < 0)
        throw new ArgumentException("Row number must be greater or equal 0, but was " + nDelFirstRow.ToString(), "nDelFirstRow");

      if (nDelCount <= 0)
        return; // nothing to do here, but we dont catch it

      // we must be careful, since the range to delete can be
      // above the range this column actually holds, but
      // we must handle this the right way
      int i, j;
      for (i = nDelFirstRow, j = nDelFirstRow + nDelCount; j < _count; i++, j++)
        _data[i] = _data[j];

      int prevCount = _count;
      _count = i < _count ? i : _count; // m_Count can only decrease

      if (_count != prevCount) // raise a event only if something really changed
        this.NotifyDataChanged(nDelFirstRow, prevCount, true);
    }

    public override void InsertRows(int nInsBeforeColumn, int nInsCount)
    {
      if (nInsCount <= 0 || nInsBeforeColumn >= Count)
        return; // nothing to do

      int newlen = this._count + nInsCount;
      if (newlen > _capacity)
        Realloc(newlen);

      // copy values from m_Count downto nBeforeColumn 
      for (int i = _count - 1, j = newlen - 1; i >= nInsBeforeColumn; i--, j--)
        _data[j] = _data[i];

      for (int i = nInsBeforeColumn + nInsCount - 1; i >= nInsBeforeColumn; i--)
        _data[i] = NullValue;

      this._count = newlen;
      this.NotifyDataChanged(nInsBeforeColumn, _count, false);
    }

    public override void CopyDataFrom(Altaxo.Data.DataColumn v)
    {
      if (v.GetType() != typeof(Altaxo.Data.DoubleColumn))
      {
        throw new ArgumentException("Try to copy " + v.GetType() + " to " + this.GetType(), "v"); // throw exception
      }

      this.CopyDataFrom(((Altaxo.Data.DoubleColumn)v)._data, v.Count);
    }     

    public override System.Type GetColumnStyleType()
    {
      return typeof(Altaxo.Worksheet.DoubleColumnStyle);
    }
    #endregion


    public DoubleColumn()
    {
    }

    
  
    public DoubleColumn(int initialcapacity)
    {
      _count = 0;
      _data = new double[initialcapacity];
      _capacity = initialcapacity;
    }
  
    public DoubleColumn(DoubleColumn from)
    {
      this._count    = from._count; 
      this._capacity = from._capacity;
      this._data    = null==from._data ? null : (double[])from._data.Clone();
    }

   


    #region "Serialization"
    public new class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
    {
      public void GetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context)
      {
        Altaxo.Data.DoubleColumn s = (Altaxo.Data.DoubleColumn)obj;
        // get the surrogate selector of the base class
        System.Runtime.Serialization.ISurrogateSelector ss  = AltaxoStreamingContext.GetSurrogateSelector(context);
        if(null!=ss)
        {
          System.Runtime.Serialization.ISerializationSurrogate surr =
            ss.GetSurrogate(obj.GetType().BaseType,context, out ss);
  
          // serialize the base class
          surr.GetObjectData(obj,info,context); // stream the data of the base object
        }
        else
        {
          ((DataColumn)s).GetObjectData(info,context);
        }

        if(s._count!=s._capacity)
        {
          // instead of the data array itself, stream only the first m_Count
          // array elements, since only they contain data
          double[] streamarray = new Double[s._count];
          System.Array.Copy(s._data,streamarray,s._count);
          info.AddValue("Data",streamarray);
        }
        else // if the array is fully filled, we don't need to save a shrinked copy
        {
          info.AddValue("Data",s._data);
        }
      }
      public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
      {
        Altaxo.Data.DoubleColumn s = (Altaxo.Data.DoubleColumn)obj;
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
          ((DataColumn)s).SetObjectData(obj,info,context,selector);
        }

        s._data = (double[])(info.GetValue("Data",typeof(double[])));
        s._capacity = null==s._data ? 0 : s._data.Length;
        s._count = s._capacity;

        return s;
      }
    }

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(Altaxo.Data.DoubleColumn),0)]
      class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        Altaxo.Data.DoubleColumn s = (Altaxo.Data.DoubleColumn)obj;
        // serialize the base class
        info.AddBaseValueEmbedded(s,typeof(Altaxo.Data.DataColumn));
        
        if(null==info.GetProperty("Altaxo.Data.DataColumn.SaveAsTemplate"))
          info.AddArray("Data",s._data,s._count);
        else
          info.AddArray("Data",s._data,0);
      }
      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        Altaxo.Data.DoubleColumn s = null!=o ? (Altaxo.Data.DoubleColumn)o : new Altaxo.Data.DoubleColumn();

        
        // deserialize the base class
        info.GetBaseValueEmbedded(s,typeof(Altaxo.Data.DataColumn),parent);

        int count = info.GetInt32Attribute("Count");
        s._data = new double[count];
        info.GetArray(s._data,count);
        s._capacity = null==s._data ? 0 : s._data.Length;
        s._count = s._capacity;

        return s;
      }
    }

    public override void OnDeserialization(object obj)
    {
      base.OnDeserialization(obj);
    }
  
    protected DoubleColumn(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
      SetObjectData(this,info,context,null);
    }
    public new object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
    {
      return new SerializationSurrogate0().SetObjectData(this,info,context,null);
    }
    public new void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
      new SerializationSurrogate0().GetObjectData(this,info,context);
    }

    #endregion

   

    public double[] Array
    {
      get 
      {
        int len = this.Count;
        double[] arr = new double[len];
        System.Array.Copy(_data,0,arr,0,len);
        return arr;
      }

      set
      {
        _data = (double[])value.Clone();
        this._count = _data.Length;
        this._capacity = _data.Length;
        this.NotifyDataChanged(0,_count,true);
      }
    }

    protected internal double GetValueDirect(int idx)
    {
      return _data[idx];
    }

  
        
        
     

    /// <summary>
    /// Returns the used length of the array. This is one plus the highest index of the number different from Double.NaN.
    /// </summary>
    /// <param name="values">The array for which the used length has to be determined.</param>
    /// <param name="currentlength">The current length of the array. Normally values.Length, but you can provide a value less than this.</param>
    /// <returns>The used length, i.e. numbers above the used length until the end of the array are NaNs.</returns>
    static public int GetUsedLength(double[] values, int currentlength)
    {
      for(int i=currentlength-1;i>=0;i--)
      {
        if(!Double.IsNaN(values[i]))
          return i+1;
      }
      return 0;
    }

    public static explicit operator DoubleColumn(double[] src)
    {
      DoubleColumn c = new DoubleColumn();
      c.CopyDataFrom(src);
      return c;
    }

    /// <summary>
    /// Copies the data from an array into the column. All data in the source array is copied.
    /// </summary>
    /// <param name="srcarray">The source array.</param>
    public void CopyDataFrom(double[] srcarray)
    {
      CopyDataFrom(srcarray,srcarray.Length);
    }

    /// <summary>
    /// Copies the data from an array into the column. The data from index 0 until <c>count-1</c> is copied to the destination.
    /// </summary>
    /// <param name="srcarray">Array containing the source data.</param>
    /// <param name="count">Length of the array (or length of the used range of the array, starting from index 0).</param>
    public void CopyDataFrom(double[] srcarray, int count)
    {
      int oldCount = this._count;
      int srcarraycount=0;

      if(null==srcarray || 0==(srcarraycount=GetUsedLength(srcarray,Math.Min(srcarray.Length,count))))
      {
        _data=null;
        _capacity=0;
        _count=0;
      }
      else
      {
        if(_capacity<srcarraycount)
          _data = new double[srcarraycount];
        System.Array.Copy(srcarray,_data,srcarraycount);
        _capacity = _data.Length;
        _count = srcarraycount;
      }
      if(oldCount>0 || _count>0) // message only if really was a change
        NotifyDataChanged(0,oldCount>_count? (oldCount):(_count),_count<oldCount);
    }

    /// <summary>
    /// Provides a setter property to which a vector can be assigned to. Copies all elements of the vector to this column. 
    /// The getter property creates a wrapper for this data column that implements IVector. The length of the wrapped vector is set to the current Count of the DoubleColumn.
    /// </summary>
    public override Altaxo.Calc.LinearAlgebra.IROVector AssignVector
    {
      set
      {
        CopyDataFrom(value, value.Length); 
      }
    }

    /// <summary>
    /// Provides a setter property to which a readonly vector can be assigned to. Copies all elements of the readonly vector to this column. 
    /// The getter property creates a wrapper for this data column that implements IROVector. For short time use only, since it reflects changes in the data, but not in the length of the DoubleColumn.
    /// </summary>
    public override Altaxo.Calc.LinearAlgebra.IROVector ToROVector(int start, int count)
    {
        return new ROVector(this, start, count);
    }

    public override Altaxo.Calc.LinearAlgebra.IVector ToVector(int start, int count)
    {
      return new RWVector(this, start, count);
    }


    /// <summary>
    /// Copies the data from an read-only into the column. The data from index 0 until <c>count-1</c> is copied to the destination.
    /// </summary>
    /// <param name="srcarray">Vector containing the source data.</param>
    /// <param name="count">Length of the array (or length of the used range of the array, starting from index 0).</param>
    public void CopyDataFrom(Altaxo.Calc.LinearAlgebra.IROVector srcarray, int count)
    {
      int oldCount = this._count;
      int srcarraycount = 0;

      if (null == srcarray || 0 == (srcarraycount = Altaxo.Calc.LinearAlgebra.VectorMath.GetUsedLength(srcarray, Math.Min(srcarray.Length, count))))
      {
        _data = null;
        _capacity = 0;
        _count = 0;
      }
      else
      {
        if (_capacity < srcarraycount)
          _data = new double[srcarraycount];
        for (int i = 0; i < srcarraycount; ++i)
          _data[i] = srcarray[i];
        _capacity = _data.Length;
        _count = srcarraycount;
      }
      if (oldCount > 0 || _count > 0) // message only if really was a change
        NotifyDataChanged(0, oldCount > _count ? (oldCount) : (_count), _count < oldCount);
    }



    protected void Realloc(int i)
    {
      int newcapacity1 = (int)(_capacity*_increaseFactor+_addSpace);
      int newcapacity2 = i+_addSpace+1;
      int newcapacity = newcapacity1>newcapacity2 ? newcapacity1:newcapacity2;
        
      double[] newarray = new double[newcapacity];
      if(_count>0)
      {
        System.Array.Copy(_data,newarray,_count);
      }

      _data = newarray;
      _capacity = _data.Length;
    }

   
    public new double this[int i]
    {
      get
      {
        if(i>=0 && i<_count)
          return _data[i];
        return double.NaN;  
      }
      set
      {
        bool bCountDecreased=false;


        if(Double.IsNaN(value))
        {
          if(i>=0 && i<_count-1) // i is inside the used range
          {
            _data[i]=value;
          }
          else if(i==(_count-1)) // m_Count is then decreasing
          {
            for(_count=i; _count>0 && Double.IsNaN(_data[_count-1]); --_count);
            bCountDecreased=true;;
          }
          else // i is above the used area
          {
            return; // no need for a change notification here
          }
        }
        else // value is not NaN
        {
          if(i>=0 && i<_count) // i is inside the used range
          {
            _data[i]=value;
          }
          else if(i==_count && i<_capacity) // i is the next value after the used range
          {
            _data[i]=value;
            _count=i+1;
          }
          else if(i>_count && i<_capacity) // is is outside used range, but inside capacity of array
          {
            for(int k=_count;k<i;k++)
              _data[k]=Double.NaN; // fill range between used range and new element with voids
          
            _data[i]=value;
            _count=i+1;
          }
          else if(i>=0) // i is outside of capacity, then realloc the array
          {
            Realloc(i);

            for(int k=_count;k<i;k++)
              _data[k]=Double.NaN; // fill range between used range and new element with voids
          
            _data[i]=value;
            _count=i+1;
          }
        }
        NotifyDataChanged(i,i+1,bCountDecreased);
      } // end set  
    } // end indexer



    #region Vector decorators

    private class ROVector : Altaxo.Calc.LinearAlgebra.IROVector
    {
      DoubleColumn _col;
      int _start;
      int _count;

      public ROVector(DoubleColumn col, int start, int count)
      {
        _col = col;
        _start = start;
        _count = count;
      }

      #region IROVector Members

      public int LowerBound
      {
        get { return 0; }
      }

      public int UpperBound
      {
        get { return _count - 1; }
      }

      public int Length
      {
        get { return _count; }
      }

      #endregion

      #region INumericSequence Members

      public double this[int i]
      {
        get { return _col[_start+i]; }
      }

      #endregion
    }

    private class RWVector : Altaxo.Calc.LinearAlgebra.IVector
    {
      DoubleColumn _col;
      int _start;
      int _count;

      public RWVector(DoubleColumn col, int start, int count)
      {
        _col = col;
        _start = start;
        _count = count;
      }

      #region IVector Members

      public double this[int i]
      {
        get
        {
          return _col[_start+i];
        }
        set
        {
          _col[_start+i] = value;
        }
      }

      #endregion

      #region IROVector Members

      public int LowerBound
      {
        get { return 0; }
      }

      public int UpperBound
      {
        get { return _count - 1; }
      }

      public int Length
      {
        get { return _count; }
      }

      #endregion
    }

    #endregion

    #region Operators

    // -----------------------------------------------------------------------------
    //
    //                        Operators
    //
    // -----------------------------------------------------------------------------


    // ----------------------- Addition operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator +(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i] + c2._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator +(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = c1._data[i] + c2;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator +(double c2, Altaxo.Data.DoubleColumn c1)
    {
      return c1+c2;
    }


    public override bool vop_Addition(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this + (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }
    public override bool vop_Addition_Rev(DataColumn c2, out DataColumn c3)
    {
      return vop_Addition(c2, out c3);
    }

    public override bool vop_Addition(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this + c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Addition_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      return vop_Addition(c2, out c3);
    }

    // --------------------- Operator Subtract -------------------------------------


    public static Altaxo.Data.DoubleColumn operator -(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i] - c2._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator -(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i]-c2;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator -(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c2 - c1._data[i];
      }
      c3._count=len;
      return c3;  
    }
    

    public override bool vop_Subtraction(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this - (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Subtraction_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 - this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Subtraction(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this - c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Subtraction_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 - this;
        return true;
      }
      c3=null;
      return false;
    }


    
    public static Altaxo.Data.DoubleColumn Subtraction(Altaxo.Data.DateTimeColumn c1, Altaxo.Data.DateTimeColumn c2)
    {
      int len = c1.Count<c2.Count ? c1.Count : c2.Count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = (c1.GetValueDirect(i)-c2.GetValueDirect(i)).TotalSeconds;
      }
      
      c3._count=len;
      
      return c3;  
    }


    public static Altaxo.Data.DoubleColumn Subtraction(Altaxo.Data.DateTimeColumn c1, DateTime c2)
    {
      int len = c1.Count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for (int i = 0; i < len; i++)
      {
        c3._data[i] = (c1.GetValueDirect(i) - c2).TotalSeconds;
      }

      c3._count = len;

      return c3;
    }

    public static Altaxo.Data.DoubleColumn Subtraction(DateTime c1, Altaxo.Data.DateTimeColumn c2)
    {
      int len = c2.Count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for (int i = 0; i < len; i++)
      {
        c3._data[i] = (c1 - c2.GetValueDirect(i)).TotalSeconds;
      }

      c3._count = len;

      return c3;
    }

    // ----------------------- Multiplication operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator *(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i] * c2._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator *(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = c1._data[i] * c2;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator *(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = c1._data[i] * c2;
      c3._count = len;
      return c3;
    }


    public override bool vop_Multiplication(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this * (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }
    public override bool vop_Multiplication_Rev(DataColumn c2, out DataColumn c3)
    {
      return vop_Multiplication(c2, out c3);
    }

    public override bool vop_Multiplication(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this * c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Multiplication_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      return vop_Multiplication(c2, out c3);
    }

    // ------------------------ Division operator --------------------------------

    public static Altaxo.Data.DoubleColumn operator /(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i] / c2._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator /(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i]/c2;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator /(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c2/c1._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public override bool vop_Division(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this / (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Division_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 / this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Division(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this / c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Division_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 / this;
        return true;
      }
      c3=null;
      return false;
    }


    // -------------------------- operator % ----------------------------------------------
    public static Altaxo.Data.DoubleColumn operator %(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i] % c2._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator %(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i] % c2;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator %(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c2%c1._data[i];
      }
      c3._count=len;
      return c3;  
    }

    public override bool vop_Modulo(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this % (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Modulo_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 % this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Modulo(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this % c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Modulo_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 % this;
        return true;
      }
      c3=null;
      return false;
    }


    // ----------------------- AND operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator &(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = ((long)c1._data[i]) & ((long)c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator &(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      long c22 = (long)c2;
      for(int i=0;i<len;i++)
        c3._data[i] = ((long)c1._data[i]) & c22;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator &(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      long c22 = (long)c2;
      for(int i=0;i<len;i++)
        c3._data[i] = c22 & ((long)c1._data[i]);
      c3._count = len;
      return c3;
    }


    public override bool vop_And(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this & (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_And_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 & this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_And(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this & c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_And_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 & this;
        return true;
      }
      c3=null;
      return false;
    }

    // ----------------------- OR operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator |(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = ((long)c1._data[i]) | ((long)c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator |(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      long c22 = (long)c2;
      for(int i=0;i<len;i++)
        c3._data[i] = ((long)c1._data[i]) | c22;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator |(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      long c22 = (long)c2;
      for(int i=0;i<len;i++)
        c3._data[i] = c22 | ((long)c1._data[i]);
      c3._count = len;
      return c3;
    }


    public override bool vop_Or(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this | (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Or_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 | this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Or(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this | c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Or_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 | this;
        return true;
      }
      c3=null;
      return false;
    }



    // ----------------------- XOR operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator ^(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = ((long)c1._data[i]) ^ ((long)c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator ^(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      long c22 = (long)c2;
      for(int i=0;i<len;i++)
        c3._data[i] = ((long)c1._data[i]) ^ c22;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator  ^(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      long c22 = (long)c2;
      for(int i=0;i<len;i++)
        c3._data[i] = c22 ^ ((long)c1._data[i]);
      c3._count = len;
      return c3;
    }


    public override bool vop_Xor(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this ^ (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Xor_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 ^ this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Xor(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this ^ c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Xor_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 ^ this;
        return true;
      }
      c3=null;
      return false;
    }

    // ----------------------- ShiftLeft operator -----------------------------------
  
    public static Altaxo.Data.DoubleColumn operator <<(Altaxo.Data.DoubleColumn c1, int c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ((long)c1._data[i]) << c2;
      c3._count = len;
      return c3;
    }



    public override bool vop_ShiftLeft(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        Altaxo.Data.DoubleColumn c1=this;
        Altaxo.Data.DoubleColumn c22 = (DoubleColumn)c2;
        int len = c1.Count<c2.Count ? c1.Count : c2.Count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        for(int i=0;i<len;i++)
        {
          c33._data[i] = ((long)c1._data[i]) << ((int)c22._data[i]);
        }
        c33._count=len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_ShiftLeft_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        Altaxo.Data.DoubleColumn c1=this;
        Altaxo.Data.DoubleColumn c22= (DoubleColumn)c2;

        int len = c1.Count<c2.Count ? c1.Count : c2.Count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        for(int i=0;i<len;i++)
        {
          c33._data[i] = ((long)c22._data[i]) << ((int)c1._data[i]);
        }
        c33._count=len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_ShiftLeft(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        int c22 = (int)(double)c2;
        c3 = this << c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_ShiftLeft_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        DoubleColumn c1=this;
        int len = c1._count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        long c22 = (long)(double)c2;
        for(int i=0;i<len;i++)
          c33._data[i] = c22 << ((int)c1._data[i]);
        c33._count = len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }


    // ----------------------- ShiftRight operator -----------------------------------

    public static Altaxo.Data.DoubleColumn operator >>(Altaxo.Data.DoubleColumn c1, int c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ((long)c1._data[i]) >> c2;
      c3._count = len;
      return c3;
    }



    public override bool vop_ShiftRight(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        DoubleColumn c1=this;
        DoubleColumn c22 = (DoubleColumn)c2;
        int len = c1.Count<c2.Count ? c1.Count : c2.Count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        for(int i=0;i<len;i++)
        {
          c33._data[i] = ((long)c1._data[i]) >> ((int)c22._data[i]);
        }
        c33._count=len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_ShiftRight_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        Altaxo.Data.DoubleColumn c1=this;
        DoubleColumn c22 = (DoubleColumn)c2;
        int len = c1.Count<c2.Count ? c1.Count : c2.Count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        for(int i=0;i<len;i++)
        {
          c33._data[i] = ((long)c22._data[i]) >> ((int)c1._data[i]);
        }
        c33._count=len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_ShiftRight(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        DoubleColumn c1=this;
        int len = c1._count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        int c22 = (int)(double)c2;
        for(int i=0;i<len;i++)
          c33._data[i] = ((long)c1._data[i]) >> c22;
        c33._count = len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_ShiftRight_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        DoubleColumn c1=this;
        int len = c1._count;
        Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
        long c22 = (long)(double)c2;
        for(int i=0;i<len;i++)
          c33._data[i] = c22 >> ((int)c1._data[i]);
        c33._count = len;
        c3=c33;
        return true;
      }
      c3=null;
      return false;
    }



    // ----------------------- Lesser operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator <(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = (c1._data[i] < c2._data[i]) ? 1 : 0;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator <(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c1._data[i] < c2 ) ? 1 : 0;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator  <(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c2 < c1._data[i] ) ? 1 : 0;
      c3._count = len;
      return c3;
    }


    public override bool vop_Lesser(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this < (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Lesser_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 < this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Lesser(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this < c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Lesser_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 < this;
        return true;
      }
      c3=null;
      return false;
    }


    // ----------------------- Greater operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator >(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = (c1._data[i] > c2._data[i]) ? 1 : 0;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator >(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c1._data[i] > c2 ) ? 1 : 0;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator >(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c2 > c1._data[i] ) ? 1 : 0;
      c3._count = len;
      return c3;
    }


    public override bool vop_Greater(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this > (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Greater_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 > this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Greater(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this > c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_Greater_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 > this;
        return true;
      }
      c3=null;
      return false;
    }




    // ----------------------- LesserOrEqual operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator <=(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = (c1._data[i] <= c2._data[i]) ? 1 : 0;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator <=(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c1._data[i] <= c2 ) ? 1 : 0;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator <=(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c2 <= c1._data[i] ) ? 1 : 0;
      c3._count = len;
      return c3;
    }


    public override bool vop_LesserOrEqual(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this <= (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_LesserOrEqual_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 <= this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_LesserOrEqual(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this <= c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_LesserOrEqual_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 <= this;
        return true;
      }
      c3=null;
      return false;
    }


    // ----------------------- GreaterOrEqual operator -----------------------------------
    public static Altaxo.Data.DoubleColumn operator >=(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = (c1._data[i] >= c2._data[i]) ? 1 : 0;
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn operator >=(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c1._data[i] >= c2 ) ? 1 : 0;
      c3._count = len;
      return c3;
    }

    public static Altaxo.Data.DoubleColumn operator >=(double c2, Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
        c3._data[i] = ( c2 >= c1._data[i] ) ? 1 : 0;
      c3._count = len;
      return c3;
    }


    public override bool vop_GreaterOrEqual(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = this >= (Altaxo.Data.DoubleColumn)c2;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_GreaterOrEqual_Rev(DataColumn c2, out DataColumn c3)
    {
      if(c2 is Altaxo.Data.DoubleColumn)
      {
        c3 = (Altaxo.Data.DoubleColumn)c2 >= this;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_GreaterOrEqual(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = this >= c22;
        return true;
      }
      c3=null;
      return false;
    }

    public override bool vop_GreaterOrEqual_Rev(AltaxoVariant c2, out DataColumn c3)
    {
      if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
      {
        double c22 = (double)c2;
        c3 = c22 >= this;
        return true;
      }
      c3=null;
      return false;
    }


    // --------------------------------- Unary Plus ----------------------------
    public static Altaxo.Data.DoubleColumn operator +(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i];
      }
      c3._count = len;
      return c3;  
    }


    public override bool vop_Plus(out DataColumn c3)
    {
      c3= +this;
      return true;
    }



    // --------------------------------- Unary Minus ----------------------------
    public static Altaxo.Data.DoubleColumn operator -(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = -c1._data[i];
      }
      c3._count = len;
      return c3;  
    }


    public override bool vop_Minus(out DataColumn c3)
    {
      c3= -this;
      return true;
    }

    // --------------------------------- Unary NOT ----------------------------
    public static Altaxo.Data.DoubleColumn operator !(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = 0==c1._data[i] ? 1 : 0;
      }
      c3._count = len;
      return c3;  
    }


    public override bool vop_Not(out DataColumn c3)
    {
      c3= !this;
      return true;
    }

    // --------------------------------- Unary Complement ----------------------------
    public static Altaxo.Data.DoubleColumn operator ~(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = ~((long)c1._data[i]);
      }
      c3._count = len;
      return c3;  
    }


    public override bool vop_Complement(out DataColumn c3)
    {
      c3= ~this;
      return true;
    }

    // --------------------------------- Unary Increment ----------------------------
    public static Altaxo.Data.DoubleColumn operator ++(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i]+1;
      }
      c3._count = len;
      return c3;  
    }


    public override bool vop_Increment(out DataColumn c3)
    {
      int len = this._count;
      Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c33._data[i] = this._data[i]+1;
      }
      c33._count = len;
      c3=c33;
      return true;
    }

    // --------------------------------- Unary Decrement ----------------------------
    public static Altaxo.Data.DoubleColumn operator --(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = c1._data[i]-1;
      }
      c3._count = len;
      return c3;  
    }


    public override bool vop_Decrement(out DataColumn c3)
    {
      int len = this._count;
      Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c33._data[i] = this._data[i]-1;
      }
      c33._count = len;
      c3=c33;
      return true;
    }


    // -----------------------------------------------------------------------------
    //
    //               arithmetic Functions
    //
    // -----------------------------------------------------------------------------

    public static Altaxo.Data.DoubleColumn Abs(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Abs(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Acos(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Acos(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Asin(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Asin(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 


    public static Altaxo.Data.DoubleColumn Atan(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Atan(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Atan2(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Atan2(c1._data[i],c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Atan2(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Atan2(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Atan2(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Atan2(c1,c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }


    public static Altaxo.Data.DoubleColumn Ceiling(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Ceiling(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Cos(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Cos(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Cosh(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Cosh(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Exp(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Exp(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Floor(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Floor(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn IEEERemainder(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.IEEERemainder(c1._data[i],c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn IEEERemainder(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.IEEERemainder(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn IEEERemainder(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.IEEERemainder(c1,c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

  
    
    public static Altaxo.Data.DoubleColumn Log(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Log(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Log(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Log(c1._data[i],c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Log(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Log(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Log(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Log(c1,c2._data[i]);
      }
      c3._count=len;
      return c3;
    }


    #region Log10
    public static Altaxo.Data.DoubleColumn Log10(Altaxo.Data.DoubleColumn c1)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for (int i = 0; i < len; i++)
      {
        c3._data[i] = System.Math.Log10(c1._data[i]);
      }
      c3._count = len;
      return c3;
    }
    #endregion


    public static Altaxo.Data.DoubleColumn Max(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Max(c1._data[i],c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Max(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Max(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Max(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Max(c1,c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    
    
    public static Altaxo.Data.DoubleColumn Min(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Min(c1._data[i],c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Min(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Min(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Min(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Min(c1,c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    
    
    public static Altaxo.Data.DoubleColumn Pow(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Pow(c1._data[i],c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Pow(Altaxo.Data.DoubleColumn c1, double c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Pow(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Pow(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Pow(c1,c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    
    
    public static Altaxo.Data.DoubleColumn Round(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Round(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Round(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c1._count<c2._count ? c1._count : c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Round(c1._data[i],(int)c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Round(Altaxo.Data.DoubleColumn c1, int c2)
    {
      int len = c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Round(c1._data[i],c2);
      }
      c3._count=len;
      return c3;  
    }

    public static Altaxo.Data.DoubleColumn Round(double c1, Altaxo.Data.DoubleColumn c2)
    {
      int len = c2._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Round(c1,(int)c2._data[i]);
      }
      c3._count=len;
      return c3;  
    }
    
    public static Altaxo.Data.DoubleColumn Sign(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Sign(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Sin(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Sin(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Sinh(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Sinh(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Sqrt(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Sqrt(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Tan(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Tan(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 

    public static Altaxo.Data.DoubleColumn Tanh(Altaxo.Data.DoubleColumn c1)
    {
      int len=c1._count;
      Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
      for(int i=0;i<len;i++)
      {
        c3._data[i] = System.Math.Tanh(c1._data[i]);
      }
      c3._count=len;
      return c3;
    } 
        
    #endregion

  } // end Altaxo.Data.DoubleColumn
}
