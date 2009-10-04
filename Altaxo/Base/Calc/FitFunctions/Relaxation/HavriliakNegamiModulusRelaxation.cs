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
using System.ComponentModel;
using Altaxo.Calc.Regression.Nonlinear;

namespace Altaxo.Calc.FitFunctions.Relaxation
{
  /// <summary>
  /// Havriliak-Negami function to fit dielectric spectra.
  /// </summary>
  [FitFunctionClass]
  public class HavriliakNegamiModulusRelaxation : IFitFunction
  {
    bool _useFrequencyInsteadOmega;
    bool _useFlowTerm;
    bool _logarithmizeResults;

    #region Serialization

  
 
    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(HavriliakNegamiModulusRelaxation), 0)]
    class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        HavriliakNegamiModulusRelaxation s = (HavriliakNegamiModulusRelaxation)obj;
        info.AddValue("UseFrequency", s._useFrequencyInsteadOmega);
        info.AddValue("FlowTerm", s._useFlowTerm);
        info.AddValue("LogarithmizeResults", s._logarithmizeResults);
      }

      public virtual object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        HavriliakNegamiModulusRelaxation s = o != null ? (HavriliakNegamiModulusRelaxation)o : new HavriliakNegamiModulusRelaxation();
        s._useFrequencyInsteadOmega = info.GetBoolean("UseFrequency");
        s._useFlowTerm = info.GetBoolean("FlowTerm");
        s._logarithmizeResults = info.GetBoolean("LogarithmizeResults");

        return s;
      }
    }

    #endregion

    public HavriliakNegamiModulusRelaxation()
    {

    }

    public override string ToString()
    {
      return "HavriliakNegami Complex " + (_useFrequencyInsteadOmega ? "(Freq)" : "(Omeg)");
    }



    [FitFunctionCreator("HavriliakNegami Complex (Omega)", "Relaxation/Modulus", 1, 2, 5)]
    [Description("FitFunctions.Relaxation.ModulusRelaxation.Introduction;XML.MML.GenericRelaxationModulus;FitFunctions.Relaxation.HavriliakNegamiSusceptibility.Part2;XML.MML.HavriliakNegamiTimeDomain;FitFunctions.IndependentVariable.Omega;FitFunctions.Relaxation.ModulusRelaxation.Part3")]
    public static IFitFunction CreateModulusOfOmega()
    {
      HavriliakNegamiModulusRelaxation result = new HavriliakNegamiModulusRelaxation();
      result._useFrequencyInsteadOmega = false;
      result._useFlowTerm = true;

      return result;
    }

    [FitFunctionCreator("Lg10 HavriliakNegami Complex (Omega)", "Relaxation/Modulus", 1, 2, 5)]
    [Description("FitFunctions.Relaxation.ModulusRelaxation.Introduction;XML.MML.GenericRelaxationModulus;FitFunctions.Relaxation.HavriliakNegamiSusceptibility.Part2;XML.MML.HavriliakNegamiTimeDomain;FitFunctions.IndependentVariable.Omega;FitFunctions.Relaxation.ModulusRelaxation.Part3")]
    public static IFitFunction CreateLg10ModulusOfOmega()
    {
      HavriliakNegamiModulusRelaxation result = new HavriliakNegamiModulusRelaxation();
      result._useFrequencyInsteadOmega = false;
      result._useFlowTerm = true;
      result._logarithmizeResults = true;

      return result;
    }

    [FitFunctionCreator("HavriliakNegami Complex (Freq)", "Relaxation/Modulus", 1, 2, 5)]
    [Description("FitFunctions.Relaxation.ModulusRelaxation.Introduction;XML.MML.GenericRelaxationModulus;FitFunctions.Relaxation.HavriliakNegamiSusceptibility.Part2;XML.MML.HavriliakNegamiTimeDomain;FitFunctions.IndependentVariable.FrequencyAsOmega;FitFunctions.Relaxation.ModulusRelaxation.Part3")]
    public static IFitFunction CreateModulusOfFrequency()
    {
      HavriliakNegamiModulusRelaxation result = new HavriliakNegamiModulusRelaxation();
      result._useFrequencyInsteadOmega = true;
      result._useFlowTerm = true;

      return result;
    }


    [FitFunctionCreator("Lg10 HavriliakNegami Complex (Freq)", "Relaxation/Modulus", 1, 2, 5)]
    [Description("FitFunctions.Relaxation.ModulusRelaxation.Introduction;XML.MML.GenericRelaxationModulus;FitFunctions.Relaxation.HavriliakNegamiSusceptibility.Part2;XML.MML.HavriliakNegamiTimeDomain;FitFunctions.IndependentVariable.FrequencyAsOmega;FitFunctions.Relaxation.ModulusRelaxation.Part3")]
    public static IFitFunction CreateLg10ModulusOfFrequency()
    {
      HavriliakNegamiModulusRelaxation result = new HavriliakNegamiModulusRelaxation();
      result._useFrequencyInsteadOmega = true;
      result._useFlowTerm = true;
      result._logarithmizeResults = true;

      return result;
    }



    #region IFitFunction Members

    #region independent variable definition

    public int NumberOfIndependentVariables
    {
      get
      {
        return 1;
      }
    }
    public string IndependentVariableName(int i)
    {
      return this._useFrequencyInsteadOmega ? "Frequency" : "Omega";
    }
    #endregion

    #region dependent variable definition
    private string[] _dependentVariableNameS = new string[] { "chi'", "chi''" };
    public int NumberOfDependentVariables
    {
      get
      {
        return _dependentVariableNameS.Length;
      }
    }
    public string DependentVariableName(int i)
    {
      return _dependentVariableNameS[i];
    }
    #endregion

    #region parameter definition
    string[] _parameterNameS = new string[] { "m_0", "m_inf", "tau_relax", "alpha", "gamma", "invviscosity" };
    public int NumberOfParameters
    {
      get
      {
        return this._useFlowTerm ? _parameterNameS.Length : _parameterNameS.Length - 1;
      }
    }
    public string ParameterName(int i)
    {
      return _parameterNameS[i];
    }

    public double DefaultParameterValue(int i)
    {
      switch (i)
      {
        case 0:
          return 1;
        case 1:
          return 1;
        case 2:
          return 1;
        case 3:
          return 1;
        case 4:
          return 1;
      }

      return 0;
    }

    public IVarianceScaling DefaultVarianceScaling(int i)
    {
      return null;
    }

    #endregion

    public void Evaluate(double[] X, double[] P, double[] Y)
    {
      double x = X[0];
      if (_useFrequencyInsteadOmega)
        x *= (2 * Math.PI);

      Complex result = 1 / ComplexMath.Pow(1 + ComplexMath.Pow(Complex.I * x * P[2], P[3]), P[4]);
      result = P[1] + (P[0] - P[1]) * result;

      if (this._useFlowTerm)
      {
        result = 1 / ((1 / result) - Complex.I * P[5] / x);
      }

      if (_logarithmizeResults)
      {
        Y[0] = Math.Log10(result.Re);
        Y[1] = Math.Log10(result.Im);
      }
      else
      {
        Y[0] = result.Re;
        Y[1] = result.Im;
      }
    }

  
    #endregion
  }

}