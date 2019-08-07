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
using System.Linq;
using System.Text;

namespace Altaxo.Gui
{
  /// <summary>
  /// Can be used for a control to denote which type of controller can control this.
  /// </summary>
  public class UserControlForControllerAttribute : UserControlPriorityAttribute, IClassForClassAttribute
  {
    private System.Type _type;

    public UserControlForControllerAttribute(System.Type type)
      : base(0)
    {
      _type = type;
    }

    public UserControlForControllerAttribute(System.Type type, int priority)
      : base(priority)
    {
      _type = type;
    }

    public System.Type TargetType
    {
      get { return _type; }
    }


  }
}
