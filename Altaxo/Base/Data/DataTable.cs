/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002 Dr. Dirk Lellinger
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

using System;
using Altaxo.Serialization;


namespace Altaxo.Data
{

	public class AltaxoUniqueNameException : System.ApplicationException
	{
	}

	/// <summary>DataTable is the central class of Altaxo, which holds the data organized in columns.</summary>
	/// <remarks>In contrast to common database
	/// programs, the data are not organized in rows, but in (relatively independent) columns. As in database programs,
	/// each column has a certain type, as <see cref="TextColumn"/> for holding strings, 
	/// <see cref="DoubleColumn"/> for storing numeric values, and <see cref="DateTimeColumn"/> for holding DateTimes.
	/// All these column types are derived from the base class <see cref="DataColumn"/>.<para/>
	/// There is also a similar concept like metadata in database programs: Each column can have some property values associated with. The property values
	/// are organized in property columns and can be retrieved by the <see cref="DataTable.PropCols"/> property of the table.</remarks>
	[SerializationSurrogate(0,typeof(Altaxo.Data.DataTable.SerializationSurrogate0))]
	[SerializationSurrogate(1,typeof(Altaxo.Data.DataTable.SerializationSurrogate1))]
	[SerializationVersion(1)]
	public class DataTable 
		:		
		System.Runtime.Serialization.IDeserializationCallback, 
		ICloneable,
		Altaxo.Main.IDocumentNode,
		IDisposable,
		Main.INamedObjectCollection,
		Main.INameOwner,
		Main.IChildChangedEventSink,
		Main.ISuspendable
		{
		// Types
		
		// Data
		/// <summary>
		/// The parent data set this table is belonging to.
		/// </summary>
		protected object m_Parent=null; // the dataSet that this table is belonging to
		/// <summary>
		/// The name of this table, has to be unique if there is a parent data set, since the tables in the parent data set
		/// can only be accessed by name.
		/// </summary>
		protected string m_TableName=null; // the name of the table


		/// <summary>
		/// Collection of property columns, i.e. "horizontal" columns.
		/// </summary>
		/// <remarks>Property columns can be used to give columns a certain property. This can be for instance the unit of the column or a
		/// descriptive name (the property column is then of type TextColumn).
		/// This can also be another parameter which corresponds with that column, i.e. frequency. In this case the property column would be of
		/// type DoubleColumn.</remarks>
		protected DataColumnCollection m_PropertyColumns;
		/// <summary>
		/// Collection of data columns, i.e. the normal, "vertical" columns.
		/// </summary>
		protected DataColumnCollection m_DataColumns;


		/// <summary>
		/// The table script that belongs to this table.
		/// </summary>
		protected TableScript m_TableScript;

		// Helper Data

		/// <summary>
		/// Used to indicate that the Deserialization process has finished.
		/// </summary>
		private bool  m_Table_DeserializationFinished=false;

		/// <summary>
		/// The number of suspends.
		/// </summary>
		[NonSerialized()]
		protected int  m_SuspendCount=0;

		/// <summary>
		/// Flag to signal that resume is currently in progress.
		/// </summary>
		[NonSerialized()]
		private   bool m_ResumeInProgress=false;

		/// <summary>
		/// Collection of child objects that were suspended by this object.
		/// </summary>
		[NonSerialized()]
		private System.Collections.ArrayList m_SuspendedChildCollection = new System.Collections.ArrayList();
		
		/// <summary>
		/// If not null, the table was changed and the table has not notified the parent and the listeners about that.
		/// </summary>
		protected System.EventArgs m_ChangeData = null;


		/// <summary>
		/// Event to signal changes in the data.
		/// </summary>
		public event System.EventHandler Changed;

		/// <summary>
		/// Event to signal that the parent of this object has changed.
		/// </summary>
		public event Main.ParentChangedEventHandler ParentChanged;

		/// <summary>
		/// Event to signal that the name of this object has changed.
		/// </summary>
		public event Main.NameChangedEventHandler NameChanged;


		#region "Serialization"
		public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				Altaxo.Data.DataTable s = (Altaxo.Data.DataTable)obj;
				System.Runtime.Serialization.ISurrogateSelector ss = AltaxoStreamingContext.GetSurrogateSelector(context);
				if(null!=ss)
				{
					System.Runtime.Serialization.ISerializationSurrogate surr =
						ss.GetSurrogate(typeof(Altaxo.Data.DataColumnCollection),context, out ss);
					surr.GetObjectData(obj,info,context); // stream the data of the base object
				}
				else 
				{
					throw new NotImplementedException(string.Format("Serializing a {0} without surrogate not implemented yet!",obj.GetType()));
				}

				info.AddValue("Name",s.m_TableName); // name of the Table
				info.AddValue("PropCols", s.m_PropertyColumns); // the property columns of that table

			}
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				Altaxo.Data.DataTable s = (Altaxo.Data.DataTable)obj;
				System.Runtime.Serialization.ISurrogateSelector ss = AltaxoStreamingContext.GetSurrogateSelector(context);
				if(null!=ss)
				{
				System.Runtime.Serialization.ISerializationSurrogate surr =
					ss.GetSurrogate(typeof(Altaxo.Data.DataColumnCollection),context, out ss);
				surr.SetObjectData(obj,info,context,selector);
				}
				else 
				{
					throw new NotImplementedException(string.Format("Serializing a {0} without surrogate not implemented yet!",obj.GetType()));
				}

				s.m_TableName = info.GetString("Name");
				s.m_PropertyColumns = (DataColumnCollection)info.GetValue("PropCols",typeof(DataColumnCollection));

				return s;
			}
		}

		public class SerializationSurrogate1 : System.Runtime.Serialization.ISerializationSurrogate
		{
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				Altaxo.Data.DataTable s = (Altaxo.Data.DataTable)obj;
	
				info.AddValue("Name",s.m_TableName); // name of the Table
				info.AddValue("DataCols", s.DataColumns); // the data columns of that table
				info.AddValue("PropCols", s.m_PropertyColumns); // the property columns of that table

			}
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				Altaxo.Data.DataTable s = (Altaxo.Data.DataTable)obj;

				s.m_TableName = info.GetString("Name");
				s.m_DataColumns = (DataColumnCollection)info.GetValue("DataCols",typeof(DataColumnCollection));
				s.m_PropertyColumns = (DataColumnCollection)info.GetValue("PropCols",typeof(DataColumnCollection));

				return s;
			}
		}

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(Altaxo.Data.DataTable),0)]
		public class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo	info)
			{
				Altaxo.Data.DataTable s = (Altaxo.Data.DataTable)obj;
				info.AddValue("Name",s.m_TableName); // name of the Table
				info.AddValue("DataCols",s.m_DataColumns);
				info.AddValue("PropCols", s.m_PropertyColumns); // the property columns of that table

			}
			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo	info, object parent)
			{
				Altaxo.Data.DataTable s = null!=o ? (Altaxo.Data.DataTable)o : new Altaxo.Data.DataTable();

				s.m_TableName = info.GetString("Name");
				s.m_DataColumns = (DataColumnCollection)info.GetValue("DataCols",s);
				s.m_DataColumns.ParentObject = s;
				s.m_PropertyColumns = (DataColumnCollection)info.GetValue("PropCols",s);
				s.m_PropertyColumns.ParentObject = s;

				return s;
			}
		}

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(Altaxo.Data.DataTable),1)]
			public class XmlSerializationSurrogate1 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo	info)
			{
				Altaxo.Data.DataTable s = (Altaxo.Data.DataTable)obj;
				info.AddValue("Name",s.m_TableName); // name of the Table
				info.AddValue("DataCols",s.m_DataColumns);
				info.AddValue("PropCols", s.m_PropertyColumns); // the property columns of that table

				// new in version 1
				info.AddValue("TableScript",s.m_TableScript);
			}
			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo	info, object parent)
			{
				Altaxo.Data.DataTable s = null!=o ? (Altaxo.Data.DataTable)o : new Altaxo.Data.DataTable();

				s.m_TableName = info.GetString("Name");
				s.m_DataColumns = (DataColumnCollection)info.GetValue("DataCols",s);
				s.m_DataColumns.ParentObject = s;
				s.m_PropertyColumns = (DataColumnCollection)info.GetValue("PropCols",s);
				s.m_PropertyColumns.ParentObject = s;

				// new in version 1
				s.m_TableScript = (TableScript)info.GetValue("TableScript",s);
				return s;
			}
		}


		public virtual void OnDeserialization(object obj)
		{
			//base.Parent = this;
			//base.OnDeserialization(obj);

			if(!m_Table_DeserializationFinished && obj is DeserializationFinisher)
			{
				m_Table_DeserializationFinished = true;
				// set the parent data table of the data column collection

				// now inform the dependent objects
				DeserializationFinisher finisher = new DeserializationFinisher(this);
				this.m_DataColumns.ParentObject = this;
				this.m_DataColumns.OnDeserialization(finisher);
				this.m_PropertyColumns.ParentObject = this;
				this.m_PropertyColumns.OnDeserialization(finisher);
			}
		}


		/// <summary>
		/// This class is responsible for the special purpose to serialize a data table for clipboard. Do not use
		/// it for permanent serialization purposes, since it does not contain version handling.
		/// </summary>
		[Serializable]
		public class ClipboardMemento : System.Runtime.Serialization.ISerializable
		{
			DataTable _table;
			Altaxo.Worksheet.IndexSelection _selectedDataColumns;
			Altaxo.Worksheet.IndexSelection _selectedDataRows;
			Altaxo.Worksheet.IndexSelection _selectedPropertyColumns;

			/// <summary>
			/// Constructor. Besides the table, the current selections must be provided. Only the areas that corresponds to the selections are
			/// serialized. The serialization process has to occur immediately after this constructor, because only a reference
			/// to the table is hold by this object.
			/// </summary>
			/// <param name="table">The table to serialize.</param>
			/// <param name="selectedDataColumns">The selected data columns.</param>
			/// <param name="selectedDataRows">The selected data rows.</param>
			/// <param name="selectedPropertyColumns">The selected property columns.</param>
			public ClipboardMemento(DataTable table, Altaxo.Worksheet.IndexSelection selectedDataColumns, 
				Altaxo.Worksheet.IndexSelection selectedDataRows,
				Altaxo.Worksheet.IndexSelection selectedPropertyColumns)
			{
				this._table										= table;
				this._selectedDataColumns			= selectedDataColumns;
				this._selectedDataRows				= selectedDataRows;
				this._selectedPropertyColumns = selectedPropertyColumns;
			}
			
			/// <summary>
			/// Returns the (deserialized) table.
			/// </summary>
			public DataTable DataTable
			{
				get { return _table; }
			}

			#region ISerializable Members

			public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			{

				info.AddValue("Name",_table.Name);
				info.AddValue("DataColumns",new DataColumnCollection.ClipboardMemento(_table.DataColumns,_selectedDataColumns,_selectedDataRows));
				info.AddValue("PropertyColumns",new DataColumnCollection.ClipboardMemento(_table.PropCols,_selectedPropertyColumns,_selectedDataColumns));
			}

			public ClipboardMemento(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			{
				_table = new DataTable();
				_table.Name = info.GetString("Name");
				DataColumnCollection.ClipboardMemento datacolMemento = (DataColumnCollection.ClipboardMemento)info.GetValue("DataColumns",typeof(DataColumnCollection.ClipboardMemento));
				_table.m_DataColumns = datacolMemento.Collection;
				DataColumnCollection.ClipboardMemento propcolMemento = (DataColumnCollection.ClipboardMemento)info.GetValue("PropertyColumns",typeof(DataColumnCollection.ClipboardMemento));
				_table.m_PropertyColumns = propcolMemento.Collection;
			}


			#endregion

		}


		#endregion

		/// <summary>
		/// Constructs an empty data table.
		/// </summary>
		public DataTable()
			: this(new DataColumnCollection(),new DataColumnCollection())
		{
		}

		/// <summary>
		/// Constructs an empty data table with the name provided by the argiment.
		/// </summary>
		/// <param name="name">The initial name of the table.</param>
		public DataTable(string name)
			: this()
		{
			this.m_TableName = name;
		}

		/// <summary>
		/// Constructs an empty table with the parent provided by the argument.
		/// </summary>
		/// <param name="parent">The initial parent of the table.</param>
		public DataTable(Altaxo.Data.DataTableCollection parent)
			: this()
		{
			this.m_Parent = parent;
		}

		/// <summary>
		/// Constructs an empty table with the parent and the name provided by the argument.
		/// </summary>
		/// <param name="parent">The initial parent of the table.</param>
		/// <param name="name">The initial name of the table.</param>
		public DataTable(Altaxo.Data.DataTableCollection parent, string name) 
			: this(name)
		{
			this.m_Parent = parent;
		}
  
		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="from">The data table to copy the structure from.</param>
		public DataTable(DataTable from)
			: this((DataColumnCollection)from.m_DataColumns.Clone(),(DataColumnCollection)from.m_PropertyColumns.Clone())
		{
			
			this.m_Parent = null; // do not clone the parent
			this.m_TableName = from.m_TableName;
			this.m_TableScript = null==from.m_TableScript ? null : (TableScript)from.m_TableScript.Clone();
		}

		/// <summary>
		/// Constructor for internal use only. Takes the two DataColumnCollections as Data and Properties. These collections are used directly (not by cloning them).
		/// </summary>
		/// <param name="datacoll">The data columns.</param>
		/// <param name="propcoll">The property columns.</param>
		protected DataTable(DataColumnCollection datacoll, DataColumnCollection propcoll)
		{
			this.m_DataColumns = datacoll;
			m_DataColumns.ParentObject = this;
			m_DataColumns.ParentChanged += new Main.ParentChangedEventHandler(this.EhChildParentChanged);

			this.m_PropertyColumns = propcoll;
			this.m_PropertyColumns.ParentObject = this; // set the parent of the cloned PropertyColumns
			m_PropertyColumns.ParentChanged += new Main.ParentChangedEventHandler(this.EhChildParentChanged);

		}

		/// <summary>
		/// Clones the table.
		/// </summary>
		/// <returns>A cloned version of this table. All data inside the table are cloned too (deep copy).</returns>
		public virtual object Clone()
		{
			return new DataTable(this);
		}

		#region Suspend and resume

		/// <summary>
		/// Returns true if the table is currently suspended.
		/// </summary>
		public bool IsSuspended
		{
			get { return m_SuspendCount>0; }
		}

		/// <summary>
		/// Suspends the change notifications of the table.
		/// </summary>
		public void Suspend()
		{
			System.Diagnostics.Debug.Assert(m_SuspendCount>=0,"SuspendCount must always be greater or equal to zero");		
			++m_SuspendCount; // suspend one step higher
		}

		/// <summary>
		/// Resumes the change notifications of this table.
		/// </summary>
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
						OnDataChanged(); // Fire the changed event
					}		
				}
			}
		}


		/// <summary>
		/// Accumulates the change data of the child. Currently only a flag is set to signal that the table has changed.
		/// </summary>
		/// <param name="sender">The sender of the change notification (currently unused).</param>
		/// <param name="e">The change event args can provide details of the change (currently unused).</param>
		void AccumulateChildChangeData(object sender, EventArgs e)
		{
			if(sender!=null && m_ChangeData==null)
				this.m_ChangeData = new EventArgs();
		}
	
		/// <summary>
		/// Used by childrens of the table to inform the table of a change in their data.
		/// </summary>
		/// <param name="sender">The sender of the change notification.</param>
		/// <param name="e">The change details.</param>
		public void OnChildChanged(object sender, System.EventArgs e)
		{
			if(this.IsSuspended &&  sender is Main.ISuspendable)
			{
				m_SuspendedChildCollection.Add(sender); // add sender to suspended child
				((Main.ISuspendable)sender).Suspend();
				return;
			}

			AccumulateChildChangeData(sender,e);	// AccumulateNotificationData
			
			if(m_ResumeInProgress || IsSuspended)
				return;

			if(m_Parent is Main.IChildChangedEventSink )
			{
				((Main.IChildChangedEventSink)m_Parent).OnChildChanged(this, m_ChangeData);
				if(IsSuspended) // maybe parent has suspended us now
				{
					this.OnChildChanged(sender, e); // we call the function recursively, but now we are suspended
					return;
				}
			}
			
			OnDataChanged(); // Fire the changed event
		}

		
		/// <summary>
		/// Fires the change event with the EventArgs provided in the argument.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnChanged(EventArgs e)
		{
			if(null!=Changed)
				Changed(this,e);
		}

		/// <summary>
		/// Fires the data change event and removes the accumulated change data.
		/// </summary>
		protected virtual void OnDataChanged()
		{
			if(null!=Changed)
				Changed(this,m_ChangeData);

			m_ChangeData=null;
		}

		#endregion

		/// <summary>
		/// Get / sets the parent object of this table.
		/// </summary>
		public virtual object ParentObject
		{
			get
			{ 
				return m_Parent;
			}
			set
			{
				object oldParent = m_Parent;
				m_Parent = value;
				if(!object.ReferenceEquals(oldParent,m_Parent))
				{
					OnParentChanged(oldParent,m_Parent);
				}
			}
		}

		/// <summary>
		/// Fires the parent change event.
		/// </summary>
		/// <param name="oldParent">The parent before the change.</param>
		/// <param name="newParent">The parent after the change.</param>
		protected virtual void OnParentChanged(object oldParent,object newParent)
		{
			if(ParentChanged!=null)
				ParentChanged(this,new Main.ParentChangedEventArgs(oldParent,newParent));
		}


		

		/// <summary>
		/// Get or sets the name of the Table.
		/// </summary>
		public string Name
		{
			get
			{
				return m_TableName;
			}
			set
			{
				string oldName = m_TableName;
				m_TableName = value;

				if(oldName!=m_TableName)
				{
					try
					{
						OnNameChanged(oldName,m_TableName);
					}
					catch(Exception ex)
					{
						m_TableName = oldName;
						throw ex;
					}
				}
			}
		}

		/// <summary>
		/// Fires the name change event.
		/// </summary>
		/// <param name="oldName">The name of the table before the change.</param>
		/// <param name="newName">The name of the table after the change.</param>
		protected virtual void OnNameChanged(string oldName, string newName)
		{
			if(NameChanged != null)
				NameChanged(this, new Main.NameChangedEventArgs(oldName,newName));
		}


		/// <summary>
		/// EventHandler used to catch unauthorized parent changes in child objects.
		/// </summary>
		/// <param name="sender">The child object which signal a parent change.</param>
		/// <param name="e">The parent change details.</param>
		protected void EhChildParentChanged(object sender, Main.ParentChangedEventArgs e)
		{
			// this event should not happen, or someone try to 
			// change the parents of my collection
			throw new ApplicationException("Unexpected change of DataColumnsCollection's parent belonging to table " + this.Name);
		}

		/// <summary>
		/// Returns the property collection of the table.
		/// </summary>
		/// <remarks>To get a certain property value for a certain data column of the table,
		/// use PropCols["propertyname", datacolumnnumber], where propertyname is the name of the property to retrieve and
		/// columnnumber is the number of the data column for which the property should be retrieved. Unfortunately you can not reference
		/// the data column here by name :-(, you have to know the number. Alternatively, you can reference the property (!) not by name, but
		/// by number by using PropCols[propertycolumnnumber, datacolumnnumber]. If you only have
		/// the data columns name, use PropCols("propertyname",this["datacolumsname"].Number] instead.
		/// </remarks>
		public DataColumnCollection PropCols
		{
			get { return m_PropertyColumns; }
		}
		

		public TableScript TableScript
		{
			get { return m_TableScript; }
			set
			{
				m_TableScript = value; 
				
			}
		}
		
		/// <summary>
		/// Copies data to the data column with the provided index if both columns are of the same type. If they are not of the same type, the column is replaced by the provided column. If the index is beyoind the limit, the provided column is added.
		/// </summary>
		/// <param name="idx">The index of the column where to copy to, or replace.</param>
		/// <param name="datac">The column to copy.</param>
		public virtual void CopyOrReplaceOrAdd(int idx, Altaxo.Data.DataColumn datac, string name)
		{
			Suspend();
			m_DataColumns.CopyOrReplaceOrAdd(idx,datac, name); // add the column to the collection
			// no need to insert a property row here (only when inserting)
			Resume();
		}

		/// <summary>
		/// Returns the collection of data columns. Used as simplification in scripts to provide access in the form table["A"].Col[2].
		/// </summary>
		public DataColumnCollection Col
		{
			get { return m_DataColumns; }
		}

		/// <summary>
		/// Returns the collection of data columns.
		/// </summary>
		public DataColumnCollection DataColumns
		{
			get { return m_DataColumns; }
		}

		/// <summary>
		/// Get/sets the data column at index i. Setting is done by copying data, if the two columns has the same type. If the two columns are not of
		/// the same type, an exception is thrown.
		/// </summary>
		public DataColumn this[int i]
		{
			get { return m_DataColumns[i]; }
			set { m_DataColumns[i]=value; }
		}

		/// <summary>
		/// Get/sets the data column with the given name. Setting is done by copying data, if the two columns has the same type. If the two columns are not of
		/// the same type, an exception is thrown.
		/// </summary>
		public DataColumn this[string name]
		{
			get { return m_DataColumns[name]; }
			set { m_DataColumns[name]=value; }
		}


		


	


		/// <summary>
		/// Transpose transpose the table, i.e. exchange columns and rows
		/// this can only work if all columns in the table are of the same type
		/// </summary>
		/// <returns>null if succeeded, error string otherwise</returns>
		public virtual string Transpose()
		{
			// TODO: do also look at the property columns for transposing
			m_DataColumns.Transpose();

			return null; // no error message
		}


		/// <summary>
		/// Signals that the table has changed, but has not currently signaled that change to it's parent.
		/// </summary>
			public virtual bool IsDirty
			{
				get
				{
					return m_ChangeData!=null;
				}
			}

		/// <summary>
		/// Remove the columns beginning at index nFirstColumn.
		/// </summary>
		/// <param name="nFirstColumn">The index of the first column to remove.</param>
		/// <param name="nDelCount">The number of columns to remove.</param>
		public virtual void RemoveColumns(int nFirstColumn, int nDelCount)
		{
	
			Suspend();
			
			m_DataColumns.RemoveColumns(nFirstColumn, nDelCount); // remove the columns from the collection
			m_PropertyColumns.RemoveRows(nFirstColumn, nDelCount); // remove also the corresponding rows from the Properties

			Resume();
		}

		#region IDisposable Members

		/// <summary>
		/// Disposes the table and all child objects.
		/// </summary>
		public void Dispose()
		{
			m_DataColumns.Dispose();
			m_PropertyColumns.Dispose();
		}

		#endregion

		/// <summary>
		/// retrieves the object with the name <code>name</code>.
		/// </summary>
		/// <param name="name">The objects name.</param>
		/// <returns>The object with the specified name.</returns>
		public object GetChildObjectNamed(string name)
		{
			switch(name)
			{
				case "DataCols":
					return m_DataColumns;
				case "PropCols":
					return this.m_PropertyColumns;
			}
		return null;
		}

		/// <summary>
		/// Retrieves the name of the provided object.
		/// </summary>
		/// <param name="o">The object for which the name should be found.</param>
		/// <returns>The name of the object. Null if the object is not found. String.Empty if the object is found but has no name.</returns>
		public string GetNameOfChildObject(object o)
		{
			if(o==null)
				return null;
			else if(o.Equals(m_DataColumns))
				return "DataCols";
			else if(o.Equals(m_PropertyColumns))
				return "PropCols";
			else
				return null;
		}


		/// <summary>
		/// Get the parent data table of a DataColumnCollection.
		/// </summary>
		/// <param name="colcol">The DataColumnCollection for which the parent table has to be found.</param>
		/// <returns>The parent data table of the DataColumnCollection, or null if it was not found.</returns>
		public static Altaxo.Data.DataTable GetParentDataTableOf(DataColumnCollection colcol)
		{
			if(colcol.ParentObject is DataTable)
				return (DataTable)colcol.ParentObject;
			else
				return (DataTable)Main.DocumentPath.GetRootNodeImplementing(colcol,typeof(DataTable));
		}

		/// <summary>
		/// Gets the parent data table of the DataColumn column.
		/// </summary>
		/// <param name="column">The data column for wich the parent data table should be found.</param>
		/// <returns>The parent data table of this column, or null if it can not be found.</returns>
		public static Altaxo.Data.DataTable GetParentDataTableOf(DataColumn column)
		{
			if(column.ParentObject is DataColumnCollection)
				return GetParentDataTableOf((DataColumnCollection)column.ParentObject);
			else
				return (DataTable)Main.DocumentPath.GetRootNodeImplementing(column,typeof(DataTable));
		}


		/// <summary>
		/// Gets the parent data table of a child object.
		/// </summary>
		/// <param name="child">The child object for which the parent table should be found.</param>
		public static Altaxo.Data.DataTable GetParentDataTableOf(Main.IDocumentNode child)
		{
				return (DataTable)Main.DocumentPath.GetRootNodeImplementing(child,typeof(DataTable));
		}
		
	} // end class Altaxo.Data.DataTable
	
}
