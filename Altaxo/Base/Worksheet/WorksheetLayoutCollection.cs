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

using System;
using System.Collections.Generic;

namespace Altaxo.Worksheet
{
	/// <summary>
	/// Summary description for WorksheetLayoutCollection.
	/// </summary>
	public class WorksheetLayoutCollection
		:
		Main.SuspendableDocumentNodeWithSetOfEventArgs,
		Main.INamedObjectCollection,
		ICollection<WorksheetLayout>
	{
		protected Dictionary<string, WorksheetLayout> _items;

		#region Serialization

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(WorksheetLayoutCollection), 0)]
		private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				WorksheetLayoutCollection s = (WorksheetLayoutCollection)obj;

				info.CreateArray("TableLayoutArray", s._items.Count);
				foreach (object style in s._items.Values)
					info.AddValue("WorksheetLayout", style);
				info.CommitArray();
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				WorksheetLayoutCollection s = null != o ? (WorksheetLayoutCollection)o : new WorksheetLayoutCollection();

				int count;
				count = info.OpenArray(); // TableLayouts

				for (int i = 0; i < count; i++)
				{
					WorksheetLayout style = (WorksheetLayout)info.GetValue("WorksheetLayout", s);
					s._items.Add(style.Guid.ToString(), style);
				}
				info.CloseArray(count);

				return s;
			}
		}

		#endregion Serialization

		protected WorksheetLayoutCollection()
		{
			_items = new Dictionary<string, WorksheetLayout>();
		}

		public WorksheetLayoutCollection(Main.IDocumentNode documentParent)
		{
			_items = new Dictionary<string, WorksheetLayout>();
			_parent = documentParent;
		}

		public WorksheetLayout this[System.Guid guid]
		{
			get { return _items[guid.ToString()]; }
		}

		public WorksheetLayout this[string guidAsString]
		{
			get { return _items[guidAsString]; }
		}

		private void EhChildNodeTunneledEvent(object sender, object source, Main.TunnelingEventArgs e)
		{
			if (e is Main.DisposeEventArgs && source is WorksheetLayout)
			{
				var src = (WorksheetLayout)source;
				Remove(src);
			}
		}

		#region IDocumentNode Members

		public override Main.IDocumentNode ParentObject
		{
			get
			{
				return base.ParentObject;
			}
			set
			{
				throw new InvalidOperationException("ParentObject of this instance is fixed and cannot be set");
			}
		}

		public override string Name
		{
			get
			{
				return "TableLayouts";
			}
			set
			{
				throw new InvalidOperationException("Name cannot be set");
			}
		}

		#endregion IDocumentNode Members

		#region INamedObjectCollection Members

		public Main.IDocumentLeafNode GetChildObjectNamed(string name)
		{
			return this[name];
		}

		public string GetNameOfChildObject(Main.IDocumentLeafNode o)
		{
			WorksheetLayout layout = o as WorksheetLayout;
			if (layout == null)
				return null;
			if (null == this[layout.Guid])
				return null; // is not contained in this collection
			return layout.Guid.ToString();
		}

		protected override IEnumerable<Tuple<Main.IDocumentLeafNode, string>> GetDocumentNodeChildrenWithName()
		{
			foreach (var entry in _items)
				yield return new Tuple<Main.IDocumentLeafNode, string>(entry.Value, entry.Key);
		}

		#endregion INamedObjectCollection Members

		#region ICollection<WorksheetLayout> Members

		#region Collection changing methods

		public void Add(WorksheetLayout layout)
		{
			if (null == layout)
				throw new ArgumentNullException("layout");

			// Test if this Guid is already present
			WorksheetLayout o = null;
			_items.TryGetValue(layout.Guid.ToString(), out o);
			if (o != null)
			{
				if (object.ReferenceEquals(o, layout))
					return;
				else
					layout.NewGuid();
			}

			layout.ParentObject = this;
			layout.TunneledEvent += EhChildNodeTunneledEvent;
			_items[layout.Guid.ToString()] = layout;
			EhSelfChanged(Main.NamedObjectCollectionChangedEventArgs.FromItemAdded(layout));
		}

		public bool Remove(WorksheetLayout item)
		{
			bool wasRemoved = _items.Remove(item.Guid.ToString());

			if (wasRemoved)
			{
				var eventArgs = Main.NamedObjectCollectionChangedEventArgs.FromItemRemoved(item);
				item.ParentObject = null;
				item.TunneledEvent -= EhChildNodeTunneledEvent;
				EhSelfChanged(eventArgs);
			}

			return wasRemoved;
		}

		public void Clear()
		{
			using (var suspendToken = this.SuspendGetToken())
			{
				foreach (var item in _items.Values)
				{
					EhSelfChanged(Main.NamedObjectCollectionChangedEventArgs.FromItemRemoved(item));
					item.ParentObject = null;
					item.TunneledEvent -= EhChildNodeTunneledEvent;
				}
				_items.Clear();

				suspendToken.Resume();
			}
		}

		#endregion Collection changing methods

		public bool Contains(WorksheetLayout item)
		{
			return _items.ContainsKey(item.Guid.ToString());
		}

		public void CopyTo(WorksheetLayout[] array, int arrayIndex)
		{
			_items.Values.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _items.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion ICollection<WorksheetLayout> Members

		#region IEnumerable<WorksheetLayout> Members

		public IEnumerator<WorksheetLayout> GetEnumerator()
		{
			return _items.Values.GetEnumerator();
		}

		#endregion IEnumerable<WorksheetLayout> Members

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _items.Values.GetEnumerator();
		}

		#endregion IEnumerable Members
	}
}