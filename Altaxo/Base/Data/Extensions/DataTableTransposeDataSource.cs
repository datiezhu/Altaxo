﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2019 Dr. Dirk Lellinger
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

namespace Altaxo.Data
{
  public class DataTableTransposeDataSource : TableDataSourceBase, Altaxo.Data.IAltaxoTableDataSource
  {
    private DataTableTransposeOptions _processOptions;
    private DataTableProxy _processData;
    private IDataSourceImportOptions _importOptions;

    public Action<IAltaxoTableDataSource> _dataSourceChanged;

    #region Serialization

    #region Version 0

    /// <summary>
    /// 2015-08-26 initial version.
    /// </summary>
    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(DataTableTransposeDataSource), 0)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (DataTableTransposeDataSource)obj;

        info.AddValue("ProcessData", s._processData);
        info.AddValue("ProcessOptions", s._processOptions);
        info.AddValue("ImportOptions", s._importOptions);
      }

      protected virtual DataTableTransposeDataSource SDeserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var s = (o == null ? new DataTableTransposeDataSource() : (DataTableTransposeDataSource)o);

        s.ChildSetMember(ref s._processData, (DataTableProxy)info.GetValue("ProcessData", s));
        s._processOptions = (DataTableTransposeOptions)info.GetValue("ProcessOptions", s);
        s.ChildSetMember(ref s._importOptions, (IDataSourceImportOptions)info.GetValue("ImportOptions", s));
        return s;
      }

      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var s = SDeserialize(o, info, parent);
        return s;
      }
    }

    #endregion Version 0

    #endregion Serialization

    protected DataTableTransposeDataSource()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataTableTransposeDataSource"/> class.
    /// </summary>
    /// <param name="inputData">The input data designates the original source of data (used then for the processing).</param>
    /// <param name="dataSourceOptions">The Fourier transformation options.</param>
    /// <param name="importOptions">The data source import options.</param>
    /// <exception cref="System.ArgumentNullException">
    /// inputData
    /// or
    /// transformationOptions
    /// or
    /// importOptions
    /// </exception>
    public DataTableTransposeDataSource(DataTableProxy inputData, DataTableTransposeOptions dataSourceOptions, IDataSourceImportOptions importOptions)
    {
      if (null == inputData)
        throw new ArgumentNullException(nameof(inputData));
      if (null == dataSourceOptions)
        throw new ArgumentNullException(nameof(dataSourceOptions));
      if (null == importOptions)
        throw new ArgumentNullException(nameof(importOptions));

      using (var token = SuspendGetToken())
      {
        TransposeOptions = dataSourceOptions;
        ImportOptions = importOptions;
        InputData = inputData;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCyclingVariableColumnDataSource"/> class.
    /// </summary>
    /// <param name="from">Another instance to copy from.</param>
    public DataTableTransposeDataSource(DataTableTransposeDataSource from)
    {
      CopyFrom(from);
    }

    /// <summary>
    /// Copies from another instance.
    /// </summary>
    /// <param name="obj">The object to copy from.</param>
    /// <returns><c>True</c> if anything could be copied from the object, otherwise <c>false</c>.</returns>
    public bool CopyFrom(object obj)
    {
      if (object.ReferenceEquals(this, obj))
        return true;

      var from = obj as DataTableTransposeDataSource;
      if (null != from)
      {
        using (var token = SuspendGetToken())
        {
          DataTableTransposeOptions dataSourceOptions = null;
          DataTableProxy inputData = null;
          IDataSourceImportOptions importOptions = null;

          CopyHelper.Copy(ref importOptions, from._importOptions);
          CopyHelper.Copy(ref dataSourceOptions, from._processOptions);
          CopyHelper.Copy(ref inputData, from._processData);

          TransposeOptions = dataSourceOptions;
          ImportOptions = importOptions;
          InputData = inputData;

          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public object Clone()
    {
      return new DataTableTransposeDataSource(this);
    }

    #region IAltaxoTableDataSource

    /// <summary>
    /// Fills (or refills) the data table with the processed data. The data source is represented by this instance, the destination table is provided in the argument <paramref name="destinationTable" />.
    /// </summary>
    /// <param name="destinationTable">The destination table.</param>
    public void FillData(DataTable destinationTable)
    {
      try
      {
        DataTable srcTable = _processData.Document;
        if (srcTable == null)
          throw new InvalidOperationException(string.Format("Source table was not found: {0}", _processData));

        Transposing.Transpose(srcTable, _processOptions, destinationTable);
      }
      catch (Exception ex)
      {
        destinationTable.Notes.WriteLine("Error during execution of data source ({0}): {1}", GetType().Name, ex.Message);
      }
    }

    /// <summary>
    /// Occurs when the data source has changed and the import trigger source is DataSourceChanged. The argument is the sender of this event.
    /// </summary>
    public event Action<Data.IAltaxoTableDataSource> DataSourceChanged
    {
      add
      {
        bool isFirst = null == _dataSourceChanged;
        _dataSourceChanged += value;
        if (isFirst)
        {
          //EhInputDataChanged(this, EventArgs.Empty);
        }
      }
      remove
      {
        _dataSourceChanged -= value;
        bool isLast = null == _dataSourceChanged;
        if (isLast)
        {
        }
      }
    }

    /// <summary>
    /// Gets or sets the input data.
    /// </summary>
    /// <value>
    /// The input data. This data is the input for the 2D-Fourier transformation.
    /// </value>
    public DataTableProxy InputData
    {
      get
      {
        return _processData;
      }
      set
      {
        if (ChildSetMember(ref _processData, value ?? throw new ArgumentNullException(nameof(value))))
        {
          EhChildChanged(_processData, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets the data source import options.
    /// </summary>
    /// <value>
    /// The import options.
    /// </value>
    /// <exception cref="System.ArgumentNullException">ImportOptions</exception>
    public Data.IDataSourceImportOptions ImportOptions
    {
      get
      {
        return _importOptions;
      }
      set
      {
        if (ChildSetMember(ref _importOptions, value ?? throw new ArgumentNullException(nameof(value))))
        {
          EhChildChanged(_importOptions, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets the options for the transpose operation.
    /// </summary>
    /// <value>
    /// The transpose options.
    /// </value>
    /// <exception cref="System.ArgumentNullException">FourierTransformation2DOptions</exception>
    public DataTableTransposeOptions TransposeOptions
    {
      get
      {
        return _processOptions;
      }
      set
      {
        if (null == value)
          throw new ArgumentNullException(nameof(value));

        var oldValue = _processOptions;

        _processOptions = value;
      }
    }

    #region Change event handling

    protected override bool HandleHighPriorityChildChangeCases(object sender, ref EventArgs e)
    {
      if (object.ReferenceEquals(_processData, sender)) // incoming call from data proxy
      {
        if (_importOptions.ImportTriggerSource == ImportTriggerSource.DataSourceChanged)
        {
          e = TableDataSourceChangedEventArgs.Empty;
        }
        else
        {
          return true; // if option is not DataSourceChanged, absorb this event
        }
      }

      return base.HandleHighPriorityChildChangeCases(sender, ref e);
    }

    #endregion Change event handling

    #region Document Node functions

    protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
    {
      if (null != _processData)
        yield return new Main.DocumentNodeAndName(_processData, "ProcessData");
      // if (null != _processOptions)
      //   yield return new Main.DocumentNodeAndName(_processOptions, "ProcessOptions");
      if (null != _importOptions)
        yield return new Main.DocumentNodeAndName(_importOptions, "ImportOptions");
    }

    #endregion Document Node functions

    /// <summary>
    /// Called after deserization of a data source instance, when it is already associated with a data table.
    /// </summary>
    public void OnAfterDeserialization()
    {
    }

    /// <summary>
    /// Visits all document references.
    /// </summary>
    /// <param name="ReportProxies">The report proxies.</param>
    public void VisitDocumentReferences(Main.DocNodeProxyReporter ReportProxies)
    {
      if (_processData != null)
        ReportProxies(_processData, this, "ProcessData");
    }

    #endregion IAltaxoTableDataSource
  }
}
