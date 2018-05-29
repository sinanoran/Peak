using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Peak.Common {
  public class XmlTool {

    #region ToObject

    #region ToObject from string source
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xmlData"></param>
    /// <returns></returns>
    public T ToObject<T>(string xmlSource) {
      return (T)ToObject(typeof(T), xmlSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeString"></param>
    /// <param name="xmlSource"></param>
    /// <returns></returns>
    public object ToObject(string typeString, string xmlSource) {
      Type type = Type.GetType(typeString);
      return ToObject(type, xmlSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typw"></param>
    /// <param name="xmlSource"></param>
    /// <returns></returns>
    public object ToObject(Type type, string xmlSource) {
      if (string.IsNullOrEmpty(xmlSource)) {
        return null;
      }

      return ToObject(type, new StringReader(xmlSource));
    }
    #endregion ToObject from string source

    #region ToObject from XmlNode
    /// <summary>
    /// XML'i XSD Schema'ya göre parametre olarak verilen tipte nesne örneğine çevirir.
    /// </summary>
    /// <typeparam name="T">Nesne Tipi</typeparam>
    /// <param name="node">XML Veri</param>
    /// <returns></returns>
    public T ToObject<T>(XmlNode node) {
      return (T)ToObject(typeof(T), node);
    }

    /// <summary>
    /// XML'i XSD Schema'ya göre parametre olarak verilen tipte nesne örneğine çevirir.
    /// </summary>
    /// <param name="typeString"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public object ToObject(string typeString, XmlNode node) {
      Type type = Type.GetType(typeString);
      return ToObject(type, node);
    }


    /// <summary>
    /// XML'i XSD Schema'ya göre parametre olarak verilen tipte nesne örneğine çevirir.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public object ToObject(Type type, XmlNode node) {
      if (node == null) {
        return null;
      }

      return ToObject(type, node.OuterXml);
    }
    #endregion ToObject from XmlNode

    #region ToObject from TextReader
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public T ToObject<T>(TextReader reader) {
      return (T)ToObject(typeof(T), reader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeString"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public object ToObject(string typeString, TextReader reader) {
      Type type = Type.GetType(typeString);
      return ToObject(type, reader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public object ToObject(Type type, TextReader reader) {
      XmlSerializer ser = new XmlSerializer(type);
      return ser.Deserialize(reader);
    }
    #endregion ToObject from TextReader

    #region ToObject from XmlReader
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public T ToObject<T>(XmlReader reader) {
      return (T)ToObject(typeof(T), reader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeString"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public object ToObject(string typeString, XmlReader reader) {
      Type type = Type.GetType(typeString);
      return ToObject(type, reader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public object ToObject(Type type, XmlReader reader) {
      XmlSerializer ser = new XmlSerializer(type);
      return ser.Deserialize(reader);
    }
    #endregion ToObject from XmlReader

    #region ToObject from stream
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <returns></returns>
    public T ToObject<T>(Stream stream) {
      return (T)ToObject(typeof(T), stream);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeString"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public object ToObject(string typeString, Stream stream) {
      Type type = Type.GetType(typeString);
      return ToObject(type, stream);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public object ToObject(Type type, Stream stream) {
      XmlSerializer ser = new XmlSerializer(type);
      return ser.Deserialize(stream);
    }
    #endregion ToObject from stream
    #endregion ToObject

    #region ToXml
    /// <summary>
    /// Verilen nesneyi ilgili XmlWriter'a serileştirir.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="writer"></param>
    public void WriteObject(object obj, XmlWriter writer, XmlSerializerNamespaces namespaces = null) {
      XmlSerializer ser = new XmlSerializer(obj.GetType());
      if (namespaces != null) {
        ser.Serialize(writer, obj, namespaces);
      }
      else {
        ser.Serialize(writer, obj);
      }
    }

    /// <summary>
    /// Verilen nesneyi xml olarak serileştirir.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string ToXml(object obj) {
      if (obj == null) {
        return null;
      }
      StringBuilder sb = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(sb)) {
        WriteObject(obj, writer);
      }
      return sb.ToString();
    }

    public string ToOnlyPlainXml(object obj) {
      if (obj == null) {
        return null;
      }
      var settings = new XmlWriterSettings();
      settings.OmitXmlDeclaration = true;
      StringBuilder sb = new StringBuilder();
      var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
      using (XmlWriter writer = XmlWriter.Create(sb, settings)) {
        WriteObject(obj, writer, emptyNamepsaces);
      }
      return sb.ToString();
    }

    #endregion ToXml

    #region DataContractSerialization
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string ToXmlAsDataContract(object obj) {
      if (obj == null) {
        return null;
      }
      Type type = obj.GetType();

      using (StringWriter writer = new StringWriter()) {
        using (XmlWriter xmlWriter = new XmlTextWriter(writer)) {
          DataContractSerializer ser = new DataContractSerializer(type);
          ser.WriteObject(xmlWriter, obj);
          return writer.ToString();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xmlSource"></param>
    /// <returns></returns>
    public T ToObjectAsDataContract<T>(string xmlSource) {
      return (T)this.ToObjectAsDataContract(typeof(T), xmlSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="xmlSource"></param>
    /// <returns></returns>
    public object ToObjectAsDataContract(Type type, string xmlSource) {
      using (XmlReader reader = XmlTextReader.Create(new StringReader(xmlSource))) {
        DataContractSerializer ser = new DataContractSerializer(type);
        return (object)ser.ReadObject(reader);
      }
    }
    #endregion  

    /// <summary>
    /// Verilen metin içindeki XML'e özel karakterleri &apos;, 
    /// &gt; gibi eşleniğine çevirir.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public string EscPeakML(string s) {
      if (string.IsNullOrEmpty(s))
        return s;
      return !SecurityElement.IsValidText(s) ? SecurityElement.Escape(s) : s;
    }

    /// <summary>
    /// Verilen metin içindeki &gt;, &apos; gibi karakterleri eşleniğine çevirir.
    /// Örnek: '&gt;' --> '>'
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public string UnescPeakML(string s) {
      if (string.IsNullOrEmpty(s))
        return s;
      string returnString = s;
      returnString = returnString.Replace("&apos;", "'");
      returnString = returnString.Replace("&quot;", "\"");
      returnString = returnString.Replace("&gt;", ">");
      returnString = returnString.Replace("&lt;", "<");
      returnString = returnString.Replace("&amp;", "&");
      return returnString;
    }
  }
}
