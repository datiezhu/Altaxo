﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Science
{
	public enum PhysicalDimension
	{
		DimensionLess,
		Length,
		Mass,
		Time,
		ElectricCurrent,
		Temperature,
		AmountOfSubstance,
		LuminousIntensity,

		// derived units
		Area,
		Volume,
		Velocity,
		Acceleration,
		WaveNumber,
		SpecificVolume,
		CurrentDensity,
		MagneticFieldStrength,
	}

	public struct SIUnit
	{
		sbyte _metre;
		sbyte _kilogram;
		sbyte _second;
		sbyte _ampere;
		sbyte _kelvin;
		sbyte _mole;
		sbyte _candela;

		static Dictionary<SIUnit, string> _specialNames;


		public SIUnit(sbyte m, sbyte kg, sbyte s, sbyte A, sbyte K, sbyte mol, sbyte CD)
		{
			_metre = m;
			_kilogram = kg;
			_second = s;
			_ampere = A;
			_kelvin = K;
			_mole = mol;
			_candela = CD;
		}

		void Multiply(SIUnit b)
		{
			this._metre += b._metre;
			this._kilogram += b._kilogram;
			this._second += b._second;
			this._ampere += b._ampere;
			this._kelvin += b._kelvin;
			this._mole += b._mole;
			this._candela += b._candela;
		}

		void DivideBy(SIUnit b)
		{
			this._metre -= b._metre;
			this._kilogram -= b._kilogram;
			this._second -= b._second;
			this._ampere -= b._ampere;
			this._kelvin -= b._kelvin;
			this._mole -= b._mole;
			this._candela -= b._candela;
		}

		void Invert()
		{
			this._metre = (sbyte)-this._metre;
			this._kilogram = (sbyte)-this._kilogram;
			this._second = (sbyte)-this._second;
			this._ampere = (sbyte)-this._ampere;
			this._kelvin = (sbyte)-this._kelvin;
			this._mole = (sbyte)-this._mole;
			this._candela = (sbyte)-this._candela;
		}


		public override bool Equals(object obj)
		{
			if (obj is SIUnit)
			{
				SIUnit b = (SIUnit)obj;
				return
				this._metre == b._metre &&
				this._kilogram == b._kilogram &&
				this._second == b._second &&
				this._ampere == b._ampere &&
				this._kelvin == b._kelvin &&
				this._mole == b._mole &&
				this._candela == b._candela;
			}
			else
				return false;
		}

		public override int GetHashCode()
		{
			return
				_metre << 24 +
				_kilogram << 20 +
				_second << 16 +
				_ampere << 12 +
				_kelvin << 8 +
				_mole << 4 +
				_candela;
		}
	}
}