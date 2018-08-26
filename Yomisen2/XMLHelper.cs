using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Yomisen2
{
    public static class XMLHelper
    {
        // XML書き込み
        public static void Serialize<T>(string savePath, T deserializedObect)
        {
            using (var sw = new StreamWriter(savePath, false, Encoding.UTF8))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                new XmlSerializer(typeof(T)).Serialize(sw, deserializedObect, ns);
            }
        }

        // XML読み込み
        public static T Deserialize<T>(string loadPath)
        {
            try
            {
                using (var sr = new StreamReader(loadPath))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(sr);
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
