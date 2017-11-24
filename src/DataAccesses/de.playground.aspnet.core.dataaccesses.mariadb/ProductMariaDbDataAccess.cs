using System.Collections.Generic;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.utils.entityframework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.dataaccesses.mariadb
{
    public class ProductMariaDbDataAccess : EntityDataAccessBase<ProductPoco, MariaDbContext>, IProductDataAccess
    {
        #region Constructor

        public ProductMariaDbDataAccess(ILogger<ProductMariaDbDataAccess> logger, MariaDbContext mariaDbContext)
            : base(logger, mariaDbContext)
        {
        }

        #endregion

        #region Public Methods

        public Task<IEnumerable<ProductPoco>> SelectProductsAsync(int customerId)
            => this.SelectPocosAsync(product => product.CustomerId == customerId);

        public Task<ProductPoco> SelectProductAsync(int customerId, int id)
            => this.SelectPocoAsync(product => product.CustomerId == customerId && product.Id == id);

        public Task<bool> ExistsProductAsync(int customerId, int id)
            => this.ExistsPocoAsync(product => product.CustomerId == customerId && product.Id == id);

        public Task<ProductPoco> InsertProductAsync(ProductPoco product)
            => this.InsertPocoAsync(product);

        public Task<ProductPoco> UpdateProductAsync(ProductPoco product)
            => this.UpdatePocoAsync(product);

        public Task<bool> RemoveProductAsync(int id)
            => this.RemovePocoAsync(product => product.Id == id);

        #endregion

        #region Protected Methods

        protected override DbSet<ProductPoco> GetDbSet(MariaDbContext dbContext) => dbContext.Products;

        protected override string GetPrimaryKeyAsString(ProductPoco poco) => poco.Id.ToString();

        #endregion
    }
}
