/*
 * CostFunction.cs
 * 
 * Copyright (c) 2004, dnAnalytics Project. All rights reserved.
 * NB: Problem class inspired by the optimization frame in the QuantLib library
*/

using System;
using Altaxo.Calc.LinearAlgebra;

namespace Altaxo.Calc.Optimization
{

	///<summary>Base class for cost function declaration</summary>
  /// <remarks>
  /// <para>Copyright (c) 2003-2004, dnAnalytics Project. All rights reserved. See <a>http://www.dnAnalytics.net</a> for details.</para>
  /// <para>Adopted to Altaxo (c) 2005 Dr. Dirk Lellinger.</para>
  /// </remarks>
  public abstract class CostFunction : ICostFunction 
  {

		///<summary>Method to override to compute the cost function value of x</summary>
		public abstract double Value( DoubleVector x);
			
		///<summary>Method to override to calculate the grad_f, the first derivative of 
		/// the cost function with respect to x</summary>
		public virtual DoubleVector Gradient(DoubleVector x)
		{
			double eps = 1e-8;
			double fp, fm;
			DoubleVector grad = new DoubleVector(x.Length,0.0);
			
			DoubleVector xx = new DoubleVector(x);
			for (int i=0; i<x.Length; i++) {
                xx[i] += eps;
                fp = this.Value(xx);
                xx[i] -= 2.0*eps;
                fm = this.Value(xx);
                grad[i] = 0.5*(fp - fm)/eps;
                xx[i] = x[i];
            }
			return grad;
		}
		
		///<summary>Method to override to calculate the Hessian of f, the second derivative of 
		/// the cost function with respect to x</summary>
		public virtual DoubleMatrix Hessian(DoubleVector x) {
			throw new OptimizationException("Hessian Evaluation not implemented");
		}
		
		///<summary>Access the constraints for the given cost function </summary>
		///<remarks>Defaults to no constraints</remarks>
		public virtual ConstraintDefinition Constraint {
			get { return constraint_; }
			set { this.constraint_ = value; }
		}
		
		protected ConstraintDefinition constraint_ = new NoConstraint();
		
	}
}
