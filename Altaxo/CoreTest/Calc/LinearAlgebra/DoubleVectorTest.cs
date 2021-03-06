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

#endregion Copyright

using System;
using System.Collections;
using Altaxo.Calc.LinearAlgebra;
using NUnit.Framework;

namespace AltaxoTest.Calc.LinearAlgebra
{
  [TestFixture]
  public class DoubleVectorTest
  {
    private const double TOLERENCE = 0.001;

    //Test dimensions Constructor.
    [Test]
    public void CtorDimensions()
    {
      var test = new DoubleVector(2);

      Assert.AreEqual(test.Length, 2);
      Assert.AreEqual(test[0], 0);
      Assert.AreEqual(test[1], 0);
    }

    //Test Copy Constructor.
    [Test]
    public void CtorDimensionsNegative()
    {
      Assert.Throws(typeof(ArgumentException), () =>
      {
        var test = new DoubleVector(-1);
      });
    }

    //Test Intital Values Constructor.
    [Test]
    public void CtorInitialValues()
    {
      var test = new DoubleVector(2, 1);

      Assert.AreEqual(test.Length, 2);
      Assert.AreEqual(test[0], 1);
      Assert.AreEqual(test[1], 1);
    }

    //Test Array Constructor
    [Test]
    public void CtorArray()
    {
      double[] testvector = new double[2] { 0, 1 };

      var test = new DoubleVector(testvector);
      Assert.AreEqual(test.Length, testvector.Length);
      Assert.AreEqual(test[0], testvector[0]);
      Assert.AreEqual(test[1], testvector[1]);
    }

    //*TODO IList Constructor

    //Test Copy Constructor.
    [Test]
    public void CtorCopy()
    {
      var a = new DoubleVector(new double[2] { 0, 1 });
      var b = new DoubleVector(a);

      Assert.AreEqual(b.Length, a.Length);
      Assert.AreEqual(b[0], a[0]);
      Assert.AreEqual(b[1], a[1]);
    }

    //Test Copy Constructor.
    [Test]
    public void CtorCopyNull()
    {
      Assert.Throws(typeof(ArgumentNullException), () =>
      {
        DoubleVector a = null;
        var b = new DoubleVector(a);
      });
    }

    //Test Index Access
    [Test]
    public void IndexAccessGetNegative()
    {
      Assert.Throws(typeof(IndexOutOfRangeException), () =>
      {
        var a = new DoubleVector(new double[2] { 0, 1 });
        double b = a[-1];
      });
    }

    //Test Index Access
    [Test]
    public void IndexAccessSetNegative()
    {
      Assert.Throws(typeof(IndexOutOfRangeException), () =>
      {
        var a = new DoubleVector(2)
        {
          [-1] = 1
        };
      });
    }

    //Test Index Access
    [Test]
    public void IndexAccessGetOutOfRange()
    {
      Assert.Throws(typeof(IndexOutOfRangeException), () =>
      {
        var a = new DoubleVector(new double[2] { 0, 1 });
        double b = a[2];
      });
    }

    //Test Index Access
    [Test]
    public void IndexAccessSetOutOfRange()
    {
      Assert.Throws(typeof(IndexOutOfRangeException), () =>
      {
        var a = new DoubleVector(2)
        {
          [2] = 1
        };
      });
    }

    //Test Equals
    [Test]
    public void Equals()
    {
      var a = new DoubleVector(2, 4);
      var b = new DoubleVector(2, 4);
      var c = new DoubleVector(2)
      {
        [0] = 4,
        [1] = 4
      };

      var d = new DoubleVector(2, 5);
      DoubleVector e = null;
      var f = new FloatVector(2, 4);
      Assert.IsTrue(a.Equals(b));
      Assert.IsTrue(b.Equals(a));
      Assert.IsTrue(a.Equals(c));
      Assert.IsTrue(b.Equals(c));
      Assert.IsTrue(c.Equals(b));
      Assert.IsTrue(c.Equals(a));
      Assert.IsFalse(a.Equals(d));
      Assert.IsFalse(d.Equals(b));
      Assert.IsFalse(a.Equals(e));
      Assert.IsFalse(a.Equals(f));
    }

    //test GetHashCode
    [Test]
    public void TestHashCode()
    {
      var a = new DoubleVector(2)
      {
        [0] = 0,
        [1] = 1
      };

      int hash = a.GetHashCode();
      Assert.AreEqual(-1106247678, hash);
    }

    //Test GetInternalData
    [Test]
    public void GetInternalData()
    {
      double[] testvector = new double[2] { 0, 1 };
      var test = new DoubleVector(testvector);
      double[] internaldata = test.GetInternalData();

      Assert.AreEqual(internaldata.Length, testvector.Length);
      Assert.AreEqual(internaldata[0], testvector[0]);
      Assert.AreEqual(internaldata[1], testvector[1]);
    }

    //Test ToArray
    [Test]
    public void ToArray()
    {
      double[] testvector = new double[2] { 0, 1 };
      var test = new DoubleVector(testvector);
      double[] internaldata = test.ToArray();

      Assert.AreEqual(internaldata.Length, testvector.Length);
      Assert.AreEqual(internaldata[0], testvector[0]);
      Assert.AreEqual(internaldata[1], testvector[1]);
    }

    //Test GetSubVector
    [Test]
    public void GetSubVector()
    {
      var test = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var subvector = test.GetSubVector(1, 2);

      Assert.AreEqual(subvector.Length, 2);
      Assert.AreEqual(subvector[0], test[1]);
      Assert.AreEqual(subvector[1], test[2]);
    }

    //Test Implicit cast conversion to DoubleVector
    [Test]
    public void ImplicitConversion()
    {
      float[] a = new float[4] { 0, 1, 2, 3 };
      double[] b = new double[4] { 0, 1, 2, 3 };
      var c = new FloatVector(a);
      DoubleVector d, e, f;

      d = (DoubleVector)a;
      e = (DoubleVector)b;
      f = (DoubleVector)c;

      Assert.AreEqual(a.Length, d.Length);
      Assert.AreEqual((double)a[0], d[0]);
      Assert.AreEqual((double)a[1], d[1]);
      Assert.AreEqual((double)a[2], d[2]);
      Assert.AreEqual((double)a[3], d[3]);

      Assert.AreEqual(b.Length, e.Length);
      Assert.AreEqual(b[0], e[0]);
      Assert.AreEqual(b[1], e[1]);
      Assert.AreEqual(b[2], e[2]);
      Assert.AreEqual(b[3], e[3]);

      Assert.AreEqual(c.Length, f.Length);
      Assert.AreEqual((double)c[0], f[0]);
      Assert.AreEqual((double)c[1], f[1]);
      Assert.AreEqual((double)c[2], f[2]);
      Assert.AreEqual((double)c[3], f[3]);
    }

    //Test GetIndex functions
    [Test]
    public void GetIndex()
    {
      var a = new DoubleVector(new double[4] { 1, 2, 3, 4 });
      var b = new DoubleVector(new double[4] { 3, 2, 1, 0 });
      var c = new DoubleVector(new double[4] { 0, -1, -2, -3 });
      var d = new DoubleVector(new double[4] { -3, -2, -1, 0 });

      Assert.AreEqual(a.GetAbsMaximumIndex(), 3);
      Assert.AreEqual(b.GetAbsMaximumIndex(), 0);
      Assert.AreEqual(c.GetAbsMaximumIndex(), 3);
      Assert.AreEqual(d.GetAbsMaximumIndex(), 0);

      Assert.AreEqual(a.GetAbsMaximum(), 4);
      Assert.AreEqual(b.GetAbsMaximum(), 3);
      Assert.AreEqual(c.GetAbsMaximum(), -3);
      Assert.AreEqual(d.GetAbsMaximum(), -3);

      Assert.AreEqual(a.GetAbsMinimumIndex(), 0);
      Assert.AreEqual(b.GetAbsMinimumIndex(), 3);
      Assert.AreEqual(c.GetAbsMinimumIndex(), 0);
      Assert.AreEqual(d.GetAbsMinimumIndex(), 3);

      Assert.AreEqual(a.GetAbsMinimum(), 1);
      Assert.AreEqual(b.GetAbsMinimum(), 0);
      Assert.AreEqual(c.GetAbsMinimum(), 0);
      Assert.AreEqual(d.GetAbsMinimum(), 0);
    }

    //Test invalid dimensions with copy
    [Test]
    public void CopyException()
    {
      Assert.Throws(typeof(ArgumentException), () =>
      {
        var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
        var b = new DoubleVector(5);

        a.CopyFrom(b);
      });
    }

    //Test invalid dimensions with swap
    [Test]
    public void SwapException()
    {
      Assert.Throws(typeof(ArgumentException), () =>
      {
        var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
        var b = new DoubleVector(new double[5] { 4, 5, 6, 7, 8 });

        a.Swap(b);
      });
    }

    //Test Copy and Swap
    [Test]
    public void CopySwap()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });
      var c = new DoubleVector(4);
      var d = new DoubleVector(4);

      a.CopyFrom(c);
      b.CopyFrom(d);

      Assert.AreEqual(a.Length, c.Length);
      Assert.AreEqual(a[0], c[0]);
      Assert.AreEqual(a[1], c[1]);
      Assert.AreEqual(a[2], c[2]);
      Assert.AreEqual(a[3], c[3]);

      Assert.AreEqual(b.Length, d.Length);
      Assert.AreEqual(b[0], d[0]);
      Assert.AreEqual(b[1], d[1]);
      Assert.AreEqual(b[2], d[2]);
      Assert.AreEqual(b[3], d[3]);

      a.Swap(b);

      Assert.AreEqual(b.Length, c.Length);
      Assert.AreEqual(b[0], c[0]);
      Assert.AreEqual(b[1], c[1]);
      Assert.AreEqual(b[2], c[2]);
      Assert.AreEqual(b[3], c[3]);

      Assert.AreEqual(a.Length, d.Length);
      Assert.AreEqual(a[0], d[0]);
      Assert.AreEqual(a[1], d[1]);
      Assert.AreEqual(a[2], d[2]);
      Assert.AreEqual(a[3], d[3]);
    }

    //Test GetDotProduct
    [Test]
    public void GetDotProduct()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });

      Assert.AreEqual(a.GetDotProduct(), 14);
      Assert.AreEqual(b.GetDotProduct(), 126);
      Assert.AreEqual(a.GetDotProduct(b), 38);
      Assert.AreEqual(a.GetDotProduct(b), b.GetDotProduct(a));
    }

    //Test GetNorm
    [Test]
    public void GetNorm()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });
      var c = new DoubleVector(new double[4] { double.NaN, 5, 6, 7 });
      var d = new DoubleVector(new double[4] { 4, 5, 6, double.NaN });

      Assert.AreEqual(System.Math.Sqrt(14), a.L2Norm);
      Assert.AreEqual(a.L2Norm, a.LpNorm(2));
      Assert.AreEqual(3, a.LpNorm(0));

      Assert.AreEqual(3 * System.Math.Sqrt(14), b.L2Norm);
      Assert.AreEqual(b.L2Norm, b.LpNorm(2));
      Assert.AreEqual(7, b.LpNorm(0));

      Assert.IsNaN(c.L1Norm);
      Assert.IsNaN(c.L2Norm);
      Assert.IsNaN(c.LInfinityNorm);
      Assert.IsNaN(c.LpNorm(0));
      Assert.IsNaN(c.LpNorm(3));

      Assert.IsNaN(d.L1Norm);
      Assert.IsNaN(d.L2Norm);
      Assert.IsNaN(d.LInfinityNorm);
      Assert.IsNaN(d.LpNorm(0));
      Assert.IsNaN(d.LpNorm(3));
    }

    //Test GetSum
    [Test]
    public void GetSum()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });

      Assert.AreEqual(a.GetSum(), 6);
      Assert.AreEqual(a.GetSum(), a.GetSumMagnitudes());

      Assert.AreEqual(b.GetSum(), 22);
      Assert.AreEqual(b.GetSum(), b.GetSumMagnitudes());
    }

    //Test Axpy and Scale
    [Test]
    public void Axpy()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      double scal = 3;
      var b = new DoubleVector(4);

      b.Axpy(scal, a);
      a.Scale(scal);

      Assert.AreEqual(a[0], b[0]);
      Assert.AreEqual(a[1], b[1]);
      Assert.AreEqual(a[2], b[2]);
      Assert.AreEqual(a[3], b[3]);
    }

    //Test Negate
    [Test]
    public void Negate()
    {
      double[] vec = new double[4] { 0, 1, 2, 3 };
      var a = new DoubleVector(vec);
      DoubleVector b = -a;

      a = DoubleVector.Negate(a);

      Assert.AreEqual(-vec[0], a[0]);
      Assert.AreEqual(-vec[1], a[1]);
      Assert.AreEqual(-vec[2], a[2]);
      Assert.AreEqual(-vec[3], a[3]);

      Assert.AreEqual(-vec[0], b[0]);
      Assert.AreEqual(-vec[1], b[1]);
      Assert.AreEqual(-vec[2], b[2]);
      Assert.AreEqual(-vec[3], b[3]);
    }

    //Test Subtract
    [Test]
    public void Subtract()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });
      var c = new DoubleVector(a.Length);
      var d = new DoubleVector(b.Length);

      c = a - b;
      d = DoubleVector.Subtract(a, b);

      Assert.AreEqual(c[0], a[0] - b[0]);
      Assert.AreEqual(c[1], a[1] - b[1]);
      Assert.AreEqual(c[2], a[2] - b[2]);
      Assert.AreEqual(c[3], a[3] - b[3]);

      Assert.AreEqual(d[0], c[0]);
      Assert.AreEqual(d[1], c[1]);
      Assert.AreEqual(d[2], c[2]);
      Assert.AreEqual(d[3], c[3]);

      a.Subtract(b);

      Assert.AreEqual(c[0], a[0]);
      Assert.AreEqual(c[1], a[1]);
      Assert.AreEqual(c[2], a[2]);
      Assert.AreEqual(c[3], a[3]);
    }

    //Test Add
    [Test]
    public void Add()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });
      var c = new DoubleVector(a.Length);
      var d = new DoubleVector(b.Length);

      c = a + b;
      d = DoubleVector.Add(a, b);

      Assert.AreEqual(c[0], a[0] + b[0]);
      Assert.AreEqual(c[1], a[1] + b[1]);
      Assert.AreEqual(c[2], a[2] + b[2]);
      Assert.AreEqual(c[3], a[3] + b[3]);

      Assert.AreEqual(d[0], c[0]);
      Assert.AreEqual(d[1], c[1]);
      Assert.AreEqual(d[2], c[2]);
      Assert.AreEqual(d[3], c[3]);

      a.Add(b);

      Assert.AreEqual(c[0], a[0]);
      Assert.AreEqual(c[1], a[1]);
      Assert.AreEqual(c[2], a[2]);
      Assert.AreEqual(c[3], a[3]);
    }

    //Test Scale Mult and Divide
    [Test]
    public void ScalarMultiplyAndDivide()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var c = new DoubleVector(a);
      var d = new DoubleVector(a);
      double scal = -4;

      c.Multiply(scal);
      d.Divide(scal);

      Assert.AreEqual(c[0], a[0] * scal);
      Assert.AreEqual(c[1], a[1] * scal);
      Assert.AreEqual(c[2], a[2] * scal);
      Assert.AreEqual(c[3], a[3] * scal);

      Assert.AreEqual(d[0], a[0] / scal);
      Assert.AreEqual(d[1], a[1] / scal);
      Assert.AreEqual(d[2], a[2] / scal);
      Assert.AreEqual(d[3], a[3] / scal);

      c = a * scal;

      Assert.AreEqual(c[0], a[0] * scal);
      Assert.AreEqual(c[1], a[1] * scal);
      Assert.AreEqual(c[2], a[2] * scal);
      Assert.AreEqual(c[3], a[3] * scal);

      c = scal * a;

      Assert.AreEqual(c[0], a[0] * scal);
      Assert.AreEqual(c[1], a[1] * scal);
      Assert.AreEqual(c[2], a[2] * scal);
      Assert.AreEqual(c[3], a[3] * scal);
    }

    //Test Multiply
    [Test]
    public void Multiply()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var b = new DoubleVector(new double[4] { 4, 5, 6, 7 });
      var c = new DoubleMatrix(a.Length, b.Length);
      var d = new DoubleMatrix(a.Length, b.Length);

      c = a * b;
      d = DoubleVector.Multiply(a, b);

      Assert.AreEqual(c[0, 0], a[0] * b[0]);
      Assert.AreEqual(c[0, 1], a[0] * b[1]);
      Assert.AreEqual(c[0, 2], a[0] * b[2]);
      Assert.AreEqual(c[0, 3], a[0] * b[3]);
      Assert.AreEqual(c[1, 0], a[1] * b[0]);
      Assert.AreEqual(c[1, 1], a[1] * b[1]);
      Assert.AreEqual(c[1, 2], a[1] * b[2]);
      Assert.AreEqual(c[1, 3], a[1] * b[3]);
      Assert.AreEqual(c[2, 0], a[2] * b[0]);
      Assert.AreEqual(c[2, 1], a[2] * b[1]);
      Assert.AreEqual(c[2, 2], a[2] * b[2]);
      Assert.AreEqual(c[2, 3], a[2] * b[3]);
      Assert.AreEqual(c[3, 0], a[3] * b[0]);
      Assert.AreEqual(c[3, 1], a[3] * b[1]);
      Assert.AreEqual(c[3, 2], a[3] * b[2]);
      Assert.AreEqual(c[3, 3], a[3] * b[3]);

      Assert.AreEqual(d[0, 0], a[0] * b[0]);
      Assert.AreEqual(d[0, 1], a[0] * b[1]);
      Assert.AreEqual(d[0, 2], a[0] * b[2]);
      Assert.AreEqual(d[0, 3], a[0] * b[3]);
      Assert.AreEqual(d[1, 0], a[1] * b[0]);
      Assert.AreEqual(d[1, 1], a[1] * b[1]);
      Assert.AreEqual(d[1, 2], a[1] * b[2]);
      Assert.AreEqual(d[1, 3], a[1] * b[3]);
      Assert.AreEqual(d[2, 0], a[2] * b[0]);
      Assert.AreEqual(d[2, 1], a[2] * b[1]);
      Assert.AreEqual(d[2, 2], a[2] * b[2]);
      Assert.AreEqual(d[2, 3], a[2] * b[3]);
      Assert.AreEqual(d[3, 0], a[3] * b[0]);
      Assert.AreEqual(d[3, 1], a[3] * b[1]);
      Assert.AreEqual(d[3, 2], a[3] * b[2]);
      Assert.AreEqual(d[3, 3], a[3] * b[3]);
    }

    //Test Divide
    [Test]
    public void Divide()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      var c = new DoubleVector(a);
      var d = new DoubleVector(a);
      double scal = -4;

      c = a / scal;
      d = DoubleVector.Divide(a, scal);

      Assert.AreEqual(c[0], a[0] / scal);
      Assert.AreEqual(c[1], a[1] / scal);
      Assert.AreEqual(c[2], a[2] / scal);
      Assert.AreEqual(c[3], a[3] / scal);

      Assert.AreEqual(d[0], a[0] / scal);
      Assert.AreEqual(d[1], a[1] / scal);
      Assert.AreEqual(d[2], a[2] / scal);
      Assert.AreEqual(d[3], a[3] / scal);
    }

    //Test Clone
    [Test]
    public void Clone()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      DoubleVector b = a.Clone();

      Assert.AreEqual(a[0], b[0]);
      Assert.AreEqual(a[1], b[1]);
      Assert.AreEqual(a[2], b[2]);
      Assert.AreEqual(a[3], b[3]);

      a = a * 2;

      Assert.AreEqual(a[0], b[0] * 2);
      Assert.AreEqual(a[1], b[1] * 2);
      Assert.AreEqual(a[2], b[2] * 2);
      Assert.AreEqual(a[3], b[3] * 2);
    }

    //Test IEnumerable and DoubleVectorEnumerator
    [Test]
    public void GetEnumerator()
    {
      var a = new DoubleVector(new double[4] { 0, 1, 2, 3 });
      IEnumerator dve = a.GetEnumerator();
      double b;
      bool c;

      c = dve.MoveNext();
      b = (double)dve.Current;
      Assert.AreEqual(c, true);
      Assert.AreEqual(b, 0);

      c = dve.MoveNext();
      b = (double)dve.Current;
      Assert.AreEqual(c, true);
      Assert.AreEqual(b, 1);

      c = dve.MoveNext();
      b = (double)dve.Current;
      Assert.AreEqual(c, true);
      Assert.AreEqual(b, 2);

      c = dve.MoveNext();
      b = (double)dve.Current;
      Assert.AreEqual(c, true);
      Assert.AreEqual(b, 3);

      c = dve.MoveNext();
      Assert.AreEqual(c, false);
    }
  }
}
