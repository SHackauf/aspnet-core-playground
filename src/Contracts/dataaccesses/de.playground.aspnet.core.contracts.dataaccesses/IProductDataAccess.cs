using System.Collections.Generic;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.contracts.dataaccesses
{
    public interface IProductDataAccess : IDataAccess
    {
        Task<IEnumerable<IProductPoco>> SelectProductsAsync(int customerId);

        Task<IProductPoco> SelectProductAsync(int customerId, int id);

        Task<bool> ExistsProductAsync(int customerId, int id);

        Task<IProductPoco> InsertProductAsync(IProductPoco product);

        Task<IProductPoco> UpdateProductAsync(IProductPoco product);

        Task<bool> RemoveProductAsync(int id);
    }
}
