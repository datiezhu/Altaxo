using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Calc.LinearAlgebra
{
  /// <summary>
  /// 
  /// </summary>
  /// <remarks>This class was translated to C# from the JAMA1.0.2 package.</remarks>
  public class QRDecomposition
  {
    /* ------------------------
   Class variables
 * ------------------------ */

    /** Array for internal storage of decomposition.
    @serial internal array storage.
    */
    private double[][] QR;

    /** Row and column dimensions.
    @serial column dimension.
    @serial row dimension.
    */
    private int m, n;

    /** Array for internal storage of diagonal of R.
    @serial diagonal of R.
    */
    private double[] Rdiag;

    private JaggedArrayMatrix _solveMatrixWorkspace;
    private double[] _solveVectorWorkspace;

    /* ------------------------
       Constructor
     * ------------------------ */

    /** QR Decomposition, computed by Householder reflections.
    @param A    Rectangular matrix
    @return     Structure to access R and the Householder vectors and compute Q.
    */

    public QRDecomposition(IROMatrix A)
    {
      Decompose(A);
    }

    public QRDecomposition()
    {
    }

    public void Decompose(IROMatrix A)
    {
      // Initialize.
      if (m == A.Rows && n == A.Columns)
      {
        MatrixMath.Copy(A, new JaggedArrayMatrix(QR, m, n));
        //JaggedArrayMath.Copy(A, QR);
      }
      else
      {
        QR = JaggedArrayMath.GetMatrixCopy(A);
        m = A.Rows;
        n = A.Columns;
        Rdiag = new double[n];
      }

      // Main loop.
      for (int k = 0; k < n; k++)
      {
        // Compute 2-norm of k-th column without under/overflow.
        double nrm = 0;
        for (int i = k; i < m; i++)
        {
          nrm = RMath.Hypot(nrm, QR[i][k]);
        }

        if (nrm != 0.0)
        {
          // Form k-th Householder vector.
          if (QR[k][k] < 0)
          {
            nrm = -nrm;
          }
          for (int i = k; i < m; i++)
          {
            QR[i][k] /= nrm;
          }
          QR[k][k] += 1.0;

          // Apply transformation to remaining columns.
          for (int j = k + 1; j < n; j++)
          {
            double s = 0.0;
            for (int i = k; i < m; i++)
            {
              s += QR[i][k] * QR[i][j];
            }
            s = -s / QR[k][k];
            for (int i = k; i < m; i++)
            {
              QR[i][j] += s * QR[i][k];
            }
          }
        }
        Rdiag[k] = -nrm;
      }
    }

    /* ------------------------
       Public Methods
     * ------------------------ */

    /** Is the matrix full rank?
    @return     true if R, and hence A, has full rank.
    */

    public bool IsFullRank()
    {
      for (int j = 0; j < n; j++)
      {
        if (Rdiag[j] == 0)
          return false;
      }
      return true;
    }

    /** Return the Householder vectors
    @return     Lower trapezoidal matrix whose columns define the reflections
    */

    public JaggedArrayMatrix GetH()
    {
      JaggedArrayMatrix X = new JaggedArrayMatrix(m, n);
      double[][] H = X.Array;
      for (int i = 0; i < m; i++)
      {
        for (int j = 0; j < n; j++)
        {
          if (i >= j)
          {
            H[i][j] = QR[i][j];
          }
          else
          {
            H[i][j] = 0.0;
          }
        }
      }
      return X;
    }

    /** Return the upper triangular factor
    @return     R
    */

    public JaggedArrayMatrix GetR()
    {
      JaggedArrayMatrix X = new JaggedArrayMatrix(n, n);
      double[][] R = X.Array;
      for (int i = 0; i < n; i++)
      {
        for (int j = 0; j < n; j++)
        {
          if (i < j)
          {
            R[i][j] = QR[i][j];
          }
          else if (i == j)
          {
            R[i][j] = Rdiag[i];
          }
          else
          {
            R[i][j] = 0.0;
          }
        }
      }
      return X;
    }

    /** Generate and return the (economy-sized) orthogonal factor
    @return     Q
    */

    public JaggedArrayMatrix GetQ()
    {
      JaggedArrayMatrix X = new JaggedArrayMatrix(m, n);
      double[][] Q = X.Array;
      for (int k = n - 1; k >= 0; k--)
      {
        for (int i = 0; i < m; i++)
        {
          Q[i][k] = 0.0;
        }
        Q[k][k] = 1.0;
        for (int j = k; j < n; j++)
        {
          if (QR[k][k] != 0)
          {
            double s = 0.0;
            for (int i = k; i < m; i++)
            {
              s += QR[i][k] * Q[i][j];
            }
            s = -s / QR[k][k];
            for (int i = k; i < m; i++)
            {
              Q[i][j] += s * QR[i][k];
            }
          }
        }
      }
      return X;
    }

    public IMatrix GetSolution(IROMatrix B)
    {
      JaggedArrayMatrix result = new JaggedArrayMatrix(m, B.Columns);
      Solve(B, result);
      return result;
    }
    public IMatrix GetSolution(IROMatrix A, IROMatrix B)
    {
      Decompose(A);
      JaggedArrayMatrix result = new JaggedArrayMatrix(m, B.Columns);
      Solve(B, result);
      return result;
    }

    public DoubleVector GetSolution(IROVector B)
    {
      DoubleVector result = new DoubleVector(m);
      Solve(B, result);
      return result;
    }
    public DoubleVector GetSolution(IROMatrix A, IROVector B)
    {
      Decompose(A);
      DoubleVector result = new DoubleVector(m);
      Solve(B, result);
      return result;
    }

    public void Solve(IROMatrix A, IROMatrix B, IMatrix Result)
    {
      Decompose(A);
      Solve(B, Result);
    }
    public void Solve(IROMatrix A, IROVector B, IVector Result)
    {
      Decompose(A);
      Solve(B, Result);
    }

    /** Least squares solution of A*X = B
    @param B    A Matrix with as many rows as A and any number of columns.
    @return     X that minimizes the two norm of Q*R*X-B.
    @exception  IllegalArgumentException  Matrix row dimensions must agree.
    @exception  RuntimeException  Matrix is rank deficient.
    */

    public void Solve(IROMatrix B, IMatrix result)
    {
      if (B.Rows != m)
      {
        throw new ArgumentException("Matrix row dimensions must agree.");
      }
      if (!this.IsFullRank())
      {
        throw new Exception("Matrix is rank deficient.");
      }

      // Copy right hand side
      int nx = B.Columns;
      double[][] X;
      if (_solveMatrixWorkspace != null && _solveMatrixWorkspace.Rows == B.Rows && _solveMatrixWorkspace.Columns == B.Columns)
      {
        X = _solveMatrixWorkspace.Array;
        MatrixMath.Copy(B, _solveMatrixWorkspace);
      }
      else
      {
        X = JaggedArrayMath.GetMatrixCopy(B);
        _solveMatrixWorkspace = new JaggedArrayMatrix(X, B.Rows, B.Columns);
      }

      // Compute Y = transpose(Q)*B
      for (int k = 0; k < n; k++)
      {
        for (int j = 0; j < nx; j++)
        {
          double s = 0.0;
          for (int i = k; i < m; i++)
          {
            s += QR[i][k] * X[i][j];
          }
          s = -s / QR[k][k];
          for (int i = k; i < m; i++)
          {
            X[i][j] += s * QR[i][k];
          }
        }
      }
      // Solve R*X = Y;
      for (int k = n - 1; k >= 0; k--)
      {
        for (int j = 0; j < nx; j++)
        {
          X[k][j] /= Rdiag[k];
        }
        for (int i = 0; i < k; i++)
        {
          for (int j = 0; j < nx; j++)
          {
            X[i][j] -= X[k][j] * QR[i][k];
          }
        }
      }

      MatrixMath.Submatrix(_solveMatrixWorkspace, result, 0, 0);
    }

    /** Least squares solution of A*X = B
   @param B    A Matrix with as many rows as A and any number of columns.
   @return     X that minimizes the two norm of Q*R*X-B.
   @exception  IllegalArgumentException  Matrix row dimensions must agree.
   @exception  RuntimeException  Matrix is rank deficient.
   */

    public void Solve(IROVector B, IVector result)
    {
      if (B.Length != m)
      {
        throw new ArgumentException("Matrix row dimensions must agree.");
      }
      if (!this.IsFullRank())
      {
        throw new Exception("Matrix is rank deficient.");
      }

      // Copy right hand side
      double[] X;
      if (_solveVectorWorkspace != null && _solveVectorWorkspace.Length == B.Length)
      {
        X = _solveVectorWorkspace;
      }
      else
      {
        _solveVectorWorkspace = X = new double[B.Length];
      }
      for (int i = 0; i < X.Length; i++)
        X[i] = B[i]; // copy to workspace vector

      // Compute Y = transpose(Q)*B
      for (int k = 0; k < n; k++)
      {
        double s = 0.0;
        for (int i = k; i < m; i++)
        {
          s += QR[i][k] * X[i];
        }
        s = -s / QR[k][k];
        for (int i = k; i < m; i++)
        {
          X[i] += s * QR[i][k];
        }
      }
      // Solve R*X = Y;
      for (int k = n - 1; k >= 0; k--)
      {
        X[k] /= Rdiag[k];
        for (int i = 0; i < k; i++)
        {
            X[i] -= X[k] * QR[i][k];
        }
      }

      for (int i = 0; i < result.Length; i++)
        result[i] = X[i];
    }

  }

 
}