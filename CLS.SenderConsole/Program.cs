using CLS.Core.StaticData;
using CLS.Sender.Classes;
using System;

namespace CLS.SenderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var ls = new LogSender();
            var pSystem = ls.GetPublishingSystem("CLS Console Client", StaticData.EnvironmentType.DEV, StaticData.SystemType.ConsoleApplication);
            var ex = new Exception("This is a test exception");
            ls.LogErrorToDb(pSystem, ex);
            Console.ReadKey();
        }
    }
}
