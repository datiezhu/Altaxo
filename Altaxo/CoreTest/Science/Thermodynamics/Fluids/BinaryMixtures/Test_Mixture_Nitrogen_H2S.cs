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
  /// Tests and test data for <see cref="Mixture_Nitrogen_H2S"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Nitrogen_H2S : MixtureTestBase
  {

    public Test_Mixture_Nitrogen_H2S()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("7727-37-9", 0.5), ("7783-06-4", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601526316010158, 999.999999779308, -0.000274671452322993, 25.0693195037025, 2 ),
      ( 200, 6.03021516514792, 9999.99767344034, -0.00275350410575244, 25.1604401316022, 2 ),
      ( 200, 28513.0129352611, 1000000.00170316, -0.978909240625866, 43.0287166414381, 1 ),
      ( 200, 28726.7584923618, 10000000.0000011, -0.790661694719559, 43.1828863706901, 1 ),
      ( 200, 30350.67036014, 100000000.000004, 0.981376644296726, 44.5984104816827, 1 ),
      ( 250, 0.481151948197467, 999.999999985644, -0.000131087643039897, 25.332755491733, 2 ),
      ( 250, 4.81721066045786, 9999.99985277371, -0.00131235886942802, 25.3657796057617, 2 ),
      ( 250, 48.7562211035435, 99999.9999997635, -0.0132769434674601, 25.7012995572335, 2 ),
      ( 250, 25841.5474647872, 1000000.00003185, -0.981383124374905, 39.0134429864697, 1 ),
      ( 250, 26197.7135883941, 10000000.0000005, -0.816362266310569, 39.159078173741, 1 ),
      ( 250, 28520.0527495607, 100000000.000049, 0.686844268302634, 40.5580705551853, 1 ),
      ( 300, 0.400936803231848, 999.999999998351, -7.33464627946065E-05, 25.8118390010477, 2 ),
      ( 300, 4.01201839450573, 9999.99998326314, -0.000733903700254217, 25.8264575331122, 2 ),
      ( 300, 40.3889539753247, 99999.9999999987, -0.00738356280819519, 25.9737636670864, 2 ),
      ( 300, 435.314698850539, 999999.999999982, -0.0790400668892279, 27.6145411362937, 2 ),
      ( 300, 23216.0233738001, 10000000.0016935, -0.827314355457736, 36.7579476181036, 1 ),
      ( 300, 26708.4283777605, 100000000.000799, 0.50105199103464, 38.0967558569165, 1 ),
      ( 350, 0.343650415227777, 999.999999999984, -4.51168548329828E-05, 26.4594029222459, 2 ),
      ( 350, 3.43790070126945, 9999.99999779418, -0.000451319972584615, 26.4668603562729, 2 ),
      ( 350, 34.5198124624087, 99999.9999999999, -0.00452845396369198, 26.5417789212684, 2 ),
      ( 350, 360.556976185484, 999999.989842123, -0.0469331238117421, 27.3300328652605, 2 ),
      ( 350, 18936.6751418215, 9999999.99999979, -0.818534717298244, 35.8423772065174, 1 ),
      ( 350, 24900.8796227071, 100000000.009865, 0.380011132289916, 36.600543444973, 1 ),
      ( 400, 0.30068940223893, 999.999999999945, -2.94499480818829E-05, 27.2149697616568, 2 ),
      ( 400, 3.00769139810635, 9999.99999944061, -0.000294554351682053, 27.2192062009868, 2 ),
      ( 400, 30.1570497548117, 99999.9941710913, -0.00295105307105568, 27.2616991341337, 2 ),
      ( 400, 310.007162952262, 999999.999870003, -0.0300851629057799, 27.7000405651761, 2 ),
      ( 400, 5034.04504776772, 9999999.98808065, -0.402705886701082, 34.5997852381142, 2 ),
      ( 400, 23105.4286766771, 100000000.000001, 0.301341564180363, 35.7262123161279, 1 ),
      ( 500, 0.240547770157084, 999.99801282958, -1.3854167689813E-05, 28.8960449746135, 2 ),
      ( 500, 2.4057776857588, 9999.99999999974, -0.000138545696723574, 28.8977827198884, 2 ),
      ( 500, 24.0878254263728, 99999.9999972959, -0.00138583162392562, 28.915186311622, 2 ),
      ( 500, 243.934123796579, 1000000, -0.0138959083809432, 29.0918113368431, 2 ),
      ( 500, 2802.4882601914, 10000000, -0.141675485395639, 31.0658566693982, 2 ),
      ( 500, 19667.9644297861, 100000000.000085, 0.223026604609721, 35.1098093659166, 1 ),
      ( 600, 0.200455070004011, 999.998395132114, -6.84459637449612E-06, 30.6888728642185, 2 ),
      ( 600, 2.00467418286745, 9999.99999999993, -6.8441640536561E-05, 30.6897721043508, 2 ),
      ( 600, 20.0590896715421, 99999.9999992914, -0.000683972951365936, 30.6987719635692, 2 ),
      ( 600, 201.825142820678, 999999.993506467, -0.00679521298954965, 30.7894953192374, 2 ),
      ( 600, 2139.49339660881, 9999999.99999949, -0.063078678880282, 31.7440150991416, 2 ),
      ( 600, 16672.3397123819, 100000000, 0.20231294122918, 35.3758184018527, 1 ),
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
      ( 200, 0.601417973375783, 999.99999947932, -9.68559921956906E-05, 22.9298132976436, 2 ),
      ( 200, 6.01943173050103, 9999.9999998399, -0.00096927855466823, 22.9437354362911, 2 ),
      ( 200, 60.7290437884961, 99999.9999906697, -0.0097658631792301, 23.084910543028, 2 ),
      ( 250, 0.481111216181558, 999.999999956645, -4.87168617046365E-05, 23.0660568334859, 2 ),
      ( 250, 4.81322329239135, 9999.99955923895, -0.000487306034979212, 23.0716896014646, 2 ),
      ( 250, 48.3450349146118, 99999.9999088191, -0.00488689517708787, 23.1283532035667, 2 ),
      ( 250, 506.594668285383, 999999.998499604, -0.0503497014272546, 23.73066341403, 2 ),
      ( 300, 0.400917250110823, 999.999999994851, -2.6859615897343E-05, 23.3137500297187, 2 ),
      ( 300, 4.0101420014399, 9999.99994812397, -0.000268615215900397, 23.3165763742611, 2 ),
      ( 300, 40.1987046579126, 99999.9999999962, -0.00268805912847841, 23.3449168147746, 2 ),
      ( 300, 412.061667137087, 1000000.00003415, -0.0270716409694119, 23.6359934288078, 2 ),
      ( 350, 0.3436394250618, 999.999999999957, -1.5417184836871E-05, 23.6582563154155, 2 ),
      ( 350, 3.43687111103201, 9999.99999407527, -0.000154163459119268, 23.6599041411611, 2 ),
      ( 350, 34.4164412859163, 100000, -0.00154079192918442, 23.6764027529615, 2 ),
      ( 350, 348.980355390229, 999999.997463454, -0.0153195679496449, 23.8433346198583, 2 ),
      ( 350, 3979.13005681362, 9999999.99999987, -0.136408907970099, 25.5566204105131, 2 ),
      ( 400, 0.300682523209506, 999.999999999894, -8.8531547112757E-06, 24.0781124837859, 2 ),
      ( 400, 3.00706479130927, 9999.99999892609, -8.85179141889004E-05, 24.0791837592413, 2 ),
      ( 400, 30.0945841416218, 99999.9896065668, -0.000883814066742302, 24.0899019197811, 2 ),
      ( 400, 303.318685612508, 999999.999981763, -0.00869984118031782, 24.1975723500482, 2 ),
      ( 400, 3234.47904311381, 10000000, -0.0703916852985813, 25.2614993827884, 2 ),
      ( 500, 0.24054444923956, 999.999999999999, -2.2834812498914E-06, 25.0847051073922, 2 ),
      ( 500, 2.40549379210387, 9999.99999998793, -2.28237292423054E-05, 25.0852708602145, 2 ),
      ( 500, 24.0598536015003, 99999.9998913736, -0.000227129555154682, 25.0909280927885, 2 ),
      ( 500, 241.064825283585, 1000000, -0.00216098016849345, 25.1474558771879, 2 ),
      ( 500, 2431.76009117509, 10000000.0000214, -0.0108239301671794, 25.6943762874892, 1 ),
      ( 500, 15308.2984291788, 100000000.000374, 0.571330021359422, 28.2562121884494, 1 ),
      ( 600, 0.200453129455265, 1000.00655259267, 5.44641283627777E-07, 26.2461845239706, 1 ),
      ( 600, 2.00452147611444, 10000.0000000001, 5.45368212794219E-06, 26.2465459076195, 1 ),
      ( 600, 20.0442163066919, 100000.000001541, 5.52665499646342E-05, 26.2501590607112, 1 ),
      ( 600, 200.328057463319, 999999.999999999, 0.000624891749991918, 26.2862168574911, 1 ),
      ( 600, 1979.05459002582, 10000000.0001007, 0.0128737318517489, 26.6350877731107, 1 ),
      ( 600, 13319.4327041571, 100000000.000373, 0.504968306582021, 28.6932903450696, 1 ),
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
      ( 200, 0.601371331503912, 999.999999979267, -2.15848641793937E-05, 20.7975162575629, 2 ),
      ( 200, 6.01488168981442, 9999.9997931042, -0.00021582800801322, 20.7994641865421, 2 ),
      ( 200, 60.2657800045228, 99999.9999999611, -0.00215619719604529, 20.8189520884198, 2 ),
      ( 200, 614.466236789626, 999999.981150794, -0.0213321493933735, 21.0145389855664, 2 ),
      ( 200, 7127.03999920307, 10000000.0000199, -0.15622986393711, 22.7246470362133, 1 ),
      ( 250, 0.481090510330918, 999.999999999322, -7.9601291554583E-06, 20.8018741681312, 2 ),
      ( 250, 4.8112496590426, 9999.99999332512, -7.95741618661041E-05, 20.8028899898089, 2 ),
      ( 250, 48.1468496033612, 99999.9999999999, -0.000793022281605437, 20.8130387601849, 2 ),
      ( 250, 484.796206294392, 999999.999956017, -0.00765172139912227, 20.9135376220553, 2 ),
      ( 250, 5020.98879710055, 10000000, -0.0418487269556725, 21.7784980595471, 1 ),
      ( 250, 22530.3552708148, 100000000.000001, 1.1352822669922, 24.7689719039882, 1 ),
      ( 300, 0.40090632600664, 999.999999999996, -1.85532247247791E-06, 20.8167148922208, 2 ),
      ( 300, 4.0091299747608, 9999.99999996117, -1.85330795634833E-05, 20.817345298596, 2 ),
      ( 300, 40.097907292113, 99999.9997735322, -0.000183315295807029, 20.8236425641715, 2 ),
      ( 300, 401.560313978343, 999999.999999979, -0.00163050638178404, 20.8859242914314, 2 ),
      ( 300, 3989.21500088518, 10000000.0000346, 0.00497357809664664, 21.4333400765181, 1 ),
      ( 300, 20373.0393110602, 100000000.00003, 0.96782405021923, 23.9907064821359, 1 ),
      ( 350, 0.343632937195176, 1000, 1.15896269021351E-06, 20.8576076924174, 1 ),
      ( 350, 3.43629356065083, 10000.0000000079, 1.1603651072008E-05, 20.8580457103228, 1 ),
      ( 350, 34.3592992391213, 100000.000118115, 0.000117438451763772, 20.8624217878424, 1 ),
      ( 350, 343.182347490251, 1000000.00000009, 0.00131415829800852, 20.9057691852641, 1 ),
      ( 350, 3346.73917941833, 10000000.0000017, 0.026770611625478, 21.2970875154633, 1 ),
      ( 350, 18571.2863195618, 100000000.00001, 0.850347560784126, 23.4852808750305, 1 ),
      ( 400, 0.300678339630215, 1000.00000000001, 2.72540976435078E-06, 20.941509950958, 1 ),
      ( 400, 3.00670978036174, 10000.0000000766, 2.72638768791209E-05, 20.9418386842601, 1 ),
      ( 400, 30.0596927428263, 100000.000860487, 0.000273615787924583, 20.9451235498137, 1 ),
      ( 400, 299.829720261998, 1000000.00000062, 0.00283312550957697, 20.977724195553, 1 ),
      ( 400, 2898.33147188135, 10000000.0000004, 0.0374216283050773, 21.2788857535617, 1 ),
      ( 400, 17060.0456970703, 100000000.000001, 0.762475791869287, 23.1745882775948, 1 ),
      ( 500, 0.24054236556105, 1000.00000000002, 3.973179853015E-06, 21.2734390531855, 1 ),
      ( 500, 2.40533782351164, 10000.0000001545, 3.97368008072504E-05, 21.2736538503506, 1 ),
      ( 500, 24.0447674123792, 100000.001598068, 0.000397867320796597, 21.2758008410545, 1 ),
      ( 500, 239.578315712828, 1000000.00000091, 0.00402801344960339, 21.2971721174615, 1 ),
      ( 500, 2302.44201021866, 10000000.0101524, 0.0447313736376402, 21.5008932660469, 1 ),
      ( 500, 14691.0077932344, 100000000.000062, 0.637350846039309, 22.9810560385964, 1 ),
      ( 600, 0.200451919690966, 1000.00000000001, 4.22560111691522E-06, 21.8035131154238, 1 ),
      ( 600, 2.00444313134901, 10000.0000001315, 4.2258753896174E-05, 21.8036714955958, 1 ),
      ( 600, 20.0368055811149, 100000.001333486, 0.000422861076512538, 21.8052548468322, 1 ),
      ( 600, 199.603353799665, 1000000.00000048, 0.00425558911079487, 21.8210429393158, 1 ),
      ( 600, 1918.30689382585, 10000000.0091039, 0.0449463759695242, 21.9741883943924, 1 ),
      ( 600, 12929.4131349327, 100000000.000001, 0.550362584673063, 23.1807391408689, 1 ),
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
