#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2014 Dr. Dirk Lellinger
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

namespace Altaxo.Main
{
	/// <summary>
	/// Interface of a document node at the end of the hierarchie, i.e. a leaf node.
	/// </summary>
	public interface IDocumentLeafNode
		:
		INamedObject,
		Main.IChangedEventSource,
		ISuspendableByToken,
		Main.ITunnelingEventSource,
		IDisposable,
		// ICloneable,
		Altaxo.Collections.ITreeNodeWithParent<IDocumentLeafNode>,
		Altaxo.Collections.INodeWithParentNode<IDocumentNode>
	{
		/// <summary>
		/// Retrieves the parent object.
		/// </summary>
		IDocumentNode ParentObject { get; set; }

		void EhParentTunnelingEventHappened(IDocumentNode sender, IDocumentNode originalSource, TunnelingEventArgs e);
	}

	/// <summary>
	/// Provides the document hierarchy by getting the parent node. The document node is required to have a name, thus it also implements <see cref="INamedObject"/>.
	/// </summary>
	public interface IDocumentNode : IDocumentLeafNode, IChildChangedEventSink, INamedObjectCollection
	{
	}
}