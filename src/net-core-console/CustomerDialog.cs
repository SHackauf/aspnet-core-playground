using System;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.modules;

namespace de.playground.net.core.console
{
    public class CustomerDialog
    {
        #region Private Fields

        private readonly ICustomerModule customerModule;

        #endregion

        #region Constructor

        public CustomerDialog(ICustomerModule customerModule) => this.customerModule = customerModule;

        #endregion

        #region Public Methods

        public async Task ShowAsync()
        {
            var customers = await this.customerModule.GetCustomersAsync();

            while (true)
            {
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
