﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Graph.Scales
{
	public class LinkedScaleParameters : ICloneable, Main.IChangedEventSource
	{
		/// <summary>The value a of x-axis link for link of origin: org' = a + b*org.</summary>
		private double _orgA;
		/// <summary>The value b of x-axis link for link of origin: org' = a + b*org.</summary>
		private double _orgB;
		/// <summary>The value a of x-axis link for link of end: end' = a + b*end.</summary>
		private double _endA;
		/// <summary>The value b of x-axis link for link of end: end' = a + b*end.</summary>
		private double _endB;

		public event EventHandler Changed;


		#region Serialization

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(LinkedScaleParameters), 0)]
		class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				LinkedScaleParameters s = (LinkedScaleParameters)obj;

				info.AddValue("OrgA", s._orgA);
				info.AddValue("OrgB", s._orgB);
				info.AddValue("EndA", s._endA);
				info.AddValue("EndB", s._endB);
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				LinkedScaleParameters s = SDeserialize(o, info, parent);
				return s;
			}


			protected virtual LinkedScaleParameters SDeserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				LinkedScaleParameters s = null != o ? (LinkedScaleParameters)o : new LinkedScaleParameters();

				s._orgA = info.GetDouble("OrgA");
				s._orgB = info.GetDouble("OrgB");
				s._endA = info.GetDouble("EndA");
				s._endB = info.GetDouble("EndB");

				return s;
			}
		}
		#endregion



		public LinkedScaleParameters()
		{
			_orgA = 0;
			_orgB = 1;
			_endA = 0;
			_endB = 1;
		}

		public LinkedScaleParameters(LinkedScaleParameters from)
		{
			CopyFrom(from);
		}

		public void CopyFrom(LinkedScaleParameters from)
		{
			// this call has the advantage, that a change event is raised when the parameter really change
			SetTo(from._orgA, from._orgB, from._endA, from.EndB);
		}

		public object Clone()
		{
			return new LinkedScaleParameters(this);
		}

		public bool IsStraightLink
		{
			get
			{
				return OrgA == 0 && OrgB == 1 && EndA == 0 && EndB == 1;
			}
		}

		public void SetToStraightLink()
		{
			SetTo(0, 1, 0, 1);
		}

		public void SetTo(LinkedScaleParameters from)
		{
			SetTo(from.OrgA, from.OrgB, from.EndA, from.EndB);
		}

		/// <summary>
		/// Set all parameters of the axis link by once.
		/// </summary>
		/// <param name="linktype">The type of the axis link, i.e. None, Straight or Custom.</param>
		/// <param name="orgA">The value a of x-axis link for link of axis origin: org' = a + b*org.</param>
		/// <param name="orgB">The value b of x-axis link for link of axis origin: org' = a + b*org.</param>
		/// <param name="endA">The value a of x-axis link for link of axis end: end' = a + b*end.</param>
		/// <param name="endB">The value b of x-axis link for link of axis end: end' = a + b*end.</param>
		public void SetTo(double orgA, double orgB, double endA, double endB)
		{

			if (
				(orgA != this.OrgA) ||
				(orgB != this.OrgB) ||
				(endA != this.EndA) ||
				(endB != this.EndB))
			{
				this._orgA = orgA;
				this._orgB = orgB;
				this._endA = endA;
				this._endB = endB;

				OnChanged();
			}
		}

		public double OrgA
		{
			get { return _orgA; }
			set
			{
				if (_orgA != value)
				{
					_orgA = value;
					OnChanged();
				}
			}
		}



		public double OrgB
		{
			get { return _orgB; }
			set
			{
				if (_orgB != value)
				{
					_orgB = value;
					OnChanged();
				}
			}
		}



		public double EndA
		{
			get { return _endA; }
			set
			{
				if (_endA != value)
				{
					_endA = value;
					OnChanged();
				}
			}
		}



		public double EndB
		{
			get { return _endB; }
			set
			{
				if (_endB != value)
				{
					_endB = value;
					OnChanged();
				}
			}
		}

		#region IChangedEventSource Members

		protected void OnChanged()
		{
			if (Changed != null)
				Changed(this, EventArgs.Empty);
		}

		#endregion
	}
}