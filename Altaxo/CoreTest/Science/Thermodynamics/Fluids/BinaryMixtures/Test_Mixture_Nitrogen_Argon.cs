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
  /// Tests and test data for <see cref="Mixture_Nitrogen_Argon"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_Nitrogen_Argon : MixtureTestBase
  {

    public Test_Mixture_Nitrogen_Argon()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("7727-37-9", 0.5), ("7440-37-1", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 100, 1.20298115560089, 999.999988692858, -0.000219836464328665, 12.4832135508899, 2 ),
      ( 100, 12.0537188561511, 9999.99999999817, -0.00220279663910998, 12.5115778988073, 2 ),
      ( 100, 123.039228308864, 100000.001109712, -0.0224933034037491, 12.8147638828335, 2 ),
      ( 100, 50233.4168711376, 1000000000.00009, 22.942561971434, 34.9844734336243, 1 ),
      ( 150, 0.801866612100534, 999.999999908102, -6.91899588141839E-05, 12.4807462560323, 2 ),
      ( 150, 8.0236652864336, 9999.99845828699, -0.000692199331786864, 12.4867112713064, 2 ),
      ( 150, 80.7424598809027, 99999.9999850832, -0.00695231205334041, 12.5470188789455, 2 ),
      ( 150, 864.894322031227, 1000000, -0.0729374554112118, 13.2270365836556, 2 ),
      ( 150, 24142.0833598152, 9999999.9992708, -0.667878236113674, 17.2671197228857, 1 ),
      ( 150, 34122.2444751276, 100000000.000001, 1.34981943103814, 19.6635532597185, 1 ),
      ( 150, 48721.1542805579, 1000000000.00011, 15.4571456243741, 28.3199269563464, 1 ),
      ( 200, 0.601375689886946, 999.999999993431, -2.88366328074978E-05, 12.4803338360584, 2 ),
      ( 200, 6.01531823105319, 9999.99993380402, -0.000288388511641722, 12.4825624569115, 2 ),
      ( 200, 60.3098953176608, 99999.9999999932, -0.00288610173760884, 12.5048958696807, 2 ),
      ( 200, 619.369491642651, 1000000.00006542, -0.0290798039778693, 12.733075028505, 2 ),
      ( 200, 8452.63672367303, 9999999.99999912, -0.288555313689502, 15.2469625227567, 2 ),
      ( 200, 30360.985348506, 100000000.000001, 0.980694438385856, 17.6061970503211, 1 ),
      ( 200, 47309.2843446837, 1000000000.00012, 11.711212113371, 25.9936393127769, 1 ),
      ( 250, 0.481093075192631, 999.999999999581, -1.32959781851611E-05, 12.4802138933708, 2 ),
      ( 250, 4.81150647428391, 9999.99999579552, -0.000132949709426209, 12.4813247912334, 2 ),
      ( 250, 48.1726644468258, 100000, -0.00132848345796251, 12.4924333725364, 2 ),
      ( 250, 487.510529426304, 999999.999420988, -0.0131768453095401, 12.6034443515462, 2 ),
      ( 250, 5419.15861444298, 10000000, -0.112248389800918, 13.6455872613899, 2 ),
      ( 250, 27022.3447371312, 99999999.9999994, 0.780329143416405, 16.3521575433844, 1 ),
      ( 250, 46016.0532292762, 1000000000.00013, 9.4547575210899, 24.2218918925476, 1 ),
      ( 300, 0.400908003772437, 999.999999999974, -6.08189679851945E-06, 12.4801781414808, 2 ),
      ( 300, 4.00929944841696, 9999.99999971693, -6.08070079991203E-05, 12.4808374899147, 2 ),
      ( 300, 40.1149011323307, 99999.9972861445, -0.000606871308401009, 12.4874271249289, 2 ),
      ( 300, 403.303634286873, 999999.999999366, -0.00594606290941212, 12.5529251423037, 2 ),
      ( 300, 4195.30301418969, 10000000.0000837, -0.0443942567789038, 13.1530352097948, 2 ),
      ( 300, 24147.1415942492, 100000000.000269, 0.6602609626778, 15.5421787812229, 1 ),
      ( 300, 44828.629549856, 1000000000.00013, 7.94306985328443, 22.8294014841785, 1 ),
      ( 350, 0.343634161286775, 999.999999999998, -2.33786771845504E-06, 12.4801940238659, 2 ),
      ( 350, 3.43641372419893, 9999.99999998807, -2.33690434982671E-05, 12.4806335737786, 2 ),
      ( 350, 34.3713332935323, 99999.999890538, -0.000232726164093683, 12.4850261900437, 2 ),
      ( 350, 344.401299235641, 999999.999999999, -0.00222983300898401, 12.5286598374046, 2 ),
      ( 350, 3476.96815172068, 10000000.0000289, -0.0116868292864378, 12.9317508837842, 1 ),
      ( 350, 21725.9693274121, 100000000.000293, 0.581670933396218, 14.987455749126, 1 ),
      ( 350, 43730.4737265258, 1000000000.00013, 6.85798351965489, 21.7303216443639, 1 ),
      ( 400, 0.30067925147627, 999.996972617229, -2.5223447890293E-07, 12.4802637441967, 2 ),
      ( 400, 3.00679930361703, 9999.99999999998, -2.51511034799791E-06, 12.4805816191425, 2 ),
      ( 400, 30.0686518864218, 99999.9999999416, -2.44265880173739E-05, 12.4837585126304, 2 ),
      ( 400, 300.730753605131, 999999.999889286, -0.000171513840906367, 12.5153406433666, 2 ),
      ( 400, 2989.54129586362, 10000000.00008, 0.00577026493849505, 12.8113845868736, 1 ),
      ( 400, 19706.6356860878, 100000000.000931, 0.525776286269495, 14.5874748127212, 1 ),
      ( 400, 42707.8867314191, 1000000000.00012, 6.04036647866526, 20.8503790650507, 1 ),
      ( 500, 0.240542925149311, 1000.01642249853, 1.68797821192263E-06, 12.4805807239004, 1 ),
      ( 500, 2.40539278141632, 10000.0000000023, 1.68835343594474E-05, 12.4807761160945, 1 ),
      ( 500, 24.0502637068208, 100000.00002404, 0.000169238171759763, 12.4827293085398, 1 ),
      ( 500, 240.127294337001, 999999.999999998, 0.00173260169634487, 12.5021877738095, 1 ),
      ( 500, 2355.35699089877, 10000000.0000167, 0.0212606421204154, 12.6893303848725, 1 ),
      ( 500, 16604.546232968, 100000000.000002, 0.448659517218285, 14.0565281261062, 1 ),
      ( 500, 40849.6569663945, 1000000000.0001, 4.8885032863971, 19.5379167253608, 1 ),
      ( 600, 0.200452295703749, 1000.0194881629, 2.38211859507456E-06, 12.4811031069533, 1 ),
      ( 600, 2.00448007462649, 10000.0000000046, 2.38230505648658E-05, 12.4812406170168, 1 ),
      ( 600, 20.0404993532292, 100000.00004653, 0.000238463186736019, 12.4826154225796, 1 ),
      ( 600, 199.971289943418, 999999.999999997, 0.0024078096500122, 12.4963336092595, 1 ),
      ( 600, 1953.15832746296, 10000000.0000031, 0.0263007352099523, 12.630481945169, 1 ),
      ( 600, 14370.7311364934, 100000000, 0.394868367112038, 13.7256700009536, 1 ),
      ( 600, 39194.7295300603, 1000000000.0001, 4.11427901529306, 18.6061131667253, 1 ),
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
      ( 100, 1.20296420972226, 999.999975826554, -0.000205752789878856, 16.6332584317516, 2 ),
      ( 100, 12.05200860393, 9999.99999999129, -0.00206120323882063, 16.6583241442922, 2 ),
      ( 100, 122.850883587802, 100000.000448876, -0.0209946715635977, 16.9230930454669, 2 ),
      ( 100, 46901.6879313131, 999999999.999991, 24.6433563379505, 34.7751625659239, 1 ),
      ( 150, 0.801861904991904, 999.999999684905, -6.33201414211785E-05, 16.6319684148313, 2 ),
      ( 150, 8.02319335878284, 9999.99676087236, -0.000633419522846104, 16.6373882987054, 2 ),
      ( 150, 80.6940218167728, 99999.9999402259, -0.00635621700921275, 16.6920768410616, 2 ),
      ( 150, 858.444330324174, 999999.997758688, -0.0659718949830954, 17.2929493724097, 2 ),
      ( 150, 18967.7981171743, 10000000.0000003, -0.577277696636517, 21.4634468181526, 1 ),
      ( 150, 30876.1158243186, 100000000.008375, 1.59686527800661, 23.8275980893218, 1 ),
      ( 150, 45445.1079768548, 999999999.999998, 16.6435081063372, 32.4580555064374, 1 ),
      ( 200, 0.601373492784259, 999.999999988614, -2.51832637164317E-05, 16.6326267872595, 2 ),
      ( 200, 6.01509828016659, 9999.99988554417, -0.000251832590043053, 16.6346935921451, 2 ),
      ( 200, 60.2876577269058, 99999.9999999832, -0.00251830820334835, 16.6553875060456, 2 ),
      ( 200, 616.883209053377, 1000000.00000068, -0.0251666127210903, 16.8648498105263, 2 ),
      ( 200, 7677.85516018923, 10000000, -0.216762577972132, 18.8733400022629, 1 ),
      ( 200, 27478.5722033152, 99999999.9999992, 1.18846286403644, 21.7710511338128, 1 ),
      ( 200, 44061.806126671, 999999999.999998, 12.6480639606143, 30.7251580490327, 1 ),
      ( 250, 0.481091795310876, 999.999999999439, -1.06356444402841E-05, 16.6346119415198, 2 ),
      ( 250, 4.81137841625433, 9999.99999441012, -0.000106337584325789, 16.6356649684896, 2 ),
      ( 250, 48.1597886433433, 99999.9999999999, -0.00106148273927727, 16.6461899177548, 2 ),
      ( 250, 486.151511188627, 999999.999949692, -0.0104182183582987, 16.7508639164131, 2 ),
      ( 250, 5207.48207288946, 10000000, -0.0761625832680794, 17.6858958015201, 1 ),
      ( 250, 24551.861761166, 100000000.000381, 0.959471274600282, 20.5378596638006, 1 ),
      ( 250, 42810.4477433615, 999999999.999998, 10.2375997904436, 29.122538622482, 1 ),
      ( 300, 0.400907164045167, 999.999999999979, -3.98734165588553E-06, 16.6417741240479, 2 ),
      ( 300, 4.00921545119764, 9999.99999979318, -3.98572454552382E-05, 16.6424131133251, 2 ),
      ( 300, 40.106476939972, 99999.9981974128, -0.000396953105019545, 16.6487975936288, 2 ),
      ( 300, 402.436979388701, 999999.999999999, -0.00380535083150387, 16.7120886579007, 2 ),
      ( 300, 4088.93160112942, 10000000, -0.0195346740928691, 17.2787152745876, 1 ),
      ( 300, 22076.1563077727, 100000000.000038, 0.816011627668297, 19.7468605389847, 1 ),
      ( 300, 41680.3874443631, 1000000000, 8.61856619078887, 27.7830088524032, 1 ),
      ( 350, 0.343633556164061, 999.986489024652, -6.1143930805113E-07, 16.6619042119094, 2 ),
      ( 350, 3.4363543891186, 9999.99999999945, -6.10257822691945E-06, 16.6623392000856, 2 ),
      ( 350, 34.3653904581994, 99999.9999972941, -5.98355846004121E-05, 16.6666855733945, 2 ),
      ( 350, 343.797979094608, 1000000, -0.000478877872776998, 16.7097955172336, 2 ),
      ( 350, 3410.71637747645, 10000000, 0.0075107508762232, 17.1031529110415, 1 ),
      ( 350, 20003.7299194982, 100000000.006415, 0.717846337881278, 19.2188686671732, 1 ),
      ( 350, 40652.1026828421, 1000000000, 7.45302749849519, 26.698903642555, 1 ),
      ( 400, 0.300678801715049, 1000, 1.21425352637981E-06, 16.7035120233788, 1 ),
      ( 400, 3.00675520582769, 10000.0000000035, 1.21510913926384E-05, 16.7038326273648, 1 ),
      ( 400, 30.0642385597774, 100000.000041944, 0.000122366381132225, 16.7070365187496, 1 ),
      ( 400, 300.286071256557, 1000000, 0.0013090945580486, 16.7388594285744, 1 ),
      ( 400, 2943.53978536118, 10000000.0033272, 0.0214883984814339, 17.0349706178206, 1 ),
      ( 400, 18271.2520461225, 100000000.000075, 0.645640776886007, 18.865102833738, 1 ),
      ( 400, 39708.2941622789, 999999999.999999, 6.57220073190981, 25.832137024111, 1 ),
      ( 500, 0.240542649789216, 1000, 2.81025145384416E-06, 16.8687938206003, 1 ),
      ( 500, 2.40536578520086, 10000.000000028, 2.81070544637392E-05, 16.8689972233982, 1 ),
      ( 500, 24.047563960831, 100000.000290507, 0.000281524094478215, 16.8710304141149, 1 ),
      ( 500, 239.857272334723, 1000000.00000002, 0.00286031335281632, 16.891278314521, 1 ),
      ( 500, 2328.93180100484, 10000000.0037281, 0.0328483607560066, 17.0852678394486, 1 ),
      ( 500, 15580.9542462525, 100000000.000018, 0.543829315541905, 18.5048017961599, 1 ),
      ( 500, 38022.072869114, 1000000000, 5.32641308439015, 24.6045447172815, 1 ),
      ( 600, 0.20045211084631, 1000, 3.28619418052636E-06, 17.1331949942964, 1 ),
      ( 600, 2.00446195183378, 10000.0000000317, 3.28644915526225E-05, 17.1333416714782, 1 ),
      ( 600, 20.038687561013, 100000.000321349, 0.000328899462831116, 17.1348080868288, 1 ),
      ( 600, 199.790628776636, 1000000.00000002, 0.0033142393692264, 17.1494364033808, 1 ),
      ( 600, 1935.84759850164, 10000000.0008213, 0.03547811770627, 17.2920239372584, 1 ),
      ( 600, 13609.0393330197, 100000000, 0.472938521525955, 18.4398946616498, 1 ),
      ( 600, 36543.7781911192, 1000000000, 4.48527800539187, 23.8550320953062, 1 ),
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
      ( 100, 1.20294851839319, 999.999962192199, -0.000192711414398719, 20.783377298617, 2 ),
      ( 100, 12.0504262904521, 9999.99999997839, -0.00193016621437828, 20.8058272552381, 2 ),
      ( 100, 122.67832752326, 100000.000153122, -0.019617628705913, 21.0411471544563, 2 ),
      ( 100, 24666.1534541105, 1000000.00922311, -0.95124020007252, 27.5380433246891, 1 ),
      ( 100, 26194.4439820299, 10000000.0090083, -0.540850457323264, 27.9967652743845, 1 ),
      ( 100, 31798.7612255735, 99999999.999999, 2.78227531552248, 31.8774179463847, 1 ),
      ( 100, 44139.8412440765, 1000000000.00675, 26.2478709162132, 35.9513790530513, 1 ),
      ( 150, 0.8018572846444, 999.999999549424, -5.75584491148523E-05, 20.7832015683235, 2 ),
      ( 150, 8.02273022702394, 9999.99537288295, -0.000575728577239265, 20.7881756279811, 2 ),
      ( 150, 80.6465895541491, 99999.9998958729, -0.00577180578688107, 20.8382758277173, 2 ),
      ( 150, 852.313618551323, 1000000.00444221, -0.0592534094264788, 21.3771019356872, 2 ),
      ( 150, 14969.5188695721, 10000000.000009, -0.464370807125475, 25.2916729196258, 1 ),
      ( 150, 28195.2356715794, 100000000.003954, 1.84378233382482, 28.0053271868695, 1 ),
      ( 150, 42677.3406093294, 1000000000.00001, 17.7877482414459, 36.8911568074278, 1 ),
      ( 200, 0.601371275625492, 999.999999986453, -2.14965180710446E-05, 20.7849236889492, 2 ),
      ( 200, 6.01487634647642, 9999.99986465351, -0.000214944416886653, 20.7868642693421, 2 ),
      ( 200, 60.2652452870634, 99999.9999999826, -0.0021473481544649, 20.8062783593419, 2 ),
      ( 200, 614.409892826044, 999999.999798661, -0.0212424063135133, 21.0010953946301, 2 ),
      ( 200, 7120.3714873742, 10000000, -0.155439643418433, 22.7038318775753, 1 ),
      ( 200, 25124.880480905, 100000000, 1.39347744835654, 25.9662645901584, 1 ),
      ( 200, 41293.0218238941, 1000000000, 13.5631954668195, 35.7110379172539, 1 ),
      ( 250, 0.481090486122478, 999.999999999557, -7.91437991271531E-06, 20.7890119088639, 2 ),
      ( 250, 4.81124743594828, 9999.99999562159, -7.91167066747537E-05, 20.7900243454593, 2 ),
      ( 250, 48.1466291403033, 99999.9999999999, -0.000788451486982956, 20.8001392439059, 2 ),
      ( 250, 484.774077291754, 999999.999998923, -0.00760642714798614, 20.9002977177346, 2 ),
      ( 250, 5019.0966161076, 10000000.0032627, -0.0414875118242015, 21.7622180695577, 1 ),
      ( 250, 22541.7477050189, 100000000, 1.13420310121829, 24.7495744081188, 1 ),
      ( 250, 40065.9830202075, 999999999.999994, 11.0073599179399, 34.2437280625579, 1 ),
      ( 300, 0.400906313704742, 999.999999999995, -1.8297196570949E-06, 20.8033712039709, 2 ),
      ( 300, 4.00912893017085, 9999.99999996287, -1.82771017851967E-05, 20.8039997121724, 2 ),
      ( 300, 40.0978046508987, 99999.9997846977, -0.000180760563156622, 20.8102780076786, 2 ),
      ( 300, 401.550243753277, 999999.99999998, -0.00160547346652489, 20.8723713411977, 2 ),
      ( 300, 3988.46968389357, 10000000.0000338, 0.00516137081429431, 21.4181511469263, 1 ),
      ( 300, 20382.2033246243, 100000000.000028, 0.966939290645991, 23.9721702305698, 1 ),
      ( 300, 38980.8902670651, 999999999.999999, 9.2846693019307, 32.917149146641, 1 ),
      ( 350, 0.343632930432276, 1000, 1.1737767175806E-06, 20.8436151102832, 1 ),
      ( 350, 3.4362930360398, 10000.0000000081, 1.17517501864678E-05, 20.8440519169552, 1 ),
      ( 350, 34.3592483437472, 100000.00012168, 0.000118915326325037, 20.8484158940813, 1 ),
      ( 350, 343.177425423613, 1000000.00000009, 0.00132851519949499, 20.8916435294234, 1 ),
      ( 350, 3346.40848772003, 10000000.0000017, 0.0268720722849919, 21.2819200806671, 1 ),
      ( 350, 18578.8804982994, 100000000.000011, 0.849591216656924, 23.4670509995388, 1 ),
      ( 350, 38011.7976835592, 999999999.999994, 8.04017601879409, 31.8124676721077, 1 ),
      ( 400, 0.300678335636651, 1000, 2.73395051010014E-06, 20.9267608085558, 1 ),
      ( 400, 3.00670950992466, 10000.0000000772, 2.73492534146542E-05, 20.9270886964408, 1 ),
      ( 400, 30.059667041434, 100000.000866439, 0.000274466462855924, 20.9303651166179, 1 ),
      ( 400, 299.827267184199, 1000000.00000063, 0.00284132574261622, 20.9628822064726, 1 ),
      ( 400, 2898.18326386908, 10000000.0000004, 0.0374746754871957, 21.2633079008565, 1 ),
      ( 400, 17066.5124490551, 99999999.9999999, 0.7618079558788, 23.1563002467058, 1 ),
      ( 400, 37136.2320510054, 1000000000, 7.09665271655794, 30.930444673792, 1 ),
      ( 500, 0.240542363923213, 1000.00000000001, 3.97537458388039E-06, 21.2570072390391, 1 ),
      ( 500, 2.40533775977078, 10000.0000001545, 3.97587311081775E-05, 21.2572215441728, 1 ),
      ( 500, 24.0447620723164, 100000.001597838, 0.000398084925423924, 21.259363618918, 1 ),
      ( 500, 239.577835272658, 1000000.0000009, 0.00403002229997937, 21.2806861755787, 1 ),
      ( 500, 2302.43021284025, 10000000.0101809, 0.0447367219422191, 21.4839657391773, 1 ),
      ( 500, 14695.9954566826, 100000000.000061, 0.636795139219243, 22.9620112692961, 1 ),
      ( 500, 35599.6105070471, 1000000000, 5.75690929953245, 29.7489732059434, 1 ),
      ( 600, 0.200451918889208, 1000.00000000002, 4.2250417384052E-06, 21.7852871267783, 1 ),
      ( 600, 2.00444313341945, 10000.0000001312, 4.2253150410897E-05, 21.7854451740156, 1 ),
      ( 600, 20.0368066312143, 100000.001330679, 0.000422804073558774, 21.7870251982262, 1 ),
      ( 600, 199.603485073076, 1000000.00000048, 0.00425492405132033, 21.8027802543956, 1 ),
      ( 600, 1918.33404651745, 10000000.0090703, 0.044931580700464, 21.9556194001002, 1 ),
      ( 600, 12933.4725648743, 100000000.000001, 0.549875965176369, 23.1604666379804, 1 ),
      ( 600, 34274.2429102895, 999999999.999997, 4.84849629706881, 29.1586680447047, 1 ),
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
