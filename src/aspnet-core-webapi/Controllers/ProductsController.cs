using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.webapi.Controllers
{
    [Route("api/customers/{customerid}/[controller]")]
    public class ProductsController : Controller
    {
        #region Private Fields

        private readonly Dictionary<int, Dictionary<int, string>> storage;
        private int nextFreeId = 1;

        #endregion

        #region Constructor

        public ProductsController() => this.storage = new Dictionary<int, Dictionary<int, string>>
            {
                { 1, new Dictionary<int, string> { { nextFreeId++, "Product 1" }, { nextFreeId++, "Product 2" }, { nextFreeId++, "Product 3" } } },
                { 2, new Dictionary<int, string> { { nextFreeId++, "Product 1" }, { nextFreeId++, "Product 2" } } },
                { 3, new Dictionary<int, string> { { nextFreeId++, "Product 1" } } },
            };

        #endregion

        #region Public Methods

        [HttpGet]
        public IActionResult Get(int customerId)
        {
            // TODO: check if customer exists.
            if (this.storage.ContainsKey(customerId))
            {
                return this.Ok(this.storage[customerId].Values);
            }

            return this.Ok(Enumerable.Empty<string>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int customerId, int id)
        {
            if (this.storage.ContainsKey(customerId) && this.storage[customerId].ContainsKey(id))
            {
                return this.Ok(this.storage[customerId][id]);
            }

            return this.NotFound();
        }

        [HttpHead("{id}")]
        public IActionResult Head(int customerId, int id) => this.storage.ContainsKey(customerId) && this.storage[customerId].ContainsKey(id) ? this.Ok() : (IActionResult)this.NotFound();

        [HttpPost]
        public IActionResult Post(int customerId, [FromBody]string product) // [FromBody]string product
        {
            // TODO: check if customer exists.
            if (string.IsNullOrEmpty(product))
            {
                return this.BadRequest();
            }

            if (!this.storage.ContainsKey(customerId))
            {
                this.storage.Add(customerId, new Dictionary<int, string>());
            }

            var productId = nextFreeId++;
            this.storage[customerId].Add(productId, product);

            return this.CreatedAtRoute(nameof(this.Get), new { customerId = customerId, id = productId }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int customerId, int id, string product) // [FromBody]string value
        {
            if (string.IsNullOrEmpty(product))
            {
                return this.BadRequest();
            }

            if (!this.storage.ContainsKey(customerId) || !this.storage[customerId].ContainsKey(id))
            {
                return NotFound();
            }

            this.storage[customerId][id] = product;
            return this.Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int customerId, int id)
        {
            if (!this.storage.ContainsKey(customerId) || !this.storage[customerId].ContainsKey(id))
            {
                return NotFound();
            }

            this.storage[customerId].Remove(id);
            return this.NoContent();
        }

        #endregion
    }
}
