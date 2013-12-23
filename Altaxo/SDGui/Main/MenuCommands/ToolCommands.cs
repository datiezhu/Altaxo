#region Copyright

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

using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Main.Commands
{
	public class AddTemporaryUserAssembly : AbstractMenuCommand
	{
		public override void Run()
		{
			Settings.Scripting.ReferencedAssembliesCommands.ShowAddTemporaryAssemblyDialog();
		}
	}

	public class TestProjectLoading : AbstractMenuCommand
	{
		public override void Run()
		{
			Altaxo.Main.Commands.TestAllProjectsInFolder.VerifyOpeningOfDocumentsWithoutException();
		}
	}

	public class ShowOptions : AbstractMenuCommand
	{
		public override void Run()
		{
			var ctrl = new Altaxo.Gui.Settings.SettingsController();
			Current.Gui.ShowDialog(ctrl, "Altaxo settings", false);
		}
	}

	public class RegisterApplicationForCom : AbstractMenuCommand
	{
		public override void Run()
		{
			Current.ComManager.RegisterApplicationForCom();
		}
	}

	public class UnregisterApplicationForCom : AbstractMenuCommand
	{
		public override void Run()
		{
			Current.ComManager.UnregisterApplicationForCom();
		}
	}

	public class CopyDocumentAsComObjectToClipboard : AbstractMenuCommand
	{
		public override void Run()
		{
			{
				Altaxo.Gui.SharpDevelop.SDGraphViewContent ctrl = Current.Workbench.ActiveViewContent as Altaxo.Gui.SharpDevelop.SDGraphViewContent;
				if (null != ctrl)
				{
					var doc = ((Altaxo.Gui.Graph.Viewing.GraphController)ctrl.MVCController).Doc;
					var dataObject = Current.ComManager.GetDocumentsComObjectForDocument(doc);
					if (null != dataObject)
						System.Windows.Clipboard.SetDataObject(dataObject);
				}
			}
		}
	}
}
