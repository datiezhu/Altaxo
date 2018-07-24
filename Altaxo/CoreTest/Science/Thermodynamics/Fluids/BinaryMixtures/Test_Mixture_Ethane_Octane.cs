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
	/// Tests and test data for <see cref="Mixture_Ethane_Octane"/>.
	/// </summary>
	/// <remarks>
	/// <para>Reference:</para>
  /// <para>The test data was created automatically using calls into the TREND.DLL of the following software:</para>
  /// <para>TREND 3.0.: Span, R.; Eckermann, T.; Herrig, S.; Hielscher, S.; Jäger, A.; Thol, M. (2016): TREND.Thermodynamic Reference and Engineering Data 3.0.Lehrstuhl für Thermodynamik, Ruhr-Universität Bochum.</para>
	/// </remarks>
  [TestFixture]
  public class Test_Mixture_Ethane_Octane : MixtureTestBase
    {

    public Test_Mixture_Ethane_Octane()
      {
      _mixture = MixtureOfFluids.FromCASRegistryNumbersAndMoleFractions(new[]{("74-84-0", 0.5), ("111-65-9", 0.5) });

      // TestData for 1 Permille to 999 Permille Molefraction contains:
      // 0. Temperature (Kelvin)
      // 1. Mole density (mol/m³)
      // 2. Pressure (Pa)
      // 3. delta*AlphaR_delta
      // 4. Isochoric heat capacity (J/mol K)
      // 5. Phasetype (1: liquid, 2: gas)
      _testData_001_999 = new (double temperature, double moleDensity, double pressure, double deltaPhiR_delta, double cv, int phase)[]
      {
      ( 250, 6453.73279318609, 99999.9999990957, -0.992545605834585, 188.915159032296, 1 ),
      ( 250, 6459.18615169697, 999999.999999738, -0.925518994268055, 188.975576396195, 1 ),
      ( 250, 6511.3791714231, 9999999.99999899, -0.261160089082534, 189.572647583034, 1 ),
      ( 250, 6893.10859785897, 100000000.005848, 5.97924128113056, 194.857892376453, 1 ),
      ( 300, 0.401539939835048, 999.999991968926, -0.001579849083726, 180.341464970159, 2 ),
      ( 300, 6103.41651707704, 99999.9999995331, -0.993431456526035, 206.682440205025, 1 ),
      ( 300, 6110.86797789175, 1000000.00466232, -0.934394660359104, 206.737845317925, 1 ),
      ( 300, 6180.88871487988, 10000000.0000039, -0.351378764787241, 207.288289558979, 1 ),
      ( 300, 6649.39396687613, 100000000.008782, 5.02920460652226, 212.156537215326, 1 ),
      ( 350, 0.343927756157114, 999.999975493513, -0.000856030734216821, 205.785946905716, 2 ),
      ( 350, 3.4662384231664, 9999.98555206592, -0.00862749218877732, 206.195540898764, 2 ),
      ( 350, 5742.27113067503, 100000.000001726, -0.994015724168953, 227.852956250947, 1 ),
      ( 350, 5752.83140210939, 999999.999999666, -0.940267092949374, 227.901412934718, 1 ),
      ( 350, 5849.18778668286, 10000000.0000822, -0.412511008458241, 228.395973473657, 1 ),
      ( 350, 6422.61026725055, 100000000.012512, 4.35036891795294, 232.947656255897, 1 ),
      ( 400, 0.300832978386784, 999.999995956494, -0.000511256758746213, 231.281037648613, 2 ),
      ( 400, 3.0223145982858, 9999.99999999947, -0.00513607791996143, 231.501250903299, 2 ),
      ( 400, 31.7836352494924, 99999.9999895036, -0.0539811663003195, 233.836667659772, 2 ),
      ( 400, 5368.04488910688, 999999.999999968, -0.943987209178739, 250.187503306812, 1 ),
      ( 400, 5506.59970408665, 10000000.0009516, -0.453965801630314, 250.576107834014, 1 ),
      ( 400, 6208.81206894982, 100000000.016982, 3.84278107005938, 254.831533986673, 1 ),
      ( 500, 0.240596511098311, 999.999999783409, -0.00022099532489764, 278.281505950812, 2 ),
      ( 500, 2.41077091573463, 9999.99773754894, -0.00221402645222466, 278.360206106522, 2 ),
      ( 500, 24.6096150364844, 99999.9998511098, -0.0225635791295637, 279.165644355695, 2 ),
      ( 500, 4356.34415626441, 1000000.00254007, -0.944783209878109, 293.942514332296, 1 ),
      ( 500, 4746.19855123465, 10000000.0000008, -0.493187362902567, 293.207347773516, 1 ),
      ( 500, 5812.37813484409, 99999999.9999998, 3.13846681708733, 296.633238990906, 1 ),
      ( 600, 0.200475156167857, 999.999999979756, -0.000111597399320045, 318.233321767954, 2 ),
      ( 600, 2.00676900188031, 9999.99979334282, -0.0011168027876113, 318.267624294946, 2 ),
      ( 600, 20.2733985368022, 99999.9999996485, -0.0112521915976014, 318.614698743255, 2 ),
      ( 600, 228.426594479564, 999999.999727118, -0.122463020880106, 322.553303599104, 2 ),
      ( 600, 3797.55187689321, 9999999.99996332, -0.472152612623522, 331.312813039108, 1 ),
      ( 600, 5452.08236084077, 100000000, 2.67662794497232, 333.062604451558, 1 ),
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
      ( 250, 9226.44321611919, 1000000.00296235, -0.947857720692231, 116.556897434138, 1 ),
      ( 250, 9340.42535118115, 9999999.99999954, -0.484940182207773, 116.978976242238, 1 ),
      ( 300, 0.401086843269182, 999.999999993403, -0.000449682268870886, 112.400926325378, 2 ),
      ( 300, 8718.74648414647, 10000000.0001876, -0.540178760374449, 128.067167779785, 1 ),
      ( 300, 9686.54024459894, 100000000.000236, 3.13879952495828, 131.748637269105, 1 ),
      ( 350, 0.343724724203721, 999.999992290263, -0.000263574562358081, 128.284029726103, 2 ),
      ( 350, 3.44544489867129, 9999.99998658603, -0.00264222119965596, 128.360230838892, 2 ),
      ( 350, 8054.37308970334, 10000000.0000302, -0.573357077847898, 141.308604337404, 1 ),
      ( 350, 9294.36045130744, 99999999.9974871, 2.69723262726591, 144.760775302343, 1 ),
      ( 400, 0.300730140277708, 999.999998467468, -0.000167189949684372, 144.278902844982, 2 ),
      ( 400, 3.01184095229428, 9999.98372869156, -0.001674169369587, 144.320973906353, 2 ),
      ( 400, 30.5872158974529, 100000.000007461, -0.0169753853103898, 144.754781223908, 2 ),
      ( 400, 7310.75157819175, 10000000.0083917, -0.588715526383196, 155.446102139418, 1 ),
      ( 400, 8921.67260075994, 99999999.9985247, 2.37021850803944, 158.479730003321, 1 ),
      ( 500, 0.240562590233849, 999.999999897717, -7.77396740772238E-05, 174.043058026296, 2 ),
      ( 500, 2.4073111621962, 9999.9989552351, -0.000777744175659374, 174.059151497592, 2 ),
      ( 500, 24.243794760608, 99999.999993661, -0.00781255007612563, 174.221639074186, 2 ),
      ( 500, 262.03954521832, 999999.999999997, -0.0820321078786931, 176.015608206532, 2 ),
      ( 500, 5324.68567486098, 10000000.000098, -0.54824772077396, 183.908060771768, 1 ),
      ( 500, 8229.78937207767, 99999999.9999998, 1.9228438068247, 184.943119733891, 1 ),
      ( 600, 0.200461284267304, 999.999999989889, -4.01247233031348E-05, 199.663696419978, 2 ),
      ( 600, 2.0053370881018, 9999.99989808909, -0.000401269173624535, 199.671314609946, 2 ),
      ( 600, 20.1261281382304, 99999.9999999772, -0.00401488334179152, 199.747789397932, 2 ),
      ( 600, 208.88388997165, 1000000.00027655, -0.0403604565263461, 200.541125199164, 2 ),
      ( 600, 2986.58253034033, 10000000.000006, -0.328820687935848, 207.304886016373, 1 ),
      ( 600, 7607.63908014925, 100000000, 1.63489419913184, 208.351112777128, 1 ),
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
      ( 250, 0.481150280179256, 999.999998851277, -0.000127621367132692, 38.82027354157, 2 ),
      ( 250, 4.81704307541122, 9999.98716048765, -0.00127761287890445, 38.8361271383834, 2 ),
      ( 250, 15601.1169446302, 10000000.0002725, -0.691631774287517, 46.2306777781889, 1 ),
      ( 250, 18482.414143539, 100000000.000608, 1.60295474059778, 49.0117012634486, 1 ),
      ( 300, 0.400936893259782, 999.999999940747, -7.35709902200243E-05, 44.5200601126317, 2 ),
      ( 300, 4.012027007351, 9999.99939569611, -0.000736048838020147, 44.5282394188176, 2 ),
      ( 300, 40.3894101986021, 99999.9999980599, -0.00739477502270138, 44.6117842198528, 2 ),
      ( 300, 434.743640654616, 999999.99999979, -0.0778303385142843, 45.6276712946995, 2 ),
      ( 300, 12683.1456819509, 10000000.0000029, -0.683905392251269, 50.9414617967388, 1 ),
      ( 300, 17327.6616817588, 99999999.9998951, 1.31368434644152, 52.7410851850479, 1 ),
      ( 350, 0.343650481808418, 999.999999999127, -4.53105913175681E-05, 50.8112962406469, 2 ),
      ( 350, 3.43790716649569, 9999.99987399958, -0.000453199688569996, 50.8161680748708, 2 ),
      ( 350, 34.5202620750407, 99999.999999937, -0.00454141958312515, 50.8651744702377, 2 ),
      ( 350, 360.356663236532, 1000000.01755588, -0.0464033399169003, 51.3860449052781, 2 ),
      ( 350, 7280.16572647009, 10000000.019875, -0.527984769213762, 57.9893372121386, 1 ),
      ( 350, 16233.1651554557, 99999999.9996351, 1.11686943063815, 57.5784324215115, 1 ),
      ( 400, 0.300689279688295, 999.999999996172, -2.90423943909124E-05, 57.2925403553373, 2 ),
      ( 400, 3.00767903367836, 9999.99996144225, -0.000290444608200282, 57.2957330652328, 2 ),
      ( 400, 30.1557024386309, 99999.9999999978, -0.00290650643926201, 57.3277072962432, 2 ),
      ( 400, 309.744930516439, 1000000.0000236, -0.0292640255641834, 57.652329881265, 2 ),
      ( 400, 4207.95814546676, 10000000, -0.285447866738957, 61.084848248017, 2 ),
      ( 400, 15203.791806048, 99999999.9999963, 0.977668142180344, 62.9796095546865, 1 ),
      ( 500, 0.240547451530745, 999.999999999703, -1.25296252757139E-05, 69.8104211140702, 2 ),
      ( 500, 2.40574578084775, 9999.99999701866, -0.000125285571274897, 69.8120751227527, 2 ),
      ( 500, 24.0845923504248, 100000, -0.00125177930542278, 69.8286039236037, 2 ),
      ( 500, 243.565066720885, 999999.999739808, -0.0124017339574107, 69.9927123299769, 2 ),
      ( 500, 2681.27730534358, 10000000, -0.102873704700631, 71.4436688689686, 2 ),
      ( 500, 13362.9447785812, 100000000.001212, 0.800085546625025, 74.0494321031574, 1 ),
      ( 600, 0.200454727149613, 999.999999999988, -5.13423586333358E-06, 81.0968384111447, 2 ),
      ( 600, 2.00463987905777, 9999.99999985356, -5.13306061651332E-05, 81.0978454405097, 2 ),
      ( 600, 20.0556408335527, 99999.9986043336, -0.000512127072779013, 81.1079050511168, 2 ),
      ( 600, 201.46093469186, 999999.999999889, -0.00499966269623381, 81.2074170136025, 2 ),
      ( 600, 2076.33399388508, 10000000.0000003, -0.0345787404236964, 82.0783848608944, 1 ),
      ( 600, 11827.0311857075, 100000000.00277, 0.694877563300722, 84.4322213178241, 1 ),
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
