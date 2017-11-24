using System.Collections.Generic;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.contracts.dataaccesses
{
    public interface IProductDataAccess : IDataAccess
    {
        Task<IEnumerable<ProductPoco>> SelectProductsAsync(int customerId);

        Task<ProductPoco> SelectProductAsync(int customerId, int id);

        Task<bool> ExistsProductAsync(int customerId, int id);

        Task<ProductPoco> InsertProductAsync(ProductPoco product);

        Task<ProductPoco> UpdateProductAsync(ProductPoco product);

        Task<bool> RemoveProductAsync(int id);
    }
}
