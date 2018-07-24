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
	/// Tests and test data for <see cref="Mixture_HCl_H2S"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_HCl_H2S : MixtureTestBase
    {

    public Test_Mixture_HCl_H2S()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("7647-01-0", 0.5), ("7783-06-4", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601526670802571, 999.999999839864, -0.000275255350798964, 25.069374871353, 2 ),
      ( 200, 6.03025068689061, 9999.99831418236, -0.00275937290811064, 25.16085017769, 2 ),
      ( 200, 28509.0070476566, 99999.9999993599, -0.997890627703971, 43.0348485449078, 1 ),
      ( 200, 28531.1389892644, 1000000.00138627, -0.978922639634205, 43.0503257712994, 1 ),
      ( 200, 28744.4664789262, 10000000.0000008, -0.790790656071311, 43.2042365948893, 1 ),
      ( 250, 0.481152062379124, 999.999999986103, -0.000131319160529296, 25.332784263087, 2 ),
      ( 250, 4.81722188215093, 9999.99985748693, -0.00131467955415931, 25.3659439195961, 2 ),
      ( 250, 48.7573970990667, 99999.999999778, -0.0133007368774666, 25.7028529863498, 2 ),
      ( 250, 25861.9598460332, 1000000.00002482, -0.981397818234016, 39.0327903788007, 1 ),
      ( 250, 26217.1585820185, 10000000.0000006, -0.816498467448994, 39.1781649869115, 1 ),
      ( 300, 0.400936852206132, 999.999999998409, -7.34628428379715E-05, 25.8118475044967, 2 ),
      ( 300, 4.01202309639844, 9999.99998384457, -0.000735069033949978, 25.8265225004068, 2 ),
      ( 300, 40.3894347652114, 99999.9999999989, -0.00739537304932737, 25.9743995427046, 2 ),
      ( 300, 435.380172789236, 999999.999999985, -0.0791785586450055, 27.6220558856382, 2 ),
      ( 300, 23238.9610645739, 10000000.0017028, -0.82748480140859, 36.77495828534, 1 ),
      ( 350, 0.34365044102999, 999.999999999984, -4.51861733955115E-05, 26.459377101528, 2 ),
      ( 350, 3.43790310714901, 9999.99999787539, -0.000452013708483548, 26.4668603853106, 2 ),
      ( 350, 34.5200551587122, 100000, -0.00453544697951769, 26.5420392293211, 2 ),
      ( 350, 360.585815472031, 999999.990550908, -0.047009343674156, 27.3331177161042, 2 ),
      ( 350, 18971.8077138487, 10000000.0000003, -0.81887075919872, 35.8518466401526, 1 ),
      ( 400, 0.300689417914392, 999.999999999947, -2.94963174147179E-05, 27.214883946121, 2 ),
      ( 400, 3.00769281119396, 9999.99999946196, -0.000295018278403433, 27.2191333325398, 2 ),
      ( 400, 30.1571909601372, 99999.9943932415, -0.00295571582586869, 27.2617563615758, 2 ),
      ( 400, 310.022868341084, 999999.999879269, -0.0301342920476909, 27.7014669914977, 2 ),
      ( 400, 5045.09159880988, 10000000, -0.404013697680322, 34.6408308690804, 2 ),
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
      ( 200, 0.601519589005286, 999.987172075742, -0.000262888039461, 22.9480330515255, 2 ),
      ( 200, 6.0295045181371, 9999.99999981199, -0.00263536935070984, 23.0534650077301, 2 ),
      ( 200, 30082.1141847383, 1000000.00003737, -0.980009335416581, 39.6017962804235, 1 ),
      ( 200, 30352.4790715084, 9999999.99999982, -0.801874023781985, 39.7318211397992, 1 ),
      ( 250, 0.481146866815259, 999.999999999956, -0.000119928202053817, 23.0767369968248, 2 ),
      ( 250, 4.8166742230077, 9999.99999956424, -0.00120053492764127, 23.1166595100673, 2 ),
      ( 250, 48.6998222478596, 99999.9952450217, -0.0121336341769608, 23.5217279674296, 2 ),
      ( 250, 26875.1561863377, 1000000.00013862, -0.982099111891459, 36.5983951008984, 1 ),
      ( 250, 27365.1003740042, 10000000.0000838, -0.824196090215265, 36.6577938321499, 1 ),
      ( 300, 0.400934006083469, 999.983603639655, -6.57694371472814E-05, 23.3164023144461, 2 ),
      ( 300, 4.01171623125256, 9999.99999991062, -0.000658039260128595, 23.332984254392, 2 ),
      ( 300, 40.3576960069644, 99999.9990398413, -0.00661416244476995, 23.5001467928073, 2 ),
      ( 300, 431.054391521574, 999999.999982844, -0.0699372414869559, 25.3283568006172, 2 ),
      ( 300, 23566.1838724237, 10000000.0000346, -0.82988012033734, 34.8486448701441, 1 ),
      ( 350, 0.343648961085276, 999.997761033673, -4.02855853902627E-05, 23.6445943607564, 2 ),
      ( 350, 3.43773644145145, 9999.99999998551, -0.000402960473516422, 23.6523249403374, 2 ),
      ( 350, 34.5029044465694, 99999.9998161686, -0.0040400294589032, 23.7300199533146, 2 ),
      ( 350, 358.513215920528, 999999.99999979, -0.0414994435494114, 24.5482406419965, 2 ),
      ( 350, 15991.4770691546, 10000000.0000002, -0.785113585525178, 36.7827959297955, 1 ),
      ( 400, 0.300688662708631, 999.995263613193, -2.63905383063086E-05, 24.0348249308385, 2 ),
      ( 400, 3.00760109593667, 9999.99999999702, -0.000263938786293398, 24.0388683246255, 2 ),
      ( 400, 30.147741415968, 99999.9999679958, -0.00264260865164873, 24.0794360914308, 2 ),
      ( 400, 308.946153794052, 999999.999999996, -0.0267536150140508, 24.4987583101136, 2 ),
      ( 400, 4332.98802168561, 9999999.99999947, -0.306066100735695, 30.1619144010796, 2 ),
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
      ( 200, 0.60150270487407, 999.999999999861, -0.00023423485673886, 20.8255206755074, 2 ),
      ( 200, 6.02776840481765, 9999.99999859716, -0.00234751637886299, 20.9343093024703, 2 ),
      ( 200, 61.6160056804715, 99999.9840676385, -0.0240168804317919, 22.0531423094568, 2 ),
      ( 200, 31766.4458723013, 1000000.00082471, -0.981069276216086, 36.7415137009071, 1 ),
      ( 200, 32117.3294745075, 10000000.0000011, -0.812760954347254, 36.8285170779041, 1 ),
      ( 250, 0.481139901472675, 999.999999999962, -0.000104859092311286, 20.8198665318488, 2 ),
      ( 250, 4.81594893413042, 9999.99999961045, -0.0010495207496835, 20.859102126281, 2 ),
      ( 250, 48.6238676550229, 99999.9957521722, -0.0105899157736604, 21.256667774327, 2 ),
      ( 250, 545.243233294082, 999999.99961333, -0.117660852568387, 25.8824406258221, 2 ),
      ( 250, 28472.9279246498, 10000000.0006149, -0.831036186063732, 34.5111669644205, 1 ),
      ( 300, 0.400930900537756, 999.989852361159, -5.74304814461472E-05, 20.8206106305718, 2 ),
      ( 300, 4.01138344980175, 9999.9999999126, -0.000574540762802536, 20.8359497930649, 2 ),
      ( 300, 40.3233971611869, 99999.9989956618, -0.00576860355777516, 20.9905068502676, 2 ),
      ( 300, 426.593706060449, 999999.999990712, -0.0602114636147854, 22.6637869928689, 2 ),
      ( 300, 23279.2810297841, 9999999.99994288, -0.827783394970647, 33.532289729244, 1 ),
      ( 350, 0.343647485452934, 999.988795163101, -3.53972866166694E-05, 20.8296802660032, 2 ),
      ( 350, 3.43757025236601, 9999.99999997765, -0.000354041080090565, 20.8364447977594, 2 ),
      ( 350, 34.4858483423567, 99999.9997714569, -0.00354685283492896, 20.9044145967707, 2 ),
      ( 350, 356.5164141442, 999999.999999845, -0.0361304346870647, 21.6177779135254, 2 ),
      ( 350, 6410.25245487584, 9999999.9887738, -0.463928567489382, 33.0058781410894, 2 ),
      ( 400, 0.300687940689928, 999.994085887704, -2.33952223874027E-05, 20.8547203013014, 2 ),
      ( 400, 3.00751272518943, 9999.9999999959, -0.000233969190787838, 20.8581128252966, 2 ),
      ( 400, 30.1386526195888, 99999.999958608, -0.00234124669289979, 20.8921451262378, 2 ),
      ( 400, 307.937161698782, 999999.999999999, -0.0235640793455038, 21.2432695946333, 2 ),
      ( 400, 3980.42453604827, 9999999.9827443, -0.24460091270473, 25.7216486765407, 2 ),
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
