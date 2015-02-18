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

using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Graph.Scales
{
	using Boundaries;
	using Rescaling;
	using Ticks;

	/// <summary>
	/// Scales a full circle, either by degree or by radian. The origin is choosable, and the ticks default to ratios of 180� (or Pi, respectively).
	/// </summary>
	public abstract class AngularScale : NumericalScale
	{
		/// <summary>
		/// The value where this scale starts. Default is 0. The user/programmer can set this value manually.
		/// </summary>
		protected double _cachedAxisOrg;

		protected double _cachedAxisSpan;
		protected double _cachedOneByAxisSpan;
		protected Boundaries.NumericalBoundaries _dataBounds;
		protected Rescaling.AngularRescaleConditions _rescaling;
		protected Ticks.NumericTickSpacing _tickSpacing;

		#region Serialization

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase", "Altaxo.Graph.Scales.AngularScale", 1)]
		private class XmlSerializationSurrogate1 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				throw new InvalidOperationException("Serialization of old version");
				/*
				AngularScale s = (AngularScale)obj;

				info.AddValue("Rescaling", s._rescaling);
				*/
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				var s = (AngularScale)o;

				s._rescaling = (Rescaling.AngularRescaleConditions)info.GetValue("Rescaling", s);
				s._rescaling.ParentObject = s;
				s.SetCachedValues();
				s.UpdateTicksAndOrgEndUsingRescalingObject();
				return s;
			}
		}

		/// <summary>
		/// 2015-02-13 Added TickSpacing and bounds
		/// </summary>
		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(AngularScale), 2)]
		private class XmlSerializationSurrogate2 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				var s = (AngularScale)obj;

				info.AddValue("Org", s._cachedAxisOrg);
				info.AddValue("1BySpan", s._cachedOneByAxisSpan);

				info.AddValue("Bounds", s._dataBounds);
				info.AddValue("Rescaling", s._rescaling);
				info.AddValue("TickSpacing", s._tickSpacing);
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				var s = (AngularScale)o;

				s._cachedAxisOrg = (double)info.GetDouble("Org");
				s._cachedOneByAxisSpan = (double)info.GetDouble("1BySpan");

				s._rescaling = (Rescaling.AngularRescaleConditions)info.GetValue("Rescaling", s);
				s._rescaling.ParentObject = s;

				s._dataBounds = (Boundaries.NumericalBoundaries)info.GetValue("Bounds", s);
				s._dataBounds.ParentObject = s;

				s._tickSpacing = (Ticks.NumericTickSpacing)info.GetValue("TickSpacing", s);
				s._tickSpacing.ParentObject = s;

				s.SetCachedValues();
				s.UpdateTicksAndOrgEndUsingRescalingObject();

				return s;
			}
		}

		#endregion Serialization

		/// <summary>
		/// Constructor for deserialization only.
		/// </summary>
		protected AngularScale(Altaxo.Serialization.Xml.IXmlDeserializationInfo info)
		{
		}

		protected AngularScale(AngularTickSpacing tickSpacing)
		{
			if (null == tickSpacing)
				throw new ArgumentNullException("tickSpacing");

			_cachedAxisSpan = 2 * Math.PI;
			_cachedOneByAxisSpan = 1 / _cachedAxisSpan;
			_dataBounds = new Boundaries.DummyNumericalBoundaries() { ParentObject = this };
			_rescaling = new Rescaling.AngularRescaleConditions() { ParentObject = this };
			ChildSetMember(ref _tickSpacing, tickSpacing);
			SetCachedValues();
			UpdateTicksAndOrgEndUsingRescalingObject();
		}

		public AngularScale(AngularScale from)
		{
			CopyFrom(from);
		}

		public override bool CopyFrom(object obj)
		{
			if (object.ReferenceEquals(this, obj))
				return true;

			var from = obj as AngularScale;
			if (null == from)
				return false;

			using (var suspendToken = SuspendGetToken())
			{
				this._cachedAxisOrg = from._cachedAxisOrg;
				this._cachedAxisSpan = from._cachedAxisSpan;
				this._cachedOneByAxisSpan = from._cachedOneByAxisSpan;

				ChildCopyToMemberOrCreateNew(ref _dataBounds, from._dataBounds, () => new FiniteNumericalBoundaries());
				ChildCopyToMemberOrCreateNew(ref _rescaling, from._rescaling, () => new AngularRescaleConditions());
				ChildCopyToMember(ref _tickSpacing, from._tickSpacing);

				EhSelfChanged(EventArgs.Empty);
				suspendToken.Resume();
			}
			return true;
		}

		protected override IEnumerable<Main.DocumentNodeAndName> GetDocumentNodeChildrenWithName()
		{
			if (null != _dataBounds)
				yield return new Main.DocumentNodeAndName(_dataBounds, () => _dataBounds = null, "DataBounds");
			if (null != _rescaling)
				yield return new Main.DocumentNodeAndName(_rescaling, () => _rescaling = null, "Rescaling");
			if (null != _tickSpacing)
				yield return new Main.DocumentNodeAndName(_tickSpacing, () => _tickSpacing = null, "TickSpacing");
		}

		private void SetCachedValues()
		{
			double scaleOrigin = _rescaling.ScaleOrigin % 360;
			if (UseDegree)
			{
				_cachedAxisOrg = scaleOrigin;
				_cachedAxisSpan = 360;
				_cachedOneByAxisSpan = 1.0 / 360;
			}
			else
			{
				_cachedAxisOrg = scaleOrigin * Math.PI / 180;
				_cachedAxisSpan = 2 * Math.PI;
				_cachedOneByAxisSpan = 1 / (2 * Math.PI);
			}
		}

		#region Properties

		/// <summary>If true, use degree instead of radian.</summary>
		protected abstract bool UseDegree { get; }

		#endregion Properties

		#region NumericalScale

		public override double PhysicalToNormal(double x)
		{
			return (x - _cachedAxisOrg) * _cachedOneByAxisSpan;
		}

		public override double NormalToPhysical(double x)
		{
			return _cachedAxisOrg + x * _cachedAxisSpan;
		}

		/// <summary>
		/// PhysicalVariantToNormal translates physical values into a normal value linear along the axis
		/// a physical value of the axis origin must return a value of zero
		/// a physical value of the axis end must return a value of one
		/// the function physicalToNormal must be provided by any derived class
		/// </summary>
		/// <param name="x">the physical value</param>
		/// <returns>
		/// the normalized value linear along the axis,
		/// 0 for axis origin, 1 for axis end</returns>
		public override double PhysicalVariantToNormal(Altaxo.Data.AltaxoVariant x)
		{
			return PhysicalToNormal(x.ToDouble());
		}

		/// <summary>
		/// NormalToPhysicalVariant is the inverse function to PhysicalToNormal
		/// It translates a normalized value (0 for the axis origin, 1 for the axis end)
		/// into the physical value
		/// </summary>
		/// <param name="x">the normal value (0 for axis origin, 1 for axis end</param>
		/// <returns>the corresponding physical value</returns>
		public override Altaxo.Data.AltaxoVariant NormalToPhysicalVariant(double x)
		{
			return new Altaxo.Data.AltaxoVariant(NormalToPhysical(x));
		}

		private bool IsDoubleEqual(double x, double y, double dev)
		{
			return Math.Abs(x - y) < dev;
		}

		private double GetOriginInDegrees()
		{
			return _rescaling.ScaleOrigin % 360;
		}

		public override Altaxo.Graph.Scales.Rescaling.NumericScaleRescaleConditions Rescaling
		{
			get
			{
				return null;
			}
		}

		public override Altaxo.Graph.Scales.Boundaries.NumericalBoundaries DataBounds
		{
			get
			{
				return _dataBounds;
			}
		}

		public override double Org
		{
			get
			{
				return _cachedAxisOrg;
			}
		}

		public override double End
		{
			get
			{
				return _cachedAxisOrg + _cachedAxisSpan;
			}
		}

		protected override string SetScaleOrgEnd(Altaxo.Data.AltaxoVariant org, Altaxo.Data.AltaxoVariant end)
		{
			// ignore all this stuff, org and end are fixed here!
			/*
			double o = org.ToDouble();
			double e = end.ToDouble();

			if (!(o < e))
				return "org is not less than end";

			InternalSetOrgEnd(o, e, false, false);
			*/
			return null;
		}

		private void InternalSetOrgEnd(double org, double end, bool isOrgExtendable, bool isEndExtendable)
		{
			double angle = UseDegree ? org : org * 180 / Math.PI;
			// round the angle to full 90�
			angle = Math.Round(angle / 90);
			angle = Math.IEEERemainder(angle, 4);
			int scaleOrigin = (int)angle;
			org = UseDegree ? GetOriginInDegrees() : GetOriginInDegrees() * Math.PI / 180;
			double span = Math.Abs(end - org);

			bool changed = _cachedAxisOrg != org ||
				_cachedAxisSpan != span;

			_cachedAxisOrg = org;
			_cachedAxisSpan = span;
			_cachedOneByAxisSpan = 1 / _cachedAxisSpan;

			if (changed)
				EhSelfChanged(EventArgs.Empty);
		}

		public override void OnUserRescaled()
		{
		}

		#endregion NumericalScale

		public override object Clone()
		{
			throw new NotImplementedException();
		}

		public override Ticks.TickSpacing TickSpacing
		{
			get
			{
				return _tickSpacing;
			}
			set
			{
				if (null == value)
					throw new ArgumentNullException();
				if (!(value is Ticks.NumericTickSpacing))
					throw new ArgumentException("Value must be of type NumericTickSpacing");

				if (ChildSetMember(ref _tickSpacing, (Ticks.NumericTickSpacing)value))
				{
					EhChildChanged(Rescaling, EventArgs.Empty);
				}
			}
		}
	}
}