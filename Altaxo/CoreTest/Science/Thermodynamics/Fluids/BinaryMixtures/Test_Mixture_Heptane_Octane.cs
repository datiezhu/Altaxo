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
  /// Tests and test data for <see cref="Mixture_Heptane_Octane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Heptane_Octane : MixtureTestBase
  {

    public Test_Mixture_Heptane_Octane()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("142-82-5", 0.5), ("111-65-9", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 6450.42291989984, 100000.000001011, -0.992541780832536, 189.035970527643, 1 ),
      ( 250, 6455.87019928361, 1000000.0000048, -0.925480738654674, 189.096428798778, 1 ),
      ( 250, 6508.00610059042, 9999999.99999911, -0.260777154997603, 189.693895858555, 1 ),
      ( 250, 6889.35678643263, 100000000, 5.98304200962357, 194.98246949129, 1 ),
      ( 300, 0.401541109984532, 999.999991923935, -0.00158276318896492, 180.454960602294, 2 ),
      ( 300, 6100.39990379756, 99999.9999997129, -0.993428208448415, 206.813623527006, 1 ),
      ( 300, 6107.8423717696, 1000000.00488209, -0.934362162111632, 206.869063520344, 1 ),
      ( 300, 6177.78116342697, 10000000.0000038, -0.351052497882583, 207.419834269058, 1 ),
      ( 350, 0.343928265430772, 999.999975779628, -0.000857514788722523, 205.91533215047, 2 ),
      ( 350, 3.46629102347038, 9999.98547199723, -0.00864254051520514, 206.325861735235, 2 ),
      ( 350, 5739.59126859997, 100000.000001011, -0.994012930089063, 227.997062396109, 1 ),
      ( 350, 5750.13716245362, 1000000.00000031, -0.94023910523499, 228.04555793227, 1 ),
      ( 350, 5846.36917225182, 10000000.0000831, -0.412227774677463, 228.540451365198, 1 ),
      ( 400, 0.300833229251052, 999.999996005498, -0.000512094798547505, 231.426319681574, 2 ),
      ( 400, 3.02234026766001, 9999.99999999949, -0.00514453205603916, 231.647032976388, 2 ),
      ( 400, 31.7867746475228, 99999.9912545848, -0.0540745984899831, 233.987845207825, 2 ),
      ( 400, 5365.7554013982, 1000000.00000019, -0.943963309613366, 250.345191111962, 1 ),
      ( 400, 5504.10443872055, 10000000.000925, -0.453718261546505, 250.73428114801, 1 ),
      ( 500, 0.240596590018365, 999.999999786171, -0.000221327839891023, 278.455710121811, 2 ),
      ( 500, 2.4107789703539, 9999.9977664411, -0.00221736470656598, 278.53458463339, 2 ),
      ( 500, 24.6104904710123, 99999.9998548013, -0.0225983525749898, 279.341820900444, 2 ),
      ( 500, 4355.69567338576, 1000000.0024052, -0.944774989375583, 294.121696774833, 1 ),
      ( 500, 4744.53266674299, 10000000.0000019, -0.493009414856213, 293.390854316084, 1 ),
      ( 500, 5809.41188706084, 100000000, 3.1405798723027, 296.819152343153, 1 ),
      ( 600, 0.200475187390365, 999.99999998002, -0.000111757694257413, 318.431577891669, 2 ),
      ( 600, 2.00677221901277, 9999.99979603218, -0.00111840870053494, 318.465953662048, 2 ),
      ( 600, 20.2737339467406, 99999.9999996573, -0.0112685540214752, 318.813773686089, 2 ),
      ( 600, 228.479490988606, 999999.999715753, -0.122666188167958, 322.761578049315, 2 ),
      ( 600, 3797.12500029478, 9999999.99996022, -0.47209327390346, 331.516956929242, 1 ),
      ( 600, 5449.38307100711, 100000000, 2.678449103935, 333.270586510824, 1 ),
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
      ( 250, 6774.90140631337, 99999.9999999013, -0.992898986276901, 175.843716084208, 1 ),
      ( 250, 6780.81666109052, 1000000.00004938, -0.929051808556627, 175.902024638533, 1 ),
      ( 250, 6837.37103510037, 9999999.99999871, -0.296386467759381, 176.477775652795, 1 ),
      ( 300, 0.401443629188551, 999.999995112954, -0.00134032191637829, 169.101650558636, 2 ),
      ( 300, 6395.245218018, 100000.000000529, -0.993731193225182, 192.601005832987, 1 ),
      ( 300, 6403.41289266712, 1000000.00641696, -0.937391891805728, 192.65458201915, 1 ),
      ( 300, 6479.9843410373, 10000000.0000063, -0.381317076756475, 193.186827674572, 1 ),
      ( 350, 0.343885174771198, 999.999986683726, -0.000732316879521638, 192.95350647498, 2 ),
      ( 350, 3.46185399298297, 9999.99115021043, -0.00737193177418073, 193.290546425316, 2 ),
      ( 350, 6000.41738206317, 100000.000002569, -0.994273176014635, 212.58919630856, 1 ),
      ( 350, 6012.18421017101, 1000000.00000035, -0.942843843462328, 212.635452729623, 1 ),
      ( 350, 6118.98090382206, 10000000.000121, -0.438414096650593, 213.110239518823, 1 ),
      ( 350, 6743.11934597798, 100000000, 4.09605902281228, 217.495253309738, 1 ),
      ( 400, 0.300811777139265, 999.999997749413, -0.000440817250320333, 216.907433718009, 2 ),
      ( 400, 3.02015727568593, 9999.99999999986, -0.00442544318978976, 217.088611172664, 2 ),
      ( 400, 31.5225150512529, 99999.9999994346, -0.0461447202746768, 218.991720047118, 2 ),
      ( 400, 5586.57215276397, 1000000.00952902, -0.946178234389291, 233.732671244985, 1 ),
      ( 400, 5743.42370725721, 10000000.0017908, -0.47648094674456, 234.087314844547, 1 ),
      ( 400, 6512.59018521562, 100000000, 3.61689075417454, 238.17915254295, 1 ),
      ( 500, 0.240589804350279, 999.999999874449, -0.000193129777877805, 261.140633134271, 2 ),
      ( 500, 2.41009525574923, 9999.99869688869, -0.00193430619725991, 261.205318835783, 2 ),
      ( 500, 24.5365708770153, 99999.9999635701, -0.0196538036946414, 261.865327351493, 2 ),
      ( 500, 318.764400768807, 999999.999729302, -0.245388322071762, 270.65439075503, 2 ),
      ( 500, 4896.8651687445, 10000000.0013571, -0.508780962848763, 274.52771520973, 1 ),
      ( 600, 0.200472535829155, 999.999999987889, -9.85326164882949E-05, 298.789706274884, 2 ),
      ( 600, 2.0065060654689, 9999.99987675883, -0.000985911790404434, 298.817878362566, 2 ),
      ( 600, 20.2460876823142, 99999.9999998973, -0.00991843001278427, 299.102543876561, 2 ),
      ( 600, 224.207922920832, 999999.999999208, -0.105951385953174, 302.274832314052, 2 ),
      ( 600, 3815.99900444582, 9999999.99418373, -0.474704310911496, 310.896867284745, 1 ),
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
      ( 250, 7181.51060562555, 99999.9999995178, -0.99330103796364, 162.817721150732, 1 ),
      ( 250, 7187.92861165499, 1000000.00011309, -0.933070192572955, 162.873469000351, 1 ),
      ( 250, 7249.24964269382, 10000000.0000001, -0.336363482755159, 163.423533931818, 1 ),
      ( 300, 0.401358702279605, 999.999942935494, -0.00112900694490077, 157.750734263923, 2 ),
      ( 300, 6765.81574481064, 100000.000001122, -0.994074542184681, 178.543137725905, 1 ),
      ( 300, 6774.77518832626, 1000000.00779466, -0.940823782812221, 178.594445074097, 1 ),
      ( 300, 6858.61176244673, 10000000.0000078, -0.415471265384767, 179.104218605488, 1 ),
      ( 350, 0.343847364514119, 999.999992942393, -0.00062243508302127, 179.992857105822, 2 ),
      ( 350, 3.45797781479999, 9999.99999999793, -0.00625926402338045, 180.267307549654, 2 ),
      ( 350, 6329.5284639592, 100000.000003786, -0.994570948786781, 197.323622966721, 1 ),
      ( 350, 6342.66718556048, 1000000.00000001, -0.945821949694604, 197.367193945724, 1 ),
      ( 350, 6461.32383195788, 10000000.0001637, -0.468168829200796, 197.817600993763, 1 ),
      ( 400, 0.300792823605901, 999.999998776645, -0.000377833107509454, 202.389184981723, 2 ),
      ( 400, 3.01823337689253, 9999.98680313926, -0.00379083360708271, 202.53665409418, 2 ),
      ( 400, 31.2964999235658, 99999.9999999997, -0.0392562272048446, 204.073363431835, 2 ),
      ( 400, 5866.18106473406, 1000000.0029016, -0.948743625253044, 217.254450983885, 1 ),
      ( 400, 6044.69817512369, 10000000.0023695, -0.502573717540844, 217.567711139869, 1 ),
      ( 500, 0.240583735017722, 999.99999992879, -0.000167907123548701, 243.825786015447, 2 ),
      ( 500, 2.40948435156675, 9999.99926498253, -0.00168125529250258, 243.878364869429, 2 ),
      ( 500, 24.4712490530852, 99999.9999912577, -0.0170369368013801, 244.413405277203, 2 ),
      ( 500, 301.48908889615, 999999.999917082, -0.20214910535421, 251.115659901122, 2 ),
      ( 500, 5088.06739960383, 10000000, -0.527240265501348, 255.79482700616, 1 ),
      ( 500, 6424.05145121287, 100000000, 2.7444180066368, 258.688939427432, 1 ),
      ( 600, 0.200470142551323, 999.999999992902, -8.65954672165384E-05, 279.147935089137, 2 ),
      ( 600, 2.00626596694463, 9999.99992794966, -0.000866355460250419, 279.170810125981, 2 ),
      ( 600, 20.2212853578178, 99999.9999999711, -0.00870405008538282, 279.40167058263, 2 ),
      ( 600, 220.64808395351, 999999.999999997, -0.0915271995386962, 281.935444424193, 2 ),
      ( 600, 3833.70862281051, 9999999.9999522, -0.47713088691106, 290.427313084591, 1 ),
      ( 600, 5999.83783633586, 100000000, 2.34097000975085, 291.169697939304, 1 ),
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
