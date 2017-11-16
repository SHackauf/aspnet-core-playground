using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.webapi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/customers/{customerid}/products")]
    public class ProductsControllerV1 : Controller
    {
        #region Private Fields

        private readonly IProductModule productModule;

        #endregion

        #region Constructor

        public ProductsControllerV1(IProductModule productModule) => this.productModule = productModule;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all products from customer.
        /// </summary>
        /// <remarks>
        /// Filters by customer id.
        /// </remarks>
        /// <param name="customerId">The customer id.</param>
        /// <returns>Returns all products.</returns>
        /// <response code="200">Returns all products.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IImmutableList<IProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync(int customerId) => this.Ok(await this.productModule.GetProductsAsync(customerId));

        /// <summary>
        /// Gets a special product from customer.
        /// </summary>
        /// <remarks>
        /// Filters by customer id and product id.
        /// </remarks>
        /// <param name="customerId">The customer id.</param>
        /// <param name="id">The product id.</param>
        /// <returns>Returns the product.</returns>
        /// <response code="200">Return the product.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id}", Name = nameof(ProductsControllerV1) + "_" + nameof(GetAsync))]
        [ProducesResponseType(typeof(IProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int customerId, int id)
        {
            var productDto = await this.productModule.GetProductAsync(customerId, id);
            return productDto == null ? (IActionResult)this.NotFound() : this.Ok(productDto);
        }

        /// <summary>
        /// Checks if special product from customer excists.
        /// </summary>
        /// <remarks>
        /// Filters by customer id and product id.
        /// </remarks>
        /// <param name="customerId">The customer id.</param>
        /// <param name="id">The product id.</param>
        /// <returns>Returns the check result.</returns>
        /// <response code="200">Product found.</response>
        /// <response code="404">Product not found.</response>
        [HttpHead("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> HeadAsync(int customerId, int id) => await this.productModule.HasProductAsync(customerId, id) ? this.Ok() : (IActionResult)this.NotFound();

        /// <summary>
        /// Adds a product.
        /// </summary>
        /// <remarks>
        /// Customer id from route need to be the same like customer id of product.
        /// Product need to have id 0. During adding product creates a new product id. 
        /// </remarks>
        /// <param name="customerId">The customer id.</param>
        /// <param name="product">The product.</param>
        /// <returns>Returns the added product.</returns>
        /// <response code="201">Product added.</response>
        /// <response code="400">Product can not added.</response>
        [HttpPost]
        [ProducesResponseType(typeof(IProductDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync(int customerId, [FromBody]ProductDto product)
        {
            if (product == null || product.Id > 0 || product.CustomerId != customerId)
            {
                return this.BadRequest();
            }

            var productDto = await this.productModule.AddProductAsync(product);
            return productDto == null
                ? (IActionResult)this.BadRequest()
                : this.CreatedAtRoute(nameof(ProductsControllerV1) + "_" + nameof(GetAsync), new { id = productDto.Id }, productDto);
        }

        /// <summary>
        /// Modifies a product.
        /// </summary>
        /// <remarks>
        /// Customer id from route need to be the same like customer id of product.
        /// Product id and route id must be the same. 
        /// </remarks>
        /// <param name="customerId">The customer id.</param>
        /// <param name="id">The product id.</param>
        /// <param name="product">The product.</param>
        /// <returns>Returns the modified product.</returns>
        /// <response code="200">Product modified.</response>
        /// <response code="400">Product can not modified.</response>
        /// <response code="404">Product not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutAsync(int customerId, int id, [FromBody]ProductDto product)
        {
            if (product == null || product.Id <= 0 || id != product.Id || customerId != product.CustomerId)
            {
                return this.BadRequest();
            }

            var productDto = await this.productModule.ModifyProductAsync(product);
            return productDto == null ? (IActionResult)this.NotFound() : this.Ok(productDto);
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="customerId">The customer id.</param>
        /// <param name="id">The product id.</param>
        /// <returns>Returns the delete result.</returns>
        /// <response code="204">Product deleted.</response>
        /// <response code="404">Product not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ICustomerDto), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAsync(int customerId, int id)
        {
            var productDto = await this.productModule.GetProductAsync(customerId, id);
            if (productDto == null)
            {
                return this.NotFound();
            }

            return await this.productModule.DeleteProductAsync(productDto) ? (IActionResult)this.NoContent() : this.NotFound();
        }

        #endregion
    }
}
