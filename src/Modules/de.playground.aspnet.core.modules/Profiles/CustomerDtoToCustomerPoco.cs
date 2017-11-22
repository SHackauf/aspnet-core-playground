using AutoMapper;

using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.pocos;

namespace de.playground.aspnet.core.modules.Profiles
{
    public class CustomerDtoToCustomerPoco : Profile
    {
        public CustomerDtoToCustomerPoco()
        {
            this.CreateMap<CustomerDto, CustomerPoco>()
                .ForMember(poco => poco.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(poco => poco.Name, opt => opt.MapFrom(dto => dto.Name));
        }
    }
}
