using System;
using System.Threading.Tasks;

using de.playground.aspnet.core.modules;

namespace de.playground.net.core.console
{
    internal class CustomerDialog
    {
        #region Public Methods

        public async Task ShowAsync()
        {
            var customerModule = new CustomerModule();
            var customers = await customerModule.GetCustomersAsync();

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
