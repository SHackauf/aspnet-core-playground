using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.contracts.dataaccesses
{
    public interface ICustomerDataAccess : IDataAccess
    {
        Task<IEnumerable<CustomerPoco>> SelectCustomersAsync();

        Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(Expression<Func<CustomerPoco, bool>> whereExpression);

        Task<CustomerPoco> SelectCustomerAsync(int id);

        Task<bool> ExistsCustomerAsync(int id);

        Task<CustomerPoco> InsertCustomerAsync(CustomerPoco customer);

        Task<CustomerPoco> UpdateCustomerAsync(CustomerPoco customer);

        Task<bool> RemoveCustomerAsync(int id);
    }
}
