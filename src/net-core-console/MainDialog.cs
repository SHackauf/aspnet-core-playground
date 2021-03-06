﻿using System;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.utils.logger;

using Microsoft.Extensions.Logging;

namespace de.playground.net.core.console
{
    public class MainDialog
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly CustomerDialog customerDialog;
        private readonly XmlDialog xmlDialog;

        #endregion

        #region Constructor

        public MainDialog(CustomerDialog customerDialog, XmlDialog xmlDialog, ILogger<MainDialog> logger)
        {
            this.customerDialog = customerDialog ?? throw new ArgumentNullException(nameof(customerDialog));
            this.xmlDialog = xmlDialog ?? throw new ArgumentNullException(nameof(xmlDialog));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public async Task ShowAsync()
        {
            while (true)
            {
                this.logger.LogDebug(LoggingEvents.Show, $"{nameof(this.ShowAsync)}: print");

                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("   net-core-console");
                Console.WriteLine("====================================");
                Console.WriteLine("");
                Console.WriteLine("====================================");
                Console.WriteLine("   1:        show customers");
                Console.WriteLine("   9:        XML import");
                Console.WriteLine("   <return>: end program");
                Console.WriteLine("====================================");

                var input = Console.ReadLine();
                this.logger.LogDebug(LoggingEvents.Input, $"{nameof(this.ShowAsync)}: [input: {input}]");

                switch (input)
                {
                    case "1":
                        await this.customerDialog.ShowAsync();
                        break;

                    case "9":
                        await this.xmlDialog.ShowAsync();
                        break;

                    case "":
                        return;
                }
            }
        }

        #endregion
    }
}
