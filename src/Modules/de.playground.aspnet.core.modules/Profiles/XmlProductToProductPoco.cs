using AutoMapper;

using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.modules.XmlModels;

namespace de.playground.aspnet.core.modules.Profiles
{
    public class XmlProductToProductPoco : Profile
    {
        public XmlProductToProductPoco()
        {
            this.CreateMap<XmlProduct, ProductPoco>()
                .BeforeMap((xml, poco, resolutionContext) =>
                {
                    if (!resolutionContext.Items.ContainsKey("CustomerId") || !(resolutionContext.Items["CustomerId"] is int customerId) || customerId < 1)
                    {
                        throw new AutoMapperMappingException("Item CustomerId is missing");
                    }
                })
                .ForMember(poco => poco.Name, opt => opt.MapFrom(xml => xml.Name))
                .ForMember(poco => poco.CustomerId, opt => opt.ResolveUsing((xml, poco, destMember, resolutionContext) => resolutionContext.Options.Items["CustomerId"]));
        }
    }
}
