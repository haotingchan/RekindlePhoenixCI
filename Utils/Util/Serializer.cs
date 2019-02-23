using System.IO;
using System.Xml.Serialization;

namespace Utils.Util
{
    public class Serializer
    {
        public T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer ser = new XmlSerializer(ObjectToSerialize.GetType());
            using (StringWriter sw = new StringWriter())
            {
                ser.Serialize(sw, ObjectToSerialize);
                return sw.ToString();
            }
        }
    }
}