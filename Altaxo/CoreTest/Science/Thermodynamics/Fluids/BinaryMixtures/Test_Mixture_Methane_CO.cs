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
	/// Tests and test data for <see cref="Mixture_Methane_CO"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Methane_CO : MixtureTestBase
    {

    public Test_Mixture_Methane_CO()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-82-8", 0.5), ("630-08-0", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 1.20298200301983, 999.999999836173, -0.00021597597682667, 20.7953945733992, 2 ),
      ( 100, 12.0532978467718, 9999.998294446, -0.00216338835704086, 20.8241374306121, 2 ),
      ( 100, 122.980166045347, 99999.9999205354, -0.0220193820643793, 21.1229393694136, 2 ),
      ( 100, 25265.4845628603, 999999.999999925, -0.952396630874037, 29.0686238439532, 1 ),
      ( 100, 26484.7454087394, 10000000.000023, -0.5458811594193, 29.5455279104202, 1 ),
      ( 100, 31472.764200499, 100000000.000001, 2.82146982751368, 33.0425929015478, 1 ),
      ( 150, 0.801868135563449, 999.9999999979, -6.65242700508129E-05, 20.7943552217577, 2 ),
      ( 150, 8.02348710131692, 9999.99997875257, -0.000665444155033264, 20.7994325908719, 2 ),
      ( 150, 80.720267587908, 99999.9999999982, -0.00667475978553168, 20.8505585842368, 2 ),
      ( 150, 861.207029541401, 1000000.00033379, -0.0689639490315856, 21.4022881882513, 2 ),
      ( 150, 16423.5021256946, 10000000.0000241, -0.511788176641953, 25.60121350025, 1 ),
      ( 150, 27936.6229372153, 100000000.000001, 1.87012067877069, 28.6164795232304, 1 ),
      ( 200, 0.601376810551668, 999.999999999928, -2.61344436658741E-05, 20.7964261989032, 2 ),
      ( 200, 6.015182797511, 9999.99999926671, -0.00026131516379018, 20.7982166050217, 2 ),
      ( 200, 60.2934881222728, 99999.9926018461, -0.00261021090344602, 20.8161730542673, 2 ),
      ( 200, 617.282553016996, 999999.999920381, -0.0257928221600283, 21.0006900775056, 2 ),
      ( 200, 7432.39987707778, 10000000, -0.190892438715597, 22.8383568601949, 1 ),
      ( 200, 24888.0998462444, 99999999.9999998, 1.41625956830141, 26.4427694871945, 1 ),
      ( 250, 0.481093952155766, 999.999999999997, -1.05531011367137E-05, 20.8043041164298, 2 ),
      ( 250, 4.81139631111981, 9999.99999996802, -0.000105491183985958, 20.805243194213, 2 ),
      ( 250, 48.1594997092576, 99999.9996886896, -0.00105092862534127, 20.8146473329449, 2 ),
      ( 250, 486.002473695147, 999999.999999976, -0.0101102336684744, 20.9099078097083, 2 ),
      ( 250, 5110.10639359701, 9999999.99999901, -0.0585540924833974, 21.8631312571144, 1 ),
      ( 250, 22324.3381839913, 100000000.000001, 1.15499725526746, 25.1692496943131, 1 ),
      ( 300, 0.400908761136563, 999.999985794467, -3.40526610772453E-06, 20.8326542158722, 2 ),
      ( 300, 4.00921036513841, 9999.99860235209, -3.40230997294043E-05, 20.8332695227968, 2 ),
      ( 300, 40.1042659277132, 99999.9999999998, -0.000337279185826324, 20.8394269565366, 2 ),
      ( 300, 402.146546644369, 999999.999999998, -0.00308134116286553, 20.9013903153323, 2 ),
      ( 300, 4026.61338066504, 10000000.0170496, -0.00435587415350044, 21.5179575686149, 1 ),
      ( 300, 20200.5939514253, 100000000, 0.984631723698372, 24.3656241993381, 1 ),
      ( 350, 0.343634837458858, 1000.00002369265, 2.0923924383701E-07, 20.9035196693364, 1 ),
      ( 350, 3.43634184818673, 10000.0024122542, 2.11266345491972E-06, 20.9039755310405, 1 ),
      ( 350, 34.3626955937447, 100000, 2.31497101604694E-05, 20.9085356223988, 1 ),
      ( 350, 343.487234800772, 1000000.00000561, 0.000429931555250877, 20.9542623530894, 1 ),
      ( 350, 3365.06367670404, 10000000.0088623, 0.0211839769692229, 21.4061144856281, 1 ),
      ( 350, 18441.0978418362, 100000000, 0.863418944734912, 23.8583212779763, 1 ),
      ( 400, 0.300679890758386, 1000.00034017522, 2.13956813934476E-06, 21.0348762104274, 1 ),
      ( 400, 3.00674109679741, 9999.99999999999, 2.14094652534168E-05, 21.0352385904066, 1 ),
      ( 400, 30.061577311854, 100000.000000016, 0.00021547050719563, 21.0388627510003, 1 ),
      ( 400, 299.993799633641, 1000000.0002757, 0.00228920503695795, 21.0751293609655, 1 ),
      ( 400, 2907.98998082925, 10000000.0052129, 0.0339806840484751, 21.4314724877374, 1 ),
      ( 400, 16970.4645333073, 100000000.013202, 0.771787368473655, 23.5667530471937, 1 ),
      ( 500, 0.240543513806013, 1000.00078764526, 3.76498354124268E-06, 21.4932199862813, 1 ),
      ( 500, 2.40535379870509, 10000, 3.76563764226224E-05, 21.4934768802686, 1 ),
      ( 500, 24.0453733882312, 100000.000000142, 0.000377218841800466, 21.496045508368, 1 ),
      ( 500, 239.625226386261, 1000000.00166431, 0.00383603674561668, 21.5216970923675, 1 ),
      ( 500, 2305.22310381668, 10000000.0000037, 0.0434757363096852, 21.7720595248892, 1 ),
      ( 500, 14661.7783352169, 100000000.005171, 0.640622522502123, 23.4504660645769, 1 ),
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
      ( 100, 1.20310344618306, 999.999981668913, -0.000319175624335459, 22.8809054164517, 2 ),
      ( 100, 12.0658333863489, 9999.99992674591, -0.00320234300629962, 22.9373804495257, 2 ),
      ( 100, 26764.0255925603, 1000000.00542549, -0.955062087083453, 32.0841754108162, 1 ),
      ( 100, 27404.1785552997, 10000000.0002319, -0.561118227834291, 32.528252819414, 1 ),
      ( 100, 30959.5988546295, 100000000.000299, 2.88480306393203, 35.6364134805772, 1 ),
      ( 150, 0.801892691620439, 999.99999974727, -9.94252302023255E-05, 22.8903667597696, 2 ),
      ( 150, 8.02611494015551, 9999.99739537133, -0.000994915420784151, 22.8997275464763, 2 ),
      ( 150, 80.992565184853, 99999.9999356342, -0.0100165841186059, 22.9951092525131, 2 ),
      ( 150, 899.184387191086, 999999.999999701, -0.108288606122558, 24.1705912663193, 2 ),
      ( 150, 21027.9065960518, 10000000.0000049, -0.618691019197388, 28.4592404696149, 1 ),
      ( 150, 27920.1946349162, 99999999.9996694, 1.8718029144771, 31.5021859469931, 1 ),
      ( 200, 0.601384914329785, 999.999999989061, -4.18897913251764E-05, 22.9955205498683, 2 ),
      ( 200, 6.01611767575882, 9999.99988958085, -0.00041894980360503, 22.9986662569495, 2 ),
      ( 200, 60.3892874934822, 99999.999999971, -0.00419470503765872, 23.0302816544658, 2 ),
      ( 200, 628.04095479242, 1000000.00140315, -0.0424832683053227, 23.3625331558228, 2 ),
      ( 200, 9986.56381228212, 10000000.0000209, -0.397831192245161, 26.898415370479, 1 ),
      ( 200, 25185.5208581416, 100000000.000101, 1.38772001511857, 29.3169060453682, 1 ),
      ( 250, 0.481097246447278, 999.999999999255, -1.96810403179863E-05, 23.3761728664846, 2 ),
      ( 250, 4.81182469993879, 9999.99999253096, -0.000196790296277086, 23.3776832905601, 2 ),
      ( 250, 48.203540327215, 100000, -0.00196588323599926, 23.3928089836235, 2 ),
      ( 250, 490.629037694753, 999999.9968864, -0.019446993511183, 23.5460760246365, 2 ),
      ( 250, 5692.25282176308, 9999999.99623886, -0.154837648581217, 25.0453220232374, 1 ),
      ( 250, 22760.1840345581, 100000000.000001, 1.1137253425654, 28.2671544308097, 1 ),
      ( 300, 0.400910212696845, 999.999999999944, -9.3064961889838E-06, 24.1445686094464, 2 ),
      ( 300, 4.00943785531575, 9999.99999944087, -9.30402351664306E-05, 24.1454563874778, 2 ),
      ( 300, 40.1278838994743, 99999.9946522406, -0.000927926696260556, 24.1543361171087, 2 ),
      ( 300, 404.559096871471, 999999.999996197, -0.00902863208916926, 24.2432738199028, 2 ),
      ( 300, 4270.24712813913, 9999999.99978675, -0.0611632779145356, 25.085858165837, 1 ),
      ( 300, 20655.3685266339, 100000000.00002, 0.940931148771986, 28.0663427402246, 1 ),
      ( 350, 0.343635461325873, 999.999999999998, -3.88265111117094E-06, 25.2715729246484, 2 ),
      ( 350, 3.43647462927198, 9999.99999996969, -3.88066823388005E-05, 25.2721630066386, 2 ),
      ( 350, 34.3766849976699, 99999.9997220318, -0.000386083960887807, 25.2780627220274, 2 ),
      ( 350, 344.897326029418, 999999.999999997, -0.0036625361304201, 25.3369289391912, 2 ),
      ( 350, 3494.2183614073, 10000000.0010471, -0.0165636729952744, 25.8932197524535, 1 ),
      ( 350, 18854.7599172222, 100000000.000269, 0.822532499053048, 28.4975065834266, 1 ),
      ( 400, 0.300680117569075, 999.992505066793, -8.35845391755266E-07, 26.6584507247822, 2 ),
      ( 400, 3.00682370054478, 9999.99999999977, -8.34380103158233E-06, 26.6588753264446, 2 ),
      ( 400, 30.0704509179888, 99999.9999982882, -8.19673750358495E-05, 26.6631200600217, 2 ),
      ( 400, 300.882503613342, 999999.999441193, -0.0006734934408316, 26.7054311906107, 2 ),
      ( 400, 2985.42258587057, 10000000.0006825, 0.00716013419378673, 27.1078111556473, 1 ),
      ( 400, 17321.6612623633, 100000000.00144, 0.735860415861688, 29.3634263368856, 1 ),
      ( 500, 0.240543388004971, 1000.01489257158, 2.04185620711808E-06, 29.834412044131, 1 ),
      ( 500, 2.40538975696124, 10000.0000000023, 2.04261298953881E-05, 29.8346665152991, 1 ),
      ( 500, 24.0494576164489, 100000.000024365, 0.000205047501676205, 29.8372105542752, 1 ),
      ( 500, 240.033049532353, 999999.999999999, 0.00212820461409595, 29.8625814596518, 1 ),
      ( 500, 2339.23976766944, 10000000.0000695, 0.0282994171914069, 30.1079121482857, 1 ),
      ( 500, 14890.0948469368, 100000000.000002, 0.615462436264962, 31.8186301729415, 1 ),
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
      ( 100, 1.2032847533037, 999.999811273737, -0.000472083788986204, 24.9701691892345, 2 ),
      ( 100, 12.0847059896372, 9999.99774809012, -0.00476130380252755, 25.0935272113543, 2 ),
      ( 150, 0.801927072754623, 999.999997595421, -0.000144574377250626, 24.9867309136325, 2 ),
      ( 150, 8.0297348689767, 9999.99999999995, -0.00144755993684255, 25.0035338512623, 2 ),
      ( 150, 81.3741231918562, 100000.000116957, -0.0146607997942875, 25.1811793010911, 2 ),
      ( 150, 968.252071686888, 999999.999277688, -0.17189835349373, 28.1528892738823, 2 ),
      ( 150, 23410.8184887267, 10000000.0000841, -0.657504014633372, 31.0643003432165, 1 ),
      ( 150, 27885.9284623709, 100000000.000393, 1.87532522265978, 33.9468058358128, 1 ),
      ( 200, 0.60139613866641, 999.99999988644, -6.28332617658305E-05, 25.194722525375, 2 ),
      ( 200, 6.01736578943217, 9999.99883991866, -0.000628560614607241, 25.2001064299854, 2 ),
      ( 200, 60.5176181082939, 99999.9999936171, -0.00630862579511418, 25.2546600412764, 2 ),
      ( 200, 643.594018246687, 1000000, -0.0656247044935988, 25.8736754772148, 2 ),
      ( 200, 16581.3068540454, 9999999.99997164, -0.637327530165364, 30.1257457978036, 1 ),
      ( 200, 25495.3524990513, 100000000.019385, 1.35869792781337, 32.0527097945004, 1 ),
      ( 250, 0.48110187036358, 999.999999990593, -3.15724718519151E-05, 25.948100665623, 2 ),
      ( 250, 4.81238631521564, 9999.99990517248, -0.000315749239249224, 25.9506089192574, 2 ),
      ( 250, 48.261170830577, 99999.9999999843, -0.00315994719598335, 25.9757819910348, 2 ),
      ( 250, 496.91144293167, 1000000.0001512, -0.0318462421641994, 26.2366048904056, 2 ),
      ( 250, 7003.79893188449, 9999999.99999273, -0.31310609361113, 29.1859852880228, 2 ),
      ( 250, 23281.1296693786, 99999999.9999993, 1.06642326906106, 31.3488358824289, 1 ),
      ( 300, 0.400912345159001, 999.999999998978, -1.69060281710985E-05, 27.4565264236987, 2 ),
      ( 300, 4.00973351843595, 9999.99998976382, -0.000169049937144475, 27.457941598041, 2 ),
      ( 300, 40.158402669815, 100000, -0.00168945806869749, 27.4721020382003, 2 ),
      ( 300, 407.74887395204, 999999.995694194, -0.0167831403993279, 27.614510759584, 2 ),
      ( 300, 4684.8892836843, 9999999.99999998, -0.14425818189599, 28.9869259169091, 2 ),
      ( 300, 21264.3830703734, 100000000.017378, 0.885338342745246, 31.8154002336618, 1 ),
      ( 350, 0.343636481300692, 999.99999999988, -9.1313950477119E-06, 29.6396647751862, 2 ),
      ( 350, 3.43664719745626, 9999.99999879986, -9.12992332102655E-05, 29.6405571222665, 2 ),
      ( 350, 34.3946856697048, 99999.9884140404, -0.000911516516011757, 29.6494765947068, 2 ),
      ( 350, 346.741363803696, 999999.999980962, -0.00896351202323996, 29.7382454797994, 2 ),
      ( 350, 3693.47841020326, 10000000.0014455, -0.0696213562026265, 30.5488363351597, 2 ),
      ( 350, 19463.0443241969, 99999999.9999999, 0.765568313444457, 33.2204922099344, 1 ),
      ( 400, 0.300680577600555, 999.999999999988, -4.66311414697074E-06, 32.2820639323318, 2 ),
      ( 400, 3.00693193252222, 9999.99999987625, -4.66181471233495E-05, 32.2826681178704, 2 ),
      ( 400, 30.0819020276044, 99999.9988365518, -0.000464880118244835, 32.288705097013, 2 ),
      ( 400, 302.043428253006, 999999.999999947, -0.00451674371369767, 32.3485801532825, 2 ),
      ( 400, 3099.93989571291, 9999999.99999722, -0.030048369942502, 32.8898628430052, 1 ),
      ( 400, 17879.4310817843, 100000000.003967, 0.681704379319653, 35.2613807505959, 1 ),
      ( 500, 0.240543395957968, 999.997148105413, -2.2646649604004E-07, 38.1756492708056, 2 ),
      ( 500, 2.40543883194335, 10000, -2.25655363810276E-06, 38.1759654217889, 2 ),
      ( 500, 24.0548573142655, 99999.9999999631, -2.17533965267576E-05, 38.1791243172002, 2 ),
      ( 500, 240.576050853938, 1000000.00111663, -0.00013596723229113, 38.2104532253754, 1 ),
      ( 500, 2388.60906710649, 10000000.0003522, 0.00704357069902409, 38.4989480546532, 1 ),
      ( 500, 15303.5423504692, 100000000.001564, 0.571814779136159, 40.2945939014711, 1 ),
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
