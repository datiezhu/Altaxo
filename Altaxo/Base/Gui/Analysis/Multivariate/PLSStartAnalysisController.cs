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
using Altaxo.Calc.Regression.Multivariate;

namespace Altaxo.Gui.Worksheet
{
  #region Interfaces

  public interface IPLSStartAnalysisView
  {
    void InitializeNumberOfFactors(int numFactors);

    void InitializeAnalysisMethod(string[] methods, int actMethod);

    void InitializeCrossPressCalculation(CrossPRESSCalculationType val);

    event Action<int> MaxNumberOfFactorsChanged;

    event Action<CrossPRESSCalculationType> CrossValidationSelected;

    event Action<int> AnalysisMethodChanged;
  }

  #endregion Interfaces

  /// <summary>
  /// Summary description for PLSStartAnalysisController.
  /// </summary>
  [ExpectedTypeOfView(typeof(IPLSStartAnalysisView))]
  public class PLSStartAnalysisController : IMVCAController
  {
    private MultivariateAnalysisOptions _doc;
    private IPLSStartAnalysisView _view;

    private System.Collections.ArrayList _methoddictionary = new System.Collections.ArrayList();

    public PLSStartAnalysisController(MultivariateAnalysisOptions options)
    {
      _doc = options;
    }

    private void SetElements(bool bInit)
    {
      if (null != _view)
      {
        _view.InitializeNumberOfFactors(_doc.MaxNumberOfFactors);
        _view.InitializeCrossPressCalculation(_doc.CrossPRESSCalculation);
        InitializeAnalysisMethods();
      }
    }

    public IPLSStartAnalysisView View
    {
      get { return _view; }
      set
      {
        if (null != _view)
        {
          _view.AnalysisMethodChanged -= EhView_AnalysisMethodChanged;
          _view.CrossValidationSelected -= EhView_CrossValidationSelected;
          _view.MaxNumberOfFactorsChanged -= EhView_MaxNumberOfFactorsChanged;
        }

        _view = value;

        if (null != _view)
        {
          SetElements(false); // set only the view elements, dont't initialize the variables

          _view.AnalysisMethodChanged += EhView_AnalysisMethodChanged;
          _view.CrossValidationSelected += EhView_CrossValidationSelected;
          _view.MaxNumberOfFactorsChanged += EhView_MaxNumberOfFactorsChanged;
        }
      }
    }

    public MultivariateAnalysisOptions Doc
    {
      get { return _doc; }
    }

    public void EhView_MaxNumberOfFactorsChanged(int numFactors)
    {
      _doc.MaxNumberOfFactors = numFactors;
    }

    public void EhView_CrossValidationSelected(CrossPRESSCalculationType val)
    {
      _doc.CrossPRESSCalculation = val;
    }

    private static bool ReferencesOwnAssembly(System.Reflection.AssemblyName[] references)
    {
      string myassembly = System.Reflection.Assembly.GetCallingAssembly().GetName().FullName;

      foreach (System.Reflection.AssemblyName assname in references)
        if (assname.FullName == myassembly)
          return true;
      return false;
    }

    private static bool IsOwnAssembly(System.Reflection.Assembly ass)
    {
      return ass.FullName == System.Reflection.Assembly.GetCallingAssembly().FullName;
    }

    public void InitializeAnalysisMethods()
    {
      _methoddictionary.Clear();
      var nameList = new System.Collections.ArrayList();

      System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
      foreach (System.Reflection.Assembly assembly in assemblies)
      {
        if (IsOwnAssembly(assembly) || ReferencesOwnAssembly(assembly.GetReferencedAssemblies()))
        {
          Type[] definedtypes = assembly.GetTypes();
          foreach (Type definedtype in definedtypes)
          {
            if (definedtype.IsSubclassOf(typeof(Altaxo.Calc.Regression.Multivariate.WorksheetAnalysis)) && !definedtype.IsAbstract)
            {
              Attribute[] descriptionattributes = Attribute.GetCustomAttributes(definedtype, typeof(System.ComponentModel.DescriptionAttribute));

              string name =
                (descriptionattributes.Length > 0) ?
                ((System.ComponentModel.DescriptionAttribute)descriptionattributes[0]).Description : definedtype.ToString();

              _methoddictionary.Add(definedtype);
              nameList.Add(name);
            }
          }
        } // end foreach type
      } // end foreach assembly
      if (_view != null)
        _view.InitializeAnalysisMethod((string[])nameList.ToArray(typeof(string)), 0);
      _doc.AnalysisMethod = (System.Type)_methoddictionary[0];
    }

    public void EhView_AnalysisMethodChanged(int item)
    {
      _doc.AnalysisMethod = (System.Type)_methoddictionary[item];
    }

    #region IApplyController Members

    public bool Apply(bool disposeController)
    {
      // nothing to do here, since the hosted doc is a struct
      return true;
    }

    /// <summary>
    /// Try to revert changes to the model, i.e. restores the original state of the model.
    /// </summary>
    /// <param name="disposeController">If set to <c>true</c>, the controller should release all temporary resources, since the controller is not needed anymore.</param>
    /// <returns>
    ///   <c>True</c> if the revert operation was successfull; <c>false</c> if the revert operation was not possible (i.e. because the controller has not stored the original state of the model).
    /// </returns>
    public bool Revert(bool disposeController)
    {
      return false;
    }

    #endregion IApplyController Members

    #region IMVCController Members

    public object ViewObject
    {
      get
      {
        return _view;
      }
      set
      {
        View = value as IPLSStartAnalysisView;
      }
    }

    public object ModelObject
    {
      get { return _doc; }
    }

    public void Dispose()
    {
    }

    #endregion IMVCController Members
  }
}
