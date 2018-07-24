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
	/// Tests and test data for <see cref="Mixture_HCl_CO"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_HCl_CO : MixtureTestBase
    {

    public Test_Mixture_HCl_CO()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("7647-01-0", 0.5), ("630-08-0", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601376847142587, 999.999999999928, -2.6189526428298E-05, 20.7920373749723, 2 ),
      ( 200, 6.01518614747087, 9999.99999928084, -0.000261866175802784, 20.7938320669739, 2 ),
      ( 200, 60.2938226775844, 99999.9927427485, -0.00261573942670309, 20.8118316366091, 2 ),
      ( 200, 617.318727439202, 999999.999922811, -0.025849904370192, 20.9968057536189, 2 ),
      ( 200, 7438.21055844639, 10000000, -0.191524503585226, 22.83963941141, 1 ),
      ( 250, 0.481093971158467, 999.999999999997, -1.05868386807583E-05, 20.7991623243246, 2 ),
      ( 250, 4.81139796238761, 9999.99999996853, -0.000105828586523653, 20.8001029350091, 2 ),
      ( 250, 48.1596627781521, 99999.9996935462, -0.00105430531770162, 20.8095224680226, 2 ),
      ( 250, 486.019165519202, 999999.999999976, -0.0101442247030126, 20.9049431201808, 2 ),
      ( 250, 5111.87607760992, 9999999.99999915, -0.0588800068894866, 21.8600036874083, 1 ),
      ( 300, 0.400908772667278, 999.999943985256, -3.42826628936836E-06, 20.8260052360426, 2 ),
      ( 300, 4.00921131037675, 9999.99442775946, -3.42530832049314E-05, 20.8266213518057, 2 ),
      ( 300, 40.1043584063504, 99999.9999999997, -0.000339578598790896, 20.832786900079, 2 ),
      ( 300, 402.15579456226, 999999.999999372, -0.00310426042039708, 20.8948335788251, 2 ),
      ( 300, 4027.46104608445, 10000000.0154995, -0.00456542304918525, 21.5123289374357, 1 ),
      ( 350, 0.343634845331864, 1000.00002333797, 1.92425581027334E-07, 20.8946947391122, 1 ),
      ( 350, 3.43634244571793, 10000.0023831113, 1.94453827820579E-06, 20.8951511501583, 1 ),
      ( 350, 34.3627535230327, 100000, 2.14696160571624E-05, 20.8997167442882, 1 ),
      ( 350, 343.492963039956, 1000000.00000606, 0.000413253710709283, 20.9454993772684, 1 ),
      ( 350, 3365.55915506797, 10000000.0082031, 0.0210336439176737, 21.397946672369, 1 ),
      ( 400, 0.300679896454685, 1000.00034801718, 2.12664293290009E-06, 21.0234310134039, 1 ),
      ( 400, 3.00674150270529, 10000, 2.12802242654518E-05, 21.0237938218876, 1 ),
      ( 400, 30.0616162948158, 100000.000000016, 0.000214179221396047, 21.0274222713077, 1 ),
      ( 400, 299.997631844933, 1000000.0002874, 0.00227640743008719, 21.0637321170186, 1 ),
      ( 400, 2908.31395167351, 10000000.0048686, 0.0338655100050291, 21.4205174651133, 1 ),
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
      ( 200, 0.601410530654691, 999.999999999984, -8.16014709445622E-05, 20.8022877139211, 2 ),
      ( 200, 6.01852851806301, 9999.99999982883, -0.000816473883948138, 20.8182476258406, 2 ),
      ( 200, 60.6340251300481, 99999.998147644, -0.00821122539294224, 20.9795373536691, 2 ),
      ( 250, 0.481109046157727, 999.990710093037, -4.13258419866295E-05, 20.8055972847757, 2 ),
      ( 250, 4.81288096393086, 9999.99999998202, -0.00041333383886211, 20.8108136444191, 2 ),
      ( 250, 48.3089401947343, 99999.999812259, -0.00414051351992809, 20.8632571670654, 2 ),
      ( 250, 502.253924935729, 999999.999999815, -0.0421395635720228, 21.4166830732596, 2 ),
      ( 300, 0.400916970157036, 999.998043535729, -2.32808593898727E-05, 20.8201788732237, 2 ),
      ( 300, 4.01000993024214, 9999.99999999869, -0.000232808842550454, 20.8224414215052, 2 ),
      ( 300, 40.184315057648, 99999.9999847583, -0.00232805791717729, 20.8451328438341, 2 ),
      ( 300, 410.457817290398, 999999.999999999, -0.0232671432749842, 21.0785377582254, 2 ),
      ( 300, 5124.96520105058, 9999999.9963591, -0.217735885504866, 23.7472251274228, 2 ),
      ( 350, 0.343639852716587, 999.99834146633, -1.37811548416755E-05, 20.8591397617349, 2 ),
      ( 350, 3.43682475644306, 9999.9999999998, -0.000137797801602927, 20.8603818689861, 2 ),
      ( 350, 34.4108808409088, 99999.9999980464, -0.0013765746258518, 20.8728228346363, 2 ),
      ( 350, 348.379862862682, 1000000, -0.0136194608637278, 20.9991430953508, 2 ),
      ( 350, 3886.62926957419, 9999999.99997716, -0.115853113017802, 22.3467274095286, 2 ),
      ( 400, 0.300683215754128, 999.99911094014, -8.27587444688543E-06, 20.9358584021942, 2 ),
      ( 400, 3.00705608962375, 9999.99999999996, -8.27441399825424E-05, 20.9366682094584, 2 ),
      ( 400, 30.0929286246479, 99999.99999967, -0.000825971158112083, 20.9447733251731, 2 ),
      ( 400, 303.139265170301, 999999.996603397, -0.00811025858154267, 21.0264885980266, 2 ),
      ( 400, 3213.04663606439, 9999999.99999996, -0.0641881012232284, 21.867674257913, 2 ),
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
      ( 200, 0.601502342730549, 999.999999999861, -0.000233632932741431, 20.8212136019175, 2 ),
      ( 200, 6.02773186975588, 9999.99999858569, -0.00234146944515885, 20.9296820720171, 2 ),
      ( 200, 61.6120009042011, 99999.9839200965, -0.0239534415636891, 22.0451530569266, 2 ),
      ( 200, 31755.9815879881, 1000000.00089343, -0.981063038130932, 36.7241450429474, 1 ),
      ( 200, 32107.8548656022, 10000000.000003, -0.812705702548642, 36.8109857725633, 1 ),
      ( 250, 0.481139788665835, 999.999999999961, -0.000104624659380565, 20.815315928549, 2 ),
      ( 250, 4.81593760904194, 9999.9999996095, -0.00104717163234113, 20.854406990939, 2 ),
      ( 250, 48.6226890119479, 99999.995741745, -0.0105659318828393, 21.2504980664811, 2 ),
      ( 250, 545.05188962653, 999999.999314129, -0.117351102187981, 25.8575129083794, 2 ),
      ( 250, 28456.8922817424, 10000000.0006399, -0.830940973861022, 34.4953128973411, 1 ),
      ( 300, 0.400930854553395, 999.989842501896, -5.73157946372225E-05, 20.8156145537821, 2 ),
      ( 300, 4.0113788417021, 9999.99999991259, -0.000573392665780453, 20.830892340505, 2 ),
      ( 300, 40.3229265016378, 99999.9989957808, -0.00575699863405654, 20.9848289553506, 2 ),
      ( 300, 426.534525959032, 999999.99999075, -0.0600810714014764, 22.651132100003, 2 ),
      ( 300, 23246.2646950848, 9999999.99995089, -0.827538798208546, 33.5251356138264, 1 ),
      ( 350, 0.343647462524045, 999.988799549223, -3.53305677568699E-05, 20.8241077724837, 2 ),
      ( 350, 3.43756795662247, 9999.9999999777, -0.000353373477071308, 20.8308450268872, 2 ),
      ( 350, 34.4856158835948, 99999.9997720805, -0.00354013599789942, 20.8985401780654, 2 ),
      ( 350, 356.489957002144, 999999.999999845, -0.0360589004586483, 21.6089562042247, 2 ),
      ( 350, 6385.89589432426, 9999999.99464882, -0.461883928194561, 32.9256305905567, 2 ),
      ( 400, 0.300687927556605, 999.994099625091, -2.33515464007252E-05, 20.8485216462809, 2 ),
      ( 400, 3.00751141082757, 9999.9999999959, -0.000233532266648979, 20.8519011685238, 2 ),
      ( 400, 30.138520147968, 99999.9999588797, -0.00233686155816977, 20.8858028332372, 2 ),
      ( 400, 307.922817526442, 999999.999999997, -0.0235185933849734, 21.2355570473245, 2 ),
      ( 400, 3976.9603745792, 9999999.98296243, -0.243942916613202, 25.6927956101305, 2 ),
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
