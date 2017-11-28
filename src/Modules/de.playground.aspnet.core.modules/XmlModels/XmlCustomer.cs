using System.Xml.Serialization;

namespace de.playground.aspnet.core.modules.XmlModels
{
    [XmlRoot(ElementName = "Customer")]
    public class XmlCustomer
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlArray("Products")]
        [XmlArrayItem("Product")]
        public XmlProduct[] Products { get; set; }
    }
}
