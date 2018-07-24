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
	/// Tests and test data for <see cref="Mixture_Ethane_Helium"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Ethane_Helium : MixtureTestBase
    {

    public Test_Mixture_Ethane_Helium()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-84-0", 0.5), ("7440-59-7", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 150, 0.80180694401868, 1000.0000000001, 9.79227855582345E-06, 12.4896688711387, 1 ),
      ( 150, 8.01736287505398, 10000.0000009612, 9.7922510751948E-05, 12.489851141882, 1 ),
      ( 150, 80.1030429128299, 100000.009546559, 0.000979196861354094, 12.491673263437, 1 ),
      ( 150, 794.042403502887, 1000000.00003791, 0.00978838409478267, 12.5098358241637, 1 ),
      ( 150, 7306.89406812804, 10000000, 0.0973401120361943, 12.6853766969945, 1 ),
      ( 150, 42301.8100652368, 100000000.000005, 0.89546214287072, 13.9289847823868, 1 ),
      ( 200, 0.601356676555147, 1000.00000000002, 7.35020786011999E-06, 12.4932561789669, 1 ),
      ( 200, 6.01316899014889, 10000.0000002293, 7.35014048373926E-05, 12.4933930928451, 1 ),
      ( 200, 60.0919453039882, 100000.002278903, 0.000734946455165918, 12.4947618326122, 1 ),
      ( 200, 596.977789423219, 1000000.00000165, 0.00734249633096285, 12.5084093562153, 1 ),
      ( 200, 5606.14026264428, 10000000.0150036, 0.0726829307487604, 12.6409104756309, 1 ),
      ( 200, 36080.5873126604, 99999999.9999999, 0.666716485074243, 13.6450616335721, 1 ),
      ( 250, 0.481086082628772, 1000.00000000001, 5.80913209270363E-06, 12.4979487776811, 1 ),
      ( 250, 4.81060932145034, 10000.0000000721, 5.80907208896668E-05, 12.4980571018798, 1 ),
      ( 250, 48.0809600447453, 100000.000717782, 0.000580847128774621, 12.4991400667897, 1 ),
      ( 250, 478.313515221822, 1000000.00000013, 0.00580239109371303, 12.5099420479447, 1 ),
      ( 250, 4549.71776427861, 10000000.0004381, 0.0574037824932757, 12.6152354610548, 1 ),
      ( 250, 31478.4238277566, 100000000.001855, 0.528313107278347, 13.448880031416, 1 ),
      ( 300, 0.4009054890602, 1000, 4.76099146406055E-06, 12.5036293174377, 1 ),
      ( 300, 4.00888311701956, 10000.0000000274, 4.76094345944174E-05, 12.5037181794585, 1 ),
      ( 300, 40.0716638091192, 100000.000272402, 0.00047604631052758, 12.5046066003185, 1 ),
      ( 300, 399.009854157262, 1000000.00000001, 0.00475563092530412, 12.513470908101, 1 ),
      ( 300, 3828.84789131472, 10000000.0000219, 0.0470705787954145, 12.6001596766961, 1 ),
      ( 300, 27932.6584824574, 100000000, 0.435264022647878, 13.3086912409768, 1 ),
      ( 350, 0.343633535246552, 1000, 4.00754209696695E-06, 12.5099001247551, 1 ),
      ( 350, 3.43621141740471, 10000.0000000119, 4.00750427323269E-05, 12.5099749795387, 1 ),
      ( 350, 34.3497268692343, 100000.000118179, 0.000400712590088001, 12.5107233790826, 1 ),
      ( 350, 342.26471382696, 1000000, 0.00400332983711628, 12.5181925715776, 1 ),
      ( 350, 3305.28092325405, 10000000.0000016, 0.0396541787259146, 12.591431816314, 1 ),
      ( 350, 25111.3058491439, 100000000, 0.368447003261383, 13.2049022463866, 1 ),
      ( 400, 0.300679513146248, 1000.01814889134, 3.44286309087796E-06, 12.5163613148772, 1 ),
      ( 400, 3.00670196940683, 10000.0000000057, 3.44277061076778E-05, 12.5164256584412, 1 ),
      ( 400, 30.0577075552035, 100000.000056707, 0.000344247057731005, 12.5170689818599, 1 ),
      ( 400, 299.649913021578, 999999.999999999, 0.00343946471217245, 12.5234908840272, 1 ),
      ( 400, 2907.66732709005, 10000000.0000002, 0.0340954259948852, 12.5865974486906, 1 ),
      ( 400, 22808.6801964936, 100000000, 0.318272454765198, 13.1256189135436, 1 ),
      ( 500, 0.240543786506077, 1000.01095612999, 2.65803677814306E-06, 12.5288431741905, 1 ),
      ( 500, 2.40538045187837, 10000.0000000016, 2.657988196653E-05, 12.5288928908357, 1 ),
      ( 500, 24.0480523935323, 100000.00001628, 0.000265779217077055, 12.5293899866342, 1 ),
      ( 500, 239.907285476197, 1000000, 0.00265583090704434, 12.5343538304094, 1 ),
      ( 500, 2343.65713588761, 10000000.0018639, 0.0263636048910713, 12.5832927154063, 1 ),
      ( 500, 19270.3055143331, 100000000.000658, 0.248264790000711, 13.0136183453172, 1 ),
      ( 600, 0.200453260734053, 1000.00724632346, 2.14295228813585E-06, 12.540099867513, 1 ),
      ( 600, 2.00449403406796, 10000.0000000006, 2.14292336984019E-05, 12.5401399700371, 1 ),
      ( 600, 20.0410755096738, 100000.000005769, 0.000214278855192529, 12.5405409476215, 1 ),
      ( 600, 200.025356498956, 1000000, 0.00214144042562914, 12.5445459327381, 1 ),
      ( 600, 1962.76796500647, 10000000.0002112, 0.0212806733032899, 12.5841238566454, 1 ),
      ( 600, 16677.2763052595, 99999999.9999996, 0.201957053506922, 12.9393381011411, 1 ),
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
      ( 150, 0.801846767410262, 999.999999960937, -3.98727985650722E-05, 21.4422498592917, 2 ),
      ( 150, 8.02134644025005, 9999.99999993908, -0.000398746634061555, 21.444344699294, 2 ),
      ( 200, 0.601369984211869, 999.99999999886, -1.47788555868437E-05, 23.238295878324, 2 ),
      ( 200, 6.01449965254533, 9999.9999886174, -0.00014775726635191, 23.2392401664679, 2 ),
      ( 200, 60.2249075238496, 99999.9999861038, -0.00147443744254638, 23.2486611801592, 2 ),
      ( 250, 0.481091329832048, 999.999999999969, -5.09780684508782E-06, 25.5861395136712, 2 ),
      ( 250, 4.81113391121369, 9999.99999969692, -5.09522296549787E-05, 25.5866869483912, 2 ),
      ( 250, 48.1332882482309, 99999.997388135, -0.000506936391560859, 25.5921491999251, 2 ),
      ( 250, 483.41314410402, 999999.99958352, -0.00480803388020834, 25.6455656124641, 2 ),
      ( 400, 0.30067976598057, 999.999999999999, 2.55089865325333E-06, 34.7948145503275, 1 ),
      ( 400, 3.00672875621865, 10000.0000000156, 2.55184441839153E-05, 34.7950295352099, 1 ),
      ( 400, 30.0603554830786, 100000.000167831, 0.000256129689633828, 34.7971769321629, 1 ),
      ( 400, 299.884213595453, 1000000.00000001, 0.00265547398914384, 34.8184094364707, 1 ),
      ( 500, 0.240543602899316, 1000, 3.40633904185712E-06, 41.0365497479615, 1 ),
      ( 500, 2.40536243874287, 10000.0000000239, 3.40688216816348E-05, 41.0367005952923, 1 ),
      ( 500, 24.0462385493543, 100000.000245182, 0.000341230779921456, 41.03820797842, 1 ),
      ( 500, 239.713533301777, 1000000.00000002, 0.00346624299932535, 41.0531743043178, 1 ),
      ( 500, 2313.36035602903, 10000000.0036803, 0.0398053119465029, 41.1932256757462, 1 ),
      ( 500, 14750.4954486261, 100000000.000004, 0.630754977000423, 42.1094476973865, 1 ),
      ( 600, 0.200452975002159, 999.999999999999, 3.54041896274976E-06, 46.6654267443584, 1 ),
      ( 600, 2.00446601547604, 10000.000000019, 3.54076160275374E-05, 46.6655432941113, 1 ),
      ( 600, 20.0382679592785, 100000.000192976, 0.000354418313004711, 46.666708231278, 1 ),
      ( 600, 199.738999757135, 1000000.00000001, 0.00357816513379211, 46.6783022077436, 1 ),
      ( 600, 1929.33997575566, 10000000.0002791, 0.0389755118489297, 46.7891952053242, 1 ),
      ( 600, 13048.0974399331, 100000000, 0.536267642134758, 47.5887054204918, 1 ),
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
      ( 150, 0.802282603532236, 999.999982037587, -0.000583096263196565, 30.409172754479, 2 ),
      ( 200, 0.601508460470134, 999.999987456536, -0.000244990430308729, 33.9872458043306, 2 ),
      ( 200, 6.02843558714676, 9999.99999999718, -0.00245911570207382, 34.0260467288118, 2 ),
      ( 200, 61.7078272895754, 100000.002645277, -0.0254703128690021, 34.527549484042, 2 ),
      ( 250, 0.481149717369075, 999.999998886985, -0.000126447227207302, 38.6759403050022, 2 ),
      ( 250, 4.81698630355978, 9999.98822882708, -0.00126583776114019, 38.6915583664593, 2 ),
      ( 250, 48.7325395804118, 100000.000063986, -0.0127974419976471, 38.8610967451865, 2 ),
      ( 250, 563.088602472567, 999999.998684142, -0.145624906389681, 41.6323985296594, 2 ),
      ( 250, 18516.5121025331, 100000000.000002, 1.59816143914271, 48.8096424938855, 1 ),
      ( 300, 0.400936623299599, 999.999999953613, -7.28931459179638E-05, 44.3521350324009, 2 ),
      ( 300, 4.0119997785941, 9999.99860665107, -0.000729262380175044, 44.3602054624609, 2 ),
      ( 300, 40.3866162393512, 99999.9999860733, -0.00732610173509321, 44.4426011425652, 2 ),
      ( 300, 434.374107265374, 999999.999999995, -0.0770458204984938, 45.4411586046432, 2 ),
      ( 300, 12651.6405475432, 9999999.99999986, -0.683118251533269, 50.7594497008168, 1 ),
      ( 300, 17357.0434239826, 100000000.012878, 1.30976778717083, 52.5252552533545, 1 ),
      ( 350, 0.34365033625482, 999.999999973632, -4.48824889204373E-05, 50.6178660148118, 2 ),
      ( 350, 3.43789244923356, 9999.99973290614, -0.000448916157487419, 50.62267868248, 2 ),
      ( 350, 34.5187681162093, 99999.9999998038, -0.00449833197445683, 50.6710826249875, 2 ),
      ( 350, 360.183443395807, 1000000.01223173, -0.0459447304815224, 51.1847686172076, 2 ),
      ( 350, 7182.14114235916, 10000000.0000041, -0.521542523933915, 57.7192384821742, 1 ),
      ( 350, 16258.320515501, 100000000, 1.11359415657223, 57.3453687322927, 1 ),
      ( 400, 0.300689194756265, 999.999999994594, -2.87553745082271E-05, 57.0735778671778, 2 ),
      ( 400, 3.00767040968758, 9999.99994551915, -0.000287573539135659, 57.0767347228125, 2 ),
      ( 400, 30.1548316471284, 99999.9999999954, -0.00287770846855806, 57.1083479686587, 2 ),
      ( 400, 309.650189669089, 1000000.00004217, -0.0289670138849165, 57.4291037342645, 2 ),
      ( 400, 4186.62301182482, 9999999.99999999, -0.281806488244564, 60.8039033845695, 2 ),
      ( 400, 15225.2557817164, 99999999.9940704, 0.974880111196416, 62.7275698467996, 1 ),
      ( 500, 0.240547417857187, 999.999999999712, -1.23850695507439E-05, 69.5444102920753, 2 ),
      ( 500, 2.4057423136197, 9999.99999713178, -0.00012383995204003, 69.5460479089738, 2 ),
      ( 500, 24.0842437101659, 99999.9999999999, -0.00123731699640611, 69.5624126538621, 2 ),
      ( 500, 243.529276205985, 999999.999764119, -0.0122565861119602, 69.7248676397171, 2 ),
      ( 500, 2677.1986467202, 9999999.99999999, -0.101506946616104, 71.1598908666515, 2 ),
      ( 500, 13378.6913850649, 100000000.000902, 0.797966869391644, 73.7593435093553, 1 ),
      ( 600, 0.200454712102008, 999.999999999986, -5.05459857447502E-06, 80.7908430710826, 2 ),
      ( 600, 2.00463829181865, 9999.99999986079, -5.05342933971293E-05, 80.7918410202038, 2 ),
      ( 600, 20.0554812605804, 99999.9986741474, -0.000504170003412106, 80.8018098753259, 2 ),
      ( 600, 201.444957953316, 999999.9999999, -0.00492074398641378, 80.9004194673431, 2 ),
      ( 600, 2074.87704450432, 10000000.0000002, -0.0339008308234794, 81.7634080164368, 1 ),
      ( 600, 11839.0574125711, 100000000.003254, 0.693155898332039, 84.1077192427332, 1 ),
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
