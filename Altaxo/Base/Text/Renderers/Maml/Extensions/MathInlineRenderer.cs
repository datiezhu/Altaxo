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

using Altaxo;
using Altaxo.Graph;
using Altaxo.Main.Services;
using Altaxo.Text.Renderers;
using Altaxo.Text.Renderers.Maml;
using Markdig.Extensions.Mathematics;
using System.Collections.Generic;
using System.IO;

namespace Altaxo.Text.Renderers.Maml.Extensions
{
	public class MathInlineRenderer : MamlObjectRenderer<MathInline>
	{
		protected override void Write(MamlRenderer renderer, MathInline obj)
		{
			var formulaText = obj.Content.Text.Substring(obj.Content.Start, obj.Content.Length);

			if (string.IsNullOrEmpty(formulaText))
				return;

			var formulaService = Current.GetRequiredService<ILaTeXFormulaImageStreamProvider>();

			var (stream, placement) = formulaService.Parse(formulaText, 15, 96);

			if (null == stream)
				return;

			stream.Seek(0, SeekOrigin.Begin);
			var streamHash = MemoryStreamImageProxy.ComputeStreamHash(stream);
			stream.Seek(0, SeekOrigin.Begin);

			try
			{
				renderer.StorePngImageFile(stream, streamHash);
				stream.Close();
			}
			finally
			{
				stream.Dispose();
			}

			// now render to Maml file

			string localUrl = "../media/" + streamHash + ".png";

			var attributes = new Dictionary<string, string>();
			attributes.Add("src", localUrl);
			attributes.Add("align", placement);
			/*
			if (width.HasValue)
				attributes.Add("width", System.Xml.XmlConvert.ToString(Math.Round(width.Value)));
			if (height.HasValue)
				attributes.Add("height", System.Xml.XmlConvert.ToString(height.Value));
				*/

			renderer.Push(MamlElements.markup);

			renderer.Push(MamlElements.img, attributes);

			renderer.PopTo(MamlElements.img);

			renderer.PopTo(MamlElements.markup);
		}
	}
}
