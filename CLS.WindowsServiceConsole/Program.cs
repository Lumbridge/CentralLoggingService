using CLS.WindowsService.Classes;
using System;
using System.Threading;

namespace CLS.WindowsServiceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseWatcher = new Thread(Processor.ProcessTriggersDebug);
            databaseWatcher.Start();
            Console.ReadKey();
        }
    }
}
