#region Disclaimer
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002 Dr. Dirk Lellinger
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
#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Altaxo.Serialization;

namespace Altaxo.Graph
{
	[SerializationSurrogate(0,typeof(PlotList.SerializationSurrogate0))]
	[SerializationVersion(0)]
	public class PlotList : Altaxo.Data.CollectionBase, System.Runtime.Serialization.IDeserializationCallback, IChangedEventSource, IChildChangedEventSink, System.ICloneable
	{
		/// <summary>The parent layer of this list.</summary>
		private Layer m_Owner; 

		private PlotGroup.Collection m_PlotGroups;

		#region Serialization
		/// <summary>Used to serialize the PlotList Version 0.</summary>
		public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			public object[] m_PlotItems = null; 

			/// <summary>
			/// Serializes PlotList Version 0.
			/// </summary>
			/// <param name="obj">The PlotList to serialize.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				PlotList s = (PlotList)obj;
				info.AddValue("Data",s.myList);
				info.AddValue("PlotGroups",s.m_PlotGroups);

			}

			/// <summary>
			/// Deserializes the PlotList Version 0.
			/// </summary>
			/// <param name="obj">The empty PlotList object to deserialize into.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			/// <param name="selector">The deserialization surrogate selector.</param>
			/// <returns>The deserialized PlotList.</returns>
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				PlotList s = (PlotList)obj;

				s.myList = (System.Collections.ArrayList)info.GetValue("Data",typeof(System.Collections.ArrayList));
				s.m_PlotGroups = (Graph.PlotGroup.Collection)info.GetValue("PlotGroups",typeof(Graph.PlotGroup.Collection));
				
				return s;
			}
		}

		/// <summary>
		/// Finale measures after deserialization.
		/// </summary>
		/// <param name="obj">Not used.</param>
		public virtual void OnDeserialization(object obj)
		{
			// restore the event chain
			for(int i=0;i<Count;i++)
				WireItem(this[i]);

			if(null!=m_PlotGroups)
				m_PlotGroups.Changed += new EventHandler(this.EhPlotGroups_Changed);

		}
				
		#endregion



		public PlotList(Layer owner)
		{
			m_Owner = owner;
			m_PlotGroups = new PlotGroup.Collection();
		}

		/// <summary>
		/// Copy constructor. Clones (!) all items. The parent owner is set to null and has to be set afterwards.
		/// </summary>
		/// <param name="from">The PlotList to clone this list from.</param>
		public PlotList(PlotList from)
			:
			this(null,from)
		{
		}

		/// <summary>
		/// Copy constructor. Clones (!) all the items in the list.
		/// </summary>
		/// <param name="owner">The new owner of the cloned list.</param>
		/// <param name="from">The list to clone all items from.</param>
		public PlotList(Layer owner, PlotList from)
		{
			m_Owner = owner;
			m_PlotGroups = new PlotGroup.Collection();

			// Clone all the items in the list.
			for(int i=0;i<from.Count;i++)
				Add((PlotItem)from[i].Clone()); // clone the items

			// special way neccessary to handle plot groups
			this.m_PlotGroups = null==from.m_PlotGroups ? null : from.m_PlotGroups.Clone(this,from);
		}

		public object Clone()
		{
			return new PlotList(this);
		}

		public Layer ParentLayer
		{
			get { return m_Owner; }
			set
			{
				SetParentLayer(value,false);
			}
		}
		

		/// <summary>
		/// Sets the parent layer.
		/// </summary>
		/// <param name="parent">The parent layer to set for this collection.</param>
		/// <param name="bSuppressEvents">If true, only the parent layer will set, but nothing else. If false, the boundaries of the items in the collection are merged into the parent layer collection.</param>
		/// <remarks>Use this with bSuppressEvents = true if you are in constructor or deserialization code where not all variables are currently initalized.</remarks>
		public void SetParentLayer(Layer parent, bool bSuppressEvents)
		{
			if(null==parent)
			{
				throw new ArgumentNullException();
			}
			else
			{
				m_Owner = parent;
						
				if(!bSuppressEvents)
				{
					// if the owner changed, it has possibly other x and y axis boundaries, so we have to set the plot items to this new boundaries
					for(int i=0;i<Count;i++)
						SetItemBoundaries(this[i]);
				}
			}
		}

		/// <summary>
		/// Restores the event chain of a item.
		/// </summary>
		/// <param name="plotitem">The plotitem for which the event chain should be restored.</param>
		public void WireItem(Graph.PlotItem plotitem)
		{
			SetItemBoundaries(plotitem);
			plotitem.Changed += new EventHandler(this.OnChildChanged);
		}

		/// <summary>
		/// This sets the type of the item boundaries to the type of the owner layer
		/// </summary>
		/// <param name="plotitem">The plot item for which the boundary type should be set.</param>
		public void SetItemBoundaries(Graph.PlotItem plotitem)
		{
			if(plotitem.Data is Graph.IXBoundsHolder)
			{
				IXBoundsHolder pa = (IXBoundsHolder)plotitem.Data;
				pa.SetXBoundsFromTemplate(m_Owner.XAxis.DataBounds); // ensure that data bound object is of the right type
				pa.XBoundariesChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.EhXBoundaryChanged);
				pa.MergeXBoundsInto(m_Owner.XAxis.DataBounds); // merge all x-boundaries in the x-axis boundary object
			}
			if(plotitem.Data is Graph.IYBoundsHolder)
			{
				IYBoundsHolder pa = (IYBoundsHolder)plotitem.Data;
				pa.SetYBoundsFromTemplate(m_Owner.YAxis.DataBounds); // ensure that data bound object is of the right type
				pa.YBoundariesChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.EhYBoundaryChanged);
				pa.MergeYBoundsInto(m_Owner.YAxis.DataBounds); // merge the y-boundaries in the y-Axis data boundaries
			}
		}

		public new void Clear()
		{
			m_PlotGroups.Clear();
			base.Clear();
		}

		public void Add(Graph.PlotItem plotitem)
		{
			if(plotitem==null)
				throw new ArgumentNullException();

			base.InnerList.Add(plotitem);
			WireItem(plotitem);
			OnChanged();
		}

		public PlotItem this[int i]
		{
			get { return (PlotItem)base.InnerList[i]; }
		}
			
		public int IndexOf(PlotItem it)
		{
			return base.InnerList.IndexOf(it,0,Count);
		}

		public void EhXBoundaryChanged(object sender, BoundariesChangedEventArgs args)
		{
			if(null!=this.m_Owner)
				m_Owner.OnPlotAssociationXBoundariesChanged(sender,args);
		}
		
		public void EhYBoundaryChanged(object sender, BoundariesChangedEventArgs args)
		{
			if(null!=this.m_Owner)
				m_Owner.OnPlotAssociationYBoundariesChanged(sender,args);
		}

		public void EhPlotGroups_Changed(object sender, EventArgs e)
		{
			OnChanged();
		}

		#region IChangedEventSource Members

		public event System.EventHandler Changed;


		public virtual void OnChildChanged(object child, EventArgs e)
		{
			if(null!=Changed)
				Changed(this,e);
		}

		protected virtual void OnChanged()
		{
			if(null!=Changed)
				Changed(this,new ChangedEventArgs(this,null));
		}

		#endregion


		#region PlotGroup handling

		public PlotGroup GetPlotGroupOf(PlotItem assoc)
		{
			return m_PlotGroups.GetPlotGroupOf(assoc);
		}

		/// <summary>
		/// Add the PlotGroup and all items in this group to the collection.
		/// </summary>
		/// <param name="pg"></param>
		public void Add(Altaxo.Graph.PlotGroup pg)
		{
			// 1. make sure that all PlotItems of this group are contained in our collection

			for(int i=0;i<pg.Count;i++)
			{
				PlotItem pa = pg[i];
				if(!base.InnerList.Contains(pa))
					this.Add(pa);
			}

			// 2. add the plotgroup to the plotgroup collection
			m_PlotGroups.Add(pg);
		}

		#endregion
	}


}