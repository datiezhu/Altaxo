﻿using Altaxo.Graph3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altaxo.Gui.Graph3D.Viewing
{
	[UserControllerForObject(typeof(Altaxo.Graph3D.Graph3DDocument))]
	[ExpectedTypeOfView(typeof(IGraph3DView))]
	public class Graph3DControllerWpf : Graph3DController
	{
		private Graph3DDocument graphdoc;

		public Graph3DControllerWpf()
		{
		}

		public Graph3DControllerWpf(Graph3DDocument graphdoc)
		{
			this.graphdoc = graphdoc;
		}
	}
}