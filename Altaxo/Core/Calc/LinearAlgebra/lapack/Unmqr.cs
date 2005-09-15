/*
 * Unmqr.cs
 * 
 * Copyright (c) 2003-2004, dnAnalytics. All rights reserved.
*/

#if !MANAGED
using System;
using System.Runtime.InteropServices;



namespace Altaxo.Calc.LinearAlgebra.Lapack{
	[System.Security.SuppressUnmanagedCodeSecurityAttribute]
	internal sealed class Unmqr {
		private  Unmqr() {}                           
		private static void ArgumentCheck(Side side, int m, int n, int k, Object A, int lda, Object tau, Object C, int ldc) {
			if ( A == null ) {
				throw new ArgumentNullException("A","A cannot be null.");
			}
			if ( tau == null ) {
				throw new ArgumentNullException("tau","tau cannot be null.");
			}
			if ( C == null ) {
				throw new ArgumentNullException("C","C cannot be null.");
			}
			if ( m<0 ) {
				throw new ArgumentException("m must be at least zero.", "m");
			}
			if ( n<0 ) {
				throw new ArgumentException("n must be at least zero.", "n");
			}
			if( side == Side.Left ){
				if( k < 0 || k > m ){
					throw new ArgumentException("k must be positive and less than or equal to m.", "k");
				}
			}else{
				if( k < 0 || k > n ){
					throw new ArgumentException("k must be positive and less than or equal to n.", "k");
				}
			}
			if( side == Side.Left ){
				if ( lda < System.Math.Max(1,m) ) {
					throw new ArgumentException("lda must be at least max(1,m)", "lda");
				}
			}else{
				if ( lda < System.Math.Max(1,n) ) {
					throw new ArgumentException("lda must be at least max(1,n)", "lda");
				}
			}
			if ( ldc < System.Math.Max(1,m) ) {
				throw new ArgumentException("ldc must be at least max(1,m)", "ldc");
			}			
		}

		internal static int Compute( Side side, Transpose trans, int m, int n, int k, ComplexFloat[] A, int lda, ComplexFloat[] tau, ComplexFloat[] C, int ldc  ){
			ArgumentCheck(side, m, n, k, A, lda, tau, C, ldc);
			return dna_lapack_cunmqr(Configuration.BlockSize, side, trans, m, n, k, A, lda, tau, C, ldc);
		}

		internal static int Compute( Side side, Transpose trans, int m, int n, int k, Complex[] A, int lda, Complex[] tau, Complex[] C, int ldc  ){
			ArgumentCheck(side, m, n, k, A, lda, tau, C, ldc);
			return dna_lapack_zunmqr(Configuration.BlockSize, side, trans, m, n, k, A, lda, tau, C, ldc);
		}

		[DllImport(Configuration.BLASLibrary, ExactSpelling=true, SetLastError=false)]
		private static extern int dna_lapack_cunmqr( int block_size, Side side, Transpose trans, int m, int n, int k, [In,Out]ComplexFloat[] A, int lda, [In,Out]ComplexFloat[] tau, [In,Out]ComplexFloat[] C, int ldc   );

		[DllImport(Configuration.BLASLibrary, ExactSpelling=true, SetLastError=false)]
		private static extern int dna_lapack_zunmqr( int block_size, Side side, Transpose trans, int m, int n, int k, [In,Out]Complex[] A, int lda, [In,Out]Complex[] tau, [In,Out]Complex[] C, int ldc   );
	}
}
#endif