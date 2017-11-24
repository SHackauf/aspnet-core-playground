using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.pocos;
using de.playground.aspnet.core.utils.entityframework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.dataaccesses.mariadb
{
    public class CustomerMariaDbDataAccess : EntityDataAccessBase<ICustomerPoco, CustomerPoco, MariaDbContext>, ICustomerDataAccess
    {
        #region Constructor

        public CustomerMariaDbDataAccess(ILogger<CustomerMariaDbDataAccess> logger, MariaDbContext mariaDbContext)
            : base(logger, mariaDbContext)
        {
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync()
            => await this.SelectPocosAsync();

        public async Task<IEnumerable<ICustomerPoco>> SelectCustomersAsync(Expression<Func<ICustomerPoco, bool>> whereExpression)
            => await this.SelectPocosAsync(whereExpression);

        public async Task<ICustomerPoco> SelectCustomerAsync(int id)
            => await this.SelectPocoAsync(customer => customer.Id == id);

        public async Task<bool> ExistsCustomerAsync(int id)
            => await this.ExistsPocoAsync(customer => customer.Id == id);

        public async Task<ICustomerPoco> InsertCustomerAsync(ICustomerPoco customer)
            => await this.InsertPocoAsync(customer);

        public async Task<ICustomerPoco> UpdateCustomerAsync(ICustomerPoco customer)
            => await this.UpdatePocoAsync(customer);

        public async Task<bool> RemoveCustomerAsync(int id)
            => await this.RemovePocoAsync(customer => customer.Id == id);

        #endregion

        #region Protected Methods

        protected override DbSet<CustomerPoco> GetDbSet(MariaDbContext dbContext) => dbContext.Customers;

        protected override string GetPrimaryKeyAsString(ICustomerPoco poco) => poco.Id.ToString();

        #endregion
    }
}
