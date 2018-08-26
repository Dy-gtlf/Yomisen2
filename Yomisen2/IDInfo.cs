using System.Xml.Serialization;

namespace Yomisen2
{
    public class IDInfo
    {
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }
    }
}
