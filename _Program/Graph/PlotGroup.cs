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

using System;
using Altaxo.Serialization;

namespace Altaxo.Graph
{
	[Flags]
	[Serializable]
	public enum PlotGroupStyle
	{
		Color = 0x01,
		Line  = 0x02,
		Symbol = 0x04,
		All = Color | Line | Symbol
	}

	
	/// <summary>
	/// Summary description for PlotGroup.
	/// </summary>
	[SerializationSurrogate(0,typeof(PlotGroup.SerializationSurrogate0))]
	[SerializationVersion(0)]
	public class PlotGroup : System.Runtime.Serialization.IDeserializationCallback
	{
		/// <summary>
		/// Designates which dependencies the plot styles have on each other.
		/// </summary>
		PlotGroupStyle m_Style;
		System.Collections.ArrayList m_PlotItems;
		
		private PlotGroup.Collection m_Parent;



		#region Serialization
		/// <summary>Used to serialize the PlotGroup Version 0.</summary>
		public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			/// <summary>
			/// Serializes PlotGroup Version 0.
			/// </summary>
			/// <param name="obj">The PlotGroup to serialize.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				PlotGroup s = (PlotGroup)obj;
				info.AddValue("Style",s.m_Style);  
				info.AddValue("Group",s.m_PlotItems);  
			}
			/// <summary>
			/// Deserializes the PlotGroup Version 0.
			/// </summary>
			/// <param name="obj">The empty axis object to deserialize into.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			/// <param name="selector">The deserialization surrogate selector.</param>
			/// <returns>The deserialized PlotGroup.</returns>
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				PlotGroup s = (PlotGroup)obj;

				s.m_Style = (PlotGroupStyle)info.GetValue("Style",typeof(PlotGroupStyle));
				s.m_PlotItems = (System.Collections.ArrayList)info.GetValue("Group",typeof(System.Collections.ArrayList));
				return s;
			}
		}

		/// <summary>
		/// Finale measures after deserialization.
		/// </summary>
		/// <param name="obj">Not used.</param>
		public virtual void OnDeserialization(object obj)
		{
			if(m_PlotItems.Count>0)
				((PlotItem)m_PlotItems[0]).StyleChanged += new EventHandler(this.OnMasterStyleChangedEventHandler);
		}
		#endregion



		public PlotGroup(PlotItem master, PlotGroupStyle style)
		{
			m_Style = style;
			m_PlotItems = new System.Collections.ArrayList();
			m_PlotItems.Add(master);
		}

		public PlotGroup(PlotGroupStyle style)
		{
			m_Style = style;
			m_PlotItems = new System.Collections.ArrayList();
		}


		/// <summary>
		/// This is !!! not !!! cloneable, since the PlotGroup itself stores only references to PlotItems! Since the only use
		/// is to clone a Layer, and the PlotItems of the layer are cloned into new objects, it is not usefull here.
		/// </summary>
		/// <returns>Null!</returns>
		private object Clone()
		{
			return null; // see above for explanation
		}

		/// <summary>
		/// Clones the PlotGroup into a new collection and fixed the references to the PlotItems. It presumes, that the PlotList, from which the PlotItems are referred in the PlotGroup items,
		/// are cloned before so that the PlotList <paramref name="newList"/> is an exact copy of the PlotList <paramref name="oldList"/>.
		/// </summary>
		/// <param name="newList">The new PlotList, which was cloned from oldList before.</param>
		/// <param name="oldList">The old PlotList, to which the items in these PlotGroupList refers to.</param>
		/// <returns></returns>
		public PlotGroup Clone(Layer.PlotList newList, Layer.PlotList oldList)
		{
			PlotGroup newGroup = new PlotGroup(this.Style);

			for(int i=0;i<this.Count;i++)
			{
				// look for the position of the PlotItem in the old list
				int position = oldList.IndexOf(this[i]);

				if(position>=0)
					newGroup.Add(newList[position]);
			}
			return newGroup;
		}

		public int Count
		{
			get { return null!=m_PlotItems ? m_PlotItems.Count : 0; }
		}

		public PlotItem this[int i]
		{
			get { return (PlotItem)m_PlotItems[i]; }
		}

		public void Add(PlotItem assoc)
		{
			if(null!=assoc)
			{
				int cnt = m_PlotItems.Count;
				if(cnt==0) // this is the first, i.e. the master item, it must be wired by a Changed event handler
				{
					assoc.StyleChanged += new EventHandler(this.OnMasterStyleChangedEventHandler);
				}
				if(cnt>0)
				{
					((PlotStyle)assoc.Style).SetToNextStyle((PlotStyle)((PlotItem)m_PlotItems[cnt-1]).Style,m_Style);
				}
				m_PlotItems.Add(assoc);

				OnChanged();
			}
		}

		public bool Contains(PlotItem assoc)
		{
			return this.m_PlotItems.Contains(assoc);
		}

		public void Clear()
		{
			m_PlotItems.Clear();

			OnChanged();
		}

		public PlotItem MasterItem
		{
			get { return m_PlotItems.Count>0 ? (PlotItem)m_PlotItems[0] : null; }
		}

		public bool IsIndependent 
		{
			get { return this.m_Style == 0; }
		}

		public PlotGroupStyle Style
		{
			get { return this.m_Style; }
			set 
			{
				bool changed = (this.m_Style==value);
				this.m_Style = value;
				
				// update the styles beginning from the master item
				if(changed)
				{
					UpdateMembers();
					OnChanged();
				}
			}
		}

		public void UpdateMembers()
		{
			// update the styles beginning from the master item
			if(!IsIndependent && Count>0)
			{
				for(int i=1;i<Count;i++)
					((PlotStyle)this[i].Style).SetToNextStyle((PlotStyle)this[i-1].Style,this.m_Style);
			}
			// no changed event here since we track only the members structure and the grouping style
		}

		protected void OnMasterStyleChangedEventHandler(object sender, EventArgs e)
		{
			UpdateMembers();
		}

		protected virtual void OnChanged()
		{
			if(null!=m_Parent)
				m_Parent.OnChildChangedEventHandler(this);
		}

			[SerializationSurrogate(0,typeof(PlotGroup.Collection.SerializationSurrogate0))]
			[SerializationVersion(0)]
			public class Collection : Altaxo.Data.CollectionBase, IChangedEventSource
		{
				#region "Serialization"

				/// <summary>Used to serialize the Collection Version 0.</summary>
				public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
				{

					/// <summary>
					/// Serializes LayerCollection Version 0.
					/// </summary>
					/// <param name="obj">The Collection to serialize.</param>
					/// <param name="info">The serialization info.</param>
					/// <param name="context">The streaming context.</param>
					public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
					{
						PlotGroup.Collection s = (PlotGroup.Collection)obj;
						info.AddValue("Data",s.myList);
					}

					/// <summary>
					/// Deserializes the Collection Version 0.
					/// </summary>
					/// <param name="obj">The empty  object to deserialize into.</param>
					/// <param name="info">The serialization info.</param>
					/// <param name="context">The streaming context.</param>
					/// <param name="selector">The deserialization surrogate selector.</param>
					/// <returns>The deserialized Collection.</returns>
					public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
					{
						PlotGroup.Collection s = (PlotGroup.Collection)obj;

						s.myList =	(System.Collections.ArrayList)info.GetValue("Data",typeof(System.Collections.ArrayList));
		
						return s;
					}
				}

				/// <summary>
				/// Finale measures after deserialization.
				/// </summary>
				/// <param name="obj">Not used.</param>
				public virtual void OnDeserialization(object obj)
				{
				}
				#endregion


				/// <summary>
				/// Clones the collection into a new collection and fixed the references. It presumes, that the PlotList, from which the PlotItems are referred in the PlotGroup items,
				/// are cloned before so that the PlotList <paramref name="newList"/> is an exact copy of the PlotList <paramref name="oldList"/>.
				/// </summary>
				/// <param name="newList">The new PlotList, which was cloned from oldList before.</param>
				/// <param name="oldList">The old PlotList, to which the items in these PlotGroupList refers to.</param>
				/// <returns></returns>
				public PlotGroup.Collection Clone(Layer.PlotList newList, Layer.PlotList oldList)
				{
					PlotGroup.Collection coto = new PlotGroup.Collection();
					for(int i=0;i<this.Count;i++)
						coto.Add(((PlotGroup)base.InnerList[i]).Clone(newList,oldList));
				
					return coto;
				}

			public void Add(PlotGroup g)
			{
				g.m_Parent=this;
				base.InnerList.Add(g);

				OnChanged();
			}

			public new void Clear()
			{
				base.InnerList.Clear();

				OnChanged();
			}

			public PlotGroup GetPlotGroupOf(PlotItem assoc)
			{
				// search for the (first) plot group, to which assoc belongs,
				// and return this group

				for(int i=0;i<Count;i++)
					if(((PlotGroup)base.InnerList[i]).Contains(assoc)	)
						return ((PlotGroup)base.InnerList[i]);

				return null; // assoc belongs not to any plot group
			}


			#region IChangedEventSource Members

			public event System.EventHandler Changed;

			protected virtual void OnChanged()
			{
				if(null!=Changed)
					Changed(this,new EventArgs());
			}

			public virtual void OnChildChangedEventHandler(object sender)
			{
				OnChanged();
			}


			#endregion
		} // end of class Collection

	}
}
