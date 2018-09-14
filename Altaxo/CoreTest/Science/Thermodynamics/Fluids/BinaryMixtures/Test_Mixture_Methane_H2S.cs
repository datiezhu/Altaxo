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
  /// Tests and test data for <see cref="Mixture_Methane_H2S"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Methane_H2S : MixtureTestBase
  {

    public Test_Mixture_Methane_H2S()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("74-82-8", 0.5), ("7783-06-4", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601526411457407, 999.999999783116, -0.000274830083815709, 25.073735056205, 2 ),
      ( 200, 6.03022482529403, 9999.99771370206, -0.00275510166005661, 25.1649480431677, 2 ),
      ( 200, 28517.4823811816, 1000000.00168848, -0.978912546107585, 43.0359811776019, 1 ),
      ( 200, 28731.0908448666, 10000000.0000006, -0.79069326077934, 43.1900251830461, 1 ),
      ( 200, 30354.2552113352, 100000000.000006, 0.981142642823754, 44.6046247507602, 1 ),
      ( 250, 0.481151981355668, 999.999999985912, -0.000131156548200669, 25.3379145134642, 2 ),
      ( 250, 4.8172139929991, 9999.99985552649, -0.00131304976035417, 25.3709684192568, 2 ),
      ( 250, 48.7565720332135, 99999.9999997721, -0.0132840454925256, 25.7068021784649, 2 ),
      ( 250, 25847.0939349039, 1000000.00002762, -0.981387119328702, 39.0216224252681, 1 ),
      ( 250, 26202.9182359943, 9999999.99999912, -0.816398742006978, 39.1671916389121, 1 ),
      ( 250, 28523.8433818353, 100000000.000055, 0.686620097729359, 40.5654598988051, 1 ),
      ( 300, 0.400936818235617, 999.999999998383, -7.33838818291662E-05, 25.818491690946, 2 ),
      ( 300, 4.01201989893397, 9999.99998359667, -0.000734278405332169, 25.8331224432478, 2 ),
      ( 300, 40.3891085814141, 99999.9999999987, -0.00738736245977928, 25.9805531601329, 2 ),
      ( 300, 435.335860243137, 999999.999999984, -0.0790848341516836, 27.6229509986633, 2 ),
      ( 300, 23222.8008820404, 10000000.0017173, -0.827364753270452, 36.7673309781659, 1 ),
      ( 300, 26712.4509345054, 100000000.000848, 0.500825951619232, 38.1057356192746, 1 ),
      ( 350, 0.34365042326606, 999.999999999983, -4.51402446496978E-05, 26.4681999053128, 2 ),
      ( 350, 3.43790150640888, 9999.99999784038, -0.000451554061913623, 26.4756632015946, 2 ),
      ( 350, 34.519894308098, 100000, -0.00453081419843277, 26.5506409723588, 2 ),
      ( 350, 360.566735379663, 999999.990259097, -0.0469589197908071, 27.3395564906507, 2 ),
      ( 350, 18948.2934924065, 10000000.0000004, -0.818645984694175, 35.8512868944165, 1 ),
      ( 350, 24905.0866754931, 100000000.010298, 0.379778016074574, 36.6115979894593, 1 ),
      ( 400, 0.300689407065313, 999.999999999946, -2.94659986664697E-05, 27.2263281375633, 2 ),
      ( 400, 3.0076918812533, 9999.99999945272, -0.000294714941484073, 27.2305677199578, 2 ),
      ( 400, 30.1570985830241, 99999.9942971978, -0.00295266742487171, 27.2730922768668, 2 ),
      ( 400, 310.012612543361, 999999.999875397, -0.0301022126615001, 27.7117709556534, 2 ),
      ( 400, 5038.04916141038, 9999999.98000107, -0.403180600419257, 34.6237820225369, 2 ),
      ( 400, 23109.7204643151, 100000000, 0.301099887451871, 35.739696312192, 1 ),
      ( 500, 0.240547772325928, 999.997922547845, -1.38631825694674E-05, 28.9129715759901, 2 ),
      ( 500, 2.40577790274957, 9999.99999999969, -0.000138635879907331, 28.9147104560373, 2 ),
      ( 500, 24.0878472321269, 99999.9999970375, -0.00138673562894662, 28.9321254303665, 2 ),
      ( 500, 243.936414822907, 1000000, -0.01390516977161, 29.1088678588787, 2 ),
      ( 500, 2802.87495197385, 10000000, -0.141793902036467, 31.0844968648601, 2 ),
      ( 500, 19672.0139551772, 100000000.000095, 0.222774842014446, 35.1285456332002, 1 ),
      ( 600, 0.200455071176301, 999.998374395753, -6.85044432685324E-06, 30.7112556384955, 2 ),
      ( 600, 2.00467430012478, 9999.99999999991, -6.85001284941239E-05, 30.7121553772, 2 ),
      ( 600, 20.0591014256107, 99999.9999992672, -0.000684558522419007, 30.7211602313563, 2 ),
      ( 600, 201.826346692726, 999999.9932677, -0.00680113734548511, 30.8119344590188, 2 ),
      ( 600, 2139.64262364879, 9999999.99999942, -0.0631440234354396, 31.7670458123503, 2 ),
      ( 600, 16675.6981374107, 100000000, 0.202070799770334, 35.3996722727686, 1 ),
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
      ( 250, 0.481119241549969, 999.999999985845, -6.53966715045915E-05, 25.6442548472692, 2 ),
      ( 250, 4.81402748882245, 9999.99985614766, -0.000654277370542874, 25.6528484873067, 2 ),
      ( 250, 48.4271460213077, 99999.9999999103, -0.00657416860918164, 25.7397126494504, 2 ),
      ( 250, 516.888465812115, 1000000, -0.0692619205632534, 26.7078453722509, 2 ),
      ( 250, 25290.7093973857, 100000000.000007, 0.902231251776453, 34.5638788771068, 1 ),
      ( 300, 0.400921439285007, 999.999999995653, -3.73082007165616E-05, 26.6395692086112, 2 ),
      ( 300, 4.0105613894429, 9999.99995608458, -0.000373158023573725, 26.643695175979, 2 ),
      ( 300, 40.2411185645203, 99999.9999999958, -0.00373922014885372, 26.6851612772253, 2 ),
      ( 300, 416.827660570856, 1000000.00100802, -0.0381960711004995, 27.121098057006, 2 ),
      ( 300, 23328.6056192897, 99999999.9999941, 0.718518835501957, 33.8199325787339, 1 ),
      ( 350, 0.343641905769289, 999.999999999959, -2.26359471630567E-05, 28.0565065289957, 2 ),
      ( 350, 3.43711934403126, 9999.99999407632, -0.000226373558857156, 28.0588208151248, 2 ),
      ( 350, 34.441427579341, 100000, -0.00226514618636275, 28.0820221853709, 2 ),
      ( 350, 351.649653669316, 999999.988816935, -0.0227940690006206, 28.3198754996231, 2 ),
      ( 350, 4502.75758341315, 9999999.99999999, -0.236836270344276, 31.1208252259719, 2 ),
      ( 350, 21461.94745381, 99999999.9999996, 0.601132086679592, 33.9605569201886, 1 ),
      ( 400, 0.300684123232914, 999.999999999862, -1.41743842659384E-05, 29.7571809264714, 2 ),
      ( 400, 3.00722485655688, 9999.99999860869, -0.000141740099613873, 29.7586307421361, 2 ),
      ( 400, 30.1106536687585, 99999.9860141741, -0.00141702472434884, 29.7731476102835, 2 ),
      ( 400, 304.989576118829, 999999.999852647, -0.0141306957189533, 29.9201362786503, 2 ),
      ( 400, 3468.62927044303, 10000000, -0.133145004043039, 31.4818166320766, 2 ),
      ( 400, 19724.3518634334, 100000000.000018, 0.52440933574133, 34.6945496628193, 1 ),
      ( 500, 0.240545226749338, 999.999999999996, -5.56141923368283E-06, 33.5479782293737, 2 ),
      ( 500, 2.40557265242622, 9999.99999994486, -5.56053300716593E-05, 33.5486893163107, 2 ),
      ( 500, 24.06775050953, 99999.9994594954, -0.000555166624447173, 33.5558019840181, 2 ),
      ( 500, 241.865083615546, 999999.999999952, -0.00546252734100508, 33.627089283472, 2 ),
      ( 500, 2518.77505181122, 9999999.99999977, -0.0449965398715735, 34.3345658026566, 2 ),
      ( 500, 16730.373390246, 99999999.999909, 0.437767606051007, 37.1326275696069, 1 ),
      ( 600, 0.200453588642869, 999.988584853359, -1.70117026979086E-06, 37.4375762009797, 2 ),
      ( 600, 2.00456649585531, 9999.99999999888, -1.70050322547914E-05, 37.4379979647798, 2 ),
      ( 600, 20.0487196154735, 99999.9999894006, -0.000169364137176715, 37.4422153083897, 2 ),
      ( 600, 200.779554907494, 999999.999999998, -0.00162523566593969, 37.4843537069939, 2 ),
      ( 600, 2023.87837456269, 10000000.0000881, -0.00955885821425289, 37.8962470636668, 1 ),
      ( 600, 14391.1820569181, 100000000.000207, 0.392889340299708, 40.1385922461386, 1 ),
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
      ( 200, 0.601396216490954, 999.999999964846, -6.29626600798739E-05, 25.1989962846331, 2 ),
      ( 200, 6.01737358920577, 9999.99894712019, -0.000629856015431827, 25.204398804842, 2 ),
      ( 200, 60.5184157503885, 99999.9999936432, -0.00632172280103895, 25.2591438867741, 2 ),
      ( 200, 643.695696546785, 1000000, -0.0657722986008924, 25.880607159612, 2 ),
      ( 200, 16615.1349351054, 10000000.0000001, -0.638065924030009, 30.1309947587872, 1 ),
      ( 200, 25498.7784922427, 100000000.019276, 1.35838101525631, 32.0638971401317, 1 ),
      ( 250, 0.481101904710821, 999.999999990527, -3.16438624574515E-05, 25.9526360915953, 2 ),
      ( 250, 4.81238975349148, 9999.9999044848, -0.00031646347692498, 25.9551517664433, 2 ),
      ( 250, 48.2615182395834, 99999.999999984, -0.00316712291740349, 25.9803998424582, 2 ),
      ( 250, 496.95008217762, 1000000.00015664, -0.0319215188018361, 26.2420542212879, 2 ),
      ( 250, 7015.33400375945, 9999999.99999306, -0.314235529583312, 29.204873032393, 2 ),
      ( 250, 23285.2103750379, 99999999.9999995, 1.06606113081998, 31.3589903030421, 1 ),
      ( 300, 0.400912363041917, 999.99999999897, -1.69506329655512E-05, 27.4615161351531, 2 ),
      ( 300, 4.00973530765286, 9999.99998967808, -0.000169496079918554, 27.4629350733375, 2 ),
      ( 300, 40.158582519499, 99999.9999999999, -0.00169392898933167, 27.4771333184652, 2 ),
      ( 300, 407.76781306229, 999999.995583291, -0.0168288067100793, 27.6199366332544, 2 ),
      ( 300, 4687.75611525488, 9999999.99999999, -0.144781517069596, 28.997199030685, 2 ),
      ( 300, 21268.658000108, 100000000.018535, 0.884959396005695, 31.8250250548155, 1 ),
      ( 350, 0.343636491674881, 999.99999999988, -9.1615841987227E-06, 29.6452345183046, 2 ),
      ( 350, 3.43664823514072, 9999.99999878741, -9.16011523016527E-05, 29.6461290818345, 2 ),
      ( 350, 34.3947897036153, 99999.9882910498, -0.000914538457186787, 29.6550707556048, 2 ),
      ( 350, 346.752030987089, 999999.999980371, -0.00899399942351454, 29.7440651803378, 2 ),
      ( 350, 3694.73057242401, 10000000.0016437, -0.0699366660660115, 30.5570005598363, 2 ),
      ( 350, 19467.1542819469, 99999999.9999993, 0.765195561933439, 33.2299626811399, 1 ),
      ( 400, 0.300680584099089, 999.999999999987, -4.68472679530817E-06, 32.2882613392501, 2 ),
      ( 400, 3.00693258244728, 9999.99999987448, -4.68342792583834E-05, 32.288866966299, 2 ),
      ( 400, 30.0819670914877, 99999.9988193838, -0.000467041999247967, 32.2949183649695, 2 ),
      ( 400, 302.050003103658, 999999.999999943, -0.00453841282013614, 32.3549379798722, 2 ),
      ( 400, 3100.62247744537, 9999999.99999804, -0.0302618984416533, 32.8976066099075, 1 ),
      ( 400, 17883.1608306486, 100000000.003831, 0.681353639588435, 35.2709105494501, 1 ),
      ( 500, 0.240543399010134, 999.996990390302, -2.389062523217E-07, 38.1830764007508, 2 ),
      ( 500, 2.40543913116599, 10000, -2.38094754151948E-06, 38.1833932906391, 2 ),
      ( 500, 24.0548872271243, 99999.9999999545, -2.29968946865829E-05, 38.186559569605, 2 ),
      ( 500, 240.579031781772, 1000000.00078623, -0.000148356186148391, 38.217961755567, 1 ),
      ( 500, 2388.88694260629, 10000000.0003461, 0.00692643131893743, 38.5071206045532, 1 ),
      ( 500, 15306.3325681779, 100000000.001584, 0.571528250314871, 40.3045260710596, 1 ),
      ( 600, 0.200452450776072, 1000.01730774622, 1.62813556438941E-06, 44.1639541370177, 1 ),
      ( 600, 2.00449519147255, 10000.0000000025, 1.62859684503187E-05, 44.1641382259938, 1 ),
      ( 600, 20.0420045224769, 100000.000026107, 0.000163349115118729, 44.1659780355809, 1 ),
      ( 600, 200.116098119681, 999999.999999998, 0.00168245106241711, 44.1842690890183, 1 ),
      ( 600, 1961.98730727546, 10000000.0000952, 0.0216823672534932, 44.3576619525232, 1 ),
      ( 600, 13357.8490871742, 100000000.000043, 0.50063668449665, 45.7124859828639, 1 ),
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
