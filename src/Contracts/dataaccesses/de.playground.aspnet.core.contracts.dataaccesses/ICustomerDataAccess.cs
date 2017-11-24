using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.contracts.dataaccesses
{
    public interface ICustomerDataAccess : IDataAccess
    {
        Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync();

        Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync(Expression<Func<ICustomerPoco, bool>> whereExpression);

        Task<ICustomerPoco> SelectCustomerAsync(int id);

        Task<bool> ExistsCustomerAsync(int id);

        Task<ICustomerPoco> InsertCustomerAsync(ICustomerPoco customer);

        Task<ICustomerPoco> UpdateCustomerAsync(ICustomerPoco customer);

        Task<bool> RemoveCustomerAsync(int id);
    }
}
