using System;

namespace Altaxo.Serialization.Xml
{

	/// <summary>
	/// Used to point to the target type for which this class provides a serialization surrogate.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
	public class XmlSerializationSurrogateForAttribute : Attribute
	{
		protected	int m_Version;
		protected System.Type m_SerializationType;
		protected string m_AssemblyName;
		protected string m_TypeName;

		/// <summary>
		/// Constructor. The class this attribute is applied provides a serialization surrogate for the type <code>serializationtype</code>, version <code>version.</code>.
		/// </summary>
		/// <param name="serializationtype">The type this class provides a surrogate for.</param>
		/// <param name="version">The version of the class for which this surrogate is intended.</param>
		public XmlSerializationSurrogateForAttribute(Type serializationtype, int version )
		{
			m_Version = version;
			m_SerializationType = serializationtype; 
		}

		/// <summary>
		/// Constructor. Used when the target type is deprecated and no longer available. The class this attribute is applied for is then
		/// responsible for deserialization
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="typename"></param>
		/// <param name="version"></param>
		public XmlSerializationSurrogateForAttribute(string assembly, string typename, int version)
		{
			m_Version = version;
			m_AssemblyName = assembly;
			m_TypeName = typename;
		}
		/// <summary>
		/// returns the version of the class, for which the surrogate is intended
		/// </summary>
		public int Version
		{
			get { return m_Version; }
		}
		/// <summary>
		///Returns the target type for which the class this attribute is applied for is the serialization surrogate.
		/// </summary>
		public System.Type SerializationType
		{
			get { return m_SerializationType; }
		}

		/// <summary>
		/// Returns the assembly name (short form) of the target class type.
		/// </summary>
		public string AssemblyName
		{
			get 
			{
				if(null!=m_SerializationType)
					return (m_SerializationType.Assembly.FullName.Split(new char[]{','},2))[0];
				else
					return m_AssemblyName;
			}
		}

		/// <summary>
		/// Returns the name of the target type (the full name inclusive namespaces).
		/// </summary>
		public string TypeName
		{
			get
			{
				return null!=m_SerializationType ? m_SerializationType.ToString() : m_TypeName;
			}
		}


	} // end class SerializationSurrogateForAttribute


}