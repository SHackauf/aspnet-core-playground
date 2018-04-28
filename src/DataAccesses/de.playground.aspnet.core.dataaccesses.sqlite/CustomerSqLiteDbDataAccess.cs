using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.utils.entityframework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace de.playground.aspnet.core.dataaccesses.sqlite
{
    public class CustomerSqLiteDbDataAccess : EntityDataAccessBase<CustomerPoco, SqLiteDbContext>, ICustomerDataAccess
    {
        #region Constructor

        public CustomerSqLiteDbDataAccess(ILogger<CustomerSqLiteDbDataAccess> logger, SqLiteDbContext sqLiteDbContext)
            : base(logger, sqLiteDbContext)
        {
        }

        #endregion

        #region Public Methods

        public async Task<int> CountCustomersAsync()
            => await this.CountPocosAsync();

        public async Task<IEnumerable<CustomerPoco>> SelectCustomersAsync()
            => await this.SelectPocosAsync();

        public async Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(int offset, int limit)
            => await this.SelectPocosAsync(offset, limit, poco => poco.Id);

        public async Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(Expression<Func<CustomerPoco, bool>> whereExpression)
            => await this.SelectPocosAsync(whereExpression);

        public async Task<IEnumerable<CustomerPoco>> SelectCustomersAsync(Expression<Func<CustomerPoco, bool>> whereExpression, int offset, int limit)
            => await this.SelectPocosAsync(whereExpression, offset, limit, poco => poco.Id);

        public async Task<CustomerPoco> SelectCustomerAsync(int id)
            => await this.SelectPocoAsync(customer => customer.Id == id);

        public async Task<bool> ExistsCustomerAsync(int id)
            => await this.ExistsPocoAsync(customer => customer.Id == id);

        public async Task<CustomerPoco> InsertCustomerAsync(CustomerPoco customer)
            => await this.InsertPocoAsync(customer);

        public async Task<CustomerPoco> UpdateCustomerAsync(CustomerPoco customer)
            => await this.UpdatePocoAsync(customer);

        public async Task<bool> RemoveCustomerAsync(int id)
            => await this.RemovePocoAsync(customer => customer.Id == id);

        #endregion

        #region Protected Methods

        protected override DbSet<CustomerPoco> GetDbSet(SqLiteDbContext dbContext) => dbContext.Customers;

        protected override string GetPrimaryKeyAsString(CustomerPoco poco) => poco.Id.ToString();

        #endregion
    }
}
