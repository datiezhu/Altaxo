using System;
using Altaxo.Serialization;
using Altaxo;

namespace Altaxo.Data
{
	/// <summary>
	/// Summary description for Altaxo.Data.DoubleColumn.
	/// </summary>
	[SerializationSurrogate(0,typeof(Altaxo.Data.DoubleColumn.SerializationSurrogate0))]
	[SerializationVersion(0)]
	public class DoubleColumn : Altaxo.Data.DataColumn, System.Runtime.Serialization.IDeserializationCallback, INumericColumn
	{
		private double[] m_Array;
		private int      m_Capacity; // shortcout to m_Array.Length;
		public static readonly double NullValue = Double.NaN;
		
		public DoubleColumn()
		{
		}

		public DoubleColumn(string name)
			: base(name)
		{
		}
		public DoubleColumn(DataTable parenttable, string name)
			: base(parenttable,name)
		{
		}
	
		public DoubleColumn(int initialcapacity)
		{
			m_Array = new double[initialcapacity];
			m_Capacity = initialcapacity;
		}
	

		#region "Serialization"
		public new class SerializationSurrogate0 : System.Runtime.Serialization.ISerializationSurrogate
		{
			public void GetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context	)
			{
				Altaxo.Data.DoubleColumn s = (Altaxo.Data.DoubleColumn)obj;
				System.Runtime.Serialization.ISurrogateSelector ss;
				System.Runtime.Serialization.ISerializationSurrogate surr =
					App.m_SurrogateSelector.GetSurrogate(typeof(Altaxo.Data.DataColumn),context, out ss);
	
				surr.GetObjectData(obj,info,context); // stream the data of the base object

				if(s.m_Count!=s.m_Capacity)
				{
					// instead of the data array itself, stream only the first m_Count
					// array elements, since only they contain data
					double[] streamarray = new Double[s.m_Count];
					System.Array.Copy(s.m_Array,streamarray,s.m_Count);
					info.AddValue("Data",streamarray);
				}
				else // if the array is fully filled, we don't need to save a shrinked copy
				{
					info.AddValue("Data",s.m_Array);
				}
			}
			public object SetObjectData(object obj,System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context,System.Runtime.Serialization.ISurrogateSelector selector)
			{
				Altaxo.Data.DoubleColumn s = (Altaxo.Data.DoubleColumn)obj;
				System.Runtime.Serialization.ISurrogateSelector ss;
				System.Runtime.Serialization.ISerializationSurrogate surr =
					App.m_SurrogateSelector.GetSurrogate(typeof(Altaxo.Data.DataColumn),context, out ss);
				surr.SetObjectData(obj,info,context,selector);

				s.m_Array = (double[])(info.GetValue("Data",typeof(double[])));
				s.m_Capacity = null==s.m_Array ? 0 : s.m_Array.Length;
				return s;
			}
		}

		public override void OnDeserialization(object obj)
		{
			base.OnDeserialization(obj);
		}
		#endregion


		public double[] Array
		{
			get 
			{
				int len = this.Count;
				double[] arr = new double[len];
				System.Array.Copy(m_Array,0,arr,0,len);
				return arr;
			}

			set
			{
				m_Array = (double[])value.Clone();
				this.m_Count = m_Array.Length;
				this.m_Capacity = m_Array.Length;
				this.NotifyDataChanged(0,m_Count<=0?0:m_Count-1,true);
			}
		}

		protected internal double GetValueDirect(int idx)
		{
				return m_Array[idx];
		}

		public override System.Type GetColumnStyleType()
		{
			return typeof(Altaxo.Worksheet.DoubleColumnStyle);
		}
				
				
		public override void CopyDataFrom(Altaxo.Data.DataColumn v)
		{
			if(v.GetType()!=typeof(Altaxo.Data.DoubleColumn))
			{
				throw new ArgumentException("Try to copy " + v.GetType() + " to " + this.GetType(),"v"); // throw exception
			}
			Altaxo.Data.DoubleColumn vd = (Altaxo.Data.DoubleColumn)v;	

			// suggestion, but __not__ implemented:
			// if v is a standalone column, then simply take the dataarray
			// otherwise: copy the data by value	
			int oldCount = this.m_Count;
			if(null==vd.m_Array || vd.m_Count==0)
			{
				m_Array=null;
				m_Capacity=0;
				m_Count=0;
			}
			else
			{
				m_Array = (double[])vd.m_Array.Clone();
				m_Capacity = m_Array.Length;
				m_Count = ((Altaxo.Data.DoubleColumn)v).m_Count;
			}
			if(oldCount>0 || m_Count>0) // message only if really was a change
				NotifyDataChanged(0,oldCount>m_Count? (oldCount-1):(m_Count-1),m_Count<oldCount);
		}				

		protected void Realloc(int i)
		{
			int newcapacity1 = (int)(m_Capacity*increaseFactor+addSpace);
			int newcapacity2 = i+addSpace+1;
			int newcapacity = newcapacity1>newcapacity2 ? newcapacity1:newcapacity2;
				
			double[] newarray = new double[newcapacity];
			if(m_Count>0)
			{
				System.Array.Copy(m_Array,newarray,m_Count);
			}

			m_Array = newarray;
			m_Capacity = m_Array.Length;
		}

		// indexers
		public override void SetValueAt(int i, AltaxoVariant val)
		{
			if(val.IsType(AltaxoVariant.Content.VDouble))
				this[i] = val.m_Double;
			else
				throw new ApplicationException("Error: Try to set " + this.TypeAndName + "[" + i + "] with " + val.ToString());
		}

		public override AltaxoVariant GetVariantAt(int i)
		{
			return new AltaxoVariant(this[i]);
		}

		public double GetDoubleAt(int i)
		{
			return i<m_Count ? this[i] : Double.NaN;
		}

		public override bool IsElementEmpty(int i)
		{
			return i<m_Count ? Double.IsNaN(m_Array[i]) : true;
		}

		public new double this[int i]
		{
			get
			{
				if(i>=0 && i<m_Count)
					return m_Array[i];
				return double.NaN;	
			}
			set
			{
				bool bCountDecreased=false;


				if(Double.IsNaN(value))
				{
					if(i>=0 && i<m_Count-1) // i is inside the used range
					{
						m_Array[i]=value;
					}
					else if(i==(m_Count-1)) // m_Count is then decreasing
					{
						for(m_Count=i; m_Count>0 && Double.IsNaN(m_Array[m_Count-1]); --m_Count);
						bCountDecreased=true;;
					}
				}
				else // value is not NaN
				{
					if(i>=0 && i<m_Count) // i is inside the used range
					{
						m_Array[i]=value;
					}
					else if(i==m_Count && i<m_Capacity) // i is the next value after the used range
					{
						m_Array[i]=value;
						m_Count=i+1;
					}
					else if(i>m_Count && i<m_Capacity) // is is outside used range, but inside capacity of array
					{
						for(int k=m_Count;k<i;k++)
							m_Array[k]=Double.NaN; // fill range between used range and new element with voids
					
						m_Array[i]=value;
						m_Count=i+1;
					}
					else if(i>=0) // i is outside of capacity, then realloc the array
					{
						Realloc(i);

						for(int k=m_Count;k<i;k++)
							m_Array[k]=Double.NaN; // fill range between used range and new element with voids
					
						m_Array[i]=value;
						m_Count=i+1;
					}
				}
			NotifyDataChanged(i,i,bCountDecreased);
			} // end set	
		} // end indexer


		public override void InsertRows(int nInsBeforeColumn, int nInsCount)
		{
			if(nInsCount<=0)
				return; // nothing to do

			int newlen = this.m_Count + nInsCount;
			if(newlen>m_Capacity)
				Realloc(newlen);

			// copy values from m_Count downto nBeforeColumn 
			for(int i=m_Count-1, j=newlen-1; i>=nInsBeforeColumn;i--,j--)
				m_Array[j] = m_Array[i];

			for(int i=nInsBeforeColumn+nInsCount-1;i>=nInsBeforeColumn;i++)
				m_Array[i]=NullValue;
		
			this.m_Count=newlen;
			this.NotifyDataChanged(nInsBeforeColumn,m_Count-1,false);
		}

		public override void RemoveRows(int nDelFirstRow, int nDelCount)
		{
			if(nDelFirstRow<0)
				throw new ArgumentException("Row number must be greater or equal 0, but was " + nDelFirstRow.ToString(), "nDelFirstRow");

			if(nDelCount<=0)
				return; // nothing to do here, but we dont catch it

			// we must be careful, since the range to delete can be
			// above the range this column actually holds, but
			// we must handle this the right way
			int i,j;
			for(i=nDelFirstRow,j=nDelFirstRow+nDelCount;j<m_Count;i++,j++)
				m_Array[i]=m_Array[j];
			
			int prevCount = m_Count;
			m_Count= i<m_Count ? i : m_Count; // m_Count can only decrease

			if(m_Count!=prevCount) // raise a event only if something really changed
				this.NotifyDataChanged(nDelFirstRow,m_Count-1,true);
		}



		#region "Operators"

		// -----------------------------------------------------------------------------
		//
		//                        Operators
		//
		// -----------------------------------------------------------------------------


		// ----------------------- Addition operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator +(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i] + c2.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator +(Altaxo.Data.DoubleColumn c1, double c2)
		{
				int len = c1.m_Count;
				Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
				for(int i=0;i<len;i++)
					c3.m_Array[i] = c1.m_Array[i] + c2;
				c3.m_Count = len;
				return c3;
		}

		public static Altaxo.Data.DoubleColumn operator +(double c2, Altaxo.Data.DoubleColumn c1)
		{
			return c1+c2;
		}


		public override bool vop_Addition(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this + (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}
		public override bool vop_Addition_Rev(DataColumn c2, out DataColumn c3)
		{
			return vop_Addition(c2, out c3);
		}

		public override bool vop_Addition(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this + c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Addition_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			return vop_Addition(c2, out c3);
		}

		// --------------------- Operator Subtract -------------------------------------


		public static Altaxo.Data.DoubleColumn operator -(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i] - c2.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator -(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i]-c2;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator -(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c2 - c1.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}
		

		public override bool vop_Subtraction(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this - (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Subtraction_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 - this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Subtraction(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this - c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Subtraction_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 - this;
				return true;
			}
			c3=null;
			return false;
		}


		
		public static Altaxo.Data.DoubleColumn Subtraction(Altaxo.Data.DateTimeColumn c1, Altaxo.Data.DateTimeColumn c2)
		{
			int len = c1.Count<c2.Count ? c1.Count : c2.Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = (c1.GetValueDirect(i)-c2.GetValueDirect(i)).TotalSeconds;
			}
			
			
			c3.m_Count=len;
			
			return c3;	
		}



		// ----------------------- Multiplication operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator *(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i] * c2.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator *(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c1.m_Array[i] * c2;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator *(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c1.m_Array[i] * c2;
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_Multiplication(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this * (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}
		public override bool vop_Multiplication_Rev(DataColumn c2, out DataColumn c3)
		{
			return vop_Multiplication(c2, out c3);
		}

		public override bool vop_Multiplication(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this * c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Multiplication_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			return vop_Multiplication(c2, out c3);
		}

		// ------------------------ Division operator --------------------------------

		public static Altaxo.Data.DoubleColumn operator /(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i] / c2.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator /(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i]/c2;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator /(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c2/c1.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public override bool vop_Division(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this / (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Division_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 / this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Division(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this / c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Division_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 / this;
				return true;
			}
			c3=null;
			return false;
		}


		// -------------------------- operator % ----------------------------------------------
		public static Altaxo.Data.DoubleColumn operator %(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i] % c2.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator %(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i] % c2;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator %(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c2%c1.m_Array[i];
			}
			c3.m_Count=len;
			return c3;	
		}

		public override bool vop_Modulo(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this % (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Modulo_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 % this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Modulo(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this % c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Modulo_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 % this;
				return true;
			}
			c3=null;
			return false;
		}


		// ----------------------- AND operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator &(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = ((long)c1.m_Array[i]) & ((long)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator &(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ((long)c1.m_Array[i]) & c22;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator &(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c22 & ((long)c1.m_Array[i]);
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_And(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this & (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_And_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 & this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_And(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this & c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_And_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 & this;
				return true;
			}
			c3=null;
			return false;
		}

		// ----------------------- OR operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator |(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = ((long)c1.m_Array[i]) | ((long)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator |(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ((long)c1.m_Array[i]) | c22;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator |(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c22 | ((long)c1.m_Array[i]);
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_Or(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this | (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Or_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 | this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Or(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this | c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Or_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 | this;
				return true;
			}
			c3=null;
			return false;
		}



		// ----------------------- XOR operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator ^(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = ((long)c1.m_Array[i]) ^ ((long)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator ^(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ((long)c1.m_Array[i]) ^ c22;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator  ^(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c22 ^ ((long)c1.m_Array[i]);
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_Xor(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this ^ (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Xor_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 ^ this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Xor(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this ^ c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Xor_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 ^ this;
				return true;
			}
			c3=null;
			return false;
		}

		// ----------------------- ShiftLeft operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator <<(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = ((long)c1.m_Array[i]) << ((int)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator <<(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			int c22 = (int)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ((long)c1.m_Array[i]) << c22;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator <<(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c22 << ((int)c1.m_Array[i]);
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_ShiftLeft(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this << (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_ShiftLeft_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 << this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_ShiftLeft(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this << c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_ShiftLeft_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 << this;
				return true;
			}
			c3=null;
			return false;
		}


		// ----------------------- ShiftRight operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator >>(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = ((long)c1.m_Array[i]) >> ((int)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator >>(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			int c22 = (int)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ((long)c1.m_Array[i]) >> c22;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator >>(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			long c22 = (long)c2;
			for(int i=0;i<len;i++)
				c3.m_Array[i] = c22 >> ((int)c1.m_Array[i]);
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_ShiftRight(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this >> (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_ShiftRight_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 >> this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_ShiftRight(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this >> c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_ShiftRight_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 >> this;
				return true;
			}
			c3=null;
			return false;
		}



		// ----------------------- Lesser operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator <(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = (c1.m_Array[i] < c2.m_Array[i]) ? 1 : 0;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator <(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c1.m_Array[i] < c2 ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator  <(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c2 < c1.m_Array[i] ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_Lesser(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this < (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Lesser_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 < this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Lesser(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this < c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Lesser_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 < this;
				return true;
			}
			c3=null;
			return false;
		}


		// ----------------------- Greater operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator >(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = (c1.m_Array[i] > c2.m_Array[i]) ? 1 : 0;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator >(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c1.m_Array[i] > c2 ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator >(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c2 > c1.m_Array[i] ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_Greater(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this > (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Greater_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 > this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Greater(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this > c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_Greater_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 > this;
				return true;
			}
			c3=null;
			return false;
		}




		// ----------------------- LesserOrEqual operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator <=(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = (c1.m_Array[i] <= c2.m_Array[i]) ? 1 : 0;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator <=(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c1.m_Array[i] <= c2 ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator <=(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c2 <= c1.m_Array[i] ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_LesserOrEqual(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this <= (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_LesserOrEqual_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 <= this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_LesserOrEqual(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this <= c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_LesserOrEqual_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 <= this;
				return true;
			}
			c3=null;
			return false;
		}


		// ----------------------- GreaterOrEqual operator -----------------------------------
		public static Altaxo.Data.DoubleColumn operator >=(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = (c1.m_Array[i] >= c2.m_Array[i]) ? 1 : 0;
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn operator >=(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c1.m_Array[i] >= c2 ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}

		public static Altaxo.Data.DoubleColumn operator >=(double c2, Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
				c3.m_Array[i] = ( c2 >= c1.m_Array[i] ) ? 1 : 0;
			c3.m_Count = len;
			return c3;
		}


		public override bool vop_GreaterOrEqual(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = this >= (Altaxo.Data.DoubleColumn)c2;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_GreaterOrEqual_Rev(DataColumn c2, out DataColumn c3)
		{
			if(c2 is Altaxo.Data.DoubleColumn)
			{
				c3 = (Altaxo.Data.DoubleColumn)c2 >= this;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_GreaterOrEqual(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = this >= c22;
				return true;
			}
			c3=null;
			return false;
		}

		public override bool vop_GreaterOrEqual_Rev(AltaxoVariant c2, out DataColumn c3)
		{
			if(((AltaxoVariant)c2).IsType(AltaxoVariant.Content.VDouble))
			{
				double c22 = (double)c2;
				c3 = c22 >= this;
				return true;
			}
			c3=null;
			return false;
		}


		// --------------------------------- Unary Plus ----------------------------
		public static Altaxo.Data.DoubleColumn operator +(Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i];
			}
			c3.m_Count = len;
			return c3;	
		}


		public override bool vop_Plus(out DataColumn c3)
		{
			c3= +this;
			return true;
		}



		// --------------------------------- Unary Minus ----------------------------
		public static Altaxo.Data.DoubleColumn operator -(Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = -c1.m_Array[i];
			}
			c3.m_Count = len;
			return c3;	
		}


		public override bool vop_Minus(out DataColumn c3)
		{
			c3= -this;
			return true;
		}

		// --------------------------------- Unary NOT ----------------------------
		public static Altaxo.Data.DoubleColumn operator !(Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = 0==c1.m_Array[i] ? 1 : 0;
			}
			c3.m_Count = len;
			return c3;	
		}


		public override bool vop_Not(out DataColumn c3)
		{
			c3= !this;
			return true;
		}

		// --------------------------------- Unary Complement ----------------------------
		public static Altaxo.Data.DoubleColumn operator ~(Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = ~((long)c1.m_Array[i]);
			}
			c3.m_Count = len;
			return c3;	
		}


		public override bool vop_Complement(out DataColumn c3)
		{
			c3= ~this;
			return true;
		}

		// --------------------------------- Unary Increment ----------------------------
		public static Altaxo.Data.DoubleColumn operator ++(Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i]+1;
			}
			c3.m_Count = len;
			return c3;	
		}


		public override bool vop_Increment(out DataColumn c3)
		{
			int len = this.m_Count;
			Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c33.m_Array[i] = this.m_Array[i]+1;
			}
			c33.m_Count = len;
			c3=c33;
			return true;
		}

		// --------------------------------- Unary Decrement ----------------------------
		public static Altaxo.Data.DoubleColumn operator --(Altaxo.Data.DoubleColumn c1)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = c1.m_Array[i]-1;
			}
			c3.m_Count = len;
			return c3;	
		}


		public override bool vop_Decrement(out DataColumn c3)
		{
			int len = this.m_Count;
			Altaxo.Data.DoubleColumn c33 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c33.m_Array[i] = this.m_Array[i]-1;
			}
			c33.m_Count = len;
			c3=c33;
			return true;
		}


		// -----------------------------------------------------------------------------
		//
		//               arithmetic Functions
		//
		// -----------------------------------------------------------------------------

		public static Altaxo.Data.DoubleColumn Abs(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Abs(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Acos(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Acos(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Asin(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Asin(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	


		public static Altaxo.Data.DoubleColumn Atan(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Atan(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Atan2(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Atan2(c1.m_Array[i],c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Atan2(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Atan2(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Atan2(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Atan2(c1,c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}


		public static Altaxo.Data.DoubleColumn Ceiling(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Ceiling(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Cos(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Cos(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Cosh(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Cosh(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Exp(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Exp(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Floor(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Floor(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn IEEERemainder(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.IEEERemainder(c1.m_Array[i],c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn IEEERemainder(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.IEEERemainder(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn IEEERemainder(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.IEEERemainder(c1,c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

	
		
		public static Altaxo.Data.DoubleColumn Log(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Log(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Log(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Log(c1.m_Array[i],c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Log(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Log(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Log(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Log(c1,c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Max(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Max(c1.m_Array[i],c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Max(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Max(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Max(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Max(c1,c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		
		
		public static Altaxo.Data.DoubleColumn Min(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Min(c1.m_Array[i],c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Min(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Min(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Min(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Min(c1,c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		
		
		public static Altaxo.Data.DoubleColumn Pow(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Pow(c1.m_Array[i],c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Pow(Altaxo.Data.DoubleColumn c1, double c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Pow(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Pow(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Pow(c1,c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		
		
		public static Altaxo.Data.DoubleColumn Round(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Round(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Round(Altaxo.Data.DoubleColumn c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c1.m_Count<c2.m_Count ? c1.m_Count : c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Round(c1.m_Array[i],(int)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Round(Altaxo.Data.DoubleColumn c1, int c2)
		{
			int len = c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Round(c1.m_Array[i],c2);
			}
			c3.m_Count=len;
			return c3;	
		}

		public static Altaxo.Data.DoubleColumn Round(double c1, Altaxo.Data.DoubleColumn c2)
		{
			int len = c2.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Round(c1,(int)c2.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;	
		}
		
		public static Altaxo.Data.DoubleColumn Sign(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Sign(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Sin(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Sin(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Sinh(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Sinh(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Sqrt(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Sqrt(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Tan(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Tan(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	

		public static Altaxo.Data.DoubleColumn Tanh(Altaxo.Data.DoubleColumn c1)
		{
			int len=c1.m_Count;
			Altaxo.Data.DoubleColumn c3 = new Altaxo.Data.DoubleColumn(len);
			for(int i=0;i<len;i++)
			{
				c3.m_Array[i] = System.Math.Tanh(c1.m_Array[i]);
			}
			c3.m_Count=len;
			return c3;
		}	
				
		#endregion

	} // end Altaxo.Data.DoubleColumn
}
