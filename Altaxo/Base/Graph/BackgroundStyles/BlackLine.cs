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
namespace Altaxo.Graph.BackgroundStyles
{
  /// <summary>
  /// Backs the item with a color filled rectangle.
  /// </summary>
  public class BlackLine : IBackgroundStyle
  {
    protected BrushHolder _brush;

    #region Serialization

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(BlackLine), 0)]
      public class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        BlackLine s = (BlackLine)obj;
       

      }
      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        BlackLine s = null != o ? (BlackLine)o : new BlackLine();
       

        return s;
      }
    }

    #endregion


    public BlackLine()
    {
    }

   

    public BlackLine(BlackLine from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(BlackLine from)
    {
      
    }

    public object Clone()
    {
      return new BlackLine(this);
    }



    #region IBackgroundStyle Members

    public System.Drawing.RectangleF MeasureItem(System.Drawing.Graphics g, System.Drawing.RectangleF innerArea)
    {
      return innerArea;
    }

    public void Draw(System.Drawing.Graphics g, System.Drawing.RectangleF innerArea)
    {
      g.DrawRectangle(Pens.Black, innerArea.Left, innerArea.Top, innerArea.Width, innerArea.Height);
    }

    public bool SupportsColor { get { return false; } }

    public Color Color
    {
      get
      {
        return  Color.Black;
      }
      set
      {
        
      }
    }
    #endregion
  }
}
