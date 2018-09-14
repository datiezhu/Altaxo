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
  /// Tests and test data for <see cref="Mixture_Ethane_Decane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Ethane_Decane : MixtureTestBase
  {

    public Test_Mixture_Ethane_Decane()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("74-84-0", 0.5), ("124-18-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 5378.19335015094, 1000000.0018546, -0.910548236760668, 226.943060737129, 1 ),
      ( 250, 5415.52306868274, 10000000.0000004, -0.11164836485796, 227.647868826695, 1 ),
      ( 250, 5696.45338219767, 100000000.005576, 7.44541059267071, 234.006005600932, 1 ),
      ( 300, 5104.7380853029, 1000000.00349344, -0.921463669150623, 253.187843690512, 1 ),
      ( 300, 5153.85598761402, 10000000.000002, -0.222121458885003, 253.826252170421, 1 ),
      ( 300, 5496.44400697353, 100000000.009797, 6.29394126995928, 259.581653058698, 1 ),
      ( 350, 0.344176662502582, 999.999992142958, -0.00157404665877116, 257.781105558602, 2 ),
      ( 350, 4831.51666530974, 1000000.00000427, -0.928876388890229, 281.750539089083, 1 ),
      ( 350, 4897.0757019421, 10000000.0000008, -0.298285480430905, 282.320953833601, 1 ),
      ( 350, 5312.31090865173, 99999999.9999992, 5.46865212299154, 287.622858363821, 1 ),
      ( 400, 0.300956373045682, 999.999998461105, -0.000916494030548959, 289.487173021221, 2 ),
      ( 400, 3.03485118620514, 9999.98365981984, -0.00924119651280854, 289.950829438612, 2 ),
      ( 400, 4538.70915589037, 100000.000000342, -0.993375196823602, 310.625784735278, 1 ),
      ( 400, 4548.74136165959, 999999.999999573, -0.933898077639639, 310.668726017976, 1 ),
      ( 400, 4638.5640715234, 10000000.0000272, -0.351780974278311, 311.137086310605, 1 ),
      ( 400, 5140.26256252085, 100000000.000001, 4.84951730906143, 316.037249313936, 1 ),
      ( 500, 0.240636242468689, 999.999894563033, -0.00038150445408688, 347.858201266497, 2 ),
      ( 500, 2.41468595627598, 9999.99999961075, -0.00382723460055304, 348.022477170006, 2 ),
      ( 500, 25.0458941804576, 100000.011988481, -0.0395853480334671, 349.735693627199, 2 ),
      ( 500, 3895.82071817559, 1000000.00000009, -0.938255773029172, 365.506889853902, 1 ),
      ( 500, 4093.46819640233, 10000000.0012533, -0.41237008046314, 365.310946706877, 1 ),
      ( 500, 4824.60207031075, 100000000.000994, 3.98578815739206, 369.272015576252, 1 ),
      ( 600, 0.200491613082016, 999.999999484119, -0.00018910615515164, 397.109769482457, 2 ),
      ( 600, 2.00833968915399, 9999.99999999937, -0.00189345474410069, 397.180638570049, 2 ),
      ( 600, 20.4373817351677, 100000.000213715, -0.0191811188295531, 397.903604516077, 2 ),
      ( 600, 259.679545487906, 999999.998745938, -0.228072821052995, 407.324393010154, 2 ),
      ( 600, 3472.14173373159, 10000000.004285, -0.422679964320452, 412.414091602644, 1 ),
      ( 600, 4540.684861044, 100000000.000003, 3.41461376462527, 414.808325467699, 1 ),
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
      ( 250, 8076.17183158562, 1000000.00093476, -0.940431074574087, 137.043765585614, 1 ),
      ( 250, 8162.73535337, 9999999.99999915, -0.410627863714837, 137.480169839816, 1 ),
      ( 300, 7668.65936488497, 10000000.0000262, -0.47721318852654, 152.380018390585, 1 ),
      ( 350, 0.343773985872871, 999.999999996466, -0.000404549227606243, 154.290174557161, 2 ),
      ( 350, 7156.39742552257, 10000000.0010186, -0.51982136825024, 169.039703059888, 1 ),
      ( 350, 8093.05671604205, 100000000.000568, 3.24604601736008, 172.709562233157, 1 ),
      ( 400, 0.300756477011949, 999.999984049526, -0.000252459018113522, 173.401851652043, 2 ),
      ( 400, 3.01443218045863, 9999.99998680929, -0.00253006096124634, 173.479334176716, 2 ),
      ( 400, 6606.959664094, 10000000.0002203, -0.544903308603419, 186.257853072235, 1 ),
      ( 400, 7786.7514113887, 100000000.001443, 2.86143761947231, 189.549925642689, 1 ),
      ( 500, 0.240572314101176, 999.99999877128, -0.000115871356845054, 208.86314823851, 2 ),
      ( 500, 2.40823681982611, 9999.98714605686, -0.00115953281807243, 208.892001078548, 2 ),
      ( 500, 24.3386921344933, 99999.9861626421, -0.0116788617592517, 209.185055990181, 2 ),
      ( 500, 5285.86997857238, 10000000.0000002, -0.544929331149239, 219.82602863549, 1 ),
      ( 500, 7221.11044107954, 100000000.015526, 2.33112809526592, 221.547404133601, 1 ),
      ( 600, 0.200465796527567, 999.999999992927, -6.0347669628455E-05, 239.140264809976, 2 ),
      ( 600, 2.00574755340936, 9999.99898020481, -0.000603547761874824, 239.153503663721, 2 ),
      ( 600, 20.1672332663369, 99999.9999944333, -0.00604264231206699, 239.286758149814, 2 ),
      ( 600, 213.518577977731, 1000000.00002795, -0.0611884886923413, 240.707168929378, 2 ),
      ( 600, 3531.76728397116, 10000000.0000044, -0.432426649984657, 249.590514607225, 1 ),
      ( 600, 6713.72215475025, 100000000.000023, 1.98573122723158, 249.401496836268, 1 ),
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
      ( 250, 0.481150384567698, 999.999997769657, -0.000127833725503679, 38.8592275484853, 2 ),
      ( 250, 15593.9628626486, 10000000.0002894, -0.69149030200226, 46.2749444231149, 1 ),
      ( 250, 18471.0598739462, 100000000.000602, 1.60455480415324, 49.0569088076063, 1 ),
      ( 300, 0.400936944003771, 999.999999940274, -7.36929744179209E-05, 44.5655532618928, 2 ),
      ( 300, 4.01203192845422, 9999.99939086083, -0.000737269954041119, 44.5737529174372, 2 ),
      ( 300, 40.3899125430712, 99999.9999979743, -0.00740711588871922, 44.6575108447904, 2 ),
      ( 300, 12682.5605238498, 10000000.000004, -0.683890806581185, 50.9888563898528, 1 ),
      ( 300, 17317.5779959572, 99999999.9998932, 1.31503157000981, 52.7923944467284, 1 ),
      ( 350, 0.343650509745828, 999.999999984513, -4.53873134935147E-05, 50.8632956777648, 2 ),
      ( 350, 3.43790982227053, 9999.99984330643, -0.000453967265746711, 50.8681784942533, 2 ),
      ( 350, 34.5205296633842, 99999.9999999332, -0.00454913140153576, 50.9172964561725, 2 ),
      ( 350, 360.387316623321, 1000000.01798412, -0.0464844454478278, 51.4394704303169, 2 ),
      ( 350, 7294.14471604667, 10000000, -0.528889368459561, 58.0511119781186, 1 ),
      ( 350, 16224.292433018, 99999999.9996191, 1.11802711144433, 57.6356331585109, 1 ),
      ( 400, 0.30068929646973, 999.99999999978, -2.90936324438615E-05, 57.3507795712878, 2 ),
      ( 400, 3.00768058922316, 9999.99996848484, -0.000290957079993937, 57.3539788794598, 2 ),
      ( 400, 30.1558578437117, 99999.9999999976, -0.00291164029994216, 57.3860194900515, 2 ),
      ( 400, 309.761617790352, 1000000.00002416, -0.0293163159770834, 57.7113472850946, 2 ),
      ( 400, 4211.24628962848, 9999999.99999999, -0.286005786300413, 61.1531781374955, 2 ),
      ( 400, 15196.0415500903, 99999999.999995, 0.978676797735265, 63.0424417879245, 1 ),
      ( 500, 0.240547458783746, 999.999999999698, -1.25552066715187E-05, 69.8800592712154, 2 ),
      ( 500, 2.40574640727855, 9999.9999969918, -0.000125541358254991, 69.8817162429737, 2 ),
      ( 500, 24.0846540777835, 100000, -0.00125433446574619, 69.8982746943281, 2 ),
      ( 500, 243.571299731291, 999999.999734977, -0.0124270021679489, 70.0626813098186, 2 ),
      ( 500, 2681.89019072308, 9999999.99999999, -0.103078718536823, 71.5163747578041, 2 ),
      ( 500, 13357.0800766956, 100000000.001364, 0.800875919595846, 74.122662558929, 1 ),
      ( 600, 0.200454730856523, 999.999999999983, -5.14815795625297E-06, 81.1757910215542, 2 ),
      ( 600, 2.00464016725029, 9999.99999985195, -5.14697913945003E-05, 81.1767996663117, 2 ),
      ( 600, 20.0556687823417, 99999.9985890196, -0.00051351535156363, 81.1868754210797, 2 ),
      ( 600, 201.463673492344, 999999.999999885, -0.00501318469343644, 81.2865477294982, 2 ),
      ( 600, 2076.54071208139, 10000000.0000003, -0.0346748430326499, 82.1588948936834, 1 ),
      ( 600, 11822.5312145826, 100000000.002647, 0.695522686714312, 84.5140464422814, 1 ),
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
