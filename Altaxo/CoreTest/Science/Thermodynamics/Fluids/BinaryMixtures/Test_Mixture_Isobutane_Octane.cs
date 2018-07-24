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
	/// Tests and test data for <see cref="Mixture_Isobutane_Octane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Isobutane_Octane : MixtureTestBase
    {

    public Test_Mixture_Isobutane_Octane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("75-28-5", 0.5), ("111-65-9", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 6452.13237755745, 99999.9999998099, -0.992543756813492, 188.957501954285, 1 ),
      ( 250, 6457.58407937239, 1000000.00000753, -0.925500516156085, 189.017942042769, 1 ),
      ( 250, 6509.76130863024, 9999999.99999807, -0.260976466128689, 189.615210641228, 1 ),
      ( 300, 0.401540241244568, 999.99999195726, -0.00158059853122053, 180.38596042511, 2 ),
      ( 300, 6101.92058586399, 100000.000000614, -0.993429846198672, 206.730626186949, 1 ),
      ( 300, 6109.36978328157, 1000000.0049204, -0.934378572010689, 206.786053854784, 1 ),
      ( 300, 6179.36934117372, 10000000.0000037, -0.35121928280849, 207.336689003692, 1 ),
      ( 350, 0.343927888787118, 999.999975926022, -0.00085641603764038, 205.837887386392, 2 ),
      ( 350, 3.46625203567049, 9999.98553038447, -0.00863138543606963, 206.247707382897, 2 ),
      ( 350, 5740.88491838133, 100000.00000135, -0.994014279186717, 227.907563407236, 1 ),
      ( 350, 5751.44190332787, 1000000, -0.940252661993959, 227.956044949662, 1 ),
      ( 350, 5847.76858419611, 10000000.0000838, -0.41236842999837, 228.450810023587, 1 ),
      ( 400, 0.300833044222768, 999.999996028813, -0.000511475492481001, 231.340056879326, 2 ),
      ( 400, 3.0223212946628, 9999.99999999949, -0.0051382821805714, 231.560394156588, 2 ),
      ( 400, 31.7844482136359, 99999.9999912518, -0.0540053630212269, 233.897085039653, 2 ),
      ( 400, 5366.77839602088, 1000000.00000018, -0.94397399085535, 250.248536206727, 1 ),
      ( 400, 5505.28793984033, 10000000.000927, -0.453835696150007, 250.637383554336, 1 ),
      ( 500, 0.24059653200908, 999.99999978736, -0.000221082217867788, 278.352844871619, 2 ),
      ( 500, 2.41077302308305, 9999.99777898376, -0.00221489866390011, 278.431589085061, 2 ),
      ( 500, 24.6098434081528, 99999.9998568254, -0.0225726494358858, 279.237476657992, 2 ),
      ( 500, 4355.48862481537, 1000000.00224429, -0.944772363878715, 294.014599577997, 1 ),
      ( 500, 4745.15181032446, 10000000.0000029, -0.49307556426203, 293.280118843604, 1 ),
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
      ( 250, 7901.34687234286, 100000.000000704, -0.993911319390087, 137.498192071156, 1 ),
      ( 250, 7910.61777116767, 1000000.00639849, -0.939184549843939, 137.554372061624, 1 ),
      ( 250, 7998.12215582153, 10000000.000003, -0.398499086944649, 138.097691615555, 1 ),
      ( 300, 0.401168697362875, 999.999996862643, -0.000653629598407902, 134.634286677221, 2 ),
      ( 300, 7383.06946068904, 1000000.00236324, -0.945699212982779, 151.831494831472, 1 ),
      ( 300, 7509.04709005038, 10000000.0000014, -0.466102054202405, 152.339930761659, 1 ),
      ( 350, 0.343763185514563, 999.999999570211, -0.000375428232792185, 154.247029717451, 2 ),
      ( 350, 3.44933539254303, 9999.99539544512, -0.00376713596333935, 154.374016054957, 2 ),
      ( 350, 6804.23647445927, 1000000.01503625, -0.949497032687297, 168.487589960737, 1 ),
      ( 350, 6997.15190387569, 9999999.99999975, -0.508894287520828, 168.899284567141, 1 ),
      ( 400, 0.300750484541829, 999.999999920027, -0.000234823632360437, 173.784566947736, 2 ),
      ( 400, 3.01388996627757, 9999.99916613312, -0.00235289063312899, 173.854152202516, 2 ),
      ( 400, 30.807837130354, 99999.9890780852, -0.0240150232703808, 174.572755170765, 2 ),
      ( 400, 6442.61985131308, 10000000.0000001, -0.533295665180917, 186.22204425551, 1 ),
      ( 500, 0.240569814947508, 999.999999995004, -0.000107769010946821, 209.711084223315, 2 ),
      ( 500, 2.40803584433196, 9999.99994908147, -0.00107845344551661, 209.737014016484, 2 ),
      ( 500, 24.3185385254919, 99999.9999999805, -0.0108620680953518, 209.999626993645, 2 ),
      ( 500, 272.675922798065, 999999.999999557, -0.117839644556201, 213.019635072997, 2 ),
      ( 500, 5080.45260838308, 9999999.9998884, -0.52653059182686, 219.994871050538, 1 ),
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
      ( 250, 0.481306022784855, 999.999999997157, -0.000451163401107085, 75.7231353700369, 2 ),
      ( 250, 4.83280126598187, 9999.99996974733, -0.00453412283906586, 75.8446812679571, 2 ),
      ( 250, 10424.9903811595, 99999.9999994139, -0.995385234376234, 87.6817274215976, 1 ),
      ( 250, 10443.4243126303, 1000000.00000123, -0.95393379982432, 87.7394264790058, 1 ),
      ( 250, 10612.8451325664, 10000000.0000531, -0.546691891653454, 88.2916035413977, 1 ),
      ( 300, 0.401007553809777, 999.999999999579, -0.000249765555031227, 88.9246641628949, 2 ),
      ( 300, 4.01913530617493, 9999.99999562922, -0.00250336105556118, 88.9798481671485, 2 ),
      ( 300, 41.1454532136303, 99999.9999999995, -0.0256337830233982, 89.5680045833116, 2 ),
      ( 300, 9452.10173555669, 1000000.00000032, -0.95758537020106, 98.3755158682734, 1 ),
      ( 300, 9738.34001651852, 10000000.0021817, -0.58832060152088, 98.8157708508703, 1 ),
      ( 350, 0.343687371664383, 999.999999999969, -0.000152641228125883, 102.676651062488, 2 ),
      ( 350, 3.44160864259222, 9999.99999966738, -0.00152821982885777, 102.705348484897, 2 ),
      ( 350, 34.9033765836732, 99999.994698468, -0.0154680013028818, 103.001183417633, 2 ),
      ( 350, 419.65091670566, 999999.999930411, -0.181141045737557, 107.19777693732, 2 ),
      ( 350, 8753.45648419268, 10000000.0000003, -0.607429463524196, 110.807426955182, 1 ),
      ( 400, 0.300710459449887, 999.99830099157, -9.94725872551208E-05, 116.240128601037, 2 ),
      ( 400, 3.00980133296653, 9999.99999997877, -0.00099536916827725, 116.25680089041, 2 ),
      ( 400, 30.3723510065419, 99999.9996391322, -0.0100188590583002, 116.42621809195, 2 ),
      ( 400, 337.066566012174, 999999.999510339, -0.107949060245015, 118.448971953835, 2 ),
      ( 400, 7553.20626358945, 10000000.0000002, -0.601916674245881, 123.607723015408, 1 ),
      ( 500, 0.240555839691352, 999.998391089631, -4.73990226344988E-05, 141.073381509355, 2 ),
      ( 500, 2.40658524283017, 9999.99999999936, -0.000474060588695275, 141.080315907117, 2 ),
      ( 500, 24.16918900745, 99999.9999924257, -0.00474758382486985, 141.149950148206, 2 ),
      ( 500, 252.722802781556, 999999.999999994, -0.0481886283556563, 141.876268110821, 2 ),
      ( 500, 4104.31853445561, 9999999.99999999, -0.413923564796557, 148.010456514943, 1 ),
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
