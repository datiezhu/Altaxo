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
  /// Tests and test data for <see cref="Mixture_CO2_Helium"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_CO2_Helium : MixtureTestBase
  {

    public Test_Mixture_CO2_Helium()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("124-38-9", 0.5), ("7440-59-7", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481086077845228, 1000.00000000006, 5.81450499751649E-06, 12.4857719843787, 1 ),
      ( 250, 4.81060904101443, 10000.0000005724, 5.81444489727013E-05, 12.4858802484542, 1 ),
      ( 250, 48.0809340117146, 100000.005685464, 0.000581384341016067, 12.4869626137557, 1 ),
      ( 250, 478.310963333173, 1000000.00000808, 0.005807752662084, 12.4977587549667, 1 ),
      ( 250, 4549.48954593729, 10000000.0150058, 0.0574568208229374, 12.6030088894953, 1 ),
      ( 250, 31466.5221112961, 100000000.000455, 0.528891160649205, 13.4366747613019, 1 ),
      ( 300, 0.400905484103392, 1000, 4.76878518988941E-06, 12.4881570954858, 1 ),
      ( 300, 4.00888278628607, 10000.0000000275, 4.768736812506E-05, 12.4882459666354, 1 ),
      ( 300, 40.0716324265311, 100000.000274195, 0.0004768252730437, 12.4891344793129, 1 ),
      ( 300, 399.006773732311, 1000000.00000002, 0.00476338327988131, 12.4979997585425, 1 ),
      ( 300, 3828.57654125441, 10000000.0000224, 0.0471447850542695, 12.5847035631843, 1 ),
      ( 300, 27920.1080750351, 99999999.9999998, 0.435909183653911, 13.2935001616938, 1 ),
      ( 350, 0.343633530683338, 1000, 4.01625108937187E-06, 12.4903241025661, 1 ),
      ( 350, 3.4362111024675, 10000.000000012, 4.01621283421222E-05, 12.4903990018802, 1 ),
      ( 350, 34.3496968253923, 100000.000119139, 0.000401583014889156, 12.4911478468087, 1 ),
      ( 350, 342.261759664215, 1000000, 0.00401199109883042, 12.4986215013862, 1 ),
      ( 350, 3305.01818305185, 10000000.0000016, 0.0397368237307855, 12.5719063539371, 1 ),
      ( 350, 25098.9721952299, 99999999.9999999, 0.369119452896117, 13.1857851635344, 1 ),
      ( 400, 0.300679509083172, 1000.01360610483, 3.45179023827552E-06, 12.492262745826, 1 ),
      ( 400, 3.00670168680772, 10000.0000000032, 3.4517128583352E-05, 12.4923271525059, 1 ),
      ( 400, 30.0576805613136, 100000.000031955, 0.000345140864507859, 12.4929711066384, 1 ),
      ( 400, 299.647254995108, 1000000, 0.00344836115417859, 12.4993993040429, 1 ),
      ( 400, 2907.42815968638, 10000000.0079011, 0.0341804868253573, 12.5625678310016, 1 ),
      ( 400, 22796.9512538978, 100000000.006115, 0.318950694797076, 13.1020831736702, 1 ),
      ( 500, 0.240543783294507, 1000.01097122905, 2.66664570413795E-06, 12.4955593325348, 1 ),
      ( 500, 2.40538023382449, 10000.0000000016, 2.66659664747908E-05, 12.4956091263464, 1 ),
      ( 500, 24.0480315957026, 100000.000016378, 0.000266639720780433, 12.4961069935672, 1 ),
      ( 500, 239.905233592278, 1000000, 0.00266440193305183, 12.5010785275796, 1 ),
      ( 500, 2343.46882972633, 10000000.0018855, 0.0264460722123871, 12.5500920863931, 1 ),
      ( 500, 19260.1492273596, 100000000.000658, 0.24892302090741, 12.9809895698382, 1 ),
      ( 600, 0.200453258191652, 1000.0072590075, 2.15090623314938E-06, 12.4982564726033, 1 ),
      ( 600, 2.00449386548086, 10000.0000000006, 2.15087696273188E-05, 12.4982966537889, 1 ),
      ( 600, 20.041059487017, 100000.000005811, 0.000215073946681501, 12.4986984177482, 1 ),
      ( 600, 200.023773938873, 999999.999999999, 0.00214936464815026, 12.5027112426132, 1 ),
      ( 600, 1962.62061162247, 10000000.0002143, 0.0213573462990393, 12.5423652878184, 1 ),
      ( 600, 16668.6708088072, 100000000, 0.202577579622304, 12.8981687380858, 1 ),
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
      ( 350, 0.343634964834859, 1000, -2.39003823647375E-06, 21.7756444697608, 2 ),
      ( 350, 3.43642337639297, 9999.99999999203, -2.38926614414609E-05, 21.7758705802246, 2 ),
      ( 350, 34.3715985019195, 99999.9999246711, -0.000238155667567212, 21.7781306846731, 2 ),
      ( 350, 344.428173467907, 999999.999999999, -0.00230540478139791, 21.8006295866897, 2 ),
      ( 350, 3490.62955439644, 10000000.0029098, -0.0155525765379842, 22.0128174744942, 1 ),
      ( 400, 0.300680046579499, 999.994257860689, -6.04376947205269E-07, 22.7455434250792, 2 ),
      ( 400, 3.00681676776771, 9999.99999999988, -6.0381336901664E-06, 22.7456995555145, 2 ),
      ( 400, 30.0697847470645, 99999.9999991536, -5.98150275665643E-05, 22.7472601758333, 2 ),
      ( 400, 300.842979553259, 999999.999241805, -0.000542204218223405, 22.7627981477986, 2 ),
      ( 400, 3007.06948630422, 10000000.0000001, -9.007909506876E-05, 22.9111679913458, 1 ),
      ( 400, 21046.1306415991, 100000000.000052, 0.428670506427269, 24.0182146948742, 1 ),
      ( 500, 0.24054361329759, 1000.00853615963, 1.12360244743259E-06, 24.3946454589986, 1 ),
      ( 500, 2.4054118554211, 10000.0000000004, 1.1238966711213E-05, 24.3947390264052, 1 ),
      ( 500, 24.0516784397767, 100000.000004333, 0.000112693086219594, 24.3956744105626, 1 ),
      ( 500, 240.265920964327, 1000000, 0.00115691818030589, 24.4049997448813, 1 ),
      ( 500, 2371.39260364443, 10000000.0000001, 0.0143570853970452, 24.4958418868228, 1 ),
      ( 500, 17618.6973989134, 99999999.9999997, 0.365276237682141, 25.309231876537, 1 ),
      ( 600, 0.200452874133485, 1000.0113161107, 1.79341072369982E-06, 25.7437497192296, 1 ),
      ( 600, 2.00449645632096, 10000.0000000012, 1.79355849308683E-05, 25.7438170597217, 1 ),
      ( 600, 20.0417261174595, 100000.00001182, 0.000179523654486467, 25.7444903308377, 1 ),
      ( 600, 200.090710562601, 1000000, 0.00181182949405873, 25.751209953838, 1 ),
      ( 600, 1965.90182618378, 10000000.00867, 0.0196503108524324, 25.8174052552596, 1 ),
      ( 600, 15164.8333698578, 100000000.000021, 0.321829498055555, 26.4515645383947, 1 ),
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
      ( 250, 0.481129158839782, 999.999999868009, -8.82882499967472E-05, 26.5120597432335, 2 ),
      ( 250, 4.81512082315776, 9999.99864358851, -0.000883469979816047, 26.5335473405445, 2 ),
      ( 250, 48.5404112131658, 99999.9999856073, -0.00889450905869376, 26.7508652285812, 2 ),
      ( 250, 532.320235156384, 999999.996767871, -0.0962457385647636, 29.331471901056, 2 ),
      ( 250, 24444.1445271181, 10000000.0025842, -0.803189397618354, 41.4319280197541, 1 ),
      ( 250, 28074.2199821485, 100000000.000292, 0.71362438954851, 43.5079399697781, 1 ),
      ( 300, 0.400925009134624, 999.9999999952, -4.84923878057044E-05, 28.8956075758299, 2 ),
      ( 300, 4.01100127997082, 9999.99985734159, -0.00048506758566153, 28.9050013396322, 2 ),
      ( 300, 40.2865570313468, 99999.9999999255, -0.00486515385010493, 28.9994376668507, 2 ),
      ( 300, 422.101959420927, 999999.999999997, -0.0502162845356261, 30.0004177425923, 2 ),
      ( 300, 18166.6779189021, 9999999.99994272, -0.779318173024522, 41.7843323353235, 1 ),
      ( 300, 25662.1574127073, 99999999.9999999, 0.562244206043326, 41.5503880063012, 1 ),
      ( 350, 0.343643284281501, 999.999999997507, -2.89278496824569E-05, 31.0615046765257, 2 ),
      ( 350, 3.43732789610679, 9999.99997485732, -0.000289312492740846, 31.0663607476443, 2 ),
      ( 350, 34.4631581307544, 99999.999999999, -0.00289653630073442, 31.1150546249332, 2 ),
      ( 350, 354.012399106646, 1000000.00001995, -0.0293183394503094, 31.6156178518613, 2 ),
      ( 350, 5188.10539466063, 9999999.99982716, -0.337651575510963, 38.2586482442047, 2 ),
      ( 350, 23350.7251415035, 99999999.9999989, 0.471617439449429, 40.7359638610742, 1 ),
      ( 400, 0.300684609987575, 999.999999999522, -1.80737380464796E-05, 32.9991558231337, 2 ),
      ( 400, 3.00733530066546, 9999.99999521559, -0.000180739985354913, 33.0019686646837, 2 ),
      ( 400, 30.1223685637873, 99999.9999999999, -0.00180766045560232, 33.0301377586347, 2 ),
      ( 400, 306.222587009054, 999999.997144562, -0.0181025559043321, 33.3158207802059, 2 ),
      ( 400, 3667.61899383507, 9999999.99697566, -0.180178813510594, 36.4327585955344, 2 ),
      ( 400, 21193.9690098224, 100000000.004097, 0.418701590827185, 40.5778255374237, 1 ),
      ( 500, 0.240545112475849, 999.99999999998, -7.3669411548617E-06, 36.2939023536332, 2 ),
      ( 500, 2.40561060356979, 9999.99999978552, -7.36609773518791E-05, 36.2950881874595, 2 ),
      ( 500, 24.0720454387542, 99999.9978865937, -0.000735766264960639, 36.3069502336577, 2 ),
      ( 500, 242.305673292306, 999999.999998863, -0.00727318043082183, 36.425900743633, 2 ),
      ( 500, 2568.61578014833, 9999999.99989242, -0.0635293053427694, 37.6063044172615, 2 ),
      ( 500, 17526.1896353336, 100000000.000001, 0.372479388840235, 41.2448389652219, 1 ),
      ( 600, 0.20045332936923, 999.999999999999, -2.66899036723317E-06, 38.9893612219825, 2 ),
      ( 600, 2.00458132443655, 9999.99999999271, -2.66827880214975E-05, 38.9899762895311, 2 ),
      ( 600, 20.0506141739288, 99999.9999300757, -0.000266116922840186, 38.996126460406, 2 ),
      ( 600, 200.973408159436, 999999.999999999, -0.00259051434630715, 39.0575649596411, 2 ),
      ( 600, 2043.44874134297, 9999999.99983965, -0.0190466753274234, 39.6541030976377, 2 ),
      ( 600, 14764.1605485795, 100000000.001357, 0.357698482101522, 42.4250173203276, 1 ),
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
