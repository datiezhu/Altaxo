﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altaxo.Main.Properties
{
	/// <summary>
	/// A property bag that loads its properties lazy. During deserialization the properties are temporarily stored as Xml documents.
	/// Only when trying to access a property, the Xml is then deserialized into the property.
	/// This behavior is neccessary for the UserSettings. Without lazy loading, a class that is deserialized could require another
	/// property, but the property service is not ready when the UserSettings are loaded.
	/// </summary>
	/// <seealso cref="Altaxo.Main.Properties.PropertyBag" />
	public class PropertyBagLazyLoaded : PropertyBag
	{
		/// <summary>
		/// The properties lazy loaded. Key is the property key. Value is a string, that is the Xml representation of the property value.
		/// </summary>
		private Dictionary<string, string> _propertiesLazyLoaded = new Dictionary<string, string>();

		/// <summary>
		/// Gets the assembly version this property bag was loaded from, i.e. the version of Altaxo that has serialized this bag before it was loaded again.
		/// </summary>
		/// <value>
		/// The assembly version of Altaxo this bag was loaded from, or null if this is the default bag.
		/// </value>
		public Version AssemblyVersionLoadedFrom { get; private set; }

		#region Serialization

		/// <summary>
		/// 2017-02-23 Initial version
		/// </summary>
		[Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(PropertyBagLazyLoaded), 0)]
		private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
		{
			public void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
			{
				var s = (PropertyBagLazyLoaded)obj;

				var assemblyVersion = s.GetType().Assembly.GetName().Version;
				var keyList = new HashSet<string>();
				foreach (var entry in s._properties)
				{
					if (entry.Key.StartsWith(TemporaryPropertyPrefixString))
						continue;
					if (!info.IsSerializable(entry.Value))
						continue;
					keyList.Add(entry.Key);
				}
				foreach (var entry in s._propertiesLazyLoaded)
				{
					if (entry.Key.StartsWith(TemporaryPropertyPrefixString))
						continue;
					keyList.Add(entry.Key);
				}

				info.CreateArray("Properties", keyList.Count);
				info.AddAttributeValue("AssemblyVersion", assemblyVersion.ToString());
				foreach (var key in keyList)
				{
					info.CreateElement("e");
					info.AddValue("Key", key);

					if (s._properties.TryGetValue(key, out var value))
						info.AddValue("Value", value);
					else if (s._propertiesLazyLoaded.TryGetValue(key, out var rawXml))
						info.WriteRaw(rawXml);
					else
						throw new InvalidOperationException("Key neither found in regular properties nor in lazy loaded properties");

					info.CommitElement();
				}
				info.CommitArray();
			}

			public void Deserialize(PropertyBagLazyLoaded s, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				var assemblyVersionString = info.GetStringAttribute("AssemblyVersion");
				s.AssemblyVersionLoadedFrom = Version.Parse(assemblyVersionString);

				int count = info.OpenArray("Properties");

				for (int i = 0; i < count; i++)
				{
					info.OpenElement(); // "e"
					string propkey = info.GetString("Key");
					string valueAsXmlString = info.GetElementAsOuterXml("Value");
					info.CloseElement(); // "e"
					s._propertiesLazyLoaded[propkey] = valueAsXmlString;
				}
				info.CloseArray(count);
			}

			public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
			{
				var s = o as PropertyBagLazyLoaded ?? new PropertyBagLazyLoaded();
				Deserialize(s, info, parent);
				return s;
			}
		}

		#endregion Serialization

		#region Conversion from Lazy

		/// <summary>
		/// Converts a property from the lazy representation as Xml string into the real value. Then the lazy property entry
		/// is removed, and the property is stored in the regular property dictionary.
		/// </summary>
		/// <param name="propName">Name of the property.</param>
		/// <param name="xml">The XML.</param>
		public void ConvertFromLazy(string propName, string xml)
		{
			using (var info = new Altaxo.Serialization.Xml.XmlStreamDeserializationInfo())
			{
				info.BeginReading(xml);
				object propval = info.GetValue("Value", this);

				if (propval is IDocumentLeafNode documentLeafNode)
					documentLeafNode.ParentObject = this;

				_propertiesLazyLoaded.Remove(propName);
				_properties[propName] = propval;
			}
		}

		/// <summary>
		/// Converts all properties from lazy properties to regular properties.
		/// </summary>
		public void ConvertAllFromLazy()
		{
			var keys = _propertiesLazyLoaded.Keys.ToArray();

			foreach (var key in keys)
			{
				if (_propertiesLazyLoaded.TryGetValue(key, out var xml)) // Try neccessary here because during conversion other properties may get converted
					ConvertFromLazy(key, xml);
			}
		}

		#endregion Conversion from Lazy

		#region Overrides Count and Clear

		public override void Clear()
		{
			_propertiesLazyLoaded.Clear();
			base.Clear();
		}

		public override int Count
		{
			get
			{
				return _properties.Count + base.Count;
			}
		}

		#endregion Overrides Count and Clear

		#region GetValue overrides

		/// <summary>
		/// Gets the value of a property.
		/// </summary>
		/// <typeparam name="T">The of the property.</typeparam>
		/// <param name="p">The property key.</param>
		/// <returns>
		/// The property.
		/// </returns>
		public override T GetValue<T>(PropertyKey<T> p)
		{
			if (_propertiesLazyLoaded.TryGetValue(p.PropertyName, out var propValueAsString))
			{
				ConvertFromLazy(p.GuidString, propValueAsString);
			}
			return base.GetValue<T>(p);
		}

		public override T GetValue<T>(PropertyKey<T> p, T defaultValue)
		{
			if (_propertiesLazyLoaded.TryGetValue(p.PropertyName, out var propValueAsString))
			{
				ConvertFromLazy(p.GuidString, propValueAsString);
			}
			return base.GetValue<T>(p, defaultValue);
		}

		/// <summary>
		/// Tries to get the value of a property.
		/// </summary>
		/// <typeparam name="T">Type of the property.</typeparam>
		/// <param name="p">The property key.</param>
		/// <param name="value">If successfull, on return this value contains the property value.</param>
		/// <returns>
		///   <c>True</c> if the property could be successfully retrieved, otherwise <c>false</c>.
		/// </returns>
		public override bool TryGetValue<T>(PropertyKey<T> p, out T value)
		{
			if (_propertiesLazyLoaded.TryGetValue(p.GuidString, out var propValueAsString))
			{
				ConvertFromLazy(p.GuidString, propValueAsString);
			}
			return base.TryGetValue<T>(p, out value);
		}

		/// <summary>
		/// Gets the value of a property.
		/// </summary>
		/// <typeparam name="T">Type of the property value.</typeparam>
		/// <param name="propName">The property name.</param>
		/// <returns>
		/// The property.
		/// </returns>
		public override T GetValue<T>(string propName)
		{
			if (_propertiesLazyLoaded.TryGetValue(propName, out var propValueAsString))
			{
				ConvertFromLazy(propName, propValueAsString);
			}
			return base.GetValue<T>(propName);
		}

		/// <summary>
		/// Tries to get the value of a property.
		/// </summary>
		/// <typeparam name="T">Type of the property value.</typeparam>
		/// <param name="propName">The property name.</param>
		/// <param name="value">If successfull, on return this value contains the property value.</param>
		/// <returns>
		///   <c>True</c> if the property could be successfully retrieved, otherwise <c>false</c>.
		/// </returns>
		public override bool TryGetValue<T>(string propName, out T value)
		{
			if (_propertiesLazyLoaded.TryGetValue(propName, out var propValueAsString))
			{
				ConvertFromLazy(propName, propValueAsString);
			}
			return base.TryGetValue<T>(propName, out value);
		}

		#endregion GetValue overrides

		#region SetValue overrides

		/// <summary>
		/// Sets the value of a property.
		/// </summary>
		/// <typeparam name="T">Type of the property.</typeparam>
		/// <param name="p">The property key.</param>
		/// <param name="value">The value of the property.</param>
		/// <exception cref="System.ArgumentException">Thrown if the type of the provided value is not compatible with the registered property.</exception>
		public override void SetValue<T>(PropertyKey<T> p, T value)
		{
			_propertiesLazyLoaded.Remove(p.GuidString);
			base.SetValue<T>(p, value);
		}

		/// <summary>
		/// Sets the value of a property.
		/// </summary>
		/// <typeparam name="T">Type of the property value.</typeparam>
		/// <param name="propName">The property name.</param>
		/// <param name="value">The value of the property.</param>
		public override void SetValue<T>(string propName, T value)
		{
			_propertiesLazyLoaded.Remove(propName);
			base.SetValue<T>(propName, value);
		}

		#endregion SetValue overrides

		#region Remove overrides

		/// <summary>
		/// Removes a property from this instance.
		/// </summary>
		/// <typeparam name="T">Type of the property value.</typeparam>
		/// <param name="p">The property key.</param>
		/// <returns><c>True</c> if the property has been successful removed, <c>false</c> if the property has not been found in this collection.</returns>
		public override bool RemoveValue<T>(PropertyKey<T> p)
		{
			var result1 = _propertiesLazyLoaded.Remove(p.GuidString);
			var result2 = base.RemoveValue<T>(p);
			return result1 | result2;
		}

		/// <summary>
		/// Removes a property from this instance.
		/// </summary>
		/// <param name="propName">The property name.</param>
		/// <returns>
		///   <c>True</c> if the property has been successful removed, <c>false</c> if the property has not been found in this collection.
		/// </returns>
		public override bool RemoveValue(string propName)
		{
			var result1 = _properties.Remove(propName);
			var result2 = base.RemoveValue(propName);
			return result1 | result2;
		}

		#endregion Remove overrides

		#region Enumerator override

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			ConvertAllFromLazy();
			return base.GetEnumerator();
		}

		#endregion Enumerator override
	}
}