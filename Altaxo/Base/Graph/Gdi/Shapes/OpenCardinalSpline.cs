﻿#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2007 Dr. Dirk Lellinger
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
using System.Drawing;
using System.Drawing.Drawing2D;
using Altaxo.Serialization;

namespace Altaxo.Graph.Gdi.Shapes
{
	[Serializable]
	public class OpenCardinalSpline : OpenPathShapeBase
	{
		static double _defaultTension=0.5;

		List<PointD2D> _curvePoints = new List<PointD2D>();
		double _tension = _defaultTension;


		#region Serialization

		#region Clipboard serialization

		protected OpenCardinalSpline(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			SetObjectData(this, info, context, null);
		}

		/// <summary>
		/// Serializes LineGraphic. 
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			OpenCardinalSpline s = this;
			base.GetObjectData(info, context);
		}
		/// <summary>
		/// Deserializes the LineGraphic Version 0.
		/// </summary>
		/// <param name="obj">The empty SLineGraphic object to deserialize into.</param>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		/// <param name="selector">The deserialization surrogate selector.</param>
		/// <returns>The deserialized LineGraphic.</returns>
		public override object SetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
		{
			OpenCardinalSpline s = (OpenCardinalSpline)base.SetObjectData(obj, info, context, selector);
			return s;
		}


		/// <summary>
		/// Finale measures after deserialization.
		/// </summary>
		/// <param name="obj">Not used.</param>
		public override void OnDeserialization(object obj)
		{
			base.OnDeserialization(obj);
		}
		#endregion

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(OpenCardinalSpline), 0)]
		class XmlSerializationSurrogate2 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				var s = (OpenCardinalSpline)obj;
				info.AddBaseValueEmbedded(s, typeof(OpenCardinalSpline).BaseType);
				info.AddValue("Tension", s._tension);
				info.CreateArray("Points", s._curvePoints.Count);
				for (int i = 0; i < s._curvePoints.Count; i++)
					info.AddValue("e", s._curvePoints[i]);
				info.CommitArray();
			}
			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{

				var s = null != o ? (OpenCardinalSpline)o : new OpenCardinalSpline();
				info.GetBaseValueEmbedded(s, typeof(OpenCardinalSpline).BaseType, parent);
				s._tension = info.GetDouble("Tension");
				s._curvePoints.Clear();
				int count = info.OpenArray("Points");
				for(int i=0;i<count;i++)
					s._curvePoints.Add((PointD2D)info.GetValue("e",s));
				info.CloseArray(count);
				return s;
			}
		}

		#endregion


		#region Constructors
		public OpenCardinalSpline()
		{
		}

		public OpenCardinalSpline(IEnumerable<PointD2D> points)
			: this(points,DefaultTension)
		{
		}


		public OpenCardinalSpline(IEnumerable<PointD2D> points, double tension)
		{
			_curvePoints.AddRange(points);
			_tension = Math.Abs(tension);

			if (!(_curvePoints.Count >=2))
				throw new ArgumentException("Number of curve points has to be >= 2");

			CalculateAndSetBounds();
		}


		public OpenCardinalSpline(OpenCardinalSpline from)
			: base(from)
		{
			_tension = from._tension;
			_curvePoints.Clear();
			_curvePoints.AddRange(from._curvePoints);
		}

		#endregion


		public static double DefaultTension { get { return _defaultTension; } }

		public override bool AllowNegativeSize
		{
			get
			{
				return true;
			}
		}

		public override object Clone()
		{
			return new OpenCardinalSpline(this);
		}


		void CalculateAndSetBounds()
		{
			var path = GetPath();
			var bounds = path.GetBounds();
			_position += bounds.Location;
			for (int i = 0; i < _curvePoints.Count; i++)
				_curvePoints[i] -= bounds.Location;
			_bounds = new RectangleD(0, 0, bounds.Width, bounds.Height);
			UpdateTransformationMatrix();
		}

		public void SetPoint(int idx, PointD2D newPos)
		{
			_curvePoints[idx] = newPos;
		}

		public GraphicsPath GetSelectionPath()
		{
			return GetPath();
		}

		/// <summary>
		/// Gets the path of the object in object world coordinates.
		/// </summary>
		/// <returns></returns>
		public override GraphicsPath GetObjectOutlineForArrangements()
		{
			return GetPath();
		}

		/// <summary>
		/// Gets the path of the object in object world coordinates.
		/// </summary>
		/// <returns></returns>
		protected GraphicsPath GetPath()
		{
			GraphicsPath gp = new GraphicsPath();

			PointF[] pt = new PointF[_curvePoints.Count];
			for(int i=0;i<_curvePoints.Count;i++)
				pt[i] = new PointF((float)_curvePoints[i].X, (float)_curvePoints[i].Y);
			gp.AddCurve(pt,(float)_tension);
			return gp;
		}

		public override IHitTestObject HitTest(HitTestPointData htd)
		{
			HitTestObjectBase result = null;
			GraphicsPath gp = GetPath();
			if (gp.IsOutlineVisible((PointF)htd.GetHittedPointInWorldCoord(_transformation), _linePen))
			{
				result = new OpenBSplineHitTestObject(this);
			}
			else
			{
				gp.Transform(htd.GetTransformation(_transformation)); // Transform to page coord
				if (gp.IsOutlineVisible((PointF)htd.HittedPointInPageCoord, new Pen(Color.Black, 6)))
				{
					result = new OpenBSplineHitTestObject(this);
				}
			}

			if (result != null)
				result.DoubleClick = EhHitDoubleClick;

			return result;
		}

		static bool EhHitDoubleClick(IHitTestObject o)
		{
			object hitted = o.HittedObject;
			Current.Gui.ShowDialog(ref hitted, "Line properties", true);
			((OpenCardinalSpline)hitted).OnChanged();
			return true;
		}





		public override void Paint(Graphics g, object obj)
		{
			GraphicsState gs = g.Save();
			TransformGraphics(g);
			Pen.SetEnvironment((RectangleF)_bounds, BrushX.GetEffectiveMaximumResolution(g, Math.Max(_scaleX, _scaleY)));
			var path = GetPath();
			g.DrawPath(Pen, path);
			if (_outlinePen != null && _outlinePen.IsVisible)
			{
				path.Widen(Pen);
				OutlinePen.SetEnvironment((RectangleF)_bounds, BrushX.GetEffectiveMaximumResolution(g, Math.Max(_scaleX, _scaleY)));
				g.DrawPath(OutlinePen, path);
			}

			g.Restore(gs);
		}



		protected class OpenBSplineHitTestObject : GraphicBaseHitTestObject
		{
			public OpenBSplineHitTestObject(OpenCardinalSpline parent)
				: base(parent)
			{
			}

			public override IGripManipulationHandle[] GetGrips(double pageScale, int gripLevel)
			{
				if (gripLevel <= 1)
				{
					OpenCardinalSpline ls = (OpenCardinalSpline)_hitobject;
					PointF[] pts = new PointF[ls._curvePoints.Count];
					for (int i = 0; i < pts.Length; i++)
					{
						pts[i] = (PointF)ls._curvePoints[i];
						var pt = ls._transformation.TransformPoint(pts[i]);
						pt = this.Transformation.TransformPoint(pt);
						pts[i] = pt;
					}

					IGripManipulationHandle[] grips = new IGripManipulationHandle[gripLevel == 0 ? 1 : 1+ls._curvePoints.Count];

					// Translation grips
					GraphicsPath path = new GraphicsPath();
					path.AddCurve(pts,(float)ls._tension);
					path.Widen(new Pen(Color.Black, (float)(6 / pageScale)));
					grips[grips.Length-1] = new MovementGripHandle(this, path, null);

					// PathNode grips
					if (gripLevel == 1)
					{
						float gripRadius = (float)(3 / pageScale);
						for (int i = 0; i < ls._curvePoints.Count; i++)
						{
							grips[i] = new BSplinePathNodeGripHandle(this, i, pts[i], gripRadius);
						}
					}
					return grips;
				}
				else
				{
					return base.GetGrips(pageScale, gripLevel);
				}
			}

		}

		class BSplinePathNodeGripHandle : PathNodeGripHandle
		{
			int _pointNumber;

			public BSplinePathNodeGripHandle(IHitTestObject parent, int pointNr, PointD2D gripCenter, double gripRadius)
			: base(parent,new PointD2D(0,0),gripCenter,gripRadius)
			{
				_pointNumber = pointNr;
			}


			public override void MoveGrip(PointD2D newPosition)
			{
					newPosition = _parent.Transformation.InverseTransformPoint(newPosition);
					var obj = (OpenCardinalSpline)GraphObject;
					newPosition = obj._transformation.InverseTransformPoint(newPosition);
					obj.SetPoint(_pointNumber, newPosition);
			}

			public override bool Deactivate()
			{
				var obj = (OpenCardinalSpline)GraphObject;
				obj.CalculateAndSetBounds();
				return false;
			}
		}
	} // End Class
} // end Namespace