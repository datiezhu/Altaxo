#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2005 Dr. Dirk Lellinger
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Altaxo.Graph;

namespace Altaxo.Gui.Common.Drawing
{
  public partial class PenAllPropertiesControl : UserControl, Altaxo.Main.GUI.IMVCAController
  {
    

    public PenAllPropertiesControl()
    {
      InitializeComponent();
    }

    public PenHolder Pen
    {
      get { return _penGlue.Pen; }
      set
      { 
        _penGlue.Pen = value;
      }
    }


    #region IMVCController Members

    public object ViewObject
    {
      get
      {
        return this;
      }
      set
      {
        throw new Exception("The method or operation is not implemented.");
      }
    }

    public object ModelObject
    {
      get { return _penGlue.Pen; }
    }

    #endregion

    #region IApplyController Members

    public bool Apply()
    {
      return true;
    }

    #endregion
  }
}
