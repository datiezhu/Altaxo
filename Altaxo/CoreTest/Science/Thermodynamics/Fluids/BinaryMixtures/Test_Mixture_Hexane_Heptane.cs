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
	/// Tests and test data for <see cref="Mixture_Hexane_Heptane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Hexane_Heptane : MixtureTestBase
    {

    public Test_Mixture_Hexane_Heptane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("110-54-3", 0.5), ("142-82-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 7598.41509699972, 100000.000000692, -0.992085739812452, 151.717195760574, 1 ),
      ( 200, 7603.13016135546, 1000000.00000553, -0.920906477359471, 151.777900769202, 1 ),
      ( 200, 7648.78908870382, 9999999.99999957, -0.213786206904334, 152.377128175903, 1 ),
      ( 250, 7183.22272053836, 99999.9999992634, -0.993302634655334, 162.764267491603, 1 ),
      ( 250, 7189.64348640855, 1000000.00011361, -0.933086156679608, 162.819997965683, 1 ),
      ( 250, 7250.99042635861, 10000000.0000006, -0.336522805487629, 163.369893465262, 1 ),
      ( 250, 7695.85832231959, 100000000, 5.2512413617895, 168.175623317505, 1 ),
      ( 300, 0.401358364444153, 999.999943098164, -0.0011281661652643, 157.70524783982, 2 ),
      ( 300, 6767.35960873273, 99999.9999973175, -0.994075893982299, 178.486799262627, 1 ),
      ( 300, 6776.32338697538, 1000000.00780147, -0.940837302907225, 178.538089471592, 1 ),
      ( 300, 6860.19928204607, 10000000.0000073, -0.415606531254523, 179.047699974198, 1 ),
      ( 350, 0.343847212928702, 999.999992961703, -0.000621994506353414, 179.940876652406, 2 ),
      ( 350, 3.45796230845148, 9999.99999999796, -0.00625480784704479, 180.215080069157, 2 ),
      ( 350, 6330.87741945462, 100000.000003697, -0.994572105585145, 197.26305521025, 1 ),
      ( 350, 6344.02361615912, 999999.999999736, -0.945833533630874, 197.306605347336, 1 ),
      ( 350, 6462.74401296252, 10000000.000164, -0.468285698525141, 197.756829163017, 1 ),
      ( 400, 0.300792746758518, 999.999998779873, -0.000377577721210189, 202.330928579258, 2 ),
      ( 400, 3.01822558650345, 9999.98683875295, -0.00378826229307928, 202.478265145984, 2 ),
      ( 400, 31.2955983969606, 99999.9999999997, -0.0392285512338987, 204.013545834552, 2 ),
      ( 400, 5867.29626927039, 1000000.00287646, -0.948753367619299, 217.189341742215, 1 ),
      ( 400, 6045.92878780604, 10000000.0023727, -0.502674965688517, 217.502321526188, 1 ),
      ( 500, 0.240583709595253, 999.999999928967, -0.000167801471339675, 243.756335502168, 2 ),
      ( 500, 2.40948179426103, 9999.99926683754, -0.00168019572637516, 243.808867215927, 2 ),
      ( 500, 24.4709773710287, 99999.9999913117, -0.0170260237339486, 244.343422009041, 2 ),
      ( 500, 301.426439157942, 999999.999918904, -0.201983276670878, 251.038134255984, 2 ),
      ( 500, 5088.79256862474, 9999999.99999991, -0.527307635257375, 255.720798604621, 1 ),
      ( 600, 0.200470132058179, 999.99999999292, -8.65431290679599E-05, 279.069211346654, 2 ),
      ( 600, 2.00626491464954, 9999.9999281252, -0.000865831410123061, 279.092066006382, 2 ),
      ( 600, 20.2211770732692, 99999.9999999709, -0.00869874168869628, 279.322719599649, 2 ),
      ( 600, 220.633255398066, 1000000, -0.091466141928558, 281.854064764876, 2 ),
      ( 600, 3833.66304312542, 9999999.99995309, -0.477124670347587, 290.346035647112, 1 ),
      ( 600, 6001.13061833897, 99999999.9999999, 2.34025028772242, 291.086240357715, 1 ),
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
      ( 200, 8068.19610651291, 100000.00000058, -0.992546557751599, 137.602703902666, 1 ),
      ( 200, 8073.53521491651, 1000000.00044409, -0.925514866601026, 137.657058960005, 1 ),
      ( 200, 8125.13124383514, 9999999.99999807, -0.259878603633062, 138.194371377708, 1 ),
      ( 250, 7609.31473674009, 100000.000001908, -0.993677661050566, 149.272737466115, 1 ),
      ( 250, 7616.66512161384, 1000000.0005193, -0.936837622460454, 149.322576049316, 1 ),
      ( 250, 7686.65831618225, 10000000.0000004, -0.374127665409377, 149.815745821793, 1 ),
      ( 250, 8185.35764352395, 100000000, 4.87740572301303, 154.175416401713, 1 ),
      ( 300, 0.401276512707276, 999.99997317382, -0.000924417936692011, 146.35876326528, 2 ),
      ( 300, 4.04678221620565, 9999.99982033243, -0.00932260693043093, 146.799952759839, 2 ),
      ( 300, 7145.45554817995, 100000.000000827, -0.99438936318741, 164.523186161717, 1 ),
      ( 300, 7155.88262869679, 1000000.01246618, -0.943975384901884, 164.568432730406, 1 ),
      ( 300, 7252.87441525914, 10000000.0000166, -0.447245957223456, 165.02159069608, 1 ),
      ( 300, 7874.24452342278, 100000000, 4.09135275516001, 169.081255178818, 1 ),
      ( 350, 0.343810203794741, 999.999996584941, -0.000514417380467634, 166.963433809726, 2 ),
      ( 350, 3.45418434533044, 9999.99999999962, -0.00516791373929791, 167.178354420268, 2 ),
      ( 350, 6652.74549537806, 100000.000000109, -0.994834713847254, 182.357371778092, 1 ),
      ( 350, 6668.42244723832, 1000000.00000186, -0.948468570463101, 182.393439068246, 1 ),
      ( 350, 6808.37072966863, 10000000.0003009, -0.49527815758142, 182.779412248904, 1 ),
      ( 350, 7582.50445863026, 100000000, 3.53192403281209, 186.586510724829, 1 ),
      ( 400, 0.300773786548893, 999.999999394891, -0.000314563417979108, 187.779312406627, 2 ),
      ( 400, 3.01630589955798, 9999.99356180369, -0.00315423983063724, 187.894939134546, 2 ),
      ( 400, 31.0765698540355, 100000.013604414, -0.0324570114619877, 189.090946448617, 2 ),
      ( 400, 6121.21047279057, 1000000.00000361, -0.950879131593969, 201.20320555241, 1 ),
      ( 400, 6338.21139545458, 10000000.0029552, -0.525608794906543, 201.420217588468, 1 ),
      ( 500, 0.240577256170422, 999.999999997944, -0.000140981222259069, 226.415402351059, 2 ),
      ( 500, 2.40883301007833, 9999.99969936158, -0.00141131286910696, 226.456751331961, 2 ),
      ( 500, 24.4024728525476, 99999.9999984226, -0.0142665421794807, 226.876347326815, 2 ),
      ( 500, 287.280657025482, 999999.999998356, -0.162688703842519, 231.880232434441, 2 ),
      ( 500, 5240.08031714382, 10000000.0086857, -0.540954861521407, 237.495652889259, 1 ),
      ( 500, 6793.00261987414, 100000000, 2.54104587846106, 239.959057123996, 1 ),
      ( 600, 0.20046736893057, 999.999999996392, -7.27608936147193E-05, 259.427844832421, 2 ),
      ( 600, 2.00598791105474, 9999.99996346068, -0.000727862608017737, 259.445916745833, 2 ),
      ( 600, 20.1927706381249, 99999.999999994, -0.00730421617776035, 259.628053090531, 2 ),
      ( 600, 216.898706115757, 1000000.00292959, -0.0758230591361699, 261.595960118027, 2 ),
      ( 600, 3774.13536678969, 10000000.0000385, -0.468877601717709, 270.194337127798, 1 ),
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
      ( 200, 8608.67946248901, 100000.0000014, -0.993014511248267, 124.165050342159, 1 ),
      ( 200, 8614.74326419901, 1000000.00006761, -0.930194281053769, 124.213092349474, 1 ),
      ( 200, 8673.22506009428, 10000000.000001, -0.306649667211121, 124.688858088664, 1 ),
      ( 250, 0.481826381990439, 999.995095923431, -0.00153519980425687, 117.665474290129, 2 ),
      ( 250, 8098.32408090524, 100000.000000138, -0.994059429252296, 136.224804410079, 1 ),
      ( 250, 8106.7644350958, 1000000.00129329, -0.940656141865845, 136.268872003841, 1 ),
      ( 250, 8186.86913328954, 10000000.0000002, -0.41236793854736, 136.706450012162, 1 ),
      ( 300, 0.40120809850527, 999.999987686317, -0.00075405509336124, 135.014796375594, 2 ),
      ( 300, 4.03972728657681, 9999.99028062046, -0.00759249302855351, 135.356861250619, 2 ),
      ( 300, 7577.49820974924, 100000.000001048, -0.99470926220686, 150.866884015884, 1 ),
      ( 300, 7589.66907355554, 1000000.01808344, -0.947177463403178, 150.906331906059, 1 ),
      ( 300, 7702.20134189783, 10000000.0000302, -0.479492228664588, 151.30532442406, 1 ),
      ( 350, 0.343778979689214, 999.999998386827, -0.000423637999601311, 153.987219045397, 2 ),
      ( 350, 3.45100852644988, 9999.98243797661, -0.00425240387126473, 154.15411674011, 2 ),
      ( 350, 35.9553448344614, 99999.9999996197, -0.0442774407202428, 155.905824756857, 2 ),
      ( 350, 7036.08616286443, 999999.999999164, -0.951161294265996, 167.698136558038, 1 ),
      ( 350, 7201.72738882796, 10000000.0004946, -0.522845946103315, 168.023578950047, 1 ),
      ( 400, 0.300757651374481, 999.999999708058, -0.000260931868562434, 173.228355463072, 2 ),
      ( 400, 3.01467571168224, 9999.99692871432, -0.0026151960651499, 173.318269697408, 2 ),
      ( 400, 30.8949559214572, 99999.9996052161, -0.0267693700019272, 174.242675463714, 2 ),
      ( 400, 6402.21865937659, 1000000.00000017, -0.953035160134961, 185.375307604581, 1 ),
      ( 400, 6667.67592477759, 10000000.0007782, -0.54904950760582, 185.496227768494, 1 ),
      ( 500, 0.240571687795514, 999.999999998987, -0.000117838058893303, 209.074701830971, 2 ),
      ( 500, 2.40827370826735, 9999.99985236458, -0.00117939886435663, 209.106971432631, 2 ),
      ( 500, 24.3439705293717, 99999.9999997135, -0.0118976726099376, 209.433658685997, 2 ),
      ( 500, 277.038294940698, 999999.982737063, -0.131732527955795, 213.190709502854, 2 ),
      ( 500, 5395.32872315199, 10000000.0017626, -0.554163700351519, 219.361560675048, 1 ),
      ( 500, 7211.11621119057, 100000000, 2.33572961868944, 221.418850326448, 1 ),
      ( 600, 0.200464964780206, 999.999999998225, -6.07688957190899E-05, 239.786577338793, 2 ),
      ( 600, 2.00574700602829, 9999.99998205853, -0.000607842646996056, 239.800759827364, 2 ),
      ( 600, 20.1681814888466, 99999.9999999986, -0.00609391647083904, 239.943528068477, 2 ),
      ( 600, 213.839870963638, 1000000.00009721, -0.0626033309830715, 241.466358344476, 2 ),
      ( 600, 3651.79134544185, 10000000, -0.451083690759158, 250.025100411827, 1 ),
      ( 600, 6699.82899659084, 100000000, 1.9919089404698, 249.92989276641, 1 ),
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
