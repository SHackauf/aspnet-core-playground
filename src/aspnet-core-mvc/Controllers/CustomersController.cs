using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using de.playground.aspnet.core.contracts.modules;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.mvc.Controllers
{
    public class CustomersController : Controller
    {
        #region Private Fields

        private ICustomerModule customerModule;

        #endregion

        #region Constructor

        public CustomersController(ICustomerModule customerModule) => this.customerModule = customerModule;

        #endregion

        #region Public Methods

        public async Task<IActionResult> Index()
        {
            var customers = await this.customerModule.GetCustomersAsync();
            return this.View(customers);
        }

        public async Task<IActionResult> Create()
        {
            var customer = await this.customerModule.CreateCustomerAsync();
            return this.View(nameof(this.Edit), customer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return this.NotFound();
            }

            var customer = await this.customerModule.GetCustomerAsync(id.Value);
            return customer == null ? (IActionResult)this.NotFound() : this.View(customer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return this.NotFound();
            }

            var customer = await this.customerModule.GetCustomerAsync(id.Value);
            if (customer == null)
            {
                return this.NotFound();
            }

            // TODO: Show Error on ui
            var successful = await this.customerModule.DeleteCustomerAsync(customer);
            //return successful ? this.View(nameof(this.Index)) : this.View(nameof(this.Index));
            return this.NoContent();
        }

        #endregion
    }
}
