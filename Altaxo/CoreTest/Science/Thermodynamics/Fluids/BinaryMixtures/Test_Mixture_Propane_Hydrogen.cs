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
  /// Tests and test data for <see cref="Mixture_Propane_Hydrogen"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Propane_Hydrogen : MixtureTestBase
  {

    public Test_Mixture_Propane_Hydrogen()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("74-98-6", 0.5), ("1333-74-0", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 1.20272528533008, 999.999999999895, -2.5708502833114E-06, 14.2755289234112, 2 ),
      ( 150, 0.801810307277205, 1000.00000000164, 5.59765617579931E-06, 17.0927177909518, 1 ),
      ( 150, 8.01769903995125, 10000.0000133582, 5.59905533175482E-05, 17.093047587699, 1 ),
      ( 150, 80.1364974870835, 100000, 0.000561318099573818, 17.0963437683893, 1 ),
      ( 200, 0.601357096468202, 1000.00000000189, 6.65192701446778E-06, 18.9920683633667, 1 ),
      ( 200, 6.01321093907925, 10000.0000191207, 6.6524764014924E-05, 18.992298581355, 1 ),
      ( 200, 60.0960976428502, 100000, 0.000665800674043695, 18.9946005175361, 1 ),
      ( 200, 597.348905428144, 1000000.00986609, 0.00671666295376388, 19.0175946595147, 1 ),
      ( 200, 5597.66082394706, 10000000.0000746, 0.0743078503066997, 19.2443061189627, 1 ),
      ( 250, 0.481085810781334, 1000.00000000104, 6.37420595484476E-06, 20.0441945549791, 1 ),
      ( 250, 4.81058212562336, 10000.0000104587, 6.37443832561669E-05, 20.0443751041762, 1 ),
      ( 250, 48.0782293244762, 100000, 0.000637677553634088, 20.0461805268911, 1 ),
      ( 250, 478.028821595932, 1000000.00620112, 0.00640140427994815, 20.0642269076428, 1 ),
      ( 250, 4508.69651359068, 10000000.0008724, 0.0670243070785217, 20.2431485059532, 1 ),
      ( 300, 0.400905060860668, 1000.0000000005, 5.8290786828749E-06, 20.5777008436172, 1 ),
      ( 300, 4.00884029522788, 10000.0000050175, 5.82917834461782E-05, 20.5778504172282, 1 ),
      ( 300, 40.0673797687858, 99999.9999999999, 0.000583018109209238, 20.579346082664, 1 ),
      ( 300, 398.579381341204, 1000000.0009296, 0.00584078485098946, 20.5942950680328, 1 ),
      ( 300, 3783.0256267125, 10000000.0000174, 0.0597533226763959, 20.742519917409, 1 ),
      ( 350, 0.343633100727098, 1000.00000000022, 5.27203377114585E-06, 20.8265643696418, 1 ),
      ( 350, 3.43616796644103, 10000.0000022371, 5.27207298593585E-05, 20.8266922442781, 1 ),
      ( 350, 34.345382742922, 99999.9999999999, 0.000527246834124149, 20.8279709051653, 1 ),
      ( 350, 341.831163028622, 1000000.00014106, 0.00527672584861096, 20.8407486010492, 1 ),
      ( 350, 3262.27191893981, 10000000.0000005, 0.0533607280783858, 20.9673296624802, 1 ),
      ( 350, 22131.5345565868, 100000000, 0.552693562634762, 22.0503457708686, 1 ),
      ( 400, 0.300679114943044, 1000.00000000001, 4.76715123605654E-06, 20.9358231982394, 1 ),
      ( 400, 3.00666215082066, 10000.000000091, 4.76716141471681E-05, 20.9359348168299, 1 ),
      ( 400, 30.0537274241029, 100000.000907764, 0.000476726510833845, 20.9370509098594, 1 ),
      ( 400, 299.253564008034, 1000000.00000019, 0.00476847891848043, 20.9482023308939, 1 ),
      ( 400, 2869.3266516564, 10000000.0016685, 0.0479132731527485, 21.058580833426, 1 ),
      ( 400, 20256.9346970529, 99999999.9999997, 0.484333897613853, 22.0126621750297, 1 ),
      ( 400, 63782.6487927908, 1000000000, 3.7141433292093, 25.9414446362142, 1 ),
      ( 500, 0.240543471183641, 1000, 3.9431823615726E-06, 21.0233154606015, 1 ),
      ( 500, 2.40534953954666, 10000.0000000331, 3.94317161184169E-05, 21.0234041198804, 1 ),
      ( 500, 24.0449627886992, 100000.000329954, 0.000394306178961115, 21.0242906226732, 1 ),
      ( 500, 239.599926696471, 1000000.00000002, 0.00394203777225002, 21.0331465686646, 1 ),
      ( 500, 2314.33088251208, 10000000.0000509, 0.0393692642585121, 21.1207282628731, 1 ),
      ( 500, 17338.1800583269, 100000000.000002, 0.387368442659372, 21.8887852889277, 1 ),
      ( 500, 60387.7724883696, 1000000000, 2.98333021319662, 25.409275595721, 1 ),
      ( 600, 0.200453019093834, 1000, 3.3247729930605E-06, 21.1132543002522, 1 ),
      ( 600, 2.00447034505454, 10000.0000000138, 3.32475781850182E-05, 21.1133274348389, 1 ),
      ( 600, 20.0387078110799, 100000.000137847, 0.000332460425219281, 21.1140587025367, 1 ),
      ( 600, 199.789776279466, 1000000, 0.00332310599338066, 21.121363531025, 1 ),
      ( 600, 1940.29487383502, 10000000.0000027, 0.0331094597374696, 21.193600317023, 1 ),
      ( 600, 15166.7328679899, 99999999.9999999, 0.321666970920103, 21.8341619220258, 1 ),
      ( 600, 57379.1649273724, 1000000000, 2.49349278849963, 24.9997080841821, 1 ),
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
      ( 200, 0.601430860231604, 999.999997727362, -0.000115996009418149, 33.3348976802725, 2 ),
      ( 200, 6.02060042401874, 9999.99999865099, -0.00116092366316888, 33.3475900776855, 2 ),
      ( 250, 0.481117996580979, 999.999999811273, -6.05241537548427E-05, 37.9939910559622, 2 ),
      ( 250, 4.81380325468409, 9999.99806742763, -0.000605442464612973, 37.9991814491381, 2 ),
      ( 250, 48.4029200345045, 99999.9987618899, -0.00607468095537669, 38.0515103780904, 2 ),
      ( 250, 19371.6070213651, 99999999.9997161, 1.48347427650001, 42.754955225884, 1 ),
      ( 300, 0.400921259735089, 999.99999998153, -3.45752861440037E-05, 42.9584884366185, 2 ),
      ( 300, 4.01046077068437, 9999.99981339671, -0.000345793927339499, 42.9609408501222, 2 ),
      ( 300, 40.2300176449295, 99999.999999931, -0.00346203844709055, 42.9855194674275, 2 ),
      ( 300, 415.457115368067, 1000000.00000871, -0.0350209854692056, 43.2371090141719, 2 ),
      ( 300, 18108.5262085406, 100000000.000001, 1.21391510910887, 47.1316034464852, 1 ),
      ( 350, 0.343642001897716, 999.999999999908, -2.06305552496058E-05, 48.0594352108097, 2 ),
      ( 350, 3.43705820974347, 9999.9999868916, -0.00020630608362095, 48.0608554574813, 2 ),
      ( 350, 34.4345330048674, 99999.9999999998, -0.00206309658841883, 48.0750498497767, 2 ),
      ( 350, 350.868610631428, 1000000.00000033, -0.0206165443127063, 48.2162009087704, 2 ),
      ( 350, 16918.7152059978, 100000000.001279, 1.03109342637242, 51.6994775658146, 1 ),
      ( 350, 27607.3256907738, 1000000000.01564, 11.4472365134567, 58.206663327928, 1 ),
      ( 400, 0.300684291245471, 999.999999999882, -1.24480051704007E-05, 53.0849011276068, 2 ),
      ( 400, 3.00717978773325, 9999.9999988105, -0.000124470268171482, 53.0858612647642, 2 ),
      ( 400, 30.1054974058767, 99999.988186328, -0.00124371201562124, 53.0954481637726, 2 ),
      ( 400, 304.432927725155, 999999.999943213, -0.0123258000608684, 53.1898660752343, 2 ),
      ( 400, 3338.97378726989, 10000000, -0.0994821538514975, 53.9793616069571, 2 ),
      ( 400, 15814.5989785655, 100000000.000197, 0.901284684700524, 56.2720886828679, 1 ),
      ( 400, 27161.9552148183, 1000000000.00008, 10.0699154736217, 62.8308739322738, 1 ),
      ( 500, 0.240545429795196, 999.999999999983, -4.03957692103401E-06, 62.3985973184408, 2 ),
      ( 500, 2.4055415351445, 9999.99999982664, -4.03853087590181E-05, 62.3991539844086, 2 ),
      ( 500, 24.0641369669335, 99999.9984010504, -0.000402802756783625, 62.4047121643129, 2 ),
      ( 500, 241.490713066897, 999999.999999981, -0.00391847119850492, 62.459447912397, 2 ),
      ( 500, 2466.12107494849, 9999999.99999999, -0.024604099514007, 62.9250544818593, 1 ),
      ( 500, 13886.4779927847, 99999999.9999995, 0.732220645044807, 64.915829690065, 1 ),
      ( 500, 26322.8917389958, 1000000000, 8.13822239008922, 71.32958743825, 1 ),
      ( 600, 0.200453758394801, 999.994406451966, -2.91061321956752E-07, 70.5100455365299, 2 ),
      ( 600, 2.00454280850401, 9999.99999999992, -2.90323805096634E-06, 70.5104214205806, 2 ),
      ( 600, 20.0459370172761, 99999.9999998145, -2.82914630031922E-05, 70.5141759325006, 2 ),
      ( 600, 200.495222516557, 999999.995571468, -0.000207105346560243, 70.5512904707135, 2 ),
      ( 600, 1991.18966440858, 9999999.99999999, 0.00670319089590066, 70.8815498893009, 1 ),
      ( 600, 12316.0844053741, 99999999.9999997, 0.627576527458975, 72.5866343436415, 1 ),
      ( 600, 25547.8228838483, 1000000000.00001, 6.84621452071499, 78.6870860130801, 1 ),
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
      ( 150, 15257.6296781552, 9999999.9999967, -0.474482726055686, 59.3502450900394, 1 ),
      ( 150, 16055.6359402877, 100000000.000264, 3.99397718358613, 62.5331907904148, 1 ),
      ( 200, 0.601700672611853, 999.99999964454, -0.000564360279870104, 47.6881404143873, 2 ),
      ( 200, 6.04793764732415, 9999.99622334836, -0.00567576412588122, 47.8079811771879, 2 ),
      ( 200, 13980.1727443427, 1000000.0012526, -0.956984716266034, 60.9554398697202, 1 ),
      ( 200, 14138.4602431902, 9999999.99999942, -0.57466295034674, 61.3715267463018, 1 ),
      ( 200, 15224.1673329132, 100000000.000353, 2.95004260990766, 64.7056927068947, 1 ),
      ( 200, 18929.2305244367, 1000000000.00006, 30.7689140018358, 80.6743493651588, 1 ),
      ( 250, 0.481220948756986, 999.999973987951, -0.000274450712326991, 55.9494393953103, 2 ),
      ( 250, 4.82416152212112, 9999.99959045026, -0.0027513067879522, 56.0107754683165, 2 ),
      ( 250, 49.506576311771, 100000.003639762, -0.0282323831731302, 56.6532101312438, 2 ),
      ( 250, 12690.9528527729, 999999.999998866, -0.962091981358523, 66.0010640924767, 1 ),
      ( 250, 12955.479098288, 10000000.0000212, -0.62865991008909, 66.3321388118252, 1 ),
      ( 250, 14441.7618993305, 100000000.000003, 2.33123396352138, 69.5241510726625, 1 ),
      ( 250, 18574.801057806, 1000000000.00007, 24.9000823654629, 84.6038201583475, 1 ),
      ( 300, 0.400969226801756, 999.9999967609, -0.000154198949971171, 65.3420192704515, 2 ),
      ( 300, 4.01527296340703, 9999.9999999999, -0.00154385163410766, 65.3715340267222, 2 ),
      ( 300, 40.7273139932042, 100000.000158147, -0.0156301546807147, 65.6739621089755, 2 ),
      ( 300, 11628.3201187299, 10000000.0098286, -0.655231887279115, 73.7009830248142, 1 ),
      ( 300, 13699.2580844085, 100000000.000046, 1.92648985294406, 76.5511615209389, 1 ),
      ( 300, 18252.7969660689, 1000000000.00007, 20.9641624520946, 90.7027691144102, 1 ),
      ( 350, 0.34366755123172, 999.999999967906, -9.49721880678155E-05, 75.2937506663035, 2 ),
      ( 350, 3.43961782098037, 9999.99525786832, -0.00095030781751937, 75.3094877919242, 2 ),
      ( 350, 34.695270822778, 99999.999615909, -0.00956267439819856, 75.4691520670961, 2 ),
      ( 350, 382.993491923942, 999999.999999999, -0.102765661509846, 77.3415850522747, 2 ),
      ( 350, 9990.08049231961, 10000000.0015382, -0.656023880204104, 82.6875483559096, 1 ),
      ( 350, 12993.1091424988, 99999999.9999977, 1.64474737034604, 84.7777213843395, 1 ),
      ( 350, 17955.5530501186, 1000000000.00006, 18.1380856614789, 98.0673975928487, 1 ),
      ( 400, 0.300699216744874, 999.999999877419, -6.2083364283248E-05, 85.2347308254278, 2 ),
      ( 400, 3.00867394493656, 9999.99874971716, -0.000621024902775266, 85.243313458158, 2 ),
      ( 400, 30.2565397376724, 99999.9999929289, -0.0062295591376429, 85.3299154207883, 2 ),
      ( 400, 321.383795899767, 1000000.01565926, -0.0644190782957515, 86.2802997528769, 2 ),
      ( 400, 7575.64943029291, 10000000.0008511, -0.603096010307642, 93.0168008048837, 1 ),
      ( 400, 12322.8955729807, 100000000.000001, 1.44001538879496, 93.3608056447273, 1 ),
      ( 400, 17678.1842015777, 1000000000.00006, 16.0085651839033, 105.918937504299, 1 ),
      ( 500, 0.240551455819247, 999.999999991616, -2.91711332019687E-05, 103.774102175598, 2 ),
      ( 500, 2.40614629634704, 9999.99991565013, -0.000291715318931647, 103.777134700462, 2 ),
      ( 500, 24.1248291127619, 99999.9999999892, -0.00291754384020539, 103.80752822498, 2 ),
      ( 500, 247.780816077741, 1000000.00002202, -0.0292047525379977, 104.118439590375, 2 ),
      ( 500, 3246.80163500169, 10000000, -0.25913417047863, 107.410006253292, 1 ),
      ( 500, 11095.8324799073, 100000000.000002, 1.16788095076486, 109.853001613815, 1 ),
      ( 500, 17170.7548633525, 1000000000.00005, 13.0089612003078, 121.283713566474, 1 ),
      ( 600, 0.200456537812803, 999.999999999316, -1.41623163207061E-05, 119.906930592161, 2 ),
      ( 600, 2.00482087757106, 9999.99999314886, -0.000141603040441821, 119.908453552823, 2 ),
      ( 600, 20.0737543788109, 99999.9999999999, -0.00141401004881695, 119.923670361661, 2 ),
      ( 600, 203.285393195555, 999999.999362649, -0.0139296496681972, 120.074530619242, 2 ),
      ( 600, 2244.88373229378, 10000000, -0.107064227862044, 121.395964071508, 2 ),
      ( 600, 10028.4834121544, 100000000.011053, 0.998843600385653, 124.570672931902, 1 ),
      ( 600, 16713.4152559362, 1000000000.00005, 10.9935809536435, 135.113991108276, 1 ),
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
