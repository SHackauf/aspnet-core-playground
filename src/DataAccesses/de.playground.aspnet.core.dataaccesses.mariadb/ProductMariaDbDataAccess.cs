using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.pocos;

namespace de.playground.aspnet.core.dataaccesses.mariadb
{
    public class ProductMariaDbDataAccess : IProductDataAccess
    {
        public Task<IEnumerable<IProductPoco>> SelectProductsAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IProductPoco> SelectProductAsync(int customerId, int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsProductAsync(int customerId, int id)
        {
            throw new NotImplementedException();
        }

        public Task<IProductPoco> InsertProductAsync(IProductPoco product)
        {
            throw new NotImplementedException();
        }

        public Task<IProductPoco> UpdateProductAsync(IProductPoco product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveProductAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
