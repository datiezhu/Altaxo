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
	/// Equation of state based on the dimensionless Helmholtz energy, both for pure fluids and for mixtures of fluids.
	/// </summary>
	public abstract class HelmholtzEquationOfState
	{
		/// <summary>
		/// The universal gas constant in J/(K mol)
		/// </summary>
		public static readonly double UniversalGasConstant = 8.314459848;

		#region Reduced density and pressure

		/// <summary>
		/// Gets the (typical) molecular weight of the fluid.
		/// </summary>
		public abstract double MolecularWeight { get; }

		/// <summary>
		/// Gets the molar density (in mol/m³) used to calculate the reduced (dimensionless) density.
		/// </summary>
		/// <remarks>The reduced density called delta and is calculated by: delta = density / <see cref="ReducingMassDensity"/>.</remarks>
		public abstract double ReducingMoleDensity { get; }

		/// <summary>
		/// Gets the temperature (in Kelvin) that is used to calculate the inverse reduced temperature.
		/// </summary>
		/// <remarks>The inverse reduced temperature is called tau and is calculated by: tau = <see cref="ReducingTemperature"/> / temperature.</remarks>
		public abstract double ReducingTemperature { get; }

		/// <summary>
		/// Gets the universal gas constant that was used at the time this model was developed.
		/// </summary>
		public abstract double WorkingUniversalGasConstant { get; }

		/// <summary>
		/// Gets the density (in kg/m³) used to calculate the reduced (dimensionless) density.
		/// </summary>
		/// <remarks>The reduced density called delta and is calculated by: delta = density / <see cref="ReducingMassDensity"/>.</remarks>
		public double ReducingMassDensity => ReducingMoleDensity * MolecularWeight;

		public double WorkingSpecificGasConstant => WorkingUniversalGasConstant / MolecularWeight;

		/// <summary>
		/// Gets the reduced density by density / <see cref="ReducingMassDensity"/>.
		/// </summary>
		/// <param name="moleDensity">The mass density in kg/m³.</param>
		/// <returns>Reduced density.</returns>
		public virtual double GetDeltaFromMoleDensity(double moleDensity)
		{
			return moleDensity / ReducingMoleDensity;
		}

		/// <summary>
		/// Gets the reduced density by density / <see cref="ReducingMassDensity"/>.
		/// </summary>
		/// <param name="massDensity">The mass density in kg/m³.</param>
		/// <returns>Reduced density.</returns>
		public virtual double GetDeltaFromMassDensity(double massDensity)
		{
			return massDensity / (ReducingMoleDensity * MolecularWeight);
		}

		/// <summary>
		/// Gets the inverse reduced temperature by <see cref="ReducingTemperature"/> / temperature.
		/// </summary>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The inverse reduced temperature.</returns>
		public virtual double GetTauFromTemperature(double temperature)
		{
			return ReducingTemperature / temperature;
		}

		#endregion Reduced density and pressure

		#region Dimensionless Helmholtz energy and derivatives

		#region Ideal part of dimensionless Helmholtz energy and derivatives

		/// <summary>
		/// Ideal part of the dimensionless Helmholtz energy as function of reduced variables. (Page 1541, Table 28)
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>Ideal part of the dimensionless Helmholtz energy.</returns>
		public abstract double Phi0_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// First derivative of the dimensionless Helmholtz energy as function of reduced variables with respect to the inverse reduced temperature. (Page 1541, Table 28)
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>First derivative of the dimensionless Helmholtz energy as function of reduced variables with respect to the inverse reduced temperature.</returns>
		public abstract double Phi0_tau_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// Second derivative of Phi0 the of reduced variables with respect to the inverse reduced temperature. (Page 1541, Table 28)
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>Second derivative the dimensionless Helmholtz energy of reduced variables with respect to the inverse reduced temperature.</returns>
		public abstract double Phi0_tautau_OfReducedVariables(double delta, double tau);

		#endregion Ideal part of dimensionless Helmholtz energy and derivatives

		#region Residual part of dimensionless Helmholtz energy and derivatives

		/// <summary>
		/// Calculates the residual part of the dimensionless Helmholtz energy in dependence on reduced density and reduced inverse temperature.
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>The residual part of the dimensionless Helmholtz energy.</returns>
		public abstract double PhiR_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// Calculates the first derivative of the residual part of the dimensionless Helmholtz energy with respect to the reduced density delta.
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>First derivative of the residual part of the dimensionless Helmholtz energy with respect to the reduced density.</returns>
		public abstract double PhiR_delta_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// Calculates the second derivative of the residual part of the dimensionless Helmholtz energy with respect to the reduced density delta.
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>Second derivative of the residual part of the dimensionless Helmholtz energy with respect to the reduced density.</returns>
		public abstract double PhiR_deltadelta_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// Calculates the first derivative of the residual part of the dimensionless Helmholtz energy with respect to the inverse reduced temperature.
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>First derivative of the residual part of the dimensionless Helmholtz energy with respect to the inverse reduced temperature.</returns>
		public abstract double PhiR_tau_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// Calculates the second derivative of the residual part of the dimensionless Helmholtz energy with respect to the inverse reduced temperature.
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>Second derivative of the residual part of the dimensionless Helmholtz energy with respect to the inverse reduced temperature.</returns>
		public abstract double PhiR_tautau_OfReducedVariables(double delta, double tau);

		/// <summary>
		/// Calculates the derivative of the residual part of the dimensionless Helmholtz energy with respect to the reduced density delta and the inverse reduced temperature tau.
		/// </summary>
		/// <param name="delta">The reduced density ( = density / <see cref="ReducingMassDensity"/>)</param>
		/// <param name="tau">The reduced inverse temperature (= <see cref="ReducingTemperature"/> / temperature)</param>
		/// <returns>First derivative of the residual part of the dimensionless Helmholtz energy with respect to the reduced density delta and the inverse reduced temperature tau.</returns>
		public abstract double PhiR_deltatau_OfReducedVariables(double delta, double tau);

		#endregion Residual part of dimensionless Helmholtz energy and derivatives

		#endregion Dimensionless Helmholtz energy and derivatives

		#region Thermodynamic properties derived from dimensionless Helmholtz energy

		/// <summary>
		/// Get the pressure from a given molar density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The pressure in Pa.</returns>
		public virtual double Pressure_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phir_delta = PhiR_delta_OfReducedVariables(delta, tau); // derivative of PhiR with respect to delta

			return moleDensity * temperature * WorkingUniversalGasConstant * (1 + delta * phir_delta);
		}

		/// <summary>
		/// Get the pressure from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The pressure in Pa.</returns>
		public virtual double Pressure_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return Pressure_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		public double IsothermalDerivativePressureWrtMoleDensity_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature
			double phir_delta = PhiR_delta_OfReducedVariables(delta, tau); // derivative of PhiR with respect to delta
			double phir_deltadelta = PhiR_deltadelta_OfReducedVariables(delta, tau); // derivative of PhiR with respect to delta

			return WorkingUniversalGasConstant * temperature * (1 + 2 * delta * phir_delta + delta * delta * phir_deltadelta);
		}

		public double IsothermalDerivativePressureByMassDensity_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return IsothermalDerivativePressureWrtMoleDensity_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Gets the isothermal compressibility in 1/Pa from mole density (mol/m³) and temperature (K).
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isothermal compressibility in 1/Pa.</returns>
		public double IsothermalCompressibility_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double dpdrho = IsothermalDerivativePressureWrtMoleDensity_FromMoleDensityAndTemperature(moleDensity, temperature);
			return 1 / (dpdrho * moleDensity);
		}

		/// <summary>
		/// Gets the isothermal compressibility in 1/Pa from mass density (kg/m³) and temperature (K).
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isothermal compressibility in 1/Pa.</returns>
		public double IsothermalCompressibility_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return IsothermalDerivativePressureWrtMoleDensity_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Gets the isothermal compressional modulus in Pa from density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isothermal compressional modulus K in Pa.</returns>
		public double IsothermalCompressionalModulus_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double dpdrho = IsothermalDerivativePressureWrtMoleDensity_FromMoleDensityAndTemperature(moleDensity, temperature);
			return dpdrho * moleDensity;
		}

		/// <summary>
		/// Gets the isothermal compressional modulus K in Pa from density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isothermal compressional modulus K in Pa.</returns>
		public double IsothermalCompressionalModulus_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return IsothermalCompressionalModulus_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the densities from a given pressure and temperature.
		/// </summary>
		/// <param name="pressure">The pressure in Pa.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <param name="relativeAccuracy">The target relative accuracy of the result.</param>
		/// <param name="moleDensityStartValue">The start value for the density to search for.</param>
		/// <returns>The density in kg/m³</returns>
		/// <remarks>The density has to be calculated iteratively, using Newton-Raphson.
		/// Therefore we need the target accuracy.
		/// The iteration is ended if the pressure calculated back from the density compared with the pressure given in the argument
		/// is within the relative accuracy.
		/// </remarks>
		public double MoleDensity_FromPressureAndTemperature(double pressure, double temperature, double relativeAccuracy = 1E-4, double moleDensityStartValue = double.NaN)
		{
			if (!(pressure > 0))
				throw new ArgumentOutOfRangeException(nameof(pressure), "Must be >0");
			if (!(temperature > 0))
				throw new ArgumentOutOfRangeException(nameof(temperature), "Must be >0");
			if (!(relativeAccuracy > 0))
				throw new ArgumentOutOfRangeException(nameof(relativeAccuracy), "Must be >0");

			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double RTRhoC = WorkingUniversalGasConstant * temperature * ReducingMoleDensity;

			double delta;
			if (moleDensityStartValue > 0)
				delta = moleDensityStartValue / ReducingMoleDensity;
			else
				delta = 1.5;

			for (int i = 0; i < 100000; ++i)
			{
				double phir_delta = PhiR_delta_OfReducedVariables(delta, tau); // derivative of PhiR with respect to delta
				double phir_deltadelta = PhiR_deltadelta_OfReducedVariables(delta, tau);

				double press = RTRhoC * (delta + delta * delta * phir_delta);
				double press_delta = RTRhoC * (1 + 2 * delta * phir_delta + delta * delta * phir_deltadelta);

				double delta_new = delta - (press - pressure) / press_delta;

				if (delta_new > 0 && press_delta > 0)    // if density remains positive and volume stability
					delta = delta_new;  // then use that new value
				else                  // otherwise, if it goes into the negative region
					delta = delta / 2;  // then make it simply smaller

				if (Math.Abs((press - pressure) / pressure) < relativeAccuracy)
					break;
			}

			return delta * ReducingMoleDensity;
		}

		/// <summary>
		/// Get the densities from a given pressure and temperature.
		/// </summary>
		/// <param name="pressure">The pressure in Pa.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <param name="relativeAccuracy">The target relative accuracy of the result.</param>
		/// <param name="densityStartValue">The start value for the density to search for.</param>
		/// <returns>The density in kg/m³</returns>
		/// <remarks>The density has to be calculated iteratively, using Newton-Raphson.
		/// Therefore we need the target accuracy.
		/// The iteration is ended if the pressure calculated back from the density compared with the pressure given in the argument
		/// is within the relative accuracy.
		/// </remarks>
		public double MassDensity_FromPressureAndTemperature(double pressure, double temperature, double relativeAccuracy = 1E-4, double densityStartValue = double.NaN)
		{
			if (!(pressure > 0))
				throw new ArgumentOutOfRangeException(nameof(pressure), "Must be >0");
			if (!(temperature > 0))
				throw new ArgumentOutOfRangeException(nameof(temperature), "Must be >0");
			if (!(relativeAccuracy > 0))
				throw new ArgumentOutOfRangeException(nameof(relativeAccuracy), "Must be >0");

			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double RTRhoC = WorkingSpecificGasConstant * temperature * ReducingMassDensity;

			double delta;
			if (densityStartValue > 0)
				delta = densityStartValue / ReducingMassDensity;
			else
				delta = 1.5;

			for (int i = 0; i < 100000; ++i)
			{
				double phir_delta = PhiR_delta_OfReducedVariables(delta, tau); // derivative of PhiR with respect to delta
				double phir_deltadelta = PhiR_deltadelta_OfReducedVariables(delta, tau);

				double press = RTRhoC * (delta + delta * delta * phir_delta);
				double press_delta = RTRhoC * (1 + 2 * delta * phir_delta + delta * delta * phir_deltadelta);

				double delta_new = delta - (press - pressure) / press_delta;

				if (delta_new > 0 && press_delta > 0)    // if density remains positive and volume stability
					delta = delta_new;  // then use that new value
				else                  // otherwise, if it goes into the negative region
					delta = delta / 2;  // then make it simply smaller

				if (Math.Abs((press - pressure) / pressure) < relativeAccuracy)
					break;
			}

			return delta * ReducingMassDensity;
		}

		/// <summary>
		/// Get the Helmholtz energy from a given mole density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Helmholtz energy in J/(mol K).</returns>
		public double MoleSpecificHelmholtzEnergy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phi0 = Phi0_OfReducedVariables(delta, tau);
			double phiR = PhiR_OfReducedVariables(delta, tau);

			return WorkingUniversalGasConstant * temperature * (phi0 + phiR);
		}

		/// <summary>
		/// Get the Helmholtz energy from a given mass density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Helmholtz energy in J/(mol K).</returns>
		public double MoleSpecificHelmholtzEnergy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificHelmholtzEnergy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the mass specific Helmholtz energy from a given mass density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Helmholtz energy in J/(kg K).</returns>
		public double MassSpecificHelmholtzEnergy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificHelmholtzEnergy_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the Helmholtz energy from a given mass density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Helmholtz energy in J/(kg K).</returns>
		public double MassSpecificHelmholtzEnergy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificHelmholtzEnergy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the mole specific Gibbs energy from a given mass density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Gibbs energy in J/(mol K).</returns>
		public double MoleSpecificGibbsEnergy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phi0 = Phi0_OfReducedVariables(delta, tau);
			double phiR = PhiR_OfReducedVariables(delta, tau);
			double phiR_delta = PhiR_delta_OfReducedVariables(delta, tau);

			return WorkingUniversalGasConstant * temperature * ((1 + delta * phiR_delta) + (phi0 + phiR));
		}

		/// <summary>
		/// Get the mole specific Gibbs energy from a given mass density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Gibbs energy in J/(mol K).</returns>
		public double MoleSpecificGibbsEnergy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificGibbsEnergy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the mass specific Gibbs energy from a given mass density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Gibbs energy in J/(kg K).</returns>
		public double MassSpecificGibbsEnergy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificGibbsEnergy_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the mass specific Gibbs energy from a given mass density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The Gibbs energy in J/(kg K).</returns>
		public double MassSpecificGibbsEnergy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificGibbsEnergy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the entropy from a given mole density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The entropy in J/(mol K).</returns>
		public double MoleSpecificEntropy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phi0 = Phi0_OfReducedVariables(delta, tau);
			double phiR = PhiR_OfReducedVariables(delta, tau);

			double phi0_tau = Phi0_tau_OfReducedVariables(delta, tau);
			double phiR_tau = PhiR_tau_OfReducedVariables(delta, tau);

			return WorkingUniversalGasConstant * (tau * (phi0_tau + phiR_tau) - phi0 - phiR);
		}

		/// <summary>
		/// Get the entropy from a given mole density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The entropy in J/(mol K).</returns>
		public double MoleSpecificEntropy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificEntropy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the entropy from a given mole density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The entropy in J/(kg K).</returns>
		public double MassSpecificEntropy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificEntropy_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight; ;
		}

		/// <summary>
		/// Get the entropy from a given mole density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The entropy in J/(kg K).</returns>
		public double MassSpecificEntropy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificEntropy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight; ;
		}

		/// <summary>
		/// Get the internal energy from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The internal energy in J/mol.</returns>
		public double MoleSpecificInternalEnergy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature
			double phi0_tau = Phi0_tau_OfReducedVariables(delta, tau);
			double phiR_tau = PhiR_tau_OfReducedVariables(delta, tau);

			return WorkingUniversalGasConstant * ReducingTemperature * (phi0_tau + phiR_tau);
		}

		/// <summary>
		/// Get the internal energy from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The internal energy in J/mol.</returns>
		public double MoleSpecificInternalEnergy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificInternalEnergy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the internal energy from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The internal energy in J/mol.</returns>
		public double MassSpecificInternalEnergy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificInternalEnergy_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the internal energy from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The internal energy in J/kg.</returns>
		public double MassSpecificInternalEnergy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificInternalEnergy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the enthalpy from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The enthalpy in J/mol.</returns>
		public double MoleSpecificEnthalpy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phi0_tau = Phi0_tau_OfReducedVariables(delta, tau);
			double phiR_tau = PhiR_tau_OfReducedVariables(delta, tau);
			double phiR_delta = PhiR_delta_OfReducedVariables(delta, tau);

			return WorkingUniversalGasConstant * temperature *
				(1 + tau * (phi0_tau + phiR_tau) + delta * phiR_delta);
		}

		/// <summary>
		/// Get the enthalpy from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The enthalpy in J/mol.</returns>
		public double MoleSpecificEnthalpy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificEnthalpy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the enthalpy from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The enthalpy in J/kg.</returns>
		public double MassSpecificEnthalpy_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificEnthalpy_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the enthalpy from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The enthalpy in J/kg.</returns>
		public double MassSpecificEnthalpy_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificEnthalpy_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the mole specific isochoric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isochoric heat capacity in J/(mol K).</returns>
		public double MoleSpecificIsochoricHeatCapacity_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phi0_tautau = Phi0_tautau_OfReducedVariables(delta, tau);
			double phiR_tautau = PhiR_tautau_OfReducedVariables(delta, tau);

			return -Pow2(tau) * (phi0_tautau + phiR_tautau) * WorkingUniversalGasConstant;
		}

		/// <summary>
		/// Get the mole specific isochoric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isochoric heat capacity in J/(mol K).</returns>
		public double MoleSpecificIsochoricHeatCapacity_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificIsochoricHeatCapacity_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Get the isochoric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isochoric heat capacity in J/(kg K).</returns>
		public double MassSpecificIsochoricHeatCapacity_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificIsochoricHeatCapacity_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the isochoric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isochoric heat capacity in J/(kg K).</returns>
		public double MassSpecificIsochoricHeatCapacity_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificIsochoricHeatCapacity_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Gets the isobaric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isobaric heat capacity in J/(mol K).</returns>
		public double MoleSpecificIsobaricHeatCapacity_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phi0_tautau = Phi0_tautau_OfReducedVariables(delta, tau);
			double phiR_delta = PhiR_delta_OfReducedVariables(delta, tau);
			double phiR_deltadelta = PhiR_deltadelta_OfReducedVariables(delta, tau);
			double phiR_deltatau = PhiR_deltatau_OfReducedVariables(delta, tau);
			double phiR_tautau = PhiR_tautau_OfReducedVariables(delta, tau);

			return WorkingUniversalGasConstant *
							(-Pow2(tau) * (phi0_tautau + phiR_tautau) // isochoric heat capacity
							+
							Pow2(1 + delta * phiR_delta - delta * tau * phiR_deltatau) /
							(1 + 2 * delta * phiR_delta + delta * delta * phiR_deltadelta)
							);
		}

		/// <summary>
		/// Gets the isobaric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isobaric heat capacity in J/(mol K).</returns>
		public double MoleSpecificIsobaricHeatCapacity_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificIsobaricHeatCapacity_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		/// <summary>
		/// Gets the isobaric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isobaric heat capacity in J/(kg K).</returns>
		public double MassSpecificIsobaricHeatCapacity_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			return MoleSpecificIsobaricHeatCapacity_FromMoleDensityAndTemperature(moleDensity, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Gets the isobaric heat capacity from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The isobaric heat capacity in J/(kg K).</returns>
		public double MassSpecificIsobaricHeatCapacity_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return MoleSpecificIsobaricHeatCapacity_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature) / MolecularWeight;
		}

		/// <summary>
		/// Get the speed of sound from a given density and temperature.
		/// </summary>
		/// <param name="moleDensity">The density in mol/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The speed of sound in m/s.</returns>
		public double SpeedOfSound_FromMoleDensityAndTemperature(double moleDensity, double temperature)
		{
			double delta = GetDeltaFromMoleDensity(moleDensity); // reduced density
			double tau = GetTauFromTemperature(temperature); // reduced inverse temperature

			double phir_delta = PhiR_delta_OfReducedVariables(delta, tau); // 1st derivative of PhiR with respect to delta
			double phir_deltadelta = PhiR_deltadelta_OfReducedVariables(delta, tau); // 2nd derivative of PhiR with respect to delta
			double phir_deltatau = PhiR_deltatau_OfReducedVariables(delta, tau); // derivative of PhiR with respect to delta and tau
			double phir_tautau = PhiR_tautau_OfReducedVariables(delta, tau); // 2nd derivative of PhiR with respect to tau
			double phi0_tautau = Phi0_tautau_OfReducedVariables(delta, tau);
			double w2_RT = 1 + 2 * delta * phir_delta + Pow2(delta) * phir_deltadelta -
											Pow2(1 + delta * phir_delta - delta * tau * phir_deltatau) / (Pow2(tau) * (phi0_tautau + phir_tautau));

			return Math.Sqrt(w2_RT * temperature * WorkingUniversalGasConstant / MolecularWeight);
		}

		/// <summary>
		/// Get the speed of sound from a given density and temperature.
		/// </summary>
		/// <param name="massDensity">The density in kg/m³.</param>
		/// <param name="temperature">The temperature in Kelvin.</param>
		/// <returns>The speed of sound in m/s.</returns>
		public double SpeedOfSound_FromMassDensityAndTemperature(double massDensity, double temperature)
		{
			return SpeedOfSound_FromMoleDensityAndTemperature(massDensity / MolecularWeight, temperature);
		}

		#endregion Thermodynamic properties derived from dimensionless Helmholtz energy

		#region Helper functions

		protected static double Pow2(double x)
		{
			return x * x;
		}

		#endregion Helper functions
	}
}
