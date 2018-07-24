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
	/// Tests and test data for <see cref="Mixture_CO2_Propane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_CO2_Propane : MixtureTestBase
    {

    public Test_Mixture_CO2_Propane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("124-38-9", 0.5), ("74-98-6", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481221117210883, 999.999973932598, -0.000274805240399795, 55.9559734282786, 2 ),
      ( 250, 4.82417874150934, 9999.99958132783, -0.00275487091519345, 56.0174758293782, 2 ),
      ( 250, 49.50849805974, 100000.003670117, -0.0282701082688918, 56.6616875768068, 2 ),
      ( 250, 12692.5101941896, 1000000.00000037, -0.962096632757218, 66.0321420677097, 1 ),
      ( 250, 12956.5008966465, 10000000.0000203, -0.628689197061352, 66.36468227794, 1 ),
      ( 250, 14440.9033802952, 99999999.9999973, 2.33143199184528, 69.560903268162, 1 ),
      ( 300, 0.400969296032688, 999.999996755322, -0.00015437615192176, 65.3504054931465, 2 ),
      ( 300, 4.01528009083266, 9999.9999999999, -0.00154562853251028, 65.3799914449042, 2 ),
      ( 300, 40.7280701053453, 100000.000159233, -0.0156484338976339, 65.683155262882, 2 ),
      ( 300, 11629.9403138309, 10000000.0101172, -0.655279919324309, 73.7278740830162, 1 ),
      ( 300, 13698.3133568182, 100000000.000045, 1.926691669942, 76.5820397929629, 1 ),
      ( 350, 0.343667584113314, 999.99999996786, -9.50724273763544E-05, 75.3040626134624, 2 ),
      ( 350, 3.43962126160113, 9999.99525079744, -0.0009513117223641, 75.3198371347333, 2 ),
      ( 350, 34.6956277688343, 99999.9996139844, -0.00957286847004622, 75.4798823947197, 2 ),
      ( 350, 383.045786111344, 999999.999999998, -0.102888157861122, 77.3571138684392, 2 ),
      ( 350, 9992.8958947219, 10000000.0014648, -0.656120793744669, 82.7103970811536, 1 ),
      ( 350, 12992.0917969943, 99999999.9999976, 1.6449544551509, 84.8052342158578, 1 ),
      ( 400, 0.300699233983379, 999.999999877258, -6.21452588493701E-05, 85.2468814257848, 2 ),
      ( 400, 3.00867579608203, 9999.99874806118, -0.000621644357172221, 85.2554852753928, 2 ),
      ( 400, 30.2567297737814, 99999.9999929002, -0.006235805340918, 85.3423020201434, 2 ),
      ( 400, 321.407329446047, 1000000.01588801, -0.064487586118864, 86.2951288105698, 2 ),
      ( 400, 7581.3804959418, 10000000.0007775, -0.603396047560203, 93.0377503337337, 1 ),
      ( 400, 12321.8183781058, 99999999.9999983, 1.44022868804744, 93.3865178063621, 1 ),
      ( 500, 0.240551461477224, 999.999999991602, -2.91992235886547E-05, 103.789480500236, 2 ),
      ( 500, 2.40614696154908, 9999.99991554478, -0.00029199626672813, 103.792520130032, 2 ),
      ( 500, 24.124897086631, 99999.9999999893, -0.00292035775849151, 103.822985120399, 2 ),
      ( 500, 247.788121922667, 1000000.00002228, -0.0292333801366518, 104.134653219827, 2 ),
      ( 500, 3248.08781528605, 9999999.99999998, -0.259427542478152, 107.435214537434, 1 ),
      ( 500, 11094.6727636552, 99999999.9999984, 1.16810754751872, 109.877350697647, 1 ),
      ( 600, 0.200456539789071, 999.999999999316, -1.41767452938397E-05, 119.924932857826, 2 ),
      ( 600, 2.00482115768425, 9999.99999313898, -0.000141747310141833, 119.926458639959, 2 ),
      ( 600, 20.0737832484021, 100000, -0.00141545075299887, 119.941703725139, 2 ),
      ( 600, 203.288323187912, 999999.99935842, -0.0139438663958144, 120.092852623248, 2 ),
      ( 600, 2245.17776492205, 9999999.99999999, -0.10718117246844, 121.41728943007, 2 ),
      ( 600, 10027.2886672856, 100000000.010824, 0.99908175215007, 124.594844586205, 1 ),
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
      ( 250, 0.481161193687643, 999.999997985617, -0.000152580331671931, 41.2551279420537, 2 ),
      ( 250, 4.81823842587307, 9999.99999999997, -0.00152766336766351, 41.2904657577368, 2 ),
      ( 250, 48.8646491996429, 100000.000177291, -0.0154686755780535, 41.6520419190369, 2 ),
      ( 300, 0.400940499896635, 999.999999754568, -8.48461783154081E-05, 47.1491077426626, 2 ),
      ( 300, 4.01247115626337, 9999.99747282798, -0.000848937972151053, 47.1650096110942, 2 ),
      ( 300, 40.4358777103475, 99999.9999490929, -0.00853770368859548, 47.3258090656665, 2 ),
      ( 300, 441.058236237155, 999999.999999917, -0.0910350409783136, 49.1590017024428, 2 ),
      ( 300, 13670.1055254495, 10000000.0002728, -0.706727588246534, 57.1141222946249, 1 ),
      ( 300, 17542.4000876054, 100000000.006877, 1.28535707571142, 59.1532702630068, 1 ),
      ( 350, 0.343651791585798, 999.999999997672, -5.14022535699809E-05, 53.2140183110269, 2 ),
      ( 350, 3.43810898061991, 9999.99966349952, -0.000514151668143602, 53.2220468285034, 2 ),
      ( 350, 34.5414574827431, 99999.999999452, -0.0051545239789366, 53.3028248961594, 2 ),
      ( 350, 362.844824765986, 1000000, -0.0529446648966496, 54.1634634365903, 2 ),
      ( 350, 8923.79958087322, 10000000, -0.614923974934317, 63.6804942459314, 1 ),
      ( 350, 16380.8513499539, 100000000.000021, 1.09777941187589, 62.8589504612544, 1 ),
      ( 400, 0.300689703353924, 999.999999991762, -3.27318601370617E-05, 59.1593525509273, 2 ),
      ( 400, 3.00778319541807, 9999.99991693927, -0.000327345137534377, 59.1638073891217, 2 ),
      ( 400, 30.1668159876322, 99999.9999999877, -0.00327611191714056, 59.2084986583457, 2 ),
      ( 400, 310.95149118254, 1000000.00026002, -0.0330329014521344, 59.6698976409586, 2 ),
      ( 400, 4512.05093711253, 9999999.99979442, -0.333607121431568, 65.0302341365816, 2 ),
      ( 400, 15293.163700656, 99999999.9999991, 0.966106340754775, 67.0489357358927, 1 ),
      ( 500, 0.240547274527815, 999.999999999565, -1.40743699342237E-05, 70.087428336668, 2 ),
      ( 500, 2.40577745111753, 9999.99999565771, -0.000140728458632232, 70.0892469419033, 2 ),
      ( 500, 24.0882511258869, 100000, -0.00140575702436649, 70.1074387106874, 2 ),
      ( 500, 243.934824277638, 999999.999372346, -0.0139009889592071, 70.2898315509877, 2 ),
      ( 500, 2721.36304138153, 9999999.99999999, -0.116090410141044, 72.0344711381019, 2 ),
      ( 500, 13367.5478118164, 100000000.000005, 0.799461594323051, 75.593944560653, 1 ),
      ( 600, 0.200454388429352, 999.999999999977, -5.72507103633326E-06, 79.5109966789218, 2 ),
      ( 600, 2.00464714429467, 9999.999999792, -5.72350887805963E-05, 79.51197893658, 2 ),
      ( 600, 20.0567722525209, 99999.998034948, -0.000570788299223974, 79.5217956947434, 2 ),
      ( 600, 201.572192331831, 999999.999999764, -0.00555112041766886, 79.6193491107632, 2 ),
      ( 600, 2085.03066413755, 10000000.0000035, -0.0386077084547201, 80.5051679901586, 1 ),
      ( 600, 11786.6547832325, 100000000.001153, 0.700679662734511, 83.5669124777398, 1 ),
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
      ( 250, 0.481129317899971, 999.999999866121, -8.86188183908712E-05, 26.5555797464039, 2 ),
      ( 250, 4.81513677796953, 9999.99862401903, -0.000886780520382396, 26.5771835511298, 2 ),
      ( 250, 48.5420571402961, 99999.999985081, -0.00892811470767252, 26.7956884610941, 2 ),
      ( 250, 532.559253498871, 999999.99720202, -0.0966513532787013, 29.3932896235019, 2 ),
      ( 250, 24429.8233452384, 10000000.0025302, -0.803074023865243, 41.5090493362162, 1 ),
      ( 250, 28044.5629424561, 100000000.000282, 0.715436542108635, 43.5915177142008, 1 ),
      ( 300, 0.400925079787634, 999.999999995132, -4.86686042307624E-05, 28.9485251167826, 2 ),
      ( 300, 4.01100835643885, 9999.99985533861, -0.000486830991354791, 28.9579676076217, 2 ),
      ( 300, 40.2872760162684, 99999.9999999229, -0.00488291347571664, 29.0528960356948, 2 ),
      ( 300, 422.187202244122, 999999.999999995, -0.0504080531276177, 30.0594024169516, 2 ),
      ( 300, 18194.9451722495, 9999999.99978925, -0.779661019298631, 41.8217798394697, 1 ),
      ( 300, 25637.1600308338, 100000000.019962, 0.563767464543014, 41.6344096880213, 1 ),
      ( 350, 0.343643320180609, 999.99999999747, -2.9032312850705E-05, 31.1243825132535, 2 ),
      ( 350, 3.43733148912481, 9999.99997450603, -0.000290357483157354, 31.129262703161, 2 ),
      ( 350, 34.4635205633468, 99999.9999999989, -0.00290702225183545, 31.178199063786, 2 ),
      ( 350, 354.052026975843, 1000000.00002122, -0.0294269846294363, 31.6813245527671, 2 ),
      ( 350, 5201.74733936041, 9999999.99985196, -0.33938863038772, 38.3755327920303, 2 ),
      ( 350, 23330.0099910556, 99999999.9999998, 0.472924115986297, 40.8233097216936, 1 ),
      ( 400, 0.300684629864659, 999.999999999517, -1.81398429364223E-05, 33.0719841443266, 2 ),
      ( 400, 3.00733728931335, 9999.99999514799, -0.000181401131166024, 33.0748104161016, 2 ),
      ( 400, 30.1225683712214, 99999.9999999999, -0.00181428161244522, 33.1031142111441, 2 ),
      ( 400, 306.243542165169, 999999.997042845, -0.0181697436512358, 33.3901842949785, 2 ),
      ( 400, 3670.9814816588, 9999999.99669871, -0.180929740409153, 36.5243320743339, 2 ),
      ( 400, 21177.0125066872, 100000000.004632, 0.41983754983389, 40.6701718648698, 1 ),
      ( 500, 0.240545119542423, 999.999999999978, -7.39631827246881E-06, 36.3852883445683, 2 ),
      ( 500, 2.4056113102806, 9999.99999978226, -7.39547316879083E-05, 36.386479579661, 2 ),
      ( 500, 24.0721161629687, 99999.9978544133, -0.000738702116976522, 36.3983956789945, 2 ),
      ( 500, 242.312796286269, 999999.999998819, -0.00730236249237959, 36.517890469728, 2 ),
      ( 500, 2569.33896520687, 9999999.99988412, -0.0637928912779814, 37.7037756439992, 2 ),
      ( 500, 17514.8434029096, 99999999.9999997, 0.373368490146146, 41.3494096178283, 1 ),
      ( 600, 0.200453332144952, 1000, -2.68256605295205E-06, 39.0968960623298, 2 ),
      ( 600, 2.00458159652978, 9999.99999999257, -2.68185200729676E-05, 39.0975138704809, 2 ),
      ( 600, 20.0506413465905, 99999.9999287735, -0.000267471763817718, 39.1036914459169, 2 ),
      ( 600, 200.976088075123, 1000000, -0.00260381430325006, 39.1654037380454, 2 ),
      ( 600, 2043.67141689565, 9999999.99982964, -0.0191535586169271, 39.7645581058742, 2 ),
      ( 600, 14755.9788951786, 100000000.001407, 0.358451276510309, 42.5421799107266, 1 ),
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
