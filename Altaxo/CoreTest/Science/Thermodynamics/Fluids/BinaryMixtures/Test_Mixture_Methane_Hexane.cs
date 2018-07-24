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
	/// Tests and test data for <see cref="Mixture_Methane_Hexane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Methane_Hexane : MixtureTestBase
    {

    public Test_Mixture_Methane_Hexane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-82-8", 0.5), ("110-54-3", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 8616.0198675044, 100000.000001506, -0.993020462522527, 124.045449048563, 1 ),
      ( 200, 8622.09480857048, 1000000.00000171, -0.930253800216296, 124.093436925521, 1 ),
      ( 200, 8680.68256701606, 10000000.0000001, -0.307245319091895, 124.56868256214, 1 ),
      ( 200, 9127.51893315099, 100000000.007241, 5.58840976044926, 128.862944564444, 1 ),
      ( 250, 0.481824173761465, 999.988777228578, -0.00153061411645774, 117.553959013926, 2 ),
      ( 250, 8104.9499784059, 99999.9999984165, -0.994064285742142, 136.091836912725, 1 ),
      ( 250, 8113.40671067521, 1000000.00115143, -0.940704725443168, 136.135850628751, 1 ),
      ( 250, 8193.66314515792, 10000000.0000014, -0.412855190601326, 136.572912590917, 1 ),
      ( 250, 8755.64328980946, 100000000.011687, 4.4945897487925, 140.489512810779, 1 ),
      ( 300, 0.401207252252354, 999.999987555732, -0.000751947417544081, 134.884458267585, 2 ),
      ( 300, 4.03964033763667, 9999.99036619804, -0.00757113260507474, 135.225293561281, 2 ),
      ( 300, 7583.33131412029, 100000.00000013, -0.994713331846241, 150.718690661733, 1 ),
      ( 300, 7595.52859606064, 1000000.01796925, -0.947218213020793, 150.758085074351, 1 ),
      ( 300, 7708.29474875968, 10000000.0000298, -0.47990369004853, 151.156588999864, 1 ),
      ( 300, 8412.45892725748, 100000000.017699, 3.76561691443798, 154.819176590261, 1 ),
      ( 350, 0.343778595450217, 999.99999836839, -0.000422520779896892, 153.836864606424, 2 ),
      ( 350, 3.45096951404924, 9999.98223705954, -0.00424114711290339, 154.003162990814, 2 ),
      ( 350, 35.950742919618, 99999.9999996299, -0.0441551023907854, 155.748364190246, 2 ),
      ( 350, 7040.99730106379, 999999.999999297, -0.9511953595583, 167.53277686094, 1 ),
      ( 350, 7207.03634878019, 10000000.0005072, -0.523197434792452, 167.8576114913, 1 ),
      ( 350, 8090.03784935118, 100000000, 3.24761105261421, 171.297483559076, 1 ),
      ( 400, 0.300757454099415, 999.999999704528, -0.00026027611227848, 173.058269394757, 2 ),
      ( 400, 3.01465579906003, 9999.99689162476, -0.00260860805937813, 173.147862297625, 2 ),
      ( 400, 30.8927603347198, 99999.9995965913, -0.0267002012936475, 174.068909440277, 2 ),
      ( 400, 6405.80419774623, 999999.999999925, -0.953061447893728, 185.19330013825, 1 ),
      ( 400, 6672.06023164355, 10000000.0009641, -0.549345833660667, 185.312847268523, 1 ),
      ( 400, 7784.39591350808, 100000000.000001, 2.86258840714377, 188.483743846752, 1 ),
      ( 500, 0.240571620820747, 999.999999998975, -0.000117559693245797, 208.86909636099, 2 ),
      ( 500, 2.40826698324465, 9999.99985046468, -0.00117660968371517, 208.901252866819, 2 ),
      ( 500, 24.3432693883829, 99999.999999707, -0.011869213039873, 209.22678730894, 2 ),
      ( 500, 276.922325871725, 999999.984100622, -0.131368916566472, 212.969124421687, 2 ),
      ( 500, 5396.98208507707, 10000000.0016446, -0.55430028201437, 219.148164310815, 1 ),
      ( 500, 7216.91353220687, 99999999.9999997, 2.33305003893047, 221.200235982728, 1 ),
      ( 600, 0.200464936490522, 999.999999998203, -6.0627783933717E-05, 239.551643373105, 2 ),
      ( 600, 2.00574417161155, 9999.99998182398, -0.00060643035627441, 239.565777710654, 2 ),
      ( 600, 20.1678925097526, 99999.999999999, -0.00607967511800704, 239.708059393792, 2 ),
      ( 600, 213.804413041307, 1000000.00009965, -0.0624478704957016, 241.225485072552, 2 ),
      ( 600, 3649.2236959529, 10000000, -0.450697464866139, 249.782461838187, 1 ),
      ( 600, 6704.99628405561, 100000000, 1.98960318921946, 249.684192111233, 1 ),
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
      ( 200, 12729.3831821936, 10000000.0000008, -0.527582491916878, 77.4498949638782, 1 ),
      ( 200, 13784.0347981107, 100000000.00003, 3.36271641101478, 80.2551794029588, 1 ),
      ( 250, 0.481253250251089, 999.99999999659, -0.000346120595340447, 71.7673575860106, 2 ),
      ( 250, 11724.9259433665, 10000000.0002964, -0.589688940529683, 83.0087155321201, 1 ),
      ( 250, 13109.579697616, 100000000.000299, 2.66973381059966, 85.8349451714849, 1 ),
      ( 300, 0.400981335144755, 999.999997555653, -0.000188960549025083, 81.2138196545023, 2 ),
      ( 300, 4.01665796369386, 9999.99999601423, -0.00189269508151645, 81.2601203314149, 2 ),
      ( 300, 12477.1174448382, 99999999.995299, 2.21312648732211, 93.271929768163, 1 ),
      ( 350, 0.343672681194454, 999.999999592837, -0.000114467476984612, 91.7981074053277, 2 ),
      ( 350, 3.44027482956102, 9999.99577187171, -0.00114566701499973, 91.821250719599, 2 ),
      ( 350, 34.7651423085604, 99999.9864643797, -0.0115577854960261, 92.0564299840911, 2 ),
      ( 350, 11878.0199718121, 99999999.9957377, 1.89301872410012, 102.213731528062, 1 ),
      ( 400, 0.300701434902129, 999.999999914016, -7.40295224560539E-05, 102.742683889966, 2 ),
      ( 400, 3.00902032831209, 9999.9991217264, -0.000740635385562954, 102.75558555354, 2 ),
      ( 400, 30.2933222074493, 99999.9999430304, -0.00744074193610298, 102.885846533751, 2 ),
      ( 400, 326.207578335764, 999999.999999999, -0.0782581580339275, 104.321628914297, 2 ),
      ( 400, 11310.0053703292, 99999999.9999151, 1.65852370775633, 111.82549772195, 1 ),
      ( 500, 0.24055165915671, 999.999999999668, -3.45865912550202E-05, 123.614914277474, 2 ),
      ( 500, 2.40626567709482, 9999.99995248166, -0.000345882065630472, 123.619986245008, 2 ),
      ( 500, 24.137861607058, 99999.9999999937, -0.00346044230175121, 123.670908702851, 2 ),
      ( 500, 249.20840584334, 1000000.00008694, -0.0347703622595518, 124.199924180039, 2 ),
      ( 500, 3510.81541153694, 10000000, -0.314850508789649, 129.372241477718, 1 ),
      ( 500, 10268.3135083637, 100000000.000009, 1.34257883827653, 130.716745418995, 1 ),
      ( 600, 0.200456223397147, 999.999999999489, -1.7164104076606E-05, 141.96180439839, 2 ),
      ( 600, 2.00487188230606, 9999.9999948899, -0.000171609394227291, 141.964294005079, 2 ),
      ( 600, 20.0796733280118, 99999.9999999999, -0.00171292893382806, 141.989234666177, 2 ),
      ( 600, 203.88039243026, 999999.99909639, -0.0168118652348322, 142.242729224229, 2 ),
      ( 600, 2300.7090189141, 10000000, -0.128734746125017, 144.686382602461, 2 ),
      ( 600, 9355.86471544685, 100000000.000002, 1.14253614008243, 147.689176558525, 1 ),
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
      ( 200, 0.601396443786904, 999.999999883038, -6.33451532598E-05, 25.2762027314964, 2 ),
      ( 200, 6.01739661385558, 9999.99860291126, -0.000633684497972043, 25.2816521243751, 2 ),
      ( 200, 16648.3192854941, 9999999.99999354, -0.638787352690483, 30.2037384505433, 1 ),
      ( 200, 25452.9226888695, 100000000.019046, 1.36262984681424, 32.1558563037012, 1 ),
      ( 250, 0.481102000663762, 999.999999990296, -3.18478703942628E-05, 26.0448625825213, 2 ),
      ( 250, 4.81239955552447, 9999.99990215381, -0.000318504229702409, 26.0473972798569, 2 ),
      ( 250, 48.2625093438836, 99999.9999999833, -0.00318759813161338, 26.0728374531207, 2 ),
      ( 250, 497.058856531827, 1000000.00016907, -0.032133373613736, 26.3366040844798, 2 ),
      ( 250, 7041.23761320983, 9999999.99999654, -0.316758352697681, 29.3273421769873, 2 ),
      ( 250, 23247.6644630599, 100000000, 1.06939789308314, 31.4642636604566, 1 ),
      ( 300, 0.400912409689274, 999.999999999676, -1.70715542251876E-05, 27.5706563826971, 2 ),
      ( 300, 4.00974013914622, 9999.99999044456, -0.000170705384511804, 27.5720850829721, 2 ),
      ( 300, 40.1590691743494, 100000, -0.0017060312050348, 27.586381296647, 2 ),
      ( 300, 407.818376406734, 999999.995308677, -0.0169507096372853, 27.7301982754422, 2 ),
      ( 300, 4694.12011457818, 10000000, -0.145940973588916, 29.1184584206726, 2 ),
      ( 300, 21238.1648761053, 99999999.9999999, 0.887665755634015, 31.945714390865, 1 ),
      ( 350, 0.343636516347969, 999.999999999876, -9.23795378167745E-06, 29.7727137502335, 2 ),
      ( 350, 3.43665084408494, 9999.99999875083, -9.23648057867005E-05, 29.7736141047532, 2 ),
      ( 350, 34.3950522988723, 99999.9879355623, -0.000922170718051413, 29.7826137403273, 2 ),
      ( 350, 346.778574055302, 999999.999978936, -0.00906985736962642, 29.8721924575048, 2 ),
      ( 350, 3697.32335603334, 10000000.0017229, -0.0705888861837441, 30.6906727132382, 2 ),
      ( 350, 19442.3334472999, 100000000.000001, 0.767449070763938, 33.3677640399029, 1 ),
      ( 400, 0.300680597808547, 999.999999999986, -4.73489163849335E-06, 32.4342302346775, 2 ),
      ( 400, 3.00693407699364, 9999.99999986975, -4.73358593863724E-05, 32.4348396420896, 2 ),
      ( 400, 30.082117704289, 99999.9987748102, -0.000472050950822825, 32.4409288327861, 2 ),
      ( 400, 302.064987193422, 999999.99999994, -0.00458779775535462, 32.5013249910946, 2 ),
      ( 400, 3101.9004816655, 9999999.99999826, -0.0306614416041592, 33.0473759819327, 1 ),
      ( 400, 17862.7274209875, 100000000.003382, 0.683276954510785, 35.4261673805609, 1 ),
      ( 500, 0.240543403519857, 999.996693666068, -2.61767289873329E-07, 38.3632095402837, 2 ),
      ( 500, 2.40543966994954, 9999.99999999998, -2.60950279899661E-06, 38.3635283640524, 2 ),
      ( 500, 24.0549419611041, 99999.9999999348, -2.52767861644061E-05, 38.3667139617063, 2 ),
      ( 500, 240.584379602755, 1000000.0002863, -0.000170585920951012, 38.3983073110392, 1 ),
      ( 500, 2389.27275029192, 10000000.0003494, 0.00676383332771898, 38.6891550772277, 1 ),
      ( 500, 15291.7533987936, 100000000.00165, 0.573026539361795, 40.4922385903925, 1 ),
      ( 600, 0.200452451946404, 1000.01724133472, 1.6179305683345E-06, 44.3730022927104, 1 ),
      ( 600, 2.00449538678403, 10000.0000000025, 1.61839597358789E-05, 44.3731875333511, 1 ),
      ( 600, 20.042024795192, 100000.000025767, 0.000162332868492546, 44.3750388480524, 1 ),
      ( 600, 200.118050898223, 1000000, 0.00167267193396115, 44.3934438030836, 1 ),
      ( 600, 1962.10328517752, 10000000.0000988, 0.0216219719930922, 44.5678654147974, 1 ),
      ( 600, 13346.7236783386, 100000000.000014, 0.501887561147367, 45.9277568425488, 1 ),
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
