using de.playground.aspnet.core.contracts.dataaccesses;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;

namespace de.playground.aspnet.core.dataaccesses.mariadb
{
    public class MariaDbServiceScopeInitialize : IDataAccessInitialize
    {
        #region Private Fields

        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public MariaDbServiceScopeInitialize(ILogger<MariaDbInitialize> logger, MariaDbContext mariaDbContext, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            try
            {
                this.logger.LogInformation($"{nameof(Initialize)} - Start");

                using (var serviceScope = serviceScopeFactory.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<MariaDbContext>();
                    context.Database.EnsureCreated();
                }
            }
            finally
            {
                this.logger.LogInformation($"{nameof(Initialize)} - End");
            }
        }

        #endregion
    }
}
