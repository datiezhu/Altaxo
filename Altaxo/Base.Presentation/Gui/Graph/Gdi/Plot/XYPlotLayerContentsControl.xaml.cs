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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Altaxo.Collections;
using GongSolutions.Wpf.DragDrop;

namespace Altaxo.Gui.Graph.Gdi.Plot
{
  /// <summary>
  /// Interaction logic for XYPlotLayerContentsControl.xaml
  /// </summary>
  public partial class XYPlotLayerContentsControl : UserControl, IXYPlotLayerContentsView
  {
    private IXYPlotLayerContentsViewEventSink _controller;

    public XYPlotLayerContentsControl()
    {
      InitializeComponent();
    }

    public IEnumerable<object> AvailableItemsSelected
    {
      get
      {
        return _guiAvailableContent.SelectedItems;
      }
    }

    public IEnumerable<object> PlotItemsSelected
    {
      get
      {
        return _guiPlotItemsTree.SelectedItems;
      }
    }

    private void EhCommand_CopyCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = _guiPlotItemsTree.SelectedItems.Count > 0;
      e.Handled = true;
      object o = Keyboard.FocusedElement;
    }

    private void EhCommand_CopyExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Controller.PlotItems_Copy();
      e.Handled = true;
    }

    private void EhCommand_CutCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = _guiPlotItemsTree.SelectedItems.Count > 0;
      e.Handled = true;
      object o = Keyboard.FocusedElement;
    }

    private void EhCommand_CutExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Controller.PlotItems_Cut();
      e.Handled = true;
    }

    private void EhCommand_PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = Controller.PlotItems_CanPaste();
      e.Handled = true;
    }

    private void EhCommand_PasteExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Controller.PlotItems_Paste();
      e.Handled = true;
    }

    private void EhCommand_DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = Controller.PlotItems_CanDelete();
      e.Handled = true;
    }

    private void EhCommand_DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Controller.PlotItems_Delete();
      e.Handled = true;
    }

    private NGTreeNode[] SelectedNodes(Altaxo.Gui.Common.MultiSelectTreeView tree)
    {
      var result = tree.SelectedItems.OfType<NGTreeNode>().ToArray();
      NGTreeNode.SortByOrder(result);
      return result;
    }

    private void EhPutData_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.AvailableItems_PutDataToPlotItems();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhPlotItemsDelete_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_Delete();
      }
    }

    private void EhListSelUp_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_MoveUpSelected();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhListSelDown_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_MoveDownSelected();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhPlotAssociations_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItem_Open();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhGroup_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_GroupClick();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhUngroup_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_UngroupClick();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhEditRange_Click(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_EditRangeClick();
        _guiPlotItemsTree.Focus();
      }
    }

    private void EhShowRange_CheckedChanged(object sender, RoutedEventArgs e)
    {
      if (null != Controller)
      {
        Controller.PlotItems_ShowRangeChanged(_guiShowRange.IsChecked == true);
      }
    }

    public bool ShowRange { set { _guiShowRange.IsChecked = value; } }

    #region ILineScatterLayerContentsView

    public IXYPlotLayerContentsViewEventSink Controller
    {
      get
      {
        return _controller;
      }
      set
      {
        _controller = value;
      }
    }

    private Collections.NGTreeNodeCollection _dataAvailable;

    public void InitializeAvailableItems(Collections.NGTreeNodeCollection nodes)
    {
      var oldItems = _dataAvailable;
      _dataAvailable = nodes;
      if (oldItems != _dataAvailable)
        _guiAvailableContent.ItemsSource = _dataAvailable;
    }

    private Collections.NGTreeNodeCollection _layerContents;

    public void InitializePlotItems(Collections.NGTreeNodeCollection items)
    {
      var oldItems = _layerContents;
      _layerContents = items;
      if (oldItems != _layerContents)
        _guiPlotItemsTree.ItemsSource = _layerContents;
    }

    public object ControllerObject
    {
      get
      {
        return _controller;
      }
      set
      {
        _controller = value as IXYPlotLayerContentsViewEventSink;
      }
    }

    #endregion ILineScatterLayerContentsView

    private void EhItemMouseDoubleClick(object sender, EventArgs e)
    {
      if (null != Controller)
      {
        if (_guiPlotItemsTree.SelectedItems.Count == 1)
        {
          Controller.EhView_ContentsDoubleClick(_guiPlotItemsTree.SelectedItems.First() as NGTreeNode);
        }
        _guiPlotItemsTree.Focus();
      }
    }

    #region PlotItemTreeView Drag drop support

    private IDragSource _plotItemTreeDragHandler;

    public IDragSource PlotItemTreeDragHandler
    {
      get
      {
        if (null == _plotItemTreeDragHandler)
          _plotItemTreeDragHandler = new PlotItemTree_DragHandler(this);
        return _plotItemTreeDragHandler;
      }
    }

    public class PlotItemTree_DragHandler : IDragSource
    {
      private XYPlotLayerContentsControl _projectBrowseControl;

      public PlotItemTree_DragHandler(XYPlotLayerContentsControl ctrl)
      {
        _projectBrowseControl = ctrl;
      }

      public bool CanStartDrag(IDragInfo dragInfo)
      {
        return _projectBrowseControl._controller.PlotItems_CanStartDrag(dragInfo.SourceItems);
      }

      public void StartDrag(IDragInfo dragInfo)
      {
        _projectBrowseControl._controller.PlotItems_StartDrag(dragInfo.SourceItems, out var data, out var canCopy, out var canMove);

        dragInfo.Effects = GuiHelper.ConvertCopyMoveToDragDropEffect(canCopy, canMove);
        dragInfo.Data = data;
      }

      public void Dropped(IDropInfo dropInfo, DragDropEffects effects)
      {
        GuiHelper.ConvertDragDropEffectToCopyMove(effects, out var isCopy, out var isMove);

        _projectBrowseControl._controller.PlotItems_DragEnded(isCopy, isMove);
      }

      public void DragCancelled()
      {
        _projectBrowseControl._controller.PlotItems_DragCancelled();
      }
    }

    private IDropTarget _treeViewDropHandler;

    public IDropTarget PlotItemTreeDropHandler
    {
      get
      {
        if (null == _treeViewDropHandler)
          _treeViewDropHandler = new PlotItemTree_DropHandler(this);
        return _treeViewDropHandler;
      }
    }

    public class PlotItemTree_DropHandler : IDropTarget
    {
      private XYPlotLayerContentsControl _projectBrowseControl;

      public PlotItemTree_DropHandler(XYPlotLayerContentsControl ctrl)
      {
        _projectBrowseControl = ctrl;
      }

      public void DragOver(IDropInfo dropInfo)
      {
        if (CanAcceptData(dropInfo, out var resultingEffect, out var adornerType))
        {
          dropInfo.Effects = resultingEffect;
          dropInfo.DropTargetAdorner = adornerType;
        }
      }

      protected bool CanAcceptData(IDropInfo dropInfo, out System.Windows.DragDropEffects resultingEffect, out Type adornerType)
      {
        _projectBrowseControl._controller.PlotItems_DropCanAcceptData(
          dropInfo.Data is System.Windows.IDataObject ? GuiHelper.ToAltaxo((System.Windows.IDataObject)dropInfo.Data) : dropInfo.Data,
          dropInfo.TargetItem as Altaxo.Collections.NGTreeNode,
          GuiHelper.ToAltaxo(dropInfo.InsertPosition),
          dropInfo.KeyStates.HasFlag(DragDropKeyStates.ControlKey),
          dropInfo.KeyStates.HasFlag(DragDropKeyStates.ShiftKey),
          out var canCopy, out var canMove, out var itemIsSwallowingData);

        resultingEffect = GuiHelper.ConvertCopyMoveToDragDropEffect(canCopy, canMove);
        adornerType = itemIsSwallowingData ? DropTargetAdorners.Highlight : DropTargetAdorners.Insert;

        return canCopy | canMove;
      }

      public void Drop(IDropInfo dropInfo)
      {
        _projectBrowseControl._controller.PlotItems_Drop(
          dropInfo.Data is System.Windows.IDataObject ? GuiHelper.ToAltaxo((System.Windows.IDataObject)dropInfo.Data) : dropInfo.Data,
          dropInfo.TargetItem as Altaxo.Collections.NGTreeNode,
          GuiHelper.ToAltaxo(dropInfo.InsertPosition),
          dropInfo.KeyStates.HasFlag(DragDropKeyStates.ControlKey),
          dropInfo.KeyStates.HasFlag(DragDropKeyStates.ShiftKey),
          out var isCopy, out var isMove);

        dropInfo.Effects = GuiHelper.ConvertCopyMoveToDragDropEffect(isCopy, isMove); // it is important to get back the resulting effect to dropInfo, because dropInfo informs the drag handler about the resulting effect, which can e.g. delete the items after a move operation
      }
    }

    #endregion PlotItemTreeView Drag drop support

    #region AvailableItemTree_DragHander

    private IDragSource _availableItemTreeDragHandler;

    public IDragSource AvailableItemTreeDragHandler
    {
      get
      {
        if (null == _availableItemTreeDragHandler)
          _availableItemTreeDragHandler = new AvailableItemTree_DragHandler(this);
        return _availableItemTreeDragHandler;
      }
    }

    public class AvailableItemTree_DragHandler : IDragSource
    {
      private XYPlotLayerContentsControl _projectBrowseControl;

      public AvailableItemTree_DragHandler(XYPlotLayerContentsControl ctrl)
      {
        _projectBrowseControl = ctrl;
      }

      public bool CanStartDrag(IDragInfo dragInfo)
      {
        return _projectBrowseControl._controller.AvailableItems_CanStartDrag(dragInfo.SourceItems);
      }

      public void StartDrag(IDragInfo dragInfo)
      {
        _projectBrowseControl._controller.AvailableItems_StartDrag(dragInfo.SourceItems, out var data, out var canCopy, out var canMove);

        dragInfo.Effects = GuiHelper.ConvertCopyMoveToDragDropEffect(canCopy, canMove);
        dragInfo.Data = data;
      }

      public void Dropped(IDropInfo dropInfo, DragDropEffects effects)
      {
        GuiHelper.ConvertDragDropEffectToCopyMove(effects, out var isCopy, out var isMove);

        _projectBrowseControl._controller.AvailableItems_DragEnded(isCopy, isMove);
      }

      public void DragCancelled()
      {
        _projectBrowseControl._controller.AvailableItems_DragCancelled();
      }
    }

    #endregion AvailableItemTree_DragHander

    public void InitializeDataClipping(SelectableListNodeList list)
    {
      GuiHelper.Initialize(_guiClip, list);
    }

    private void EhClipSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      GuiHelper.SynchronizeSelectionFromGui(_guiClip);
    }
  }
}
