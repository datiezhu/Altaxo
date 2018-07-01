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
	/// Tests and test data for decamethyltetrasiloxane.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Decamethyltetrasiloxane : FluidTestBase
    {

    public Test_Decamethyltetrasiloxane()
      {
      _fluid = Decamethyltetrasiloxane.Instance;
      // TestData contains:
      // 0. Temperature (Kelvin)
      // 1. Pressure (Pa)
      // 2. Saturated liquid density (mol/m³
      // 3. Saturated vapor density (mol/m³)
      _testDataSaturatedProperties = new double[][]
      {
        new double[]{254.475,0.723253676337255,2872.56124600905,0.000341832105957102,},
        new double[]{303.75,81.1481570569515,2713.51267367598,0.0321385130700514,},
        new double[]{353.025,1768.55297090016,2554.78108628801,0.604363758460658,},
        new double[]{402.3,14704.9496705884,2389.97218587643,4.47073501503346,},
        new double[]{451.575,67490.2331043696,2210.50557673405,19.0029572423843,},
        new double[]{500.85,212475.228077906,2002.44865658291,58.5720385780658,},
        new double[]{550.125,525420.15432413,1736.90282517324,155.693253831976,},
      };
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
		   _testDataEquationOfState = new double[][]
      {
      new double[]{207.206697884451,2.72258341291526E-07,0.0004690500000362,-77536.3670021592,-75813.5547723808,-105.425710804604,313.98433115616,322.298793355107,75.4455973596414,},
      new double[]{225,2.50727785900584E-07,0.000469050000029209,-71797.5836733126,-69926.8297066375,-78.1797817244187,331.304701890881,339.619164077376,78.5651979054032,},
      new double[]{275,2.05140915453887E-07,0.000469050000016524,-53922.7815533802,-51636.3044799075,-4.95004484719607,384.138108708483,392.452570867106,86.7107404761004,},
      new double[]{325,1.73580774488472E-07,0.000469050000009829,-33409.9509288371,-30707.7507491681,64.854073253995,435.620331094461,443.934793232804,94.1464303908364,},
      new double[]{375,1.50436671163523E-07,0.000469050000006172,-10459.4533923032,-7341.53010685428,131.650593466332,481.30453456486,489.618996689943,101.039559278292,},
      new double[]{425,1.32738239231665E-07,0.000469050000004073,14612.569822575,18146.2162135404,195.401876360757,520.562015923193,528.876478040097,107.495927087424,},
      new double[]{475,1.18765792980331E-07,0.000469050000002804,41505.7822095022,45455.1517058224,256.116108160992,554.371695115457,562.686157227342,113.588941542745,},
      new double[]{525,1.07454765067926E-07,0.000469050000001989,69981.7817948782,74346.8743964532,313.922977504884,584.097684871864,592.412146980598,119.372844618707,},
      new double[]{575,9.81108724474821E-08,0.00046905000000144,99867.9731935396,104648.788900306,369.036842060444,610.95805567399,619.272517780673,124.889459093878,},
      new double[]{205.2,3038.15572835916,18.9137174849992,-148122.974207315,-148122.967981921,-454.425019241839,370.987306206721,478.320228637325,1287.99282227516,},
      new double[]{225,2970.38678892579,18.9137179801495,-138578.244484681,-138578.238117255,-410.025938074218,383.561689690589,486.434255935522,1202.45645236351,},
      new double[]{250,2887.22959571082,18.9137197959098,-126242.263996451,-126242.257445631,-358.050502743701,402.978028742497,501.088599238664,1101.81053838245,},
      new double[]{275,2805.88895550696,18.9137160829569,-113500.198667339,-113500.191926617,-309.485829335738,424.37762318541,518.598710705808,1008.8685121361,},
      new double[]{350,0.00649964267526576,18.913732467624,-22220.4968612143,-19310.5318965864,10.4562539431042,459.285574299081,467.601284190957,97.6500975340944,},
      new double[]{400,0.00568711874790036,18.91372799444,1831.17751957503,5156.89108963594,75.7366119473413,501.694927983371,510.01021644009,104.315039769799,},
      new double[]{450,0.00505518072168087,18.9137253574951,27847.9220708837,31589.3759177021,137.96054610191,538.063266553546,546.378297032878,110.582570778492,},
      new double[]{500,0.00454964247886961,18.9137237132789,55558.0529419866,59715.2419521389,197.19844336798,569.662709285318,577.977580030053,116.515544078554,},
      new double[]{550,0.00413602657409873,18.9137226334229,84757.0337453034,89329.9547105282,253.628809324646,597.821775696465,606.136544434314,122.161461114285,},
      new double[]{600,0.00379135010998139,18.9137218916794,115300.915729956,120289.566564889,307.489315051129,623.609901178249,631.924602195227,127.557533246627,},
      new double[]{205.2,3038.15818260146,999.999998691363,-148123.050005055,-148122.72085827,-454.425388627194,370.987496556878,478.32018201357,1287.99757012325,},
      new double[]{225,2970.38955863731,999.999999453403,-138578.328914839,-138577.99225866,-410.026313320078,383.561873735021,486.434174035096,1202.4616950295,},
      new double[]{250,2887.23283018119,999.999997506059,-126242.360623853,-126242.01427146,-358.050889254099,402.978201038037,501.088464519156,1101.81646989296,},
      new double[]{275,2805.89274685041,999.999994527662,-113500.309308931,-113499.952916124,-309.486231669695,424.377780159194,518.598509690879,1008.87521581461,},
      new double[]{300,2725.5346620029,1000.0008906474,-100302.266626379,-100301.899725563,-263.56283971799,446.244671863708,537.351197986642,922.556607905909,},
      new double[]{341.330163054778,2592.74714628546,1000.00000139354,-77446.1091454025,-77445.7234541087,-192.229861321079,481.1366137883,568.552897563678,791.010154449522,},
      new double[]{342.330163054778,0.352001030282746,1000.00000000137,-25727.9223596112,-22887.0215838432,-32.8514272947872,452.229947846712,460.615385627994,96.4149904489132,},
      new double[]{350,0.344241862011953,1000.00000000111,-22231.8495105377,-19326.915188615,-22.5668783191881,459.317384444545,467.698287509903,97.4878296844516,},
      new double[]{400,0.301022409787512,1000.00000000029,1822.59217697504,5144.6039856845,42.7244603323233,501.719876813638,510.078228272265,104.203208062774,},
      new double[]{450,0.26747655485756,1000.00000000009,27841.2821777711,31579.9276098264,104.955105025093,538.080553936877,546.425157010508,110.502494432521,},
      new double[]{500,0.240673465896428,1000.00000000003,55552.7503669752,59707.7576349154,164.197153201809,569.674157512787,578.010268853666,116.456503056977,},
      new double[]{550,0.21876099266826,1000.00000000001,84752.6578250404,89323.85668368,220.630168240028,597.829265430722,606.159962494662,122.11704379059,},
      new double[]{600,0.200510101343821,1000,115297.196780881,120284.476689962,274.492431890417,623.614820812406,631.941927201112,127.523721581402,},
      new double[]{205.2,3038.18069569151,9999.99999733085,-148123.745302953,-148120.453859487,-454.428777083882,370.989242699557,478.319754490196,1288.04112268534,},
      new double[]{225,2970.4149653894,10000.0000006133,-138579.103393743,-138575.736860744,-410.029755518924,383.56356203324,486.433422962651,1202.50978625735,},
      new double[]{250,2887.26249988173,10000.0000004762,-126243.24698011,-126239.783491771,-358.054434757429,402.979781567656,501.087229022115,1101.87087932049,},
      new double[]{275,2805.92752427412,9999.99999504396,-113501.324205066,-113497.760321143,-309.489922289438,424.379220156885,518.596666203833,1008.93670746423,},
      new double[]{300,2725.5756433625,10000.0008903976,-100303.431500168,-100299.762550119,-263.566722731757,446.245947433438,537.348574094816,922.626180138424,},
      new double[]{325,2645.42349217924,10000.0030678494,-86632.4167872764,-86628.6366730158,-219.805897981634,467.651376986545,556.319434194539,841.669449734427,},
      new double[]{350,2564.68461394036,9999.99999914393,-72490.5987546602,-72486.6996397574,-177.893215958951,488.106551152606,574.952292474555,764.859028815642,},
      new double[]{375,2482.50456593307,9999.9999999683,-57889.7757978881,-57885.7476079152,-137.605920660872,507.412524043115,593.025684889812,691.131279991337,},
      new double[]{390.969881483843,2428.79983819507,10000.0000004064,-48329.2022346619,-48325.0849748354,-112.640385883598,519.12975198059,604.275784627554,645.219233254069,},
      new double[]{391.969881483843,3.10660513066003,10000.0000040116,-2264.61821437304,954.329667132261,13.0859699814222,495.591518929461,504.395892971773,102.045586688213,},
      new double[]{400,3.04173679246896,10000.0000032485,1742.61783491786,5030.21327315811,23.379132558442,501.954555859018,510.726984620894,103.157058732529,},
      new double[]{450,2.6934775746696,10000.0000009553,27779.7633816607,31492.4358371505,85.6733818596104,538.241998074351,546.866362149501,109.759193092098,},
      new double[]{500,2.41844153197474,10000.0000003182,55503.7881606904,59638.6824172213,144.954363095119,569.780553782034,578.315581893499,115.91102064453,},
      new double[]{550,2.19521854349288,10000.0000001156,84712.3398209614,89267.6949266104,201.412060098336,597.898630265333,606.377535706759,121.707883923579,},
      new double[]{600,2.01017793123454,10000.0000000444,115262.980499301,120237.664503469,255.290630130131,623.66026493472,632.102313576003,127.212873654496,},
      new double[]{205.2,3038.40573597931,100000.000001477,-148130.695005876,-148097.783009029,-454.462651565175,371.006700610507,478.315497076991,1288.47646528744,},
      new double[]{225,2970.66891292338,99999.9999999721,-138586.844060791,-138553.181608687,-410.064165517413,383.580441612445,486.425936161803,1202.99046115971,},
      new double[]{250,2887.55902546295,99999.9999909729,-126252.105022145,-126217.47369544,-358.08987474893,402.99558444242,501.074909012545,1102.41464519348,},
      new double[]{275,2806.27505234281,99999.9999539893,-113511.465727234,-113475.831301496,-309.526809377019,424.393619917688,518.578283248114,1009.55117033452,},
      new double[]{300,2725.98510006982,100000.000898036,-100315.070106641,-100278.386120035,-263.605528187476,446.258706706357,537.322413793472,923.321273009058,},
      new double[]{325,2645.90946733585,100000.003074749,-86645.8301288062,-86608.0359395834,-219.847181547595,467.66223062931,556.283048385735,842.45877701724,},
      new double[]{350,2565.26717437917,100000.000000322,-72506.1550901981,-72467.1727958784,-177.937676540927,488.115037792986,574.902019369009,765.761382523363,},
      new double[]{375,2483.21223594571,99999.9999990722,-57907.978028872,-57867.7076087478,-137.654476771731,507.417795194945,592.955829183445,692.172658974868,},
      new double[]{400,2398.75099380936,100000.000000456,-42865.7588875792,-42824.0705254863,-98.8247851673061,525.551087411182,610.452326749917,620.783834536358,},
      new double[]{425,2310.62833918342,100000.000000163,-27391.5979233368,-27348.3196520959,-61.3015631957709,542.606592454717,627.56606174801,550.821619827708,},
      new double[]{450,2217.15462091439,99999.999999676,-11491.4309670136,-11446.3281135381,-24.9488987453533,558.74987710017,644.618688784891,481.638033508159,},
      new double[]{466.036192224207,2153.29855875627,100000.000000026,-1066.61216286876,-1020.17178441154,-2.1840763610103,568.72746515113,655.755637172625,437.405140560222,},
      new double[]{467.036192224207,2149.1923895525,99999.9999998438,-410.592847197983,-364.063741434368,-0.777737090501023,569.341922668017,656.460710501733,434.646033842083,},
      new double[]{475,27.1790486689969,100000,40848.1303199739,44527.4349644397,95.2585020465854,556.064298740132,567.746526521256,105.841100501893,},
      new double[]{525,24.0410469232935,100000,69464.3683024679,73623.9209342258,153.476700235508,585.158051322101,595.713354417176,113.843174385302,},
      new double[]{575,21.6419904681481,100000,99442.7262395877,104063.373316721,208.841502823034,611.62995972555,621.559474679964,120.810873501418,},
      new double[]{205.2,3038.40904784268,101324.999997315,-148130.797276484,-148097.449232029,-454.463150135931,371.006957582021,478.315434640133,1288.4828720143,},
      new double[]{225,2970.6726499672,101324.999998894,-138586.957964673,-138552.849527986,-410.064671946344,383.580690071159,486.425826263909,1202.99753454154,},
      new double[]{250,2887.56338865267,101324.999988508,-126252.235357303,-126217.145218542,-358.090396300263,402.995817062956,501.074728108639,1102.42264618037,},
      new double[]{275,2806.28016538914,101324.999951583,-113511.614932058,-113475.508415966,-309.527352177675,424.393831911152,518.578013313805,1009.56021044101,},
      new double[]{300,2725.99112334284,101325.00089948,-100315.241315386,-100278.071348091,-263.606099155267,446.258894599547,537.322029720832,923.331497786198,},
      new double[]{325,2645.91661484924,101325.003077957,-86646.0274130449,-86607.7325542768,-219.847788892241,467.662390551086,556.282514356203,842.470385693285,},
      new double[]{350,2565.27574025745,101324.999999622,-72506.3838459922,-72466.885168166,-177.938330503284,488.11516300762,574.901281870118,765.77465006778,},
      new double[]{375,2483.22263784846,101324.999999962,-57908.2456176809,-57867.4417854124,-137.655190794702,507.41787331883,592.954805108972,692.187965364462,},
      new double[]{400,2398.76384780142,101324.999999867,-42866.0757112599,-42823.8352047203,-98.8255777886848,525.551096503124,610.45088247059,620.80170830482,},
      new double[]{425,2310.64459579659,101324.999999517,-27391.9792878394,-27348.1278880249,-61.3024612451955,542.606493875136,627.563973255896,550.842807692623,},
      new double[]{450,2217.17582965667,101325.00000037,-11491.9005217361,-11446.2004926067,-24.9499431653955,558.749600528792,644.61555689123,481.663623039887,},
      new double[]{466.578000795012,2151.10164809366,101324.999999935,-711.810819066067,-664.707047800897,-1.42309907666156,569.060019782987,656.133240547897,435.939656144573,},
      new double[]{467.578000795012,2146.98717293708,101324.999999992,-55.4150548440893,-8.22101415432611,-0.0175818881848987,569.674032134072,656.839094255305,433.180618154966,},
      new double[]{475,27.5681932947793,101325,40838.536218733,44513.9674998678,95.1282437823735,556.090814891597,567.832542862477,105.725894196817,},
      new double[]{525,24.3762874963551,101325,69457.0867430105,73613.7902613437,153.353149565237,585.173871105099,595.764975991871,113.764943616742,},
      new double[]{575,21.9390796075596,101325,99436.8677611529,104055.33862741,208.721777753606,611.639656045571,621.593455155713,120.754844858364,},
      new double[]{225,2973.19648593043,999999.996399514,-138663.841587942,-138327.503239255,-410.40707530325,383.748898119682,486.353427797069,1207.77368217273,},
      new double[]{250,2890.50730673029,1000000.00000177,-126340.138348803,-125994.178316918,-358.442783853285,403.153370197273,500.955148233743,1107.81981686819,},
      new double[]{275,2809.72601031479,999999.999998905,-113612.145149729,-113256.238560856,-309.893789892151,424.537590224457,518.399544546413,1015.65103036101,},
      new double[]{300,2730.04450047094,1000000.00094758,-100430.4565302,-100064.162134349,-263.991147085948,446.386641129253,537.068489121884,930.210262601127,},
      new double[]{325,2650.71763675819,1000000.00312753,-86778.5854759762,-86401.3291476294,-220.25681607617,467.771706676933,555.931033080064,850.265427334811,},
      new double[]{350,2571.01554213064,999999.999999726,-72659.781090555,-72270.8297273241,-178.377972253219,488.201839699442,574.418067732561,774.661710908617,},
      new double[]{375,2490.17002629186,1000000.00000044,-58087.2062365383,-57685.627232116,-138.134060803708,507.47418881267,592.288099240494,702.407252489826,},
      new double[]{400,2407.3102805046,1000000.00000019,-43077.1914469767,-42661.7900722147,-99.3553916905779,525.563511462598,609.518997697191,632.682228312002,},
      new double[]{425,2321.38361233556,999999.999999957,-27644.7856755738,-27214.0081019379,-61.8998721839133,542.551607493993,626.233499269338,564.839267292313,},
      new double[]{450,2231.05142340273,1000000.00000038,-11800.781251162,-11352.5620888824,-25.6397382969588,558.586014167872,642.657505949762,498.417254440202,},
      new double[]{475,2134.53493323868,999999.999999652,4450.66077804629,4919.14690325688,9.54689707862439,573.858844103889,659.120944295344,433.145032749363,},
      new double[]{500,2029.32802839936,1000000.00014939,21115.3776943645,21608.1516501542,43.784526412449,588.60201941514,676.16276290206,368.83500975793,},
      new double[]{525,1911.45064162629,999999.999947848,38218.5218693586,38741.6847374811,77.2187165255891,603.107396237756,695.002159100983,304.89467599371,},
      new double[]{550,1772.74283486066,999999.999723789,55839.4185276158,56403.516139553,110.079470404351,617.837183650865,719.603744214704,238.781337186343,},
      new double[]{575,1586.06166102112,1000000,74290.7684657516,74921.2609772195,142.997125696666,634.142086372935,771.741816573094,160.562693736773,},
      new double[]{589.874309079093,1375.50563388176,999999.999999997,86539.213027849,87266.218410719,164.184680178808,648.554311092258,970.724843540895,86.4435040312984,},
      new double[]{590.874309079093,1346.931947854,1000000.00000237,87526.9752122388,88269.4032180454,165.883898849464,650.265669995148,1042.1325493861,78.1077804989022,},
      new double[]{600,340.907688242736,1000000,108573.537737117,111506.883139664,205.112683201506,639.42025743258,773.320028973054,68.7321137129204,},
      new double[]{225,2973.7586074559,1201199.99779825,-138680.953968748,-138277.020712436,-410.483441309353,383.786473440877,486.33779537164,1208.83720856115,},
      new double[]{244,2910.77743302675,1201199.99999919,-129350.865825698,-128938.192569965,-370.643102000675,398.280787404876,497.093658766581,1132.234180604,},
      new double[]{263,2849.02599805124,1201199.99999997,-119797.048178471,-119375.430404249,-332.908437546708,414.170633426415,509.756435341554,1060.25634603908,},
      new double[]{282,2788.13744923878,1201199.99999937,-109991.379437074,-109560.554191953,-296.881521973202,430.689410642011,523.514841069943,992.500761403082,},
      new double[]{301,2727.77427037429,1201200.0000009,-99919.1270285979,-99478.7680102411,-262.28843390614,447.283961718063,537.76873528594,928.447443159168,},
      new double[]{320,2667.61629079056,1201200.00000038,-89575.1443423124,-89124.8546945294,-228.93656357916,463.585922885323,552.104677145741,867.554065349211,},
      new double[]{339,2607.34905996955,1201199.99999982,-78960.6488705053,-78499.9510612086,-196.686015112448,479.370483287223,566.26120830729,809.311058688226,},
      new double[]{358,2546.65098253628,1201200.00000064,-68080.6778234657,-67608.9995258749,-165.430762099543,494.516965316008,580.093630064221,753.26365577561,},
      new double[]{377,2485.17872806324,1201199.99999985,-56942.1740339306,-56458.8285157851,-135.086396991366,508.97719235742,593.543569867336,699.016308940552,},
      new double[]{396,2422.55063080752,1201199.99999974,-45552.5911271349,-45056.7500971377,-105.58223739047,522.752379758042,606.615050667968,646.231912881758,},
      new double[]{415,2358.3275170813,1201200.0000001,-33918.8949962213,-33409.5509838651,-76.8562566549274,535.877142263201,619.356917006846,594.632661466267,},
      new double[]{434,2291.98981822774,1201200.00000014,-22046.847704242,-21522.7616064561,-48.8518071613522,548.408837818048,631.851085287144,544.004505550762,},
      new double[]{453,2222.9086700914,1201199.99999952,-9940.46442236313,-9400.0913444399,-21.5154223058419,560.420799185584,644.207040882014,494.202778198447,},
      new double[]{472,2150.30571545321,1201200.00000008,2398.4965917007,2957.11483438481,5.20488174798729,571.99850982924,656.566138183347,445.150489288195,},
      new double[]{491,2073.1875435312,1201199.99999965,14971.2795235315,15550.6771780021,31.3615010512104,583.238382557096,669.127567995368,396.809815648598,},
      new double[]{510,1990.21418989161,1201200.00000016,27783.7633663635,28387.3164945147,57.0106404501571,594.249968126617,682.229453645448,349.088081032014,},
      new double[]{529,1899.38055550315,1201200.00000009,40850.6849722757,41483.1016812495,82.2202955568694,605.165980585227,696.577892044822,301.610694924025,},
      new double[]{548,1797.12671805316,1201199.99999836,54207.2660775416,54875.6663566825,107.091213902103,616.177195449059,713.923426578702,253.246637000811,},
      new double[]{567,1675.39484069505,1201199.99999996,67945.4649032017,68662.4302243445,131.820771567438,627.664233763725,739.615020610854,201.080932793829,},
      new double[]{586,1507.0717240376,1201199.99999864,82382.4781111953,83179.5204687949,156.999169402257,640.900950328962,801.012821765337,136.964656231311,},
      new double[]{225,2997.36968288083,9999999.99884229,-139395.695077036,-136059.43660483,-413.725197270809,385.400553319245,485.835598761587,1253.44246319688,},
      new double[]{244,2937.12522757269,10000000,-130137.430809033,-126732.741112412,-373.936472324674,399.824092772539,496.312989399819,1180.83586781418,},
      new double[]{263,2878.45059898351,10000000.0000016,-120661.851713529,-117187.760477246,-336.271836112578,415.629154653785,508.658080357752,1113.10793647655,},
      new double[]{282,2821.0359521638,9999999.99999795,-110941.941110425,-107397.144032483,-300.333835157735,432.054576522555,522.050801891043,1049.89236194291,},
      new double[]{301,2764.61597519139,9999999.99822918,-100964.234651365,-97347.0957474569,-265.849513686559,448.550920227011,535.881398344749,990.729393534864,},
      new double[]{320,2708.96155857516,9999999.99785611,-90725.0601875488,-87033.6087654342,-232.627727513946,464.750046929498,549.723542278983,935.155114420355,},
      new double[]{339,2653.87317535697,9999999.99802514,-80227.3981311177,-76459.3205564687,-200.530673213598,480.423905661473,563.297860883047,882.751785451496,},
      new double[]{358,2599.17508708312,9999999.99852012,-69478.4468834265,-65631.0723651741,-169.455216307768,495.445850889848,576.435472416789,833.166935744925,},
      new double[]{377,2544.71054650048,9999999.99905355,-58487.8391975002,-54558.1191698901,-139.320827783796,509.759641014742,589.046446284358,786.115676922684,},
      new double[]{396,2490.33844377113,9999999.99946688,-47266.3976143633,-43250.8791516102,-110.061889185737,523.356474638584,601.09458933962,741.377096503087,},
      new double[]{415,2435.93178577685,9999999.99972998,-35825.3097665465,-31720.1044984355,-81.6228443204219,536.258562076895,612.578169052264,698.790171928822,},
      new double[]{434,2381.37823961184,9999999.99999963,-24175.6184793057,-19976.3695596425,-53.9551865910182,548.507465643381,623.515620403866,658.250581051707,},
      new double[]{453,2326.58276077626,10000000.0000003,-12327.9396694455,-8029.79039722109,-27.015615329111,560.155772962915,633.935358179349,619.707388689926,},
      new double[]{472,2271.47201658344,9999999.9999998,-292.335170899788,4110.09634795334,-0.764916534608353,571.261095113896,643.869128986539,583.157415497704,},
      new double[]{491,2215.99984217641,9999999.99999898,11921.7239539739,16434.3596544249,24.8327375519737,581.881733665305,653.348639090006,548.634841171308,},
      new double[]{510,2160.15234001859,9999999.99999973,24305.362956855,28934.6660919819,49.8102869674777,592.073613087522,662.405283306017,516.194328603261,},
      new double[]{529,2103.95064461158,9999999.99999978,36850.3295388432,41603.2927443303,74.1982313470661,601.888245353106,671.072488591626,485.887951733069,},
      new double[]{548,2047.44925962777,10000000.0000003,49549.074399587,54433.2003201572,98.0251099636931,611.371594920438,679.389414924893,457.739428955933,},
      new double[]{567,1990.72878117725,9999999.99999981,62394.8867550865,67418.1727469061,121.318019326023,620.563743428593,687.403827913767,431.722582231747,},
      new double[]{586,1933.88395519502,10000000.0000003,75382.0420005487,80552.9830868208,144.103091449427,629.499215125256,695.171607668105,407.752403986396,},
      new double[]{301,3001.71675196431,99999999.999998,-107381.790299473,-74067.5210785149,-291.776360357252,460.572264001329,535.49273362471,1396.92680443276,},
      new double[]{320,2963.10201543645,99999999.9999968,-97518.539400999,-63770.1232212518,-258.606122355802,476.379409276789,548.422579514568,1358.52831795521,},
      new double[]{339,2925.97035261294,100000000.000002,-87405.737673201,-53229.0414155988,-226.609627398532,491.709291113323,561.10676750273,1323.14030088631,},
      new double[]{358,2890.19771710175,100000000.000001,-77050.446737117,-42450.7377247246,-195.677302537628,506.423829040065,573.371709092032,1290.43954103105,},
      new double[]{377,2855.67938253322,100000000.000001,-66462.1215874379,-31444.1848359763,-165.723443694655,520.455862477603,585.122434021029,1260.14542922531,},
      new double[]{396,2822.32596669801,100000000,-55651.3659879137,-20219.597588392,-136.678226125987,533.788264875495,596.31984017163,1232.01552821163,},
      new double[]{415,2790.06039115472,99999999.9999995,-44629.0628949972,-8787.54479846145,-108.482529153527,546.438000615154,606.963251656545,1205.8402496186,},
      new double[]{434,2758.81555225237,100000000.000001,-33405.789502603,2841.65006194445,-81.0846024457447,558.444108077249,617.07729056389,1181.43784529382,},
      new double[]{453,2728.53252175507,99999999.9999999,-21991.4449430463,14658.2923068059,-54.4379349947491,569.858747480005,626.702139598032,1158.65007035007,},
      new double[]{472,2699.15913793182,100000000.00232,-10395.0345888155,26653.5405014783,-28.4999067054415,580.740633710912,635.886431538191,1137.33853143627,},
      new double[]{491,2670.64888500855,100000000.000498,1375.43230985788,38819.5158664192,-3.23094571351741,591.150324095277,644.682159197213,1117.38162849159,},
      new double[]{510,2642.95998821404,100000000.000097,13312.9648678267,51149.3303239951,21.4059913070938,601.146943984907,653.14113089647,1098.67198713303,},
      new double[]{529,2616.05467280938,100000000.000018,25411.5580542593,63637.0588587313,45.445736342374,610.78601798617,661.312599724042,1081.11429500877,},
      new double[]{548,2589.89855099988,100000000.000003,37666.1246249808,76277.675630095,68.9211037619124,620.118140521118,669.241775124028,1064.62347554695,},
      new double[]{567,2564.46011074156,99999999.9999988,50072.4058591019,89066.9683329483,91.863011903589,629.188272226353,676.968988213841,1049.1231474366,},
      new double[]{586,2539.71028758857,99999999.9999995,62626.8716561301,102001.441459896,114.300565300956,638.035492053465,684.529332013509,1034.54432883125,},
      };
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
    public override void SaturatedData_Test()
    {
      base.SaturatedData_Test();
    }

    [Test]
    public override void EquationOfState_Test()
    {
      base.EquationOfState_Test();
    }

    [Test]
    public void ConstantsAndCharacteristicPoints_Test()
    {

    Assert.AreEqual(0.3106854,_fluid.MolecularWeight, "MolecularWeight");
    Assert.IsTrue(IsInToleranceLevel(205.2, _fluid.TriplePointTemperature, 1E-4, 0.01),"TriplePointTemperature");
    Assert.IsTrue(IsInToleranceLevel(0.000312727446056659, _fluid.TriplePointPressure, 1E-4, 0),"TriplePointPressure");
    Assert.IsTrue(IsInToleranceLevel(3038.15568104603, _fluid.TriplePointSaturatedLiquidMoleDensity, 1E-4, 0),"TriplePointSaturatedLiquidMoleDensity");
    Assert.IsTrue(IsInToleranceLevel(1.83296631905013E-07, _fluid.TriplePointSaturatedVaporMoleDensity, 1E-4, 0),"TriplePointSaturatedVaporMoleDensity");
    Assert.IsTrue(IsInToleranceLevel(599.4, _fluid.CriticalPointTemperature, 1E-4, 0.01),"CriticalPointTemperature");
    Assert.IsTrue(IsInToleranceLevel(1144024.61282446, _fluid.CriticalPointPressure, 1E-4, 0),"CriticalPointPressure");
    Assert.IsTrue(IsInToleranceLevel(864, _fluid.CriticalPointMoleDensity, 1E-4, 0),"CriticalPointLiquidMoleDensity");
    Assert.IsTrue(IsInToleranceLevel(467.590516735181, _fluid.NormalBoilingPointTemperature.Value, 1E-4, 0.01),"NormalBoilingPointTemperature");
    Assert.IsTrue(_fluid.NormalSublimationPointTemperature is null,"NormalSublimationPointTemperature");
    }
  }
}
