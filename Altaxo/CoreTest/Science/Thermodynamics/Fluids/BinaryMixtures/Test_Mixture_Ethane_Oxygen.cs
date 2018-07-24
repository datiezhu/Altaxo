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
	/// Tests and test data for <see cref="Mixture_Ethane_Oxygen"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Ethane_Oxygen : MixtureTestBase
    {

    public Test_Mixture_Ethane_Oxygen()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-84-0", 0.5), ("7782-44-7", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 1.20301720005803, 999.999999939373, -0.000229365972044026, 20.8061289445093, 2 ),
      ( 100, 12.0551286894096, 9999.9991609266, -0.00229910432953832, 20.8058391422652, 2 ),
      ( 100, 34142.2691674064, 1000000.00410392, -0.964772661481056, 28.7003577057644, 1 ),
      ( 100, 34867.484637267, 10000000.0000446, -0.655053617613639, 29.2522091060243, 1 ),
      ( 150, 0.80188542717987, 999.997818879538, -7.22228960511786E-05, 20.8082290363639, 2 ),
      ( 150, 8.02407316260339, 9999.99999999732, -0.00072258026919392, 20.8151383886928, 2 ),
      ( 150, 80.7692292248066, 99999.999985724, -0.00726115585713515, 20.8850590729248, 2 ),
      ( 150, 868.335942154045, 1000000.00003905, -0.0765929709487016, 21.6731803131014, 2 ),
      ( 150, 25486.8071723649, 9999999.99999991, -0.685395072382744, 25.9447239952195, 1 ),
      ( 200, 0.601388631155821, 999.993519927557, -2.99251000075231E-05, 20.8257569713598, 2 ),
      ( 200, 6.01550668863852, 9999.99999999369, -0.000299283915551165, 20.8281811903298, 2 ),
      ( 200, 60.3177714730915, 99999.999936652, -0.00299593346723762, 20.8524932718723, 2 ),
      ( 200, 620.138940639631, 999999.999999994, -0.0302646793894341, 21.1027339630937, 2 ),
      ( 200, 8690.47165612807, 9999999.99992963, -0.308011511663208, 24.0466350572346, 2 ),
      ( 250, 0.481103113792486, 999.996601384553, -1.37314235329135E-05, 20.9044442651526, 2 ),
      ( 250, 4.81162574876333, 9999.99999999919, -0.000137307721688462, 20.9056088576609, 2 ),
      ( 250, 48.1757657883939, 99999.9999920571, -0.00137237121902236, 20.9172585843713, 2 ),
      ( 250, 487.751845526289, 1000000, -0.0136449263040561, 21.0340840444969, 2 ),
      ( 250, 5457.1274061606, 9999999.99248265, -0.118407045010776, 22.15962821581, 2 ),
      ( 300, 0.400916252479337, 999.998451661347, -6.22621959372101E-06, 21.0940610182919, 2 ),
      ( 300, 4.00938715575862, 9999.99999999993, -6.22521399068469E-05, 21.0947640961601, 2 ),
      ( 300, 40.1163080140081, 99999.9999992613, -0.000621502499999549, 21.1017915296563, 2 ),
      ( 300, 403.37818113133, 999999.993645116, -0.00610946484556175, 21.1717104380432, 2 ),
      ( 300, 4206.57491195703, 9999999.99999997, -0.046935417355189, 21.8147614092699, 2 ),
      ( 350, 0.34364118266294, 999.999493410284, -2.33980875403148E-06, 21.4090991674763, 2 ),
      ( 350, 3.43648400399493, 9999.99999999999, -2.33898115924157E-05, 21.4095815133938, 2 ),
      ( 350, 34.3720472723693, 99999.9999999632, -0.00023306786151844, 21.4144014085721, 2 ),
      ( 350, 344.413944566323, 999999.999741313, -0.00224608222050813, 21.462236682698, 2 ),
      ( 350, 3480.33644744155, 9999999.99999999, -0.0126231537736282, 21.8981577554099, 1 ),
      ( 400, 0.300685373301188, 999.999955437293, -1.82882441021108E-07, 21.8276367606814, 2 ),
      ( 400, 3.00685865260876, 9999.99566873235, -1.82266135071852E-06, 21.8279923417981, 2 ),
      ( 400, 30.0690612275554, 100000, -1.76096732091683E-05, 21.8315452052143, 2 ),
      ( 400, 300.719509045483, 1000000, -0.000113700083699474, 21.8667764594302, 1 ),
      ( 400, 2989.89529044773, 10000000, 0.00567173095586156, 22.1876364395547, 1 ),
      ( 500, 0.240547809925867, 1000.00042172376, 1.80895778686993E-06, 22.8253839234351, 1 ),
      ( 500, 2.40543901635875, 9999.99999999999, 1.80928878377991E-05, 22.8256014590053, 1 ),
      ( 500, 24.0504659715683, 100000.000000016, 0.000181260745949703, 22.8277749176657, 1 ),
      ( 500, 240.105028910983, 1000000.00018357, 0.00184596241268818, 22.8493198392889, 1 ),
      ( 500, 2353.90129697325, 10000000.0000001, 0.0219130856464353, 23.0460269338612, 1 ),
      ( 600, 0.200456364725142, 1000.00053141222, 2.51101707887933E-06, 23.8360371006941, 1 ),
      ( 600, 2.00451844388405, 9999.99999999999, 2.51120314359502E-05, 23.8361829890628, 1 ),
      ( 600, 20.0406514452828, 100000.000000034, 0.000251307642029276, 23.8376405950525, 1 ),
      ( 600, 199.950630811626, 1000000.00034665, 0.0025318616262161, 23.8520892465151, 1 ),
      ( 600, 1951.43458862224, 10000000, 0.0272282725241176, 23.9842468986541, 1 ),
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
      ( 150, 0.801997933739044, 999.999997672491, -0.000220416245578962, 25.6072352881549, 2 ),
      ( 150, 8.03597171627403, 9999.99999026813, -0.00221007654270481, 25.6279035666683, 2 ),
      ( 200, 0.601423233202837, 999.999999867289, -9.5378638247842E-05, 27.4095677406157, 2 ),
      ( 200, 6.01940392305131, 9999.99863652229, -0.000954449909225602, 27.4198204933188, 2 ),
      ( 200, 60.7202222765074, 99999.9963980081, -0.00961187549940053, 27.525683798029, 2 ),
      ( 200, 19430.9478302941, 10000000.0000032, -0.690511304169952, 33.5298669115423, 1 ),
      ( 250, 0.481116028338988, 999.999999988542, -4.84958275039595E-05, 29.7939298809093, 2 ),
      ( 250, 4.81326177472434, 9999.99988403575, -0.000485079061921021, 29.7986609079631, 2 ),
      ( 250, 48.344365177209, 99999.9999999606, -0.00486293600750339, 29.8462970522118, 2 ),
      ( 250, 506.36791716705, 1000000.01069285, -0.0499147366361591, 30.357455672006, 2 ),
      ( 300, 0.400921332678661, 999.999999998621, -2.68194661329936E-05, 32.7271034269229, 2 ),
      ( 300, 4.01018137823204, 9999.99998611092, -0.000268211410784234, 32.7296664254367, 2 ),
      ( 300, 40.1989431757422, 99999.9999999998, -0.00268378093571776, 32.7553366824689, 2 ),
      ( 300, 412.033632185462, 1000000.0000011, -0.0269954953529384, 33.0161205395818, 2 ),
      ( 300, 5422.82330099636, 9999999.99999633, -0.260697688399812, 35.7777730813219, 2 ),
      ( 350, 0.343642928938507, 999.999999999805, -1.53903414959149E-05, 36.0177034925302, 2 ),
      ( 350, 3.43690532941999, 9999.99999803059, -0.000153896544830656, 36.0192951037091, 2 ),
      ( 350, 34.4167061693668, 99999.9802637713, -0.00153826874048114, 36.0352104020224, 2 ),
      ( 350, 348.978179990418, 999999.999736006, -0.0153033631864498, 36.1942232120916, 2 ),
      ( 350, 3961.84164384661, 9999999.99999994, -0.132631561157415, 37.670818802908, 2 ),
      ( 400, 0.300685587414197, 999.999999999972, -8.82076624193018E-06, 39.4550536359076, 2 ),
      ( 400, 3.00709456607159, 9999.99999971014, -8.81963292406778E-05, 39.4561432597287, 2 ),
      ( 400, 30.0948017439923, 99999.997145458, -0.000880824195698258, 39.4670324877845, 2 ),
      ( 400, 303.318325935776, 999999.999997823, -0.00868853139922296, 39.5751903471248, 2 ),
      ( 400, 3229.60881524412, 10000000.0000128, -0.0689803244354475, 40.5456211519834, 2 ),
      ( 500, 0.240546901894179, 999.983164971816, -2.25701515918048E-06, 46.1898301883354, 2 ),
      ( 500, 2.40551775265735, 9999.99999999677, -2.25612810167131E-05, 46.1904439723967, 2 ),
      ( 500, 24.0600407655251, 99999.9999695921, -0.000224686010414329, 46.196575988206, 2 ),
      ( 500, 241.065206598461, 1000000, -0.00215235743016979, 46.2573083002326, 2 ),
      ( 500, 2431.16905192177, 10000000.000001, -0.0105733374674643, 46.8004179510676, 1 ),
      ( 600, 0.2004551776062, 1000.00348402611, 5.50144585368335E-07, 52.3188484808336, 1 ),
      ( 600, 2.00454186065569, 10000, 5.5076211245801E-06, 52.3192482884696, 1 ),
      ( 600, 20.0444126099547, 100000.000000399, 5.5696277804813E-05, 52.3232425964939, 1 ),
      ( 600, 200.331168865111, 1000000.01308523, 0.000619580211415114, 52.3628080040875, 1 ),
      ( 600, 1979.04873729078, 10000000.0000014, 0.0128870821326658, 52.7204612209358, 1 ),
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
      ( 100, 21338.2121480681, 1000000.00013173, -0.943635286582982, 46.3401676583576, 1 ),
      ( 100, 21453.1249282686, 10000000.0000005, -0.439372018566387, 46.6879459115387, 1 ),
      ( 150, 0.802283942600382, 999.99999959366, -0.000584748503697541, 30.4175437046176, 2 ),
      ( 200, 0.601508867525729, 999.999987370878, -0.000245651116635496, 33.9956014615505, 2 ),
      ( 200, 6.02847612113696, 9999.99999999711, -0.00246580708473627, 34.0345612827158, 2 ),
      ( 200, 61.7125204790363, 100000.002692851, -0.0255444096425371, 34.5381179893765, 2 ),
      ( 250, 0.481149885218548, 999.999999235249, -0.000126780159916729, 38.6843616746759, 2 ),
      ( 250, 4.81700248783235, 9999.99196260374, -0.00126917794520845, 38.7000431321327, 2 ),
      ( 250, 48.7342410286239, 100000.000068966, -0.0128318923194542, 38.8703658693716, 2 ),
      ( 250, 563.411568484717, 999999.990842223, -0.146114647342362, 41.6626749887511, 2 ),
      ( 250, 15619.3623830941, 10000000.0000013, -0.691991982028944, 46.0680523672216, 1 ),
      ( 300, 0.400936707141408, 999.999999953308, -7.30863707250047E-05, 44.360737008687, 2 ),
      ( 300, 4.01200760984846, 9999.99859731125, -0.000731197045424589, 44.3688380905341, 2 ),
      ( 300, 40.3874140106443, 99999.9999858192, -0.00734569423053642, 44.4515602161072, 2 ),
      ( 300, 434.48029163441, 999999.999999994, -0.077271370297432, 45.455279308478, 2 ),
      ( 300, 12674.2794872294, 10000000.0000003, -0.683684264232492, 50.7752826289243, 1 ),
      ( 350, 0.343650383901381, 999.999999973451, -4.50052558349562E-05, 50.6267754882604, 2 ),
      ( 350, 3.43789672900236, 9999.99973105328, -0.000450144609713933, 50.631605070939, 2 ),
      ( 350, 34.5191973722186, 99999.9999998001, -0.00451069551556088, 50.6801817300485, 2 ),
      ( 350, 360.233406977624, 1000000.0126636, -0.0460770407283524, 51.1959776445639, 2 ),
      ( 350, 7216.89179404634, 10000000.0000024, -0.523846376481092, 57.7610913562659, 1 ),
      ( 400, 0.300689224470953, 999.999999994551, -2.88383181177972E-05, 57.082899019258, 2 ),
      ( 400, 3.0076729537183, 9999.99994511655, -0.000288403271379373, 57.0860662162878, 2 ),
      ( 400, 30.1550839531597, 99999.9999999952, -0.00288603550916061, 57.1177836921067, 2 ),
      ( 400, 309.677731928754, 1000000.00004398, -0.0290533606458307, 57.4396680346339, 2 ),
      ( 400, 4193.49308916596, 9999999.99999999, -0.282983072331126, 60.8339591466297, 2 ),
      ( 500, 0.240547431947661, 999.999999999711, -1.24277697848999E-05, 69.5547169139185, 2 ),
      ( 500, 2.40574337931269, 9999.99999710563, -0.000124267001887512, 69.556359344377, 2 ),
      ( 500, 24.084347186108, 100000, -0.0012415922222157, 69.5727722808599, 2 ),
      ( 500, 243.539931761858, 999999.99975749, -0.0122997869791392, 69.7357146241866, 2 ),
      ( 500, 2678.52229876783, 9999999.99999998, -0.10195094291278, 71.1757394812776, 2 ),
      ( 600, 0.20045472016073, 999.999999999986, -5.07892472501855E-06, 80.8021497008722, 2 ),
      ( 600, 2.00463881132686, 9999.99999985896, -5.07775581969998E-05, 80.8031503630641, 2 ),
      ( 600, 20.0555303979446, 99999.998656132, -0.000506602965703006, 80.8131463404444, 2 ),
      ( 600, 201.449889919158, 999999.9999999, -0.00494509006300009, 80.912026192231, 2 ),
      ( 600, 2075.37432955563, 10000000.0000003, -0.0341323046565019, 81.7774918756782, 1 ),
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
