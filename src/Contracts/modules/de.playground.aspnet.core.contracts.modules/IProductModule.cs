using System.Collections.Immutable;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.contracts.modules
{
    public interface IProductModule
    {
        Task<IImmutableList<IProductDto>> GetProductsAsync(int customerId);

        Task<IProductDto> GetProductAsync(int customerId, int id);

        Task<bool> HasProductAsync(int customerId, int id);

        Task<IProductDto> AddProductAsync(IProductDto product);

        Task<IProductDto> ModifyProductAsync(IProductDto product);

        Task<bool> DeleteProductAsync(IProductDto product);
    }
}
