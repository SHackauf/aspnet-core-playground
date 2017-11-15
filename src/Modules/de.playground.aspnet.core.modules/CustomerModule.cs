using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;

using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.modules
{
    public class CustomerModule : ICustomerModule
    {
        #region Private Fields

        private readonly ILogger logger;

        private static IList<ICustomerDto> storage = new List<ICustomerDto>
            {
                new CustomerDto() { Id = 1, Name = "Customer1" },
                new CustomerDto() { Id = 2, Name = "Customer2" },
                new CustomerDto() { Id = 3, Name = "Customer3" }
            };

        private static int nextFreeId = 4;

        #endregion

        #region Constructor

        public CustomerModule(ILogger<CustomerModule> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        #endregion

        #region Public Methods

        public Task<IImmutableList<ICustomerDto>> GetCustomersAsync()
        {
            var customerDtos = storage.ToImmutableList();
            this.logger.LogDebug($"{nameof(this.GetCustomersAsync)}: [count: {customerDtos.Count()}]");

            return Task.FromResult<IImmutableList<ICustomerDto>>(customerDtos);
        }

        public Task<ICustomerDto> GetCustomerAsync(int id)
        {
            var customerDto = storage.FirstOrDefault(customer => customer.Id == id);
            this.logger.LogDebug($"{nameof(this.GetCustomerAsync)}: [id: {id}][found: {customerDto != null}]");

            return Task.FromResult(customerDto);
        }

        public Task<bool> HasCustomerAsync(int id)
        {
            var found = storage.Any(customer => customer.Id == id);
            this.logger.LogDebug($"{nameof(this.HasCustomerAsync)}: [id: {id}][found: {found}]");

            return Task.FromResult(found);
        }

        public Task<ICustomerDto> CreateCustomerAsync()
        {
            var customer = new CustomerDto { Id = 0, Name = string.Empty };
            this.logger.LogDebug($"{nameof(this.CreateCustomerAsync)}: [id: {customer.Id}]");

            return Task.FromResult<ICustomerDto>(customer);
        }

        public Task<ICustomerDto> AddCustomerAsync(ICustomerDto customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (customer.Id > 0)
            {
                throw new ArgumentException("Customer has already an id.", nameof(customer));
            }

            customer.Id = nextFreeId++;
            storage.Add(customer);
            this.logger.LogInformation($"{nameof(this.AddCustomerAsync)}: successful [Id: {customer.Id}]");

            return Task.FromResult(customer);
        }

        public Task<ICustomerDto> ModifyCustomerAsync(ICustomerDto customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            var customerDto = storage.FirstOrDefault(internalCustomer => internalCustomer.Id == customer.Id);
            if (customerDto == null)
            {
                return Task.FromResult<ICustomerDto>(null);
            }

            storage.Remove(customerDto);
            storage.Add(customer);
            this.logger.LogInformation($"{nameof(this.ModifyCustomerAsync)}: successful [Id: {customer.Id}]");

            return Task.FromResult(customer);
        }

        public Task<bool> DeleteCustomerAsync(ICustomerDto customer)
        {
            var customerDto = storage.FirstOrDefault(internalCustomer => internalCustomer.Id == customer.Id);
            if (customerDto == null)
            {
                return Task.FromResult(false);
            }

            // TODO: Remove products
            storage.Remove(customerDto);
            this.logger.LogInformation($"{nameof(this.DeleteCustomerAsync)}: successful [Id: {customer.Id}]");

            return Task.FromResult(true);
        }

        #endregion
    }
}
