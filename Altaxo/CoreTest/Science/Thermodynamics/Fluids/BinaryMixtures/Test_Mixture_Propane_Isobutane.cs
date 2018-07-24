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
	/// Tests and test data for <see cref="Mixture_Propane_Isobutane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Propane_Isobutane : MixtureTestBase
    {

    public Test_Mixture_Propane_Isobutane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-98-6", 0.5), ("75-28-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 150, 12150.7668359, 99999.9999983731, -0.993401117755784, 72.7278861555603, 1 ),
      ( 150, 12158.3439214366, 1000000.00005453, -0.934052301798873, 72.7985373635398, 1 ),
      ( 150, 12232.0082131966, 9999999.99999327, -0.344494557591079, 73.4742170052155, 1 ),
      ( 200, 0.601935397072923, 999.999999992713, -0.000954089797490375, 63.5396415924458, 2 ),
      ( 200, 11319.3242600466, 99999.9999987545, -0.994687305693533, 79.1890618417011, 1 ),
      ( 200, 11330.8514651735, 1000000.00180911, -0.946927104332243, 79.251358344795, 1 ),
      ( 200, 11440.6934441315, 9999999.99999925, -0.474366567389426, 79.8432455715307, 1 ),
      ( 250, 0.481305438157246, 999.999999997267, -0.00044994471014485, 75.6222756339824, 2 ),
      ( 250, 4.83274150199425, 9999.99997104067, -0.00452180788409309, 75.7432813014423, 2 ),
      ( 250, 10433.8194478265, 100000.000001596, -0.995389139355883, 87.5617158345775, 1 ),
      ( 250, 10452.2913920613, 1000000.00000219, -0.953972879337303, 87.6193857268581, 1 ),
      ( 250, 10622.0449538505, 10000000.0000471, -0.547084502644741, 88.1713280106915, 1 ),
      ( 300, 0.401007304598909, 999.999999999556, -0.000249139678879098, 88.8096374408514, 2 ),
      ( 300, 4.01910998026631, 9999.9999953883, -0.00249707089882745, 88.8645911239423, 2 ),
      ( 300, 41.1426542910924, 99999.9999999994, -0.0255674927258882, 89.4502200391357, 2 ),
      ( 300, 9458.97477569, 1000000.00000006, -0.957616189145762, 98.2450808626602, 1 ),
      ( 300, 9745.95206809592, 10000000.0022971, -0.588642140804564, 98.6844951614155, 1 ),
      ( 350, 0.343687248012836, 999.999999999955, -0.000152276934124879, 102.54608946618, 2 ),
      ( 350, 3.44159606761114, 9999.99999949978, -0.0015245670268232, 102.574673252317, 2 ),
      ( 350, 34.9020453811058, 99999.9922015457, -0.0154304452765514, 102.869316529754, 2 ),
      ( 350, 419.36616040984, 999999.999983151, -0.180585023744862, 107.044683239953, 2 ),
      ( 350, 8758.98705342659, 9999999.99999989, -0.607677337258084, 110.664335030844, 1 ),
      ( 400, 0.300710391485512, 999.997812134747, -9.9241978995314E-05, 116.093989312159, 2 ),
      ( 400, 3.009794389567, 9999.99999996496, -0.000993059963885517, 116.110598356185, 2 ),
      ( 400, 30.371631389202, 99999.9994179155, -0.00999539816361071, 116.279365750721, 2 ),
      ( 400, 336.959678415018, 999999.999437617, -0.107666087044852, 118.29327212205, 2 ),
      ( 400, 7555.5486099457, 10000000.0000022, -0.602040084911746, 123.454485587849, 1 ),
      ( 500, 0.240555814748382, 999.997830929393, -4.72907421959216E-05, 140.89875441037, 2 ),
      ( 500, 2.40658264602217, 9999.99999999885, -0.000472977488279262, 140.905664075192, 2 ),
      ( 500, 24.16892472819, 99999.9999865395, -0.00473669651749881, 140.975048714833, 2 ),
      ( 500, 252.692302781549, 999999.999999989, -0.0480737402253347, 141.698630529959, 2 ),
      ( 500, 4099.0047226534, 9999999.99999999, -0.413163792343776, 147.820040412509, 1 ),
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
      ( 150, 13498.9915196973, 1000000.00083158, -0.940601874222646, 68.410806856838, 1 ),
      ( 150, 13585.1865086002, 9999999.99999559, -0.409787421743917, 68.9873858469979, 1 ),
      ( 200, 0.60180450571339, 999.999999196369, -0.000736799171587599, 55.6328491811807, 2 ),
      ( 200, 12521.36324336, 999999.999999872, -0.9519731929478, 70.4177285352602, 1 ),
      ( 200, 12651.7235927152, 10000000.0000022, -0.524680497292625, 70.9252957958941, 1 ),
      ( 250, 0.481258913625614, 999.999978702661, -0.000353315639042363, 65.813201857098, 2 ),
      ( 250, 4.82800709301803, 9999.99908386837, -0.00354562821809053, 65.8999118212924, 2 ),
      ( 250, 11472.7144129136, 1000000.00000033, -0.958066691128611, 76.7721386638812, 1 ),
      ( 250, 11680.6885078444, 10000000.0000927, -0.588133116468569, 77.2141871161907, 1 ),
      ( 300, 0.400986421027873, 999.999997387186, -0.000197072159329719, 77.1098003958168, 2 ),
      ( 300, 4.01700367089258, 9999.99999999991, -0.00197403185506176, 77.1504117669882, 2 ),
      ( 300, 40.9124361079363, 100000.000728499, -0.0200842681450546, 77.5731626738472, 2 ),
      ( 300, 10247.5230186028, 1000000.00000069, -0.960877628960647, 85.8579042689517, 1 ),
      ( 300, 10621.9132474474, 10000000.0000301, -0.622565738932193, 86.0849667257732, 1 ),
      ( 350, 0.343676476313088, 999.99999954675, -0.000120939149238873, 88.9607560232738, 2 ),
      ( 350, 3.44051369009966, 9999.99529267119, -0.00121044843507412, 88.9823384953984, 2 ),
      ( 350, 34.7883440335936, 99999.9996773012, -0.0122125037455929, 89.2026999547612, 2 ),
      ( 350, 397.735216599521, 999999.99998605, -0.136020905286568, 92.0050861020267, 2 ),
      ( 350, 9387.40237070628, 10000000.0000306, -0.633940360918481, 96.5549891314931, 1 ),
      ( 400, 0.300704302650947, 999.999999901184, -7.89956275210489E-05, 100.711951707857, 2 ),
      ( 400, 3.00918370850393, 9999.99899050226, -0.000790322305054924, 100.724237411795, 2 ),
      ( 400, 30.3087145094118, 99999.9999939272, -0.00794027980497146, 100.848542277113, 2 ),
      ( 400, 328.118954209493, 999999.999999966, -0.0836233491897415, 102.256494063851, 2 ),
      ( 400, 7781.63306849787, 10000000.0089825, -0.613602253034587, 108.057721077326, 1 ),
      ( 500, 0.240553469167858, 999.999999992933, -3.75405401638641E-05, 122.396398903078, 2 ),
      ( 500, 2.40634780970469, 9999.9999287708, -0.000375433296763832, 122.401129197578, 2 ),
      ( 500, 24.1451601743458, 99999.9999999897, -0.00375712181515822, 122.448586244015, 2 ),
      ( 500, 250.006402822814, 1000000.00021292, -0.0378468873489734, 122.938874637601, 2 ),
      ( 500, 3638.68988893036, 10000000.0000425, -0.338925695777896, 127.782487202136, 1 ),
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
      ( 150, 15144.6349381107, 1000000.00084681, -0.947056181980495, 59.0442603597596, 1 ),
      ( 150, 15246.7361241601, 9999999.99999821, -0.474107252199863, 59.4334120663175, 1 ),
      ( 200, 0.601701726671267, 999.999999637367, -0.000566111088443789, 47.7327322584323, 2 ),
      ( 200, 6.04804545725073, 9999.99618263157, -0.00569348850297229, 47.8528943757258, 2 ),
      ( 200, 13971.494281903, 1000000.00126888, -0.956957997110161, 61.0319877111317, 1 ),
      ( 200, 14129.293721342, 10000000.0000048, -0.574387008644436, 61.4486056155426, 1 ),
      ( 250, 0.481221334089462, 999.999973799195, -0.000275251231048276, 56.0050886799882, 2 ),
      ( 250, 4.82420046029922, 9999.99957926013, -0.00275935600373777, 56.066686239151, 2 ),
      ( 250, 49.5109216050971, 100000.003703798, -0.0283176697381224, 56.7119635307471, 2 ),
      ( 250, 12684.8534555679, 1000000.00000122, -0.962073753630094, 66.0781996700035, 1 ),
      ( 250, 12948.3959258455, 10000000.0000208, -0.628456775592243, 66.4104626601998, 1 ),
      ( 300, 0.40096939889424, 999.999996738661, -0.00015462807485103, 65.4103269110815, 2 ),
      ( 300, 4.01529026860241, 9999.9999999999, -0.00154815480462435, 65.4399633385949, 2 ),
      ( 300, 40.7291459716671, 100000.000160616, -0.0156744311891483, 65.7436624038706, 2 ),
      ( 300, 11098.1642561339, 1000000.00000058, -0.963876242186045, 73.8731717998583, 1 ),
      ( 300, 11624.0741810167, 10000000.0103506, -0.655105953453281, 73.7844751258352, 1 ),
      ( 350, 0.343667639638145, 999.999999967692, -9.52294075983369E-05, 75.3755563845096, 2 ),
      ( 350, 3.43962669014164, 9999.99522575644, -0.000952883887216698, 75.3913615981915, 2 ),
      ( 350, 34.6961871719399, 99999.9996081417, -0.00958883249813969, 75.5517233901143, 2 ),
      ( 350, 383.12765977205, 999999.999999999, -0.1030798648762, 77.4334071479737, 2 ),
      ( 350, 9990.58790425517, 10000000.0013782, -0.656041350410983, 82.7778387913471, 1 ),
      ( 400, 0.300699266985588, 999.999999876605, -6.2250433513021E-05, 85.3299868199333, 2 ),
      ( 400, 3.00867897883214, 9999.99874133641, -0.000622696987846449, 85.3386091754903, 2 ),
      ( 400, 30.2570531368638, 99999.9999928036, -0.00624642135241353, 85.4256144557834, 2 ),
      ( 400, 321.447382048998, 1000000.01599943, -0.0646041474544334, 86.3807231621765, 2 ),
      ( 400, 7586.83128650927, 10000000.000662, -0.603680987514314, 93.1131370018973, 1 ),
      ( 500, 0.240551475709378, 999.999999991556, -2.92538163399505E-05, 103.894098325353, 2 ),
      ( 500, 2.40614828682441, 9999.99991504685, -0.000292542323929371, 103.897144122156, 2 ),
      ( 500, 24.1250296336123, 99999.9999999889, -0.00292583132519795, 103.92767122887, 2 ),
      ( 500, 247.802431777795, 1000000.00002275, -0.0292894345919149, 104.240007069419, 2 ),
      ( 500, 3250.51603030364, 9999999.99999999, -0.259980764844231, 107.54852061383, 1 ),
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
