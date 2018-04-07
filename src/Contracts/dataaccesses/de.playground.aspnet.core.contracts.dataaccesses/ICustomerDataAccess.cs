using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.contracts.dataaccesses
{
    public interface ICustomerDataAccess : IDataAccess
    {
        Task<int> CountCustomersAsync();

        Task<IEnumerable<CustomerPoco>> SelectCustomersAsync();

        Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(int offset, int limit);

        Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(Expression<Func<CustomerPoco, bool>> whereExpression);

        Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(Expression<Func<CustomerPoco, bool>> whereExpression, int offset, int limit);

        Task<CustomerPoco> SelectCustomerAsync(int id);

        Task<bool> ExistsCustomerAsync(int id);

        Task<CustomerPoco> InsertCustomerAsync(CustomerPoco customer);

        Task<CustomerPoco> UpdateCustomerAsync(CustomerPoco customer);

        Task<bool> RemoveCustomerAsync(int id);
    }
}
