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

using Altaxo;
using Altaxo.Main;
using System;
using System.Collections.Generic;

namespace Altaxo.Graph.Gdi
{
	public class GraphDocumentCollection :
		Main.SuspendableDocumentNode,
		System.Runtime.Serialization.IDeserializationCallback,
		IEnumerable<GraphDocument>,
		Altaxo.Main.IDocumentNode,
		Altaxo.Main.IChangedEventSource,
		Altaxo.Main.IChildChangedEventSink,
		Altaxo.Main.INamedObjectCollection
	{
		#region ChangedEventArgs

		/// <summary>
		/// Holds information about what has changed in the table.
		/// </summary>
		protected class ChangedEventArgs : System.EventArgs
		{
			/// <summary>
			/// If true, one or more tables where added.
			/// </summary>
			public bool ItemAdded;

			/// <summary>
			/// If true, one or more table where removed.
			/// </summary>
			public bool ItemRemoved;

			/// <summary>
			/// If true, one or more tables where renamed.
			/// </summary>
			public bool ItemRenamed;

			/// <summary>
			/// Empty constructor.
			/// </summary>
			public ChangedEventArgs()
			{
			}

			/// <summary>
			/// Returns an empty instance.
			/// </summary>
			public static new ChangedEventArgs Empty
			{
				get { return new ChangedEventArgs(); }
			}

			/// <summary>
			/// Returns an instance with TableAdded set to true;.
			/// </summary>
			public static ChangedEventArgs IfItemAdded
			{
				get
				{
					ChangedEventArgs e = new ChangedEventArgs();
					e.ItemAdded = true;
					return e;
				}
			}

			/// <summary>
			/// Returns an instance with TableRemoved set to true.
			/// </summary>
			public static ChangedEventArgs IfItemRemoved
			{
				get
				{
					ChangedEventArgs e = new ChangedEventArgs();
					e.ItemRemoved = true;
					return e;
				}
			}

			/// <summary>
			/// Returns an  instance with TableRenamed set to true.
			/// </summary>
			public static ChangedEventArgs IfItemRenamed
			{
				get
				{
					ChangedEventArgs e = new ChangedEventArgs();
					e.ItemRenamed = true;
					return e;
				}
			}

			/// <summary>
			/// Merges information from another instance in this ChangedEventArg.
			/// </summary>
			/// <param name="from"></param>
			public void Merge(ChangedEventArgs from)
			{
				this.ItemAdded |= from.ItemAdded;
				this.ItemRemoved |= from.ItemRemoved;
				this.ItemRenamed |= from.ItemRenamed;
			}

			/// <summary>
			/// Returns true when the collection has changed (addition, removal or renaming of tables).
			/// </summary>
			public bool CollectionChanged
			{
				get { return ItemAdded | ItemRemoved | ItemRenamed; }
			}
		}

		#endregion ChangedEventArgs

		// Data
		protected SortedDictionary<string, GraphDocument> m_GraphsByName = new SortedDictionary<string, GraphDocument>();

		protected bool bIsDirty = false;

		[NonSerialized]
		protected ChangedEventArgs _accumulatedEventData = null;

		// Events

		/// <summary>
		/// Fired when one or more graphs are added, deleted or renamed. Not fired when content in the graph has changed.
		/// </summary>
		public event Action<Main.NamedObjectCollectionChangeType, object, string, string> CollectionChanged;

		#region IChangedEventSource Members

		public event System.EventHandler Changed;

		#endregion IChangedEventSource Members

		public GraphDocumentCollection(AltaxoDocument parent)
		{
			this._parent = parent;
		}

		public override object ParentObject
		{
			get
			{
				return _parent;
			}
			set
			{
				throw new InvalidOperationException("ParentObject of GraphDocumentCollection is fixed and cannot be set");
			}
		}

		public override string Name
		{
			get { return "Graphs"; }
			set
			{
				throw new InvalidOperationException("Name of GraphDocumentCollection is fixed and cannot be set");
			}
		}

		#region Serialization

		public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			public void GetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			{
				GraphDocumentCollection s = (GraphDocumentCollection)obj;
				// info.AddValue("Parent",s.parent);
				info.AddValue("Graphs", s.m_GraphsByName);
			}

			public object SetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
			{
				GraphDocumentCollection s = (GraphDocumentCollection)obj;
				// s.parent = (AltaxoDocument)(info.GetValue("Parent",typeof(AltaxoDocument)));
				s.m_GraphsByName = (SortedDictionary<string, GraphDocument>)(info.GetValue("Graphs", typeof(SortedDictionary<string, GraphDocument>)));

				return s;
			}
		}

		public void OnDeserialization(object obj)
		{
		}

		#endregion Serialization

		public bool IsDirty
		{
			get
			{
				return bIsDirty;
			}
		}

		public string[] GetSortedGraphNames()
		{
			string[] arr = new string[m_GraphsByName.Count];
			this.m_GraphsByName.Keys.CopyTo(arr, 0);
			System.Array.Sort(arr);
			return arr;
		}

		public GraphDocument this[string name]
		{
			get
			{
				return (GraphDocument)m_GraphsByName[name];
			}
		}

		public bool Contains(string graphname)
		{
			return m_GraphsByName.ContainsKey(graphname);
		}

		public bool TryGetValue(string graphName, out GraphDocument doc)
		{
			return m_GraphsByName.TryGetValue(graphName, out doc);
		}

		public void Add(GraphDocument theGraph)
		{
			if (!string.IsNullOrEmpty(theGraph.Name) && m_GraphsByName.ContainsKey(theGraph.Name) && theGraph.Equals(m_GraphsByName[theGraph.Name]))
				return; // do silently nothing if the graph (the same!) is already registered
			if (string.IsNullOrEmpty(theGraph.Name)) // if no table name provided
				theGraph.Name = FindNewName();                  // find a new one
			else if (m_GraphsByName.ContainsKey(theGraph.Name)) // else if this table name is already in use
				theGraph.Name = FindNewName(theGraph.Name); // find a new table name based on the original name

			// now the table has a unique name in any case
			m_GraphsByName.Add(theGraph.Name, theGraph);
			theGraph.ParentObject = this;
			theGraph.NameChanged += EhChild_NameChanged;
			this.EhSelfChanged(ChangedEventArgs.IfItemAdded);
			OnCollectionChanged(Main.NamedObjectCollectionChangeType.ItemAdded, theGraph, theGraph.Name);
		}

		public void Remove(GraphDocument theGraph)
		{
			if (theGraph != null && theGraph.Name != null)
			{
				GraphDocument gr = (GraphDocument)m_GraphsByName[theGraph.Name];

				if (null != Current.ComManager && object.ReferenceEquals(gr, Current.ComManager.EmbeddedObject)) // test if the graph is currently the embedded Com object
					return; // it is not allowed to remove the current embedded graph object.

				if (object.ReferenceEquals(gr, theGraph))
				{
					m_GraphsByName.Remove(theGraph.Name);
					theGraph.ParentObject = null;
					theGraph.NameChanged -= EhChild_NameChanged;
					this.EhSelfChanged(ChangedEventArgs.IfItemRemoved);
					OnCollectionChanged(Main.NamedObjectCollectionChangeType.ItemRemoved, theGraph, theGraph.Name);
				}
			}
		}

		protected void EhChild_NameChanged(Main.INameOwner item, string oldName)
		{
			if (m_GraphsByName.ContainsKey(item.Name))
			{
				if (object.ReferenceEquals(m_GraphsByName[item.Name], item))
					return;
				else
					throw new ApplicationException(string.Format("The GraphDocumentCollection contains already a Graph named {0}, renaming the old graph {1} fails.", item.Name, oldName));
			}
			m_GraphsByName.Remove(oldName);
			m_GraphsByName[item.Name] = (GraphDocument)item;
			this.EhSelfChanged(ChangedEventArgs.IfItemRenamed);
			OnCollectionChanged(Main.NamedObjectCollectionChangeType.ItemRenamed, item, oldName);
		}

		/// <summary>
		/// Looks for the next free standard  name.
		/// </summary>
		/// <returns>A new table name unique for this set.</returns>
		public string FindNewName()
		{
			return FindNewName("GRAPH");
		}

		/// <summary>
		/// Looks for the next unique name base on a basic name.
		/// </summary>
		/// <returns>A new  name unique for this  set.</returns>
		public string FindNewName(string basicname)
		{
			for (int i = 0; ; i++)
			{
				if (!m_GraphsByName.ContainsKey(basicname + i.ToString()))
					return basicname + i;
			}
		}

		public object GetChildObjectNamed(string name)
		{
			GraphDocument result = null;
			if (m_GraphsByName.TryGetValue(name, out result))
				return result;
			else return null;
		}

		public string GetNameOfChildObject(object o)
		{
			if (o is GraphDocument)
			{
				GraphDocument gr = (GraphDocument)o;
				if (m_GraphsByName.ContainsKey(gr.Name))
					return gr.Name;
			}
			return null;
		}

		#region Change event handling

		[NonSerialized()]
		protected bool m_ResumeInProgress = false;

		[NonSerialized()]
		protected System.Collections.ArrayList m_SuspendedChildCollection = new System.Collections.ArrayList();

		public bool IsSuspended
		{
			get
			{
				return false; // m_SuspendCount>0;
			}
		}

#if false
    public void Suspend()
    {
      System.Diagnostics.Debug.Assert(m_SuspendCount>=0,"SuspendCount must always be greater or equal to zero");

      ++m_SuspendCount; // suspend one step higher
    }

    public void Resume()
    {
      System.Diagnostics.Debug.Assert(m_SuspendCount>=0,"SuspendCount must always be greater or equal to zero");
      if(m_SuspendCount>0 && (--m_SuspendCount)==0)
      {
        this.m_ResumeInProgress = true;
        foreach(Main.ISuspendable obj in m_SuspendedChildCollection)
          obj.Resume();
        m_SuspendedChildCollection.Clear();
        this.m_ResumeInProgress = false;

        // send accumulated data if available and release it thereafter
        if(null!=m_ChangeData)
        {
          if(m_Parent is Main.IChildChangedEventSink)
          {
            ((Main.IChildChangedEventSink)m_Parent).OnChildChanged(this, m_ChangeData);
          }
          if(!IsSuspended)
          {
            OnChanged(); // Fire the changed event
          }
        }
      }
    }

#endif

		protected override IEnumerable<EventArgs> AccumulatedEventData
		{
			get
			{
				if (null != _accumulatedEventData)
					yield return _accumulatedEventData;
			}
		}

		protected override void AccumulatedEventData_Clear()
		{
			_accumulatedEventData = null;
		}

		protected override void AccumulateChangeData(object sender, EventArgs e)
		{
			if (_accumulatedEventData == null)
				this._accumulatedEventData = ChangedEventArgs.Empty;

			if (e is ChangedEventArgs)
				_accumulatedEventData.Merge((ChangedEventArgs)e);
		}

		protected virtual void OnCollectionChanged(Main.NamedObjectCollectionChangeType changeType, Main.INameOwner item, string oldName)
		{
			if (this.CollectionChanged != null)
				CollectionChanged(changeType, item, oldName, item.Name);
		}

		#endregion Change event handling

		/// <summary>
		/// Gets the parent GraphDocumentCollection of a child graph.
		/// </summary>
		/// <param name="child">A graph for which the parent collection is searched.</param>
		/// <returns>The parent GraphDocumentCollection, if it exists, or null otherwise.</returns>
		public static GraphDocumentCollection GetParentGraphDocumentCollectionOf(Main.IDocumentNode child)
		{
			return (GraphDocumentCollection)Main.DocumentPath.GetRootNodeImplementing(child, typeof(GraphDocumentCollection));
		}

		#region IEnumerable<GraphDocument> Members

		IEnumerator<GraphDocument> IEnumerable<GraphDocument>.GetEnumerator()
		{
			return m_GraphsByName.Values.GetEnumerator();
		}

		#endregion IEnumerable<GraphDocument> Members

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			return m_GraphsByName.Values.GetEnumerator();
		}

		#endregion IEnumerable Members
	}
}