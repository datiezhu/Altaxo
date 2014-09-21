﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2014 Dr. Dirk Lellinger
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Com
{
	using UnmanagedApi.Ole32;

	/// <summary>
	/// IDataAdviseHolder's interface methods must always be called from the same thread in which IDataAdviseHolder was created. Thus we bundle it here together with an InvokeableThread.
	/// </summary>
	public class ManagedDataAdviseHolder : IDisposable
	{
		private IDataAdviseHolder _dataAdviseHolder;

		private InvokeableThread _dataAdviseThread;

		public ManagedDataAdviseHolder()
		{
			_dataAdviseThread = new InvokeableThread("DataAdviseThread", ((System.Windows.Window)Current.Workbench.ViewObject).Dispatcher);

			this.Invoke("Creation of IDataAdviseHolder", () =>
				{
					int res = Ole32Func.CreateDataAdviseHolder(out _dataAdviseHolder);
					System.Diagnostics.Debug.Assert(res == ComReturnValue.S_OK);
				});
		}

		/// <summary>Makes invocation of Invoke more concise with parameter-
		/// less methods.</summary>
		protected void Invoke(string actionName, Action invoker)
		{
			ComDebug.ReportInfo("DataAdviseThread {0}", actionName);
			_dataAdviseThread.Invoke(invoker);
		}

		public void Advise(System.Runtime.InteropServices.ComTypes.IDataObject dataObject, ref System.Runtime.InteropServices.ComTypes.FORMATETC fetc, System.Runtime.InteropServices.ComTypes.ADVF advf, System.Runtime.InteropServices.ComTypes.IAdviseSink advise, out int pdwConnection)
		{
			int con = -1;
			System.Runtime.InteropServices.ComTypes.FORMATETC etc = fetc;

			Invoke("Advise", () => _dataAdviseHolder.Advise(dataObject, ref etc, advf, advise, out con));
			System.Diagnostics.Debug.Assert(-1 != con); // make sure that our Invoke-Construction is really working

			ComDebug.ReportInfo("ManagedDataAdviseHolder giving out con={0}", con);
			pdwConnection = con;
		}

		public void Unadvise(int dwConnection)
		{
			Invoke("Unadvise", () => _dataAdviseHolder.Unadvise(dwConnection));
		}

		public System.Runtime.InteropServices.ComTypes.IEnumSTATDATA EnumAdvise()
		{
			System.Runtime.InteropServices.ComTypes.IEnumSTATDATA result = null;
			Invoke("EnumAdvise", () => result = _dataAdviseHolder.EnumAdvise());
			return result;
		}

		public void SendOnDataChange(System.Runtime.InteropServices.ComTypes.IDataObject dataObject, int dwReserved, System.Runtime.InteropServices.ComTypes.ADVF advf)
		{
			Invoke("SendOnDataChange", () => _dataAdviseHolder.SendOnDataChange(dataObject, dwReserved, advf));
		}

		public void Dispose()
		{
			_dataAdviseThread.Dispose();
			_dataAdviseThread = null;
			_dataAdviseHolder = null;
		}
	}
}