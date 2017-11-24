using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.contracts.utils.logger;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.dataaccesses.mariadb
{
    public class CustomerMariaDbDataAccess : ICustomerDataAccess
    {
        #region Private Fields

        private readonly MariaDbContext mariaDbContext;

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public CustomerMariaDbDataAccess(ILogger<CustomerMariaDbDataAccess> logger, MariaDbContext mariaDbContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mariaDbContext = mariaDbContext ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync()
        {
            try
            {
                return await this.mariaDbContext.Customers.AsNoTracking().ToArrayAsync();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select customers.");
                throw exception;
            }
        }

        public async Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync(Expression<Func<ICustomerPoco, bool>> whereExpression)
        {
            try
            {
                return await this.mariaDbContext.Customers.AsNoTracking().Where(whereExpression).ToArrayAsync();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select customers.");
                throw exception;
            }
        }

        public async Task<ICustomerPoco> SelectCustomerAsync(int id)
        {
            try
            {
                return await this.mariaDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(customer => customer.Id == id);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select customer.");
                throw exception;
            }
        }

        public async Task<bool> ExistsCustomerAsync(int id)
        {
            try
            {
                //return await this.mariaDbContext.Customers.AnyAsync(customer => customer.Id == id);
                return 1 == await this.mariaDbContext.Customers.AsNoTracking().CountAsync(customer => customer.Id == id);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select customer.");
                throw exception;
            }
        }

        public async Task<ICustomerPoco> InsertCustomerAsync(ICustomerPoco customer)
        {
            try
            {
                this.mariaDbContext.Add(customer);
                var count = await this.mariaDbContext.SaveChangesAsync();

                if (count == 1)
                {
                    this.logger.LogInformation(LoggingEvents.InsertItem, $"{nameof(this.InsertCustomerAsync)}: successful [Id: {customer.Id}]");
                    return customer;
                }

                this.logger.LogWarning(LoggingEvents.InsertItem, $"{nameof(this.InsertCustomerAsync)}: faulty [Id: {customer.Id}]");
                return null;
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.logger.LogError(dbUpdateException, "Can't create customer.");
                throw dbUpdateException;
            }
        }

        public async Task<ICustomerPoco> UpdateCustomerAsync(ICustomerPoco customer)
        {
            try
            {
                this.mariaDbContext.Update(customer);
                var count = await this.mariaDbContext.SaveChangesAsync();

                if (count == 1)
                {
                    this.logger.LogInformation(LoggingEvents.UpdateItem, $"{nameof(this.UpdateCustomerAsync)}: successful [Id: {customer.Id}]");
                    return customer;
                }

                this.logger.LogWarning(LoggingEvents.UpdateItem, $"{nameof(this.UpdateCustomerAsync)}: faulty [Id: {customer.Id}]");
                return null;
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.logger.LogError(dbUpdateException, "Can't update customer.");
                throw dbUpdateException;
            }
        }

        public async Task<bool> RemoveCustomerAsync(int id)
        {
            try
            {
                var customer = await this.SelectCustomerAsync(id);
                if (customer == null)
                {
                    return false;
                }

                this.mariaDbContext.Remove(customer);
                var count = await this.mariaDbContext.SaveChangesAsync();

                if (count == 1)
                {
                    this.logger.LogInformation(LoggingEvents.DeleteItem, $"{nameof(this.RemoveCustomerAsync)}: successful [Id: {customer.Id}]");
                    return true;
                }

                this.logger.LogWarning(LoggingEvents.DeleteItem, $"{nameof(this.RemoveCustomerAsync)}: faulty [Id: {customer.Id}]");
                return false;
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.logger.LogError(dbUpdateException, "Can't remove customer.");
                throw dbUpdateException;
            }
        }

        #endregion
    }
}
