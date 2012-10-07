﻿#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2012 Dr. Dirk Lellinger
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
//    along with ctrl program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Altaxo.Collections;
using Altaxo.Graph;
using Altaxo.Graph.Gdi;
using Altaxo.Graph.ColorManagement;

namespace Altaxo.Gui.Common.Drawing
{
	/// <summary>
	/// Interaction logic for ColorComboBoxEx.xaml
	/// </summary>
	public partial class BrushComboBox : ColorComboBoxBase
	{
		public static readonly DependencyProperty SelectedBrushProperty;

		public event DependencyPropertyChangedEventHandler SelectedBrushChanged;

		List<BrushX> _lastLocalUsedItems = new List<BrushX>();


		#region Constructors

		static BrushComboBox()
		{
			SelectedBrushProperty =	DependencyProperty.Register("SelectedBrush", typeof(BrushX), typeof(BrushComboBox),	new FrameworkPropertyMetadata(new BrushX(NamedColors.Black), EhSelectedBrushChanged, EhSelectedBrushCoerce));
		}


		public BrushComboBox()
		{
			UpdateTreeViewTreeNodes();

			InitializeComponent();

			UpdateComboBoxSourceSelection(SelectedBrush);
			UpdateTreeViewSelection();
		}

		#endregion

		#region Implementation of abstract base class members

		protected override TreeView GuiTreeView { get { return _treeView; } }
		protected override ComboBox GuiComboBox { get { return _guiComboBox; } }
		protected override NamedColor InternalSelectedColor 
		{
			get
			{ 
				return SelectedBrush.Color; 
			}
			set
			{
				var selBrush = SelectedBrush;
				if (null != selBrush)
				{
					selBrush = selBrush.Clone();
					selBrush.Color = value;
					SelectedBrush = selBrush;
				}
			}
		}

		#endregion Implementation of abstract base class members

		#region Dependency property
		public BrushX SelectedBrush
		{
			get { return (BrushX)GetValue(SelectedBrushProperty); }
			set
			{
				SetValue(SelectedBrushProperty, value); 
			}
		}

    private static object EhSelectedBrushCoerce(DependencyObject obj, object coerceValue)
    {
      var thiss = (BrushComboBox)obj;
      return thiss.InternalSelectedBrushCoerce(obj, (BrushX)coerceValue);
    }

    protected virtual BrushX InternalSelectedBrushCoerce(DependencyObject obj, BrushX brush)
    {
			if (null == brush)
				brush = new BrushX(NamedColors.Transparent);

      var coercedColor = brush.Color.CoerceParentColorSetToNullIfNotMember();
      if (!brush.Color.Equals(coercedColor))
      {
        brush = brush.Clone();
        brush.Color = coercedColor;
      }

      if (this.ShowPlotColorsOnly && (brush.Color.ParentColorSet == null || false == brush.Color.ParentColorSet.IsPlotColorSet))
      {
        brush = brush.Clone();
        brush.Color = ColorSetManager.Instance.BuiltinDarkPlotColors[0];
      }
      return brush;
    }

		private static void EhSelectedBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			((BrushComboBox)obj).OnSelectedBrushChanged(obj, args);
		}

		protected virtual void OnSelectedBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var oldBrush = (BrushX)args.OldValue;
			var newBrush = (BrushX)args.NewValue;

			var oldColor = oldBrush.Color;
			var newColor = newBrush.Color;

      if (newBrush.BrushType != BrushType.SolidBrush || newBrush.Color.ParentColorSet == null)
      {
        StoreAsLastUsedItem(_lastLocalUsedItems, newBrush);
      }

			if (!newBrush.Equals(_guiComboBox.SelectedValue))
				this.UpdateComboBoxSourceSelection(newBrush);

			if (!object.ReferenceEquals(oldColor.ParentColorSet, newColor.ParentColorSet) && !object.ReferenceEquals(newColor.ParentColorSet, _treeView.SelectedValue))
				this.UpdateTreeViewSelection();


			if (null != SelectedBrushChanged)
				SelectedBrushChanged(obj, args);
		}

		#endregion

		#region ComboBox

		#region ComboBox data handling

		private void UpdateComboBoxSourceSelection(BrushX brush)
		{
			if (brush.Equals(_guiComboBox.SelectedValue))
				return;

			_filterString = string.Empty;
			FillComboBoxWithFilteredItems(_filterString, false);
			_guiComboBox.SelectedValue = brush;
		}


		List<object> _comboBoxSeparator1 = new List<object> { new Separator() { Name = "ThisIsASeparatorForTheComboBox", Tag = "Last used brushes" } };
		List<object> _comboBoxSeparator2 = new List<object> { new Separator() { Name = "ThisIsASeparatorForTheComboBox", Tag = "Color set" } };
		protected override bool FillComboBoxWithFilteredItems(string filterString, bool onlyIfItemsRemaining)
		{
			List<object> lastUsed;

			lastUsed = GetFilteredList(_lastLocalUsedItems, filterString, ShowPlotColorsOnly);

			var colorSet = GetColorSetForComboBox();
			var known = GetFilteredList(colorSet, filterString); 


			if ((lastUsed.Count + known.Count) > 0 || !onlyIfItemsRemaining)
			{
				IEnumerable<object> source = null;

				if (lastUsed.Count > 0)
				{
					source = _comboBoxSeparator1.Concat(lastUsed);
				}
				if (known.Count > 0)
				{
					(_comboBoxSeparator2[0] as Separator).Tag = colorSet.Name;
					if (source == null)
						source = _comboBoxSeparator2.Concat(known);
					else
						source = source.Concat(_comboBoxSeparator2).Concat(known);
				}
				_guiComboBox.ItemsSource = source;
				return true;
			}

			return false;
		}

		protected static List<object> GetFilteredList(IList<NamedColor> originalList, string filterString)
		{
			var result = new List<object>();
			filterString = filterString.ToLowerInvariant();
			foreach (var item in originalList)
			{
				if (item.Name.ToLowerInvariant().StartsWith(filterString))
					result.Add(new BrushX(item));
			}
			return result;
		}

		protected static List<object> GetFilteredList(IList<BrushX> originalList, string filterString, bool showPlotColorsOnly)
		{
			var result = new List<object>();
			filterString = filterString.ToLowerInvariant();
			foreach (var item in originalList)
			{
				if (showPlotColorsOnly && (item.Color.ParentColorSet == null || !item.Color.ParentColorSet.IsPlotColorSet))
					continue;

				if (item.Color.Name.ToLowerInvariant().StartsWith(filterString))
					result.Add(item);
			}
			return result;
		}

		#endregion ComboBox data

		#region ComboBox event handling

		void EhPopupClosed(object sender, EventArgs e)
		{
		}

		private void EhComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		protected void EhComboBox_DropDownClosed(object sender, EventArgs e)
		{
			if (_filterString.Length > 0)
			{
				var selItem = _guiComboBox.SelectedValue;
				_filterString = string.Empty;
				FillComboBoxWithFilteredItems(_filterString, false);
				_guiComboBox.SelectedValue = selItem;
			}

			if (_guiComboBox.SelectedValue == null)
			{
				_guiComboBox.SelectedValue = SelectedBrush;
			}
			else
			{
				if (_guiComboBox.SelectedValue is BrushX)
					this.SelectedBrush = (BrushX)_guiComboBox.SelectedValue;
				else
					this.SelectedBrush = new BrushX((NamedColor)_guiComboBox.SelectedValue);
			}
		}

		#endregion ComboBox event handling

		#endregion ComboBox

		#region Context menus

    private void EhShowCustomBrushDialog(object sender, RoutedEventArgs e)
    {
      var localBrush = this.SelectedBrush.Clone();
      var ctrl = new BrushControllerAdvanced();
      ctrl.RestrictBrushColorToPlotColorsOnly = ShowPlotColorsOnly;
      ctrl.InitializeDocument(localBrush);
      if (Current.Gui.ShowDialog(ctrl, "Edit brush properties", false))
        this.SelectedBrush = (BrushX)ctrl.ModelObject;
    }

		protected void EhShowCustomColorDialog(object sender, RoutedEventArgs e)
		{
			NamedColor newColor;
			if (base.InternalShowCustomColorDialog(sender, out newColor))
			{
				var newBrush = SelectedBrush.Clone();
				newBrush.Color = newColor;
				SelectedBrush = newBrush;
			}
		}

		protected void EhChooseOpacityFromContextMenu(object sender, RoutedEventArgs e)
		{
			NamedColor newColor;
			if (base.InternalChooseOpacityFromContextMenu(sender, out newColor))
			{
				var newBrush = SelectedBrush.Clone();
				newBrush.Color = newColor;
				SelectedBrush = newBrush;
			}
		}

		#endregion
	}
}
