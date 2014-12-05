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

using Altaxo.Collections;
using Altaxo.Graph;
using Altaxo.Graph.Gdi;
using Altaxo.Graph.Gdi.Axis;
using Altaxo.Graph.Gdi.Plot;
using Altaxo.Graph.Gdi.Plot.Data;
using Altaxo.Graph.Gdi.Plot.Styles;
using Altaxo.Gui;
using Altaxo.Gui.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Altaxo.Gui.Graph
{
	#region Interfaces

	public interface IHostLayerView
	{
		void AddTab(string name, string text);

		object CurrentContent { get; set; }

		void SelectTab(string name);

		event CancelEventHandler TabValidating;

		event Action<string> PageChanged;
	}

	#endregion Interfaces

	/// <summary>
	/// Summary description for LayerController.
	/// </summary>
	[UserControllerForObject(typeof(HostLayer))]
	[ExpectedTypeOfView(typeof(IHostLayerView))]
	public class HostLayerController : MVCANControllerBase<HostLayer, IHostLayerView>
	{
		protected IDisposable _docSuspendLock;

		private string _currentPageName;

		private IMVCAController _currentController;

		protected IMVCANController _layerPositionController;
		protected IMVCANController _layerGraphItemsController;
		protected IMVCANController _layerGridController;

		private object _lastControllerApplied;

		private SelectableListNodeList _listOfUniqueItem;

		public HostLayerController(HostLayer layer)
			: this(layer, "Position")
		{
		}

		private HostLayerController(HostLayer layer, string currentPage)
		{
			_originalDoc = layer;
			_doc = (HostLayer)layer.Clone();

			_currentPageName = currentPage;

			Initialize(true);
		}

		protected override void AttachView()
		{
			_view.TabValidating += EhView_TabValidating;
			_view.PageChanged += EhView_PageChanged;
		}

		protected override void DetachView()
		{
			_view.TabValidating -= EhView_TabValidating;
			_view.PageChanged -= EhView_PageChanged;
		}

		public override bool Apply()
		{
			ApplyCurrentController(true);

			_originalDoc.CopyFrom(_doc, GraphCopyOptions.All); // _doc remains suspended

			return true;
		}

		protected override void Initialize(bool initData)
		{
			if (initData)
			{
				_docSuspendLock = _doc.SuspendGetToken();

				_listOfUniqueItem = new SelectableListNodeList();
				_listOfUniqueItem.Add(new SelectableListNode("Common", null, true));
			}

			if (null != _view)
			{
				// add all necessary Tabs
				_view.AddTab("GraphicItems", "GraphicItems");
				_view.AddTab("Position", "Position");
				_view.AddTab("HostGrid", "HostGrid");

				// Set the controller of the current visible Tab
				SetCurrentTabController(true);
			}
		}

		private void SetCurrentTabController(bool pageChanged)
		{
			switch (_currentPageName)
			{
				case "GraphicItems":
					if (pageChanged)
					{
						_view.SelectTab(_currentPageName);
					}
					if (null == _layerGraphItemsController)
					{
						_layerGraphItemsController = (IMVCANController)Current.Gui.GetControllerAndControl(new object[] { _doc.GraphObjects }, typeof(IMVCANController), UseDocument.Directly);
					}
					_currentController = _layerGraphItemsController;
					_view.CurrentContent = _currentController.ViewObject;
					break;

				case "Position":
					if (pageChanged)
					{
						_view.SelectTab(_currentPageName);
					}
					if (null == _layerPositionController)
					{
						_layerPositionController = new LayerPositionController() { UseDocumentCopy = UseDocument.Directly };
						_layerPositionController.InitializeDocument(_doc.Location, _doc);
						Current.Gui.FindAndAttachControlTo(_layerPositionController);
					}
					_currentController = _layerPositionController;
					_view.CurrentContent = _layerPositionController.ViewObject;
					break;

				case "HostGrid":
					if (pageChanged)
					{
						_view.SelectTab(_currentPageName);
					}
					if (null == _layerGridController)
					{
						_layerGridController = new GridPartitioningController() { UseDocumentCopy = UseDocument.Directly };
						_layerGridController.InitializeDocument(_doc.Grid, _doc);
						Current.Gui.FindAndAttachControlTo(_layerGridController);
					}
					_currentController = _layerGridController;
					_view.CurrentContent = _layerGridController.ViewObject;
					break;
			}
		}

		public void EhView_PageChanged(string firstChoice)
		{
			ApplyCurrentController(false);

			_currentPageName = firstChoice;
			SetCurrentTabController(true);
		}

		private void EhView_TabValidating(object sender, CancelEventArgs e)
		{
			if (!ApplyCurrentController(true))
				e.Cancel = true;
		}

		private bool ApplyCurrentController(bool force)
		{
			if (_currentController == null)
				return true;

			if (!force && object.ReferenceEquals(_currentController, _lastControllerApplied))
				return true;

			if (!_currentController.Apply())
				return false;

			if (object.ReferenceEquals(_currentController, _layerPositionController))
			{
				_doc.Location = (IItemLocation)_currentController.ModelObject;
			}

			_lastControllerApplied = _currentController;

			return true;
		}

		#region Dialog

		public static bool ShowDialog(HostLayer layer)
		{
			return ShowDialog(layer, "Position");
		}

		public static bool ShowDialog(HostLayer layer, string currentPage)
		{
			HostLayerController ctrl = new HostLayerController(layer, currentPage);
			return Current.Gui.ShowDialog(ctrl, layer.Name, true);
		}

		#endregion Dialog

		#region Edit Handlers

		public static void RegisterEditHandlers()
		{
			// register here editor methods

			HostLayer.LayerPositionEditorMethod = new DoubleClickHandler(EhLayerPositionEdit);
		}

		public static bool EhLayerPositionEdit(IHitTestObject hit)
		{
			XYPlotLayer layer = hit.HittedObject as XYPlotLayer;
			if (layer == null)
				return false;

			ShowDialog(layer, "Position");

			return false;
		}

		#endregion Edit Handlers
	}
}