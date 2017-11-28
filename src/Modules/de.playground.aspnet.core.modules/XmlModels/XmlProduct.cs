using System.Xml.Serialization;

namespace de.playground.aspnet.core.modules.XmlModels
{
    public class XmlProduct
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}
