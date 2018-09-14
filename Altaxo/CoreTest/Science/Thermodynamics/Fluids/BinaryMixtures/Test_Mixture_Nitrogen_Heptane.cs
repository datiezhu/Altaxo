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
  /// Tests and test data for <see cref="Mixture_Nitrogen_Heptane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Nitrogen_Heptane : MixtureTestBase
  {

    public Test_Mixture_Nitrogen_Heptane()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("7727-37-9", 0.5), ("142-82-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 7605.1911664386, 1000000.00000858, -0.92092791168809, 151.672100220594, 1 ),
      ( 200, 7650.91761792366, 9999999.99999988, -0.214004936097785, 152.27110460776, 1 ),
      ( 200, 8008.28087285195, 100000000, 6.50920650492649, 157.58958221721, 1 ),
      ( 250, 7191.4330166229, 1000000.00002574, -0.933102807657445, 162.703485756932, 1 ),
      ( 250, 7252.87932466267, 9999999.99999997, -0.336695597684051, 163.253237516529, 1 ),
      ( 250, 7698.33559670445, 100000000.000001, 5.24922975292884, 168.056953295746, 1 ),
      ( 300, 0.401357366452331, 999.999940744146, -0.00112568242630274, 157.590929464466, 2 ),
      ( 300, 6777.76241011816, 1000000.00719407, -0.940849864093238, 178.406925085486, 1 ),
      ( 300, 6861.7902331247, 10000000.0000059, -0.415742026713751, 178.916409469657, 1 ),
      ( 300, 7413.86246203644, 99999999.999999, 4.40751285236539, 183.379422701207, 1 ),
      ( 350, 0.343846771965071, 999.999992662762, -0.00062071286158961, 179.807700388899, 2 ),
      ( 350, 3.45791720618123, 9999.99999999778, -0.00624184624025492, 180.081126614453, 2 ),
      ( 350, 6345.00948785304, 1000000.00000039, -0.945841949880808, 197.158721554612, 1 ),
      ( 350, 6463.9781485655, 10000000.0001624, -0.468387216125427, 197.608699414951, 1 ),
      ( 350, 7147.55152300288, 99999999.9999995, 3.8077070972318, 201.795825667566, 1 ),
      ( 400, 0.300792525729961, 999.999998726901, -0.000376843178001026, 202.1786288147, 2 ),
      ( 400, 3.01820317979783, 9999.98626219599, -0.00378086635176786, 202.325551396621, 2 ),
      ( 400, 31.2930052013762, 99999.9999999998, -0.0391489338158032, 203.856394764789, 2 ),
      ( 400, 5867.6446598571, 1000000.00398836, -0.948756410324229, 217.024724709705, 1 ),
      ( 400, 6046.73164921081, 10000000.0026388, -0.502740998550736, 217.336934656451, 1 ),
      ( 400, 6895.54342853284, 99999999.9999992, 3.36048554018711, 221.231963866275, 1 ),
      ( 500, 0.240583637118223, 999.999999925794, -0.000167500266864043, 243.568548887498, 2 ),
      ( 500, 2.40947450283452, 9999.99923402265, -0.00167717465758691, 243.62093603132, 2 ),
      ( 500, 24.4702018874074, 99999.9999905109, -0.0169948723676633, 244.154004830844, 2 ),
      ( 500, 301.244978529029, 999999.999935928, -0.201502576159694, 250.826036280675, 2 ),
      ( 500, 5088.35493619467, 10000000.0000057, -0.527266980564033, 255.524260831737, 1 ),
      ( 500, 6427.34102017603, 100000000.000001, 2.74250158097634, 258.414125097088, 1 ),
      ( 600, 0.200470101857175, 999.999999992603, -8.63924911903326E-05, 278.851253514031, 2 ),
      ( 600, 2.00626188551426, 9999.99992488548, -0.000864322876701809, 278.874047040397, 2 ),
      ( 600, 20.2208648936261, 99999.9999999682, -0.00868343749374035, 279.104080470804, 2 ),
      ( 600, 220.589807084412, 1000000, -0.0912871932067098, 281.628174135278, 2 ),
      ( 600, 3831.48182665325, 9999999.99999996, -0.476827003716319, 290.122285623426, 1 ),
      ( 600, 6002.7719230682, 99999999.9999998, 2.33933698155914, 290.858756456001, 1 ),
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
      ( 300, 0.400993883367354, 999.999999999638, -0.000220247439388197, 89.2449232731726, 2 ),
      ( 300, 4.01792107292707, 10000.0000245871, -0.0022064689340528, 89.299739221536, 2 ),
      ( 350, 0.343678820184305, 999.999996933175, -0.000132328010285287, 100.397335380196, 2 ),
      ( 350, 3.44089097849801, 9999.99999905508, -0.00132452903254383, 100.424951891864, 2 ),
      ( 350, 34.8290954140149, 99999.9999812544, -0.0133727622310635, 100.705575682327, 2 ),
      ( 400, 0.300704658590542, 999.99999937365, -8.47491759740468E-05, 111.64119472367, 2 ),
      ( 400, 3.0093433281043, 9999.99351979147, -0.000847887723800404, 111.656629503127, 2 ),
      ( 400, 30.3262657252888, 99999.9964637042, -0.00851896183963542, 111.812501670433, 2 ),
      ( 500, 0.240552617977383, 999.999999997783, -3.85723616812548E-05, 132.526190342266, 2 ),
      ( 500, 2.40636155077061, 9999.99968294657, -0.000385710033002229, 132.532248267587, 2 ),
      ( 500, 24.1474399623602, 99999.9999997136, -0.00385573099057043, 132.593076862142, 2 ),
      ( 500, 250.149555021594, 999999.999446979, -0.0384018901040222, 133.224812887313, 2 ),
      ( 500, 8442.02299740675, 100000000.000089, 1.84935659819305, 140.354975539857, 1 ),
      ( 600, 0.20045645732082, 999.999999997025, -1.83310390837794E-05, 150.450643265778, 2 ),
      ( 600, 2.00489521596926, 9999.99997048939, -0.00018324574201507, 150.453611374561, 2 ),
      ( 600, 20.0819477915139, 99999.9999999994, -0.00182599403724229, 150.483346667711, 2 ),
      ( 600, 204.046876509232, 999999.999958397, -0.0176140592049694, 150.785344078653, 2 ),
      ( 600, 2235.95490936798, 10000000.0000263, -0.10350257106786, 153.420650969846, 1 ),
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
      ( 250, 0.48109057366774, 999.999999994691, -8.0963509881392E-06, 20.9136967352517, 2 ),
      ( 250, 4.81125619075703, 9999.99994201347, -8.09362136099869E-05, 20.9147224767595, 2 ),
      ( 250, 48.1475048907756, 99999.9999999997, -0.000806626052564766, 20.9249704409019, 2 ),
      ( 300, 0.400906354697527, 999.99999999991, -1.92996495911874E-06, 20.9485663171124, 2 ),
      ( 300, 4.00913294825635, 9999.99999925979, -1.92793165050521E-05, 20.9492022111849, 2 ),
      ( 300, 40.0982056363545, 99999.9999999311, -0.000190758840697354, 20.9555542895017, 2 ),
      ( 300, 401.589480425552, 999999.999971778, -0.00170302004256951, 21.0183773480768, 2 ),
      ( 300, 3991.18105034724, 10000000.000225, 0.00447852511404473, 21.570242279734, 1 ),
      ( 350, 0.343632950249978, 1000.00000000002, 1.11723624042465E-06, 21.0110804599308, 1 ),
      ( 350, 3.43629497825739, 10000.0000001962, 1.11865366564427E-05, 21.0115219263877, 1 ),
      ( 350, 34.3594418676462, 100000.007107783, 0.000113282329222957, 21.0159324412807, 1 ),
      ( 350, 343.196074956255, 1000000.00012223, 0.00127410225568409, 21.0596192446574, 1 ),
      ( 350, 3347.5420545772, 10000000.0000097, 0.0265243460641982, 21.4537724840213, 1 ),
      ( 400, 0.300678345200613, 1000.00000000018, 2.70276610139709E-06, 21.1166331763517, 1 ),
      ( 400, 3.00671044709165, 10000.0000018506, 2.70375530955262E-05, 21.1169642831488, 1 ),
      ( 400, 30.0597602802563, 100000, 0.000271363827170243, 21.1202728520476, 1 ),
      ( 400, 299.8361167684, 1000000.00086498, 0.0028117271470662, 21.1531073325364, 1 ),
      ( 400, 2898.63107428183, 10000000.0000015, 0.0373143957031446, 21.4562726375311, 1 ),
      ( 500, 0.240542365311647, 1000.00000000001, 3.96971560570903E-06, 21.4883209368024, 1 ),
      ( 500, 2.40533789568949, 10000.0000001553, 3.97022217448851E-05, 21.4885370882057, 1 ),
      ( 500, 24.0447754614258, 100000.001606944, 0.000397527863086594, 21.490697608201, 1 ),
      ( 500, 239.578975533924, 1000000.00000093, 0.00402524368143199, 21.5122028576416, 1 ),
      ( 500, 2302.38024036219, 10000000.0100537, 0.0447593976413823, 21.7171294672258, 1 ),
      ( 500, 14669.9342359156, 100000000.000067, 0.639702914999842, 23.2035290811379, 1 ),
      ( 600, 0.200451917807098, 1000.00000000001, 4.23033424001219E-06, 22.0519215343812, 1 ),
      ( 600, 2.00444302724455, 10000.000000133, 4.23061224278412E-05, 22.0520808228215, 1 ),
      ( 600, 20.0367959278485, 100000.001348486, 0.000423338484995617, 22.0536732508803, 1 ),
      ( 600, 199.602331202527, 1000000.00000049, 0.00426072949536553, 22.0695515197034, 1 ),
      ( 600, 1918.15409486033, 10000000.0092601, 0.045029610974456, 22.2235372022543, 1 ),
      ( 600, 12912.5264725329, 100000000.000001, 0.552390100977249, 23.4352299578161, 1 ),
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
