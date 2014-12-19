#region Copyright

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

using Altaxo.Main;
using System;

namespace Altaxo.Data
{
	/// <summary>
	/// Proxy that holds instances of type <see cref="INumericColumnProxy"/>.
	/// </summary>
	public interface INumericColumnProxy : IDocumentLeafNode, IProxy, ICloneable
	{
		/// <summary>
		/// Returns the holded object. Null can be returned if the object is no longer available (e.g. disposed).
		/// </summary>
		INumericColumn Document { get; }
	}

	/// <summary>
	/// Static class to create instances of <see cref="INumericColumnProxy"/>.
	/// </summary>
	public static class NumericColumnProxyBase
	{
		/// <summary>
		/// Creates an <see cref="INumericColumnProxy"/> from a given column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns>An instance of <see cref="INumericColumnProxy"/>. The type of instance returned depends on the type of the provided column (e.g. whether the column is part of the document or not).</returns>
		public static INumericColumnProxy FromColumn(INumericColumn column)
		{
			if (column is IDocumentLeafNode)
				return NumericColumnProxy.FromColumn(column);
			else
				return NumericColumnProxyForStandaloneColumns.FromColumn(column);
		}
	}

	public class NumericColumnProxyForStandaloneColumns : Main.SuspendableDocumentLeafNodeWithEventArgs, INumericColumnProxy
	{
		private INumericColumn _column;

		public static NumericColumnProxyForStandaloneColumns FromColumn(INumericColumn column)
		{
			var colAsDocumentNode = column as IDocumentLeafNode;
			if (null != colAsDocumentNode)
				throw new ArgumentException(string.Format("column does implement {0}. The actual type of column is {1}", typeof(IDocumentLeafNode), column.GetType()));

			return new NumericColumnProxyForStandaloneColumns(column); ;
		}

		/// <summary>
		/// Constructor by giving a numeric column.
		/// </summary>
		/// <param name="column">The numeric column to hold.</param>
		protected NumericColumnProxyForStandaloneColumns(INumericColumn column)
		{
			_column = column;
		}

		public INumericColumn Document
		{
			get { return _column; }
		}

		public bool IsEmpty
		{
			get { return null == _column; }
		}

		public object Clone()
		{
			return FromColumn(this._column);
		}

		public object DocumentObject
		{
			get { return _column; }
		}

		public DocumentPath DocumentPath
		{
			get { return new DocumentPath(); }
		}

		public bool ReplacePathParts(DocumentPath partToReplace, DocumentPath newPart)
		{
			return false;
		}
	}

	/// <summary>
	/// Holds a "weak" reference to a numeric column, altogether with a document path to that column.
	/// </summary>
	public class NumericColumnProxy : DocNodeProxy, INumericColumnProxy
	{
		#region Serialization

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(NumericColumnProxy), 0)]
		private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				info.AddBaseValueEmbedded(obj, typeof(DocNodeProxy)); // serialize the base class
			}

			public virtual object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				NumericColumnProxy s = o != null ? (NumericColumnProxy)o : new NumericColumnProxy();
				info.GetBaseValueEmbedded(s, typeof(DocNodeProxy), parent);         // deserialize the base class

				return s;
			}
		}

		#endregion Serialization

		public static NumericColumnProxy FromColumn(INumericColumn column)
		{
			if (null == column)
				throw new ArgumentNullException("column");
			var colAsDocumentNode = column as IDocumentLeafNode;
			if (null == colAsDocumentNode)
				throw new ArgumentException(string.Format("column does not implement {0}. The actual type of column is {1}", typeof(IDocumentLeafNode), column.GetType()));

			return new NumericColumnProxy(colAsDocumentNode);
		}

		/// <summary>
		/// Constructor by giving a numeric column.
		/// </summary>
		/// <param name="column">The numeric column to hold.</param>
		protected NumericColumnProxy(IDocumentLeafNode column)
			: base(column)
		{
		}

		/// <summary>
		/// For deserialization purposes only.
		/// </summary>
		protected NumericColumnProxy()
		{
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="from">Object to clone from.</param>
		public NumericColumnProxy(NumericColumnProxy from)
			: base(from)
		{
		}

		/// <summary>
		/// Tests whether or not the holded object is valid. Here the test returns true if the column
		/// is either of type <see cref="INumericColumn" /> or the holded object is <c>null</c>.
		/// </summary>
		/// <param name="obj">Object to test.</param>
		/// <returns>True if this is a valid document object.</returns>
		protected override bool IsValidDocument(object obj)
		{
			return ((obj is INumericColumn) && obj is IDocumentLeafNode) || obj == null;
		}

		/// <summary>
		/// Returns the holded object. Null can be returned if the object is no longer available (e.g. disposed).
		/// </summary>
		public INumericColumn Document
		{
			get
			{
				return (INumericColumn)base.DocumentObject;
			}
		}

		/// <summary>
		/// Clones this holder. For holded objects, which are part of the document hierarchy,
		/// the holded object is <b>not</b> cloned (only the reference is copied). For all other objects, the object
		/// is cloned, too, if the object supports the <see cref="ICloneable" /> interface.
		/// </summary>
		/// <returns>The cloned object holder.</returns>
		public override object Clone()
		{
			return new NumericColumnProxy(this);
		}
	}
}