﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2012 Dr. Dirk Lellinger
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

using Altaxo.Drawing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Altaxo.Graph.Gdi
{
	/// <summary>
	/// Manages <see cref="System.Drawing.Font"/> instances and corresponds them with the Altaxo fonts  (<see cref="FontX"/>).
	/// </summary>
	public class GdiFontManager
	{
		/// <summary>The instance used by the static methods of this class. Is not neccessarily of type <see cref="GdiFontManager"/>, but could also be a derived type.</summary>
		protected static GdiFontManager _instance;

		/// <summary>
		/// Converter. Converts invariant font strings to Gdi fonts and vice versa.
		/// </summary>
		protected FontConverter _gdiFontConverter = new FontConverter();

		/// <summary>Corresponds the font's invariant description string with the Gdi+ font instance.
		/// Key is the invariant description string, value is the Gdi font instance with the specific style and size.
		/// </summary>
		protected ConcurrentDictionary<string, Font> _descriptionToGdiFont = new ConcurrentDictionary<string, Font>();

		/// <summary>Corresponds the font's invariant description string with a reference counter. It counts the number of <see cref="FontX"/> instances with this description string.
		/// When the reference counter falls down to zero, the Gdi+ font instance can be released.</summary>
		protected ConcurrentDictionary<string, int> _gdiFontReferenceCounter = new ConcurrentDictionary<string, int>();

		/// <summary>
		/// Dictionary of the Gdi font families. Key is the Win32FamilyName, value is an indicator which styles of that font are present.
		/// </summary>
		protected ConcurrentDictionary<string, FontStylePresence> _gdiFontFamilies;

		/// <summary>
		/// Registers this instance with the <see cref="FontX"/> font system.
		/// </summary>
		public static void Register()
		{
			// when called, the instance of this class is set to a new instance of this class,
			// which then registers this FontManager with FontX

			if (null == _instance)
				SetInstance(new GdiFontManager());
		}

		/// <summary>
		/// Sets the instance of <see cref="GdiFontManager"/> here in this class (used by the static methods of this class).
		/// </summary>
		/// <param name="newInstance">The new instance.</param>
		public static void SetInstance(GdiFontManager newInstance)
		{
			var oldInstance = _instance;

			if (!object.ReferenceEquals(oldInstance, newInstance))
			{
				_instance = newInstance;

				oldInstance?.Dispose();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GdiFontManager" /> class.
		/// </summary>
		protected GdiFontManager()
		{
			FontX.FontConstructed += EhAnnounceConstructionOfFontX;
			FontX.FontDestructed += EhAnnounceDestructionOfFontX;

			Microsoft.Win32.SystemEvents.InstalledFontsChanged += EhInstalledFontsChanged;

			InternalBuildDictionaries();
		}

		protected virtual void Dispose()
		{
			FontX.FontConstructed -= EhAnnounceConstructionOfFontX;
			FontX.FontDestructed -= EhAnnounceDestructionOfFontX;

			Microsoft.Win32.SystemEvents.InstalledFontsChanged -= EhInstalledFontsChanged;
		}

		/// <summary>
		/// Build the font dictionaries.
		/// </summary>
		protected virtual void InternalBuildDictionaries()
		{
			_gdiFontFamilies = BuildGdiFontFamilies();
		}

		/// <summary>
		/// Builds the GDI font families dictionary.
		/// </summary>
		/// <returns>The Gdi font family dictionary.</returns>
		protected virtual ConcurrentDictionary<string, FontStylePresence> BuildGdiFontFamilies()
		{
			var dict = new ConcurrentDictionary<string, FontStylePresence>();
			foreach (var fontFamily in System.Drawing.FontFamily.Families)
			{
				FontStylePresence pres = GetFontStylePresence(fontFamily);

				if (FontStylePresence.NoStyleAvailable != pres)
					dict.TryAdd(fontFamily.Name, pres);
			}
			return dict;
		}

		/// <summary>
		/// Finds out which font styles are available for the given Gdi font family.
		/// </summary>
		/// <param name="fontFamily">The Gdi font family.</param>
		/// <returns>Value that indicate which styles are available for the fontFamily.</returns>
		protected static FontStylePresence GetFontStylePresence(FontFamily fontFamily)
		{
			FontStylePresence pres = FontStylePresence.NoStyleAvailable;
			if (fontFamily.IsStyleAvailable(FontStyle.Regular))
				pres |= FontStylePresence.RegularStyleAvailable;
			if (fontFamily.IsStyleAvailable(FontStyle.Bold))
				pres |= FontStylePresence.BoldStyleAvailable;
			if (fontFamily.IsStyleAvailable(FontStyle.Italic))
				pres |= FontStylePresence.ItalicStyleAvailable;
			if (fontFamily.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic))
				pres |= FontStylePresence.BoldAndItalicStyleAvailable;
			return pres;
		}

		/// <summary>
		/// Gets all available font families and stores them in the provided dictionary.
		/// </summary>
		/// <param name="dictionaryToStoreTheResult">The dictionary to store the result.</param>
		protected virtual void InternalGetAvailableFontFamilies(IDictionary<string, FontStylePresence> dictionaryToStoreTheResult)
		{
			foreach (var entry in _gdiFontFamilies)
				dictionaryToStoreTheResult.Add(entry.Key, entry.Value);
		}

		/// <summary>
		/// Gets all available font families and stores them in the provided dictionary.
		/// </summary>
		/// <param name="dictionaryToStoreTheResult">The dictionary to store the result.</param>
		public static void GetAvailableFontFamilies(IDictionary<string, FontStylePresence> dictionaryToStoreTheResult)
		{
			if (null == dictionaryToStoreTheResult)
				throw new ArgumentNullException("Argument dictionaryToStoreTheResult is null");
			if (dictionaryToStoreTheResult.Count != 0)
				throw new ArgumentException("The provided dictionary is not empty");

			_instance.InternalGetAvailableFontFamilies(dictionaryToStoreTheResult);
		}

		/// <summary>
		/// Determines whether a font family with the provided name is available.
		/// </summary>
		/// <param name="fontFamilyName">Name of the font family to test.</param>
		/// <returns>
		///   <c>true</c> if a font family with the given font family name is available; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool InternalIsFontFamilyAvailable(string fontFamilyName)
		{
			return _gdiFontFamilies.ContainsKey(fontFamilyName);
		}

		/// <summary>
		/// Determines whether a font family with the provided name is available.
		/// </summary>
		/// <param name="fontFamilyName">Name of the font family to test.</param>
		/// <returns>
		///   <c>true</c> if a font family with the given font family name is available; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFontFamilyAvailable(string fontFamilyName)
		{
			return _instance.InternalIsFontFamilyAvailable(fontFamilyName);
		}

		/// <summary>
		/// Determines whether a font with the given font family name and font style is available.
		/// </summary>
		/// <param name="fontFamilyName">Name of the font family.</param>
		/// <param name="style">The font style to test for (underline and strikeout are always available, thus they are not tested).</param>
		/// <returns>
		///   <c>true</c> if a font with the provided family name and style is available; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool InternalIsFontFamilyAndStyleAvailable(string fontFamilyName, FontXStyle style)
		{
			FontStylePresence pres;
			return _gdiFontFamilies.TryGetValue(fontFamilyName, out pres) && pres.HasFlag(ConvertFontXStyleToFontStylePresence(style));
		}

		/// <summary>
		/// Determines whether a font with the given font family name and font style is available.
		/// </summary>
		/// <param name="fontFamilyName">Name of the font family.</param>
		/// <param name="style">The font style to test for (underline and strikeout are always available, thus they are not tested).</param>
		/// <returns>
		///   <c>true</c> if a font with the provided family name and style is available; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFontFamilyAndStyleAvailable(string fontFamilyName, FontXStyle style)
		{
			return _instance.InternalIsFontFamilyAndStyleAvailable(fontFamilyName, style);
		}

		/// <summary>
		/// Converts a <see cref="FontXStyle"/> instance to a <see cref="FontStylePresence"/> instance.
		/// The styles 'Underline' and 'Strikeout' will not be considered for the conversion.
		/// </summary>
		/// <param name="style">The style to convert..</param>
		/// <returns>
		/// The converted <see cref="FontStylePresence"/> instance.
		/// </returns>
		private static FontStylePresence ConvertFontXStyleToFontStylePresence(FontXStyle style)
		{
			FontStylePresence result;
			if (style.HasFlag(FontXStyle.Bold) && style.HasFlag(FontXStyle.Italic))
				result = FontStylePresence.BoldAndItalicStyleAvailable;
			else if (style.HasFlag(FontXStyle.Bold))
				result = FontStylePresence.BoldStyleAvailable;
			else if (style.HasFlag(FontXStyle.Italic))
				result = FontStylePresence.ItalicStyleAvailable;
			else
				result = FontStylePresence.RegularStyleAvailable;

			return result;
		}

		/// <summary>
		/// Constructs a font from a font family, size and style.
		/// </summary>
		/// <param name="fontFamily">The font family.</param>
		/// <param name="fontSize">Size of the font.</param>
		/// <param name="fontStyle">The font style.</param>
		/// <returns>A <see cref="FontX"/> instance describing the font. It can then be used with the FontManager to get a Gdi+ font instance.</returns>
		public static FontX GetFont(FontFamily fontFamily, double fontSize, FontStyle fontStyle)
		{
			return FontX.InternalCreateFromNameSizeStyle(fontFamily.Name, fontSize, (FontXStyle)fontStyle);
		}

		/// <summary>
		/// Constructs a font from a font family name, the size and font style.
		/// </summary>
		/// <param name="fontFamilyName">Name of the font family.</param>
		/// <param name="fontSize">Size of the font.</param>
		/// <param name="fontStyle">The font style.</param>
		/// <returns>A <see cref="FontX"/> instance describing the font. It can then be used with the FontManager to get a Gdi+ font instance.</returns>
		public static FontX GetFont(string fontFamilyName, double fontSize, FontXStyle fontStyle)
		{
			return FontX.InternalCreateFromNameSizeStyle(fontFamilyName, fontSize, fontStyle);
		}

		/// <summary>
		/// Gets a GDI font from the invariant string using the fontConverter.
		/// </summary>
		/// <param name="invariantDescriptionString">The invariant description string.</param>
		/// <returns>Gdi font.</returns>
		protected virtual Font InternalGetGdiFontFromInvariantString(string invariantDescriptionString)
		{
			return (Font)_gdiFontConverter.ConvertFromInvariantString(invariantDescriptionString);
		}

		/// <summary>
		/// Retrieves the Gdi+ font instance that the provided <see cref="FontX"/> argument is describing.
		/// </summary>
		/// <param name="fontX">The fontX instance.</param>
		/// <returns>The Gdi+ font instance that corresponds with the argument. If the font family of the fontX argument is not found, a default font (Microsoft Sans Serif) is returned.</returns>
		protected virtual Font InternalToGdi(FontX fontX)
		{
			string fontID = fontX.InvariantDescriptionString;
			Font result;
			if (!_descriptionToGdiFont.TryGetValue(fontID, out result))
			{
				result = _descriptionToGdiFont.AddOrUpdate(fontID,
					x => InternalGetGdiFontFromInvariantString(x),
					(x, y) => y);
			}

			return result;
		}

		/// <summary>
		/// Retrieves the Gdi+ font instance that the provided <see cref="FontX"/> argument is describing.
		/// </summary>
		/// <param name="fontX">The fontX instance.</param>
		/// <returns>The Gdi+ font instance that corresponds with the argument. If the font family of the fontX argument is not found, a default font (Microsoft Sans Serif) is returned.</returns>
		public static Font ToGdi(FontX fontX)
		{
			return _instance.InternalToGdi(fontX);
		}

		/// <summary>Gets the height of the font in points (1/72 inch).</summary>
		/// <param name="fontX">The font instance.</param>
		public static double Height(FontX fontX)
		{
			return ToGdi(fontX).GetHeight(72);
		}

		/// <summary>
		/// Gets the font family of the provided <see cref="FontX"/> instance. Be aware that the returned font family can differ from the font family which is coded within the <see cref="FontX"/> invariant description string.
		/// </summary>
		/// <param name="fontX">The provided FontX instance.</param>
		/// <returns>The Gdi font family of the Gdi font which is associated with the provided FontX instance.</returns>
		public static FontFamily GdiFontFamily(FontX fontX)
		{
			return ToGdi(fontX).FontFamily;
		}

		/// <summary>
		/// Is called upon every construction of a <see cref="FontX"/> instance.
		/// </summary>
		/// <param name="fontID">The invariant description string of the constructed <see cref="FontX"/> instance.</param>
		protected virtual void EhAnnounceConstructionOfFontX(string fontID)
		{
			_gdiFontReferenceCounter.AddOrUpdate(fontID, 1, (x, y) => y + 1);
		}

		/// <summary>
		/// Is called upon every destruction of a <see cref="FontX"/> instance.
		/// </summary>
		/// <param name="fontID">The invariant description string of the destructed <see cref="FontX"/> instance.</param>
		protected virtual void EhAnnounceDestructionOfFontX(string fontID)
		{
			int refCount = _gdiFontReferenceCounter.AddOrUpdate(fontID, 0, (x, y) => Math.Max(0, y - 1));
			if (0 == refCount)
			{
				_gdiFontReferenceCounter.TryRemove(fontID, out refCount);
				Font gdiFont;
				_descriptionToGdiFont.TryRemove(fontID, out gdiFont);
			}
		}

		/// <summary>
		/// Called when the installed fonts changed during execution of this program.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void EhInstalledFontsChanged(object sender, EventArgs e)
		{
			_gdiFontFamilies = BuildGdiFontFamilies();
		}
	}
}