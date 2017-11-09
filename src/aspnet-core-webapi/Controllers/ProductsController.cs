using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.webapi.Controllers
{
    [Route("api/customers/{customerid}/[controller]")]
    public class ProductsController : Controller
    {
        #region Private Fields

        private readonly IProductModule productModule;

        #endregion

        #region Constructor

        public ProductsController(IProductModule productModule) => this.productModule = productModule;

        #endregion

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> GetAsync(int customerId) => this.Ok(await this.productModule.GetProductsAsync(customerId));

        [HttpGet("{id}", Name = nameof(GetAsync))]
        public async Task<IActionResult> GetAsync(int customerId, int id)
        {
            var productDto = await this.productModule.GetProductAsync(customerId, id);
            return productDto == null ? (IActionResult)this.NotFound() : this.Ok(productDto);
        }

        [HttpHead("{id}")]
        public async Task<IActionResult> HeadAsync(int customerId, int id) => await this.productModule.HasProductAsync(customerId, id) ? this.Ok() : (IActionResult)this.NotFound();

        [HttpPost]
        public async Task<IActionResult> PostAsync(int customerId, [FromBody]ProductDto product)
        {
            if (product == null || product.Id > 0 || product.CustomerId != customerId)
            {
                return this.BadRequest();
            }

            var productDto = await this.productModule.AddProductAsync(product);
            return productDto == null
                ? (IActionResult)this.BadRequest()
                : this.CreatedAtRoute(nameof(this.GetAsync), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int customerId, int id, [FromBody]ProductDto product)
        {
            if (product == null || product.Id <= 0 || id != product.Id || customerId != product.CustomerId)
            {
                return this.BadRequest();
            }

            var productDto = await this.productModule.ModifyProductAsync(product);
            return productDto == null ? (IActionResult)this.NotFound() : this.Ok(productDto);
        }

        [HttpDelete("{id}")]
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
