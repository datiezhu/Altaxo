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
	/// Tests and test data for <see cref="Mixture_Isobutane_Pentane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Isobutane_Pentane : MixtureTestBase
    {

    public Test_Mixture_Isobutane_Pentane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("75-28-5", 0.5), ("109-66-0", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 150, 10483.6690757818, 1000.00030441814, -0.999923518232405, 104.050703749242, 1 ),
      ( 150, 10483.7223087416, 9999.99999393395, -0.999235184863699, 104.051218069834, 1 ),
      ( 150, 10484.2544951505, 99999.9999956863, -0.992352235280471, 104.05636073894, 1 ),
      ( 150, 10489.5621041898, 1000000.00011972, -0.923561048109312, 104.107733714918, 1 ),
      ( 150, 10541.2732550254, 10000000.0000063, -0.239360260227793, 104.616086250729, 1 ),
      ( 200, 9859.36146462461, 99999.9999996059, -0.993900636038512, 107.25442636495, 1 ),
      ( 200, 9866.81782936073, 1000000.00000263, -0.939052452332559, 107.30031371088, 1 ),
      ( 200, 9938.57523776673, 10000000.0000004, -0.394924990199208, 107.754078328468, 1 ),
      ( 250, 0.481519285220814, 999.999983668013, -0.000898415574682711, 97.786396327907, 2 ),
      ( 250, 9237.53844727414, 100000.008195205, -0.994792046423449, 114.419532702429, 1 ),
      ( 250, 9248.28410779687, 1000000.00578679, -0.947980979236825, 114.461411181103, 1 ),
      ( 250, 9349.67312361815, 10000000.0000038, -0.485450801955904, 114.878078681522, 1 ),
      ( 300, 0.401091960285691, 999.9999979875, -0.000464713781783235, 112.284160695107, 2 ),
      ( 300, 4.02785037588617, 9999.99999999987, -0.00466618687713506, 112.452049942672, 2 ),
      ( 300, 8580.16951761784, 100000.000002152, -0.995327533255499, 125.490905473728, 1 ),
      ( 300, 8596.59373828446, 1000000.00000052, -0.953364602361241, 125.526199712346, 1 ),
      ( 300, 8746.16069718858, 10000000.0000969, -0.541621082436876, 125.89185077617, 1 ),
      ( 350, 0.343727211606794, 999.9999996994, -0.000273089192145298, 128.13966404791, 2 ),
      ( 350, 3.44576493542297, 9999.99683532101, -0.0027371275727765, 128.221389648392, 2 ),
      ( 350, 35.3545058467408, 99999.9965341705, -0.0280352233942883, 129.067033304913, 2 ),
      ( 350, 7861.75017791776, 1000000.00256357, -0.956290477753556, 139.115214518337, 1 ),
      ( 350, 8101.25788139108, 10000000.0022033, -0.575827175826536, 139.347908862207, 1 ),
      ( 400, 0.300731685062691, 999.999999939373, -0.000174606044509173, 144.204654904051, 2 ),
      ( 400, 3.01205808628573, 9999.9993740638, -0.00174841615748853, 144.248582232064, 2 ),
      ( 400, 30.6105655095665, 99999.9999933262, -0.0177274725646663, 144.696271975137, 2 ),
      ( 400, 383.093941525949, 999999.999979172, -0.215129390197628, 151.219473168638, 2 ),
      ( 400, 7379.62686697861, 10000000.0000006, -0.592555042535087, 153.803638739245, 1 ),
      ( 500, 0.24056337028166, 999.999999995662, -8.32624163714207E-05, 174.286385069926, 2 ),
      ( 500, 2.4074388031507, 9999.99995599608, -0.000833001111957124, 174.302125474324, 2 ),
      ( 500, 24.2573221765371, 99999.9999999897, -0.00836811799584515, 174.460915221221, 2 ),
      ( 500, 263.739622838578, 999999.999999998, -0.087951450732965, 176.207829700005, 2 ),
      ( 500, 5457.4366729048, 10000000, -0.559237505057244, 182.958223957301, 1 ),
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
      ( 150, 11242.9736640146, 100000.008722321, -0.992868318867269, 87.8906340561643, 1 ),
      ( 150, 11249.2724343414, 1000000.00056097, -0.928723126919443, 87.950393717228, 1 ),
      ( 150, 11310.5946230598, 9999999.99999489, -0.291095658567738, 88.5299495765949, 1 ),
      ( 200, 10533.579261304, 99999.9999964578, -0.994291022028561, 93.0560764020787, 1 ),
      ( 200, 10542.7378947893, 1000000.00275644, -0.942959814628089, 93.1091328190063, 1 ),
      ( 200, 10630.5236067961, 9999999.99999858, -0.434308464310535, 93.6221005550765, 1 ),
      ( 250, 0.481394169163483, 999.999997752126, -0.000636466391620184, 86.721698015212, 2 ),
      ( 250, 4.84189761265249, 9999.99999884759, -0.00640654456974062, 86.9480286741838, 2 ),
      ( 250, 9802.20178628981, 100000.000002097, -0.995092043756178, 100.952974315852, 1 ),
      ( 250, 9816.00801858437, 1000000.00691634, -0.950989467400241, 101.002058863356, 1 ),
      ( 250, 9944.91526357158, 10000000.0000027, -0.51624748406922, 101.479109172287, 1 ),
      ( 300, 0.40104329987974, 999.999999770305, -0.000341155811024765, 100.568880505699, 2 ),
      ( 300, 4.02283194185942, 9999.9975566759, -0.00342224641133767, 100.667269656127, 2 ),
      ( 300, 41.5613385081524, 99999.9961699206, -0.0353860183439858, 101.721161148613, 2 ),
      ( 300, 9025.59988087906, 1000000.00006333, -0.955581181645091, 111.868807934972, 1 ),
      ( 300, 9226.79437010583, 9999999.99999958, -0.565497543845827, 112.282061588752, 1 ),
      ( 350, 0.34370456773889, 999.999999964578, -0.000204945282174323, 115.368588759275, 2 ),
      ( 350, 3.44341027759037, 9999.99963284711, -0.00205290850110696, 115.417764909026, 2 ),
      ( 350, 35.0965433620497, 99999.9999970335, -0.0208889702765873, 115.925841894266, 2 ),
      ( 350, 8085.86805819365, 1000000.00289827, -0.957501887784217, 124.823157931441, 1 ),
      ( 350, 8441.58627044021, 10000000.0000303, -0.592927068325635, 124.984003166151, 1 ),
      ( 400, 0.300719741433922, 999.999999992794, -0.000132615880561928, 130.178411898479, 2 ),
      ( 400, 3.01079522961673, 9999.99992626215, -0.00132742915905184, 130.205589508818, 2 ),
      ( 400, 30.4764995825879, 99999.9999999438, -0.0134042114690413, 130.482390448153, 2 ),
      ( 400, 354.195418548822, 999999.999857275, -0.151090484282704, 133.997337359419, 2 ),
      ( 400, 7532.76836873047, 10000000.0000537, -0.600837505544126, 138.517712439127, 1 ),
      ( 500, 0.240559162546802, 999.999750047717, -6.349193410875E-05, 157.62765491869, 2 ),
      ( 500, 2.40696753957807, 9999.9821339912, -0.000635092520369953, 157.63801195719, 2 ),
      ( 500, 24.2085604635118, 99999.9999999998, -0.00636847303814825, 157.742301757964, 2 ),
      ( 500, 257.420971911427, 1000000.00000287, -0.0655621910276878, 158.862040586213, 2 ),
      ( 500, 4939.00907839838, 10000000.0000004, -0.512971356889074, 165.887238208325, 1 ),
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
      ( 150, 12146.3418921164, 100000.000004096, -0.993398713795636, 72.7575076897066, 1 ),
      ( 150, 12153.9136532064, 1000000.000119, -0.934028263255555, 72.8281405952729, 1 ),
      ( 150, 12227.526805963, 10000000.0000022, -0.34425431684246, 73.5036438075058, 1 ),
      ( 200, 0.601936158686925, 999.999999989043, -0.000955358429999465, 63.5772608637839, 2 ),
      ( 200, 11315.4658633606, 100000.000000677, -0.994685494172122, 79.2330657729686, 1 ),
      ( 200, 11326.9834589911, 1000000.00226126, -0.94690898090053, 79.2953584378442, 1 ),
      ( 200, 11436.7363368412, 10000000.0000018, -0.474184700784795, 79.8872153108892, 1 ),
      ( 250, 0.481305692142797, 999.999999997168, -0.000450476742110108, 75.6640885949332, 2 ),
      ( 250, 4.83276757792554, 9999.99996987124, -0.0045271836868716, 75.7853380174234, 2 ),
      ( 250, 10430.6277228362, 100000.000000247, -0.995387728474563, 87.6101747317197, 1 ),
      ( 250, 10449.0803886552, 1000000.00000074, -0.953958735409186, 87.6678521813314, 1 ),
      ( 250, 10618.6667523523, 10000000.0000555, -0.546940415080452, 88.2198489938829, 1 ),
      ( 300, 0.401007413498409, 999.999999999582, -0.000249415745231736, 88.8565537914415, 2 ),
      ( 300, 4.01912114025823, 9999.99999565493, -0.00249984524856905, 88.9116073091019, 2 ),
      ( 300, 41.1438882162561, 99999.9999999996, -0.0255967208827697, 89.4983351740957, 2 ),
      ( 300, 9456.65047842873, 999999.999999964, -0.957605772059567, 98.2973429870608, 1 ),
      ( 300, 9743.25969469281, 10000000.0021912, -0.588528471393107, 98.7371545448896, 1 ),
      ( 350, 0.343687302513848, 999.99999999997, -0.000152440056946578, 102.598904230747, 2 ),
      ( 350, 3.44160168951957, 9999.99999967193, -0.00152620261494265, 102.627535631712, 2 ),
      ( 350, 34.902641148319, 99999.9947716745, -0.0154472561909057, 102.922678663721, 2 ),
      ( 350, 419.493415224561, 999999.999928103, -0.180833599914531, 107.107436121498, 2 ),
      ( 350, 8757.194425177, 10000000.0000001, -0.607597029233752, 110.721396539026, 1 ),
      ( 400, 0.300710421668799, 999.998312926658, -9.93469617154379E-05, 116.152921753887, 2 ),
      ( 400, 3.00979754208909, 9999.99999997909, -0.0009941109095214, 116.169556843629, 2 ),
      ( 400, 30.3719587332133, 99999.9996446066, -0.0100060728182633, 116.338591985772, 2 ),
      ( 400, 337.008169832136, 999999.999519971, -0.107794487236031, 118.356203544536, 2 ),
      ( 400, 7555.1320850719, 10000000.0000001, -0.602018146650596, 123.515327038918, 1 ),
      ( 500, 0.240555825901947, 999.998402816862, -4.73417029025521E-05, 140.969216745931, 2 ),
      ( 500, 2.40658386196358, 9999.99999999941, -0.0004734870736786, 140.976136755543, 2 ),
      ( 500, 24.1690489720793, 99999.9999925436, -0.00474181733653394, 141.045625844458, 2 ),
      ( 500, 252.706602880588, 999999.999999996, -0.0481276119445868, 141.770357662323, 2 ),
      ( 500, 4101.61620627012, 10000000, -0.41353743143106, 147.897251551404, 1 ),
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
