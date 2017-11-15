using System;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.contracts.utils.logger;
using Microsoft.Extensions.Logging;

namespace de.playground.net.core.console
{
    public class CustomerDialog
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly ICustomerModule customerModule;

        #endregion

        #region Constructor

        public CustomerDialog(ICustomerModule customerModule, ILogger<MainDialog> logger)
        {
            this.customerModule = customerModule ?? throw new ArgumentNullException(nameof(customerModule));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public async Task ShowAsync()
        {
            var customers = await this.customerModule.GetCustomersAsync();

            while (true)
            {
                this.logger.LogDebug(LoggingEvents.ShowItems, $"{nameof(this.ShowAsync)}: print");

                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("   net-core-console");
                Console.WriteLine("====================================");

                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.Id} - {customer.Name}");
                }

                Console.WriteLine("====================================");
                Console.WriteLine("   <return> go back");
                Console.WriteLine("====================================");

                var input = Console.ReadLine();
                this.logger.LogDebug(LoggingEvents.Input, $"{nameof(this.ShowAsync)}: [input: {input}]");

                switch (input)
                {
                    case "":
                        return;
                }
            }
        }

        #endregion
    }
}
