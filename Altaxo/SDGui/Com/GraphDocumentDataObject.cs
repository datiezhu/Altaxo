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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Altaxo.Com
{
	using UnmanagedApi.Kernel32;
	using UnmanagedApi.Ole32;

	public class GraphDocumentDataObject : DataObjectBase, System.Runtime.InteropServices.ComTypes.IDataObject
	{
		private ManagedDataAdviseHolder _dataAdviseHolder;
		private AltaxoDocument _altaxoMiniProject;
		private string _graphDocumentName;
		private Altaxo.Graph.PointD2D _graphDocumentSize;
		private System.Drawing.Image _graphDocumentClipboardImage;
		private string _graphDocumentDropdownFileName;
		private GraphClipboardExportOptions _graphExportOptions;

		public GraphDocumentDataObject(GraphDocument graphDocument, ProjectFileComObject fileComObject, ComManager comManager)
			: base(comManager)
		{
			ComDebug.ReportInfo("{0} constructor.", this.GetType().Name);
			_dataAdviseHolder = new ManagedDataAdviseHolder();

			_graphDocumentName = graphDocument.Name;
			_graphDocumentSize = graphDocument.Size;

			_graphExportOptions = graphDocument.GetPropertyValue(GraphDocumentClipboardActions.PropertyKeyCopyPageSettings, () => new GraphClipboardExportOptions());
			_graphExportOptions = (GraphClipboardExportOptions)_graphExportOptions.Clone();
			GraphDocumentClipboardActions.GetImageForClipboard(graphDocument, _graphExportOptions, out _graphDocumentClipboardImage, out _graphDocumentDropdownFileName);

			if (_graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsEmbeddedObject))
			{
				var miniProjectBuilder = new Altaxo.Graph.Procedures.MiniProjectBuilder();
				_altaxoMiniProject = miniProjectBuilder.GetMiniProject(graphDocument, true);
			}
			else
			{
				_altaxoMiniProject = null;
			}
		}

		~GraphDocumentDataObject()
		{
			ComDebug.ReportInfo("{0} destructor.", this.GetType().Name);

			if (null != _dataAdviseHolder)
			{
				_dataAdviseHolder.Dispose();
				_dataAdviseHolder = null;
			}
		}

		#region Base class overrides

		protected override IList<Rendering> Renderings
		{
			get
			{
				var list = new List<Rendering>();

				if (null != _altaxoMiniProject && _graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsEmbeddedObject))
				{
					list.Add(new Rendering(DataObjectHelper.CF_EMBEDSOURCE, TYMED.TYMED_ISTORAGE, null));
					list.Add(new Rendering(DataObjectHelper.CF_OBJECTDESCRIPTOR, TYMED.TYMED_HGLOBAL, RenderEmbeddedObjectDescriptor));
				}

				if ((_graphDocumentClipboardImage is System.Drawing.Imaging.Metafile) &&
						(_graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsNative) || _graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsNativeWrappedInEnhancedMetafile))
						)
				{
					list.Add(new Rendering(CF.CF_ENHMETAFILE, TYMED.TYMED_ENHMF, RenderEnhancedMetaFile));
					list.Add(new Rendering(CF.CF_METAFILEPICT, TYMED.TYMED_MFPICT, RenderWindowsMetafilePict));
				}

				if (_graphDocumentClipboardImage is System.Drawing.Bitmap && _graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsNativeWrappedInEnhancedMetafile))
				{
					list.Add(new Rendering(CF.CF_ENHMETAFILE, TYMED.TYMED_ENHMF, RenderEnhancedMetaFile));
					list.Add(new Rendering(CF.CF_METAFILEPICT, TYMED.TYMED_MFPICT, RenderWindowsMetafilePict));
				}

				if (_graphDocumentClipboardImage is System.Drawing.Bitmap && _graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsNative))
				{
					list.Add(new Rendering(CF.CF_BITMAP, TYMED.TYMED_GDI, RenderBitmap));
					list.Add(new Rendering(CF.CF_DIB, TYMED.TYMED_HGLOBAL, RenderBitmapDIB));
				}

				if (!string.IsNullOrEmpty(_graphDocumentDropdownFileName) && _graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsDropDownList))
				{
					list.Add(new Rendering(CF.CF_HDROP, TYMED.TYMED_HGLOBAL, RenderBitmapAsDropFile));
				}

				if (!string.IsNullOrEmpty(Current.ProjectService.CurrentProjectFileName) && _graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsLinkedObject))
				{
					list.Add(new Rendering(DataObjectHelper.CF_LINKSOURCE, TYMED.TYMED_ISTREAM, RenderMoniker));
					list.Add(new Rendering(DataObjectHelper.CF_LINKSRCDESCRIPTOR, TYMED.TYMED_HGLOBAL, RenderLinkedObjectDescriptor));
				}

				return list;
			}
		}

		public override void ConvertToNetDataObjectAndPutToClipboard()
		{
			var result = new System.Windows.Forms.DataObject();

			if (_graphDocumentClipboardImage is System.Drawing.Imaging.Metafile)
			{
				result.SetImage(_graphDocumentClipboardImage);
			}
			else if (_graphExportOptions.ClipboardFormat.HasFlag(GraphCopyPageClipboardFormat.AsNativeWrappedInEnhancedMetafile))
			{
				result.SetImage(DataObjectHelper.RenderEnhancedMetafile(_graphDocumentClipboardImage, _graphDocumentSize.X, _graphDocumentSize.Y, UseMetafileDC.Printer));
			}

			if (_graphDocumentClipboardImage is System.Drawing.Bitmap)
			{
				result.SetImage(_graphDocumentClipboardImage);
			}

			if (!string.IsNullOrEmpty(_graphDocumentDropdownFileName))
			{
				var coll = new System.Collections.Specialized.StringCollection();
				coll.Add(_graphDocumentDropdownFileName);
				result.SetFileDropList(coll);
			}

			System.Windows.Forms.Clipboard.SetDataObject(result, true);
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
				// we don't save the document directly, since this would mean to save the whole (and probably huge) project
				// instead we first make a mini project with the neccessary data only and then save this instead
				InternalSaveMiniProject(stg, _altaxoMiniProject, _graphDocumentName);
				return true;
			}

			if (format.cfFormat == DataObjectHelper.CF_LINKSOURCE && (format.tymed & TYMED.TYMED_ISTREAM) != 0)
			{
				// we should make sure that ComManager is already started, so that the moniker can be used by the program
				if (!_comManager.IsActive)
					_comManager.StartLocalServer();

				IMoniker documentMoniker = CreateNewDocumentMoniker();

				if (null != documentMoniker)
				{
					medium.tymed = TYMED.TYMED_ISTREAM;
					medium.pUnkForRelease = null;
					IStream strm = (IStream)Marshal.GetObjectForIUnknown(medium.unionmember);
					DataObjectHelper.SaveMonikerToStream(documentMoniker, strm);
					return true;
				}
			}

			return false;
		}

		#endregion Base class overrides

		#region Renderings

		private IntPtr RenderWindowsMetafilePict(TYMED tymed)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.RenderWindowsMetafilePict");

			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_MFPICT);

			if (_graphDocumentClipboardImage is System.Drawing.Imaging.Metafile)
			{
				return DataObjectHelper.RenderWindowsMetafilePict(_graphDocumentSize.X, _graphDocumentSize.Y, (System.Drawing.Imaging.Metafile)_graphDocumentClipboardImage);
			}
			else
			{
				return DataObjectHelper.RenderWindowsMetafilePict(_graphDocumentClipboardImage, _graphDocumentSize.X, _graphDocumentSize.Y, UseMetafileDC.Printer);
			}
		}

		private IntPtr RenderEnhancedMetaFile(TYMED tymed)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.RenderEnhancedMetafile");

			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_ENHMF);

			if (_graphDocumentClipboardImage is System.Drawing.Imaging.Metafile)
			{
				var mf = (System.Drawing.Imaging.Metafile)_graphDocumentClipboardImage;
				var mfCloned = (System.Drawing.Imaging.Metafile)mf.Clone();
				return mfCloned.GetHenhmetafile();
			}
			else
			{
				return DataObjectHelper.RenderEnhancedMetafile(_graphDocumentClipboardImage, _graphDocumentSize.X, _graphDocumentSize.Y, UseMetafileDC.Printer).GetHenhmetafile();
			}
		}

		private IntPtr RenderBitmap(TYMED tymed)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.RenderBitmap");

			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_GDI);

			if (_graphDocumentClipboardImage is System.Drawing.Bitmap)
			{
				var bmp = (System.Drawing.Bitmap)_graphDocumentClipboardImage;
				return DataObjectHelper.RenderHBitmap(tymed, bmp, bmp.Width, bmp.Height);
			}
			else if (_graphDocumentClipboardImage is System.Drawing.Imaging.Metafile)
			{
				var mf = (System.Drawing.Imaging.Metafile)_graphDocumentClipboardImage;
				var dpi = _graphExportOptions.SourceDpiResolution;
				int pixelsX = (int)(dpi * _graphDocumentSize.X / 72.0);
				int pixelsY = (int)(dpi * _graphDocumentSize.Y / 72.0);
				return DataObjectHelper.RenderHBitmap(tymed, _graphDocumentClipboardImage, pixelsX, pixelsY);
			}
			else
				throw new NotImplementedException();
		}

		private IntPtr RenderBitmapDIB(TYMED tymed)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.RenderBitmap");

			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_HGLOBAL);

			if (_graphDocumentClipboardImage is System.Drawing.Bitmap)
			{
				var bmp = (System.Drawing.Bitmap)_graphDocumentClipboardImage;
				return DataObjectHelper.RenderDIBBitmapToHGLOBAL(bmp, bmp.Width, bmp.Height);
			}
			else if (_graphDocumentClipboardImage is System.Drawing.Imaging.Metafile)
			{
				var mf = (System.Drawing.Imaging.Metafile)_graphDocumentClipboardImage;
				var dpi = _graphExportOptions.SourceDpiResolution;
				int pixelsX = (int)(dpi * _graphDocumentSize.X / 72.0);
				int pixelsY = (int)(dpi * _graphDocumentSize.Y / 72.0);
				return DataObjectHelper.RenderDIBBitmapToHGLOBAL(_graphDocumentClipboardImage, pixelsX, pixelsY);
			}
			else
				throw new NotImplementedException();
		}

		private IntPtr RenderBitmapAsDropFile(TYMED tymed)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.BitmapAsDropFile");

			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_HGLOBAL);

			if (!string.IsNullOrEmpty(_graphDocumentDropdownFileName))
			{
				return DataObjectHelper.RenderFiles(new string[] { _graphDocumentDropdownFileName });
			}
			return IntPtr.Zero;
		}

		private IntPtr RenderMoniker(TYMED tymed)
		{
			ComDebug.ReportInfo("{0}.RenderMoniker", this.GetType().Name);

			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_ISTREAM);

			return DataObjectHelper.RenderMonikerToNewStream(tymed, CreateNewDocumentMoniker());
		}

		public static int MiscStatus(int aspect)
		{
			int misc = (int)OLEMISC.OLEMISC_CANTLINKINSIDE;
			if (aspect == (int)DVASPECT.DVASPECT_CONTENT)
			{
				// misc |= (int)OLEMISC.OLEMISC_RECOMPOSEONRESIZE;
				misc |= (int)OLEMISC.OLEMISC_RENDERINGISDEVICEINDEPENDENT;
			}
			return misc;
		}

		public IntPtr RenderEmbeddedObjectDescriptor(TYMED tymed)
		{
			return RenderEmbeddedObjectDescriptor(tymed, _graphDocumentSize);
		}

		public static IntPtr RenderEmbeddedObjectDescriptor(TYMED tymed, Altaxo.Graph.PointD2D graphDocumentSize)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.RenderEmbeddedObjectDescriptor");

			// Brockschmidt, Inside Ole 2nd ed. page 991
			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_HGLOBAL);
			// Fill in the basic information.
			OBJECTDESCRIPTOR od = new OBJECTDESCRIPTOR();
			// According to the documentation this is used just to find an icon.
			od.clsid = typeof(GraphDocumentEmbeddedComObject).GUID;
			od.dwDrawAspect = DVASPECT.DVASPECT_CONTENT;
			od.sizelcx = 0;  // zero in imitation of Word/Excel, but could be (int)(graphDocumentSize.X * 2540 / 72.0)
			od.sizelcy = 0;  // zero in imitation of Word/Excel, but could be (int)(graphDocumentSize.Y * 2540 / 72.0);
			od.pointlx = 0;
			od.pointly = 0;
			od.dwStatus = MiscStatus((int)od.dwDrawAspect);

			// Descriptive strings to tack on after the struct.
			string name = GraphDocumentEmbeddedComObject.USER_TYPE;
			int name_size = (name.Length + 1) * sizeof(char);
			string source = "Altaxo";
			int source_size = (source.Length + 1) * sizeof(char);
			int od_size = Marshal.SizeOf(typeof(OBJECTDESCRIPTOR));
			od.dwFullUserTypeName = od_size;
			od.dwSrcOfCopy = od_size + name_size;
			int full_size = od_size + name_size + source_size;
			od.cbSize = full_size;

			// To avoid 'unsafe', we will arrange the strings in a byte array.
			byte[] strings = new byte[full_size];
			Encoding unicode = Encoding.Unicode;
			Array.Copy(unicode.GetBytes(name), 0, strings, od.dwFullUserTypeName, name.Length * sizeof(char));
			Array.Copy(unicode.GetBytes(source), 0, strings, od.dwSrcOfCopy, source.Length * sizeof(char));

			// Combine the strings and the struct into a single block of mem.
			IntPtr hod = Kernel32Func.GlobalAlloc(GlobalAllocFlags.GHND, full_size);
			System.Diagnostics.Debug.Assert(hod != IntPtr.Zero);
			IntPtr buf = Kernel32Func.GlobalLock(hod);
			Marshal.Copy(strings, 0, buf, full_size);
			Marshal.StructureToPtr(od, buf, false);

			Kernel32Func.GlobalUnlock(hod);
			return hod;
		}

		public static IntPtr RenderLinkedObjectDescriptor(TYMED tymed)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.RenderLinkedObjectDescriptor");

			// Brockschmidt, Inside Ole 2nd ed. page 991
			System.Diagnostics.Debug.Assert(tymed == TYMED.TYMED_HGLOBAL);
			// Fill in the basic information.
			OBJECTDESCRIPTOR od = new OBJECTDESCRIPTOR();
			// According to the documentation this is used just to find an icon.
			od.clsid = typeof(GraphDocumentLinkedComObject).GUID;
			od.dwDrawAspect = DVASPECT.DVASPECT_CONTENT;
			od.sizelcx = 0; // zero in imitation of Word/Excel, but could be box.Extent.cx;
			od.sizelcy = 0; // zero in imitation of Word/Excel, but could be box.Extent.cy;
			od.pointlx = 0;
			od.pointly = 0;
			od.dwStatus = MiscStatus((int)od.dwDrawAspect);

			// Descriptive strings to tack on after the struct.
			string name = GraphDocumentLinkedComObject.USER_TYPE;
			int name_size = (name.Length + 1) * sizeof(char);
			string source = "Altaxo";
			int source_size = (source.Length + 1) * sizeof(char);
			int od_size = Marshal.SizeOf(typeof(OBJECTDESCRIPTOR));
			od.dwFullUserTypeName = od_size;
			od.dwSrcOfCopy = od_size + name_size;
			int full_size = od_size + name_size + source_size;
			od.cbSize = full_size;

			// To avoid 'unsafe', we will arrange the strings in a byte array.
			byte[] strings = new byte[full_size];
			Encoding unicode = Encoding.Unicode;
			Array.Copy(unicode.GetBytes(name), 0, strings, od.dwFullUserTypeName, name.Length * sizeof(char));
			Array.Copy(unicode.GetBytes(source), 0, strings, od.dwSrcOfCopy, source.Length * sizeof(char));

			// Combine the strings and the struct into a single block of mem.
			IntPtr hod = Kernel32Func.GlobalAlloc(GlobalAllocFlags.GHND, full_size);
			System.Diagnostics.Debug.Assert(hod != IntPtr.Zero);
			IntPtr buf = Kernel32Func.GlobalLock(hod);
			Marshal.Copy(strings, 0, buf, full_size);
			Marshal.StructureToPtr(od, buf, false);

			Kernel32Func.GlobalUnlock(hod);
			return hod;
		}

		#endregion Renderings

		#region Helper Functions

		private IMoniker CreateNewDocumentMoniker()
		{
			// create the moniker on the fly
			IMoniker documentMoniker = null;

			var fileMoniker = _comManager.FileComObject.FileMoniker;

			if (null != fileMoniker)
			{
				IMoniker itemMoniker;
				Ole32Func.CreateItemMoniker("!", DataObjectHelper.NormalStringToMonikerNameString(_graphDocumentName), out itemMoniker);
				if (null != itemMoniker)
				{
					fileMoniker.ComposeWith(itemMoniker, false, out documentMoniker);
				}
			}
			return documentMoniker;
		}

		public static void InternalSaveMiniProject(IStorage pStgSave, AltaxoDocument projectToSave, string graphDocumentName)
		{
			ComDebug.ReportInfo("GraphDocumentDataObject.InternalSaveMiniProject BEGIN");

			try
			{
				Exception saveEx = null;
				Ole32Func.WriteClassStg(pStgSave, typeof(GraphDocumentEmbeddedComObject).GUID);

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
					byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(graphDocumentName);
					stream.Write(nameBytes, 0, nameBytes.Length);
				}

				// Store the project
				using (var stream = new ComStreamWrapper(pStgSave.CreateStream("AltaxoProjectZip", (int)(STGM.DIRECT | STGM.READWRITE | STGM.CREATE | STGM.SHARE_EXCLUSIVE), 0, 0), true))
				{
					using (var zippedStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(stream))
					{
						var zippedStreamWrapper = new Altaxo.Main.ZipOutputStreamWrapper(zippedStream);
						var info = new Altaxo.Serialization.Xml.XmlStreamSerializationInfo();
						projectToSave.SaveToZippedFile(zippedStreamWrapper, info);
						zippedStream.Close();
					}
					stream.Close();
				}

				if (null != saveEx)
					throw saveEx;
			}
			catch (Exception ex)
			{
				ComDebug.ReportError("InternalSaveMiniProject, Exception ", ex);
			}
			finally
			{
				Marshal.ReleaseComObject(pStgSave);
			}

			ComDebug.ReportInfo("GraphDocumentDataObject.InternalSaveMiniProject END");
		}

		#endregion Helper Functions
	}
}