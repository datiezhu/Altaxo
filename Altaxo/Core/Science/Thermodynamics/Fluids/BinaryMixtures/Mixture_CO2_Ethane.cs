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
    /// State equations and constants of mixtures of CO2 and Ethane.
    /// </summary>
    /// <remarks>
    /// <para>References:</para>
    /// <para>The source code was created automatically using the mixture file 'co2-ethane.mix' from the following software:</para>
    /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
    /// <para>Further references (extracted from the mixture file):</para>
    /// <para>Info: Kunz and Wagner (2007)</para>
    /// <para>Departure function (MXM): Kunz, O., Klimeck, R., Wagner, W., Jaeschke, M. The GERG-2004 Wide-Range Equation of State for Natural Gases and Other Mixtures. GERG Technical Monograph 15. Fortschr.-Ber. VDI, VDI-Verlag, D�sseldorf, 2007.</para>
    /// </remarks>
    [CASRegistryNumber("124-38-9")]
    [CASRegistryNumber("74-84-0")]
    public class Mixture_CO2_Ethane : BinaryMixtureDefinitionBase
    {

        /// <summary>Gets the (only) instance of this class.</summary>
        public static Mixture_CO2_Ethane Instance { get; } = new Mixture_CO2_Ethane();

        #region Constants for the binary mixture of CO2 and Ethane

        /// <summary>Gets the CAS registry number of component 1 (CO2).</summary>
        public override string CASRegistryNumber1 { get; } = "124-38-9";

        /// <summary>Gets the CAS registry number of component 2 (Ethane).</summary>
        public override string CASRegistryNumber2 { get; } = "74-84-0";

        #endregion Constants for the binary mixture of CO2 and Ethane

        private Mixture_CO2_Ethane()
        {
            #region  Mixture parameter

            _beta_T = 1.013871147;
            _gamma_T = 0.90094953;
            _beta_v = 1.002525718;
            _gamma_v = 1.032876701;
            _F = 0;
            #endregion

        }
    }
}
