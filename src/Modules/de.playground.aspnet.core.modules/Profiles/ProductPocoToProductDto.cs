using AutoMapper;

using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.pocos;

namespace de.playground.aspnet.core.modules.Profiles
{
    public class ProductPocoToProductDto : Profile
    {
        public ProductPocoToProductDto()
        {
            this.CreateMap<ProductPoco, ProductDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(poco => poco.Id))
                .ForMember(dto => dto.CustomerId, opt => opt.MapFrom(poco => poco.CustomerId))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(poco => poco.Name));
        }
    }
}
