using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace de.playground.net.core.console
{
    internal class MainDialog
    {
        #region Public Methods

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("   net-core-console");
                Console.WriteLine("====================================");
                Console.WriteLine("");
                Console.WriteLine("====================================");
                Console.WriteLine("   1:        show customers");
                Console.WriteLine("   <return>: end program");
                Console.WriteLine("====================================");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        await new CustomerDialog().ShowAsync();
                        break;

                    case "":
                        return;
                }
            }
        }

        #endregion
    }
}
