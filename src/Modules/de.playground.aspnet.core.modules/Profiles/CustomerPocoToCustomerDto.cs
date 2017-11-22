using AutoMapper;

using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.pocos;

namespace de.playground.aspnet.core.modules.Profiles
{
    public class CustomerPocoToCustomerDto : Profile
    {
        public CustomerPocoToCustomerDto()
        {
            this.CreateMap<CustomerPoco, CustomerDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(poco => poco.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(poco => poco.Name));
        }
    }
}
