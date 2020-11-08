﻿using CLS.Core.StaticData;
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
            var ls = new LogSender(StaticData.EnvironmentType.DEV, StaticData.SystemType.ConsoleApplication);

            var logGeneratorThread = new Thread(RandomLogGenerator);
            logGeneratorThread.Start(new { ls });

            Console.ReadKey();
        }

        public static void RandomLogGenerator(object args)
        {
            var ls = (LogSender)((dynamic)args).ls;
            var r = new Random(DateTime.Now.Ticks.GetHashCode());
            var counter = 1;

            while (true)
            {
                var messageType = string.Empty;
                var randomNumber = r.Next(100);
                var guid = Guid.NewGuid();

                // 1. generate random message to send to CLSDb
                if (randomNumber <= 29) // 30% chance
                {
                    messageType = "Debug";
                    if (r.Next(0, 2) == 1)
                    {
                        ls.Log(StaticData.SeverityType.Debug,
                              new Exception($"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}."),
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                    else
                    {
                        ls.Log(StaticData.SeverityType.Debug,
                              null,
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                }
                else if (randomNumber >= 30 && randomNumber <= 59) // 30% chance
                {
                    messageType = "Info";
                    if (r.Next(0, 2) == 1)
                    {
                        ls.Log(StaticData.SeverityType.Info,
                              new Exception($"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}."),
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                    else
                    {
                        ls.Log(StaticData.SeverityType.Info,
                              null,
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                }
                else if (randomNumber >= 60 && randomNumber <= 79) // 20% chance
                {
                    messageType = "Warn";
                    if (r.Next(0, 2) == 1)
                    {
                        ls.Log(StaticData.SeverityType.Warn,
                              new Exception($"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}."),
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                    else
                    {
                        ls.Log(StaticData.SeverityType.Warn,
                              null,
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                }
                else if (randomNumber >= 80 && randomNumber <= 94) // 10% chance
                {
                    messageType = "Error";
                    if (r.Next(0, 2) == 1)
                    {
                        ls.Log(StaticData.SeverityType.Error,
                              new Exception($"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}."),
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                    else
                    {
                        ls.Log(StaticData.SeverityType.Error,
                              null,
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                }
                else if (randomNumber >= 95) // 5% chance
                {
                    messageType = "Fatal";
                    if (r.Next(0, 2) == 1)
                    {
                        ls.Log(StaticData.SeverityType.Fatal,
                              new Exception($"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}."),
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                    else
                    {
                        ls.Log(StaticData.SeverityType.Fatal,
                              null,
                              $"This {messageType} message was randomly generated by the CLS Sender Console with guid: {guid}.");
                    }
                }

                var sleepTimeMs = r.Next(2000, 5000);
                ConsoleHelper.LogMessageToConsole($"Generated message with type: {messageType}, now sleeping for {sleepTimeMs}ms ({sleepTimeMs/1000} seconds).");
                ConsoleHelper.LogColouredMessageToConsole(ConsoleColor.Yellow, $"Logged {counter} total messages.");
                counter++;
                Thread.Sleep(sleepTimeMs);
            }
        }
    }
}
