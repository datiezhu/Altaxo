#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2004 Dr. Dirk Lellinger
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
using NUnit.Framework;
using Altaxo.Calc.FFT;

namespace AltaxoTest.Calc.FFT
{

  [TestFixture]
  public class TestNativeFFT
  {
    const int nLowerLimit=5;
    const int nUpperLimit=100;
    const double maxTolerableEpsPerN=1E-15;

    

    private static void zzTestReOne_OnePosFwd(uint n)
    {
      double[] re = new double[n];
      double[] im = new double[n];

      re[1] = 1;

      NativeFourierMethods.FFT(re,im,re,im,(int)n,FourierDirection.Forward);

      for(uint i=0;i<n;i++)
      {
        Assertion.AssertEquals(string.Format("FFT({0}) of re 1 at pos 1 re[{1}]",n,i), Math.Cos((2*Math.PI*i)/n), re[i],n*maxTolerableEpsPerN);
        Assertion.AssertEquals(string.Format("FFT({0}) of re 1 at pos 1 im[{1}]",n,i), Math.Sin((2*Math.PI*i)/n), im[i],n*maxTolerableEpsPerN);
      }
    }


    [Test]
    public void TestReOne_OnePosFwd()
    {
      // Testing 2^n
      for(uint i=nLowerLimit;i<=nUpperLimit;i++)
        zzTestReOne_OnePosFwd(i);
    }


    private static void zzTestReOne_OnePosInv(uint n)
    {
      double[] re = new double[n];
      double[] im = new double[n];

      re[1] = 1;

      NativeFourierMethods.FFT(re,im,re,im,(int)n,FourierDirection.Inverse);

      for(uint i=0;i<n;i++)
      {
        Assertion.AssertEquals(string.Format("FFT({0}) of re 1 at pos 1 re[{1}]",n,i), Math.Cos((2*Math.PI*i)/n), re[i],n*maxTolerableEpsPerN);
        Assertion.AssertEquals(string.Format("FFT({0}) of re 1 at pos 1 im[{1}]",n,i), -Math.Sin((2*Math.PI*i)/n), im[i],n*maxTolerableEpsPerN);
      }
    }


    [Test]
    public void TestReOne_OnePosInv()
    {
      // Testing 2^n
      for(uint i=nLowerLimit;i<=nUpperLimit;i++)
        zzTestReOne_OnePosInv(i);
    }



    private static void zzTestReImOne_RandomPosInv(uint n)
    {
      double[] re = new double[n];
      double[] im = new double[n];
      
      System.Random rnd = new System.Random();

      int repos = rnd.Next((int)n);
      int impos = rnd.Next((int)n);

      re[repos]=1;
      im[impos]=1;

     

      NativeFourierMethods.FFT(re,im,re,im,(int)n, FourierDirection.Inverse);

      for(uint i=0;i<n;i++)
      {
        Assertion.AssertEquals(string.Format("FFT({0}) of im 1 at pos(re={1},im={2}) re[{3}]",n,repos,impos,i), 
          Math.Cos((2*Math.PI*i*(double)repos)/n) + Math.Sin((2*Math.PI*i*(double)impos)/n),
          re[i],n*1E-14);
        Assertion.AssertEquals(string.Format("FFT({0}) of im 1 at pos(re={1},im={2}) arb im[{3}]",n,repos,impos,i), 
          -Math.Sin((2*Math.PI*i*(double)repos)/n) + Math.Cos((2*Math.PI*i*(double)impos)/n), 
          im[i],n*1E-14);
      }
    }

    [Test]
    public void TestNativeReImOne_RandomPosInv()
    {
      // Testing 2^n
      for(uint i=nLowerLimit;i<=nUpperLimit;i++)
        zzTestReImOne_RandomPosInv(i);
    }

  }

}
