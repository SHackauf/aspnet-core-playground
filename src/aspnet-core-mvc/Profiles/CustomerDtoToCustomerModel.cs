using AutoMapper;
using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.mvc.Models;

namespace de.playground.aspnet.core.mvc.Profiles
{
    public class CustomerDtoToCustomerModel : Profile
    {
        public CustomerDtoToCustomerModel()
        {
            CreateMap<CustomerDto, CustomerModel>();
        }
    }
}
