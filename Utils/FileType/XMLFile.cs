using System.IO;
using System.Xml.Linq;
using Utils.Util;

namespace Utils.FileType
{
    public class XMLFile<T> where T : class, new()
    {
        private string FilePath { get; set; }

        public XMLFile()
        {
        }

        public XMLFile(string filePath)
        {
            this.FilePath = filePath;
            if (!File.Exists(FilePath))
            {
                write(new T());
            }
        }

        public T read() 
        {
            Serializer ser = new Serializer();
            string xmlInputData = File.ReadAllText(this.FilePath);
            T obj = ser.Deserialize<T>(xmlInputData);
            return obj;
        }

        public void write(T obj)
        {
            Serializer ser = new Serializer();
            string xmlOutput = ser.Serialize<T>(obj);
            XElement xElement = XElement.Parse(xmlOutput);
            xElement.Save(this.FilePath);
        }
    }
}