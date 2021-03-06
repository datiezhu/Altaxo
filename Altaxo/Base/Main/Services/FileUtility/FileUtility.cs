﻿// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Altaxo.Collections;

namespace Altaxo.Main.Services
{
  public enum FileErrorPolicy
  {
    Inform,
    ProvideAlternative
  }

  public enum FileOperationResult
  {
    OK,
    Failed,
    SavedAlternatively
  }

  public delegate void FileOperationDelegate();

  public delegate void NamedFileOperationDelegate(FileName fileName);

  public delegate void NamedFileOrFolderOperationDelegate(PathName fileName);


  /// <summary>
  /// A utility class related to file utilities.
  /// </summary>
  public static partial class FileUtility
  {
    private static readonly char[] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
    private static string applicationRootPath = AppDomain.CurrentDomain.BaseDirectory;
    private const string fileNameRegEx = @"^([a-zA-Z]:)?[^:]+$";

    public static string ApplicationRootPath
    {
      get
      {
        return applicationRootPath;
      }
      set
      {
        applicationRootPath = value;
      }
    }

    public static bool IsUrl(string path)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      return path.IndexOf("://", StringComparison.Ordinal) > 0;
    }

    public static bool IsEqualFileName(FileName fileName1, FileName fileName2)
    {
      return fileName1 == fileName2;
    }

    public static string GetCommonBaseDirectory(string dir1, string dir2)
    {
      if (dir1 == null || dir2 == null)
        return null;
      if (IsUrl(dir1) || IsUrl(dir2))
        return null;

      dir1 = NormalizePath(dir1);
      dir2 = NormalizePath(dir2);

      string[] aPath = dir1.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
      string[] bPath = dir2.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
      var result = new StringBuilder();
      int indx = 0;
      for (; indx < Math.Min(bPath.Length, aPath.Length); ++indx)
      {
        if (bPath[indx].Equals(aPath[indx], StringComparison.OrdinalIgnoreCase))
        {
          if (result.Length > 0)
            result.Append(Path.DirectorySeparatorChar);
          result.Append(aPath[indx]);
        }
        else
        {
          break;
        }
      }
      if (indx == 0)
        return null;
      else
        return result.ToString();
    }



    /// <summary>
    /// Converts a given absolute path and a given base path to a path that leads
    /// from the base path to the absoulte path. (as a relative path)
    /// </summary>
    public static string GetRelativePath(string baseDirectoryPath, string absPath)
    {
      if (string.IsNullOrEmpty(baseDirectoryPath))
      {
        return absPath;
      }
      if (IsUrl(absPath) || IsUrl(baseDirectoryPath))
      {
        return absPath;
      }

      baseDirectoryPath = NormalizePath(baseDirectoryPath);
      absPath = NormalizePath(absPath);

      string[] bPath = baseDirectoryPath != "." ? baseDirectoryPath.Split(separators) : new string[0];
      string[] aPath = absPath != "." ? absPath.Split(separators) : new string[0];
      int indx = 0;
      for (; indx < Math.Min(bPath.Length, aPath.Length); ++indx)
      {
        if (!bPath[indx].Equals(aPath[indx], StringComparison.OrdinalIgnoreCase))
          break;
      }

      if (indx == 0 && (Path.IsPathRooted(baseDirectoryPath) || Path.IsPathRooted(absPath)))
      {
        return absPath;
      }

      if (indx == bPath.Length && indx == aPath.Length)
      {
        return ".";
      }
      var erg = new StringBuilder();
      for (int i = indx; i < bPath.Length; ++i)
      {
        erg.Append("..");
        erg.Append(Path.DirectorySeparatorChar);
      }
      erg.Append(string.Join(Path.DirectorySeparatorChar.ToString(), aPath, indx, aPath.Length - indx));
      if (erg[erg.Length - 1] == Path.DirectorySeparatorChar)
        erg.Length -= 1;
      return erg.ToString();
    }

    /// <summary>
    /// Combines baseDirectoryPath with relPath and normalizes the resulting path.
    /// </summary>
    public static string GetAbsolutePath(string baseDirectoryPath, string relPath)
    {
      return NormalizePath(Path.Combine(baseDirectoryPath, relPath));
    }

    public static string RenameBaseDirectory(string fileName, string oldDirectory, string newDirectory)
    {
      fileName = NormalizePath(fileName);
      oldDirectory = NormalizePath(oldDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
      newDirectory = NormalizePath(newDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
      if (IsBaseDirectory(oldDirectory, fileName))
      {
        if (fileName.Length == oldDirectory.Length)
        {
          return newDirectory;
        }
        return Path.Combine(newDirectory, fileName.Substring(oldDirectory.Length + 1));
      }
      return fileName;
    }

    public static void DeepCopy(string sourceDirectory, string destinationDirectory, bool overwrite)
    {
      if (!Directory.Exists(destinationDirectory))
      {
        Directory.CreateDirectory(destinationDirectory);
      }
      foreach (string fileName in Directory.GetFiles(sourceDirectory))
      {
        File.Copy(fileName, Path.Combine(destinationDirectory, Path.GetFileName(fileName)), overwrite);
      }
      foreach (string directoryName in Directory.GetDirectories(sourceDirectory))
      {
        DeepCopy(directoryName, Path.Combine(destinationDirectory, Path.GetFileName(directoryName)), overwrite);
      }
    }


    public static IEnumerable<FileName> LazySearchDirectory(string directory, string filemask, bool searchSubdirectories = true, bool ignoreHidden = true)
    {
      return SearchDirectoryInternal(directory, filemask, searchSubdirectories, ignoreHidden);
    }

    /// <summary>
    /// Finds all files which are valid to the mask <paramref name="filemask"/> in the path
    /// <paramref name="directory"/> and all subdirectories
    /// (if <paramref name="searchSubdirectories"/> is true).
    /// If <paramref name="ignoreHidden"/> is true, hidden files and folders are ignored.
    /// </summary>
    private static IEnumerable<FileName> SearchDirectoryInternal(string directory, string filemask, bool searchSubdirectories, bool ignoreHidden)
    {
      // If Directory.GetFiles() searches the 8.3 name as well as the full name so if the filemask is
      // "*.xpt" it will return "Template.xpt~"
      bool isExtMatch = filemask != null && Regex.IsMatch(filemask, @"^\*\.[\w\d_]{3}$");
      string ext = null;
      if (isExtMatch)
        ext = filemask.Substring(1);
      IEnumerable<string> dir = new[] { directory };

      if (searchSubdirectories)
        dir = dir.FlattenFromRootToLeaves(
            d =>
            {
              try
              {
                if (ignoreHidden)
                  return Directory.EnumerateDirectories(d).Where(IsNotHidden);
                else
                  return Directory.EnumerateDirectories(d);
              }
              catch (UnauthorizedAccessException)
              {
                return new string[0];
              }
            });
      foreach (string d in dir)
      {
        IEnumerable<string> files;
        try
        {
          files = Directory.EnumerateFiles(d, filemask);
        }
        catch (UnauthorizedAccessException)
        {
          continue;
        }
        foreach (string f in files)
        {
          if (ext != null && !f.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
            continue; // file extension didn't match
          if (!ignoreHidden || IsNotHidden(f))
            yield return new FileName(f);
        }
      }
    }

    private static bool IsNotHidden(string dir)
    {
      try
      {
        return (File.GetAttributes(dir) & FileAttributes.Hidden) != FileAttributes.Hidden;
      }
      catch (UnauthorizedAccessException)
      {
        return false;
      }
    }

    // This is an arbitrary limitation built into the .NET Framework.
    // Windows supports paths up to 32k length.
    public static readonly int MaxPathLength = 260;

    /// <summary>
    /// This method checks if a path (full or relative) is valid.
    /// </summary>
    public static bool IsValidPath(string fileName)
    {
      // Fixme: 260 is the hardcoded maximal length for a path on my Windows XP system
      //        I can't find a .NET property or method for determining this variable.

      if (fileName == null || fileName.Length == 0 || fileName.Length >= MaxPathLength)
      {
        return false;
      }

      // platform independend : check for invalid path chars

      if (fileName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
      {
        return false;
      }
      if (fileName.IndexOf('?') >= 0 || fileName.IndexOf('*') >= 0)
      {
        return false;
      }

      if (!Regex.IsMatch(fileName, fileNameRegEx))
      {
        return false;
      }

      if (fileName[fileName.Length - 1] == ' ')
      {
        return false;
      }

      if (fileName[fileName.Length - 1] == '.')
      {
        return false;
      }

      // platform dependend : Check for invalid file names (DOS)
      // this routine checks for follwing bad file names :
      // CON, PRN, AUX, NUL, COM1-9 and LPT1-9

      string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
      if (nameWithoutExtension != null)
      {
        nameWithoutExtension = nameWithoutExtension.ToUpperInvariant();
      }

      if (nameWithoutExtension == "CON" ||
              nameWithoutExtension == "PRN" ||
              nameWithoutExtension == "AUX" ||
              nameWithoutExtension == "NUL")
      {
        return false;
      }

      char ch = nameWithoutExtension.Length == 4 ? nameWithoutExtension[3] : '\0';

      return !((nameWithoutExtension.StartsWith("COM", StringComparison.Ordinal) ||
                          nameWithoutExtension.StartsWith("LPT", StringComparison.Ordinal)) &&
                       char.IsDigit(ch));
    }

    /// <summary>
    /// Checks that a single directory name (not the full path) is valid.
    /// </summary>
    public static bool IsValidDirectoryEntryName(string name)
    {
      if (!IsValidPath(name))
      {
        return false;
      }
      if (name.IndexOfAny(new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.VolumeSeparatorChar }) >= 0)
      {
        return false;
      }
      if (name.Trim(' ').Length == 0)
      {
        return false;
      }
      return true;
    }

    public static bool TestFileExists(string filename)
    {
      if (!File.Exists(filename))
      {
        var messageService = Altaxo.Current.GetRequiredService<IMessageService>();
        messageService.ShowWarning(StringParser.Parse("${res:Fileutility.CantFindFileError}", new StringTagPair("FILE", filename)));
        return false;
      }
      return true;
    }

    public static bool IsDirectory(string filename)
    {
      if (!Directory.Exists(filename))
      {
        return false;
      }
      FileAttributes attr = File.GetAttributes(filename);
      return (attr & FileAttributes.Directory) != 0;
    }

    //TODO This code is Windows specific
    private static bool MatchN(string src, int srcidx, string pattern, int patidx)
    {
      int patlen = pattern.Length;
      int srclen = src.Length;
      char next_char;

      for (; ; )
      {
        if (patidx == patlen)
          return (srcidx == srclen);
        next_char = pattern[patidx++];
        if (next_char == '?')
        {
          if (srcidx == src.Length)
            return false;
          srcidx++;
        }
        else if (next_char != '*')
        {
          if ((srcidx == src.Length) || (src[srcidx] != next_char))
            return false;
          srcidx++;
        }
        else
        {
          if (patidx == pattern.Length)
            return true;
          while (srcidx < srclen)
          {
            if (MatchN(src, srcidx, pattern, patidx))
              return true;
            srcidx++;
          }
          return false;
        }
      }
    }

    private static bool Match(string src, string pattern)
    {
      if (pattern[0] == '*')
      {
        // common case optimization
        int i = pattern.Length;
        int j = src.Length;
        while (--i > 0)
        {
          if (pattern[i] == '*')
            return MatchN(src, 0, pattern, 0);
          if (j-- == 0)
            return false;
          if ((pattern[i] != src[j]) && (pattern[i] != '?'))
            return false;
        }
        return true;
      }
      return MatchN(src, 0, pattern, 0);
    }

    public static bool MatchesPattern(string filename, string pattern)
    {
      filename = filename.ToUpperInvariant();
      pattern = pattern.ToUpperInvariant();
      string[] patterns = pattern.Split(';');
      foreach (string p in patterns)
      {
        if (Match(filename, p))
        {
          return true;
        }
      }
      return false;
    }

    // Observe SAVE functions
    public static FileOperationResult ObservedSave(FileOperationDelegate saveFile, PathName fileName, string message, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      System.Diagnostics.Debug.Assert(IsValidPath(fileName));
      try
      {
        saveFile();
        RaiseFileSaved(new FileNameEventArgs(fileName));
        return FileOperationResult.OK;
      }
      catch (IOException e)
      {
        return ObservedSaveHandleException(e, saveFile, fileName, message, policy);
      }
      catch (UnauthorizedAccessException e)
      {
        return ObservedSaveHandleException(e, saveFile, fileName, message, policy);
      }
    }

    private static FileOperationResult ObservedSaveHandleException(Exception e, FileOperationDelegate saveFile, PathName fileName, string message, FileErrorPolicy policy)
    {
      var messageService = Altaxo.Current.GetRequiredService<IMessageService>();
      switch (policy)
      {
        case FileErrorPolicy.Inform:
          messageService.InformSaveError(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", e);
          break;

        case FileErrorPolicy.ProvideAlternative:
          ChooseSaveErrorResult r = messageService.ChooseSaveError(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", e, false);
          if (r.IsRetry)
          {
            return ObservedSave(saveFile, fileName, message, policy);
          }
          else if (r.IsIgnore)
          {
            return FileOperationResult.Failed;
          }
          break;
      }
      return FileOperationResult.Failed;
    }

    public static FileOperationResult ObservedSave(FileOperationDelegate saveFile, FileName fileName, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      return ObservedSave(saveFile,
                                              fileName,
                                              Current.ResourceService.GetString("ICSharpCode.Services.FileUtilityService.CantSaveFileStandardText"),
                                              policy);
    }

    public static FileOperationResult ObservedSave(NamedFileOrFolderOperationDelegate saveFileAs, PathName fileName, string message, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      System.Diagnostics.Debug.Assert(IsValidPath(fileName));
      try
      {
        Directory.CreateDirectory(fileName.GetParentDirectory());
        saveFileAs(fileName);
        RaiseFileSaved(new FileNameEventArgs(fileName));
        return FileOperationResult.OK;
      }
      catch (IOException e)
      {
        return ObservedSaveHandleError(e, saveFileAs, fileName, message, policy);
      }
      catch (UnauthorizedAccessException e)
      {
        return ObservedSaveHandleError(e, saveFileAs, fileName, message, policy);
      }
    }


    private static FileOperationResult ObservedSaveHandleError(Exception e, NamedFileOrFolderOperationDelegate saveFileAs, PathName fileName, string message, FileErrorPolicy policy)
    {
      message = message + Environment.NewLine + Environment.NewLine + e.Message;
      var messageService = Altaxo.Current.GetRequiredService<IMessageService>();
      switch (policy)
      {
        case FileErrorPolicy.Inform:
          messageService.InformSaveError(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", e);
          break;

        case FileErrorPolicy.ProvideAlternative:
          ChooseSaveErrorResult r = messageService.ChooseSaveError(fileName, message, "${res:FileUtilityService.ErrorWhileSaving}", e, true);
          if (r.IsRetry)
          {
            return ObservedSave(saveFileAs, fileName, message, policy);
          }
          else if (r.IsIgnore)
          {
            return FileOperationResult.Failed;
          }
          else if (r.IsSaveAlternative)
          {
            return ObservedSave(saveFileAs, r.AlternativeFileName, message, policy);
          }
          break;
      }
      return FileOperationResult.Failed;
    }

    public static FileOperationResult ObservedSave(NamedFileOrFolderOperationDelegate saveFileAs, PathName fileName, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      return ObservedSave(saveFileAs,
                                              fileName,
                                              Current.ResourceService.GetString("ICSharpCode.Services.FileUtilityService.CantSaveFileStandardText"),
                                              policy);
    }

    // Observe LOAD functions
    public static FileOperationResult ObservedLoad(FileOperationDelegate loadFile, FileName fileName, string message, FileErrorPolicy policy)
    {
      try
      {
        loadFile();
        OnFileLoaded(new FileNameEventArgs(fileName));
        return FileOperationResult.OK;
      }
      catch (IOException e)
      {
        return ObservedLoadHandleException(e, loadFile, fileName, message, policy);
      }
      catch (UnauthorizedAccessException e)
      {
        return ObservedLoadHandleException(e, loadFile, fileName, message, policy);
      }
      catch (FormatException e)
      {
        return ObservedLoadHandleException(e, loadFile, fileName, message, policy);
      }
    }

    private static FileOperationResult ObservedLoadHandleException(Exception e, FileOperationDelegate loadFile, FileName fileName, string message, FileErrorPolicy policy)
    {
      message = message + Environment.NewLine + Environment.NewLine + e.Message;
      var messageService = Altaxo.Current.GetRequiredService<IMessageService>();
      switch (policy)
      {
        case FileErrorPolicy.Inform:
          messageService.InformSaveError(fileName, message, "${res:FileUtilityService.ErrorWhileLoading}", e);
          break;

        case FileErrorPolicy.ProvideAlternative:
          ChooseSaveErrorResult r = messageService.ChooseSaveError(fileName, message, "${res:FileUtilityService.ErrorWhileLoading}", e, false);
          if (r.IsRetry)
            return ObservedLoad(loadFile, fileName, message, policy);
          else if (r.IsIgnore)
            return FileOperationResult.Failed;
          break;
      }
      return FileOperationResult.Failed;
    }

    public static FileOperationResult ObservedLoad(FileOperationDelegate loadFile, FileName fileName, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      return ObservedLoad(loadFile,
                                              fileName,
                                              Current.ResourceService.GetString("ICSharpCode.Services.FileUtilityService.CantLoadFileStandardText"),
                                              policy);
    }

    public static FileOperationResult ObservedLoad(NamedFileOperationDelegate saveFileAs, FileName fileName, string message, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      return ObservedLoad(new FileOperationDelegate(delegate
      { saveFileAs(fileName); }), fileName, message, policy);
    }

    public static FileOperationResult ObservedLoad(NamedFileOperationDelegate saveFileAs, FileName fileName, FileErrorPolicy policy = FileErrorPolicy.Inform)
    {
      return ObservedLoad(saveFileAs,
                                              fileName,
                                              Current.ResourceService.GetString("ICSharpCode.Services.FileUtilityService.CantLoadFileStandardText"),
                                              policy);
    }

    private static void OnFileLoaded(FileNameEventArgs e)
    {
      if (FileLoaded != null)
      {
        FileLoaded(null, e);
      }
    }

    public static void RaiseFileSaved(FileNameEventArgs e)
    {
      if (FileSaved != null)
      {
        FileSaved(null, e);
      }
    }

    public static event EventHandler<FileNameEventArgs> FileLoaded;

    public static event EventHandler<FileNameEventArgs> FileSaved;
  }
}
