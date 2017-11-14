using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dtos;

namespace de.playground.aspnet.core.modules
{
    public class CustomerModule : ICustomerModule
    {
        #region Private Fields

        private static IList<ICustomerDto> storage = new List<ICustomerDto>
            {
                new CustomerDto() { Id = 1, Name = "Customer1" },
                new CustomerDto() { Id = 2, Name = "Customer2" },
                new CustomerDto() { Id = 3, Name = "Customer3" }
            };

        private static int nextFreeId = 4;

        #endregion

        #region Public Methods

        public Task<IImmutableList<ICustomerDto>> GetCustomersAsync() => Task.FromResult<IImmutableList<ICustomerDto>>(storage.ToImmutableList());

        public Task<ICustomerDto> GetCustomerAsync(int id) => Task.FromResult(storage.FirstOrDefault(customer => customer.Id == id));

        public Task<bool> HasCustomerAsync(int id) => Task.FromResult(storage.Any(customer => customer.Id == id));

        public Task<ICustomerDto> CreateCustomerAsync() => Task.FromResult<ICustomerDto>(new CustomerDto { Id = 0, Name = string.Empty });

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
            return Task.FromResult(true);
        }

        #endregion
    }
}
