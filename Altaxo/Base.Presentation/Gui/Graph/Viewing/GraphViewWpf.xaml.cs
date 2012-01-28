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
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Altaxo.Gui.Graph.Viewing
{
	/// <summary>
	/// Interaction logic for GraphView.xaml
	/// </summary>
	public partial class GraphViewWpf : UserControl, Altaxo.Gui.Graph.Viewing.IGraphView, IDisposable
	{
		[Browsable(false)]
		private int _cachedCurrentLayer = -1;

		private WeakReference _controller = new WeakReference(null);

		private Altaxo.Gui.Graph.Viewing.PresentationGraphController _guiController;

		GdiToWpfBitmap _wpfGdiBitmap = new GdiToWpfBitmap(100,100);

		public GraphViewWpf()
		{
			InitializeComponent();
			_graphPanel.Source = _wpfGdiBitmap.WpfBitmap;
			_guiController = new PresentationGraphController(this);
		}

		public virtual void Dispose()
		{
			if (null != _wpfGdiBitmap)
			{
				_graphPanel.Source = null;
				_wpfGdiBitmap.Dispose();
				_wpfGdiBitmap = null;
			}
			_guiController = null;
			_controller = new WeakReference(null);
		}

		public Altaxo.Gui.Graph.Viewing.GraphController GC
		{
			get
			{
				return (Altaxo.Gui.Graph.Viewing.GraphController)_controller.Target;
			}
		}

		public Altaxo.Gui.Graph.Viewing.PresentationGraphController GuiController
		{
			get
			{
				return _guiController;
			}
		}

		public Altaxo.Graph.Gdi.GraphDocument Doc
		{
			get
			{
				var gc = GC;
				return null != gc ? gc.Doc : null;
			}
		}

		public Altaxo.Graph.Gdi.XYPlotLayer ActiveLayer
		{
			get
			{
				var gc = GC;
				return null != gc ? gc.ActiveLayer : null;
			}
		}

		public void SetGraphToolFromInternal(Altaxo.Gui.Graph.Viewing.GraphToolType value)
		{

			_guiController.GraphTool = value;
			bool result = _graphPanel.Focus();
			Keyboard.Focus(_graphPanel);

			var gc = GC;
			if (null != gc)
				gc.EhView_CurrentGraphToolChanged();
		}

		public void SetActiveLayerFromInternal(int layerNumber)
		{
			var gc = GC;
			if(null != gc)
			gc.EhView_CurrentLayerChoosen(layerNumber, false);
		}

		public void SetPanelCursor(Cursor cursor)
		{
			_graphPanel.Cursor = cursor;
		}


		public void SetHorizontalScrollbarParameter(bool isEnabled, double value, double maximum, double smallIncrement, double largeIncrement)
		{
			_horizontalScrollBar.IsEnabled = isEnabled;
			_horizontalScrollBar.Maximum = maximum;
			_horizontalScrollBar.SmallChange = smallIncrement;
			_horizontalScrollBar.LargeChange = largeIncrement;
			_horizontalScrollBar.Value = value;
		}

		public void SetVerticalScrollbarParameter(bool isEnabled, double value, double maximum, double smallIncrement, double largeIncrement)
		{
			_verticalScrollBar.IsEnabled = isEnabled;
			_verticalScrollBar.Maximum = maximum;
			_verticalScrollBar.SmallChange = smallIncrement;
			_verticalScrollBar.LargeChange = largeIncrement;
			_verticalScrollBar.Value = value;
		}


		/// <summary>
		/// Called by the PresentationGraphController to get a new graphics context for painting.
		/// </summary>
		/// <returns></returns>
		public System.Drawing.Graphics BeginPaintingGraph()
		{
			var result = _wpfGdiBitmap.GdiGraphics;
			result.ResetTransform();
			result.PageScale = 1;
			result.PageUnit = System.Drawing.GraphicsUnit.Pixel;
			return result;
		}

		/// <summary>
		/// Called by the PresentationGraphController when the painting (into the _wpfGdiBitmap) is finished.
		/// </summary>
		public void EndPaintingGraph()
		{
			_wpfGdiBitmap.WpfBitmap.Invalidate();
		}





		/// <summary>
		/// Triggers the rendering of the graph area by invalidating its visual. This function is safe to call from other threads.
		/// </summary>
		public void TriggerRenderingOfGraphArea()
		{
			Current.Gui.Execute(() => this.InvalidateVisual());
		}

	

		public void OnViewSelection()
		{

		}

		public void OnViewDeselection()
		{
			/* TODO (Wpf)
			if (null != _graphToolsToolBar)
				_graphToolsToolBar.Parent = null;
			*/
		}

		/// <summary>
		/// Gets the pixel dimensions of the graph bitmap.
		/// </summary>
		public System.Drawing.Size GraphSize
		{
			get
			{
				return _wpfGdiBitmap.GdiBitmap.Size;
			}
		}

		Altaxo.Graph.PointD2D GetMousePosition(MouseEventArgs e)
		{
			var p = e.GetPosition(_graphPanel);
			return new Altaxo.Graph.PointD2D(p.X, p.Y);
		}

		private void EhGraphPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (null != _guiController)
				_guiController.EhView_GraphPanelMouseMove(GetMousePosition(e),e);
		}

		private void EhGraphPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Keyboard.Focus(_graphPanel);
			var pos = GetMousePosition(e);
			if (null != _guiController)
			{
				_guiController.EhView_GraphPanelMouseDown(pos, e);
				if (e.ClickCount >= 2 && e.LeftButton == MouseButtonState.Pressed)
					_guiController.EhView_GraphPanelMouseDoubleClick(pos, e);
				else if (e.ClickCount == 1 && e.LeftButton == MouseButtonState.Pressed)
					_guiController.EhView_GraphPanelMouseClick(pos, e);
			}
		}

		private void EhGraphPanel_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (null != _guiController)
				_guiController.EhView_GraphPanelMouseUp(GetMousePosition(e),e);
		}

		private void EhGraphPanel_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (null != _guiController)
				_guiController.EhView_GraphPanelMouseWheel(GetMousePosition(e), e);
		}

		private void EhGraphPanel_KeyDown(object sender, KeyEventArgs e)
		{
			if (null != _guiController)
			{
				bool result = _guiController.EhView_ProcessCmdKey(e);
				e.Handled = result;
			}
		}

		private void EhGraphPanel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var gc = GC;
			if (null != gc)
			{
				int actWidth = (int)_graphPanel.ActualWidth;
				int actHeight = (int)_graphPanel.ActualHeight;
				if (actWidth > 0 && actHeight > 0)
				{
					_wpfGdiBitmap.Resize((int)_graphPanel.ActualWidth, (int)_graphPanel.ActualHeight);
					_wpfGdiBitmap.GdiBitmap.SetResolution(96, 96);
					_graphPanel.Source = _wpfGdiBitmap.WpfBitmap;
					gc.EhView_GraphPanelSizeChanged();

					_horizontalScrollBar.ViewportSize = actWidth;
					_verticalScrollBar.ViewportSize = actHeight;
				}
				else // either actWidth or actHeight was 0, thus the graph panel is momentarily invisible
				{
				}
			}
		}


		#region  Altaxo.Gui.Graph.Viewing.IGraphView

		object Altaxo.Gui.Graph.Viewing.IGraphView.GuiInitiallyFocusedElement
		{
			get
			{
				// make sure that the returned element implements IInputElement
				var result = _graphPanel;
				System.Diagnostics.Debug.Assert(null != (result as System.Windows.IInputElement));
				return result;
			}
		}

		Altaxo.Gui.Graph.Viewing.IGraphViewEventSink Altaxo.Gui.Graph.Viewing.IGraphView.Controller
		{
			set
			{
					_controller = new WeakReference(value as Altaxo.Gui.Graph.Viewing.GraphController); 
			}
		}

		/// <summary>
		/// Causes a complete redrawing of the graph. The cached graph bitmap will be marked as dirty and a repainting of the graph area is triggered with Gui render priority.
		/// Note: it is save to call this function from non-Gui threads.
		/// </summary>
			void Altaxo.Gui.Graph.Viewing.IGraphView.InvalidateCachedGraphBitmapAndRepaint()
		{
			// TODO (Wpf) -> start a background thread which draws the image
			// for the time being, we do the work directly
			if (null != _guiController)
				_guiController.InvalidateCachedGraphImage();
			TriggerRenderingOfGraphArea();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			_guiController.RepaintGraphAreaImmediately();
			base.OnRender(drawingContext);
		}

		public string GraphViewTitle
		{
			set 
			{
				// TODO (Wpf)
			}
		}

		public bool ShowGraphScrollBars
		{
			set 
			{
				_horizontalScrollBar.IsEnabled = value;
				_verticalScrollBar.IsEnabled = value;
			}
		}

		public System.Drawing.PointF GraphScrollPosition
		{
			get
			{
				return new System.Drawing.PointF((float)(_horizontalScrollBar.Value / _horizontalScrollBar.Maximum), (float)(_verticalScrollBar.Value / _verticalScrollBar.Maximum));
			}
			set
			{
				var controller = _guiController;
				_guiController = null; // suppress scrollbar events

				this._horizontalScrollBar.Value = (int)(value.X * _horizontalScrollBar.Maximum);
				this._verticalScrollBar.Value = (int)(value.Y * _verticalScrollBar.Maximum);

				_guiController = controller;
			}
		}

		public int NumberOfLayers
		{
			set
			{
				{
					int nNumButtons = _layerToolBar.Children.Count;

					if (value > nNumButtons)
					{
						for (int i = nNumButtons; i < value; i++)
						{

							var newbutton = new ToggleButton() { Content = i.ToString(), Tag = i };
							newbutton.Click += new RoutedEventHandler(EhLayerButton_Click);
							newbutton.ContextMenuOpening += new ContextMenuEventHandler(EhLayerButton_ContextMenuOpening);
							_layerToolBar.Children.Add(newbutton);
						}
					}
					else if (nNumButtons > value)
					{
						for (int i = nNumButtons - 1; i >= value; i--)
							_layerToolBar.Children.RemoveAt(i);
					}

					// now press the currently active layer button
					for (int i = 0; i < _layerToolBar.Children.Count; i++)
					{
						var button = (ToggleButton)_layerToolBar.Children[i];
						button.IsChecked = (i == _cachedCurrentLayer);
					}
				}
			}
		}

		void EhLayerButton_Click(object sender, RoutedEventArgs e)
		{
			var gc = GC;
			if (null != gc)
			{
				int pushedLayerNumber = (int)((ButtonBase)sender).Tag;

				gc.EhView_CurrentLayerChoosen(pushedLayerNumber, false);
			}
		}

		void EhLayerButton_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			var gc = GC;
			if (null != gc)
			{
				int i = (int)((ToggleButton)sender).Tag;
				GC.EhView_ShowDataContextMenu(i, this, new System.Drawing.Point((int)e.CursorLeft, (int)e.CursorTop));
			}
		}

		public int CurrentLayer
		{
			set
			{
				_cachedCurrentLayer = value;

				for (int i = 0; i < _layerToolBar.Children.Count; i++)
					((ToggleButton)_layerToolBar.Children[i]).IsChecked = (i == _cachedCurrentLayer);
			}
		}

		public System.Drawing.SizeF ViewportSizeInInch
		{
			get 
			{
				return new System.Drawing.SizeF((float)(_graphPanel.ActualWidth / 96.0), (float)( _graphPanel.ActualHeight / 96.0));
			}
		}

		public IList<Altaxo.Graph.Gdi.IHitTestObject> SelectedObjects
		{
			get 
			{
				return _guiController.SelectedObjects;
			} 
		}

		Altaxo.Gui.Graph.Viewing.GraphToolType Altaxo.Gui.Graph.Viewing.IGraphView.GraphTool
		{
			get
			{
				return _guiController.GraphTool;
			}
			set
			{
				_guiController.GraphTool = value;
			}
		}

		#endregion

		private void EhHorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollEventType == ScrollEventType.ThumbTrack)
				return;

			var gc = GC;
			if (null != gc)
				gc.EhView_Scroll();
		}

		private void EhVerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollEventType == ScrollEventType.ThumbTrack)
				return;

			var gc = GC;
			if (null != gc)
				gc.EhView_Scroll();
		}

		private void EhEnableCmdCopy(object sender, CanExecuteRoutedEventArgs e)
		{
			var gc = GC;
			e.CanExecute = null!=gc && gc.IsCmdCopyEnabled();
			e.Handled = true;
		}

		private void EhEnableCmdCut(object sender, CanExecuteRoutedEventArgs e)
		{
			var gc = GC;
			e.CanExecute = null!=gc && gc.IsCmdCutEnabled();
			e.Handled = true;
		}

		private void EhEnableCmdDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			var gc = GC;
			e.CanExecute = null != gc && gc.IsCmdDeleteEnabled();
			e.Handled = true;
		}

		private void EhEnableCmdPaste(object sender, CanExecuteRoutedEventArgs e)
		{
			var gc = GC;
			e.CanExecute = null != gc && gc.IsCmdPasteEnabled();
			e.Handled = true;
		}

		private void EhCmdCopy(object sender, ExecutedRoutedEventArgs e)
		{
			var gc = GC;
			if (null != gc)
			{
				gc.CopySelectedObjectsToClipboard();
				e.Handled = true;
			}
		}

		private void EhCmdCut(object sender, ExecutedRoutedEventArgs e)
		{
			var gc = GC;
			if (null != gc)
			{
				gc.CutSelectedObjectsToClipboard();
				e.Handled = true;
			}
		}

		private void EhCmdDelete(object sender, ExecutedRoutedEventArgs e)
		{
			var gc = GC;
			if (null != _controller)
			{
				gc.CmdDelete();
				e.Handled = true;
			}
		}

		private void EhCmdPaste(object sender, ExecutedRoutedEventArgs e)
		{
			var gc = GC;
			if (null != _controller)
			{
				gc.PasteObjectsFromClipboard();
				e.Handled = true;
			}
		}

	
	}
}
