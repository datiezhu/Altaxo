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
	/// Tests and test data for <see cref="Mixture_Methane_CO2"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Methane_CO2 : MixtureTestBase
    {

    public Test_Mixture_Methane_CO2()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-82-8", 0.5), ("124-38-9", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481129231765225, 999.999999867188, -8.84443779577514E-05, 26.5255471039055, 2 ),
      ( 250, 4.81512834175502, 9999.99863502041, -0.000885034618763747, 26.5470893112606, 2 ),
      ( 250, 48.5411941144285, 99999.9999853663, -0.00891049872657466, 26.7649896740623, 2 ),
      ( 250, 532.440549644618, 999999.997245371, -0.0964499622227429, 29.3564132994449, 2 ),
      ( 250, 24445.2552743521, 10000000.0025555, -0.803198341228481, 41.457773161236, 1 ),
      ( 250, 28067.8042476303, 100000000.000286, 0.714016081722764, 43.540731834243, 1 ),
      ( 300, 0.400925040470228, 999.999999995173, -4.85751123765171E-05, 28.9106015067995, 2 ),
      ( 300, 4.01100458437862, 9999.99985647426, -0.000485895589552328, 28.9200183515516, 2 ),
      ( 300, 40.2868951486795, 99999.9999999243, -0.00487351030350072, 29.0146916938661, 2 ),
      ( 300, 422.142802357071, 999999.999999997, -0.0503081818454772, 30.018712348683, 2 ),
      ( 300, 18186.0838373241, 9999999.99988769, -0.779553658130029, 41.798866307007, 1 ),
      ( 300, 25656.9221340266, 100000000.000001, 0.562562973841004, 41.5842132588566, 1 ),
      ( 350, 0.343643299414532, 999.999999997491, -2.89764556326382E-05, 31.0786826870392, 2 ),
      ( 350, 3.43732955233799, 9999.99997470652, -0.00028979875893636, 31.0835506014801, 2 ),
      ( 350, 34.4633267596628, 99999.999999999, -0.00290141968069565, 31.1323643683627, 2 ),
      ( 350, 354.030983274291, 1000000.00002051, -0.0293692979315853, 31.6342722654712, 2 ),
      ( 350, 5194.6724400005, 9999999.99982843, -0.338488911818477, 38.3079806866182, 2 ),
      ( 350, 23346.4469547649, 99999999.9999989, 0.471887103487575, 40.7686648730782, 1 ),
      ( 400, 0.300684617775462, 999.999999999522, -1.81042083331778E-05, 33.0189782381367, 2 ),
      ( 400, 3.00733620360077, 9999.99999518679, -0.000181044744801603, 33.0217979476582, 2 ),
      ( 400, 30.1224605638761, 99999.9999999999, -0.00181071369899876, 33.0500361365409, 2 ),
      ( 400, 306.232281325466, 999999.997100247, -0.018133644065228, 33.3364505889579, 2 ),
      ( 400, 3669.19916339397, 9999999.99685476, -0.180531879713556, 36.4635264304116, 2 ),
      ( 400, 21190.4614520638, 100000000.004331, 0.418936415369142, 40.6101587388991, 1 ),
      ( 500, 0.240545114587188, 999.999999999979, -7.38028869029631E-06, 36.3196234079833, 2 ),
      ( 500, 2.40561091367627, 9999.99999978416, -7.37944474980914E-05, 36.3208121529088, 2 ),
      ( 500, 24.0720774687359, 99999.9978729454, -0.00073710043930214, 36.3327033547987, 2 ),
      ( 500, 242.308914328177, 999999.999998846, -0.00728646332018063, 36.4519496263383, 2 ),
      ( 500, 2568.94810148246, 9999999.99988955, -0.0636504522760227, 37.6354769755408, 2 ),
      ( 500, 17523.7303789409, 99999999.9999994, 0.372671994449962, 41.2789899329731, 1 ),
      ( 600, 0.200453329710095, 999.999999999998, -2.67513819418126E-06, 39.021067215069, 2 ),
      ( 600, 2.00458143849645, 9999.99999999267, -2.67442563127835E-05, 39.0216837803646, 2 ),
      ( 600, 20.0506263903229, 99999.9999295179, -0.000266730607232974, 39.0278489319278, 2 ),
      ( 600, 200.974623407574, 999999.999999998, -0.00259655001454134, 39.0894374911631, 2 ),
      ( 600, 2043.5513216251, 9999999.99983543, -0.0190959207900007, 39.6874420433823, 2 ),
      ( 600, 14762.1492633623, 100000000.001375, 0.357883457014471, 42.4626956177897, 1 ),
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
      ( 250, 0.481111917096487, 999.999999964062, -5.24587024165509E-05, 26.2391266365928, 2 ),
      ( 250, 4.81339276796018, 9999.99963412103, -0.00052478201829256, 26.2461180550841, 2 ),
      ( 250, 48.3634176618713, 99999.9999995196, -0.00526740693259049, 26.3174951145724, 2 ),
      ( 250, 508.940886943317, 999999.999999998, -0.0547297516633233, 27.1766947674181, 2 ),
      ( 250, 15242.7842680454, 10000000.000007, -0.684383987774266, 36.7918842480021, 1 ),
      ( 250, 25107.5370725828, 100000000.000001, 0.916104623081302, 35.3351553149269, 1 ),
      ( 300, 0.400917247530944, 999.999999995644, -2.91382814894952E-05, 28.1873868823138, 2 ),
      ( 300, 4.0102242849561, 9999.99995609867, -0.000291412637946389, 28.190826303445, 2 ),
      ( 300, 40.2078471570685, 99999.9999999968, -0.00291710738148546, 28.2254773644538, 2 ),
      ( 300, 413.077907677247, 1000000.00005966, -0.0294674248136261, 28.5966579041077, 2 ),
      ( 300, 5802.44697275931, 9999999.99999996, -0.309075003402013, 33.535825581165, 2 ),
      ( 300, 22927.3637158441, 99999999.985984, 0.748589896404393, 35.3201096406321, 1 ),
      ( 350, 0.343639194769021, 999.999999999349, -1.70321645421663E-05, 30.3641638790023, 2 ),
      ( 350, 3.43691878382313, 9999.99999348234, -0.000170316892085203, 30.3661232060099, 2 ),
      ( 350, 34.4219440623991, 99999.9999999999, -0.00170268934513921, 30.3857719073626, 2 ),
      ( 350, 349.567038592706, 999999.996712216, -0.0169744171173721, 30.5874651868777, 2 ),
      ( 350, 4073.00355234047, 9999999.99985578, -0.156314652234936, 32.7873062456675, 2 ),
      ( 350, 20917.8478291266, 100000000.001175, 0.642775799210745, 36.0235773945403, 1 ),
      ( 400, 0.300682214558864, 999.999999999895, -1.01118064781952E-05, 32.6564657108182, 2 ),
      ( 400, 3.00709577826329, 9999.99999894366, -0.000101106549538665, 32.657697783983, 2 ),
      ( 400, 30.0983140588567, 99999.9896233023, -0.00100991184393507, 32.6700304631663, 2 ),
      ( 400, 303.710704538144, 999999.999968105, -0.00998163836259998, 32.7944352136103, 2 ),
      ( 400, 3285.6323647233, 9999999.99999999, -0.0848666535343787, 34.0388438262442, 2 ),
      ( 400, 19108.2816413897, 100000000.000001, 0.573554230365031, 37.232602338512, 1 ),
      ( 500, 0.240544117674401, 999.999999999999, -3.17246755866341E-06, 37.2550392421658, 2 ),
      ( 500, 2.40550968343365, 9999.99999997871, -3.17148944299716E-05, 37.2556323177296, 2 ),
      ( 500, 24.0619416189428, 99999.9997987592, -0.000316171054186888, 37.2615617572156, 2 ),
      ( 500, 241.282610559666, 999999.999999999, -0.00306392268406246, 37.3207068118193, 2 ),
      ( 500, 2456.1560536846, 10000000.002606, -0.0206512369906249, 37.8824464911102, 1 ),
      ( 500, 16117.1159593314, 100000000.000324, 0.49247135717196, 40.3937216147925, 1 ),
      ( 600, 0.200452812027986, 999.998584419299, -1.43219122849355E-07, 41.5966629754586, 2 ),
      ( 600, 2.0045306855203, 10000, -1.42580189021951E-06, 41.5970019903206, 2 ),
      ( 600, 20.0455512796102, 99999.9999999947, -1.36192335063521E-05, 41.6003906269019, 2 ),
      ( 600, 200.467347364166, 1000000.00035144, -7.26533208125251E-05, 41.6341228714392, 1 ),
      ( 600, 1993.78784320597, 10000000.0000015, 0.00538672371149639, 41.953794346964, 1 ),
      ( 600, 13869.9230472776, 100000000.000062, 0.445233560867281, 43.858360576254, 1 ),
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
      ( 250, 0.481101894039045, 999.999999990532, -3.16262513931111E-05, 25.9538261347697, 2 ),
      ( 250, 4.81238888332623, 9999.99990456013, -0.000316287285295555, 25.9563412820821, 2 ),
      ( 250, 48.261432319719, 99999.9999999843, -0.00316535281094542, 25.9815844903607, 2 ),
      ( 250, 496.940526058636, 1000000.00015519, -0.031902907169316, 26.2432246161949, 2 ),
      ( 250, 7012.14227245825, 9999999.99999305, -0.313923391886554, 29.2057148043124, 2 ),
      ( 250, 23284.7562591065, 99999999.9999999, 1.06610141517564, 31.3605602845536, 1 ),
      ( 300, 0.400912356557429, 999.999999998972, -1.69390291574943E-05, 27.4646118929118, 2 ),
      ( 300, 4.00973482384509, 9999.99998969033, -0.00016938001162229, 27.4660305252691, 2 ),
      ( 300, 40.1585355236612, 99999.9999999999, -0.00169276527649482, 27.4802257540923, 2 ),
      ( 300, 407.76285600219, 999999.995608939, -0.0168168590641432, 27.6230021423227, 2 ),
      ( 300, 4686.95716112433, 9999999.99999999, -0.144635737623937, 28.9998737353691, 2 ),
      ( 300, 21268.0110967362, 100000000.018495, 0.885016721692822, 31.8280251016176, 1 ),
      ( 350, 0.343636487302956, 999.999999999877, -9.15343207023262E-06, 29.6498498922431, 2 ),
      ( 350, 3.43664793921271, 9999.99999878961, -9.15196206020136E-05, 29.6507442137124, 2 ),
      ( 350, 34.3947614423144, 99999.9883132767, -0.000913722100512727, 29.6596834675404, 2 ),
      ( 350, 346.749137209807, 999999.999980514, -0.00898573356261608, 29.7486535958164, 2 ),
      ( 350, 3694.37799321477, 10000000.001571, -0.0698479081502111, 30.5612842982177, 2 ),
      ( 350, 19466.4142659563, 99999999.9999994, 0.765262657797796, 33.2341792632341, 1 ),
      ( 400, 0.300680580918705, 999.999999999988, -4.67871986225222E-06, 32.2940599451436, 2 ),
      ( 400, 3.00693238806342, 9999.99999987486, -4.67742071565599E-05, 32.2946653596242, 2 ),
      ( 400, 30.0819488664143, 99999.9988230772, -0.000466441002923138, 32.3007146296855, 2 ),
      ( 400, 302.048170349216, 999999.999999945, -0.00453237715230535, 32.360712640551, 2 ),
      ( 400, 3100.42823109291, 9999999.99999776, -0.0302011473669591, 32.9031354646214, 1 ),
      ( 400, 17882.3957911969, 100000000.003852, 0.681425563086005, 35.276134633881, 1 ),
      ( 500, 0.240543397020913, 999.997034986859, -2.35279437234738E-07, 38.1904905509568, 2 ),
      ( 500, 2.40543903293412, 9999.99999999999, -2.34468055611705E-06, 38.190807267828, 2 ),
      ( 500, 24.0548783963371, 99999.9999999569, -2.26343633196876E-05, 38.1939718155257, 2 ),
      ( 500, 240.578161848207, 1000000.00088157, -0.000144745280085707, 38.2253565818742, 1 ),
      ( 500, 2388.80575217112, 10000000.0003484, 0.00696065000811799, 38.5143367841873, 1 ),
      ( 500, 15305.6214254018, 100000000.001577, 0.571601260811773, 40.3112241988838, 1 ),
      ( 600, 0.20045244936269, 1000.01734048855, 1.63056765829215E-06, 44.1722723439315, 1 ),
      ( 600, 2.0044951335665, 10000.0000000025, 1.63102866191036E-05, 44.1724562910489, 1 ),
      ( 600, 20.0419995614696, 100000.000026242, 0.00016359211503732, 44.1742946818951, 1 ),
      ( 600, 200.115615384943, 1000000, 0.0016848628221392, 44.192571535202, 1 ),
      ( 600, 1961.94450306174, 10000000.0000958, 0.021704652873195, 44.3658241561029, 1 ),
      ( 600, 13357.2429867915, 100000000.000013, 0.500704770766675, 45.7200770176043, 1 ),
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
