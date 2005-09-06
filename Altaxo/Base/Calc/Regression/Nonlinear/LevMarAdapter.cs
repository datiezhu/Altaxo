using System;
using System.Text;
using Altaxo.Collections;

namespace Altaxo.Calc.Regression.Nonlinear
{
  /// <summary>
  /// Adapts a <see>FitEnsemble</see> to the requirements of a Levenberg-Marquardt fitting procedure.
  /// </summary>
  public class LevMarAdapter
  {
    FitEnsemble _fitEnsemble;

    /// <summary>
    /// List of constant parameters (i.e. parameters that are not changed during the fitting session).
    /// For convinience, all parameters are stored here (the varying parameters too), but only the constant parameters are used.
    /// </summary>
    double[] _constantParameters;

    class CachedFitElementInfo
    {
      /// <summary>Parameter array for temporary purpose.</summary>
      public double[] Parameters;
      /// <summary>Value result array for temporary purpose.</summary>
      public double[] Ys;
      /// <summary>Independent variable array for temporary purpose.</summary>
      public double[] Xs;

      /// <summary>Parameter mapping from the global parameter list to the local parameter list. Positive entries
      /// give the position in the variable parameter list, negative entries gives the position -entry-1 in the 
      /// constant parameter list.
      /// </summary>
      
      public int[] ParameterMapping;

      /// <summary>Designates which dependent variable columns are really in use.</summary>
      public int[] DependentVariablesInUse;

      /// <summary>
      /// Information, which of the rows are valid, i.e. all independent columns contains values, and all used dependent columns contain values in those rows.
      /// </summary>
      public IAscendingIntegerCollection ValidRows;
    }

    CachedFitElementInfo[] _cachedFitElementInfo;


    /// <summary>Number of total valid data points (y-values) of the fit ensemble.</summary>
    int _cachedNumberOfData;
  

    public LevMarAdapter(FitEnsemble ensemble, ParameterSet paraSet)
    {
      _fitEnsemble = ensemble;

      CalculateCachedData(paraSet);
    }

    void CalculateCachedData(ParameterSet paraSet)
    {
      // Preparation: Store the parameter names by name and index, and store
      // all parameter values in _constantParameters
      System.Collections.Hashtable paraNames = new System.Collections.Hashtable();
      _constantParameters = new double[paraSet.Count];
      for (int i = 0; i < paraSet.Count; ++i)
      {
        paraNames.Add(paraSet[i].Name, i);
        _constantParameters[i] = paraSet[i].Parameter;
      }

      _cachedNumberOfData = 0;
      _cachedFitElementInfo = new CachedFitElementInfo[_fitEnsemble.Count];
      for (int i = 0; i < _fitEnsemble.Count; i++)
      {
        CachedFitElementInfo info = new CachedFitElementInfo();
        _cachedFitElementInfo[i] = info;
        FitElement fitEle = _fitEnsemble[i];

        info.ValidRows = fitEle.CalculateValidNumericRows();

        info.Xs = new double[fitEle.NumberOfIndependentVariables];
        info.Parameters = new double[fitEle.NumberOfParameters];
        info.Ys = new double[fitEle.NumberOfDependentVariables];
        
        // Calculate the number of used variables
        int numVariablesUsed=0;
        for(int j=0;j<fitEle.NumberOfDependentVariables;++j)
        {
          if(fitEle.DependentVariables(j)!=null)
            ++numVariablesUsed;
        }
        info.DependentVariablesInUse = new int[numVariablesUsed];
        for (int j = 0, used=0; j < fitEle.NumberOfDependentVariables; ++j)
        {
          if (fitEle.DependentVariables(j) != null)
            info.DependentVariablesInUse[used++] = j;
        }

        // calculate the total number of data points
        _cachedNumberOfData += numVariablesUsed * info.ValidRows.Count;


        // now create the parameter mapping
        info.ParameterMapping = new int[fitEle.NumberOfParameters];

        for (int j = 0; i < info.ParameterMapping.Length; ++j)
        {
          if(!paraNames.Contains(fitEle.ParameterName(j)))
            throw new ArgumentException(string.Format("ParameterSet does not contain parameter {0}, which is used by function[{1}]",fitEle.ParameterName(j),i));

          int idx = (int)paraNames[fitEle.ParameterName(j)];
          if (paraSet[idx].Vary)
          {
            info.ParameterMapping[j] = idx;
          }
          else
          {
            info.ParameterMapping[j] = -idx - 1;
          }
        }
      }
    }

    /// <summary>Number of total valid data points (y-values) of the fit ensemble. This is the array
    /// size you will need to store the fitting functions output.</summary>
    public int NumberOfData
    {
      get
      {
        return _cachedNumberOfData;
      }
    }

    /// <summary>
    /// Stores the real data points ("measured data" or "dependent values") in an array. The data
    /// are stored from FitElement_0 to FitElement_n. For FitElements with more than one dependent
    /// variable in use, the data are stored interleaved.
    /// </summary>
    /// <param name="values"></param>
    public void GetDependentValues(double[] values)
    {
       int outputValuesPointer = 0;
       for (int ele = 0; ele < _cachedFitElementInfo.Length; ele++)
       {
         CachedFitElementInfo info = _cachedFitElementInfo[ele];
         FitElement fitEle = _fitEnsemble[ele];

         IAscendingIntegerCollection validRows = info.ValidRows;
         int numValidRows = validRows.Count;
         // Evaluate the function for all points
         for (int i = 0; i < numValidRows; ++i)
         {
           for (int j = 0; j < info.DependentVariablesInUse.Length; ++j)
           {
             values[outputValuesPointer++] = fitEle.DependentVariables(j)[validRows[i]];
           }
         }
       }
    }


    /// <summary>
    /// Calculates the fitting values.
    /// </summary>
    /// <param name="param">The parameter used to calculate the values.</param>
    /// <param name="outputValues">You must provide an array to hold the calculated values. Size of the array must be
    /// at least <see>NumberOfData</see>.</param>
    /// <remarks>The values of the fit elements are stored in the order from element_0 to element_n. If there is more
    /// than one used dependent variable per fit element, the output values are stored in interleaved order.
    /// </remarks>
    public void EvalulateFitValues(double[] parameter, double[] outputValues, object additionalData)
    {
      EvaluateFitValues(parameter, outputValues);
    }

    /// <summary>
    /// Calculates the fitting values.
    /// </summary>
    /// <param name="param">The parameter used to calculate the values.</param>
    /// <param name="outputValues">You must provide an array to hold the calculated values. Size of the array must be
    /// at least <see>NumberOfData</see>.</param>
    /// <remarks>The values of the fit elements are stored in the order from element_0 to element_n. If there is more
    /// than one used dependent variable per fit element, the output values are stored in interleaved order.
    /// </remarks>
    public void EvaluateFitValues(double [] parameter, double[] outputValues)
    {
      int outputValuesPointer = 0;
      for (int ele = 0; ele < _cachedFitElementInfo.Length; ele++)
      {
        CachedFitElementInfo info = _cachedFitElementInfo[ele];
        FitElement fitEle = _fitEnsemble[ele];
        
        // copy of the parameter to the temporary array
        for (int i = 0; i < info.Parameters.Length; i++)
        {
          int idx = info.ParameterMapping[i];
          info.Parameters[i] = idx>=0 ? parameter[idx] : _constantParameters[-1-idx];
        }

       
        IAscendingIntegerCollection validRows = info.ValidRows;
        int numValidRows = validRows.Count;
        // Evaluate the function for all points
        for (int i = 0; i < numValidRows; ++i)
        {
          for (int k = info.Xs.Length - 1; k >= 0; k--)
            info.Xs[k] = fitEle.IndependentVariables(k)[validRows[i]];

          fitEle.FitFunction.Evaluate(info.Xs, info.Parameters, info.Ys);

          // copy the evaluation result to the output array (interleaved)
          for (int k = 0; k < info.DependentVariablesInUse.Length; ++k)
            outputValues[outputValuesPointer++] = info.Ys[info.DependentVariablesInUse[k]];
        }
      }
    }
  }
}
