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
	/// Tests and test data for <see cref="Mixture_Ethane_Nonane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Ethane_Nonane : MixtureTestBase
    {

    public Test_Mixture_Ethane_Nonane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-84-0", 0.5), ("111-84-2", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 5871.09572688818, 1000000.00176603, -0.918058076407474, 207.611276900393, 1 ),
      ( 250, 5914.27523871609, 9999999.99999889, -0.186563259459171, 208.30498064564, 1 ),
      ( 250, 6238.06552070379, 100000000.009598, 6.71214851412942, 214.328812110014, 1 ),
      ( 300, 5565.19240126953, 1000000.00000393, -0.927961628589486, 229.210263011371, 1 ),
      ( 300, 6017.8395946043, 100000000.000024, 5.66198211941932, 235.432388757479, 1 ),
      ( 350, 0.344026094649436, 999.999997060014, -0.00113707152409933, 231.789958835892, 2 ),
      ( 350, 5255.42313653752, 1000000.00014248, -0.934613274123522, 253.744165209089, 1 ),
      ( 350, 5332.29767239191, 10000000.0000102, -0.355559397701477, 254.35455442758, 1 ),
      ( 350, 5813.35561107615, 100000000.000029, 4.91112836307094, 259.594043128583, 1 ),
      ( 400, 0.300883817964922, 999.999261774553, -0.000675574679490814, 260.286741369959, 2 ),
      ( 400, 3.02738198369278, 9999.99362687348, -0.00679679245889988, 260.59138937537, 2 ),
      ( 400, 4930.00656190946, 1000000.00000074, -0.939010112019073, 279.042826855488, 1 ),
      ( 400, 5036.90407054646, 10000000.0001571, -0.403044917840139, 279.583264155115, 1 ),
      ( 400, 5621.03646620507, 100000000.000004, 4.34920116839001, 284.5312774895, 1 ),
      ( 500, 0.240614444769809, 999.999962110497, -0.000290947231095863, 312.930059605809, 2 ),
      ( 500, 2.41248037606705, 9999.99999996898, -0.00291649603365336, 313.038196540716, 2 ),
      ( 500, 24.7959856332555, 100000.003012795, -0.029905719463688, 314.153698345849, 2 ),
      ( 500, 4147.35125519248, 1000000.00000022, -0.942000466357567, 327.848176854986, 1 ),
      ( 500, 4400.81177542026, 10000000.0000011, -0.453408936951725, 327.770496547629, 1 ),
      ( 500, 5265.72891563659, 100000000.002034, 3.56811283905666, 331.977265822769, 1 ),
      ( 600, 0.200483305257317, 999.999996475847, -0.000147675006000657, 357.619750384095, 2 ),
      ( 600, 2.00750448419859, 9999.99999999988, -0.00147820111067697, 357.666505839062, 2 ),
      ( 600, 20.3491893991353, 100000.00003122, -0.0149302997193762, 358.141365945207, 2 ),
      ( 600, 241.049715404538, 999999.9999955, -0.168413459655889, 363.834908837728, 2 ),
      ( 600, 3646.55054089427, 10000000.0004011, -0.450292278571758, 370.357967976722, 1 ),
      ( 600, 4944.30111201788, 100000000.017416, 3.05423727987314, 373.10586946269, 1 ),
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
      ( 300, 0.401100930499131, 999.994435204295, -0.000482501133776985, 123.781789650574, 2 ),
      ( 350, 0.343732248371015, 999.987779136841, -0.000283170430474851, 141.291045859192, 2 ),
      ( 350, 3.44613240009249, 9999.99997547266, -0.00283891481595809, 141.371683027122, 2 ),
      ( 500, 0.240564508369635, 999.999999821756, -8.34275557936415E-05, 191.383384337939, 2 ),
      ( 500, 2.40745367912124, 9999.99817791302, -0.000834613004288221, 191.400367535789, 2 ),
      ( 500, 24.2577294609326, 99999.9998773461, -0.00838023987545597, 191.571886520832, 2 ),
      ( 500, 263.636322482922, 999999.999999922, -0.0875899178260168, 193.470148592973, 2 ),
      ( 500, 7388.2184190259, 100000000, 2.25578407429364, 202.185866275019, 1 ),
      ( 600, 0.200462282756199, 999.999999960296, -4.28203858340865E-05, 219.376249598729, 2 ),
      ( 600, 2.00539567479945, 9999.99960101964, -0.000428187782040223, 219.384279852518, 2 ),
      ( 600, 20.1315380121274, 99999.9999996522, -0.00428025535713291, 219.464905507956, 2 ),
      ( 600, 209.376676558067, 1000000.00000134, -0.0426168655499633, 220.301801633454, 2 ),
      ( 600, 2833.79536819641, 10000000.0011899, -0.292631708224009, 226.548427993367, 1 ),
      ( 600, 6847.74158818208, 99999999.9819142, 1.92729648561567, 228.037585369153, 1 ),
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
      ( 250, 0.481150289436238, 999.999983013405, -0.000127636032168227, 38.8395105364914, 2 ),
      ( 300, 0.400936897624105, 999.999999940571, -7.35773047229841E-05, 44.5428201869077, 2 ),
      ( 300, 4.01202727864376, 9999.9993939001, -0.000736111841002079, 44.5509997727546, 2 ),
      ( 300, 40.3894354326013, 99999.9999980481, -0.0073953906334364, 44.6345448942305, 2 ),
      ( 300, 434.745723474825, 999999.999999794, -0.0778347523157744, 45.6502337252941, 2 ),
      ( 300, 12670.2521090332, 10000000.0000025, -0.683583724839957, 50.9628355829649, 1 ),
      ( 300, 17316.5300116193, 99999999.9998938, 1.31517167410833, 52.7628235025119, 1 ),
      ( 350, 0.343650484022135, 999.999999984596, -4.53124626573498E-05, 50.8373096816847, 2 ),
      ( 350, 3.43790724625957, 9999.99984412657, -0.000453218309715961, 50.8421818388114, 2 ),
      ( 350, 34.520268371398, 99999.9999999342, -0.00454159660115111, 50.8911910119576, 2 ),
      ( 350, 360.356936256236, 1000000.01752681, -0.0464040580372435, 51.4120419185827, 2 ),
      ( 350, 7272.67046973035, 10000000, -0.527498305054992, 58.0069068732101, 1 ),
      ( 350, 16222.92256664, 99999999.9996303, 1.11820595801891, 57.6037325658847, 1 ),
      ( 400, 0.300689280876456, 999.999999996161, -2.90417755223895E-05, 57.3215666383369, 2 ),
      ( 400, 3.00767902859685, 9999.99996134433, -0.000290438350157565, 57.3247596410211, 2 ),
      ( 400, 30.1557004723786, 99999.9999999977, -0.00290643686837934, 57.3567366760093, 2 ),
      ( 400, 309.744468272223, 1000000.00002363, -0.0292625724589653, 57.6813739317078, 2 ),
      ( 400, 4206.91603061946, 10000000, -0.285270858421235, 61.1114083209581, 2 ),
      ( 400, 15194.4602309001, 99999999.9999958, 0.978882722759523, 63.0081583624578, 1 ),
      ( 500, 0.240547451930307, 999.999999999701, -1.25267160221795E-05, 69.8451020704853, 2 ),
      ( 500, 2.40574572174349, 9999.99999701315, -0.000125256436625318, 69.8467563024471, 2 ),
      ( 500, 24.0845853328632, 99999.9999999999, -0.00125148373403898, 69.8632873079104, 2 ),
      ( 500, 243.564231390176, 999999.999740121, -0.0123983423652861, 70.027414821895, 2 ),
      ( 500, 2681.01754360557, 10000000, -0.102786778719878, 71.4781774598089, 2 ),
      ( 500, 13355.3833381068, 100000000.001281, 0.801104712403807, 74.0839390037303, 1 ),
      ( 600, 0.200454727327718, 999.999999999986, -5.13055404400372E-06, 81.1362639874036, 2 ),
      ( 600, 2.00463981435523, 9999.9999998535, -5.12937613207195E-05, 81.137271192277, 2 ),
      ( 600, 20.0556334784191, 99999.9986040914, -0.00051175595605771, 81.1473325450047, 2 ),
      ( 600, 201.460129576155, 999999.999999889, -0.00499568172724424, 81.246860610327, 2 ),
      ( 600, 2076.18909614869, 10000000.0000002, -0.0345113590291691, 82.1178480635357, 1 ),
      ( 600, 11820.9993422352, 100000000.002722, 0.695742407928407, 84.4716521207123, 1 ),
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
