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
using NUnit.Framework;

namespace Altaxo.Science.Thermodynamics.Fluids
{

  /// <summary>
  /// Tests and test data for <see cref="Mixture_CO2_Isopentane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_CO2_Isopentane : MixtureTestBase
  {

    public Test_Mixture_CO2_Isopentane()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("124-38-9", 0.5), ("78-78-4", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.48147525923388, 999.999998966556, -0.000802500447090377, 93.9675241547822, 2 ),
      ( 250, 4.85010380702273, 9999.99990372843, -0.00808540527141879, 94.3047720254571, 2 ),
      ( 250, 9188.07285528472, 1000000.00134939, -0.947639849728749, 109.416068341127, 1 ),
      ( 250, 9297.71865486906, 10000000.0000009, -0.482573206416888, 109.818598714727, 1 ),
      ( 250, 10027.0575063537, 100000000.00003, 3.79790681182317, 113.458090958659, 1 ),
      ( 300, 0.40107585417819, 999.999843404812, -0.000420015851146254, 111.141888697736, 2 ),
      ( 300, 4.02604618433124, 9999.99850305475, -0.00421560548155028, 111.284817656976, 2 ),
      ( 300, 8519.27969286788, 999999.999999682, -0.9529411628226, 123.404139091386, 1 ),
      ( 300, 8683.36790108876, 10000000.0000003, -0.538304261085986, 123.742209057259, 1 ),
      ( 300, 9618.65304508554, 100000000, 3.1680201381249, 127.121837498391, 1 ),
      ( 350, 0.34372054407872, 999.999975880063, -0.00024913632963778, 128.068129875751, 2 ),
      ( 350, 3.44494939610238, 9999.99999998974, -0.00249649184469608, 128.137636572881, 2 ),
      ( 350, 35.2629696593726, 100000.001643096, -0.0255077380895785, 128.854639150642, 2 ),
      ( 350, 8022.39089559313, 10000000.0054538, -0.571655239125589, 138.457433537522, 1 ),
      ( 350, 9230.94745935733, 100000000.001011, 2.72263965663586, 141.561192686802, 1 ),
      ( 400, 0.300728786183702, 999.999995012904, -0.000160407762848656, 144.463488882999, 2 ),
      ( 400, 3.01164224697022, 9999.99999999978, -0.00160602656445005, 144.500817529807, 2 ),
      ( 400, 30.565071520455, 100000.000154737, -0.01626094104631, 144.880666431353, 2 ),
      ( 400, 372.156024721304, 999999.999116711, -0.19205782774404, 150.166285738523, 2 ),
      ( 400, 7276.4541981434, 9999999.99999986, -0.586776005505494, 153.404245200327, 1 ),
      ( 400, 8860.70643530365, 100000000.000225, 2.39341506399817, 156.095983005322, 1 ),
      ( 500, 0.240562991267321, 999.999999635088, -7.71261859543958E-05, 174.798219637464, 2 ),
      ( 500, 2.4073017675491, 9999.99624977952, -0.000771565602569134, 174.811590348053, 2 ),
      ( 500, 24.2422328356972, 99999.9999044695, -0.00774636059216114, 174.946394312358, 2 ),
      ( 500, 261.7054689968, 1000000.00000352, -0.0808581934366109, 176.417889744357, 2 ),
      ( 500, 5266.8577416742, 10000000.0010534, -0.543286624822239, 183.008205734202, 1 ),
      ( 500, 8169.28065061848, 99999999.9999996, 1.94449961812494, 183.832614421495, 1 ),
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
      ( 250, 0.481233173855406, 999.999995222738, -0.000302131917817555, 60.2719553315625, 2 ),
      ( 250, 4.82549525665396, 9999.99997489113, -0.00302921799376517, 60.3715847331429, 2 ),
      ( 300, 0.400972233207665, 999.999996365447, -0.000163980381935954, 70.0643906827308, 2 ),
      ( 300, 4.01565815343342, 9999.99999999411, -0.00164190698090837, 70.1059202767512, 2 ),
      ( 300, 40.7689041272004, 100000.000030846, -0.0166366003498331, 70.529771018398, 2 ),
      ( 300, 11770.1319276022, 10000000.0002176, -0.659386586232617, 82.6155670422472, 1 ),
      ( 350, 0.343668112119929, 999.999999236907, -9.88890427629178E-05, 79.6209115829803, 2 ),
      ( 350, 3.43974506747305, 9999.99204273265, -0.00098954823757152, 79.6412905590569, 2 ),
      ( 350, 34.7092001150167, 99999.9911102422, -0.00996241241314117, 79.8473003475637, 2 ),
      ( 350, 10075.0120847922, 10000000.0001652, -0.658924352425301, 89.9656488442462, 1 ),
      ( 400, 0.300698996413544, 999.999999763006, -6.36357060013584E-05, 88.7965176251662, 2 ),
      ( 400, 3.00871385763144, 9999.99757170893, -0.000636566004804508, 88.8077306495573, 2 ),
      ( 400, 30.2612571214261, 99999.9999693547, -0.0063867471512417, 88.9206011300104, 2 ),
      ( 400, 321.984462208506, 1000000.00054049, -0.0661665499439443, 90.1311338603175, 2 ),
      ( 400, 7606.61508434632, 10000000.0000373, -0.604712664060005, 98.7938713537988, 1 ),
      ( 500, 0.240550933999714, 999.999999992809, -2.92870333513436E-05, 105.627122084355, 2 ),
      ( 500, 2.4061435593785, 9999.9999277486, -0.000292862662237748, 105.631436406729, 2 ),
      ( 500, 24.1250234815345, 99999.999999992, -0.00292785554466879, 105.674701254216, 2 ),
      ( 500, 247.777862790039, 1000000.00001733, -0.0291953999934496, 106.119001868203, 2 ),
      ( 500, 3213.68390268508, 9999999.99999999, -0.251501092638548, 110.572160333499, 2 ),
      ( 500, 11027.2635622109, 99999999.9922955, 1.18135612343894, 113.236907713368, 1 ),
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
      ( 250, 0.481129395267146, 999.999999865158, -8.87796073987143E-05, 26.5936054921311, 2 ),
      ( 250, 4.8151445384132, 9999.99861401929, -0.000888390764153728, 26.615265424789, 2 ),
      ( 250, 48.542857719577, 99999.9999848091, -0.00894445967814786, 26.834342816709, 2 ),
      ( 300, 0.400925114825412, 999.999999995099, -4.87559923017044E-05, 28.9943526243723, 2 ),
      ( 300, 4.01101186578411, 9999.99985430102, -0.000487705493018599, 29.0038178075829, 2 ),
      ( 300, 40.2876326037912, 99999.9999999216, -0.00489172129858166, 29.0989752446853, 2 ),
      ( 300, 422.229515924023, 999999.999999997, -0.05050321636979, 30.1080404719765, 2 ),
      ( 300, 18204.1672151629, 9999999.99963852, -0.779772640750925, 41.8569860020988, 1 ),
      ( 300, 25621.321155158, 100000000.01987, 0.564734171852273, 41.6899573524069, 1 ),
      ( 350, 0.343643338401746, 999.999999997452, -2.90853347079416E-05, 31.177194941067, 2 ),
      ( 350, 3.43733331281649, 9999.99997432112, -0.000290887883317546, 31.1820864917237, 2 ),
      ( 350, 34.4637045227081, 99999.9999999988, -0.00291234450575244, 31.2311370461908, 2 ),
      ( 350, 354.072144135074, 1000000.00002191, -0.0294821292439648, 31.735466550147, 2 ),
      ( 350, 5208.63018569656, 9999999.99986251, -0.340261582850237, 38.453354865519, 2 ),
      ( 350, 23317.2163972356, 99999999.9999995, 0.473732273896174, 40.8842369429025, 1 ),
      ( 400, 0.300684640241811, 999.999999999515, -1.8174354054696E-05, 33.1312578240693, 2 ),
      ( 400, 3.00733832750685, 9999.9999951117, -0.000181746288596785, 33.1340905321575, 2 ),
      ( 400, 30.1226726706821, 100000, -0.00181773782076452, 33.1624588996196, 2 ),
      ( 400, 306.25446999116, 999999.996987793, -0.0182047774904287, 33.4501955244027, 2 ),
      ( 400, 3672.72666357786, 9999999.99654518, -0.181318940784817, 36.5925469551495, 2 ),
      ( 400, 21166.8794768591, 100000000.004933, 0.420517255896671, 40.736357544166, 1 ),
      ( 500, 0.240545123537187, 999.999999999979, -7.41292527780064E-06, 36.4563676184374, 2 ),
      ( 500, 2.4056117097755, 9999.99999978041, -7.41207873105395E-05, 36.4575613614674, 2 ),
      ( 500, 24.0721561309202, 99999.9978361219, -0.000740361229802167, 36.4695025803786, 2 ),
      ( 500, 242.31681084061, 999999.999998798, -0.00731880888947247, 36.5892523823994, 2 ),
      ( 500, 2569.74192547854, 9999999.99987927, -0.0639396975650463, 37.7778386263548, 2 ),
      ( 500, 17508.561379754, 99999999.9999993, 0.373861250942923, 41.4258465344214, 1 ),
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
