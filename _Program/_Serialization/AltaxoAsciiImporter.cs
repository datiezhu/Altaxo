using System;

namespace Altaxo.Serialization
{
	public class AsciiLineStructure 
	{
		protected System.Collections.ArrayList mylist = new System.Collections.ArrayList();
		protected int nLineNumber;
		protected bool bContainsDBNull=false;
		protected bool bDirty=true;
		protected int prty=0;
		protected int hash=0;

		public int Count
		{
			get
			{
				return mylist.Count;
			}
		}
		public void Add(object o)
		{
			mylist.Add(o);
			bDirty=true;
		}

		public object this[int i]
		{
			get
			{
				return mylist[i];
			}
			set
			{
				mylist[i]=value;
				bDirty=true;
			}
		}
		
		public int LineNumber
		{
			get
			{
				return nLineNumber;
			}
			set
			{
				nLineNumber=value;
			}
		}

		public bool ContainsDBNull
		{
			get
			{
				if(bDirty)
					ResetDirty();
				return bContainsDBNull;
			}
		}

		public int Priority
		{
			get
			{
				if(bDirty)
					ResetDirty();
				return prty;
			}
		}
		
		public void ResetDirty()
		{
			bDirty = false;

			// Calculate priority and hash

			int len = Count;
			prty = 0;
			for(int i=0;i<len;i++)
			{
				Type t = (Type) this[i];
				if(t==typeof(DateTime))
					prty += 10;
				else if(t==typeof(Double))
					prty += 5;
				else if(t==typeof(String))
					prty += 2;
				else if(t==typeof(DBNull))
				{
					prty += 1;
					bContainsDBNull=true;
				}
			} // for

			// calculate hash

			hash = Count.GetHashCode();
			for(int i=0;i<len;i++)
				hash = ((hash<<1) | 1) ^ this[i].GetHashCode();
		}

		public override int GetHashCode()
		{
			if(bDirty)
				ResetDirty();
			return hash;
		}
		public bool IsCompatibleWith(AsciiLineStructure ano)
		{
			// our structure can have more columns, but not lesser than ano
			if(this.Count<ano.Count)
				return false;

			for(int i=0;i<ano.Count;i++)
			{
				if(this[i]==typeof(DBNull) || ano[i]==typeof(DBNull))
					continue;
				if(this[i]!=ano[i])
					return false;
			}
			return true;
		}
			
	} // end class AsciiLineStructure


	public struct NumberAndStructure
	{
		public int nLines;
		public AsciiLineStructure structure;
	} // end class


	public class AsciiLineAnalyzer
	{
		public enum Separation { Tab=0, Comma=1, Semicolon=2 };

		public int nNumberOfTabs=0;
		public int nNumberOfCommas=0;
		public int nNumberOfPoints=0;
		public int nNumberOfSemicolons=0;
		public int nPositionTab4=0;
		public int nPositionTab8=0;

		public System.Collections.ArrayList wordStartsTab4 = new System.Collections.ArrayList();
		public System.Collections.ArrayList wordStartsTab8 = new System.Collections.ArrayList();

		public AsciiLineStructure[] structure; 
		public AsciiLineStructure structWithCommas = null; 
		public AsciiLineStructure structWithSemicolons = null; 


		public AsciiLineAnalyzer(int nLine,string sLine)
		{
			structure = new AsciiLineStructure[3];
			structure[(int)Separation.Tab] = AssumeSeparator(nLine,sLine,"\t");
			structure[(int)Separation.Comma] = AssumeSeparator(nLine,sLine,",");
			structure[(int)Separation.Semicolon] = AssumeSeparator(nLine,sLine,";");
		}
		
		public AsciiLineAnalyzer(string sLine, bool bDummy)
		{
			int nLen = sLine.Length;
			if(nLen==0)
				return;

			bool bInWord=false;
			bool bInString=false; // true when starting with " char
			for(int i=0;i<nLen;i++)
			{
				char cc = sLine[i];

				if(cc=='\t')
				{
					nNumberOfTabs++;
					nPositionTab8 += 8-(nPositionTab8%8);
					nPositionTab4 += 4-(nPositionTab4%4);
					bInWord &= bInString;
					continue;
				}
				else if(cc==' ')
				{
					bInWord &= bInString;
					nPositionTab4++;
					nPositionTab8++;
				}
				else if(cc=='\"')
				{
					bInWord = !bInWord;
					bInString = !bInString;
					if(bInWord) 
					{
						wordStartsTab4.Add(nPositionTab4);
						wordStartsTab8.Add(nPositionTab8);
					}

					nPositionTab4++;
					nPositionTab8++;
				}
				else if(cc>' ') // all other chars are no space chars
				{
					if(!bInWord)
					{
						bInWord=true;
						wordStartsTab4.Add(nPositionTab4);
						wordStartsTab8.Add(nPositionTab8);
					}
					nPositionTab4++;
					nPositionTab8++;
					
					if(cc=='.') nNumberOfPoints++;
					if(cc==',') nNumberOfCommas++;
					if(cc==';') nNumberOfSemicolons++;
				}
			}
		}

		public static AsciiLineStructure AssumeSeparator(int nLine, string sLine, string separator)
		{
			AsciiLineStructure tabStruc = new AsciiLineStructure();
			tabStruc.LineNumber = nLine;

			int len =sLine.Length;
			int ix=0;
			for(int start=0; start<=len; start=ix+1)
			{
				ix = sLine.IndexOf(separator,start,len-start);
				if(ix==-1)
				{
					ix = len;
				}

				// try to interpret ix first as DateTime, then as numeric and then as string
				string substring = sLine.Substring(start,ix-start);
				if(ix==start) // just this char is a tab, so nothing is between the last and this
				{
					tabStruc.Add(typeof(DBNull));
				}
				else if(IsNumeric(substring))
				{
					tabStruc.Add(typeof(double));
				}
				else if(IsDateTime(substring))
				{
					tabStruc.Add(typeof(System.DateTime));
				}
				else
				{
					tabStruc.Add(typeof(string));
				}
			} // end for
			return tabStruc;
		}

		public static bool IsDateTime(string s)
		{
			bool bRet=false;
			try
			{
				System.Convert.ToDateTime(s);
				bRet=true;
			}
			catch(Exception)
			{
			}
			return bRet;
		}
		public static bool IsNumeric(string s)
		{
			bool bRet=false;
			try
			{
				System.Convert.ToDouble(s);
				bRet=true;
			}
			catch(Exception)
			{
			}
			return bRet;
		}

		public int WordStartsTab4_GetHashCode()
		{
			return GetHashOf(wordStartsTab4);
		}

		public int WordStartsTab8_GetHashCode()
		{
			return GetHashOf(wordStartsTab4);
		}


		public static int GetHashOf(System.Collections.ArrayList al)
		{
			int len = al.Count;
			int hash = al.Count.GetHashCode();
			for(int i=0;i<len;i++)
				hash ^= al[i].GetHashCode();
			
			return hash;
		}


		public static int GetPriorityOf(System.Collections.ArrayList al)
		{
			int len = al.Count;
			int prty = 0;
			for(int i=0;i<len;i++)
			{
				Type t = (Type) al[i];
				if(t==typeof(DateTime))
					prty += 10;
				else if(t==typeof(Double))
					prty += 5;
				else if(t==typeof(String))
					prty += 2;
				else if(t==typeof(DBNull))
					prty += 1;
			} // for
			return prty;
		} 
	} // end class



	public class AltaxoAsciiImporter
	{
		protected System.IO.Stream stream;

		public AltaxoAsciiImporter(System.IO.Stream _stream)
		{
			this.stream = _stream;
		}


		/// <summary>
		/// calculates the priority of the result
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public static int GetPriorityOf(System.Collections.ArrayList result, AsciiLineAnalyzer.Separation sep, ref AsciiLineStructure bestLine)
		{
			System.Collections.Hashtable sl = new System.Collections.Hashtable();
			bestLine=null;
			for(int i=0;i<result.Count;i++)
			{
				AsciiLineAnalyzer ala = (AsciiLineAnalyzer)result[i];
				int p  = ((AsciiLineAnalyzer)result[i]).structure[(int)sep].GetHashCode(); // and hash code
				if(null==sl[p])
					sl.Add(p,1);
				else 
					sl[p] = 1+(int)sl[p];
			}
			// get the count with the topmost frequency
			int nNumberOfMaxSame = 0;
			int nHashOfMaxSame = 0;
			foreach(System.Collections.DictionaryEntry ohash in sl)
			{
				int hash = (int)ohash.Key;
				int cnt = (int)ohash.Value;
				if(nNumberOfMaxSame<cnt)
				{
					nNumberOfMaxSame  = cnt;
					nHashOfMaxSame = hash;
				}
			} // for each
			// search for the max priority of the hash
			int nMaxPriorityOfMaxSame=0;
			for(int i=0;i<result.Count;i++)
			{
				AsciiLineAnalyzer ala = (AsciiLineAnalyzer)result[i];
				if(nHashOfMaxSame == ((AsciiLineAnalyzer)result[i]).structure[(int)sep].GetHashCode())
				{
					int prty = ((AsciiLineAnalyzer)result[i]).structure[(int)sep].Priority;
					if(prty>nMaxPriorityOfMaxSame)
					{
						nMaxPriorityOfMaxSame = prty;
						bestLine = ((AsciiLineAnalyzer)result[i]).structure[(int)sep];
					}
				}// if
			} // for
			return nNumberOfMaxSame;
		}


		public AsciiImportOptions Analyze(int nLines, AsciiImportOptions defaultImportOptions)
		{

			string sLine;
			System.IO.StreamReader sr = new System.IO.StreamReader(stream,System.Text.Encoding.ASCII,true);
			System.Collections.ArrayList result = new System.Collections.ArrayList();
		
			for(int i=0;i<nLines;i++)
			{
				sLine = sr.ReadLine();
				if(null==sLine)
					break;
				result.Add(new AsciiLineAnalyzer(i,sLine));
			}
		
			// now view the results
			// calc the frequency o
			System.Collections.SortedList sl= new System.Collections.SortedList();
			int nItems;
			// first the tabs

			/*
			sl.Clear();
			for(int i=0;i<result.Count;i++)
			{
				nItems = ((AsciiLineAnalyzer)result[i]).nNumberOfTabs;
				if(0!=nItems)
				{
					if(null==sl[nItems])
						sl.Add(nItems,1);
					else 
						sl[nItems] = 1+(int)sl[nItems];
				}
			}
			// get the tab count with the topmost frequency
			int nMaxNumberOfSameTabs = 0;
			int nMaxTabsOfSameNumber = 0;
			for(int i=0;i<sl.Count;i++)
			{
				if(nMaxNumberOfSameTabs<(int)sl.GetByIndex(i))
				{
					nMaxNumberOfSameTabs = (int)sl.GetByIndex(i);
					nMaxTabsOfSameNumber = (int)sl.GetKey(i);
				}
			}
*/
			
			
			// Count the commas
			sl.Clear();
			for(int i=0;i<result.Count;i++)
			{
				nItems = ((AsciiLineAnalyzer)result[i]).nNumberOfCommas;
				if(0!=nItems)
				{
					if(null==sl[nItems])
						sl.Add(nItems,1);
					else 
						sl[nItems] = 1+(int)sl[nItems];
				}
			}
			// get the comma count with the topmost frequency
			int nMaxNumberOfSameCommas = 0;
			int nMaxCommasOfSameNumber = 0;
			for(int i=0;i<sl.Count;i++)
			{
				if(nMaxNumberOfSameCommas<(int)sl.GetByIndex(i))
				{
					nMaxNumberOfSameCommas = (int)sl.GetByIndex(i);
					nMaxCommasOfSameNumber = (int)sl.GetKey(i);
				}
			}

			// Count the semicolons
			sl.Clear();
			for(int i=0;i<result.Count;i++)
			{
				nItems = ((AsciiLineAnalyzer)result[i]).nNumberOfSemicolons;
				if(0!=nItems)
				{
					if(null==sl[nItems])
						sl.Add(nItems,1);
					else 
						sl[nItems] = 1+(int)sl[nItems];
				}
			}
			// get the tab count with the topmost frequency
			int nMaxNumberOfSameSemicolons = 0;
			int nMaxSemicolonsOfSameNumber = 0;
			for(int i=0;i<sl.Count;i++)
			{
				if(nMaxNumberOfSameSemicolons<(int)sl.GetByIndex(i))
				{
					nMaxNumberOfSameSemicolons = (int)sl.GetByIndex(i);
					nMaxSemicolonsOfSameNumber = (int)sl.GetKey(i);
				}
			}

		
			NumberAndStructure[] st = new NumberAndStructure[3];

			for(int i=0;i<3;i++)
			{
				st[i].nLines = GetPriorityOf(result,(AsciiLineAnalyzer.Separation)i,ref st[i].structure);
			}

			// look for the top index
		
			int nMaxLines = int.MinValue;
			double maxprtylines=0;
			int nBestSeparator = int.MinValue;
			for(int i=0;i<3;i++)
			{
				double prtylines = (double)st[i].nLines * st[i].structure.Priority;
				if(prtylines==maxprtylines)
				{
					if(st[i].nLines > nMaxLines)
					{
						nMaxLines = st[i].nLines;
						nBestSeparator = i;
					}
				}
				else if(prtylines>maxprtylines)
				{
					maxprtylines = prtylines;
					nBestSeparator = i;
					nMaxLines=st[i].nLines;
				}
			}

			AsciiImportOptions opt = defaultImportOptions.Clone();
			
			opt.bDelimited = true;
			opt.cDelimiter = nBestSeparator==0 ? '\t' : (nBestSeparator==1 ? ',' : ';');
			opt.recognizedStructure = st[nBestSeparator].structure;

			for(int i=0;i<result.Count;i++)
			{
				opt.nMainHeaderLines=i;
				if(((AsciiLineAnalyzer)result[i]).structure[nBestSeparator].IsCompatibleWith(opt.recognizedStructure))
					break;
			}

			return opt;

		}


		public void ImportAscii(AsciiImportOptions impopt, Altaxo.Data.DataTable table)
		{
			string sLine;
			stream.Position=0; // rewind the stream to the beginning
			System.IO.StreamReader sr = new System.IO.StreamReader(stream,System.Text.Encoding.ASCII,true);
			System.Collections.ArrayList newcols = new System.Collections.ArrayList();
		

			// in case a structure is provided, allocate already the columsn
			
			if(null!=impopt.recognizedStructure)
			{
				for(int i=0;i<impopt.recognizedStructure.Count;i++)
				{
					if(impopt.recognizedStructure[i]==typeof(Double))
						newcols.Add(new Altaxo.Data.DoubleColumn());
					else if(impopt.recognizedStructure[i]==typeof(DateTime))
						newcols.Add(new Altaxo.Data.DateTimeColumn());
					else if(impopt.recognizedStructure[i]==typeof(string))
						newcols.Add(new Altaxo.Data.TextColumn());
					else
						newcols.Add(new Altaxo.Data.DBNullColumn());;
				}
			}

			char [] splitchar = new char[]{impopt.cDelimiter};

			// first of all, read the header if existent
			for(int i=0;i<impopt.nMainHeaderLines;i++)
			{
				sLine = sr.ReadLine();
				if(null==sLine) break;

				string[] substr = sLine.Split(splitchar);
				int cnt = substr.Length;
				for(int k=0;k<cnt;k++)
				{
					if(substr[k].Length==0)
						continue;
				
					if(i==0) // is it the column name line
					{
						((Altaxo.Data.DataColumn)newcols[k]).ColumnName = substr[k];
					}
				}
			}
			
			for(int i=0;true;i++)
			{
				sLine = sr.ReadLine();
				if(null==sLine) break;

				string[] substr = sLine.Split(splitchar);
				int cnt = substr.Length;
				for(int k=0;k<cnt;k++)
				{
					if(substr[k].Length==0)
						continue;

					if(newcols[k] is Altaxo.Data.DoubleColumn)
					{
						try { ((Altaxo.Data.DoubleColumn)newcols[k])[i] = System.Convert.ToDouble(substr[k]); }
						catch {}
					}
					else if( newcols[k] is Altaxo.Data.DateTimeColumn)
					{
						try { ((Altaxo.Data.DateTimeColumn)newcols[k])[i] = System.Convert.ToDateTime(substr[k]); }
						catch {}
					}
					else if( newcols[k] is Altaxo.Data.TextColumn)
					{
						((Altaxo.Data.TextColumn)newcols[k])[i] = substr[k];
					}
					else if(null==newcols[k] || newcols[k] is Altaxo.Data.DBNullColumn)
					{
						bool bConverted = false;
						double val=Double.NaN;
						DateTime valDateTime=DateTime.MinValue;

						try
						{ 
							val = System.Convert.ToDouble(substr[k]);
							bConverted=true;
						}
						catch
						{
						}
						if(bConverted)
						{
							Altaxo.Data.DoubleColumn newc = new Altaxo.Data.DoubleColumn();
							newc[i]=val;
							if(newcols[k] is Altaxo.Data.DBNullColumn)
								newc.CopyHeaderFrom((Altaxo.Data.DBNullColumn)newcols[k]);
							newcols[k] = newc;
						}
						else
						{
							try
							{ 
								valDateTime = System.Convert.ToDateTime(substr[k]);
								bConverted=true;
							}
							catch
							{
							}
							if(bConverted)
							{
								Altaxo.Data.DateTimeColumn newc = new Altaxo.Data.DateTimeColumn();
								newc[i]=valDateTime;
								if(newcols[k] is Altaxo.Data.DBNullColumn)
									newc.CopyHeaderFrom((Altaxo.Data.DBNullColumn)newcols[k]);
								newcols[k] = newc;
							}
							else
							{
								newcols[k] = new Altaxo.Data.TextColumn();
								((Altaxo.Data.TextColumn)newcols[k])[i]=substr[k];
							}
						} // end outer if null==newcol
					}
				} // end of for all cols


			} // end of for all lines
			
			// insert the new columns or replace the old ones
			table.SuspendDataChangedNotifications();
			for(int i=0;i<newcols.Count;i++)
			{
				if(newcols[i] is Altaxo.Data.DBNullColumn)
					continue;
				table.Add(i,(Altaxo.Data.DataColumn)newcols[i]);
			} // end for loop
			table.ResumeDataChangedNotifications();
		} // end of function ImportAscii
	} // end class 
	public class AsciiImportOptions
	{
		public bool bRenameColumns; /// rename the columns if 1st line contain  the column names
		public bool bRenameWorksheet; // rename the worksheet to the data file name

		public int nMainHeaderLines; // lines to skip (the main header)
		public bool bDelimited;      // true if delimited by a single char
		public char cDelimiter;      // the delimiter char

		public AsciiLineStructure recognizedStructure=null;


		public AsciiImportOptions Clone()
		{
			return (AsciiImportOptions)MemberwiseClone();
		}

	}

}
