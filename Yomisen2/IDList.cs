using System.Collections.Generic;
using System.Xml.Serialization;

namespace Yomisen2
{
    [XmlRoot(ElementName = "IDList")]
    public class IDList
    {
        [XmlElement(ElementName = "ChannelIDList")]
        public List<IDInfo> ChannelIDList { get; set; }

        [XmlElement(ElementName = "UserIDList")]
        public List<IDInfo> UserIDList { get; set; }
    }
}
