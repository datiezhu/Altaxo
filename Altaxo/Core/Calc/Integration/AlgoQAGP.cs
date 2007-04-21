using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Calc.Integration
{
  public class AlgoQagp
  {
    /* integration/qagp.c
 * 
 * Copyright (C) 1996, 1997, 1998, 1999, 2000 Brian Gough
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or (at
 * your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

    static bool test_positivity(double result, double resabs)
    {
      bool status = (Math.Abs(result) >= (1 - 50 * GSL_CONST.GSL_DBL_EPSILON) * resabs);
      return status;
    }
    static bool subinterval_too_small(double a1, double a2, double b2)
    {
      const double e = GSL_CONST.GSL_DBL_EPSILON;
      const double u = GSL_CONST.GSL_DBL_MIN;

      double tmp = (1 + 100 * e) * (Math.Abs(a2) + 1000 * u);

      bool status = Math.Abs(a1) <= tmp && Math.Abs(b2) <= tmp;

      return status;
    }



    public static GSL_ERROR
    qagp(ScalarFunctionDD f,
          double[] pts, int npts,
          double epsabs, double epsrel, int limit,
          gsl_integration_workspace workspace,
          out double result, out double abserr)
    {
      GSL_ERROR status = qagp(f, pts, npts,
                         epsabs, epsrel, limit,
                         workspace,
                         out result, out abserr,
                         QK.AlgorithmQk21);

      return status;
    }


    public static GSL_ERROR
    qagp(ScalarFunctionDD f,
          double[] pts, int npts,
          double epsabs, double epsrel,
          int limit,
          gsl_integration_workspace workspace,
          out double result, out double abserr,
          gsl_integration_rule q)
    {
      bool bDebug = true;
      double area, errsum;
      double res_ext, err_ext;
      double result0, abserr0, resabs0;
      double tolerance;

      double ertest = 0;
      double error_over_large_intervals = 0;
      double reseps = 0, abseps = 0, correc = 0;
      int ktmin = 0;
      int roundoff_type1 = 0, roundoff_type2 = 0, roundoff_type3 = 0;
      int error_type = 0;
      bool error_type2 = false;

      int iteration = 0;

      bool positive_integrand = false;
      bool extrapolate = false;
      bool disallow_extrapolation = false;

      extrapolation_table table = new extrapolation_table();

      int nint = npts - 1; /* number of intervals */

      int[] ndin = workspace.level; /* temporarily alias ndin to level */

      int i;

      /* Initialize results */

      result = 0;
      abserr = 0;

      /* Test on validity of parameters */

      if (limit > workspace.limit)
      {
        return new GSL_ERROR("iteration limit exceeds available workspace", GSL_ERR.GSL_EINVAL, bDebug);
      }

      if (npts > workspace.limit)
      {
        return new GSL_ERROR("npts exceeds size of workspace", GSL_ERR.GSL_EINVAL, bDebug);
      }

      if (epsabs <= 0 && (epsrel < 50 * GSL_CONST.GSL_DBL_EPSILON || epsrel < 0.5e-28))
      {
        return new GSL_ERROR("tolerance cannot be acheived with given epsabs and epsrel",
                   GSL_ERR.GSL_EBADTOL, bDebug);
      }

      /* Check that the integration range and break points are an
         ascending sequence */

      for (i = 0; i < nint; i++)
      {
        if (pts[i + 1] < pts[i])
        {
          return new GSL_ERROR("points are not in an ascending sequence", GSL_ERR.GSL_EINVAL, bDebug);
        }
      }

      /* Perform the first integration */

      result0 = 0;
      abserr0 = 0;
      resabs0 = 0;

      workspace.initialise(0.0, 0.0);

      for (i = 0; i < nint; i++)
      {
        double area1, error1, resabs1, resasc1;
        double a1 = pts[i];
        double b1 = pts[i + 1];

        q(f, a1, b1, out area1, out error1, out resabs1, out resasc1);

        result0 = result0 + area1;
        abserr0 = abserr0 + error1;
        resabs0 = resabs0 + resabs1;

        workspace.append_interval(a1, b1, area1, error1);

        if (error1 == resasc1 && error1 != 0.0)
        {
          ndin[i] = 1;
        }
        else
        {
          ndin[i] = 0;
        }
      }

      /* Compute the initial error estimate */

      errsum = 0.0;

      for (i = 0; i < nint; i++)
      {
        if (0 != ndin[i])
        {
          workspace.elist[i] = abserr0;
        }

        errsum = errsum + workspace.elist[i];

      }

      for (i = 0; i < nint; i++)
      {
        workspace.level[i] = 0;
      }

      /* Sort results into order of decreasing error via the indirection
         array order[] */

      workspace.sort_results();

      /* Test on accuracy */

      tolerance = Math.Max(epsabs, epsrel * Math.Abs(result0));

      if (abserr0 <= 100 * GSL_CONST.GSL_DBL_EPSILON * resabs0 && abserr0 > tolerance)
      {
        result = result0;
        abserr = abserr0;

        return new GSL_ERROR("cannot reach tolerance because of roundoff error on first attempt", GSL_ERR.GSL_EROUND, bDebug);
      }
      else if (abserr0 <= tolerance)
      {
        result = result0;
        abserr = abserr0;

        return null; // GSL_SUCCESS;
      }
      else if (limit == 1)
      {
        result = result0;
        abserr = abserr0;

        return new GSL_ERROR("a maximum of one iteration was insufficient", GSL_ERR.GSL_EMAXITER, bDebug);
      }

      /* Initialization */

      table.initialise_table();
      table.append_table(result0);

      area = result0;

      res_ext = result0;
      err_ext = GSL_CONST.GSL_DBL_MAX;

      error_over_large_intervals = errsum;
      ertest = tolerance;

      positive_integrand = test_positivity(result0, resabs0);

      iteration = nint - 1;

      do
      {
        int current_level;
        double a1, b1, a2, b2;
        double a_i, b_i, r_i, e_i;
        double area1 = 0, area2 = 0, area12 = 0;
        double error1 = 0, error2 = 0, error12 = 0;
        double resasc1, resasc2;
        double resabs1, resabs2;
        double last_e_i;

        /* Bisect the subinterval with the largest error estimate */

        workspace.retrieve(out a_i, out b_i, out r_i, out e_i);

        current_level = workspace.level[workspace.i] + 1;

        a1 = a_i;
        b1 = 0.5 * (a_i + b_i);
        a2 = b1;
        b2 = b_i;

        iteration++;

        q(f, a1, b1, out area1, out error1, out resabs1, out resasc1);
        q(f, a2, b2, out area2, out error2, out resabs2, out resasc2);

        area12 = area1 + area2;
        error12 = error1 + error2;
        last_e_i = e_i;

        /* Improve previous approximations to the integral and test for
           accuracy.

           We write these expressions in the same way as the original
           QUADPACK code so that the rounding errors are the same, which
           makes testing easier. */

        errsum = errsum + error12 - e_i;
        area = area + area12 - r_i;

        tolerance = Math.Max(epsabs, epsrel * Math.Abs(area));

        if (resasc1 != error1 && resasc2 != error2)
        {
          double delta = r_i - area12;

          if (Math.Abs(delta) <= 1.0e-5 * Math.Abs(area12) && error12 >= 0.99 * e_i)
          {
            if (!extrapolate)
            {
              roundoff_type1++;
            }
            else
            {
              roundoff_type2++;
            }
          }

          if (i > 10 && error12 > e_i)
          {
            roundoff_type3++;
          }
        }

        /* Test for roundoff and eventually set error flag */

        if (roundoff_type1 + roundoff_type2 >= 10 || roundoff_type3 >= 20)
        {
          error_type = 2;       /* round off error */
        }

        if (roundoff_type2 >= 5)
        {
          error_type2 = true;
        }

        /* set error flag in the case of bad integrand behaviour at
           a point of the integration range */

        if (subinterval_too_small(a1, a2, b2))
        {
          error_type = 4;
        }

        /* append the newly-created intervals to the list */

        workspace.update(a1, b1, area1, error1, a2, b2, area2, error2);

        if (errsum <= tolerance)
        {
          goto compute_result;
        }

        if (0 != error_type)
        {
          break;
        }

        if (iteration >= limit - 1)
        {
          error_type = 1;
          break;
        }

        if (disallow_extrapolation)
        {
          continue;
        }

        error_over_large_intervals += -last_e_i;

        if (current_level < workspace.maximum_level)
        {
          error_over_large_intervals += error12;
        }

        if (!extrapolate)
        {
          /* test whether the interval to be bisected next is the
             smallest interval. */
          if (workspace.large_interval())
            continue;

          extrapolate = true;
          workspace.nrmax = 1;
        }

        /* The smallest interval has the largest error.  Before
           bisecting decrease the sum of the errors over the larger
           intervals (error_over_large_intervals) and perform
           extrapolation. */

        if (!error_type2 && error_over_large_intervals > ertest)
        {
          if (workspace.increase_nrmax())
            continue;
        }

        /* Perform extrapolation */

        table.append_table(area);

        if (table.n < 3)
        {
          goto skip_extrapolation;
        }

        table.qelg(out reseps, out abseps);

        ktmin++;

        if (ktmin > 5 && err_ext < 0.001 * errsum)
        {
          error_type = 5;
        }

        if (abseps < err_ext)
        {
          ktmin = 0;
          err_ext = abseps;
          res_ext = reseps;
          correc = error_over_large_intervals;
          ertest = Math.Max(epsabs, epsrel * Math.Abs(reseps));
          if (err_ext <= ertest)
            break;
        }

        /* Prepare bisection of the smallest interval. */

        if (table.n == 1)
        {
          disallow_extrapolation = true;
        }

        if (error_type == 5)
        {
          break;
        }

      skip_extrapolation:

        workspace.reset_nrmax();
        extrapolate = false;
        error_over_large_intervals = errsum;

      }
      while (iteration < limit);

      result = res_ext;
      abserr = err_ext;

      if (err_ext == GSL_CONST.GSL_DBL_MAX)
        goto compute_result;

      if ((0 != error_type) || error_type2)
      {
        if (error_type2)
        {
          err_ext += correc;
        }

        if (error_type == 0)
          error_type = 3;

        if (result != 0 && area != 0)
        {
          if (err_ext / Math.Abs(res_ext) > errsum / Math.Abs(area))
            goto compute_result;
        }
        else if (err_ext > errsum)
        {
          goto compute_result;
        }
        else if (area == 0.0)
        {
          goto return_error;
        }
      }

      /*  Test on divergence. */

      {
        double max_area = Math.Max(Math.Abs(res_ext), Math.Abs(area));

        if (!positive_integrand && max_area < 0.01 * resabs0)
          goto return_error;
      }

      {
        double ratio = res_ext / area;

        if (ratio < 0.01 || ratio > 100 || errsum > Math.Abs(area))
          error_type = 6;
      }

      goto return_error;

    compute_result:

      result = workspace.sum_results();
      abserr = errsum;

    return_error:

      if (error_type > 2)
        error_type--;

      if (error_type == 0)
      {
        return null; // GSL_SUCCESS;
      }
      else if (error_type == 1)
      {
        return new GSL_ERROR("number of iterations was insufficient", GSL_ERR.GSL_EMAXITER, bDebug);
      }
      else if (error_type == 2)
      {
        return new GSL_ERROR("cannot reach tolerance because of roundoff error",
                   GSL_ERR.GSL_EROUND, bDebug);
      }
      else if (error_type == 3)
      {
        return new GSL_ERROR("bad integrand behavior found in the integration interval",
                   GSL_ERR.GSL_ESING, bDebug);
      }
      else if (error_type == 4)
      {
        return new GSL_ERROR("roundoff error detected in the extrapolation table",
                   GSL_ERR.GSL_EROUND, bDebug);
      }
      else if (error_type == 5)
      {
        return new GSL_ERROR("integral is divergent, or slowly convergent",
                   GSL_ERR.GSL_EDIVERGE, bDebug);
      }
      else
      {
        return new GSL_ERROR("could not integrate function", GSL_ERR.GSL_EFAILED, bDebug);
      }
    }

  }
}
