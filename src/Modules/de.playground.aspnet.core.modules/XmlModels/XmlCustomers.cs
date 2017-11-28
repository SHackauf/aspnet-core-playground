using System.Xml.Serialization;

namespace de.playground.aspnet.core.modules.XmlModels
{
    [XmlRoot(ElementName = "Customers")]
    public class XmlCustomers
    {
        [XmlElement("Customer")]
        public XmlCustomer[] Customers { get; set; }
    }
}
