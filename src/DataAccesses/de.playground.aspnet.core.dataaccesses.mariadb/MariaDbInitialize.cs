using System;

using de.playground.aspnet.core.contracts.dataaccesses;

using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.dataaccesses.mariadb
{
    public class MariaDbInitialize : IDataAccessInitialize
    {
        #region Private Fields

        private readonly MariaDbContext mariaDbContext;

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public MariaDbInitialize(ILogger<MariaDbInitialize> logger, MariaDbContext mariaDbContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mariaDbContext = mariaDbContext ?? throw new ArgumentNullException(nameof(mariaDbContext));
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            try
            {
                this.logger.LogInformation($"{nameof(Initialize)} - Start");

                mariaDbContext.Database.EnsureCreated();
            }
            finally
            {
                this.logger.LogInformation($"{nameof(Initialize)} - End");
            }
        }

        #endregion
    }
}
