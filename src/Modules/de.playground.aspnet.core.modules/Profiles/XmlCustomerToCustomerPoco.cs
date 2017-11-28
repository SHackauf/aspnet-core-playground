using AutoMapper;

using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.modules.XmlModels;

namespace de.playground.aspnet.core.modules.Profiles
{
    public class XmlCustomerToCustomerPoco : Profile
    {
        public XmlCustomerToCustomerPoco()
        {
            this.CreateMap<XmlCustomer, CustomerPoco>()
                .ForMember(poco => poco.Name, opt => opt.MapFrom(xml => xml.Name));
        }
    }
}
