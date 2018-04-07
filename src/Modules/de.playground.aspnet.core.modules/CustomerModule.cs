using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.dtos;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.contracts.utils.logger;
using de.playground.aspnet.core.dtos;

using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.modules
{
    public class CustomerModule : ICustomerModule
    {
        #region Private Fields

        private readonly ICustomerDataAccess customerDataAccess;

        private readonly IMapper mapper;
        private readonly ILogger logger;

        #endregion

        #region Constructor

        public CustomerModule(ILogger<CustomerModule> logger, IMapper mapper, ICustomerDataAccess customerDataAccess)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.customerDataAccess = customerDataAccess ?? throw new ArgumentNullException(nameof(customerDataAccess));
        }

        #endregion

        #region Public Methods

        public async Task<int> CountCustomersAsync()
        {
            var count = await this.customerDataAccess.CountCustomersAsync();
            this.logger.LogDebug(LoggingEvents.CountIems, $"{nameof(this.CountCustomersAsync)}: [count: {count}]");

            return count;
        }

        public async Task<IImmutableList<ICustomerDto>> GetCustomersAsync()
        {
            var customerPocos = await this.customerDataAccess.SelectCustomersAsync();
            var customerDtos = this.mapper.Map<IEnumerable<CustomerDto>>(customerPocos);
            this.logger.LogDebug(LoggingEvents.GetItems, $"{nameof(this.GetCustomersAsync)}: [count: {customerDtos.Count()}]");

            return customerDtos.ToImmutableList<ICustomerDto>();
        }

        public async Task<IImmutableList<ICustomerDto>> GetCustomersAsync(int offset, int limit)
        {
            var customerPocos = await this.customerDataAccess.SelectCustomersAsync();
            var customerDtos = this.mapper.Map<IEnumerable<CustomerDto>>(customerPocos);
            this.logger.LogDebug(LoggingEvents.GetItems, $"{nameof(this.GetCustomersAsync)}: [count: {customerDtos.Count()}]");

            return customerDtos.ToImmutableList<ICustomerDto>();
        }

        public async Task<ICustomerDto> GetCustomerAsync(int id)
        {
            var customerPoco = await this.customerDataAccess.SelectCustomerAsync(id);
            var customerDto = this.mapper.Map<CustomerDto>(customerPoco);
            this.logger.LogDebug(LoggingEvents.GetItem, $"{nameof(this.GetCustomerAsync)}: [id: {id}][found: {customerDto != null}]");

            return customerDto;
        }

        public async Task<bool> HasCustomerAsync(int id)
        {
            var found = await this.customerDataAccess.ExistsCustomerAsync(id);
            this.logger.LogDebug(LoggingEvents.HasItem, $"{nameof(this.HasCustomerAsync)}: [id: {id}][found: {found}]");

            return found;
        }

        public Task<ICustomerDto> CreateCustomerAsync()
        {
            var customer = new CustomerDto { Id = 0, Name = string.Empty };
            this.logger.LogDebug(LoggingEvents.CreateItem, $"{nameof(this.CreateCustomerAsync)}: [id: {customer.Id}]");

            return Task.FromResult<ICustomerDto>(customer);
        }

        public async Task<ICustomerDto> AddCustomerAsync(ICustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            if (customerDto.Id > 0)
            {
                throw new ArgumentException("Customer has already an id.", nameof(customerDto));
            }

            var customerPoco = this.mapper.Map<CustomerPoco>(customerDto);
            var savedCustomerPoco = await this.customerDataAccess.InsertCustomerAsync(customerPoco);
            this.logger.LogInformation(LoggingEvents.InsertItem, $"{nameof(this.AddCustomerAsync)}: successful [Id: {savedCustomerPoco.Id}]");

            var savedCustomerDto = this.mapper.Map<CustomerDto>(savedCustomerPoco);
            return savedCustomerDto;
        }

        public async Task<ICustomerDto> ModifyCustomerAsync(ICustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            var customerPoco = this.mapper.Map<CustomerPoco>(customerDto);
            if (!await this.customerDataAccess.ExistsCustomerAsync(customerPoco.Id))
            {
                throw new ArgumentException("Customer doesn't exist.", nameof(customerDto));
            }

            var savedCustomerPoco = await this.customerDataAccess.UpdateCustomerAsync(customerPoco);
            this.logger.LogInformation(LoggingEvents.UpdateItem, $"{nameof(this.ModifyCustomerAsync)}: successful [Id: {savedCustomerPoco.Id}]");

            var savedCustomerDto = this.mapper.Map<CustomerDto>(savedCustomerPoco);
            return savedCustomerDto;
        }

        public async Task<bool> DeleteCustomerAsync(ICustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            var customerPoco = this.mapper.Map<CustomerPoco>(customerDto);
            if (!await this.customerDataAccess.ExistsCustomerAsync(customerPoco.Id))
            {
                return false;
            }

            // TODO: Remove products
            var successful = await this.customerDataAccess.RemoveCustomerAsync(customerPoco.Id);
            if (successful)
            {
                this.logger.LogInformation(LoggingEvents.DeleteItem, $"{nameof(this.DeleteCustomerAsync)}: successful [Id: {customerPoco.Id}]");
            }

            return successful;
        }

        #endregion
    }
}
