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
using Altaxo.Scripting;

namespace Altaxo.Gui.Scripting
{
  #region Interfaces

  public interface IPureScriptView
  {
    string ScriptText { get; set; }

    int ScriptCursorLocation { set; }

    int InitialScriptCursorLocation { set; }

    void SetScriptCursorLocation(int line, int column);

    void MarkText(int pos1, int pos2);
  }

  public interface IPureScriptController : IMVCANController
  {
    // Initializes or reinitializes the script controller.
    void SetText(string text);

    /// <summary>
    /// Sets the cursor location inside the script. Line and column are starting with 1.
    /// </summary>
    /// <param name="line">Script line (1-based).</param>
    /// <param name="column">Script column (1-based).</param>
    void SetScriptCursorLocation(int line, int column);

    void SetScriptCursorLocation(int offset);

    void SetInitialScriptCursorLocation(int offset);

    /// <summary>
    /// Gets the most current script text (if a view is present, it returns the script text of the view).
    /// </summary>
    /// <returns></returns>
    string GetCurrentScriptText();

    IPureScriptText Model { get; }
  }

  #endregion Interfaces

  /// <summary>
  /// Summary description for PureScriptController.
  /// </summary>
  [UserControllerForObject(typeof(IPureScriptText))]
  [ExpectedTypeOfView(typeof(IPureScriptView))]
  public class PureScriptController : IPureScriptController
  {
    private IPureScriptView _view;
    private IPureScriptText _doc;

    public PureScriptController()
    {
    }

    public PureScriptController(IPureScriptText doc)
    {
      InitializeDocument(doc);
    }

    #region IMVCANController Members

    public bool InitializeDocument(params object[] args)
    {
      if (args == null || args.Length == 0)
        return false;
      var doc = args[0] as IPureScriptText;
      if (doc == null)
        return false;

      _doc = doc;

      return true;
    }

    public UseDocument UseDocumentCopy
    {
      set { }
    }

    #endregion IMVCANController Members

    public void SetText(string text)
    {
      if (_view != null)
        _view.ScriptText = text;
    }

    public void Initialize()
    {
      if (_view != null)
        _view.ScriptText = _doc.ScriptText;
    }

    public void AttachView()
    {
    }

    public void DetachView()
    {
    }

    #region IMVCController Members

    public object ModelObject
    {
      get
      {
        return _doc;
      }
    }

    public object ViewObject
    {
      get
      {
        return _view;
      }
      set
      {
        if (null != _view)
        {
          DetachView();
        }

        _view = value as IPureScriptView;

        if (null != _view)
        {
          Initialize();
          AttachView();
        }
      }
    }

    public void Dispose()
    {
    }

    #endregion IMVCController Members

    #region IApplyController Members

    public bool Apply(bool disposeController)
    {
      if (_view != null)
      {
        _doc.ScriptText = _view.ScriptText;
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Try to revert changes to the model, i.e. restores the original state of the model.
    /// </summary>
    /// <param name="disposeController">If set to <c>true</c>, the controller should release all temporary resources, since the controller is not needed anymore.</param>
    /// <returns>
    ///   <c>True</c> if the revert operation was successfull; <c>false</c> if the revert operation was not possible (i.e. because the controller has not stored the original state of the model).
    /// </returns>
    public bool Revert(bool disposeController)
    {
      return false;
    }

    #endregion IApplyController Members

    #region IPureScriptController

    /// <summary>
    /// Sets the cursor location inside the script. Line and column are starting with 1.
    /// </summary>
    /// <param name="line">Script line (1-based).</param>
    /// <param name="column">Script column (1-based).</param>
    public void SetScriptCursorLocation(int line, int column)
    {
      if (_view != null)
        _view.SetScriptCursorLocation(line, column);
    }

    public void SetScriptCursorLocation(int offset)
    {
      if (_view != null)
        _view.ScriptCursorLocation = offset;
    }

    public void SetInitialScriptCursorLocation(int offset)
    {
      if (_view != null)
        _view.InitialScriptCursorLocation = offset;
    }

    /// <summary>
    /// Gets the most current script text (if a view is present, it returns the script text of the view).
    /// </summary>
    /// <returns></returns>
    public string GetCurrentScriptText()
    {
      if (_view != null)
        return _view.ScriptText;
      else
        return _doc.ScriptText;
    }

    public IPureScriptText Model
    {
      get
      {
        return _doc;
      }
    }

    #endregion IPureScriptController
  }
}
