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
	/// Tests and test data for <see cref="Mixture_CO2_SO2"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_CO2_SO2 : MixtureTestBase
    {

    public Test_Mixture_CO2_SO2()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("124-38-9", 0.5), ("7446-09-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481245812425088, 999.999999999553, -0.000324917205435317, 29.8543448565972, 2 ),
      ( 250, 4.82662609559325, 9999.99999543808, -0.00325934129448964, 30.0084641380654, 2 ),
      ( 250, 23348.7629932223, 99999.9999981846, -0.997939550598417, 53.1222550213276, 1 ),
      ( 250, 23368.3380231841, 1000000.00068275, -0.979412765803369, 53.1219692359758, 1 ),
      ( 250, 23556.2883347457, 9999999.99999967, -0.795770267139191, 53.1367701671372, 1 ),
      ( 300, 0.400972058555434, 999.99999999987, -0.00016007533919125, 31.6469580381622, 2 ),
      ( 300, 4.01551577645797, 9999.9999986641, -0.00160304395265595, 31.7225931013543, 2 ),
      ( 300, 40.7537285330804, 99999.9846521658, -0.0162670060332321, 32.4949656177566, 2 ),
      ( 300, 21275.0131935653, 1000000.01429482, -0.981155928088459, 51.8913385628147, 1 ),
      ( 300, 21572.1984596183, 10000000.000021, -0.814155301061492, 51.6499708626559, 1 ),
      ( 350, 0.343666559963293, 999.999999999964, -9.09032456791189E-05, 33.4352097448651, 2 ),
      ( 350, 3.43948204020108, 9999.99999961936, -0.000909684827035519, 33.4702092839798, 2 ),
      ( 350, 34.6813234555802, 99999.9958754501, -0.00916318783293538, 33.8246242684408, 2 ),
      ( 350, 381.678010580994, 999999.999263483, -0.099672210443771, 37.9094626761555, 2 ),
      ( 350, 19318.6375061522, 10000000.0033493, -0.822122383341005, 50.8452579637916, 1 ),
      ( 400, 0.300697994608449, 999.995313963891, -5.68341518811031E-05, 35.1606063595005, 2 ),
      ( 400, 3.00851957967283, 9999.99999991041, -0.000568563208166167, 35.1770256699112, 2 ),
      ( 400, 30.2406963841229, 99999.9988655715, -0.00570773627484907, 35.3425056994787, 2 ),
      ( 400, 319.708820444625, 999999.999987471, -0.059516393088707, 37.1412158014568, 2 ),
      ( 400, 16300.8650351644, 9999999.99999835, -0.815542976422299, 51.2041172828984, 1 ),
      ( 500, 0.240550932427998, 999.992856118801, -2.58103067316407E-05, 38.2380693196888, 2 ),
      ( 500, 2.40606832655483, 9999.9999999934, -0.0002581346693435, 38.2428117455766, 2 ),
      ( 500, 24.1167980748154, 99999.9999329445, -0.00258432754910073, 38.2903520066864, 2 ),
      ( 500, 247.002945013561, 999999.999999991, -0.0261463332872983, 38.777803364301, 2 ),
      ( 500, 3408.19002991555, 10000000.0000012, -0.29421563475105, 45.3135891236976, 2 ),
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
      ( 250, 0.481169186066442, 999.999999801473, -0.000168592881433211, 28.1897845872368, 2 ),
      ( 250, 4.81901726679426, 9999.99793517334, -0.00168843998420782, 28.2584694228508, 2 ),
      ( 250, 48.948104238925, 99999.9996350735, -0.017146686519714, 28.9623369579946, 2 ),
      ( 300, 0.400942301784909, 999.999999967617, -8.87446373411452E-05, 30.2792584755294, 2 ),
      ( 300, 4.01263063531195, 9999.99966929782, -0.000888053862890529, 30.3072071697481, 2 ),
      ( 300, 40.4524109978583, 99999.9999992753, -0.00894233377952254, 30.5900609510004, 2 ),
      ( 300, 21537.923055576, 10000000.0000075, -0.813860083318442, 46.7565356563234, 1 ),
      ( 350, 0.34365238956362, 999.99999999494, -5.25469152353576E-05, 32.2580104158866, 2 ),
      ( 350, 3.43815057446959, 9999.99994876448, -0.00052564816890173, 32.2704429875814, 2 ),
      ( 350, 34.5456452749846, 99999.9999999916, -0.00527453180981891, 32.3955935260999, 2 ),
      ( 350, 363.524863364977, 1000000, -0.0547157393588566, 33.7405051468125, 2 ),
      ( 350, 17343.1211475789, 10000000.0000007, -0.801861309295746, 46.7660677201617, 1 ),
      ( 400, 0.300690051510682, 999.999999999943, -3.32943551865163E-05, 34.0908609121873, 2 ),
      ( 400, 3.00780199953522, 9999.99999175276, -0.000332999725868469, 34.0971613532657, 2 ),
      ( 400, 30.1686356422139, 99999.9999999999, -0.0033356370586866, 34.1603909186195, 2 ),
      ( 400, 311.24482725467, 1000000.00000197, -0.0339436549631101, 34.8163825268587, 2 ),
      ( 400, 5341.44170262319, 9999999.99999998, -0.43708074155028, 45.9115418239524, 2 ),
      ( 500, 0.240547566227627, 999.999999999942, -1.46916646799861E-05, 37.2788093011235, 2 ),
      ( 500, 2.40579377506225, 9999.99999941777, -0.000146917508659769, 37.2812039143866, 2 ),
      ( 500, 24.0897973891039, 99999.9941120291, -0.0014692597072702, 37.3051681168239, 2 ),
      ( 500, 244.132639305306, 999999.999968258, -0.0146994155798202, 37.5465695748551, 2 ),
      ( 500, 2810.04150368139, 9999999.9995726, -0.143984059061295, 40.0559709572198, 2 ),
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
      ( 250, 0.481129331831824, 999.999999866538, -8.86465818060529E-05, 26.5294458051744, 2 ),
      ( 250, 4.81513812464237, 9999.9986283161, -0.000887058757890923, 26.5510874339885, 2 ),
      ( 250, 48.5421965447352, 99999.9999851745, -0.00893095970730514, 26.7699767042985, 2 ),
      ( 300, 0.400925085172231, 999.99999999515, -4.86808433756732E-05, 28.9147839574202, 2 ),
      ( 300, 4.01100885310605, 9999.99985581291, -0.000486953566988648, 28.9242380506902, 2 ),
      ( 300, 40.2873264433531, 99999.9999999233, -0.00488415786500636, 29.0192834041869, 2 ),
      ( 300, 422.1936763216, 999999.999999996, -0.0504226133989508, 30.0270806301079, 2 ),
      ( 300, 18233.0221856708, 9999999.99964815, -0.780121164934956, 41.790955702689, 1 ),
      ( 350, 0.343643323130162, 999.999999997479, -2.90397051274535E-05, 31.0824699421835, 2 ),
      ( 350, 3.43733174769616, 9999.99997458878, -0.000290431495412697, 31.0873547856516, 2 ),
      ( 350, 34.4635464966894, 99999.9999999989, -0.00290777136329424, 31.1363378681572, 2 ),
      ( 350, 354.055104106109, 1000000.00002108, -0.0294354188262723, 31.639951801231, 2 ),
      ( 350, 5204.16884970143, 9999999.99983978, -0.33969601341371, 38.3474722592512, 2 ),
      ( 400, 0.3006846318715, 999.999999999517, -1.81453263867228E-05, 33.0218469136138, 2 ),
      ( 400, 3.00733745799395, 9999.99999516295, -0.00018145602019982, 33.0246756662206, 2 ),
      ( 400, 30.1225851359526, 100000, -0.00181483596438998, 33.0530043247094, 2 ),
      ( 400, 306.245444865288, 999999.997054686, -0.0181758425846934, 33.3403289821925, 2 ),
      ( 400, 3671.58252424132, 9999999.99669028, -0.18106382224439, 36.4781295369711, 2 ),
      ( 500, 0.240545120836451, 999.999999999979, -7.40050711558138E-06, 36.3196709527937, 2 ),
      ( 500, 2.40561141398403, 9999.99999978276, -7.39966468662093E-05, 36.3208632929886, 2 ),
      ( 500, 24.0721263533561, 99999.9978590322, -0.000739123941746321, 36.3327904557943, 2 ),
      ( 500, 242.313891075197, 999999.999998821, -0.00730684637866772, 36.4523971588808, 2 ),
      ( 500, 2569.53307997262, 9999999.99988818, -0.0638636157068436, 37.6395489278795, 2 ),
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
