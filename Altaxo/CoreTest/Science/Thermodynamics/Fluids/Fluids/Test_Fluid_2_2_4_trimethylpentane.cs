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
  /// Tests and test data for <see cref="Fluid_2_2_4_trimethylpentane"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Fluid_2_2_4_trimethylpentane : FluidTestBase
  {

    public Test_Fluid_2_2_4_trimethylpentane()
    {
      _fluid = Fluid_2_2_4_trimethylpentane.Instance;

      _testDataMolecularWeight = 0.11422852;

      _testDataTriplePointTemperature = 165.77;

      _testDataTriplePointPressure = 0.01796;

      _testDataTriplePointLiquidMoleDensity = 6961.07620120858;

      _testDataTriplePointVaporMoleDensity = 1.30322349639554E-05;

      _testDataCriticalPointTemperature = 544;

      _testDataCriticalPointPressure = 2571941.67632537;

      _testDataCriticalPointMoleDensity = 2120;

      _testDataNormalBoilingPointTemperature = 372.358256398524;

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
      ( 175, 1.851505280411E-05, 0.0269400001062882, -4221.12508449005, -2766.09277152208, 84.2091930149898, 112.649191787862, 120.963671183264, 116.953289261651 ),
      ( 225, 1.44005949618546E-05, 0.0269400000452662, 2027.54484687063, 3898.30088970268, 117.575379068007, 137.554653271228, 145.869128805832, 131.784804455778 ),
      ( 275, 1.17823044503332E-05, 0.0269400000220223, 9557.08271270863, 11843.5624192537, 149.374819893098, 163.860395131806, 172.174868913081, 145.02546725279 ),
      ( 325, 9.96964203932056E-06, 0.0269400000120674, 18434.6880272759, 21136.8913667554, 180.360552852689, 191.399268553935, 199.713741509105, 157.110630133723 ),
      ( 375, 8.64035635600111E-06, 0.0269400000072941, 28703.644170744, 31821.5711285347, 210.89260736038, 219.32841967461, 227.642892230643, 168.315954106504 ),
      ( 425, 7.6238438067562E-06, 0.0269400000047461, 40355.3636400846, 43889.014208958, 241.065050920113, 246.539091986758, 254.853564338743, 178.824485298385 ),
      ( 475, 6.821333913107E-06, 0.0269400000032551, 53331.6332851743, 57281.0074613146, 270.829237577567, 272.209451668734, 280.523923908476, 188.760107922475 ),
      ( 525, 6.17168305327067E-06, 0.0269400000023163, 67543.7852185041, 71908.8829997389, 300.089593917346, 295.939538836622, 304.254011009796, 198.210370000662 ),
      ( 575, 5.63501495500822E-06, 0.0269400000016906, 82891.9935946093, 87672.814979608, 328.756072048457, 317.660322557115, 325.974794688013, 207.240440887123 ),
      ( 165.77, 6961.07705184837, 214.923409486287, -46918.6355370185, -46918.6046619961, -178.590195938137, 137.472239119799, 184.064324780577, 1720.8421406436 ),
      ( 175, 6893.75707101291, 214.923409325201, -45207.4092005386, -45207.3780240108, -168.545111281717, 140.453508000422, 186.787383680213, 1669.83646417441 ),
      ( 200, 6714.87828637008, 214.923421634992, -40432.7654036058, -40432.7333965593, -143.055371528642, 149.478615230421, 195.432579058521, 1537.70409143874 ),
      ( 225, 6538.8417577158, 214.923412442451, -35426.5040167342, -35426.471148006, -119.480572584704, 159.346199561907, 205.219598895804, 1413.13442030182 ),
      ( 241.446841161475, 6423.51137965868, 214.923409637096, -31994.9432698788, -31994.9098110124, -104.763715118314, 166.232055238343, 212.133770023866, 1334.62128990738 ),
      ( 250, 0.103444886838571, 214.923410792787, 5625.41922741593, 7703.08026375291, 58.9021350310346, 150.546166573452, 158.88056973174, 138.516202253138 ),
      ( 300, 0.0861849087526219, 214.923410792784, 13822.0994628245, 16315.8470794714, 90.2333384108145, 177.521875400291, 185.846664583694, 151.160629968677 ),
      ( 350, 0.0738653403387221, 214.923410792784, 23393.4497907392, 26303.1149978203, 120.969029545362, 205.391979769603, 213.712206217308, 162.789843580815 ),
      ( 400, 0.0646288761710387, 214.923410792784, 34358.6300543186, 37684.1313399048, 151.322320854441, 233.087831025041, 241.405798317222, 173.634715130183 ),
      ( 450, 0.0574462487075484, 214.923410792784, 46682.4911841905, 50423.787015132, 181.301963742051, 259.602831571148, 267.919602423105, 183.847916524869 ),
      ( 500, 0.0517007252615614, 214.923410792783, 60288.9855506988, 64446.0531683281, 210.826950044012, 284.328452037765, 292.644539326556, 193.534112754814 ),
      ( 550, 0.0470001292235565, 214.923410792784, 75081.8236451519, 79654.6495999594, 239.800432051755, 307.046993487664, 315.362660811523, 202.768937630419 ),
      ( 600, 0.0430831389641096, 214.92350335369, 90960.95438743, 95949.5301625779, 268.144133865845, 327.802328720606, 336.117721110714, 211.609920301127 ),
      ( 165.77, 6961.08015933355, 1000.00000213696, -46918.6552563438, -46918.5116004773, -178.590314894307, 137.472289842774, 184.064313434374, 1720.84558695096 ),
      ( 175, 6893.76034897188, 1000.00000157821, -45207.4301546382, -45207.2850959256, -168.545231019669, 140.453557850778, 186.787366039953, 1669.84005824727 ),
      ( 200, 6714.88208658873, 1000.00001190104, -40432.7900638529, -40432.6411409085, -143.055494830133, 149.478663223243, 195.432542501888, 1537.70812794741 ),
      ( 225, 6538.84619018909, 1000.00000205014, -35426.5330000875, -35426.3800679076, -119.480701399888, 159.346246604542, 205.219540915307, 1413.13897263989 ),
      ( 250, 6363.48135669548, 1000.0000002781, -30164.6220178335, -30164.4648711485, -97.3146324949042, 169.944768334355, 215.878699398017, 1294.78091145694 ),
      ( 263.141598136685, 6270.96811744074, 999.999998632127, -27288.7533790398, -27288.5939140289, -86.1046054424721, 175.829197806723, 221.837383785522, 1234.81829019919 ),
      ( 275, 0.43801907540397, 1000.00000000071, 9547.97722629164, 11830.9828216038, 61.8576700475746, 163.914174887594, 172.295125380496, 144.831743977519 ),
      ( 325, 0.37037652037553, 1000.00000000012, 18428.921794428, 21128.8769616351, 92.8587783960325, 191.431125530504, 199.781156341659, 156.9932949539 ),
      ( 375, 0.320887236430809, 1000.00000000003, 28699.7724187093, 31816.1319571032, 123.39825226832, 219.346341076459, 227.681487829275, 168.238708954997 ),
      ( 425, 0.283086041437516, 1000.00000000001, 40352.586447187, 43885.0813555265, 153.5744864863, 246.549258235038, 254.876809544794, 178.770461083121 ),
      ( 475, 0.253261475300016, 1000, 53329.5193915747, 57278.0078018842, 183.340757613603, 272.215442736587, 280.538819990357, 188.720703389574 ),
      ( 525, 0.229126507293256, 1000, 67542.0976570713, 71906.4988423087, 212.60234993308, 295.943254207425, 304.264157380513, 198.180798026931 ),
      ( 575, 0.20919344384328, 1000, 82890.5965826039, 87670.8611149339, 241.269612915194, 317.662754973466, 325.982087458254, 207.217826286359 ),
      ( 165.77, 6961.11578209157, 9999.99999921908, -46918.8813074352, -46917.4447561247, -178.591678561645, 137.472871321236, 184.06418340267, 1720.88509366779 ),
      ( 175, 6893.79792588737, 10000.0000006814, -45207.670360057, -45206.2197808395, -168.546603646912, 140.454129325781, 186.787163861354, 1669.88125879166 ),
      ( 200, 6714.92565016076, 10000.0000089481, -40433.0727528014, -40431.5835330349, -143.056908301444, 149.479213404252, 195.432123494812, 1537.7544000492 ),
      ( 225, 6538.89700117358, 10000.0000036165, -35426.8652440375, -35425.3359341245, -119.482178068714, 159.346785893833, 205.218876346669, 1413.19115748353 ),
      ( 250, 6363.54105416514, 10000.0000019221, -30165.0123431234, -30163.4408910158, -97.3161938284963, 169.945291342415, 215.877740706164, 1294.84000366675 ),
      ( 275, 6187.09925676814, 10000.0000003611, -24625.6259417279, -24624.0096754316, -76.2063661020041, 181.323228360442, 227.427444193473, 1182.04118918577 ),
      ( 300, 6007.86227618468, 9999.99999993932, -18786.3218160453, -18784.6573304861, -55.8906071392243, 193.420721881292, 239.863734693276, 1074.31250778303 ),
      ( 306.18760617603, 5962.85381173643, 10000.0000000341, -17292.2556406271, -17290.5785912954, -50.9611204352573, 196.508142803081, 243.070664834046, 1048.35691493196 ),
      ( 325, 3.73195354081754, 10000.0000012139, 18376.5782245452, 21056.1399853873, 73.5526712721333, 191.720561221051, 200.400193109641, 155.925721404001 ),
      ( 375, 3.22352785360463, 10000.0000002619, 28664.7517067663, 31766.9429873782, 104.159994588351, 219.508573086973, 228.033001459324, 167.539287903945 ),
      ( 425, 2.83924132119819, 10000.0000000712, 40327.5141580774, 43849.5818053076, 134.370679837279, 246.641097942134, 255.087618875805, 178.282532856907 ),
      ( 475, 2.53774610414567, 10000.0000000228, 53310.4562648856, 57250.9607872595, 164.155831777539, 272.269495256143, 280.673577939883, 188.365326606121 ),
      ( 525, 2.29456408341065, 10000.0000000082, 67526.8893607799, 71885.0156263772, 193.428596819077, 295.976746401005, 304.355804215443, 197.914331720601 ),
      ( 575, 2.0941310165288, 10000.0000000032, 82878.0120704811, 87653.2624827399, 222.102945312074, 317.684669194926, 326.047891254186, 207.014167435609 ),
      ( 165.77, 6961.47191008766, 99999.9999980679, -46921.1409801443, -46906.776201935, -178.60531237276, 137.478685893588, 184.062887075281, 1721.28003621368 ),
      ( 175, 6894.17358246684, 100000.000000056, -45210.0714809784, -45195.5664792108, -168.560326821803, 140.459843873998, 186.785146795475, 1670.29312605701 ),
      ( 200, 6715.361127823, 100000.000009571, -40435.8983867733, -40421.0071548491, -143.071039126773, 149.484715063813, 195.427940768737, 1538.21693638799 ),
      ( 225, 6539.40488627696, 100000.000003761, -35430.1859793504, -35414.8940679696, -119.496939795321, 159.352178745306, 205.212241883117, 1413.71275565652 ),
      ( 250, 6364.13770477235, 99999.9999997927, -30168.9132600858, -30153.2002122797, -97.3318007370157, 169.950521755646, 215.868170931379, 1295.43058286209 ),
      ( 275, 6187.80610644428, 100000.000001197, -24630.2144395676, -24614.0536229096, -76.2230552405729, 181.328065387688, 227.414055265563, 1182.71221198083 ),
      ( 300, 6008.70821191687, 99999.9999999543, -18791.740623016, -18775.0981107699, -55.9086741242022, 193.424835918371, 239.845076018279, 1075.0779437378 ),
      ( 325, 5824.92736821475, 100000.000000273, -12632.2378525812, -12615.0702565445, -36.193128614399, 206.074933634974, 253.082911012177, 971.825963394944 ),
      ( 350, 5634.09512962791, 100000.000002038, -6132.81925249977, -6115.07017341941, -16.931437044242, 219.073247295819, 267.028343934227, 871.963567437846 ),
      ( 370.893631856071, 5466.94925048348, 100000.000010234, -427.941673940706, -409.64993655145, -1.10166532526845, 230.062370163515, 279.188735660948, 790.207363326506 ),
      ( 375, 33.8547164965526, 100000, 28295.4672117295, 31249.2653925585, 84.0202674684603, 221.233574499009, 232.031464740405, 160.078259366192 ),
      ( 425, 29.2821590350493, 99999.9999999999, 40068.6984150491, 43483.7471442192, 114.613278188848, 247.595451828756, 257.370200393925, 173.222826210095 ),
      ( 475, 25.9104292060975, 100000, 53115.9573897121, 56975.4071581655, 144.600145074823, 272.823574467192, 282.093900138362, 184.73619795498 ),
      ( 525, 23.2838193652562, 100000, 67372.7920457638, 71667.6200733862, 173.989713853137, 296.317055627228, 305.306051268407, 195.217810416136 ),
      ( 575, 21.1645798209082, 100000.000032568, 82751.0529827396, 87475.928263929, 202.737140413801, 317.906018895337, 326.722955632977, 204.965186592863 ),
      ( 165.77, 6961.47715173138, 101324.999998865, -46921.1742361695, -46906.6191356082, -178.605513053902, 137.478771494127, 184.062868044599, 1721.28584895407 ),
      ( 175, 6894.17911143841, 101324.999999086, -45210.1068181457, -45195.4096368916, -168.560528815386, 140.459928002101, 186.78511716383, 1670.29918778295 ),
      ( 200, 6715.36753687671, 101325.000010738, -40435.9399693435, -40420.8514429963, -143.071247111155, 149.484796058415, 195.427879289358, 1538.22374344443 ),
      ( 225, 6539.41236042447, 101325.000002556, -35430.2348448254, -35414.7403333284, -119.497157053403, 159.352258139488, 205.212144361299, 1413.72043134482 ),
      ( 250, 6364.1464843979, 101325.000002275, -30168.9706585553, -30153.0494348294, -97.3320304181754, 169.950598763443, 215.86803027551, 1295.43927284811 ),
      ( 275, 6187.81650640125, 101325.000000798, -24630.2819484576, -24613.9070285006, -76.2233008268698, 181.328136617182, 227.413858512095, 1182.72208449429 ),
      ( 300, 6008.72065635031, 101324.999999225, -18791.8203377292, -18774.9573471204, -55.9089399555664, 193.424896531101, 239.844801899537, 1075.08920365724 ),
      ( 325, 5824.94245149622, 101325.000000372, -12632.3326696083, -12614.9376479674, -36.1934204967864, 206.074977949118, 253.08252565086, 971.838881521072 ),
      ( 350, 5634.1137249945, 101325.000001354, -6132.93333291593, -6114.94913789483, -16.9317631567706, 219.073268235453, 267.027791483137, 871.97852779937 ),
      ( 371.358256398524, 5463.15643202741, 101325.000011003, -298.307839552329, -279.760869266618, -0.752332104627822, 230.307205267444, 279.463760368309, 788.417106788416 ),
      ( 375, 34.3298610361298, 101325, 28289.7426735129, 31241.2547665378, 83.8952636754527, 221.260522006773, 232.098088111655, 159.961270286567 ),
      ( 425, 29.6841884517739, 101325, 40064.7710639287, 43478.2044533784, 114.494491542803, 247.610022744056, 257.406423705734, 173.145702075809 ),
      ( 475, 26.261979543575, 101325, 53113.0392888091, 56971.2785289957, 144.484518621275, 272.831923550286, 282.115863616482, 184.681696245045 ),
      ( 525, 23.5974923342911, 101325, 67370.4953194271, 71664.3837786155, 173.875879780214, 296.32214093679, 305.320519748461, 195.177664466063 ),
      ( 575, 21.4483918672838, 101325.000034343, 82749.1684766023, 87473.2988758095, 202.624413515938, 317.909308204916, 326.733132111086, 204.934848352575 ),
      ( 165.77, 6965.02327998282, 1000000.00000154, -46943.6542563772, -46800.0797183171, -178.741365500896, 137.536810079154, 184.050319477389, 1725.21706858952 ),
      ( 175, 6897.91894917781, 999999.999996998, -45233.9898043449, -45089.0185445541, -168.697250260456, 140.51696881759, 186.765444036378, 1674.39806237071 ),
      ( 200, 6719.7001975143, 1000000.00001223, -40464.0298907909, -40315.2137277749, -143.211960793347, 149.539716053692, 195.386840477562, 1542.82396119536 ),
      ( 225, 6544.46143358042, 1000000.00000251, -35463.2240911764, -35310.4231296074, -119.644064289013, 159.406102176585, 205.147005916405, 1418.90392085773 ),
      ( 250, 6370.07210144937, 999999.999998669, -30207.6908010363, -30050.7067066536, -97.4872324331893, 170.002857259104, 215.774162615239, 1301.30243082693 ),
      ( 275, 6194.8276594413, 999999.99999989, -24675.77847379, -24514.3534826563, -76.3891082717876, 181.376562232198, 227.28278428741, 1189.37559154638 ),
      ( 300, 6017.09762443128, 1000000.00000012, -18845.4761867664, -18679.2831045835, -56.0882170426676, 193.466298818584, 239.662666070673, 1082.66693393582 ),
      ( 325, 5835.07576402918, 1000000.00000023, -12696.0492899368, -12524.6719093439, -36.3899752408468, 206.105651942502, 252.827427409673, 980.516484309912 ),
      ( 350, 5646.57283104935, 1000000.0000006, -6209.42268128963, -6032.32410664594, -17.1509179065652, 219.08867188196, 266.664025667108, 882.002946804627 ),
      ( 375, 5448.74620431417, 1000000.00000618, 629.959417495203, 813.48787753146, 1.73584632474595, 232.21998473504, 281.099306646448, 785.99918620486 ),
      ( 400, 5237.62238576367, 999999.999999941, 7836.98607580787, 8027.91240198175, 20.3549476517101, 245.342226823412, 296.174454907809, 691.178189046926 ),
      ( 425, 5007.11825717912, 1000000.00000005, 15430.0142832007, 15629.729957694, 38.7843465219941, 258.35205119142, 312.168177823291, 595.883008297217 ),
      ( 450, 4746.71274725003, 999999.999999996, 23439.8947376122, 23650.5668497132, 57.1178579635176, 271.220176336602, 329.918349591048, 497.710893031818 ),
      ( 475, 4434.43052628453, 999999.999999942, 31935.1649839781, 32160.6730833998, 75.5172096408168, 284.05256998978, 352.081967773276, 392.173588700008 ),
      ( 478.641128337565, 4381.94714332142, 999999.999978792, 33221.6796789808, 33449.8887301792, 78.2209784654612, 285.939052126822, 356.112217222984, 375.66040792498 ),
      ( 500, 309.037657774059, 1000000.00000461, 57954.6167940726, 61190.4683961311, 135.781578964503, 290.435471908883, 315.7523431322, 150.826080941334 ),
      ( 550, 256.49625109052, 1000000.00000009, 73344.2610145248, 77242.9534739603, 166.371536671883, 310.522790727123, 327.846082851687, 173.965468024769 ),
      ( 600, 223.727824576272, 1000000, 89568.6394217372, 94038.3561496138, 195.589028969689, 329.994879967354, 344.175795700268, 190.783777896474 ),
      ( 175, 6904.94095629586, 2700538.76014223, -45278.7279531161, -44887.6255849373, -168.954454137156, 140.624799116038, 186.730484577751, 1682.0871797204 ),
      ( 197, 6748.85929630941, 2700538.76014202, -41099.6043056764, -40699.4568671872, -146.419860320929, 148.509062681409, 194.211091104039, 1566.67247373851 ),
      ( 219, 6595.47549348633, 2700538.76014167, -36745.1408738113, -36335.6876413933, -125.42845955604, 157.075887122322, 202.616782511796, 1457.44748791832 ),
      ( 241, 6443.30583488024, 2700538.76014086, -32198.534560212, -31779.4113912447, -105.610370573363, 166.199956467002, 211.699000001815, 1353.42449579274 ),
      ( 263, 6291.18916381324, 2700538.76014224, -27445.3167797942, -27016.0594979315, -86.7023574895869, 175.917015191298, 221.44565067742, 1254.06992664975 ),
      ( 285, 6138.09285621155, 2700538.76014079, -22470.814476909, -22030.8506602313, -68.5039834773824, 186.222168436902, 231.865657201401, 1159.09666513022 ),
      ( 307, 5982.97099442196, 2700538.76014213, -17260.6887158661, -16809.3178563617, -50.8606350052004, 197.033829381987, 242.918697585993, 1068.18217823768 ),
      ( 329, 5824.66827986844, 2700538.76014193, -11802.0175172228, -11338.3793097946, -33.6541909691264, 208.222776630562, 254.523472378878, 980.865700620419 ),
      ( 351, 5661.83684839576, 2700538.76014177, -6083.89278219786, -5606.92058528495, -16.795286199768, 219.646211491879, 266.587681498115, 896.566213061413 ),
      ( 373, 5492.8317944158, 2700538.76014167, -97.3500872111455, 394.297729655762, -0.216063807724964, 231.17046563887, 279.037836867181, 814.628685901485 ),
      ( 395, 5315.54836213987, 2700538.76014138, 6165.3116549761, 6673.35684169212, 16.1366225487674, 242.683281056973, 291.846815400432, 734.351385754833 ),
      ( 417, 5127.14336079068, 2700538.76014147, 12711.8248611862, 13238.5389727504, 32.3077451037401, 254.099986603551, 305.066613382758, 654.97999083648 ),
      ( 439, 4923.52238364591, 2700538.76014138, 19552.1161573522, 20100.6134626781, 48.3411191817219, 265.36759803652, 318.884279068547, 575.661644204145 ),
      ( 461, 4698.30195136796, 2700538.76014179, 26702.1990821593, 27276.989461342, 64.2888409094869, 276.471135379938, 333.748489350141, 495.335508788415 ),
      ( 483, 4440.39449003716, 2700538.76014169, 34192.5141777352, 34800.6895923748, 80.2287035507252, 287.450390539888, 350.730095602364, 412.471627747354 ),
      ( 505, 4126.98504287856, 2700538.76013942, 42091.209719233, 42745.5708889686, 96.310479861987, 298.454888196596, 372.909349972207, 324.295281650994 ),
      ( 527, 3692.1621609832, 2700538.76013745, 50606.9835432836, 51338.408286399, 112.959632850623, 310.000278811281, 414.955754979031, 223.313971265404 ),
      ( 549, 1641.6725742494, 2700538.7601421, 65230.4651393383, 66875.4574493164, 141.668754798207, 326.05324597737, 1284.0809027647, 73.7598109958184 ),
      ( 571, 971.11578566203, 2700538.76014163, 75771.5669107636, 78552.4286782413, 162.580682169417, 327.024777506659, 410.121526415617, 122.07323725463 ),
      ( 593, 815.982528035048, 2700538.76014163, 83868.0587479877, 87177.6133859341, 177.405627595488, 333.057554078949, 381.314988465658, 144.122287091636 ),
      ( 175, 6934.30764062798, 9999999.99621299, -45464.3001380763, -44022.1950982562, -170.037015629761, 141.08584510271, 186.611428695931, 1714.14996598247 ),
      ( 197, 6782.07826606329, 10000000.0000003, -41312.5627167317, -39838.0884320513, -147.524130852772, 148.954651934793, 193.959042306226, 1601.94585447839 ),
      ( 219, 6633.17275143599, 10000000.0000009, -36988.9889088196, -35481.4147245296, -126.56674194807, 157.513539364272, 202.222113664726, 1496.3228520619 ),
      ( 241, 6486.24722815295, 9999999.99999928, -32477.2365991325, -30935.5130649763, -106.793674548312, 166.630602833591, 211.14716702908, 1396.34066321696 ),
      ( 263, 6340.30665830738, 9999999.9969437, -27763.4795519704, -26186.2687740278, -87.9415623810624, 176.332590760896, 220.709080424374, 1301.49344794436 ),
      ( 285, 6194.53184396293, 9999999.99774622, -22834.0098931756, -21219.6828948421, -69.8110692077367, 186.609326171407, 230.900440226914, 1211.52846607564 ),
      ( 307, 6048.16554115328, 10000000, -17675.8500248378, -16022.4561268108, -52.2497395354748, 197.377777198993, 241.662015878281, 1126.19148503563 ),
      ( 329, 5900.4533682167, 9999999.99999994, -12277.9257957331, -10583.1407722863, -35.1426284675257, 208.508841127714, 252.888373101251, 1045.13988685781 ),
      ( 351, 5750.61474203941, 9999999.99999959, -6631.83304199251, -4892.88852065371, -18.4047874586116, 219.859227770317, 264.452857941288, 967.970232764229 ),
      ( 373, 5597.82462986789, 10000000.0000005, -732.071376534763, 1054.33685544954, -1.97455589358542, 231.292618487873, 276.229477042894, 894.276443056879 ),
      ( 395, 5441.19658583883, 9999999.99999984, 5424.11070296306, 7261.94174660256, 14.1922550048197, 242.690608434919, 288.106823968879, 823.698794979045 ),
      ( 417, 5279.76448561744, 9999999.99954493, 11837.0864306792, 13731.1103074612, 30.1271578266716, 253.956991073259, 299.994933570318, 755.957171964572 ),
      ( 439, 5112.46502219276, 9999999.99969728, 18505.2976424863, 20461.3012447189, 45.8527602801549, 265.018201800491, 311.826790189732, 690.874909864244 ),
      ( 461, 4938.12758581853, 9999999.99984881, 25425.670963549, 27450.730022081, 61.3855818744924, 275.821627591945, 323.555608655969, 628.401175661016 ),
      ( 483, 4755.48477762311, 9999999.99995407, 32593.9130790607, 34696.7481143044, 76.7379957794639, 286.332704104662, 335.148187352145, 568.636569870769 ),
      ( 505, 4563.22669002348, 9999999.99999353, 40004.5999927293, 42196.0317753879, 91.9193525246362, 296.531209610551, 346.574246100303, 511.860003625309 ),
      ( 527, 4360.13284116033, 9999999.99999992, 47650.9905009962, 49944.4986036848, 106.93633637483, 306.406819384591, 357.792729354974, 458.543443128381 ),
      ( 549, 4145.31538952746, 10000000, 55524.5670625744, 57936.9287432473, 121.792699004653, 315.953843351236, 368.739942836915, 409.325782798215 ),
      ( 571, 3918.57205778706, 10000000, 63614.4833204115, 66166.433329887, 136.48877087719, 325.165387704045, 379.329229750298, 364.913220542729 ),
      ( 593, 3680.7731226902, 10000000.0000688, 71907.2608104406, 74624.0813442637, 151.021335610539, 334.028219275771, 389.463573093023, 325.920203381907 ),
      ( 241, 6872.1701298781, 99999999.9999918, -34800.3112276681, -20248.8670535458, -118.239137377536, 171.544343127853, 209.583486574092, 1779.04015638374 ),
      ( 263, 6763.74697726075, 99999999.9999926, -30326.5454907741, -15541.8410457105, -99.5542104303902, 181.14930935843, 218.430253189013, 1708.88163414411 ),
      ( 285, 6658.98416389163, 100000000.000001, -25650.959408553, -10633.6538347913, -81.6365562684661, 191.289239239963, 227.859623977498, 1644.02783234994 ),
      ( 307, 6557.47001309502, 100000000.000001, -20762.1590714851, -5512.37526762395, -64.331541297562, 201.883905269863, 237.784545836822, 1584.06787395357 ),
      ( 329, 6458.87716500253, 100000000.000002, -15650.939444151, -168.372204468478, -47.523904840211, 212.818568070119, 248.085546679366, 1528.61693476624 ),
      ( 351, 6362.94261656876, 100000000, -10310.8419400772, 5405.15709150383, -31.1291297094622, 223.963951433279, 258.627733662929, 1477.30481736681 ),
      ( 373, 6269.45547097853, 100000000.000951, -4738.31725764113, 11212.0312955711, -15.0863809556079, 235.192504695341, 269.277150950749, 1429.78326981752 ),
      ( 395, 6178.24781761576, 100000000.000149, 1067.42185524042, 17253.2407074761, 0.647329683322735, 246.388840788314, 279.912058356278, 1385.73346376495 ),
      ( 417, 6089.18709108701, 100000000.000021, 7104.73940013867, 23527.2927728862, 16.1018678523866, 257.455280787228, 290.429157164126, 1344.86885194474 ),
      ( 439, 6002.1694288403, 100000000.000003, 13370.007025617, 30030.6496791725, 31.2977011160484, 268.313873145449, 300.745955540193, 1306.93411683848 ),
      ( 461, 5917.11390396444, 100000000.000001, 19858.0562981862, 36758.1872780887, 46.2487598546334, 278.906079133088, 310.800536827874, 1271.70198596737 ),
      ( 483, 5833.95757623606, 99999999.9999987, 26562.6072574392, 43703.6300868367, 60.9645801822053, 289.191051142766, 320.549766098632, 1238.96930686196 ),
      ( 505, 5752.65129789954, 99999999.9987041, 33476.6478894915, 50859.9368844662, 75.4518691895355, 299.143184706997, 329.966697932599, 1208.55319655434 ),
      ( 527, 5673.15619954142, 99999999.999805, 40592.753149102, 58219.6254724637, 89.7156181951868, 308.749417074921, 339.037707596288, 1180.2876508554 ),
      ( 549, 5595.44077891773, 99999999.9999759, 47903.3414275493, 65775.0344631993, 103.759871006151, 318.006578587574, 347.759677509799, 1154.02074152358 ),
      ( 571, 5519.47852038, 99999999.9999974, 55400.8720195173, 73518.5256403729, 117.588234884991, 326.918976799726, 356.137430619831, 1129.61239478083 ),
      ( 593, 5445.245978814, 100000000, 63077.9902125391, 81442.6334975392, 131.204203994195, 335.49630292213, 364.181504627286, 1106.93268239528 ),
      };

      // TestData contains:
      // 0. Temperature (Kelvin)
      // 1. Pressure (Pa)
      // 2. Saturated liquid density (mol/m³
      // 3. Saturated vapor density (mol/m³)
      _testDataSaturatedProperties = new (double temperature, double pressure, double saturatedLiquidMoleDensity, double saturatedVaporMoleDensity)[]
      {
      ( 213.04875, 14.6766336390724, 6622.79221699954, 0.00828584600027596 ),
      ( 260.3275, 780.079309670281, 6290.8144570534, 0.360921037677404 ),
      ( 307.60625, 10188.2177938239, 5952.49265903476, 4.02546522982985 ),
      ( 354.885, 60068.8267138863, 5595.1467853533, 21.1587958841282 ),
      ( 402.16375, 219236.66763684, 5200.51587410119, 72.3537926369574 ),
      ( 449.4425, 591179.060499773, 4735.17556125592, 194.45418637264 ),
      ( 496.72125, 1309292.89258039, 4113.27127121456, 473.761966302047 ),
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
