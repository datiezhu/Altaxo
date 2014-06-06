﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2014 Dr. Dirk Lellinger
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
using Altaxo.DataConnection;
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

namespace Altaxo.Gui.DataConnection
{
	/// <summary>
	/// Interaction logic for QueryDesignerControl.xaml
	/// </summary>
	public partial class QueryDesignerControl : UserControl, IQueryDesignerView
	{
		public event Action ChooseConnectionString;

		public event Action<bool> GroupByChanged;

		public event Action ChooseProperties;

		public event Action CheckSql;

		public event Action ViewResults;

		public event Action ClearQuery;

		public event Action<NGTreeNode> TreeNodeMouseDoubleClick;

		public event Action<NGTreeNode, List<string>> RelatedTablesRequired;

		public event Action<NGTreeNode> HideTableChosen;

		public event Action ShowTablesAllChosen;

		public event Action<string> RelatedTableNameChosen;

		public QueryDesignerControl()
		{
			InitializeComponent();
			_grid.LoadingRow += EhGrid_Loading_Row;
		}

		private static IndexToImageConverter _treeImageConverter;

		public static IValueConverter TreeImageConverter
		{
			get
			{
				if (null == _treeImageConverter)
				{
					_treeImageConverter = new IndexToImageConverter(
							new string[]{
														"Icons.16x16.DataConnection.Table",
														"Icons.16x16.DataConnection.View",
														"Icons.16x16.DataConnection.Procedure",
														"Icons.16x16.DataConnection.Column",
													});
				}
				return _treeImageConverter;
			}
		}

		private void EhGrid_Loading_Row(object sender, DataGridRowEventArgs e)
		{
			UpdateGridColumns(_btnGroupBy.IsChecked == true);
		}

		public void UpdateSqlDisplay(string sqlText, bool isStatusVisible)
		{
			_txtSql.Text = sqlText;
			_lblStatus.Visibility = isStatusVisible ? Visibility.Visible : Visibility.Hidden;
		}

		public void SetDataGridDataSource(QueryFieldCollection data, bool isGrouped)
		{
			_grid.ItemsSource = data;

			FixGridColumns();

			// update UI
			UpdateGridColumns(isGrouped);
		}

		private DataGridColumn GetGridColumnByName(string name)
		{
			foreach (var c in _grid.Columns)
				if (name == (string)c.Header)
					return c;

			return null;
		}

		// update state of the grid columns
		public void UpdateGridColumns(bool isGrouped)
		{
			// freeze the first column so that it is visible even when horizontally scrolling
			var frozenCol = GetGridColumnByName("Column");
			if (null != frozenCol && !frozenCol.IsFrozen)
			{
				frozenCol.DisplayIndex = _grid.FrozenColumnCount;
				_grid.FrozenColumnCount += 1;
			}

			// make GroupBy column visible or invisible
			var groupByCol = GetGridColumnByName("GroupBy");
			if (null != groupByCol)
				groupByCol.Visibility = isGrouped ? Visibility.Visible : Visibility.Collapsed;

			_btnGroupBy.IsChecked = isGrouped;
		}

		// replace grid columns with ones with better editors
		private void FixGridColumns()
		{
			/*

			for (int i = 0; i < _grid.Columns.Count; i++)
			{
				var col = _grid.Columns[i];
				if (col.ValueType.IsEnum)
				{
					// create combo column for enum types
					var cmb = new DataGridViewComboBoxColumn();
					cmb.ValueType = col.ValueType;
					cmb.Name = col.Name;
					cmb.DataPropertyName = col.DataPropertyName;
					cmb.HeaderText = col.HeaderText;
					cmb.DisplayStyleForCurrentCellOnly = true;
					cmb.DataSource = Enum.GetValues(col.ValueType);
					cmb.Width = col.Width;

					// replace original column with new combo column
					_grid.Columns.RemoveAt(i);
					_grid.Columns.Insert(i, cmb);
				}
				else if (col.Name == "Filter")
				{
					var btn = new DataGridViewButtonColumn();
					btn.ValueType = col.ValueType;
					btn.Name = col.Name;
					btn.DataPropertyName = col.DataPropertyName;
					btn.HeaderText = col.HeaderText;
					btn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					btn.Width = col.Width;

					// replace original column with new combo column
					_grid.Columns.RemoveAt(i);
					_grid.Columns.Insert(i, btn);
				}
			}
			 */
		}

		private void _btnConnString_Click(object sender, RoutedEventArgs e)
		{
			var ev = ChooseConnectionString;
			if (null != ev)
			{
				ev();
			}
		}

		private void _btnGroupBy_Click(object sender, RoutedEventArgs e)
		{
			var ev = GroupByChanged;
			if (null != ev)
			{
				ev(_btnGroupBy.IsChecked == true);
			}
		}

		private void _btnProperties_Click(object sender, RoutedEventArgs e)
		{
			var ev = ChooseProperties;
			if (null != ev)
			{
				ev();
			}
		}

		private void _btnCheckSql_Click(object sender, RoutedEventArgs e)
		{
			var ev = CheckSql;
			if (null != ev)
			{
				ev();
			}
		}

		private void _btnViewResults_Click(object sender, RoutedEventArgs e)
		{
			var ev = ViewResults;
			if (null != ev)
			{
				ev();
			}
		}

		private void _btnClearQuery_Click(object sender, RoutedEventArgs e)
		{
			var ev = ClearQuery;
			if (null != ev)
			{
				ev();
			}
		}

		private void EhTreeMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var node = _treeTables.SelectedItem as Collections.NGTreeNode;
			if (null == node)
				return;

			var ev = TreeNodeMouseDoubleClick;
			if (null != ev)
				ev(node);
		}

		private void EhGrid_BeginEdit(object sender, DataGridBeginningEditEventArgs e)
		{
			if ("Filter" == ((string)e.Column.Header))
			{
				var field = e.Row.Item as QueryField;
				using (var dlg = new FilterEditController(field))
				{
					if (Current.Gui.ShowDialog(dlg, "Edit field"))
					{
						field.Filter = (string)dlg.ModelObject;
					}
				}
			}
		}

		/// <summary>
		/// Used to avoid stack overflow in <see cref="EhGrid_CellEndEdit"/>
		/// </summary>
		private bool _isReentrantEditCommit;

		private void EhGrid_CellEndEdit(object sender, DataGridCellEditEndingEventArgs e)
		{
			//see http://codefluff.blogspot.de/2010/05/commiting-bound-cell-changes.html how to commit individual cell changed and to handle reentrancy here
			if (!_isReentrantEditCommit)
			{
				_isReentrantEditCommit = true;
				_grid.CommitEdit(DataGridEditingUnit.Row, true);
				_isReentrantEditCommit = false;
			}
		}

		private void EhTreeViewItem_PreviewRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			var twi = sender as TreeViewItem;
			if (null != twi)
				twi.IsSelected = true;
		}

		private void EhTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (e.OriginalSource is DependencyObject)
			{
				var twi = GuiHelper.GetLogicalParentOfType<TreeViewItem>((DependencyObject)e.OriginalSource);
				if (null != twi)
				{
					twi.IsSelected = true;
				}
			}

			// get node that was clicked
			var nd = _treeTables.SelectedItem as Collections.NGTreeNode;
			System.Data.DataTable dt = nd == null ? null : nd.Tag as System.Data.DataTable;

			// make sure this is a table node
			if (dt == null)
			{
				return;
			}

			// populate related tables menu
			_contextMenuRelatedTables.Items.Clear();
			if (null != dt)
			{
				var list = new List<string>();

				var ev = RelatedTablesRequired;
				if (null != ev)
					ev(nd, list);

				foreach (string tableName in list)
				{
					MenuItem toAdd = new MenuItem() { Header = tableName, Tag = tableName };
					toAdd.Click += EhMenuRelatedTablesClicked;

					_contextMenuRelatedTables.Items.Add(toAdd);
				}
			}
			e.Handled = false;
		}

		private void EhMenuRelatedTablesClicked(object sender, RoutedEventArgs e)
		{
			var ev = RelatedTableNameChosen;
			if (null != ev)
			{
				string tableName = ((MenuItem)sender).Tag as string;
				ev(tableName);
			}
		}

		private void EhTreeMenu_HideThisTable(object sender, RoutedEventArgs e)
		{
			var ev = HideTableChosen;
			if (null != ev)
				ev(_treeTables.SelectedItem as Collections.NGTreeNode);
		}

		private void EhTreeMenu_ShowAllTables(object sender, RoutedEventArgs e)
		{
			var ev = ShowTablesAllChosen;
			if (null != ev)
				ev();
		}

		public void SetTableTreeDataSource(Collections.NGTreeNode rootNode)
		{
			_treeTables.ItemsSource = rootNode.Nodes;
		}
	}
}