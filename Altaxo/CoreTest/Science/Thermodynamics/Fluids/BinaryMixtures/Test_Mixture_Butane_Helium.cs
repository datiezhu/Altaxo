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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altaxo.Science.Thermodynamics.Fluids
{

	/// <summary>
	/// Tests and test data for <see cref="Mixture_Butane_Helium"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Butane_Helium : MixtureTestBase
    {

    public Test_Mixture_Butane_Helium()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("106-97-8", 0.5), ("7440-59-7", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601356670314765, 1000.00000000001, 7.36058510907575E-06, 12.527381859611, 1 ),
      ( 200, 6.01316836617103, 10000.0000002082, 7.36051810297212E-05, 12.5275190573846, 1 ),
      ( 200, 60.0918829667103, 100000.002271354, 0.000735984583568835, 12.5288906332946, 1 ),
      ( 200, 596.971617610881, 1000000.00000158, 0.00735291077725086, 12.5425662384478, 1 ),
      ( 250, 0.481086078489472, 1000.00000000001, 5.81773621586668E-06, 12.5377787103108, 1 ),
      ( 250, 4.81060890755535, 10000.0000000727, 5.81767638501959E-05, 12.5378872599831, 1 ),
      ( 250, 48.0809186902869, 100000.00072293, 0.000581707729648662, 12.5389724777431, 1 ),
      ( 250, 478.309415230649, 1000000.00000014, 0.00581101266942903, 12.5497967994072, 1 ),
      ( 250, 4549.34383083754, 10000000.0004483, 0.0574906958262691, 12.6552954733447, 1 ),
      ( 250, 31460.8867230853, 100000000.001931, 0.529165028178758, 13.489838212357, 1 ),
      ( 300, 0.400905486152124, 1000, 4.76824527006665E-06, 12.5498803119887, 1 ),
      ( 300, 4.00888282623276, 10000.0000000276, 4.76819736679032E-05, 12.5499693591864, 1 ),
      ( 300, 40.0716347513328, 100000.000274405, 0.000476771801761095, 12.5508596304813, 1 ),
      ( 300, 399.006969430216, 1000000.00000002, 0.00476289507336377, 12.5597423094684, 1 ),
      ( 300, 3828.58073090491, 10000000.0000225, 0.0471436439400752, 12.6466018954487, 1 ),
      ( 300, 27918.7421736124, 100000000, 0.435979440888757, 13.355970161021, 1 ),
      ( 350, 0.343633533109833, 1000, 4.01376013770047E-06, 12.5628076658198, 1 ),
      ( 350, 3.43621120374584, 10000.000000012, 4.01372238435618E-05, 12.5628826765694, 1 ),
      ( 350, 34.3497055163611, 100000.000119063, 0.000401334471100427, 12.5636326347884, 1 ),
      ( 350, 342.262591626636, 1000000, 0.00400955516300294, 12.5711173157345, 1 ),
      ( 350, 3305.08195133293, 10000000.0000016, 0.0397167677909619, 12.6445018906621, 1 ),
      ( 350, 25100.0183060173, 100000000, 0.369062397416779, 13.2587475075375, 1 ),
      ( 400, 0.300679511520054, 1000.01820334017, 3.44827179331196E-06, 12.5756986106957, 1 ),
      ( 400, 3.00670180679582, 10000.0000000057, 3.4481790820974E-05, 12.5757630882137, 1 ),
      ( 400, 30.0576913025443, 100000.000057137, 0.000344787959359211, 12.5764077504284, 1 ),
      ( 400, 299.648296236629, 1000000, 0.00344487887882388, 12.5828429655461, 1 ),
      ( 400, 2907.51424734811, 10000000.0000002, 0.0341498708049476, 12.6460753453896, 1 ),
      ( 400, 22799.3716655009, 99999999.9999999, 0.31881067924708, 13.1858148586406, 1 ),
      ( 500, 0.240543785466042, 1000.01098986128, 2.66227582617232E-06, 12.5995800201522, 1 ),
      ( 500, 2.40538034991816, 10000.0000000016, 2.6622271488337E-05, 12.5996298400276, 1 ),
      ( 500, 24.0480422014339, 100000.000016405, 0.000266203152094606, 12.6001279676593, 1 ),
      ( 500, 239.906270204813, 1000000, 0.0026600740965543, 12.605102082669, 1 ),
      ( 500, 2343.55961984783, 10000000.0018955, 0.0264063121102471, 12.6541390634328, 1 ),
      ( 500, 19263.7101675525, 100000000.000681, 0.2486921603823, 13.0850837603716, 1 ),
      };

      // TestData for 500 Permille to 500 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_500_500 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601386201456031, 999.9850023865, -4.17442697170478E-05, 40.3013388178573, 2 ),
      ( 250, 0.481097737571143, 999.999999998441, -1.84167354705823E-05, 45.501187569474, 2 ),
      ( 250, 4.81177479162477, 9999.99999999939, -0.000184135469631751, 45.5025832849564, 2 ),
      ( 300, 0.400910433202887, 999.999999999946, -7.57135465724788E-06, 51.5530116648053, 2 ),
      ( 300, 4.00937740238465, 9999.99999947355, -7.56787590860834E-05, 51.5538651525029, 2 ),
      ( 300, 40.1209633639209, 99999.999999403, -0.000753311600772535, 51.5623842048541, 2 ),
      };

      // TestData for 999 Permille to 1 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_999_001 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.602047177324229, 999.999999946661, -0.00113957958468522, 68.1241953042003, 2 ),
      ( 250, 0.481338496055236, 999.999999717044, -0.000518592915014006, 78.4824239412123, 2 ),
      ( 250, 4.83610758893076, 9999.99688450688, -0.00521469119687183, 78.6475897954593, 2 ),
      ( 300, 0.401019699007496, 999.999998593248, -0.000280039209550953, 90.5640446183679, 2 ),
      ( 300, 4.02036195676815, 9999.9848962499, -0.00280769793734084, 90.6371985232481, 2 ),
      ( 300, 41.2817329029233, 100000.006754148, -0.0288503685615197, 91.4128767305155, 2 ),
      ( 300, 9838.28241971249, 999999.999999023, -0.959250265375165, 100.421836632219, 1 ),
      ( 300, 10080.9061254896, 10000000.0015025, -0.60231015661336, 100.834336198498, 1 ),
      ( 300, 11351.9133350983, 100000000.000006, 2.53162842186519, 104.382783006482, 1 ),
      ( 350, 0.343692636148946, 999.999999772943, -0.000167951740744558, 103.476074176271, 2 ),
      ( 350, 3.44213835421368, 9999.9976400468, -0.0016818699663362, 103.513320982884, 2 ),
      ( 350, 34.959950730381, 99999.9998907703, -0.0170612223420446, 103.896957634937, 2 ),
      ( 350, 9144.00728343115, 10000000.0000001, -0.624196589393917, 111.695632757041, 1 ),
      ( 350, 10847.483144275, 100000000.00003, 2.16787689644896, 115.145071885303, 1 ),
      ( 400, 0.300712948382303, 999.999999997336, -0.000107744134791419, 116.353506388955, 2 ),
      ( 400, 3.01005127813037, 9999.99961171125, -0.00107831875438892, 116.374560031424, 2 ),
      ( 400, 30.3985611941065, 99999.9999981185, -0.0108724341032868, 116.588466685199, 2 ),
      ( 400, 341.518020536245, 999999.998424165, -0.119576331796655, 119.147035796307, 2 ),
      ( 400, 8024.64301878757, 10000000.0000047, -0.625303521138544, 123.452152506224, 1 ),
      ( 400, 10368.3471152367, 100000000.000311, 1.89998535914041, 126.413194693797, 1 ),
      ( 500, 0.240556423838458, 999.999999996923, -4.982273006641E-05, 140.211255523143, 2 ),
      ( 500, 2.40664379676028, 9999.99996888656, -0.000498374604179918, 140.219625059456, 2 ),
      ( 500, 24.1752847560385, 99999.999999997, -0.00499853013438468, 140.3036815782, 2 ),
      ( 500, 253.609159680157, 1000000.00034266, -0.0515151780817988, 141.181938499792, 2 ),
      ( 500, 4584.5149145896, 10000000, -0.475311034772322, 147.673451082202, 1 ),
      ( 500, 9481.68555287797, 100000000.008632, 1.5369375237805, 148.053335847118, 1 ),
      };
    }

    [Test]
    public override void CASNumberAttribute_Test()
    {
      base.CASNumberAttribute_Test();
    }

    [Test]
    public override void Constants_Test()
    {
      base.Constants_Test();
    }

    [Test]
    public override void DeltaPhiRDelta_001_999_Test()
    {
      base.DeltaPhiRDelta_001_999_Test();
    }

    [Test]
    public override void DeltaPhiRDelta_500_500_Test()
    {
      base.DeltaPhiRDelta_500_500_Test();
    }

    [Test]
    public override void DeltaPhiRDelta_999_001_Test()
    {
      base.DeltaPhiRDelta_999_001_Test();
    }
  }
}
