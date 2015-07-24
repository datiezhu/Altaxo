﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Calc.LinearAlgebra
{
	/// <summary>
	///
	/// </summary>
	public static class TikhonovRegularizedNonnegativeMatrixFactorization
	{
		/// <summary>
		/// Factorize a nonnegative matrix A into two nonnegative matrices B and C so that A is nearly equal to B*C.
		/// Tikhonovs the nm f3.
		/// </summary>
		/// <param name="A">Matrix to factorize.</param>
		/// <param name="r">The number of factors.</param>
		/// <param name="B0">Original B matrix. Can be null.</param>
		/// <param name="C0">Original C matrix. Can be null.</param>
		/// <param name="oldalpha">The oldalpha.</param>
		/// <param name="oldbeta">The oldbeta.</param>
		/// <param name="gammaB">The gamma b.</param>
		/// <param name="gammaC">The gamma c.</param>
		/// <param name="maxiter">The maxiter.</param>
		/// <param name="tol">The tol.</param>
		public static void TikhonovNMF3(
					IMatrix A,
					int r,
					IMatrix B0,
					IMatrix C0,
					IVector oldalpha,
		IVector oldbeta,
		IMatrix gammaB,
		IMatrix gammaC,
		int maxiter,
		double tol)
		{
			// The converged version of the algorithm
			// Use complementary slackness as stopping criterion
			// format long;
			// Check the input matrix

			if (null == A)
				throw new ArgumentNullException(nameof(A));

			if (MatrixMath.Min(A) < 0)
				throw new ArgumentException("Input matrix must not contain negative elements", nameof(A));

			int m = A.Rows;
			int n = A.Columns;
			// Check input arguments

			//if ˜exist(’r’)

			if (null == B0)
			{
				B0 = DoubleMatrix.Random(m, r);
			}

			if (null == C0)
			{
				C0 = DoubleMatrix.Random(r, n);
			}

			if (null == oldalpha)
			{
				oldalpha = new DoubleVector(n);
			}

			if (null == oldbeta)
			{
				oldbeta = new DoubleVector(m);
			}

			if (null == gammaB)
			{
				gammaB = new DoubleMatrix(m, 1);
				gammaB.SetMatrixElements(0.1);    // small values lead to better convergence property
			}

			if (null == gammaC)
			{
				gammaC = new DoubleMatrix(n, 1);
				gammaC.SetMatrixElements(0.1); // small values lead	to better convergence property
			}

			if (0 == maxiter)
				maxiter = 1000;

			if (double.IsNaN(tol) || tol <= 0)
				tol = 1.0e-9;

			var B = B0; B0 = null;
			var C = C0; C0 = null;
			var newalpha = oldalpha;
			var newbeta = oldbeta;

			var AtA = new DoubleMatrix(n, n);
			MatrixMath.MultiplyFirstTransposed(A, A, AtA);
			double trAtA = MatrixMath.Trace(AtA);

			var olderror = new DoubleVector(maxiter + 1);

			var BtA = new DoubleMatrix(r, n);
			MatrixMath.MultiplyFirstTransposed(B, A, BtA);

			var CtBtA = new DoubleMatrix(n, n);
			MatrixMath.MultiplyFirstTransposed(C, BtA, CtBtA);

			var BtB = new DoubleMatrix(r, r);
			MatrixMath.MultiplyFirstTransposed(B, B, BtB);

			var BtBC = new DoubleMatrix(r, n);
			MatrixMath.Multiply(BtB, C, BtBC);
			var CtBtBC = new DoubleMatrix(n, n);
			MatrixMath.MultiplyFirstTransposed(C, BtBC, CtBtBC);

			var BtDgNewbeta = new DoubleMatrix(r, m);
			MatrixMath.MultiplyFirstTransposed(B, DoubleMatrix.Diag(newbeta), BtDgNewbeta);
			var BtDgNewbetaB = new DoubleMatrix(r, r); // really rxr ?
			MatrixMath.Multiply(BtDgNewbeta, B, BtDgNewbetaB);

			var CDgNewalpha = new DoubleMatrix(r, n);
			MatrixMath.Multiply(C, DoubleMatrix.Diag(newalpha), CDgNewalpha);
			var CtCDgNewalpha = new DoubleMatrix(n, n);
			MatrixMath.MultiplyFirstTransposed(C, CDgNewalpha, CtCDgNewalpha);

			olderror[0] =
				0.5 * trAtA -
				MatrixMath.Trace(CtBtA) +
				0.5 * MatrixMath.Trace(CtBtBC) +
				0.5 * MatrixMath.Trace(BtDgNewbetaB) +
				0.5 * MatrixMath.Trace(CtCDgNewalpha);

			double sigma = 1.0e-9;
			double delta = sigma;

			for (int iteration = 1; iteration <= maxiter; ++iteration)
			{
				var CCt = new DoubleMatrix(r, r);
				MatrixMath.MultiplySecondTransposed(C, C, CCt);

				var gradB = new DoubleMatrix(m, r);
				var tempMR = new DoubleMatrix(m, r);
				//gradB = B*CCt - A*C’ +diag(newbeta)*B;
				MatrixMath.Multiply(B, CCt, gradB);
				MatrixMath.MultiplySecondTransposed(A, C, tempMR);
				MatrixMath.Add(gradB, tempMR, gradB);
				MatrixMath.Multiply(DoubleMatrix.Diag(newbeta), B, tempMR);
				MatrixMath.Add(gradB, tempMR, gradB);

				// Bm = max(B, (gradB < 0)	*	sigma);
				var sigMR = new DoubleMatrix(m, r);
				sigMR.SetMatrixElements((i, j) => gradB[i, j] < 0 ? sigma : 0);
				var Bm = new DoubleMatrix(m, r);
				Bm.SetMatrixElements((i, j) => Math.Max(B[i, j], sigMR[i, j]));
			}
		}
	}
}