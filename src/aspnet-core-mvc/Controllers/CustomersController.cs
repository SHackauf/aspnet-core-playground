using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;
using de.playground.aspnet.core.mvc.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.mvc.Controllers
{
    public class CustomersController : Controller
    {
        #region Private Fields

        private readonly ICustomerModule customerModule;
        private readonly IMapper mapper;

        #endregion

        #region Constructor

        public CustomersController(ICustomerModule customerModule, IMapper mapper)
        {
            this.customerModule = customerModule;
            this.mapper = mapper;
        }

        #endregion

        #region Public Methods

        public async Task<IActionResult> Index()
        {
            var customers = await this.customerModule.GetCustomersAsync();
            var customerModels = this.mapper.Map<IEnumerable<CustomerModel>>(customers);
            return this.View(customerModels);
        }

        public async Task<IActionResult> Create()
        {
            var customer = await this.customerModule.CreateCustomerAsync();
            var customerModel = this.mapper.Map<CustomerModel>(customer);
            return this.View(nameof(this.Edit), customerModel);
        }

        public async Task<IActionResult> Show(int? id)
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

            var customerModel = this.mapper.Map<CustomerModel>(customer);
            return this.View(customerModel);
        }

        public async Task<IActionResult> Edit(int? id)
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

            var customerModel = this.mapper.Map<CustomerModel>(customer);
            return this.View(customerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CustomerModel customerModel)
        {
            if (id != customerModel.Id)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                if (customerModel.Id > 0)
                {
                    var customerDto = await this.customerModule.GetCustomerAsync(id);
                    if (customerDto == null)
                    {
                        return this.NotFound();
                    }

                    var mappedCustomerDto = this.mapper.Map(customerModel, customerDto);
                    var savedCustomerDto = this.customerModule.ModifyCustomerAsync(mappedCustomerDto);
                    if (savedCustomerDto == null)
                    {
                        return View(savedCustomerDto);
                    }
                }
                else
                {
                    var mappedCustomerDto = this.mapper.Map<CustomerDto>(customerModel);
                    var savedCustomerDto = this.customerModule.AddCustomerAsync(mappedCustomerDto);
                    if (savedCustomerDto == null)
                    {
                        return View(savedCustomerDto);
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return View(customerModel);
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
            return successful ? this.RedirectToAction(nameof(this.Index)) : this.RedirectToAction(nameof(this.Index));
        }

        #endregion
    }
}
