﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altaxo.Collections;
using Altaxo.Main.Services;

namespace Altaxo.Gui.Units
{
	using System.ComponentModel;
	using Altaxo.Units;
	using System.Windows.Input;
	using System.Collections;

	public interface IUnitEnvironmentView : IDataContextAwareView
	{
	}

	/// <summary>
	/// Creates a custom unit environment
	/// </summary>
	[ExpectedTypeOfView(typeof(IUnitEnvironmentView))]
	public class UnitEnvironmentController : MVCANControllerEditCopyOfDocBase<QuantityWithUnitGuiEnvironment, IUnitEnvironmentView>, System.ComponentModel.INotifyPropertyChanged
	{
		private Dictionary<Type, UnitDescriptionAttribute> _listOfUnits = new Dictionary<Type, UnitDescriptionAttribute>();

		public event PropertyChangedEventHandler PropertyChanged;

		private string _quantity;
		private SelectableListNodeList _availableUnits = new SelectableListNodeList();
		private SelectableListNode _selectedAvailableUnit;
		private SelectableListNodeList _includedUnits = new SelectableListNodeList();
		private SelectableListNode _selectedIncludedUnit;
		private SelectableListNodeList _prefixes = new SelectableListNodeList();
		private SelectableListNodeList _allChoosenPrefixedUnits = new SelectableListNodeList();

		private Dictionary<IUnit, List<SIPrefix>> _prefixesForUnit = new Dictionary<IUnit, List<SIPrefix>>();

		protected override void Initialize(bool initData)
		{
			base.Initialize(initData);

			if (initData)
			{
				FillAvailableUnitsDictionary();
				GetAvailableUnitsForQuantity(_quantity, _availableUnits);
				AddToIncludedUnits = Current.Gui.NewRelayCommand(EhAddToIncludedUnits);
				RemoveFromIncludedUnits = Current.Gui.NewRelayCommand(EhRemoveFromIncludedUnits);
				SelectedPrefixesChangedCommand = Current.Gui.NewRelayCommand(EhSelectedPrefixesChangedCommand);
			}
		}

		public void SetQuantity(string quantity)
		{
			_quantity = quantity;

			if (!(string.IsNullOrEmpty(quantity)))
				GetAvailableUnitsForQuantity(quantity, _availableUnits);
		}

		private void EhRemoveFromIncludedUnits()
		{
			if (null != SelectedIncludedUnit)
			{
				_includedUnits.Remove(SelectedIncludedUnit);
			}
		}

		private void EhAddToIncludedUnits()
		{
			if (null != SelectedAvailableUnit && !IncludedUnits.Any(x => x.Tag == SelectedAvailableUnit.Tag))
			{
				var newNode = new SelectableListNode(SelectedAvailableUnit.Text, SelectedAvailableUnit.Tag, false);
				_includedUnits.Add(newNode);
				SelectedIncludedUnit = newNode;
			}
		}

		protected override void AttachView()
		{
			base.AttachView();
			_view.DataContext = this;
		}

		protected override void DetachView()
		{
			_view.DataContext = null;
			base.DetachView();
		}

		public override bool Apply(bool disposeController)
		{
			var prefixedUnits = new List<IUnit>();

			foreach (var entry in _prefixesForUnit)
			{
				if (entry.Value.Count == 1 && entry.Value[0] == SIPrefix.None)
				{
					prefixedUnits.Add(entry.Key); // add unit directly if there are no prefixes
				}
				else if (entry.Value.Count > 1)
				{
					prefixedUnits.Add(new UnitWithLimitedPrefixes(entry.Key, entry.Value));
				}
			}

			_doc = new QuantityWithUnitGuiEnvironment(prefixedUnits);
			_originalDoc = _doc;

			return ApplyEnd(true, disposeController);
		}

		public override IEnumerable<ControllerAndSetNullMethod> GetSubControllers()
		{
			yield break;
		}

		private void FillAvailableUnitsDictionary()
		{
			var types = ReflectionService.GetSortedClassTypesHavingAttribute(typeof(UnitDescriptionAttribute), false);

			var list = new HashSet<string>();

			foreach (var ty in types)
			{
				var attribute = (UnitDescriptionAttribute)ty.GetCustomAttributes(typeof(UnitDescriptionAttribute), false).First();
				_listOfUnits.Add(ty, attribute);
			}
		}

		private void GetAvailableUnitQuantities(SelectableListNodeList result)
		{
			result.Clear();

			var list = new HashSet<string>(_listOfUnits.Values.Select(x => x.Quantity));
			var quantities = list.ToArray();
			Array.Sort(quantities);

			foreach (var quantity in quantities)
			{
				result.Add(new SelectableListNode(quantity, quantity, false));
			}
		}

		private void GetAvailableUnitsForQuantity(string quantity, SelectableListNodeList result)
		{
			result.Clear();
			var unitTypes = _listOfUnits.Where(x => x.Value.Quantity == quantity).Select(x => x.Key);
			foreach (var ty in unitTypes)
			{
				var propInfo = ty.GetProperty("Instance");
				var propMethod = propInfo.GetGetMethod();
				if (null != propMethod)
				{
					var instance = (IUnit)propMethod.Invoke(null, null);
					result.Add(new SelectableListNode(instance.Name, instance, false));
				}
			}
		}

		private bool _selectedPrefixesChangedCommandDisabled;

		private void EhSelectedPrefixesChangedCommand()
		{
			if (!_selectedPrefixesChangedCommandDisabled)
				StoreSelectedPrefixes();
		}

		private void StoreSelectedPrefixes()
		{
			if (null != SelectedIncludedUnit)
			{
				var unit = (IUnit)SelectedIncludedUnit.Tag;
				var list = new List<SIPrefix>(Prefixes.Where(x => x.IsSelected).Select(x => (SIPrefix)x.Tag));
				_prefixesForUnit[unit] = list;
			}

			UpdateAllPrefixedUnits(_allChoosenPrefixedUnits);
		}

		private void GetAvailablePrefixes(IUnit unit, SelectableListNodeList result)
		{
			result.Clear();

			HashSet<SIPrefix> previouslySelectedPrefixes = null;

			if (_prefixesForUnit.TryGetValue(unit, out var list))
				previouslySelectedPrefixes = new HashSet<SIPrefix>(list);

			foreach (var prefix in unit.Prefixes)
			{
				bool isSelected = null != previouslySelectedPrefixes ? previouslySelectedPrefixes.Contains(prefix) : string.IsNullOrEmpty(prefix.Name);
				string name = prefix.Name;
				if (string.IsNullOrEmpty(prefix.Name))
					name = "<< without prefix >>";

				result.Add(new SelectableListNode(name, prefix, isSelected));
			}
		}

		private void UpdateAllPrefixedUnits(SelectableListNodeList result)
		{
			result.Clear();
			foreach (var entry in _prefixesForUnit)
			{
				foreach (var prefix in entry.Value)
				{
					result.Add(new SelectableListNode(prefix.ShortCut + entry.Key.ShortCut, null, false));
				}
			}
		}

		#region Binding properties

		public SelectableListNodeList AvailableUnits
		{
			get
			{
				return _availableUnits;
			}
		}

		public SelectableListNode SelectedAvailableUnit
		{
			get
			{
				return _selectedAvailableUnit;
			}
			set
			{
				if (!object.ReferenceEquals(_selectedAvailableUnit, value))
				{
					_selectedAvailableUnit = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAvailableUnit)));
				}
			}
		}

		public SelectableListNodeList IncludedUnits
		{
			get
			{
				return _includedUnits;
			}
		}

		public SelectableListNode SelectedIncludedUnit
		{
			get
			{
				return _selectedIncludedUnit;
			}
			set
			{
				if (!object.ReferenceEquals(_selectedIncludedUnit, value))
				{
					StoreSelectedPrefixes(); // store selected prefixes for old unit
					_selectedPrefixesChangedCommandDisabled = true; // temporary diabling of Store
					_selectedIncludedUnit = value;
					GetAvailablePrefixes((IUnit)_selectedIncludedUnit?.Tag, _prefixes);
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIncludedUnit)));
					_selectedPrefixesChangedCommandDisabled = false;
				}
			}
		}

		public SelectableListNodeList Prefixes
		{
			get
			{
				return _prefixes;
			}
		}

		public SelectableListNodeList AllPrefixedUnits
		{
			get
			{
				return _allChoosenPrefixedUnits;
			}
		}

		public ICommand AddToIncludedUnits { get; private set; }

		public ICommand RemoveFromIncludedUnits { get; private set; }

		public ICommand SelectedPrefixesChangedCommand { get; private set; }

		#endregion Binding properties
	}
}