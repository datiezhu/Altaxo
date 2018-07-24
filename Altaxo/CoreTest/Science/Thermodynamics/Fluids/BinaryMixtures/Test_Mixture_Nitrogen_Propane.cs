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
	/// Tests and test data for <see cref="Mixture_Nitrogen_Propane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Nitrogen_Propane : MixtureTestBase
    {

    public Test_Mixture_Nitrogen_Propane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("7727-37-9", 0.5), ("74-98-6", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 16370.2412399994, 10000000.0009409, -0.265299655463065, 59.4911423411895, 1 ),
      ( 100, 16957.5942554405, 100000000.000531, 6.09252839580648, 62.3563159519132, 1 ),
      ( 100, 19797.6219929172, 1000000000.0001, 59.7508410978336, 79.3907401059424, 1 ),
      ( 150, 19326.2445759776, 1000000000.00008, 40.4883910176611, 79.400980629313, 1 ),
      ( 200, 0.601700822032849, 999.999999643257, -0.000564613038533329, 47.6899804359012, 2 ),
      ( 200, 6.0479531817862, 9999.99621414229, -0.00567832263416209, 47.8099141601537, 2 ),
      ( 200, 13979.1655757201, 1000000.00125983, -0.956981617304321, 60.9744828517499, 1 ),
      ( 200, 14137.3374449907, 10000000.0000033, -0.574629171695448, 61.3909045278007, 1 ),
      ( 200, 15222.4396226769, 100000000.000337, 2.95049091217514, 64.7274366603685, 1 ),
      ( 200, 18926.2245556001, 1000000000.00012, 30.7739595732216, 80.7033073627428, 1 ),
      ( 250, 0.481220997532487, 999.999973939268, -0.000274556611381099, 55.9502334347749, 2 ),
      ( 250, 4.82416665097321, 9999.99958432414, -0.00275237157705193, 56.0116135351622, 2 ),
      ( 250, 49.5071505539488, 100000.003647032, -0.0282436593210713, 56.6545204312635, 2 ),
      ( 250, 12690.0370970592, 999999.999998502, -0.962089245961831, 66.0129203879623, 1 ),
      ( 250, 12954.3838749179, 10000000.0000216, -0.628628516982208, 66.3446380175661, 1 ),
      ( 250, 14439.9630763637, 100000000.000002, 2.33164892859123, 69.5398414901393, 1 ),
      ( 250, 18571.7563941315, 1000000000.00008, 24.9043283204025, 84.6283783067457, 1 ),
      ( 300, 0.400969245749395, 999.999996755308, -0.000154250766917343, 65.342300525568, 2 ),
      ( 300, 4.01527503451694, 9999.9999999999, -0.00154437120879407, 65.371835551604, 2 ),
      ( 300, 40.7275348373934, 100000.000158403, -0.0156354969041443, 65.6744740083182, 2 ),
      ( 300, 11627.2772893056, 10000000.0099551, -0.65520096722369, 73.7088293629376, 1 ),
      ( 300, 13697.3981644712, 100000000.000046, 1.92688721699303, 76.5624962328072, 1 ),
      ( 300, 18249.7097383803, 1000000000.00013, 20.96787793794, 90.7235866462832, 1 ),
      ( 350, 0.34366755926068, 999.999999967855, -9.50001183367473E-05, 75.293832045349, 2 ),
      ( 350, 3.43961876816217, 9999.99525014823, -0.00095058749502542, 75.3095796254669, 2 ),
      ( 350, 34.6953699856337, 99999.9996142489, -0.00956550969395065, 75.4693507737696, 2 ),
      ( 350, 383.007776204356, 1000000, -0.102799127977666, 77.3431437595035, 2 ),
      ( 350, 9989.16865210492, 10000000.0015252, -0.655992482641344, 82.692724614882, 1 ),
      ( 350, 12991.2085911266, 99999999.9999982, 1.64513427208466, 84.7860882945545, 1 ),
      ( 350, 17952.4281021281, 1000000000.00012, 18.1414169073403, 98.0851317472165, 1 ),
      ( 400, 0.300699220147813, 999.999999877235, -6.2099250395162E-05, 85.2347955638684, 2 ),
      ( 400, 3.00867440970942, 9999.99874782818, -0.000621183851828742, 85.2433839714711, 2 ),
      ( 400, 30.2565882666583, 99999.9999929043, -0.00623115760246178, 85.3300444916383, 2 ),
      ( 400, 321.389656883354, 1000000.01561461, -0.0644361441771826, 86.2811019733302, 2 ),
      ( 400, 7575.24026201174, 10000000.0008375, -0.60307457378901, 93.0208926898242, 1 ),
      ( 400, 12320.9793150608, 99999999.9999965, 1.44039486848332, 93.3672038182703, 1 ),
      ( 400, 17675.0292858658, 1000000000.00013, 16.0116010609289, 105.934218617961, 1 ),
      ( 500, 0.240551455966666, 999.999999991603, -2.91763162348639E-05, 103.774428172634, 2 ),
      ( 500, 2.40614641003916, 9999.99991554182, -0.000291767124864294, 103.777462785017, 2 ),
      ( 500, 24.1248414783454, 99999.999999989, -0.00291805946819111, 103.80787729484, 2 ),
      ( 500, 247.782070557069, 1000000.00002201, -0.02920967194939, 104.119009404256, 2 ),
      ( 500, 3246.82478960754, 10000000, -0.259139457320726, 107.412782885052, 1 ),
      ( 500, 11093.9583399707, 100000000.000002, 1.16824716832275, 109.857273115276, 1 ),
      ( 500, 17167.5645946166, 1000000000.00014, 13.0115644380204, 121.295577804975, 1 ),
      ( 600, 0.200456537094278, 999.999999999315, -1.41633022086121E-05, 119.907711487278, 2 ),
      ( 600, 2.00482088812028, 9999.99999314222, -0.000141612871320825, 119.909235428955, 2 ),
      ( 600, 20.073756207144, 99999.9999999997, -0.00141410556467675, 119.924462058828, 2 ),
      ( 600, 203.285532961643, 999999.999363021, -0.0139303321335799, 120.075421500944, 2 ),
      ( 600, 2244.838813002, 10000000, -0.107046364272129, 121.397782864568, 2 ),
      ( 600, 10026.7242457561, 100000000.011159, 0.999194283900255, 124.574101516046, 1 ),
      ( 600, 16710.2182982543, 1000000000.00011, 10.9958754810964, 135.123794516668, 1 ),
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
      ( 200, 0.601462068200794, 999.999990643588, -0.000170161617879428, 34.2513817851962, 2 ),
      ( 200, 6.02386171434844, 9999.99999526709, -0.00170397170654807, 34.2784440198245, 2 ),
      ( 200, 18975.0493364968, 100000000.001563, 2.16921295847748, 45.4221337349857, 1 ),
      ( 250, 0.481129326173957, 999.999999301076, -8.63556193601782E-05, 38.3891907100187, 2 ),
      ( 250, 4.81503808551456, 9999.99273338852, -0.000864022856175985, 38.4002339855568, 2 ),
      ( 250, 48.5303911893269, 99999.9948299272, -0.00868761522996965, 38.5122295704133, 2 ),
      ( 250, 17675.1367476846, 99999999.9835128, 1.72183341352473, 47.1594708527123, 1 ),
      ( 300, 0.400926004387043, 999.999999921064, -4.86941713663648E-05, 43.0982344558583, 2 ),
      ( 300, 4.01101832915671, 9999.99919839772, -0.000487036598910866, 43.1033103492989, 2 ),
      ( 300, 40.2872462472376, 99999.9999979664, -0.0048799087021238, 43.1543884367727, 2 ),
      ( 300, 421.919796952975, 1000000.00036235, -0.049804051587633, 43.6987531532354, 2 ),
      ( 300, 16470.2537080487, 100000000.000002, 1.43412450551122, 50.1717246641202, 1 ),
      ( 350, 0.343644077120254, 999.999999999364, -2.89544084974845E-05, 48.099627541701, 2 ),
      ( 350, 3.43733654815801, 9999.99990874007, -0.000289548911576586, 48.1023486396305, 2 ),
      ( 350, 34.4632168541301, 99999.9999999815, -0.0028959613218721, 48.1296148158704, 2 ),
      ( 350, 353.895102972798, 1000000.00001066, -0.0289943991240504, 48.4078325673853, 2 ),
      ( 350, 4601.1538233698, 10000000.0000671, -0.253156620490554, 51.2146916465209, 1 ),
      ( 350, 15363.3044108081, 100000000.001628, 1.23672016074438, 53.9125241515002, 1 ),
      ( 400, 0.300685134146944, 999.999999998242, -1.75363718928648E-05, 53.1169589188801, 2 ),
      ( 400, 3.00732593539175, 9999.99998238266, -0.000175346203257388, 53.1186602462762, 2 ),
      ( 400, 30.1207486193253, 99999.9999999998, -0.00175169939828477, 53.1356717578486, 2 ),
      ( 400, 305.982222976895, 999999.996034752, -0.0173289862526771, 53.3055510734174, 2 ),
      ( 400, 3483.61694957768, 10000000, -0.136874502642881, 54.8454247156952, 1 ),
      ( 400, 14355.4397513997, 99999999.9999024, 1.09453605342175, 57.978552829783, 1 ),
      ( 400, 23632.360856933, 1000000000, 11.7232257090472, 68.1781226281177, 1 ),
      ( 500, 0.240545313398476, 999.999999999916, -5.92163683821879E-06, 62.5614234026407, 2 ),
      ( 500, 2.40558129263958, 9999.99999916846, -5.91968665266391E-05, 62.5623189432346, 2 ),
      ( 500, 24.0685897350367, 99999.9926225855, -0.000590015306238254, 62.5712628359141, 2 ),
      ( 500, 241.923247776724, 999999.999999925, -0.00570163804432247, 62.6595351198263, 2 ),
      ( 500, 2489.85646354796, 10000000, -0.0339045945093553, 63.413551162288, 1 ),
      ( 500, 12627.014727385, 100000000.000032, 0.904994127035024, 66.1480668151649, 1 ),
      ( 500, 22839.7295402625, 1000000000.00001, 9.53181862561731, 75.4977490258604, 1 ),
      ( 600, 0.200453399590281, 999.981454240543, -7.76547578775755E-07, 70.9003488978701, 2 ),
      ( 600, 2.00454794716706, 9999.99999999873, -7.75188724727079E-06, 70.900925355474, 2 ),
      ( 600, 20.04685055597, 99999.9999933238, -7.61453571853816E-05, 70.9066827768397, 2 ),
      ( 600, 200.578366197613, 1000000, -0.000623822928883877, 70.96354059426, 2 ),
      ( 600, 1989.12653460819, 10000000, 0.00774504450127426, 71.4605129891052, 1 ),
      ( 600, 11238.0802800725, 100000000.000083, 0.783696466106235, 73.7232246285552, 1 ),
      ( 600, 22136.096200168, 1000000000.00001, 8.05549194405885, 82.2282482194794, 1 ),
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
      ( 150, 0.801857555418065, 999.999997467533, -5.78915424986693E-05, 20.8112803564065, 2 ),
      ( 150, 8.02275702321602, 9999.99999999999, -0.000579062375971658, 20.8162887769107, 2 ),
      ( 150, 80.649315307915, 99999.9978095988, -0.00580540365348059, 20.8667390307258, 2 ),
      ( 150, 15025.69195144, 10000000.0000001, -0.466373237759371, 25.3332460280199, 1 ),
      ( 200, 0.601371368534469, 999.999999929839, -2.16464397036429E-05, 20.8201569751967, 2 ),
      ( 200, 6.01488539500564, 9999.99930375669, -0.000216443868087487, 20.8221088113529, 2 ),
      ( 200, 60.2661526624998, 99999.9999996686, -0.00216236739996616, 20.8416359025423, 2 ),
      ( 200, 614.505667773595, 1000000.00000895, -0.0213949479684132, 21.0376264488728, 2 ),
      ( 200, 7131.87777638449, 10000000.0002477, -0.156802219780469, 22.7513665344306, 1 ),
      ( 250, 0.481090526160612, 999.999999999542, -7.99303266861217E-06, 20.8325195140499, 2 ),
      ( 250, 4.8112512422247, 9999.99999547464, -7.99031939860324E-05, 20.8335372064525, 2 ),
      ( 250, 48.1470081316413, 100000, -0.000796312258898504, 20.8437046502394, 2 ),
      ( 250, 484.812242188432, 999999.999998913, -0.00768454481118624, 20.9443867822007, 2 ),
      ( 250, 5022.42729671205, 10000000.0163445, -0.0421231560584637, 21.8106951742739, 1 ),
      ( 250, 22524.9903686312, 99999999.9999997, 1.13579083904073, 24.801865989349, 1 ),
      ( 250, 40008.0059887498, 999999999.999994, 11.0247602673252, 34.3019473972428, 1 ),
      ( 300, 0.400906333801567, 999.999999999995, -1.87438447167849E-06, 20.8562835719873, 2 ),
      ( 300, 4.00913073891738, 9999.99999995989, -1.87236800870527E-05, 20.8569150583709, 2 ),
      ( 300, 40.0979836543336, 99999.9997647709, -0.000185219337220151, 20.8632230941392, 2 ),
      ( 300, 401.56788612708, 999999.999999979, -0.0016493320960774, 20.9256095274553, 2 ),
      ( 300, 3989.81473957982, 10000000.000035, 0.00482251305197434, 21.473772775089, 1 ),
      ( 300, 20368.6809744105, 100000000.000033, 0.968245110360026, 24.0320353509289, 1 ),
      ( 300, 38925.1701481601, 999999999.999998, 9.29939151962422, 32.984618840299, 1 ),
      ( 350, 0.343632941234852, 999.999999999999, 1.14743738630111E-06, 20.9064903845888, 1 ),
      ( 350, 3.43629395662592, 10000.0000000076, 1.14884165515114E-05, 20.9069290922848, 1 ),
      ( 350, 34.3593387643802, 100000.000115394, 0.000116287966852318, 20.9113120487788, 1 ),
      ( 350, 343.186224193618, 1000000.00000009, 0.00130284724386561, 20.9547263514523, 1 ),
      ( 350, 3347.0255271331, 10000000.0000017, 0.0266827684298255, 21.3465383824606, 1 ),
      ( 350, 18567.6096829016, 100000000.000009, 0.850713954509112, 23.5356274774092, 1 ),
      ( 350, 37958.017893639, 1000000000, 8.05298438879447, 31.8892946444866, 1 ),
      ( 400, 0.300678341799368, 1000.00000000001, 2.71833699621232E-06, 20.9995876888076, 1 ),
      ( 400, 3.00670999296862, 10000.0000000761, 2.71931641479567E-05, 20.9999168949917, 1 ),
      ( 400, 30.0597139480045, 100000.000855721, 0.000272910159759051, 21.0032064777056, 1 ),
      ( 400, 299.831784178089, 1000000.00000062, 0.00282622242750813, 21.0358531375535, 1 ),
      ( 400, 2898.47523153103, 10000000.0000004, 0.0373701738823416, 21.3373694758818, 1 ),
      ( 400, 17056.843108498, 99999999.9999999, 0.762806713880697, 23.2339369345579, 1 ),
      ( 400, 37084.1726792893, 999999999.999994, 7.10801896790383, 31.0164737371523, 1 ),
      ( 500, 0.24054236616452, 1000.00000000002, 3.97072024353698E-06, 21.3483926163656, 1 ),
      ( 500, 2.40533788264954, 10000.0000001545, 3.97122137181084E-05, 21.3486076741454, 1 ),
      ( 500, 24.0447733002718, 100000.001597895, 0.000397622351329882, 21.3507572662856, 1 ),
      ( 500, 239.578878636343, 1000000.00000091, 0.00402565434776323, 21.3721540896979, 1 ),
      ( 500, 2302.47363187929, 10000000.0101195, 0.0447170255292111, 21.5760881871026, 1 ),
      ( 500, 14688.4104143427, 100000000.000064, 0.637640381830184, 23.0570390518144, 1 ),
      ( 500, 35550.5482796026, 1000000000, 5.76623433490544, 29.8520716798317, 1 ),
      ( 600, 0.200451919767214, 1000.00000000001, 4.22522819350464E-06, 21.892821618746, 1 ),
      ( 600, 2.00444313881267, 10000.0000001317, 4.22550301850536E-05, 21.8929801672304, 1 ),
      ( 600, 20.0368063158517, 100000.001335381, 0.000422824391654834, 21.8945651994717, 1 ),
      ( 600, 199.6034157612, 1000000.00000048, 0.00425527736654361, 21.9103698914461, 1 ),
      ( 600, 1918.30282798767, 10000000.0091215, 0.0449485907309194, 22.0636618275675, 1 ),
      ( 600, 12927.196573455, 100000000.000001, 0.550628417559726, 23.2709244750639, 1 ),
      ( 600, 34227.7827468165, 1000000000, 4.85643496526062, 29.2766400024867, 1 ),
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
