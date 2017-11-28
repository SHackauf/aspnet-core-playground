using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.contracts.utils.logger;

using Microsoft.Extensions.Logging;

namespace de.playground.net.core.console
{
    public class XmlDialog
    {
        #region Private Fields

        private readonly IXmlImportModule xmlImportModule;
        private readonly ILogger logger;

        #endregion

        #region Constructor

        public XmlDialog(ILogger<XmlDialog> logger, IXmlImportModule xmlImportModule)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.xmlImportModule = xmlImportModule ?? throw new ArgumentNullException(nameof(xmlImportModule));
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
                Console.WriteLine("   <number> show file");
                Console.WriteLine("   <return> go back");
                Console.WriteLine("====================================");

                var input = Console.ReadLine();
                this.logger.LogDebug(LoggingEvents.Input, $"{nameof(this.ShowAsync)}: [input: {input}]");

                switch (input)
                {
                    case string inputAsString when int.TryParse(input, out var inputAsNumber) && inputAsNumber < files.Count():

                        await this.ShowFileAsync(files[inputAsNumber]);
                        break;

                    case "":
                        return;
                }
            }
        }

        #endregion

        #region Private Methods

        private async Task ShowFileAsync(string path)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("   net-core-console");
                Console.WriteLine("====================================");

                if (!File.Exists(path))
                {
                    Console.WriteLine("File doesn't exists.");
                }
                else
                {
                    this.logger.LogDebug(LoggingEvents.ShowItem, $"{nameof(this.ShowFileAsync)}: {path}");

                    using (StreamReader streamReader = new StreamReader(new FileStream(path, FileMode.Open)))
                    {
                        string fileLine = await streamReader.ReadLineAsync();
                        while (fileLine != null)
                        {
                            Console.WriteLine(fileLine);
                            fileLine = await streamReader.ReadLineAsync();
                        }
                    }
                }

                Console.WriteLine("====================================");
                Console.WriteLine("   1:       import");
                Console.WriteLine("   <return> go back");
                Console.WriteLine("====================================");

                var input = Console.ReadLine();
                this.logger.LogDebug(LoggingEvents.Input, $"{nameof(this.ShowFileAsync)}: [input: {input}]");

                switch (input)
                {
                    case "1":

                        var xmlData = await File.ReadAllTextAsync(path);
                        var successful = this.xmlImportModule.Import(xmlData);

                        Console.WriteLine(successful ? "Import successful" : "Import failed");
                        Console.WriteLine("====================================");
                        Console.WriteLine("   <return> go back");
                        Console.WriteLine("====================================");
                        Console.ReadLine();

                        return;

                    case "":
                        return;
                }
            }
        }

        #endregion
    }
}
