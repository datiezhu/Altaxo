﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Serialization
{
	public static class ClipboardSerialization
	{
		public static void PutObjectToClipboard(string clipBoardFormat, object toSerialize)
		{
			var dao = new System.Windows.Forms.DataObject();
			var stb = new System.Text.StringBuilder();
			var info = new Altaxo.Serialization.Xml.XmlStreamSerializationInfo();
			info.BeginWriting(stb);

			info.AddValue("Object", toSerialize);

			info.EndWriting();
			dao.SetData(clipBoardFormat, stb.ToString());
			System.Windows.Forms.Clipboard.SetDataObject(dao, true);
		}


		public static object GetObjectFromClipboard(string clipBoardFormat)
		{
			var dao = System.Windows.Forms.Clipboard.GetDataObject();
			string s = (string)dao.GetData(clipBoardFormat);
			var info = new Altaxo.Serialization.Xml.XmlStreamDeserializationInfo();
			info.BeginReading(s);
			object o = info.GetValue("Object", null);
			info.EndReading();

			return o;
		}
	}
}