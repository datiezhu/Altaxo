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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;


namespace Altaxo.Graph
{
  [Serializable]
  public enum BrushType 
  {
    SolidBrush,
    HatchBrush, 
    TextureBrush, 
    LinearGradientBrush, 
    PathGradientBrush
  };

  [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(BrushType),0)]
  public class BrushTypeXmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
  {
    public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
    {
      info.SetNodeContent(obj.ToString()); 
    }
    public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
    {
      
      string val = info.GetNodeContent();
      return System.Enum.Parse(typeof(BrushType),val,true);
    }
  }
  
  /// <summary>
  /// Holds all information neccessary to create a brush
  /// of any kind without allocating resources, so this class
  /// can be made serializable.
  /// </summary>
  [Serializable]
  public class BrushHolder : System.ICloneable, System.IDisposable, ISerializable, IDeserializationCallback, Main.IChangedEventSource
  {

    protected BrushType m_BrushType; // Type of the brush
    protected Brush     m_Brush;      // this is the cached brush object

    protected Color     m_ForeColor; // Color of the brush
    protected Color     m_BackColor; // Backcolor of brush, f.i.f. HatchStyle brushes
    protected HatchStyle  m_HatchStyle; // f�r HatchBrush
    protected Image     m_Image; // f�r Texturebrush
    protected Matrix    m_Matrix; // f�r TextureBrush
    protected WrapMode  m_WrapMode; // f�r TextureBrush und LinearGradientBrush
    protected PointF    m_Point1;
    protected PointF    m_Point2;
    protected float     m_Float1;
    protected bool      m_Bool1;

    #region "Serialization"

    protected BrushHolder(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
      SetObjectData(this, info, context, null);
    }
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {

      info.AddValue("Type", m_BrushType);
      switch (m_BrushType)
      {
        case BrushType.SolidBrush:
          info.AddValue("ForeColor", m_ForeColor);
          break;
        case BrushType.HatchBrush:
          info.AddValue("ForeColor", m_ForeColor);
          info.AddValue("BackColor", m_BackColor);
          info.AddValue("HatchStyle", m_HatchStyle);
          break;
      } // end of switch
    }
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
      BrushHolder s = (BrushHolder)obj;
      s.m_BrushType = (BrushType)info.GetValue("Type", typeof(BrushType));
      switch (s.m_BrushType)
      {
        case BrushType.SolidBrush:
          s.m_ForeColor = (Color)info.GetValue("ForeColor", typeof(Color));
          break;
        case BrushType.HatchBrush:
          s.m_ForeColor = (Color)info.GetValue("ForeColor", typeof(Color));
          s.m_BackColor = (Color)info.GetValue("BackColor", typeof(Color));
          break;
      }
      return s;
    } // end of SetObjectData
   


    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(BrushHolder),0)]
      public class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        BrushHolder s = (BrushHolder)obj;
        info.AddValue("Type",s.m_BrushType);
        switch(s.m_BrushType)
        {
          case BrushType.SolidBrush:
            info.AddValue("ForeColor",s.m_ForeColor);
            break;
          case BrushType.HatchBrush:
            info.AddValue("ForeColor",s.m_ForeColor);
            info.AddValue("BackColor",s.m_BackColor);
            info.AddEnum("HatchStyle",s.m_HatchStyle);
            break;
        } // end of switch
      }
      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        
        BrushHolder s = null!=o ? (BrushHolder)o : new BrushHolder(Color.Black);

        s.m_BrushType  = (BrushType)info.GetValue("Type",s);
        switch(s.m_BrushType)
        {
          case BrushType.SolidBrush:
            s.m_ForeColor = (Color)info.GetValue("ForeColor",s);
            break;
          case BrushType.HatchBrush:
            s.m_ForeColor = (Color)info.GetValue("ForeColor",s);
            s.m_BackColor = (Color)info.GetValue("BackColor",s);
            s.m_HatchStyle = (HatchStyle)info.GetEnum("HatchStyle",typeof(HatchStyle));
            break;
        }
        return s;
      }
    }


    /// <summary>
    /// Finale measures after deserialization of the linear axis.
    /// </summary>
    /// <param name="obj">Not used.</param>
    public virtual void OnDeserialization(object obj)
    {
    }
    #endregion

    public BrushHolder(BrushHolder bh)
    {
      m_BrushType   = bh.m_BrushType; // Type of the brush
      m_Brush       = null==bh.m_Brush ? null : (Brush)bh.m_Brush.Clone();      // this is the cached brush object
      m_ForeColor   = bh.m_ForeColor; // Color of the brush
      m_BackColor   = bh.m_BackColor; // Backcolor of brush, f.i.f. HatchStyle brushes
      m_HatchStyle  = bh.m_HatchStyle; // f�r HatchBrush
      m_Image       = null==bh.m_Image ? null : (Image)bh.m_Image.Clone(); // f�r Texturebrush
      m_Matrix      = null==bh.m_Matrix ? null : (Matrix)bh.m_Matrix.Clone(); // f�r TextureBrush
      m_WrapMode    = bh.m_WrapMode; // f�r TextureBrush und LinearGradientBrush
      m_Point1      = bh.m_Point1;
      m_Point2      = bh.m_Point2;
      m_Float1      = bh.m_Float1;
      m_Bool1       = bh.m_Bool1;
    }

  
    public BrushHolder(Color c)
    {
      this.m_BrushType = BrushType.SolidBrush;
      this.m_ForeColor = c;
    }

    public static implicit operator System.Drawing.Brush(BrushHolder bh)
    {
      return bh.Brush;
    }
 

    public BrushType BrushType
    {
      get 
      {
        return this.m_BrushType; 
      }
      set
      {
        BrushType oldValue = this.m_BrushType;
        m_BrushType = value;
        if (m_BrushType != oldValue)
        {
          _SetBrushVariable(null);
          OnChanged();
        }
      }
    }

    public Color Color
    {
      get { return m_ForeColor; }
      set
      {
        bool bChanged = (m_ForeColor!=value);
        m_ForeColor = value;
        if (bChanged)
        {
          _SetBrushVariable(null);
          OnChanged();
        }
      }
    }

    public Color BackColor
    {
      get { return m_BackColor; }
      set
      {
        bool bChanged = (m_BackColor!=value);
        m_BackColor = value;
        if (bChanged)
        {
          _SetBrushVariable(null);
          OnChanged();
        }
      }
    }

    public HatchStyle HatchStyle
    {
      get
      {
        return m_HatchStyle;
      }
      set
      {
        bool bChanged = (m_HatchStyle != value);
        m_HatchStyle = value;
        if (bChanged)
        {
          _SetBrushVariable(null);
          OnChanged();
        }
      }
    }


    public Brush Brush
    {
      get
      {
        if (m_Brush == null)
        {
          Brush br = null;
          switch (m_BrushType)
          {
            case BrushType.SolidBrush:
              br = new SolidBrush(m_ForeColor);
              break;
            case BrushType.HatchBrush:
              br = new HatchBrush(m_HatchStyle, m_ForeColor, m_BackColor);
              break;
          } // end of switch
          this._SetBrushVariable(br);
        }
        return m_Brush;
      } // end of get
    /* set
      {
        if(value is SolidBrush)
        {
          m_BrushType = BrushType.SolidBrush;
          m_ForeColor = ((SolidBrush)value).Color;
        }
        else if(value is HatchBrush)
        {
          m_BrushType = BrushType.HatchBrush;
          m_ForeColor = ((HatchBrush)value).ForegroundColor;
          m_BackColor = ((HatchBrush)value).BackgroundColor;
          m_HatchStyle = ((HatchBrush)value).HatchStyle;
        }
        
        _SetBrushVariable(m_CachedMode ? (Brush)value.Clone() : null);

        OnChanged();
      } // end of set
     */
    } // end of prop. Brush



    public void SetSolidBrush(Color c)
    {
      m_BrushType = BrushType.SolidBrush;
      m_ForeColor     = c;
      _SetBrushVariable(null);
      OnChanged();
    }

    public void SetHatchBrush(HatchStyle hs, Color fc)
    {
      SetHatchBrush(hs,fc,Color.Black);
    }

    public void SetHatchBrush(HatchStyle hs, Color fc, Color bc)
    {
      m_BrushType = BrushType.HatchBrush;
      m_HatchStyle = hs;
      m_ForeColor = fc;
      m_BackColor = bc;

      _SetBrushVariable(null);
      OnChanged();
    }

    protected void _SetBrushVariable(Brush br)
    {
      if(null!=m_Brush)
        m_Brush.Dispose();

      m_Brush = br;
    }
    
    public object Clone()
    {
      return new BrushHolder(this);
    }

    public void Dispose()
    {
      if(null!=m_Brush)
        m_Brush.Dispose();
      m_Brush = null;
    }
    #region IChangedEventSource Members

    public event System.EventHandler Changed;

    protected virtual void OnChanged()
    {
      if(null!=Changed)
        Changed(this, new EventArgs());
    }

    #endregion
  } // end of class BrushHolder
} // end of namespace
