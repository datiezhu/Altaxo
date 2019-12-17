﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2018 Dr. Dirk Lellinger
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altaxo.Science.Thermodynamics.Fluids
{

    /// <summary>
    /// State equations and constants of cis-2-butene.
    /// Short name: cis-butene.
    /// Synomym: (Z)-2-butene.
    /// Chemical formula: CH3-CH=CH-CH3.
    /// </summary>
    /// <remarks>
    /// <para>References:</para>
    /// <para>The source code was created automatically using the fluid file 'c2butene.fld' from the following software:</para>
    /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
    /// <para>Further references (extracted from the fluid file):</para>
    /// <para>EquationOfState (EOS): Lemmon, E.W. and Ihmels, E.C., "Thermodynamic Properties of the Butenes.  Part II. Short Fundamental Equations of State," Fluid Phase Equilibria, 228-229C:173-187, 2005.</para>
    /// <para>HeatCapacity (CPP): Lemmon, E.W. and Ihmels, E.C.,</para>
    /// <para>Saturated vapor pressure: Lemmon, C.K. and Lemmon, E.W., 2010.</para>
    /// <para>Saturated liquid density: Lemmon, C.K. and Lemmon, E.W., 2010.</para>
    /// <para>Saturated vapor density: Lemmon, C.K. and Lemmon, E.W., 2010.</para>
    /// </remarks>
    [CASRegistryNumber("590-18-1")]
    public class Cis_2_butene : HelmholtzEquationOfStateOfPureFluidsBySpanEtAl
    {

        /// <summary>Gets the (only) instance of this class.</summary>
        public static Cis_2_butene Instance { get; } = new Cis_2_butene();

        #region Constants for cis-2-butene

        /// <summary>The full name of the fluid.</summary>
        public override string FullName => "cis-2-butene";

        /// <summary>The short name of the fluid.</summary>
        public override string ShortName => "cis-butene";

        /// <summary>The synonym of the name of the fluid.</summary>
        public override string Synonym => "(Z)-2-butene";

        /// <summary>The chemical formula of the fluid.</summary>
        public override string ChemicalFormula => "CH3-CH=CH-CH3";

        /// <summary>The chemical formula of the fluid.</summary>
        public override string FluidFamily => "n-alkene";

        /// <summary>Gets the CAS registry number.</summary>
        public override string CASRegistryNumber { get; } = "590-18-1";

        private int[] _unNumbers = new int[] { 1012, };
        /// <summary>The UN number of the fluid.</summary>
        public override IReadOnlyList<int> UN_Numbers => _unNumbers;

        /// <summary>The Universal Gas Constant R at the time the model was developed.</summary>
        public override double WorkingUniversalGasConstant => 8.314472;

        /// <summary>Gets the molecular weight in kg/mol.</summary>
        public override double MolecularWeight { get; } = 0.05610632; // kg/mol

        /// <summary>Gets the temperature at the critical point in K.</summary>
        public override double CriticalPointTemperature { get; } = 435.75;

        /// <summary>Gets the pressure at the critical point in Pa.</summary>);
        public override double CriticalPointPressure { get; } = 4225500;

        /// <summary>Gets the mole density at the critical point in mol/m³.</summary>
        public override double CriticalPointMoleDensity { get; } = 4244;

        /// <summary>Gets the triple point temperature in K.</summary>
        public override double TriplePointTemperature { get; } = 134.3;

        /// <summary>Gets the triple point pressure in Pa.</summary>
        public override double TriplePointPressure { get; } = 0.2636;

        /// <summary>Gets the triple point liquid mole density in mol/m³.</summary>
        public override double TriplePointSaturatedLiquidMoleDensity { get; } = 14084.011372882;

        /// <summary>Gets the triple point vapor mole density in mol/m³.</summary>
        public override double TriplePointSaturatedVaporMoleDensity { get; } = 0.000236111780831419;

        /// <summary>Gets the boiling temperature at normal pressure (101325 Pa) in K (if existent). If not existent, the return value is null.</summary>
        public override double? NormalBoilingPointTemperature { get; } = 276.873516912612;

        /// <summary>Gets the sublimation temperature at normal pressure (101325 Pa) in K (if existent). If not existent, the return value is null.</summary>
        public override double? NormalSublimationPointTemperature { get; } = null;

        /// <summary>Gets the acentric factor.</summary>
        public override double AcentricFactor { get; } = 0.202;

        /// <summary>Gets the dipole moment in Debye.</summary>
        public override double DipoleMoment { get; } = 0.3;

        /// <summary>Gets the lower temperature limit of this model in K.</summary>
        public override double LowerTemperatureLimit { get; } = 134.3;

        /// <summary>Gets the upper temperature limit of this model in K.</summary>
        public override double UpperTemperatureLimit { get; } = 525;

        /// <summary>Gets the upper density limit of this model in mol/m³.</summary>
        public override double UpperMoleDensityLimit { get; } = 14090;

        /// <summary>Gets the upper pressure limit of this model in Pa.</summary>
        public override double UpperPressureLimit { get; } = 50000000;

        #endregion Constants for cis-2-butene

        private Cis_2_butene()
        {
            #region Ideal part of dimensionless Helmholtz energy and derivatives

            _alpha0_n_const = 0.259154200178084;
            _alpha0_n_tau = 2.4189887758146;
            _alpha0_n_lntau = 2.9687;
            _alpha0_n_taulntau = 0;

            _alpha0_Poly = new (double ni, double thetai)[]
            {
            };

            _alpha0_Exp = new (double ni, double thetai)[]
            {
          (              3.2375,    0.569133677567413),
          (              7.0437,       2.714859437751),
          (              11.414,     4.80091795754446),
          (              7.3722,     10.0906483075158),
            };

            _alpha0_Cosh = new (double ni, double thetai)[]
            {
            };

            _alpha0_Sinh = new (double ni, double thetai)[]
            {
            };
            #endregion Ideal part of dimensionless Helmholtz energy and derivatives

            #region Residual part(s) of dimensionless Helmholtz energy and derivatives

            _alphaR_Poly = new (double ni, double ti, int di)[]
            {
          (             0.77827,                 0.12,                    1),
          (             -2.8064,                  1.3,                    1),
          (               1.003,                 1.74,                    1),
          (            0.013762,                  2.1,                    2),
          (            0.085514,                 0.28,                    3),
          (          0.00021268,                 0.69,                    7),
            };

            _alphaR_Exp = new (double ni, double ti, int di, double gi, int li)[]
            {
          (             0.22962,                 0.75,                    2,                   -1,                    1),
          (           -0.072442,                    2,                    5,                   -1,                    1),
          (            -0.23722,                  4.4,                    1,                   -1,                    2),
          (           -0.074071,                  4.7,                    4,                   -1,                    2),
          (           -0.026547,                   15,                    3,                   -1,                    3),
          (            0.012032,                   14,                    4,                   -1,                    3),
            };

            _alphaR_Gauss = new (double ni, double ti, int di, double alpha, double beta, double gamma, double epsilon)[]
            {
            };

            _alphaR_Nonanalytical = new (double ni, double b, double beta, double A, double C, double D, double B, double a)[]
            {
            };

            #endregion

            #region Saturated densities and pressure

            _saturatedLiquidDensity_Type = 1;
            _saturatedLiquidDensity_Coefficients = new (double factor, double exponent)[]
            {
          (              4.6849,                0.402),
          (             -5.4614,                 0.54),
          (              3.4718,                 0.69),
          (              5.0511,                  6.6),
          (             -5.0389,                    7),
            };

            _saturatedVaporDensity_Type = 3;
            _saturatedVaporDensity_Coefficients = new (double factor, double exponent)[]
            {
          (             -2.8918,               0.4098),
          (             -5.8582,                1.174),
          (             -17.443,                 3.11),
          (             -24.566,                  6.1),
          (             -29.413,                  7.6),
          (             -113.92,                 14.8),
            };

            _saturatedVaporPressure_Type = 5;
            _saturatedVaporPressure_Coefficients = new (double factor, double exponent)[]
            {
          (             -7.0022,                    1),
          (              1.3695,                  1.5),
          (             -3.0509,                  3.2),
          (             0.10012,                 3.46),
          (             -1.5577,                  6.4),
            };

            #endregion Saturated densities and pressure

        }
    }
}
