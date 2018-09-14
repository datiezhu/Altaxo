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
  /// Tests and test data for <see cref="Mixture_Hydrogen_CO"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Hydrogen_CO : MixtureTestBase
  {

    public Test_Mixture_Hydrogen_CO()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("1333-74-0", 0.5), ("630-08-0", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 1.20298145009989, 999.999999833328, -0.000215511882072606, 20.7846760237422, 2 ),
      ( 100, 12.0532416232121, 9999.99826479343, -0.00215872928157832, 20.8132931128655, 2 ),
      ( 100, 122.974052147325, 99999.9999178799, -0.0219707553548008, 21.1107090050312, 2 ),
      ( 100, 25249.7915095557, 1000000, -0.952367044581979, 29.0388916017759, 1 ),
      ( 100, 26474.6838467535, 10000000.0000299, -0.545708571906404, 29.5147119769863, 1 ),
      ( 100, 31473.9045346558, 100000000.000001, 2.82133138892614, 33.0099600711171, 1 ),
      ( 150, 0.801868022965626, 999.999999997861, -6.63792899557166E-05, 20.7864332676569, 2 ),
      ( 150, 8.02347548609116, 9999.99997835823, -0.00066399289586603, 20.7914916942123, 2 ),
      ( 150, 80.719076622379, 99999.9999999981, -0.00666009927906434, 20.8424258530797, 2 ),
      ( 150, 861.055525757624, 1000000.00034788, -0.068800127832959, 21.3919129385991, 2 ),
      ( 150, 16393.6644150063, 10000000.0001861, -0.510899591912771, 25.5823742651376, 1 ),
      ( 150, 27937.0091859345, 100000000, 1.87008101045929, 28.5965225452728, 1 ),
      ( 200, 0.601376772625431, 999.999999999927, -2.60668094055183E-05, 20.7901904094403, 2 ),
      ( 200, 6.01517875450187, 9999.99999925373, -0.000260638635780868, 20.7919755913418, 2 ),
      ( 200, 60.2930783087713, 99999.9924725374, -0.00260342705981773, 20.8098795098122, 2 ),
      ( 200, 617.238392285377, 999999.999918265, -0.0257231174062139, 20.9938426276408, 2 ),
      ( 200, 7425.85156240457, 10000000, -0.190178942309762, 22.8256702009987, 1 ),
      ( 200, 24888.1563396548, 99999999.9999999, 1.41625409469787, 26.4290447283744, 1 ),
      ( 250, 0.481093936280484, 999.999999999997, -1.05155328898064E-05, 20.7983591407655, 2 ),
      ( 250, 4.81139452536533, 9999.99999996756, -0.00010511550215183, 20.7992961727494, 2 ),
      ( 250, 48.1593188173043, 99999.9996842552, -0.00104717189188006, 20.8086797922151, 2 ),
      ( 250, 485.984047798897, 999999.999999976, -0.0100726978584863, 20.9037293082611, 2 ),
      ( 250, 5108.28808671858, 9999999.99999885, -0.0582189783458112, 21.854784926883, 1 ),
      ( 250, 22324.4088043685, 99999999.9999993, 1.15499044806171, 25.1582280696276, 1 ),
      ( 300, 0.400908753772982, 999.993195059128, -3.3823056510257E-06, 20.8257239619988, 2 ),
      ( 300, 4.00920946393703, 9999.99999999922, -3.37937591272084E-05, 20.8263382712614, 2 ),
      ( 300, 40.1041742216836, 99999.9999927773, -0.000334988692869751, 20.8324857038117, 2 ),
      ( 300, 402.137430624381, 1000000, -0.00305873754036588, 20.8943473049788, 2 ),
      ( 300, 4025.84720371904, 10000000.0185696, -0.00416638412194389, 21.5098740446632, 1 ),
      ( 300, 20200.8228228644, 100000000, 0.984609247272827, 24.355059265891, 1 ),
      ( 350, 0.343634833834221, 1000.00002353541, 2.2406110214052E-07, 20.8946433032884, 1 ),
      ( 350, 3.43634135466369, 10000.0023908867, 2.26085289087195E-06, 20.8950986041483, 1 ),
      ( 350, 34.3626449303648, 100000, 2.46286884470032E-05, 20.8996530793447, 1 ),
      ( 350, 343.482258008903, 1000000.00000507, 0.000444431582428556, 20.945322935659, 1 ),
      ( 350, 3364.67720185548, 10000000.0094977, 0.0213012772666371, 21.3965924206039, 1 ),
      ( 350, 18441.503807532, 99999999.9999994, 0.863377932509251, 23.8467497353215, 1 ),
      ( 400, 0.300679889091913, 1000.00033150965, 2.14948255734896E-06, 21.0234543410144, 1 ),
      ( 400, 3.00674081251563, 10000, 2.15085858709886E-05, 21.0238163762967, 1 ),
      ( 400, 30.0615477296648, 100000.000000015, 0.000216459344658211, 21.0274370859616, 1 ),
      ( 400, 299.990911180428, 1000000.00026265, 0.00229886012836473, 21.063668820123, 1 ),
      ( 400, 2907.77591074008, 10000000.0055403, 0.0340568103011497, 21.4196525561439, 1 ),
      ( 400, 16971.0187572258, 100000000, 0.771729515046567, 23.5532868413982, 1 ),
      ( 500, 0.240543513768192, 1000.00078160795, 3.7696184220719E-06, 21.47596810522, 1 ),
      ( 500, 2.40535369825054, 10000, 3.77027114116668E-05, 21.4762248495436, 1 ),
      ( 500, 24.0453623945306, 100000.000000139, 0.000377680793028572, 21.4787919793561, 1 ),
      ( 500, 239.624157433996, 1000000.00163982, 0.00384051939944034, 21.5044284248369, 1 ),
      ( 500, 2305.14897962606, 10000000.0000036, 0.043509295003189, 21.7546316358846, 1 ),
      ( 500, 14662.5185013499, 100000000, 0.640539711090848, 23.431995839068, 1 ),
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
      ( 100, 1.20282503600091, 999.999999389126, -8.55009617633354E-05, 17.5229055048063, 2 ),
      ( 100, 12.0375186982566, 9999.99369172283, -0.00085538880396821, 17.5275175655397, 2 ),
      ( 100, 121.31457641821, 99999.9983018234, -0.00859218310978386, 17.5739888575526, 2 ),
      ( 100, 1321.83532805595, 1000000, -0.0901119316637523, 18.0829864572573, 2 ),
      ( 150, 0.801833565645947, 999.999999994305, -2.34089855156104E-05, 18.9295499527512, 2 ),
      ( 150, 8.02002511342023, 9999.99994305491, -0.00023405887472182, 18.9309189677533, 2 ),
      ( 150, 80.3693382291919, 99999.9999999966, -0.00233744210872094, 18.9446144187194, 2 ),
      ( 150, 820.698708341321, 999999.999657464, -0.02300955589113, 19.0819166019732, 2 ),
      ( 150, 9316.92223289059, 10000000.0000002, -0.139399497502581, 20.2376444458276, 1 ),
      ( 200, 0.601364852582278, 999.999999999939, -6.24567688677468E-06, 19.8776375508376, 2 ),
      ( 200, 6.01398638501197, 9999.99999939031, -6.24242342327742E-05, 19.8783218040027, 2 ),
      ( 200, 60.1734760652024, 99999.9948192784, -0.000620977888378168, 19.8851620554774, 2 ),
      ( 200, 604.913559908333, 1000000, -0.00587267913342571, 19.9532950210457, 2 ),
      ( 200, 6124.24250669419, 10000000.0008713, -0.0180645263540666, 20.572650612859, 1 ),
      ( 250, 0.481088853075126, 1000.00058761677, 4.94017429625893E-08, 20.4036738336773, 1 ),
      ( 250, 4.81088629974347, 10000, 5.1414024783152E-07, 20.4041135220153, 1 ),
      ( 250, 48.1085434843804, 100000.00000004, 7.1556470451204E-06, 20.4085088716763, 1 ),
      ( 250, 480.956722530896, 1000000, 0.000274774806650001, 20.452296748266, 1 ),
      ( 250, 4699.77528229693, 10000000.0000001, 0.0236422986723131, 20.8644187559311, 1 ),
      ( 250, 23578.6166125479, 100000000.002724, 1.04036091359671, 23.0771874872004, 1 ),
      ( 300, 0.400906324320305, 1000, 2.67755194188378E-06, 20.6794269498538, 1 ),
      ( 300, 4.00896658716325, 10000.0000000134, 2.67875804236266E-05, 20.6797492047121, 1 ),
      ( 300, 40.0799549856913, 100000.000145523, 0.000269081916618538, 20.6829707959903, 1 ),
      ( 300, 399.783448245133, 1000000.00000001, 0.00281139583258628, 20.7150860986918, 1 ),
      ( 300, 3855.68805278579, 10000000, 0.0397817258015743, 21.022623073098, 1 ),
      ( 300, 21354.4177016034, 100000000.00076, 0.877397938785105, 22.9337762753392, 1 ),
      ( 350, 0.343633599629554, 1000, 3.82018189580683E-06, 20.8333737906586, 1 ),
      ( 350, 3.43621782865168, 10000.0000000348, 3.82091820030277E-05, 20.8336283394026, 1 ),
      ( 350, 34.3503409717574, 100000.000359979, 0.000382827801956922, 20.8361731706152, 1 ),
      ( 350, 342.299417875323, 1000000.00000005, 0.00390153889466915, 20.8615534765639, 1 ),
      ( 350, 3285.50131595662, 10000000.0190892, 0.0459131784144536, 21.1068303035973, 1 ),
      ( 350, 19509.2328856117, 100000000.000094, 0.761396331610782, 22.7722983986865, 1 ),
      ( 400, 0.30067925841881, 1000, 4.28997681820568E-06, 20.9474985370841, 1 ),
      ( 400, 3.00667648370721, 10000.0000000554, 4.29043670244937E-05, 20.9477091384886, 1 ),
      ( 400, 30.0551460498077, 100000.000563344, 0.000429503248099071, 20.9498146610037, 1 ),
      ( 400, 299.381031473383, 1000000.00000009, 0.00434067863549844, 20.9708194208876, 1 ),
      ( 400, 2870.05284386677, 10000000.0120154, 0.0476481260139479, 21.1748691960822, 1 ),
      ( 400, 17961.2897813755, 100000000.000061, 0.674047643492309, 22.6430448171198, 1 ),
      ( 500, 0.240543377310116, 1000.00000000001, 4.41230427145321E-06, 21.2084499033028, 1 ),
      ( 500, 2.40533825124297, 10000.0000000474, 4.41249224907311E-05, 21.2086066141624, 1 ),
      ( 500, 24.0438300289298, 100000.000476542, 0.000441437041281713, 21.2101733970062, 1 ),
      ( 500, 239.482813768395, 1000000.00000005, 0.00443298987202681, 21.2258081708377, 1 ),
      ( 500, 2299.56396751793, 10000000.0009455, 0.0460436937588754, 21.3785020151271, 1 ),
      ( 500, 15517.3072320255, 100000000, 0.550168692698425, 22.5565119477934, 1 ),
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
      ( 100, 1.20272488262511, 999.999999999951, -2.23602397694594E-06, 14.2633540929881, 2 ),
      ( 100, 12.0274904171112, 9999.99999960519, -2.23225350840846E-05, 14.2639927143712, 2 ),
      ( 100, 120.298609779737, 99999.9987467484, -0.000219374514114907, 14.2703631777149, 2 ),
      ( 100, 1204.81269710712, 999999.999999982, -0.00173512763326239, 14.3325427321325, 2 ),
      ( 100, 11414.3243298246, 10000000.0122016, 0.0536954783566095, 14.8480384207942, 1 ),
      ( 100, 41217.2353270024, 99999999.9999995, 1.91800792498932, 17.3175115087559, 1 ),
      ( 150, 0.801810209012394, 1000.00000000011, 5.72021056556917E-06, 17.0729605370279, 1 ),
      ( 150, 8.01768921576872, 10000.0000011126, 5.72159353174958E-05, 17.0732885938602, 1 ),
      ( 150, 80.1355173886846, 100000.012398125, 0.00057355554798139, 17.0765674605399, 1 ),
      ( 150, 797.122046685791, 1000000.00030786, 0.00588711461542455, 17.1091896129584, 1 ),
      ( 150, 7437.54708326839, 10000000.0114861, 0.0780634887323999, 17.4217128109886, 1 ),
      ( 150, 35307.3018491392, 100000000.000319, 1.27096026471543, 19.574767860387, 1 ),
      ( 200, 0.601357062870544, 1000.0000000001, 6.70779711755579E-06, 18.9651564331671, 1 ),
      ( 200, 6.01320758029919, 10000.0000010023, 6.70833681747755E-05, 18.9653859978087, 1 ),
      ( 200, 60.0957627539346, 100000.010270062, 0.000671377041108216, 18.9676814154322, 1 ),
      ( 200, 597.316436187479, 1000000.00006125, 0.00677138652052668, 18.990611771372, 1 ),
      ( 200, 5595.50481065999, 10000000.0000576, 0.0747217936553834, 19.2167785234538, 1 ),
      ( 200, 30751.5096807106, 100000000.000011, 0.955549834449146, 20.9807690831363, 1 ),
      ( 250, 0.481085797419394, 1000.00000000005, 6.40198067819562E-06, 20.0090149361248, 1 ),
      ( 250, 4.81058078985381, 10000.0000005579, 6.40220741319654E-05, 20.0091951327595, 1 ),
      ( 250, 48.0780961726858, 100000.005616016, 0.000640448845321192, 20.0109970306466, 1 ),
      ( 250, 478.015938042548, 1000000.0000119, 0.00642852891358895, 20.0290082205742, 1 ),
      ( 250, 4507.84149259346, 10000000, 0.0672266939993772, 20.2075776039131, 1 ),
      ( 250, 27215.4967396771, 100000000.000008, 0.767701989506657, 21.673966668023, 1 ),
      ( 300, 0.400905055295568, 1000.00000000003, 5.84296010371318E-06, 20.5331431659569, 1 ),
      ( 300, 4.00883973891369, 10000.0000002951, 5.84305633772399E-05, 20.533292488161, 1 ),
      ( 300, 40.0673243333226, 100000.002953247, 0.000584402491012246, 20.5347856375089, 1 ),
      ( 300, 398.574035492477, 1000000.00000262, 0.0058542756204101, 20.5497092564031, 1 ),
      ( 300, 3782.6850585568, 10000000, 0.0598487359155649, 20.6976621713419, 1 ),
      ( 300, 24413.3167598854, 100000000.000001, 0.642166862089634, 21.9439780058439, 1 ),
      ( 350, 0.3436330985635, 1000.00000000002, 5.2783300497719E-06, 20.7721124823439, 1 ),
      ( 350, 3.43616775017672, 10000.0000001602, 5.27836708140663E-05, 20.7722401452933, 1 ),
      ( 350, 34.3453612120792, 100000.001599402, 0.000527874065403817, 20.7735166870815, 1 ),
      ( 350, 341.82910612113, 1000000.00000066, 0.00528277495591776, 20.786272970112, 1 ),
      ( 350, 3262.15993757152, 10000000.0112374, 0.0533968872416984, 20.9126211463503, 1 ),
      ( 350, 22143.3463400532, 99999999.9999998, 0.55186531924869, 21.9929545892506, 1 ),
      ( 400, 0.300679114378546, 1000.00000000001, 4.76902865678517E-06, 20.8715485381868, 1 ),
      ( 400, 3.00666209441913, 10000.0000000907, 4.76903738968914E-05, 20.8716599635603, 1 ),
      ( 400, 30.0537218322906, 100000.0009043, 0.000476912660076892, 20.8727741225426, 1 ),
      ( 400, 299.253053419068, 1000000.00000019, 0.00477019326587896, 20.8839060186625, 1 ),
      ( 400, 2869.32294906781, 10000000.0016234, 0.0479146253850214, 20.9940738018217, 1 ),
      ( 400, 20267.7864973383, 100000000, 0.483539153944345, 21.9457388104803, 1 ),
      ( 500, 0.240543471794442, 1000, 3.94069289141028E-06, 20.9409352352117, 1 ),
      ( 500, 2.40534959944148, 10000.0000000328, 3.94068144646217E-05, 20.9410237193686, 1 ),
      ( 500, 24.0449687906547, 100000.000327684, 0.000394056466748504, 20.9419084698576, 1 ),
      ( 500, 239.600539450097, 1000000.00000002, 0.00393947029423286, 20.9507467862757, 1 ),
      ( 500, 2314.40457736151, 10000000.0000493, 0.0393361688515147, 21.0381432650358, 1 ),
      ( 500, 17347.4170714586, 100000000.000002, 0.386629707868453, 21.8041799815074, 1 ),
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
