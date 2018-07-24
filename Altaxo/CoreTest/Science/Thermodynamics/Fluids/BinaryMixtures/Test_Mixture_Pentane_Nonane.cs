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
	/// Tests and test data for <see cref="Mixture_Pentane_Nonane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Pentane_Nonane : MixtureTestBase
    {

    public Test_Mixture_Pentane_Nonane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("109-66-0", 0.5), ("111-84-2", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 5870.0883696133, 1000000.00211453, -0.918044014819954, 207.693145751721, 1 ),
      ( 250, 5913.20870091638, 9999999.99999949, -0.186416547396844, 208.386928877058, 1 ),
      ( 250, 6236.62943653688, 100000000.000003, 6.7139243243183, 214.411906481089, 1 ),
      ( 300, 5564.40871456589, 1000000.00147606, -0.927951482988752, 229.296483696081, 1 ),
      ( 300, 5621.41301665888, 9999999.99999918, -0.286820956360895, 229.948247972866, 1 ),
      ( 300, 6016.50492784171, 100000000, 5.6634599446645, 235.519076477472, 1 ),
      ( 350, 0.344026807209187, 999.999997035245, -0.00113914496727931, 231.867606678544, 2 ),
      ( 350, 5254.89932610884, 1000000.00012307, -0.934606756649357, 253.836977534193, 1 ),
      ( 350, 5331.64508500307, 10000000.0000145, -0.355480521838862, 254.447400922571, 1 ),
      ( 350, 5812.11742990762, 100000000.000033, 4.91238760995648, 259.687023819377, 1 ),
      ( 400, 0.300884169832282, 999.999258752727, -0.000676747898356959, 260.373930330032, 2 ),
      ( 400, 3.02741814492178, 9999.99359202828, -0.00680866035833116, 260.679288063948, 2 ),
      ( 400, 4917.80531126182, 100000.000004472, -0.993885879412988, 279.090852985732, 1 ),
      ( 400, 4929.80296157296, 1000000.00000007, -0.93900759342185, 279.142880788693, 1 ),
      ( 400, 5036.493846854, 10000000.0001533, -0.402996298427152, 279.683542779374, 1 ),
      ( 400, 5619.88823524119, 100000000.000004, 4.35029406930459, 284.631667377741, 1 ),
      ( 500, 0.240614557490009, 999.99996196898, -0.000291420131665989, 313.034804212725, 2 ),
      ( 500, 2.41249186662109, 9999.99999996868, -0.00292124963953692, 313.143187857435, 2 ),
      ( 500, 24.7972691616053, 100000.003044423, -0.0299559368373406, 314.261266162514, 2 ),
      ( 500, 4148.33059270921, 1000000.0000001, -0.942014159147151, 327.958964918635, 1 ),
      ( 500, 4401.00999081735, 10000000.0000014, -0.453433557154468, 327.884641356055, 1 ),
      ( 500, 5264.73759130867, 100000000.00203, 3.56897297147044, 332.09250020766, 1 ),
      ( 600, 0.200483351612657, 999.999996462993, -0.000147910759422632, 357.739377928112, 2 ),
      ( 600, 2.00750922703977, 9999.99999999987, -0.00148056473205558, 357.786236544902, 2 ),
      ( 600, 20.3496905871912, 100000.000033944, -0.0149545652834446, 358.262153089835, 2 ),
      ( 600, 241.147868831247, 999999.999995323, -0.16875194070931, 363.970591420109, 2 ),
      ( 600, 3647.59539894129, 10000000.0003954, -0.450449745515261, 370.482768721289, 1 ),
      ( 600, 4943.43411875027, 100000000.017361, 3.05494830478306, 373.23425785173, 1 ),
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
      ( 250, 7170.03079487979, 1000000.00008033, -0.932902969184257, 161.996611674535, 1 ),
      ( 250, 7227.22565535677, 9999999.99999873, -0.33433961957528, 162.536539583527, 1 ),
      ( 250, 7649.48167227374, 100000000.000009, 5.2891552469065, 167.262652884545, 1 ),
      ( 300, 0.401384062447002, 999.999999992933, -0.001189835033491, 157.78105909433, 2 ),
      ( 350, 0.343861302114821, 999.999999997718, -0.000660658829989177, 180.063879322407, 2 ),
      ( 350, 3.45933160440091, 9999.99999413759, -0.00664588877753646, 180.347349564256, 2 ),
      ( 350, 6364.84197487558, 999999.999999732, -0.946010579973389, 197.006716037786, 1 ),
      ( 350, 6471.83928092905, 10000000.0000728, -0.469031735512184, 197.463337294122, 1 ),
      ( 350, 7112.28289879295, 100000000.000159, 3.83155875546085, 201.613976621436, 1 ),
      ( 400, 0.300801412685184, 999.999965996518, -0.000404092053129401, 202.359135440485, 2 ),
      ( 400, 3.01904121530765, 9999.99983162897, -0.00405512942047849, 202.511200772353, 2 ),
      ( 400, 5919.27620282506, 1000000.00051282, -0.949203272313813, 216.811044401349, 1 ),
      ( 400, 6075.47962259904, 10000000.0007223, -0.505092799389342, 217.166336350913, 1 ),
      ( 400, 6865.55969726312, 99999999.9987102, 3.37953895218306, 221.060930173745, 1 ),
      ( 500, 0.240587791585994, 999.999997927222, -0.000182480620177482, 243.745983924905, 2 ),
      ( 500, 2.40984253298707, 9999.99999999994, -0.00182735724802308, 243.800037410838, 2 ),
      ( 500, 24.5087006208698, 100000.000116686, -0.0185367527551795, 244.35108317929, 2 ),
      ( 500, 5179.24277858118, 10000000.0000247, -0.535561665555024, 255.298436761356, 1 ),
      ( 500, 6405.41989237987, 99999999.9999463, 2.75531804343597, 258.364744516026, 1 ),
      ( 600, 0.200472440568525, 999.999999784198, -9.5772539742326E-05, 279.185834273163, 2 ),
      ( 600, 2.00645503540437, 9999.99778718842, -0.000958220746046826, 279.209275874805, 2 ),
      ( 600, 20.2402863492815, 99999.9999614056, -0.00963238665719933, 279.446077209703, 2 ),
      ( 600, 223.230700011877, 999999.999999999, -0.102035513918673, 282.083290625284, 2 ),
      ( 600, 4017.47588424247, 10000000.0000208, -0.501046810012542, 289.833952969346, 1 ),
      ( 600, 5985.85489640664, 100000000.006314, 2.34878215881199, 291.045868854381, 1 ),
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
      ( 250, 0.481520651169764, 999.999932256382, -0.000901249719806841, 97.8869699179657, 2 ),
      ( 250, 9231.09471575659, 100000.000000183, -0.994788411457985, 114.542893044625, 1 ),
      ( 250, 9241.81751349227, 1000000.0031505, -0.947944581132831, 114.58478473545, 1 ),
      ( 250, 9342.99863070534, 10000000.0000028, -0.485083216011609, 115.001588424781, 1 ),
      ( 250, 10034.1788157866, 100000000.004563, 3.79447984375263, 118.697538836066, 1 ),
      ( 300, 0.40109250140323, 999.999997927542, -0.000466062263892765, 112.398557768803, 2 ),
      ( 300, 4.02790539913742, 9999.99999999989, -0.00467978364674873, 112.567161499558, 2 ),
      ( 300, 8574.89971319585, 100000.000002214, -0.995324661736679, 125.624014977423, 1 ),
      ( 300, 8591.28017461014, 1000000.00000017, -0.953335759144666, 125.659342015078, 1 ),
      ( 300, 8740.47537804405, 10000000.0000948, -0.541322925835366, 126.025243166267, 1 ),
      ( 300, 9624.41517272508, 100000000.007137, 3.16550574923551, 129.479691300414, 1 ),
      ( 350, 0.34372746681526, 999.999999690494, -0.00027383146267864, 128.269041357543, 2 ),
      ( 350, 3.44579070576001, 9999.99674071301, -0.0027445858616052, 128.351111176747, 2 ),
      ( 350, 35.3573626768042, 100000.004401201, -0.0281137591861838, 129.200403577537, 2 ),
      ( 350, 7858.03911208083, 1000000.00310124, -0.95626983531382, 139.259530171839, 1 ),
      ( 350, 8096.77348944683, 10000000.002267, -0.575592248022348, 139.493121549914, 1 ),
      ( 350, 9235.31644134658, 100000000.010437, 2.72086160382709, 142.691441092594, 1 ),
      ( 400, 0.300731821393595, 999.999999937579, -0.000175059295846458, 144.34900933003, 2 ),
      ( 400, 3.01207180337989, 9999.99935546215, -0.00175296223113699, 144.393119596831, 2 ),
      ( 400, 30.6120267712945, 99999.9999928799, -0.0177743612355434, 144.842693522679, 2 ),
      ( 400, 383.47234714842, 999999.999999486, -0.215903890512953, 151.407656913076, 2 ),
      ( 400, 7376.71584061732, 10000000.0000013, -0.592394255128098, 153.960642583623, 1 ),
      ( 400, 8863.59741060123, 100000000.014274, 2.39229278590182, 156.775257523392, 1 ),
      ( 500, 0.240563420267836, 999.999999995536, -8.34701870024704E-05, 174.458619458182, 2 ),
      ( 500, 2.40744381553505, 9999.99995469037, -0.000835081413356807, 174.47442318982, 2 ),
      ( 500, 24.2578375281922, 99999.9999999888, -0.00838918496477588, 174.633854878529, 2 ),
      ( 500, 263.809743882206, 999999.999999998, -0.0881938746676271, 176.388325036022, 2 ),
      ( 500, 5460.05339468933, 10000000, -0.559448739772172, 183.134212194401, 1 ),
      ( 500, 8168.86606496479, 100000000, 1.944635626061, 184.092246816726, 1 ),
      ( 600, 0.200461711091782, 999.999999999508, -4.4534339915103E-05, 200.632702527241, 2 ),
      ( 600, 2.00542096367466, 9999.9999950372, -0.00044535639766172, 200.639687268023, 2 ),
      ( 600, 20.1349768550962, 100000, -0.00445485928334226, 200.709882703966, 2 ),
      ( 600, 209.825059967653, 999999.987433817, -0.04466709643369, 201.445750683017, 2 ),
      ( 600, 3080.81404952484, 10000000.000205, -0.349351241461566, 207.712272604747, 1 ),
      ( 600, 7541.18149165941, 100000000, 1.65810846594061, 208.421726789713, 1 ),
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
