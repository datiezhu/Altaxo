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
using System.Collections.Generic;
using Altaxo.Serialization;


namespace Altaxo.Data
{
  /// <summary>
  /// Summary description for Altaxo.Data.DataTableCollection.
  /// </summary>
  [SerializationSurrogate(0,typeof(Altaxo.Data.DataTableCollection.SerializationSurrogate0))]
  [SerializationVersion(0)]
  public class DataTableCollection 
    :
    System.Runtime.Serialization.IDeserializationCallback, 
    ICollection<DataTable>, 
    Altaxo.Main.IDocumentNode,
    Main.INamedObjectCollection,
    Altaxo.Main.IChangedEventSource,
    Altaxo.Main.IChildChangedEventSink,
    Main.ISuspendable
  {
    // Data
    protected SortedDictionary<string,DataTable> _tablesByName = new SortedDictionary<string,DataTable>();
    protected object _parent=null;

    // helper data
    /// <summary>
    /// Fired when table(s) are added, removed or renamed, and when the content of one table has changed.
    /// </summary>
    public event System.EventHandler Changed;

    /// <summary>
    /// Fired when one or more tables are added, deleted or renamed. Not fired when content in the table has changed.
    /// </summary>
    public event System.EventHandler CollectionChanged;

    [NonSerialized()]
    protected List<Main.ISuspendable> _suspendedChilds = new List<Main.ISuspendable>();
    [NonSerialized()]
    protected int  _suspendCount=0;
    [NonSerialized()]
    private   bool _isResumeInProgress=false;
    [NonSerialized()]
    protected ChangedEventArgs _changeData=null;
    [NonSerialized()]
    private bool _isDeserializationFinished=false;

    #region ChangedEventArgs

    /// <summary>
    /// Holds information about what has changed in the table.
    /// </summary>
    protected class ChangedEventArgs : System.EventArgs
    {
      /// <summary>
      /// If true, one or more tables where added.
      /// </summary>
      public bool TableAdded;
      /// <summary>
      /// If true, one or more table where removed.
      /// </summary>
      public bool TableRemoved;
      /// <summary>
      /// If true, one or more tables where renamed.
      /// </summary>
      public bool TableRenamed;

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
      public static ChangedEventArgs IfTableAdded
      {
        get
        { 
          ChangedEventArgs e =  new ChangedEventArgs();
          e.TableAdded=true;
          return e;
        }
      }
      /// <summary>
      /// Returns an instance with TableRemoved set to true.
      /// </summary>
      public static ChangedEventArgs IfTableRemoved
      {
        get
        { 
          ChangedEventArgs e =  new ChangedEventArgs();
          e.TableRemoved=true;
          return e;
        }
      }
      /// <summary>
      /// Returns an  instance with TableRenamed set to true.
      /// </summary>
      public static ChangedEventArgs IfTableRenamed
      {
        get
        { 
          ChangedEventArgs e =  new ChangedEventArgs();
          e.TableRenamed=true;
          return e;
        }
      }

      
      /// <summary>
      /// Merges information from another instance in this ChangedEventArg.
      /// </summary>
      /// <param name="from"></param>
      public void Merge(ChangedEventArgs from)
      {
        this.TableAdded |= from.TableAdded;
        this.TableRemoved |= from.TableRemoved;
        this.TableRenamed |= from.TableRenamed;
      }

      /// <summary>
      /// Returns true when the collection has changed (addition, removal or renaming of tables).
      /// </summary>
      public bool CollectionChanged
      {
        get { return TableAdded | TableRemoved | TableRenamed; }
      }
    }

    #endregion

    public DataTableCollection(AltaxoDocument parent)
    {
      this._parent = parent;
    }
  

    public object ParentObject
    {
      get { return this._parent; }
      set { this._parent = value; }
    }

    public string Name
    {
      get { return "Tables"; }
    }

    #region Serialization
    public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
    {
      public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context  )
      {
        Altaxo.Data.DataTableCollection s = (Altaxo.Data.DataTableCollection)obj;
        // info.AddValue("Parent",s._parent);
        info.AddValue("Tables",s._tablesByName);
      }
      public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
      {
        Altaxo.Data.DataTableCollection s = (Altaxo.Data.DataTableCollection)obj;
        // s._parent = (AltaxoDocument)(info.GetValue("Parent",typeof(AltaxoDocument)));
        s._tablesByName = (SortedDictionary<string,DataTable>)(info.GetValue("Tables",typeof(SortedDictionary<string,DataTable>)));

        // setup helper objects
        s._suspendedChilds = new List<Main.ISuspendable>();
        return s;
      }
    }

    public void OnDeserialization(object obj)
    {
      if(!_isDeserializationFinished && obj is DeserializationFinisher) // if deserialization has completely finished now
      {
        _isDeserializationFinished = true;
        DeserializationFinisher finisher = new DeserializationFinisher(this);
        // set the _parent object for the data tables
        foreach(DataTable dt in _tablesByName.Values)
        {
          dt.ParentObject = this;
          dt.OnDeserialization(finisher);
        }
      }
    }

    #endregion

    #region ICollection<DataTable> Members


    public void Clear()
    {
      foreach (DataTable table in _tablesByName.Values)
        Detach(table);
      _tablesByName.Clear();
      this.SelfChanged(ChangedEventArgs.IfTableRemoved);
    }

    public bool Contains(DataTable item)
    {
      if (null == item)
        throw new ArgumentNullException("item");

      DataTable r;
      if (_tablesByName.TryGetValue(item.Name, out r))
        return object.ReferenceEquals(r, item);
      else
        return false;
    }

    public void CopyTo(DataTable[] array, int arrayIndex)
    {
      _tablesByName.Values.CopyTo(array, arrayIndex);
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    bool ICollection<DataTable>.Remove(DataTable item)
    {
      return Remove(item);
    }

     public int Count
    {
      get { return _tablesByName.Count; }
    }

    #endregion

     #region IEnumerable<DataTable> Members

     IEnumerator<DataTable> IEnumerable<DataTable>.GetEnumerator()
     {
       return _tablesByName.Values.GetEnumerator();
     }

     #endregion
     
    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return _tablesByName.Values.GetEnumerator();
    }

    #endregion
   
 
    #region Suspend and resume

    public bool IsSuspended
    {
      get { return _suspendCount>0; }
    }

    public void Suspend()
    {
      System.Diagnostics.Debug.Assert(_suspendCount>=0,"SuspendCount must always be greater or equal to zero");    

      ++_suspendCount; // suspend one step higher
    }

    public void Resume()
    {
      System.Diagnostics.Debug.Assert(_suspendCount>=0,"SuspendCount must always be greater or equal to zero");    
      if(_suspendCount>0 && (--_suspendCount)==0)
      {
        this._isResumeInProgress = true;
        foreach(Main.ISuspendable obj in _suspendedChilds)
          obj.Resume();
        _suspendedChilds.Clear();
        this._isResumeInProgress = false;

        // send accumulated data if available and release it thereafter
        if(null!=_changeData)
        {
          if(_parent is Main.IChildChangedEventSink)
          {
            ((Main.IChildChangedEventSink)_parent).EhChildChanged(this, _changeData);
          }
          if(!IsSuspended)
          {
            if(_changeData.CollectionChanged)
              OnCollectionChanged();
            
            OnDataChanged(); // Fire the changed event
          }   
        }
      }
    }


    void AccumulateChildChangeData(object sender, System.EventArgs e)
    {

      if(_changeData==null)
        _changeData = ChangedEventArgs.Empty;

      if(e is ChangedEventArgs)
        _changeData.Merge((ChangedEventArgs)e);
    }
  
    public void EhChildChanged(object sender, System.EventArgs e)
    {
      if(this.IsSuspended &&  sender is Main.ISuspendable)
      {
        _suspendedChilds.Add((Main.ISuspendable)sender); // add sender to suspended child
        ((Main.ISuspendable)sender).Suspend();
        return;
      }

      AccumulateChildChangeData(sender,e);  // AccumulateNotificationData
      
      if(_isResumeInProgress || IsSuspended)
        return;

      if(_parent is Main.IChildChangedEventSink )
      {
        ((Main.IChildChangedEventSink)_parent).EhChildChanged(this, _changeData);
        if(IsSuspended) // maybe parent has suspended us now
        {
          this.EhChildChanged(sender, e); // we call the function recursively, but now we are suspended
          return;
        }
      }
      
      if(_changeData.CollectionChanged)
        OnCollectionChanged();

      OnDataChanged(); // Fire the changed event
    }

    private void SelfChanged(ChangedEventArgs e)
    {
      EhChildChanged(null,e);
    }

    protected virtual void OnCollectionChanged()
    {
      if(this.CollectionChanged!=null)
        CollectionChanged(this,_changeData);
    }


    protected virtual void OnChanged(EventArgs e)
    {
      if(null!=Changed)
        Changed(this,e);
    }

    protected virtual void OnDataChanged()
    {
      if(null!=Changed)
        Changed(this,_changeData);
    
      _changeData=null;
    }


    #endregion


    public bool IsDirty
    {
      get
      {
        return _changeData!=null;
      }
    }

    public string[] GetSortedTableNames()
    {
      string[] arr = new string[_tablesByName.Count];
      this._tablesByName.Keys.CopyTo(arr,0);
      // System.Array.Sort(arr);
      return arr;
    }

    public Altaxo.Data.DataTable this[string name]
    {
      get
      {
        DataTable result;
        if (_tablesByName.TryGetValue(name, out result))
          return result;
        else
          throw new ArgumentOutOfRangeException(string.Format("The table \"{0}\" don't exist!", name));
      }
    }

  
    public bool Contains(string tablename)
    {
      return _tablesByName.ContainsKey(tablename);
    }

   

    public void Add(Altaxo.Data.DataTable theTable)
    {
      if(null==theTable.Name || 0==theTable.Name.Length) // if no table name provided
        theTable.Name = FindNewTableName();                 // find a new one
      else if(_tablesByName.ContainsKey(theTable.Name)) // else if this table name is already in use
        theTable.Name = FindNewTableName(theTable.Name); // find a new table name based on the original name

      // now the table has a unique name in any case
      _tablesByName.Add(theTable.Name,theTable);
      Attach(theTable);

      // raise data event to all listeners
      this.SelfChanged(ChangedEventArgs.IfTableAdded);

    }

    /// <summary>
    /// Attaches the table to this object but not adds it to the collection. You should do this as soon as possible.
    /// </summary>
    /// <param name="theTable"></param>
    private void Attach(DataTable theTable)
    {
      theTable.ParentObject = this;
      theTable.NameChanged += this.EhTableNameChanged;
      theTable.ParentChanged += this.EhTableParentChanged;
    }

    /// <summary>
    /// Detaches the table but not removes it from the collection. You should remove the table as soon as possible.
    /// </summary>
    /// <param name="theTable"></param>
    private void Detach(DataTable theTable)
    {
      theTable.ParentChanged -= this.EhTableParentChanged;
      theTable.NameChanged -= this.EhTableNameChanged;
      theTable.ParentObject = null;
    }

    public bool Remove(DataTable theTable)
    {
      if (!_tablesByName.ContainsValue(theTable))
        return false;

      _tablesByName.Remove(theTable.Name);
      Detach(theTable);
      this.SelfChanged(ChangedEventArgs.IfTableRemoved);

      return true;
    }

    protected void EhTableParentChanged(object sender, Main.ParentChangedEventArgs pce)
    {
      if(object.ReferenceEquals(this,pce.OldParent) && this.Contains((DataTable)sender))
        this.Remove((DataTable)sender);
      else
        if(!this.Contains((DataTable)sender))
        throw new ApplicationException("Not allowed to set child's parent to this collection before adding it to the collection");
    }

    protected void EhTableNameChanged(object sender, Main.NameChangedEventArgs nce)
    {
      if(object.ReferenceEquals(this[nce.NewName],sender))
        return; // Table alredy renamed

      if(this.Contains(nce.NewName))
        throw new ApplicationException("Table with name " + nce.NewName + " already exists!");

      if(!this.Contains(nce.OldName))
        throw new ApplicationException("Error renaming table " + nce.OldName + " : this table name was not found in the collection!" );
        
      if(!object.ReferenceEquals(this[nce.OldName],sender))
        throw new ApplicationException("Names between DataTableCollection and Tables not in sync");

      _tablesByName.Remove(nce.OldName);
      _tablesByName.Add(nce.NewName,(DataTable)sender);

      SelfChanged(ChangedEventArgs.IfTableRenamed);
    }

    /// <summary>
    /// Looks for the next free standard table name.
    /// </summary>
    /// <returns>A new table name unique for this data set.</returns>
    public string FindNewTableName()
    {
      return FindNewTableName("WKS");
    } 

    /// <summary>
    /// Looks for the next unique table name base on a basic name.
    /// </summary>
    /// <returns>A new table name unique for this data set.</returns>
    public string FindNewTableName(string basicname)
    {
      for(int i=0;;i++)
      {
        if(!_tablesByName.ContainsKey(basicname+i))
          return basicname+i; 
      }
    } 

    public object GetChildObjectNamed(string name)
    {
      DataTable result;
      if (_tablesByName.TryGetValue(name, out result))
        return result;

      return null;
    }

    public string GetNameOfChildObject(object o)
    {
      if(o is DataTable)
      {
        DataTable gr = (DataTable)o;
        if(_tablesByName.ContainsKey(gr.Name))
          return gr.Name;
      }
      return null;
    }


    /// <summary>
    /// Gets the parent DataTableCollection of a child table, a child ColumnCollection, or a child column.
    /// </summary>
    /// <param name="child">Can be a DataTable, a DataColumnCollection, or a DataColumn for which the parent table collection is searched.</param>
    /// <returns>The parent DataTableCollection, if it exists, or null otherwise.</returns>
    public static Altaxo.Data.DataTableCollection GetParentDataTableCollectionOf(Main.IDocumentNode child)
    {
      return (DataTableCollection)Main.DocumentPath.GetRootNodeImplementing(child,typeof(DataTableCollection));
    }



   
  }
}
