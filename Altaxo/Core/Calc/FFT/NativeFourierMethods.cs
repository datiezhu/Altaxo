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

namespace Altaxo.Calc.FFT
{

  /// <summary>
  /// This class provides reference methods concerning FFT that are slow and not very accurate.
  /// Do not use them except for comparism and testing purposes!
  /// The direction of the forward Fourier transform will be defined here as reference for all other Fourier methods as
  /// multiplication of f(x)*exp(iwt), i.e. as (wt) having a positive sign (forward) and wt having a negative sign (backward).
  /// </summary>
  public class NativeFourierMethods
  {
    

    /// <summary>
    /// Performes a cyclic correlation between array arr1 and arr2 and stores the result in resultarr. Resultarr must be
    /// different from the other two arrays. 
    /// </summary>
    /// <param name="resultarr">The array that stores the correleation result.</param>
    /// <param name="arr1">First array.</param>
    /// <param name="arr2">Second array.</param>
    /// <param name="count">Number of points to correlate.</param>
    public static  void CyclicCorrelation(double[] resultarr, double[] arr1, double[] arr2, int count)
    {
      if(object.ReferenceEquals(resultarr,arr1) || object.ReferenceEquals(resultarr,arr2))
        throw new ArgumentException("Resultarr must not be identical to arr1 or arr2!");

      int i,k;
      for(k=0;k<count;k++)
      {
        resultarr[k]=0;
        for(i=0;i<count;i++)
        {
          resultarr[k] += arr1[i]*arr2[(i+k)%count];
        }
      }
    }


    /// <summary>
    /// Performes a cyclic convolution between array arr1 and arr2 and stores the result in resultarr. Resultarr must be
    /// different from the other two arrays. 
    /// </summary>
    /// <param name="resultarr">The array that stores the correleation result.</param>
    /// <param name="arr1">First array.</param>
    /// <param name="arr2">Second array.</param>
    /// <param name="count">Number of points to correlate.</param>
    public static void CyclicConvolution(double[] resultarr, double[] arr1, double[] arr2, int count)
    {
      if(object.ReferenceEquals(resultarr,arr1) || object.ReferenceEquals(resultarr,arr2))
        throw new ArgumentException("resultarr must not be identical to arr1 or arr2!");
      
      int i,k;
      for(k=0;k<count;k++)
      {
        resultarr[k]=0;
        for(i=0;i<count;i++)
        {
          resultarr[k] += arr1[i]*arr2[(count+k-i)%count];
        }
      }
    }

    /// <summary>
    /// Performs a native fouriertransformation of a real value array.
    /// </summary>
    /// <param name="resultarr">Used to store the result of the transformation.</param>
    /// <param name="arr">The double valued array to transform.</param>
    /// <param name="count">Number of points to transform.</param>
    /// <param name="direction">Direction of the Fourier transform.</param>
    public static void FFT(Complex[] resultarr, double[] arr, int count, FourierDirection direction)
    {
      int iss = direction==FourierDirection.Forward ? 1 : -1;
      for(int k=0;k<count;k++)
      {
        resultarr[k]=new Complex(0,0);
        for(int i=0;i<count;i++)
        {
          double phi = iss*2*Math.PI*((i*k)%count)/count;
          resultarr[k] += new Complex(arr[i]*Math.Cos(phi),arr[i]*Math.Sin(phi));
        }
      }
    }

    /// <summary>
    /// Performs a inline native fouriertransformation of real and imaginary part arrays.
    /// </summary>
    /// <param name="real">The real part of the array to transform.</param>
    /// <param name="imag">The real part of the array to transform.</param>
    /// <param name="direction">Direction of the Fourier transform.</param>
    public static void FFT(double[] real, double[] imag, FourierDirection direction)
    {
      if(real.Length!=imag.Length)
        throw new ArgumentException("Length of real and imaginary array do not match!");

      FFT(real, imag, real, imag, real.Length, direction);
    }

    /// <summary>
    /// Performs a native fouriertransformation of a complex value array.
    /// </summary>
    /// <param name="resultreal">Used to store the real part of the result of the transformation. May be equal to the input array.</param>
    /// <param name="resultimag">Used to store the imaginary part of the result of the transformation.  May be equal to the input array.</param>
    /// <param name="inputreal">The real part of the array to transform.</param>
    /// <param name="inputimag">The real part of the array to transform.</param>
    /// <param name="count">Number of points to transform.</param>
    /// <param name="direction">Direction of the Fourier transform.</param>
    public static void FFT(double[] resultreal, double[] resultimag, double[] inputreal, double[] inputimag, int count,FourierDirection direction)
    {
      bool useShadowCopy = false;
      double[] resre = resultreal;
      double[] resim = resultimag;

      if( object.ReferenceEquals(resultreal,inputreal) || 
        object.ReferenceEquals(resultreal,inputimag) ||
        object.ReferenceEquals(resultimag,inputreal) ||
        object.ReferenceEquals(resultimag,inputimag)
        )
        useShadowCopy = true;

      if(useShadowCopy)
      {
        resre = new double[count];
        resim = new double[count];
      }

      int iss = direction==FourierDirection.Forward ? 1 : -1;
      for(int k=0;k<count;k++)
      {
        double sumreal=0, sumimag=0;
        for(int i=0;i<count;i++)
        {
          double phi = iss*2*Math.PI*((i*k)%count)/count;
          double  vre = Math.Cos(phi);
          double vim = Math.Sin(phi);
          double addre = inputreal[i]*vre - inputimag[i]*vim;
          double addim = inputreal[i]*vim + inputimag[i]*vre;
          sumreal += addre;
          sumimag += addim;
        }
        resre[k] = sumreal;
        resim[k] = sumimag;
      }

      if(useShadowCopy)
      {
        Array.Copy(resre,0,resultreal,0,count);
        Array.Copy(resim,0,resultimag,0,count);
      }
    }
  }

}
