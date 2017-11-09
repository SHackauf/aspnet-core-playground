using System.Collections.Immutable;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.contracts.modules
{
    public interface ICustomerModule : IModule
    {
        Task<IImmutableList<ICustomerDto>> GetCustomersAsync();

        Task<ICustomerDto> GetCustomerAsync(int id);

        Task<bool> HasCustomerAsync(int id);

        Task<ICustomerDto> AddCustomerAsync(ICustomerDto customer);

        Task<ICustomerDto> ModifyCustomerAsync(ICustomerDto customer);

        Task<bool> DeleteCustomerAsync(ICustomerDto customer);
    }
}
