using de.playground.aspnet.core.contracts.dataaccesses;

using Microsoft.Extensions.Logging;

using System;

namespace de.playground.aspnet.core.dataaccesses.sqlite
{
    public class SqLiteDbInitialize : IDataAccessInitialize
    {
        #region Private Fields

        private readonly SqLiteDbContext sqLiteDbContext;

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public SqLiteDbInitialize(ILogger<SqLiteDbInitialize> logger, SqLiteDbContext sqLiteDbContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.sqLiteDbContext = sqLiteDbContext ?? throw new ArgumentNullException(nameof(sqLiteDbContext));
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            try
            {
                this.logger.LogInformation($"{nameof(Initialize)} - Start");

                sqLiteDbContext.Database.EnsureCreated();
            }
            finally
            {
                this.logger.LogInformation($"{nameof(Initialize)} - End");
            }
        }

        #endregion
    }
}
