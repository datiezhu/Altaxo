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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfMath;

namespace Altaxo.Gui.Markdown
{
  /// <summary>
  /// This class converts LaTeX formulas to an image (as a framework independent png stream).
  /// </summary>
  public class LaTeXFormulaImageStreamProvider : Altaxo.Main.Services.ILaTeXFormulaImageStreamProvider
  {
    private static TexFormulaParser _formulaParser = new TexFormulaParser();

    /// <inheritdoc/>
    public (Stream bitmapStream, string placement, int yoffset, int width96thInch, int height96thInch) Parse(string text, string fontFamily, double fontSize, double dpiResolution, bool isIntendedForHelp1File)
    {
      TexFormula formula = null;
      try
      {
        formula = _formulaParser.Parse(text);
      }
      catch (Exception)
      {
        return (null, null, 0, 0, 0);
      }

      double cyAscent;
      double cyDescent;
      double cyMiddle;
      {
        var gdiFont = new System.Drawing.Font(fontFamily, (float)fontSize, System.Drawing.FontStyle.Regular);
        int iCellSpace = gdiFont.FontFamily.GetLineSpacing(gdiFont.Style);
        int iEmHeight = gdiFont.FontFamily.GetEmHeight(gdiFont.Style);
        int iCellAscent = gdiFont.FontFamily.GetCellAscent(gdiFont.Style);
        int iCellDescent = gdiFont.FontFamily.GetCellDescent(gdiFont.Style);
        cyAscent = fontSize * iCellAscent / iEmHeight;
        cyDescent = fontSize * iCellDescent / iEmHeight;
        cyMiddle = cyAscent / 3; // is only a first guess, details coming from the Wpf font
        gdiFont.Dispose();
        var ff = new FontFamily(fontFamily);
        var tf = new Typeface(ff, System.Windows.FontStyles.Normal, System.Windows.FontWeights.Normal, System.Windows.FontStretches.Normal);
        if (tf.TryGetGlyphTypeface(out var gtf))
        {
          cyMiddle = Math.Floor(fontSize * gtf.Height * gtf.StrikethroughPosition);
        }
      }

      var formulaRenderer = formula.GetRenderer(TexStyle.Display, fontSize, fontFamily);
      // the placement of the image depends on the depth value
      var absoluteDepth = formulaRenderer.RenderSize.Height * formulaRenderer.RelativeDepth;
      var absoluteAscent = formulaRenderer.RenderSize.Height * (1 - formulaRenderer.RelativeDepth);

      double yoffset = 0;

      BitmapSource bmp;
      int width96thInch, height96thInch;
      string alignment;


      if (isIntendedForHelp1File)
      {
        // dictionary that holds the number of additional pixels neccessary to exactly align the formula, and the value is the alignment
        var sort = new SortedDictionary<double, (double, string)>();

        if (formulaRenderer.RelativeDepth < (1 / 16.0)) // if the formulas baseline is almost at the bottom of the image
        {
          sort.Add(0, (0.0, "baseline")); // then we can use baseline as vertical alight
        }
        if (absoluteAscent <= cyAscent)  // if our formula is higher than the top of the text
        {
          yoffset = cyAscent - absoluteAscent;
          sort.Add(Math.Abs(yoffset), (yoffset, "texttop")); // then we can use texttop alignment, and we shift the formula downwards (positive offset)
        }
        if (absoluteDepth <= cyDescent) // then we can use bottom alignment, and we shift the formula upwards (negative offset)
        {
          yoffset = absoluteDepth - cyDescent;
          sort.Add(Math.Abs(yoffset), (yoffset, "bottom"));
        }

        {
          // Alignment: middle
          // Note that this is a moving target: we must change the vertical size of the image, but by that
          // we change the middle of the image, which changes again the offset...
          if (isIntendedForHelp1File)
            yoffset = absoluteDepth - absoluteAscent; // in help1 file, the baseline of text is aligned with the middle of the image
          else
            yoffset = 2 * cyMiddle + absoluteDepth - absoluteAscent; // if yoffset is negative, then pad at the bottom, else pad at the top

          sort.Add(Math.Abs(yoffset), (yoffset, "middle"));
        }

        var firstEntry = sort.First();
        (bmp, width96thInch, height96thInch) = RenderToBitmap(formulaRenderer, 0, firstEntry.Value.Item1, dpiResolution);
        alignment = firstEntry.Value.Item2;
        yoffset = 0; // 0 as return value
      }
      else // MAML is intended for HTML help (so we can use HTML5 alignment with pixel accuracy        )
      {
        alignment = "baseline";
        var yshift = Math.Ceiling(absoluteAscent) - absoluteAscent; // we shift the formula downwards, so that the new absoluteAscent is Math.Ceiling(absoluteAscent)
        // by providing a positive offset in arg2, the image is lowered compared to the baseline
        (bmp, width96thInch, height96thInch) = RenderToBitmap(formulaRenderer, 0, yshift, dpiResolution);
        yoffset = Math.Ceiling(absoluteAscent) - height96thInch; // number of pixels from image bottom to baseline (negative sign)
      }

      var fileStream = new MemoryStream();
      BitmapEncoder encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(bmp));
      encoder.Save(fileStream);
      fileStream.Seek(0, SeekOrigin.Begin);

      return (fileStream, alignment, (int)Math.Round(yoffset), width96thInch, height96thInch);
    }

    /// <summary>
    /// Renders a formula to a bitmap.
    /// </summary>
    /// <param name="formulaRenderer">The formula renderer.</param>
    /// <param name="x">The x offset of the formula.</param>
    /// <param name="y">The y offset.
    /// If y is negative, the distance of top to text does not change, and the distance from bottom to text changes by Ceiling(-y), because Ceiling(-y) pixel lines are added to the image.
    /// That means, that if looked from the bottom of the image, the formula shifts upwardes, but in integer pixel steps.
    /// If y is positive, the distance of top of image to text changes by y. Because Ceiling(y) pixel lines are added to the image,
    /// the distance from bottom of the image to text changes by [Ceiling(y)-y], i.e. the shift is 0 for y==0, 1 for y==(0+epsilon), and 0 again for y==1.
    /// That means, if measured from the bottom of the image, the formula shifts upwards, but maximal by 1 pixel.
    /// </param>
    /// <param name="dpiResolution">The resolution of the image in dpi. If not sure, use 96 dpi.</param>
    /// <returns>The bitmap souce that represents the formula.</returns>
    public static (BitmapSource bitmapSource, int width96thInch, int heigth96thInch) RenderToBitmap(TexRenderer formulaRenderer, double x, double y, double dpiResolution)
    {
      var visual = new DrawingVisual();
      using (var drawingContext = visual.RenderOpen())
      {
        // Note that argument y in Render measures from the top. Thus is y is positive, this shifts the text down.
        formulaRenderer.Render(drawingContext, x, Math.Max(0, y)); // if y negative, then we don't change y, because we measure relative to the upper edge of the image. Only if y is positive, we translate the formula downwards.
      }

      var width = (int)Math.Ceiling(formulaRenderer.RenderSize.Width);
      var height = (int)Math.Ceiling(formulaRenderer.RenderSize.Height);
      height += (int)Math.Ceiling(Math.Abs(y));

      var relativeResolution = dpiResolution / 96.0;

      var bitmap = new RenderTargetBitmap((int)(relativeResolution * width), (int)(relativeResolution * height), (int)(relativeResolution * 96), (int)(relativeResolution * 96), PixelFormats.Default);
      bitmap.Render(visual);

      return (bitmap, width, height);
    }
  }
}
