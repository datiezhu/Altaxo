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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Altaxo.Main.Services.Files;
using Altaxo.Serialization.Xml;

namespace Altaxo.Main.Services
{
  /// <summary>
  /// Manages the permanent storage of projects into Zip files, including cloning to, and maintaining a safety copy.
  /// </summary>
  /// <seealso cref="Altaxo.Main.Services.IFileBasedProjectArchiveManager" />
  public class ZipFileProjectArchiveManager : IFileBasedProjectArchiveManager
  {
    const string ClonedProjectRelativePath = "CurrProj";
    const string ClonedProjectFileName = "CurrProj";

    bool _isDisposed;

    public event EventHandler<NameChangedEventArgs> FileOrFolderNameChanged;

    /// <summary>
    /// The stream of the original project file that is kept open in order to prevent modifications.
    /// </summary>
    FileStream _originalFileStream;

    /// <summary>
    /// The stream of a copy of the original project file. Is also kept open to prevent modifications.
    /// </summary>
    FileStream _clonedFileStream;
    Task _cloneTask;
    CancellationTokenSource _cloneTaskCancel;

    /// <summary>
    /// Gets the name of the file or folder. Can be null if no file or folder is set (up to now).
    /// </summary>
    /// <value>
    /// The name of the file or folder, if known. Otherwise, null is returned.
    /// </value>
    public PathName FileOrFolderName => FileName.Create(_originalFileStream?.Name);

    /// <inheritdoc/>
    public bool IsDisposed => _isDisposed;

    /// <summary>
    /// Loads a project from a file.
    /// </summary>
    /// <param name="fileName">Name of the file to load from.</param>
    /// <param name="restoreProjectAndWindowsState">Delegate that is used to deserialize and restore the project and the windows state.</param>
    /// <exception cref="ObjectDisposedException"></exception>
    public void LoadFromFile(FileName fileName, RestoreProjectAndWindowsState restoreProjectAndWindowsState)
    {
      if (_isDisposed) throw new ObjectDisposedException(this.GetType().Name);

      var oldFileName = _originalFileStream?.Name;
      bool hasFileNameChanged = 0 != string.Compare(fileName, _originalFileStream?.Name, false);

      CloneTask_CancelAndClearAll();

      try
      {
        // Open the stream for reading ...
        _originalFileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
      }
      catch (System.IO.IOException exIO)
      {
        FileStream roFileStream;
        try
        {
          roFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        catch (Exception ex)
        {
          _originalFileStream = null;
          throw exIO;
        }
        var shouldOpenReadonly = Current.Gui.YesNoMessageBox($"The file {fileName} seems to be read-only or currently in use.\r\n\r\nDo you want try to open it in read-only mode?", "Question", true);

        if (shouldOpenReadonly)
        {
          LoadFromFileStreamReadonly(roFileStream, restoreProjectAndWindowsState);
        }

        return;
      }
      catch (Exception ex)
      {
        _originalFileStream = null;
        throw;
      }

      // deserialize the project....
      using (var projectArchive = new Services.Files.ZipArchiveAsProjectArchive(_originalFileStream, ZipArchiveMode.Read, leaveOpen: true, archiveManager: this))
      {
        // Restore the state of the windows
        restoreProjectAndWindowsState(projectArchive);
      }

      // make a copy of the original file
      StartCloneTask();

      if (hasFileNameChanged)
        FileOrFolderNameChanged?.Invoke(this, new NameChangedEventArgs(this, oldFileName, _originalFileStream?.Name));
    }

    /// <summary>
    /// Loads a project from a file stream in read-only mode. For that, it is tried to make a copy of the file stream, and then
    /// use the copy to read the project from.
    /// </summary>
    /// <param name="fileStream">The file stream to copy from.</param>
    /// <param name="restoreProjectAndWindowsState">Delegate that is used to deserialize and restore the project and the windows state.</param>
    protected void LoadFromFileStreamReadonly(FileStream fileStream, RestoreProjectAndWindowsState restoreProjectAndWindowsState)
    {
      try
      {
        // Here, we can't copy the data to the cloned file in the background...
        // Instead, we have to wait for the end of the copy process, and then restore the project from the cloned file
        var clonedFileName = GetClonedFileName(fileStream.Name);
        fileStream.Seek(0, SeekOrigin.Begin);
        var clonedFileStream = new FileStream(clonedFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        fileStream.CopyTo(clonedFileStream);
        _originalFileStream = null;
        _clonedFileStream = clonedFileStream;
      }
      finally
      {
        fileStream.Dispose();
      }

      // now, deserialize the project from the cloned file....
      using (var projectArchive = new Services.Files.ZipArchiveAsProjectArchive(_clonedFileStream, ZipArchiveMode.Read, leaveOpen: true, archiveManager: this))
      {
        // Restore the state of the windows
        restoreProjectAndWindowsState(projectArchive);
      }
    }


    /// <summary>
    /// Saves the specified save project and windows state to the same file or folder that was used to open the project.
    /// </summary>
    /// <param name="saveProjectAndWindowsState">State of the save project and windows.</param>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException">Save is not possible because no file name was given up to now</exception>
    public void Save(SaveProjectAndWindowsStateDelegate saveProjectAndWindowsState)
    {
      if (_isDisposed) throw new ObjectDisposedException(this.GetType().Name);

      if (null == _originalFileStream)
        throw new InvalidOperationException("Save is not possible because no file name was given up to now");

      SaveAs(FileName.Create(_originalFileStream.Name), saveProjectAndWindowsState);
    }

    /// <summary>
    /// Saves the project with a name given in <paramref name="destinationFileName"/>. The name can or can not be the same name as was used before.
    /// </summary>
    /// <param name="destinationFileName">Name of the destination file.</param>
    /// <param name="saveProjectAndWindowsState">Delegate to store the project document and the windows state into an <see cref="IProjectArchive"/>.</param>
    /// <returns>A dictionary where the keys are the archive entry names that where used to store the project items that are the values. The dictionary contains only those project items that need further handling (e.g. late load handling).</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    public IDictionary<string, IProjectItem> SaveAs(FileName destinationFileName, SaveProjectAndWindowsStateDelegate saveProjectAndWindowsState)
    {
      if (_isDisposed) throw new ObjectDisposedException(this.GetType().Name);

      IDictionary<string, IProjectItem> dictionaryResult = null;

      var originalFileName = _originalFileStream?.Name;
      bool isNewDestinationFileName = destinationFileName != originalFileName;

      TryFinishCloneTask();  // Force decision whether we have a cloned file of the original file or not
      bool useClonedStreamAsBackup = _clonedFileStream != null;

      // Open the old archive, either using the copied stream or the original stream
      _clonedFileStream?.Seek(0, SeekOrigin.Begin);
      _originalFileStream?.Seek(0, SeekOrigin.Begin);

      // Create a new archive, either with the name of the original file (if we have the cloned file), or with a temporary file name
      FileStream newProjectArchiveFileStream = null;
      if (isNewDestinationFileName)
      {
        // create a new file stream for writing to
        newProjectArchiveFileStream = new FileStream(destinationFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
      }
      else if (useClonedStreamAsBackup)
      {
        // use the original file stream for writing to (but cut the length to zero)
        _originalFileStream.SetLength(0);
      }
      else // there is no cloned file in the local app folder yet
      {
        // create a file in the local app folder for writing to
        var instanceStorageService = Current.GetService<IInstanceStorageService>();
        var path = instanceStorageService.InstanceStoragePath;
        var clonedPath = Path.Combine(path, ClonedProjectRelativePath);
        var clonedFileName = Path.Combine(clonedPath, ClonedProjectFileName + Path.GetExtension(destinationFileName));
        Directory.CreateDirectory(clonedPath);
        newProjectArchiveFileStream = new FileStream(clonedFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
      }

      // now serialize the data
      Exception savingException = null;

      using (var oldProjectArchive = useClonedStreamAsBackup ?
                new Services.Files.ZipArchiveAsProjectArchive(_clonedFileStream, ZipArchiveMode.Read, leaveOpen: true, archiveManager: this) :
                _originalFileStream != null ?
                  new Services.Files.ZipArchiveAsProjectArchive(_originalFileStream, ZipArchiveMode.Read, leaveOpen: true, archiveManager: this) : null
            )
      {

        try
        {
          using (var newProjectArchive = new Services.Files.ZipArchiveAsProjectArchive(newProjectArchiveFileStream ?? _originalFileStream, ZipArchiveMode.Create, leaveOpen: true, archiveManager: this))
          {
            dictionaryResult = saveProjectAndWindowsState(newProjectArchive, oldProjectArchive);
          }
        }
        catch (Exception ex)
        {
          savingException = ex;
        }
      }

      if (null == savingException)
      {
        // if saving was successfull, we can now clone the data from the new project archive again....
        if (isNewDestinationFileName)
        {
          // we have written to a new file, so we take it as the original file, and clone this file
          _originalFileStream?.Close();
          _originalFileStream?.Dispose();
          _originalFileStream = newProjectArchiveFileStream;
          StartCloneTask();
        }
        else if (useClonedStreamAsBackup)
        {
          // we have written to the original file, and now we need to clone the original file again
          StartCloneTask();
        }
        else
        {
          // we have written to the appdata folder directly
          _clonedFileStream = newProjectArchiveFileStream;
          // but we have now to copy this data to the original file

          _originalFileStream?.Close();
          _originalFileStream?.Dispose();
          _originalFileStream = null;

          var orgFileStream = new FileStream(destinationFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
          _clonedFileStream.Seek(0, SeekOrigin.Begin);
          _clonedFileStream.CopyTo(orgFileStream);
          _originalFileStream = orgFileStream;
        }
      }
      else // exceptions suring saving have occured !!!
      {
        // if saving has failed, we have to restore the old state
        if (isNewDestinationFileName)
        {
          // there is nothing to do - except do close the new file stream
          // we leave it on disk for diagnosing purposes
          newProjectArchiveFileStream?.Close();
          newProjectArchiveFileStream.Dispose();
        }
        else if (useClonedStreamAsBackup)
        {
          // we have written to the original file, thus this original file is now corrupted!
          // so we try to restore it with the data from the cloned stream
          // this we will __not__ do in the background - we wait until the file is really restored!
          _clonedFileStream.Seek(0, SeekOrigin.Begin);
          _originalFileStream.Seek(0, SeekOrigin.Begin);
          _clonedFileStream.CopyTo(_originalFileStream);
          _originalFileStream.Flush();
          _originalFileStream.SetLength(_clonedFileStream.Length);
        }
        else
        {
          // we have written to the appdata folder directly
          // so the cloned file is now corrupted
          // so we delete the cloned file; then we try to clone the original (!) file again
          newProjectArchiveFileStream.Close();
          newProjectArchiveFileStream.Dispose();
          StartCloneTask();
        }
      }

      if (null != savingException)
        throw savingException;

      if (isNewDestinationFileName)
        FileOrFolderNameChanged?.Invoke(this, new NameChangedEventArgs(this, originalFileName, _originalFileStream?.Name));

      return dictionaryResult;
    }

    #region Clone task

    /// <summary>
    /// Starts a task to clone the original file into a file located in the local app data folder.
    /// </summary>
    void StartCloneTask()
    {
      _clonedFileStream?.Dispose(); // Close/dispose old cloned stream
      _clonedFileStream = null;

      var clonedFileName = GetClonedFileName();
      _cloneTaskCancel = new CancellationTokenSource();

      {
        var cancellationToken = _cloneTaskCancel.Token;
        var clonedFileStream = new FileStream(clonedFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        var orgStream = new FileStream(_originalFileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        _cloneTask = orgStream.CopyToAsync(clonedFileStream, 81920, cancellationToken)
          .ContinueWith(async (task1) =>
          {
            await clonedFileStream.FlushAsync(cancellationToken);
          }, cancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default)
          .ContinueWith((task2) =>
          {
            orgStream.Close();
            orgStream.Dispose();
            if (task2.Status == TaskStatus.RanToCompletion)
              _clonedFileStream = clonedFileStream;
            else
              clonedFileStream?.Dispose();
          });
      }
    }

    private string GetClonedFileName(string originalFileName = null)
    {
      var instanceStorageService = Current.GetService<IInstanceStorageService>();
      var path = instanceStorageService.InstanceStoragePath;
      var clonedFileDir = Path.Combine(path, ClonedProjectRelativePath);
      Directory.CreateDirectory(clonedFileDir);
      var clonedFileName = Path.Combine(clonedFileDir, ClonedProjectFileName + Path.GetExtension(originalFileName ?? _originalFileStream.Name));
      return clonedFileName;
    }

    /// <summary>
    /// If the clone task is still active, cancels the clone task and invalidates the clone stream.
    /// </summary>
    void CloneTask_CancelAndClearAll()
    {
      try
      {
        if (null != _cloneTask)
        {
          _cloneTaskCancel?.Cancel();
          if (null != _cloneTask && _cloneTask.Status == TaskStatus.Running)
          {
            _cloneTask.Wait();
          }
          while (!(_cloneTask.Status == TaskStatus.RanToCompletion || _cloneTask.Status == TaskStatus.Faulted || _cloneTask.Status == TaskStatus.Canceled))
          {
            System.Threading.Thread.Sleep(1);
          }
          _cloneTask?.Dispose();
          _cloneTask = null;
          _cloneTaskCancel?.Dispose();
          _cloneTaskCancel = null;
          _clonedFileStream?.Dispose();
          _clonedFileStream = null;
        }
      }
      catch (Exception ex)
      {

      }
    }

    /// <summary>
    /// Tests the state of the clone task. If it is finished, the call returns. If it is yet not finished, the task is cancelled, and the cloned stream is disposed.
    /// </summary>
    void TryFinishCloneTask()
    {
      if (null != _cloneTask)
      {
        if (!_cloneTask.IsCompleted)
        {
          _cloneTaskCancel.Cancel();
          if (null != _cloneTask && _cloneTask.Status == TaskStatus.Running)
          {
            _cloneTask.Wait();
          }

          // System.Diagnostics.Debug.WriteLine($"Status of clone task is {_cloneTask.Status}");

          // int slept = 0;
          while (!(_cloneTask.Status == TaskStatus.RanToCompletion || _cloneTask.Status == TaskStatus.Faulted || _cloneTask.Status == TaskStatus.Canceled))
          {
            System.Threading.Thread.Sleep(1);
            // slept += 1;
          }

          // System.Diagnostics.Debug.WriteLine($"Status of clone task is now {_cloneTask.Status}, slept {slept} ms");

          _cloneTask.Dispose();
          _cloneTask = null;
          _cloneTaskCancel.Dispose();
          _cloneTaskCancel = null;
          _clonedFileStream?.Dispose();
          _clonedFileStream = null;
        }
        else // Clone task runs to completion
        {
          if (!(_cloneTask.Exception is null))
          {
            _cloneTask?.Dispose();
            _cloneTask = null;
            _cloneTaskCancel?.Dispose();
            _cloneTaskCancel = null;
            _clonedFileStream?.Dispose();
            _clonedFileStream = null;
          }
        }
      }
    }


    /// <summary>
    /// Gets an archive, for read-only purposes only. The call to this function should be thread-safe.
    /// It is required to call <see cref="ReleaseArchiveThreadSave(object, ref IProjectArchive)" /> to release the returned archive if it is no longer in use.
    /// </summary>
    /// <param name="claimer">The claimer. If the returned archive is no longer</param>
    /// <returns>
    /// The archive that can be used to retrieve data (read-only).
    /// </returns>
    public IProjectArchive GetArchiveReadOnlyThreadSave(object claimer)
    {
      var name = _clonedFileStream?.Name ?? _originalFileStream?.Name;
      var stream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      var archive = new ZipArchiveAsProjectArchive(stream, ZipArchiveMode.Read, leaveOpen: false, archiveManager: this);
      return archive;
    }

    /// <summary>
    /// Releases the archive that was claimed with <see cref="GetArchiveReadOnlyThreadSave(object)" />.
    /// </summary>
    /// <param name="claimer">The claimer. This parameter should be identical to that used in the call to <see cref="GetArchiveReadOnlyThreadSave(object)" /></param>
    /// <param name="archive">The archive to release.</param>
    /// .
    public void ReleaseArchiveThreadSave(object claimer, ref IProjectArchive archive)
    {
      archive?.Dispose();
      archive = null;
    }



    /// <inheritdoc/>
    public void Dispose()
    {
      if (!_isDisposed)
      {
        _isDisposed = true;
        CloneTask_CancelAndClearAll();
        _originalFileStream?.Dispose();
        _originalFileStream = null;
      }
    }

    #endregion

  }
}