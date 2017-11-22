using AutoMapper;

using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.pocos;

namespace de.playground.aspnet.core.modules.Profiles
{
    public class ProductDtoToProductPoco : Profile
    {
        public ProductDtoToProductPoco()
        {
            this.CreateMap<ProductDto, ProductPoco>()
                .ForMember(poco => poco.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(poco => poco.CustomerId, opt => opt.MapFrom(dto => dto.CustomerId))
                .ForMember(poco => poco.Name, opt => opt.MapFrom(dto => dto.Name));
        }
    }
}
