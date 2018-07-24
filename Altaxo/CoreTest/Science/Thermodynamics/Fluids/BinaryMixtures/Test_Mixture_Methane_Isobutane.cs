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
	/// Tests and test data for <see cref="Mixture_Methane_Isobutane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Methane_Isobutane : MixtureTestBase
    {

    public Test_Mixture_Methane_Isobutane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-82-8", 0.5), ("75-28-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 150, 12154.6234411862, 100000.000004894, -0.993403211580053, 72.4923973133365, 1 ),
      ( 150, 12162.2087282835, 1000000.00004325, -0.93407325841918, 72.5626365593755, 1 ),
      ( 150, 12235.9512484997, 10000000.0000027, -0.344705797214996, 73.2342695144965, 1 ),
      ( 200, 0.601934763255321, 999.99999999269, -0.000953042400839735, 63.517087157161, 2 ),
      ( 200, 11322.6622372307, 99999.999999775, -0.994688871926466, 79.1287497482466, 1 ),
      ( 200, 11334.2028454077, 1000000.00181143, -0.946942797557696, 79.1909883862838, 1 ),
      ( 200, 11444.1680260427, 9999999.99999734, -0.474526158187027, 79.7823139892727, 1 ),
      ( 250, 0.481305229664678, 999.999999997274, -0.000449516291744427, 75.592226243718, 2 ),
      ( 250, 4.83272042320923, 9999.99997110842, -0.00451747047553816, 75.7130121712898, 2 ),
      ( 250, 10436.3830478139, 99999.9999986961, -0.995390271991991, 87.5234874111241, 1 ),
      ( 250, 10454.8820699307, 999999.999999049, -0.953984284883769, 87.581190903745, 1 ),
      ( 250, 10624.8675113413, 10000000.0000487, -0.547204824334874, 88.1334608212258, 1 ),
      ( 300, 0.401007217097606, 999.999999999557, -0.000248926098638451, 88.7717061380205, 2 ),
      ( 300, 4.01910130256959, 9999.99999540322, -0.00249492173553303, 88.826559961285, 2 ),
      ( 300, 41.1416854446964, 99999.9999999995, -0.0255445502483453, 89.4110850666161, 2 ),
      ( 300, 9460.47863857071, 999999.999999512, -0.957622926782921, 98.2051858742952, 1 ),
      ( 300, 9747.98761894973, 10000000.002319, -0.588728041422058, 98.6450482668598, 1 ),
      ( 350, 0.343687203861663, 999.999999999952, -0.000152153060094313, 102.500386023557, 2 ),
      ( 350, 3.44159176697848, 9999.99999950317, -0.00152332388962466, 102.528919911108, 2 ),
      ( 350, 34.9015882754437, 99999.9922564089, -0.015417554891009, 102.823037296574, 2 ),
      ( 350, 419.258229197613, 999999.99991632, -0.180374082417471, 106.988302973319, 2 ),
      ( 350, 8759.91989443972, 9999999.99999987, -0.607719117363362, 110.618519991554, 1 ),
      ( 400, 0.300710365935793, 999.997819011034, -9.91615936326997E-05, 116.040981812417, 2 ),
      ( 400, 3.00979194865609, 9999.99999996519, -0.000992254345146469, 116.057564272716, 2 ),
      ( 400, 30.371378578769, 99999.9994221232, -0.00998716192106572, 116.22605704808, 2 ),
      ( 400, 336.919531000013, 999999.999445091, -0.107559760393854, 118.235992045501, 2 ),
      ( 400, 7554.60562528622, 10000000.0000018, -0.60199041238463, 123.403123072861, 1 ),
      ( 500, 0.24055580350175, 999.997836816195, -4.72485622103144E-05, 140.833089674094, 2 ),
      ( 500, 2.40658161850256, 9999.99999999887, -0.000472555296111524, 140.839991643569, 2 ),
      ( 500, 24.1688211414606, 99999.9999866256, -0.00473243540226998, 140.909298266972, 2 ),
      ( 500, 252.679856969928, 999999.999999989, -0.0480268572023773, 141.63198279318, 2 ),
      ( 500, 4094.83179825776, 9999999.99999998, -0.41256576725896, 147.748439957724, 1 ),
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
      ( 150, 16548.2625772702, 10000000.0000027, -0.515470002055839, 44.4991173515236, 1 ),
      ( 200, 0.601570473770631, 999.999999998834, -0.000350335228535291, 44.3670882212696, 2 ),
      ( 200, 15030.228556175, 10000000.0001681, -0.599899815094566, 54.7964007263957, 1 ),
      ( 250, 0.481172983881554, 999.999998413268, -0.0001770796185538, 50.7938762640099, 2 ),
      ( 250, 4.81942356443491, 9999.99999999996, -0.00177319647993451, 50.8209599797398, 2 ),
      ( 250, 48.9895705227894, 99999.9999999995, -0.0179791885924434, 51.1038162957644, 2 ),
      ( 250, 13321.2239866902, 10000000.0000001, -0.638856175353479, 59.4893861385983, 1 ),
      ( 300, 0.400946997332363, 999.999999791594, -0.00010105002692561, 58.1465482356329, 2 ),
      ( 300, 4.01312248540548, 9999.99786007577, -0.00101110002890156, 58.1597864716388, 2 ),
      ( 300, 40.502637096377, 99999.9988527182, -0.010171903602194, 58.2945325340696, 2 ),
      ( 300, 11094.2885236934, 10000000.000142, -0.638637051149171, 65.3850079893448, 1 ),
      ( 350, 0.343655403212206, 999.999999964242, -6.19111550100327E-05, 66.1101517786452, 2 ),
      ( 350, 3.43847059468261, 9999.99963749194, -0.000619264714916578, 66.1176769464751, 2 ),
      ( 350, 34.5780755805934, 99999.9999994911, -0.00620806289533743, 66.1935020806602, 2 ),
      ( 350, 367.021876267753, 1000000, -0.0637230385183469, 67.01280389882, 2 ),
      ( 350, 7487.20861728754, 10000000, -0.5410383967186, 73.7821945910177, 1 ),
      ( 400, 0.300691762807984, 999.999999992751, -3.95806897553194E-05, 74.2087435694847, 2 ),
      ( 400, 3.0079892670997, 9999.99992698241, -0.000395830828887008, 74.2135035723064, 2 ),
      ( 400, 30.1875496909405, 99999.9999999886, -0.003960691401922, 74.2612544499011, 2 ),
      ( 400, 313.153324123718, 1000000.00022699, -0.0398318074408471, 74.7539296196145, 2 ),
      ( 400, 4556.73539599349, 10000000, -0.340141932566541, 79.5644774834501, 1 ),
      ( 500, 0.24054799340763, 999.999999999621, -1.70628367533167E-05, 89.564076835595, 2 ),
      ( 500, 2.4058493379308, 9999.99999620033, -0.000170604268231528, 89.56643228891, 2 ),
      ( 500, 24.0954384107933, 100000, -0.00170362175699862, 89.5899850817632, 2 ),
      ( 500, 244.649767480629, 999999.999400047, -0.0167826789455141, 89.8251472842416, 2 ),
      ( 500, 2756.10479809198, 9999999.99462174, -0.127232429069249, 91.8807008364253, 1 ),
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
      ( 150, 0.801927714304807, 999.999997547998, -0.000145374271571952, 25.0179369250697, 2 ),
      ( 150, 8.02979938227655, 9999.99999999995, -0.00145558254236871, 25.0348888983924, 2 ),
      ( 150, 23402.6227643714, 10000000.0001053, -0.657384070696605, 31.1145923761076, 1 ),
      ( 200, 0.601396348980959, 999.999999884196, -6.31829501769109E-05, 25.237455864754, 2 ),
      ( 200, 6.01738686380832, 9999.99881680524, -0.000632060658538248, 25.2428825725337, 2 ),
      ( 200, 60.5197692999213, 99999.999993297, -0.006343946823852, 25.2978762131732, 2 ),
      ( 200, 643.863089344516, 1000000, -0.0660151809639962, 25.9225136393857, 2 ),
      ( 200, 16625.693515076, 9999999.99998811, -0.638295779698082, 30.1692543350212, 1 ),
      ( 250, 0.481101959589689, 999.999999990399, -3.17579279499215E-05, 26.0029326201296, 2 ),
      ( 250, 4.81239524592125, 9999.9999032078, -0.000317604424640391, 26.0054595987046, 2 ),
      ( 250, 48.2620720573361, 99999.9999999835, -0.0031785617909726, 26.0308215659436, 2 ),
      ( 250, 497.01037353115, 1000000.0001626, -0.0320389545037211, 26.2937023885409, 2 ),
      ( 250, 7027.81946945648, 9999999.99999529, -0.315453843287752, 29.2709642357996, 2 ),
      ( 300, 0.400912389077094, 999.999999998954, -1.70155716823975E-05, 27.5245289375907, 2 ),
      ( 300, 4.00973791208083, 9999.99998953375, -0.000170145495559687, 27.5259543644059, 2 ),
      ( 300, 40.1588438728967, 100000, -0.00170042595632446, 27.540217601879, 2 ),
      ( 300, 407.79486032425, 999999.995442944, -0.0168940161825965, 27.6836810080554, 2 ),
      ( 300, 4690.92666418756, 9999999.99999999, -0.145359550418309, 29.0672573122134, 2 ),
      ( 350, 0.343636505093153, 999.999999999878, -9.20063170613814E-06, 29.7213412914101, 2 ),
      ( 350, 3.43664957711288, 9999.99999876894, -9.19916050396477E-05, 29.7222400475215, 2 ),
      ( 350, 34.3949240445719, 99999.9881116079, -0.000918440719741837, 29.7312236270893, 2 ),
      ( 350, 346.765595735259, 999999.999979669, -0.00903276551506337, 29.8206348495608, 2 ),
      ( 350, 3696.00886592476, 10000000.0016071, -0.0702583357258359, 30.6371435177755, 2 ),
      ( 400, 0.300680591362473, 999.999999999987, -4.70888315996615E-06, 32.3771642567952, 2 ),
      ( 400, 3.0069333087652, 9999.99999987222, -4.70758156780986E-05, 32.3777728067552, 2 ),
      ( 400, 30.0820397020625, 99999.9987980754, -0.000469454623509524, 32.3838533973941, 2 ),
      ( 400, 302.057235430711, 999999.999999941, -0.00456224771911534, 32.4441611233382, 2 ),
      ( 400, 3101.2320326688, 9999999.99999804, -0.0304525029881498, 32.9892268042388, 1 ),
      ( 500, 0.240543401266396, 999.996870842906, -2.48102136991138E-07, 38.2951086704945, 2 ),
      ( 500, 2.40543935232916, 9999.99999999997, -2.47289026804343E-06, 38.2954272022551, 2 ),
      ( 500, 24.0549093040601, 99999.999999947, -2.39146461468085E-05, 38.2986098738527, 2 ),
      ( 500, 240.581198161731, 1000000.00057479, -0.000157359626891197, 38.3301733832668, 1 ),
      ( 500, 2389.04565492694, 10000000.0003485, 0.00685953781679601, 38.6206881571672, 1 ),
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
