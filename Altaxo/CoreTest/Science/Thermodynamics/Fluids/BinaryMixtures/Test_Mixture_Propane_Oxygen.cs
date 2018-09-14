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
  /// Tests and test data for <see cref="Mixture_Propane_Oxygen"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Propane_Oxygen : MixtureTestBase
  {

    public Test_Mixture_Propane_Oxygen()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("74-98-6", 0.5), ("7782-44-7", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 1.20301751976895, 999.999999938854, -0.000229631668574479, 20.8117045562767, 2 ),
      ( 100, 34128.1650619741, 1000000.00418992, -0.964758103118509, 28.7102173605661, 1 ),
      ( 100, 34852.5919246705, 10000000.000046, -0.65490622004427, 29.2621330303844, 1 ),
      ( 150, 0.80188549310484, 999.997322518035, -7.23050664915952E-05, 20.8183646156819, 2 ),
      ( 150, 8.02407977092295, 9999.99999999754, -0.000723403235153346, 20.8252867131977, 2 ),
      ( 150, 80.7699062528808, 99999.9999884114, -0.00726947717497128, 20.8953366194277, 2 ),
      ( 150, 25484.8650578657, 9999999.99999992, -0.685371097416029, 25.9564170431044, 1 ),
      ( 200, 0.60138865276409, 999.993535954646, -2.99610298026314E-05, 20.8394561023786, 2 ),
      ( 200, 6.01550885129548, 9999.99999999372, -0.000299643321506386, 20.8418834194274, 2 ),
      ( 200, 60.3179895818626, 99999.9999369063, -0.00299953861608984, 20.866226753498, 2 ),
      ( 200, 620.16273169133, 999999.999999994, -0.0303018809527787, 21.116808289149, 2 ),
      ( 200, 8696.04057449559, 9999999.99991276, -0.308454659020461, 24.0659396826132, 2 ),
      ( 250, 0.481103122632605, 999.996598977788, -1.37497978719097E-05, 20.9217222757149, 2 ),
      ( 250, 4.81162663302162, 9999.99999999921, -0.000137491471786548, 20.9228878439433, 2 ),
      ( 250, 48.1758544604328, 99999.9999920355, -0.00137420928393807, 20.9345473565674, 2 ),
      ( 250, 487.760956754139, 1000000, -0.0136633511199279, 21.0514734889503, 2 ),
      ( 250, 5458.15829959787, 9999999.99241694, -0.118573553261013, 22.1781304215941, 2 ),
      ( 300, 0.400916256471634, 999.998448707701, -6.23617742990123E-06, 21.1150611868807, 2 ),
      ( 300, 4.00938755499247, 9999.99999999994, -6.23517084787877E-05, 21.1157647575768, 2 ),
      ( 300, 40.1163479412154, 99999.9999992575, -0.000622497166623668, 21.1227971191431, 2 ),
      ( 300, 403.382174131745, 999999.993611627, -0.00611930317107291, 21.1927652447806, 2 ),
      ( 300, 4206.92848966128, 9999999.99999999, -0.0470155191177825, 21.8362564778563, 2 ),
      ( 350, 0.343641184555103, 999.99949175956, -2.34520699198206E-06, 21.433789876717, 2 ),
      ( 350, 3.43648418947303, 10000, -2.34437835444943E-05, 21.434272555581, 2 ),
      ( 350, 34.3720657918253, 99999.9999999626, -0.000233606529746864, 21.4390957771325, 2 ),
      ( 350, 344.415766859838, 999999.999739107, -0.00225136130660734, 21.486963988562, 2 ),
      ( 350, 3480.47603169323, 9999999.99999999, -0.012662752454082, 21.9231675533148, 1 ),
      ( 400, 0.300685374140198, 999.99995473279, -1.85618057120832E-07, 21.8558166736037, 2 ),
      ( 400, 3.00685873484025, 9999.99559965869, -1.85000924780892E-06, 21.8561725093979, 2 ),
      ( 400, 30.069069426159, 100000, -1.78823274481377E-05, 21.8597279168452, 2 ),
      ( 400, 300.720304010448, 1000000, -0.000116343319180373, 21.8949843267523, 1 ),
      ( 400, 2989.94696997151, 10000000, 0.00565434849501679, 22.2160623402572, 1 ),
      ( 500, 0.24054780994021, 1000.00042212085, 1.80889932644969E-06, 22.859638722775, 1 ),
      ( 500, 2.40543901775472, 9999.99999999999, 1.80923074881338E-05, 22.8598564293812, 1 ),
      ( 500, 24.0504661008735, 100000.000000016, 0.000181255368563103, 22.8620315966208, 1 ),
      ( 500, 240.10503159649, 1000000.00018397, 0.00184595120732313, 22.8835934247071, 1 ),
      ( 500, 2353.89188707304, 10000000, 0.0219171708383718, 23.0804510735794, 1 ),
      ( 600, 0.200456364506356, 1000.00053380032, 2.51208712801896E-06, 23.8751831269801, 1 ),
      ( 600, 2.00451842243129, 10000, 2.5122733908503E-05, 23.8753291402847, 1 ),
      ( 600, 20.04064929688, 100000.000000035, 0.000251414871223852, 23.8767879944153, 1 ),
      ( 600, 199.950412891059, 1000000.00034995, 0.00253295425868093, 23.8912490055176, 1 ),
      ( 600, 1951.41016393005, 10000000, 0.0272411297567103, 24.0235182925819, 1 ),
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
      ( 200, 0.601455578949475, 999.999999093845, -0.000149152620238913, 34.2602962853001, 2 ),
      ( 200, 6.02265235866315, 9999.9999981816, -0.00149330484038006, 34.2821037486359, 2 ),
      ( 250, 0.481129174269806, 999.999999921877, -7.58175822316439E-05, 38.4334822928657, 2 ),
      ( 250, 4.81457897578961, 9999.99920230486, -0.000758532225849843, 38.4430171613947, 2 ),
      ( 250, 48.4787449525984, 99999.9983789957, -0.00762138790189885, 38.5394831685565, 2 ),
      ( 300, 0.400927726963814, 999.999999990016, -4.27677611287701E-05, 43.2274416605273, 2 ),
      ( 300, 4.010821433426, 9999.99989912129, -0.000427750676896251, 43.2317897600385, 2 ),
      ( 300, 40.2635810194529, 99999.9999999747, -0.00428483995191639, 43.2754990554536, 2 ),
      ( 300, 419.190601263884, 1000000.00080296, -0.0436078982715314, 43.7365294381466, 2 ),
      ( 350, 0.343646373886472, 999.999999999911, -2.54148760407007E-05, 48.3632173909137, 2 ),
      ( 350, 3.43724998846568, 9999.99998714765, -0.000254152855539008, 48.365529168023, 2 ),
      ( 350, 34.4513368019588, 99999.9999999997, -0.00254192708693648, 48.3886855590476, 2 ),
      ( 350, 352.610540790812, 1000000.00000083, -0.0254470572950479, 48.6241227508057, 2 ),
      ( 400, 0.300687554271234, 999.999999999727, -1.53619072382831E-05, 53.545147564953, 2 ),
      ( 400, 3.00729128942078, 9999.99999726588, -0.000153606021837133, 53.5465971936459, 2 ),
      ( 400, 30.1145116253688, 100000, -0.00153474551506406, 53.5610915802194, 2 ),
      ( 400, 305.325708964101, 999999.999623815, -0.0152059708342541, 53.7057948839504, 2 ),
      ( 400, 3434.97034118206, 10000000, -0.124641830143297, 55.0393842699543, 2 ),
      ( 500, 0.240547580704762, 999.999999999994, -5.12412232167817E-06, 63.3173746977755, 2 ),
      ( 500, 2.405586711347, 9999.9999999326, -5.12266928785363E-05, 63.3181563402954, 2 ),
      ( 500, 24.0669284585587, 99999.9993561229, -0.000510810822698669, 63.3259639067441, 2 ),
      ( 500, 241.745265602347, 999999.99999997, -0.00495942489330413, 63.4031398061108, 2 ),
      ( 500, 2484.5443829901, 10000000.0000433, -0.0318291363343197, 64.0738613142784, 1 ),
      ( 600, 0.200455410838633, 999.99500836824, -5.90555527996754E-07, 71.8920224454347, 2 ),
      ( 600, 2.00456471881036, 9999.99999999991, -5.8954931552293E-06, 71.8925365507852, 2 ),
      ( 600, 20.0466906154757, 99999.9999994528, -5.79450429079439E-05, 71.8976717332553, 2 ),
      ( 600, 200.551101562353, 999999.999831654, -0.000477740936425436, 71.9484344006385, 2 ),
      ( 600, 1992.72894874429, 10000000.0000987, 0.00593354764897047, 72.3962387589992, 1 ),
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
      ( 150, 15156.5941048813, 1000000.00086077, -0.947097955957168, 58.9844986555909, 1 ),
      ( 150, 15258.927932456, 9999999.99999996, -0.474527429574004, 59.3732771774282, 1 ),
      ( 200, 0.6017006620559, 999.999999644524, -0.000564326879268035, 47.6899715055297, 2 ),
      ( 200, 6.04793597945826, 9999.99621935746, -0.00567547412870341, 47.8097497003034, 2 ),
      ( 200, 13981.1588088903, 1000000.00123723, -0.956987749370113, 60.9714789484316, 1 ),
      ( 200, 14139.3918134405, 10000000.000003, -0.574690966818291, 61.3879062775741, 1 ),
      ( 250, 0.481220949278628, 999.99997408232, -0.000274435924476867, 55.950315631727, 2 ),
      ( 250, 4.82416088014784, 9999.99959056366, -0.00275115824726252, 56.0116385266169, 2 ),
      ( 250, 49.5064965715224, 100000.003642875, -0.0282308025174867, 56.6539622199535, 2 ),
      ( 250, 12691.5530472282, 999999.999999886, -0.962093773459492, 66.0108204039952, 1 ),
      ( 250, 12956.028109571, 10000000.0000227, -0.628675639718522, 66.3425301602942, 1 ),
      ( 300, 0.400969227680323, 999.999996772828, -0.000154185267257918, 65.3425566674416, 2 ),
      ( 300, 4.01527247530616, 9999.99999999991, -0.00154371440927161, 65.3720737842643, 2 ),
      ( 300, 40.7272561191066, 100000.000158205, -0.0156287402472468, 65.6745247931185, 2 ),
      ( 300, 11628.3922007466, 10000000.0096082, -0.655234018959701, 73.7076091272661, 1 ),
      ( 350, 0.34366755248375, 999.999999968025, -9.49599564415506E-05, 75.2943577751169, 2 ),
      ( 350, 3.43961745371939, 9999.99527593346, -0.000950185286126591, 75.3100964291608, 2 ),
      ( 350, 34.695227671156, 99999.9996195958, -0.00956142683426676, 75.4697758042736, 2 ),
      ( 350, 382.986903582079, 1000000, -0.102750212568843, 77.3422977843915, 2 ),
      ( 350, 9989.42289652052, 10000000.0015666, -0.656001231066338, 82.6926503727527, 1 ),
      ( 400, 0.300699218299149, 999.999999877893, -6.20726578847035E-05, 85.2356507689105, 2 ),
      ( 400, 3.00867367000011, 9999.99875457744, -0.000620917712484431, 85.2442339626506, 2 ),
      ( 400, 30.256507192228, 99999.9999929893, -0.00622847441038232, 85.3308414248191, 2 ),
      ( 400, 321.379569373075, 1000000.01582948, -0.0644067594425597, 86.281263973803, 2 ),
      ( 400, 7573.61593999656, 10000000.0008818, -0.602989436623605, 93.0226116599697, 1 ),
      ( 500, 0.240551457627565, 999.999999991648, -2.91627749002075E-05, 103.775939069407, 2 ),
      ( 500, 2.4061461332508, 9999.99991600078, -0.000291631684230867, 103.778971876283, 2 ),
      ( 500, 24.1248091347738, 99999.9999999891, -0.0029167023169452, 103.809368204869, 2 ),
      ( 500, 247.778538225316, 1000000.00002192, -0.0291958125095649, 104.120305456461, 2 ),
      ( 500, 3246.21470522083, 9999999.99999997, -0.259000206940438, 107.41175305866, 1 ),
      ( 600, 0.200456539643745, 999.999999999318, -1.41555743434502E-05, 119.909693780407, 2 ),
      ( 600, 2.00482077416712, 9999.99999318087, -0.000141535596412665, 119.911216988266, 2 ),
      ( 600, 20.0737410912155, 99999.9999999999, -0.00141333319214661, 119.926436272254, 2 ),
      ( 600, 203.283953175569, 999999.999367442, -0.0139226489021975, 120.07732148634, 2 ),
      ( 600, 2244.67789258283, 10000000, -0.106982330381523, 121.398952960589, 2 ),
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
