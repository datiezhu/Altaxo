﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2019 Dr. Dirk Lellinger
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
using Altaxo;
using Altaxo.Graph;
using Altaxo.Main.Services;
using Altaxo.Text.Renderers;
using DocumentFormat.OpenXml.Math;
using Markdig.Extensions.Mathematics;
using WpfMath;

namespace Altaxo.Text.Renderers.OpenXML.Extensions
{
  public class FigureCaptionRenderer : OpenXMLObjectRenderer<Markdig.Extensions.Figures.FigureCaption>
  {
    protected override void Write(OpenXMLRenderer renderer, Markdig.Extensions.Figures.FigureCaption obj)
    {
      if (null != renderer.FigureCaptionList)
      {
        var idx = renderer.FigureCaptionList.FindIndex(x => object.ReferenceEquals(x.FigureCaption, obj));
        if (idx >= 0)
          renderer.CurrentFigureCaptionListIndex = idx;
      }

      renderer.PushParagraphFormat(FormatStyle.Caption);
      var paragraph = renderer.PushNewParagraph();
      renderer.WriteLeafInline(obj);
      renderer.PopTo(paragraph);
      renderer.PopParagraphFormat();
      renderer.CurrentFigureCaptionListIndex = null;
    }
  }
}
