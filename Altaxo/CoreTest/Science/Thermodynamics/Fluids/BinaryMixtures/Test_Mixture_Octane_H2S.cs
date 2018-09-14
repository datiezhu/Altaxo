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
  /// Tests and test data for <see cref="Mixture_Octane_H2S"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Octane_H2S : MixtureTestBase
  {

    public Test_Mixture_Octane_H2S()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("111-65-9", 0.5), ("7783-06-4", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481152348244993, 999.99999999482, -0.000131918970346354, 25.4687726310353, 2 ),
      ( 250, 4.81725085331721, 9999.99994691652, -0.00132069145837804, 25.5021134726876, 2 ),
      ( 250, 25792.6842093623, 1000000.00360668, -0.981347855348229, 39.184425746359, 1 ),
      ( 250, 26146.7322881319, 10000000.0000016, -0.816004206636479, 39.3302184606136, 1 ),
      ( 250, 28457.856847553, 99999999.9999968, 0.690530940891922, 40.7303031823002, 1 ),
      ( 300, 0.400936980844839, 999.999999998371, -7.37894250852805E-05, 25.9714271998288, 2 ),
      ( 300, 4.0120362019213, 9999.99998345992, -0.000738338941056484, 25.9861825891958, 2 ),
      ( 300, 40.3907821054769, 99999.9999999988, -0.007428489692044, 26.1348743747987, 2 ),
      ( 300, 435.562214610854, 999999.999999982, -0.0795634182967138, 27.7928151268026, 2 ),
      ( 300, 23182.2866811345, 10000000.0016613, -0.827063049683399, 36.9466173137711, 1 ),
      ( 300, 26653.78761912, 100000000.000876, 0.504129175438933, 38.2870557902621, 1 ),
      ( 350, 0.343650506775286, 999.999999999733, -4.53832399451234E-05, 26.6444537257705, 2 ),
      ( 350, 3.43790987032748, 9999.99999731656, -0.000453985813202684, 26.6519788402574, 2 ),
      ( 350, 34.5207438953117, 100000, -0.00455531360909892, 26.7275792214938, 2 ),
      ( 350, 360.667227139417, 999999.989904474, -0.0472244630969214, 27.5232782786758, 2 ),
      ( 350, 18944.0954857466, 10000000.0000002, -0.818605796691716, 36.0385355854878, 1 ),
      ( 350, 24854.0102526795, 100000000.010454, 0.382613539383259, 36.8123106191432, 1 ),
      ( 400, 0.300689454410414, 999.999999999945, -2.96234491713282E-05, 27.4254687347032, 2 ),
      ( 400, 3.00769662038848, 9999.99999944896, -0.00029629014639442, 27.4297421910776, 2 ),
      ( 400, 30.1575771638041, 99999.9942565066, -0.00296848990361639, 27.4726071268895, 2 ),
      ( 400, 310.065599310546, 999999.999872124, -0.0302679574210828, 27.9148675887291, 2 ),
      ( 400, 5070.66448047388, 9999999.99318873, -0.407019438338726, 34.9255391771692, 2 ),
      ( 400, 23066.3076897587, 100000000, 0.303548669322334, 35.9603370751143, 1 ),
      ( 500, 0.240547790674679, 999.997876964136, -1.39394597402285E-05, 29.1532612212967, 2 ),
      ( 500, 2.40577973833319, 9999.99999999971, -0.000139398763186394, 29.1550127942863, 2 ),
      ( 500, 24.0880315012621, 99999.9999968871, -0.00139437484190495, 29.1725550280873, 2 ),
      ( 500, 243.955574667936, 999999.999999999, -0.0139826159384276, 29.3506030218778, 2 ),
      ( 500, 2805.74583464358, 10000000, -0.142672031831147, 31.3420610028314, 2 ),
      ( 500, 19642.7897210494, 100000000.000101, 0.224594067225182, 35.3861089138593, 1 ),
      ( 600, 0.200455079410543, 999.998357269561, -6.89152160520778E-06, 30.9855447544193, 2 ),
      ( 600, 2.00467512363453, 9999.99999999991, -6.89108949722535E-05, 30.9864504142376, 2 ),
      ( 600, 20.0591838620592, 99999.9999992452, -0.000688665370259903, 30.9955145747273, 2 ),
      ( 600, 201.834675498944, 999999.993057913, -0.00684212217764778, 31.0868911249762, 2 ),
      ( 600, 2140.5459988686, 9999999.99999935, -0.0635394050222915, 32.0486060666336, 2 ),
      ( 600, 16657.0257975355, 100000000.000001, 0.20341830771081, 35.6883464976754, 1 ),
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
      ( 300, 0.401068550980387, 999.989934369847, -0.000404089825857578, 103.117212182909, 2 ),
      ( 300, 10186.4819498455, 10000000.0000123, -0.606432835593406, 121.135834635831, 1 ),
      ( 300, 11228.3083627683, 99999999.9996114, 2.57049760901999, 124.036824081734, 1 ),
      ( 350, 0.343714263822423, 999.999999740987, -0.000233149221806251, 116.188364926702, 2 ),
      ( 350, 3.44438808171002, 9999.98493932229, -0.00233620552366606, 116.264470809655, 2 ),
      ( 350, 9395.7451235983, 10000000.0029216, -0.634266231484264, 131.166400288852, 1 ),
      ( 350, 10752.3802890707, 99999999.9978886, 2.19588889031339, 133.857595024088, 1 ),
      ( 400, 0.300723857779897, 999.999999950502, -0.000146302191023674, 129.330262889764, 2 ),
      ( 400, 3.01120938010832, 9999.99949044246, -0.00146478278940473, 129.371710018707, 2 ),
      ( 400, 30.5205605408772, 99999.9924339141, -0.0148285082606306, 129.792539108059, 2 ),
      ( 400, 8499.83819828084, 10000000.0103511, -0.646252252689479, 142.161221745088, 1 ),
      ( 400, 10297.3193750457, 99999999.9906447, 1.91998189280382, 144.512396121982, 1 ),
      ( 500, 0.240560056171651, 999.999999996838, -6.72064823290834E-05, 153.693987439609, 2 ),
      ( 500, 2.40705727845356, 9999.99996796865, -0.000672351546365883, 153.709267079378, 2 ),
      ( 500, 24.2179192177953, 99999.9999999956, -0.00675245123892491, 153.863142896922, 2 ),
      ( 500, 258.846142885891, 1000000.01557165, -0.0707070772076918, 155.518771518413, 2 ),
      ( 500, 5961.00841067015, 10000000.0000809, -0.596471146480677, 164.95293892606, 1 ),
      ( 500, 9445.01558567269, 99999999.9999999, 1.54678128156159, 165.342610427149, 1 ),
      ( 600, 0.200460154156919, 999.999999999694, -3.44873683679478E-05, 174.582843207054, 2 ),
      ( 600, 2.00522400043949, 9999.99999689969, -0.000344895284375966, 174.589794198524, 2 ),
      ( 600, 20.114742513914, 99999.9999999997, -0.00345112209748019, 174.659572271029, 2 ),
      ( 600, 207.666457975152, 999999.996410329, -0.0347346278530691, 175.383484447926, 2 ),
      ( 600, 2950.23700039884, 10000000.0000086, -0.320552075014326, 182.521439666924, 1 ),
      ( 600, 8673.18558239182, 100000000, 1.31118357735475, 183.868206149635, 1 ),
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
      ( 250, 6455.4152286209, 100000.000000005, -0.992547548627764, 188.907971729883, 1 ),
      ( 250, 6460.86965697607, 1000000.00000443, -0.925538401742718, 188.968377002209, 1 ),
      ( 250, 6513.07285754029, 9999999.99999822, -0.261352220036988, 189.565327252985, 1 ),
      ( 250, 6894.87222639835, 99999999.9999995, 5.97745607157488, 194.849505123624, 1 ),
      ( 300, 0.401539691196623, 999.999992015887, -0.0015792308494964, 180.32286766725, 2 ),
      ( 300, 6104.96962700914, 100000.000000382, -0.993433127569539, 206.667958773195, 1 ),
      ( 300, 6112.42293744509, 1000000.00763093, -0.934411349724712, 206.723346946246, 1 ),
      ( 300, 6182.46080430652, 10000000.0000077, -0.351543697589761, 207.273623240522, 1 ),
      ( 350, 0.343927635746903, 999.999975943298, -0.000855680930978159, 205.761741118433, 2 ),
      ( 350, 3.46622589779054, 9999.98556975219, -0.00862390983691636, 206.171193050091, 2 ),
      ( 350, 5743.68488467263, 100000.000001866, -0.99401719714214, 227.831819358508, 1 ),
      ( 350, 5754.24844201094, 999999.999999719, -0.940281802761358, 227.880255408313, 1 ),
      ( 350, 5850.63392874658, 10000000.0000843, -0.412656222216811, 228.374615347223, 1 ),
      ( 350, 6424.20395831941, 100000000.000002, 4.3490416190044, 232.924726212834, 1 ),
      ( 400, 0.300832913098698, 999.999996031495, -0.000511039845327062, 231.251132471647, 2 ),
      ( 400, 3.02230793229835, 9999.99999999948, -0.00513388365304479, 231.471270814623, 2 ),
      ( 400, 31.7828038788995, 99999.9999890525, -0.0539564204582053, 233.805663318944, 2 ),
      ( 400, 5369.30105581219, 999999.9999999, -0.944000313566482, 250.160157528246, 1 ),
      ( 400, 5507.90802389254, 10000000.0009318, -0.454095503753037, 250.548523369773, 1 ),
      ( 400, 6210.32849356002, 100000000.000001, 3.84159856947928, 254.802308811755, 1 ),
      ( 500, 0.240596487120774, 999.999999787491, -0.000220895688205456, 278.240804807867, 2 ),
      ( 500, 2.41076849935792, 9999.99778043041, -0.00221302635482487, 278.319476860668, 2 ),
      ( 500, 24.6093532606671, 99999.9998572245, -0.0225531818964392, 279.1246036441, 2 ),
      ( 500, 4357.01947803414, 1000000.00223714, -0.944791768291249, 293.904836040342, 1 ),
      ( 500, 4747.15909262776, 10000000.0000025, -0.493289911501966, 293.168642616039, 1 ),
      ( 500, 5813.74878686664, 100000000.000001, 3.13749113029374, 296.592817100996, 1 ),
      ( 600, 0.200475145507401, 999.99999998014, -0.000111544229306472, 318.183158619746, 2 ),
      ( 600, 2.00676793207599, 9999.99979727508, -0.00111627028525794, 318.217447820035, 2 ),
      ( 600, 20.2732876663266, 99999.9999996624, -0.0112467843379097, 318.564384320182, 2 ),
      ( 600, 228.409698512435, 999999.999731345, -0.12239810754274, 322.50101889929, 2 ),
      ( 600, 3797.98886771081, 9999999.99995749, -0.472213345940633, 331.264860511771, 1 ),
      ( 600, 5453.31864004663, 100000000.000002, 2.67579444541415, 333.012647820431, 1 ),
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
