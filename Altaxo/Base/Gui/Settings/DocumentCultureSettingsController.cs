﻿#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2011 Dr. Dirk Lellinger
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Altaxo.Collections;
using Altaxo.Settings;

namespace Altaxo.Gui.Settings
{
	/// <summary>
	/// Interface that the Gui component has to implement in order to be a view for <see cref="DocumentCultureSettingsController"/>.
	/// </summary>
	public interface IDocumentCultureSettingsView
	{
		/// <summary>Gets or sets a value indicating whether to use the operating system settings for the UI culture or use another one.</summary>
		/// <value>If <see langword="true"/>, own settings are used; otherwise the operating system settings are used for the current UI culture.</value>
		bool OverrideOperatingSystemSettings { get; set; }

		/// <summary>Initializes the culture format list.</summary>
		/// <param name="list">List containing all selectable cultures.</param>
		void InitializeCultureFormatList(SelectableListNodeList list);

		/// <summary>Occurs when the culture name changed.</summary>
		event Action CultureChanged;

		/// <summary>Occurs when the user chooses to change the state of the override system culture UI element (probably a checkbox or two radio buttons).</summary>
		event Action OverrideSystemCultureChanged;

		/// <summary>Gets or sets the number decimal separator.</summary>
		/// <value>The number decimal separator.</value>
		string NumberDecimalSeparator { get; set; }

		/// <summary>Gets or sets the number group separator.</summary>
		/// <value>The number group separator.</value>
		string NumberGroupSeparator { get; set; }

	}

	/// <summary>Manages the user interaction to set the members of <see cref="DocumentCultureSettings"/>.</summary>
	[ExpectedTypeOfView(typeof(IDocumentCultureSettingsView))]
	[UserControllerForObject(typeof(DocumentCultureSettings))]
	public class DocumentCultureSettingsController : IMVCANController
	{
		IDocumentCultureSettingsView _view;
		DocumentCultureSettings _originalDoc;

		/// <summary>Holds temporary the settings.</summary>
		DocumentCultureSettings _doc;

		/// <summary>Represents the document with the operation system settings.</summary>
		DocumentCultureSettings _sysSettingsDoc;

		/// <summary>If true, indicates that the document was created by this controller and should be saved to Altaxo settings when <see cref="Apply"/> is called.</summary>
		bool _isHoldingOwnDocument;

		/// <summary>List of available cultures.</summary>
		SelectableListNodeList _availableCulturesList;

		/// <summary>List with only a single entry: the operating system UI culture;</summary>
		SelectableListNodeList _sysSettingsCultureList;


		/// <summary>Initialize the controller with the document. If successfull, the function has to return true.</summary>
		/// <param name="args">The arguments neccessary to create the controller. Normally, the first argument is the document, the second can be the parent of the document and so on.</param>
		/// <returns>Returns <see langword="true"/> if successfull; otherwise <see langword="false"/>.</returns>
		public bool InitializeDocument(params object[] args)
		{
			if (null == args || args.Length == 0 || (null!=args[0] && !(args[0] is AutoUpdateSettings)))
				return false;

			_originalDoc = args[0] as DocumentCultureSettings;

			if (null == _originalDoc)
			{
				_isHoldingOwnDocument = true;
				_originalDoc = Current.PropertyService.Get(DocumentCultureSettings.SettingsStoragePath, DocumentCultureSettings.FromDefault());
			}

			_doc = (DocumentCultureSettings)_originalDoc.Clone();
			_sysSettingsDoc = DocumentCultureSettings.FromDefault();

			Initialize(true);
			
			return true;
		}

		void Initialize(bool initData)
		{
			if (initData)
			{
				_availableCulturesList = new SelectableListNodeList();
				var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
				Array.Sort(cultures, CompareCultures);
				AddToCultureList(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture.ThreeLetterISOLanguageName == _doc.ToCulture().ThreeLetterISOLanguageName);
				foreach (var cult in cultures)
					AddToCultureList(cult, cult.Name == _doc.CultureName);
				if (null == _availableCulturesList.FirstSelectedNode)
					_availableCulturesList[0].IsSelected = true;

				var defCult = _sysSettingsDoc.ToCulture();
				_sysSettingsCultureList = new SelectableListNodeList();
				_sysSettingsCultureList.Add(new SelectableListNode(defCult.DisplayName,defCult,true));

			}

			if (null != _view)
			{
				_view.OverrideOperatingSystemSettings = _doc.OverrideParentCulture;
				_view.InitializeCultureFormatList(_doc.OverrideParentCulture ? _availableCulturesList : _sysSettingsCultureList);

				_view.NumberDecimalSeparator = _doc.NumberDecimalSeparator;
				_view.NumberGroupSeparator = _doc.NumberGroupSeparator;
			}
		}

		void AddToCultureList(CultureInfo cult, bool isSelected)
		{
			_availableCulturesList.Add(new SelectableListNode(cult.DisplayName, cult, cult.Name == _doc.CultureName));
		}

		private int CompareCultures(CultureInfo x, CultureInfo y)
		{
			return string.Compare(x.DisplayName, y.DisplayName);
		}

		public UseDocument UseDocumentCopy
		{
			set {  }
		}

		/// <summary>Returns the Gui element that shows the model to the user.</summary>
		public object ViewObject
		{
			get
			{
				return _view;
			}
			set
			{
				if (null != _view)
				{
					_view.CultureChanged -= EhCultureChanged;
					_view.OverrideSystemCultureChanged -= EhOverrideSystemCultureChanged;
				}

				_view = value as IDocumentCultureSettingsView;

				if (null != _view)
				{
					Initialize(false);
					_view.CultureChanged += EhCultureChanged;
					_view.OverrideSystemCultureChanged += EhOverrideSystemCultureChanged;
				}
			}
		}

		void EhOverrideSystemCultureChanged()
		{
			bool overrideSysSettings = _view.OverrideOperatingSystemSettings;
			_view.InitializeCultureFormatList(overrideSysSettings ? _availableCulturesList : _sysSettingsCultureList);
			SetElementsAfterCultureChanged(overrideSysSettings ? _doc : _sysSettingsDoc);
		}

		void EhCultureChanged()
		{
			var node = _availableCulturesList.FirstSelectedNode;
			if (node != null)
			{
				CultureInfo c = (CultureInfo)node.Tag;
				_doc.SetMembersFromCulture(c);
				SetElementsAfterCultureChanged(_doc);
			}
		}

		void SetElementsAfterCultureChanged(DocumentCultureSettings s)
		{
			_view.NumberDecimalSeparator = s.NumberDecimalSeparator;
			_view.NumberGroupSeparator = s.NumberGroupSeparator;
		}

		public object ModelObject
		{
			get { return _originalDoc; }
		}

		public bool Apply()
		{
			if (_view.OverrideOperatingSystemSettings)
			{
				_doc.OverrideParentCulture = true;
				_doc.NumberDecimalSeparator = _view.NumberDecimalSeparator;
				_doc.NumberGroupSeparator = _view.NumberGroupSeparator;
				_originalDoc.CopyFrom(_doc);
			}
			else
			{
				_originalDoc.CopyFrom(_sysSettingsDoc);
			}

			if (_isHoldingOwnDocument)
			{

				// then we set our own culture settings
				Current.PropertyService.Set(DocumentCultureSettings.SettingsStoragePath, _originalDoc);
				System.Threading.Thread.CurrentThread.CurrentCulture = _originalDoc.ToCulture();
			}

			return true;
		}
	}
}
