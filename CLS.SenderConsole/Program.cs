﻿using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Helpers;
using CLS.Sender.Classes;
using System;
using System.Threading;

namespace CLS.SenderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var ls = new LogSender("CLS.SenderConsole", StaticData.EnvironmentType.DEV, StaticData.SystemType.ConsoleApplication);

            var logGeneratorThread = new Thread(RandomLogGenerator);
            logGeneratorThread.Start(new { ls });

            Console.ReadKey();
        }

        public static void RandomLogGenerator(object args)
        {
            var ls = (LogSender)((dynamic)args).ls;
            var r = new Random(DateTime.Now.Ticks.GetHashCode());

            while (true)
            {
                var messageType = string.Empty;
                var randomNumber = r.Next(100);

                // 1. generate random message to send to CLSDb
                if (randomNumber <= 29) // 30% chance
                {
                    messageType = "Debug";
                    if (r.Next(1, 2) == 1)
                        ls.Log(StaticData.SeverityType.Debug,
                            new Exception("This message was randomly generated by the CLS Sender Console."),
                            "This debug message was randomly generated by the CLS Sender Console.");
                    ls.Log(StaticData.SeverityType.Debug, null,
                        "This debug message was randomly generated by the CLS Sender Console.");
                }
                else if (randomNumber >= 30 && randomNumber <= 59) // 30% chance
                {
                    messageType = "Info";
                    ls.Log(StaticData.SeverityType.Info, null, "This info message was randomly generated by the CLS Sender Console.");
                }
                else if (randomNumber >= 60 && randomNumber <= 79) // 20% chance
                {
                    messageType = "Warn";
                    if (r.Next(1, 2) == 1)
                        ls.Log(StaticData.SeverityType.Warn, new Exception("This warning message was randomly generated by the CLS Sender Console."),
                            "This warning message was randomly generated by the CLS Sender Console.");
                    ls.Log(StaticData.SeverityType.Warn, null,
                        "This warning message was randomly generated by the CLS Sender Console.");
                }
                else if (randomNumber >= 80 && randomNumber <= 94) // 10% chance
                {
                    messageType = "Error";
                    ls.Log(StaticData.SeverityType.Error, new Exception("This error message was randomly generated by the CLS Sender Console."));
                }
                else if (randomNumber >= 95) // 5% chance
                {
                    messageType = "Fatal";
                    ls.Log(StaticData.SeverityType.Fatal, new Exception("This fatal message was randomly generated by the CLS Sender Console."),
                        "This fatal message was randomly generated by the CLS Sender Console.");
                }

                var sleepTimeMs = r.Next(5000, 10000);
                ConsoleHelper.LogMessageToConsole($"Generated message with type: {messageType}, now sleeping for {sleepTimeMs}ms ({sleepTimeMs/1000} seconds).");
                Thread.Sleep(sleepTimeMs);
            }
        }
    }
}
