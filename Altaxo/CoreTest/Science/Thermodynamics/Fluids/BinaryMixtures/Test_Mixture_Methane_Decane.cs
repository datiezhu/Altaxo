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
  /// Tests and test data for <see cref="Mixture_Methane_Decane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Methane_Decane : MixtureTestBase
  {

    public Test_Mixture_Methane_Decane()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("74-82-8", 0.5), ("124-18-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 5378.46438212573, 1000000.00185608, -0.910552744829242, 226.924550020766, 1 ),
      ( 250, 5415.80767657463, 10000000.0000013, -0.11169505297683, 227.629422481167, 1 ),
      ( 250, 5696.82537325234, 100000000.00553, 7.44485908601862, 233.989408445828, 1 ),
      ( 300, 5104.95978504394, 1000000.00348943, -0.921467080209434, 253.165065829177, 1 ),
      ( 300, 5154.09444026913, 10000000.0000005, -0.222157450760377, 253.803324364498, 1 ),
      ( 300, 5496.77649457419, 100000000.009806, 6.29350004258549, 259.558293227339, 1 ),
      ( 350, 0.344176504232508, 999.999992149843, -0.00157359209428844, 257.760074041446, 2 ),
      ( 350, 4831.69870147078, 1000000.0000044, -0.928879068825448, 281.724906937181, 1 ),
      ( 350, 4897.28007736232, 10000000.0000007, -0.298314767893002, 282.295105274425, 1 ),
      ( 350, 5312.6212403942, 100000000.000001, 5.46827423323682, 287.595920107101, 1 ),
      ( 400, 0.300956292675852, 999.999998472222, -0.000916231793283896, 289.462335198733, 2 ),
      ( 400, 3.03484296230317, 9999.98367485652, -0.00923851627382736, 289.925853624276, 2 ),
      ( 400, 4538.84564605141, 100000.000001156, -0.993375396071992, 310.597480396054, 1 ),
      ( 400, 4548.88178695119, 999999.999999505, -0.933900118527146, 310.640390991186, 1 ),
      ( 400, 4638.73656923111, 10000000.0000281, -0.351805082143911, 311.10849453406, 1 ),
      ( 400, 5140.55801106863, 100000000.000001, 4.84918108703812, 316.007424790093, 1 ),
      ( 500, 0.240636216043536, 999.999894643043, -0.000381399250926008, 347.826788101099, 2 ),
      ( 500, 2.41468337706066, 9999.99999961151, -0.00382617510333538, 347.991013269891, 2 ),
      ( 500, 25.0455960372041, 100000.011967546, -0.0395739196184221, 349.70364384902, 2 ),
      ( 500, 3895.83214853017, 999999.999999818, -0.938255954468668, 365.473755063797, 1 ),
      ( 500, 4093.5693710566, 10000000.0012442, -0.412384606722326, 365.277233820645, 1 ),
      ( 500, 4824.87659028298, 100000000.000997, 3.98550445928345, 369.237162220497, 1 ),
      ( 600, 0.20049160219167, 999.999999484487, -0.000189056416689225, 397.073086841086, 2 ),
      ( 600, 2.00833867611929, 9999.99999999939, -0.00189295584660138, 397.143932890567, 2 ),
      ( 600, 20.4372743861902, 100000.000213361, -0.0191759714557188, 397.866657534357, 2 ),
      ( 600, 259.651996879236, 999999.998767823, -0.227990924506112, 407.282896551776, 2 ),
      ( 600, 3472.15434207978, 10000000.0043, -0.422682063366204, 412.375940353535, 1 ),
      ( 600, 4540.94370972673, 100000000.000001, 3.41436209698848, 414.769218344637, 1 ),
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
      ( 250, 9362.34885678795, 100000000, 4.13853719095473, 131.813774771817, 1 ),
      ( 300, 8980.82018693494, 100000000.007047, 3.46402971341896, 145.016409878916, 1 ),
      ( 350, 0.343741115765623, 999.999999997998, -0.00031124777178753, 143.782668083321, 2 ),
      ( 350, 8625.21349260376, 100000000.000062, 2.9840651759418, 159.886095885461, 1 ),
      ( 400, 0.300738715354649, 999.999991566728, -0.000195698560039028, 160.98750278147, 2 ),
      ( 400, 3.01270363356764, 9999.99999427097, -0.00196004057213256, 161.04368552196, 2 ),
      ( 400, 8290.02317165124, 100000000.001599, 2.62700869465715, 175.21169391717, 1 ),
      ( 500, 0.240565722785402, 999.999999445424, -9.07602656374811E-05, 193.15808639748, 2 ),
      ( 500, 2.407625088413, 9999.99427749929, -0.000908030659165913, 193.178799187887, 2 ),
      ( 500, 24.2758752974243, 99999.9962630442, -0.00912372406777561, 193.388233713859, 2 ),
      ( 500, 7672.70847558401, 100000000, 2.13505836618184, 204.479597638676, 1 ),
      ( 600, 0.200462750118659, 999.999999996774, -4.74367677498928E-05, 220.799392319103, 2 ),
      ( 600, 2.00548372553793, 9999.99953818349, -0.000474358053596767, 220.808820375933, 2 ),
      ( 600, 20.1408448008975, 99999.9999992213, -0.00474263716550938, 220.903628700009, 2 ),
      ( 600, 210.415180323004, 1000000.00002043, -0.047344205366626, 221.902892545994, 2 ),
      ( 600, 7121.3841126449, 100000000.000509, 1.81480731335132, 230.051315637049, 1 ),
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
      ( 250, 0.481102084594327, 999.999999912293, -3.20177494515954E-05, 26.1230357397063, 2 ),
      ( 250, 23224.078316686, 99999999.9999996, 1.07149956277347, 31.5552286796738, 1 ),
      ( 300, 0.400912452360299, 999.99999999892, -1.71734169346497E-05, 27.6615795226716, 2 ),
      ( 300, 4.0097442428835, 9999.99998916362, -0.000171724081397279, 27.6630167863359, 2 ),
      ( 300, 40.1594794408596, 99999.9999999998, -0.00171622514581734, 27.6773989561281, 2 ),
      ( 300, 21219.4237443689, 100000000, 0.889332962823703, 32.047568254792, 1 ),
      ( 350, 0.343636540405759, 999.999999999872, -9.30339226094456E-06, 29.8766661829925, 2 ),
      ( 350, 3.43665310875395, 9999.99999871814, -9.30191501745881E-05, 29.8775715866492, 2 ),
      ( 350, 34.3952775875792, 99999.9876180181, -0.000928710097525294, 29.8866217714045, 2 ),
      ( 350, 346.801312092735, 999999.999977608, -0.00913482320271446, 29.9767113038744, 2 ),
      ( 350, 3699.58258561195, 10000000.0018083, -0.0711564468009047, 30.8001672339878, 2 ),
      ( 350, 19427.365204826, 99999999.9999996, 0.768810849009262, 33.4812126745769, 1 ),
      ( 400, 0.300680612409276, 999.982027859932, -4.77879415386633E-06, 32.5507112341003, 2 ),
      ( 400, 3.00693541331781, 9999.99999989091, -4.77756822361678E-05, 32.5513239202284, 2 ),
      ( 400, 30.0822500285563, 99999.998759548, -0.000476443055119012, 32.5574458836561, 2 ),
      ( 400, 302.078128414563, 999999.999999935, -0.00463109634702865, 32.6181693466412, 2 ),
      ( 400, 3103.04329060338, 9999999.99999856, -0.0310184314759139, 33.1672263927584, 1 ),
      ( 400, 17850.5734253136, 100000000.000132, 0.684423062099002, 35.5509924619189, 1 ),
      ( 500, 0.240543409901509, 999.996412720732, -2.83296398697309E-07, 38.5022933048176, 2 ),
      ( 500, 2.40544019869661, 9999.99999999997, -2.82474493305671E-06, 38.5026137893887, 2 ),
      ( 500, 24.0549937274663, 99999.9999999114, -2.74241622227724E-05, 38.5058159807236, 2 ),
      ( 500, 240.589426686558, 999999.999860847, -0.000191555767973162, 38.5375738132106, 2 ),
      ( 500, 2389.65187698316, 10000000.00035, 0.00660411129830531, 38.8298955262674, 1 ),
      ( 500, 15283.1651983489, 100000000.001707, 0.573910490885767, 40.6378849997462, 1 ),
      ( 600, 0.200452455110555, 1000.0171645095, 1.60693539599325E-06, 44.5306768425144, 1 ),
      ( 600, 2.00449561626345, 10000.0000000024, 1.60740459045377E-05, 44.5308630654268, 1 ),
      ( 600, 20.0420468425723, 100000.000025387, 0.000161237204658424, 44.532724194106, 1 ),
      ( 600, 200.120171861991, 1000000, 0.00166206033350651, 44.5512264138582, 1 ),
      ( 600, 1962.24524465335, 10000000.0001015, 0.0215480669811277, 44.7265333802686, 1 ),
      ( 600, 13340.1237130623, 100000000.000135, 0.502630620026687, 46.0906835474426, 1 ),
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
