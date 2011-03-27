﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Science
{
	public interface IRelativeUnit : IUnit
	{
		/// <summary>
		/// The corresponding quantity that this unit encapsulates.
		/// </summary>
		QuantityWithUnit ReferenceQuantity { get; }


		/// <summary>
		/// Returns the RelativeValue. RelativeValue is defined in such a way, that one piece of this unit is equal to RelativeValue times <see cref="ReferenceQuantity"/>. 
		/// Since the <see cref="Value"/> property can be different from the RelativeValue, it is neccessary to have a function for converting <see cref="Value"/> to RelativeValue.
		/// For instance, the value can be expressed in percent, thus the <see cref="Value"/> property amounts to 100 times the RelativeValue.
		/// </summary>
		double GetRelativeValueFromValue(double x);
	}


	/// <summary>
	/// This unit refers to a reference quantity. Since the reference quantity can be changed, instances of this class are <b>not</b> immutable.
	/// </summary>
	public class ChangeableRelativeUnit : IRelativeUnit
	{
		string _name;
		string _shortCut;
		protected double _divider;
		QuantityWithUnit _referenceQuantity;

		public ChangeableRelativeUnit(string name, string shortcut, double divider, QuantityWithUnit referenceQuantity)
		{
			_name = name;
			_shortCut = shortcut;
			_divider = divider;
			_referenceQuantity = referenceQuantity;
		}

		public string Name
		{
			get { return _name; }
		}

		public string ShortCut
		{
			get { return _shortCut; }
		}

		public QuantityWithUnit ReferenceQuantity
		{
			get
			{
				return _referenceQuantity;
			}
			set
			{
				_referenceQuantity = value;
			}
		}

		public double ToSIUnit(double x)
		{
			return (x / _divider) * _referenceQuantity.InSIUnits;
		}

		public double FromSIUnit(double x)
		{
			return _divider * x / _referenceQuantity.InSIUnits;
		}


		/// <summary>
		/// Returns the RelativeValue. RelativeValue is defined in such a way, that one piece of this unit is equal to RelativeValue times <see cref="ReferenceQuantity"/>. 
		/// Since the <see cref="Value"/> property can be different from the RelativeValue, it is neccessary to have a function for converting <see cref="Value"/> to RelativeValue.
		/// For instance, the value can be expressed in percent, thus the <see cref="Value"/> property amounts to 100 times the RelativeValue.
		/// </summary>
		public double GetRelativeValueFromValue(double x)
		{
			return x / _divider;
		}


		public ISIPrefixList Prefixes
		{
			get { return SIPrefix.ListWithNonePrefixOnly; }
		}

		public SIUnit SIUnit
		{
			get { return _referenceQuantity.Unit.SIUnit; }
		}
	}

	public class ChangeableRelativePercentUnit : ChangeableRelativeUnit
	{
		public ChangeableRelativePercentUnit(string fullName, QuantityWithUnit valueForHundredPercent)
			: base(fullName, "%", 100, valueForHundredPercent)
		{
		}
	}
}
