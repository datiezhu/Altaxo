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
using System.Drawing;
using System.Drawing.Drawing2D;
using Altaxo.Serialization;


namespace Altaxo.Graph
{

	/// <summary>
	/// PlotItem holds the pair of data and style neccessary to plot a curve, function,
	/// surface and so on.  
	/// </summary>
	public abstract class  PlotItem : IChangedEventSource, System.ICloneable
	{
		/// <summary>
		/// Get/sets the data object of this plot.
		/// </summary>
		public abstract object Data { get; set; }
		/// <summary>
		/// Get/sets the style object of this plot.
		/// </summary>
		public abstract object Style { get; set; }
		/// <summary>
		/// The name of the plot. It can be of different length. An argument of zero or less
		/// returns the shortest possible name, higher values return more verbose names.
		/// </summary>
		/// <param name="level">The naming level, 0 returns the shortest possible name, 1 or more returns more
		/// verbose names.</param>
		/// <returns>The name of the plot.</returns>
		public abstract string GetName(int level);

		/// <summary>
		/// This paints the plot to the layer.
		/// </summary>
		/// <param name="g">The graphics context.</param>
		/// <param name="layer">The plot layer.</param>
		public abstract void Paint(Graphics g, Graph.Layer layer);


		/// <summary>
		/// Creates a cloned copy of this object.
		/// </summary>
		/// <returns>The cloned copy of this object.</returns>
		/// <remarks>The data (DataColumns which belongs to a table in the document's TableSet) are not cloned, only the reference to this columns is cloned.</remarks>
		public abstract object Clone();

		/// <summary>
		/// Fired if the data object changed or something inside the data object changed
		/// </summary>
		public event System.EventHandler DataChanged;

		/// <summary>
		/// Fired if the style object changed or something inside the style object changed
		/// </summary>
		public event System.EventHandler StyleChanged;

		/// <summary>
		/// Fired if either data or style has changed.
		/// </summary>
		public event System.EventHandler Changed;

		/// <summary>
		/// Intended to used by derived classes, fires the DataChanged event and the Changed event
		/// </summary>
		public virtual void OnDataChanged()
		{
			if(null!=DataChanged)
				DataChanged(this,new System.EventArgs());

			OnChanged();
		}

		/// <summary>
		/// Intended to used by derived classes, fires the StyleChanged event and the Changed event
		/// </summary>
		public virtual void OnStyleChanged()
		{
			if(null!=StyleChanged)
				StyleChanged(this,new System.EventArgs());
			
			OnChanged();}

		/// <summary>
		/// Intended to used by derived classes, fires the Changed event
		/// </summary>
		public virtual void OnChanged()
		{
			if(null!=Changed)
				Changed(this,new System.EventArgs());
		
		
		}


		/// <summary>
		/// Intended to use by derived classes, serves as event sink for the Changed event from the Data object and fires the DataChanged event.
		/// </summary>
		/// <param name="sender">The sender of the event (the Data object).</param>
		/// <param name="e">EventArgs (not used).</param>
		public virtual void OnDataChangedEventHandler(object sender, System.EventArgs e)
		{
			OnDataChanged();
		}

		/// <summary>
		/// Intended to use by derived classes, serves as event sink for the Changed event from the Style object and fires the StyleChanged event.
		/// </summary>
		/// <param name="sender">The sender of the event (the Style object).</param>
		/// <param name="e">EventArgs (not used).</param>
		public virtual void OnStyleChangedEventHandler(object sender, System.EventArgs e)
		{
			OnStyleChanged();
		}
	} // end of class PlotItem



	/// <summary>
	/// Association of data and style specialized for x-y-plots of column data.
	/// </summary>
	[SerializationSurrogate(0,typeof(XYDataPlot.SerializationSurrogate0))]
	[SerializationVersion(0)]
	public class XYDataPlot : PlotItem, System.Runtime.Serialization.IDeserializationCallback
	{
		protected PlotAssociation m_PlotAssociation;
		protected PlotStyle       m_PlotStyle;

		#region Serialization
		/// <summary>Used to serialize theXYDataPlot Version 0.</summary>
		public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			/// <summary>
			/// Serializes XYDataPlot Version 0.
			/// </summary>
			/// <param name="obj">The XYDataPlot to serialize.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				XYDataPlot s = (XYDataPlot)obj;
				info.AddValue("Data",s.m_PlotAssociation);  
				info.AddValue("Style",s.m_PlotStyle);  
			}
			/// <summary>
			/// Deserializes the XYDataPlot Version 0.
			/// </summary>
			/// <param name="obj">The empty XYDataPlot object to deserialize into.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			/// <param name="selector">The deserialization surrogate selector.</param>
			/// <returns>The deserialized XYDataPlot.</returns>
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				XYDataPlot s = (XYDataPlot)obj;

				s.m_PlotAssociation = (PlotAssociation)info.GetValue("Data",typeof(PlotAssociation));
				s.m_PlotStyle = (PlotStyle)info.GetValue("Style",typeof(PlotStyle));
		
				return s;
			}
		}

		/// <summary>
		/// Finale measures after deserialization of the linear axis.
		/// </summary>
		/// <param name="obj">Not used.</param>
		public virtual void OnDeserialization(object obj)
		{
			// Restore the event chain

			if(null!=m_PlotAssociation)
			{
				m_PlotAssociation.Changed += new EventHandler(OnDataChangedEventHandler);
			}

			if(null!=m_PlotStyle && m_PlotStyle is IChangedEventSource)
			{
				((IChangedEventSource)m_PlotStyle).Changed += new EventHandler(OnStyleChangedEventHandler);
			}
		}
		#endregion



		public XYDataPlot(PlotAssociation pa, PlotStyle ps)
		{
			this.Data = pa;
			this.Style = ps;
		}

		public XYDataPlot(XYDataPlot from)
		{
			this.Data = from.Data;   // also wires the event
			this.Style = from.Style; // also wires the event
		}

		public override object Clone()
		{
			return new XYDataPlot(this);
		}


		public override object Data
		{
			get { return m_PlotAssociation; }
			set
			{
				if(null==value)
					throw new System.ArgumentNullException();
				else if(!(value is PlotAssociation))
					throw new System.ArgumentException("The provided data object is not of the type " + m_PlotAssociation.GetType().ToString() + ", but of type " + value.GetType().ToString() + "!");
				else
				{
					if(!object.ReferenceEquals(m_PlotAssociation,value))
					{
						if(null!=m_PlotAssociation)
						{
							m_PlotAssociation.Changed -= new EventHandler(OnDataChangedEventHandler);
						}

						m_PlotAssociation = (PlotAssociation)value;
					
						if(null!=m_PlotAssociation )
						{
							m_PlotAssociation.Changed += new EventHandler(OnDataChangedEventHandler);
						}

						OnDataChanged();
					}
				}
			}
		}
		public override object Style
		{
			get { return m_PlotStyle; }
			set
			{
				if(null==value)
					throw new System.ArgumentNullException();
				else if(!(value is PlotStyle))
					throw new System.ArgumentException("The provided data object is not of the type " + m_PlotAssociation.GetType().ToString() + ", but of type " + value.GetType().ToString() + "!");
				else
				{
					if(!object.ReferenceEquals(m_PlotStyle,value))
					{
						// delete event wiring to old PlotStyle
						if(null!=m_PlotStyle && m_PlotStyle is IChangedEventSource)
						{
							((IChangedEventSource)m_PlotStyle).Changed -= new EventHandler(OnStyleChangedEventHandler);
						}
					
						m_PlotStyle = (PlotStyle)value;

						// create event wire to new Plotstyle
						if(null!=m_PlotStyle && m_PlotStyle is IChangedEventSource)
						{
							((IChangedEventSource)m_PlotStyle).Changed += new EventHandler(OnStyleChangedEventHandler);
						}

						// indicate the style has changed
						OnStyleChanged();
					}
					}
			}
		}


		public override string GetName(int level)
		{
			return m_PlotAssociation.ToString();
		}

		public override string ToString()
		{
			return GetName(int.MaxValue);
		}

		public override void Paint(Graphics g, Graph.Layer layer)
		{
			if(null!=this.m_PlotStyle)
			{
				m_PlotStyle.Paint(g,layer,m_PlotAssociation);
			}
		}

	}
}