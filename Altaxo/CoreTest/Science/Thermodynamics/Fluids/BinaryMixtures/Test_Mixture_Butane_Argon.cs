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
  /// Tests and test data for <see cref="Mixture_Butane_Argon"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Butane_Argon : MixtureTestBase
  {

    public Test_Mixture_Butane_Argon()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("106-97-8", 0.5), ("7440-37-1", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 150, 0.801867015221941, 999.999997376771, -6.96880823552017E-05, 12.518964048949, 2 ),
      ( 200, 0.6013758277108, 999.999999927231, -2.90612369059612E-05, 12.5276735680656, 2 ),
      ( 200, 6.01533177770864, 9999.99926348188, -0.000290635295015825, 12.5299218593672, 2 ),
      ( 200, 60.3112592170963, 99999.9999990753, -0.00290864625503316, 12.5524531988172, 2 ),
      ( 200, 619.518416771636, 999999.999658004, -0.0293131976198639, 12.7827408021383, 2 ),
      ( 200, 8487.73209711543, 9999999.99999999, -0.291497016983166, 15.326361759107, 2 ),
      ( 250, 0.481093135025393, 999.999999999567, -1.34157746259263E-05, 12.5379483129362, 2 ),
      ( 250, 4.81151226154602, 9999.99999567407, -0.000134147774640911, 12.5390680851472, 2 ),
      ( 250, 48.1732430652402, 100000, -0.00134047413618586, 12.5502655342968, 2 ),
      ( 250, 487.570240835957, 999999.999375922, -0.0132976943640224, 12.6621772775372, 2 ),
      ( 250, 5426.36183504966, 9999999.99999999, -0.11342683106587, 13.7133821818181, 2 ),
      ( 250, 26990.3147949151, 100000000, 0.782441903489602, 16.4229574424382, 1 ),
      ( 300, 0.400908033687227, 999.99999999997, -6.15194363770909E-06, 12.5500015204807, 2 ),
      ( 300, 4.00930227520301, 9999.99999970658, -6.15074519386279E-05, 12.5506657084353, 2 ),
      ( 300, 40.1151823709563, 99999.9971856006, -0.000613873263125951, 12.5573037281775, 2 ),
      ( 300, 403.331929313321, 999999.999999302, -0.00601579443224471, 12.6232842982872, 2 ),
      ( 300, 4198.03314471995, 10000000.0001093, -0.0450157168775096, 13.2277677469461, 2 ),
      ( 300, 24123.2539430492, 100000000.000229, 0.661905016091466, 15.6226647546992, 1 ),
      ( 350, 0.34363417798968, 999.999999999999, -2.38104050961373E-06, 12.5629058655831, 2 ),
      ( 350, 3.43641522338076, 9999.99999998737, -2.3800726384713E-05, 12.5633484162565, 2 ),
      ( 350, 34.3714817056831, 99999.9998840312, -0.000237038476273239, 12.5677710171814, 2 ),
      ( 350, 344.416024074282, 999999.999999999, -0.00227248617588724, 12.6117021221594, 2 ),
      ( 350, 3478.24076446921, 10000000.0000278, -0.0120484270942403, 13.0174316511526, 1 ),
      ( 350, 21707.4660761542, 100000000.000296, 0.58301914288495, 15.0791141818244, 1 ),
      ( 400, 0.300679261238034, 999.996641082003, -2.79582731011518E-07, 12.5757844121317, 2 ),
      ( 400, 3.00680013954646, 9999.99999999999, -2.78855230433728E-06, 12.5761043274317, 2 ),
      ( 400, 30.0687341189074, 99999.9999999141, -2.71567680299008E-05, 12.5793016059178, 2 ),
      ( 400, 300.738837222968, 999999.999614352, -0.00019838385625688, 12.6110857104208, 2 ),
      ( 400, 2990.19151612676, 10000000.0000805, 0.00555156374133699, 12.9089304667223, 1 ),
      ( 400, 19691.6589324589, 100000000.000978, 0.526936742736781, 14.6906698905162, 1 ),
      ( 500, 0.240542928936445, 1000.0163287093, 1.67702315221774E-06, 12.5996538493579, 1 ),
      ( 500, 2.4053930558413, 10000.0000000023, 1.67740154579048E-05, 12.5998503810774, 1 ),
      ( 500, 24.0502900837455, 100000.000023627, 0.000168145816844198, 12.6018149597258, 1 ),
      ( 500, 240.12984580189, 1000000, 0.00172196250992355, 12.6213863772504, 1 ),
      ( 500, 2355.53559866773, 10000000.000017, 0.0211832100105791, 12.8095639927987, 1 ),
      ( 500, 16593.6142284165, 100000000.000002, 0.449613912213481, 14.1814995640378, 1 ),
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
      ( 200, 0.601534674177523, 999.999999998251, -0.000290842313523486, 40.3079865233793, 2 ),
      ( 250, 0.48115776875115, 999.999999016622, -0.000145463302585329, 45.5037964135936, 2 ),
      ( 250, 4.81789420070553, 9999.99999949887, -0.00145632529070885, 45.5278013993123, 2 ),
      ( 300, 0.400939513050821, 999.99999987607, -8.23850537623612E-05, 51.5542976034579, 2 ),
      ( 300, 4.01237225769303, 9999.99872960343, -0.000824310603538551, 51.565677681613, 2 ),
      ( 300, 40.4257655985089, 99999.9981048264, -0.00828969890131322, 51.6808093815647, 2 ),
      ( 350, 0.343651344200371, 999.999999991903, -5.01004619779969E-05, 58.0181965864777, 2 ),
      ( 350, 3.43806422421348, 9999.99991806126, -0.000501140465750537, 58.0244136632516, 2 ),
      ( 350, 34.5369628884963, 99999.9999999798, -0.00502505611874777, 58.086890865632, 2 ),
      ( 350, 362.362296775947, 1000000.00399048, -0.051683549614428, 58.7442657017833, 2 ),
      ( 350, 14190.1987281404, 100000000.000001, 1.4216301243746, 65.1235979786444, 1 ),
      ( 400, 0.300689398527479, 999.999999999824, -3.17181347812972E-05, 64.4639982194842, 2 ),
      ( 400, 3.00775271795635, 9999.99997455017, -0.000317215488534953, 64.4677769506422, 2 ),
      ( 400, 30.1637730411938, 99999.9999999984, -0.00317556158537484, 64.5056323972476, 2 ),
      ( 400, 310.647437389719, 1000000.00001998, -0.0320864586975707, 64.8909406224609, 2 ),
      ( 400, 13377.8953357873, 100000000, 1.2475871852298, 70.5694527094898, 1 ),
      ( 500, 0.240547058138337, 999.999999999775, -1.31748102618749E-05, 76.4053203511755, 2 ),
      ( 500, 2.40575581479077, 9999.99999774082, -0.000131736157046293, 76.4070799631949, 2 ),
      ( 500, 24.0860899717352, 99999.9999999999, -0.00131615692347029, 76.4246680078003, 2 ),
      ( 500, 243.719693610631, 999999.999829396, -0.0130305622272895, 76.5996227375295, 2 ),
      ( 500, 2682.31256873101, 9999999.99995264, -0.103222004099404, 78.1038894686397, 1 ),
      ( 500, 11925.4797613216, 100000000.000005, 1.01705838080126, 81.135981951996, 1 ),
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
      ( 150, 12416.4891153606, 1000000.00038253, -0.935423388639238, 83.8414294189675, 1 ),
      ( 150, 12485.7155147108, 9999999.99999319, -0.357814303136589, 84.3286217498261, 1 ),
      ( 150, 13044.5555386973, 100000000.000192, 5.14673907051803, 87.9334820039955, 1 ),
      ( 200, 0.602049893332491, 999.999999215917, -0.00114409027575028, 68.1244513421313, 2 ),
      ( 200, 11602.0319009076, 99999.9999957473, -0.994816760575625, 85.5608643079742, 1 ),
      ( 200, 11612.4754796977, 1000000.00304533, -0.948214220424222, 85.6056383948118, 1 ),
      ( 200, 11712.4295541687, 10000000.0000005, -0.4865616129308, 86.0399629021258, 1 ),
      ( 200, 12445.9727460371, 100000000.002636, 3.83177254353049, 89.4577208220907, 1 ),
      ( 250, 0.481339412727339, 999.999987169849, -0.000520500908198508, 78.4825160992606, 2 ),
      ( 250, 4.83620149067089, 9999.99692332513, -0.00523401093091779, 78.6485276788773, 2 ),
      ( 250, 10773.2589336544, 1000000.0007576, -0.955344164824721, 91.3969136985802, 1 ),
      ( 250, 10922.7521148837, 10000000.0000019, -0.55955342567726, 91.8362046692267, 1 ),
      ( 300, 0.401020091509872, 999.999998577732, -0.000281022264391442, 90.5640853534934, 2 ),
      ( 300, 4.02040180733407, 9999.98472516993, -0.0028175866865661, 90.6376062045114, 2 ),
      ( 300, 41.286206912543, 100000.006898921, -0.0289556123815684, 91.4173896000787, 2 ),
      ( 300, 9843.51599000939, 999999.999999921, -0.959271931254809, 100.455045371601, 1 ),
      ( 300, 10084.6831034799, 10000000.0009889, -0.602459103714077, 100.868665241006, 1 ),
      ( 300, 11351.2498790807, 100000000.000006, 2.53183482177047, 104.417968966214, 1 ),
      ( 350, 0.343692832718453, 999.999999770497, -0.000168528147940951, 103.476095278439, 2 ),
      ( 350, 3.4421582758997, 9999.99761428339, -0.00168765234596253, 103.513528875448, 2 ),
      ( 350, 34.9620756023111, 99999.9998875393, -0.0171209663936365, 103.899150422366, 2 ),
      ( 350, 8695.17121005546, 1000000.00000035, -0.960479799362133, 111.631035698214, 1 ),
      ( 350, 9149.98080246963, 10000000.0000004, -0.624441932480613, 111.720678818229, 1 ),
      ( 350, 10847.1025324577, 100000000.000035, 2.16798803897113, 115.17393388352, 1 ),
      ( 400, 0.300713058172763, 999.999999997307, -0.000108113765712245, 116.353518229814, 2 ),
      ( 400, 3.01006242326288, 9999.99960750224, -0.00108202195161725, 116.374673997207, 2 ),
      ( 400, 30.3997209575121, 99999.9999980671, -0.0109101742947814, 116.589636804581, 2 ),
      ( 400, 341.702827672299, 999999.998292721, -0.120052505627366, 119.163615133548, 2 ),
      ( 400, 8034.02872278155, 10000000.0000079, -0.625741259675557, 123.465825942106, 1 ),
      ( 400, 10368.1709934972, 100000000.000302, 1.90003460727134, 126.437040140639, 1 ),
      ( 500, 0.240556466316577, 999.999999996889, -5.00038741196876E-05, 140.211260453388, 2 ),
      ( 500, 2.40664815072003, 9999.99996853123, -0.000500187409055443, 140.219668640915, 2 ),
      ( 500, 24.1757284517225, 99999.9999999969, -0.00501679588339875, 140.304116298424, 2 ),
      ( 500, 253.662061178199, 1000000.00036418, -0.051712989955592, 141.186795054326, 2 ),
      ( 500, 4601.79931931621, 9999999.99999999, -0.477281774214657, 147.698516858107, 1 ),
      ( 500, 9481.73608786565, 100000000.00851, 1.5369239910196, 148.070014139745, 1 ),
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
