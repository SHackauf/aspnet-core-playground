using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.contracts.utils.logger;
using de.playground.aspnet.core.pocos;

using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.dataaccesses.inmemory
{
    // TODO: change to https://docs.microsoft.com/en-us/ef/core/providers/in-memory/
    public class ProductInMemoryDataAccess : IProductDataAccess
    {
        #region Private Fields

        private readonly ILogger logger;

        private static IList<IProductPoco> storage = new List<IProductPoco>
            {
                new ProductPoco() { Id = 1, CustomerId = 1, Name = "Product1" },
                new ProductPoco() { Id = 2, CustomerId = 1, Name = "Product2" },
                new ProductPoco() { Id = 3, CustomerId = 1, Name = "Product3" },
                new ProductPoco() { Id = 4, CustomerId = 2, Name = "Product4" },
                new ProductPoco() { Id = 5, CustomerId = 2, Name = "Product5" },
                new ProductPoco() { Id = 6, CustomerId = 3, Name = "Product6" }
            };

        private static int nextFreeId = 7;

        #endregion

        #region Constructor

        public ProductInMemoryDataAccess(ILogger<ProductInMemoryDataAccess> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        #endregion

        #region Public Methods

        public Task<IEnumerable<IProductPoco>> SelectProductsAsync(int customerId)
        {
            var productPocos = storage.Where(product => product.CustomerId == customerId);
            this.logger.LogDebug(LoggingEvents.GetItems, $"{nameof(this.SelectProductsAsync)}: [count: {productPocos.Count()}]");

            return Task.FromResult<IEnumerable<IProductPoco>>(productPocos);
        }

        public Task<IProductPoco> SelectProductAsync(int customerId, int id)
        {
            var productPocos = storage.FirstOrDefault(product => product.CustomerId == customerId && product.Id == id);
            this.logger.LogDebug(LoggingEvents.GetItem, $"{nameof(this.SelectProductAsync)}: [id: {id}][found: {productPocos != null}]");

            return Task.FromResult(productPocos);
        }

        public Task<bool> ExistsProductAsync(int customerId, int id)
        {
            var found = storage.Any(product => product.CustomerId == customerId && product.Id == id);
            this.logger.LogDebug(LoggingEvents.HasItem, $"{nameof(this.ExistsProductAsync)}: [id: {id}][found: {found}]");

            return Task.FromResult(found);
        }

        public Task<IProductPoco> InsertProductAsync(IProductPoco productPoco)
        {
            if (productPoco == null)
            {
                new ArgumentNullException(nameof(productPoco));
            }

            if (productPoco.Id > 0)
            {
                new ArgumentException("Product has already an id.", nameof(productPoco));
            }

            //TODO: Prüfen ob customer vorhanden ist.

            productPoco.Id = nextFreeId++;
            storage.Add(productPoco);
            this.logger.LogInformation(LoggingEvents.InsertItem, $"{nameof(this.InsertProductAsync)}: successful [Id: {productPoco.Id}]");

            return Task.FromResult(productPoco);
        }

        public Task<IProductPoco> UpdateProductAsync(IProductPoco productPoco)
        {
            if (productPoco == null)
            {
                throw new ArgumentNullException(nameof(productPoco));
            }

            var foundProductPoco = storage.FirstOrDefault(internalProduct => internalProduct.Id == productPoco.Id);
            if (foundProductPoco == null)
            {
                return Task.FromResult<IProductPoco>(null);
            }

            storage.Remove(foundProductPoco);
            storage.Add(productPoco);
            this.logger.LogInformation(LoggingEvents.UpdateItem, $"{nameof(this.UpdateProductAsync)}: successful [Id: {productPoco.Id}]");

            return Task.FromResult(productPoco);
        }

        public Task<bool> RemoveProductAsync(int id)
        {
            var productDto = storage.FirstOrDefault(internalProduct => internalProduct.Id == id);
            if (productDto == null)
            {
                return Task.FromResult(false);
            }

            storage.Remove(productDto);
            this.logger.LogInformation(LoggingEvents.DeleteItem, $"{nameof(this.RemoveProductAsync)}: successful [Id: {id}]");

            return Task.FromResult(true);
        }

        #endregion
    }
}
