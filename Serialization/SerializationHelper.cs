using System.IO;
using System.Text;
using System.Xml.Serialization;
using System;

namespace UmbCodeGen.Serialization
{
    public static class SerializationHelper
    {
        public static T Deserialize<T>(string serial)
        {
            return Deserialize<T>(serial, null);
        }

        public static T Deserialize<T>(string serial, Type[] extraTypes)
        {
            T result;
            var xs = new XmlSerializer(typeof(T), extraTypes);
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(serial)))
            {
                result = (T)xs.Deserialize(stream);
            }
            return result;
        }

        public static string Serialize<T>(T data)
        {
            return Serialize<T>(data, null);
        }

        public static string Serialize<T>(T data, Type[] extraTypes)
        {
            string serial;
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var xs = new XmlSerializer(typeof(T), extraTypes);
            using (Stream stream = new MemoryStream())
            {
                xs.Serialize(stream, data, namespaces);
                stream.Seek(0, SeekOrigin.Begin);
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                serial = Encoding.UTF8.GetString(buffer);
            }

            return serial;
        }
    }
}