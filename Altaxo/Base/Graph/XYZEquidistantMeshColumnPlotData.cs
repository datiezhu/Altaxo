////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2003 Dr. Dirk Lellinger
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
using Altaxo.Data;

namespace Altaxo.Graph
{
	/// <summary>
	/// Summary description for XYColumnPlotData.
	/// </summary>
	[SerializationSurrogate(0,typeof(XYZEquidistantMeshColumnPlotData.SerializationSurrogate0))]
	[SerializationVersion(0)]
	public class XYZEquidistantMeshColumnPlotData 
		:
		IXYBoundsHolder, System.Runtime.Serialization.IDeserializationCallback,
		Main.IChangedEventSource,
		System.ICloneable,
		Main.IDocumentNode
	{
		protected object m_Parent;

		protected Altaxo.Data.IReadableColumn[] m_DataColumns; // the columns that are involved in the picture


		protected Altaxo.Data.INumericColumn m_XColumn;
		protected Altaxo.Data.INumericColumn m_YColumn;

		// cached or temporary data
		protected PhysicalBoundaries m_xBoundaries; 
		protected PhysicalBoundaries m_yBoundaries;
		protected PhysicalBoundaries m_vBoundaries;


		/// <summary>
		/// Number of rows, here the maximum of the row counts of all columns.
		/// </summary>
		protected int                m_Rows;
		protected bool   m_bCachedDataValid=false;

		// events
		public event PhysicalBoundaries.BoundaryChangedHandler	XBoundariesChanged;
		public event PhysicalBoundaries.BoundaryChangedHandler	YBoundariesChanged;
		public event PhysicalBoundaries.BoundaryChangedHandler	VBoundariesChanged;


		/// <summary>
		/// Fired if either the data of this XYColumnPlotData changed or if the bounds changed
		/// </summary>
		public event System.EventHandler Changed;


		#region Serialization
		/// <summary>Used to serialize the XYColumnPlotData Version 0.</summary>
		public class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			/// <summary>
			/// Serializes XYZEquidistantMeshColumnPlotData Version 0.
			/// </summary>
			/// <param name="obj">The XYZEquidistantMeshColumnPlotData to serialize.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				XYZEquidistantMeshColumnPlotData s = (XYZEquidistantMeshColumnPlotData)obj;
				
				info.AddValue("XColumn",s.m_XColumn);
				info.AddValue("YColumn",s.m_YColumn);
				info.AddValue("DataColumns",s.m_DataColumns);

				info.AddValue("XBoundaries",s.m_xBoundaries);
				info.AddValue("YBoundaries",s.m_yBoundaries);
				info.AddValue("VBoundaries",s.m_vBoundaries);

			}
			/// <summary>
			/// Deserializes theD2EquidistantMeshDataAssociation Version 0.
			/// </summary>
			/// <param name="obj">The empty XYZEquidistantMeshColumnPlotData object to deserialize into.</param>
			/// <param name="info">The serialization info.</param>
			/// <param name="context">The streaming context.</param>
			/// <param name="selector">The deserialization surrogate selector.</param>
			/// <returns>The deserialized XYZEquidistantMeshColumnPlotData.</returns>
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				XYZEquidistantMeshColumnPlotData s = (XYZEquidistantMeshColumnPlotData)obj;


				s.m_XColumn = (Altaxo.Data.INumericColumn)info.GetValue("XColumn",typeof(Altaxo.Data.INumericColumn));
				s.m_YColumn = (Altaxo.Data.INumericColumn)info.GetValue("YColumn",typeof(Altaxo.Data.INumericColumn));
				s.m_DataColumns = (Altaxo.Data.IReadableColumn[])info.GetValue("DataColumns",typeof(Altaxo.Data.IReadableColumn[]));
		
				s.m_xBoundaries = (PhysicalBoundaries)info.GetValue("XBoundaries",typeof(PhysicalBoundaries));
				s.m_yBoundaries = (PhysicalBoundaries)info.GetValue("YBoundaries",typeof(PhysicalBoundaries));
				s.m_vBoundaries = (PhysicalBoundaries)info.GetValue("VBoundaries",typeof(PhysicalBoundaries));

				return s;
			}
		}

		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(XYZEquidistantMeshColumnPlotData),0)]
			public class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				XYZEquidistantMeshColumnPlotData s = (XYZEquidistantMeshColumnPlotData)obj;
		
				if(s.m_XColumn is Main.IDocumentNode && !s.Equals(((Main.IDocumentNode)s.m_XColumn).ParentObject))
				{
					info.AddValue("XColumn",Main.DocumentPath.GetAbsolutePath((Main.IDocumentNode)s.m_XColumn));
				}
				else
				{
					info.AddValue("XColumn",s.m_XColumn);
				}

				if(s.m_YColumn is Main.IDocumentNode && !s.Equals(((Main.IDocumentNode)s.m_YColumn).ParentObject))
				{
					info.AddValue("YColumn",Main.DocumentPath.GetAbsolutePath((Main.IDocumentNode)s.m_YColumn));
				}
				else
				{
					info.AddValue("YColumn",s.m_YColumn);
				}

				info.CreateArray("DataColumns",s.m_DataColumns.Length);
				for(int i=0;i<s.m_DataColumns.Length;i++)
				{
					Altaxo.Data.IReadableColumn col = s.m_DataColumns[i];
					if(col is Main.IDocumentNode && !s.Equals(((Main.IDocumentNode)col).ParentObject))
					{
						info.AddValue("e",Main.DocumentPath.GetAbsolutePath((Main.IDocumentNode)col));
					}
					else
					{
						info.AddValue("e",col);
					}
				}
				info.AddValue("XBoundaries",s.m_xBoundaries);
				info.AddValue("YBoundaries",s.m_yBoundaries);
				info.AddValue("VBoundaries",s.m_vBoundaries);
			}

			Main.DocumentPath _xColumn = null;
			Main.DocumentPath _yColumn = null;
			Main.DocumentPath[] _vColumns=null;
			XYZEquidistantMeshColumnPlotData _plotAssociation = null;

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				bool bSurrogateUsed = false;
				
				XYZEquidistantMeshColumnPlotData s = null!=o ? (XYZEquidistantMeshColumnPlotData)o : new XYZEquidistantMeshColumnPlotData();

				XmlSerializationSurrogate0 surr = new XmlSerializationSurrogate0();

				object deserobj;
				deserobj = info.GetValue("XColumn",s);
				if(deserobj is Main.DocumentPath)
				{
					surr._xColumn = (Main.DocumentPath)deserobj;
					bSurrogateUsed=true;
				}
				else
				{
					s.m_XColumn = (Altaxo.Data.INumericColumn)deserobj;
					if(deserobj is Altaxo.Data.DataColumn)
						((Altaxo.Data.DataColumn)deserobj).Changed += new EventHandler(s.EhColumnDataChangedEventHandler);
				}


				deserobj = info.GetValue("YColumn",s);
				if(deserobj is Main.DocumentPath)
				{
					surr._yColumn = (Main.DocumentPath)deserobj;
					bSurrogateUsed=true;
				}
				else
				{
					s.m_YColumn = (Altaxo.Data.INumericColumn)deserobj;
					if(deserobj is Altaxo.Data.DataColumn)
						((Altaxo.Data.DataColumn)deserobj).Changed += new EventHandler(s.EhColumnDataChangedEventHandler);
				}

				int count = info.OpenArray();
				surr._vColumns = new Main.DocumentPath[count];
				s.m_DataColumns = new Altaxo.Data.IReadableColumn[count];
				for(int i=0;i<count;i++)
				{
					deserobj = info.GetValue("YColumn",s);
					if(deserobj is Main.DocumentPath)
					{
						surr._vColumns[i] = (Main.DocumentPath)deserobj;
						bSurrogateUsed=true;
					}
					else
					{
						s.m_DataColumns[i] = (Altaxo.Data.IReadableColumn)deserobj;
						if(deserobj is Altaxo.Data.DataColumn)
							((Altaxo.Data.DataColumn)deserobj).Changed += new EventHandler(s.EhColumnDataChangedEventHandler);
					}
				}
				info.CloseArray(count);
				
				
				s.m_xBoundaries = (PhysicalBoundaries)info.GetValue("XBoundaries",typeof(PhysicalBoundaries));
				s.m_yBoundaries = (PhysicalBoundaries)info.GetValue("YBoundaries",typeof(PhysicalBoundaries));
				s.m_vBoundaries = (PhysicalBoundaries)info.GetValue("VBoundaries",typeof(PhysicalBoundaries));

				s.m_xBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(s.OnXBoundariesChangedEventHandler);
				s.m_yBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(s.OnYBoundariesChangedEventHandler);
				s.m_vBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(s.OnVBoundariesChangedEventHandler);


				if(bSurrogateUsed)
				{
					surr._plotAssociation = s;
					info.DeserializationFinished += new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(surr.EhDeserializationFinished);
				}

				return s;
			}

			public void EhDeserializationFinished(Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object documentRoot)
			{
				bool bAllResolved = true;

				if(this._xColumn != null)
				{
					object xColumn = Main.DocumentPath.GetObject(this._xColumn, this._plotAssociation, documentRoot);
					bAllResolved &= (null!=xColumn);
					if(xColumn is Altaxo.Data.INumericColumn)
					{
						this._xColumn = null;
						_plotAssociation.m_XColumn = (Altaxo.Data.INumericColumn)xColumn;
						if(xColumn is Altaxo.Data.DataColumn)
							((Altaxo.Data.DataColumn)xColumn).Changed += new EventHandler(_plotAssociation.EhColumnDataChangedEventHandler);
					}
				}

				if(this._yColumn != null)
				{
					object yColumn = Main.DocumentPath.GetObject(this._yColumn, this._plotAssociation, documentRoot);
					bAllResolved &= (null!=yColumn);
					if(yColumn is Altaxo.Data.INumericColumn)
					{
						this._yColumn = null;
						_plotAssociation.m_YColumn = (Altaxo.Data.INumericColumn)yColumn;
						if(yColumn is Altaxo.Data.DataColumn)
							((Altaxo.Data.DataColumn)yColumn).Changed += new EventHandler(_plotAssociation.EhColumnDataChangedEventHandler);
					}
				}

				for(int i=0;i<this._vColumns.Length;i++)
				{
					if(this._vColumns[i]!=null)
					{
					object vColumn = Main.DocumentPath.GetObject(this._vColumns[i], this._plotAssociation, documentRoot);
					bAllResolved &= (null!=vColumn);
					if(vColumn is Altaxo.Data.IReadableColumn)
					{
						this._vColumns[i] = null;
						_plotAssociation.m_DataColumns[i] = (Altaxo.Data.IReadableColumn)vColumn;
						if(vColumn is Altaxo.Data.DataColumn)
							((Altaxo.Data.DataColumn)vColumn).Changed += new EventHandler(_plotAssociation.EhColumnDataChangedEventHandler);

					}
					}
				}

				if(bAllResolved)
					info.DeserializationFinished -= new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(this.EhDeserializationFinished);
			}
		}




		/// <summary>
		/// Finale measures after deserialization.
		/// </summary>
		/// <param name="obj">Not used.</param>
		public virtual void OnDeserialization(object obj)
		{
			// restore the event chain

			if(m_XColumn is Altaxo.Data.DataColumn)
				((Altaxo.Data.DataColumn)m_XColumn).Changed += new EventHandler(EhColumnDataChangedEventHandler);

			if(m_YColumn is Altaxo.Data.DataColumn)
				((Altaxo.Data.DataColumn)m_YColumn).Changed += new EventHandler(EhColumnDataChangedEventHandler);

			for(int i=0;i<m_DataColumns.Length;i++)
			{
				if(m_DataColumns[i] is Altaxo.Data.DataColumn)
					((Altaxo.Data.DataColumn)m_DataColumns[i]).Changed += new EventHandler(EhColumnDataChangedEventHandler);
			}			
		
			m_xBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.OnXBoundariesChangedEventHandler);
			m_yBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.OnYBoundariesChangedEventHandler);
			m_vBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.OnVBoundariesChangedEventHandler);


			// do not calculate cached data here, since it is done the first time this data is really needed
			this.m_bCachedDataValid=false;
		}
		#endregion

		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		protected XYZEquidistantMeshColumnPlotData()
		{
		}

		public XYZEquidistantMeshColumnPlotData(Altaxo.Data.DataColumnCollection coll, int[] selected)
		{
			m_XColumn = new Altaxo.Data.IndexerColumn();
			m_YColumn = new Altaxo.Data.IndexerColumn();

			int len = selected==null ? coll.ColumnCount : selected.Length;
			m_DataColumns = new Altaxo.Data.IReadableColumn[len];
			for(int i=0;i<len;i++)
			{
				int idx = null==selected ? i : selected[i];
				m_DataColumns[i] = coll[idx];

				// set the event chain
				if(m_DataColumns[i] is Altaxo.Data.DataColumn)
					((Altaxo.Data.DataColumn)m_DataColumns[i]).Changed += new EventHandler(EhColumnDataChangedEventHandler);
			}


			this.SetXBoundsFromTemplate( new FinitePhysicalBoundaries() );
			this.SetYBoundsFromTemplate( new FinitePhysicalBoundaries() );
			this.SetVBoundsFromTemplate( new FinitePhysicalBoundaries() );

		}
	

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="from">The object to copy from.</param>
		/// <remarks>Only clones the references to the data columns, not the columns itself.</remarks>
		public XYZEquidistantMeshColumnPlotData(XYZEquidistantMeshColumnPlotData from)
		{
			if(from.m_XColumn is Altaxo.Data.DataColumn && ((Altaxo.Data.DataColumn)from.m_XColumn).ParentObject!=null)
				m_XColumn = from.m_XColumn;
			else
				m_XColumn = (Altaxo.Data.INumericColumn)from.m_XColumn.Clone();

			if(from.m_YColumn is Altaxo.Data.DataColumn && ((Altaxo.Data.DataColumn)from.m_YColumn).ParentObject!=null)
				m_YColumn = from.m_YColumn;
			else
				m_YColumn = (Altaxo.Data.INumericColumn)from.m_YColumn.Clone();

			int len = from.m_DataColumns.Length;
			m_DataColumns = new Altaxo.Data.IReadableColumn[len];
	
			for(int i=0;i<len;i++)
			{
				m_DataColumns[i] = from.m_DataColumns[i]; // do not clone the data columns!

				// set the event chain
				if(m_DataColumns[i] is Altaxo.Data.DataColumn)
					((Altaxo.Data.DataColumn)m_DataColumns[i]).Changed += new EventHandler(EhColumnDataChangedEventHandler);
			}


			this.SetXBoundsFromTemplate( new FinitePhysicalBoundaries() );
			this.SetYBoundsFromTemplate( new FinitePhysicalBoundaries() );
			this.SetVBoundsFromTemplate( new FinitePhysicalBoundaries() );

				
		}

		/// <summary>
		/// Creates a cloned copy of this object.
		/// </summary>
		/// <returns>The cloned copy of this object.</returns>
		/// <remarks>The data columns refered by this object are <b>not</b> cloned, only the reference is cloned here.</remarks>
		public object Clone()
		{
			return new XYZEquidistantMeshColumnPlotData(this);
		}

		public void MergeXBoundsInto(PhysicalBoundaries pb)
		{
			if(!this.m_bCachedDataValid)
				this.CalculateCachedData();
			pb.Add(m_xBoundaries);
		}

		public void MergeYBoundsInto(PhysicalBoundaries pb)
		{
			if(!this.m_bCachedDataValid)
				this.CalculateCachedData();
			pb.Add(m_yBoundaries);
		}

		public void MergeVBoundsInto(PhysicalBoundaries pb)
		{
			if(!this.m_bCachedDataValid)
				this.CalculateCachedData();
			pb.Add(m_vBoundaries);
		}

		public void SetXBoundsFromTemplate(PhysicalBoundaries val)
		{
			if(null==m_xBoundaries || val.GetType() != m_xBoundaries.GetType())
			{
				if(null!=m_xBoundaries)
				{
					m_xBoundaries.BoundaryChanged -= new PhysicalBoundaries.BoundaryChangedHandler(this.OnXBoundariesChangedEventHandler);
				}
				m_xBoundaries = (PhysicalBoundaries)val.Clone();
				m_xBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.OnXBoundariesChangedEventHandler);
				this.m_bCachedDataValid = false;

				OnChanged();
			}
		}


		public void SetYBoundsFromTemplate(PhysicalBoundaries val)
		{
			if(null==m_yBoundaries || val.GetType() != m_yBoundaries.GetType())
			{
				if(null!=m_yBoundaries)
				{
					m_yBoundaries.BoundaryChanged -= new PhysicalBoundaries.BoundaryChangedHandler(this.OnYBoundariesChangedEventHandler);
				}
				m_yBoundaries = (PhysicalBoundaries)val.Clone();
				m_yBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.OnYBoundariesChangedEventHandler);
				this.m_bCachedDataValid = false;

				OnChanged();
			}
		}


		public void SetVBoundsFromTemplate(PhysicalBoundaries val)
		{
			if(null==m_vBoundaries || val.GetType() != m_vBoundaries.GetType())
			{
				if(null!=m_vBoundaries)
				{
					m_vBoundaries.BoundaryChanged -= new PhysicalBoundaries.BoundaryChangedHandler(this.OnVBoundariesChangedEventHandler);
				}
				m_vBoundaries = (PhysicalBoundaries)val.Clone();
				m_vBoundaries.BoundaryChanged += new PhysicalBoundaries.BoundaryChangedHandler(this.OnVBoundariesChangedEventHandler);
				this.m_bCachedDataValid = false;

				OnChanged();
			}
		}

		
		protected virtual void OnXBoundariesChangedEventHandler(object sender, BoundariesChangedEventArgs e)
		{
			if(null!=this.XBoundariesChanged)
				XBoundariesChanged(this, e);

			OnChanged();
		}

		protected virtual void OnYBoundariesChangedEventHandler(object sender, BoundariesChangedEventArgs e)
		{
			if(null!=this.YBoundariesChanged)
				YBoundariesChanged(this, e);

			OnChanged();
		}

		protected virtual void OnVBoundariesChangedEventHandler(object sender, BoundariesChangedEventArgs e)
		{
			if(null!=this.VBoundariesChanged)
				VBoundariesChanged(this, e);

			OnChanged();
		}

		public int RowCount
		{
			get
			{
				if(!this.m_bCachedDataValid)
					this.CalculateCachedData();
				return m_Rows;
			}
		}

		public int ColumnCount
		{
			get
			{
				return null==this.m_DataColumns ? 0 : m_DataColumns.Length;
			}
		}



		public Altaxo.Data.IReadableColumn[] DataColumns
		{
			get
			{
				return m_DataColumns;
			}
		}

		public Altaxo.Data.IReadableColumn XColumn
		{
			get { return this.m_XColumn; }
		}

		public Altaxo.Data.IReadableColumn YColumn
		{
			get { return this.m_YColumn; }
		}


		public override string ToString()
		{
			if(null!=m_DataColumns && m_DataColumns.Length>0)
			return String.Format("PictureData {0}-{1}",m_DataColumns[0].FullName,m_DataColumns[m_DataColumns.Length-1].FullName);
			else
				return "Empty (no data)";
		}

		public void CalculateCachedData()
		{
			if(null==m_DataColumns || m_DataColumns.Length==0)
			{
				m_Rows = 0;
				return;
			}
			
			this.m_xBoundaries.BeginUpdate(); // disable events
			this.m_yBoundaries.BeginUpdate(); // disable events
			this.m_vBoundaries.BeginUpdate();
			
			this.m_xBoundaries.Reset();
			this.m_yBoundaries.Reset();
			this.m_vBoundaries.Reset();

			// get the length of the largest column as row count
			m_Rows = 0;
			for(int i=0;i<m_DataColumns.Length;i++)
			{
				if(m_DataColumns[i] is IDefinedCount)
					m_Rows = System.Math.Max(m_Rows,((IDefinedCount)m_DataColumns[i]).Count);
			}

			for(int i=0;i<m_DataColumns.Length;i++)
			{
				Altaxo.Data.IReadableColumn col = m_DataColumns[i];
				int collength = (col is Altaxo.Data.IDefinedCount) ? ((Altaxo.Data.IDefinedCount)col).Count : m_Rows;
				for(int j=0;j<collength;j++)
				{
					this.m_vBoundaries.Add(col,j);
				}
			}


			// enter the two bounds for x
			for(int i=0;i<m_DataColumns.Length;i++)
				this.m_yBoundaries.Add(m_YColumn,i);

			// enter the bounds for y
			for(int i=0;i<m_Rows;i++)
				this.m_xBoundaries.Add(m_XColumn,i);

			// now the cached data are valid
			m_bCachedDataValid = true;


			// now when the cached data are valid, we can reenable the events
			this.m_xBoundaries.EndUpdate(); // enable events
			this.m_yBoundaries.EndUpdate(); // enable events
			this.m_vBoundaries.EndUpdate(); // enable events

		}

		void EhColumnDataChangedEventHandler(object sender, EventArgs e)
		{
			
			m_bCachedDataValid = false;
		
			OnChanged();
		}

		protected virtual void OnChanged()
		{
			if(null!=Changed)
				Changed(this,new System.EventArgs());
		}

		public virtual object ParentObject
		{
			get { return m_Parent; }
			set { m_Parent = value; }
		}

		public virtual string Name
		{
			get
			{
				Main.INamedObjectCollection noc = ParentObject as Main.INamedObjectCollection;
				return null==noc ? null : noc.GetNameOfChildObject(this);
			}
		}
	}


} // end name space
