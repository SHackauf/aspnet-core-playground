using System;
using System.Threading.Tasks;

namespace de.playground.net.core.console
{
    public class MainDialog
    {
        #region Private Fields

        private readonly CustomerDialog customerDialog;

        #endregion

        #region Constructor

        public MainDialog(CustomerDialog customerDialog) => this.customerDialog = customerDialog;

        #endregion

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
                        await this.customerDialog.ShowAsync();
                        break;

                    case "":
                        return;
                }
            }
        }

        #endregion
    }
}
