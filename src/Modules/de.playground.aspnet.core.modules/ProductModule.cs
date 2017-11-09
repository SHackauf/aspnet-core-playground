using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;

namespace de.playground.aspnet.core.modules
{
    public class ProductModule : IProductModule
    {
        #region Private Fields

        private static IList<IProductDto> storage = new List<IProductDto>
            {
                new ProductDto() { Id = 1, CustomerId = 1, Name = "Product1" },
                new ProductDto() { Id = 2, CustomerId = 1, Name = "Product2" },
                new ProductDto() { Id = 3, CustomerId = 1, Name = "Product3" },
                new ProductDto() { Id = 4, CustomerId = 2, Name = "Product4" },
                new ProductDto() { Id = 5, CustomerId = 2, Name = "Product5" },
                new ProductDto() { Id = 6, CustomerId = 3, Name = "Product6" }
            };

        private static int nextFreeId = 7;

        #endregion

        #region Public Methods

        public Task<IImmutableList<IProductDto>> GetProductsAsync(int customerId) 
            => Task.FromResult<IImmutableList<IProductDto>>(storage.Where(product => product.CustomerId == customerId).ToImmutableList());

        public Task<IProductDto> GetProductAsync(int customerId, int id)
            => Task.FromResult(storage.FirstOrDefault(product => product.CustomerId == customerId && product.Id == id));

        public Task<bool> HasProductAsync(int customerId, int id)
            => Task.FromResult(storage.Any(product => product.CustomerId == customerId && product.Id == id));

        public Task<IProductDto> AddProductAsync(IProductDto product)
        {
            if (product == null)
            {
                new ArgumentNullException(nameof(product));
            }

            if (product.Id > 0)
            {
                new ArgumentException("Product has already an id.", nameof(product));
            }

            //TODO: Prüfen ob customer vorhanden ist.

            product.Id = nextFreeId++;
            storage.Add(product);
            return Task.FromResult(product);
        }

        public Task<IProductDto> ModifyProductAsync(IProductDto product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var productDto = storage.FirstOrDefault(internalProduct => internalProduct.Id == product.Id);
            if (productDto == null)
            {
                return Task.FromResult<IProductDto>(null);
            }

            storage.Remove(productDto);
            storage.Add(product);
            return Task.FromResult(product);
        }

        public Task<bool> DeleteProductAsync(IProductDto product)
        {
            var productDto = storage.FirstOrDefault(internalProduct => internalProduct.Id == product.Id);
            if (productDto == null)
            {
                return Task.FromResult(false);
            }

            storage.Remove(productDto);
            return Task.FromResult(true);
        }

        #endregion
    }
}