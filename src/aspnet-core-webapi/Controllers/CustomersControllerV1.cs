using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetAsync() => this.Ok(await this.customerModule.GetCustomersAsync());

        [HttpGet("{id}", Name = nameof(CustomersControllerV1) + "_" + nameof(GetAsync))]
        public async Task<IActionResult> GetAsync(int id)
        {
            var customerDto = await this.customerModule.GetCustomerAsync(id);
            return customerDto == null ? (IActionResult)this.NotFound() : this.Ok(customerDto);
        }

        [HttpHead("{id}")]
        public async Task<IActionResult> HeadAsync(int id) => await this.customerModule.HasCustomerAsync(id) ? this.Ok() : (IActionResult)this.NotFound();

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]CustomerDto customer)
        {
            if (customer == null || customer.Id <= 0 || id != customer.Id)
            {
                return this.BadRequest();
            }

            var customerDto = await this.customerModule.ModifyCustomerAsync(customer);
            return customerDto == null ? (IActionResult)this.NotFound() : this.Ok(customerDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
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
