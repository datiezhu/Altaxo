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
  /// Tests and test data for <see cref="Mixture_HCl_Nitrogen"/>.
  /// </summary>
  /// <remarks>
  /// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
  /// </remarks>
  [TestFixture]
  public class Test_Mixture_HCl_Nitrogen : MixtureTestBase
  {

    public Test_Mixture_HCl_Nitrogen()
    {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[] { ("7647-01-0", 0.5), ("7727-37-9", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 200, 0.601371322217698, 999.999999986321, -2.15682321178486E-05, 20.7932617599331, 2 ),
      ( 200, 6.01488069641669, 9999.99986332045, -0.000215661698063957, 20.7952079598676, 2 ),
      ( 200, 60.2656796868095, 99999.9999999823, -0.00215453500602418, 20.8146785674437, 2 ),
      ( 200, 614.45587009225, 999999.999805736, -0.0213156371772331, 21.0100923341266, 2 ),
      ( 200, 7126.56627136121, 9999999.99999998, -0.156173774573532, 22.7192845106207, 1 ),
      ( 250, 0.481090508860678, 999.999999999549, -7.95588244426727E-06, 20.7973514089393, 2 ),
      ( 250, 4.8112494606249, 9999.99999555303, -7.9531734201309E-05, 20.7983663197204, 2 ),
      ( 250, 48.146829406998, 100000, -0.000792601950094554, 20.8085059958919, 2 ),
      ( 250, 484.794348045005, 999999.999998971, -0.00764791648006575, 20.9089154801301, 2 ),
      ( 250, 5021.07131790023, 10000000.0032118, -0.0418644729334747, 21.7732623995196, 1 ),
      ( 300, 0.400906326951178, 999.999999999996, -1.85646492498435E-06, 20.811713500409, 2 ),
      ( 300, 4.00913002546891, 9999.99999996118, -1.85445368183368E-05, 20.8123434048136, 2 ),
      ( 300, 40.0979120659093, 99999.9997730737, -0.000183433137000074, 20.8186356600111, 2 ),
      ( 300, 401.560918333185, 999999.999999979, -0.00163200775319526, 20.8808682410833, 2 ),
      ( 300, 3989.40001305956, 10000000.0000333, 0.00492697269915745, 21.4279119733826, 1 ),
      ( 350, 0.343632938902523, 1000, 1.15525894384035E-06, 20.8519667339942, 1 ),
      ( 350, 3.43629369209434, 10000.0000000078, 1.15665897949281E-05, 20.8524044472883, 1 ),
      ( 350, 34.3593120941536, 100000.000116811, 0.000117065463332816, 20.8567774835646, 1 ),
      ( 350, 343.18370588765, 1000000.00000009, 0.00131019606622917, 20.9000949638624, 1 ),
      ( 350, 3346.93335547868, 10000000.0000016, 0.0267110436091872, 21.291169248984, 1 ),
      ( 400, 0.300678341510486, 1000.00000000001, 2.72044629017136E-06, 20.9351370850078, 1 ),
      ( 400, 3.00670993322601, 10000.000000076, 2.72142251701951E-05, 20.9354656143757, 1 ),
      ( 400, 30.0597077505209, 100000.000854165, 0.000273117579488964, 20.938748442987, 1 ),
      ( 400, 299.831259359968, 1000000.00000061, 0.00282797894678623, 20.9713289634551, 1 ),
      ( 400, 2898.51208524227, 10000000.0000004, 0.0373569852670931, 21.2723159147601, 1 ),
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
      ( 200, 0.601406581438205, 999.999999981722, -7.73203703248606E-05, 20.8027457882883, 2 ),
      ( 200, 6.01825660920381, 9999.99981404, -0.000773613495208909, 20.8173891024499, 2 ),
      ( 200, 60.6073863288042, 99999.9996687001, -0.00777757141656792, 20.9655067156398, 2 ),
      ( 250, 0.481106787265537, 999.999999982926, -3.8916305366976E-05, 20.8046599359739, 2 ),
      ( 250, 4.81275388781598, 9999.99982735762, -0.000389224996406978, 20.8096343666681, 2 ),
      ( 250, 48.2970904683209, 99999.9999999319, -0.00389845494278424, 20.8596417375765, 2 ),
      ( 250, 500.93223919784, 1000000.00146178, -0.0396144893613864, 21.3866936052124, 2 ),
      ( 300, 0.400915429521535, 999.99999999935, -2.17233240561084E-05, 20.8130232568312, 2 ),
      ( 300, 4.00993829372118, 9999.99998088283, -0.00021723293434762, 20.8152692124638, 2 ),
      ( 300, 40.1779501353777, 99999.9999999995, -0.00217228864159382, 20.8377825950304, 2 ),
      ( 300, 409.802805896564, 1000000.000001, -0.0217082105413347, 21.0682060661031, 2 ),
      ( 300, 5027.15747133362, 9999999.99963696, -0.202518077870093, 23.6293885921693, 2 ),
      ( 350, 0.343638700617275, 999.999999999683, -1.27137212206478E-05, 20.8377579092965, 2 ),
      ( 350, 3.43678021884165, 9999.99999682922, -0.000127125363664786, 20.8390226048439, 2 ),
      ( 350, 34.4071323872924, 99999.9999999998, -0.00127006278009718, 20.8516818064858, 2 ),
      ( 350, 348.010723969957, 999999.999665188, -0.0125754523537354, 20.9794333524324, 2 ),
      ( 350, 3849.81954928117, 9999999.99999972, -0.107401457933793, 22.2869706724938, 2 ),
      ( 400, 0.300682301828657, 999.99999999995, -7.52155792851451E-06, 20.891671366541, 2 ),
      ( 400, 3.00702654046448, 9999.99999948976, -7.52032509091032E-05, 20.8925017166996, 2 ),
      ( 400, 30.0905959481581, 99999.9950620508, -0.000750796832021978, 20.9008074717953, 2 ),
      ( 400, 302.916046708725, 999999.999996415, -0.00738160458551424, 20.984059041974, 2 ),
      ( 400, 3194.64505963724, 10000000.0000978, -0.0587998522022911, 21.8042457628869, 2 ),
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
      ( 200, 0.601502324076437, 999.999999999922, -0.000233606496734146, 20.8212128074512, 2 ),
      ( 200, 6.0277302399969, 9999.99999921361, -0.00234120426114197, 20.9296632331081, 2 ),
      ( 200, 61.6118266506824, 99999.9915564836, -0.0239506874001297, 22.0449440389167, 2 ),
      ( 200, 31755.796893093, 1000000.00008583, -0.981062928093503, 36.7225281318348, 1 ),
      ( 200, 32107.7163345908, 10000000.0000021, -0.81270489531018, 36.8093826399118, 1 ),
      ( 250, 0.481139781395255, 999.999999999972, -0.000104614119673981, 20.8153133554899, 2 ),
      ( 250, 4.8159370781186, 9999.9999997293, -0.00104706607036491, 20.8543976462209, 2 ),
      ( 250, 48.6226360662203, 99999.9970807315, -0.010564859142182, 21.2504205666544, 2 ),
      ( 250, 545.043491887951, 999999.999668465, -0.117337506887251, 25.8564726624696, 2 ),
      ( 250, 28456.5853233291, 10000000.0004794, -0.830939151013092, 34.4942882898061, 1 ),
      ( 300, 0.400930850469654, 999.991208406712, -5.73102580012719E-05, 20.8155999777296, 2 ),
      ( 300, 4.01137859780887, 9999.99999993457, -0.000573336468020337, 20.8308753239709, 2 ),
      ( 300, 40.3229033280121, 99999.9992512725, -0.0057564318001253, 20.9847874646906, 2 ),
      ( 300, 426.531692132042, 999999.999995375, -0.0600748309854343, 22.650826786896, 2 ),
      ( 300, 23245.4468351042, 9999999.9995884, -0.82753273118555, 33.524760064056, 1 ),
      ( 350, 0.343647459716974, 999.990127605965, -3.5327016729923E-05, 20.824064908645, 2 ),
      ( 350, 3.43756781713154, 9999.99999998269, -0.00035333748176452, 20.8308012972151, 2 ),
      ( 350, 34.4856032148437, 99999.9998233837, -0.00353977449093323, 20.8984877622356, 2 ),
      ( 350, 356.488557012171, 999999.999999913, -0.0360551193068736, 21.6088124317725, 2 ),
      ( 350, 6384.8647948147, 9999999.99972293, -0.461797029987701, 32.9228190403455, 2 ),
      ( 400, 0.300687925416173, 999.994715048987, -2.33490127446613E-05, 20.8484332356653, 2 ),
      ( 400, 3.00751132041901, 9999.99999999671, -0.000233506782031019, 20.8518124689909, 2 ),
      ( 400, 30.1385122988008, 99999.9999670656, -0.00233660629018573, 20.8857112264274, 2 ),
      ( 400, 307.921997555691, 999999.999999998, -0.0235159975588166, 21.2354342652146, 2 ),
      ( 400, 3976.80026692834, 9999999.99628984, -0.243912481222696, 25.6921374027957, 2 ),
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
