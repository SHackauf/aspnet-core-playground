using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.contracts.utils.logger;

using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.dataaccesses.inmemory
{
    // TODO: change to https://docs.microsoft.com/en-us/ef/core/providers/in-memory/
    public class CustomerInMemoryDataAccess : ICustomerDataAccess
    {
        #region Private Fields

        private readonly ILogger logger;

        private static IList<CustomerPoco> storage = new List<CustomerPoco>
            {
                new CustomerPoco() { Id = 1, Name = "Customer1" },
                new CustomerPoco() { Id = 2, Name = "Customer2" },
                new CustomerPoco() { Id = 3, Name = "Customer3" }
            };

        private static int nextFreeId = 4;

        #endregion

        #region Constructor

        public CustomerInMemoryDataAccess(ILogger<CustomerInMemoryDataAccess> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger)); 

        #endregion

        #region Public Methods

        public Task<IEnumerable<CustomerPoco>> SelectCustomersAsync()
        {
            var customerPocos = storage.ToArray();
            this.logger.LogDebug(LoggingEvents.GetItems, $"{nameof(this.SelectCustomersAsync)}: [count: {customerPocos.Count()}]");

            return Task.FromResult<IEnumerable<CustomerPoco>>(customerPocos);
        }

        public Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(Expression<Func<CustomerPoco, bool>> whereExpression)
        {
            var customerPocos = storage.Where(whereExpression.Compile()).ToArray();
            this.logger.LogDebug(LoggingEvents.GetItems, $"{nameof(this.SelectCustomersAsync)}: [count: {customerPocos.Count()}]");

            return Task.FromResult<IEnumerable<CustomerPoco>>(customerPocos);
        }

        public Task<CustomerPoco> SelectCustomerAsync(int id)
        {
            var customerPoco = storage.FirstOrDefault(customer => customer.Id == id);
            this.logger.LogDebug(LoggingEvents.GetItem, $"{nameof(this.SelectCustomerAsync)}: [id: {id}][found: {customerPoco != null}]");

            return Task.FromResult(customerPoco);
        }

        public Task<bool> ExistsCustomerAsync(int id)
        {
            var found = storage.Any(customer => customer.Id == id);
            this.logger.LogDebug(LoggingEvents.HasItem, $"{nameof(this.ExistsCustomerAsync)}: [id: {id}][found: {found}]");

            return Task.FromResult(found);
        }

        public Task<CustomerPoco> InsertCustomerAsync(CustomerPoco customerPoco)
        {
            if (customerPoco == null)
            {
                throw new ArgumentNullException(nameof(customerPoco));
            }

            if (customerPoco.Id > 0)
            {
                throw new ArgumentException("Customer has already an id.", nameof(customerPoco));
            }

            customerPoco.Id = nextFreeId++;
            storage.Add(customerPoco);
            this.logger.LogInformation(LoggingEvents.InsertItem, $"{nameof(this.InsertCustomerAsync)}: successful [Id: {customerPoco.Id}]");

            return Task.FromResult(customerPoco);
        }

        public Task<CustomerPoco> UpdateCustomerAsync(CustomerPoco customerPoco)
        {
            if (customerPoco == null)
            {
                throw new ArgumentNullException(nameof(customerPoco));
            }

            var foundCustomerPoco = storage.FirstOrDefault(internalCustomer => internalCustomer.Id == customerPoco.Id);
            if (foundCustomerPoco == null)
            {
                return Task.FromResult<CustomerPoco>(null);
            }

            storage.Remove(foundCustomerPoco);
            storage.Add(customerPoco);
            this.logger.LogInformation(LoggingEvents.UpdateItem, $"{nameof(this.UpdateCustomerAsync)}: successful [Id: {customerPoco.Id}]");

            return Task.FromResult(customerPoco);
        }

        public Task<bool> RemoveCustomerAsync(int id)
        {
            var customerPoco = storage.FirstOrDefault(internalCustomer => internalCustomer.Id == id);
            if (customerPoco == null)
            {
                return Task.FromResult(false);
            }

            // TODO: Remove products
            storage.Remove(customerPoco);
            this.logger.LogInformation(LoggingEvents.DeleteItem, $"{nameof(this.RemoveCustomerAsync)}: successful [Id: {id}]");

            return Task.FromResult(true);
        }

        #endregion
    }
}
