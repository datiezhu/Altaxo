//This file is part of MathML.Rendering, a library for displaying mathml
//Copyright (C) 2003, Andy Somogyi
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
//For details, see http://numerator.sourceforge.net, or send mail to
//(slightly obfuscated for spam mail harvesters)
//andy[at]epsilon3[dot]net

using System;
using System.Drawing;
using Scaled=System.Single;

namespace MathML.Rendering
{
	/**
	 * the purpose of the color area is to change the rendering context 
	 * so that the default color for rendering is set to the specified
	 * color
	 */
	internal class ColorArea : BinContainerArea
	{
		private Color color;

		public ColorArea(Color color, Area area) : base(area)
		{
			this.color = color;
		}

		/**
		 * change the color of the rendering context, and call the child's
		 * render method recursivly
		 */
		public override void Render(IGraphicDevice device, float x, float y)
		{
			Color oldColor = device.Color;
			device.Color = color;
			child.Render(device, x, y);
			device.Color = oldColor;
		}
	
		public override Object Clone()
		{
			return new ColorArea(color, child);
		}
	}
}
