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
  /// Tests and test data for <see cref="Mixture_Nitrogen_Butane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Nitrogen_Butane : MixtureTestBase
  {

    public Test_Mixture_Nitrogen_Butane()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("7727-37-9", 0.5), ("106-97-8", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 150, 12415.9804416135, 1000000.00037005, -0.935420742982887, 83.822385092634, 1 ),
      ( 150, 12485.2084982909, 9999999.99999501, -0.357788224381687, 84.3079810279053, 1 ),
      ( 150, 13044.0652913749, 100000000.000105, 5.14697008916421, 87.8954143503926, 1 ),
      ( 200, 0.60204980077731, 999.999999212256, -0.00114393671820103, 68.1327634724908, 2 ),
      ( 200, 11601.6596074842, 100000.000000047, -0.994816594246961, 85.567251758357, 1 ),
      ( 200, 11612.1041339928, 1000000.00305156, -0.948212564356434, 85.6120058941321, 1 ),
      ( 200, 11712.0668479185, 9999999.99999895, -0.486545712465634, 86.0461156889625, 1 ),
      ( 200, 12445.659352229, 100000000.001529, 3.83189421221217, 89.4607202233133, 1 ),
      ( 250, 0.481339380653931, 999.999987012113, -0.000520434309134485, 78.4908383826127, 2 ),
      ( 250, 4.83619821229371, 9999.9969345534, -0.00523333660197155, 78.6568209426731, 2 ),
      ( 250, 10772.8474723466, 1000000.00081422, -0.955342459224119, 91.4053537765511, 1 ),
      ( 250, 10922.362269794, 9999999.999999, -0.559537705091523, 91.8446791103235, 1 ),
      ( 250, 11882.6817342381, 100000000.015784, 3.04865573237886, 95.347969501811, 1 ),
      ( 300, 0.401020077118382, 999.99999856013, -0.000280986387267965, 90.5724238326746, 2 ),
      ( 300, 4.02040035256901, 9999.98453329914, -0.00281722580662404, 90.6459323051209, 2 ),
      ( 300, 41.2860438248409, 100000.006886877, -0.0289517765682095, 91.4255754368168, 2 ),
      ( 300, 9842.99824992951, 1000000.0000004, -0.959269788965097, 100.463466964744, 1 ),
      ( 300, 10084.2179342174, 10000000.000587, -0.602440765787458, 100.877117700732, 1 ),
      ( 300, 11350.9344985349, 100000000.000002, 2.53193295219245, 104.426648938336, 1 ),
      ( 350, 0.343692825054102, 999.999999767642, -0.000168505851697357, 103.484474724175, 2 ),
      ( 350, 3.44215750477098, 9999.99758441742, -0.00168742869422454, 103.521902001855, 2 ),
      ( 350, 34.9619934801231, 99999.9998844447, -0.0171186577096205, 103.907456145448, 2 ),
      ( 350, 8694.43156881544, 999999.999999989, -0.960476437351649, 111.639801103076, 1 ),
      ( 350, 9149.40460402124, 10000000.0000002, -0.624418281108179, 111.729102999885, 1 ),
      ( 350, 10846.7661725153, 100000000.000022, 2.16808627878325, 115.18257944683, 1 ),
      ( 400, 0.300713053570823, 999.999999997274, -0.000108098463940559, 116.361981237309, 2 ),
      ( 400, 3.01006196132329, 9999.99960259429, -0.00108186865200191, 116.383133603321, 2 ),
      ( 400, 30.3996729491248, 99999.9999980168, -0.0109086122844233, 116.598061211907, 2 ),
      ( 400, 341.695144866589, 999999.998335412, -0.120032720553586, 119.171502738701, 2 ),
      ( 400, 8033.27765241172, 10000000.0000105, -0.625706268397602, 123.474556491115, 1 ),
      ( 400, 10367.8169233315, 100000000.000227, 1.90013364602963, 126.445667621807, 1 ),
      ( 500, 0.240556464215363, 999.99999999685, -4.99951397518193E-05, 140.22005428678, 2 ),
      ( 500, 2.40664794023969, 9999.99996813987, -0.000500099994896897, 140.228461222634, 2 ),
      ( 500, 24.1757070391082, 99999.9999999971, -0.00501591461894227, 140.31289620162, 2 ),
      ( 500, 253.659490631221, 1000000.00037844, -0.0517033801589758, 141.195428951788, 2 ),
      ( 500, 4600.72547601271, 10000000, -0.477159767920368, 147.706610071133, 1 ),
      ( 500, 9481.36303737416, 100000000.00707, 1.53702380793958, 148.078871920773, 1 ),
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
      ( 200, 0.601525764551185, 999.999999998, -0.000276034909475353, 44.4680972453899, 2 ),
      ( 250, 0.481153931417348, 999.999996828917, -0.000137489189726809, 49.6663270544171, 2 ),
      ( 250, 4.81750846320218, 9999.99999854109, -0.00137637198140911, 49.6886144905497, 2 ),
      ( 300, 0.400937436839138, 999.99999961236, -7.72070871995741E-05, 55.7241417211301, 2 ),
      ( 300, 4.01216404401072, 9999.99599333075, -0.000772457571304462, 55.7347595511501, 2 ),
      ( 300, 40.4043350954619, 99999.9972081836, -0.00776369464601944, 55.8420639729139, 2 ),
      ( 350, 0.343650052501346, 999.9999999396, -4.63418875169793E-05, 62.2082353205004, 2 ),
      ( 350, 3.43793483198336, 9999.99938627095, -0.000463522687506242, 62.214068726371, 2 ),
      ( 350, 34.5237979451247, 99999.9999988612, -0.00464564280427157, 62.2726525052637, 2 ),
      ( 350, 360.781025080129, 1000000.00007192, -0.0475271612976818, 62.8842274287873, 2 ),
      ( 400, 0.300688518651252, 999.999999999389, -2.87920226624918E-05, 68.6956767584765, 2 ),
      ( 400, 3.00766463352727, 9999.99991247615, -0.000287938124460669, 68.6992465888031, 2 ),
      ( 400, 30.1548669841645, 99999.9999999826, -0.00288115553635999, 68.7349932007728, 2 ),
      ( 400, 309.650617278911, 1000000.00000698, -0.0289705737944177, 69.0970319419462, 2 ),
      ( 500, 0.240546574937477, 999.999999999785, -1.11660745569824E-05, 80.8023087871729, 2 ),
      ( 500, 2.40570747104287, 9999.99999785696, -0.000111643365280384, 80.8039987414153, 2 ),
      ( 500, 24.0812319097836, 100000, -0.00111468600231293, 80.8208864012638, 2 ),
      ( 500, 243.209970461125, 999999.999945714, -0.0109620566929936, 80.9884553851472, 2 ),
      ( 500, 2610.89978659534, 10000000.0000001, -0.0786935208640823, 82.4000893608179, 1 ),
      ( 500, 11469.0337170622, 99999999.9999999, 1.09733352356122, 85.5645570080728, 1 ),
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
      ( 200, 0.601371401288344, 999.999999877676, -2.17009038244681E-05, 20.8405843003701, 2 ),
      ( 200, 6.01488867238001, 9999.99879097584, -0.000216988615968164, 20.8425414226964, 2 ),
      ( 200, 60.2664823902037, 99999.9999992582, -0.00216782673153685, 20.8621215505438, 2 ),
      ( 200, 614.540656616933, 1000000.01158379, -0.0214506650460467, 21.0586591082595, 2 ),
      ( 200, 7136.14597330195, 10000000.0010448, -0.157306544413237, 22.7771304607254, 1 ),
      ( 250, 0.481090539714061, 999.999999995974, -8.02120478899202E-06, 20.8550716156944, 2 ),
      ( 250, 4.81125259773285, 9999.99996139942, -8.01849082597677E-05, 20.8560917140051, 2 ),
      ( 250, 48.147143843632, 99999.9999999998, -0.000799128706786297, 20.8662832212347, 2 ),
      ( 250, 484.825952432745, 999999.990373328, -0.00771260612420319, 20.967206102427, 2 ),
      ( 250, 5023.63241882907, 10000000.0000064, -0.0423529416978033, 21.8355644742236, 1 ),
      ( 300, 0.400906340289926, 999.999999999996, -1.89025134253169E-06, 20.8815343611397, 2 ),
      ( 300, 4.00913137495829, 9999.99999995883, -1.88823251746732E-05, 20.8821671685148, 2 ),
      ( 300, 40.0980471845512, 99999.9997574462, -0.000186803415605025, 20.8884884035218, 2 ),
      ( 300, 401.57415771395, 999999.999999976, -0.00166492384374316, 20.9510056652375, 2 ),
      ( 300, 3990.2892627991, 10000000.0000355, 0.00470302005728392, 21.5002807817222, 1 ),
      ( 300, 20362.9857142932, 100000000.000035, 0.968795602710992, 24.0607978111839, 1 ),
      ( 350, 0.343632944499609, 1000, 1.13812294589955E-06, 20.9347070667682, 1 ),
      ( 350, 3.43629427661759, 10000.0000000075, 1.13952943328168E-05, 20.9351466155182, 1 ),
      ( 350, 34.3593706805233, 100000.000113185, 0.000115358966871743, 20.9395379736999, 1 ),
      ( 350, 343.189330976421, 1000000.00000009, 0.00129378277075687, 20.9830353752552, 1 ),
      ( 350, 3347.23767940142, 10000000.0000017, 0.0266176959434866, 21.375564990894, 1 ),
      ( 350, 18562.8253478823, 100000000.000009, 0.851190952779434, 23.5668523844319, 1 ),
      ( 400, 0.300678343491441, 1000.00000000001, 2.71281980069745E-06, 21.0307448466263, 1 ),
      ( 400, 3.00671015879597, 10000.0000000757, 2.71380102240559E-05, 21.0310746484859, 1 ),
      ( 400, 30.0597304683913, 100000.000851704, 0.000272360424428456, 21.0343701818479, 1 ),
      ( 400, 299.833373815753, 1000000.00000062, 0.00282090570673821, 21.0670757242878, 1 ),
      ( 400, 2898.57255460623, 10000000.0000004, 0.0373353429255386, 21.3691112924657, 1 ),
      ( 400, 17052.706542184, 99999999.9999998, 0.763234327342094, 23.2676952753446, 1 ),
      ( 500, 0.240542366578577, 1000.00000000001, 3.96903263345325E-06, 21.3848743342917, 1 ),
      ( 500, 2.40533792321375, 10000.0000001544, 3.96953488019293E-05, 21.3850897564901, 1 ),
      ( 500, 24.044777326929, 100000.001597421, 0.00039745481980049, 21.3872429899083, 1 ),
      ( 500, 239.579251982291, 1000000.00000091, 0.00402408973437276, 21.4086759317173, 1 ),
      ( 500, 2302.48557565254, 10000000.0101041, 0.0447116062277588, 21.6129400879341, 1 ),
      ( 500, 14685.144791967, 100000000.000064, 0.63800455359355, 23.0955333379299, 1 ),
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
