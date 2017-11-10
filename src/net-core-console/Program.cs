using System;

namespace de.playground.net.core.console
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Include Dependency Injection
            var mainDialog = new MainDialog();
            mainDialog.ShowAsync().Wait();
        }
    }
}
