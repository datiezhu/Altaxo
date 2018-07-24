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
	/// Tests and test data for <see cref="Mixture_Heptane_Hydrogen"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Heptane_Hydrogen : MixtureTestBase
    {

    public Test_Mixture_Heptane_Hydrogen()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("142-82-5", 0.5), ("1333-74-0", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.6013571236531, 1000.00000000694, 6.60215042730588E-06, 19.0636999458959, 1 ),
      ( 250, 0.481085818584615, 1000.00000000301, 6.35341533536783E-06, 20.1253705767019, 1 ),
      ( 250, 4.810583103368, 10000.0000243477, 6.35365510794626E-05, 20.1255517475012, 1 ),
      ( 250, 48.0783286069686, 100000, 0.000635606648099925, 20.1273633875661, 1 ),
      ( 300, 0.400905061830809, 1000.00000000106, 5.8220884203617E-06, 20.6699827283875, 1 ),
      ( 300, 4.00884055694672, 10000.0000106825, 5.82219236421092E-05, 20.6701327703589, 1 ),
      ( 300, 40.0674073886403, 100000, 0.00058232379960447, 20.671633122324, 1 ),
      ( 300, 398.581957257919, 1000000.00449827, 0.00583427982493903, 20.6866292567906, 1 ),
      ( 300, 3783.08873734424, 10000000.0000007, 0.0597356387071406, 20.8353465801995, 1 ),
      ( 350, 0.343633099085558, 1000.00000000002, 5.27224044237221E-06, 20.9311537041443, 1 ),
      ( 350, 3.43616794354599, 10000.000002951, 5.27228225884061E-05, 20.9312819713681, 1 ),
      ( 350, 34.3453817781167, 99999.9999999998, 0.000527270367447083, 20.9325645606971, 1 ),
      ( 350, 341.830991326567, 1000000.00038329, 0.00527722620692995, 20.9453817903197, 1 ),
      ( 350, 3262.16870499714, 10000000.0000018, 0.0533940512502843, 21.0723768757353, 1 ),
      ( 400, 0.300679112316211, 1000.00000000015, 4.77131724695447E-06, 21.0528679785244, 1 ),
      ( 400, 3.00666201177789, 10000.0000015303, 4.77132906917833E-05, 21.0529799448283, 1 ),
      ( 400, 30.0537147179393, 100000.015294161, 0.000477144990306857, 21.0540995170963, 1 ),
      ( 400, 299.252266759568, 1000000.00005579, 0.00477282996393292, 21.0652859157225, 1 ),
      ( 400, 2869.15888512547, 10000000.0000001, 0.0479745423317564, 21.1760283043686, 1 ),
      ( 400, 20235.633128725, 100000000.000003, 0.485896413712032, 22.1335816265013, 1 ),
      ( 500, 0.240543468204735, 1000.00000000004, 3.95084289845608E-06, 21.1632430517836, 1 ),
      ( 500, 2.40534934428093, 10000.000000407, 3.95083285776276E-05, 21.1633320047486, 1 ),
      ( 500, 24.0449442476109, 100000.004057049, 0.000395073026913045, 21.1642214453758, 1 ),
      ( 500, 239.598078367846, 1000000.00000306, 0.00394977788199011, 21.1731068602305, 1 ),
      ( 500, 2314.14147954203, 10000000.0024115, 0.0394543276201124, 21.260990011268, 1 ),
      ( 500, 17321.6374432804, 100000000.000003, 0.388693409321024, 22.0319105928207, 1 ),
      ( 600, 0.200453016403271, 1000.00000000001, 3.33345151681336E-06, 21.2723534324119, 1 ),
      ( 600, 2.00447016193434, 10000.0000001412, 3.33343666414413E-05, 21.2724268254682, 1 ),
      ( 600, 20.0386903275727, 100000.001406747, 0.000333328635166717, 21.2731606782723, 1 ),
      ( 600, 199.788040056609, 1000000.00000031, 0.00333182061090704, 21.2804913963274, 1 ),
      ( 600, 1940.12457706011, 10000000.0011267, 0.0332001374480619, 21.3529897539968, 1 ),
      ( 600, 15153.4594849493, 100000000.009832, 0.322824653812515, 21.9959963842281, 1 ),
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
      ( 300, 0.400971987773408, 999.998172386293, -0.000163368086024219, 89.1031419046079, 2 ),
      ( 300, 4.01563359278705, 10000.0000114461, -0.00163580076829254, 89.1378851587581, 2 ),
      ( 350, 0.343668572635408, 999.999999075208, -0.000100228909046999, 100.356060340109, 2 ),
      ( 350, 3.43979134168173, 9999.99999970014, -0.00100298833433564, 100.373683580252, 2 ),
      ( 350, 34.7140529906746, 99999.9999923157, -0.0101008163013292, 100.551844790582, 2 ),
      ( 400, 0.300699531527467, 999.999999800844, -6.54151563015536E-05, 111.60847508745, 2 ),
      ( 400, 3.00876751859799, 9999.9979595865, -0.000654389539126282, 111.618407885811, 2 ),
      ( 400, 30.2667742876747, 99999.9984909091, -0.00656786742358256, 111.71840332547, 2 ),
      ( 500, 0.240551331414218, 999.999999999215, -3.09390835085743E-05, 132.363007913417, 2 ),
      ( 500, 2.40618334632346, 9999.99988737458, -0.000309393110597475, 132.366960710065, 2 ),
      ( 500, 24.1290475693658, 99999.9999999699, -0.00309414084249362, 132.406602154545, 2 ),
      ( 500, 248.224476284921, 1000000.00000081, -0.0309421029843315, 132.8137466251, 2 ),
      ( 500, 9956.79970255894, 100000000.000787, 1.41587554398779, 137.709811034909, 1 ),
      ( 600, 0.200456343187596, 999.999999998771, -1.54765561797533E-05, 150.060066899532, 2 ),
      ( 600, 2.00484262417695, 9999.99998774649, -0.000154733361826974, 150.062021172315, 2 ),
      ( 600, 20.0763240487477, 100000, -0.00154410574784194, 150.0815906582, 2 ),
      ( 600, 203.528602280845, 999999.999203884, -0.0151102175824643, 150.279638614615, 2 ),
      ( 600, 2249.62789805514, 10000000.0000104, -0.108949346747418, 152.122051420628, 1 ),
      ( 600, 9147.18502733581, 99999999.9999929, 1.19141998565336, 154.582678324299, 1 ),
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
      ( 200, 7653.55600672323, 9999999.99999996, -0.21427588636924, 152.242140808158, 1 ),
      ( 200, 8011.07479934281, 99999999.9999997, 6.50658764332991, 157.555902536004, 1 ),
      ( 250, 7193.94353545892, 1000000.00003289, -0.933126152912813, 162.682677760676, 1 ),
      ( 250, 7255.41358839091, 10000000.0000003, -0.336927282105028, 163.231908903601, 1 ),
      ( 250, 7701.03806926925, 99999999.9999997, 5.24703678206763, 168.032085354281, 1 ),
      ( 300, 0.401357261440413, 999.999941104673, -0.00112541651406703, 157.590638568144, 2 ),
      ( 300, 6780.18699301331, 1000000.0072573, -0.940871015802705, 178.391414548528, 1 ),
      ( 300, 6864.24062404711, 10000000.000008, -0.415950591970836, 178.900265741036, 1 ),
      ( 300, 7416.49205149902, 99999999.999999, 4.40559559074224, 183.359324019641, 1 ),
      ( 350, 0.343846723504339, 999.999992707991, -0.000620567444595076, 179.807613521174, 2 ),
      ( 350, 3.45791210121061, 9999.99999999781, -0.00624037459752305, 180.080983252957, 2 ),
      ( 350, 6347.36250360639, 1000000.00000008, -0.94586202643618, 197.147528855654, 1 ),
      ( 350, 6466.35637886314, 10000000.000163, -0.468582733020861, 197.596737503635, 1 ),
      ( 350, 7150.11721949477, 99999999.9999996, 3.80598195625814, 201.779413664991, 1 ),
      ( 400, 0.30079250146785, 999.999998734823, -0.000376757979174264, 202.178560669546, 2 ),
      ( 400, 3.0182005948531, 9999.98634874795, -0.00378000861862291, 202.325451316545, 2 ),
      ( 400, 31.2927051334252, 99999.9999999996, -0.0391397157558927, 203.855941181506, 2 ),
      ( 400, 5869.94052227333, 1000000.00387559, -0.948776452587129, 217.016857249612, 1 ),
      ( 400, 6049.04330117185, 10000000.0026126, -0.502931024634872, 217.328215321447, 1 ),
      ( 400, 6898.04817791786, 99999999.9999998, 3.35890222476611, 221.218497581612, 1 ),
      ( 500, 0.240583630420059, 999.999999926257, -0.000167467860658773, 243.568221173281, 2 ),
      ( 500, 2.40947373019364, 9999.99923883108, -0.00167684996567921, 243.620596487797, 2 ),
      ( 500, 24.4701195399627, 99999.9999906369, -0.0169915598421217, 244.153541793088, 2 ),
      ( 500, 301.228353375499, 999999.999930171, -0.201458502479675, 250.82358456163, 2 ),
      ( 500, 5090.52907268965, 10000000.000139, -0.52746888002719, 255.519346762281, 1 ),
      ( 500, 6429.72086139511, 100000000, 2.74111638093722, 258.404723905691, 1 ),
      ( 600, 0.200470100385747, 999.999999992646, -8.63805820112992E-05, 278.850471411179, 2 ),
      ( 600, 2.00626165576042, 9999.99992534871, -0.000864203890971278, 278.873259645305, 2 ),
      ( 600, 20.2208409327288, 99999.999999969, -0.00868225829220011, 279.103239025188, 2 ),
      ( 600, 220.587242666235, 999999.999999999, -0.0912766248903139, 281.626690305619, 2 ),
      ( 600, 3833.29420008295, 9999999.99999999, -0.477074356417876, 290.11898884107, 1 ),
      ( 600, 6005.01633450829, 100000000, 2.33808889927382, 290.851647373342, 1 ),
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
