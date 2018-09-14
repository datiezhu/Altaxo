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
  /// Tests and test data for <see cref="Mixture_Butane_H2S"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Butane_H2S : MixtureTestBase
  {

    public Test_Mixture_Butane_H2S()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("106-97-8", 0.5), ("7783-06-4", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601526863592296, 999.999999783556, -0.000275576953538051, 25.1167037786379, 2 ),
      ( 200, 6.0302702332076, 9999.9977181581, -0.00276260635237268, 25.2082503446376, 2 ),
      ( 200, 28473.5774935081, 99999.9999978296, -0.997888003023309, 43.0810010077381, 1 ),
      ( 200, 28495.6920590765, 1000000.0016738, -0.978896420682318, 43.0965065281009, 1 ),
      ( 200, 28708.8479393913, 9999999.99999978, -0.790531094136162, 43.2506952896259, 1 ),
      ( 200, 30328.9538516001, 100000000.000005, 0.982795382901094, 44.6665350251698, 1 ),
      ( 250, 0.481152142225362, 999.999999994891, -0.000131486277281628, 25.3905054506387, 2 ),
      ( 250, 4.81722995619842, 9999.99994764135, -0.00131635462846563, 25.4236815704713, 2 ),
      ( 250, 48.7582451374073, 99999.9999999706, -0.0133178994330033, 25.7607720928703, 2 ),
      ( 250, 25830.2694623677, 1000000.00362207, -0.981374995729846, 39.0874070071435, 1 ),
      ( 250, 26185.153502629, 10000000.0000017, -0.816274180988764, 39.2331209370559, 1 ),
      ( 250, 28501.1887940037, 99999999.9999966, 0.687960740158928, 40.6323319166451, 1 ),
      ( 300, 0.40093689037769, 999.99999999839, -7.35592323536752E-05, 25.8816687750244, 2 ),
      ( 300, 4.01202696644064, 9999.9999836685, -0.000736034124915401, 25.8963532035511, 2 ),
      ( 300, 40.3898323948556, 99999.9999999987, -0.00740514622053443, 26.0443273096294, 2 ),
      ( 300, 435.433826811709, 999999.999999983, -0.0792920230766239, 27.6935166974406, 2 ),
      ( 300, 23211.7207141457, 10000000.0016968, -0.82728234465212, 36.8409489468035, 1 ),
      ( 300, 26692.54924302, 100000000.000876, 0.501944958950814, 38.1804828316083, 1 ),
      ( 350, 0.343650460901786, 999.999999999985, -4.5245187028616E-05, 26.5421176160633, 2 ),
      ( 350, 3.43790513429014, 9999.99999785154, -0.000452604276400461, 26.5496077186173, 2 ),
      ( 350, 34.5202614261921, 100000, -0.00454139632259893, 26.6248554901374, 2 ),
      ( 350, 360.610197716504, 999999.990292423, -0.0470737801644456, 27.4167196714143, 2 ),
      ( 350, 18953.6259088986, 9999999.99999995, -0.818697006036683, 35.9288394785927, 1 ),
      ( 350, 24888.0168628006, 100000000.010504, 0.380724363448763, 36.6953303256874, 1 ),
      ( 400, 0.300689428856223, 999.999999999946, -2.95338961459112E-05, 27.3104916863192, 2 ),
      ( 400, 3.00769393870593, 9999.99999945583, -0.000295394234050422, 27.314746016525, 2 ),
      ( 400, 30.1573051509953, 99999.9943289976, -0.00295949232642574, 27.35741873132, 2 ),
      ( 400, 310.035520280197, 999999.999876139, -0.0301738715122038, 27.7976536549801, 2 ),
      ( 400, 5052.52748669631, 9999999.98353665, -0.40489082024526, 34.7537791436692, 2 ),
      ( 400, 23095.4673857444, 99999999.9999987, 0.301902850909393, 35.8324247889002, 1 ),
      ( 500, 0.240547781313821, 999.997871304152, -1.38959752603033E-05, 29.0151180987796, 2 ),
      ( 500, 2.40577870293099, 9999.9999999997, -0.000138963872157849, 29.0168625420497, 2 ),
      ( 500, 24.0879265974263, 99999.9999968794, -0.00139002131246824, 29.0343332694639, 2 ),
      ( 500, 243.944688377906, 1000000, -0.013938609361935, 29.2116453588553, 2 ),
      ( 500, 2804.15571347595, 10000000, -0.14218587254372, 31.1940325715437, 2 ),
      ( 500, 19662.7764918282, 100000000.000102, 0.223349300445084, 35.2375790621248, 1 ),
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
      ( 200, 0.601697085722943, 999.997075901037, -0.000558400723678533, 46.6185966450222, 2 ),
      ( 200, 16492.204700584, 99999.9999988866, -0.996353664617933, 63.509247562806, 1 ),
      ( 200, 16508.6679891458, 1000000.00000173, -0.963573009226132, 63.5401567835667, 1 ),
      ( 200, 16665.5574105594, 10000000.0000307, -0.639159326127518, 63.8433042290494, 1 ),
      ( 200, 17789.0651526122, 100000000.016154, 2.38050983334126, 66.3776775900402, 1 ),
      ( 250, 0.481214775309449, 999.99999941975, -0.000261625358281142, 51.9399295612391, 2 ),
      ( 250, 4.82353930141097, 9999.99384567764, -0.00262266348055483, 52.0116892215306, 2 ),
      ( 250, 15072.3693595404, 999999.999999534, -0.968081403410501, 64.6020475096469, 1 ),
      ( 250, 15325.8567130221, 10000000.0000006, -0.686093321679965, 64.8979718992756, 1 ),
      ( 250, 16863.1253699969, 100000000.000031, 1.85290458776681, 67.4439806753659, 1 ),
      ( 300, 0.400965185514091, 999.999999932351, -0.000144121605472187, 58.2282136155765, 2 ),
      ( 300, 4.01486734185326, 9999.99930263444, -0.00144297763160696, 58.2601526400322, 2 ),
      ( 300, 40.6851859615231, 99999.9999934115, -0.0146108754489747, 58.5867437603182, 2 ),
      ( 300, 13827.7919468857, 10000000.0026917, -0.710071282955517, 68.3561070417717, 1 ),
      ( 300, 15978.5649193126, 100000000.00031, 1.50903256827763, 70.8583378874348, 1 ),
      ( 350, 0.343664982047612, 999.999999988794, -8.74970589734453E-05, 65.0154266020394, 2 ),
      ( 350, 3.43936048735327, 9999.99988586668, -0.00087555916437947, 65.0317677075322, 2 ),
      ( 350, 34.6691101610117, 99999.9999999203, -0.00881530900168102, 65.1970821081357, 2 ),
      ( 350, 379.783239160113, 999999.997830316, -0.0951814692551721, 67.0855521589701, 2 ),
      ( 350, 11983.9537750039, 10000000.0138463, -0.713254140276621, 73.4291230175191, 1 ),
      ( 350, 15128.6901633193, 100000000.002616, 1.27141218883243, 75.5751290723546, 1 ),
      ( 400, 0.300697526177373, 999.999999999868, -5.64615603250295E-05, 71.8390003725498, 2 ),
      ( 400, 3.00850478301465, 9999.99998088749, -0.00056483199305427, 71.8482841294583, 2 ),
      ( 400, 30.2395167810494, 99999.9999999981, -0.00567012854686573, 71.9417166469919, 2 ),
      ( 400, 319.556315792517, 1000000.00047639, -0.0590686728543268, 72.9412365915707, 2 ),
      ( 400, 9190.35672757049, 10000000.0034269, -0.672830383736444, 80.4362146267906, 1 ),
      ( 400, 14312.388188931, 100000000.015435, 1.10084120411564, 80.843666221208, 1 ),
      ( 500, 0.240550692617406, 999.99999999985, -2.59984980752422E-05, 84.6212612896929, 2 ),
      ( 500, 2.40606998913265, 9999.99999849571, -0.000260010111304089, 84.6250443896468, 2 ),
      ( 500, 24.1172116547553, 99999.9845024182, -0.00260261342654226, 84.6629514963421, 2 ),
      ( 500, 247.035559558651, 999999.999404611, -0.0262760588218799, 85.0495314859005, 2 ),
      ( 500, 3281.18837386737, 9999999.99969227, -0.266898418333094, 89.1137027485053, 2 ),
      ( 500, 12791.8484551913, 99999999.999999, 0.880450972377388, 91.4715252937818, 1 ),
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
      ( 200, 0.602050607104946, 999.999999561399, -0.00114526992350148, 68.1371203556061, 2 ),
      ( 200, 12446.0023151727, 100000000.003679, 3.83176108636574, 89.4790643572986, 1 ),
      ( 250, 0.481339641604712, 999.99998724638, -0.000520971593546555, 78.495405278059, 2 ),
      ( 250, 4.83622466485102, 9999.99691450268, -0.0052387730894803, 78.6616696110946, 2 ),
      ( 250, 10758.0638886645, 99999.9999985623, -0.995528109152804, 91.3770924077037, 1 ),
      ( 250, 10774.1192701075, 1000000.00072345, -0.955347730486095, 91.4220326438271, 1 ),
      ( 250, 10923.4725785716, 9999999.99999819, -0.559582473562528, 91.8611234388437, 1 ),
      ( 250, 11883.0927349911, 99999999.999999, 3.04851571935262, 95.3630218350349, 1 ),
      ( 300, 0.40102018600524, 999.999998586651, -0.000281253266551583, 90.5774403371417, 2 ),
      ( 300, 4.02041119330608, 9999.9848220086, -0.00281991015513271, 90.6510690252452, 2 ),
      ( 300, 41.2872574527918, 100000.00693791, -0.0289803158519811, 91.4320270649622, 2 ),
      ( 300, 9844.84543691375, 1000000.00000074, -0.959277430982836, 100.478256523663, 1 ),
      ( 300, 10085.7240566418, 10000000.001256, -0.602500132302943, 100.891860192815, 1 ),
      ( 300, 11351.41929595, 100000000.000002, 2.53178212623034, 104.440073611011, 1 ),
      ( 350, 0.343692879116088, 999.999999771981, -0.000168658552988602, 103.490093184082, 2 ),
      ( 350, 3.44216280255415, 9999.99762977804, -0.00168896062739138, 103.527580119765, 2 ),
      ( 350, 34.9625569083666, 99999.9998890026, -0.0171344925378292, 103.913760370254, 2 ),
      ( 350, 8697.46512512339, 1000000.00006152, -0.960490222444154, 111.651256914521, 1 ),
      ( 350, 9151.47610798206, 10000000.0000001, -0.624503295077937, 111.742516898249, 1 ),
      ( 350, 10847.3166936394, 100000000.000045, 2.16792550710053, 115.195179774032, 1 ),
      ( 400, 0.300713083979834, 999.999999997327, -0.00010819500616334, 116.368269925749, 2 ),
      ( 400, 3.0100648898584, 9999.99961008581, -0.00108283594909298, 116.389454126445, 2 ),
      ( 400, 30.3999763123678, 99999.9999980912, -0.0109184779673491, 116.604708887473, 2 ),
      ( 400, 341.743914289059, 999999.99822898, -0.120158294455535, 119.182678345259, 2 ),
      ( 400, 8036.24555312367, 10000000.0000073, -0.625844498729661, 123.485900533893, 1 ),
      ( 400, 10368.4185623471, 100000000.000365, 1.89996537580753, 126.457875403447, 1 ),
      ( 500, 0.240556476623701, 999.999999996907, -5.00421488674892E-05, 140.227692723269, 2 ),
      ( 500, 2.40664908406891, 9999.99996873787, -0.000500570467953894, 140.236111083564, 2 ),
      ( 500, 24.1758224011096, 99999.999999997, -0.00502065792879737, 140.320661561188, 2 ),
      ( 500, 253.673326668247, 1000000.00035852, -0.0517550985153342, 141.204487235187, 2 ),
      ( 500, 4605.65390375437, 10000000, -0.477719247500113, 147.721957165069, 1 ),
      ( 500, 9482.01628814244, 100000000.009589, 1.53684903474119, 148.090932298219, 1 ),
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
