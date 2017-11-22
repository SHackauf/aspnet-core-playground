using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.contracts.utils.logger;
using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.pocos;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.modules
{
    public class ProductModule : IProductModule
    {
        #region Private Fields

        private readonly IProductDataAccess productDataAccess;

        private readonly IMapper mapper;
        private readonly ILogger logger;

        #endregion

        #region Constructor

        public ProductModule(ILogger<ProductModule> logger, IMapper mapper, IProductDataAccess productDataAccess)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.productDataAccess = productDataAccess ?? throw new ArgumentNullException(nameof(productDataAccess));
        }

        #endregion

        #region Public Methods

        public async Task<IImmutableList<IProductDto>> GetProductsAsync(int customerId)
        {
            var productPocos = await this.productDataAccess.SelectProductsAsync(customerId);
            var productDtos = this.mapper.Map<IEnumerable<ProductDto>>(productPocos);
            this.logger.LogDebug(LoggingEvents.GetItems, $"{nameof(this.GetProductsAsync)}: [count: {productDtos.Count()}]");

            return productDtos.ToImmutableList<IProductDto>();
        }

        public async Task<IProductDto> GetProductAsync(int customerId, int id)
        {
            var productPoco = await this.productDataAccess.SelectProductAsync(customerId, id);
            var productDto = this.mapper.Map<ProductDto>(productPoco);
            this.logger.LogDebug(LoggingEvents.GetItem, $"{nameof(this.GetProductAsync)}: [id: {id}][found: {productDto != null}]");

            return productDto;
        }

        public async Task<bool> HasProductAsync(int customerId, int id)
        {
            var found = await this.productDataAccess.ExistsProductAsync(customerId, id);
            this.logger.LogDebug(LoggingEvents.HasItem, $"{nameof(this.HasProductAsync)}: [id: {id}][found: {found}]");

            return found;
        }

        public async Task<IProductDto> AddProductAsync(IProductDto productDto)
        {
            if (productDto == null)
            {
                new ArgumentNullException(nameof(productDto));
            }

            if (productDto.Id > 0)
            {
                new ArgumentException("Product has already an id.", nameof(productDto));
            }

            //TODO: Prüfen ob customer vorhanden ist.

            var productPoco = this.mapper.Map<ProductPoco>(productDto);
            var savedProductPoco = await this.productDataAccess.InsertProductAsync(productPoco);
            this.logger.LogInformation(LoggingEvents.InsertItem, $"{nameof(this.AddProductAsync)}: successful [Id: {savedProductPoco.Id}]");

            var savedProductDto = this.mapper.Map<ProductDto>(savedProductPoco);
            return savedProductDto;
        }

        public async Task<IProductDto> ModifyProductAsync(IProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto));
            }

            var productPoco = this.mapper.Map<ProductPoco>(productDto);
            if (!await this.productDataAccess.ExistsProductAsync(productPoco.CustomerId, productPoco.Id))
            {
                throw new ArgumentException("Product doesn't exist.", nameof(productDto));
            }

            var savedProductPoco = await this.productDataAccess.UpdateProductAsync(productPoco);
            this.logger.LogInformation(LoggingEvents.UpdateItem, $"{nameof(this.ModifyProductAsync)}: successful [Id: {savedProductPoco.Id}]");

            var savedProductDto = this.mapper.Map<ProductDto>(savedProductPoco);
            return savedProductDto;
        }

        public async Task<bool> DeleteProductAsync(IProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto));
            }

            var productPoco = this.mapper.Map<ProductPoco>(productDto);
            if (!await this.productDataAccess.ExistsProductAsync(productPoco.CustomerId, productPoco.Id))
            {
                return false;
            }

            var successful = await this.productDataAccess.RemoveProductAsync(productPoco.Id);
            if (successful)
            {
                this.logger.LogInformation(LoggingEvents.DeleteItem, $"{nameof(this.DeleteProductAsync)}: successful [Id: {productPoco.Id}]");
            }

            return successful;
        }

        #endregion
    }
}