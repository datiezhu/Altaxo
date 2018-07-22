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
	/// Tests and test data for <see cref="MethylOctadecanoate"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_MethylOctadecanoate : FluidTestBase
    {

    public Test_MethylOctadecanoate()
      {
      _fluid = MethylOctadecanoate.Instance;

    _testDataMolecularWeight = 0.29850382;

    _testDataTriplePointTemperature = 311.84;

    _testDataTriplePointPressure = 0.006011;

    _testDataTriplePointLiquidMoleDensity = 2851.43273206711;

    _testDataTriplePointVaporMoleDensity = 2.31833639285455E-06;

    _testDataCriticalPointTemperature = 775;

    _testDataCriticalPointPressure = 1238999.35574745;

    _testDataCriticalPointMoleDensity = 794.3;

    _testDataNormalBoilingPointTemperature = 629.55700065929;

    _testDataNormalSublimationPointTemperature = null;

    _testDataIsMeltingCurveImplemented = false;

    _testDataIsSublimationCurveImplemented = false;

      // TestData contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. Internal energy (J/mol)
      // 4. Enthalpy (J/mol)
      // 5. Entropy (J/mol K)
      // 6. Isochoric heat capacity (J/(mol K))
      // 7. Isobaric heat capacity (J/(mol K))
      // 8. Speed of sound (m/s)
      _testDataEquationOfState = new (double temperature, double moleDensity, double pressure, double internalEnergy, double enthalpy, double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity, double speedOfSound)[]
      {
      ( 325, 3.33672179921317E-06, 0.00901650001942525, -139217.919211705, -136515.716102788, -187.47353685346, 483.263892783303, 491.578370771034, 95.9595568371573 ),
      ( 375, 2.89182541104075E-06, 0.0090165000103602, -113572.426930792, -110454.500109921, -112.985903683971, 541.875628102803, 550.190103353337, 102.982772051733 ),
      ( 425, 2.551610594135E-06, 0.00901650000601826, -85094.4703756372, -81560.8198935679, -40.7289122312276, 596.639183631264, 604.95365743292, 109.557473624178 ),
      ( 475, 2.28301997667699E-06, 0.00901650000380337, -53966.0119117393, -50016.6377950366, 29.388630439289, 647.944746754725, 656.259219805578, 115.759837630401 ),
      ( 525, 2.06558948841169E-06, 0.00901650000257978, -20354.4026337445, -15989.3048961911, 97.4599948629606, 695.98078321214, 704.295255870456, 121.646829326185 ),
      ( 575, 1.88597300331908E-06, 0.00901650000184491, 15578.2383797418, 20359.0597308306, 163.56259946747, 740.785824067502, 749.100296512596, 127.262356824556 ),
      ( 625, 1.73509515838292E-06, 0.00901650000136899, 53670.2080937369, 58866.7530542866, 227.755182421199, 782.353600520584, 790.668072842895, 132.64105108418 ),
      ( 675, 1.60656958811789E-06, 0.00901650000104137, 93759.937215792, 99372.2057833829, 290.083088199562, 820.70360168572, 829.018073932316, 137.810752664995 ),
      ( 725, 1.49577168348937E-06, 0.00901650000080498, 135688.128237508, 141716.120410599, 350.585151615922, 855.909053102923, 864.223525299501, 142.794218645989 ),
      ( 775, 1.39927028315758E-06, 0.00901650000062847, 179300.533428915, 185744.249206458, 409.299149219502, 888.097368656732, 896.411840818152, 147.610326653757 ),
      ( 825, 1.31446602256679E-06, 0.00901650000049324, 224450.388786659, 231309.828167896, 466.265117095729, 917.437985577241, 925.75245771265, 152.274941746542 ),
      ( 875, 1.23935367767959E-06, 0.00901650000038763, 271000.131563494, 278275.294547856, 521.526847388188, 944.126986107055, 952.441458222432, 156.801551834955 ),
      ( 925, 1.17236158643881E-06, 0.00901650000030385, 318822.372290713, 326513.258877753, 575.132138101366, 968.373125819187, 976.687597918663, 161.201741348606 ),
      ( 975, 1.11224047901922E-06, 0.00901650000023665, 367800.242064136, 375906.852253497, 627.132317554265, 990.386932664665, 998.701404751227, 165.485550044851 ),
      ( 311.84, 2851.43292516756, 86.2996226939457, -244795.286288327, -244795.256022975, -530.909074567074, 541.561098566649, 651.133299946456, 1341.6335823541 ),
      ( 325, 2818.47504629851, 86.2996247042832, -236174.24130366, -236174.210684399, -503.831914654511, 553.541455385236, 659.150412296302, 1292.9552518857 ),
      ( 400, 2638.2480897784, 86.2996390151843, -184808.917111284, -184808.884400319, -361.816549466341, 620.868266771662, 712.193665697834, 1046.69017875339 ),
      ( 422.638875578281, 2584.79182158172, 86.2996817218269, -168488.754421608, -168488.721034129, -322.133352453273, 640.78515078417, 729.663266414015, 979.147711809086 ),
      ( 425, 0.0244300352639919, 86.2996241440135, -85098.1356782162, -81565.614205715, -116.952346454926, 596.663664887894, 604.995403116185, 109.523995747251 ),
      ( 475, 0.0218558809885907, 86.2996241440131, -53968.4009766853, -50019.82419824, -46.8312088781985, 647.958840967439, 656.283378863597, 115.737336117442 ),
      ( 525, 0.0197730836378967, 86.2996241440132, -20356.0567030337, -15991.5566723651, 21.2420346434771, 695.988677769118, 704.309453909977, 121.630707516354 ),
      ( 575, 0.0180529806431798, 86.2996241440128, 15577.0135976299, 20357.3668173967, 87.3456598378834, 740.790262164079, 749.108995760445, 127.250252676626 ),
      ( 625, 0.0166083225677315, 86.2996241440129, 53669.2440864507, 58865.4114773517, 151.538830452062, 782.356161467911, 790.673719146979, 132.631669929845 ),
      ( 725, 0.0143171030305606, 86.2996978887392, 135687.446238628, 141715.180879116, 274.369394276728, 855.910039340476, 864.226393093063, 142.788272670101 ),
      ( 775, 0.0133932909694552, 86.2996817182533, 179299.93478825, 185743.435636179, 333.083561688629, 888.098041592623, 896.414058746698, 147.605529785306 ),
      ( 825, 0.0125814842442937, 86.2996693290443, 224449.854218376, 231309.114036485, 390.049655237413, 917.438476525455, 925.754244658287, 152.271061782829 ),
      ( 875, 0.0118624744430947, 86.2996596517298, 270999.648472911, 278274.661798816, 445.311482320681, 944.127366686923, 952.442943059274, 156.798416874495 ),
      ( 925, 0.0112212087262853, 86.2996519791564, 318821.931980152, 326512.694542745, 498.916849865338, 968.373435925757, 976.688860082223, 161.19922014385 ),
      ( 975, 0.0106457233976097, 86.2996458237229, 367799.838202259, 375906.346571911, 550.917091705413, 990.387195124841, 998.702495669681, 165.483540001804 ),
      ( 311.84, 2851.43496976259, 999.999999849148, -244795.374893131, -244795.024192516, -530.909358702979, 541.561483788559, 651.133279039635, 1341.63683710451 ),
      ( 325, 2818.47722661715, 999.99999864852, -236174.334080258, -236173.97927874, -503.832200121425, 553.541838635659, 659.150368440136, 1292.958719519 ),
      ( 375, 2697.43392507979, 1000.00078202025, -202378.795953442, -202378.425230448, -407.167435373625, 598.644768221362, 693.510288796812, 1124.27588929743 ),
      ( 400, 2638.25129467456, 1000.00001401861, -184809.041081447, -184808.662042492, -361.816859392374, 620.868664626842, 712.193455618533, 1046.69521392289 ),
      ( 425, 2579.2113355882, 1000.00006441636, -166763.887772044, -166763.500056602, -318.063530758319, 642.850869601364, 731.509460262498, 972.235662961001 ),
      ( 450, 2519.79358947994, 1000.00018475569, -148229.690692152, -148229.293834176, -275.693484206862, 664.571737601962, 751.294530418761, 900.263994921339 ),
      ( 472.113399265869, 2466.50124934768, 1000.0003444937, -131419.667300703, -131419.261867969, -239.230112760219, 683.543779536722, 769.094875573988, 838.317483504739 ),
      ( 475, 0.253799729656636, 1000.0000000026, -53993.7636611314, -50053.6491410903, -67.2545010486736, 648.108522253126, 656.540606856162, 115.498277034612 ),
      ( 525, 0.229454521957063, 1000.00000000081, -20373.6011460047, -16015.4390515225, 0.838732617964401, 696.072447058712, 704.460363007899, 121.459645589274 ),
      ( 575, 0.209406898374906, 1000.0000000003, 15564.0296101705, 20339.4214800696, 66.9532000735441, 740.837329540561, 749.201363945442, 127.121918104025 ),
      ( 625, 0.192597805910326, 1000.00000000012, 53659.0282160492, 58851.1953608074, 131.152608375803, 782.383310843719, 790.733630603643, 132.532253593741 ),
      ( 675, 0.178295297350477, 1000.00000000005, 93750.7015099941, 99359.3743962696, 193.484720490794, 820.721509829085, 829.063387404321, 137.724705226234 ),
      ( 725, 0.16597490649302, 1000.00000000003, 135680.221821114, 141705.228981109, 253.989561559006, 855.920489261494, 864.256796436686, 142.725294585431 ),
      ( 775, 0.15524997624329, 1000.00000000001, 179293.594125315, 185734.818943494, 312.705510894422, 888.105170476898, 896.437565775968, 147.554730991796 ),
      ( 825, 0.145828748025105, 1000.00000000001, 224444.192675504, 231301.551142491, 369.672922412384, 917.443676679513, 925.773179675638, 152.229978144723 ),
      ( 875, 0.137486739253674, 1000, 270994.53244605, 278267.961181274, 424.935764221378, 944.131397329395, 952.458674134045, 156.765225184264 ),
      ( 925, 0.130048322060522, 1000, 318817.269225085, 326506.718686298, 478.541937130761, 968.376719908701, 976.70223025095, 161.172528960464 ),
      ( 975, 0.123374077467734, 1000, 367795.561585089, 375900.992000661, 530.54283293198, 990.389974362935, 998.714050608929, 165.462262010963 ),
      ( 311.84, 2851.4551085271, 10000.0000021939, -244796.247617732, -244792.740636341, -530.912157376091, 541.565278136508, 651.133073279773, 1341.66889557642 ),
      ( 325, 2818.49870216488, 9999.9999986024, -236175.247892522, -236171.699904372, -503.835011897218, 553.545613554351, 659.149936661595, 1292.99287472039 ),
      ( 350, 2757.33056723257, 9999.99608857597, -219492.80593023, -219489.179235554, -454.390278433901, 576.197238438337, 675.702417506253, 1205.94132929219 ),
      ( 375, 2697.46155704867, 10000.0007840677, -202379.898583192, -202376.191393832, -407.170375775323, 598.648600715391, 693.508853601579, 1124.31955794437 ),
      ( 400, 2638.2828612739, 10000.0000127909, -184810.262106977, -184806.471762825, -361.819912018556, 620.872583310217, 712.191386901507, 1046.74480756187 ),
      ( 425, 2579.24762933368, 10000.0000660343, -166765.247913401, -166761.370813756, -318.066731161524, 642.854898825294, 731.506618536066, 972.292163706807 ),
      ( 450, 2519.83564157162, 10000.0001854266, -148231.215794542, -148227.247281668, -275.69687340423, 664.575892644477, 751.290717984264, 900.328568234328 ),
      ( 475, 2459.52190758543, 10000.0003656869, -129197.943534526, -129193.877703547, -234.538523413146, 686.00659896834, 771.4309358148, 830.410862951406 ),
      ( 500, 2397.74630942299, 10000.0005374795, -109657.530293505, -109653.359710279, -194.451099175454, 707.111611797293, 791.853227875516, 762.227399008592 ),
      ( 525, 2333.88107754219, 10000.000580969, -89603.4766802447, -89599.1919715347, -155.317353172792, 727.854488849436, 812.519780855253, 695.567968305524 ),
      ( 535.734475770978, 2305.639897541, 10000.0005352757, -80833.5712616069, -80829.2340706405, -138.781461317743, 736.641744617508, 821.467639077025, 667.380185505056 ),
      ( 550, 2.21668898508042, 10000.0000051479, -2834.05538891148, 1677.17738774488, 14.7215041463514, 719.484786824173, 728.424102454412, 122.848108314143 ),
      ( 600, 2.02439387260773, 10000.0000019396, 34238.1419906849, 39177.8921733202, 79.9543428748939, 762.368585105433, 771.114254552245, 128.735457534895 ),
      ( 650, 1.86405916983972, 10000.000000809, 73373.6789195837, 78738.3154947568, 143.263624274833, 802.15895981159, 810.791857494542, 134.279091632211 ),
      ( 700, 1.72791613637637, 10000.0000003607, 114418.50198047, 120205.819888926, 204.708322779042, 838.835586586512, 847.397532904238, 139.552554393514 ),
      ( 750, 1.61067963105149, 10000.0000001678, 157219.060707626, 163427.620006063, 264.334923370952, 872.464711252281, 880.978757666203, 144.602329148927 ),
      ( 800, 1.50856922709379, 10000.00000008, 201626.503807715, 208255.301360045, 322.186519431596, 903.178164232608, 911.657886523369, 149.460463954429 ),
      ( 850, 1.41878210144768, 10000.0000000386, 247499.559744675, 254547.858415651, 378.307335061053, 931.150916738579, 939.604848300471, 154.15062112786 ),
      ( 900, 1.33918376910813, 10000.0000000186, 294706.276948237, 302173.512013876, 432.744553193642, 956.581847922827, 965.015684684304, 158.691182747508 ),
      ( 950, 1.26811422863671, 10000.0000000088, 343124.873417884, 351010.598358262, 485.548641722856, 979.679028305058, 988.096775333683, 163.09696352084 ),
      ( 1000, 1.20426019811511, 10000.0000000041, 392643.941761933, 400947.795045182, 536.772913643007, 1000.64933317242, 1009.05392997276, 167.380225155296 ),
      ( 311.84, 2851.65643515873, 99999.9999988619, -244804.971101657, -244769.903763683, -530.940135972665, 541.603210688593, 651.131032989351, 1341.9893802623 ),
      ( 325, 2818.71338644948, 100000.000001404, -236184.381779767, -236148.90460054, -503.863120738293, 553.583350814821, 659.145639540636, 1293.33430884273 ),
      ( 350, 2757.57360210127, 99999.9948823594, -219502.812312982, -219466.548550229, -454.418873121109, 576.235031777758, 675.693451748325, 1206.32686628086 ),
      ( 375, 2697.73774590793, 100000.000793184, -202390.918088666, -202353.84999302, -407.199766688948, 598.686908291498, 693.494542251287, 1124.75602269787 ),
      ( 400, 2638.59834691762, 100000.000014864, -184822.463628944, -184784.564719421, -361.850422053816, 620.911748944422, 712.170757138662, 1047.24043837534 ),
      ( 425, 2579.61031480753, 100000.000065743, -166778.837960282, -166740.072415156, -318.098714795283, 642.895165175373, 731.478283355211, 972.856748524215 ),
      ( 450, 2520.2558050121, 100000.000187217, -148246.451815477, -148206.773303483, -275.730739312789, 664.617411566147, 751.252712759743, 900.973714381811 ),
      ( 475, 2460.01311038594, 100000.000367574, -129215.152862009, -129174.502671997, -234.574762973611, 686.049430912591, 771.38047566972, 831.150396139209 ),
      ( 500, 2398.32675808006, 100000.000541822, -109677.137488067, -109635.441751569, -194.490324663363, 707.155706263583, 791.786331905065, 763.077896016499 ),
      ( 525, 2334.57570645911, 100000.000588806, -89626.0439703242, -89583.2096341697, -155.360351836486, 727.899634512575, 812.430669243058, 696.549442316977 ),
      ( 550, 2268.03580498455, 100000.000426522, -69056.021516678, -69011.9305017978, -117.085068568634, 748.249302097092, 833.314071098702, 631.44714617926 ),
      ( 575, 2197.83050275642, 100000.000153852, -47960.5821624466, -47915.0827483524, -79.5768723721408, 768.182813783759, 854.490997234597, 567.733217739809 ),
      ( 600, 2122.85717551296, 99999.9994292011, -26330.906117887, -26283.7997931739, -42.7553711115535, 787.692982848889, 876.102860720801, 505.425220055651 ),
      ( 625, 2041.65556146526, 99999.9999943563, -4152.78357358899, -4103.80371533975, -6.54120734445027, 806.791607690961, 898.463180775993, 444.512387164909 ),
      ( 627.914350091379, 2031.69719159471, 99999.9999994805, -1530.68470132015, -1481.46476814645, -2.35522185105622, 808.99258553245, 901.144732705973, 437.495069036366 ),
      ( 650, 20.048713690965, 100000, 72386.09263552, 77373.9438035012, 122.58297221149, 804.491549450233, 816.868867073738, 124.86010590737 ),
      ( 700, 18.2381404242601, 100000, 113607.618766291, 119090.633902362, 184.397907709217, 840.234076326053, 851.487091620761, 132.300214575668 ),
      ( 750, 16.7812192922122, 100000, 156527.2046026, 162486.247164329, 244.264791173751, 873.363993225133, 883.939913746417, 138.902152457482 ),
      ( 800, 15.5697971710627, 100000, 201020.715612367, 207443.406858844, 302.283753064582, 903.800099715204, 913.927928975554, 144.930274557034 ),
      ( 850, 14.5397100512056, 99999.9999999997, 246959.692162297, 253837.408406144, 358.527707715614, 931.611833315321, 941.421937526534, 150.533530365857 ),
      ( 900, 13.6492857225777, 100000, 294219.205935311, 301545.596636811, 413.059362474499, 956.944411942844, 966.517501310387, 155.804522006554 ),
      ( 950, 12.8696959950681, 100000.000074658, 342681.529925281, 350451.721242386, 465.938161332384, 979.97799855863, 989.367537004836, 160.805357918095 ),
      ( 1000, 12.1800684266275, 100000.00003346, 392237.720992901, 400447.855495499, 517.222933985311, 1000.90463716067, 1010.14800916779, 165.580203728649 ),
      ( 311.84, 2851.65939830596, 101324.999998713, -244805.09947966, -244769.567536379, -530.940547769363, 541.603768991725, 651.13100318656, 1341.99409715132 ),
      ( 325, 2818.71654611292, 101325.000000101, -236184.516193396, -236148.56898184, -503.863534441866, 553.583906229273, 659.145576557795, 1293.3393339117 ),
      ( 350, 2757.57717881133, 101324.994861939, -219502.959556719, -219466.215346751, -454.419293952777, 576.235587985621, 675.693320144752, 1206.3325400517 ),
      ( 375, 2697.74181024638, 101325.000791765, -202391.080228106, -202353.521036783, -407.200199210697, 598.68747202851, 693.494332106992, 1124.7624454206 ),
      ( 400, 2638.60298912051, 101325.000012297, -184822.643143968, -184784.242141457, -361.850871009176, 620.912325261953, 712.170454200979, 1047.24773101684 ),
      ( 425, 2579.6156509257, 101325.000064538, -166779.037881781, -166739.758774434, -318.09918538874, 642.89575763432, 731.477867310826, 972.86505473505 ),
      ( 450, 2520.26198590398, 101325.000186845, -148246.675920095, -148206.471766418, -275.73123754073, 664.618022389656, 751.252154852131, 900.983204407617 ),
      ( 475, 2460.02033498742, 101325.000368636, -129215.405948669, -129174.217264605, -234.575296040626, 686.050060978069, 771.379735168115, 831.161272603076 ),
      ( 500, 2398.3352932781, 101325.000541221, -109677.425776582, -109635.177721931, -194.490901539132, 707.156354817527, 791.785350646065, 763.090401618158 ),
      ( 525, 2334.58591743742, 101325.0005905, -89626.375689886, -89582.9739886106, -155.360984042488, 727.900298442956, 812.429362893029, 696.563869695151 ),
      ( 550, 2268.04820271611, 101325.000427137, -69056.4078021652, -69011.7328255461, -117.085771346991, 748.249974478679, 833.312314525045, 631.463850746481 ),
      ( 575, 2197.84582084676, 101325.000153702, -47961.0384797045, -47914.9365196886, -79.5776665225253, 768.183481180837, 854.48859823772, 567.75263632321 ),
      ( 600, 2122.87650097036, 101324.999404612, -26331.4546847604, -26283.724635764, -42.756286109126, 787.693620416242, 876.09950737702, 505.447905281046 ),
      ( 625, 2041.68057699735, 101324.999993233, -4153.4579303671, -4103.82969707008, -6.54228728185121, 806.792170133616, 898.458318056009, 444.539090891866 ),
      ( 628.55700065929, 2029.51179884358, 101324.999999243, -952.122352073966, -902.196552156479, -1.43420487888228, 809.477791044956, 901.733612838032, 435.97710224189 ),
      ( 650, 20.3381730624701, 101325, 72370.4365292197, 77352.4474349571, 122.448952144382, 804.529791894807, 816.977446632233, 124.710736141047 ),
      ( 700, 18.4953576964729, 101325, 113595.07120615, 119073.472957705, 184.270331270197, 840.256328332196, 851.556372303217, 132.188877773132 ),
      ( 750, 17.0142170910729, 101325, 156516.663884961, 162471.977576895, 244.141212642334, 873.377985124352, 883.988283200486, 138.816478893455 ),
      ( 800, 15.7835907552362, 101325, 201011.582597443, 207431.224462928, 302.162872852962, 903.809609237437, 913.964045647602, 144.863193541904 ),
      ( 850, 14.7377289113239, 101325, 246951.612873369, 253826.824149792, 358.40876760723, 931.618786874262, 941.450287491134, 150.480567839915 ),
      ( 900, 13.8340209069253, 101325, 294211.955730463, 301536.290476066, 412.941884873741, 956.949827305864, 966.540588776406, 155.762635282259 ),
      ( 950, 13.0430359440575, 101325.000078481, 342674.956900164, 350443.470343895, 465.821825750396, 979.982432096026, 989.386853123992, 160.772366427436 ),
      ( 1000, 12.3434908608745, 101325.000035155, 392231.716582839, 400440.496512051, 517.107514008292, 1000.90840416887, 1010.16449527756, 165.554482900357 ),
      ( 311.84, 2853.66363184248, 1000000.00000171, -244891.83164261, -244541.404917875, -531.219112607333, 541.981448852939, 651.112343223156, 1345.18428101829 ),
      ( 325, 2820.85315061083, 999999.9999976, -236275.299464668, -235920.796785299, -504.143322416253, 553.95953724383, 659.104712083899, 1296.73693502307 ),
      ( 350, 2759.99441325418, 1000000.00000071, -219602.345813129, -219240.026239148, -454.703753771651, 576.611544490754, 675.606649894605, 1210.16621695563 ),
      ( 375, 2700.48665858429, 999999.999651823, -202500.439291589, -202130.135666325, -407.492375846132, 599.068261138408, 693.35542907281, 1129.09869796402 ),
      ( 400, 2641.735346179, 1000000.0000157, -184943.613757478, -184565.074703041, -362.153915111951, 621.301304944527, 711.970101097468, 1052.16651738002 ),
      ( 425, 2583.21227202022, 1000000.0000698, -166913.614670083, -166526.499754711, -318.416534524876, 643.295268127549, 731.202972791396, 978.460876892698 ),
      ( 450, 2524.42221006016, 1000000.00019464, -148397.332339263, -148001.202089652, -276.066827736558, 665.029491162839, 750.884284921326, 907.367385691757 ),
      ( 475, 2464.87445036333, 1000000.00038407, -129385.267019278, -128979.566842445, -234.933823474609, 686.474003252344, 770.89297205735, 838.465308709161 ),
      ( 500, 2404.05691608655, 1000000.00057168, -109870.513798236, -109454.550268471, -194.878166314472, 707.59219711546, 791.143041334506, 771.470306006849 ),
      ( 525, 2341.41060943907, 1000000.000657, -89847.9671502135, -89420.8741847056, -155.78436633214, 728.345927045933, 811.57902198237, 706.205495725653 ),
      ( 550, 2276.30205578019, 1000000.00054139, -69313.5662885486, -68874.2572794467, -117.554923272173, 748.700846782654, 832.177299455848, 642.589405239675 ),
      ( 575, 2207.9905945302, 1000000.0002664, -48263.4417655834, -47810.5412855072, -80.1055719203224, 768.631020108295, 852.953581001747, 580.629603175055 ),
      ( 600, 2135.58408686895, 1000000.0000506, -26692.7586820114, -26224.5027111043, -43.3610063254267, 788.122403267062, 873.982256290993, 520.405308218442 ),
      ( 625, 2057.96559369821, 1000000.00000123, -4593.78033118899, -4107.86355830252, -7.25017299228688, 807.17500017803, 895.445920759995, 462.008060883357 ),
      ( 650, 1973.63918276139, 999999.999999601, 18048.1014477465, 18554.7796738042, 28.3007693107435, 825.803870154803, 917.765507816793, 405.384776229961 ),
      ( 675, 1880.33844921275, 999999.999998547, 41263.7777098229, 41795.5968620704, 63.3825458721957, 844.041361267986, 941.954688568621, 349.994906828126 ),
      ( 700, 1773.89832725982, 999999.999995494, 65125.4670175109, 65689.1971843577, 98.1375498720377, 861.952446606682, 970.699921353855, 294.139094853915 ),
      ( 725, 1644.38524594826, 999999.999966078, 89827.7104415143, 90435.8404417256, 132.86897683962, 879.71835790868, 1012.83528778582, 233.509799681471 ),
      ( 750, 1454.84342860173, 999999.999999988, 116121.052532349, 116808.411721895, 168.621937219002, 898.214149835748, 1122.61725832641, 155.70733956441 ),
      ( 761.369868279901, 1286.82532346479, 999999.99999999, 129868.25453026, 130645.360779077, 186.928044978278, 908.642667951747, 1399.66604882236, 100.969604141804 ),
      ( 775, 292.191713411158, 1000000.00000041, 167815.710377746, 171238.120918846, 240.02328255232, 905.298624358625, 1065.99759216106, 77.122858448163 ),
      ( 825, 209.328245051242, 1000000.00001325, 216472.417158382, 221249.603341587, 302.581532266229, 926.045529810522, 978.400667339656, 108.903872793953 ),
      ( 875, 175.666069873723, 1000000, 264518.624702268, 270211.243662315, 360.198514760294, 949.670755257424, 983.308905807941, 126.189726023975 ),
      ( 925, 154.802366032179, 999999.999999999, 313273.084715097, 319732.934300291, 415.232913892855, 972.399696691617, 998.217337498636, 138.829244305339 ),
      ( 975, 139.909396916752, 999999.999999999, 362915.590687143, 370063.073429814, 468.220343854712, 993.551572521278, 1015.07056498113, 149.062162483092 ),
      ( 311.84, 2854.33236900209, 1300949.32353442, -244920.725939749, -244464.94533842, -531.312072931448, 542.107488932761, 651.106779349448, 1346.24861311412 ),
      ( 325, 2821.56581204807, 1300949.3235336, -236305.531631529, -235844.458096409, -504.236661412216, 554.084851724956, 659.091843719598, 1297.87001923516 ),
      ( 359, 2739.291574679, 1300949.32353505, -213530.831334842, -213055.90952091, -437.56729425068, 584.846288125263, 681.842439211576, 1181.76603624904 ),
      ( 393, 2659.14464288365, 1300949.32353136, -189945.585109232, -189456.34909606, -374.776600260771, 615.228080299951, 706.627886988436, 1074.92832010917 ),
      ( 427, 2579.73334514035, 1300949.32351103, -165495.359697086, -164991.06365193, -315.085884107636, 645.176960162372, 732.671809360034, 974.560423594403 ),
      ( 461, 2499.81697763573, 1300949.32341214, -140146.033082838, -139625.615254233, -257.941686726487, 674.641117341131, 759.519507227074, 879.041143738624 ),
      ( 495, 2418.17326678083, 1300949.32314064, -113875.925131111, -113337.936666425, -202.934884976611, 703.540004588912, 786.882503051276, 787.414797219689 ),
      ( 529, 2333.483671287, 1300949.32271142, -86671.3406006299, -86113.8268118174, -149.75342880447, 731.777872817795, 814.582158550237, 699.154400902756 ),
      ( 563, 2244.21892396736, 1300949.32259322, -58523.1362685807, -57943.4471354487, -98.1516848126917, 759.267339818486, 842.542106288075, 614.073298309009 ),
      ( 597, 2148.50320113414, 1300949.32314469, -29422.9678050619, -28817.453546364, -47.9281668329851, 785.949687272049, 870.819268390117, 532.284088089123 ),
      ( 631, 2043.90002334911, 1300949.32353464, 642.664879651926, 1279.16828425727, 1.09427808700457, 811.807237335658, 899.725309486735, 454.054330893411 ),
      ( 665, 1926.88540473137, 1300949.32353505, 31706.5402877655, 32381.6968488878, 49.0960252339578, 836.866770568661, 930.268563815159, 379.251531023875 ),
      ( 699, 1790.98897515603, 1300949.32353418, 63865.1950235977, 64591.5810268509, 96.3270488007902, 861.209017854318, 965.842417044647, 305.838462243619 ),
      ( 733, 1618.45883037722, 1300949.32353494, 97458.0615272825, 98261.8813706527, 143.351292966512, 885.110380835389, 1020.99685430616, 226.348458468731 ),
      ( 767, 1302.95552163625, 1300949.32353482, 134658.194875735, 135656.655146966, 193.181651954321, 911.016427624066, 1285.14265772172, 113.539634935199 ),
      ( 801, 380.600863187138, 1300949.32355089, 188628.694113017, 192046.840127339, 265.293290297396, 922.37007867232, 1099.22293866233, 82.1615540295442 ),
      ( 835, 292.680835710284, 1300949.32354059, 222993.44483455, 227438.386780897, 308.577105874733, 934.44175429755, 1011.4679907886, 104.048061623457 ),
      ( 869, 250.558375178428, 1300949.32353544, 256363.731046669, 261555.931557265, 348.627981833172, 949.179249323904, 999.662513894511, 118.349110227908 ),
      ( 903, 223.515499710341, 1300949.32353482, 289751.825161616, 295572.223859786, 387.025430887082, 964.209811981745, 1002.55510173701, 129.293285132996 ),
      ( 937, 203.912153158737, 1300949.32353482, 323402.774680814, 329782.72459249, 424.214104870255, 978.871634570835, 1010.29758728374, 138.306935538835 ),
      ( 971, 188.70699415712, 1300949.32353482, 357398.746774002, 364292.764292248, 460.39096380392, 992.917935801813, 1019.88229196383, 146.054727489384 ),
      ( 325, 2841.58083786895, 10000000.0000018, -237144.558169794, -233625.390294387, -506.861298051732, 557.608844822053, 658.879268881022, 1329.66054928994 ),
      ( 359, 2762.78521591226, 9999999.99999889, -214472.236885636, -210852.701084444, -440.237915088332, 588.356211437621, 681.112137839625, 1218.89139011995 ),
      ( 393, 2686.86200495991, 9999999.99999907, -191009.631351098, -187287.817584131, -377.539104310405, 618.786568263987, 705.302050760672, 1118.37871916976 ),
      ( 427, 2612.67113315419, 9999999.99999488, -166706.910151842, -162879.409678056, -317.986781582742, 648.819691058369, 730.626560581966, 1025.52178073032 ),
      ( 461, 2539.31327717232, 9999999.99998297, -141536.347506275, -137598.274921895, -261.032130178341, 678.381167167615, 756.567555528973, 938.884424783488 ),
      ( 495, 2466.04203437141, 9999999.99996286, -115485.182267164, -111430.101347786, -206.274947643164, 707.369591809069, 782.750498475056, 857.692534277355 ),
      ( 529, 2392.21352436776, 9999999.99995007, -88552.1157431793, -84371.8868889874, -153.417048385942, 735.665475257106, 808.878148922298, 781.580498234308 ),
      ( 563, 2317.26406456935, 9999999.99995574, -60745.2364076192, -56429.8024642658, -102.232875141829, 763.148849830703, 834.705942576079, 710.462329640055 ),
      ( 597, 2240.70909111187, 9999999.99997341, -32080.4252966446, -27617.5523428819, -52.5496070982642, 789.712595080663, 860.035087049075, 644.446212042412 ),
      ( 631, 2162.15480568695, 9999999.99999047, -2579.79932916119, 2045.2164068482, -4.23286872184091, 815.268135373926, 884.716296698305, 583.736684713584 ),
      ( 665, 2081.30640184165, 9999999.99999841, 27730.1413983599, 32534.8159965594, 42.8242847493231, 839.744354970721, 908.664589191597, 528.491582402999 ),
      ( 699, 1997.94675322125, 9999999.99999983, 58820.9585238521, 63826.0969159925, 88.7106240339242, 863.082684756025, 931.883208554515, 478.643459158105 ),
      ( 733, 1911.86126630524, 10000000.0000002, 90665.3126619029, 95895.817707557, 133.504908656881, 885.232459870315, 954.48258200101, 433.761832991067 ),
      ( 767, 1822.71674388445, 9999999.99999989, 123239.884292594, 128726.20027315, 177.282451192315, 906.149973097453, 976.667132032404, 393.078393065899 ),
      ( 801, 1729.96483714485, 10000000.0000002, 156526.578447692, 162307.04275846, 220.118426070019, 925.801436843724, 998.657920543476, 355.757935107104 ),
      ( 835, 1632.91481659595, 9999999.99999988, 190509.482042406, 196633.500208183, 262.085206430756, 944.166639260515, 1020.50439687827, 321.387211549826 ),
      ( 869, 1531.19880746123, 10000000.0000466, 225163.641079093, 231694.471661849, 303.239225617229, 961.240686507909, 1041.70846673713, 290.529275746328 ),
      ( 903, 1425.82430767105, 10000000.000002, 260432.180050179, 267445.666877554, 343.593070398843, 977.037547854968, 1060.76258776826, 264.938211488712 ),
      ( 937, 1320.31417183101, 9999999.99999796, 296202.90057197, 303776.855478574, 383.086308200093, 991.605629974539, 1075.49728299368, 246.671378535746 ),
      ( 971, 1220.10288300794, 9999999.99999959, 332323.282622299, 340519.312759812, 421.603486352679, 1005.05125445971, 1085.0163341603, 236.231658129987 ),
      };

      // TestData contains:
      // 0. Temperature (Kelvin)
      // 1. Pressure (Pa)
      // 2. Saturated liquid density (mol/m³
      // 3. Saturated vapor density (mol/m³)
      _testDataSaturatedProperties = new (double temperature, double pressure, double saturatedLiquidMoleDensity, double saturatedVaporMoleDensity)[]
      {
      ( 369.735, 2.15784331880561, 2709.96348343477, 0.000701941668279051 ),
      ( 427.63, 108.14206993172, 2572.98368238821, 0.0304271509511044 ),
      ( 485.525, 1669.70987926818, 2433.67810235671, 0.415104927433123 ),
      ( 543.42, 12228.2384240841, 2285.08492017936, 2.75410218048258 ),
      ( 601.315, 55044.2380778852, 2118.09078205969, 11.6565227792329 ),
      ( 659.21, 180258.229040816, 1918.67405104089, 38.1425986767114 ),
      ( 717.105, 485326.331502489, 1655.26445101531, 115.999474603532 ),
      };
    }

    [Test]
    public override void CASNumberAttribute_Test()
    {
      base.CASNumberAttribute_Test();
    }

    [Test]
    public override void ConstantsAndCharacteristicPoints_Test()
    {
      base.ConstantsAndCharacteristicPoints_Test();
    }

    [Test]
    public override void EquationOfState_Test()
    {
      base.EquationOfState_Test();
    }

    [Test]
    public override void SaturatedVaporPressure_TestMonotony()
    {
      base.SaturatedVaporPressure_TestMonotony();
    }

    [Test]
    public override void SaturatedVaporPressure_TestInverseIteration()
    {
      base.SaturatedVaporPressure_TestInverseIteration();
    }

    [Test]
    public override void SaturatedVaporProperties_TestData()
    {
      base.SaturatedVaporProperties_TestData();
    }

    [Test]
    public override void MeltingPressure_TestImplemented()
    {
      base.MeltingPressure_TestImplemented();
    }

    [Test]
    public override void SublimationPressure_TestImplemented()
    {
      base.SublimationPressure_TestImplemented();
    }
  }
}
