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
using DocumentFormat.OpenXml.Math;
using WpfMath.Atoms;

namespace Altaxo.Text.Renderers.OpenXML.Extensions.MathRendering.Renderers
{
  /// <summary>
  /// Renderer for <see cref="SymbolAtom"/> objects, like operator symbols, greek characters,
  /// and special symbols.
  /// </summary>
  /// <seealso cref="Altaxo.Text.Renderers.OpenXML.Extensions.MathRendering.OpenXMLAtomRenderer{WpfMath.Atoms.SymbolAtom}" />
  internal class SymbolAtomRenderer : OpenXMLAtomRenderer<SymbolAtom>
  {
    protected override void Write(OpenXMLWpfMathRenderer renderer, SymbolAtom item)
    {
      bool runCreated = false;
      var run = renderer.Peek() as Run;

      if (run == null)
      {
        runCreated = true;
        run = (Run)renderer.Push(new Run());
      }

      if (!_nameToSymbol.TryGetValue(item.Name, out var textString))
        textString = item.Name;

      var text = new DocumentFormat.OpenXml.Math.Text() { Text = textString };
      run.AppendChild(text);


      if (runCreated)
        renderer.PopTo(run);

    }

    public static bool TryConvert(string symbolName, out string textString)
    {
      var result = _nameToSymbol.TryGetValue(symbolName, out textString);

      if (!result)
        textString = symbolName;

      return result;
    }


    private static Dictionary<string, string> _nameToSymbol = new Dictionary<string, string>()
    {
      // miscellaneous
      ["%"] = "%",
      ["comma"] = ",",
      ["ldotp"] = ".",
      ["cdotp"] = "·",
      ["normaldot"] = ".",
      ["slash"] = "/",
      ["semicolon"] = ";",
      ["faculty"] = "!",
      ["question"] = "?",

      // < !--math accents-- >
      // see https://de.wikipedia.org/wiki/Unicodeblock_Kombinierende_diakritische_Zeichen

      ["acute"] = "\u0301",
      ["grave"] = "\u0300",
      ["ddot"] = "\u0308",
      ["tilde"] = "\u0303",
      ["bar"] = "\u0304",
      ["breve"] = "\u0306",
      ["check"] = "\u030C",
      ["hat"] = "\u0302",
      ["vec"] = "\u20D7",
      ["dot"] = "\u0307",
      ["widehat"] = "\u0361", // Word renders a widehat only if the AccentChar instance is null (this is handled in AccentedAtomRenderer)
      ["widetilde"] = "\u0303", // in literature this is \u0360, but for Word it seems to be \u0303

      // < !--delimiters that can change size-- >

      ["lbrace"] = "{",
      ["rbrace"] = "}",
      ["lbrack"] = "[",
      ["rbrack"] = "}",
      ["rsqbrack"] = "]",
      ["lsqbrack"] = "[",
      ["langle"] = "\u2329",
      ["rangle"] = "\u232A",
      ["lfloor"] = "\u230A",
      ["rfloor"] = "\u230B",
      ["lceil"] = "\u2308",
      ["rceil"] = "\u2309",
      ["uparrow"] = "\u2191",
      ["Uparrow"] = "\u21D1",
      ["downarrow"] = "\u2193",
      ["Downarrow"] = "\u21D3",
      ["updownarrow"] = "\u2195",
      ["Updownarrow"] = "\u21D5",
      ["vert"] = "\u007C",
      ["Vert"] = "\u2016",
      ["\u007C"] = "\u2016", // note \| is parsed in WpfMath to SymbolAtom |, so when we render a symbol atom with |, we need a double vertical line
      // ["slashdel" ] = "",                                               

      //lowercase greek letters
      ["alpha"] = "α",
      ["beta"] = "β",
      ["gamma"] = "γ",
      ["delta"] = "δ",
      ["epsilon"] = "ϵ",
      ["varepsilon"] = "ε",
      ["zeta"] = "ζ",
      ["eta"] = "η",
      ["theta"] = "θ",
      ["vartheta"] = "ϑ",
      ["iota"] = "ι",
      ["kappa"] = "κ",
      ["lambda"] = "λ",
      ["mu"] = "μ",
      ["nu"] = "ν",
      ["xi"] = "ξ",
      ["omicron"] = "ο",
      ["pi"] = "π",
      ["varpi"] = "ϖ",
      ["rho"] = "ρ",
      ["varrho"] = "ϱ",
      ["sigma"] = "σ",
      ["varsigma"] = "ς",
      ["tau"] = "τ",
      ["upsilon"] = "υ",
      ["phi"] = "φ",
      ["varphi"] = "ϕ",
      ["chi"] = "χ",
      ["psi"] = "ψ",
      ["omega"] = "ω",

      // greek uppercase letters
      ["Alpha"] = "Α",
      ["Beta"] = "Β",
      ["Gamma"] = "Γ",
      ["Delta"] = "Δ",
      ["Epsilon"] = "Ε",
      ["Zeta"] = "Ζ",
      ["Eta"] = "Η",
      ["Theta"] = "Θ",
      ["Iota"] = "Ι",
      ["Kappa"] = "Κ",
      ["Lambda"] = "Λ",
      ["Mu"] = "Μ",
      ["Nu"] = "Ν",
      ["Xi"] = "Ξ",
      ["Omicron"] = "Ο",
      ["Pi"] = "Π",
      ["Rho"] = "Ρ",
      ["Sigma"] = "Σ",
      ["Tau"] = "Τ",
      ["Upsilon"] = "Υ",
      ["Phi"] = "Φ",
      ["Chi"] = "Χ",
      ["Psi"] = "Ψ",
      ["Omega"] = "Ω",

      // < !--special relation symbol with "width=0"(to overlap other relational symbols)-- >
      ["not"] = "\u0338",

      // other chars
      ["aleph"] = "ℵ",
      ["imath"] = "ı",
      ["jmath"] = "ȷ",
      ["ell"] = "ℓ",
      ["wp"] = "℘",
      ["Re"] = "ℜ",
      ["Im"] = "ℑ",
      ["partial"] = "∂",
      ["infty"] = "∞",
      ["prime"] = "′",
      ["emptyset"] = "∅",
      ["nabla"] = "∇",
      ["surdsign"] = "√",
      ["top"] = "⊤",
      ["bot"] = "⊥",
      ["triangle"] = "△",
      ["forall"] = "∀",
      ["exists"] = "∃",
      ["neg"] = "¬",
      ["lnot"] = "¬",
      ["flat"] = "\\u266d",
      ["natural"] = "\\u266e",
      ["sharp"] = "\\u266f",
      ["clubsuit"] = "\\u2667",
      ["diamondsuit"] = "\\u2662",
      ["heartsuit"] = "\\u2661",
      ["spadesuit"] = "\\u2664",
      ["lacc"] = "{",
      ["racc"] = "}",



      ["plus"] = "+",
      ["minus"] = "-",
      ["slash"] = "/",
      ["ast"] = "*",
      ["lbrack"] = "(",
      ["normaldot"] = ".",
      ["semicolon"] = ";",
      ["gt"] = ">",
      ["lbrace"] = "{",
      ["faculty"] = "!",
      ["rbrack"] = ")",
      ["comma"] = ",",
      ["lt"] = "<",
      ["question"] = "?",
      ["rsqbrack"] = "]",
      ["vert"] = "|",
      ["colon"] = ":",
      ["equals"] = "=",
      ["lsqbrack"] = "[",
      ["rbrace"] = "}",
      // [""] = "",

    };
  }
}
