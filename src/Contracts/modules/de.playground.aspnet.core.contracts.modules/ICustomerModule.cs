﻿using System.Collections.Immutable;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dtos;

namespace de.playground.aspnet.core.contracts.modules
{
    public interface ICustomerModule : IModule
    {
        Task<int> CountCustomersAsync();

        Task<IImmutableList<ICustomerDto>> GetCustomersAsync();

        Task<IImmutableList<ICustomerDto>> GetCustomersAsync(int offset, int limit);

        Task<ICustomerDto> GetCustomerAsync(int id);

        Task<bool> HasCustomerAsync(int id);

        Task<ICustomerDto> CreateCustomerAsync();

        Task<ICustomerDto> AddCustomerAsync(ICustomerDto customer);

        Task<ICustomerDto> ModifyCustomerAsync(ICustomerDto customer);

        Task<bool> DeleteCustomerAsync(ICustomerDto customer);
    }
}
