using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.webapi.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        #region Private Fields

        private readonly Dictionary<int, string> storage;
        private int nextFreeId = 1;

        #endregion

        #region Constructor

        public CustomersController() => this.storage = new Dictionary<int, string>
            {
                { nextFreeId++, "Customer 1" },
                { nextFreeId++, "Customer 2" },
                { nextFreeId++, "Customer 3" }
            };

        #endregion

        #region Public Methods

        [HttpGet]
        public IActionResult Get() => this.Ok(this.storage.Values);

        [HttpGet("{id}", Name = nameof(Get))]
        public IActionResult Get(int id)
        {
            if (this.storage.ContainsKey(id))
            {
                return this.Ok(this.storage[id]);
            }

            return this.NotFound();
        }

        [HttpHead("{id}")]
        public IActionResult Head(int id) => this.storage.ContainsKey(id) ? this.Ok() : (IActionResult)this.NotFound();

        [HttpPost]
        public IActionResult Post(string customer) // [FromBody]string value
        {
            if (string.IsNullOrEmpty(customer))
            {
                return this.BadRequest();
            }

            var customerId = nextFreeId++;
            this.storage.Add(customerId, customer);

            return this.CreatedAtRoute(nameof(this.Get), new { id = customerId }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, string customer)  // [FromBody]string value
        {
            if (string.IsNullOrEmpty(customer))
            {
                return this.BadRequest();
            }

            if (!this.storage.ContainsKey(id))
            {
                return NotFound();
            }

            this.storage[id] = customer;
            return this.Ok(customer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!this.storage.ContainsKey(id))
            {
                return NotFound();
            }

            this.storage.Remove(id);
            return this.NoContent();
        }

        #endregion
    }
}
