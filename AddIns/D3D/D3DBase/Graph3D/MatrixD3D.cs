﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2015 Dr. Dirk Lellinger
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
using System.Threading.Tasks;

namespace Altaxo.Graph3D
{
	public struct MatrixD3D
	{
		private double M11, M12, M13;
		private double M21, M22, M23;
		private double M31, M32, M33;
		private double OffsetX, OffsetY, OffsetZ;
		private double _determinant;

		private static MatrixD3D _identityMatrix;

		static MatrixD3D()
		{
			_identityMatrix = new MatrixD3D(
				1, 0, 0,
				0, 1, 0,
				0, 0, 1,
				0, 0, 0);
		}

		public static MatrixD3D Identity
		{
			get
			{
				return _identityMatrix;
			}
		}

		public MatrixD3D(
		double m11, double m12, double m13,
		double m21, double m22, double m23,
		double m31, double m32, double m33,
		double offsetX, double offsetY, double offsetZ)
		{
			M11 = m11; M12 = m12; M13 = m13;
			M21 = m21; M22 = m22; M23 = m23;
			M31 = m31; M32 = m32; M33 = m33;
			OffsetX = offsetX; OffsetY = offsetY; OffsetZ = offsetZ;

			_determinant = -(m13 * m22 * m31) + m12 * m23 * m31 + m13 * m21 * m32 - m11 * m23 * m32 - m12 * m21 * m33 + m11 * m22 * m33;
		}

		public VectorD3D Transform(VectorD3D v)
		{
			double x = v.X;
			double y = v.Y;
			double z = v.Z;
			return new VectorD3D(
			x * M11 + y * M21 + z * M31,
			x * M12 + y * M22 + z * M32,
			x * M13 + y * M23 + z * M33
			);
		}

		public PointD3D TransformPoint(PointD3D p)
		{
			return Transform(p);
		}

		public PointD3D Transform(PointD3D p)
		{
			double x = p.X;
			double y = p.Y;
			double z = p.Z;
			return new PointD3D(
			x * M11 + y * M21 + z * M31 + OffsetX,
			x * M12 + y * M22 + z * M32 + OffsetY,
			x * M13 + y * M23 + z * M33 + OffsetZ
			);
		}

		public void SetTranslationRotationShearScale(double translateX, double translateY, double translateZ, double angleX, double angleY, double angleZ, double shearX, double shearY, double shearZ, double scaleX, double scaleY, double scaleZ)
		{
			double phi;
			phi = angleX * Math.PI / 180;
			double cosX = Math.Cos(phi);
			double sinX = Math.Sin(phi);
			phi = angleY * Math.PI / 180;
			double cosY = Math.Cos(phi);
			double sinY = Math.Sin(phi);
			phi = angleZ * Math.PI / 180;
			double cosZ = Math.Cos(phi);
			double sinZ = Math.Sin(phi);

			M11 = scaleX * (cosY * cosZ - cosX * cosZ * shearY * sinY + shearY * sinX * sinZ);
			M12 = -(scaleX * (cosZ * shearY * sinX - cosY * sinZ + cosX * shearY * sinY * sinZ));
			M13 = scaleX * (cosX * cosY * shearY + sinY);
			M21 = scaleY * (cosY * cosZ * shearZ - cosZ * sinX * sinY - cosX * sinZ);
			M22 = scaleY * (cosX * cosZ + cosY * shearZ * sinZ - sinX * sinY * sinZ);
			M23 = scaleY * (cosY * sinX + shearZ * sinY);
			M31 = scaleZ * (cosY * cosZ * shearX * shearZ - cosZ * (cosX + shearX * sinX) * sinY + (-(cosX * shearX) + sinX) * sinZ);
			M32 = scaleZ * (cosY * shearX * shearZ * sinZ + cosX * (cosZ * shearX - sinY * sinZ) - sinX * (cosZ + shearX * sinY * sinZ));
			M33 = scaleZ * (cosX * cosY + cosY * shearX * sinX + shearX * shearZ * sinY);
			OffsetX = translateX;
			OffsetY = translateY;
			OffsetZ = translateZ;

			_determinant = scaleX * scaleY * scaleZ;
		}

		public static MatrixD3D FromTranslationRotationShearScale(double translateX, double translateY, double translateZ, double angleX, double angleY, double angleZ, double shearX, double shearY, double shearZ, double scaleX, double scaleY, double scaleZ)
		{
			var result = new MatrixD3D();
			result.SetTranslationRotationShearScale(translateX, translateY, translateZ, angleX, angleY, angleZ, shearX, shearY, shearZ, scaleX, scaleY, scaleZ);
			return result;
		}

		#region Prepend transformations

		public void TranslatePrepend(double x, double y, double z)
		{
			OffsetX += M11 * x + M21 * y + M31 * z;
			OffsetY += M12 * x + M22 * y + M32 * z;
			OffsetZ += M13 * x + M23 * y + M33 * z;
		}

		public void RotationXDegreePrepend(double angleX)
		{
			double phi;
			phi = angleX * Math.PI / 180;
			double cx = Math.Cos(phi);
			double sx = Math.Sin(phi);

			double h21 = M21, h22 = M22, h23 = M23;

			M21 = cx * M21 + M31 * sx;
			M22 = cx * M22 + M32 * sx;
			M23 = cx * M23 + M33 * sx;
			M31 = cx * M31 - h21 * sx;
			M32 = cx * M32 - h22 * sx;
			M33 = cx * M33 - h23 * sx;
		}

		public void RotationYDegreePrepend(double angleY)
		{
			double phi;
			phi = angleY * Math.PI / 180;
			double cy = Math.Cos(phi);
			double sy = Math.Sin(phi);

			double h11 = M11, h12 = M12, h13 = M13;

			M11 = cy * M11 + M31 * sy;
			M12 = cy * M12 + M32 * sy;
			M13 = cy * M13 + M33 * sy;
			M31 = cy * M31 - h11 * sy;
			M32 = cy * M32 - h12 * sy;
			M33 = cy * M33 - h13 * sy;
		}

		public void RotationZDegreePrepend(double angleZ)
		{
			double phi;
			phi = angleZ * Math.PI / 180;
			double cz = Math.Cos(phi);
			double sz = Math.Sin(phi);

			double h11 = M11, h12 = M12, h13 = M13;

			M11 = cz * M11 + M21 * sz;
			M12 = cz * M12 + M22 * sz;
			M13 = cz * M13 + M23 * sz;
			M21 = cz * M21 - h11 * sz;
			M22 = cz * M22 - h12 * sz;
			M23 = cz * M23 - h13 * sz;
		}

		public void PrependTransform(MatrixD3D a)
		{
			M11 = a.M11 * M11 + a.M12 * M21 + a.M13 * M31;
			M12 = a.M11 * M12 + a.M12 * M22 + a.M13 * M32;
			M13 = a.M11 * M13 + a.M12 * M23 + a.M13 * M33;
			M21 = a.M21 * M11 + a.M22 * M21 + a.M23 * M31;
			M22 = a.M21 * M12 + a.M22 * M22 + a.M23 * M32;
			M23 = a.M21 * M13 + a.M22 * M23 + a.M23 * M33;
			M31 = a.M31 * M11 + a.M32 * M21 + a.M33 * M31;
			M32 = a.M31 * M12 + a.M32 * M22 + a.M33 * M32;
			M33 = a.M31 * M13 + a.M32 * M23 + a.M33 * M33;
			OffsetX += a.OffsetX * M11 + a.OffsetY * M21 + a.OffsetZ * M31;
			OffsetY += a.OffsetX * M12 + a.OffsetY * M22 + a.OffsetZ * M32;
			OffsetZ += a.OffsetX * M13 + a.OffsetY * M23 + a.OffsetZ * M33;

			_determinant *= a._determinant;
		}

		#endregion Prepend transformations

		#region Inverse transformations

		public PointD3D InverseTransformPoint(PointD3D p)
		{
			return new PointD3D(
				(M23 * (M32 * (OffsetX - p.X) + M31 * (-OffsetY + p.Y)) + M22 * (-(M33 * OffsetX) + M31 * OffsetZ + M33 * p.X - M31 * p.Z) + M21 * (M33 * OffsetY - M32 * OffsetZ - M33 * p.Y + M32 * p.Z)) / _determinant,

				(M13 * (M32 * (-OffsetX + p.X) + M31 * (OffsetY - p.Y)) + M12 * (M33 * OffsetX - M31 * OffsetZ - M33 * p.X + M31 * p.Z) + M11 * (-(M33 * OffsetY) + M32 * OffsetZ + M33 * p.Y - M32 * p.Z)) / _determinant,

				(M13 * (M22 * (OffsetX - p.X) + M21 * (-OffsetY + p.Y)) + M12 * (-(M23 * OffsetX) + M21 * OffsetZ + M23 * p.X - M21 * p.Z) + M11 * (M23 * OffsetY - M22 * OffsetZ - M23 * p.Y + M22 * p.Z)) / _determinant
				);
		}

		public VectorD3D InverseTransformVector(VectorD3D p)
		{
			return new VectorD3D(
				(-(M23 * M32 * p.X) + M22 * M33 * p.X + M23 * M31 * p.Y - M21 * M33 * p.Y - M22 * M31 * p.Z + M21 * M32 * p.Z) / _determinant,

				(M13 * M32 * p.X - M12 * M33 * p.X - M13 * M31 * p.Y + M11 * M33 * p.Y + M12 * M31 * p.Z - M11 * M32 * p.Z) / _determinant,

				(-(M13 * M22 * p.X) + M12 * M23 * p.X + M13 * M21 * p.Y - M11 * M23 * p.Y - M12 * M21 * p.Z + M11 * M22 * p.Z) / _determinant

				);
		}

		#endregion Inverse transformations
	}
}