using AutoMapper;
using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.mvc.Models;

namespace de.playground.aspnet.core.mvc.Profiles
{
    public class CustomerModelToCustomerDto : Profile
    {
        public CustomerModelToCustomerDto()
        {
            CreateMap<CustomerModel, CustomerDto>();
        }
    }
}
