using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Classes;
using CLS.Infrastructure.Data;
using CLS.Infrastructure.Helpers;
using CLS.Sender.Classes;
using CLS.WindowsService.Helpers;
using System;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;

namespace CLS.WindowsService.Classes
{
    public class Processor
    {
        public static void ProcessTriggers()
        {
            var uow = new UnitOfWork(new DBEntities());

            while (true)
            {
                // 1. Get all alert trigger groups 
                var alertTriggerGroups = uow.Repository<AlertTriggerGroup>().ToList();

                // 2. Build chained expressions from node group nodes
                foreach (var alertTriggerGroup in alertTriggerGroups)
                {
                    // 3. Run the expression against the log table to see if we should send an alert
                    var logs = uow.Repository<Log>().GetAll();
                    var alertHistories = uow.Repository<AlertHistory>().GetAll();

                    // 4. filter down the logs to ones which haven't been alerted for this subscriber before
                    logs = logs
                        .Where(x => !alertHistories.Any(y => y.LogId == x.Id && y.SubscriberId == alertTriggerGroup.SubscriberId))
                        .ToList()
                        .AsQueryable()
                        .Where(alertTriggerGroup.ExpressionString);

                    // 5. determine if we should send an alert to the user by checking to see if any of the filtered logs match the trigger group
                    if (logs.Any())
                    {
                        var logCount = logs.Count();

                        // 6. Send an alert to the subscriber for this trigger group
                        EmailHelper.SendEmail(alertTriggerGroup.Subscriber.Email, "CLS Alert",
                            $"You are receiving this alert because you are subscribed via the CLS dashboard.\n\n" +
                            $"System: {alertTriggerGroup.Subscriptions.First().PublishingSystemName}\n" +
                            $"Environment Type: {alertTriggerGroup.Subscriptions.First().PublishingSystemEnvironmentTypeName}\n" +
                            $"Timestamp of most recent log message that met the criteria: {logs.OrderByDescending(x => x.Timestamp).First().Timestamp}\n" +
                            $"Criteria met: {alertTriggerGroup.ExpressionString}\n" +
                            $"Number of log messages that met criteria: {logCount}\n\n" +
                            $"You can review the log messages at https://localhost:44356/Logs/.");

                        // 7. Add a record to the Alert History table for each of the log messages flagged by this alert
                        foreach (var log in logs)
                        {
                            uow.Repository<AlertHistory>().Put(new AlertHistory
                            {
                                AlertTriggerGroup = alertTriggerGroup,
                                Log = log,
                                LogId = log.Id,
                                AlertTriggerGroupId = alertTriggerGroup.Id,
                                SubscriberId = alertTriggerGroup.SubscriberId.Value,
                                Subscriber = alertTriggerGroup.Subscriber,
                                Timestamp = DateTime.Now
                            });
                        }

                        // 8. Commit changes to the Alert History table
                        try
                        {
                            uow.Commit();
                        }
                        catch (Exception ex)
                        {
                            new LogSender("CLS.WindowsService", StaticData.EnvironmentType.DEV, StaticData.SystemType.WindowsService).LogErrorToDb(ex);
                        }
                    }
                }

                var sleepTime = int.Parse(ConfigurationManager.AppSettings["PollTimeMinutes"]) * 60000;
                Thread.Sleep(sleepTime);
            }
        }

        public static void ProcessTriggersDebug()
        {
            var uow = new UnitOfWork(new DBEntities());

            ConsoleHelper.LogMessageToConsole("Started Trigger Processor.\n");

            while (true)
            {
                // 1. Get all alert trigger groups 
                var alertTriggerGroups = uow.Repository<AlertTriggerGroup>().ToList();

                ConsoleHelper.LogMessageToConsole($"Found {alertTriggerGroups.Count} trigger groups in database.");

                // 2. Build chained expressions from node group nodes
                foreach (var alertTriggerGroup in alertTriggerGroups)
                {
                    ConsoleHelper.LogMessageToConsole($"Currently processing trigger group #{alertTriggerGroup.Id} with {alertTriggerGroup.AlertTriggerNodes.Count} nodes.");

                    ConsoleHelper.LogMessageToConsole($"Built expression from node group: {alertTriggerGroup.ExpressionString}.");

                    // 3. Run the expression against the log table to see if we should send an alert
                    var logs = uow.Repository<Log>().GetAll();
                    var alertHistories = uow.Repository<AlertHistory>().GetAll();

                    // 4. filter down the logs to ones which haven't been alerted for this subscriber before
                    logs = logs
                        .Where(x => !alertHistories.Any(y => y.LogId == x.Id && y.SubscriberId == alertTriggerGroup.SubscriberId))
                        .ToList()
                        .AsQueryable()
                        .Where(alertTriggerGroup.ExpressionString);

                    // 5. determine if we should send an alert to the user by checking to see if any of the filtered logs match the trigger group
                    if (logs.Any())
                    {
                        var logCount = logs.Count();

                        ConsoleHelper.LogColouredMessageToConsole(ConsoleColor.Green,
                            $"Sending alert for alertTriggerGroup #{alertTriggerGroup.Id} for subscriber {alertTriggerGroup.Subscriber.Name} with " +
                            $"email address {alertTriggerGroup.Subscriber.Email}.");

                        // 6. Send an alert to the subscriber for this trigger group
                        EmailHelper.SendEmail(alertTriggerGroup.Subscriber.Email, "CLS Alert",
                            $"You are receiving this alert because you are subscribed via the CLS dashboard.\n\n" +
                            $"System: {alertTriggerGroup.Subscriptions.First().PublishingSystemName}\n" +
                            $"Environment Type: {alertTriggerGroup.Subscriptions.First().PublishingSystemEnvironmentTypeName}\n" +
                            $"Timestamp of most recent log message that met the criteria: {logs.OrderByDescending(x=>x.Timestamp).First().Timestamp}\n" +
                            $"Criteria met: {alertTriggerGroup.ExpressionString}\n" +
                            $"Number of log messages that met criteria: {logCount}\n\n" +
                            $"You can review the log messages at https://localhost:44356/Logs/.");

                        // 7. Add a record to the Alert History table for each of the log messages flagged by this alert
                        foreach (var log in logs)
                        {
                            uow.Repository<AlertHistory>().Put(new AlertHistory
                            {
                                AlertTriggerGroup = alertTriggerGroup,
                                Log = log,
                                LogId = log.Id,
                                AlertTriggerGroupId = alertTriggerGroup.Id,
                                SubscriberId = alertTriggerGroup.SubscriberId.Value,
                                Subscriber = alertTriggerGroup.Subscriber,
                                Timestamp = DateTime.Now
                            });
                        }

                        // 8. Commit changes to the Alert History table
                        try
                        {
                            uow.Commit();
                            ConsoleHelper.LogMessageToConsole($"Successfully stored {logCount} records in the Alert History table.\n");
                        }
                        catch (Exception ex)
                        {
                            ConsoleHelper.LogMessageToConsole(exception: ex);
                            new LogSender("CLS.WindowsService", StaticData.EnvironmentType.DEV, StaticData.SystemType.WindowsService).LogErrorToDb(ex);
                        }
                    }
                    else
                    {
                        ConsoleHelper.LogMessageToConsole($"The criteria for trigger group #{alertTriggerGroup.Id} were not met, continuing.\n");
                    }
                }

                var sleepTime = int.Parse(ConfigurationManager.AppSettings["PollTimeMinutes"]) * 60000;
                ConsoleHelper.LogMessageToConsole($"Sleeping for {sleepTime}ms ({ConfigurationManager.AppSettings["PollTimeMinutes"]} minute(s))\n");
                Thread.Sleep(sleepTime);
            }
        }
    }
}
