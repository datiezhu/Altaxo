﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2019 Dr. Dirk Lellinger
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
using System.Linq;
using System.Text;
using Altaxo.Data;
using Altaxo.Scripting;
using Altaxo.Serialization;

namespace Altaxo.Data
{
  public class FileImportScriptDataSource : TableDataSourceBase, Altaxo.Data.IAltaxoTableDataSource
  {
    private IDataSourceImportOptions _importOptions;

    private FileImportScript _importScript;

    private List<AbsoluteAndRelativeFileName> _files = new List<AbsoluteAndRelativeFileName>();

    private HashSet<string> _resolvedFileNames = new HashSet<string>();

    protected bool _isDirty = false;

    protected System.IO.FileSystemWatcher[] _fileSystemWatchers = new System.IO.FileSystemWatcher[0];

    protected Altaxo.Main.TriggerBasedUpdate _triggerBasedUpdate;

    /// <summary>Indicates that serialization of the whole AltaxoDocument (!) is still in progress. Data sources should not be updated during serialization.</summary>
    [NonSerialized]
    protected bool _isDeserializationInProgress;

    #region Serialization

    #region Version 0

    /// <summary>
    /// 2014-07-28 initial version.
    /// </summary>
    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(FileImportScriptDataSource), 0)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (FileImportScriptDataSource)obj;

        info.AddValue("ImportOptions", s._importOptions);
        info.AddValue("ImportScript", s._importScript);
        info.AddArray("Files", s._files.ToArray(), s._files.Count);
      }

      protected virtual FileImportScriptDataSource SDeserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var s = (o == null ? new FileImportScriptDataSource() : (FileImportScriptDataSource)o);
        s._isDeserializationInProgress = true;
        s.ChildSetMember(ref s._importOptions, (IDataSourceImportOptions)info.GetValue("ImportOptions", s));
        s.ChildSetMember(ref s._importScript, (FileImportScript)info.GetValue("ImportScript", s));
        var count = info.OpenArray("Files");
        for (int i = 0; i < count; ++i)
          s._files.Add((AbsoluteAndRelativeFileName)info.GetValue("e", s));
        info.CloseArray(count);

        info.AfterDeserializationHasCompletelyFinished += s.EhAfterDeserializationHasCompletelyFinished;

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

    #region Construction

    public bool CopyFrom(object obj)
    {
      if (object.ReferenceEquals(this, obj))
        return true;

      var from = obj as FileImportScriptDataSource;
      if (null != from)
      {
        using (var token = SuspendGetToken())
        {
          _files = new List<AbsoluteAndRelativeFileName>(CopyHelper.GetEnumerationMembersCloned(from._files));
          ChildSetMember(ref _importOptions, from._importOptions);
          ChildCopyToMember(ref _importScript, from._importScript);
          _files = new List<AbsoluteAndRelativeFileName>(CopyHelper.GetEnumerationMembersCloned(from._files));

          EhSelfChanged(EventArgs.Empty);
          token.Resume();
        }
        return true;
      }
      return false;
    }

    /// <summary>
    /// Deserialization constructor
    /// </summary>
    protected FileImportScriptDataSource()
    {
      _files = new List<AbsoluteAndRelativeFileName>();
    }

    public FileImportScriptDataSource(string fileName, FileImportScript script)
      : this(new string[] { fileName }, script)
    {
    }

    public FileImportScriptDataSource(IEnumerable<string> fileNames, FileImportScript script)
    {
      _files = new List<AbsoluteAndRelativeFileName>();
      foreach (var fileName in fileNames)
      {
        _files.Add(new AbsoluteAndRelativeFileName(fileName));
      }
      ChildCopyToMember(ref _importScript, script);
      _importOptions = new DataSourceImportOptions() { ParentObject = this };
    }

    public FileImportScriptDataSource(FileImportScriptDataSource from)
    {
      CopyFrom(from);
    }

    public object Clone()
    {
      return new FileImportScriptDataSource(this);
    }

    protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
    {
      if (null != _importScript)
        yield return new Main.DocumentNodeAndName(_importScript, () => _importScript = null, "ImportScript");

      if (null != _importOptions)
        yield return new Main.DocumentNodeAndName(_importOptions, () => _importOptions = null, "ImportOptions");
    }

    #endregion Construction

    protected override void OnResume(int eventCount)
    {
      base.OnResume(eventCount);

      // UpdateWatching should only be called if something concerning the watch (Times etc.) has changed during the suspend phase
      // Otherwise it will cause endless loops because UpdateWatching triggers immediatly an EhUpdateByTimerQueue event, which triggers an UpdateDataSource event, which leads to another Suspend and then Resume, which calls OnResume(). So the loop is closed.
      if (null == _triggerBasedUpdate)
        UpdateWatching(); // Compromise - we update only if the watch is off
    }

    public void FillData(DataTable destinationTable)
    {
      var validFileNames = _files.Select(x => x.GetResolvedFileNameOrNull()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

      if (validFileNames.Length == 0)
        return;

      _importScript.ExecuteWithoutExceptionCatching(destinationTable, validFileNames, null);
    }

    #region Properties

    public string SourceFileName
    {
      get
      {
        if (_files.Count != 1)
          throw new InvalidOperationException("In order to get the source file name, the number of files has to be one");

        return _files[0].GetResolvedFileNameOrNull() ?? _files[0].AbsoluteFileName;
      }
      set
      {
        string oldName = null;
        if (_files.Count == 1)
          oldName = SourceFileName;

        _files.Clear();
        _files.Add(new AbsoluteAndRelativeFileName(value));

        if (oldName != SourceFileName)
        {
          UpdateWatching();
        }
      }
    }

    public IEnumerable<string> SourceFileNames
    {
      get
      {
        return _files.Select(x => x.GetResolvedFileNameOrNull() ?? x.AbsoluteFileName);
      }
      set
      {
        _files.Clear();
        foreach (var name in value)
          _files.Add(new AbsoluteAndRelativeFileName(name));

        UpdateWatching();
      }
    }

    public int SourceFileNameCount
    {
      get
      {
        return _files.Count;
      }
    }

    public IDataSourceImportOptions ImportOptions
    {
      get
      {
        return _importOptions;
      }
      set
      {
        var oldValue = _importOptions;

        _importOptions = value ?? throw new ArgumentNullException(nameof(ImportOptions));

        if (!object.ReferenceEquals(oldValue, value))
        {
          UpdateWatching();
        }
      }
    }

    public FileImportScript ImportScript
    {
      get
      {
        return (FileImportScript)_importScript.Clone();
      }
      set
      {
        ChildCloneToMember(ref _importScript, value);
      }
    }

    #endregion Properties

    private void SetAbsoluteRelativeFilePath(AbsoluteAndRelativeFileName value)
    {
      if (null == value)
        throw new ArgumentNullException("value");

      var oldValue = _files.Count == 1 ? _files[0] : null;
      _files.Clear();
      _files.Add(value);

      if (!value.Equals(oldValue))
      {
        UpdateWatching();
      }
    }

    public void OnAfterDeserialization()
    {
      // Note: it is not neccessary to call UpdateWatching here; UpdateWatching is called when the table connects to this data source via subscription to the DataSourceChanged event
    }

    public void VisitDocumentReferences(Main.DocNodeProxyReporter ReportProxies)
    {
    }

    public void UpdateWatching()
    {
      SwitchOffWatching();

      if (_isDeserializationInProgress)
        return; // in serialization process - wait until serialization has finished

      if (IsSuspended)
        return; // in update operation - wait until finished

      if (null == _parent)
        return; // No listener - no need to watch

      if (_importOptions.ImportTriggerSource != ImportTriggerSource.DataSourceChanged)
        return; // DataSource is updated manually

      var validFileNames = _files.Select(x => x.GetResolvedFileNameOrNull()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
      if (0 == validFileNames.Length)
        return;  // No file name set

      _resolvedFileNames = new HashSet<string>(validFileNames);

      SwitchOnWatching(validFileNames);
    }

    private void SwitchOnWatching(string[] validFileNames)
    {
      _triggerBasedUpdate = new Main.TriggerBasedUpdate(Current.TimerQueue)
      {
        MinimumWaitingTimeAfterUpdate = TimeSpanExtensions.FromSecondsAccurate(_importOptions.MinimumWaitingTimeAfterUpdateInSeconds),
        MaximumWaitingTimeAfterUpdate = TimeSpanExtensions.FromSecondsAccurate(Math.Max(_importOptions.MinimumWaitingTimeAfterUpdateInSeconds, _importOptions.MaximumWaitingTimeAfterUpdateInSeconds)),
        MinimumWaitingTimeAfterFirstTrigger = TimeSpanExtensions.FromSecondsAccurate(_importOptions.MinimumWaitingTimeAfterFirstTriggerInSeconds),
        MinimumWaitingTimeAfterLastTrigger = TimeSpanExtensions.FromSecondsAccurate(_importOptions.MinimumWaitingTimeAfterLastTriggerInSeconds),
        MaximumWaitingTimeAfterFirstTrigger = TimeSpanExtensions.FromSecondsAccurate(Math.Max(_importOptions.MaximumWaitingTimeAfterFirstTriggerInSeconds, _importOptions.MinimumWaitingTimeAfterFirstTriggerInSeconds))
      };

      _triggerBasedUpdate.UpdateAction += EhUpdateByTimerQueue;

      var directories = new HashSet<string>(validFileNames.Select(x => System.IO.Path.GetDirectoryName(x)));
      var watchers = new List<System.IO.FileSystemWatcher>();
      foreach (var directory in directories)
      {
        try
        {
          var watcher = new System.IO.FileSystemWatcher(directory)
          {
            NotifyFilter = System.IO.NotifyFilters.LastWrite | System.IO.NotifyFilters.Size
          };
          watcher.Changed += EhTriggerByFileSystemWatcher;
          watcher.IncludeSubdirectories = false;
          watcher.EnableRaisingEvents = true;
          watchers.Add(watcher);
        }
        catch (Exception)
        {
        }
      }
      _fileSystemWatchers = watchers.ToArray();
    }

    private void SwitchOffWatching()
    {
      IDisposable disp = null;

      var watchers = _fileSystemWatchers;
      _fileSystemWatchers = new System.IO.FileSystemWatcher[0];

      for (int i = 0; i < watchers.Length; ++i)
      {
        disp = watchers[i];
        if (null != disp)
          disp.Dispose();
      }

      disp = _triggerBasedUpdate;
      _triggerBasedUpdate = null;
      if (null != disp)
        disp.Dispose();
    }

    public void EhUpdateByTimerQueue()
    {
      if (null != _parent)
      {
        if (!IsSuspended) // no events during the suspend phase
        {
          EhChildChanged(this, TableDataSourceChangedEventArgs.Empty);
        }
      }
      else
        SwitchOffWatching();
    }

    private void EhTriggerByFileSystemWatcher(object sender, System.IO.FileSystemEventArgs e)
    {
      if (!_resolvedFileNames.Contains(e.FullPath))
        return;

      if (null != _triggerBasedUpdate)
      {
        _triggerBasedUpdate.Trigger();
      }
    }

    private void EhAfterDeserializationHasCompletelyFinished()
    {
      _isDeserializationInProgress = false;
      UpdateWatching();
    }

    protected override void Dispose(bool disposing)
    {
      if (!IsDisposed)
        SwitchOffWatching();

      base.Dispose(disposing);
    }
  }
}
