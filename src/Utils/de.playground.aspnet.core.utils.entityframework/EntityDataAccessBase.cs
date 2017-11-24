using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.pocos;
using de.playground.aspnet.core.contracts.utils.logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.utils.entityframework
{
    public abstract class EntityDataAccessBase<TPocoInterface, TPocoClass, TDbContext>
        where TPocoInterface : IPoco
        where TPocoClass : class, TPocoInterface
        where TDbContext : DbContext
    {
        #region Private Fields

        private readonly TDbContext dbContext;

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public EntityDataAccessBase(ILogger<EntityDataAccessBase<TPocoInterface, TPocoClass, TDbContext>> logger, TDbContext dbContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract DbSet<TPocoClass> GetDbSet(TDbContext dbContext);
        protected abstract string GetPrimaryKeyAsString(TPocoInterface poco);

        #endregion

        #region Protected Methods

        protected async Task<IEnumerable<TPocoInterface>> SelectPocosAsync()
        {
            try
            {
                return await this.GetDbSet(this.dbContext).AsNoTracking().ToArrayAsync();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select pocos.");
                throw exception;
            }
        }

        public async Task<IEnumerable<TPocoInterface>> SelectPocosAsync(Expression<Func<TPocoInterface, bool>> whereExpression)
        {
            try
            {
                return await this.GetDbSet(this.dbContext).AsNoTracking().Where(whereExpression).ToArrayAsync();
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select pocos with where.");
                throw exception;
            }
        }

        public async Task<TPocoInterface> SelectPocoAsync(Expression<Func<TPocoInterface, bool>> whereExpression)
        {
            try
            {
                return await this.GetDbSet(this.dbContext).AsNoTracking().FirstOrDefaultAsync(whereExpression);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't select poco.");
                throw exception;
            }
        }

        public async Task<bool> ExistsPocoAsync(Expression<Func<TPocoInterface, bool>> whereExpression)
        {
            try
            {
                //return await this.GetDbSet(this.dbContext).AnyAsync(whereExpression);
                return 1 == await this.GetDbSet(this.dbContext).AsNoTracking().CountAsync(whereExpression);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Can't count poco.");
                throw exception;
            }
        }

        public async Task<TPocoInterface> InsertPocoAsync(TPocoInterface poco)
        {
            try
            {
                this.dbContext.Add(poco as TPocoClass);
                var count = await this.dbContext.SaveChangesAsync();

                if (count == 1)
                {
                    this.logger.LogInformation(LoggingEvents.InsertItem, $"{nameof(this.InsertPocoAsync)}: successful [Id: {this.GetPrimaryKeyAsString(poco)}]");
                    return poco;
                }

                this.logger.LogWarning(LoggingEvents.InsertItem, $"{nameof(this.InsertPocoAsync)}: faulty [Id: {this.GetPrimaryKeyAsString(poco)}]");
                return default(TPocoInterface);
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.logger.LogError(dbUpdateException, "Can't insert poco.");
                throw dbUpdateException;
            }
        }

        public async Task<TPocoInterface> UpdatePocoAsync(TPocoInterface poco)
        {
            try
            {
                this.dbContext.Update(poco as TPocoClass);
                var count = await this.dbContext.SaveChangesAsync();

                if (count == 1)
                {
                    this.logger.LogInformation(LoggingEvents.UpdateItem, $"{nameof(this.UpdatePocoAsync)}: successful [Id: {this.GetPrimaryKeyAsString(poco)}]");
                    return poco;
                }

                this.logger.LogWarning(LoggingEvents.UpdateItem, $"{nameof(this.UpdatePocoAsync)}: faulty [Id: {this.GetPrimaryKeyAsString(poco)}]");
                return default(TPocoInterface);
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.logger.LogError(dbUpdateException, "Can't update poco.");
                throw dbUpdateException;
            }
        }

        public async Task<bool> RemovePocoAsync(Expression<Func<TPocoInterface, bool>> whereExpression)
        {
            try
            {
                var poco = await this.SelectPocoAsync(whereExpression) as TPocoClass;
                if (poco == null)
                {
                    return false;
                }

                this.dbContext.Remove(poco);
                var count = await this.dbContext.SaveChangesAsync();

                if (count == 1)
                {
                    this.logger.LogInformation(LoggingEvents.DeleteItem, $"{nameof(this.RemovePocoAsync)}: successful [Id: {this.GetPrimaryKeyAsString(poco)}]");
                    return true;
                }

                this.logger.LogWarning(LoggingEvents.DeleteItem, $"{nameof(this.RemovePocoAsync)}: faulty [Id: {this.GetPrimaryKeyAsString(poco)}]");
                return false;
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.logger.LogError(dbUpdateException, "Can't remove poco.");
                throw dbUpdateException;
            }
        }

        #endregion
    }
}
