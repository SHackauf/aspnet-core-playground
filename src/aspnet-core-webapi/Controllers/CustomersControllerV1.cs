using System.Collections.Immutable;
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
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomersControllerV1 : Controller
    {
        #region Private Fields

        private readonly ICustomerModule customerModule;

        #endregion

        #region Constructor

        public CustomersControllerV1(ICustomerModule customerModule) => this.customerModule = customerModule;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <remarks>
        /// Doesn't use any filter.
        /// </remarks>
        /// <returns>Returns all customers.</returns>
        /// <response code="200">Returns all customers.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IImmutableList<ICustomerDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync() => this.Ok(await this.customerModule.GetCustomersAsync());

        /// <summary>
        /// Gets customer from id.
        /// </summary>
        /// <remarks>
        /// Filters customers by id.
        /// </remarks>
        /// <param name="id">The customer id.</param>
        /// <returns>Returns customer.</returns>
        /// <response code="200">Returns the customer.</response>
        /// <response code="404">Customer not found.</response>
        [HttpGet("{id}", Name = nameof(CustomersControllerV1) + "_" + nameof(GetAsync))]
        [ProducesResponseType(typeof(ICustomerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            var customerDto = await this.customerModule.GetCustomerAsync(id);
            return customerDto == null ? (IActionResult)this.NotFound() : this.Ok(customerDto);
        }

        /// <summary>
        /// Checks if customer from id exists.
        /// </summary>
        /// <remarks>
        /// Filters customers by id.
        /// </remarks>
        /// <param name="id">The customer id.</param>
        /// <returns>Returns the check result.</returns>
        /// <response code="200">Customer found.</response>
        /// <response code="404">Customer not found.</response>
        [HttpHead("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> HeadAsync([FromRoute]int id) => await this.customerModule.HasCustomerAsync(id) ? this.Ok() : (IActionResult)this.NotFound();

        /// <summary>
        /// Adds a customer.
        /// </summary>
        /// <remarks>
        /// Customer need to have id 0. During adding customer creates a new customer id. 
        /// </remarks>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns the added customer.</returns>
        /// <response code="201">Customer added.</response>
        /// <response code="400">Customer can not added.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ICustomerDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody]CustomerDto customer)
        {
            if (customer == null || customer.Id > 0)
            {
                return this.BadRequest();
            }

            var customerDto = await this.customerModule.AddCustomerAsync(customer);
            return customerDto == null
                ? (IActionResult)this.BadRequest()
                : this.CreatedAtRoute(nameof(CustomersControllerV1) + "_" + nameof(GetAsync), new { id = customerDto.Id }, customerDto);
        }

        /// <summary>
        /// Modifies a customer.
        /// </summary>
        /// <remarks>
        /// Customer id and route id must be the same. 
        /// </remarks>
        /// <param name="id">The customer id.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns the modified customer.</returns>
        /// <response code="200">Customer modified.</response>
        /// <response code="400">Customer can not modified.</response>
        /// <response code="404">Customer not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ICustomerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutAsync([FromRoute]int id, [FromBody]CustomerDto customer)
        {
            if (customer == null || customer.Id <= 0 || id != customer.Id)
            {
                return this.BadRequest();
            }

            var customerDto = await this.customerModule.ModifyCustomerAsync(customer);
            return customerDto == null ? (IActionResult)this.NotFound() : this.Ok(customerDto);
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">The customer id.</param>
        /// <returns>Returns the delete result.</returns>
        /// <response code="204">Customer deleted.</response>
        /// <response code="404">Customer not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ICustomerDto), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id)
        {
            var customerDto = await this.customerModule.GetCustomerAsync(id);
            if (customerDto == null)
            {
                return this.NotFound();
            }

            return await this.customerModule.DeleteCustomerAsync(customerDto) ? (IActionResult)this.NoContent(): this.NotFound();
        }

        #endregion
    }
}
