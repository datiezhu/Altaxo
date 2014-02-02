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

using Altaxo.Graph.Gdi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;  // For use of the GuidAttribute, ProgIdAttribute and ClassInterfaceAttribute.
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Altaxo.Com
{
	using UnmanagedApi.Ole32;

	/// <summary>
	/// Supports the embedding of a Altaxo Graph document as a mini-project.
	/// </summary>
	[
		Guid(GraphDocumentEmbeddedComObject.GUID_STRING),  // the CLSID
		ProgId(GraphDocumentEmbeddedComObject.USER_TYPE),
		ClassInterface(ClassInterfaceType.None)  // Specify that we will not generate any additional interface
		]
	public class GraphDocumentEmbeddedComObject :
		OleObjectBase,
		System.Runtime.InteropServices.ComTypes.IDataObject,
		IOleObject,
		IPersistStorage
	{
		public const string GUID_STRING = "0915F010-2A4C-43F5-B230-A89340CF862C";
		public const string USER_TYPE = "Altaxo.Graph.0";
		public const string USER_TYPE_LONG = "Altaxo Graph-Document";
		public const double PointsToHimetric = 2540 / 72.0;

		private ManagedDataAdviseHolder _dataAdviseHolder;

		private int _lastVerb;

		private GraphDocument _document;

		public GraphDocumentEmbeddedComObject(ComManager comManager)
			: base(comManager)
		{
#if COMLOGGING
			Debug.ReportInfo("{0} constructor.", this.GetType().Name);
#endif

			_dataAdviseHolder = new ManagedDataAdviseHolder();
			_oleAdviseHolder = new ManagedOleAdviseHolderFM();
		}

		public void Dispose()
		{
			if (_isDocumentDirty)
			{
#if COMLOGGING
				Debug.ReportInfo("{0}.Dispose Step 0 : Document is dirty -> Advise DataChanged", this.GetType().Name);
#endif
				SendAdvise_DataChanged(); // update the image of the graph before we close
			}

#if COMLOGGING
			Debug.ReportInfo("{0}.Dispose Step 1 : SaveObject", this.GetType().Name);
#endif

			SendAdvise_SaveObject(); // make an advise to save the mini-project into the container application

#if COMLOGGING
			Debug.ReportInfo("{0}.Dispose Step 2 : Calling SendAdvise.HideWindow", this.GetType().Name);
#endif

			SendAdvise_HideWindow();

#if COMLOGGING
			Debug.ReportInfo("{0}.Dispose Step 3 : Calling SendAdvise.Closed", this.GetType().Name);
#endif

			SendAdvise_Closed();

			// if we had a document moniker, we should unregister it here
			// but since this is an embedded object,we have no document moniker

			// Disconnect the container.
#if COMLOGGING
			Debug.ReportInfo("{0}.Dispose Step 4 : Disconnecting this object", this.GetType().Name);
#endif

			Ole32Func.CoDisconnectObject(this, 0);

#if COMLOGGING
			Debug.ReportInfo("{0}.Dispose completed.", this.GetType().Name);
#endif
		}

		#region Document management

		public GraphDocument Document
		{
			get
			{
				return _document;
			}
			private set
			{
				if (object.ReferenceEquals(_document, value))
					return;

				if (null != _document)
				{
					_document.Changed -= EhDocumentChanged;
				}

				var oldValue = _document;
				_document = value;

				if (null != _document)
				{
					_document.Changed += EhDocumentChanged;
				}

				_comManager.NotifyDocumentOfDocumentsComObjectChanged(this, oldValue, _document);
			}
		}

		private void EhDocumentChanged(object sender, EventArgs e)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.EhDocumentChanged", this.GetType().Name);
#endif
			_isDocumentDirty = true;

			// see Brockschmidt Inside Ole 2nd edition, page 909
			// we must send IDataAdviseHolder:SendOnDataChange
			//SendAdvise_DataChanged();
		}

		/// <summary>
		/// Called by the ComManager when  document of this instance was renamed.
		/// </summary>
		public void EhDocumentRenamed(IMoniker fileMoniker)
		{
			// Trick to create a new document moniker, and send the advise
			_isDocumentDirty = true;
		}

		#endregion Document management

		#region IDataObject members

		protected override IList<Rendering> Renderings
		{
			get
			{
				List<Rendering> renderings = new List<Rendering>();

				// Renderings are in the order of preference.
				// First the Embedding format
				// then the enhanced metafile format for visual representation

				// Allows us to be embedded with an OLE container.
				// No callback because this should go via GetDataHere.
				// EMBEDSOURCE and OBJECTDESCRIPTOR should be placed after private data,
				// but before the presentations (Brockschmidt, Inside Ole 2nd ed. page 911)
				renderings.Add(new Rendering(DataObjectHelper.CF_EMBEDSOURCE, TYMED.TYMED_ISTORAGE, null));
				renderings.Add(new Rendering(DataObjectHelper.CF_OBJECTDESCRIPTOR, TYMED.TYMED_HGLOBAL, GraphDocumentDataObject.RenderEmbeddedObjectDescriptor));

				// Nice because it is resolution independent.
				renderings.Add(new Rendering((short)CF.CF_ENHMETAFILE, TYMED.TYMED_ENHMF, RenderEnhMetaFile));

				return renderings;
			}
		}

		private IntPtr RenderEnhMetaFile(TYMED tymed)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.RenderEnhMetafile", this.GetType().Name);
#endif

			var docSize = _document.Size;
			using (var bmp = Altaxo.Graph.Gdi.GraphDocumentExportActions.RenderAsBitmap(_document, System.Drawing.Brushes.Transparent, System.Drawing.Imaging.PixelFormat.Format32bppArgb, 300, 300))
			{
				return DataObjectHelper.RenderEnhMetafileIntPtr(docSize.X, docSize.Y,
				(grfx) =>
				{
					grfx.DrawImage(bmp, 0, 0);
				}
				);
			}
		}

		protected override ManagedDataAdviseHolder DataAdviseHolder
		{
			get { return _dataAdviseHolder; }
		}

		protected override bool InternalGetDataHere(ref System.Runtime.InteropServices.ComTypes.FORMATETC format, ref System.Runtime.InteropServices.ComTypes.STGMEDIUM medium)
		{
			if (format.cfFormat == DataObjectHelper.CF_EMBEDSOURCE && (format.tymed & TYMED.TYMED_ISTORAGE) != 0)
			{
				medium.tymed = TYMED.TYMED_ISTORAGE;
				medium.pUnkForRelease = null;
				var stg = (IStorage)Marshal.GetObjectForIUnknown(medium.unionmember);

				Save(stg, false);
				return true;
			}

			return false;
		}

		#endregion IDataObject members

		#region IOleObject members

		public int Close(tagOLECLOSE dwSaveOption)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IOleObject.Close {1}", this.GetType().Name, dwSaveOption);
#endif
			try
			{
				bool save = false, cancelled = false;

				switch (dwSaveOption)
				{
					case tagOLECLOSE.OLECLOSE_SAVEIFDIRTY:
						save = true;
						break;

					case tagOLECLOSE.OLECLOSE_PROMPTSAVE:
						_comManager.InvokeGuiThread(
							 new Action(() =>
						 {
							 // If asked to prompt, do so only if dirty: if we get a YES, save as
							 // usual and close.  On NO, just close.  On CANCEL, return
							 // OLE_E_PROMPTSAVECANCELLED.
							 var r = System.Windows.MessageBox.Show("Save?", "Box", System.Windows.MessageBoxButton.YesNoCancel);
							 switch (r)
							 {
								 case System.Windows.MessageBoxResult.Yes:
									 save = true;
									 break;

								 case System.Windows.MessageBoxResult.Cancel:
									 cancelled = true;
									 break;
							 }
						 }));
						break;

					case tagOLECLOSE.OLECLOSE_NOSAVE:
						break;

					default:
#if COMLOGGING
						Debug.ReportError("{0}.IOleObject.Close called with unknown parameter: {1}", this.GetType().Name, dwSaveOption);
#endif
						break;
				}

				if (cancelled)
				{
#if COMLOGGING
					Debug.ReportInfo("{0}.IOleObject.Close -> OLE_E_PROMPTSAVECANCELLED", this.GetType().Name);
#endif
					return ComReturnValue.OLE_E_PROMPTSAVECANCELLED;
				}

				if (save)
				{
					SendAdvise_SaveObject();
					SendAdvise_Saved();
				}

				// Begin shutdown of the application
#if COMLOGGING
				Debug.ReportInfo("{0}.IOleObject.Close -> BeginInvoke MainWindow.Close", this.GetType().Name);
#endif
				_comManager.ApplicationAdapter.BeginClosingApplication();
				return ComReturnValue.NOERROR;
			}
			catch (Exception e)
			{
#if COMLOGGING
				Debug.ReportError("{0}.IOleObject.Close threw an exception: {1}", this.GetType().Name, e);
#endif
				throw;
			}
			// }
		}

		public int DoVerb(int iVerb, IntPtr lpmsg, IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, COMRECT lprcPosRect)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IOleObject.DoVerb {1}", this.GetType().Name, iVerb);
#endif
			try
			{
				_lastVerb = iVerb;
				switch (iVerb)
				{
					case (int)OLEIVERB.OLEIVERB_HIDE:
#if COMLOGGING
						Debug.ReportInfo("{0}.IOleObject.DoVerb OLEIVERB_HIDE", this.GetType().Name);
#endif
						_comManager.ApplicationAdapter.HideMainWindow();
						SendAdvise_HideWindow();
						break;

					case (int)OLEIVERB.OLEIVERB_PRIMARY:
					case (int)OLEIVERB.OLEIVERB_SHOW:
					case (int)OLEIVERB.OLEIVERB_OPEN:

#if COMLOGGING
						if ((int)OLEIVERB.OLEIVERB_PRIMARY == iVerb) Debug.ReportInfo("{0}.IOleObject.DoVerb OLEIVERB_PRIMARY", this.GetType().Name);
						if ((int)OLEIVERB.OLEIVERB_SHOW == iVerb) Debug.ReportInfo("{0}.IOleObject.DoVerb OLEIVERB_SHOW", this.GetType().Name);
						if ((int)OLEIVERB.OLEIVERB_OPEN == iVerb) Debug.ReportInfo("{0}.IOleObject.DoVerb OLEIVERB_OPEN", this.GetType().Name);
#endif
						_comManager.ApplicationAdapter.ShowMainWindow();
						if (pActiveSite != null)
						{
#if COMLOGGING
							Debug.ReportInfo("{0}.IOleObject.DoVerb -> calling ClientSite.ShowObject()", this.GetType().Name);
#endif
							try
							{
								pActiveSite.ShowObject();
							}
							catch (Exception ex)
							{
#if COMLOGGING
								Debug.ReportInfo("{0}.IOleObject.DoVerb pActiveSite.ShowObject caused an exception: {1}", this.GetType().Name, ex);
#endif
							}

							SendAdvise_ShowWindow();
						}

						return ComReturnValue.NOERROR;

					default:
#if COMLOGGING
						Debug.ReportError("{0}.IOleObject.DoVerb Unexpected verb: {1}", this.GetType().Name, iVerb);
#endif
						return ComReturnValue.OLEOBJ_S_INVALIDVERB;
				}
#if COMLOGGING
				Debug.ReportInfo("{0}.IOleObject.DoVerb -> returning NOERROR", this.GetType().Name);
#endif
				return ComReturnValue.NOERROR;
			}
			catch (Exception e)
			{
#if COMLOGGING
				Debug.ReportError("{0}.IOleObject.DoVerb throw an exception. Details: {1}", this.GetType().Name, e);
#endif
				throw;
			}
		}

		public int SetExtent(int dwDrawAspect, tagSIZEL pSizel)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IOleObject.SetExtent({1}x{2}) -> not supported.", this.GetType().Name, pSizel.cx, pSizel.cy);
#endif

			/*
			if ((dwDrawAspect & (int)DVASPECT.DVASPECT_CONTENT) != 0)
			{
				try
				{
					_document.Size = new Graph.PointD2D(pSizel.cx / PointsToHimetric, pSizel.cy / PointsToHimetric);
					return ComReturnValue.S_OK;
				}
				catch (Exception e)
				{
#if COMLOGGING
					Debug.ReportError("{0}.SetExtent threw an exception: {1}", this.GetType().Name, e);
#endif
				}
			}
*/
			return ComReturnValue.E_FAIL;
		}

		public int GetExtent(int dwDrawAspect, tagSIZEL pSizel)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IOleObject.GetExtent", this.GetType().Name);
#endif
			if ((dwDrawAspect & (int)DVASPECT.DVASPECT_CONTENT) == 0)
				return ComReturnValue.E_FAIL;

			var docSize_pt = _document.Size;

			pSizel = new tagSIZEL((int)(docSize_pt.X * PointsToHimetric), (int)(docSize_pt.Y * PointsToHimetric));

			return ComReturnValue.NOERROR;
		}

		public int GetMiscStatus(int dwAspect, out int misc)
		{
			misc = GraphDocumentDataObject.MiscStatus(dwAspect);

#if COMLOGGING
			Debug.ReportInfo("{0}.IOleObject.GetMiscStatus -> returning 0x{1:X}", this.GetType().Name, misc);
#endif

			return ComReturnValue.S_OK;
		}

		#endregion IOleObject members

		#region IPersistStorage mebers

		public void GetClassID(out Guid pClassID)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.GetClassID", this.GetType().Name);
#endif
			pClassID = this.GetType().GUID;
		}

		public int IsDirty()
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.IsDirty returning {1}", this.GetType().Name, _isDocumentDirty);
#endif

			if (_isDocumentDirty)
				return ComReturnValue.S_OK;
			else
				return ComReturnValue.S_FALSE;
		}

		public void InitNew(IStorage pstg)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.InitNew", this.GetType().Name);
#endif

			Document = Current.ProjectService.CreateNewGraph().Doc;

			// We don't need an IStorage except at Load/Save.
			_isDocumentDirty = true; // but we set the document dirty flag thus it is saved
		}

		public int Load(IStorage pstg)
		{
			System.Diagnostics.Debug.Assert(null == _document);

			string documentName = null;
			Version altaxoVersion;
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.Load", this.GetType().Name);
#endif

			try
			{
				using (var stream = new ComStreamWrapper(pstg.OpenStream("AltaxoVersion", IntPtr.Zero, (int)(STGM.READ | STGM.SHARE_EXCLUSIVE), 0), true))
				{
					var bytes = new byte[stream.Length];
					stream.Read(bytes, 0, bytes.Length);
					var versionString = System.Text.Encoding.UTF8.GetString(bytes);
					altaxoVersion = Version.Parse(versionString);
				}

				using (var stream = new ComStreamWrapper(pstg.OpenStream("AltaxoGraphName", IntPtr.Zero, (int)(STGM.READ | STGM.SHARE_EXCLUSIVE), 0), true))
				{
					var bytes = new byte[stream.Length];
					stream.Read(bytes, 0, bytes.Length);
					documentName = System.Text.Encoding.UTF8.GetString(bytes);
				}
#if COMLOGGING
				Debug.ReportInfo("{0}.IPersistStorage.Load -> Name of GraphDocument: {1}", this.GetType().Name, documentName);
#endif
			}
			catch (Exception ex)
			{
#if COMLOGGING
				Debug.ReportInfo("{0}.IPersistStorage.Load Failed to load stream GraphName, exception: {1}", this.GetType().Name, ex);
#endif
			}

			try
			{
				using (var streamWrapper = new ComStreamWrapper(pstg.OpenStream("AltaxoProjectZip", IntPtr.Zero, (int)(STGM.READ | STGM.SHARE_EXCLUSIVE), 0), true))
				{
					_comManager.InvokeGuiThread(() =>
					{
						Current.ProjectService.CloseProject(true);
						Current.ProjectService.LoadProject(streamWrapper);
					});
				}
#if COMLOGGING
				Debug.ReportInfo("{0}.IPersistStorage.Load Project loaded", this.GetType().Name);
#endif
			}
			catch (Exception ex)
			{
#if COMLOGGING
				Debug.ReportInfo("{0}.IPersistStorage.Load Failed to load stream AltaxoProjectZip, exception: {1}", this.GetType().Name, ex);
#endif
			}

			Marshal.ReleaseComObject(pstg);

			Altaxo.Graph.Gdi.GraphDocument newDocument = null;

			if (null != documentName && Current.Project.GraphDocumentCollection.Contains(documentName))
				newDocument = Current.Project.GraphDocumentCollection[documentName];
			else if (null != Current.Project.GraphDocumentCollection.FirstOrDefault())
				newDocument = Current.Project.GraphDocumentCollection.First();

			if (null != newDocument)
			{
				Document = newDocument;
				_comManager.InvokeGuiThread(() => Current.ProjectService.ShowDocumentView(_document));
			}

			if (null == Document)
			{
#if COMLOGGING
				Debug.ReportError("{0}.IPersistStorage.Load Document is null, have to throw an exception now!!", this.GetType().Name);
#endif
				throw new InvalidOperationException();
			}

			_isDocumentDirty = false;
			return ComReturnValue.S_OK;
		}

		public void Save(IStorage pStgSave, bool fSameAsLoad)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.Save fSameAsLoad={1}", this.GetType().Name, fSameAsLoad);
#endif

			try
			{
				Exception saveEx = null;

				Ole32Func.WriteClassStg(pStgSave, this.GetType().GUID);

				// Store the version of this assembly
				{
					var assembly = System.Reflection.Assembly.GetExecutingAssembly();
					Version version = assembly.GetName().Version;
					using (var stream = new ComStreamWrapper(pStgSave.CreateStream("AltaxoVersion", (int)(STGM.DIRECT | STGM.READWRITE | STGM.CREATE | STGM.SHARE_EXCLUSIVE), 0, 0), true))
					{
						string text = version.ToString();
						byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(text);
						stream.Write(nameBytes, 0, nameBytes.Length);
					}
				}

				// Store the name of the item
				using (var stream = new ComStreamWrapper(pStgSave.CreateStream("AltaxoGraphName", (int)(STGM.DIRECT | STGM.READWRITE | STGM.CREATE | STGM.SHARE_EXCLUSIVE), 0, 0), true))
				{
					byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(_document.Name);
					stream.Write(nameBytes, 0, nameBytes.Length);
				}

				// Store the project
				using (var stream = new ComStreamWrapper(pStgSave.CreateStream("AltaxoProjectZip", (int)(STGM.DIRECT | STGM.READWRITE | STGM.CREATE | STGM.SHARE_EXCLUSIVE), 0, 0), true))
				{
					_comManager.InvokeGuiThread(() =>
					{
						saveEx = Current.ProjectService.SaveProject(stream);
					}
					);
				}

				_isDocumentDirty = false;

				if (null != saveEx)
					throw saveEx;
			}
			catch (Exception ex)
			{
#if COMLOGGING
				Debug.ReportError("{0}.IPersistStorage:Save threw an exception: {1}", this.GetType().Name, ex);
#endif
			}
			finally
			{
				Marshal.ReleaseComObject(pStgSave);
			}
		}

		public void SaveCompleted(IStorage pStgNew)
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.SaveCompleted", this.GetType().Name);
#endif

			SendAdvise_Saved();
		}

		public int HandsOffStorage()
		{
#if COMLOGGING
			Debug.ReportInfo("{0}.IPersistStorage.HandsOffStorage", this.GetType().Name);
#endif
			return ComReturnValue.S_OK;
		}

		#endregion IPersistStorage mebers
	}
}