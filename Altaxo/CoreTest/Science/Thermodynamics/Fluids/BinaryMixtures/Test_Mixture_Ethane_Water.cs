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
	/// Tests and test data for <see cref="Mixture_Ethane_Water"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Ethane_Water : MixtureTestBase
    {

    public Test_Mixture_Ethane_Water()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-84-0", 0.5), ("7732-18-5", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 300, 0.401105638967227, 999.999999768708, -0.000482150327982219, 25.4240358331497, 2 ),
      ( 300, 55248.5376576175, 1000000.00015791, -0.992743477696171, 74.3573020192273, 1 ),
      ( 300, 55469.7818147381, 10000000.000159, -0.9277242071911, 73.8391903428974, 1 ),
      ( 300, 57476.3842474392, 100000000.000079, -0.302474832052254, 69.8241437394523, 1 ),
      ( 350, 0.34370792307702, 999.999999940704, -0.00020033081911676, 25.6210771678337, 2 ),
      ( 350, 3.44333370866972, 9999.99935034499, -0.00201636870673791, 25.9126718261827, 2 ),
      ( 350, 53977.0209400411, 1000000.00004112, -0.993633604415039, 70.0311926082647, 1 ),
      ( 350, 54196.6821281073, 9999999.99992259, -0.936594076557569, 69.7229611519326, 1 ),
      ( 350, 56164.4413418034, 100000000.000639, -0.388155459967763, 67.1426519449845, 1 ),
      ( 400, 0.30071568754809, 999.99999999402, -0.000104760860680819, 25.9901804372452, 2 ),
      ( 400, 3.01000344673748, 9999.99993786879, -0.00105036543465868, 26.0922252264382, 2 ),
      ( 400, 30.3962594139597, 99999.9999986997, -0.0107855696773447, 27.1967943130526, 2 ),
      ( 400, 51962.7227458551, 1000000.00015865, -0.994213463643324, 65.4143658046267, 1 ),
      ( 400, 52215.0684909604, 9999999.99999886, -0.94241428901594, 65.225710541398, 1 ),
      ( 400, 54402.0334555395, 100000000.003417, -0.447292380036711, 63.5810305087247, 1 ),
      ( 500, 0.240557153196349, 999.999999998936, -4.07626426593209E-05, 26.9571140824705, 2 ),
      ( 500, 2.40645502305223, 9999.99998918967, -0.000407881523235962, 26.9798114879483, 2 ),
      ( 500, 24.153874240139, 99999.9999999996, -0.00410449659963816, 27.2122653277045, 2 ),
      ( 500, 251.568973613107, 999999.999999981, -0.0438115479971038, 30.1171511578022, 2 ),
      ( 500, 46418.8045244066, 10000000.000008, -0.948178900789432, 58.0308018615956, 1 ),
      ( 500, 49819.5532714143, 99999999.9999951, -0.51716277715167, 57.3300391644938, 1 ),
      ( 600, 0.200460140822881, 999.999999999993, -2.00436204936794E-05, 28.0642792861208, 2 ),
      ( 600, 2.00496318410236, 9999.99999929778, -0.000200480161713711, 28.072524902784, 2 ),
      ( 600, 20.0859691890039, 99999.991165512, -0.00200920838958242, 28.155627570777, 2 ),
      ( 600, 204.660798722305, 999999.999793929, -0.0205446078182623, 29.0523331392481, 2 ),
      ( 600, 2761.24775882989, 10000000.0000012, -0.2740378974148, 47.24167595832, 2 ),
      ( 600, 43846.4394576288, 99999999.9999994, -0.542822346910052, 52.7934159020327, 1 ),
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
      ( 300, 0.400972649288034, 999.981260750522, -0.000156678862855059, 34.8425016491764, 2 ),
      ( 350, 0.343667333764219, 999.999999939608, -8.82877544128068E-05, 38.1137164481004, 2 ),
      ( 350, 3.43941035250829, 9999.99937444726, -0.000883997701695873, 38.1491544240694, 2 ),
      ( 400, 0.300698918328203, 999.999999982949, -5.50391056347379E-05, 41.5346104543964, 2 ),
      ( 400, 3.00848049065991, 9999.99982608325, -0.000550713009241062, 41.5516385489163, 2 ),
      ( 400, 30.2357297576371, 99999.9997181952, -0.00553957005776531, 41.7266562298814, 2 ),
      ( 500, 0.240551965852022, 999.999999998929, -2.52392411868051E-05, 48.2571693600633, 2 ),
      ( 500, 2.40606632842719, 9999.99998918707, -0.000252438343022959, 48.2631570593965, 2 ),
      ( 500, 24.1155774197947, 100000, -0.00252898648706153, 48.3234028682433, 2 ),
      ( 500, 246.906316466344, 1000000.00000695, -0.025760466781408, 48.9629439650103, 2 ),
      ( 500, 22995.5650538431, 100000000, 0.0460534191689188, 61.4176633019958, 1 ),
      ( 600, 0.200457522785764, 999.999999999896, -1.30237067000384E-05, 54.4349735232156, 2 ),
      ( 600, 2.00481023806124, 9999.99999895194, -0.000130245346136439, 54.4378154801496, 2 ),
      ( 600, 20.0716501938513, 99999.9893118911, -0.00130328010969202, 54.4662808167356, 2 ),
      ( 600, 203.118673222696, 999999.999888986, -0.0131143094544355, 54.7554505774487, 2 ),
      ( 600, 2324.13383972885, 10000000.0009919, -0.137507020224373, 58.0090914301265, 2 ),
      ( 600, 19638.4386572187, 99999999.9999999, 0.020727337771824, 64.3715779421112, 1 ),
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
      ( 300, 0.400936799142526, 999.999999846486, -7.33195905062235E-05, 44.3649539332972, 2 ),
      ( 300, 4.01201697142887, 9999.99842668935, -0.00073353248013469, 44.3731112110839, 2 ),
      ( 300, 40.3883775163877, 99999.9999850898, -0.00736937874943124, 44.4564281487522, 2 ),
      ( 300, 434.610379055315, 999999.999999994, -0.0775475645749245, 45.4692481905279, 2 ),
      ( 300, 12707.5035777161, 9999999.99991124, -0.68451128094064, 50.8001019421397, 1 ),
      ( 300, 17370.3564709298, 100000000.012542, 1.30799755511636, 52.5864518137495, 1 ),
      ( 350, 0.343650433282918, 999.999999998471, -4.51527176492096E-05, 50.6309632678064, 2 ),
      ( 350, 3.4379017915626, 9999.99977953779, -0.00045162029141256, 50.6358212010016, 2 ),
      ( 350, 34.5197126641596, 99999.9999998029, -0.00452555940549634, 50.6846870991495, 2 ),
      ( 350, 360.293987222772, 1000000.01318159, -0.0462374380551196, 51.2039746296227, 2 ),
      ( 350, 7263.36479477362, 10000000.0000006, -0.526892939785718, 57.8161810928125, 1 ),
      ( 350, 16271.5707572994, 100000000.000025, 1.11187304322081, 57.3976501412068, 1 ),
      ( 400, 0.30068925364192, 999.999999994488, -2.89391003229128E-05, 57.0870565132876, 2 ),
      ( 400, 3.00767597574895, 9999.99994446002, -0.00028941152461006, 57.0902401040142, 2 ),
      ( 400, 30.1553900719511, 99999.9999999952, -0.00289616135132235, 57.1221228169609, 2 ),
      ( 400, 309.711473181966, 1000000.00004668, -0.0291591432637745, 57.4457936806238, 2 ),
      ( 400, 4202.63440090016, 10000000, -0.284542686127919, 60.868933045301, 2 ),
      ( 400, 15238.0652202581, 99999999.9932064, 0.973220009254826, 62.7730830578294, 1 ),
      ( 500, 0.240547444248579, 999.999999999706, -1.24826777296561E-05, 69.5588512868912, 2 ),
      ( 500, 2.40574469158975, 9999.99999706307, -0.000124816181521216, 69.5605006925989, 2 ),
      ( 500, 24.084479768194, 99999.9999999999, -0.00124709403061451, 69.5769834850731, 2 ),
      ( 500, 243.553742342439, 999999.999746998, -0.0123557976994684, 69.7406344395011, 2 ),
      ( 500, 2680.38901876341, 10000000, -0.102576380187795, 71.1880898473078, 2 ),
      ( 500, 13389.8265967005, 100000000.001203, 0.796471670760944, 73.7961389328222, 1 ),
      ( 600, 0.200454726266035, 999.999999999983, -5.11315327704938E-06, 80.806381944413, 2 ),
      ( 600, 2.00463949002033, 9999.99999985588, -5.11198736190994E-05, 80.807386234209, 2 ),
      ( 600, 20.0555990702842, 99999.9986262614, -0.000510029098517282, 80.817418490444, 2 ),
      ( 600, 201.456883539319, 999999.999999891, -0.00497963736595866, 80.9166612652651, 2 ),
      ( 600, 2076.14337853853, 10000000.0000006, -0.0344900868484281, 81.7855889485429, 1 ),
      ( 600, 11848.0841796536, 100000000.002997, 0.691865944522704, 84.1396856200772, 1 ),
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
