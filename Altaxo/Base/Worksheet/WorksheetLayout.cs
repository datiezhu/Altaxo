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
using System.Collections.Generic;

namespace Altaxo.Worksheet
{
  using Altaxo.Data;
  using Altaxo.Main;

  /// <summary>
  /// Stores the layout of a table to be shown in a WorksheetView.
  /// </summary>
  public class WorksheetLayout
    :
    Main.SuspendableDocumentNodeWithSetOfEventArgs,
    Main.IEventIndicatedDisposable,
    Main.IProjectItemPresentationModel
  {
    #region Member variables

    /// <summary>
    /// The unique identifier of this object.
    /// </summary>
    protected System.Guid _guid;

    /// <summary>
    /// The data table this layout is for.
    /// </summary>
    protected Altaxo.Data.DataTable _dataTable;

    /*
    /// <summary>
    /// defaultColumnsStyles stores the default column Styles in a dictionary
    /// the key for the hash table is the Type of the ColumnStyle
    /// </summary>
    protected Dictionary<System.Type, ColumnStyle> _defaultDataColumnStyles;

    /// <summary>
    /// Stores the default property column Styles in a Hashtable
    /// the key for the hash table is the Type of the ColumnStyle
    /// </summary>
        protected Dictionary<System.Type, ColumnStyle> _defaultPropertyColumnStyles;
        */

    /// <summary>
    /// m_ColumnStyles stores the column styles for each data column individually,
    /// key is the data column itself.
    /// There is no need to store a column style here if the column is styled as default,
    /// instead the defaultColumnStyle is used in this case
    /// </summary>
    protected ColumnStyleDictionary _dataColumnStyles;

    protected ColumnStyleDictionary _propertyColumnStyles;

    /// <summary>
    /// The style of the row header. This is the leftmost column that shows usually the row number.
    /// </summary>
    private RowHeaderStyle _rowHeaderStyle; // holds the style of the row header (leftmost column of data grid)

    /// <summary>
    /// The style of the column header. This is the upmost row that shows the name of the columns.
    /// </summary>
    private ColumnHeaderStyle _columnHeaderStyle; // the style of the column header (uppermost row of datagrid)

    /// <summary>
    /// The style of the property column header. This is the leftmost column in the left of the property columns,
    /// that shows the names of the property columns.
    /// </summary>
    private ColumnHeaderStyle _propertyColumnHeaderStyle;

    /// <summary>
    /// The visibility of the property columns in the view. If true, the property columns are shown in the view.
    /// </summary>
    protected bool _doShowPropertyColumns;

    protected WeakActionHandler<object, object, TunnelingEventArgs> _weakEventHandlerForTable_TunneledEvent;

    #endregion Member variables

    #region Serialization

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(WorksheetLayout), 0)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      protected WorksheetLayout _worksheetLayout;
      protected System.Collections.Hashtable _colStyles;
      protected Main.AbsoluteDocumentPath _pathToTable;

      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (WorksheetLayout)obj;

        info.AddValue("Guid", System.Xml.XmlConvert.ToString(s._guid));
        info.AddValue("Table", Main.AbsoluteDocumentPath.GetAbsolutePath(s._dataTable));
        info.AddValue("RowHeaderStyle", s._rowHeaderStyle);
        info.AddValue("ColumnHeaderStyle", s._columnHeaderStyle);
        info.AddValue("PropertyColumnHeaderStyle", s._propertyColumnHeaderStyle);

        info.CreateArray("DefaultColumnStyles", s._dataColumnStyles.DefaultColumnStyles.Values.Count);
        foreach (object style in s._dataColumnStyles.DefaultColumnStyles.Values)
          info.AddValue("DefaultColumnStyle", style);
        info.CommitArray();

        info.CreateArray("ColumnStyles", s._dataColumnStyles.Count);
        foreach (KeyValuePair<DataColumn, ColumnStyle> dictentry in s._dataColumnStyles)
        {
          info.CreateElement("e");
          info.AddValue("Column", Main.AbsoluteDocumentPath.GetAbsolutePath(dictentry.Key));
          info.AddValue("Style", dictentry.Value);
          info.CommitElement(); // "e"
        }
        info.CommitArray();
      }

      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        WorksheetLayout s = null != o ? (WorksheetLayout)o : new WorksheetLayout();
        Deserialize(s, info, parent);
        return s;
      }

      protected virtual void Deserialize(WorksheetLayout s, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var surr = new XmlSerializationSurrogate0
        {
          _colStyles = new System.Collections.Hashtable(),
          _worksheetLayout = s
        };
        info.DeserializationFinished += new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(surr.EhDeserializationFinished);

        s._guid = System.Xml.XmlConvert.ToGuid(info.GetString("Guid"));
        surr._pathToTable = (Main.AbsoluteDocumentPath)info.GetValue("Table", s);
        s.RowHeaderStyle = (RowHeaderStyle)info.GetValue("RowHeaderStyle", s);
        s.ColumnHeaderStyle = (ColumnHeaderStyle)info.GetValue("ColumnHeaderStyle", s);
        s.PropertyColumnHeaderStyle = (ColumnHeaderStyle)info.GetValue("PropertyColumnHeaderStyle", s);

        int count;
        count = info.OpenArray(); // DefaultColumnStyles

        for (int i = 0; i < count; i++)
        {
          var defstyle = (ColumnStyle)info.GetValue("DefaultColumnStyle", s._dataColumnStyles);
          s._dataColumnStyles.SetDefaultColumnStyle(defstyle.GetType(), defstyle);
        }
        info.CloseArray(count);

        // deserialize the columnstyles
        // this must be deserialized in a new instance of this surrogate, since we can not resolve it immediately
        count = info.OpenArray();
        if (count > 0)
        {
          for (int i = 0; i < count; i++)
          {
            info.OpenElement(); // "e"
            var key = (Main.AbsoluteDocumentPath)info.GetValue("Column", s);
            object val = info.GetValue("Style", null);
            surr._colStyles[key] = val;
            info.CloseElement();
          }
        }
        info.CloseArray(count);
      }

      public void EhDeserializationFinished(Altaxo.Serialization.Xml.IXmlDeserializationInfo info, Main.IDocumentNode documentRoot, bool isFinallyCall)
      {
        if (_pathToTable != null && _worksheetLayout.DataTable == null)
        {
          object table = Main.AbsoluteDocumentPath.GetObject(_pathToTable, _worksheetLayout, documentRoot);
          if (table is Altaxo.Data.DataTable)
          {
            _worksheetLayout.DataTable = (Altaxo.Data.DataTable)table;
            _pathToTable = null;
          }
        }

        var resolvedStyles = new System.Collections.ArrayList();
        foreach (System.Collections.DictionaryEntry entry in _colStyles)
        {
          object resolvedobj = Main.AbsoluteDocumentPath.GetObject((Main.AbsoluteDocumentPath)entry.Key, _worksheetLayout, documentRoot);
          if (null != resolvedobj)
          {
            _worksheetLayout.DataColumnStyles.Add((DataColumn)resolvedobj, (ColumnStyle)entry.Value);
            resolvedStyles.Add(entry.Key);
          }
        }

        foreach (object resstyle in resolvedStyles)
          _colStyles.Remove(resstyle);

        // if all columns have resolved, we can close the event link
        if (_colStyles.Count == 0 && _pathToTable == null)
          info.DeserializationFinished -= new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(EhDeserializationFinished);
      }
    }

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(WorksheetLayout), 1)]
    private class XmlSerializationSurrogate1 : XmlSerializationSurrogate0
    {
      public override void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        base.Serialize(obj, info);

        var s = (WorksheetLayout)obj;
        info.CreateArray("DefaultPropertyColumnStyles", s._propertyColumnStyles.DefaultColumnStyles.Values.Count);
        foreach (object style in s._propertyColumnStyles.DefaultColumnStyles.Values)
          info.AddValue("DefaultPropertyColumnStyle", style);
        info.CommitArray();
      }

      protected override void Deserialize(WorksheetLayout s, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        base.Deserialize(s, info, parent);

        int count = info.OpenArray(); // DefaultPropertyColumnStyles
        for (int i = 0; i < count; i++)
        {
          var defstyle = (ColumnStyle)info.GetValue("DefaultPropertyColumnStyle", s._propertyColumnStyles);
          s._propertyColumnStyles.SetDefaultColumnStyle(defstyle.GetType(), defstyle);
        }
        info.CloseArray(count);
      }
    }

    // TODO (Wpf) Uncomment the next serialization if this is also implemented in Altaxo3
    /*

        // New in version 2 (2010-08): the key of the default styles is now the type of the column (before it was the type of default style)
        // both data columns and property columns have their own default styles
        // ColumnDictionary now has its own serialization code
        [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(WorksheetLayout), 2)]
        class XmlSerializationSurrogate2 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
        {
            protected WorksheetLayout _deserializedInstance;
            protected Main.DocumentPath _unresolvedPathToTable;

            public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
            {
                WorksheetLayout s = (WorksheetLayout)obj;

                info.AddValue("Guid", System.Xml.XmlConvert.ToString(s._guid));
                info.AddValue("Table", Main.DocumentPath.GetAbsolutePath(s._dataTable));
                info.AddValue("RowHeaderStyle", s._rowHeaderStyle);
                info.AddValue("ColumnHeaderStyle", s._columnHeaderStyle);
                info.AddValue("PropertyColumnHeaderStyle", s._propertyColumnHeaderStyle);

                info.AddValue("DataColumnStyles", s._dataColumnStyles);
                info.AddValue("PropertyColumnStyles", s._propertyColumnStyles);
            }

            public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
            {
                WorksheetLayout s = null != o ? (WorksheetLayout)o : new WorksheetLayout();
                Deserialize(s, info, parent);
                return s;
            }

            protected virtual void Deserialize(WorksheetLayout s, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
            {
                XmlSerializationSurrogate2 surr = new XmlSerializationSurrogate2();
                surr._deserializedInstance = s;
                info.DeserializationFinished += new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(surr.EhDeserializationFinished);

                s._guid = System.Xml.XmlConvert.ToGuid(info.GetString("Guid"));
                surr._unresolvedPathToTable = (Main.DocumentPath)info.GetValue("Table", s);
                s._rowHeaderStyle = (RowHeaderStyle)info.GetValue("RowHeaderStyle", s);
                s._columnHeaderStyle = (ColumnHeaderStyle)info.GetValue("ColumnHeaderStyle", s);
                s._propertyColumnHeaderStyle = (ColumnHeaderStyle)info.GetValue("PropertyColumnHeaderStyle", s);

                s._dataColumnStyles = (ColumnStyleDictionary)info.GetValue("DataColumnStypes", s);
                s._propertyColumnStyles = (ColumnStyleDictionary)info.GetValue("PropertyColumnStyles", s);
            }

            public void EhDeserializationFinished(Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object documentRoot)
            {
                if (this._unresolvedPathToTable != null)
                {
                    object table = Main.DocumentPath.GetObject(this._unresolvedPathToTable, this._deserializedInstance, documentRoot);
                    if (table is Altaxo.Data.DataTable)
                    {
                        this._deserializedInstance._dataTable = (Altaxo.Data.DataTable)table;
                        this._unresolvedPathToTable = null;
                    }
                }

                // if the table path has been resolved, we can finish deserialization
                if (this._unresolvedPathToTable == null)
                    info.DeserializationFinished -= new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(this.EhDeserializationFinished);
            }
        }
        */

    #endregion Serialization

    #region Constructors

    protected WorksheetLayout()
    {
      _guid = System.Guid.NewGuid();

      // m_ColumnStyles stores the column styles for each data column individually,
      _dataColumnStyles = new ColumnStyleDictionary() { ParentObject = this };

      _propertyColumnStyles = new ColumnStyleDictionary() { ParentObject = this };

      // The style of the row header. This is the leftmost column that shows usually the row number.
      _rowHeaderStyle = new RowHeaderStyle() { ParentObject = this }; // holds the style of the row header (leftmost column of data grid)

      // The style of the column header. This is the upmost row that shows the name of the columns.
      _columnHeaderStyle = new ColumnHeaderStyle() { ParentObject = this }; // the style of the column header (uppermost row of datagrid)

      // The style of the property column header. This is the leftmost column in the left of the property columns,
      _propertyColumnHeaderStyle = new ColumnHeaderStyle() { ParentObject = this };

      _doShowPropertyColumns = true;
    }

    public WorksheetLayout(Altaxo.Data.DataTable table)
      : this()
    {
      if (null != table)
        DataTable = table;
    }

    #endregion Constructors

    #region Properties

    public System.Guid Guid
    {
      get { return _guid; }
    }

    /// <summary>
    /// Don't use this! Only intended for use by WorksheetLayoutCollection.
    /// </summary>
    protected internal void NewGuid()
    {
      _guid = System.Guid.NewGuid();
    }

    public Altaxo.Data.DataTable DataTable
    {
      get { return _dataTable; }
      set
      {
        if (null == value)
          throw new ArgumentNullException("DataTable is null");

        if (object.ReferenceEquals(_dataTable, value))
          return;

        if (null != _dataTable)
          throw new InvalidOperationException("This instance is already bound to a data table. It is not allowed to assign another data table to it");

        _dataTable = value;

        {
          var table = _dataTable;

          // use LOCAL variables only to connect to weak event handlers!
          _weakEventHandlerForTable_TunneledEvent?.Remove();
          table.TunneledEvent += (_weakEventHandlerForTable_TunneledEvent = new WeakActionHandler<object, object, TunnelingEventArgs>(EhDataTableTunneledEvent, table, nameof(table.TunneledEvent)));
        }
      }
    }

    private void EhDataTableTunneledEvent(object sender, object originalSource, Main.TunnelingEventArgs e)
    {
      if (e is DisposeEventArgs && object.ReferenceEquals(originalSource, _dataTable))
      {
        if (!Current.Project?.TableLayouts?.Remove(this) ?? true)
          Dispose();
      }
    }

    public IDictionary<Data.DataColumn, ColumnStyle> DataColumnStyles
    {
      get { return _dataColumnStyles; }
    }

    public ColumnStyle GetDataColumnStyle(int i)
    {
      return _dataColumnStyles[_dataTable.DataColumns[i]];
    }

    public IDictionary<Data.DataColumn, ColumnStyle> PropertyColumnStyles
    {
      get { return _propertyColumnStyles; }
    }

    public ColumnStyle GetPropertyColumnStyle(int i)
    {
      return _propertyColumnStyles[_dataTable.PropertyColumns[i]];
    }

    public RowHeaderStyle RowHeaderStyle
    {
      get { return _rowHeaderStyle; }
      protected set
      {
        ChildSetMember(ref _rowHeaderStyle, value ?? throw new ArgumentNullException(nameof(value)));
      }
    }

    public ColumnHeaderStyle ColumnHeaderStyle
    {
      get { return _columnHeaderStyle; }
      protected set
      {
        ChildSetMember(ref _columnHeaderStyle, value ?? throw new ArgumentNullException(nameof(value)));
      }
    }

    public ColumnHeaderStyle PropertyColumnHeaderStyle
    {
      get { return _propertyColumnHeaderStyle; }
      protected set
      {
        ChildSetMember(ref _propertyColumnHeaderStyle, value ?? throw new ArgumentNullException(nameof(value)));
      }
    }

    public bool ShowPropertyColumns
    {
      get { return _doShowPropertyColumns; }
      set { _doShowPropertyColumns = value; }
    }

    #endregion Properties

    #region IDocumentNode Members

    public override string Name
    {
      get
      {
        return System.Xml.XmlConvert.ToString(_guid);
      }
      set
      {
        throw new InvalidOperationException("Name of this instance is based on a Guid and can therefore not be set.");
      }
    }

    IProjectItem IProjectItemPresentationModel.Document
    {
      get
      {
        return _dataTable;
      }
    }

    #endregion IDocumentNode Members

    #region Document node functions

    protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
    {
      if (null != _columnHeaderStyle)
        yield return new Main.DocumentNodeAndName(_columnHeaderStyle, () => _columnHeaderStyle = null, "ColumnHeaderStyle");

      if (null != _rowHeaderStyle)
        yield return new Main.DocumentNodeAndName(_rowHeaderStyle, () => _rowHeaderStyle = null, "RowHeaderStyle");

      if (null != _propertyColumnHeaderStyle)
        yield return new Main.DocumentNodeAndName(_propertyColumnHeaderStyle, () => _propertyColumnHeaderStyle = null, "PropertyColumnHeaderStyle");

      if (null != _dataColumnStyles)
        yield return new Main.DocumentNodeAndName(_dataColumnStyles, () => _dataColumnStyles = null, "DataColumnStyles");

      if (null != _propertyColumnStyles)
        yield return new Main.DocumentNodeAndName(_propertyColumnStyles, () => _propertyColumnStyles = null, "PropertyColumnStyles");
    }

    #endregion Document node functions

    protected override void Dispose(bool isDisposing)
    {
      if (null != _parent)
      {
        _weakEventHandlerForTable_TunneledEvent?.Remove();
        _weakEventHandlerForTable_TunneledEvent = null;

        base.Dispose(isDisposing);
        _dataTable = null;
      }
    }
  }
}
