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
	/// Tests and test data for <see cref="Mixture_Cl2_Methane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Cl2_Methane : MixtureTestBase
    {

    public Test_Mixture_Cl2_Methane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("7782-50-5", 0.5), ("74-82-8", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601396264324361, 999.999999964706, -6.30410017121595E-05, 25.1973132845753, 2 ),
      ( 200, 6.0173783180439, 9999.99894296428, -0.000630640193690324, 25.2027272357746, 2 ),
      ( 200, 60.5188981317728, 99999.9999935791, -0.00632964198500577, 25.257590119069, 2 ),
      ( 200, 643.756556126439, 1000000, -0.0658606176846143, 25.8805894990106, 2 ),
      ( 200, 16631.6417356066, 9999999.99999997, -0.638425140908378, 30.1277660422553, 1 ),
      ( 250, 0.481101926616256, 999.999999990485, -3.16882021606755E-05, 25.951957744264, 2 ),
      ( 250, 4.81239189451097, 9999.99990406293, -0.000316907042960562, 25.9544766380321, 2 ),
      ( 250, 48.2617338736965, 99999.9999999839, -0.00317157559393012, 25.9797573507766, 2 ),
      ( 250, 496.973874369576, 1000000.00015987, -0.0319678635645512, 26.2417846062479, 2 ),
      ( 250, 7022.05216248811, 9999999.99999352, -0.314891615400953, 29.2114257923611, 2 ),
      ( 300, 0.40091237479866, 999.999999998966, -1.69787667673846E-05, 27.4613558636524, 2 ),
      ( 300, 4.00973644089072, 9999.99998962437, -0.000169777463059622, 27.4627758622946, 2 ),
      ( 300, 40.1586959406814, 100000, -0.00169674734079147, 27.4769847698095, 2 ),
      ( 300, 407.779692863479, 999999.995516022, -0.016857448157039, 27.6199005124801, 2 ),
      ( 300, 4689.46127683458, 9999999.99999999, -0.145092486888882, 28.9985497873527, 2 ),
      ( 350, 0.343636498626613, 999.999999999879, -9.18062323441122E-06, 29.6451792728293, 2 ),
      ( 350, 3.43664889363567, 9999.99999877956, -9.17915537780317E-05, 29.6460743151494, 2 ),
      ( 350, 34.3948553309526, 99999.9882137329, -0.000916443578454621, 29.6550207754257, 2 ),
      ( 350, 346.758734426979, 999999.999980006, -0.00901315608586884, 29.7440629024913, 2 ),
      ( 350, 3695.48225753013, 10000000.0017404, -0.0701258458853615, 30.5573888866447, 2 ),
      ( 400, 0.300680588496671, 999.999999999986, -4.69816147091879E-06, 32.2880185817367, 2 ),
      ( 400, 3.00693299002018, 9999.99999987337, -4.6968626666632E-05, 32.2886245052684, 2 ),
      ( 400, 30.0820075624103, 99999.9988086737, -0.00046838553382904, 32.2946788595665, 2 ),
      ( 400, 302.054080500638, 999999.999999945, -0.00455184926834683, 32.3547271132261, 2 ),
      ( 400, 3101.02928089484, 9999999.99999835, -0.0303891107924276, 32.8975854764895, 1 ),
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
      ( 200, 0.601471457864671, 999.999998917521, -0.000185174881504625, 24.2928369154952, 2 ),
      ( 200, 6.02477806578937, 9999.99999527475, -0.00185521542339827, 24.3286323700492, 2 ),
      ( 200, 22985.3963967798, 10000000.0000468, -0.738372978181361, 32.8804794552953, 1 ),
      ( 250, 0.481132878776423, 999.999999909817, -9.3143519195243E-05, 25.305721707531, 2 ),
      ( 250, 4.81536925756478, 9999.99907391772, -0.000932143152267016, 25.3216983503117, 2 ),
      ( 250, 48.5649965606851, 99999.9951841812, -0.00939339289227776, 25.4844971893957, 2 ),
      ( 250, 20013.2728216983, 9999999.99999935, -0.759615496849092, 32.0475027513024, 1 ),
      ( 300, 0.400928098208385, 999.999999987992, -5.33210387093937E-05, 26.5598045828045, 2 ),
      ( 300, 4.01120674901272, 9999.99987825676, -0.000533392084478977, 26.5677996575173, 2 ),
      ( 300, 40.306402488437, 99999.9999999503, -0.00535226274966474, 26.6485074405788, 2 ),
      ( 300, 424.486030135321, 1000000, -0.0555479053623116, 27.5358363110436, 2 ),
      ( 350, 0.343645703914707, 999.999999997847, -3.3092874267782E-05, 28.0290411196164, 2 ),
      ( 350, 3.43748104354734, 9999.99997829789, -0.00033097684731513, 28.0330030465772, 2 ),
      ( 350, 34.4777132532697, 99999.9999999991, -0.00331460741363128, 28.0728313128159, 2 ),
      ( 350, 355.603697896021, 1000000.00002788, -0.033659284975224, 28.4927884457914, 2 ),
      ( 400, 0.300686518271017, 999.999999999975, -2.15441708138531E-05, 29.6358555933998, 2 ),
      ( 400, 3.00744835757071, 9999.9999962228, -0.000215450175817122, 29.6379181806904, 2 ),
      ( 400, 30.1329511662753, 99999.9999999999, -0.00215535288681599, 29.6585998362276, 2 ),
      ( 400, 307.331531942162, 999999.996060784, -0.0216427245343038, 29.8710509794157, 2 ),
      ( 400, 3873.25603517846, 10000000, -0.223702132008663, 32.4978339930469, 2 ),
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
      ( 200, 0.601631323364274, 999.999999991079, -0.000447972251549049, 23.3826344197033, 2 ),
      ( 200, 6.0407995554809, 9999.99999999635, -0.0044996465885223, 23.3969034978506, 2 ),
      ( 200, 23550.9590801612, 100000.000001555, -0.997446550660126, 36.7389454262338, 1 ),
      ( 200, 23566.4875183494, 1000000.0000807, -0.97448233179425, 36.7567190890778, 1 ),
      ( 200, 23717.2647735199, 10000000.0000699, -0.746445547169061, 36.9330740031103, 1 ),
      ( 250, 0.481195216766584, 999.999999998145, -0.000219805563964888, 24.6610317704643, 2 ),
      ( 250, 4.82151463512908, 9999.99998086252, -0.00220266080187585, 24.7044223165045, 2 ),
      ( 250, 49.2167500796287, 99999.999999992, -0.0225087056698753, 25.1523537921883, 2 ),
      ( 250, 21649.5627125997, 1000000.00009235, -0.977778329576548, 35.0521667641493, 1 ),
      ( 250, 21873.0104959746, 9999999.99999945, -0.780053391613073, 35.1869911562451, 1 ),
      ( 300, 0.40095630682378, 999.999999999584, -0.00012079622093994, 25.6596994919844, 2 ),
      ( 300, 4.01393261551178, 9999.99999577955, -0.00120925979624207, 25.6872681564705, 2 ),
      ( 300, 40.5869853659577, 100000, -0.0122255462981453, 25.9674741113738, 2 ),
      ( 300, 19565.0200178714, 1000000.00000043, -0.979508946455135, 34.4543019844198, 1 ),
      ( 300, 19929.1601211746, 10000000.0000008, -0.798833533184995, 34.4365657713155, 1 ),
      ( 350, 0.343660424786493, 999.999999999894, -7.30524294382759E-05, 26.4139965504506, 2 ),
      ( 350, 3.43886683347792, 9999.99999893179, -0.00073094947361959, 26.4307962391821, 2 ),
      ( 350, 34.618065736011, 99999.9882986198, -0.00735262765371904, 26.6004487735847, 2 ),
      ( 350, 372.942261412442, 999999.993960982, -0.0785830534522933, 28.4882371315035, 2 ),
      ( 350, 17694.6222639465, 9999999.99999909, -0.8057967474911, 34.0269054796315, 1 ),
      ( 400, 0.300695183818727, 999.991640505214, -4.74869145207371E-05, 26.9844588597922, 2 ),
      ( 400, 3.00823803201806, 9999.99999976161, -0.000475024208224249, 26.9947432740974, 2 ),
      ( 400, 30.2120658654452, 99999.9969605704, -0.00476549335441954, 27.0983049698887, 2 ),
      ( 400, 316.285607473858, 999999.999911978, -0.0493373789095107, 28.2120227166825, 2 ),
      ( 400, 14638.8548600216, 10000000.0000558, -0.794600802119121, 35.3245156123112, 1 ),
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
