using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.modules
{
    public class ProductModule : IProductModule
    {
        #region Private Fields

        private readonly ILogger logger;

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

        #region Constructor

        public ProductModule(ILogger<ProductModule> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        #endregion

        #region Public Methods

        public Task<IImmutableList<IProductDto>> GetProductsAsync(int customerId)
        {
            var productDtos = storage.Where(product => product.CustomerId == customerId).ToImmutableList();
            this.logger.LogDebug($"{nameof(this.GetProductsAsync)}: [count: {productDtos.Count()}]");

            return Task.FromResult<IImmutableList<IProductDto>>(productDtos);
        }

        public Task<IProductDto> GetProductAsync(int customerId, int id)
        {
            var productDto = storage.FirstOrDefault(product => product.CustomerId == customerId && product.Id == id);
            this.logger.LogDebug($"{nameof(this.GetProductAsync)}: [id: {id}][found: {productDto != null}]");

            return Task.FromResult(productDto);
        }

        public Task<bool> HasProductAsync(int customerId, int id)
        {
            var found = storage.Any(product => product.CustomerId == customerId && product.Id == id);
            this.logger.LogDebug($"{nameof(this.HasProductAsync)}: [id: {id}][found: {found}]");

            return Task.FromResult(found);
        }

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
            this.logger.LogInformation($"{nameof(this.AddProductAsync)}: successful [Id: {product.Id}]");

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
            this.logger.LogInformation($"{nameof(this.ModifyProductAsync)}: successful [Id: {product.Id}]");

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
            this.logger.LogInformation($"{nameof(this.DeleteProductAsync)}: successful [Id: {product.Id}]");

            return Task.FromResult(true);
        }

        #endregion
    }
}