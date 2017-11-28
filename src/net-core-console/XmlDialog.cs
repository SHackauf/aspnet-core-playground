using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.utils.logger;

using Microsoft.Extensions.Logging;

namespace de.playground.net.core.console
{
    public class XmlDialog
    {
        #region Private Fields

        private readonly ILogger logger;

        #endregion

        #region Constructor

        public XmlDialog(ILogger<XmlDialog> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public async Task ShowAsync()
        {
            while (true)
            {
                var files = Directory.EnumerateFiles("Examples", "*.xml", SearchOption.TopDirectoryOnly).ToArray();

                this.logger.LogDebug(LoggingEvents.ShowItems, $"{nameof(this.ShowAsync)}: print");

                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("   net-core-console");
                Console.WriteLine("====================================");

                for(var pos = 0; pos < files.Count(); pos++)
                {
                    Console.WriteLine($"{pos} - {files[pos]}");
                }

                Console.WriteLine("====================================");
                Console.WriteLine("   <number> import file");
                Console.WriteLine("   <return> go back");
                Console.WriteLine("====================================");

                var input = Console.ReadLine();
                this.logger.LogDebug(LoggingEvents.Input, $"{nameof(this.ShowAsync)}: [input: {input}]");

                switch (input)
                {
                    case string inputAsString when int.TryParse(input, out var inputAsNumber) && inputAsNumber < files.Count():

                        Console.WriteLine($"Import file: {files[inputAsNumber]}");
                        Console.ReadLine();
                        break;

                    case "":
                        return;
                }
            }
        }

        #endregion
    }
}
