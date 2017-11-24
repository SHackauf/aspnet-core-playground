using AutoMapper;

using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.dtos;

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
