using System.Collections.Generic;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.contracts.dataaccesses
{
    public interface ICustomerDataAccess : IDataAccess
    {
        Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync();

        Task<ICustomerPoco> SelectCustomerAsync(int id);

        Task<bool> ExistsCustomerAsync(int id);

        Task<ICustomerPoco> InsertCustomerAsync(ICustomerPoco customer);

        Task<ICustomerPoco> UpdateCustomerAsync(ICustomerPoco customer);

        Task<bool> RemoveCustomerAsync(int id);
    }
}
