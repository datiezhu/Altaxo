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
  /// Tests and test data for <see cref="Mixture_Hexane_Argon"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Hexane_Argon : MixtureTestBase
  {

    public Test_Mixture_Hexane_Argon()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("110-54-3", 0.5), ("7440-37-1", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601375808250691, 999.999999993285, -2.90334487250732E-05, 12.5618076588707, 2 ),
      ( 200, 6.01533007719247, 9999.99989702661, -0.000290357267525674, 12.5640552035267, 2 ),
      ( 250, 0.4810931247303, 999.99999999957, -1.33989457972254E-05, 12.5769755903419, 2 ),
      ( 250, 4.81151142941901, 9999.99999568992, -0.000133979422516971, 12.5780947498118, 2 ),
      ( 250, 48.1731613282164, 100000, -0.00133878424117187, 12.5892860930717, 2 ),
      ( 250, 487.561578627373, 999999.99938612, -0.013280168736975, 12.7011387728222, 2 ),
      ( 250, 26982.8078185215, 100000000, 0.782937794410605, 16.4618570387689, 1 ),
      ( 300, 0.400908027344505, 999.999999999972, -6.14069313876891E-06, 12.5943213638394, 2 ),
      ( 300, 4.0093018056599, 9999.99999970813, -6.13949157510276E-05, 12.5949851077627, 2 ),
      ( 300, 40.1151368910863, 99999.9972012952, -0.000612744793359206, 12.6016187055875, 2 ),
      ( 300, 403.327227243514, 999999.999999315, -0.00600421090788598, 12.6675568554076, 2 ),
      ( 300, 4197.45056649814, 10000000.000098, -0.0448831757756801, 13.2717672026559, 2 ),
      ( 300, 24116.8173675254, 100000000.000234, 0.662348556951809, 15.6668232519314, 1 ),
      ( 350, 0.343634173591397, 999.999999999997, -2.37297289194041E-06, 12.6132851352521, 2 ),
      ( 350, 3.4364149303721, 9999.99999998749, -2.3720032831757E-05, 12.6137273600145, 2 ),
      ( 350, 34.3714537469937, 99999.9998852481, -0.000236229810724777, 12.6181467137089, 2 ),
      ( 350, 344.41317416859, 1000000, -0.00226423486899261, 12.6620465156435, 2 ),
      ( 350, 3477.92014910806, 10000000.0000287, -0.0119573563589684, 13.067564690832, 1 ),
      ( 350, 21702.1534394091, 100000000.000296, 0.583406655056896, 15.1292809485827, 1 ),
      ( 400, 0.300679257994869, 999.996714184981, -2.73488849456425E-07, 12.6325179925589, 2 ),
      ( 400, 3.00679994254024, 9999.99999999997, -2.72760257166869E-06, 12.6328376601649, 2 ),
      ( 400, 30.0687156227395, 99999.999999921, -2.65462249736687E-05, 12.63603246936, 2 ),
      ( 400, 300.736969566389, 999999.999674254, -0.000192179391721469, 12.667792632633, 2 ),
      ( 400, 2989.99216042079, 10000000.0000819, 0.00561860361606419, 12.9654640515152, 1 ),
      ( 400, 19687.3447514381, 100000000.000974, 0.527271340630902, 14.7471446277493, 1 ),
      ( 500, 0.24054292688337, 1000.01636813953, 1.68091039276382E-06, 12.6683514273256, 1 ),
      ( 500, 2.40539295133848, 10000.0000000023, 1.68128910050809E-05, 12.6685478007059, 1 ),
      ( 500, 24.0502806151149, 100000.000023795, 0.000168535013410026, 12.6705107994713, 1 ),
      ( 500, 240.12890176952, 1000000, 0.00172589605806617, 12.6900667642021, 1 ),
      ( 500, 2355.44017053316, 10000000.0000172, 0.0212245774867708, 12.8781207084197, 1 ),
      ( 500, 16590.7634309387, 100000000.000002, 0.449862993322133, 14.2498841116663, 1 ),
      ( 600, 0.20045229585655, 1000.01949958731, 2.38137126007009E-06, 12.6990542275456, 1 ),
      ( 600, 2.0044800895662, 10000.0000000046, 2.38155972232272E-05, 12.6991923748893, 1 ),
      ( 600, 20.0405008062499, 100000.000046577, 0.000238390665237716, 12.7005735506546, 1 ),
      ( 600, 199.971394806895, 1000000, 0.00240728399499136, 12.7143551751806, 1 ),
      ( 600, 1953.13329305133, 10000000.0000031, 0.026313889885647, 12.849109273522, 1 ),
      ( 600, 14360.0662526193, 100000000, 0.395904303081496, 13.9480344290792, 1 ),
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
      ( 250, 0.481174327116554, 999.999999999605, -0.000182155451809709, 65.0194866456643, 2 ),
      ( 300, 0.400945990498403, 999.999999969693, -0.000100824071142308, 73.7151348292635, 2 ),
      ( 300, 4.01310461985255, 9999.99999999839, -0.00100893580455586, 73.7337390915324, 2 ),
      ( 350, 0.343654160404597, 999.999999875612, -6.05799588078703E-05, 83.2083313595088, 2 ),
      ( 350, 3.43841701375934, 9999.99873182893, -0.000605975072073444, 83.2179534025902, 2 ),
      ( 350, 34.5734522304281, 99999.9985355372, -0.00607743882169223, 83.3148159536091, 2 ),
      ( 400, 0.300690642340832, 999.999999990219, -3.81396050224188E-05, 92.8311167047484, 2 ),
      ( 400, 3.00793901450104, 9999.99990145155, -0.000381415081613916, 92.8366604062095, 2 ),
      ( 400, 30.1830978677977, 99999.9999983174, -0.00381605811679693, 92.8923185024071, 2 ),
      ( 500, 0.240547154222594, 999.999999999973, -1.58593761259215E-05, 110.754328094321, 2 ),
      ( 500, 2.40581483825819, 9999.99999621198, -0.000158551399710908, 110.756665548976, 2 ),
      ( 500, 24.0924307655515, 99999.9999999998, -0.00158127822068289, 110.780076233892, 2 ),
      ( 500, 244.302920495494, 999999.997105064, -0.0153890145173832, 111.017378973596, 2 ),
      ( 600, 0.200453986053846, 999.999999999982, -6.00291470257038E-06, 126.1088414624, 2 ),
      ( 600, 2.00464808699622, 9999.99999981937, -5.9990349599491E-05, 126.110082433231, 2 ),
      ( 600, 20.0572330023295, 99999.9984099821, -0.000596030747209841, 126.122500066425, 2 ),
      ( 600, 201.577561563518, 999999.999999975, -0.00557988103816224, 126.247314343176, 2 ),
      ( 600, 2050.5136485501, 10000000, -0.0224264886634399, 127.41967108233, 1 ),
      ( 600, 9475.05569932798, 100000000.00002, 1.11558421508654, 130.62308573598, 1 ),
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
      ( 250, 0.481822936353365, 999.999999999514, -0.00152806707167997, 117.540296341253, 2 ),
      ( 300, 0.401206776534848, 999.999987615549, -0.000750762592672813, 134.869390278511, 2 ),
      ( 300, 4.0395914034469, 9999.99048388923, -0.00755911077268726, 135.209522216275, 2 ),
      ( 300, 8412.6650057383, 100000000.01773, 3.76550017484569, 154.799755421707, 1 ),
      ( 350, 0.343778376933794, 999.999998375817, -0.00042188541680179, 153.819650982888, 2 ),
      ( 350, 3.45094731876746, 9999.98232028457, -0.00423474278848511, 153.985612762994, 2 ),
      ( 350, 35.9481158092852, 99999.9999996016, -0.0440852486599029, 155.727096140732, 2 ),
      ( 350, 7206.1292797203, 10000000.0005131, -0.523137417432246, 167.83387178722, 1 ),
      ( 350, 8090.14366068703, 99999999.9999997, 3.2475554979271, 171.274588832556, 1 ),
      ( 400, 0.300757339855226, 999.999999705825, -0.000259896356113464, 173.038428111607, 2 ),
      ( 400, 3.01464426520921, 9999.99690548861, -0.00260479210368076, 173.127842067522, 2 ),
      ( 400, 30.8914862226107, 99999.9996016223, -0.0266600577731201, 174.04700155207, 2 ),
      ( 400, 6670.67361357447, 10000000.0009412, -0.549252157183673, 185.28710172112, 1 ),
      ( 400, 7784.41027752561, 100000000.000001, 2.86258127978404, 188.458075177053, 1 ),
      ( 500, 0.240571579915715, 999.999999998977, -0.000117389680549864, 208.843368840697, 2 ),
      ( 500, 2.40826287529203, 9999.99985111062, -0.00117490591667214, 208.875462610643, 2 ),
      ( 500, 24.3428404405198, 99999.9999997102, -0.0118518010803664, 209.200355537587, 2 ),
      ( 500, 276.850091572208, 999999.984682219, -0.131142277911234, 212.934236530939, 2 ),
      ( 500, 5394.09710359535, 10000000.0017917, -0.554061903757789, 219.1201236992, 1 ),
      ( 500, 7216.78177514911, 100000000.000001, 2.3331108905511, 221.169175521238, 1 ),
      ( 600, 0.200464917878977, 999.999999998209, -6.05349476591031E-05, 239.519934751882, 2 ),
      ( 600, 2.00574230644065, 9999.99998190599, -0.000605501004693584, 239.534042303138, 2 ),
      ( 600, 20.1677019088474, 99999.9999999986, -0.00607028177660669, 239.676052962034, 2 ),
      ( 600, 213.780483462206, 1000000.00009705, -0.0623429253317654, 241.190417147943, 2 ),
      ( 600, 3644.89511954475, 9999999.99999999, -0.450045128401961, 249.745201763164, 1 ),
      ( 600, 6704.77517698278, 99999999.9999999, 1.98970177901466, 249.647788777614, 1 ),
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
