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
  /// Tests and test data for <see cref="Mixture_Isopentane_Helium"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Isopentane_Helium : MixtureTestBase
  {

    public Test_Mixture_Isopentane_Helium()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("78-78-4", 0.5), ("7440-59-7", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new(double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601356665785543, 1000.00000000002, 7.36811683673617E-06, 12.5359082587641, 1 ),
      ( 200, 6.0131679134267, 10000.0000002325, 7.36804787203085E-05, 12.5360455007158, 1 ),
      ( 200, 60.0918378695606, 100000.001839708, 0.000736735603208767, 12.5374175187305, 1 ),
      ( 250, 0.481086075419512, 1000.00000000001, 5.82411756448493E-06, 12.5532463310058, 1 ),
      ( 250, 4.81060860065819, 10000.0000000732, 5.8240563473368E-05, 12.5533549286249, 1 ),
      ( 250, 48.0808880991826, 100000.000728153, 0.000582344342288918, 12.5544406258624, 1 ),
      ( 250, 478.306452384352, 1000000.00000014, 0.00581724311720898, 12.5652697446429, 1 ),
      ( 250, 4549.12286922934, 10000000.000456, 0.0575420606391621, 12.6708176968438, 1 ),
      ( 250, 31452.2293182348, 100000000.002398, 0.529585939544707, 13.5060157853333, 1 ),
      ( 300, 0.400905483948001, 1000, 4.77374315814756E-06, 12.5704545694211, 1 ),
      ( 300, 4.00888260588043, 10000.0000000278, 4.77369423105243E-05, 12.5705436637543, 1 ),
      ( 300, 40.0716127759473, 100000.000276448, 0.000477320466056214, 12.5714344063138, 1 ),
      ( 300, 399.004830535599, 1000000.00000002, 0.00476828117838122, 12.5803217899481, 1 ),
      ( 300, 3828.41463005239, 10000000.0000229, 0.0471890756575832, 12.6672284031608, 1 ),
      ( 300, 27912.4233431754, 100000000, 0.436304518739803, 13.3771755668706, 1 ),
      ( 300, 87650.6709546883, 1000000000, 3.57392274812208, 16.2686183763049, 1 ),
      ( 350, 0.343633531456991, 1000, 4.01857005533291E-06, 12.5874021359512, 1 ),
      ( 350, 3.43621103850048, 10000.000000012, 4.01853151835732E-05, 12.5874771915269, 1 ),
      ( 350, 34.3496890306311, 100000.000119972, 0.000401814601980845, 12.5882275978905, 1 ),
      ( 350, 342.260981187444, 1000000, 0.00401427932500315, 12.5957167493461, 1 ),
      ( 350, 3304.95291259499, 10000000.0000017, 0.0397573625416549, 12.6691455014546, 1 ),
      ( 350, 25095.1791660864, 100000000, 0.369326395711884, 13.2839052547508, 1 ),
      ( 400, 0.30067951023818, 1000.01826310404, 3.45253534868182E-06, 12.603814360211, 1 ),
      ( 400, 3.00670167863496, 10000.0000000058, 3.45244173602785E-05, 12.6038788798369, 1 ),
      ( 400, 30.0576785129436, 100000.000057583, 0.000345213608016399, 12.6045239630183, 1 ),
      ( 400, 299.647043360863, 999999.999999999, 0.00344907445421794, 12.610963376584, 1 ),
      ( 400, 2907.411347627, 10000000.0000002, 0.0341864716572403, 12.6742370349184, 1 ),
      ( 400, 22795.521513838, 99999999.9999998, 0.319033425681138, 13.2144371929769, 1 ),
      ( 500, 0.240543784618285, 1000.01102718019, 2.66573120454678E-06, 12.6341771841054, 1 ),
      ( 500, 2.40538026681855, 10000.0000000017, 2.66568198003696E-05, 12.6342270408168, 1 ),
      ( 500, 24.0480339053047, 100000.000016538, 0.000266548225532681, 12.6347255367117, 1 ),
      ( 500, 239.905454257231, 999999.999999999, 0.00266348426520369, 12.6397033247643, 1 ),
      ( 500, 2343.490131593, 10000000.0019219, 0.0264367467097295, 12.6887763150115, 1 ),
      ( 500, 19261.0524243791, 100000000.000698, 0.248864461614476, 13.1201005596377, 1 ),
      };

      // TestData for 500 Permille to 500 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_500_500 = new(double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481102570702948, 999.999999988305, -2.84624992841641E-05, 53.2350057336164, 2 ),
      ( 250, 4.81225801373074, 9999.99999997949, -0.000284531817095276, 53.2364965823876, 2 ),
      ( 300, 0.400912953292109, 999.999999999558, -1.3857183340264E-05, 61.8401316772496, 2 ),
      ( 300, 4.00962925251374, 9999.99999562494, -0.000138485331288391, 61.8409307322025, 2 ),
      ( 300, 40.1459902395094, 99999.9999840137, -0.00137623863228671, 61.8489382196835, 2 ),
      ( 350, 0.34363699710266, 999.999999999993, -6.06666414552955E-06, 70.314664127327, 2 ),
      ( 350, 3.43655737925157, 9999.9999999168, -6.06000437314387E-05, 70.3151861083367, 2 ),
      ( 350, 34.3841000013177, 99999.9992989281, -0.000599369008692114, 70.3204130531499, 2 ),
      ( 500, 0.240543772885025, 1000, 2.71350919942583E-06, 93.7035600496202, 1 ),
      ( 500, 2.40537904999174, 10000.0000000421, 2.71627106480033E-05, 93.703829057651, 1 ),
      ( 500, 24.0478456937341, 100000.000538851, 0.000274376859137146, 93.7065204460305, 1 ),
      ( 500, 239.823187488184, 1000000.00000079, 0.00300742883900832, 93.7335497616118, 1 ),
      ( 500, 21644.2066376635, 999999999.999999, 10.1135715292105, 100.389231449651, 1 ),
      };

      // TestData for 999 Permille to 1 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_999_001 = new(double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 0.481473463370684, 999.999965483849, -0.000798768915079077, 93.9532268362855, 2 ),
      ( 250, 4.84991840997489, 9999.99987278548, -0.00804748295562058, 94.2882111194244, 2 ),
      ( 300, 0.401075138545328, 999.999845499681, -0.000418227747030996, 111.125346849261, 2 ),
      ( 300, 4.025973350327, 9999.99852260321, -0.00419758617630509, 111.267348370506, 2 ),
      ( 300, 9619.52107572292, 99999999.9999998, 3.1676440501762, 127.0498763601, 1 ),
      ( 350, 0.343720204410895, 999.999976181214, -0.000248143796488513, 128.049472311798, 2 ),
      ( 350, 3.44491498164198, 9999.99999999007, -0.0024865222952772, 128.11853379639, 2 ),
      ( 350, 35.2591858452109, 100000.001604865, -0.0254031566875363, 128.830804015162, 2 ),
      ( 350, 7747.17850028563, 1000000.00365163, -0.955643862651576, 138.240081448726, 1 ),
      ( 350, 8018.28078327521, 10000000.005679, -0.571435670753494, 138.398760678784, 1 ),
      ( 350, 9231.58713248604, 100000000.001363, 2.72238172532247, 141.497206911544, 1 ),
      ( 350, 11948.4205864157, 999999999.999999, 27.7598607604391, 155.986725645729, 1 ),
      ( 400, 0.300728603707421, 999.999995072465, -0.000159796509985858, 144.442914374319, 2 ),
      ( 400, 3.01162377071773, 9999.99999999977, -0.0015998968740742, 144.480005551122, 2 ),
      ( 400, 30.5631113284856, 100000.000151381, -0.0161978435813984, 144.857408337824, 2 ),
      ( 400, 371.703939308484, 999999.998774622, -0.191075163220582, 150.091384320785, 2 ),
      ( 400, 7270.32541216476, 10000000.0000002, -0.586427661377521, 153.354265624867, 1 ),
      ( 400, 8861.16631119788, 100000000.000239, 2.3932389684023, 156.037769517527, 1 ),
      ( 400, 11782.1070724532, 1000000000, 24.5200997985794, 169.476553462105, 1 ),
      ( 500, 0.240562923704937, 999.999999639324, -7.68407864278355E-05, 174.774363791092, 2 ),
      ( 500, 2.40729489412219, 9999.99629395605, -0.000768707993987604, 174.787650654679, 2 ),
      ( 500, 24.2415258393886, 99999.9999075194, -0.00771741729998089, 174.921605633952, 2 ),
      ( 500, 261.610804449195, 1000000.00000344, -0.0805255953894491, 176.383214714594, 2 ),
      ( 500, 5253.890410031, 10000000.001098, -0.542159390651113, 182.970768628631, 1 ),
      ( 500, 8169.52207103122, 99999999.9999998, 1.94441261764082, 183.78179815568, 1 ),
      ( 500, 11480.5691878063, 1000000000, 19.952309482719, 195.676475229667, 1 ),
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