using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BOMBS.Core.Serialization
{
    public class XMLHelper
    {
        public static string SerializeToXMLString<T>(T dataToSerialize)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, dataToSerialize);
            return writer.ToString();
        }

        public static T DeserializeFromXMLString<T>(string stringToDeserialize)
        {
            StringReader reader = new StringReader(stringToDeserialize);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }
    }
}
